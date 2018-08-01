﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Stubs;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Serialization.Xml;

namespace ESFA.DC.ILR.FundingService.FM35.Service
{
    public class TaskProviderService : ITaskProviderService
    {
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly IPopulationService _populationService;
        private readonly IPagingService<ILearner> _learnerPerActorService;
        private readonly IFundingService<ILearner, FM35FundingOutputs> _fundingService;

        public TaskProviderService(IKeyValuePersistenceService keyValuePersistenceService, IPopulationService populationService, IPagingService<ILearner> learnerPerActorService, IFundingService<ILearner, FM35FundingOutputs> fundingService)
        {
            _keyValuePersistenceService = keyValuePersistenceService;
            _populationService = populationService;
            _learnerPerActorService = learnerPerActorService;
            _fundingService = fundingService;
        }

        public void ExecuteTasks(IMessage message)
        {
            // Build Persistance Dictionary
            BuildKeyValueDictionary(message);

            _populationService.Populate();

            // pre funding
            var learnersToProcess = _learnerPerActorService.BuildPages();

            // process funding
            var fundingOutputs = ProcessFunding(learnersToProcess);

            // persist
            var dataPersister = new DataPersister();
            dataPersister.PersistData(fundingOutputs, @"C:\Code\temp\FM35FundingService\Json_Output.json");
        }

        private void BuildKeyValueDictionary(IMessage message)
        {
            var learners = message.Learners.Select(l => l.LearnRefNumber).ToList();

            var serializer = new XmlSerializationService();

            _keyValuePersistenceService.SaveAsync("ValidLearnRefNumbers", serializer.Serialize(learners)).Wait();
        }

        private FM35FundingOutputs ProcessFunding(IEnumerable<IEnumerable<ILearner>> learnersList)
        {
            ConcurrentBag<FM35FundingOutputs> fundingOutputsList = new ConcurrentBag<FM35FundingOutputs>();

            Parallel.ForEach(learnersList, ll =>
            {
                fundingOutputsList.Add(_fundingService.ProcessFunding(ll));
            });

            //foreach (var list in learnersList)
            //{
            //    fundingOutputsList.Add(_fm35OrchestrationService.Execute(ukprn, list));
            //}

            return TransformFundingOutput(fundingOutputsList.ToList());
        }

        private FM35FundingOutputs TransformFundingOutput(IEnumerable<FM35FundingOutputs> fundingOutputs)
        {
            var global = fundingOutputs.Select(g => g.Global).FirstOrDefault();

            var learnerAttributes = fundingOutputs.SelectMany(l => l.Learners).ToArray();
                // fundingOutputs.SelectMany(f => f.SelectMany(l => l.Learners)).ToArray();

            return new FM35FundingOutputs
            {
                Global = global,
                Learners = learnerAttributes,
            };
        }
    }
}
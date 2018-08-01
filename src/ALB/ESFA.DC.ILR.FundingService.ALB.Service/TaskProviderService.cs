﻿using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Stubs;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Serialization.Xml;

namespace ESFA.DC.ILR.FundingService.ALB.Service
{
    public class TaskProviderService : ITaskProviderService
    {
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly IFundingService<ILearner, FundingOutputs> _fundingService;
        private readonly IPopulationService _populationService;
        private readonly IPagingService<ILearner> _learnerPerActorService;

        public TaskProviderService(IKeyValuePersistenceService keyValuePersistenceService, IFundingService<ILearner, FundingOutputs> fundingService, IPopulationService populationService, IPagingService<ILearner> learnerPerActorService)
        {
            _keyValuePersistenceService = keyValuePersistenceService;
            _fundingService = fundingService;
            _populationService = populationService;
            _learnerPerActorService = learnerPerActorService;
        }

        public void ExecuteTasks(IMessage message)
        {
            // Build Persistance Dictionary
            BuildKeyValueDictionary(message);

            // pre funding
            _populationService.Populate();
            var learnersToProcess = _learnerPerActorService.BuildPages();

            // process funding
            var fundingOutputs = ProcessFunding(learnersToProcess);

            // transform shards in to new object
            var fundingOutputsToPersist = TransformFundingOutput(fundingOutputs);

            // persist
            var dataPersister = new DataPersister();
            dataPersister.PersistData(fundingOutputsToPersist, @"C:\Code\temp\ALBFundingService\Json_Output.json");
        }

        private void BuildKeyValueDictionary(IMessage message)
        {
            var learners = message.Learners.ToList();

            var serializer = new XmlSerializationService();

            _keyValuePersistenceService.SaveAsync("ValidLearnRefNumbers", serializer.Serialize(learners)).Wait();
        }

        private IEnumerable<FundingOutputs> ProcessFunding(IEnumerable<IEnumerable<ILearner>> learnersList)
        {
            var fundingOutputsList = new List<FundingOutputs>();

            foreach (var list in learnersList)
            {
                fundingOutputsList.Add(_fundingService.ProcessFunding(list));
            }

            return fundingOutputsList;
        }

        private FundingOutputs TransformFundingOutput(IEnumerable<FundingOutputs> fundingOutputsList)
        {
            var global = fundingOutputsList.First().Global;

            var learnerAttributes = fundingOutputsList.SelectMany(l => l.Learners).ToArray();

            return new FundingOutputs
            {
                Global = global,
                Learners = learnerAttributes,
            };
        }
    }
}
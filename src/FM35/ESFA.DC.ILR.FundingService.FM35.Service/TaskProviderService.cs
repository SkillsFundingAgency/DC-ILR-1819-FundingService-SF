using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Xml;

namespace ESFA.DC.ILR.FundingService.FM35.Service
{
    public class TaskProviderService : ITaskProviderService
    {
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly IPopulationService _populationService;
        private readonly IPagingService<ILearner> _learnerPerActorService;
        private readonly IFundingService<ILearner, FM35Global> _fundingService;
        private readonly IJsonSerializationService _jsonSerializationService;

        public TaskProviderService(IKeyValuePersistenceService keyValuePersistenceService, IPopulationService populationService, IPagingService<ILearner> learnerPerActorService, IFundingService<ILearner, FM35Global> fundingService, IJsonSerializationService jsonSerializationService)
        {
            _keyValuePersistenceService = keyValuePersistenceService;
            _populationService = populationService;
            _learnerPerActorService = learnerPerActorService;
            _fundingService = fundingService;
            _jsonSerializationService = jsonSerializationService;
        }

        public void ExecuteTasks(IMessage message)
        {
            // Build Persistance Dictionary
            BuildKeyValueDictionary(message);

            _populationService.PopulateAsync(CancellationToken.None).Wait();

            // pre funding
            var learnersToProcess = _learnerPerActorService.BuildPages(new List<int> { 35 });

            // process funding
            var fundingOutputs = ProcessFunding(learnersToProcess);

            // persist
            var serializedOutputs = _jsonSerializationService.Serialize(fundingOutputs);

            System.IO.File.WriteAllText(@"C:\Code\temp\FM35FundingService\Json_Output.json", serializedOutputs);
        }

        private void BuildKeyValueDictionary(IMessage message)
        {
            var learners = message.Learners.Select(l => l.LearnRefNumber).ToList();

            var serializer = new XmlSerializationService();

            _keyValuePersistenceService.SaveAsync("ValidLearnRefNumbers", serializer.Serialize(learners)).Wait();
        }

        private FM35Global ProcessFunding(IEnumerable<IEnumerable<ILearner>> learnersList)
        {
            ConcurrentBag<FM35Global> fundingOutputsList = new ConcurrentBag<FM35Global>();

            Parallel.ForEach(learnersList, ll =>
            {
                fundingOutputsList.Add(_fundingService.ProcessFunding(ll, CancellationToken.None));
            });

            return TransformFundingOutput(fundingOutputsList.ToList());
        }

        private FM35Global TransformFundingOutput(IEnumerable<FM35Global> fundingOutputs)
        {
            var firstOutput = fundingOutputs.FirstOrDefault();

            if (firstOutput != null)
            {
                firstOutput.Learners = fundingOutputs.Where(o => o.Learners != null).SelectMany(o => o.Learners).ToList();

                return firstOutput;
            }

            return new FM35Global();
        }
    }
}

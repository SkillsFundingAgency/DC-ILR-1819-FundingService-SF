﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.Config.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35Actor.Interfaces;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM36Actor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Constants;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Implementations
{
    public class PreFundingSFOrchestrationService : IPreFundingSFOrchestrationService
    {
        private readonly ISerializationService _jsonSerializationService;
        private readonly IIlrFileProviderService _ilrFileProviderService;
        private readonly IFundingServiceDto _fundingServiceDto;
        private readonly IPopulationService _populationService;
        private readonly IActorTask<IALBActor, ALBFundingOutputs> _albActorTask;
        private readonly IActorTask<IFM35Actor, FM35FundingOutputs> _fm35ActorTask;
        private readonly IActorTask<IFM36Actor, FM36FundingOutputs> _fm36ActorTask;
        private readonly IActorTask<IFM25Actor, Global> _fm25ActorTask;
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly IPagingService<ILearner> _learnerPagingService;
        private readonly IExternalDataCache _externalDataCache;
        private readonly IInternalDataCache _internalDataCache;
        private readonly IFileDataCache _fileDataCache;
        private readonly ITopicAndTaskSectionConfig _topicAndTaskSectionConfig;
        private readonly ILogger _logger;

        public PreFundingSFOrchestrationService(
            IJsonSerializationService jsonSerializationService,
            IIlrFileProviderService ilrFileProviderService,
            IFundingServiceDto fundingServiceDto,
            IPopulationService populationService,
            IActorTask<IALBActor, ALBFundingOutputs> albActorTask,
            IActorTask<IFM35Actor, FM35FundingOutputs> fm35ActorTask,
            IActorTask<IFM36Actor, FM36FundingOutputs> fm36ActorTask,
            IActorTask<IFM25Actor, Global> fm25ActorTask,
            IKeyValuePersistenceService keyValuePersistenceService,
            IPagingService<ILearner> learnerPagingService,
            IExternalDataCache externalDataCache,
            IInternalDataCache internalDataCache,
            IFileDataCache fileDataCache,
            ITopicAndTaskSectionConfig topicAndTaskSectionConfig,
            ILogger logger)
        {
            _jsonSerializationService = jsonSerializationService;
            _ilrFileProviderService = ilrFileProviderService;
            _fundingServiceDto = fundingServiceDto;
            _populationService = populationService;
            _albActorTask = albActorTask;
            _fm35ActorTask = fm35ActorTask;
            _fm36ActorTask = fm36ActorTask;
            _fm25ActorTask = fm25ActorTask;
            _keyValuePersistenceService = keyValuePersistenceService;
            _externalDataCache = externalDataCache;
            _internalDataCache = internalDataCache;
            _fileDataCache = fileDataCache;
            _learnerPagingService = learnerPagingService;
            _topicAndTaskSectionConfig = topicAndTaskSectionConfig;
            _logger = logger;
        }

        public async Task Execute(IJobContextMessage jobContextMessage, CancellationToken cancellationToken)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Get the ilr object from file
            var ilrMessage = await _ilrFileProviderService.Provide(jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename].ToString());
            if (ilrMessage == null)
            {
                throw new ArgumentNullException(nameof(jobContextMessage), "Failed to get ILR file");
            }

            var fundingServiceDto = (FundingServiceDto)_fundingServiceDto;
            fundingServiceDto.Message = ilrMessage;

            cancellationToken.ThrowIfCancellationRequested();

            // get valid and invalid learners from intermediate storage and store it in the dto for rulebases
            fundingServiceDto.ValidLearners = _jsonSerializationService.Deserialize<string[]>(
                await _keyValuePersistenceService.GetAsync(
                    jobContextMessage.KeyValuePairs[JobContextMessageKey.ValidLearnRefNumbers].ToString()));

            fundingServiceDto.InvalidLearners = _jsonSerializationService.Deserialize<string[]>(
                await _keyValuePersistenceService.GetAsync(
                    jobContextMessage.KeyValuePairs[JobContextMessageKey.InvalidLearnRefNumbers].ToString()));

            cancellationToken.ThrowIfCancellationRequested();

            _populationService.Populate();

            cancellationToken.ThrowIfCancellationRequested();

            var ukprn = Convert.ToInt32(jobContextMessage.KeyValuePairs[JobContextMessageKey.UkPrn]);

            var externalDataCache = _jsonSerializationService.Serialize(_externalDataCache);
            var internalDataCache = _jsonSerializationService.Serialize(_internalDataCache);
            var fileDataCache = _jsonSerializationService.Serialize(_fileDataCache);

            var fundingActorDtos = _learnerPagingService
                .BuildPages()
                .Select(p =>
                    new FundingActorDto()
                    {
                        JobId = jobContextMessage.JobId,
                        Ukprn = ukprn,
                        ExternalDataCache = externalDataCache,
                        InternalDataCache = internalDataCache,
                        FileDataCache = fileDataCache,
                        ValidLearners = _jsonSerializationService.Serialize(p)
                    }).ToList();

            var taskNames = jobContextMessage.Topics[jobContextMessage.TopicPointer].Tasks.SelectMany(t => t.Tasks).ToList();

            var fundingTasks = new List<Task>();

            if (taskNames.Contains(_topicAndTaskSectionConfig.TopicFunding_TaskPerformFM35Calculation))
            {
                fundingTasks.Add(_fm35ActorTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm35Output].ToString()));
            }

            if (taskNames.Contains(_topicAndTaskSectionConfig.TopicFunding_TaskPerformFM36Calculation))
            {
                fundingTasks.Add(_fm36ActorTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm36Output].ToString()));
            }

            if (taskNames.Contains(_topicAndTaskSectionConfig.TopicFunding_TaskPerformFM25Calculation))
            {
                fundingTasks.Add(_fm25ActorTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm25Output].ToString()));
            }

            if (taskNames.Contains(_topicAndTaskSectionConfig.TopicFunding_TaskPerformALBCalculation))
            {
                fundingTasks.Add(_albActorTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingAlbOutput].ToString()));
            }

            // execute all fundingtasks
            await Task.WhenAll(fundingTasks);

            _logger.LogDebug($"Completed Funding Service for given Rulebases in: {stopWatch.ElapsedMilliseconds}");
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
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
        private readonly IALBActorTask _albOrchestrationSfTask;
        private readonly IFM35ActorTask _fm35OrchestrationSfTask;
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly IPagingService<ILearner> _learnerPagingService;
        private readonly IExternalDataCache _externalDataCache;
        private readonly ILogger _logger;

        public PreFundingSFOrchestrationService(
            IJsonSerializationService jsonSerializationService,
            IIlrFileProviderService ilrFileProviderService,
            IFundingServiceDto fundingServiceDto,
            IPopulationService populationService,
            IALBActorTask albOrchestrationSfTask,
            IFM35ActorTask fm35OrchestrationSfTask,
            IKeyValuePersistenceService keyValuePersistenceService,
            IPagingService<ILearner> learnerPagingService,
            IExternalDataCache externalDataCache,
            ILogger logger)
        {
            _jsonSerializationService = jsonSerializationService;
            _ilrFileProviderService = ilrFileProviderService;
            _fundingServiceDto = fundingServiceDto;
            _populationService = populationService;
            _albOrchestrationSfTask = albOrchestrationSfTask;
            _fm35OrchestrationSfTask = fm35OrchestrationSfTask;
            _keyValuePersistenceService = keyValuePersistenceService;
            _externalDataCache = externalDataCache;
            _learnerPagingService = learnerPagingService;
            _logger = logger;
        }

        public async Task Execute(IJobContextMessage jobContextMessage)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Get the ilr object from file
            var ilrMessage = await _ilrFileProviderService.Provide(jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename].ToString());
            var fundingServiceDto = (FundingServiceDto)_fundingServiceDto;
            fundingServiceDto.Message = ilrMessage;

            // get valid learners from intermediate storage and store it in the dto for rulebases
            fundingServiceDto.ValidLearners = _jsonSerializationService.Deserialize<string[]>(
                await _keyValuePersistenceService.GetAsync(
                    jobContextMessage.KeyValuePairs[JobContextMessageKey.ValidLearnRefNumbers].ToString()));

            _populationService.Populate();

            var ukprn = Convert.ToInt32(jobContextMessage.KeyValuePairs[JobContextMessageKey.UkPrn]);
            var jobId = Convert.ToInt32(jobContextMessage.JobId);

            var referenceDataInBytes = Encoding.UTF8.GetBytes(_jsonSerializationService.Serialize(_externalDataCache));

            var fundingActorDtos = _learnerPagingService
                .BuildPages()
                .Select(p =>
                    new FundingActorDto()
                    {
                        JobId = jobId,
                        Ukprn = ukprn,
                        ReferenceDataCache = referenceDataInBytes,
                        ValidLearners = Encoding.UTF8.GetBytes(_jsonSerializationService.Serialize(p))
                    }).ToList();

            var fundingTasks = new List<Task>()
            {
                _albOrchestrationSfTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingAlbOutput].ToString()),
                _fm35OrchestrationSfTask.Execute(fundingActorDtos, jobContextMessage.KeyValuePairs[JobContextMessageKey.FundingFm35Output].ToString())
            };

            // execute all fundingtasks
            await Task.WhenAll(fundingTasks);

            _logger.LogDebug($"Completed Funding Service for given Rulebases in: {stopWatch.ElapsedMilliseconds}");
        }
    }
}

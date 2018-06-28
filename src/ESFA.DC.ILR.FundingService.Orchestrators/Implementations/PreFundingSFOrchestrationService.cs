﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using ESFA.DC.ILR.FundingService.ALB.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Interface.Attribute;
using ESFA.DC.ILR.FundingService.ALB.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.ALB.Stubs.Persistance.Model.Output;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.Dto;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Implementations
{
    public class PreFundingSFOrchestrationService : IPreFundingSFOrchestrationService
    {
        private readonly ISerializationService _jsonSerializationService;
        private readonly IIlrFileProviderService _ilrFileProviderService;
        private readonly IFundingServiceDto _fundingServiceDto;
        private readonly IALBOrchestrationSFTask _ALBOrchestrationSfTask;
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly ILogger _logger;

        public PreFundingSFOrchestrationService(
            IJsonSerializationService jsonSerializationService,
            IIlrFileProviderService ilrFileProviderService,
            IFundingServiceDto fundingServiceDto,
            IALBOrchestrationSFTask ALBOrchestrationSfTask,
            IKeyValuePersistenceService keyValuePersistenceService,
            ILogger logger)
        {
            _jsonSerializationService = jsonSerializationService;
            _ilrFileProviderService = ilrFileProviderService;
            _fundingServiceDto = fundingServiceDto;
            _ALBOrchestrationSfTask = ALBOrchestrationSfTask;
            _keyValuePersistenceService = keyValuePersistenceService;
            _logger = logger;
        }

        public async Task Execute(IJobContextMessage jobContextMessage)
        {
            _logger.LogDebug("PreFundingSF started");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var tasks = jobContextMessage.Topics[jobContextMessage.TopicPointer].Tasks;
            _logger.LogDebug("PreFundingSF got tasks");

            // Get the ilr object from file
            var ilrMessage = await _ilrFileProviderService.Provide(jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename].ToString());
            var fundingServiceDto = (FundingServiceDto)_fundingServiceDto;
            fundingServiceDto.Message = ilrMessage;

            _logger.LogDebug("PreFundingSF got ILR");

            // get valid learners from intermediate storage and store it in the dto for rulebases
            fundingServiceDto.ValidLearners = _jsonSerializationService.Deserialize<string[]>(
                await _keyValuePersistenceService.GetAsync(
                    jobContextMessage.KeyValuePairs[JobContextMessageKey.ValidLearnRefNumbers].ToString()));

            _logger.LogDebug("PreFundingSF got valid learners");

            // loop through list of all the tasks and execute them.
            var fundingTasks = new List<Task>();
            /*
            foreach (var taskItem in tasks.Where(x => x.SupportsParallelExecution))
            {
                // populate data
                switch (taskItem.Tasks[0])
                {
                    case "ALB":
                        fundingTasks.Add(_ALBOrchestrationSfTask.Execute(jobContextMessage));
                        break;
                    case "FAM35":
                        break;
                }
            }
            */
            fundingTasks.Add(_ALBOrchestrationSfTask.Execute(jobContextMessage));
            // execute all fundingtasks
            await Task.WhenAll(fundingTasks);
            _logger.LogDebug($"Completed Funding Service for given Rulebases in: {stopWatch.ElapsedMilliseconds}");
        }
    }
}

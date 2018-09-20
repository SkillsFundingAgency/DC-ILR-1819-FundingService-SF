﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALBActor.Interfaces;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Internal;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;

namespace ESFA.DC.ILR.FundingService.ALBActor
{
    [StatePersistence(StatePersistence.None)]
    [ActorService(Name = ActorServiceNameConstants.ALB)]
    public class ALBActor : AbstractFundingActor, IALBActor
    {
        public ALBActor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope, IExecutionContext executionContext, IJsonSerializationService jsonSerializationService)
            : base(actorService, actorId, lifetimeScope, executionContext, jsonSerializationService)
        {
        }

        public async Task<string> Process(FundingActorDto actorModel, CancellationToken cancellationToken)
        {
            ExternalDataCache referenceDataCache;
            InternalDataCache internalDataCache;
            FileDataCache fileDataCache;

            if (ExecutionContext is ExecutionContext executionContextObj)
            {
                executionContextObj.JobId = "-1";
                executionContextObj.TaskKey = ActorId.ToString();
            }

            ILogger logger = LifetimeScope.Resolve<ILogger>();

            try
            {
                logger.LogDebug($"{nameof(ALBActor)} {ActorId} starting");

                referenceDataCache = JsonSerializationService.Deserialize<ExternalDataCache>(actorModel.ExternalDataCache);
                internalDataCache = JsonSerializationService.Deserialize<InternalDataCache>(actorModel.InternalDataCache);
                fileDataCache = JsonSerializationService.Deserialize<FileDataCache>(actorModel.FileDataCache);

                logger.LogDebug($"{nameof(ALBActor)} {ActorId} finished getting input data");

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception ex)
            {
                ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                logger.LogError($"Error while processing {nameof(ALBActor)} job", ex);
                throw;
            }

            using (var childLifetimeScope = LifetimeScope.BeginLifetimeScope(c =>
            {
                c.RegisterInstance(referenceDataCache).As<IExternalDataCache>();
                c.RegisterInstance(internalDataCache).As<IInternalDataCache>();
                c.RegisterInstance(fileDataCache).As<IFileDataCache>();
            }))
            {
                ExecutionContext executionContext = (ExecutionContext)childLifetimeScope.Resolve<IExecutionContext>();
                executionContext.JobId = actorModel.JobId.ToString();
                executionContext.TaskKey = ActorId.ToString();
                ILogger jobLogger = childLifetimeScope.Resolve<ILogger>();

                try
                {
                    jobLogger.LogDebug($"{nameof(ALBActor)} started processing");
                    IFundingService<ILearner, ALBFundingOutputs> fundingService = childLifetimeScope.Resolve<IFundingService<ILearner, ALBFundingOutputs>>();

                    List<MessageLearner> learners = JsonSerializationService.Deserialize<List<MessageLearner>>(actorModel.ValidLearners);

                    actorModel = null;

                    ALBFundingOutputs results = fundingService.ProcessFunding(learners, cancellationToken);

                    jobLogger.LogDebug($"{nameof(ALBActor)} completed processing");
                    return JsonSerializationService.Serialize(results);
                }
                catch (Exception ex)
                {
                    ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                    jobLogger.LogError($"Error while processing {nameof(ALBActor)} job", ex);
                    throw;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Internal;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25Actor.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;

namespace ESFA.DC.ILR.FundingService.FM25Actor
{
    [StatePersistence(StatePersistence.None)]
    [ActorService(Name = ActorServiceNameConstants.FM25)]
    public class FM25Actor : AbstractFundingActor, IFM25Actor
    {
        public FM25Actor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope, IExecutionContext executionContext, IJsonSerializationService jsonSerializationService)
            : base(actorService, actorId, lifetimeScope, executionContext, jsonSerializationService)
        {
        }

        public async Task<string> Process(FundingActorDto actorModel, CancellationToken cancellationToken)
        {
            FM25Global results = RunFunding(actorModel, cancellationToken);
            actorModel = null;

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

            return BuildFundingOutput(results);
        }

        private FM25Global RunFunding(FundingActorDto actorModel, CancellationToken cancellationToken)
        {
            if (ExecutionContext is ExecutionContext executionContextObj)
            {
                executionContextObj.JobId = "-1";
                executionContextObj.TaskKey = ActorId.ToString();
            }

            ILogger logger = LifetimeScope.Resolve<ILogger>();

            IExternalDataCache externalDataCache;
            IInternalDataCache internalDataCache;
            IFileDataCache fileDataCache;
            FM25Global condensedResults;

            try
            {
                logger.LogDebug($"{nameof(FM25Actor)} {ActorId} starting");

                externalDataCache = BuildExternalDataCache(actorModel.ExternalDataCache);
                internalDataCache = BuildInternalDataCache(actorModel.InternalDataCache);
                fileDataCache = BuildFileDataCache(actorModel.FileDataCache);

                logger.LogDebug($"{nameof(FM25Actor)} {ActorId} finished getting input data");

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception ex)
            {
                ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                logger.LogError($"Error while processing {nameof(FM25Actor)}", ex);
                throw;
            }

            using (var childLifetimeScope = LifetimeScope.BeginLifetimeScope(c =>
            {
                c.RegisterInstance(externalDataCache).As<IExternalDataCache>();
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
                    jobLogger.LogDebug($"{nameof(FM25Actor)} {ActorId} {GC.GetGeneration(actorModel)} started processing");

                    IEnumerable<FM25Global> fm25Results;
                    IEnumerable<PeriodisationGlobal> fm25PeriodisationResults;

                    using (var fundingServiceLifetimeScope = childLifetimeScope.BeginLifetimeScope(c =>
                    {
                        c.RegisterInstance(new RulebaseProvider("FM25 Funding Calc 18_19")).As<IRulebaseProvider>();
                    }))
                    {
                        jobLogger.LogDebug("FM25 Rulebase Starting");

                        IFundingService<ILearner, IEnumerable<FM25Global>> fundingService = fundingServiceLifetimeScope.Resolve<IFundingService<ILearner, IEnumerable<FM25Global>>>();

                        var learners = BuildLearners(actorModel.ValidLearners);

                        fm25Results = fundingService.ProcessFunding(learners, cancellationToken).ToList();

                        jobLogger.LogDebug("FM25 Rulebase Finishing");
                    }

                    using (var fundingServiceLifetimeScope = childLifetimeScope.BeginLifetimeScope(c =>
                    {
                        c.RegisterInstance(new RulebaseProvider("FM25 Periodisation")).As<IRulebaseProvider>();
                    }))
                    {
                        jobLogger.LogDebug("FM25 Periodisation Rulebase Starting");

                        IFundingService<FM25Global, IEnumerable<PeriodisationGlobal>> periodisationService = fundingServiceLifetimeScope.Resolve<IFundingService<FM25Global, IEnumerable<PeriodisationGlobal>>>();

                        fm25PeriodisationResults = periodisationService.ProcessFunding(fm25Results, cancellationToken).ToList();

                        jobLogger.LogDebug("FM25 Periodisation Rulebase Finishing");
                    }

                    jobLogger.LogDebug($"{nameof(FM25Actor)} {ActorId} {GC.GetGeneration(actorModel)} completed processing");

                    condensedResults = CondenseResults(fm25Results, fm25PeriodisationResults);
                }
                catch (Exception ex)
                {
                    ActorEventSource.Current.ActorMessage(this, "Exception-{0}", ex.ToString());
                    jobLogger.LogError($"Error while processing {nameof(FM25Actor)}", ex);
                    throw;
                }
            }

            externalDataCache = null;
            internalDataCache = null;
            fileDataCache = null;

            return condensedResults;
        }

        private FM25Global CondenseResults(IEnumerable<FM25Global> globals, IEnumerable<PeriodisationGlobal> periodisationGlobals)
        {
            var first = globals.FirstOrDefault();

            var emptyLearnerPeriodsList = new List<LearnerPeriod>();
            var emptyLearnerPeriodisedValuesList = new List<LearnerPeriodisedValues>();

            if (first != null)
            {
                var learners = globals.SelectMany(g => g.Learners).ToList();
                var learnerPeriodsDictionary = periodisationGlobals.SelectMany(pg => pg.LearnerPeriods).GroupBy(lp => lp.LearnRefNumber).ToDictionary(lp => lp.Key, lp => lp.ToList());
                var learnerPeriodisedValuesDictionary = periodisationGlobals.SelectMany(pg => pg.LearnerPeriodisedValues).GroupBy(lp => lp.LearnRefNumber).ToDictionary(lp => lp.Key, lp => lp.ToList());

                foreach (var learner in learners)
                {
                    learnerPeriodsDictionary.TryGetValue(learner.LearnRefNumber, out var matchingLearnerPeriods);
                    learnerPeriodisedValuesDictionary.TryGetValue(learner.LearnRefNumber, out var matchinglearnerPeriodisedValues);

                    learner.LearnerPeriods = matchingLearnerPeriods ?? emptyLearnerPeriodsList;
                    learner.LearnerPeriodisedValues = matchinglearnerPeriodisedValues ?? emptyLearnerPeriodisedValuesList;
                }

                first.Learners = learners;

                return first;
            }

            return new FM25Global();
        }
    }
}
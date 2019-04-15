using System.Collections.Generic;
using Autofac;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.File;
using ESFA.DC.ILR.FundingService.Data.Internal;
using ESFA.DC.ILR.Model;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ESFA.DC.ILR.FundingService.ServiceFabric.Common
{
    public abstract class AbstractFundingActor : Actor
    {
        protected AbstractFundingActor(ActorService actorService, ActorId actorId, ILifetimeScope lifetimeScope, IExecutionContext executionContext, IJsonSerializationService jsonSerializationService)
            : base(actorService, actorId)
        {
            LifetimeScope = lifetimeScope;
            ExecutionContext = executionContext;
            JsonSerializationService = jsonSerializationService;
            ActorId = actorId;
        }

        public IExecutionContext ExecutionContext { get; }

        protected ActorId ActorId { get; }

        protected ILifetimeScope LifetimeScope { get; }

        private IJsonSerializationService JsonSerializationService { get; }

        public ExternalDataCache BuildExternalDataCache(string serialzedCache)
        {
            var deserialzedCache = JsonSerializationService.Deserialize<ExternalDataCache>(serialzedCache);

            return new ExternalDataCache
            {
                AECLatestInYearEarningHistory = deserialzedCache.AECLatestInYearEarningHistory,
                FCSContractAllocations = deserialzedCache.FCSContractAllocations,
                LargeEmployers = deserialzedCache.LargeEmployers,
                LARSAnnualValue = deserialzedCache.LARSAnnualValue.ToCaseInsensitiveDictionary(),
                LARSApprenticeshipFundingFrameworks = deserialzedCache.LARSApprenticeshipFundingFrameworks,
                LARSApprenticeshipFundingStandards = deserialzedCache.LARSApprenticeshipFundingStandards,
                LARSCurrentVersion = deserialzedCache.LARSCurrentVersion,
                LARSFrameworkAims = deserialzedCache.LARSFrameworkAims.ToCaseInsensitiveDictionary(),
                LARSFrameworkCommonComponent = deserialzedCache.LARSFrameworkCommonComponent,
                LARSFunding = deserialzedCache.LARSFunding.ToCaseInsensitiveDictionary(),
                LARSLearningDelivery = deserialzedCache.LARSLearningDelivery.ToCaseInsensitiveDictionary(),
                LARSLearningDeliveryCategory = deserialzedCache.LARSLearningDeliveryCategory.ToCaseInsensitiveDictionary(),
                LARSStandardCommonComponent = deserialzedCache.LARSStandardCommonComponent,
                LARSStandardFundings = deserialzedCache.LARSStandardFundings,
                OrgFunding = deserialzedCache.OrgFunding,
                OrgVersion = deserialzedCache.OrgVersion,
                PostcodeCurrentVersion = deserialzedCache.PostcodeCurrentVersion,
                PostcodeRoots = deserialzedCache.PostcodeRoots.ToCaseInsensitiveDictionary(),
            };
        }

        public InternalDataCache BuildInternalDataCache(string serialzedCache)
        {
            var deserialzedCache = JsonSerializationService.Deserialize<InternalDataCache>(serialzedCache);

            return new InternalDataCache
            {
                Period1 = deserialzedCache.Period1,
                Period2 = deserialzedCache.Period2,
                Period3 = deserialzedCache.Period3,
                Period4 = deserialzedCache.Period4,
                Period5 = deserialzedCache.Period5,
                Period6 = deserialzedCache.Period6,
                Period7 = deserialzedCache.Period7,
                Period8 = deserialzedCache.Period8,
                Period9 = deserialzedCache.Period9,
                Period10 = deserialzedCache.Period10,
                Period11 = deserialzedCache.Period11,
                Period12 = deserialzedCache.Period12
            };
        }

        public FileDataCache BuildFileDataCache(string serialzedCache)
        {
            var deserialzedCache = JsonSerializationService.Deserialize<FileDataCache>(serialzedCache);

            return new FileDataCache
            {
                UKPRN = deserialzedCache.UKPRN,
                DPOutcomes = deserialzedCache.DPOutcomes.ToCaseInsensitiveDictionary()
            };
        }

        public List<MessageLearner> BuildLearners(string serializedLearners)
        {
            return JsonSerializationService.Deserialize<List<MessageLearner>>(serializedLearners);
        }

        public string BuildFundingOutput<T>(T model)
        {
            return JsonSerializationService.Serialize(model);
        }
    }
}

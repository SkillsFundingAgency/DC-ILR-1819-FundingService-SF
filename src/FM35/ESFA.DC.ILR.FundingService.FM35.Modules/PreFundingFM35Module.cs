﻿using System.Collections.Generic;
using Autofac;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.ILR.FundingService.FM35.Contexts;
using ESFA.DC.ILR.FundingService.FM35.Contexts.Interface;
using ESFA.DC.ILR.FundingService.FM35.ExternalData;
using ESFA.DC.ILR.FundingService.FM35.ExternalData.Interface;
using ESFA.DC.ILR.FundingService.FM35.InternalData;
using ESFA.DC.ILR.FundingService.FM35.InternalData.Interface;
using ESFA.DC.ILR.FundingService.FM35.OrchestrationService;
using ESFA.DC.ILR.FundingService.FM35.OrchestrationService.Interface;
using ESFA.DC.ILR.FundingService.FM35.Service.Interface;
using ESFA.DC.ILR.FundingService.FM35.Stubs;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.Modules
{
    public class PreFundingFM35Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LARS>().As<ILARS>().InstancePerLifetimeScope();
            builder.RegisterType<Postcodes>().As<IPostcodes>().InstancePerLifetimeScope();
            builder.RegisterType<PreFundingFM35OrchestrationService>().As<IPreFundingFM35OrchestrationService>().InstancePerLifetimeScope();
            builder.RegisterType<PreFundingFM35PopulationService>().As<IPreFundingFM35PopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<ReferenceDataCachePopulationService>().As<IReferenceDataCachePopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<InternalDataCache>().As<IInternalDataCache>().InstancePerLifetimeScope();
            builder.RegisterType<LearnerPerActorServiceStub<ILearner, IList<ILearner>>>().As<ILearnerPerActorService<ILearner, IList<ILearner>>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingContext>().As<IFundingContext>().InstancePerLifetimeScope();
            builder.RegisterType<FundingContextManager>().As<IFundingContextManager>().InstancePerLifetimeScope();
            builder.RegisterType<ReferenceDataCache>().As<IReferenceDataCache>().InstancePerLifetimeScope();
        }
    }
}

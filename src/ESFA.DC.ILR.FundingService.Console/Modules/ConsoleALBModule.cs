﻿using System;
using System.Collections.Generic;
using Autofac;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALB.Service;
using ESFA.DC.ILR.FundingService.ALB.Service.Builders;
using ESFA.DC.ILR.FundingService.ALB.Service.Builders.Interface;
using ESFA.DC.ILR.FundingService.Data.Context;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population;
using ESFA.DC.ILR.FundingService.Data.Population.Context;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Factory;
using ESFA.DC.ILR.FundingService.Service;
using ESFA.DC.ILR.FundingService.Stubs;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.IO.Dictionary;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.JobContext;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.OPA.Service;
using ESFA.DC.OPA.Service.Builders;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Builders;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Xml;

namespace ESFA.DC.ILR.FundingService.Console.Modules
{
    public class ConsoleALBModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LARS>().As<ILARS>().InstancePerLifetimeScope();
            builder.RegisterType<Postcodes>().As<IPostcodes>().InstancePerLifetimeScope();
            builder.RegisterType<SessionBuilder>().As<ISessionBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<OPADataEntityBuilder>().As<IOPADataEntityBuilder>().WithParameter("yearStartDate", new System.DateTime(2017, 8, 1)).InstancePerLifetimeScope();
            builder.RegisterType<RulebaseProviderFactory>().As<IRulebaseProviderFactory>().InstancePerLifetimeScope();
            builder.RegisterType<OPAService>().As<IOPAService>().InstancePerLifetimeScope();
            builder.RegisterType<ALBAttributeBuilder>().As<IALBAttributeBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<ILearner>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<FundingOutputs>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<ILearner, FundingOutputs>>().As<IFundingService<ILearner, FundingOutputs>>().InstancePerLifetimeScope();
            builder.RegisterType<LearnerPagingService<ILearner>>().As<IPagingService<ILearner>>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalDataCache>().As<IExternalDataCache>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalDataCachePopulationService>().As<IExternalDataCachePopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<PopulationService>().As<IPopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<XmlSerializationService>().As<ISerializationService>().InstancePerLifetimeScope();
            builder.RegisterType<FundingContext>().As<IFundingContext>().InstancePerLifetimeScope();
            builder.RegisterType<FundingContextPopulationService>().As<IFundingContextPopulationService>().InstancePerLifetimeScope();
            builder.RegisterType<DictionaryKeyValuePersistenceService>().As<IKeyValuePersistenceService>().InstancePerLifetimeScope();
            builder.RegisterType<TaskProviderService>().As<ITaskProviderService>().InstancePerLifetimeScope();
            builder.Register(ctx => BuildJobContext()).As<IJobContextMessage>().InstancePerLifetimeScope();
        }

        private static JobContextMessage BuildJobContext()
        {
            return new JobContextMessage
            {
                JobId = 1,
                SubmissionDateTimeUtc = System.DateTime.Parse("2018-08-01").ToUniversalTime(),
                Topics = TopicList,
                TopicPointer = 1,
                KeyValuePairs = new Dictionary<JobContextMessageKey, object>
                {
                    { JobContextMessageKey.Filename, "fileName" },
                    { JobContextMessageKey.UkPrn, 10006341 },
                    { JobContextMessageKey.ValidLearnRefNumbers, "ValidLearnRefNumbers" },
                },
            };
        }

        private static ITaskItem TaskItem => new TaskItem
        {
            Tasks = new List<string>
            {
                "Task A",
            },
            SupportsParallelExecution = true,
        };

        private static IReadOnlyList<ITaskItem> TaskItemList => new List<ITaskItem> { TaskItem };

        private static ITopicItem TopicItem => new TopicItem("Subscription", "SubscriptionFilter", TaskItemList);

        private static IReadOnlyList<ITopicItem> TopicList => new List<ITopicItem> { TopicItem };
    }
}
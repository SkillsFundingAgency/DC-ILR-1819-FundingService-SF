using System;
using System.Threading;
using Autofac;
using Autofac.Integration.ServiceFabric;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Config;
using ESFA.DC.ILR.FundingService.Config.Interfaces;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Modules;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;
using ESFA.DC.ServiceFabric.Helpers;
using Microsoft.ServiceFabric.Actors.Runtime;
using ActorFundingALBModule = ESFA.DC.ILR.FundingService.ALBActor.Modules.ActorFundingALBModule;

namespace ESFA.DC.ILR.FundingService.ALBActor
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                var builder = BuildContainer();

                // Register the Autofac magic for Service Fabric support.
                builder.RegisterServiceFabricSupport();

                // Register the actor service.
                builder.RegisterActor<ALBActor>(settings: new ActorServiceSettings
                {
                    ActorGarbageCollectionSettings = new ActorGarbageCollectionSettings(30, 30)
                });

                using (var container = builder.Build())
                {
                    // Not sure why this is being resolved here, to review
                    var ss = container.Resolve<IFundingService<ILearner, ALBGlobal>>();
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (Exception e)
            {
                ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static ContainerBuilder BuildContainer()
        {
            var containerBuilder = new ContainerBuilder();
            var configHelper = new ConfigurationHelper();

            // register actor ALB funding module
            containerBuilder.RegisterModule<ActorFundingALBModule>();

            var loggerConfig = configHelper.GetSectionValues<LoggerConfig>("LoggerSection");

            containerBuilder.RegisterInstance(loggerConfig).As<ILoggerConfig>().SingleInstance();
            containerBuilder.RegisterModule<LoggerModule>();

            return containerBuilder;
        }
    }
}

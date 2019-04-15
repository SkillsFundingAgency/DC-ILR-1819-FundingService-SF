using System;
using System.Threading;
using Autofac;
using Autofac.Integration.ServiceFabric;
using ESFA.DC.ILR.FundingService.Config;
using ESFA.DC.ILR.FundingService.Config.Interfaces;
using ESFA.DC.ILR.FundingService.FM81Actor.Modules;
using ESFA.DC.ILR.FundingService.Modules;
using ESFA.DC.ServiceFabric.Helpers;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ESFA.DC.ILR.FundingService.FM81Actor
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

                builder.RegisterServiceFabricSupport();
                builder.RegisterActor<FM81Actor>(settings: new ActorServiceSettings
                {
                    ActorGarbageCollectionSettings = new ActorGarbageCollectionSettings(30, 30)
                });

                using (var container = builder.Build())
                {
                    // Not sure why this is being resolved here, to review
                    //  var ss = container.Resolve<IFundingService<ILearner, FM35FundingOutputs>>();
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
            containerBuilder.RegisterModule<ActorFundingFM81Module>();

            var loggerConfig = configHelper.GetSectionValues<LoggerConfig>("LoggerSection");

            containerBuilder.RegisterInstance(loggerConfig).As<ILoggerConfig>().SingleInstance();
            containerBuilder.RegisterModule<LoggerModule>();

            // register serialization
            return containerBuilder;
        }
    }
}

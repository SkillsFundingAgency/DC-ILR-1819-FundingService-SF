using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace ESFA.DC.ILR.FundingService.ServiceFabric.Common
{
    public class ActorProvider<T> : IActorProvider<T>
        where T : IFundingActor
    {
        private readonly string _actorServiceName;

        public ActorProvider(string actorServiceName)
        {
            _actorServiceName = actorServiceName;
        }

        public T Provide()
        {
            return ActorProxy.Create<T>(ActorId.CreateRandom(), FabricRuntime.GetActivationContext().ApplicationName, _actorServiceName);
        }

        public async Task DestroyAsync(T actor, CancellationToken cancellationToken)
        {
            try
            {
                ActorId actorId = actor.GetActorId();

                IActorService myActorServiceProxy = ActorServiceProxy.Create(
                    new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/{_actorServiceName}"), actorId);

                await myActorServiceProxy.DeleteActorAsync(actorId, cancellationToken);
            }
            catch (Exception ex)
            {
            }
        }
    }
}

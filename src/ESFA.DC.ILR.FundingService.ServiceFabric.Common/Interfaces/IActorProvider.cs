using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces
{
    public interface IActorProvider<T>
        where T : IFundingActor
    {
        T Provide();

        Task DestroyAsync(T actor, CancellationToken cancellationToken);
    }
}

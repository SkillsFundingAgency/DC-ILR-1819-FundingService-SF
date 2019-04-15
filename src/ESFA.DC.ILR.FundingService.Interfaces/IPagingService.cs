using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Interfaces
{
    public interface IPagingService<out T>
    {
        IEnumerable<IEnumerable<T>> BuildPages(IEnumerable<int> fundModelFilter);
    }
}

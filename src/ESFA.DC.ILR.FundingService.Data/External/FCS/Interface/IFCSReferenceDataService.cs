using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;

namespace ESFA.DC.ILR.FundingService.Data.External.FCS.Interface
{
    public interface IFCSReferenceDataService
    {
        IEnumerable<FCSContractAllocation> FcsContractsForConRef(string conRefNumber);
    }
}

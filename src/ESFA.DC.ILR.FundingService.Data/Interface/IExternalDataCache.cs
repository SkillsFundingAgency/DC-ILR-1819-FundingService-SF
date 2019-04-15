using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;

namespace ESFA.DC.ILR.FundingService.Data.Interface
{
    public interface IExternalDataCache
    {
        IDictionary<string, IEnumerable<LARSFunding>> LARSFunding { get; }

        IDictionary<string, LARSLearningDelivery> LARSLearningDelivery { get; }

        string LARSCurrentVersion { get; }
        
        IDictionary<string, PostcodeRoot> PostcodeRoots { get; }

        string PostcodeCurrentVersion { get; }

        IDictionary<string, IEnumerable<LARSAnnualValue>> LARSAnnualValue { get; }

        IDictionary<int, IEnumerable<LARSStandardCommonComponent>> LARSStandardCommonComponent { get; }

        IEnumerable<LARSFrameworkCommonComponent> LARSFrameworkCommonComponent { get; }

        IEnumerable<LARSStandardApprenticeshipFunding> LARSApprenticeshipFundingStandards { get; }

        IEnumerable<LARSFrameworkApprenticeshipFunding> LARSApprenticeshipFundingFrameworks { get; }

        IDictionary<string, IEnumerable<LARSFrameworkAims>> LARSFrameworkAims { get; }

        IDictionary<string, IEnumerable<LARSLearningDeliveryCategory>> LARSLearningDeliveryCategory { get; }

        IDictionary<int, IEnumerable<LARSStandardFunding>> LARSStandardFundings { get; }

        string OrgVersion { get; }

        IDictionary<long, IEnumerable<OrgFunding>> OrgFunding { get; }

        IDictionary<int, IEnumerable<LargeEmployers>> LargeEmployers { get; }

        IDictionary<long, IEnumerable<AECEarningsHistory>> AECLatestInYearEarningHistory { get; }

        IEnumerable<FCSContractAllocation> FCSContractAllocations { get; }
    }
}

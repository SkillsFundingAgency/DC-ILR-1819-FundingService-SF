using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External
{
    public class ExternalDataCache : IExternalDataCache
    {
        public string LARSCurrentVersion { get; set; }

        public IDictionary<string, IEnumerable<LARSFunding>> LARSFunding { get; set; }

        public IDictionary<string, LARSLearningDelivery> LARSLearningDelivery { get; set; }

        public IDictionary<string, IEnumerable<LARSAnnualValue>> LARSAnnualValue { get; set; }

        public IDictionary<string, IEnumerable<LARSFrameworkAims>> LARSFrameworkAims { get; set; }

        public IDictionary<int, IEnumerable<LARSStandardCommonComponent>> LARSStandardCommonComponent { get; set; }

        public IEnumerable<LARSFrameworkCommonComponent> LARSFrameworkCommonComponent { get; set; }

        public IEnumerable<LARSStandardApprenticeshipFunding> LARSApprenticeshipFundingStandards { get; set; }

        public IEnumerable<LARSFrameworkApprenticeshipFunding> LARSApprenticeshipFundingFrameworks { get; set; }

        public IDictionary<string, IEnumerable<LARSLearningDeliveryCategory>> LARSLearningDeliveryCategory { get; set; }

        public IDictionary<int, IEnumerable<LARSStandardFunding>> LARSStandardFundings { get; set; }

        public string PostcodeCurrentVersion { get; set; }

        public IDictionary<string, PostcodeRoot> PostcodeRoots { get; set; }
        
        public string OrgVersion { get; set; }

        public IDictionary<long, IEnumerable<OrgFunding>> OrgFunding { get; set; }

        public IDictionary<int, IEnumerable<LargeEmployers>> LargeEmployers { get; set; }

        public IDictionary<long, IEnumerable<AECEarningsHistory>> AECLatestInYearEarningHistory { get; set; }

        public IEnumerable<FCSContractAllocation> FCSContractAllocations { get; set; }
    }
}

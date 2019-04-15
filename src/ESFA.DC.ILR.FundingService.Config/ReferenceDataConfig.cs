using ESFA.DC.ILR.FundingService.Config.Interfaces;

namespace ESFA.DC.ILR.FundingService.Config
{
    public class ReferenceDataConfig : IReferenceDataConfig
    {
        public string LARSConnectionString { get; set; }

        public string PostcodesConnectionString { get; set; }

        public string OrganisationConnectionString { get; set; }

        public string LargeEmployersConnectionString { get; set; }

        public string AppsEarningsHistoryConnectionString { get; set; }

        public string FCSConnectionString { get; set; }
    }
}

namespace ESFA.DC.ILR.FundingService.Config.Interfaces
{
    public interface IReferenceDataConfig
    {
        string LARSConnectionString { get; }

        string PostcodesConnectionString { get; }

        string OrganisationConnectionString { get; }

        string LargeEmployersConnectionString { get; }

        string AppsEarningsHistoryConnectionString { get; }

        string FCSConnectionString { get; }
    }
}
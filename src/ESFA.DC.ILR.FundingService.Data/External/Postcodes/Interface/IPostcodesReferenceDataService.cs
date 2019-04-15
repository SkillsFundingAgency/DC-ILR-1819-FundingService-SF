using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;

namespace ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface
{
    public interface IPostcodesReferenceDataService
    {
        string PostcodesCurrentVersion();

        IEnumerable<SfaAreaCost> SFAAreaCostsForPostcode(string postcode);

        IEnumerable<DasDisadvantage> DASDisadvantagesForPostcode(string postcode);

        IEnumerable<SfaDisadvantage> SFADisadvantagesForPostcode(string postcode);

        decimal? LatestEFADisadvantagesUpliftForPostcode(string postcode);

        IEnumerable<CareerLearningPilot> CareerLearningPilotsForPostcode(string postcode);
    }
}

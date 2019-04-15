using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External.Postcodes
{
    public class PostcodesReferenceDataService : IPostcodesReferenceDataService
    {
        private readonly IExternalDataCache _referenceDataCache;
        private readonly IEnumerable<SfaAreaCost> _emptySfaAreaCost = new List<SfaAreaCost>();
        private readonly IEnumerable<DasDisadvantage> _emptyDasDisadvantage = new List<DasDisadvantage>();
        private readonly IEnumerable<SfaDisadvantage> _emptySfaDisadvantage = new List<SfaDisadvantage>();
        private readonly IEnumerable<EfaDisadvantage> _emptyEfaDisadvantage = new List<EfaDisadvantage>();
        private readonly IEnumerable<CareerLearningPilot> _emptyCareerLearningPilot = new List<CareerLearningPilot>();

        public PostcodesReferenceDataService(IExternalDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        public string PostcodesCurrentVersion()
        {
            return _referenceDataCache.PostcodeCurrentVersion;
        }

        public IEnumerable<SfaAreaCost> SFAAreaCostsForPostcode(string postcode)
        {
            _referenceDataCache.PostcodeRoots.TryGetValue(postcode, out PostcodeRoot postcodeRoot);

            return postcodeRoot?.SfaAreaCosts ?? _emptySfaAreaCost;
        }

        public IEnumerable<DasDisadvantage> DASDisadvantagesForPostcode(string postcode)
        {
            _referenceDataCache.PostcodeRoots.TryGetValue(postcode, out PostcodeRoot postcodeRoot);

            return postcodeRoot?.DasDisadvantages ?? _emptyDasDisadvantage;
        }


        public IEnumerable<SfaDisadvantage> SFADisadvantagesForPostcode(string postcode)
        {
            _referenceDataCache.PostcodeRoots.TryGetValue(postcode, out PostcodeRoot postcodeRoot);

            return postcodeRoot?.SfaDisadvantages ?? _emptySfaDisadvantage;
        }

        public decimal? LatestEFADisadvantagesUpliftForPostcode(string postcode)
        {
            _referenceDataCache.PostcodeRoots.TryGetValue(postcode, out PostcodeRoot postcodeRoot);

            return
                postcodeRoot?
                .EfaDisadvantages
                .OrderByDescending(ef => ef.EffectiveFrom)
                .Select(u => u.Uplift)
                .FirstOrDefault();
        }

        public IEnumerable<CareerLearningPilot> CareerLearningPilotsForPostcode(string postcode)
        {
            _referenceDataCache.PostcodeRoots.TryGetValue(postcode, out PostcodeRoot postcodeRoot);

            return postcodeRoot?.CareerLearningPilots ?? _emptyCareerLearningPilot;
        }
    }
}

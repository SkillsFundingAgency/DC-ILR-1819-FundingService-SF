using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model
{
    public class PostcodeRoot
    {
        public string Postcode { get; set; }

        public IEnumerable<CareerLearningPilot> CareerLearningPilots { get; set; }

        public IEnumerable<DasDisadvantage> DasDisadvantages { get; set; }

        public IEnumerable<EfaDisadvantage> EfaDisadvantages { get; set; }

        public IEnumerable<SfaAreaCost> SfaAreaCosts { get; set; }

        public IEnumerable<SfaDisadvantage> SfaDisadvantages { get; set; }
    }
}

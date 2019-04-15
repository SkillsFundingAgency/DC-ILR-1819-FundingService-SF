using System;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Model
{
    public class LARSStandardFunding
    {
        public int StandardCode { get; set; }

        public string FundingCategory { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public int? BandNumber { get; set; }

        public decimal? CoreGovContributionCap { get; set; }

        public decimal? SixteenToEighteenIncentive { get; set; }

        public decimal? SmallBusinessIncentive { get; set; }

        public decimal? AchievementIncentive { get; set; } 

        public string FundableWithoutEmployer { get; set; }
    }
}

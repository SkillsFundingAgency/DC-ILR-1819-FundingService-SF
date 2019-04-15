using System;

namespace ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output
{
    public class LearningDeliveryValue
    {
        public bool? Achieved { get; set; }

        public bool? AddProgCostElig { get; set; }

        public decimal? AdjustedAreaCostFactor { get; set; }

        public decimal? AdjustedPremiumFactor { get; set; }

        public DateTime? AdjustedStartDate { get; set; }

        public string AimClassification { get; set; }

        public decimal? AimValue { get; set; }

        public decimal? ApplicWeightFundRate { get; set; }

        public long? EligibleProgressionOutcomeCode { get; set; }

        public string EligibleProgressionOutcomeType { get; set; }

        public DateTime? EligibleProgressionOutomeStartDate { get; set; }

        public bool? FundStart { get; set; }

        public decimal? LARSWeightedRate { get; set; }

        public DateTime? LatestPossibleStartDate { get; set; }

        public DateTime? LDESFEngagementStartDate { get; set; }

        public bool? LearnDelLearnerEmpAtStart { get; set; }

        public bool? PotentiallyEligibleForProgression { get; set; }

        public DateTime? ProgressionEndDate { get; set; }

        public bool? Restart { get; set; }

        public decimal? WeightedRateFromESOL { get; set; }
    }
}

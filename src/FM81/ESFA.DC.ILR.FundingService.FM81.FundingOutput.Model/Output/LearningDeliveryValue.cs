using System;

namespace ESFA.DC.ILR.FundingService.FM81.FundingOutput.Model.Output
{
    public class LearningDeliveryValue
    {
        public DateTime? AchApplicDate { get; set; }

        public bool? AchEligible { get; set; }

        public bool? Achieved { get; set; }

        public decimal? AchievementApplicVal { get; set; }

        public decimal? AchPayment { get; set; }

        public int? ActualDaysIL { get; set; }

        public int? ActualNumInstalm { get; set; }

        public DateTime? AdjProgStartDate { get; set; }

        public int? AgeStandardStart { get; set; }

        public DateTime? ApplicFundValDate { get; set; }

        public decimal? CombinedAdjProp { get; set; }

        public long? CoreGovContCapApplicVal { get; set; }

        public decimal? CoreGovContPayment { get; set; }

        public decimal? CoreGovContUncapped { get; set; }

        public int? EmpIdAchDate { get; set; }

        public int? EmpIdFirstDayStandard { get; set; }

        public int? EmpIdFirstYoungAppDate { get; set; }

        public int? EmpIdSecondYoungAppDate { get; set; }

        public int? EmpIdSmallBusDate { get; set; }

        public string FundLine { get; set; }

        public int? InstPerPeriod { get; set; }

        public int? LearnDelDaysIL { get; set; }

        public int? LearnDelStandardAccDaysIL { get; set; }

        public int? LearnDelStandardPrevAccDaysIL { get; set; }

        public int? LearnDelStandardTotalDaysIL { get; set; }

        public bool? LearnSuppFund { get; set; }

        public decimal? LearnSuppFundCash { get; set; }

        public decimal? MathEngAimValue { get; set; }

        public decimal? MathEngBalPayment { get; set; }

        public long? MathEngBalPct { get; set; }

        public bool? MathEngLSFFundStart { get; set; }

        public int? MathEngLSFThresholdDays { get; set; }

        public decimal? MathEngOnProgPayment { get; set; }

        public int? MathEngOnProgPct { get; set; }

        public int? OutstandNumOnProgInstalm { get; set; }

        public int? PlannedNumOnProgInstalm { get; set; }

        public int? PlannedTotalDaysIL { get; set; }

        public DateTime? ProgStandardStartDate { get; set; }

        public decimal? SmallBusApplicVal { get; set; }

        public bool? SmallBusEligible { get; set; }

        public decimal? SmallBusPayment { get; set; }

        public int? SmallBusStatusFirstDayStandard { get; set; }

        public int? SmallBusStatusThreshold { get; set; }

        public decimal? YoungAppApplicVal { get; set; }

        public bool? YoungAppEligible { get; set; }

        public decimal? YoungAppFirstPayment { get; set; }

        public DateTime? YoungAppFirstThresholdDate { get; set; }

        public decimal? YoungAppPayment { get; set; }

        public decimal? YoungAppSecondPayment { get; set; }

        public DateTime? YoungAppSecondThresholdDate { get; set; }
    }
}

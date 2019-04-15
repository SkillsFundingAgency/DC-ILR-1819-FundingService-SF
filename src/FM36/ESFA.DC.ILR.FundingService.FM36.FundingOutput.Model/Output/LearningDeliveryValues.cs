using System;

namespace ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output
{
    public class LearningDeliveryValues
    {
        public int? ActualDaysIL { get; set; }

        public int? ActualNumInstalm { get; set; }

        public DateTime? AdjStartDate { get; set; }

        public int? AgeAtProgStart { get; set; }

        public DateTime? AppAdjLearnStartDate { get; set; }

        public DateTime? AppAdjLearnStartDateMatchPathway { get; set; }

        public DateTime? ApplicCompDate { get; set; }

        public decimal? CombinedAdjProp { get; set; }

        public bool? Completed { get; set; }

        public DateTime? FirstIncentiveThresholdDate { get; set; }

        public bool? FundStart { get; set; }

        public int? FworkCode { get; set; }

        public decimal? LDApplic1618FrameworkUpliftBalancingValue { get; set; }

        public decimal? LDApplic1618FrameworkUpliftCompElement { get; set; }

        public decimal? LDApplic1618FRameworkUpliftCompletionValue { get; set; }

        public decimal? LDApplic1618FrameworkUpliftMonthInstalVal { get; set; }

        public decimal? LDApplic1618FrameworkUpliftPrevEarnings { get; set; }

        public decimal? LDApplic1618FrameworkUpliftPrevEarningsStage1 { get; set; }

        public decimal? LDApplic1618FrameworkUpliftRemainingAmount { get; set; }

        public decimal? LDApplic1618FrameworkUpliftTotalActEarnings { get; set; }

        public string LearnAimRef { get; set; }

        public DateTime LearnStartDate { get; set; }

        public bool? LearnDel1618AtStart { get; set; }

        public int? LearnDelAppAccDaysIL { get; set; }

        public decimal? LearnDelApplicDisadvAmount { get; set; }

        public decimal? LearnDelApplicEmp1618Incentive { get; set; }

        public DateTime? LearnDelApplicEmpDate { get; set; }

        public decimal? LearnDelApplicProv1618FrameworkUplift { get; set; }

        public decimal? LearnDelApplicProv1618Incentive { get; set; }

        public int? LearnDelAppPrevAccDaysIL { get; set; }

        public int? LearnDelDaysIL { get; set; }

        public decimal? LearnDelDisadAmount { get; set; }

        public bool? LearnDelEligDisadvPayment { get; set; }

        public int? LearnDelEmpIdFirstAdditionalPaymentThreshold { get; set; }

        public int? LearnDelEmpIdSecondAdditionalPaymentThreshold { get; set; }

        public int? LearnDelHistDaysThisApp { get; set; }

        public decimal? LearnDelHistProgEarnings { get; set; }

        public string LearnDelInitialFundLineType { get; set; }

        public bool? LearnDelMathEng { get; set; }

        public DateTime? LearnDelProgEarliestACT2Date { get; set; }

        public bool? LearnDelNonLevyProcured { get; set; }

        public decimal? MathEngAimValue { get; set; }

        public int? OutstandNumOnProgInstalm { get; set; }

        public int? PlannedNumOnProgInstalm { get; set; }

        public int? PlannedTotalDaysIL { get; set; }

        public int? ProgType { get; set; }

        public int? PwayCode { get; set; }

        public DateTime? SecondIncentiveThresholdDate { get; set; }

        public int? StdCode { get; set; }

        public int? ThresholdDays { get; set; }

        public decimal? LearnDelApplicCareLeaverIncentive { get; set; }

        public int? LearnDelHistDaysCareLeavers { get; set; }

        public int? LearnDelAccDaysILCareLeavers { get; set; }

        public int? LearnDelPrevAccDaysILCareLeavers { get; set; }

        public DateTime? LearnDelLearnerAddPayThresholdDate { get; set; }

        public int? LearnDelRedCode { get; set; }

        public DateTime? LearnDelRedStartDate { get; set; }
    }
}

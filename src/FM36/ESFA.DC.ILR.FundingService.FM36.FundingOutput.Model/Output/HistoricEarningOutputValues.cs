using System;

namespace ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output
{
    public class HistoricEarningOutputValues
    {
        public string AppIdentifierOutput { get; set; }

        public bool? AppProgCompletedInTheYearOutput { get; set; }

        public decimal? BalancingProgAimPaymentsInTheYear { get; set; }

        public decimal? CompletionProgAimPaymentsInTheYear { get; set; }

        public int? HistoricDaysInYearOutput { get; set; }

        public DateTime? HistoricEffectiveTNPStartDateOutput { get; set; }

        public int? HistoricEmpIdEndWithinYearOutput { get; set; }

        public int? HistoricEmpIdStartWithinYearOutput { get; set; }

        public int? HistoricFworkCodeOutput { get; set; }

        public bool? HistoricLearner1618AtStartOutput { get; set; }

        public decimal? OnProgProgAimPaymentsInTheYear { get; set; }

        public decimal? HistoricPMRAmountOutput { get; set; }

        public DateTime? HistoricProgrammeStartDateIgnorePathwayOutput { get; set; }

        public DateTime? HistoricProgrammeStartDateMatchPathwayOutput { get; set; }

        public int? HistoricProgTypeOutput { get; set; }

        public int? HistoricPwayCodeOutput { get; set; }

        public int? HistoricSTDCodeOutput { get; set; }

        public decimal? HistoricTNP1Output { get; set; }

        public decimal? HistoricTNP2Output { get; set; }

        public decimal? HistoricTNP3Output { get; set; }

        public decimal? HistoricTNP4Output { get; set; }

        public decimal? HistoricTotal1618UpliftPaymentsInTheYear { get; set; }

        public decimal? HistoricTotalProgAimPaymentsInTheYear { get; set; }

        public long? HistoricULNOutput { get; set; }

        public DateTime? HistoricUptoEndDateOutput { get; set; }

        public decimal? HistoricVirtualTNP3EndofThisYearOutput { get; set; }

        public decimal? HistoricVirtualTNP4EndofThisYearOutput { get; set; }

        public DateTime? HistoricLearnDelProgEarliestACT2DateOutput { get; set; }
    }
}

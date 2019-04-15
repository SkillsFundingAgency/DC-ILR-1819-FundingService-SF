using System;

namespace ESFA.DC.ILR.FundingService.FM70.Service.Models
{
    public class EsfData
    {
        public DateTime? EffectiveContractEndDate { get; set; }

        public DateTime? EffectiveContractStartDate { get; set; }

        public decimal? ESFDataPremiumFactor { get; set; }

        public string ESFDeliverableCode { get; set; }

        public int? CalcMethod { get; set; }

        public decimal? UnitCost { get; set; }
    }
}

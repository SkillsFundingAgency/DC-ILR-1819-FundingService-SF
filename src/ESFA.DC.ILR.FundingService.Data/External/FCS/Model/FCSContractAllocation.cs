using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.Data.External.FCS.Model
{
    public class FCSContractAllocation
    {
        public string ContractAllocationNumber { get; set; }

        public string FundingStreamPeriodCode { get; set; }

        public decimal? LearningRatePremiumFactor { get; set; }

        public string TenderSpecReference { get; set; }

        public string LotReference { get; set; }

        public int? CalcMethod { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public List<FCSContractDeliverable> FCSContractDeliverables { get; set; }
    }
}

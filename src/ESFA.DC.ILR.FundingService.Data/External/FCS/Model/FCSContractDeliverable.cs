namespace ESFA.DC.ILR.FundingService.Data.External.FCS.Model
{
    public class FCSContractDeliverable
    {
        public int? DeliverableCode { get; set; }

        public string DeliverableDescription { get; set; }

        public string ExternalDeliverableCode { get; set; }

        public decimal? UnitCost { get; set; }

        public int? PlannedVolume { get; set; }

        public decimal? PlannedValue { get; set; }
    }
}

using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output
{
    public class LearningDelivery
    {
        public int? AimSeqNumber { get; set; }

        public LearningDeliveryValue LearningDeliveryValue { get; set; }

        public List<LearningDeliveryDeliverableValues> LearningDeliveryDeliverableValues { get; set; }
    }
}
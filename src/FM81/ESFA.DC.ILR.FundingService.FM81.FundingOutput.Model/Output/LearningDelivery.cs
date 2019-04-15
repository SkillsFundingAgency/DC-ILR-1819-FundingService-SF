using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM81.FundingOutput.Model.Output
{
    public class LearningDelivery
    {
        public int? AimSeqNumber { get; set; }

        public LearningDeliveryValue LearningDeliveryValues { get; set; }

        public List<LearningDeliveryPeriodisedValue> LearningDeliveryPeriodisedValues { get; set; }
    }
}

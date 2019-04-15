using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output
{
    public class LearningDelivery
    {
        public int AimSeqNumber { get; set; }

        public LearningDeliveryValues LearningDeliveryValues { get; set; }

        public List<LearningDeliveryPeriodisedValues> LearningDeliveryPeriodisedValues { get; set; }

        public List<LearningDeliveryPeriodisedTextValues> LearningDeliveryPeriodisedTextValues { get; set; }
    }
}

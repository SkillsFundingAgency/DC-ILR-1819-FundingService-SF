using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output
{
    public class LearningDeliveryDeliverableValues
    {
        public string DeliverableCode { get; set; }

        public decimal? DeliverableUnitCost { get; set; }

        public List<LearningDeliveryDeliverablePeriodisedValue> LearningDeliveryDeliverablePeriodisedValues { get; set; }
    }
}

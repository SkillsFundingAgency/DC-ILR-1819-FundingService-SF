using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output
{
    public class FM70Learner
    {
        public string LearnRefNumber { get; set; }

        public List<LearningDelivery> LearningDeliveries { get; set; }

        public List<LearnerDPOutcome> LearnerDPOutcomes { get; set; }
    }
}

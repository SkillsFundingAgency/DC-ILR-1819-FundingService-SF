using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM81.FundingOutput.Model.Output
{
    public class FM81Learner
    {
        public string LearnRefNumber { get; set; }

        public List<LearningDelivery> LearningDeliveries { get; set; }
    }
}

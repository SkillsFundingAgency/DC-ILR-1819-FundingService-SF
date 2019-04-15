using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output
{
    public class FM70Global
    {
        public int UKPRN { get; set; }

        public string RulebaseVersion { get; set; }

        public List<FM70Learner> Learners { get; set; }
    }
}

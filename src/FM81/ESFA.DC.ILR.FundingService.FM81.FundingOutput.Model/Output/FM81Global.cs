using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM81.FundingOutput.Model.Output
{
    public class FM81Global
    {
        public int UKPRN { get; set; }

        public string CurFundYr { get; set; }

        public string LARSVersion { get; set; }

        public string RulebaseVersion { get; set; }

        public List<FM81Learner> Learners { get; set; }
    }
}

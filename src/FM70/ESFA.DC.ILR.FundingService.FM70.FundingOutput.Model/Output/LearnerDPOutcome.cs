using System;

namespace ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output
{
    public class LearnerDPOutcome
    {
        public int OutCode { get; set; }

        public string OutType { get; set; }

        public DateTime OutStartDate { get; set; }

        public DateTime? OutcomeDateForProgression { get; set; }

        public bool? PotentialESFProgressionType { get; set; }

        public string ProgressionType { get; set; }

        public bool? ReachedSixMonthPoint { get; set; }

        public bool? ReachedThreeMonthPoint { get; set; }

        public bool? ReachedTwelveMonthPoint { get; set; }
    }
}

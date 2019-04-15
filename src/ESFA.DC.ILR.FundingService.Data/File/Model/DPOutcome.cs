using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.FundingService.Data.File.Model
{
    public class DPOutcome
    {
        public int OutCode { get; set; }

        public string OutType { get; set; }

        public DateTime OutCollDate { get; set; }

        public DateTime OutStartDate { get; set; }

        public DateTime? OutEndDate { get; set; }
    }
}

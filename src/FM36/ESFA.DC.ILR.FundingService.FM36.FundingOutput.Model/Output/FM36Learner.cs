using System.Collections.Generic;

namespace ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output
{
    public class FM36Learner
    {
        public string LearnRefNumber { get; set; }

        public long ULN { get; set; }

        public List<PriceEpisode> PriceEpisodes { get; set; }

        public List<LearningDelivery> LearningDeliveries { get; set; }

        public List<HistoricEarningOutputValues> HistoricEarningOutputValues { get; set; }
    }
}

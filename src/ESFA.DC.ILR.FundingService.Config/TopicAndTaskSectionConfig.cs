using ESFA.DC.ILR.FundingService.Config.Interfaces;

namespace ESFA.DC.ILR.FundingService.Config
{
    public class TopicAndTaskSectionConfig : ITopicAndTaskSectionConfig
    {
        public string TopicFunding_TaskPerformALBCalculation { get; set; }

        public string TopicFunding_TaskPerformFM25Calculation { get; set; }

        public string TopicFunding_TaskPerformFM35Calculation { get; set; }

        public string TopicFunding_TaskPerformFM36Calculation { get; set; }

        public string TopicFunding_TaskPerformFM70Calculation { get; set; }

        public string TopicFunding_TaskPerformFM81Calculation { get; set; }
    }
}

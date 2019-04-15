namespace ESFA.DC.ILR.FundingService.Config.Interfaces
{
    public interface ITopicAndTaskSectionConfig
    {
        string TopicFunding_TaskPerformALBCalculation { get; set; }

        string TopicFunding_TaskPerformFM25Calculation { get; set; }

        string TopicFunding_TaskPerformFM35Calculation { get; set; }

        string TopicFunding_TaskPerformFM36Calculation { get; set; }

        string TopicFunding_TaskPerformFM70Calculation { get; set; }

        string TopicFunding_TaskPerformFM81Calculation { get; set; }
    }
}

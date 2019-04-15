using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IAppsEarningsHistoryDataRetrievalService
    {
        IEnumerable<LearnRefNumberULNKey> UniqueFM36Learners(IMessage message);

        IDictionary<long, IEnumerable<AECEarningsHistory>> AppsEarningsHistoryForLearners(int providerUKPRN, IEnumerable<LearnRefNumberULNKey> learners);
    }
}

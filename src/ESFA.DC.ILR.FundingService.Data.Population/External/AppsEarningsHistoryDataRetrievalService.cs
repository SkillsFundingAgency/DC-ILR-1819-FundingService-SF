using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.AppsEarningsHistory.Model;
using ESFA.DC.Data.AppsEarningsHistory.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class AppsEarningsHistoryDataRetrievalService : IAppsEarningsHistoryDataRetrievalService
    {
        private const int _fundModel = 36;
        private readonly IApprenticeshipsEarningsHistory _appsEarningsHistory;

        public AppsEarningsHistoryDataRetrievalService()
        {
        }

        public AppsEarningsHistoryDataRetrievalService(IApprenticeshipsEarningsHistory appsEarningsHistory)
        {
            _appsEarningsHistory = appsEarningsHistory;
        }

        public virtual IQueryable<AppsEarningsHistory> AecLatestInYearHistory => _appsEarningsHistory.AppsEarningsHistories;

        public IEnumerable<LearnRefNumberULNKey> UniqueFM36Learners(IMessage message)
        {
            return message
                .Learners
                .Where(l => l.LearningDeliveries.Any(ld => ld.FundModel == _fundModel))
                .Select(k => new LearnRefNumberULNKey(k.LearnRefNumber, k.ULN))
                .ToList();
        }

        public IDictionary<long, IEnumerable<AECEarningsHistory>> AppsEarningsHistoryForLearners(int providerUKPRN, IEnumerable<LearnRefNumberULNKey> learners)
        {
            var aecHistories = new List<AECEarningsHistory>();

            var learnerShards = learners.SplitList(2500);

            foreach (var shard in learnerShards)
            {
                var ulns = shard.Select(l => l.ULN).ToList();

                aecHistories.AddRange(
                    AecLatestInYearHistory
                    .Where(a => a.LatestInYear == true
                    && a.ULN < 9999999999
                    && ulns.Contains(a.ULN))
                    .Select(aec => new AECEarningsHistory
                    {
                        AppIdentifier = aec.AppIdentifier,
                        AppProgCompletedInTheYearInput = aec.AppProgCompletedInTheYearInput,
                        CollectionYear = aec.CollectionYear,
                        CollectionReturnCode = aec.CollectionReturnCode,
                        DaysInYear = aec.DaysInYear,
                        FworkCode = aec.FworkCode,
                        HistoricEffectiveTNPStartDateInput = aec.HistoricEffectiveTNPStartDateInput,
                        HistoricEmpIdEndWithinYear = aec.HistoricEmpIdEndWithinYear,
                        HistoricEmpIdStartWithinYear = aec.HistoricEmpIdStartWithinYear,
                        HistoricLearner1618StartInput = aec.HistoricLearner1618StartInput,
                        HistoricPMRAmount = aec.HistoricPMRAmount,
                        HistoricTNP1Input = aec.HistoricTNP1Input,
                        HistoricTNP2Input = aec.HistoricTNP2Input,
                        HistoricTNP3Input = aec.HistoricTNP3Input,
                        HistoricTNP4Input = aec.HistoricTNP4Input,
                        HistoricTotal1618UpliftPaymentsInTheYearInput = aec.HistoricTotal1618UpliftPaymentsInTheYearInput,
                        HistoricVirtualTNP3EndOfTheYearInput = aec.HistoricVirtualTNP3EndOfTheYearInput,
                        HistoricVirtualTNP4EndOfTheYearInput = aec.HistoricVirtualTNP4EndOfTheYearInput,
                        HistoricLearnDelProgEarliestACT2DateInput = aec.HistoricLearnDelProgEarliestACT2DateInput,
                        LatestInYear = aec.LatestInYear,
                        LearnRefNumber = aec.LearnRefNumber,
                        ProgrammeStartDateIgnorePathway = aec.ProgrammeStartDateIgnorePathway,
                        ProgrammeStartDateMatchPathway = aec.ProgrammeStartDateMatchPathway,
                        ProgType = aec.ProgType,
                        PwayCode = aec.PwayCode,
                        STDCode = aec.STDCode,
                        TotalProgAimPaymentsInTheYear = aec.TotalProgAimPaymentsInTheYear,
                        UptoEndDate = aec.UptoEndDate,
                        UKPRN = aec.UKPRN,
                        ULN = aec.ULN
                    }).ToList());
            }

            return
                aecHistories
                .GroupBy(a => a.ULN)
                .ToDictionary(k => k.Key, v => v.ToList() as IEnumerable<AECEarningsHistory>);
        }
    }
}

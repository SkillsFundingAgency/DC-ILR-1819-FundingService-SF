using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.AppsEarningsHistory.Model;
using ESFA.DC.Data.AppsEarningsHistory.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class AppsEarningsHistoryDataRetrievalServiceTests
    {
        [Fact]
        public void UniqueFM36Learners()
        {
            var message = new TestMessage
            {
                Learners = new List<TestLearner>
                {
                    new TestLearner
                    {
                        LearnRefNumber = "35and36",
                        ULN = 1,
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                FundModel = 35
                            },
                            new TestLearningDelivery
                            {
                                FundModel = 36
                            }
                        }
                    },
                    new TestLearner
                    {
                        LearnRefNumber = "36",
                        ULN = 2,
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                FundModel = 36
                            }
                        }
                    },
                    new TestLearner
                    {
                        LearnRefNumber = "35",
                        ULN = 3,
                        LearningDeliveries = new List<TestLearningDelivery>
                        {
                            new TestLearningDelivery
                            {
                                FundModel = 35
                            }
                        }
                    }
                }
            };

            var learners = NewService().UniqueFM36Learners(message);

            learners.Should().HaveCount(2);
            learners.Select(l => l.LearnRefNumber).Should().Contain("35and36", "36");
            learners.Select(l => l.LearnRefNumber).Should().NotContain("35");
            learners.Where(l => l.LearnRefNumber == "35and36").Select(u => u.ULN).Should().BeEquivalentTo(1);
            learners.Where(l => l.LearnRefNumber == "36").Select(u => u.ULN).Should().BeEquivalentTo(2);
        }

        [Fact]
        public void AppEarningsHistory()
        {
            var appsHistoryMock = new Mock<IApprenticeshipsEarningsHistory>();

            var appsHistory = NewService(appsHistoryMock.Object).AecLatestInYearHistory;

            appsHistoryMock.VerifyGet(a => a.AppsEarningsHistories);
        }

        [Fact]
        public void AppsEarningsHistoryForLearners()
        {
            var apps_Histories = new List<AppsEarningsHistory>()
            {
                new AppsEarningsHistory()
                {
                    LearnRefNumber = "123",
                    ULN = 1234567890,
                    UKPRN = 123456,
                    LatestInYear = true,
                    HistoricTNP1Input = 1
                },
                new AppsEarningsHistory()
                {
                    LearnRefNumber = "123",
                    ULN = 1234567890,
                    UKPRN = 123456,
                    LatestInYear = true,
                    HistoricTNP1Input = 2
                },
                new AppsEarningsHistory()
                {
                    LearnRefNumber = "123",
                    ULN = 12345678,
                    UKPRN = 123456,
                    LatestInYear = true,
                    HistoricTNP1Input = 2
                },
                new AppsEarningsHistory()
                {
                    LearnRefNumber = "456",
                    ULN = 1234567899,
                    UKPRN = 123456,
                    LatestInYear = true,
                    HistoricTNP1Input = 1
                },
                new AppsEarningsHistory()
                {
                    LearnRefNumber = "456",
                    ULN = 1234567890,
                    UKPRN = 1234,
                    LatestInYear = true,
                    HistoricTNP1Input = 1
                },
                new AppsEarningsHistory()
                {
                    LearnRefNumber = "456",
                    ULN = 1234567890,
                    UKPRN = 123456,
                    LatestInYear = false,
                    HistoricTNP1Input = 1
                },
                new AppsEarningsHistory(),
            }.AsQueryable();

            var appsEarningHistoryDataRetrievalServiceMock = NewMock();

            appsEarningHistoryDataRetrievalServiceMock.SetupGet(a => a.AecLatestInYearHistory).Returns(apps_Histories);

            var ukprn = 123456;
            var learners = new List<LearnRefNumberULNKey> { new LearnRefNumberULNKey("123", 1234567890), new LearnRefNumberULNKey("456", 1234567899) };

            var aecHistories = appsEarningHistoryDataRetrievalServiceMock.Object.AppsEarningsHistoryForLearners(ukprn, learners);

            aecHistories.Should().HaveCount(2);
            aecHistories.Should().ContainKeys(1234567890, 1234567899);
            aecHistories.SelectMany(v => v.Value).Should().HaveCount(4);
            aecHistories[1234567890].Should().HaveCount(3);
            aecHistories[1234567899].Should().HaveCount(1);
        }

        [Fact]
        public void AECLatestInYearEarnings_EntityOutputCorrect()
        {
            var ukprn = 123456;
            var learners = new List<LearnRefNumberULNKey> { new LearnRefNumberULNKey("123", 1234567890) };

            var aec_LatestInYearHistory = new AppsEarningsHistory()
            {
                AppIdentifier = "1",
                AppProgCompletedInTheYearInput = true,
                CollectionYear = "2",
                CollectionReturnCode = "3",
                DaysInYear = 4,
                FworkCode = 5,
                HistoricEffectiveTNPStartDateInput = new DateTime(2018, 8, 1),
                HistoricEmpIdEndWithinYear = 6,
                HistoricEmpIdStartWithinYear = 7,
                HistoricLearner1618StartInput = true,
                HistoricPMRAmount = 8m,
                HistoricTNP1Input = 9m,
                HistoricTNP2Input = 10m,
                HistoricTNP3Input = 11m,
                HistoricTNP4Input = 12m,
                HistoricTotal1618UpliftPaymentsInTheYearInput = 13m,
                HistoricVirtualTNP3EndOfTheYearInput = 14m,
                HistoricVirtualTNP4EndOfTheYearInput = 15m,
                HistoricLearnDelProgEarliestACT2DateInput = new DateTime(2018, 8, 1),
                LatestInYear = true,
                LearnRefNumber = "16",
                ProgrammeStartDateIgnorePathway = new DateTime(2018, 8, 1),
                ProgrammeStartDateMatchPathway = new DateTime(2018, 8, 1),
                ProgType = 17,
                PwayCode = 18,
                STDCode = 19,
                TotalProgAimPaymentsInTheYear = 20m,
                UptoEndDate = new DateTime(2018, 8, 1),
                UKPRN = 123456,
                ULN = 1234567890,
            };

            var apps_Histories = new List<AppsEarningsHistory>()
            {
               aec_LatestInYearHistory
            }.AsQueryable();

            var appsEarningHistoryDataRetrievalServiceMock = NewMock();

            appsEarningHistoryDataRetrievalServiceMock.SetupGet(a => a.AecLatestInYearHistory).Returns(apps_Histories);

            var aecHistories = appsEarningHistoryDataRetrievalServiceMock.Object.AppsEarningsHistoryForLearners(ukprn, learners);

            var aecLatestInYearHistory = aecHistories[1234567890].FirstOrDefault();

            aecLatestInYearHistory.AppIdentifier.Should().Be(aec_LatestInYearHistory.AppIdentifier);
            aecLatestInYearHistory.AppProgCompletedInTheYearInput.Should().Be(aec_LatestInYearHistory.AppProgCompletedInTheYearInput);
            aecLatestInYearHistory.CollectionYear.Should().Be(aec_LatestInYearHistory.CollectionYear);
            aecLatestInYearHistory.CollectionReturnCode.Should().Be(aec_LatestInYearHistory.CollectionReturnCode);
            aecLatestInYearHistory.DaysInYear.Should().Be(aec_LatestInYearHistory.DaysInYear);
            aecLatestInYearHistory.FworkCode.Should().Be(aec_LatestInYearHistory.FworkCode);
            aecLatestInYearHistory.HistoricEffectiveTNPStartDateInput.Should().Be(aec_LatestInYearHistory.HistoricEffectiveTNPStartDateInput);
            aecLatestInYearHistory.HistoricEmpIdEndWithinYear.Should().Be(aec_LatestInYearHistory.HistoricEmpIdEndWithinYear);
            aecLatestInYearHistory.HistoricEmpIdStartWithinYear.Should().Be(aec_LatestInYearHistory.HistoricEmpIdStartWithinYear);
            aecLatestInYearHistory.HistoricLearner1618StartInput.Should().Be(aec_LatestInYearHistory.HistoricLearner1618StartInput);
            aecLatestInYearHistory.HistoricPMRAmount.Should().Be(aec_LatestInYearHistory.HistoricPMRAmount);
            aecLatestInYearHistory.HistoricTNP1Input.Should().Be(aec_LatestInYearHistory.HistoricTNP1Input);
            aecLatestInYearHistory.HistoricTNP2Input.Should().Be(aec_LatestInYearHistory.HistoricTNP2Input);
            aecLatestInYearHistory.HistoricTNP3Input.Should().Be(aec_LatestInYearHistory.HistoricTNP3Input);
            aecLatestInYearHistory.HistoricTNP4Input.Should().Be(aec_LatestInYearHistory.HistoricTNP4Input);
            aecLatestInYearHistory.HistoricTotal1618UpliftPaymentsInTheYearInput.Should().Be(aec_LatestInYearHistory.HistoricTotal1618UpliftPaymentsInTheYearInput);
            aecLatestInYearHistory.HistoricVirtualTNP3EndOfTheYearInput.Should().Be(aec_LatestInYearHistory.HistoricVirtualTNP3EndOfTheYearInput);
            aecLatestInYearHistory.HistoricVirtualTNP4EndOfTheYearInput.Should().Be(aec_LatestInYearHistory.HistoricVirtualTNP4EndOfTheYearInput);
            aecLatestInYearHistory.HistoricLearnDelProgEarliestACT2DateInput.Should().Be(aec_LatestInYearHistory.HistoricLearnDelProgEarliestACT2DateInput);
            aecLatestInYearHistory.LatestInYear.Should().Be(aec_LatestInYearHistory.LatestInYear);
            aecLatestInYearHistory.LearnRefNumber.Should().Be(aec_LatestInYearHistory.LearnRefNumber);
            aecLatestInYearHistory.ProgrammeStartDateIgnorePathway.Should().Be(aec_LatestInYearHistory.ProgrammeStartDateIgnorePathway);
            aecLatestInYearHistory.ProgrammeStartDateMatchPathway.Should().Be(aec_LatestInYearHistory.ProgrammeStartDateMatchPathway);
            aecLatestInYearHistory.ProgType.Should().Be(aec_LatestInYearHistory.ProgType);
            aecLatestInYearHistory.PwayCode.Should().Be(aec_LatestInYearHistory.PwayCode);
            aecLatestInYearHistory.STDCode.Should().Be(aec_LatestInYearHistory.STDCode);
            aecLatestInYearHistory.TotalProgAimPaymentsInTheYear.Should().Be(aec_LatestInYearHistory.TotalProgAimPaymentsInTheYear);
            aecLatestInYearHistory.UptoEndDate.Should().Be(aec_LatestInYearHistory.UptoEndDate);
            aecLatestInYearHistory.UKPRN.Should().Be(aec_LatestInYearHistory.UKPRN);
            aecLatestInYearHistory.ULN.Should().Be(aec_LatestInYearHistory.ULN);
        }

        private AppsEarningsHistoryDataRetrievalService NewService(IApprenticeshipsEarningsHistory appsHistory = null)
        {
            return new AppsEarningsHistoryDataRetrievalService(appsHistory);
        }

        private Mock<AppsEarningsHistoryDataRetrievalService> NewMock()
        {
            return new Mock<AppsEarningsHistoryDataRetrievalService>();
        }
    }
}

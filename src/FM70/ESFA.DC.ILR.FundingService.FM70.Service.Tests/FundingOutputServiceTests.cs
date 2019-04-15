using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM70.Service;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using FluentAssertions;
using Moq;
using Oracle.Determinations.Masquerade.Util;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM70.Service.Tests
{
    public class FundingOutputServiceTests
    {
        [Fact]
        public void GlobalFromDataEntity()
        {
            var dataEntity = new DataEntity(string.Empty);

            var ukprn = 1;
            var rulebaseVersion = "RulebaseVersion";

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "UKPRN")).Returns(ukprn);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "RulebaseVersion")).Returns(rulebaseVersion);

            var global = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).MapGlobal(dataEntity);

            global.UKPRN.Should().Be(ukprn);
            global.RulebaseVersion.Should().Be(rulebaseVersion);
        }

        [Fact]
        public void LearnerFromDataEntity()
        {
            var learnRefNumber = "LearnRefNumber";

            var dataEntity = new DataEntity(string.Empty)
            {
                EntityName = learnRefNumber,
            };

            var learner = NewService(dataEntityAttributeService: new Mock<IDataEntityAttributeService>().Object).MapLearner(dataEntity);

            learner.LearnRefNumber = learnRefNumber;
        }

        [Fact]
        public void LearningDeliveryFromDataEntity()
        {
            var achieved = true;
            var addProgCostElig = true;
            var adjustedAreaCostFactor = 1.0m;
            var adjustedPremiumFactor = 2.0m;
            var adjustedStartDate = new DateTime(2018, 8, 1);
            var aimClassification = "AimClassification";
            var aimValue = 3.0m;
            var applicWeightFundRate = 4.0m;
            var eligibleProgressionOutcomeCode = 10000;
            var eligibleProgressionOutcomeType = "EligibleProgressionOutcomeType";
            var eligibleProgressionOutomeStartDate = new DateTime(2018, 8, 1);
            var fundStart = true;
            var lARSWeightedRate = 5.0m;
            var latestPossibleStartDate = new DateTime(2018, 8, 1);
            var lDESFEngagementStartDate = new DateTime(2018, 8, 1);
            var learnDelLearnerEmpAtStart = true;
            var potentiallyEligibleForProgression = true;
            var progressionEndDate = new DateTime(2018, 8, 1);
            var restart = true;
            var weightedRateFromESOL = 6.0m;

            var dataEntity = new DataEntity(string.Empty);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Achieved")).Returns(achieved);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AddProgCostElig")).Returns(addProgCostElig);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AdjustedAreaCostFactor")).Returns(adjustedAreaCostFactor);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AdjustedPremiumFactor")).Returns(adjustedPremiumFactor);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AdjustedStartDate")).Returns(adjustedStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "AimClassification")).Returns(aimClassification);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AimValue")).Returns(aimValue);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "ApplicWeightFundRate")).Returns(applicWeightFundRate);
            dataEntityAttributeServiceMock.Setup(s => s.GetLongAttributeValue(dataEntity, "EligibleProgressionOutcomeCode")).Returns(eligibleProgressionOutcomeCode);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "EligibleProgressionOutcomeType")).Returns(eligibleProgressionOutcomeType);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "EligibleProgressionOutomeStartDate")).Returns(eligibleProgressionOutomeStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "FundStart")).Returns(fundStart);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LARSWeightedRate")).Returns(lARSWeightedRate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LatestPossibleStartDate")).Returns(latestPossibleStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LDESFEngagementStartDate")).Returns(lDESFEngagementStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "LearnDelLearnerEmpAtStart")).Returns(learnDelLearnerEmpAtStart);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "PotentiallyEligibleForProgression")).Returns(potentiallyEligibleForProgression);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "ProgressionEndDate")).Returns(progressionEndDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Restart")).Returns(restart);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "WeightedRateFromESOL")).Returns(weightedRateFromESOL);

            var learningDelivery = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).LearningDeliveryValue(dataEntity);

            learningDelivery.Achieved.Should().Be(achieved);

            learningDelivery.AddProgCostElig.Should().Be(addProgCostElig);
            learningDelivery.AdjustedAreaCostFactor.Should().Be(adjustedAreaCostFactor);
            learningDelivery.AdjustedPremiumFactor.Should().Be(adjustedPremiumFactor);
            learningDelivery.AdjustedStartDate.Should().Be(adjustedStartDate);
            learningDelivery.AimClassification.Should().Be(aimClassification);
            learningDelivery.AimValue.Should().Be(aimValue);
            learningDelivery.ApplicWeightFundRate.Should().Be(applicWeightFundRate);
            learningDelivery.EligibleProgressionOutcomeCode.Should().Be(eligibleProgressionOutcomeCode);
            learningDelivery.EligibleProgressionOutcomeType.Should().Be(eligibleProgressionOutcomeType);
            learningDelivery.EligibleProgressionOutomeStartDate.Should().Be(eligibleProgressionOutomeStartDate);
            learningDelivery.FundStart.Should().Be(fundStart);
            learningDelivery.LARSWeightedRate.Should().Be(lARSWeightedRate);
            learningDelivery.LatestPossibleStartDate.Should().Be(latestPossibleStartDate);
            learningDelivery.LDESFEngagementStartDate.Should().Be(lDESFEngagementStartDate);
            learningDelivery.LearnDelLearnerEmpAtStart.Should().Be(learnDelLearnerEmpAtStart);
            learningDelivery.PotentiallyEligibleForProgression.Should().Be(potentiallyEligibleForProgression);
            learningDelivery.ProgressionEndDate.Should().Be(progressionEndDate);
            learningDelivery.Restart.Should().Be(restart);
            learningDelivery.WeightedRateFromESOL.Should().Be(weightedRateFromESOL);
        }

        [Fact]
        public void LearningDeliveryDeliverablesFromDataEntity()
        {
            var deliverableCode = "Code";
            var deliverableUnitCost = 1.0m;
            var achievementEarnings = 1.0m;
            var additionalProgCostEarnings = 1.0m;
            var deliverableVolume = 1.0m;
            var progressionEarnings = 1.0m;
            var reportingVolume = 1.0m;
            var startEarnings = 1.0m;

            var dataEntity = TestLearningDeliveryDeliverableEntity(new DataEntity(string.Empty)).FirstOrDefault();

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "DeliverableCode")).Returns(deliverableCode);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "DeliverableUnitCost")).Returns(deliverableUnitCost);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AchievementEarnings")).Returns(achievementEarnings);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AdditionalProgCostEarnings")).Returns(additionalProgCostEarnings);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "DeliverableVolume")).Returns(deliverableVolume);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "ProgressionEarnings")).Returns(progressionEarnings);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "ReportingVolume")).Returns(reportingVolume);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "StartEarnings")).Returns(startEarnings);

            var learningDeliveryDeliverable = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).LearningDeliveryDeliverablesFromDataEntity(dataEntity);

            learningDeliveryDeliverable.DeliverableCode.Should().Be(deliverableCode);
            learningDeliveryDeliverable.DeliverableUnitCost.Should().Be(deliverableUnitCost);
        }

        [Fact]
        public void LearningDeliveryDeliverablePeriodisedValuesFromEntity_Correct()
        {
            // ARRANGE
            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(It.IsAny<object>())).Returns(1.0m);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValueForPeriod(It.IsAny<IAttributeData>(), It.IsAny<DateTime>())).Returns(1.0m);

            var fundingOutputService = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object);

            // ACT
            var learningDeliveryDeliverablePeriodisedValues =
                fundingOutputService.LearningDeliveryDeliverablePeriodisedValuesFromEntity(TestLearningDeliveryDeliverableEntity(null).Single());

            // ASSERT
            TestLearningDeliveryDeliverablePeriodisedValuesArray().Should().BeEquivalentTo(learningDeliveryDeliverablePeriodisedValues);
        }

        [Fact]
        public void LearningDeliveryDeliverablePeriodisedValues_WithChangePoints_Correct()
        {
            var internalDataCacheMock = new Mock<IInternalDataCache>();

            internalDataCacheMock.Setup(p => p.Period1).Returns(new DateTime(2018, 8, 1));
            internalDataCacheMock.Setup(p => p.Period2).Returns(new DateTime(2018, 9, 1));
            internalDataCacheMock.Setup(p => p.Period3).Returns(new DateTime(2018, 10, 1));
            internalDataCacheMock.Setup(p => p.Period4).Returns(new DateTime(2018, 11, 1));
            internalDataCacheMock.Setup(p => p.Period5).Returns(new DateTime(2018, 12, 1));
            internalDataCacheMock.Setup(p => p.Period6).Returns(new DateTime(2019, 1, 1));
            internalDataCacheMock.Setup(p => p.Period7).Returns(new DateTime(2019, 2, 1));
            internalDataCacheMock.Setup(p => p.Period8).Returns(new DateTime(2019, 3, 1));
            internalDataCacheMock.Setup(p => p.Period9).Returns(new DateTime(2019, 4, 1));
            internalDataCacheMock.Setup(p => p.Period10).Returns(new DateTime(2019, 5, 1));
            internalDataCacheMock.Setup(p => p.Period11).Returns(new DateTime(2019, 6, 1));
            internalDataCacheMock.Setup(p => p.Period12).Returns(new DateTime(2019, 7, 1));

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(It.IsAny<object>())).Returns(1.0m);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValueForPeriod(It.IsAny<IAttributeData>(), It.IsAny<DateTime>())).Returns(1.0m);

            var learningDeliveryDeliverablePeriodisedValues =
                NewService(internalDataCacheMock.Object, dataEntityAttributeServiceMock.Object).LearningDeliveryDeliverablePeriodisedValuesFromEntity(TestLearningDeliveryEntityWithChangePoints(null).Single());

            // ASSERT
            TestLearningDeliveryDeliverablePeriodisedValuesArray().Should().BeEquivalentTo(learningDeliveryDeliverablePeriodisedValues);
        }

        private IEnumerable<IDataEntity> TestLearningDeliveryDeliverableEntity(DataEntity parent)
        {
            var entities = new List<DataEntity>();

            var entity = new DataEntity("LearningDeliveryDeliverable")
            {
                EntityName = "LearningDeliveryDeliverable",
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "AchievementEarnings", Attribute(false, "1.0") },
                    { "AdditionalProgCostEarnings", Attribute(false, "1.0") },
                    { "DeliverableVolume", Attribute(false, "1.0") },
                    { "DeliverableCode", Attribute(false, "1.0") },
                    { "DeliverableUnitCost", Attribute(false, "1.0") },
                    { "ProgressionEarnings", Attribute(false, "1.0") },
                    { "ReportingVolume", Attribute(false, "1.0") },
                    { "StartEarnings", Attribute(false, "1.0") },
                },
                Parent = parent,
            };

            entities.Add(entity);

            return entities;
        }

        private IEnumerable<IDataEntity> TestLearningDeliveryEntityWithChangePoints(DataEntity parent)
        {
            var entities = new List<DataEntity>();

            var entity = new DataEntity("LearningDelivery")
            {
                EntityName = "LearningDelivery",
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "AimSeqNumber", Attribute(false, "1.0") },
                    { "AchApplicDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "Achieved", Attribute(false, "1.0") },
                    { "AchieveElement", Attribute(false, "1.0") },
                    { "AchievementEarnings", Attribute(true, "1.0") },
                    { "AchievePayPctPreTrans", Attribute(false, "1.0") },
                    { "AchPayTransHeldBack", Attribute(false, "1.0") },
                    { "ActualDaysIL", Attribute(false, "1.0") },
                    { "ActualNumInstalm", Attribute(false, "1.0") },
                    { "ActualNumInstalmPreTrans", Attribute(false, "1.0") },
                    { "AdditionalProgCostEarnings", Attribute(false, "1.0") },
                    { "AdjLearnStartDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "AdltLearnResp", Attribute(false, "1.0") },
                    { "AgeAimStart", Attribute(false, "1.0") },
                    { "AimValue", Attribute(false, "1.0") },
                    { "AppAdjLearnStartDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "AppAgeFact", Attribute(false, "1.0") },
                    { "AppATAGTA", Attribute(false, "1.0") },
                    { "AppCompetency", Attribute(false, "1.0") },
                    { "AppFuncSkill", Attribute(false, "1.0") },
                    { "AppFuncSkill1618AdjFact", Attribute(false, "1.0") },
                    { "AppKnowl", Attribute(false, "1.0") },
                    { "AppLearnStartDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "ApplicEmpFactDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "ApplicFactDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "ApplicFundRateDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "ApplicProgWeightFact", Attribute(false, "1.0") },
                    { "ApplicUnweightFundRate", Attribute(false, "1.0") },
                    { "ApplicWeightFundRate", Attribute(false, "1.0") },
                    { "AppNonFund", Attribute(false, "1.0") },
                    { "AreaCostFactAdj", Attribute(true, "1.0") },
                    { "BalInstalmPreTrans", Attribute(false, "1.0") },
                    { "BaseValueUnweight", Attribute(false, "1.0") },
                    { "CapFactor", Attribute(false, "1.0") },
                    { "DeliverableVolume", Attribute(false, "1.0") },
                    { "EmpOutcomePayElig", Attribute(false, "1.0") },
                    { "EmpOutcomePctHeldBackTrans", Attribute(false, "1.0") },
                    { "EmpOutcomePctPreTrans", Attribute(false, "1.0") },
                    { "EmpRespOth", Attribute(false, "1.0") },
                    { "ESOL", Attribute(false, "1.0") },
                    { "FullyFund", Attribute(false, "1.0") },
                    { "FundLine", Attribute(false, "1.0") },
                    { "FundStart", Attribute(false, "1.0") },
                    { "LargeEmployerFM70Fctr", Attribute(false, "1.0") },
                    { "LargeEmployerID", Attribute(false, "1.0") },
                    { "LargeEmployerStatusDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "LTRCUpliftFctr", Attribute(false, "1.0") },
                    { "NonGovCont", Attribute(false, "1.0") },
                    { "OLASSCustody", Attribute(false, "1.0") },
                    { "OnProgPayPctPreTrans", Attribute(false, "1.0") },
                    { "OutstndNumOnProgInstalm", Attribute(false, "1.0") },
                    { "OutstndNumOnProgInstalmTrans", Attribute(false, "1.0") },
                    { "PlannedNumOnProgInstalm", Attribute(false, "1.0") },
                    { "PlannedNumOnProgInstalmTrans", Attribute(false, "1.0") },
                    { "PlannedTotalDaysIL", Attribute(false, "1.0") },
                    { "ProgressionEarnings", Attribute(false, "1.0") },
                    { "PropFundRemain", Attribute(false, "1.0") },
                    { "PropFundRemainAch", Attribute(false, "1.0") },
                    { "ReportingVolume", Attribute(false, "1.0") },
                    { "Residential", Attribute(false, "1.0") },
                    { "Restart", Attribute(false, "1.0") },
                    { "StartEarnings", Attribute(true, "1.0") },
                    { "StartPropTrans", Attribute(false, "1.0") },
                    { "ThresholdDays", Attribute(false, "1.0") },
                    { "Traineeship", Attribute(false, "1.0") },
                    { "Trans", Attribute(false, "1.0") },
                    { "TrnAdjLearnStartDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "TrnWorkPlaceAim", Attribute(false, "1.0") },
                    { "TrnWorkPrepAim", Attribute(false, "1.0") },
                    { "UnWeightedRateFromESOL", Attribute(false, "1.0") },
                    { "UnweightedRateFromLARS", Attribute(false, "1.0") },
                    { "WeightedRateFromESOL", Attribute(false, "1.0") },
                    { "WeightedRateFromLARS", Attribute(false, "1.0") },
                    { "AchievePayment", Attribute(false, "1.0") },
                    { "AchievePayPct", Attribute(false, "1.0") },
                    { "AchievePayPctTrans", Attribute(false, "1.0") },
                    { "BalancePayment", Attribute(false, "1.0") },
                    { "BalancePaymentUncapped", Attribute(false, "1.0") },
                    { "BalancePct", Attribute(false, "1.0") },
                    { "BalancePctTrans", Attribute(false, "1.0") },
                    { "EmpOutcomePay", Attribute(false, "1.0") },
                    { "EmpOutcomePct", Attribute(false, "1.0") },
                    { "EmpOutcomePctTrans", Attribute(false, "1.0") },
                    { "InstPerPeriod", Attribute(false, "1.0") },
                    { "LearnSuppFund", Attribute(false, "1.0") },
                    { "LearnSuppFundCash", Attribute(false, "1.0") },
                    { "OnProgPayment", Attribute(false, "1.0") },
                    { "OnProgPaymentUncapped", Attribute(false, "1.0") },
                    { "OnProgPayPct", Attribute(false, "1.0") },
                    { "OnProgPayPctTrans", Attribute(false, "1.0") },
                    { "TransInstPerPeriod", Attribute(false, "1.0") },
                },
                Parent = parent,
            };

            entities.Add(entity);

            return entities;
        }

        private IAttributeData Attribute(bool hasChangePoints, object attributeValue)
        {
            if (hasChangePoints)
            {
                var attribute = new AttributeData(null);
                attribute.AddChangepoints(ChangePoints(decimal.Parse(attributeValue.ToString())));

                return attribute;
            }

            return new AttributeData(attributeValue);
        }

        private IEnumerable<ITemporalValueItem> ChangePoints(decimal value)
        {
            var changePoints = new List<TemporalValueItem>();

            IEnumerable<TemporalValueItem> cps = new List<TemporalValueItem>
            {
                 new TemporalValueItem(new DateTime(2018, 08, 01), value, null),
                 new TemporalValueItem(new DateTime(2018, 09, 01), value, null),
                 new TemporalValueItem(new DateTime(2018, 10, 01), value, null),
                 new TemporalValueItem(new DateTime(2018, 11, 01), value, null),
                 new TemporalValueItem(new DateTime(2018, 12, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 01, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 02, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 03, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 04, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 05, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 06, 01), value, null),
                 new TemporalValueItem(new DateTime(2019, 07, 01), value, null),
            };

            changePoints.AddRange(cps);

            return changePoints;
        }

        private List<LearningDeliveryDeliverablePeriodisedValue> TestLearningDeliveryDeliverablePeriodisedValuesArray()
        {
            return new List<LearningDeliveryDeliverablePeriodisedValue>
            {
                TestLearningDeliveryPeriodisedAttributesData("AchievementEarnings", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("AdditionalProgCostEarnings", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("DeliverableVolume", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("ProgressionEarnings", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("ReportingVolume", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("StartEarnings", 1.0m),
            };
        }

        private LearningDeliveryDeliverablePeriodisedValue TestLearningDeliveryPeriodisedAttributesData(string attribute, decimal value)
        {
            return new LearningDeliveryDeliverablePeriodisedValue
            {
                AttributeName = attribute,
                Period1 = value,
                Period2 = value,
                Period3 = value,
                Period4 = value,
                Period5 = value,
                Period6 = value,
                Period7 = value,
                Period8 = value,
                Period9 = value,
                Period10 = value,
                Period11 = value,
                Period12 = value,
            };
        }

        private FundingOutputService NewService(IInternalDataCache internalDataCache = null, IDataEntityAttributeService dataEntityAttributeService = null)
        {
            return new FundingOutputService(internalDataCache, dataEntityAttributeService);
        }
    }
}

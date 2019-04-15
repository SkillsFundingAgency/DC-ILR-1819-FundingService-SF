using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using FluentAssertions;
using Moq;
using Oracle.Determinations.Masquerade.Util;
using Xunit;

namespace ESFA.DC.ILR.FundingService.ALB.Service.Tests
{
    public class FundingOutputServiceTests
    {
        [Fact]
        public void GlobalFromDataEntity()
        {
            var dataEntity = new DataEntity(string.Empty);

            var ukprn = 1;
            var larsVersion = "LARSVersion";
            var postcodeAreaCostVersion = "PostcodeAreaCostVersion";
            var rulebaseVersion = "RulebaseVersion";

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "UKPRN")).Returns(ukprn);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LARSVersion")).Returns(larsVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "PostcodeAreaCostVersion")).Returns(postcodeAreaCostVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "RulebaseVersion")).Returns(rulebaseVersion);

            var global = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).MapGlobal(dataEntity);

            global.UKPRN.Should().Be(ukprn);
            global.LARSVersion.Should().Be(larsVersion);
            global.PostcodeAreaCostVersion.Should().Be(postcodeAreaCostVersion);
            global.RulebaseVersion.Should().Be(rulebaseVersion);
        }

        [Fact]
        public void LearnerFromDataEntity()
        {
            var learnRefNumber = "LearnRefNumber";
            var albSeqNum = "1.0";

            var dataEntity = new DataEntity(string.Empty)
            {
                EntityName = learnRefNumber,
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "ALBSeqNum", Attribute(false, albSeqNum) }
                }
            };

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LearnRefNumber")).Returns(learnRefNumber);

            var learner = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).MapLearner(dataEntity);

            learner.LearnRefNumber = learnRefNumber;
        }

        [Fact]
        public void LearningDeliveryFromDataEntity()
        {
            var aimSeqNumber = 1;
            var albSeqNum = "1.0";

            var dataEntity = new DataEntity(string.Empty)
            {
                EntityName = "LearningDelivery",
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "ALBSeqNum", Attribute(false, albSeqNum) },
                    { "ALBCode", Attribute(false, "1.0") },
                    { "ALBSupportPayment", Attribute(false, "1.0") },
                    { "AreaUpliftBalPayment", Attribute(false, "1.0") },
                    { "AreaUpliftOnProgPayment", Attribute(false, "1.0") },
                    { "LearnDelCarLearnPilotOnProgPayment", Attribute(false, "1.0") },
                    { "LearnDelCarLearnPilotBalPayment", Attribute(false, "1.0") }
                }
            };

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "AimSeqNumber")).Returns(aimSeqNumber);

            var learningDelivery = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).LearningDeliveryFromDataEntity(dataEntity);

            learningDelivery.AimSeqNumber.Should().Be(aimSeqNumber);
        }

        [Fact]
        public void FundingOutput_LearnerPeriodisedAttributeData_Correct()
        {
            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(It.IsAny<object>())).Returns(1.0m);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValueForPeriod(It.IsAny<IAttributeData>(), It.IsAny<DateTime>())).Returns(1.0m);

            var learnerPeriodisedAttributeData =
                NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).LearnerPeriodisedValues(TestLearnerEntity(null).Single());

            var expectedLearnerPeriodisedAttributeData = TestLearnerPeriodisedAttributesArray();

            expectedLearnerPeriodisedAttributeData.Should().BeEquivalentTo(learnerPeriodisedAttributeData);
        }

        [Fact]
        public void FundingOutput_LearnerPeriodisedAttributeData_ChangePoints_Correct()
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

            var learnerPeriodisedAttributeData =
                 NewService(internalDataCacheMock.Object, dataEntityAttributeServiceMock.Object).LearnerPeriodisedValues(TestLearnerEntityWithChangePoints(null).Single());

            var expectedLearnerPeriodisedAttributeData = TestLearnerPeriodisedAttributesArray();

            expectedLearnerPeriodisedAttributeData.Should().BeEquivalentTo(learnerPeriodisedAttributeData);
        }

        [Fact]
        public void LearningDeliveryDataFromDataEntity()
        {
            var achieved = false;
            var actualNumInstalm = 1;
            var advLoan = false;
            var applicDate = new DateTime(2018, 09, 01);
            var applicProgWeightFact = "1.0";
            var albCode = "1.0";
            var albSupportPayment = "1.0";
            var areaUpliftBalPayment = "1.0";
            var areaUpliftOnProgPayment = "1.0";
            var areaCostFactAdj = 1.0m;
            var areaCostInstalment = 1.0m;
            var fundLine = "1.0";
            var fundStart = false;

            var learnDelApplicLARSCarPilFundSubRate = 1.0m;
            var learnDelApplicSubsidyPilotAreaCode = "1.0";
            var learnDelCarLearnPilotAimValue = 1.0m;
            var learnDelCarLearnPilotInstalAmount = 1.0m;
            var learnDelCarLearnPilotOnProgPayment = "1.0m";
            var learnDelCarLearnPilotBalPayment = "1.0m";
            var learnDelEligCareerLearnPilot = false;

            var liabilityDate = new DateTime(2018, 09, 01);
            var loanBursAreaUplift = false;
            var loanBursSupp = false;
            var outstndNumOnProgInstalm = 1;
            var plannedNumOnProgInstalm = 1;
            var weightedRate = 1.0m;

            var dataEntity = new DataEntity(string.Empty);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Achieved")).Returns(achieved);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualNumInstalm")).Returns(actualNumInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AdvLoan")).Returns(advLoan);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "ApplicFactDate")).Returns(applicDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "ApplicProgWeightFact")).Returns(applicProgWeightFact);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "ALBCode")).Returns(albCode);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "ALBSupportPayment")).Returns(albSupportPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "AreaUpliftBalPayment")).Returns(areaUpliftBalPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "AreaUpliftOnProgPayment")).Returns(areaUpliftOnProgPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AreaCostFactAdj")).Returns(areaCostFactAdj);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AreaCostInstalment")).Returns(areaCostInstalment);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "FundLine")).Returns(fundLine);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "FundStart")).Returns(fundStart);

            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LearnDelApplicLARSCarPilFundSubRate")).Returns(learnDelApplicLARSCarPilFundSubRate);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LearnDelApplicSubsidyPilotAreaCode")).Returns(learnDelApplicSubsidyPilotAreaCode);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LearnDelCarLearnPilotAimValue")).Returns(learnDelCarLearnPilotAimValue);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LearnDelCarLearnPilotInstalAmount")).Returns(learnDelCarLearnPilotInstalAmount);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LearnDelCarLearnPilotOnProgPayment")).Returns(learnDelCarLearnPilotOnProgPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LearnDelCarLearnPilotBalPayment")).Returns(learnDelCarLearnPilotBalPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "LearnDelEligCareerLearnPilot")).Returns(learnDelEligCareerLearnPilot);

            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LiabilityDate")).Returns(liabilityDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "LoanBursAreaUplift")).Returns(loanBursAreaUplift);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "LoanBursSupp")).Returns(loanBursSupp);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "OutstndNumOnProgInstalm")).Returns(outstndNumOnProgInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedNumOnProgInstalm")).Returns(plannedNumOnProgInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "WeightedRate")).Returns(weightedRate);

            var learningDelivery = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).LearningDeliveryValue(dataEntity);

            learningDelivery.Achieved.Should().Be(achieved);

            learningDelivery.Achieved.Should().Be(achieved);
            learningDelivery.ActualNumInstalm.Should().Be(actualNumInstalm);
            learningDelivery.AdvLoan.Should().Be(advLoan);
            learningDelivery.ApplicFactDate.Should().Be(applicDate);
            learningDelivery.ApplicProgWeightFact.Should().Be(applicProgWeightFact);
            learningDelivery.AreaCostFactAdj.Should().Be(areaCostFactAdj);
            learningDelivery.AreaCostInstalment.Should().Be(areaCostInstalment);
            learningDelivery.FundLine.Should().Be(fundLine);
            learningDelivery.FundStart.Should().Be(fundStart);
            learningDelivery.LearnDelApplicLARSCarPilFundSubRate.Should().Be(learnDelApplicLARSCarPilFundSubRate);
            learningDelivery.LearnDelApplicSubsidyPilotAreaCode.Should().Be(learnDelApplicSubsidyPilotAreaCode);
            learningDelivery.LearnDelCarLearnPilotAimValue.Should().Be(learnDelCarLearnPilotAimValue);
            learningDelivery.LearnDelCarLearnPilotInstalAmount.Should().Be(learnDelCarLearnPilotInstalAmount);
            learningDelivery.LearnDelEligCareerLearnPilot.Should().Be(learnDelEligCareerLearnPilot);
            learningDelivery.LiabilityDate.Should().Be(liabilityDate);
            learningDelivery.LoanBursAreaUplift.Should().Be(loanBursAreaUplift);
            learningDelivery.LoanBursSupp.Should().Be(loanBursSupp);
            learningDelivery.OutstndNumOnProgInstalm.Should().Be(outstndNumOnProgInstalm);
            learningDelivery.PlannedNumOnProgInstalm.Should().Be(plannedNumOnProgInstalm);
            learningDelivery.WeightedRate.Should().Be(weightedRate);
        }

        [Fact]
        public void FundingOutput_LearningDeliveryPeriodisedAttributeData_Correct()
        {
            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(It.IsAny<object>())).Returns(1.0m);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValueForPeriod(It.IsAny<IAttributeData>(), It.IsAny<DateTime>())).Returns(1.0m);

            var learningDeliveryPeriodisedAttributeData =
                NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).LearningDeliveryPeriodisedValues(TestLearningDeliveryEntity(null).Single());

            var expectedLearningDeliveryPeriodisedAttributeData = TestLearningDeliveryPeriodisedAttributesDataArray();

            expectedLearningDeliveryPeriodisedAttributeData.Should().BeEquivalentTo(learningDeliveryPeriodisedAttributeData);
        }

        [Fact]
        public void FundingOutput_LearningDeliveryPeriodisedAttributeData_WithChangePoints_Correct()
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

            var learningDeliveryPeriodisedAttributeData =
                NewService(internalDataCacheMock.Object, dataEntityAttributeServiceMock.Object).LearningDeliveryPeriodisedValues(TestLearningDeliveryEntityWithChangePoints(null).Single());

            var expectedLearningDeliveryPeriodisedAttributeData = TestLearningDeliveryPeriodisedAttributesDataArray();

            expectedLearningDeliveryPeriodisedAttributeData.Should().BeEquivalentTo(learningDeliveryPeriodisedAttributeData);
        }

        private IEnumerable<IDataEntity> TestLearningDeliveryEntity(DataEntity parent)
        {
            var entities = new List<DataEntity>();

            var entity = new DataEntity("LearningDelivery")
            {
                EntityName = "LearningDelivery",
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "AimSeqNumber", Attribute(false, "1.0") },
                    { "Achieved", Attribute(false, "1.0") },
                    { "ActualNumInstalm", Attribute(false, "1.0") },
                    { "AdvLoan", Attribute(false, "1.0") },
                    { "ApplicFactDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "ApplicProgWeightFact", Attribute(false, "1.0") },
                    { "ALBCode", Attribute(false, "1.0") },
                    { "ALBSupportPayment", Attribute(false, "1.0") },
                    { "AreaUpliftBalPayment", Attribute(false, "1.0") },
                    { "AreaUpliftOnProgPayment", Attribute(false, "1.0") },
                    { "AreaCostFactAdj", Attribute(false, "1.0") },
                    { "AreaCostInstalment", Attribute(false, "1.0") },
                    { "FundLine", Attribute(false, "1.0") },
                    { "FundStart", Attribute(false, "1.0") },
                    { "LearnDelApplicLARSCarPilFundSubRate", Attribute(false, "1.0") },
                    { "LearnDelApplicSubsidyPilotAreaCode", Attribute(false, "1.0") },
                    { "LearnDelCarLearnPilotAimValue", Attribute(false, "1.0") },
                    { "LearnDelCarLearnPilotInstalAmount", Attribute(false, "1.0") },
                    { "LearnDelCarLearnPilotOnProgPayment", Attribute(false, "1.0") },
                    { "LearnDelCarLearnPilotBalPayment", Attribute(false, "1.0") },
                    { "LearnDelEligCareerLearnPilot", Attribute(false, "1.0") },
                    { "LiabilityDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "LoanBursAreaUplift", Attribute(false, "1.0") },
                    { "LoanBursSupp", Attribute(false, "1.0") },
                    { "OutstndNumOnProgInstalm", Attribute(false, "1.0") },
                    { "PlannedNumOnProgInstalm", Attribute(false, "1.0") },
                    { "WeightedRate", Attribute(false, "1.0") }
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
                    { "Achieved", Attribute(false, "1.0") },
                    { "ActualNumInstalm", Attribute(false, "1.0") },
                    { "AdvLoan", Attribute(false, "1.0") },
                    { "ApplicFactDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "ApplicProgWeightFact", Attribute(false, "1.0") },
                    { "ALBCode", Attribute(false, "1.0") },
                    { "ALBSupportPayment", Attribute(true, "1.0") },
                    { "AreaUpliftBalPayment", Attribute(false, "1.0") },
                    { "AreaUpliftOnProgPayment", Attribute(true, "1.0") },
                    { "AreaCostFactAdj", Attribute(false, "1.0") },
                    { "AreaCostInstalment", Attribute(false, "1.0") },
                    { "FundLine", Attribute(false, "1.0") },
                    { "FundStart", Attribute(false, "1.0") },
                    { "LearnDelApplicLARSCarPilFundSubRate", Attribute(false, "1.0") },
                    { "LearnDelApplicSubsidyPilotAreaCode", Attribute(false, "1.0") },
                    { "LearnDelCarLearnPilotAimValue", Attribute(false, "1.0") },
                    { "LearnDelCarLearnPilotInstalAmount", Attribute(false, "1.0") },
                    { "LearnDelCarLearnPilotOnProgPayment", Attribute(false, "1.0") },
                    { "LearnDelCarLearnPilotBalPayment", Attribute(false, "1.0") },
                    { "LearnDelEligCareerLearnPilot", Attribute(false, "1.0") },
                    { "LiabilityDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "LoanBursAreaUplift", Attribute(false, "1.0") },
                    { "LoanBursSupp", Attribute(false, "1.0") },
                    { "OutstndNumOnProgInstalm", Attribute(false, "1.0") },
                    { "PlannedNumOnProgInstalm", Attribute(false, "1.0") },
                    { "WeightedRate", Attribute(false, "1.0") }
                },

                Parent = parent,
            };

            entities.Add(entity);

            return entities;
        }

        private IEnumerable<IDataEntity> TestLearnerEntity(DataEntity parent)
        {
            var entities = new List<DataEntity>();

            var entity = new DataEntity("Learner")
            {
                EntityName = "Learner",
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "LearnRefNumber", Attribute(false, "LearnRefNumber") },
                    { "ALBSeqNum", Attribute(false, "1.0") },
                },

                Parent = parent,
            };

            entities.Add(entity);

            return entities;
        }

        private IEnumerable<IDataEntity> TestLearnerEntityWithChangePoints(DataEntity parent)
        {
            var entities = new List<DataEntity>();

            var entity = new DataEntity("Learner")
            {
                EntityName = "Learner",
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "LearnRefNumber", Attribute(false, "LearnRefNumber") },
                    { "ALBSeqNum", Attribute(true, "1.0") },
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

        private List<LearnerPeriodisedValue> TestLearnerPeriodisedAttributesArray()
        {
            return new List<LearnerPeriodisedValue>
            {
                TestLearnerPeriodisedAttributes("ALBSeqNum", 1.0m)
            };
        }

        private LearnerPeriodisedValue TestLearnerPeriodisedAttributes(string attribute, decimal value)
        {
            return new LearnerPeriodisedValue
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

        private List<LearningDeliveryPeriodisedValue> TestLearningDeliveryPeriodisedAttributesDataArray()
        {
            return new List<LearningDeliveryPeriodisedValue>
            {
                TestLearningDeliveryPeriodisedAttributesData("ALBCode", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("ALBSupportPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("AreaUpliftBalPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("AreaUpliftOnProgPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnDelCarLearnPilotOnProgPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnDelCarLearnPilotBalPayment", 1.0m),
            };
        }

        private LearningDeliveryPeriodisedValue TestLearningDeliveryPeriodisedAttributesData(string attribute, decimal value)
        {
            return new LearningDeliveryPeriodisedValue
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

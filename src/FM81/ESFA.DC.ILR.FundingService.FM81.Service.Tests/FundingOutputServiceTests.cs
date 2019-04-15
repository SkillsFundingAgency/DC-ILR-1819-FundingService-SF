using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM81.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM81.Service;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using FluentAssertions;
using Moq;
using Oracle.Determinations.Masquerade.Util;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM81.Service.Tests
{
    public class FundingOutputServiceTests
    {
        [Fact]
        public void GlobalFromDataEntity()
        {
            var dataEntity = new DataEntity(string.Empty);

            var ukprn = 1;
            var curFundYr = "CurFundYr";
            var larsVersion = "LARSVersion";
            var rulebaseVersion = "RulebaseVersion";

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "UKPRN")).Returns(ukprn);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "CurFundYr")).Returns(curFundYr);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LARSVersion")).Returns(larsVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "RulebaseVersion")).Returns(rulebaseVersion);

            var global = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).MapGlobal(dataEntity);

            global.UKPRN.Should().Be(ukprn);
            global.CurFundYr.Should().Be(curFundYr);
            global.LARSVersion.Should().Be(larsVersion);
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
            var achApplicDate = new DateTime(2018, 09, 01);
            var achEligible = true;
            var achieved = true;
            var achievementApplicVal = 1.0m;
            var achPayment = 1.0m;
            var actualDaysIL = 1;
            var actualNumInstalm = 1;
            var adjProgStartDate = new DateTime(2018, 09, 01);
            var ageStandardStart = 1;
            var applicFundValDate = new DateTime(2018, 09, 01);
            var combinedAdjProp = 1.0m;
            var coreGovContCapApplicVal = 1;
            var coreGovContPayment = 1.0m;
            var coreGovContUncapped = 1.0m;
            var empIdAchDate = 1;
            var empIdFirstDayStandard = 1;
            var empIdFirstYoungAppDate = 1;
            var empIdSecondYoungAppDate = 1;
            var empIdSmallBusDate = 1;
            var fundLine = "FundLine";
            var instPerPeriod = 1;
            var learnDelDaysIL = 1;
            var learnDelStandardAccDaysIL = 1;
            var learnDelStandardPrevAccDaysIL = 1;
            var learnDelStandardTotalDaysIL = 1;
            var learnSuppFund = true;
            var learnSuppFundCash = 1.0m;
            var mathEngAimValue = 1.0m;
            var mathEngBalPayment = 1.0m;
            var mathEngBalPct = 1;
            var mathEngLSFFundStart = true;
            var mathEngLSFThresholdDays = 1;
            var mathEngOnProgPayment = 1.0m;
            var mathEngOnProgPct = 1;
            var outstandNumOnProgInstalm = 1;
            var plannedNumOnProgInstalm = 1;
            var plannedTotalDaysIL = 1;
            var progStandardStartDate = new DateTime(2018, 09, 01);
            var smallBusApplicVal = 1.0m;
            var smallBusEligible = true;
            var smallBusPayment = 1.0m;
            var smallBusStatusFirstDayStandard = 1;
            var smallBusStatusThreshold = 1;
            var youngAppApplicVal = 1.0m;
            var youngAppEligible = true;
            var youngAppFirstPayment = 1.0m;
            var youngAppFirstThresholdDate = new DateTime(2018, 09, 01);
            var youngAppPayment = 1.0m;
            var youngAppSecondPayment = 1.0m;
            var youngAppSecondThresholdDate = new DateTime(2018, 09, 01);

            var dataEntity = new DataEntity(string.Empty);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AchApplicDate")).Returns(achApplicDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AchEligible")).Returns(achEligible);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Achieved")).Returns(achieved);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AchievementApplicVal")).Returns(achievementApplicVal);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AchPayment")).Returns(achPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualDaysIL")).Returns(actualDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualNumInstalm")).Returns(actualNumInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AdjProgStartDate")).Returns(adjProgStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "AgeStandardStart")).Returns(ageStandardStart);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "ApplicFundValDate")).Returns(applicFundValDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "CombinedAdjProp")).Returns(combinedAdjProp);
            dataEntityAttributeServiceMock.Setup(s => s.GetLongAttributeValue(dataEntity, "CoreGovContCapApplicVal")).Returns(coreGovContCapApplicVal);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "CoreGovContPayment")).Returns(coreGovContPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "CoreGovContUncapped")).Returns(coreGovContUncapped);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "EmpIdAchDate")).Returns(empIdAchDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "EmpIdFirstDayStandard")).Returns(empIdFirstDayStandard);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "EmpIdFirstYoungAppDate")).Returns(empIdFirstYoungAppDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "EmpIdSecondYoungAppDate")).Returns(empIdSecondYoungAppDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "EmpIdSmallBusDate")).Returns(empIdSmallBusDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "FundLine")).Returns(fundLine);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "InstPerPeriod")).Returns(instPerPeriod);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelDaysIL")).Returns(learnDelDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelStandardAccDaysIL")).Returns(learnDelStandardAccDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelStandardPrevAccDaysIL")).Returns(learnDelStandardPrevAccDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelStandardTotalDaysIL")).Returns(learnDelStandardTotalDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "LearnSuppFund")).Returns(learnSuppFund);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LearnSuppFundCash")).Returns(learnSuppFundCash);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "MathEngAimValue")).Returns(mathEngAimValue);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "MathEngBalPayment")).Returns(mathEngBalPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetLongAttributeValue(dataEntity, "MathEngBalPct")).Returns(mathEngBalPct);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "MathEngLSFFundStart")).Returns(mathEngLSFFundStart);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "MathEngLSFThresholdDays")).Returns(mathEngLSFThresholdDays);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "MathEngOnProgPayment")).Returns(mathEngOnProgPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "MathEngOnProgPct")).Returns(mathEngOnProgPct);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "OutstandNumOnProgInstalm")).Returns(outstandNumOnProgInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedNumOnProgInstalm")).Returns(plannedNumOnProgInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedTotalDaysIL")).Returns(plannedTotalDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "ProgStandardStartDate")).Returns(progStandardStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "SmallBusApplicVal")).Returns(smallBusApplicVal);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "SmallBusEligible")).Returns(smallBusEligible);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "SmallBusPayment")).Returns(smallBusPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "SmallBusStatusFirstDayStandard")).Returns(smallBusStatusFirstDayStandard);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "SmallBusStatusThreshold")).Returns(smallBusStatusThreshold);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "YoungAppApplicVal")).Returns(youngAppApplicVal);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "YoungAppEligible")).Returns(youngAppEligible);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "YoungAppFirstPayment")).Returns(youngAppFirstPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "YoungAppFirstThresholdDate")).Returns(youngAppFirstThresholdDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "YoungAppPayment")).Returns(youngAppPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "YoungAppSecondPayment")).Returns(youngAppSecondPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "YoungAppSecondThresholdDate")).Returns(youngAppSecondThresholdDate);

            var learningDelivery = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).LearningDeliveryAttributeData(dataEntity);

            learningDelivery.AchApplicDate.Should().Be(achApplicDate);
            learningDelivery.AchApplicDate.Should().Be(achApplicDate);
            learningDelivery.AchEligible.Should().Be(achEligible);
            learningDelivery.Achieved.Should().Be(achieved);
            learningDelivery.AchievementApplicVal.Should().Be(achievementApplicVal);
            learningDelivery.AchPayment.Should().Be(achPayment);
            learningDelivery.ActualDaysIL.Should().Be(actualDaysIL);
            learningDelivery.ActualNumInstalm.Should().Be(actualNumInstalm);
            learningDelivery.AdjProgStartDate.Should().Be(adjProgStartDate);
            learningDelivery.AgeStandardStart.Should().Be(ageStandardStart);
            learningDelivery.ApplicFundValDate.Should().Be(applicFundValDate);
            learningDelivery.CombinedAdjProp.Should().Be(combinedAdjProp);
            learningDelivery.CoreGovContCapApplicVal.Should().Be(coreGovContCapApplicVal);
            learningDelivery.CoreGovContPayment.Should().Be(coreGovContPayment);
            learningDelivery.CoreGovContUncapped.Should().Be(coreGovContUncapped);
            learningDelivery.EmpIdAchDate.Should().Be(empIdAchDate);
            learningDelivery.EmpIdFirstDayStandard.Should().Be(empIdFirstDayStandard);
            learningDelivery.EmpIdFirstYoungAppDate.Should().Be(empIdFirstYoungAppDate);
            learningDelivery.EmpIdSecondYoungAppDate.Should().Be(empIdSecondYoungAppDate);
            learningDelivery.EmpIdSmallBusDate.Should().Be(empIdSmallBusDate);
            learningDelivery.FundLine.Should().Be(fundLine);
            learningDelivery.InstPerPeriod.Should().Be(instPerPeriod);
            learningDelivery.LearnDelDaysIL.Should().Be(learnDelDaysIL);
            learningDelivery.LearnDelStandardAccDaysIL.Should().Be(learnDelStandardAccDaysIL);
            learningDelivery.LearnDelStandardPrevAccDaysIL.Should().Be(learnDelStandardPrevAccDaysIL);
            learningDelivery.LearnDelStandardTotalDaysIL.Should().Be(learnDelStandardTotalDaysIL);
            learningDelivery.LearnSuppFund.Should().Be(learnSuppFund);
            learningDelivery.LearnSuppFundCash.Should().Be(learnSuppFundCash);
            learningDelivery.MathEngAimValue.Should().Be(mathEngAimValue);
            learningDelivery.MathEngBalPayment.Should().Be(mathEngBalPayment);
            learningDelivery.MathEngBalPct.Should().Be(mathEngBalPct);
            learningDelivery.MathEngLSFFundStart.Should().Be(mathEngLSFFundStart);
            learningDelivery.MathEngLSFThresholdDays.Should().Be(mathEngLSFThresholdDays);
            learningDelivery.MathEngOnProgPayment.Should().Be(mathEngOnProgPayment);
            learningDelivery.MathEngOnProgPct.Should().Be(mathEngOnProgPct);
            learningDelivery.OutstandNumOnProgInstalm.Should().Be(outstandNumOnProgInstalm);
            learningDelivery.PlannedNumOnProgInstalm.Should().Be(plannedNumOnProgInstalm);
            learningDelivery.PlannedTotalDaysIL.Should().Be(plannedTotalDaysIL);
            learningDelivery.ProgStandardStartDate.Should().Be(progStandardStartDate);
            learningDelivery.SmallBusApplicVal.Should().Be(smallBusApplicVal);
            learningDelivery.SmallBusEligible.Should().Be(smallBusEligible);
            learningDelivery.SmallBusPayment.Should().Be(smallBusPayment);
            learningDelivery.SmallBusStatusFirstDayStandard.Should().Be(smallBusStatusFirstDayStandard);
            learningDelivery.SmallBusStatusThreshold.Should().Be(smallBusStatusThreshold);
            learningDelivery.YoungAppApplicVal.Should().Be(youngAppApplicVal);
            learningDelivery.YoungAppEligible.Should().Be(youngAppEligible);
            learningDelivery.YoungAppFirstPayment.Should().Be(youngAppFirstPayment);
            learningDelivery.YoungAppFirstThresholdDate.Should().Be(youngAppFirstThresholdDate);
            learningDelivery.YoungAppPayment.Should().Be(youngAppPayment);
            learningDelivery.YoungAppSecondPayment.Should().Be(youngAppSecondPayment);
            learningDelivery.YoungAppSecondThresholdDate.Should().Be(youngAppSecondThresholdDate);
        }

        [Fact]
        public void FundingOutput_LearningDeliveryPeriodisedAttributeData_Correct()
        {
            // ARRANGE
            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(It.IsAny<object>())).Returns(1.0m);

            var fundingOutputService = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object);

            // ACT
            var learningDeliveryPeriodisedAttributeData =
                fundingOutputService.LearningDeliveryPeriodisedValues(TestLearningDeliveryEntity(null).Single());

            // ASSERT
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

            // ASSERT
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
                    { "AchDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "AimSeqNumber", Attribute(false, "1.0") },
                    { "AimType", Attribute(false, "1.0") },
                    { "CompStatus", Attribute(false, "1.0") },
                    { "FrameworkCommonComponent", Attribute(false, "1.0") },
                    { "LearnAimRef", Attribute(false, "1.0") },
                    { "LearnActEndDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "LearnPlanEndDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "LearnStartDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "LrnDelFAM_EEF", Attribute(false, "1.0") },
                    { "LrnDelFAM_FFI", Attribute(false, "1.0") },
                    { "LrnDelFAM_LDM1", Attribute(false, "1.0") },
                    { "LrnDelFAM_LDM2", Attribute(false, "1.0") },
                    { "LrnDelFAM_LDM3", Attribute(false, "1.0") },
                    { "LrnDelFAM_LDM4", Attribute(false, "1.0") },
                    { "LrnDelFAM_RES", Attribute(false, "1.0") },
                    { "LrnDelFAM_SOF", Attribute(false, "1.0") },
                    { "LrnDelFAM_SPP", Attribute(false, "1.0") },
                    { "OrigLearnStartDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "OtherFundAdj", Attribute(false, "1.0") },
                    { "Outcome", Attribute(false, "1.0") },
                    { "PriorLearnFundAdj", Attribute(false, "1.0") },
                    { "ProgType", Attribute(false, "1.0") },
                    { "STDCode", Attribute(false, "1.0") },
                    { "WithdrawReason", Attribute(false, "1.0") },
                    { "AchApplicDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "AchEligible", Attribute(false, "1.0") },
                    { "Achieved", Attribute(false, "1.0") },
                    { "AchievementApplicVal", Attribute(false, "1.0") },
                    { "AchPayment", Attribute(false, "1.0") },
                    { "ActualDaysIL", Attribute(false, "1.0") },
                    { "ActualNumInstalm", Attribute(false, "1.0") },
                    { "AdjProgStartDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "AgeStandardStart", Attribute(false, "1.0") },
                    { "ApplicFundValDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "CombinedAdjProp", Attribute(false, "1.0") },
                    { "CoreGovContCapApplicVal", Attribute(false, "1.0") },
                    { "CoreGovContPayment", Attribute(false, "1.0") },
                    { "CoreGovContUncapped", Attribute(false, "1.0") },
                    { "EmpIdAchDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "EmpIdFirstDayStandard", Attribute(false, "1.0") },
                    { "EmpIdFirstYoungAppDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "EmpIdSecondYoungAppDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "EmpIdSmallBusDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "FundLine", Attribute(false, "1.0") },
                    { "InstPerPeriod", Attribute(false, "1.0") },
                    { "LearnDelDaysIL", Attribute(false, "1.0") },
                    { "LearnDelStandardAccDaysIL", Attribute(false, "1.0") },
                    { "LearnDelStandardPrevAccDaysIL", Attribute(false, "1.0") },
                    { "LearnDelStandardTotalDaysIL", Attribute(false, "1.0") },
                    { "LearnSuppFund", Attribute(false, "1.0") },
                    { "LearnSuppFundCash", Attribute(false, "1.0") },
                    { "MathEngAimValue", Attribute(false, "1.0") },
                    { "MathEngBalPayment", Attribute(false, "1.0") },
                    { "MathEngBalPct", Attribute(false, "1.0") },
                    { "MathEngLSFFundStart", Attribute(false, "1.0") },
                    { "MathEngLSFThresholdDays", Attribute(false, "1.0") },
                    { "MathEngOnProgPayment", Attribute(false, "1.0") },
                    { "MathEngOnProgPct", Attribute(false, "1.0") },
                    { "OutstandNumOnProgInstalm", Attribute(false, "1.0") },
                    { "PlannedNumOnProgInstalm", Attribute(false, "1.0") },
                    { "PlannedTotalDaysIL", Attribute(false, "1.0") },
                    { "ProgStandardStartDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "SmallBusApplicVal", Attribute(false, "1.0") },
                    { "SmallBusEligible", Attribute(false, "1.0") },
                    { "SmallBusPayment", Attribute(false, "1.0") },
                    { "SmallBusStatusFirstDayStandard", Attribute(false, "1.0") },
                    { "SmallBusStatusThreshold", Attribute(false, "1.0") },
                    { "YoungAppEligible", Attribute(false, "1.0") },
                    { "YoungAppFirstPayment", Attribute(false, "1.0") },
                    { "YoungAppFirstThresholdDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "YoungAppPayment", Attribute(false, "1.0") },
                    { "YoungAppSecondPayment", Attribute(false, "1.0") },
                    { "YoungAppSecondThresholdDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
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
                    { "AchDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "AimSeqNumber", Attribute(false, "1.0") },
                    { "AimType", Attribute(false, "1.0") },
                    { "CompStatus", Attribute(false, "1.0") },
                    { "FrameworkCommonComponent", Attribute(false, "1.0") },
                    { "LearnAimRef", Attribute(false, "1.0") },
                    { "LearnActEndDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "LearnPlanEndDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "LearnStartDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "LrnDelFAM_EEF", Attribute(false, "1.0") },
                    { "LrnDelFAM_FFI", Attribute(false, "1.0") },
                    { "LrnDelFAM_LDM1", Attribute(false, "1.0") },
                    { "LrnDelFAM_LDM2", Attribute(false, "1.0") },
                    { "LrnDelFAM_LDM3", Attribute(false, "1.0") },
                    { "LrnDelFAM_LDM4", Attribute(false, "1.0") },
                    { "LrnDelFAM_RES", Attribute(false, "1.0") },
                    { "LrnDelFAM_SOF", Attribute(false, "1.0") },
                    { "LrnDelFAM_SPP", Attribute(false, "1.0") },
                    { "OrigLearnStartDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "OtherFundAdj", Attribute(false, "1.0") },
                    { "Outcome", Attribute(false, "1.0") },
                    { "PriorLearnFundAdj", Attribute(false, "1.0") },
                    { "ProgType", Attribute(false, "1.0") },
                    { "STDCode", Attribute(false, "1.0") },
                    { "WithdrawReason", Attribute(false, "1.0") },
                    { "AchApplicDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "AchEligible", Attribute(false, "1.0") },
                    { "Achieved", Attribute(false, "1.0") },
                    { "AchievementApplicVal", Attribute(false, "1.0") },
                    { "AchPayment", Attribute(false, "1.0") },
                    { "ActualDaysIL", Attribute(false, "1.0") },
                    { "ActualNumInstalm", Attribute(false, "1.0") },
                    { "AdjProgStartDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "AgeStandardStart", Attribute(false, "1.0") },
                    { "ApplicFundValDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "CombinedAdjProp", Attribute(false, "1.0") },
                    { "CoreGovContCapApplicVal", Attribute(false, "1.0") },
                    { "CoreGovContPayment", Attribute(false, "1.0") },
                    { "CoreGovContUncapped", Attribute(false, "1.0") },
                    { "EmpIdAchDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "EmpIdFirstDayStandard", Attribute(false, "1.0") },
                    { "EmpIdFirstYoungAppDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "EmpIdSecondYoungAppDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "EmpIdSmallBusDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "FundLine", Attribute(false, "1.0") },
                    { "InstPerPeriod", Attribute(false, "1.0") },
                    { "LearnDelDaysIL", Attribute(false, "1.0") },
                    { "LearnDelStandardAccDaysIL", Attribute(false, "1.0") },
                    { "LearnDelStandardPrevAccDaysIL", Attribute(false, "1.0") },
                    { "LearnDelStandardTotalDaysIL", Attribute(false, "1.0") },
                    { "LearnSuppFund", Attribute(false, "1.0") },
                    { "LearnSuppFundCash", Attribute(false, "1.0") },
                    { "MathEngAimValue", Attribute(false, "1.0") },
                    { "MathEngBalPayment", Attribute(false, "1.0") },
                    { "MathEngBalPct", Attribute(false, "1.0") },
                    { "MathEngLSFFundStart", Attribute(false, "1.0") },
                    { "MathEngLSFThresholdDays", Attribute(false, "1.0") },
                    { "MathEngOnProgPayment", Attribute(false, "1.0") },
                    { "MathEngOnProgPct", Attribute(false, "1.0") },
                    { "OutstandNumOnProgInstalm", Attribute(false, "1.0") },
                    { "PlannedNumOnProgInstalm", Attribute(false, "1.0") },
                    { "PlannedTotalDaysIL", Attribute(false, "1.0") },
                    { "ProgStandardStartDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "SmallBusApplicVal", Attribute(false, "1.0") },
                    { "SmallBusEligible", Attribute(false, "1.0") },
                    { "SmallBusPayment", Attribute(false, "1.0") },
                    { "SmallBusStatusFirstDayStandard", Attribute(false, "1.0") },
                    { "SmallBusStatusThreshold", Attribute(false, "1.0") },
                    { "YoungAppEligible", Attribute(false, "1.0") },
                    { "YoungAppFirstPayment", Attribute(true, "1.0") },
                    { "YoungAppFirstThresholdDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "YoungAppPayment", Attribute(true, "1.0") },
                    { "YoungAppSecondPayment", Attribute(true, "1.0") },
                    { "YoungAppSecondThresholdDate", Attribute(true, new Date(new DateTime(2018, 09, 01))) },
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
                decimal attrValue;
                decimal.TryParse(attributeValue.ToString(), out attrValue);

                attribute.AddChangepoints(ChangePoints(attrValue));

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

        private List<LearningDeliveryPeriodisedValue> TestLearningDeliveryPeriodisedAttributesDataArray()
        {
            return new List<LearningDeliveryPeriodisedValue>
            {
                TestLearningDeliveryPeriodisedAttributesData("AchPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("CoreGovContPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("CoreGovContUncapped", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("InstPerPeriod", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnSuppFund", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnSuppFundCash", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("MathEngBalPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("MathEngBalPct", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("MathEngOnProgPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("MathEngOnProgPct", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("SmallBusPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("YoungAppFirstPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("YoungAppPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("YoungAppSecondPayment", 1.0m),
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

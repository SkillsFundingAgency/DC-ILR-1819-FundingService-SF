using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using FluentAssertions;
using Moq;
using Oracle.Determinations.Masquerade.Util;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM36.Service.Tests
{
    public class FundingOutputServiceTests
    {
        [Fact]
        public void GlobalFromDataEntity()
        {
            var dataEntity = new DataEntity(string.Empty);

            var ukprn = 1;
            var larsVersion = "LARSVersion";
            var year = "1819";
            var rulebaseVersion = "RulebaseVersion";

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "UKPRN")).Returns(ukprn);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LARSVersion")).Returns(larsVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "Year")).Returns(year);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "RulebaseVersion")).Returns(rulebaseVersion);

            var global = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).MapGlobal(dataEntity);

            global.UKPRN.Should().Be(ukprn);
            global.LARSVersion.Should().Be(larsVersion);
            global.Year.Should().Be(year);
            global.RulebaseVersion.Should().Be(rulebaseVersion);
        }

        [Fact]
        public void LearnerFromDataEntity()
        {
            var learnRefNumber = "LearnRefNumber";
            var uln = 1234567890;

            var dataEntity = new DataEntity(string.Empty)
            {
                EntityName = learnRefNumber
            };

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LearnRefNumber")).Returns(learnRefNumber);
            dataEntityAttributeServiceMock.Setup(s => s.GetLongAttributeValue(dataEntity, "ULN")).Returns(uln);

            var learner = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).MapLearner(dataEntity);

            learner.LearnRefNumber = learnRefNumber;
            learner.ULN = uln;
        }

        [Fact]
        public void LearningDeliveryFromDataEntity()
        {
            var aimSeqNumber = 1;
            var learnStartDate = new DateTime(2018, 08, 01);

            var dataEntity = new DataEntity(string.Empty)
            {
                EntityName = "LearningDelivery",
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "DisadvFirstPayment", Attribute(false, "1.0") },
                    { "DisadvSecondPayment", Attribute(false, "1.0") },
                    { "FundLineType", Attribute(false, "1.0") },
                    { "InstPerPeriod", Attribute(false, "1.0") },
                    { "LDApplic1618FrameworkUpliftBalancingPayment", Attribute(false, "1.0") },
                    { "LDApplic1618FrameworkUpliftCompletionPayment", Attribute(false, "1.0") },
                    { "LDApplic1618FrameworkUpliftOnProgPayment", Attribute(false, "1.0") },
                    { "LearnDelContType", Attribute(false, "1.0") },
                    { "LearnDelFirstEmp1618Pay", Attribute(false, "1.0") },
                    { "LearnDelFirstProv1618Pay", Attribute(false, "1.0") },
                    { "LearnDelLevyNonPayInd", Attribute(false, "1.0") },
                    { "LearnDelSecondEmp1618Pay", Attribute(false, "1.0") },
                    { "LearnDelSecondProv1618Pay", Attribute(false, "1.0") },
                    { "LearnDelSEMContWaiver", Attribute(false, "1.0") },
                    { "LearnDelSFAContribPct", Attribute(false, "1.0") },
                    { "LearnSuppFund", Attribute(false, "1.0") },
                    { "LearnSuppFundCash", Attribute(false, "1.0") },
                    { "MathEngBalPayment", Attribute(false, "1.0") },
                    { "MathEngBalPct", Attribute(false, "1.0") },
                    { "MathEngOnProgPayment", Attribute(false, "1.0") },
                    { "MathEngOnProgPct", Attribute(false, "1.0") },
                    { "ProgrammeAimBalPayment", Attribute(false, "1.0") },
                    { "ProgrammeAimCompletionPayment", Attribute(false, "1.0") },
                    { "ProgrammeAimOnProgPayment", Attribute(false, "1.0") },
                    { "ProgrammeAimProgFundIndMaxEmpCont", Attribute(false, "1.0") },
                    { "ProgrammeAimProgFundIndMinCoInvest", Attribute(false, "1.0") },
                    { "ProgrammeAimTotProgFund", Attribute(false, "1.0") },
                    { "LearnDelLearnAddPayment", Attribute(false, "1.0") }
                }
            };

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "AimSeqNumber")).Returns(aimSeqNumber);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LearnStartDate")).Returns(learnStartDate);

            var learningDelivery = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).LearningDeliveryFromDataEntity(dataEntity);

            learningDelivery.AimSeqNumber.Should().Be(aimSeqNumber);
        }

        [Fact]
        public void LearningDeliveryDataFromDataEntity()
        {
           var actualDaysIL = 1;
           var actualNumInstalm = 2;
           var adjStartDate = new DateTime(2018, 8, 1);
           var ageAtProgStart = 3;
           var aimSeqNumber = 1;
           var aimType = 1;
           var appAdjLearnStartDate = new DateTime(2018, 8, 1);
           var appAdjLearnStartDateMatchPathway = new DateTime(2018, 8, 1);
           var applicCompDate = new DateTime(2018, 8, 1);
           var combinedAdjProp = 1.0m;
           var completed = false;
           var compStatus = 1;
           var firstIncentiveThresholdDate = new DateTime(2018, 8, 1);
           var frameworkCommonComponent = 20;
           var fundStart = false;
           var fworkCode = 10;
           var ldApplic1618FrameworkUpliftBalancingValue = 1.0m;
           var ldApplic1618FrameworkUpliftCompElement = 1.0m;
           var ldApplic1618FRameworkUpliftCompletionValue = 1.0m;
           var ldApplic1618FrameworkUpliftMonthInstalVal = 1.0m;
           var ldApplic1618FrameworkUpliftPrevEarnings = 1.0m;
           var ldApplic1618FrameworkUpliftPrevEarningsStage1 = 1.0m;
           var ldApplic1618FrameworkUpliftRemainingAmount = 1.0m;
           var ldApplic1618FrameworkUpliftTotalActEarnings = 1.0m;
           var learnAimRef = "LearnAimRef";
           var learnActEndDate = new DateTime(2018, 8, 1);
           var learnDel1618AtStart = false;
           var learnDelAppAccDaysIL = 1;
           var learnDelApplicDisadvAmount = 1.0m;
           var learnDelApplicEmp1618Incentive = 1.0m;
           var learnDelApplicEmpDate = new DateTime(2018, 8, 1);
           var learnDelApplicProv1618FrameworkUplift = 1.0m;
           var learnDelApplicProv1618Incentive = 1.0m;
           var learnDelAppPrevAccDaysIL = 4;
           var learnDelDaysIL = 5;
           var learnDelDisadAmount = 1.0m;
           var learnDelEligDisadvPayment = false;
           var learnDelEmpIdFirstAdditionalPaymentThreshold = 20;
           var learnDelEmpIdSecondAdditionalPaymentThreshold = 30;
           var learnDelHistDaysThisApp = 6;
           var learnDelHistProgEarnings = 1.0m;
           var learnDelInitialFundLineType = "LearnDelInitialFundLineType";
           var learnDelMathEng = false;
           var learnDelProgEarliestACT2Date = new DateTime(2018, 8, 1);
           var learnDelNonLevyProcured = false;
           var learnDelApplicCareLeaverIncentive = 1.0m;
           var learnDelHistDaysCareLeavers = 7;
           var learnDelAccDaysILCareLeavers = 8;
           var learnDelPrevAccDaysILCareLeavers = 9;
           var learnDelLearnerAddPayThresholdDate = new DateTime(2018, 8, 1);
           var learnDelRedCode = 10;
           var learnDelRedStartDate = new DateTime(2018, 8, 1);
           var learnPlanEndDate = new DateTime(2018, 8, 1);
           var learnStartDate = new DateTime(2018, 8, 1);
           var lrnDelFAM_EEF = "1";
           var lrnDelFAM_LDM1 = "1";
           var lrnDelFAM_LDM2 = "1";
           var lrnDelFAM_LDM3 = "1";
           var lrnDelFAM_LDM4 = "1";
           var mathEngAimValue = 1.0m;
           var outstandNumOnProgInstalm = 11;
           var origLearnStartDate = new DateTime(2018, 8, 1);
           var otherFundAdj = 0;
           var plannedNumOnProgInstalm = 12;
           var plannedTotalDaysIL = 13;
           var priorLearnFundAdj = 0;
           var progType = 14;
           var pwayCode = 15;
           var secondIncentiveThresholdDate = new DateTime(2018, 8, 1);
           var stdCode = 16;
           var thresholdDays = 12;

            var dataEntity = new DataEntity(string.Empty);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualDaysIL")).Returns(actualDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualNumInstalm")).Returns(actualNumInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AdjStartDate")).Returns(adjStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "AgeAtProgStart")).Returns(ageAtProgStart);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "AimSeqNumber")).Returns(aimSeqNumber);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "AimType")).Returns(aimType);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AppAdjLearnStartDate")).Returns(appAdjLearnStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AppAdjLearnStartDateMatchPathway")).Returns(appAdjLearnStartDateMatchPathway);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "ApplicCompDate")).Returns(applicCompDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "CombinedAdjProp")).Returns(combinedAdjProp);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Completed")).Returns(completed);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "CompStatus")).Returns(compStatus);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "FirstIncentiveThresholdDate")).Returns(firstIncentiveThresholdDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "FrameworkCommonComponent")).Returns(frameworkCommonComponent);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "FundStart")).Returns(fundStart);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "FworkCode")).Returns(fworkCode);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LDApplic1618FrameworkUpliftBalancingValue")).Returns(ldApplic1618FrameworkUpliftBalancingValue);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LDApplic1618FrameworkUpliftCompElement")).Returns(ldApplic1618FrameworkUpliftCompElement);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LDApplic1618FRameworkUpliftCompletionValue")).Returns(ldApplic1618FRameworkUpliftCompletionValue);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LDApplic1618FrameworkUpliftMonthInstalVal")).Returns(ldApplic1618FrameworkUpliftMonthInstalVal);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LDApplic1618FrameworkUpliftPrevEarnings")).Returns(ldApplic1618FrameworkUpliftPrevEarnings);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LDApplic1618FrameworkUpliftPrevEarningsStage1")).Returns(ldApplic1618FrameworkUpliftPrevEarningsStage1);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LDApplic1618FrameworkUpliftRemainingAmount")).Returns(ldApplic1618FrameworkUpliftRemainingAmount);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LDApplic1618FrameworkUpliftTotalActEarnings")).Returns(ldApplic1618FrameworkUpliftTotalActEarnings);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LearnAimRef")).Returns(learnAimRef);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LearnActEndDate")).Returns(learnActEndDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "LearnDel1618AtStart")).Returns(learnDel1618AtStart);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelAppAccDaysIL")).Returns(learnDelAppAccDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LearnDelApplicDisadvAmount")).Returns(learnDelApplicDisadvAmount);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LearnDelApplicEmp1618Incentive")).Returns(learnDelApplicEmp1618Incentive);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LearnDelApplicEmpDate")).Returns(learnDelApplicEmpDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LearnDelApplicProv1618FrameworkUplift")).Returns(learnDelApplicProv1618FrameworkUplift);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LearnDelApplicProv1618Incentive")).Returns(learnDelApplicProv1618Incentive);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelAppPrevAccDaysIL")).Returns(learnDelAppPrevAccDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelDaysIL")).Returns(learnDelDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LearnDelDisadAmount")).Returns(learnDelDisadAmount);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "LearnDelEligDisadvPayment")).Returns(learnDelEligDisadvPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelEmpIdFirstAdditionalPaymentThreshold")).Returns(learnDelEmpIdFirstAdditionalPaymentThreshold);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelEmpIdSecondAdditionalPaymentThreshold")).Returns(learnDelEmpIdSecondAdditionalPaymentThreshold);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelHistDaysThisApp")).Returns(learnDelHistDaysThisApp);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LearnDelHistProgEarnings")).Returns(learnDelHistProgEarnings);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LearnDelInitialFundLineType")).Returns(learnDelInitialFundLineType);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "LearnDelMathEng")).Returns(learnDelMathEng);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LearnDelProgEarliestACT2Date")).Returns(learnDelProgEarliestACT2Date);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "LearnDelNonLevyProcured")).Returns(learnDelNonLevyProcured);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LearnDelApplicCareLeaverIncentive")).Returns(learnDelApplicCareLeaverIncentive);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelHistDaysCareLeavers")).Returns(learnDelHistDaysCareLeavers);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelAccDaysILCareLeavers")).Returns(learnDelAccDaysILCareLeavers);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelPrevAccDaysILCareLeavers")).Returns(learnDelPrevAccDaysILCareLeavers);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LearnDelLearnerAddPayThresholdDate")).Returns(learnDelLearnerAddPayThresholdDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LearnDelRedCode")).Returns(learnDelRedCode);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LearnDelRedStartDate")).Returns(learnDelRedStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LearnPlanEndDate")).Returns(learnPlanEndDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LearnStartDate")).Returns(learnStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LrnDelFAM_EEF")).Returns(lrnDelFAM_EEF);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LrnDelFAM_LDM1")).Returns(lrnDelFAM_LDM1);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LrnDelFAM_LDM2")).Returns(lrnDelFAM_LDM2);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LrnDelFAM_LDM3")).Returns(lrnDelFAM_LDM3);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LrnDelFAM_LDM4")).Returns(lrnDelFAM_LDM4);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "MathEngAimValue")).Returns(mathEngAimValue);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "OutstandNumOnProgInstalm")).Returns(outstandNumOnProgInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "OrigLearnStartDate")).Returns(origLearnStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "OtherFundAdj")).Returns(otherFundAdj);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedNumOnProgInstalm")).Returns(plannedNumOnProgInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedTotalDaysIL")).Returns(plannedTotalDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PriorLearnFundAdj")).Returns(priorLearnFundAdj);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ProgType")).Returns(progType);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PwayCode")).Returns(pwayCode);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "SecondIncentiveThresholdDate")).Returns(secondIncentiveThresholdDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "STDCode")).Returns(stdCode);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ThresholdDays")).Returns(thresholdDays);

            var learningDelivery = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).LearningDeliveryAttributeData(dataEntity);

            learningDelivery.ActualDaysIL.Should().Be(actualDaysIL);
            learningDelivery.ActualNumInstalm.Should().Be(actualNumInstalm);
            learningDelivery.AdjStartDate.Should().Be(adjStartDate);
            learningDelivery.AgeAtProgStart.Should().Be(ageAtProgStart);
            learningDelivery.AppAdjLearnStartDate.Should().Be(appAdjLearnStartDate);
            learningDelivery.AppAdjLearnStartDateMatchPathway.Should().Be(appAdjLearnStartDateMatchPathway);
            learningDelivery.ApplicCompDate.Should().Be(applicCompDate);
            learningDelivery.CombinedAdjProp.Should().Be(combinedAdjProp);
            learningDelivery.Completed.Should().Be(completed);
            learningDelivery.FirstIncentiveThresholdDate.Should().Be(firstIncentiveThresholdDate);
            learningDelivery.FundStart.Should().Be(fundStart);
            learningDelivery.FworkCode.Should().Be(fworkCode);
            learningDelivery.LDApplic1618FrameworkUpliftBalancingValue.Should().Be(ldApplic1618FrameworkUpliftBalancingValue);
            learningDelivery.LDApplic1618FrameworkUpliftCompElement.Should().Be(ldApplic1618FrameworkUpliftCompElement);
            learningDelivery.LDApplic1618FRameworkUpliftCompletionValue.Should().Be(ldApplic1618FRameworkUpliftCompletionValue);
            learningDelivery.LDApplic1618FrameworkUpliftMonthInstalVal.Should().Be(ldApplic1618FrameworkUpliftMonthInstalVal);
            learningDelivery.LDApplic1618FrameworkUpliftPrevEarnings.Should().Be(ldApplic1618FrameworkUpliftPrevEarnings);
            learningDelivery.LDApplic1618FrameworkUpliftPrevEarningsStage1.Should().Be(ldApplic1618FrameworkUpliftPrevEarningsStage1);
            learningDelivery.LDApplic1618FrameworkUpliftRemainingAmount.Should().Be(ldApplic1618FrameworkUpliftRemainingAmount);
            learningDelivery.LDApplic1618FrameworkUpliftTotalActEarnings.Should().Be(ldApplic1618FrameworkUpliftTotalActEarnings);
            learningDelivery.LearnAimRef.Should().Be(learnAimRef);
            learningDelivery.LearnDel1618AtStart.Should().Be(learnDel1618AtStart);
            learningDelivery.LearnDelAppAccDaysIL.Should().Be(learnDelAppAccDaysIL);
            learningDelivery.LearnDelApplicDisadvAmount.Should().Be(learnDelApplicDisadvAmount);
            learningDelivery.LearnDelApplicEmp1618Incentive.Should().Be(learnDelApplicEmp1618Incentive);
            learningDelivery.LearnDelApplicEmpDate.Should().Be(learnDelApplicEmpDate);
            learningDelivery.LearnDelApplicProv1618FrameworkUplift.Should().Be(learnDelApplicProv1618FrameworkUplift);
            learningDelivery.LearnDelApplicProv1618Incentive.Should().Be(learnDelApplicProv1618Incentive);
            learningDelivery.LearnDelAppPrevAccDaysIL.Should().Be(learnDelAppPrevAccDaysIL);
            learningDelivery.LearnDelDaysIL.Should().Be(learnDelDaysIL);
            learningDelivery.LearnDelDisadAmount.Should().Be(learnDelDisadAmount);
            learningDelivery.LearnDelEligDisadvPayment.Should().Be(learnDelEligDisadvPayment);
            learningDelivery.LearnDelEmpIdFirstAdditionalPaymentThreshold.Should().Be(learnDelEmpIdFirstAdditionalPaymentThreshold);
            learningDelivery.LearnDelEmpIdSecondAdditionalPaymentThreshold.Should().Be(learnDelEmpIdSecondAdditionalPaymentThreshold);
            learningDelivery.LearnDelHistDaysThisApp.Should().Be(learnDelHistDaysThisApp);
            learningDelivery.LearnDelHistProgEarnings.Should().Be(learnDelHistProgEarnings);
            learningDelivery.LearnDelInitialFundLineType.Should().Be(learnDelInitialFundLineType);
            learningDelivery.LearnDelMathEng.Should().Be(learnDelMathEng);
            learningDelivery.LearnDelProgEarliestACT2Date.Should().Be(learnDelProgEarliestACT2Date);
            learningDelivery.LearnDelNonLevyProcured.Should().Be(learnDelNonLevyProcured);
            learningDelivery.LearnDelApplicCareLeaverIncentive.Should().Be(learnDelApplicCareLeaverIncentive);
            learningDelivery.LearnDelHistDaysCareLeavers.Should().Be(learnDelHistDaysCareLeavers);
            learningDelivery.LearnDelAccDaysILCareLeavers.Should().Be(learnDelAccDaysILCareLeavers);
            learningDelivery.LearnDelPrevAccDaysILCareLeavers.Should().Be(learnDelPrevAccDaysILCareLeavers);
            learningDelivery.LearnDelLearnerAddPayThresholdDate.Should().Be(learnDelLearnerAddPayThresholdDate);
            learningDelivery.LearnDelRedCode.Should().Be(learnDelRedCode);
            learningDelivery.LearnDelRedStartDate.Should().Be(learnDelRedStartDate);
            learningDelivery.MathEngAimValue.Should().Be(mathEngAimValue);
            learningDelivery.OutstandNumOnProgInstalm.Should().Be(outstandNumOnProgInstalm);
            learningDelivery.PlannedNumOnProgInstalm.Should().Be(plannedNumOnProgInstalm);
            learningDelivery.PlannedTotalDaysIL.Should().Be(plannedTotalDaysIL);
            learningDelivery.ProgType.Should().Be(progType);
            learningDelivery.PwayCode.Should().Be(pwayCode);
            learningDelivery.SecondIncentiveThresholdDate.Should().Be(secondIncentiveThresholdDate);
            learningDelivery.StdCode.Should().Be(stdCode);
            learningDelivery.ThresholdDays.Should().Be(thresholdDays);
        }

        [Fact]
        public void LearningDeliveryPeriodisedAttributeData_Correct()
        {
            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();
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

            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(It.IsAny<object>())).Returns(1.0m);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValueForPeriod(It.IsAny<IAttributeData>(), It.IsAny<DateTime>())).Returns(1.0m);

            var learningDeliveryPeriodisedAttributeData =
                NewService(internalDataCacheMock.Object, dataEntityAttributeServiceMock.Object).LearningDeliveryPeriodisedValues(TestLearningDeliveryEntity(null).Single());

            var expectedLearningDeliveryPeriodisedAttributeData = TestLearningDeliveryPeriodisedAttributesDataArray();

            expectedLearningDeliveryPeriodisedAttributeData.Should().BeEquivalentTo(learningDeliveryPeriodisedAttributeData);
        }

        [Fact]
        public void LearningDeliveryPeriodisedTextAttributeData_Correct()
        {
            var learningDeliveryPeriodisedTextAttributeData =
                NewService().LearningDeliveryPeriodisedTextValues(TestLearningDeliveryEntity(null).Single());

            var expectedLearningDeliveryPeriodisedTextAttributeData = TestLearningDeliveryPeriodisedTextAttributesDataArray();

            learningDeliveryPeriodisedTextAttributeData.Should().BeEquivalentTo(expectedLearningDeliveryPeriodisedTextAttributeData);
        }

        [Fact]
        public void ApprenticeshipPriceEpisodeFromDataEntity()
        {
            var priceEpisodeIdentifier = "1";

            var dataEntity = new DataEntity(string.Empty)
            {
                EntityName = "ApprenticeshipPriceEpisode",
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "PriceEpisodeIdentifier", Attribute(false, "1.0") },
                    { "EpisodeStartDate", Attribute(false, "1.0") },
                    { "TNP1", Attribute(false, "1.0") },
                    { "TNP2", Attribute(false, "1.0") },
                    { "TNP3", Attribute(false, "1.0") },
                    { "TNP4", Attribute(false, "1.0") },
                    { "PriceEpisodeUpperBandLimit", Attribute(false, "1.0") },
                    { "PriceEpisodePlannedEndDate", Attribute(false, "1.0") },
                    { "PriceEpisodeActualEndDate", Attribute(false, "1.0") },
                    { "PriceEpisodeTotalTNPPrice", Attribute(false, "1.0") },
                    { "PriceEpisodeUpperLimitAdjustment", Attribute(false, "1.0") },
                    { "PriceEpisodePlannedInstalments", Attribute(false, "1.0") },
                    { "PriceEpisodeActualInstalments", Attribute(false, "1.0") },
                    { "PriceEpisodeInstalmentsThisPeriod", Attribute(false, "1.0") },
                    { "PriceEpisodeCompletionElement", Attribute(false, "1.0") },
                    { "PriceEpisodePreviousEarnings", Attribute(false, "1.0") },
                    { "PriceEpisodeInstalmentValue", Attribute(false, "1.0") },
                    { "PriceEpisodeOnProgPayment", Attribute(false, "1.0") },
                    { "PriceEpisodeTotalEarnings", Attribute(false, "1.0") },
                    { "PriceEpisodeBalanceValue", Attribute(false, "1.0") },
                    { "PriceEpisodeBalancePayment", Attribute(false, "1.0") },
                    { "PriceEpisodeCompleted", Attribute(false, "1.0") },
                    { "PriceEpisodeCompletionPayment", Attribute(false, "1.0") },
                    { "PriceEpisodeRemainingTNPAmount", Attribute(false, "1.0") },
                    { "PriceEpisodeRemainingAmountWithinUpperLimit", Attribute(false, "1.0") },
                    { "PriceEpisodeCappedRemainingTNPAmount", Attribute(false, "1.0") },
                    { "PriceEpisodeExpectedTotalMonthlyValue", Attribute(false, "1.0") },
                    { "PriceEpisodeAimSeqNumber", Attribute(false, "1.0") },
                    { "PriceEpisodeFirstDisadvantagePayment", Attribute(false, "1.0") },
                    { "PriceEpisodeSecondDisadvantagePayment", Attribute(false, "1.0") },
                    { "PriceEpisodeApplic1618FrameworkUpliftBalancing", Attribute(false, "1.0") },
                    { "PriceEpisodeApplic1618FrameworkUpliftCompletionPayment", Attribute(false, "1.0") },
                    { "PriceEpisodeApplic1618FrameworkUpliftOnProgPayment", Attribute(false, "1.0") },
                    { "PriceEpisodeSecondProv1618Pay", Attribute(false, "1.0") },
                    { "PriceEpisodeFirstEmp1618Pay", Attribute(false, "1.0") },
                    { "PriceEpisodeSecondEmp1618Pay", Attribute(false, "1.0") },
                    { "PriceEpisodeFirstProv1618Pay", Attribute(false, "1.0") },
                    { "PriceEpisodeLSFCash", Attribute(false, "1.0") },
                    { "PriceEpisodeFundLineType", Attribute(false, "1.0") },
                    { "PriceEpisodeSFAContribPct", Attribute(false, "1.0") },
                    { "PriceEpisodeLevyNonPayInd", Attribute(false, "1.0") },
                    { "EpisodeEffectiveTNPStartDate", Attribute(false, "1.0") },
                    { "PriceEpisodeFirstAdditionalPaymentThresholdDate", Attribute(false, "1.0") },
                    { "PriceEpisodeSecondAdditionalPaymentThresholdDate", Attribute(false, "1.0") },
                    { "PriceEpisodeContractType", Attribute(false, "1.0") },
                    { "PriceEpisodePreviousEarningsSameProvider", Attribute(false, "1.0") },
                    { "PriceEpisodeTotProgFunding", Attribute(false, "1.0") },
                    { "PriceEpisodeProgFundIndMinCoInvest", Attribute(false, "1.0") },
                    { "PriceEpisodeProgFundIndMaxEmpCont", Attribute(false, "1.0") },
                    { "PriceEpisodeTotalPMRs", Attribute(false, "1.0") },
                    { "PriceEpisodeCumulativePMRs", Attribute(false, "1.0") },
                    { "PriceEpisodeCompExemCode", Attribute(false, "1.0") },
                    { "PriceEpisodeLearnerAdditionalPaymentThresholdDate", Attribute(false, "1.0") },
                    { "PriceEpisodeAgreeId", Attribute(false, "1.0") },
                    { "PriceEpisodeRedStartDate", Attribute(false, "1.0") },
                    { "PriceEpisodeRedStatusCode", Attribute(false, "1.0") },
                    { "PriceEpisodeLearnerAdditionalPayment", Attribute(false, "1.0") }
                }
            };

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "PriceEpisodeIdentifier")).Returns(priceEpisodeIdentifier);

            var priceEpisode = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).PriceEpisodeFromDataEntity(dataEntity);

            priceEpisode.PriceEpisodeIdentifier.Should().Be(priceEpisodeIdentifier);
        }

        [Fact]
        public void PriceEpisodeAttributeDataFromDataEntity()
        {
            var episodeStartDate = new DateTime(2018, 8, 1);
            var tnp1 = 1.0m;
            var tnp2 = 1.0m;
            var tnp3 = 1.0m;
            var tnp4 = 1.0m;
            var priceEpisodeUpperBandLimit = 1.0m;
            var priceEpisodePlannedEndDate = new DateTime(2018, 8, 1);
            var priceEpisodeActualEndDate = new DateTime(2018, 8, 1);
            var priceEpisodeTotalTNPPrice = 1.0m;
            var priceEpisodeUpperLimitAdjustment = 1.0m;
            var priceEpisodePlannedInstalments = 1;
            var priceEpisodeActualInstalments = 2;
            var priceEpisodeInstalmentsThisPeriod = 3;
            var priceEpisodeCompletionElement = 1.0m;
            var priceEpisodePreviousEarnings = 1.0m;
            var priceEpisodeInstalmentValue = 1.0m;
            var priceEpisodeOnProgPayment = 1.0m;
            var priceEpisodeTotalEarnings = 1.0m;
            var priceEpisodeBalanceValue = 1.0m;
            var priceEpisodeBalancePayment = 1.0m;
            var priceEpisodeCompleted = false;
            var priceEpisodeCompletionPayment = 1.0m;
            var priceEpisodeRemainingTNPAmount = 1.0m;
            var priceEpisodeRemainingAmountWithinUpperLimit = 1.0m;
            var priceEpisodeCappedRemainingTNPAmount = 1.0m;
            var priceEpisodeExpectedTotalMonthlyValue = 1.0m;
            var priceEpisodeAimSeqNumber = 1.0m;
            var priceEpisodeFirstDisadvantagePayment = 1.0m;
            var priceEpisodeSecondDisadvantagePayment = 1.0m;
            var priceEpisodeApplic1618FrameworkUpliftBalancing = 1.0m;
            var priceEpisodeApplic1618FrameworkUpliftCompletionPayment = 1.0m;
            var priceEpisodeApplic1618FrameworkUpliftOnProgPayment = 1.0m;
            var priceEpisodeSecondProv1618Pay = 1.0m;
            var priceEpisodeFirstEmp1618Pay = 1.0m;
            var priceEpisodeSecondEmp1618Pay = 1.0m;
            var priceEpisodeFirstProv1618Pay = 1.0m;
            var priceEpisodeLSFCash = 1.0m;
            var priceEpisodeFundLineType = "Type";
            var priceEpisodeSFAContribPct = 1.0m;
            var priceEpisodeLevyNonPayInd = 4;
            var episodeEffectiveTNPStartDate = new DateTime(2018, 8, 1);
            var priceEpisodeFirstAdditionalPaymentThresholdDate = new DateTime(2018, 8, 1);
            var priceEpisodeSecondAdditionalPaymentThresholdDate = new DateTime(2018, 8, 1);
            var priceEpisodeContractType = "Type";
            var priceEpisodePreviousEarningsSameProvider = 1.0m;
            var priceEpisodeTotProgFunding = 1.0m;
            var priceEpisodeProgFundIndMinCoInvest = 1.0m;
            var priceEpisodeProgFundIndMaxEmpCont = 1.0m;
            var priceEpisodeTotalPMRs = 1.0m;
            var priceEpisodeCumulativePMRs = 1.0m;
            var priceEpisodeCompExemCode = 5;
            var priceEpisodeLearnerAdditionalPaymentThresholdDate = new DateTime(2018, 8, 1);
            var priceEpisodeAgreeId = "Id";
            var priceEpisodeRedStartDate = new DateTime(2018, 8, 1);
            var priceEpisodeRedStatusCode = 6;

            var dataEntity = new DataEntity(string.Empty);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "EpisodeStartDate")).Returns(episodeStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "TNP1")).Returns(tnp1);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "TNP2")).Returns(tnp2);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "TNP3")).Returns(tnp3);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "TNP4")).Returns(tnp4);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeUpperBandLimit")).Returns(priceEpisodeUpperBandLimit);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "PriceEpisodePlannedEndDate")).Returns(priceEpisodePlannedEndDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "PriceEpisodeActualEndDate")).Returns(priceEpisodeActualEndDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeTotalTNPPrice")).Returns(priceEpisodeTotalTNPPrice);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeUpperLimitAdjustment")).Returns(priceEpisodeUpperLimitAdjustment);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PriceEpisodePlannedInstalments")).Returns(priceEpisodePlannedInstalments);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PriceEpisodeActualInstalments")).Returns(priceEpisodeActualInstalments);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PriceEpisodeInstalmentsThisPeriod")).Returns(priceEpisodeInstalmentsThisPeriod);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeCompletionElement")).Returns(priceEpisodeCompletionElement);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodePreviousEarnings")).Returns(priceEpisodePreviousEarnings);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeInstalmentValue")).Returns(priceEpisodeInstalmentValue);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeOnProgPayment")).Returns(priceEpisodeOnProgPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeTotalEarnings")).Returns(priceEpisodeTotalEarnings);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeBalanceValue")).Returns(priceEpisodeBalanceValue);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeBalancePayment")).Returns(priceEpisodeBalancePayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "PriceEpisodeCompleted")).Returns(priceEpisodeCompleted);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeCompletionPayment")).Returns(priceEpisodeCompletionPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeRemainingTNPAmount")).Returns(priceEpisodeRemainingTNPAmount);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeRemainingAmountWithinUpperLimit")).Returns(priceEpisodeRemainingAmountWithinUpperLimit);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeCappedRemainingTNPAmount")).Returns(priceEpisodeCappedRemainingTNPAmount);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeExpectedTotalMonthlyValue")).Returns(priceEpisodeExpectedTotalMonthlyValue);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeAimSeqNumber")).Returns(priceEpisodeAimSeqNumber);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeFirstDisadvantagePayment")).Returns(priceEpisodeFirstDisadvantagePayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeSecondDisadvantagePayment")).Returns(priceEpisodeSecondDisadvantagePayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeApplic1618FrameworkUpliftBalancing")).Returns(priceEpisodeApplic1618FrameworkUpliftBalancing);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeApplic1618FrameworkUpliftCompletionPayment")).Returns(priceEpisodeApplic1618FrameworkUpliftCompletionPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeApplic1618FrameworkUpliftOnProgPayment")).Returns(priceEpisodeApplic1618FrameworkUpliftOnProgPayment);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeSecondProv1618Pay")).Returns(priceEpisodeSecondProv1618Pay);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeFirstEmp1618Pay")).Returns(priceEpisodeFirstEmp1618Pay);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeSecondEmp1618Pay")).Returns(priceEpisodeSecondEmp1618Pay);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeFirstProv1618Pay")).Returns(priceEpisodeFirstProv1618Pay);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeLSFCash")).Returns(priceEpisodeLSFCash);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "PriceEpisodeFundLineType")).Returns(priceEpisodeFundLineType);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeSFAContribPct")).Returns(priceEpisodeSFAContribPct);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PriceEpisodeLevyNonPayInd")).Returns(priceEpisodeLevyNonPayInd);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "EpisodeEffectiveTNPStartDate")).Returns(episodeEffectiveTNPStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "PriceEpisodeFirstAdditionalPaymentThresholdDate")).Returns(priceEpisodeFirstAdditionalPaymentThresholdDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "PriceEpisodeSecondAdditionalPaymentThresholdDate")).Returns(priceEpisodeSecondAdditionalPaymentThresholdDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "PriceEpisodeContractType")).Returns(priceEpisodeContractType);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodePreviousEarningsSameProvider")).Returns(priceEpisodePreviousEarningsSameProvider);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeTotProgFunding")).Returns(priceEpisodeTotProgFunding);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeProgFundIndMinCoInvest")).Returns(priceEpisodeProgFundIndMinCoInvest);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeProgFundIndMaxEmpCont")).Returns(priceEpisodeProgFundIndMaxEmpCont);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeTotalPMRs")).Returns(priceEpisodeTotalPMRs);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PriceEpisodeCumulativePMRs")).Returns(priceEpisodeCumulativePMRs);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PriceEpisodeCompExemCode")).Returns(priceEpisodeCompExemCode);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "PriceEpisodeLearnerAdditionalPaymentThresholdDate")).Returns(priceEpisodeLearnerAdditionalPaymentThresholdDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "PriceEpisodeAgreeId")).Returns(priceEpisodeAgreeId);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "PriceEpisodeRedStartDate")).Returns(priceEpisodeRedStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PriceEpisodeRedStatusCode")).Returns(priceEpisodeRedStatusCode);

            var priceEpisode = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).PriceEpisodeValues(dataEntity);

            priceEpisode.EpisodeStartDate.Should().Be(episodeStartDate);
            priceEpisode.TNP1.Should().Be(tnp1);
            priceEpisode.TNP2.Should().Be(tnp2);
            priceEpisode.TNP3.Should().Be(tnp3);
            priceEpisode.TNP4.Should().Be(tnp4);
            priceEpisode.PriceEpisodeUpperBandLimit.Should().Be(priceEpisodeUpperBandLimit);
            priceEpisode.PriceEpisodePlannedEndDate.Should().Be(priceEpisodePlannedEndDate);
            priceEpisode.PriceEpisodeActualEndDate.Should().Be(priceEpisodeActualEndDate);
            priceEpisode.PriceEpisodeTotalTNPPrice.Should().Be(priceEpisodeTotalTNPPrice);
            priceEpisode.PriceEpisodeUpperLimitAdjustment.Should().Be(priceEpisodeUpperLimitAdjustment);
            priceEpisode.PriceEpisodePlannedInstalments.Should().Be(priceEpisodePlannedInstalments);
            priceEpisode.PriceEpisodeActualInstalments.Should().Be(priceEpisodeActualInstalments);
            priceEpisode.PriceEpisodeInstalmentsThisPeriod.Should().Be(priceEpisodeInstalmentsThisPeriod);
            priceEpisode.PriceEpisodeCompletionElement.Should().Be(priceEpisodeCompletionElement);
            priceEpisode.PriceEpisodePreviousEarnings.Should().Be(priceEpisodePreviousEarnings);
            priceEpisode.PriceEpisodeInstalmentValue.Should().Be(priceEpisodeInstalmentValue);
            priceEpisode.PriceEpisodeOnProgPayment.Should().Be(priceEpisodeOnProgPayment);
            priceEpisode.PriceEpisodeTotalEarnings.Should().Be(priceEpisodeTotalEarnings);
            priceEpisode.PriceEpisodeBalanceValue.Should().Be(priceEpisodeBalanceValue);
            priceEpisode.PriceEpisodeBalancePayment.Should().Be(priceEpisodeBalancePayment);
            priceEpisode.PriceEpisodeCompleted.Should().Be(priceEpisodeCompleted);
            priceEpisode.PriceEpisodeCompletionPayment.Should().Be(priceEpisodeCompletionPayment);
            priceEpisode.PriceEpisodeRemainingTNPAmount.Should().Be(priceEpisodeRemainingTNPAmount);
            priceEpisode.PriceEpisodeRemainingAmountWithinUpperLimit.Should().Be(priceEpisodeRemainingAmountWithinUpperLimit);
            priceEpisode.PriceEpisodeCappedRemainingTNPAmount.Should().Be(priceEpisodeCappedRemainingTNPAmount);
            priceEpisode.PriceEpisodeExpectedTotalMonthlyValue.Should().Be(priceEpisodeExpectedTotalMonthlyValue);
            priceEpisode.PriceEpisodeFirstDisadvantagePayment.Should().Be(priceEpisodeFirstDisadvantagePayment);
            priceEpisode.PriceEpisodeSecondDisadvantagePayment.Should().Be(priceEpisodeSecondDisadvantagePayment);
            priceEpisode.PriceEpisodeApplic1618FrameworkUpliftBalancing.Should().Be(priceEpisodeApplic1618FrameworkUpliftBalancing);
            priceEpisode.PriceEpisodeApplic1618FrameworkUpliftCompletionPayment.Should().Be(priceEpisodeApplic1618FrameworkUpliftCompletionPayment);
            priceEpisode.PriceEpisodeApplic1618FrameworkUpliftOnProgPayment.Should().Be(priceEpisodeApplic1618FrameworkUpliftOnProgPayment);
            priceEpisode.PriceEpisodeSecondProv1618Pay.Should().Be(priceEpisodeSecondProv1618Pay);
            priceEpisode.PriceEpisodeFirstEmp1618Pay.Should().Be(priceEpisodeFirstEmp1618Pay);
            priceEpisode.PriceEpisodeSecondEmp1618Pay.Should().Be(priceEpisodeSecondEmp1618Pay);
            priceEpisode.PriceEpisodeFirstProv1618Pay.Should().Be(priceEpisodeFirstProv1618Pay);
            priceEpisode.PriceEpisodeLSFCash.Should().Be(priceEpisodeLSFCash);
            priceEpisode.PriceEpisodeFundLineType.Should().Be(priceEpisodeFundLineType);
            priceEpisode.PriceEpisodeSFAContribPct.Should().Be(priceEpisodeSFAContribPct);
            priceEpisode.PriceEpisodeLevyNonPayInd.Should().Be(priceEpisodeLevyNonPayInd);
            priceEpisode.EpisodeEffectiveTNPStartDate.Should().Be(episodeEffectiveTNPStartDate);
            priceEpisode.PriceEpisodeFirstAdditionalPaymentThresholdDate.Should().Be(priceEpisodeFirstAdditionalPaymentThresholdDate);
            priceEpisode.PriceEpisodeSecondAdditionalPaymentThresholdDate.Should().Be(priceEpisodeSecondAdditionalPaymentThresholdDate);
            priceEpisode.PriceEpisodeContractType.Should().Be(priceEpisodeContractType);
            priceEpisode.PriceEpisodePreviousEarningsSameProvider.Should().Be(priceEpisodePreviousEarningsSameProvider);
            priceEpisode.PriceEpisodeTotProgFunding.Should().Be(priceEpisodeTotProgFunding);
            priceEpisode.PriceEpisodeProgFundIndMinCoInvest.Should().Be(priceEpisodeProgFundIndMinCoInvest);
            priceEpisode.PriceEpisodeProgFundIndMaxEmpCont.Should().Be(priceEpisodeProgFundIndMaxEmpCont);
            priceEpisode.PriceEpisodeTotalPMRs.Should().Be(priceEpisodeTotalPMRs);
            priceEpisode.PriceEpisodeCumulativePMRs.Should().Be(priceEpisodeCumulativePMRs);
            priceEpisode.PriceEpisodeCompExemCode.Should().Be(priceEpisodeCompExemCode);
            priceEpisode.PriceEpisodeLearnerAdditionalPaymentThresholdDate.Should().Be(priceEpisodeLearnerAdditionalPaymentThresholdDate);
            priceEpisode.PriceEpisodeAgreeId.Should().Be(priceEpisodeAgreeId);
            priceEpisode.PriceEpisodeRedStartDate.Should().Be(priceEpisodeRedStartDate);
            priceEpisode.PriceEpisodeRedStatusCode.Should().Be(priceEpisodeRedStatusCode);
        }

        [Fact]
        public void PriceEpisodePeriodisedAttributeData_Correct()
        {
            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();
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

            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(It.IsAny<object>())).Returns(1.0m);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValueForPeriod(It.IsAny<IAttributeData>(), It.IsAny<DateTime>())).Returns(1.0m);

            var priceEpisodePeriodisedAttributeData =
                NewService(internalDataCacheMock.Object, dataEntityAttributeServiceMock.Object).PriceEpisodePeriodisedValues(TestPriceEpisodeEntity(null).Single());

            var expectedPriceEpisodePeriodisedAttributeData = TestPriceEpisodePeriodisedAttributesDataArray();

            expectedPriceEpisodePeriodisedAttributeData.Should().BeEquivalentTo(priceEpisodePeriodisedAttributeData);
        }

        [Fact]
        public void HistoricEarningOutputDataFromDataEntity()
        {
            var appIdentifierOutput = "Id";
            var appProgCompletedInTheYearOutput = false;
            var BalancingProgAimPaymentsInTheYearOutput = 1.0m;
            var CompletionProgAimPaymentsInTheYearOutput = 1.0m;
            var OnProgProgAimPaymentsInTheYearOutput = 1.0m;
            var historicDaysInYearOutput = 1;
            var historicEffectiveTNPStartDateOutput = new DateTime(2018, 8, 1);
            var historicEmpIdEndWithinYearOutput = 2;
            var historicEmpIdStartWithinYearOutput = 3;
            var historicFworkCodeOutput = 4;
            var historicLearner1618AtStartOutput = false;
            var historicPMRAmountOutput = 1.0m;
            var historicProgrammeStartDateIgnorePathwayOutput = new DateTime(2018, 8, 1);
            var historicProgrammeStartDateMatchPathwayOutput = new DateTime(2018, 8, 1);
            var historicProgTypeOutput = 5;
            var historicPwayCodeOutput = 6;
            var historicSTDCodeOutput = 7;
            var historicTNP1Output = 1.0m;
            var historicTNP2Output = 1.0m;
            var historicTNP3Output = 1.0m;
            var historicTNP4Output = 1.0m;
            var historicTotal1618UpliftPaymentsInTheYear = 1.0m;
            var historicTotalProgAimPaymentsInTheYear = 1.0m;
            var historicULNOutput = 8;
            var historicUptoEndDateOutput = new DateTime(2018, 8, 1);
            var historicVirtualTNP3EndofThisYearOutput = 1.0m;
            var historicVirtualTNP4EndofThisYearOutput = 1.0m;
            var historicLearnDelProgEarliestACT2DateOutput = new DateTime(2018, 8, 1);

            var dataEntity = new DataEntity(string.Empty);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "AppIdentifierOutput")).Returns(appIdentifierOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AppProgCompletedInTheYearOutput")).Returns(appProgCompletedInTheYearOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "HistoricBalancingProgAimPaymentsInTheYear")).Returns(BalancingProgAimPaymentsInTheYearOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "HistoricCompletionProgAimPaymentsInTheYear")).Returns(CompletionProgAimPaymentsInTheYearOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "HistoricDaysInYearOutput")).Returns(historicDaysInYearOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "HistoricEffectiveTNPStartDateOutput")).Returns(historicEffectiveTNPStartDateOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "HistoricEmpIdEndWithinYearOutput")).Returns(historicEmpIdEndWithinYearOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "HistoricEmpIdStartWithinYearOutput")).Returns(historicEmpIdStartWithinYearOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "HistoricFworkCodeOutput")).Returns(historicFworkCodeOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "HistoricLearner1618AtStartOutput")).Returns(historicLearner1618AtStartOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "HistoricOnProgProgAimPaymentsInTheYear")).Returns(OnProgProgAimPaymentsInTheYearOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "HistoricPMRAmountOutput")).Returns(historicPMRAmountOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "HistoricProgrammeStartDateIgnorePathwayOutput")).Returns(historicProgrammeStartDateIgnorePathwayOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "HistoricProgrammeStartDateMatchPathwayOutput")).Returns(historicProgrammeStartDateMatchPathwayOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "HistoricProgTypeOutput")).Returns(historicProgTypeOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "HistoricPwayCodeOutput")).Returns(historicPwayCodeOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "HistoricSTDCodeOutput")).Returns(historicSTDCodeOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "HistoricTNP1Output")).Returns(historicTNP1Output);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "HistoricTNP2Output")).Returns(historicTNP2Output);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "HistoricTNP3Output")).Returns(historicTNP3Output);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "HistoricTNP4Output")).Returns(historicTNP4Output);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "HistoricTotal1618UpliftPaymentsInTheYear")).Returns(historicTotal1618UpliftPaymentsInTheYear);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "HistoricTotalProgAimPaymentsInTheYear")).Returns(historicTotalProgAimPaymentsInTheYear);
            dataEntityAttributeServiceMock.Setup(s => s.GetLongAttributeValue(dataEntity, "HistoricULNOutput")).Returns(historicULNOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "HistoricUptoEndDateOutput")).Returns(historicUptoEndDateOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "HistoricVirtualTNP3EndofThisYearOutput")).Returns(historicVirtualTNP3EndofThisYearOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "HistoricVirtualTNP4EndofThisYearOutput")).Returns(historicVirtualTNP4EndofThisYearOutput);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "HistoricLearnDelProgEarliestACT2DateOutput")).Returns(historicLearnDelProgEarliestACT2DateOutput);

            var historicEarningOutput = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).HistoricEarningOutputDataFromDataEntity(dataEntity);

            historicEarningOutput.AppIdentifierOutput.Should().Be(appIdentifierOutput);
            historicEarningOutput.AppProgCompletedInTheYearOutput.Should().Be(appProgCompletedInTheYearOutput);
            historicEarningOutput.BalancingProgAimPaymentsInTheYear.Should().Be(BalancingProgAimPaymentsInTheYearOutput);
            historicEarningOutput.CompletionProgAimPaymentsInTheYear.Should().Be(CompletionProgAimPaymentsInTheYearOutput);
            historicEarningOutput.HistoricDaysInYearOutput.Should().Be(historicDaysInYearOutput);
            historicEarningOutput.HistoricEffectiveTNPStartDateOutput.Should().Be(historicEffectiveTNPStartDateOutput);
            historicEarningOutput.HistoricEmpIdEndWithinYearOutput.Should().Be(historicEmpIdEndWithinYearOutput);
            historicEarningOutput.HistoricEmpIdStartWithinYearOutput.Should().Be(historicEmpIdStartWithinYearOutput);
            historicEarningOutput.HistoricFworkCodeOutput.Should().Be(historicFworkCodeOutput);
            historicEarningOutput.HistoricLearner1618AtStartOutput.Should().Be(historicLearner1618AtStartOutput);
            historicEarningOutput.OnProgProgAimPaymentsInTheYear.Should().Be(OnProgProgAimPaymentsInTheYearOutput);
            historicEarningOutput.HistoricPMRAmountOutput.Should().Be(historicPMRAmountOutput);
            historicEarningOutput.HistoricProgrammeStartDateIgnorePathwayOutput.Should().Be(historicProgrammeStartDateIgnorePathwayOutput);
            historicEarningOutput.HistoricProgrammeStartDateMatchPathwayOutput.Should().Be(historicProgrammeStartDateMatchPathwayOutput);
            historicEarningOutput.HistoricProgTypeOutput.Should().Be(historicProgTypeOutput);
            historicEarningOutput.HistoricPwayCodeOutput.Should().Be(historicPwayCodeOutput);
            historicEarningOutput.HistoricSTDCodeOutput.Should().Be(historicSTDCodeOutput);
            historicEarningOutput.HistoricTNP1Output.Should().Be(historicTNP1Output);
            historicEarningOutput.HistoricTNP2Output.Should().Be(historicTNP2Output);
            historicEarningOutput.HistoricTNP3Output.Should().Be(historicTNP3Output);
            historicEarningOutput.HistoricTNP4Output.Should().Be(historicTNP4Output);
            historicEarningOutput.HistoricTotal1618UpliftPaymentsInTheYear.Should().Be(historicTotal1618UpliftPaymentsInTheYear);
            historicEarningOutput.HistoricTotalProgAimPaymentsInTheYear.Should().Be(historicTotalProgAimPaymentsInTheYear);
            historicEarningOutput.HistoricULNOutput.Should().Be(historicULNOutput);
            historicEarningOutput.HistoricUptoEndDateOutput.Should().Be(historicUptoEndDateOutput);
            historicEarningOutput.HistoricVirtualTNP3EndofThisYearOutput.Should().Be(historicVirtualTNP3EndofThisYearOutput);
            historicEarningOutput.HistoricVirtualTNP4EndofThisYearOutput.Should().Be(historicVirtualTNP4EndofThisYearOutput);
            historicEarningOutput.HistoricLearnDelProgEarliestACT2DateOutput.Should().Be(historicLearnDelProgEarliestACT2DateOutput);
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

        private IEnumerable<IDataEntity> TestLearningDeliveryEntity(DataEntity parent)
        {
            var entities = new List<DataEntity>();

            var entity = new DataEntity("LearningDelivery")
            {
                EntityName = "LearningDelivery",
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "ActualDaysIL", Attribute(false, "1.0") },
                    { "ActualNumInstalm", Attribute(false, "1.0") },
                    { "AdjStartDate", Attribute(false, "1.0") },
                    { "AgeAtProgStart", Attribute(false, "1.0") },
                    { "AimSeqNumber", Attribute(false, "1.0") },
                    { "AimType", Attribute(false, "1.0") },
                    { "AppAdjLearnStartDate ", Attribute(false, "1.0") },
                    { "AppAdjLearnStartDateMatchPathway", Attribute(false, "1.0") },
                    { "ApplicCompDate", Attribute(false, "1.0") },
                    { "CombinedAdjProp", Attribute(false, "1.0") },
                    { "Completed", Attribute(false, "1.0") },
                    { "CompStatus", Attribute(false, "1.0") },
                    { "FirstIncentiveThresholdDate", Attribute(false, "1.0") },
                    { "FrameworkCommonComponent", Attribute(false, "1.0") },
                    { "FundStart", Attribute(false, "1.0") },
                    { "FworkCode", Attribute(false, "1.0") },
                    { "LDApplic1618FrameworkUpliftBalancingValue", Attribute(false, "1.0") },
                    { "LDApplic1618FrameworkUpliftCompElement", Attribute(false, "1.0") },
                    { "LDApplic1618FRameworkUpliftCompletionValue", Attribute(false, "1.0") },
                    { "LDApplic1618FrameworkUpliftMonthInstalVal", Attribute(false, "1.0") },
                    { "LDApplic1618FrameworkUpliftPrevEarnings", Attribute(false, "1.0") },
                    { "LDApplic1618FrameworkUpliftPrevEarningsStage1", Attribute(false, "1.0") },
                    { "LDApplic1618FrameworkUpliftRemainingAmount", Attribute(false, "1.0") },
                    { "LDApplic1618FrameworkUpliftTotalActEarnings", Attribute(false, "1.0") },
                    { "LearnAimRef", Attribute(false, "1.0") },
                    { "LearnActEndDate", Attribute(false, "1.0") },
                    { "LearnDel1618AtStart", Attribute(false, "1.0") },
                    { "LearnDelAppAccDaysIL", Attribute(false, "1.0") },
                    { "LearnDelApplicDisadvAmount", Attribute(false, "1.0") },
                    { "LearnDelApplicEmp1618Incentive", Attribute(false, "1.0") },
                    { "LearnDelApplicEmpDate", Attribute(false, "1.0") },
                    { "LearnDelApplicProv1618FrameworkUplift", Attribute(false, "1.0") },
                    { "LearnDelApplicProv1618Incentive", Attribute(false, "1.0") },
                    { "LearnDelAppPrevAccDaysIL", Attribute(false, "1.0") },
                    { "LearnDelDaysIL", Attribute(false, "1.0") },
                    { "LearnDelDisadAmount", Attribute(false, "1.0") },
                    { "LearnDelEligDisadvPayment", Attribute(false, "1.0") },
                    { "LearnDelEmpIdFirstAdditionalPaymentThreshold", Attribute(false, "1.0") },
                    { "LearnDelEmpIdSecondAdditionalPaymentThreshold", Attribute(false, "1.0") },
                    { "LearnDelHistDaysThisApp", Attribute(false, "1.0") },
                    { "LearnDelHistProgEarnings", Attribute(false, "1.0") },
                    { "LearnDelInitialFundLineType", Attribute(false, "1.0") },
                    { "LearnDelMathEng", Attribute(false, "1.0") },
                    { "LearnDelProgEarliestACT2Date", Attribute(false, "1.0") },
                    { "LearnDelNonLevyProcured", Attribute(false, "1.0") },
                    { "LearnDelApplicCareLeaverIncentive", Attribute(false, "1.0") },
                    { "LearnDelHistDaysCareLeavers", Attribute(false, "1.0") },
                    { "LearnDelAccDaysILCareLeavers", Attribute(false, "1.0") },
                    { "LearnDelPrevAccDaysILCareLeavers", Attribute(false, "1.0") },
                    { "LearnDelLearnerAddPayThresholdDate", Attribute(false, "1.0") },
                    { "LearnDelRedCode", Attribute(false, "1.0") },
                    { "LearnDelRedStartDate", Attribute(false, "1.0") },
                    { "LearnPlanEndDate", Attribute(false, "1.0") },
                    { "LearnStartDate", Attribute(false, "1.0") },
                    { "LrnDelFAM_EEF", Attribute(false, "1.0") },
                    { "LrnDelFAM_LDM1", Attribute(false, "1.0") },
                    { "LrnDelFAM_LDM2", Attribute(false, "1.0") },
                    { "LrnDelFAM_LDM3", Attribute(false, "1.0") },
                    { "LrnDelFAM_LDM4", Attribute(false, "1.0") },
                    { "MathEngAimValue", Attribute(false, "1.0") },
                    { "OutstandNumOnProgInstalm", Attribute(false, "1.0") },
                    { "OrigLearnStartDate", Attribute(false, "1.0") },
                    { "OtherFundAdj", Attribute(false, "1.0") },
                    { "PlannedNumOnProgInstalm", Attribute(false, "1.0") },
                    { "PlannedTotalDaysIL", Attribute(false, "1.0") },
                    { "PriorLearnFundAdj", Attribute(false, "1.0") },
                    { "ProgType", Attribute(false, "1.0") },
                    { "PwayCode", Attribute(false, "1.0") },
                    { "SecondIncentiveThresholdDate", Attribute(false, "1.0") },
                    { "STDCode", Attribute(false, "1.0") },
                    { "ThresholdDays", Attribute(false, "1.0") },
                    { "DisadvFirstPayment", Attribute(false, "1.0") },
                    { "DisadvSecondPayment", Attribute(false, "1.0") },
                    { "FundLineType", Attribute(false, "Type") },
                    { "InstPerPeriod", Attribute(false, "1.0") },
                    { "LDApplic1618FrameworkUpliftBalancingPayment", Attribute(false, "1.0") },
                    { "LDApplic1618FrameworkUpliftCompletionPayment", Attribute(false, "1.0") },
                    { "LDApplic1618FrameworkUpliftOnProgPayment", Attribute(false, "1.0") },
                    { "LearnDelContType", Attribute(false, "Type") },
                    { "LearnDelFirstEmp1618Pay", Attribute(false, "1.0") },
                    { "LearnDelFirstProv1618Pay", Attribute(false, "1.0") },
                    { "LearnDelLevyNonPayInd", Attribute(false, "1.0") },
                    { "LearnDelSecondEmp1618Pay", Attribute(false, "1.0") },
                    { "LearnDelSecondProv1618Pay", Attribute(false, "1.0") },
                    { "LearnDelSEMContWaiver", Attribute(false, "1.0") },
                    { "LearnDelSFAContribPct", Attribute(false, "1.0") },
                    { "LearnSuppFund", Attribute(false, "1.0") },
                    { "LearnSuppFundCash", Attribute(false, "1.0") },
                    { "MathEngBalPayment", Attribute(false, "1.0") },
                    { "MathEngBalPct", Attribute(false, "1.0") },
                    { "MathEngOnProgPayment", Attribute(false, "1.0") },
                    { "MathEngOnProgPct", Attribute(false, "1.0") },
                    { "ProgrammeAimBalPayment", Attribute(false, "1.0") },
                    { "ProgrammeAimCompletionPayment", Attribute(false, "1.0") },
                    { "ProgrammeAimOnProgPayment", Attribute(false, "1.0") },
                    { "ProgrammeAimProgFundIndMaxEmpCont", Attribute(false, "1.0") },
                    { "ProgrammeAimProgFundIndMinCoInvest", Attribute(false, "1.0") },
                    { "ProgrammeAimTotProgFund", Attribute(true, "1.0") },
                    { "LearnDelLearnAddPayment", Attribute(true, "1.0") },
                },

                Parent = parent,
            };

            entities.Add(entity);

            return entities;
        }

        private LearningDeliveryPeriodisedValues[] TestLearningDeliveryPeriodisedAttributesDataArray()
        {
            return new LearningDeliveryPeriodisedValues[]
            {
                TestLearningDeliveryPeriodisedAttributesData("DisadvFirstPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("DisadvSecondPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("InstPerPeriod", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LDApplic1618FrameworkUpliftBalancingPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LDApplic1618FrameworkUpliftCompletionPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LDApplic1618FrameworkUpliftOnProgPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnDelFirstEmp1618Pay", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnDelFirstProv1618Pay", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnDelLearnAddPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnDelLevyNonPayInd", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnDelSecondEmp1618Pay", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnDelSecondProv1618Pay", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnDelSEMContWaiver", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnDelSFAContribPct", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnSuppFund", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnSuppFundCash", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("MathEngBalPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("MathEngBalPct", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("MathEngOnProgPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("MathEngOnProgPct", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("ProgrammeAimBalPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("ProgrammeAimCompletionPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("ProgrammeAimOnProgPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("ProgrammeAimProgFundIndMaxEmpCont", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("ProgrammeAimProgFundIndMinCoInvest", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("ProgrammeAimTotProgFund", 1.0m)
            };
        }

        private LearningDeliveryPeriodisedTextValues[] TestLearningDeliveryPeriodisedTextAttributesDataArray()
        {
            return new LearningDeliveryPeriodisedTextValues[]
            {
                TestLearningDeliveryPeriodisedAttributesData("FundLineType", "Type"),
                TestLearningDeliveryPeriodisedAttributesData("LearnDelContType", "Type"),
            };
        }

        private LearningDeliveryPeriodisedValues TestLearningDeliveryPeriodisedAttributesData(string attribute, decimal value)
        {
            return new LearningDeliveryPeriodisedValues
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

        private LearningDeliveryPeriodisedTextValues TestLearningDeliveryPeriodisedAttributesData(string attribute, string value)
        {
            return new LearningDeliveryPeriodisedTextValues
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

        private IEnumerable<IDataEntity> TestPriceEpisodeEntity(DataEntity parent)
        {
            var entities = new List<DataEntity>();

            var entity = new DataEntity(string.Empty)
            {
                EntityName = "ApprenticeshipPriceEpisode",
                Attributes = new Dictionary<string, IAttributeData>
                {
                    { "PriceEpisodeIdentifier", Attribute(false, "1.0") },
                    { "EpisodeStartDate", Attribute(false, "1.0") },
                    { "TNP1", Attribute(false, "1.0") },
                    { "TNP2", Attribute(false, "1.0") },
                    { "TNP3", Attribute(false, "1.0") },
                    { "TNP4", Attribute(false, "1.0") },
                    { "PriceEpisodeUpperBandLimit", Attribute(false, "1.0") },
                    { "PriceEpisodePlannedEndDate", Attribute(false, "1.0") },
                    { "PriceEpisodeActualEndDate", Attribute(false, "1.0") },
                    { "PriceEpisodeTotalTNPPrice", Attribute(false, "1.0") },
                    { "PriceEpisodeUpperLimitAdjustment", Attribute(false, "1.0") },
                    { "PriceEpisodePlannedInstalments", Attribute(false, "1.0") },
                    { "PriceEpisodeActualInstalments", Attribute(false, "1.0") },
                    { "PriceEpisodeInstalmentsThisPeriod", Attribute(false, "1.0") },
                    { "PriceEpisodeCompletionElement", Attribute(false, "1.0") },
                    { "PriceEpisodePreviousEarnings", Attribute(false, "1.0") },
                    { "PriceEpisodeInstalmentValue", Attribute(false, "1.0") },
                    { "PriceEpisodeOnProgPayment", Attribute(false, "1.0") },
                    { "PriceEpisodeTotalEarnings", Attribute(false, "1.0") },
                    { "PriceEpisodeBalanceValue", Attribute(false, "1.0") },
                    { "PriceEpisodeBalancePayment", Attribute(false, "1.0") },
                    { "PriceEpisodeCompleted", Attribute(false, "1.0") },
                    { "PriceEpisodeCompletionPayment", Attribute(false, "1.0") },
                    { "PriceEpisodeRemainingTNPAmount", Attribute(false, "1.0") },
                    { "PriceEpisodeRemainingAmountWithinUpperLimit", Attribute(false, "1.0") },
                    { "PriceEpisodeCappedRemainingTNPAmount", Attribute(false, "1.0") },
                    { "PriceEpisodeExpectedTotalMonthlyValue", Attribute(false, "1.0") },
                    { "PriceEpisodeAimSeqNumber", Attribute(false, "1.0") },
                    { "PriceEpisodeFirstDisadvantagePayment", Attribute(false, "1.0") },
                    { "PriceEpisodeSecondDisadvantagePayment", Attribute(false, "1.0") },
                    { "PriceEpisodeApplic1618FrameworkUpliftBalancing", Attribute(false, "1.0") },
                    { "PriceEpisodeApplic1618FrameworkUpliftCompletionPayment", Attribute(false, "1.0") },
                    { "PriceEpisodeApplic1618FrameworkUpliftOnProgPayment", Attribute(false, "1.0") },
                    { "PriceEpisodeSecondProv1618Pay", Attribute(false, "1.0") },
                    { "PriceEpisodeFirstEmp1618Pay", Attribute(false, "1.0") },
                    { "PriceEpisodeSecondEmp1618Pay", Attribute(false, "1.0") },
                    { "PriceEpisodeFirstProv1618Pay", Attribute(false, "1.0") },
                    { "PriceEpisodeLSFCash", Attribute(false, "1.0") },
                    { "PriceEpisodeFundLineType", Attribute(false, "1.0") },
                    { "PriceEpisodeSFAContribPct", Attribute(true, "1.0") },
                    { "PriceEpisodeLevyNonPayInd", Attribute(true, "1.0") },
                    { "EpisodeEffectiveTNPStartDate", Attribute(false, "1.0") },
                    { "PriceEpisodeFirstAdditionalPaymentThresholdDate", Attribute(false, "1.0") },
                    { "PriceEpisodeSecondAdditionalPaymentThresholdDate", Attribute(false, "1.0") },
                    { "PriceEpisodeContractType", Attribute(false, "1.0") },
                    { "PriceEpisodePreviousEarningsSameProvider", Attribute(false, "1.0") },
                    { "PriceEpisodeTotProgFunding", Attribute(false, "1.0") },
                    { "PriceEpisodeProgFundIndMinCoInvest", Attribute(false, "1.0") },
                    { "PriceEpisodeProgFundIndMaxEmpCont", Attribute(false, "1.0") },
                    { "PriceEpisodeTotalPMRs", Attribute(false, "1.0") },
                    { "PriceEpisodeCumulativePMRs", Attribute(false, "1.0") },
                    { "PriceEpisodeCompExemCode", Attribute(false, "1.0") },
                    { "PriceEpisodeLearnerAdditionalPaymentThresholdDate", Attribute(false, "1.0") },
                    { "PriceEpisodeAgreeId", Attribute(false, "1.0") },
                    { "PriceEpisodeRedStartDate", Attribute(false, "1.0") },
                    { "PriceEpisodeRedStatusCode", Attribute(false, "1.0") },
                    { "PriceEpisodeLearnerAdditionalPayment", Attribute(true, "1.0") }
                },

                Parent = parent,
            };

            entities.Add(entity);

            return entities;
        }

        private PriceEpisodePeriodisedValues[] TestPriceEpisodePeriodisedAttributesDataArray()
        {
            return new PriceEpisodePeriodisedValues[]
            {
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeApplic1618FrameworkUpliftBalancing", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeApplic1618FrameworkUpliftCompletionPayment", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeApplic1618FrameworkUpliftOnProgPayment", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeBalancePayment", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeBalanceValue", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeCompletionPayment", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeFirstDisadvantagePayment", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeFirstEmp1618Pay", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeFirstProv1618Pay", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeInstalmentsThisPeriod", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeLearnerAdditionalPayment", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeLevyNonPayInd", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeLSFCash", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeOnProgPayment", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeProgFundIndMaxEmpCont", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeProgFundIndMinCoInvest", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeSecondDisadvantagePayment", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeSecondEmp1618Pay", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeSecondProv1618Pay", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeSFAContribPct", 1.0m),
                TestPriceEpisodePeriodisedAttributesData("PriceEpisodeTotProgFunding", 1.0m)
            };
        }

        private PriceEpisodePeriodisedValues TestPriceEpisodePeriodisedAttributesData(string attribute, decimal value)
        {
            return new PriceEpisodePeriodisedValues
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

using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM35.Service;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using FluentAssertions;
using Moq;
using Oracle.Determinations.Masquerade.Util;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Tests
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
            var orgVersion = "OrgVersion";
            var postcodeDisadvantageVersion = "PostcodeDisadvantageVersion";
            var rulebaseVersion = "RulebaseVersion";

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "UKPRN")).Returns(ukprn);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "CurFundYr")).Returns(curFundYr);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LARSVersion")).Returns(larsVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "OrgVersion")).Returns(orgVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "PostcodeDisadvantageVersion")).Returns(postcodeDisadvantageVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "RulebaseVersion")).Returns(rulebaseVersion);

            var global = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).MapGlobal(dataEntity);

            global.UKPRN.Should().Be(ukprn);
            global.CurFundYr.Should().Be(curFundYr);
            global.LARSVersion.Should().Be(larsVersion);
            global.OrgVersion.Should().Be(orgVersion);
            global.PostcodeDisadvantageVersion.Should().Be(postcodeDisadvantageVersion);
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
            var achieved = false;
            var achieveElement = 1;
            var achievePayElig = false;
            var achievePayPctPreTrans = 1;
            var achPayTransHeldBack = 1;
            var actualDaysIL = 1;
            var actualNumInstalm = 1;
            var actualNumInstalmPreTrans = 1;
            var actualNumInstalmTrans = 1;
            var adjLearnStartDate = new DateTime(2018, 09, 01);
            var adltLearnResp = false;
            var ageAimStart = 1;
            var aimValue = 1.0m;
            var appAdjLearnStartDate = new DateTime(2018, 09, 01);
            var appAgeFact = 1.6m;
            var appATAGTA = false;
            var appCompetency = false;
            var appFuncSkill = false;
            var appFuncSkill1618AdjFact = 1.0m;
            var appKnowl = false;
            var appLearnStartDate = new DateTime(2018, 09, 01);
            var applicEmpFactDate = new DateTime(2018, 09, 01);
            var applicFactDate = new DateTime(2018, 09, 01);
            var applicFundRateDate = new DateTime(2018, 09, 01);
            var applicProgWeightFact = "1.0";
            var applicUnweightFundRate = 1;
            var applicWeightFundRate = 1;
            var appNonFund = false;
            var areaCostFactAdj = 1;
            var balInstalmPreTrans = 1;
            var baseValueUnweight = 1;
            var capFactor = 1;
            var disUpFactAdj = 1;
            var empOutcomePayElig = false;
            var empOutcomePctHeldBackTrans = 1;
            var empOutcomePctPreTrans = 1;
            var empRespOth = false;
            var eSOL = false;
            var fullyFund = false;
            var fundLine = "1.0";
            var fundStart = false;
            var largeEmployerFM35Fctr = 1;
            var largeEmployerID = 1;
            var largeEmployerStatusDate = new DateTime(2018, 09, 01);
            var lTRCUpliftFctr = 1;
            var nonGovCont = 1;
            var oLASSCustody = false;
            var onProgPayPctPreTrans = 1.7m;
            var outstndNumOnProgInstalm = 1;
            var outstndNumOnProgInstalmTrans = 1;
            var plannedNumOnProgInstalm = 1;
            var plannedNumOnProgInstalmTrans = 1;
            var plannedTotalDaysIL = 1;
            var plannedTotalDaysILPreTrans = 1;
            var propFundRemain = 1;
            var propFundRemainAch = 1;
            var prscHEAim = false;
            var residential = false;
            var restart = false;
            var specResUplift = 1;
            var startPropTrans = 1;
            var thresholdDays = 1;
            var traineeship = false;
            var trans = false;
            var trnAdjLearnStartDate = new DateTime(2018, 09, 01);
            var trnWorkPlaceAim = false;
            var trnWorkPrepAim = false;
            var unWeightedRateFromESOL = 1;
            var unweightedRateFromLARS = 1;
            var weightedRateFromESOL = 1;
            var weightedRateFromLARS = 1;

            var dataEntity = new DataEntity(string.Empty);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AchApplicDate")).Returns(achApplicDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Achieved")).Returns(achieved);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AchieveElement")).Returns(achieveElement);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AchievePayElig")).Returns(achievePayElig);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AchievePayPctPreTrans")).Returns(achievePayPctPreTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AchPayTransHeldBack")).Returns(achPayTransHeldBack);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualDaysIL")).Returns(actualDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualNumInstalm")).Returns(actualNumInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualNumInstalmPreTrans")).Returns(actualNumInstalmPreTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ActualNumInstalmTrans")).Returns(actualNumInstalmTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AdjLearnStartDate")).Returns(adjLearnStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AdltLearnResp")).Returns(adltLearnResp);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "AgeAimStart")).Returns(ageAimStart);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AimValue")).Returns(aimValue);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AppAdjLearnStartDate")).Returns(appAdjLearnStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AppAgeFact")).Returns(appAgeFact);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AppATAGTA")).Returns(appATAGTA);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AppCompetency")).Returns(appCompetency);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AppFuncSkill")).Returns(appFuncSkill);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AppFuncSkill1618AdjFact")).Returns(appFuncSkill1618AdjFact);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AppKnowl")).Returns(appKnowl);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "AppLearnStartDate")).Returns(appLearnStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "ApplicEmpFactDate")).Returns(applicEmpFactDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "ApplicFactDate")).Returns(applicFactDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "ApplicFundRateDate")).Returns(applicFundRateDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "ApplicProgWeightFact")).Returns(applicProgWeightFact);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "ApplicUnweightFundRate")).Returns(applicUnweightFundRate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "ApplicWeightFundRate")).Returns(applicWeightFundRate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "AppNonFund")).Returns(appNonFund);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "AreaCostFactAdj")).Returns(areaCostFactAdj);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "BalInstalmPreTrans")).Returns(balInstalmPreTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "BaseValueUnweight")).Returns(baseValueUnweight);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "CapFactor")).Returns(capFactor);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "DisUpFactAdj")).Returns(disUpFactAdj);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "EmpOutcomePayElig")).Returns(empOutcomePayElig);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "EmpOutcomePctHeldBackTrans")).Returns(empOutcomePctHeldBackTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "EmpOutcomePctPreTrans")).Returns(empOutcomePctPreTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "EmpRespOth")).Returns(empRespOth);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "ESOL")).Returns(eSOL);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "FullyFund")).Returns(fullyFund);
            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "FundLine")).Returns(fundLine);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "FundStart")).Returns(fundStart);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LargeEmployerFM35Fctr")).Returns(largeEmployerFM35Fctr);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "LargeEmployerID")).Returns(largeEmployerID);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "LargeEmployerStatusDate")).Returns(largeEmployerStatusDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LTRCUpliftFctr")).Returns(lTRCUpliftFctr);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "NonGovCont")).Returns(nonGovCont);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "OLASSCustody")).Returns(oLASSCustody);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "OnProgPayPctPreTrans")).Returns(onProgPayPctPreTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "OutstndNumOnProgInstalm")).Returns(outstndNumOnProgInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "OutstndNumOnProgInstalmTrans")).Returns(outstndNumOnProgInstalmTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedNumOnProgInstalm")).Returns(plannedNumOnProgInstalm);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedNumOnProgInstalmTrans")).Returns(plannedNumOnProgInstalmTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedTotalDaysIL")).Returns(plannedTotalDaysIL);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "PlannedTotalDaysILPreTrans")).Returns(plannedTotalDaysILPreTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PropFundRemain")).Returns(propFundRemain);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "PropFundRemainAch")).Returns(propFundRemainAch);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "PrscHEAim")).Returns(prscHEAim);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Residential")).Returns(residential);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Restart")).Returns(restart);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "SpecResUplift")).Returns(specResUplift);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "StartPropTrans")).Returns(startPropTrans);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "ThresholdDays")).Returns(thresholdDays);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Traineeship")).Returns(traineeship);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "Trans")).Returns(trans);
            dataEntityAttributeServiceMock.Setup(s => s.GetDateTimeAttributeValue(dataEntity, "TrnAdjLearnStartDate")).Returns(trnAdjLearnStartDate);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "TrnWorkPlaceAim")).Returns(trnWorkPlaceAim);
            dataEntityAttributeServiceMock.Setup(s => s.GetBoolAttributeValue(dataEntity, "TrnWorkPrepAim")).Returns(trnWorkPrepAim);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "UnWeightedRateFromESOL")).Returns(unWeightedRateFromESOL);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "UnweightedRateFromLARS")).Returns(unweightedRateFromLARS);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "WeightedRateFromESOL")).Returns(weightedRateFromESOL);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "WeightedRateFromLARS")).Returns(weightedRateFromLARS);

            var learningDelivery = NewService(dataEntityAttributeService: dataEntityAttributeServiceMock.Object).LearningDeliveryValue(dataEntity);

            learningDelivery.AchApplicDate.Should().Be(achApplicDate);
            learningDelivery.Achieved.Should().Be(achieved);
            learningDelivery.AchieveElement.Should().Be(achieveElement);
            learningDelivery.AchievePayElig.Should().Be(achievePayElig);
            learningDelivery.AchievePayPctPreTrans.Should().Be(achievePayPctPreTrans);
            learningDelivery.AchPayTransHeldBack.Should().Be(achPayTransHeldBack);
            learningDelivery.ActualDaysIL.Should().Be(actualDaysIL);
            learningDelivery.ActualNumInstalm.Should().Be(actualNumInstalm);
            learningDelivery.ActualNumInstalmPreTrans.Should().Be(actualNumInstalmPreTrans);
            learningDelivery.ActualNumInstalmTrans.Should().Be(actualNumInstalmTrans);
            learningDelivery.AdjLearnStartDate.Should().Be(adjLearnStartDate);
            learningDelivery.AdltLearnResp.Should().Be(adltLearnResp);
            learningDelivery.AgeAimStart.Should().Be(ageAimStart);
            learningDelivery.AimValue.Should().Be(aimValue);
            learningDelivery.AppAdjLearnStartDate.Should().Be(appAdjLearnStartDate);
            learningDelivery.AppAgeFact.Should().Be(appAgeFact);
            learningDelivery.AppATAGTA.Should().Be(appATAGTA);
            learningDelivery.AppCompetency.Should().Be(appCompetency);
            learningDelivery.AppFuncSkill.Should().Be(appFuncSkill);
            learningDelivery.AppFuncSkill1618AdjFact.Should().Be(appFuncSkill1618AdjFact);
            learningDelivery.AppKnowl.Should().Be(appKnowl);
            learningDelivery.AppLearnStartDate.Should().Be(appLearnStartDate);
            learningDelivery.ApplicEmpFactDate.Should().Be(applicEmpFactDate);
            learningDelivery.ApplicFactDate.Should().Be(applicFactDate);
            learningDelivery.ApplicFundRateDate.Should().Be(applicFundRateDate);
            learningDelivery.ApplicProgWeightFact.Should().Be(applicProgWeightFact);
            learningDelivery.ApplicUnweightFundRate.Should().Be(applicUnweightFundRate);
            learningDelivery.ApplicWeightFundRate.Should().Be(applicWeightFundRate);
            learningDelivery.AppNonFund.Should().Be(appNonFund);
            learningDelivery.AreaCostFactAdj.Should().Be(areaCostFactAdj);
            learningDelivery.BalInstalmPreTrans.Should().Be(balInstalmPreTrans);
            learningDelivery.BaseValueUnweight.Should().Be(baseValueUnweight);
            learningDelivery.CapFactor.Should().Be(capFactor);
            learningDelivery.DisUpFactAdj.Should().Be(disUpFactAdj);
            learningDelivery.EmpOutcomePayElig.Should().Be(empOutcomePayElig);
            learningDelivery.EmpOutcomePctHeldBackTrans.Should().Be(empOutcomePctHeldBackTrans);
            learningDelivery.EmpOutcomePctPreTrans.Should().Be(empOutcomePctPreTrans);
            learningDelivery.EmpRespOth.Should().Be(empRespOth);
            learningDelivery.ESOL.Should().Be(eSOL);
            learningDelivery.FullyFund.Should().Be(fullyFund);
            learningDelivery.FundLine.Should().Be(fundLine);
            learningDelivery.FundStart.Should().Be(fundStart);
            learningDelivery.LargeEmployerFM35Fctr.Should().Be(largeEmployerFM35Fctr);
            learningDelivery.LargeEmployerID.Should().Be(largeEmployerID);
            learningDelivery.LargeEmployerStatusDate.Should().Be(largeEmployerStatusDate);
            learningDelivery.LTRCUpliftFctr.Should().Be(lTRCUpliftFctr);
            learningDelivery.NonGovCont.Should().Be(nonGovCont);
            learningDelivery.OLASSCustody.Should().Be(oLASSCustody);
            learningDelivery.OnProgPayPctPreTrans.Should().Be(onProgPayPctPreTrans);
            learningDelivery.OutstndNumOnProgInstalm.Should().Be(outstndNumOnProgInstalm);
            learningDelivery.OutstndNumOnProgInstalmTrans.Should().Be(outstndNumOnProgInstalmTrans);
            learningDelivery.PlannedNumOnProgInstalm.Should().Be(plannedNumOnProgInstalm);
            learningDelivery.PlannedNumOnProgInstalmTrans.Should().Be(plannedNumOnProgInstalmTrans);
            learningDelivery.PlannedTotalDaysIL.Should().Be(plannedTotalDaysIL);
            learningDelivery.PlannedTotalDaysILPreTrans.Should().Be(plannedTotalDaysILPreTrans);
            learningDelivery.PropFundRemain.Should().Be(propFundRemain);
            learningDelivery.PropFundRemainAch.Should().Be(propFundRemainAch);
            learningDelivery.PrscHEAim.Should().Be(prscHEAim);
            learningDelivery.Residential.Should().Be(residential);
            learningDelivery.Restart.Should().Be(restart);
            learningDelivery.SpecResUplift.Should().Be(specResUplift);
            learningDelivery.StartPropTrans.Should().Be(startPropTrans);
            learningDelivery.ThresholdDays.Should().Be(thresholdDays);
            learningDelivery.Traineeship.Should().Be(traineeship);
            learningDelivery.Trans.Should().Be(trans);
            learningDelivery.TrnAdjLearnStartDate.Should().Be(trnAdjLearnStartDate);
            learningDelivery.TrnWorkPlaceAim.Should().Be(trnWorkPlaceAim);
            learningDelivery.TrnWorkPrepAim.Should().Be(trnWorkPrepAim);
            learningDelivery.UnWeightedRateFromESOL.Should().Be(unWeightedRateFromESOL);
            learningDelivery.UnweightedRateFromLARS.Should().Be(unweightedRateFromLARS);
            learningDelivery.WeightedRateFromESOL.Should().Be(weightedRateFromESOL);
            learningDelivery.WeightedRateFromLARS.Should().Be(weightedRateFromLARS);
        }

        [Fact]
        public void FundingOutput_LearningDeliveryPeriodisedAttributeData_Correct()
        {
            // ARRANGE
            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(It.IsAny<object>())).Returns(1.0m);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValueForPeriod(It.IsAny<IAttributeData>(), It.IsAny<DateTime>())).Returns(1.0m);

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
                    { "AimSeqNumber", Attribute(false, "1.0") },
                    { "AchApplicDate", Attribute(false, new Date(new DateTime(2018, 09, 01))) },
                    { "Achieved", Attribute(false, "1.0") },
                    { "AchieveElement", Attribute(false, "1.0") },
                    { "AchievePayElig", Attribute(false, "1.0") },
                    { "AchievePayPctPreTrans", Attribute(false, "1.0") },
                    { "AchPayTransHeldBack", Attribute(false, "1.0") },
                    { "ActualDaysIL", Attribute(false, "1.0") },
                    { "ActualNumInstalm", Attribute(false, "1.0") },
                    { "ActualNumInstalmPreTrans", Attribute(false, "1.0") },
                    { "ActualNumInstalmTrans", Attribute(false, "1.0") },
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
                    { "AreaCostFactAdj", Attribute(false, "1.0") },
                    { "BalInstalmPreTrans", Attribute(false, "1.0") },
                    { "BaseValueUnweight", Attribute(false, "1.0") },
                    { "CapFactor", Attribute(false, "1.0") },
                    { "DisUpFactAdj", Attribute(false, "1.0") },
                    { "EmpOutcomePayElig", Attribute(false, "1.0") },
                    { "EmpOutcomePctHeldBackTrans", Attribute(false, "1.0") },
                    { "EmpOutcomePctPreTrans", Attribute(false, "1.0") },
                    { "EmpRespOth", Attribute(false, "1.0") },
                    { "ESOL", Attribute(false, "1.0") },
                    { "FullyFund", Attribute(false, "1.0") },
                    { "FundLine", Attribute(false, "1.0") },
                    { "FundStart", Attribute(false, "1.0") },
                    { "LargeEmployerFM35Fctr", Attribute(false, "1.0") },
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
                    { "PlannedTotalDaysILPreTrans", Attribute(false, "1.0") },
                    { "PropFundRemain", Attribute(false, "1.0") },
                    { "PropFundRemainAch", Attribute(false, "1.0") },
                    { "PrscHEAim", Attribute(false, "1.0") },
                    { "Residential", Attribute(false, "1.0") },
                    { "Restart", Attribute(false, "1.0") },
                    { "SpecResUplift", Attribute(false, "1.0") },
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
                    { "AchievePayElig", Attribute(false, "1.0") },
                    { "AchievePayPctPreTrans", Attribute(false, "1.0") },
                    { "AchPayTransHeldBack", Attribute(false, "1.0") },
                    { "ActualDaysIL", Attribute(false, "1.0") },
                    { "ActualNumInstalm", Attribute(false, "1.0") },
                    { "ActualNumInstalmPreTrans", Attribute(false, "1.0") },
                    { "ActualNumInstalmTrans", Attribute(false, "1.0") },
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
                    { "DisUpFactAdj", Attribute(false, "1.0") },
                    { "EmpOutcomePayElig", Attribute(false, "1.0") },
                    { "EmpOutcomePctHeldBackTrans", Attribute(false, "1.0") },
                    { "EmpOutcomePctPreTrans", Attribute(false, "1.0") },
                    { "EmpRespOth", Attribute(false, "1.0") },
                    { "ESOL", Attribute(false, "1.0") },
                    { "FullyFund", Attribute(false, "1.0") },
                    { "FundLine", Attribute(false, "1.0") },
                    { "FundStart", Attribute(false, "1.0") },
                    { "LargeEmployerFM35Fctr", Attribute(false, "1.0") },
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
                    { "PlannedTotalDaysILPreTrans", Attribute(false, "1.0") },
                    { "PropFundRemain", Attribute(false, "1.0") },
                    { "PropFundRemainAch", Attribute(false, "1.0") },
                    { "PrscHEAim", Attribute(false, "1.0") },
                    { "Residential", Attribute(false, "1.0") },
                    { "Restart", Attribute(false, "1.0") },
                    { "SpecResUplift", Attribute(false, "1.0") },
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
                    { "LearnSuppFund", Attribute(true, "1.0") },
                    { "LearnSuppFundCash", Attribute(true, "1.0") },
                    { "OnProgPayment", Attribute(true, "1.0") },
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

        private List<LearningDeliveryPeriodisedValue> TestLearningDeliveryPeriodisedAttributesDataArray()
        {
            return new List<LearningDeliveryPeriodisedValue>
            {
                TestLearningDeliveryPeriodisedAttributesData("AchievePayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("AchievePayPct", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("AchievePayPctTrans", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("BalancePayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("BalancePaymentUncapped", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("BalancePct", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("BalancePctTrans", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("EmpOutcomePay", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("EmpOutcomePct", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("EmpOutcomePctTrans", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("InstPerPeriod", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnSuppFund", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("LearnSuppFundCash", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("OnProgPayment", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("OnProgPaymentUncapped", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("OnProgPayPct", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("OnProgPayPctTrans", 1.0m),
                TestLearningDeliveryPeriodisedAttributesData("TransInstPerPeriod", 1.0m),
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

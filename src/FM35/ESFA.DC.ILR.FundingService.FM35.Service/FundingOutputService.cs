using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM35.Service.Constants;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.Service
{
    public class FundingOutputService : IOutputService<FM35Global>
    {
        private readonly IInternalDataCache _internalDataCache;
        private readonly IDataEntityAttributeService _dataEntityAttributeService;

        public FundingOutputService(IInternalDataCache internalDataCache, IDataEntityAttributeService dataEntityAttributeService)
        {
            _internalDataCache = internalDataCache;
            _dataEntityAttributeService = dataEntityAttributeService;
        }

        public FM35Global ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            var globals = dataEntities.Select(MapGlobal);

            return new FM35Global
            {
                LARSVersion = globals.FirstOrDefault().LARSVersion,
                RulebaseVersion = globals.FirstOrDefault().RulebaseVersion,
                PostcodeDisadvantageVersion = globals.FirstOrDefault().PostcodeDisadvantageVersion,
                OrgVersion = globals.FirstOrDefault().OrgVersion,
                CurFundYr = globals.FirstOrDefault().CurFundYr,
                UKPRN = globals.FirstOrDefault().UKPRN,
                Learners = globals.SelectMany(l => l.Learners).ToList()
            };
        }

        public FM35Global MapGlobal(IDataEntity dataEntity)
        {
            return new FM35Global
            {
                UKPRN = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.UKPRN).Value,
                CurFundYr = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.CurFundYr),
                LARSVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LARSVersion),
                OrgVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.OrgVersion),
                PostcodeDisadvantageVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.PostcodeDisadvantageVersion),
                RulebaseVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.RulebaseVersion),
                Learners = dataEntity
                    .Children?
                    .Where(c => c.EntityName == Attributes.EntityLearner)
                    .Select(MapLearner)
                    .ToList()
            };
        }

        public FM35Learner MapLearner(IDataEntity dataEntity)
        {
            return new FM35Learner()
                {
                    LearnRefNumber = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LearnRefNumber),
                    LearningDeliveries = dataEntity
                        .Children
                        .Where(e => e.EntityName == Attributes.EntityLearningDelivery)
                        .Select(LearningDeliveryFromDataEntity).ToList()
                };
        }

        public LearningDelivery LearningDeliveryFromDataEntity(IDataEntity dataEntity)
        {
            return new LearningDelivery
            {
                    AimSeqNumber = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.AimSeqNumber),
                    LearningDeliveryValue = LearningDeliveryValue(dataEntity),
                    LearningDeliveryPeriodisedValues = LearningDeliveryPeriodisedValues(dataEntity),
                };
        }

        public LearningDeliveryValue LearningDeliveryValue(IDataEntity dataEntity)
        {
            return new LearningDeliveryValue
            {
                AchApplicDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.AchApplicDate),
                Achieved = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.Achieved),
                AchieveElement = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AchieveElement),
                AchievePayElig = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.AchievePayElig),
                AchievePayPctPreTrans = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AchievePayPctPreTrans),
                AchPayTransHeldBack = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AchPayTransHeldBack),
                ActualDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ActualDaysIL),
                ActualNumInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ActualNumInstalm),
                ActualNumInstalmPreTrans = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ActualNumInstalmPreTrans),
                ActualNumInstalmTrans = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ActualNumInstalmTrans),
                AdjLearnStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.AdjLearnStartDate),
                AdltLearnResp = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.AdltLearnResp),
                AgeAimStart = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.AgeAimStart),
                AimValue = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AimValue),
                AppAdjLearnStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.AppAdjLearnStartDate),
                AppAgeFact = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AppAgeFact),
                AppATAGTA = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.AppATAGTA),
                AppCompetency = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.AppCompetency),
                AppFuncSkill = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.AppFuncSkill),
                AppFuncSkill1618AdjFact = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AppFuncSkill1618AdjFact),
                AppKnowl = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.AppKnowl),
                AppLearnStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.AppLearnStartDate),
                ApplicEmpFactDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.ApplicEmpFactDate),
                ApplicFactDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.ApplicFactDate),
                ApplicFundRateDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.ApplicFundRateDate),
                ApplicProgWeightFact = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.ApplicProgWeightFact),
                ApplicUnweightFundRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.ApplicUnweightFundRate),
                ApplicWeightFundRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.ApplicWeightFundRate),
                AppNonFund = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.AppNonFund),
                AreaCostFactAdj = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AreaCostFactAdj),
                BalInstalmPreTrans = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.BalInstalmPreTrans),
                BaseValueUnweight = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.BaseValueUnweight),
                CapFactor = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.CapFactor),
                DisUpFactAdj = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.DisUpFactAdj),
                EmpOutcomePayElig = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.EmpOutcomePayElig),
                EmpOutcomePctHeldBackTrans = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.EmpOutcomePctHeldBackTrans),
                EmpOutcomePctPreTrans = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.EmpOutcomePctPreTrans),
                EmpRespOth = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.EmpRespOth),
                ESOL = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.ESOL),
                FullyFund = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.FullyFund),
                FundLine = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.FundLine),
                FundStart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.FundStart),
                LargeEmployerFM35Fctr = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LargeEmployerFM35Fctr),
                LargeEmployerID = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.LargeEmployerID),
                LargeEmployerStatusDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.LargeEmployerStatusDate),
                LTRCUpliftFctr = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LTRCUpliftFctr),
                NonGovCont = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.NonGovCont),
                OLASSCustody = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.OLASSCustody),
                OnProgPayPctPreTrans = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.OnProgPayPctPreTrans),
                OutstndNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.OutstndNumOnProgInstalm),
                OutstndNumOnProgInstalmTrans = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.OutstndNumOnProgInstalmTrans),
                PlannedNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PlannedNumOnProgInstalm),
                PlannedNumOnProgInstalmTrans = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PlannedNumOnProgInstalmTrans),
                PlannedTotalDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PlannedTotalDaysIL),
                PlannedTotalDaysILPreTrans = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PlannedTotalDaysILPreTrans),
                PropFundRemain = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PropFundRemain),
                PropFundRemainAch = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.PropFundRemainAch),
                PrscHEAim = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.PrscHEAim),
                Residential = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.Residential),
                Restart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.Restart),
                SpecResUplift = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.SpecResUplift),
                StartPropTrans = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.StartPropTrans),
                ThresholdDays = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ThresholdDays),
                Traineeship = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.Traineeship),
                Trans = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.Trans),
                TrnAdjLearnStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.TrnAdjLearnStartDate),
                TrnWorkPlaceAim = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.TrnWorkPlaceAim),
                TrnWorkPrepAim = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.TrnWorkPrepAim),
                UnWeightedRateFromESOL = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.UnWeightedRateFromESOL),
                UnweightedRateFromLARS = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.UnweightedRateFromLARS),
                WeightedRateFromESOL = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.WeightedRateFromESOL),
                WeightedRateFromLARS = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.WeightedRateFromLARS),
            };
        }

        protected internal List<LearningDeliveryPeriodisedValue> LearningDeliveryPeriodisedValues(IDataEntity learningDelivery)
        {
            List<string> attributeList = new List<string>()
            {
                Attributes.AchievePayment,
                Attributes.AchievePayPct,
                Attributes.AchievePayPctTrans,
                Attributes.BalancePayment,
                Attributes.BalancePaymentUncapped,
                Attributes.BalancePct,
                Attributes.BalancePctTrans,
                Attributes.EmpOutcomePay,
                Attributes.EmpOutcomePct,
                Attributes.EmpOutcomePctTrans,
                Attributes.InstPerPeriod,
                Attributes.LearnSuppFund,
                Attributes.LearnSuppFundCash,
                Attributes.OnProgPayment,
                Attributes.OnProgPaymentUncapped,
                Attributes.OnProgPayPct,
                Attributes.OnProgPayPctTrans,
                Attributes.TransInstPerPeriod
            };

            List<LearningDeliveryPeriodisedValue> learningDeliveryPeriodisedAttributesList = new List<LearningDeliveryPeriodisedValue>();

            foreach (var attribute in attributeList)
            {
                var attributeValue = learningDelivery.Attributes[attribute];

                var changePoints = attributeValue.Changepoints;

                if (!changePoints.Any())
                {
                    var value = _dataEntityAttributeService.GetDecimalAttributeValue(attributeValue.Value);

                    learningDeliveryPeriodisedAttributesList.Add(new LearningDeliveryPeriodisedValue
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
                    });
                }

                if (changePoints.Any())
                {
                    learningDeliveryPeriodisedAttributesList.Add(new LearningDeliveryPeriodisedValue
                    {
                        AttributeName = attribute,
                        Period1 = _dataEntityAttributeService.GetDecimalAttributeValueForPeriod(attributeValue, _internalDataCache.Period1),
                        Period2 = _dataEntityAttributeService.GetDecimalAttributeValueForPeriod(attributeValue, _internalDataCache.Period2),
                        Period3 = _dataEntityAttributeService.GetDecimalAttributeValueForPeriod(attributeValue, _internalDataCache.Period3),
                        Period4 = _dataEntityAttributeService.GetDecimalAttributeValueForPeriod(attributeValue, _internalDataCache.Period4),
                        Period5 = _dataEntityAttributeService.GetDecimalAttributeValueForPeriod(attributeValue, _internalDataCache.Period5),
                        Period6 = _dataEntityAttributeService.GetDecimalAttributeValueForPeriod(attributeValue, _internalDataCache.Period6),
                        Period7 = _dataEntityAttributeService.GetDecimalAttributeValueForPeriod(attributeValue, _internalDataCache.Period7),
                        Period8 = _dataEntityAttributeService.GetDecimalAttributeValueForPeriod(attributeValue, _internalDataCache.Period8),
                        Period9 = _dataEntityAttributeService.GetDecimalAttributeValueForPeriod(attributeValue, _internalDataCache.Period9),
                        Period10 = _dataEntityAttributeService.GetDecimalAttributeValueForPeriod(attributeValue, _internalDataCache.Period10),
                        Period11 = _dataEntityAttributeService.GetDecimalAttributeValueForPeriod(attributeValue, _internalDataCache.Period11),
                        Period12 = _dataEntityAttributeService.GetDecimalAttributeValueForPeriod(attributeValue, _internalDataCache.Period12),
                    });
                }
            }

            return learningDeliveryPeriodisedAttributesList;
        }
    }
}

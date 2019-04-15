using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM81.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM81.Service.Constants;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM81.Service
{
    public class FundingOutputService : IOutputService<FM81Global>
    {
        private readonly IInternalDataCache _internalDataCache;
        private readonly IDataEntityAttributeService _dataEntityAttributeService;

        public FundingOutputService(IInternalDataCache internalDataCache, IDataEntityAttributeService dataEntityAttributeService)
        {
            _internalDataCache = internalDataCache;
            _dataEntityAttributeService = dataEntityAttributeService;
        }

        public FM81Global ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            var globals = dataEntities.Select(MapGlobal);

            return new FM81Global
            {
                LARSVersion = globals.FirstOrDefault().LARSVersion,
                RulebaseVersion = globals.FirstOrDefault().RulebaseVersion,
                CurFundYr = globals.FirstOrDefault().CurFundYr,
                UKPRN = globals.FirstOrDefault().UKPRN,
                Learners = globals.SelectMany(l => l.Learners).ToList()
            };
        }

        public FM81Global MapGlobal(IDataEntity dataEntity)
        {
            return new FM81Global
            {
                UKPRN = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.UKPRN).Value,
                LARSVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LARSVersion),
                RulebaseVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.RulebaseVersion),
                CurFundYr = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.CurFundYr),
                Learners = dataEntity
                    .Children?
                    .Where(c => c.EntityName == Attributes.EntityLearner)
                    .Select(MapLearner)
                    .ToList()
            };
        }

        public FM81Learner MapLearner(IDataEntity dataEntity)
        {
            return new FM81Learner()
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
                AimSeqNumber = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.AimSeqNumber).Value,
                LearningDeliveryValues = LearningDeliveryAttributeData(dataEntity),
                LearningDeliveryPeriodisedValues = LearningDeliveryPeriodisedValues(dataEntity),
            };
        }

        public LearningDeliveryValue LearningDeliveryAttributeData(IDataEntity dataEntity)
        {
            return new LearningDeliveryValue
            {
                AchApplicDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.AchApplicDate),
                AchEligible = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.AchEligible),
                Achieved = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.Achieved),
                AchievementApplicVal = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AchievementApplicVal),
                AchPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AchPayment),
                ActualDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ActualDaysIL),
                ActualNumInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ActualNumInstalm),
                AdjProgStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.AdjProgStartDate),
                AgeStandardStart = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.AgeStandardStart),
                ApplicFundValDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.ApplicFundValDate),
                CombinedAdjProp = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.CombinedAdjProp),
                CoreGovContCapApplicVal = _dataEntityAttributeService.GetLongAttributeValue(dataEntity, Attributes.CoreGovContCapApplicVal),
                CoreGovContPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.CoreGovContPayment),
                CoreGovContUncapped = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.CoreGovContUncapped),
                EmpIdAchDate = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.EmpIdAchDate),
                EmpIdFirstDayStandard = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.EmpIdFirstDayStandard),
                EmpIdFirstYoungAppDate = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.EmpIdFirstYoungAppDate),
                EmpIdSecondYoungAppDate = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.EmpIdSecondYoungAppDate),
                EmpIdSmallBusDate = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.EmpIdSmallBusDate),
                FundLine = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.FundLine),
                InstPerPeriod = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.InstPerPeriod),
                LearnDelDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.LearnDelDaysIL),
                LearnDelStandardAccDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.LearnDelStandardAccDaysIL),
                LearnDelStandardPrevAccDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.LearnDelStandardPrevAccDaysIL),
                LearnDelStandardTotalDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.LearnDelStandardTotalDaysIL),
                LearnSuppFund = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.LearnSuppFund),
                LearnSuppFundCash = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnSuppFundCash),
                MathEngAimValue = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.MathEngAimValue),
                MathEngBalPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.MathEngBalPayment),
                MathEngBalPct = _dataEntityAttributeService.GetLongAttributeValue(dataEntity, Attributes.MathEngBalPct),
                MathEngLSFFundStart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.MathEngLSFFundStart),
                MathEngLSFThresholdDays = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.MathEngLSFThresholdDays),
                MathEngOnProgPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.MathEngOnProgPayment),
                MathEngOnProgPct = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.MathEngOnProgPct),
                OutstandNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.OutstandNumOnProgInstalm),
                PlannedNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PlannedNumOnProgInstalm),
                PlannedTotalDaysIL = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PlannedTotalDaysIL),
                ProgStandardStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.ProgStandardStartDate),
                SmallBusApplicVal = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.SmallBusApplicVal),
                SmallBusEligible = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.SmallBusEligible),
                SmallBusPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.SmallBusPayment),
                SmallBusStatusFirstDayStandard = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.SmallBusStatusFirstDayStandard),
                SmallBusStatusThreshold = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.SmallBusStatusThreshold),
                YoungAppApplicVal = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.YoungAppApplicVal),
                YoungAppEligible = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.YoungAppEligible),
                YoungAppFirstPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.YoungAppFirstPayment),
                YoungAppFirstThresholdDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.YoungAppFirstThresholdDate),
                YoungAppPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.YoungAppPayment),
                YoungAppSecondPayment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.YoungAppSecondPayment),
                YoungAppSecondThresholdDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.YoungAppSecondThresholdDate)
            };
        }

        public List<LearningDeliveryPeriodisedValue> LearningDeliveryPeriodisedValues(IDataEntity learningDelivery)
        {
            List<string> attributeList = new List<string>()
            {
                Attributes.AchPayment,
                Attributes.CoreGovContPayment,
                Attributes.CoreGovContUncapped,
                Attributes.InstPerPeriod,
                Attributes.LearnSuppFund,
                Attributes.LearnSuppFundCash,
                Attributes.MathEngBalPayment,
                Attributes.MathEngBalPct,
                Attributes.MathEngOnProgPayment,
                Attributes.MathEngOnProgPct,
                Attributes.SmallBusPayment,
                Attributes.YoungAppFirstPayment,
                Attributes.YoungAppPayment,
                Attributes.YoungAppSecondPayment,
            };

            List<LearningDeliveryPeriodisedValue> learningDeliveryPeriodisedAttributeList = new List<LearningDeliveryPeriodisedValue>();

            foreach (var attribute in attributeList)
            {
                var attributeValue = learningDelivery.Attributes[attribute];

                var changePoints = attributeValue.Changepoints;

                if (!changePoints.Any())
                {
                    var value = _dataEntityAttributeService.GetDecimalAttributeValue(attributeValue.Value);

                    learningDeliveryPeriodisedAttributeList.Add(new LearningDeliveryPeriodisedValue
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
                    learningDeliveryPeriodisedAttributeList.Add(new LearningDeliveryPeriodisedValue
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

            return learningDeliveryPeriodisedAttributeList;
        }
    }
}

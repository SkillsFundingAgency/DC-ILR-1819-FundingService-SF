using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.ALB.Service.Constants;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.Service
{
    public class FundingOutputService : IOutputService<ALBGlobal>
    {
        private readonly IInternalDataCache _internalDataCache;
        private readonly IDataEntityAttributeService _dataEntityAttributeService;

        public FundingOutputService(IInternalDataCache internalDataCache, IDataEntityAttributeService dataEntityAttributeService)
        {
            _internalDataCache = internalDataCache;
            _dataEntityAttributeService = dataEntityAttributeService;
        }

        public ALBGlobal ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            var globals = dataEntities.Select(MapGlobal);

            return new ALBGlobal
            {
                LARSVersion = globals.FirstOrDefault().LARSVersion,
                PostcodeAreaCostVersion = globals.FirstOrDefault().PostcodeAreaCostVersion,
                RulebaseVersion = globals.FirstOrDefault().RulebaseVersion,
                UKPRN = globals.FirstOrDefault().UKPRN,
                Learners = globals.SelectMany(l => l.Learners).ToList()
            };
        }

        public ALBGlobal MapGlobal(IDataEntity dataEntity)
        {
            return new ALBGlobal
            {
                UKPRN = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.UKPRN).Value,
                PostcodeAreaCostVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.PostcodeAreaCostVersion),
                LARSVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LARSVersion),
                RulebaseVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.RulebaseVersion),
                Learners = dataEntity
                    .Children?
                    .Where(c => c.EntityName == Attributes.EntityLearner)
                    .Select(MapLearner)
                    .ToList()
            };
        }

        public ALBLearner MapLearner(IDataEntity dataEntity)
        {
            return new ALBLearner()
            {
                LearnRefNumber = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LearnRefNumber),
                LearnerPeriodisedValues = LearnerPeriodisedValues(dataEntity),
                LearningDeliveries = dataEntity
                        .Children
                        .Where(e => e.EntityName == Attributes.EntityLearningDelivery)
                        .Select(LearningDeliveryFromDataEntity).ToList()
            };
        }

        public List<LearnerPeriodisedValue> LearnerPeriodisedValues(IDataEntity learner)
        {
            List<string> attributeList = new List<string>()
            {
                Attributes.ALBSeqNum,
            };

            List<LearnerPeriodisedValue> learnerPeriodisedAttributesList = new List<LearnerPeriodisedValue>();

            foreach (var attribute in attributeList)
            {
                var attributeValue = learner.Attributes[attribute];

                var changePoints = attributeValue.Changepoints;

                if (!changePoints.Any())
                {
                    var value = _dataEntityAttributeService.GetDecimalAttributeValue(attributeValue.Value);

                    learnerPeriodisedAttributesList.Add(new LearnerPeriodisedValue
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
                    learnerPeriodisedAttributesList.Add(new LearnerPeriodisedValue
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

            return learnerPeriodisedAttributesList;
        }

        public LearningDelivery LearningDeliveryFromDataEntity(IDataEntity dataEntity)
        {
            return new LearningDelivery
            {
                AimSeqNumber = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.AimSeqNumber).Value,
                LearningDeliveryValue = LearningDeliveryValue(dataEntity),
                LearningDeliveryPeriodisedValues = LearningDeliveryPeriodisedValues(dataEntity),
            };
        }

        public LearningDeliveryValue LearningDeliveryValue(IDataEntity dataEntity)
        {
            return new LearningDeliveryValue
            {
                Achieved = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.Achieved),
                ActualNumInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.ActualNumInstalm),
                AdvLoan = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.AdvLoan),
                ApplicFactDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.ApplicFactDate),
                ApplicProgWeightFact = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.ApplicProgWeightFact),
                AreaCostFactAdj = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AreaCostFactAdj),
                AreaCostInstalment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AreaCostInstalment),
                FundLine = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.FundLine),
                FundStart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.FundStart),
                LearnDelApplicLARSCarPilFundSubRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelApplicLARSCarPilFundSubRate),
                LearnDelApplicSubsidyPilotAreaCode = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LearnDelApplicSubsidyPilotAreaCode),
                LearnDelCarLearnPilotAimValue = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelCarLearnPilotAimValue),
                LearnDelCarLearnPilotInstalAmount = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LearnDelCarLearnPilotInstalAmount),
                LearnDelEligCareerLearnPilot = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.LearnDelEligCareerLearnPilot),
                LiabilityDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.LiabilityDate),
                LoanBursAreaUplift = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.LoanBursAreaUplift),
                LoanBursSupp = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.LoanBursSupp),
                OutstndNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.OutstndNumOnProgInstalm),
                PlannedNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.PlannedNumOnProgInstalm),
                WeightedRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.WeightedRate),
            };
        }

        public List<LearningDeliveryPeriodisedValue> LearningDeliveryPeriodisedValues(IDataEntity learningDelivery)
        {
            List<string> attributeList = new List<string>()
            {
                Attributes.ALBCode,
                Attributes.ALBSupportPayment,
                Attributes.AreaUpliftBalPayment,
                Attributes.AreaUpliftOnProgPayment,
                Attributes.LearnDelCarLearnPilotOnProgPayment,
                Attributes.LearnDelCarLearnPilotBalPayment
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

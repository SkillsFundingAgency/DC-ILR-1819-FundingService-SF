using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM70.Service.Constants;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM70.Service
{
    public class FundingOutputService : IOutputService<FM70Global>
    {
        private readonly IInternalDataCache _internalDataCache;
        private readonly IDataEntityAttributeService _dataEntityAttributeService;

        public FundingOutputService(IInternalDataCache internalDataCache, IDataEntityAttributeService dataEntityAttributeService)
        {
            _internalDataCache = internalDataCache;
            _dataEntityAttributeService = dataEntityAttributeService;
        }

        public FM70Global ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            var globals = dataEntities.Select(MapGlobal);

            return new FM70Global
            {
                RulebaseVersion = globals.FirstOrDefault().RulebaseVersion,
                UKPRN = globals.FirstOrDefault().UKPRN,
                Learners = globals.SelectMany(l => l.Learners).ToList()
            };
        }

        public FM70Global MapGlobal(IDataEntity dataEntity)
        {
            return new FM70Global
            {
                UKPRN = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.UKPRN).Value,
                RulebaseVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.RulebaseVersion),
                Learners = dataEntity
                    .Children?
                    .Where(c => c.EntityName == Attributes.EntityLearner)
                    .Select(MapLearner)
                    .ToList()
            };
        }

        public FM70Learner MapLearner(IDataEntity dataEntity)
        {
            return new FM70Learner()
                {
                    LearnRefNumber = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LearnRefNumber),
                    LearningDeliveries = dataEntity
                        .Children
                        .Where(e => e.EntityName == Attributes.EntityLearningDelivery)
                        .Select(LearningDeliveryFromDataEntity).ToList(),
                    LearnerDPOutcomes = dataEntity
                        .Children
                        .Where(e => e.EntityName == Attributes.EntityDPOutcome)
                        .Select(DPOutcomesFromDataEntity).ToList()
            };
        }

        public LearnerDPOutcome DPOutcomesFromDataEntity(IDataEntity dataEntity)
        {
            return new LearnerDPOutcome
            {
                OutCode = (int)_dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.OutCode),
                OutType = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.OutType),
                OutStartDate = (DateTime)_dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.OutStartDate),
                OutcomeDateForProgression = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.OutcomeDateForProgression),
                PotentialESFProgressionType = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.PotentialESFProgressionType),
                ProgressionType = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.ProgressionType),
                ReachedSixMonthPoint = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.ReachedSixMonthPoint),
                ReachedThreeMonthPoint = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.ReachedThreeMonthPoint),
                ReachedTwelveMonthPoint = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.ReachedTwelveMonthPoint),
            };
        }

        public LearningDelivery LearningDeliveryFromDataEntity(IDataEntity dataEntity)
        {
            return new LearningDelivery
            {
                    AimSeqNumber = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.AimSeqNumber),
                    LearningDeliveryValue = LearningDeliveryValue(dataEntity),
                    LearningDeliveryDeliverableValues = dataEntity
                        .Children
                        .Where(e => e.EntityName == Attributes.EntityLearningDeliveryDeliverable)
                        .Select(LearningDeliveryDeliverablesFromDataEntity).ToList()
                };
        }

        public LearningDeliveryValue LearningDeliveryValue(IDataEntity dataEntity)
        {
            return new LearningDeliveryValue
            {
                Achieved = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.Achieved),
                AddProgCostElig = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.AddProgCostElig),
                AdjustedAreaCostFactor = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AdjustedAreaCostFactor),
                AdjustedPremiumFactor = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AdjustedPremiumFactor),
                AdjustedStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.AdjustedStartDate),
                AimClassification = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.AimClassification),
                AimValue = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.AimValue),
                ApplicWeightFundRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.ApplicWeightFundRate),
                EligibleProgressionOutcomeCode = _dataEntityAttributeService.GetLongAttributeValue(dataEntity, Attributes.EligibleProgressionOutcomeCode),
                EligibleProgressionOutcomeType = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.EligibleProgressionOutcomeType),
                EligibleProgressionOutomeStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.EligibleProgressionOutomeStartDate),
                FundStart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.FundStart),
                LARSWeightedRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.LARSWeightedRate),
                LatestPossibleStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.LatestPossibleStartDate),
                LDESFEngagementStartDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.LDESFEngagementStartDate),
                LearnDelLearnerEmpAtStart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.LearnDelLearnerEmpAtStart),
                PotentiallyEligibleForProgression = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.PotentiallyEligibleForProgression),
                ProgressionEndDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, Attributes.ProgressionEndDate),
                Restart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, Attributes.Restart),
                WeightedRateFromESOL = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.WeightedRateFromESOL),
            };
        }

        public LearningDeliveryDeliverableValues LearningDeliveryDeliverablesFromDataEntity(IDataEntity dataEntity)
        {
            return new LearningDeliveryDeliverableValues
            {
                DeliverableCode = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.DeliverableCode),
                DeliverableUnitCost = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.DeliverableUnitCost),
                LearningDeliveryDeliverablePeriodisedValues = LearningDeliveryDeliverablePeriodisedValuesFromEntity(dataEntity),
            };
        }

        public List<LearningDeliveryDeliverablePeriodisedValue> LearningDeliveryDeliverablePeriodisedValuesFromEntity(IDataEntity learningDeliveryDeliverable)
        {
            List<string> attributeList = new List<string>()
            {
                Attributes.AchievementEarnings,
                Attributes.AdditionalProgCostEarnings,
                Attributes.DeliverableVolume,
                Attributes.ProgressionEarnings,
                Attributes.ReportingVolume,
                Attributes.StartEarnings,
            };

            List<LearningDeliveryDeliverablePeriodisedValue> learningDeliveryPeriodisedAttributesList = new List<LearningDeliveryDeliverablePeriodisedValue>();

            foreach (var attribute in attributeList)
            {
                var attributeValue = learningDeliveryDeliverable.Attributes[attribute];

                var changePoints = attributeValue.Changepoints;

                if (!changePoints.Any())
                {
                    var value = _dataEntityAttributeService.GetDecimalAttributeValue(attributeValue.Value);

                    learningDeliveryPeriodisedAttributesList.Add(new LearningDeliveryDeliverablePeriodisedValue
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
                    learningDeliveryPeriodisedAttributesList.Add(new LearningDeliveryDeliverablePeriodisedValue
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

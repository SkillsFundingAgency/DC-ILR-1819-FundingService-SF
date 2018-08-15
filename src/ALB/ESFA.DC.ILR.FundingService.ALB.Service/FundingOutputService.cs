﻿using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.Service
{
    public class FundingOutputService : IOutputService<FundingOutputs>
    {
        private readonly IDataEntityAttributeService _dataEntityAttributeService;

        private readonly IEnumerable<string> _learnerPeriodisedAttributeList = new List<string> { "ALBSeqNum" };

        private readonly IEnumerable<string> _learningDeliveryPeriodisedAttributeList = new List<string>() { "ALBCode", "ALBSupportPayment", "AreaUpliftBalPayment", "AreaUpliftOnProgPayment" };

        private readonly IReadOnlyDictionary<int, DateTime> _periods = new Dictionary<int, DateTime>
        {
            { 1, new DateTime(2017, 08, 01) },
            { 2, new DateTime(2017, 09, 01) },
            { 3, new DateTime(2017, 10, 01) },
            { 4, new DateTime(2017, 11, 01) },
            { 5, new DateTime(2017, 12, 01) },
            { 6, new DateTime(2018, 01, 01) },
            { 7, new DateTime(2018, 02, 01) },
            { 8, new DateTime(2018, 03, 01) },
            { 9, new DateTime(2018, 04, 01) },
            { 10, new DateTime(2018, 05, 01) },
            { 11, new DateTime(2018, 06, 01) },
            { 12, new DateTime(2018, 07, 01) },
        };

        public FundingOutputService(IDataEntityAttributeService dataEntityAttributeService)
        {
            _dataEntityAttributeService = dataEntityAttributeService;
        }

        public FundingOutputs ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            if (dataEntities != null)
            {
                dataEntities = dataEntities.ToList();
                if (dataEntities.Any())
                {
                    return new FundingOutputs
                    {
                        Global = GlobalFromDataEntity(dataEntities.First()),
                        Learners = dataEntities.SelectMany(g => g.Children).Select(LearnerFromDataEntity).ToArray()
                    };
                }
            }

            return new FundingOutputs();
        }

        public GlobalAttribute GlobalFromDataEntity(IDataEntity dataEntity)
        {
           return new GlobalAttribute
           {
               UKPRN = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "UKPRN").Value,
               LARSVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "LARSVersion"),
               PostcodeAreaCostVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "PostcodeAreaCostVersion"),
               RulebaseVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "RulebaseVersion"),
           };
        }

        public LearnerAttribute LearnerFromDataEntity(IDataEntity dataEntity)
        {
            return new LearnerAttribute()
            {
                LearnRefNumber = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "LearnRefNumber"),
                LearnerPeriodisedAttributes = LearnerPeriodisedAttributes(dataEntity).ToArray(),
                LearningDeliveryAttributes = dataEntity.Children.Select(LearningDeliveryFromDataEntity).ToArray()
            };
        }

        public IEnumerable<LearnerPeriodisedAttribute> LearnerPeriodisedAttributes(IDataEntity learner)
        {
            foreach (var attribute in _learnerPeriodisedAttributeList)
            {
                var attributeValue = learner.Attributes[attribute];

                LearnerPeriodisedAttribute learnerPeriodisedAttribute;

                if (!attributeValue.Changepoints.Any())
                {
                    var value = decimal.Parse(attributeValue.Value.ToString());

                    learnerPeriodisedAttribute = new LearnerPeriodisedAttribute
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
                else
                {
                    learnerPeriodisedAttribute = new LearnerPeriodisedAttribute
                    {
                        AttributeName = attribute,
                        Period1 = GetPeriodAttributeValue(attributeValue, 1),
                        Period2 = GetPeriodAttributeValue(attributeValue, 2),
                        Period3 = GetPeriodAttributeValue(attributeValue, 3),
                        Period4 = GetPeriodAttributeValue(attributeValue, 4),
                        Period5 = GetPeriodAttributeValue(attributeValue, 5),
                        Period6 = GetPeriodAttributeValue(attributeValue, 6),
                        Period7 = GetPeriodAttributeValue(attributeValue, 7),
                        Period8 = GetPeriodAttributeValue(attributeValue, 8),
                        Period9 = GetPeriodAttributeValue(attributeValue, 9),
                        Period10 = GetPeriodAttributeValue(attributeValue, 10),
                        Period11 = GetPeriodAttributeValue(attributeValue, 11),
                        Period12 = GetPeriodAttributeValue(attributeValue, 12),
                    };
                }

                yield return learnerPeriodisedAttribute;
            }
        }

        public LearningDeliveryAttribute LearningDeliveryFromDataEntity(IDataEntity dataEntity)
        {
            return new LearningDeliveryAttribute
            {
                AimSeqNumber = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "AimSeqNumber").Value,
                LearningDeliveryAttributeDatas = LearningDeliveryAttributeData(dataEntity),
                LearningDeliveryPeriodisedAttributes = LearningDeliveryPeriodisedAttributeData(dataEntity).ToArray(),
            };
        }

        public LearningDeliveryAttributeData LearningDeliveryAttributeData(IDataEntity dataEntity)
        {
            return new LearningDeliveryAttributeData
            {
                Achieved = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "Achieved"),
                ActualNumInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "ActualNumInstalm"),
                AdvLoan = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "AdvLoan"),
                ApplicFactDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, "ApplicFactDate"),
                ApplicProgWeightFact = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "ApplicProgWeightFact"),
                AreaCostFactAdj = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "AreaCostFactAdj"),
                AreaCostInstalment = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "AreaCostInstalment"),
                FundLine = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, "FundLine"),
                FundStart = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "FundStart"),
                LiabilityDate = _dataEntityAttributeService.GetDateTimeAttributeValue(dataEntity, "LiabilityDate"),
                LoanBursAreaUplift = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "LoanBursAreaUplift"),
                LoanBursSupp = _dataEntityAttributeService.GetBoolAttributeValue(dataEntity, "LoanBursSupp"),
                OutstndNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "OutstndNumOnProgInstalm"),
                PlannedNumOnProgInstalm = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, "PlannedNumOnProgInstalm"),
                WeightedRate = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, "WeightedRate"),
            };
        }

        public IEnumerable<LearningDeliveryPeriodisedAttribute> LearningDeliveryPeriodisedAttributeData(IDataEntity learningDelivery)
        {
            foreach (var attribute in _learningDeliveryPeriodisedAttributeList)
            {
                var attributeValue = learningDelivery.Attributes[attribute];

                LearningDeliveryPeriodisedAttribute learningDeliveryPeriodisedAttribute;

                if (!attributeValue.Changepoints.Any())
                {
                    var value = decimal.Parse(attributeValue.Value.ToString());

                    learningDeliveryPeriodisedAttribute = new LearningDeliveryPeriodisedAttribute
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
                else
                {
                    learningDeliveryPeriodisedAttribute = new LearningDeliveryPeriodisedAttribute
                    {
                        AttributeName = attribute,
                        Period1 = GetPeriodAttributeValue(attributeValue, 1),
                        Period2 = GetPeriodAttributeValue(attributeValue, 2),
                        Period3 = GetPeriodAttributeValue(attributeValue, 3),
                        Period4 = GetPeriodAttributeValue(attributeValue, 4),
                        Period5 = GetPeriodAttributeValue(attributeValue, 5),
                        Period6 = GetPeriodAttributeValue(attributeValue, 6),
                        Period7 = GetPeriodAttributeValue(attributeValue, 7),
                        Period8 = GetPeriodAttributeValue(attributeValue, 8),
                        Period9 = GetPeriodAttributeValue(attributeValue, 9),
                        Period10 = GetPeriodAttributeValue(attributeValue, 10),
                        Period11 = GetPeriodAttributeValue(attributeValue, 11),
                        Period12 = GetPeriodAttributeValue(attributeValue, 12),
                    };
                }

                yield return learningDeliveryPeriodisedAttribute;
            }
        }

        private decimal GetPeriodAttributeValue(IAttributeData attributes, int period)
        {
            return decimal.Parse(attributes.Changepoints.Where(cp => cp.ChangePoint == _periods[period]).Select(v => v.Value).SingleOrDefault().ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Constants;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Output
{
    public class PeriodisationOutputService : IOutputService<IEnumerable<PeriodisationGlobal>>
    {
        private readonly IDataEntityAttributeService _dataEntityAttributeService;

        public PeriodisationOutputService(IDataEntityAttributeService dataEntityAttributeService)
        {
            _dataEntityAttributeService = dataEntityAttributeService;
        }

        public IEnumerable<PeriodisationGlobal> ProcessFundingOutputs(IEnumerable<IDataEntity> dataEntities)
        {
            return dataEntities.Select(MapGlobal);
        }

        public PeriodisationGlobal MapGlobal(IDataEntity dataEntity)
        {
            var learnerPeriodisedValues = dataEntity.Children?.Where(c => c.EntityName == Attributes.EntityLearner).Select(MapLearnerPeriodisedValues).ToList() ?? new List<LearnerPeriodisedValues>();
            var learnerPeriods = PivotLearnerPeriodisedValues(learnerPeriodisedValues).ToList();

            return new PeriodisationGlobal()
            {
                UKPRN = _dataEntityAttributeService.GetIntAttributeValue(dataEntity, Attributes.UKPRN),
                RulebaseVersion = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.RulebaseVersion),
                LearnerPeriods = learnerPeriods,
                LearnerPeriodisedValues = learnerPeriodisedValues
            };
        }

        public LearnerPeriodisedValues MapLearnerPeriodisedValues(IDataEntity dataEntity)
        {
            return new LearnerPeriodisedValues()
            {
                AttributeName = Attributes.LnrOnProgPay,
                LearnRefNumber = _dataEntityAttributeService.GetStringAttributeValue(dataEntity, Attributes.LearnRefNumber),
                Period1 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Period1),
                Period2 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Period2),
                Period3 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Period3),
                Period4 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Period4),
                Period5 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Period5),
                Period6 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Period6),
                Period7 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Period7),
                Period8 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Period8),
                Period9 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Period9),
                Period10 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Period10),
                Period11 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Period11),
                Period12 = _dataEntityAttributeService.GetDecimalAttributeValue(dataEntity, Attributes.Period12),
            };
        }

        public IEnumerable<LearnerPeriod> PivotLearnerPeriodisedValues(IEnumerable<LearnerPeriodisedValues> learnerPeriodisedValues)
        {
            return learnerPeriodisedValues.Select(PivotLearnerPeriodisedValue).SelectMany(v => v);
        }

        public IEnumerable<LearnerPeriod> PivotLearnerPeriodisedValue(LearnerPeriodisedValues learnerPeriodisedValue)
        {
            return new List<LearnerPeriod>()
            {
                new LearnerPeriod() { LearnRefNumber = learnerPeriodisedValue.LearnRefNumber, Period = 1, LnrOnProgPay = learnerPeriodisedValue.Period1 },
                new LearnerPeriod() { LearnRefNumber = learnerPeriodisedValue.LearnRefNumber, Period = 2, LnrOnProgPay = learnerPeriodisedValue.Period2 },
                new LearnerPeriod() { LearnRefNumber = learnerPeriodisedValue.LearnRefNumber, Period = 3, LnrOnProgPay = learnerPeriodisedValue.Period3 },
                new LearnerPeriod() { LearnRefNumber = learnerPeriodisedValue.LearnRefNumber, Period = 4, LnrOnProgPay = learnerPeriodisedValue.Period4 },
                new LearnerPeriod() { LearnRefNumber = learnerPeriodisedValue.LearnRefNumber, Period = 5, LnrOnProgPay = learnerPeriodisedValue.Period5 },
                new LearnerPeriod() { LearnRefNumber = learnerPeriodisedValue.LearnRefNumber, Period = 6, LnrOnProgPay = learnerPeriodisedValue.Period6 },
                new LearnerPeriod() { LearnRefNumber = learnerPeriodisedValue.LearnRefNumber, Period = 7, LnrOnProgPay = learnerPeriodisedValue.Period7 },
                new LearnerPeriod() { LearnRefNumber = learnerPeriodisedValue.LearnRefNumber, Period = 8, LnrOnProgPay = learnerPeriodisedValue.Period8 },
                new LearnerPeriod() { LearnRefNumber = learnerPeriodisedValue.LearnRefNumber, Period = 9, LnrOnProgPay = learnerPeriodisedValue.Period9 },
                new LearnerPeriod() { LearnRefNumber = learnerPeriodisedValue.LearnRefNumber, Period = 10, LnrOnProgPay = learnerPeriodisedValue.Period10 },
                new LearnerPeriod() { LearnRefNumber = learnerPeriodisedValue.LearnRefNumber, Period = 11, LnrOnProgPay = learnerPeriodisedValue.Period11 },
                new LearnerPeriod() { LearnRefNumber = learnerPeriodisedValue.LearnRefNumber, Period = 12, LnrOnProgPay = learnerPeriodisedValue.Period12 }
            };
        }

        private PeriodisationGlobal CondensePeriodisationGlobals(IEnumerable<PeriodisationGlobal> periodisationGlobals)
        {
            periodisationGlobals = periodisationGlobals.ToList();

            var firstGlobal = periodisationGlobals.FirstOrDefault();

            if (firstGlobal != null)
            {
                firstGlobal.LearnerPeriods = periodisationGlobals.SelectMany(g => g.LearnerPeriods).ToList();
                firstGlobal.LearnerPeriodisedValues = periodisationGlobals.SelectMany(g => g.LearnerPeriodisedValues).ToList();

                return firstGlobal;
            }

            return new PeriodisationGlobal();
        }
    }
}

using System.Linq;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Output;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Service.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Tests
{
    public class PeriodisationOutputServiceTests
    {
        [Fact]
        public void MapGlobal()
        {
            var rulebaseVersion = "RulebaseVersion";
            var ukprn = 1234;

            var dataEntity = new DataEntity(null);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "RulebaseVersion")).Returns(rulebaseVersion);
            dataEntityAttributeServiceMock.Setup(s => s.GetIntAttributeValue(dataEntity, "UKPRN")).Returns(ukprn);

            var global = NewService(dataEntityAttributeServiceMock.Object).MapGlobal(dataEntity);

            global.RulebaseVersion.Should().Be(rulebaseVersion);
            global.UKPRN.Should().Be(ukprn);

            global.LearnerPeriods.Should().BeEmpty();
            global.LearnerPeriodisedValues.Should().BeEmpty();
        }

        [Fact]
        public void MapLearnerPeriodisedValues()
        {
            var attributeName = "LnrOnProgPay";
            var learnRefNumber = "LearnRefNumber";
            var period1 = 1.1m;
            var period2 = 1.2m;
            var period3 = 1.3m;
            var period4 = 1.4m;
            var period5 = 1.5m;
            var period6 = 1.6m;
            var period7 = 1.7m;
            var period8 = 1.8m;
            var period9 = 1.9m;
            var period10 = 1.10m;
            var period11 = 1.11m;
            var period12 = 1.12m;

            var dataEntity = new DataEntity(null);

            var dataEntityAttributeServiceMock = new Mock<IDataEntityAttributeService>();

            dataEntityAttributeServiceMock.Setup(s => s.GetStringAttributeValue(dataEntity, "LearnRefNumber")).Returns(learnRefNumber);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LnrPrd1Pay")).Returns(period1);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LnrPrd2Pay")).Returns(period2);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LnrPrd3Pay")).Returns(period3);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LnrPrd4Pay")).Returns(period4);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LnrPrd5Pay")).Returns(period5);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LnrPrd6Pay")).Returns(period6);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LnrPrd7Pay")).Returns(period7);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LnrPrd8Pay")).Returns(period8);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LnrPrd9Pay")).Returns(period9);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LnrPrd10Pay")).Returns(period10);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LnrPrd11Pay")).Returns(period11);
            dataEntityAttributeServiceMock.Setup(s => s.GetDecimalAttributeValue(dataEntity, "LnrPrd12Pay")).Returns(period12);

            var learnerPeriodisedValues = NewService(dataEntityAttributeServiceMock.Object).MapLearnerPeriodisedValues(dataEntity);

            learnerPeriodisedValues.AttributeName.Should().Be(attributeName);
            learnerPeriodisedValues.LearnRefNumber.Should().Be(learnRefNumber);
            learnerPeriodisedValues.Period1.Should().Be(period1);
            learnerPeriodisedValues.Period2.Should().Be(period2);
            learnerPeriodisedValues.Period3.Should().Be(period3);
            learnerPeriodisedValues.Period4.Should().Be(period4);
            learnerPeriodisedValues.Period5.Should().Be(period5);
            learnerPeriodisedValues.Period6.Should().Be(period6);
            learnerPeriodisedValues.Period7.Should().Be(period7);
            learnerPeriodisedValues.Period8.Should().Be(period8);
            learnerPeriodisedValues.Period9.Should().Be(period9);
            learnerPeriodisedValues.Period10.Should().Be(period10);
            learnerPeriodisedValues.Period11.Should().Be(period11);
            learnerPeriodisedValues.Period12.Should().Be(period12);
        }

        [Fact]
        public void PivotLearnerPeriodisedValue()
        {
            var learnerPeriodisedValue = new LearnerPeriodisedValues()
            {
                AttributeName = "LnrOnProgPay",
                LearnRefNumber = "LearnRefNumber",
                Period1 = 1.1m,
                Period2 = 2.1m,
                Period3 = 3.1m,
                Period4 = 4.1m,
                Period5 = 5.1m,
                Period6 = 6.1m,
                Period7 = 7.1m,
                Period8 = 8.1m,
                Period9 = 9.1m,
                Period10 = 10.1m,
                Period11 = 11.1m,
                Period12 = 12.1m,
            };

            var pivoted = NewService().PivotLearnerPeriodisedValue(learnerPeriodisedValue).ToList();

            pivoted.Should().OnlyContain(p => p.LearnRefNumber == learnerPeriodisedValue.LearnRefNumber);

            pivoted[0].Period.Should().Be(1);
            pivoted[1].Period.Should().Be(2);
            pivoted[2].Period.Should().Be(3);
            pivoted[3].Period.Should().Be(4);
            pivoted[4].Period.Should().Be(5);
            pivoted[5].Period.Should().Be(6);
            pivoted[6].Period.Should().Be(7);
            pivoted[7].Period.Should().Be(8);
            pivoted[8].Period.Should().Be(9);
            pivoted[9].Period.Should().Be(10);
            pivoted[10].Period.Should().Be(11);
            pivoted[11].Period.Should().Be(12);

            pivoted[0].LnrOnProgPay.Should().Be(learnerPeriodisedValue.Period1);
            pivoted[1].LnrOnProgPay.Should().Be(learnerPeriodisedValue.Period2);
            pivoted[2].LnrOnProgPay.Should().Be(learnerPeriodisedValue.Period3);
            pivoted[3].LnrOnProgPay.Should().Be(learnerPeriodisedValue.Period4);
            pivoted[4].LnrOnProgPay.Should().Be(learnerPeriodisedValue.Period5);
            pivoted[5].LnrOnProgPay.Should().Be(learnerPeriodisedValue.Period6);
            pivoted[6].LnrOnProgPay.Should().Be(learnerPeriodisedValue.Period7);
            pivoted[7].LnrOnProgPay.Should().Be(learnerPeriodisedValue.Period8);
            pivoted[8].LnrOnProgPay.Should().Be(learnerPeriodisedValue.Period9);
            pivoted[9].LnrOnProgPay.Should().Be(learnerPeriodisedValue.Period10);
            pivoted[10].LnrOnProgPay.Should().Be(learnerPeriodisedValue.Period11);
            pivoted[11].LnrOnProgPay.Should().Be(learnerPeriodisedValue.Period12);
        }

        private PeriodisationOutputService NewService(IDataEntityAttributeService dataEntityAttributeService = null)
        {
            return new PeriodisationOutputService(dataEntityAttributeService);
        }
    }
}

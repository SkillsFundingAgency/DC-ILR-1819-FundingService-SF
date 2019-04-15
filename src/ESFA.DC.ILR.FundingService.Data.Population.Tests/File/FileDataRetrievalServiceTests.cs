using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Population.File;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.File
{
    public class FileDataRetrievalServiceTests
    {
        [Fact]
        public void RetrieveUKPRN()
        {
            var ukprn = 1234;

            var message = new TestMessage()
            {
                LearningProviderEntity = new TestLearningProvider()
                {
                    UKPRN = ukprn
                },
            };

            var fundingServiceDto = new Mock<IFundingServiceDto>();

            fundingServiceDto.SetupGet(fs => fs.Message).Returns(message);

            NewService(fundingServiceDto.Object).RetrieveUKPRN().Should().Be(ukprn);
        }

        [Fact]
        public void RetrieveDPOutcomes()
        {
            var learnRefNumber1 = "learnRefNumber1";
            var learnRefNumber2 = "learnRefNumber2";
            var outCode = 1;
            var outType = "outType";

            var message = new TestMessage()
            {
                LearnerDestinationAndProgressions = new List<TestLearnerDestinationAndProgression>()
                {
                    new TestLearnerDestinationAndProgression()
                    {
                        LearnRefNumber = learnRefNumber1,
                        DPOutcomes = new List<TestDPOutcome>()
                        {
                            new TestDPOutcome()
                            {
                                OutCode = outCode,
                                OutType = outType
                            }
                        }
                    },
                    new TestLearnerDestinationAndProgression()
                    {
                        LearnRefNumber = learnRefNumber2,
                        DPOutcomes = new List<TestDPOutcome>()
                        {
                            new TestDPOutcome(),
                            new TestDPOutcome(),
                        }
                    }
                }
            };

            var fundingServiceDto = new Mock<IFundingServiceDto>();

            fundingServiceDto.SetupGet(fs => fs.Message).Returns(message);

            var dpOutcomes = NewService(fundingServiceDto.Object).RetrieveDPOutcomes();

            dpOutcomes.Should().HaveCount(2);
            dpOutcomes.Should().ContainKeys(learnRefNumber1, learnRefNumber2);

            var learnRefNumber1DpOutcomes = dpOutcomes[learnRefNumber1];

            learnRefNumber1DpOutcomes.Should().HaveCount(1);

            var learnRefNumber1DpOutcome = learnRefNumber1DpOutcomes.First();

            learnRefNumber1DpOutcome.OutCode.Should().Be(outCode);
            learnRefNumber1DpOutcome.OutType.Should().Be(outType);

            dpOutcomes[learnRefNumber2].Should().HaveCount(2);
        }

        [Fact]
        public void RetrieveDPOutcomes_NullMessage()
        {
            var fundingServiceDto = new Mock<IFundingServiceDto>();

            fundingServiceDto.SetupGet(fs => fs.Message).Returns(null as IMessage);

            var dpOutcomes = NewService(fundingServiceDto.Object).RetrieveDPOutcomes();

            dpOutcomes.Should().NotBeNull();
            dpOutcomes.Should().BeEmpty();
        }

        [Fact]
        public void RetrieveDPOutcomes_NullLearnerDestinationAndProgressions()
        {
            var message = new TestMessage()
            {
                LearnerDestinationAndProgressions = null
            };

            var fundingServiceDto = new Mock<IFundingServiceDto>();

            fundingServiceDto.SetupGet(fs => fs.Message).Returns(message);

            var dpOutcomes = NewService(fundingServiceDto.Object).RetrieveDPOutcomes();

            dpOutcomes.Should().NotBeNull();
            dpOutcomes.Should().BeEmpty();
        }

        [Fact]
        public void RetrieveDPOutcomes_NullDPOutcomes()
        {
            var learnRefNumber1 = "learnRefNumber1";

            var message = new TestMessage()
            {
                LearnerDestinationAndProgressions = new List<TestLearnerDestinationAndProgression>()
                {
                    new TestLearnerDestinationAndProgression()
                    {
                        LearnRefNumber = learnRefNumber1,
                        DPOutcomes = null
                    }
                }
            };

            var fundingServiceDto = new Mock<IFundingServiceDto>();

            fundingServiceDto.SetupGet(fs => fs.Message).Returns(message);

            var dpOutcomes = NewService(fundingServiceDto.Object).RetrieveDPOutcomes();

            dpOutcomes.Should().HaveCount(1);

            var learnRefNumber1DpOutcomes = dpOutcomes[learnRefNumber1].ToList();

            learnRefNumber1DpOutcomes.Should().NotBeNull();
            learnRefNumber1DpOutcomes.Should().BeEmpty();
        }

        [Fact]
        public void RetrieveDPOutcomes_ValidLearnersFilter()
        {
            var learnRefNumber1 = "learnRefNumber1";
            var learnRefNumber2 = "learnRefNumber2";
            var outCode = 1;
            var outType = "outType";

            var message = new TestMessage()
            {
                LearnerDestinationAndProgressions = new List<TestLearnerDestinationAndProgression>()
                {
                    new TestLearnerDestinationAndProgression()
                    {
                        LearnRefNumber = learnRefNumber1,
                        DPOutcomes = new List<TestDPOutcome>()
                        {
                            new TestDPOutcome()
                            {
                                OutCode = outCode,
                                OutType = outType
                            }
                        }
                    },
                    new TestLearnerDestinationAndProgression()
                    {
                        LearnRefNumber = learnRefNumber2,
                        DPOutcomes = new List<TestDPOutcome>()
                        {
                            new TestDPOutcome(),
                            new TestDPOutcome(),
                        }
                    }
                }
            };

            var invalidLearners = new string[] { learnRefNumber2 };

            var fundingServiceDto = new Mock<IFundingServiceDto>();

            fundingServiceDto.SetupGet(fs => fs.Message).Returns(message);
            fundingServiceDto.SetupGet(fs => fs.InvalidLearners).Returns(invalidLearners);

            var dpOutcomes = NewService(fundingServiceDto.Object).RetrieveDPOutcomes();

            dpOutcomes.Should().HaveCount(1);
            dpOutcomes.Should().ContainKeys(learnRefNumber1);
            dpOutcomes.Should().NotContainKey(learnRefNumber2);

            var learnRefNumber1DpOutcomes = dpOutcomes[learnRefNumber1];

            learnRefNumber1DpOutcomes.Should().HaveCount(1);

            var learnRefNumber1DpOutcome = learnRefNumber1DpOutcomes.First();

            learnRefNumber1DpOutcome.OutCode.Should().Be(outCode);
            learnRefNumber1DpOutcome.OutType.Should().Be(outType);
        }

        private FileDataRetrievalService NewService(IFundingServiceDto fundingServiceDto = null)
        {
            return new FileDataRetrievalService(fundingServiceDto);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.Tests.Model;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class FCSDataRetrievalServiceTests
    {
        [Fact]
        public void Contractors()
        {
            var fcsMock = new Mock<IFcsContext>();

            var fcs = NewService(fcsMock.Object).Contractors;

            fcsMock.VerifyGet(l => l.Contractors);
        }

        [Fact]
        public void ElibibilityRules()
        {
            var fcsMock = new Mock<IFcsContext>();

            var fcs = NewService(fcsMock.Object).EsfEligibilityRules;

            fcsMock.VerifyGet(l => l.EsfEligibilityRules);
        }

        [Fact]
        public void DeliverableCodeMappings()
        {
            var fcsMock = new Mock<IFcsContext>();

            var fcs = NewService(fcsMock.Object).DeliverableCodeMappings;

            fcsMock.VerifyGet(l => l.ContractDeliverableCodeMappings);
        }

        [Fact]
        public void UniqueConRefNumbers()
        {
            var message = new TestMessage()
            {
                Learners = new List<TestLearner>()
                {
                    new TestLearner()
                    {
                        LearningDeliveries = new List<TestLearningDelivery>()
                        {
                            new TestLearningDelivery()
                            {
                                ConRefNumber = "1",
                            },
                            new TestLearningDelivery()
                            {
                                ConRefNumber = "2",
                            }
                        }
                    },
                    new TestLearner()
                }
            };

            var employerIds = NewService().UniqueConRefNumbers(message).ToList();

            employerIds.Should().HaveCount(2);
            employerIds.Should().Contain(new List<string>() { "1", "2" });
        }

        [Fact]
        public void ESFContractsForUKPRN()
        {
            var contractors = new List<Contractor>()
            {
                new Contractor()
                {
                    Ukprn = 1,
                    Contracts = new List<Contract>
                    {
                        new Contract
                        {
                            ContractAllocations = new List<ContractAllocation>
                            {
                                new ContractAllocation
                                {
                                    FundingStreamPeriodCode = "7",
                                    ContractAllocationNumber = "1",
                                    TenderSpecReference = "T1",
                                    LotReference = "L1",
                                    ContractDeliverables = new List<ContractDeliverable>
                                    {
                                        new ContractDeliverable
                                        {
                                            DeliverableCode = 1,
                                            UnitCost = 2.0m
                                        },
                                         new ContractDeliverable
                                        {
                                            DeliverableCode = 2,
                                            UnitCost = 3.0m
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new Contractor()
                {
                    Ukprn = 1,
                    Contracts = new List<Contract>
                    {
                        new Contract
                        {
                            ContractAllocations = new List<ContractAllocation>
                            {
                                new ContractAllocation
                                {
                                    ContractAllocationNumber = "2",
                                    TenderSpecReference = "T2",
                                    LotReference = "L2",
                                    FundingStreamPeriodCode = "5",
                                    ContractDeliverables = new List<ContractDeliverable>
                                    {
                                        new ContractDeliverable
                                        {
                                            DeliverableCode = 1,
                                            UnitCost = 2.0m
                                        },
                                        new ContractDeliverable
                                        {
                                            DeliverableCode = 2,
                                            UnitCost = 3.0m
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }.AsQueryable();

            var eligibilityRules = new List<EsfEligibilityRule>()
            {
                new EsfEligibilityRule()
                {
                    TenderSpecReference = "T1",
                    LotReference = "L1",
                    CalcMethod = 1
                },
                new EsfEligibilityRule()
                {
                    TenderSpecReference = "T2",
                    LotReference = "L2",
                    CalcMethod = 2
                }
            }.AsQueryable();

            var codeMappings = new List<ContractDeliverableCodeMapping>()
            {
                new ContractDeliverableCodeMapping()
                {
                    FundingStreamPeriodCode = "4",
                    ExternalDeliverableCode = "ST2",
                    FCSDeliverableCode = "2"
                },
                new ContractDeliverableCodeMapping()
                {
                    FundingStreamPeriodCode = "5",
                    ExternalDeliverableCode = "ST1",
                    FCSDeliverableCode = "1"
                },
                new ContractDeliverableCodeMapping()
                {
                    FundingStreamPeriodCode = "5",
                    ExternalDeliverableCode = "ST2",
                    FCSDeliverableCode = "2"
                },
                new ContractDeliverableCodeMapping()
                {
                    FundingStreamPeriodCode = "5",
                    ExternalDeliverableCode = "ST3",
                    FCSDeliverableCode = "3"
                },
            }.AsQueryable();

            var fcsDataRetrievalServiceMock = NewMock();

            fcsDataRetrievalServiceMock.SetupGet(l => l.Contractors).Returns(contractors);
            fcsDataRetrievalServiceMock.SetupGet(l => l.EsfEligibilityRules).Returns(eligibilityRules);
            fcsDataRetrievalServiceMock.SetupGet(l => l.DeliverableCodeMappings).Returns(codeMappings);

            var conRefNumbers = new List<string>() { "1", "2" }.ToCaseInsensitiveHashSet();

            var fcs = fcsDataRetrievalServiceMock.Object.FCSContractsForUKPRN(1, conRefNumbers);

            fcs.Should().HaveCount(2);

            fcs.Select(c => c.ContractAllocationNumber).Should().Contain("1", "2");

            fcs.Where(c => c.ContractAllocationNumber == "1").Select(cm => cm.CalcMethod).Should().BeEquivalentTo(1);
            fcs.Where(c => c.ContractAllocationNumber == "2").Select(cm => cm.CalcMethod).Should().BeEquivalentTo(2);
        }

        private FCSDataRetrievalService NewService(IFcsContext fcs = null)
        {
            return new FCSDataRetrievalService(fcs);
        }

        private Mock<FCSDataRetrievalService> NewMock()
        {
            return new Mock<FCSDataRetrievalService>()
            {
                CallBase = true
            };
        }
    }
}

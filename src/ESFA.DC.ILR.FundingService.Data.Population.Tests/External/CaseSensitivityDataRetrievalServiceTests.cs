using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.AppsEarningsHistory.Model;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;
using ESFA.DC.ILR.FundingService.Tests.Common;
using ESFA.DC.ReferenceData.FCS.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    // Tests for case invariance accrss applicable external reference data lookups.
    public class CaseSensitivityDataRetrievalServiceTests
    {
        [Fact]
        public void LARSFundingsForLearnAimRefs()
        {
            var lars_Fundings = new List<LARS_Funding>()
            {
                new LARS_Funding()
                {
                    LearnAimRef = "ABC1234",
                },
                new LARS_Funding()
                {
                    LearnAimRef = "abc456",
                },
                new LARS_Funding()
                {
                    LearnAimRef = "Abc123",
                },
                 new LARS_Funding()
                {
                    LearnAimRef = "Abc123",
                },
                new LARS_Funding()
                {
                    LearnAimRef = "789",
                },
                new LARS_Funding(),
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewLARSMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSFundings).Returns(lars_Fundings);

            var learnAimRefs = new List<string> { "abc1234", "ABC123", "ABC456", "234" }.ToCaseInsensitiveHashSet();

            var larsFundings = larsDataRetrievalServiceMock.Object.LARSFundingsForLearnAimRefs(learnAimRefs);

            larsFundings.Should().HaveCount(3);
            larsFundings.Should().ContainKeys("ABC1234", "Abc123", "abc456");
            larsFundings["ABC1234"].Should().HaveCount(1);
            larsFundings["Abc123"].Should().HaveCount(2);
            larsFundings["abc456"].Should().HaveCount(1);
        }

        [Fact]
        public void LARSLearningDeliveryForLearnAimRefs()
        {
            var lars_LearningDeliveries = new List<LARS_LearningDelivery>()
            {
                new LARS_LearningDelivery()
                {
                    LearnAimRef = "Abc123",
                },
                new LARS_LearningDelivery()
                {
                    LearnAimRef = "ABC456",
                },
                new LARS_LearningDelivery()
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewLARSMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSLearningDeliveries).Returns(lars_LearningDeliveries);

            var learnAimRefs = new List<string>() { "ABC123", "ABC456" }.ToCaseInsensitiveHashSet();

            var larsLearningDeliveries = larsDataRetrievalServiceMock.Object.LARSLearningDeliveriesForLearnAimRefs(learnAimRefs);

            larsLearningDeliveries.Should().HaveCount(2);
            larsLearningDeliveries.Should().ContainKeys("Abc123", "ABC456");
        }

        [Fact]
        public void LARSLearningDeliveryForLearnAimRefs_WithChildren()
        {
            var lars_LearningDeliveries = new List<LARS_LearningDelivery>()
            {
                new LARS_LearningDelivery()
                {
                    LearnAimRef = "Abc123",
                    LARS_Funding = new List<LARS_Funding>
                    {
                        new LARS_Funding
                        {
                            LearnAimRef = "ABC123"
                        }
                    }
                },
                new LARS_LearningDelivery()
                {
                    LearnAimRef = "ABC456",
                    LARS_Funding = new List<LARS_Funding>
                    {
                        new LARS_Funding
                        {
                            LearnAimRef = "ABC456"
                        },
                        new LARS_Funding
                        {
                            LearnAimRef = "abc456"
                        }
                    }
                },
                new LARS_LearningDelivery()
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewLARSMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSLearningDeliveries).Returns(lars_LearningDeliveries);

            var learnAimRefs = new List<string>() { "ABC123", "ABC456" }.ToCaseInsensitiveHashSet();

            var larsLearningDeliveries = larsDataRetrievalServiceMock.Object.LARSLearningDeliveriesForLearnAimRefs(learnAimRefs);

            larsLearningDeliveries.Should().HaveCount(2);
            larsLearningDeliveries.Should().ContainKeys("Abc123", "ABC456");
            larsLearningDeliveries["Abc123"].LARSFunding.Should().HaveCount(1);
            larsLearningDeliveries["ABC456"].LARSFunding.Should().HaveCount(2);
        }

        [Fact]
        public void LARSAnnualValuesForLearnAimRefs()
        {
            var lars_AnnualValues = new List<LARS_AnnualValue>()
            {
                new LARS_AnnualValue()
                {
                    LearnAimRef = "ABC123",
                },
                new LARS_AnnualValue()
                {
                    LearnAimRef = "abc456",
                },
                new LARS_AnnualValue()
                {
                    LearnAimRef = "789",
                },
                new LARS_AnnualValue(),
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewLARSMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSAnnualValues).Returns(lars_AnnualValues);

            var learnAimRefs = new List<string>() { "ABC123", "ABC456", "Abc123" }.ToCaseInsensitiveHashSet();

            var larsAnnualValues = larsDataRetrievalServiceMock.Object.LARSAnnualValuesForLearnAimRefs(learnAimRefs);

            larsAnnualValues.Should().HaveCount(2);
            larsAnnualValues.Should().ContainKeys("ABC123", "abc456");
            larsAnnualValues["ABC123"].Should().HaveCount(1);
            larsAnnualValues["abc456"].Should().HaveCount(1);
        }

        [Fact]
        public void LARSLearningDeliveryCategoriesForLearnAimRefs()
        {
            var lars_LearningDeliveryCategories = new List<LARS_LearningDeliveryCategory>()
            {
                new LARS_LearningDeliveryCategory()
                {
                    LearnAimRef = "ABC123",
                },
                new LARS_LearningDeliveryCategory()
                {
                    LearnAimRef = "abc456",
                },
                new LARS_LearningDeliveryCategory()
                {
                    LearnAimRef = "dD123",
                },
                new LARS_LearningDeliveryCategory()
                {
                    LearnAimRef = "789",
                },
                new LARS_LearningDeliveryCategory(),
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewLARSMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSLearningDeliveryCategories).Returns(lars_LearningDeliveryCategories);

            var learnAimRefs = new List<string>() { "ABC123", "ABC456", "DD123", "234" }.ToCaseInsensitiveHashSet();

            var larsLearningDeliveryCategories = larsDataRetrievalServiceMock.Object.LARSLearningDeliveryCategoriesForLearnAimRefs(learnAimRefs);

            larsLearningDeliveryCategories.Should().HaveCount(3);
            larsLearningDeliveryCategories.Should().ContainKeys("ABC123", "abc456", "dD123");
            larsLearningDeliveryCategories["ABC123"].Should().HaveCount(1);
            larsLearningDeliveryCategories["abc456"].Should().HaveCount(1);
            larsLearningDeliveryCategories["dD123"].Should().HaveCount(1);
        }

        [Fact]
        public void LARSFrameworkAimsForLearnAimRefs()
        {
            var lars_FrameworkAims = new List<LARS_FrameworkAims>()
            {
                new LARS_FrameworkAims()
                {
                    LearnAimRef = "ABC123",
                },
                new LARS_FrameworkAims()
                {
                    LearnAimRef = "abc456",
                },
                new LARS_FrameworkAims()
                {
                    LearnAimRef = "123",
                },
                new LARS_FrameworkAims()
                {
                    LearnAimRef = "789",
                },
                new LARS_FrameworkAims(),
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewLARSMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSFrameworkAims).Returns(lars_FrameworkAims);

            var learnAimRefs = new List<string>() { "ABC123", "ABC456", "234" }.ToCaseInsensitiveHashSet();

            var larsFrameworkAims = larsDataRetrievalServiceMock.Object.LARSFrameworkAimsForLearnAimRefs(learnAimRefs);

            larsFrameworkAims.Should().HaveCount(2);
            larsFrameworkAims.Should().ContainKeys("ABC123", "abc456");
            larsFrameworkAims["ABC123"].Should().HaveCount(1);
            larsFrameworkAims["abc456"].Should().HaveCount(1);
        }

        [Fact]
        public void LARSFrameworkCommonComponentForLearnAimRefs()
        {
            var lars_FrameworkCommonComponents = new List<LARS_FrameworkCmnComp>()
            {
                new LARS_FrameworkCmnComp()
                {
                    CommonComponent = 1,
                    FworkCode = 2,
                    ProgType = 3,
                    PwayCode = 4,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new LARS_FrameworkCmnComp()
                {
                    CommonComponent = 1,
                    FworkCode = 3,
                    ProgType = 3,
                    PwayCode = 4,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new LARS_FrameworkCmnComp()
                {
                    CommonComponent = 2,
                    FworkCode = 2,
                    ProgType = 3,
                    PwayCode = 4,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new LARS_FrameworkCmnComp()
                {
                    CommonComponent = 2,
                    FworkCode = 2,
                    ProgType = 3,
                    PwayCode = 5,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new LARS_FrameworkCmnComp(),
            }.AsQueryable();

            var lars_LearningDeliveries = new List<LARS_LearningDelivery>()
            {
                new LARS_LearningDelivery()
                {
                    LearnAimRef = "ABC123",
                    FrameworkCommonComponent = 1
                },
                new LARS_LearningDelivery()
                {
                    LearnAimRef = "abc456",
                    FrameworkCommonComponent = 2
                }
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewLARSMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSLearningDeliveries).Returns(lars_LearningDeliveries);
            larsDataRetrievalServiceMock.SetupGet(l => l.LARSFrameworkCommonComponents).Returns(lars_FrameworkCommonComponents);

            var frameworkKeys = new List<LARSFrameworkKey>
            {
                new LARSFrameworkKey("ABC123", 2, 3, 4),
                new LARSFrameworkKey("ABC123", 3, 3, 4),
                new LARSFrameworkKey("ABC456", 2, 3, 4)
            };

            var larsFramworkCommonComponents = larsDataRetrievalServiceMock.Object.LARSFrameworkCommonComponentForLearnAimRefs(frameworkKeys);

            larsFramworkCommonComponents.Should().HaveCount(3);
            larsFramworkCommonComponents.Select(l => l.LearnAimRef).ToList().Should().Contain("ABC123", "abc456");
        }

        [Fact]
        public void FCSContracts()
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
                                    ContractAllocationNumber = "aa",
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
                                    ContractAllocationNumber = "BB",
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

            var fcsDataRetrievalServiceMock = NewFCSMock();

            fcsDataRetrievalServiceMock.SetupGet(l => l.Contractors).Returns(contractors);
            fcsDataRetrievalServiceMock.SetupGet(l => l.EsfEligibilityRules).Returns(eligibilityRules);
            fcsDataRetrievalServiceMock.SetupGet(l => l.DeliverableCodeMappings).Returns(codeMappings);

            var conRefNumbers = new List<string>() { "AA", "bb", "Cc" }.ToCaseInsensitiveHashSet();

            var fcs = fcsDataRetrievalServiceMock.Object.FCSContractsForUKPRN(1, conRefNumbers);

            fcs.Should().HaveCount(2);

            fcs.Select(c => c.ContractAllocationNumber).Should().Contain("aa", "BB");

            fcs.Where(c => c.ContractAllocationNumber == "aa").Select(cm => cm.CalcMethod).Should().BeEquivalentTo(1);
            fcs.Where(c => c.ContractAllocationNumber == "BB").Select(cm => cm.CalcMethod).Should().BeEquivalentTo(2);
        }

        private Mock<PostcodesDataRetrievalService> NewPostcodesMock()
        {
            return new Mock<PostcodesDataRetrievalService>();
        }

        private Mock<LARSDataRetrievalService> NewLARSMock()
        {
            return new Mock<LARSDataRetrievalService>();
        }

        private Mock<AppsEarningsHistoryDataRetrievalService> NewAppsHistoryMock()
        {
            return new Mock<AppsEarningsHistoryDataRetrievalService>();
        }

        private Mock<FCSDataRetrievalService> NewFCSMock()
        {
            return new Mock<FCSDataRetrievalService>();
        }
    }
}

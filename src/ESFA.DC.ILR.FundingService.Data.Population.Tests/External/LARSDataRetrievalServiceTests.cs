using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;
using ESFA.DC.ILR.FundingService.Tests.Common;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class LARSDataRetrievalServiceTests
    {
        [Fact]
        public void LarsFundings()
        {
            var larsMock = new Mock<ILARS>();

            var larsFundings = NewService(larsMock.Object).LARSFundings;

            larsMock.VerifyGet(p => p.LARS_Funding);
        }

        [Fact]
        public void LarsLearningDeliveries()
        {
            var larsMock = new Mock<ILARS>();

            var larsLearningDeliveries = NewService(larsMock.Object).LARSLearningDeliveries;

            larsMock.VerifyGet(l => l.LARS_LearningDelivery);
        }

        [Fact]
        public void LarsVersions()
        {
            var larsMock = new Mock<ILARS>();

            var larsVersions = NewService(larsMock.Object).LARSVersions;

            larsMock.VerifyGet(l => l.LARS_Version);
        }

        [Fact]
        public void LarsAnnualValue()
        {
            var larsMock = new Mock<ILARS>();

            var larsAnnualValues = NewService(larsMock.Object).LARSAnnualValues;

            larsMock.VerifyGet(l => l.LARS_AnnualValue);
        }

        [Fact]
        public void LarsLearningDeliveryCategories()
        {
            var larsMock = new Mock<ILARS>();

            var larsLearningDeliveryCategories = NewService(larsMock.Object).LARSLearningDeliveryCategories;

            larsMock.VerifyGet(l => l.LARS_LearningDeliveryCategory);
        }

        [Fact]
        public void LarsFrameworkAims()
        {
            var larsMock = new Mock<ILARS>();

            var larsFrameworkAims = NewService(larsMock.Object).LARSFrameworkAims;

            larsMock.VerifyGet(l => l.LARS_FrameworkAims);
        }

        [Fact]
        public void LarsStandardCommonComponents()
        {
            var larsMock = new Mock<ILARS>();

            var larsFrameworkAims = NewService(larsMock.Object).LARSStandardCommonComponents;

            larsMock.VerifyGet(l => l.LARS_StandardCommonComponent);
        }

        [Fact]
        public void LarsFrameworkCommonComponents()
        {
            var larsMock = new Mock<ILARS>();

            var larsFrameworkAims = NewService(larsMock.Object).LARSFrameworkCommonComponents;

            larsMock.VerifyGet(l => l.LARS_FrameworkCmnComp);
        }

        [Fact]
        public void UniqueLearnAimRefs()
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
                                LearnAimRef = "one",
                            },
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "two",
                            },
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "one",
                            }
                        },
                    },
                    new TestLearner()
                    {
                        LearningDeliveries = new List<TestLearningDelivery>()
                        {
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "one",
                            }
                        }
                    },
                    new TestLearner(),
                }
            };

            var uniqueLearnAimRefs = NewService().UniqueLearnAimRefs(message).ToList();

            uniqueLearnAimRefs.Should().HaveCount(2);
            uniqueLearnAimRefs.Should().Contain(new List<string>() { "one", "two" });
        }

        [Fact]
        public void UniqueStandardCodes()
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
                                StdCodeNullable = 1,
                            },
                            new TestLearningDelivery()
                            {
                                StdCodeNullable = 2,
                            },
                            new TestLearningDelivery()
                            {
                                StdCodeNullable = 1,
                            }
                        },
                    },
                    new TestLearner()
                    {
                        LearningDeliveries = new List<TestLearningDelivery>()
                        {
                            new TestLearningDelivery()
                            {
                                StdCodeNullable = 1,
                            }
                        }
                    },
                    new TestLearner()
                    {
                        LearningDeliveries = new List<TestLearningDelivery>()
                        {
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "one",
                            }
                        }
                    },
                }
            };

            var stamdardCodes = NewService().UniqueStandardCodes(message).ToList();

            stamdardCodes.Should().HaveCount(2);
            stamdardCodes.Should().Contain(new List<int>() { 1, 2 });
        }

        [Fact]
        public void UniqueFrameworks()
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
                                LearnAimRef = "1",
                                FworkCodeNullable = 1,
                                ProgTypeNullable = 2,
                                PwayCodeNullable = 3
                            },
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "1",
                                FworkCodeNullable = 1,
                                ProgTypeNullable = 5,
                                PwayCodeNullable = 3
                            },
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "2",
                                FworkCodeNullable = 1,
                                ProgTypeNullable = 2,
                                PwayCodeNullable = 4
                            }
                        },
                    },
                    new TestLearner()
                    {
                        LearningDeliveries = new List<TestLearningDelivery>()
                        {
                            new TestLearningDelivery()
                            {
                            }
                        }
                    },
                    new TestLearner()
                    {
                        LearningDeliveries = new List<TestLearningDelivery>()
                        {
                            new TestLearningDelivery()
                            {
                                FworkCodeNullable = 1
                            },
                            new TestLearningDelivery()
                            {
                                FworkCodeNullable = 1,
                                PwayCodeNullable = 3
                            },
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "2",
                                FworkCodeNullable = 1,
                                ProgTypeNullable = 2,
                                PwayCodeNullable = 4
                            }
                        }
                    },
                }
            };

            var frameworks = NewService().UniqueFrameworkCommonComponents(message);

            frameworks.Should().HaveCount(3);
            frameworks.Should().BeEquivalentTo(new List<LARSFrameworkKey>
            {
                new LARSFrameworkKey("1", 1, 2, 3),
                new LARSFrameworkKey("1", 1, 5, 3),
                new LARSFrameworkKey("2", 1, 2, 4)
            });
        }

        [Fact]
        public void UniqueApprenticesipFundings()
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
                                LearnAimRef = "1",
                                FworkCodeNullable = 1,
                                ProgTypeNullable = 2,
                                PwayCodeNullable = 3
                            },
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "1",
                                FworkCodeNullable = 1,
                                ProgTypeNullable = 5,
                                PwayCodeNullable = 3
                            },
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "2",
                                FworkCodeNullable = 1,
                                ProgTypeNullable = 2,
                                PwayCodeNullable = 4
                            }
                        },
                    },
                    new TestLearner()
                    {
                        LearningDeliveries = new List<TestLearningDelivery>()
                        {
                            new TestLearningDelivery()
                            {
                            }
                        }
                    },
                    new TestLearner()
                    {
                        LearningDeliveries = new List<TestLearningDelivery>()
                        {
                            new TestLearningDelivery()
                            {
                                FworkCodeNullable = 1
                            },
                            new TestLearningDelivery()
                            {
                                FworkCodeNullable = 1,
                                PwayCodeNullable = 3
                            },
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "2",
                                StdCodeNullable = 10,
                                ProgTypeNullable = 2,
                                PwayCodeNullable = 4
                            }
                        }
                    },
                }
            };

            var frameworks = NewService().UniqueApprenticeshipFundingFrameworks(message);
            var standards = NewService().UniqueApprenticeshipFundingStandards(message);

            frameworks.Should().HaveCount(3);
            frameworks.Should().BeEquivalentTo(new List<LARSApprenticeshipFundingKey>
            {
                new LARSApprenticeshipFundingKey(1, 2, 3),
                new LARSApprenticeshipFundingKey(1, 5, 3),
                new LARSApprenticeshipFundingKey(1, 2, 4)
            });

            standards.Should().HaveCount(1);
            standards.Should().BeEquivalentTo(new List<LARSApprenticeshipFundingKey>
            {
                new LARSApprenticeshipFundingKey(10, 2, 0)
            });
        }

        [Fact]
        public void LARSFundingsForLearnAimRefs()
        {
            var lars_Fundings = new List<LARS_Funding>()
            {
                new LARS_Funding()
                {
                    LearnAimRef = "123",
                },
                new LARS_Funding()
                {
                    LearnAimRef = "456",
                },
                new LARS_Funding()
                {
                    LearnAimRef = "123",
                },
                new LARS_Funding()
                {
                    LearnAimRef = "789",
                },
                new LARS_Funding(),
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSFundings).Returns(lars_Fundings);

            var learnAimRefs = new List<string>() { "123", "456", "234" };

            var larsFundings = larsDataRetrievalServiceMock.Object.LARSFundingsForLearnAimRefs(learnAimRefs);

            larsFundings.Should().HaveCount(2);
            larsFundings.Should().ContainKeys("123", "456");
            larsFundings["123"].Should().HaveCount(2);
            larsFundings["456"].Should().HaveCount(1);
        }

        [Fact]
        public void LARSFundingFromEntity()
        {
            var lars_Funding = new LARS_Funding()
            {
                EffectiveFrom = new DateTime(2017, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
                FundingCategory = "FC",
                LearnAimRef = "LearnAimRef",
                RateUnWeighted = 1.2m,
                RateWeighted = 1.3m,
                WeightingFactor = "WF",
            };

            var larsFunding = NewService().LARSFundingFromEntity(lars_Funding);

            larsFunding.EffectiveFrom.Should().Be(lars_Funding.EffectiveFrom);
            larsFunding.EffectiveTo.Should().Be(lars_Funding.EffectiveTo);
            larsFunding.FundingCategory.Should().Be(lars_Funding.FundingCategory);
            larsFunding.LearnAimRef.Should().Be(lars_Funding.LearnAimRef);
            larsFunding.RateUnWeighted.Should().Be(lars_Funding.RateUnWeighted);
            larsFunding.RateWeighted.Should().Be(lars_Funding.RateWeighted);
            larsFunding.WeightingFactor.Should().Be(lars_Funding.WeightingFactor);
        }

        [Fact]
        public void LARSLearningDeliveryForLearnAimRefs()
        {
            var lars_LearningDeliveries = new List<LARS_LearningDelivery>()
            {
                new LARS_LearningDelivery()
                {
                    LearnAimRef = "123",
                },
                new LARS_LearningDelivery()
                {
                    LearnAimRef = "456",
                },
                new LARS_LearningDelivery()
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSLearningDeliveries).Returns(lars_LearningDeliveries);

            var learnAimRefs = new List<string>() { "123", "456" };

            var larsLearningDeliveries = larsDataRetrievalServiceMock.Object.LARSLearningDeliveriesForLearnAimRefs(learnAimRefs);

            larsLearningDeliveries.Should().HaveCount(2);
            larsLearningDeliveries.Should().ContainKeys("123", "456");
        }

        [Fact]
        public void LarsLearningDeliveryFromEntity()
        {
            var lars_LearningDelivery = new LARS_LearningDelivery()
            {
                EnglPrscID = 1,
                EnglandFEHEStatus = "englandFEHEStatus",
                FrameworkCommonComponent = 2,
                LearnAimRef = "learnAimRef",
                LearnAimRefType = "learnAimRefType",
                LearnAimRefTitle = "learnAimRefTitle",
                NotionalNVQLevelv2 = "notionalNVQLevelv2",
                LearningDeliveryGenre = "genre",
                RegulatedCreditValue = 3,
                EFACOFType = 1,
                AwardOrgCode = "awardOrgCode",
                SectorSubjectAreaTier2 = 1.0m,
                LARS_Validity = new List<LARS_Validity>()
                {
                    new LARS_Validity(),
                    new LARS_Validity(),
                },
                LARS_CareerLearningPilot = new List<LARS_CareerLearningPilot>()
                {
                    new LARS_CareerLearningPilot(),
                    new LARS_CareerLearningPilot(),
                    new LARS_CareerLearningPilot(),
                },
            };

            var larsLearningDelivery = NewService().LARSLearningDeliveryFromEntity(lars_LearningDelivery);

            larsLearningDelivery.EnglPrscID.Should().Be(lars_LearningDelivery.EnglPrscID);
            larsLearningDelivery.EnglandFEHEStatus.Should().Be(lars_LearningDelivery.EnglandFEHEStatus);
            larsLearningDelivery.FrameworkCommonComponent.Should().Be(lars_LearningDelivery.FrameworkCommonComponent);
            larsLearningDelivery.LearnAimRef.Should().Be(lars_LearningDelivery.LearnAimRef);
            larsLearningDelivery.LearnAimRefType.Should().Be(lars_LearningDelivery.LearnAimRefType);
            larsLearningDelivery.LearnAimRefTitle.Should().Be(lars_LearningDelivery.LearnAimRefTitle);
            larsLearningDelivery.NotionalNVQLevelv2.Should().Be(lars_LearningDelivery.NotionalNVQLevelv2);
            larsLearningDelivery.LearningDeliveryGenre.Should().Be(lars_LearningDelivery.LearningDeliveryGenre);
            larsLearningDelivery.RegulatedCreditValue.Should().Be(lars_LearningDelivery.RegulatedCreditValue);
            larsLearningDelivery.AwardOrgCode.Should().Be(lars_LearningDelivery.AwardOrgCode);
            larsLearningDelivery.EFACOFType.Should().Be(lars_LearningDelivery.EFACOFType);
            larsLearningDelivery.SectorSubjectAreaTier2.Should().Be(lars_LearningDelivery.SectorSubjectAreaTier2);

            larsLearningDelivery.LARSValidities.Should().HaveCount(2);
            larsLearningDelivery.LARSCareerLearningPilot.Should().HaveCount(3);
        }

        [Fact]
        public void LarsValidityFromEntity()
        {
            var lars_Validity = new LARS_Validity()
            {
                ValidityCategory = "Category",
                LastNewStartDate = new DateTime(2017, 1, 1),
                StartDate = new DateTime(2018, 1, 1),
            };

            var larsValidity = NewService().LARSValidityFromEntity(lars_Validity);

            larsValidity.Category.Should().Be(lars_Validity.ValidityCategory);
            larsValidity.LastNewStartDate.Should().Be(lars_Validity.LastNewStartDate);
            larsValidity.StartDate.Should().Be(lars_Validity.StartDate);
        }

        [Fact]
        public void LarsCareerLearningPilotFromEntity()
        {
            var lars_CareerLearningPilot = new LARS_CareerLearningPilot()
            {
                AreaCode = "AreaCode",
                SubsidyRate = 1.2m,
                EffectiveFrom = new DateTime(2017, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
            };

            var larsCareerLearningPilot = NewService().LARSCareerLearningPilotFromEntity(lars_CareerLearningPilot);

            larsCareerLearningPilot.AreaCode.Should().Be(lars_CareerLearningPilot.AreaCode);
            larsCareerLearningPilot.SubsidyRate.Should().Be(lars_CareerLearningPilot.SubsidyRate);
            larsCareerLearningPilot.EffectiveFrom.Should().Be(lars_CareerLearningPilot.EffectiveFrom);
            larsCareerLearningPilot.EffectiveTo.Should().Be(lars_CareerLearningPilot.EffectiveTo);
        }

        [Fact]
        public void CurrentVersion()
        {
            var versions = new List<LARS_Version>()
            {
                new LARS_Version() { MainDataSchemaName = "001" },
                new LARS_Version() { MainDataSchemaName = "002" },
                new LARS_Version() { MainDataSchemaName = "003" }
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSVersions).Returns(versions);

            larsDataRetrievalServiceMock.Object.CurrentVersion().Should().Be("003");
        }

        [Fact]
        public void LARSAnnualValuesForLearnAimRefs()
        {
            var lars_AnnualValues = new List<LARS_AnnualValue>()
            {
                new LARS_AnnualValue()
                {
                    LearnAimRef = "123",
                },
                new LARS_AnnualValue()
                {
                    LearnAimRef = "456",
                },
                new LARS_AnnualValue()
                {
                    LearnAimRef = "123",
                },
                new LARS_AnnualValue()
                {
                    LearnAimRef = "789",
                },
                new LARS_AnnualValue(),
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSAnnualValues).Returns(lars_AnnualValues);

            var learnAimRefs = new List<string>() { "123", "456", "234" };

            var larsAnnualValues = larsDataRetrievalServiceMock.Object.LARSAnnualValuesForLearnAimRefs(learnAimRefs);

            larsAnnualValues.Should().HaveCount(2);
            larsAnnualValues.Should().ContainKeys("123", "456");
            larsAnnualValues["123"].Should().HaveCount(2);
            larsAnnualValues["456"].Should().HaveCount(1);
        }

        [Fact]
        public void LARSAnnualValueFromEntity()
        {
            var entity = new LARS_AnnualValue()
            {
                BasicSkillsType = 1,
                EffectiveFrom = new DateTime(2017, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
                LearnAimRef = "learnAimRef",
            };

            var larsAnnualValues = NewService().LARSAnnualValueFromEntity(entity);

            larsAnnualValues.BasicSkillsType.Should().Be(entity.BasicSkillsType);
            larsAnnualValues.EffectiveFrom.Should().Be(entity.EffectiveFrom);
            larsAnnualValues.EffectiveTo.Should().Be(entity.EffectiveTo);
            larsAnnualValues.LearnAimRef.Should().Be(entity.LearnAimRef);
        }

        [Fact]
        public void LARSLearningDeliveryCategoriesForLearnAimRefs()
        {
            var lars_LearningDeliveryCategories = new List<LARS_LearningDeliveryCategory>()
            {
                new LARS_LearningDeliveryCategory()
                {
                    LearnAimRef = "123",
                },
                new LARS_LearningDeliveryCategory()
                {
                    LearnAimRef = "456",
                },
                new LARS_LearningDeliveryCategory()
                {
                    LearnAimRef = "123",
                },
                new LARS_LearningDeliveryCategory()
                {
                    LearnAimRef = "789",
                },
                new LARS_LearningDeliveryCategory(),
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSLearningDeliveryCategories).Returns(lars_LearningDeliveryCategories);

            var learnAimRefs = new List<string>() { "123", "456", "234" };

            var larsLearningDeliveryCategories = larsDataRetrievalServiceMock.Object.LARSLearningDeliveryCategoriesForLearnAimRefs(learnAimRefs);

            larsLearningDeliveryCategories.Should().HaveCount(2);
            larsLearningDeliveryCategories.Should().ContainKeys("123", "456");
            larsLearningDeliveryCategories["123"].Should().HaveCount(2);
            larsLearningDeliveryCategories["456"].Should().HaveCount(1);
        }

        [Fact]
        public void LARSLearningDeliveryCategoryFromEntity()
        {
            var entity = new LARS_LearningDeliveryCategory()
            {
                CategoryRef = 1,
                EffectiveFrom = new DateTime(2017, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
                LearnAimRef = "learnAimRef",
            };

            var larsLearningDeliveryCategory = NewService().LARSLearningDeliveryCategoryFromEntity(entity);

            larsLearningDeliveryCategory.CategoryRef.Should().Be(entity.CategoryRef);
            larsLearningDeliveryCategory.EffectiveFrom.Should().Be(entity.EffectiveFrom);
            larsLearningDeliveryCategory.EffectiveTo.Should().Be(entity.EffectiveTo);
            larsLearningDeliveryCategory.LearnAimRef.Should().Be(entity.LearnAimRef);
        }

        [Fact]
        public void LARSFrameworkAimsForLearnAimRefs()
        {
            var lars_FrameworkAims = new List<LARS_FrameworkAims>()
            {
                new LARS_FrameworkAims()
                {
                    LearnAimRef = "123",
                },
                new LARS_FrameworkAims()
                {
                    LearnAimRef = "456",
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

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSFrameworkAims).Returns(lars_FrameworkAims);

            var learnAimRefs = new List<string>() { "123", "456", "234" };

            var larsFrameworkAims = larsDataRetrievalServiceMock.Object.LARSFrameworkAimsForLearnAimRefs(learnAimRefs);

            larsFrameworkAims.Should().HaveCount(2);
            larsFrameworkAims.Should().ContainKeys("123", "456");
            larsFrameworkAims["123"].Should().HaveCount(2);
            larsFrameworkAims["456"].Should().HaveCount(1);
        }

        [Fact]
        public void LARSFrameworkAimsForLearnAimRefs_MixedCase()
        {
            var lars_FrameworkAims = new List<LARS_FrameworkAims>()
            {
                new LARS_FrameworkAims()
                {
                    LearnAimRef = "AAA",
                },
                new LARS_FrameworkAims()
                {
                    LearnAimRef = "AAA",
                },
                new LARS_FrameworkAims()
                {
                    LearnAimRef = "Aa",
                },
                new LARS_FrameworkAims()
                {
                    LearnAimRef = "AAb",
                },
                new LARS_FrameworkAims(),
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSFrameworkAims).Returns(lars_FrameworkAims);

            var learnAimRefs = new List<string>() { "AAA", "AA", "AAb", "AAB" }.ToCaseInsensitiveHashSet();

            var larsFrameworkAims = larsDataRetrievalServiceMock.Object.LARSFrameworkAimsForLearnAimRefs(learnAimRefs);

            larsFrameworkAims.Should().HaveCount(3);
            larsFrameworkAims.Should().ContainKeys("AAA", "Aa", "AAb");
            larsFrameworkAims["AAA"].Should().HaveCount(2);
            larsFrameworkAims["AAA"].Should().HaveCount(2);
            larsFrameworkAims["AAb"].Should().HaveCount(1);
        }

        [Fact]
        public void LARSFrameworkAimFromEntity()
        {
            var entity = new LARS_FrameworkAims()
            {
                EffectiveFrom = new DateTime(2017, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
                FrameworkComponentType = 1,
                FworkCode = 2,
                LearnAimRef = "learnAimRef",
                ProgType = 3,
                PwayCode = 4,
            };

            var larsFrameworkAims = NewService().LARSFrameworkAimsFromEntity(entity);

            larsFrameworkAims.EffectiveFrom.Should().Be(entity.EffectiveFrom);
            larsFrameworkAims.EffectiveTo.Should().Be(entity.EffectiveTo);
            larsFrameworkAims.FrameworkComponentType.Should().Be(entity.FrameworkComponentType);
            larsFrameworkAims.FworkCode.Should().Be(entity.FworkCode);
            larsFrameworkAims.LearnAimRef.Should().Be(entity.LearnAimRef);
            larsFrameworkAims.ProgType.Should().Be(entity.ProgType);
            larsFrameworkAims.PwayCode.Should().Be(entity.PwayCode);
        }

        [Fact]
        public void LARSStandardCommonComponentForStandardCode()
        {
            var lars_StandardCommonComponents = new List<LARS_StandardCommonComponent>()
            {
                new LARS_StandardCommonComponent()
                {
                    StandardCode = 123,
                },
                new LARS_StandardCommonComponent()
                {
                    StandardCode = 456,
                },
                new LARS_StandardCommonComponent()
                {
                    StandardCode = 123,
                },
                new LARS_StandardCommonComponent()
                {
                    StandardCode = 789,
                },
                new LARS_StandardCommonComponent(),
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSStandardCommonComponents).Returns(lars_StandardCommonComponents);

            var standardCodes = new List<int>() { 123, 456, 234 };

            var larsStandardCommonComponents = larsDataRetrievalServiceMock.Object.LARSStandardCommonComponentForStandardCode(standardCodes);

            larsStandardCommonComponents.Should().HaveCount(2);
            larsStandardCommonComponents.Should().ContainKeys(123, 456);
            larsStandardCommonComponents[123].Should().HaveCount(2);
            larsStandardCommonComponents[456].Should().HaveCount(1);
        }

        [Fact]
        public void LARSStandardCommonComponentFromEntity()
        {
            var entity = new LARS_StandardCommonComponent()
            {
                EffectiveFrom = new DateTime(2017, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
                StandardCode = 1,
                CommonComponent = 2,
            };

            var larsStandardCommonComponent = NewService().LARSStandardCommonComponentFromEntity(entity);

            larsStandardCommonComponent.EffectiveFrom.Should().Be(entity.EffectiveFrom);
            larsStandardCommonComponent.EffectiveTo.Should().Be(entity.EffectiveTo);
            larsStandardCommonComponent.StandardCode.Should().Be(entity.StandardCode);
            larsStandardCommonComponent.CommonComponent.Should().Be(entity.CommonComponent);
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
                    LearnAimRef = "123",
                    FrameworkCommonComponent = 1
                },
                new LARS_LearningDelivery()
                {
                    LearnAimRef = "456",
                    FrameworkCommonComponent = 2
                }
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSLearningDeliveries).Returns(lars_LearningDeliveries);
            larsDataRetrievalServiceMock.SetupGet(l => l.LARSFrameworkCommonComponents).Returns(lars_FrameworkCommonComponents);

            var frameworkKeys = new List<LARSFrameworkKey>
            {
                new LARSFrameworkKey("123", 2, 3, 4),
                new LARSFrameworkKey("123", 3, 3, 4),
                new LARSFrameworkKey("456", 2, 3, 4)
            };

            var larsFramworkCommonComponents = larsDataRetrievalServiceMock.Object.LARSFrameworkCommonComponentForLearnAimRefs(frameworkKeys);

            larsFramworkCommonComponents.Should().HaveCount(3);
            larsFramworkCommonComponents.Select(l => l.LearnAimRef).ToList().Should().Contain("123", "456");
        }

        [Fact]
        public void LARSApprenticeshipFundingStandards()
        {
            var lars_ApprenticeshipFunding = new List<LARS_ApprenticeshipFunding>()
            {
                new LARS_ApprenticeshipFunding()
                {
                    ApprenticeshipType = "STD",
                    ApprenticeshipCode = 1,
                    ProgType = 2,
                    PwayCode = 0,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
               new LARS_ApprenticeshipFunding()
                {
                    ApprenticeshipType = "STD",
                    ApprenticeshipCode = 1,
                    ProgType = 2,
                    PwayCode = 4,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
               new LARS_ApprenticeshipFunding()
                {
                    ApprenticeshipType = "STD",
                    ApprenticeshipCode = 1,
                    ProgType = 4,
                    PwayCode = 0,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new LARS_ApprenticeshipFunding(),
                new LARS_ApprenticeshipFunding()
                {
                    ApprenticeshipType = "FWK",
                    ApprenticeshipCode = 1,
                    ProgType = 4,
                    PwayCode = 3,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSApprenticeshipFundings).Returns(lars_ApprenticeshipFunding);

            var keys = new List<LARSApprenticeshipFundingKey>
            {
                new LARSApprenticeshipFundingKey(1, 2, 0),
                new LARSApprenticeshipFundingKey(1, 4, 0)
            };

            var larsApprenticeshipFundingStandards = larsDataRetrievalServiceMock.Object.LARSApprenticeshipFundingStandards(keys);

            larsApprenticeshipFundingStandards.Should().HaveCount(2);
        }

        [Fact]
        public void LARSStandardApprenticeshipFundingFromEntity()
        {
            var entity = new LARS_ApprenticeshipFunding()
            {
                EffectiveFrom = new DateTime(2017, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
                ApprenticeshipType = "STD",
                ApprenticeshipCode = 1,
                ProgType = 3,
                PwayCode = 0,
            };

            var larsApprenticeshipFunding = NewService().LARSStandardApprenticeshipFundingFromEntity(entity);

            larsApprenticeshipFunding.EffectiveFrom.Should().Be(entity.EffectiveFrom);
            larsApprenticeshipFunding.EffectiveTo.Should().Be(entity.EffectiveTo);
            larsApprenticeshipFunding.ApprenticeshipType.Should().Be(entity.ApprenticeshipType);
            larsApprenticeshipFunding.ApprenticeshipCode.Should().Be(entity.ApprenticeshipCode);
            larsApprenticeshipFunding.ProgType.Should().Be(entity.ProgType);
            larsApprenticeshipFunding.PwayCode.Should().Be(entity.PwayCode);
        }

        [Fact]
        public void LARSApprenticeshipFundingFrameworks()
        {
            var lars_ApprenticeshipFunding = new List<LARS_ApprenticeshipFunding>()
            {
                new LARS_ApprenticeshipFunding()
                {
                    ApprenticeshipType = "STD",
                    ApprenticeshipCode = 1,
                    ProgType = 2,
                    PwayCode = 0,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
               new LARS_ApprenticeshipFunding()
                {
                    ApprenticeshipType = "STD",
                    ApprenticeshipCode = 1,
                    ProgType = 2,
                    PwayCode = 4,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
               new LARS_ApprenticeshipFunding()
                {
                    ApprenticeshipType = "FWK",
                    ApprenticeshipCode = 1,
                    ProgType = 4,
                    PwayCode = 0,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
                new LARS_ApprenticeshipFunding(),
                new LARS_ApprenticeshipFunding()
                {
                    ApprenticeshipType = "FWK",
                    ApprenticeshipCode = 1,
                    ProgType = 4,
                    PwayCode = 3,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                },
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSApprenticeshipFundings).Returns(lars_ApprenticeshipFunding);

            var keys = new List<LARSApprenticeshipFundingKey>
            {
                new LARSApprenticeshipFundingKey(1, 4, 3)
            };

            var larsApprenticeshipFundingFrameworks = larsDataRetrievalServiceMock.Object.LARSApprenticeshipFundingFrameworks(keys);

            larsApprenticeshipFundingFrameworks.Should().HaveCount(1);
        }

        [Fact]
        public void LARSFrameworkApprenticeshipFundingFromEntity()
        {
            var entity = new LARS_ApprenticeshipFunding()
            {
                EffectiveFrom = new DateTime(2017, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
                ApprenticeshipType = "FWK",
                ApprenticeshipCode = 1,
                ProgType = 3,
                PwayCode = 4,
            };

            var larsApprenticeshipFunding = NewService().LARSFrameworkApprenticeshipFundingFromEntity(entity);

            larsApprenticeshipFunding.EffectiveFrom.Should().Be(entity.EffectiveFrom);
            larsApprenticeshipFunding.EffectiveTo.Should().Be(entity.EffectiveTo);
            larsApprenticeshipFunding.ApprenticeshipType.Should().Be(entity.ApprenticeshipType);
            larsApprenticeshipFunding.ApprenticeshipCode.Should().Be(entity.ApprenticeshipCode);
            larsApprenticeshipFunding.ProgType.Should().Be(entity.ProgType);
            larsApprenticeshipFunding.PwayCode.Should().Be(entity.PwayCode);
        }

        [Fact]
        public void LARSStandardFundingForStandardCodes()
        {
            var lars_StandardFunding = new List<LARS_StandardFunding>()
            {
                new LARS_StandardFunding()
                {
                    AchievementIncentive = 1.0m,
                    BandNumber = 1,
                    EffectiveFrom = new DateTime(2017, 1, 1),
                    EffectiveTo = new DateTime(2018, 1, 1),
                    CoreGovContributionCap = 2.0m,
                    FundableWithoutEmployer = "2",
                    FundingCategory = "Category",
                    C1618Incentive = 3.0m,
                    SmallBusinessIncentive = 4.0m,
                    StandardCode = 3
                },
                 new LARS_StandardFunding()
                {
                    AchievementIncentive = 1.0m,
                    BandNumber = 1,
                    EffectiveFrom = new DateTime(2017, 1, 1),
                    EffectiveTo = new DateTime(2018, 1, 1),
                    CoreGovContributionCap = 2.0m,
                    FundableWithoutEmployer = "2",
                    FundingCategory = "Category",
                    C1618Incentive = 3.0m,
                    SmallBusinessIncentive = 4.0m,
                    StandardCode = 44
                },
                new LARS_StandardFunding()
                {
                    AchievementIncentive = 1.0m,
                    BandNumber = 1,
                    EffectiveFrom = new DateTime(2017, 1, 1),
                    EffectiveTo = new DateTime(2018, 1, 1),
                    CoreGovContributionCap = 2.0m,
                    FundableWithoutEmployer = "2",
                    FundingCategory = "Category",
                    C1618Incentive = 3.0m,
                    SmallBusinessIncentive = 4.0m,
                    StandardCode = 3
                },
                new LARS_StandardFunding()
                {
                    AchievementIncentive = 1.0m,
                    BandNumber = 1,
                    EffectiveFrom = new DateTime(2017, 1, 1),
                    EffectiveTo = new DateTime(2018, 1, 1),
                    CoreGovContributionCap = 2.0m,
                    FundableWithoutEmployer = "2",
                    FundingCategory = "Category",
                    C1618Incentive = 3.0m,
                    SmallBusinessIncentive = 4.0m,
                    StandardCode = 55
                }
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSStandardFundings).Returns(lars_StandardFunding);

            var standardCodes = new List<int> { 3, 44 };

            var larsStamdardFundings = larsDataRetrievalServiceMock.Object.LARSStandardFundingForStandardCodes(standardCodes);

            larsStamdardFundings.Should().HaveCount(2);
            larsStamdardFundings.SelectMany(l => l.Value).Should().HaveCount(3);
        }

        [Fact]
        public void LARSStandardFundingFromEntity()
        {
            var entity = new LARS_StandardFunding()
            {
                AchievementIncentive = 1.0m,
                BandNumber = 1,
                EffectiveFrom = new DateTime(2017, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
                CoreGovContributionCap = 2.0m,
                FundableWithoutEmployer = "2",
                FundingCategory = "Category",
                C1618Incentive = 3.0m,
                SmallBusinessIncentive = 4.0m,
                StandardCode = 3
            };

            var larsApprenticeshipFunding = NewService().LARSStandardFundingFromEntity(entity);

            larsApprenticeshipFunding.AchievementIncentive.Should().Be(entity.AchievementIncentive);
            larsApprenticeshipFunding.BandNumber.Should().Be(entity.BandNumber);
            larsApprenticeshipFunding.CoreGovContributionCap.Should().Be(entity.CoreGovContributionCap);
            larsApprenticeshipFunding.EffectiveFrom.Should().Be(entity.EffectiveFrom);
            larsApprenticeshipFunding.EffectiveTo.Should().Be(entity.EffectiveTo);
            larsApprenticeshipFunding.FundableWithoutEmployer.Should().Be(entity.FundableWithoutEmployer);
            larsApprenticeshipFunding.FundingCategory.Should().Be(entity.FundingCategory);
            larsApprenticeshipFunding.SixteenToEighteenIncentive.Should().Be(entity.C1618Incentive);
            larsApprenticeshipFunding.SmallBusinessIncentive.Should().Be(entity.SmallBusinessIncentive);
            larsApprenticeshipFunding.StandardCode.Should().Be(entity.StandardCode);
        }

        private LARSDataRetrievalService NewService(ILARS lars = null)
        {
            return new LARSDataRetrievalService(lars);
        }

        private Mock<LARSDataRetrievalService> NewMock()
        {
            return new Mock<LARSDataRetrievalService>();
        }
    }
}

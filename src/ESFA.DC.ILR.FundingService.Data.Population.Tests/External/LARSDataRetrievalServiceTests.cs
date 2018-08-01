﻿using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Population.External;
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
                NotionalNVQLevelv2 = "notionalNVQLevelv2",
                RegulatedCreditValue = 3,
                LARS_Validity = new List<LARS_Validity>()
                {
                    new LARS_Validity(),
                    new LARS_Validity(),
                }
            };

            var larsLearningDelivery = NewService().LARSLearningDeliveryFromEntity(lars_LearningDelivery);

            larsLearningDelivery.EnglPrscID.Should().Be(lars_LearningDelivery.EnglPrscID);
            larsLearningDelivery.EnglandFEHEStatus.Should().Be(lars_LearningDelivery.EnglandFEHEStatus);
            larsLearningDelivery.FrameworkCommonComponent.Should().Be(lars_LearningDelivery.FrameworkCommonComponent);
            larsLearningDelivery.LearnAimRef.Should().Be(lars_LearningDelivery.LearnAimRef);
            larsLearningDelivery.LearnAimRefType.Should().Be(lars_LearningDelivery.LearnAimRefType);
            larsLearningDelivery.NotionalNVQLevelv2.Should().Be(lars_LearningDelivery.NotionalNVQLevelv2);
            larsLearningDelivery.RegulatedCreditValue.Should().Be(lars_LearningDelivery.RegulatedCreditValue);

            larsLearningDelivery.LARSValidities.Should().HaveCount(2);
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

        private LARSDataRetrievalService NewService(ILARS lars = null)
        {
            return  new LARSDataRetrievalService(lars);
        }

        private Mock<LARSDataRetrievalService> NewMock()
        {
            return new Mock<LARSDataRetrievalService>();
        }
    }
}
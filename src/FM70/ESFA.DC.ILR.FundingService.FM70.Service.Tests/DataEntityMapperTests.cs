using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.FundingService.FM70.Service.Input;
using ESFA.DC.ILR.FundingService.FM70.Service.Models;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM70.Service.Tests
{
    public class DataEntityMapperTests
    {
        [Fact]
        public void BuildGlobalDataEntity()
        {
            var global = new Global
            {
                UKPRN = 1234,
            };

            var dataEntity = NewService().BuildGlobalDataEntity(null, global);

            dataEntity.EntityName.Should().Be("global");
            dataEntity.Attributes.Should().HaveCount(1);
            dataEntity.Attributes["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void BuildGlobal()
        {
            var ukprn = 1234;

            var fileDataServiceMock = new Mock<IFileDataService>();

            fileDataServiceMock.Setup(f => f.UKPRN()).Returns(ukprn);

            var global = NewService(
                fileDataService: fileDataServiceMock.Object)
                .BuildGlobal();

            global.UKPRN.Should().Be(ukprn);
        }

        [Fact]
        public void BuildLearner()
        {
            var learner = new TestLearner()
            {
                LearnRefNumber = "ABC",
                DateOfBirthNullable = new DateTime(2000, 8, 1),
                ULN = 1234567890,
            };

            var fileDataDataServiceMock = new Mock<IFileDataService>();

            fileDataDataServiceMock.Setup(dp => dp.DPOutcomesForLearnRefNumber(learner.LearnRefNumber)).Returns(new List<DPOutcome>());

            var dataEntity = NewService(fileDataDataServiceMock.Object).BuildLearnerDataEntity(learner);

            dataEntity.EntityName.Should().Be("Learner");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["LearnRefNumber"].Value.Should().Be(learner.LearnRefNumber);
            dataEntity.Attributes["DateOfBirth"].Value.Should().Be(learner.DateOfBirthNullable);

            dataEntity.Children.Should().BeEmpty();
        }

        [Fact]
        public void BuildLearnerEmploymentStatus()
        {
            var learnerEmploymentStatus = new TestLearnerEmploymentStatus
            {
                AgreeId = "Id",
                DateEmpStatApp = new DateTime(2018, 1, 1),
                EmpIdNullable = 1,
                EmpStat = 2,
            };

            var dataEntity = NewService().BuildLearnerEmploymentStatus(learnerEmploymentStatus);

            dataEntity.EntityName.Should().Be("LearnerEmploymentStatus");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["DateEmpStatApp"].Value.Should().Be(learnerEmploymentStatus.DateEmpStatApp);
            dataEntity.Attributes["EMPStat"].Value.Should().Be(learnerEmploymentStatus.EmpStat);
        }

        [Fact]
        public void BuildDPOutcome()
        {
            var dpOutcome = new DPOutcome
            {
                OutCode = 100,
                OutType = "Type",
                OutCollDate = new DateTime(2018, 1, 1),
                OutStartDate = new DateTime(2018, 1, 1),
            };

            var dataEntity = NewService().BuildDPOutcomes(dpOutcome);

            dataEntity.EntityName.Should().Be("DPOutcome");
            dataEntity.Attributes.Should().HaveCount(5);
            dataEntity.Attributes["OutCode"].Value.Should().Be(dpOutcome.OutCode);
            dataEntity.Attributes["OutType"].Value.Should().Be(dpOutcome.OutType);
            dataEntity.Attributes["OutCollDate"].Value.Should().Be(dpOutcome.OutCollDate);
            dataEntity.Attributes["OutStartDate"].Value.Should().Be(dpOutcome.OutStartDate);
            dataEntity.Attributes["OutEndDate"].Value.Should().Be(dpOutcome.OutEndDate);

            dataEntity.Children.Should().BeNullOrEmpty();
        }

        [Fact]
        public void BuildLearningDelivery()
        {
            var learningDelivery = new TestLearningDelivery()
            {
                AimSeqNumber = 1,
                AimType = 2,
                CompStatus = 3,
                ConRefNumber = "ConRef1",
                DelLocPostCode = "DelLocPostcode",
                FundModel = 4,
                FworkCodeNullable = 5,
                LearnActEndDateNullable = new DateTime(2017, 1, 1),
                LearnAimRef = "LearnAimRef",
                LearnPlanEndDate = new DateTime(2018, 1, 1),
                LearnStartDate = new DateTime(2019, 1, 1),
                OrigLearnStartDateNullable = new DateTime(2019, 1, 1),
                OtherFundAdjNullable = 6,
                OutcomeNullable = 7,
                PriorLearnFundAdjNullable = 8,
                LearningDeliveryFAMs = new List<TestLearningDeliveryFAM>()
                {
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM1" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM2" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM3" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM4" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "RES", LearnDelFAMCode = "RES" },
                },
            };

            var larsLearningDelivery = new LARSLearningDelivery
            {
                AwardOrgCode = "awardOrgCode",
                EFACOFType = 1,
                FrameworkCommonComponent = 2,
                LearnAimRefTitle = "learnAimRefTitle",
                LearnAimRefType = "learnAimRefType",
                RegulatedCreditValue = 3,
                NotionalNVQLevelv2 = "NVQLevel",
                LearningDeliveryGenre = "Genre",
                LARSFunding = new List<LARSFunding>
                {
                    new LARSFunding
                    {
                        FundingCategory = "Matrix",
                        RateWeighted = 1.0m,
                        WeightingFactor = "G",
                        EffectiveFrom = new DateTime(2018, 1, 1),
                        EffectiveTo = new DateTime(2019, 1, 1),
                    },
                },
            };

            var fcsContract = new List<FCSContractAllocation>
            {
                new FCSContractAllocation
                {
                    CalcMethod = 1,
                    FCSContractDeliverables = new List<FCSContractDeliverable>
                    {
                        new FCSContractDeliverable()
                    }
                }
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var fcsReferenceDataMock = new Mock<IFCSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);
            larsReferenceDataServiceMock.Setup(l => l.LARSAnnualValuesForLearnAimRef(learningDelivery.LearnAimRef)).Returns(new List<LARSAnnualValue> { new LARSAnnualValue() });
            postcodesReferenceDataServiceMock.Setup(p => p.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode)).Returns(new List<SfaAreaCost> { new SfaAreaCost() });
            fcsReferenceDataMock.Setup(f => f.FcsContractsForConRef(learningDelivery.ConRefNumber)).Returns(fcsContract);

            var dataEntity = NewService(
                fcsReferenceDataService: fcsReferenceDataMock.Object,
                larsReferenceDataService: larsReferenceDataServiceMock.Object,
                postcodesReferenceDataService: postcodesReferenceDataServiceMock.Object)
                .BuildLearningDeliveryDataEntity(learningDelivery);

            dataEntity.EntityName.Should().Be("LearningDelivery");
            dataEntity.Attributes.Should().HaveCount(20);
            dataEntity.Attributes["AchDate"].Value.Should().Be(learningDelivery.AchDateNullable);
            dataEntity.Attributes["AddHours"].Value.Should().Be(learningDelivery.AddHoursNullable);
            dataEntity.Attributes["AimSeqNumber"].Value.Should().Be(learningDelivery.AimSeqNumber);
            dataEntity.Attributes["CalcMethod"].Value.Should().Be(1);
            dataEntity.Attributes["CompStatus"].Value.Should().Be(learningDelivery.CompStatus);
            dataEntity.Attributes["ConRefNumber"].Value.Should().Be(learningDelivery.ConRefNumber);
            dataEntity.Attributes["GenreCode"].Value.Should().Be(larsLearningDelivery.LearningDeliveryGenre);
            dataEntity.Attributes["LearnActEndDate"].Value.Should().Be(learningDelivery.LearnActEndDateNullable);
            dataEntity.Attributes["LearnAimRef"].Value.Should().Be(learningDelivery.LearnAimRef);
            dataEntity.Attributes["LearnPlanEndDate"].Value.Should().Be(learningDelivery.LearnPlanEndDate);
            dataEntity.Attributes["LearnStartDate"].Value.Should().Be(learningDelivery.LearnStartDate);
            dataEntity.Attributes["LrnDelFAM_LDM1"].Value.Should().Be("LDM1");
            dataEntity.Attributes["LrnDelFAM_LDM2"].Value.Should().Be("LDM2");
            dataEntity.Attributes["LrnDelFAM_LDM3"].Value.Should().Be("LDM3");
            dataEntity.Attributes["LrnDelFAM_LDM4"].Value.Should().Be("LDM4");
            dataEntity.Attributes["LrnDelFAM_RES"].Value.Should().Be("RES");
            dataEntity.Attributes["OrigLearnStartDate"].Value.Should().Be(learningDelivery.OrigLearnStartDateNullable);
            dataEntity.Attributes["OtherFundAdj"].Value.Should().Be(learningDelivery.OtherFundAdjNullable);
            dataEntity.Attributes["Outcome"].Value.Should().Be(learningDelivery.OutcomeNullable);
            dataEntity.Attributes["PriorLearnFundAdj"].Value.Should().Be(learningDelivery.PriorLearnFundAdjNullable);
        }

        [Fact]
        public void BuildSFAAreaCost()
        {
            var sfaAreaCost = new SfaAreaCost
            {
                AreaCostFactor = 1.2m,
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
            };

            var dataEntity = NewService().BuildSFAAreaCost(sfaAreaCost);

            dataEntity.EntityName.Should().Be("SFA_PostcodeAreaCost");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["AreaCosFactor"].Value.Should().Be(sfaAreaCost.AreaCostFactor);
            dataEntity.Attributes["AreaCosEffectiveFrom"].Value.Should().Be(sfaAreaCost.EffectiveFrom);
            dataEntity.Attributes["AreaCosEffectiveTo"].Value.Should().Be(sfaAreaCost.EffectiveTo);
        }

        [Fact]
        public void BuildLARSFunding()
        {
            var larsFunding = new LARSFunding
            {
                FundingCategory = "FundingCategory",
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
                RateWeighted = 1.0m,
                RateUnWeighted = 2.0m,
                WeightingFactor = "G",
            };

            var dataEntity = NewService().BuildLARSFunding(larsFunding);

            dataEntity.EntityName.Should().Be("LearningDeliveryLARSFunding");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LARSFundingCategory"].Value.Should().Be(larsFunding.FundingCategory);
            dataEntity.Attributes["LARSFundingEffectiveFrom"].Value.Should().Be(larsFunding.EffectiveFrom);
            dataEntity.Attributes["LARSFundingEffectiveTo"].Value.Should().Be(larsFunding.EffectiveTo);
            dataEntity.Attributes["LARSFundingWeightedRate"].Value.Should().Be(larsFunding.RateWeighted);
        }

        [Fact]
        public void BuildLARSAnnualValue()
        {
            var larsAnnualValue = new LARSAnnualValue
            {
                BasicSkillsType = 1,
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
            };

            var dataEntity = NewService().BuildLARSAnnualValue(larsAnnualValue);

            dataEntity.EntityName.Should().Be("LearningDeliveryAnnualValue");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["LearnDelAnnValBasicSkillsTypeCode"].Value.Should().Be(larsAnnualValue.BasicSkillsType);
            dataEntity.Attributes["LearnDelAnnValDateFrom"].Value.Should().Be(larsAnnualValue.EffectiveFrom);
            dataEntity.Attributes["LearnDelAnnValDateTo"].Value.Should().Be(larsAnnualValue.EffectiveTo);
        }

        [Fact]
        public void BuildEsfDataEntity()
        {
            var esfData = new EsfData
            {
                CalcMethod = 1,
                EffectiveContractStartDate = new DateTime(2018, 1, 1),
                EffectiveContractEndDate = new DateTime(2019, 1, 1),
                ESFDataPremiumFactor = 1.0m,
                ESFDeliverableCode = "1",
                UnitCost = 1.2m
            };

            var dataEntity = NewService().BuildEsfDataEntity(esfData);

            dataEntity.EntityName.Should().Be("ESFData");
            dataEntity.Attributes.Should().HaveCount(5);
            dataEntity.Attributes["EffectiveContractStartDate"].Value.Should().Be(esfData.EffectiveContractStartDate);
            dataEntity.Attributes["EffectiveContractEndDate"].Value.Should().Be(esfData.EffectiveContractEndDate);
            dataEntity.Attributes["ESFDataPremiumFactor"].Value.Should().Be(esfData.ESFDataPremiumFactor);
            dataEntity.Attributes["ESFDeliverableCode"].Value.Should().Be(esfData.ESFDeliverableCode);
            dataEntity.Attributes["UnitCost"].Value.Should().Be(esfData.UnitCost);
        }

        [Fact]
        public void BuildEsfDataFromContract()
        {
            var fcsContract = new List<FCSContractAllocation>
            {
                new FCSContractAllocation
                {
                    CalcMethod = 1,
                    FCSContractDeliverables = new List<FCSContractDeliverable>
                    {
                        new FCSContractDeliverable(),
                        new FCSContractDeliverable()
                    }
                }
            };

            var esfData = NewService().BuildEsfDataFromContract(fcsContract);

            esfData.Should().HaveCount(2);
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized()
        {
            var learningDeliveryFams = new List<TestLearningDeliveryFAM>()
            {
                new TestLearningDeliveryFAM() { LearnDelFAMType = "RES", LearnDelFAMCode = "1" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "2" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "3" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "4" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "5" },
            };

            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(learningDeliveryFams);

            learningDeliveryFAMDenormalized.RES.Should().Be("1");
            learningDeliveryFAMDenormalized.LDM1.Should().Be("2");
            learningDeliveryFAMDenormalized.LDM2.Should().Be("3");
            learningDeliveryFAMDenormalized.LDM3.Should().Be("4");
            learningDeliveryFAMDenormalized.LDM4.Should().Be("5");
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized_Null()
        {
            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(null);

            learningDeliveryFAMDenormalized.RES.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM1.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM2.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM3.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM4.Should().BeNull();
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized_NoMatches()
        {
            var learningDeliveryFams = new List<TestLearningDeliveryFAM>();

            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(learningDeliveryFams);

            learningDeliveryFAMDenormalized.RES.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM1.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM2.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM3.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM4.Should().BeNull();
        }

        private DataEntityMapper NewService(
            IFileDataService fileDataService = null,
            IFCSReferenceDataService fcsReferenceDataService = null,
            ILARSReferenceDataService larsReferenceDataService = null,
            IPostcodesReferenceDataService postcodesReferenceDataService = null)
        {
            return new DataEntityMapper(fileDataService, fcsReferenceDataService, larsReferenceDataService, postcodesReferenceDataService);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.FundingService.FM81.Service.Input;
using ESFA.DC.ILR.FundingService.FM81.Service.Model;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM81.Service.Tests
{
    public class DataEntityMapperTests
    {
        [Fact]
        public void BuildGlobalDataEntity()
        {
            var global = new Global
            {
                UKPRN = 1234,
                LARSVersion = "Version"
            };

            var dataEntity = NewService().BuildGlobalDataEntity(null, global);

            dataEntity.EntityName.Should().Be("global");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["UKPRN"].Value.Should().Be(global.UKPRN);
            dataEntity.Attributes["LARSVersion"].Value.Should().Be(global.LARSVersion);
        }

        [Fact]
        public void BuildGlobal()
        {
            var larsCurrentVersion = "1.0.0";
            var ukprn = 1234;

            var larsRefererenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var fileDataServiceMock = new Mock<IFileDataService>();

            larsRefererenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsCurrentVersion);
            fileDataServiceMock.Setup(f => f.UKPRN()).Returns(ukprn);

            var global = NewService(larsRefererenceDataServiceMock.Object, fileDataService: fileDataServiceMock.Object).BuildGlobal();

            global.LARSVersion.Should().Be(larsCurrentVersion);
            global.UKPRN.Should().Be(ukprn);
        }

        [Fact]
        public void BuildLearner()
        {
            var learner = new TestLearner()
            {
                LearnRefNumber = "ABC",
                DateOfBirthNullable = new DateTime(2000, 8, 1),
            };

            var dataEntity = NewService().BuildLearnerDataEntity(learner);

            dataEntity.EntityName.Should().Be("Learner");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["LearnRefNumber"].Value.Should().Be(learner.LearnRefNumber);
            dataEntity.Attributes["DateOfBirth"].Value.Should().Be(learner.DateOfBirthNullable);

            dataEntity.Children.Should().BeEmpty();
        }

        [Fact]
        public void BuildLearnerEmploymentStatus()
        {
            var learnerEmploymentStatus = new LearnerEmploymentStatusDenormalized
            {
                DateEmpStatApp = new DateTime(2018, 1, 1),
                EmpId = 1,
                EMPStat = 2,
                SEM = 3
            };

            var dataEntity = NewService().BuildLearnerEmploymentStatus(learnerEmploymentStatus);

            dataEntity.EntityName.Should().Be("LearnerEmploymentStatus");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["DateEmpStatApp"].Value.Should().Be(learnerEmploymentStatus.DateEmpStatApp);
            dataEntity.Attributes["EmpId"].Value.Should().Be(learnerEmploymentStatus.EmpId);
            dataEntity.Attributes["EMPStat"].Value.Should().Be(learnerEmploymentStatus.EMPStat);
            dataEntity.Attributes["EmpStatMon_SEM"].Value.Should().Be(learnerEmploymentStatus.SEM);
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
                StdCodeNullable = 9,
                LearningDeliveryFAMs = new List<TestLearningDeliveryFAM>()
                {
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM1" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM2" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM3" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM4" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "EEF", LearnDelFAMCode = "EEF" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "FFI", LearnDelFAMCode = "FFI" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "RES", LearnDelFAMCode = "RES" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "SOF", LearnDelFAMCode = "SOF" },
                    new TestLearningDeliveryFAM() { LearnDelFAMType = "SPP", LearnDelFAMCode = "SPP" },
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

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);
            larsReferenceDataServiceMock.Setup(l => l.LARSStandardCommonComponent(learningDelivery.StdCodeNullable)).Returns(new List<LARSStandardCommonComponent> { new LARSStandardCommonComponent() });
            larsReferenceDataServiceMock.Setup(l => l.LARSStandardFunding(learningDelivery.StdCodeNullable)).Returns(new List<LARSStandardFunding> { new LARSStandardFunding() });

            var dataEntity = NewService(larsReferenceDataService: larsReferenceDataServiceMock.Object)
                .BuildLearningDeliveryDataEntity(learningDelivery);

            dataEntity.EntityName.Should().Be("LearningDelivery");
            dataEntity.Attributes.Should().HaveCount(25);
            dataEntity.Attributes["AchDate"].Value.Should().Be(learningDelivery.AchDateNullable);
            dataEntity.Attributes["AimSeqNumber"].Value.Should().Be(learningDelivery.AimSeqNumber);
            dataEntity.Attributes["AimType"].Value.Should().Be(learningDelivery.AimType);
            dataEntity.Attributes["CompStatus"].Value.Should().Be(learningDelivery.CompStatus);
            dataEntity.Attributes["FrameworkCommonComponent"].Value.Should().Be(larsLearningDelivery.FrameworkCommonComponent);
            dataEntity.Attributes["LearnAimRef"].Value.Should().Be(learningDelivery.LearnAimRef);
            dataEntity.Attributes["LearnActEndDate"].Value.Should().Be(learningDelivery.LearnActEndDateNullable);
            dataEntity.Attributes["LearnPlanEndDate"].Value.Should().Be(learningDelivery.LearnPlanEndDate);
            dataEntity.Attributes["LearnStartDate"].Value.Should().Be(learningDelivery.LearnStartDate);
            dataEntity.Attributes["LrnDelFAM_EEF"].Value.Should().Be("EEF");
            dataEntity.Attributes["LrnDelFAM_FFI"].Value.Should().Be("FFI");
            dataEntity.Attributes["LrnDelFAM_LDM1"].Value.Should().Be("LDM1");
            dataEntity.Attributes["LrnDelFAM_LDM2"].Value.Should().Be("LDM2");
            dataEntity.Attributes["LrnDelFAM_LDM3"].Value.Should().Be("LDM3");
            dataEntity.Attributes["LrnDelFAM_LDM4"].Value.Should().Be("LDM4");
            dataEntity.Attributes["LrnDelFAM_RES"].Value.Should().Be("RES");
            dataEntity.Attributes["LrnDelFAM_SOF"].Value.Should().Be("SOF");
            dataEntity.Attributes["LrnDelFAM_SPP"].Value.Should().Be("SPP");
            dataEntity.Attributes["OrigLearnStartDate"].Value.Should().Be(learningDelivery.OrigLearnStartDateNullable);
            dataEntity.Attributes["OtherFundAdj"].Value.Should().Be(learningDelivery.OtherFundAdjNullable);
            dataEntity.Attributes["Outcome"].Value.Should().Be(learningDelivery.OutcomeNullable);
            dataEntity.Attributes["PriorLearnFundAdj"].Value.Should().Be(learningDelivery.PriorLearnFundAdjNullable);
            dataEntity.Attributes["ProgType"].Value.Should().Be(learningDelivery.ProgTypeNullable);
            dataEntity.Attributes["STDCode"].Value.Should().Be(learningDelivery.StdCodeNullable);
            dataEntity.Attributes["WithdrawReason"].Value.Should().Be(learningDelivery.WithdrawReasonNullable);
        }

        [Fact]
        public void BuildLearningDeliveryFAM()
        {
            var learningDeliveryFAM = new TestLearningDeliveryFAM
            {
                LearnDelFAMType = "EEF",
                LearnDelFAMCode = "1"
            };

            var dataEntity = NewService().BuildLearningDeliveryFAM(learningDeliveryFAM);

            dataEntity.EntityName.Should().Be("LearningDeliveryFAM");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LearnDelFAMCode"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMCode);
            dataEntity.Attributes["LearnDelFAMDateFrom"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMDateFromNullable);
            dataEntity.Attributes["LearnDelFAMDateTo"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMDateToNullable);
            dataEntity.Attributes["LearnDelFAMType"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMType);
        }

        [Fact]
        public void BuildApprenticeshipFinancialRecord()
        {
            var appFinRecord = new TestAppFinRecord
            {
                AFinCode = 1,
                AFinAmount = 2,
                AFinDate = new DateTime(2018, 8, 1),
                AFinType = "Type"
            };

            var dataEntity = NewService().BuildApprenticeshipFinancialRecord(appFinRecord);

            dataEntity.EntityName.Should().Be("ApprenticeshipFinancialRecord");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["AFinAmount"].Value.Should().Be(appFinRecord.AFinAmount);
            dataEntity.Attributes["AFinCode"].Value.Should().Be(appFinRecord.AFinCode);
            dataEntity.Attributes["AFinDate"].Value.Should().Be(appFinRecord.AFinDate);
            dataEntity.Attributes["AFinType"].Value.Should().Be(appFinRecord.AFinType);
        }

        [Fact]
        public void BuildLARSStandardCommonComponent()
        {
            var larsStandardCommonComponent = new LARSStandardCommonComponent
            {
                CommonComponent = 1,
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
                StandardCode = 2,
            };

            var dataEntity = NewService().BuildLARSStandardCommonComponent(larsStandardCommonComponent);

            dataEntity.EntityName.Should().Be("LARS_StandardCommonComponent");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LARSCommonComponent"].Value.Should().Be(larsStandardCommonComponent.CommonComponent);
            dataEntity.Attributes["LARSEffectiveFrom"].Value.Should().Be(larsStandardCommonComponent.EffectiveFrom);
            dataEntity.Attributes["LARSEffectiveTo"].Value.Should().Be(larsStandardCommonComponent.EffectiveTo);
            dataEntity.Attributes["LARSStandardCode"].Value.Should().Be(larsStandardCommonComponent.StandardCode);
        }

        [Fact]
        public void BuildLARSStandardFunding()
        {
            var larsStandardFunding = new LARSStandardFunding
            {
                FundableWithoutEmployer = "Fundable",
                EffectiveFrom = new DateTime(2018, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
                AchievementIncentive = 1,
                SixteenToEighteenIncentive = 2,
                CoreGovContributionCap = 1.0m,
                SmallBusinessIncentive = 2.0m
            };

            var dataEntity = NewService().BuildLARSStandardFunding(larsStandardFunding);

            dataEntity.EntityName.Should().Be("LARS_StandardFunding");
            dataEntity.Attributes.Should().HaveCount(7);
            dataEntity.Attributes["FundableWithoutEmployer"].Value.Should().Be(larsStandardFunding.FundableWithoutEmployer);
            dataEntity.Attributes["SF1618Incentive"].Value.Should().Be(larsStandardFunding.SixteenToEighteenIncentive);
            dataEntity.Attributes["SFAchIncentive"].Value.Should().Be(larsStandardFunding.AchievementIncentive);
            dataEntity.Attributes["SFCoreGovContCap"].Value.Should().Be(larsStandardFunding.CoreGovContributionCap);
            dataEntity.Attributes["SFEffectiveFromDate"].Value.Should().Be(larsStandardFunding.EffectiveFrom);
            dataEntity.Attributes["SFEffectiveToDate"].Value.Should().Be(larsStandardFunding.EffectiveTo);
            dataEntity.Attributes["SFSmallBusIncentive"].Value.Should().Be(larsStandardFunding.SmallBusinessIncentive);
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
                new TestLearningDeliveryFAM() { LearnDelFAMType = "EEF", LearnDelFAMCode = "6" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "FFI", LearnDelFAMCode = "7" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "SOF", LearnDelFAMCode = "8" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "SPP", LearnDelFAMCode = "9" },
            };

            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(learningDeliveryFams);

            learningDeliveryFAMDenormalized.RES.Should().Be("1");
            learningDeliveryFAMDenormalized.LDM1.Should().Be("2");
            learningDeliveryFAMDenormalized.LDM2.Should().Be("3");
            learningDeliveryFAMDenormalized.LDM3.Should().Be("4");
            learningDeliveryFAMDenormalized.LDM4.Should().Be("5");
            learningDeliveryFAMDenormalized.EEF.Should().Be("6");
            learningDeliveryFAMDenormalized.FFI.Should().Be("7");
            learningDeliveryFAMDenormalized.SOF.Should().Be("8");
            learningDeliveryFAMDenormalized.SPP.Should().Be("9");
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
            learningDeliveryFAMDenormalized.EEF.Should().BeNull();
            learningDeliveryFAMDenormalized.FFI.Should().BeNull();
            learningDeliveryFAMDenormalized.SOF.Should().BeNull();
            learningDeliveryFAMDenormalized.SPP.Should().BeNull();
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
            learningDeliveryFAMDenormalized.EEF.Should().BeNull();
            learningDeliveryFAMDenormalized.FFI.Should().BeNull();
            learningDeliveryFAMDenormalized.SOF.Should().BeNull();
            learningDeliveryFAMDenormalized.SPP.Should().BeNull();
        }

        [Fact]
        public void BuildLearnerEmploymentStatusDenormalized()
        {
            var learnerEmploymentStatus = new List<TestLearnerEmploymentStatus>()
            {
                new TestLearnerEmploymentStatus
                {
                    AgreeId = "Id",
                    DateEmpStatApp = new DateTime(2018, 8, 1),
                    EmpStat = 1,
                    EmploymentStatusMonitorings = new List<TestEmploymentStatusMonitoring>
                    {
                        new TestEmploymentStatusMonitoring
                        {
                            ESMCode = 1,
                            ESMType = "SEM"
                        }
                    }
                },
                new TestLearnerEmploymentStatus
                {
                    AgreeId = "Id",
                    DateEmpStatApp = new DateTime(2018, 8, 1),
                    EmpStat = 1,
                    EmploymentStatusMonitorings = new List<TestEmploymentStatusMonitoring>
                    {
                        new TestEmploymentStatusMonitoring
                        {
                            ESMCode = 1,
                            ESMType = "SEM"
                        }
                    }
                },
                 new TestLearnerEmploymentStatus
                {
                    AgreeId = "Id",
                    DateEmpStatApp = new DateTime(2018, 8, 1),
                    EmpStat = 1,
                    EmploymentStatusMonitorings = new List<TestEmploymentStatusMonitoring>
                    {
                        new TestEmploymentStatusMonitoring
                        {
                            ESMCode = 1,
                            ESMType = "DEE"
                        }
                    }
                },
            };

            var learnerEmploymentStatusDenormalized = NewService().BuildLearnerEmploymentStatusDenormalized(learnerEmploymentStatus);

            learnerEmploymentStatusDenormalized.Should().HaveCount(3);
            learnerEmploymentStatusDenormalized.ToArray()[0].SEM.Should().Be(1);
            learnerEmploymentStatusDenormalized.ToArray()[1].SEM.Should().Be(1);
            learnerEmploymentStatusDenormalized.ToArray()[2].SEM.Should().Be(null);
        }

        [Fact]
        public void BuildLearnerEmploymentStatusDenormalized_Null()
        {
            var learnerEmploymentStatusDenormalized = NewService().BuildLearnerEmploymentStatusDenormalized(null);

            learnerEmploymentStatusDenormalized.Should().BeNullOrEmpty();
        }

        private DataEntityMapper NewService(
            ILARSReferenceDataService larsReferenceDataService = null,
            IFileDataService fileDataService = null)
        {
            return new DataEntityMapper(larsReferenceDataService, fileDataService);
        }
    }
}
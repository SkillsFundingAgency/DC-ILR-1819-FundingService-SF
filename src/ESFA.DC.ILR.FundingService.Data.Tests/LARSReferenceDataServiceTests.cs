using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LARS;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Tests
{
    public class LARSReferenceDataServiceTests
    {
        [Fact]
        public void LARSCurrentVersion()
        {
            var currentVersion = "version";

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSCurrentVersion).Returns(currentVersion);

            NewService(referenceDataCacheMock.Object).LARSCurrentVersion().Should().Be(currentVersion);
        }

        [Fact]
        public void LARSLearningDelivery()
        {
            var learnAimRef = "learnAimRef";
            var learningDelivery = new LARSLearningDelivery();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSLearningDelivery)
                .Returns(
                    new Dictionary<string, LARSLearningDelivery>()
                    {
                        { learnAimRef, learningDelivery }
                    });

            NewService(referenceDataCacheMock.Object).LARSLearningDeliveryForLearnAimRef(learnAimRef).Should().Be(learningDelivery);
        }

        [Fact]
        public void LARSLearningDelivery_NotExist()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSLearningDelivery)
                .Returns(
                    new Dictionary<string, LARSLearningDelivery>()
                    {
                        { "learnAimRef", null }
                    });

            NewService(referenceDataCacheMock.Object).LARSLearningDeliveryForLearnAimRef("notLearnAimRef").Should().BeNull();
        }

        [Fact]
        public void LARSFunding_Exists()
        {
            var learnAimRef = "learnAimRef";
            var larsFundings = new List<LARSFunding>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSFunding)
                .Returns(new Dictionary<string, IEnumerable<LARSFunding>>()
                {
                    { learnAimRef, larsFundings }
                });

            NewService(referenceDataCacheMock.Object).LARSFundingsForLearnAimRef(learnAimRef).Should().BeSameAs(larsFundings);
        }

        [Fact]
        public void LARSFunding_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSFunding)
                .Returns(new Dictionary<string, IEnumerable<LARSFunding>>()
                {
                    { "learnAimRef", null }
                });

            NewService(referenceDataCacheMock.Object).LARSFundingsForLearnAimRef("notLearnAimRef").Should().BeNull();
        }

        [Fact]
        public void LARSAnnualValues()
        {
            var learnAimRef = "learnAimRef";
            var larsAnnualValues = new List<LARSAnnualValue>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSAnnualValue)
                .Returns(new Dictionary<string, IEnumerable<LARSAnnualValue>>()
                {
                    { learnAimRef, larsAnnualValues },
                });

            NewService(referenceDataCacheMock.Object).LARSAnnualValuesForLearnAimRef(learnAimRef).Should().BeSameAs(larsAnnualValues);
        }

        [Fact]
        public void LARSAnnualValues_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSAnnualValue)
                .Returns(new Dictionary<string, IEnumerable<LARSAnnualValue>>()
                {
                    { "learnAimRef", null },
                });

            NewService(referenceDataCacheMock.Object).LARSAnnualValuesForLearnAimRef("notLearnAimRef").Should().BeNull();
        }

        [Fact]
        public void LARSLearningDeliveryCategory()
        {
            var learnAimRef = "learnAimRef";
            var learningDeliveryCategories = new List<LARSLearningDeliveryCategory>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSLearningDeliveryCategory)
                .Returns(new Dictionary<string, IEnumerable<LARSLearningDeliveryCategory>>()
                {
                    { learnAimRef, learningDeliveryCategories },
                });

            NewService(referenceDataCacheMock.Object).LARSLearningDeliveryCategoriesForLearnAimRef(learnAimRef).Should().BeSameAs(learningDeliveryCategories);
        }

        [Fact]
        public void LARSLearningDeliveryCategory_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSLearningDeliveryCategory)
                .Returns(new Dictionary<string, IEnumerable<LARSLearningDeliveryCategory>>()
                {
                    { "learnAimRef", null },
                });

            NewService(referenceDataCacheMock.Object).LARSLearningDeliveryCategoriesForLearnAimRef("notLearnAimRef").Should().BeNull();
        }

        [Fact]
        public void LARSFrameworkAims()
        {
            var learnAimRef = "learnAimRef";
            var frameworkAims = new List<LARSFrameworkAims>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSFrameworkAims).Returns(new Dictionary<string, IEnumerable<LARSFrameworkAims>>()
            {
                { learnAimRef, frameworkAims },
            });

            NewService(referenceDataCacheMock.Object).LARSFFrameworkAimsForLearnAimRef(learnAimRef).Should().BeSameAs(frameworkAims);
        }

        [Fact]
        public void LARSFrameworkAims_CaseMisMatch()
        {
            var learnAimRef = "LEARNAIMREF";
            var frameworkAims = new List<LARSFrameworkAims>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSFrameworkAims).Returns(new Dictionary<string, IEnumerable<LARSFrameworkAims>>(StringComparer.OrdinalIgnoreCase)
            {
                { "LearnAimRef", frameworkAims },
            });

            NewService(referenceDataCacheMock.Object).LARSFFrameworkAimsForLearnAimRef(learnAimRef).Should().BeSameAs(frameworkAims);
        }

        [Fact]
        public void LARSFrameworkAims_Correct()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSFrameworkAims).Returns(new Dictionary<string, IEnumerable<LARSFrameworkAims>>()
            {
                { "learnAimRef", null },
            });

            NewService(referenceDataCacheMock.Object).LARSFFrameworkAimsForLearnAimRef("notLearnAimRef").Should().BeNull();
        }

        [Fact]
        public void LARSStandardApprenticeshipFunding()
        {
            int? stdCode = 1;
            int? progType = 2;
            var standardApprenticeshipFunding = new List<LARSStandardApprenticeshipFunding>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSApprenticeshipFundingStandards).Returns(standardApprenticeshipFunding);

            NewService(referenceDataCacheMock.Object).LARSStandardApprenticeshipFunding(stdCode, progType).Should().BeEquivalentTo(standardApprenticeshipFunding);
        }

        [Fact]
        public void LARSStandardApprenticeshipFunding_NullStdCode()
        {
            int? progType = 2;
            var standardApprenticeshipFunding = new List<LARSStandardApprenticeshipFunding>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSApprenticeshipFundingStandards).Returns(standardApprenticeshipFunding);

            NewService(referenceDataCacheMock.Object).LARSStandardApprenticeshipFunding(null, progType).Should().BeNull();
        }

        [Fact]
        public void LARSStandardApprenticeshipFunding_NullProgType()
        {
            int? stdCode = 1;
            var standardApprenticeshipFunding = new List<LARSStandardApprenticeshipFunding>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSApprenticeshipFundingStandards).Returns(standardApprenticeshipFunding);

            NewService(referenceDataCacheMock.Object).LARSStandardApprenticeshipFunding(stdCode, null).Should().BeNull();
        }

        [Fact]
        public void LARSFrameworkApprenticeshipFunding()
        {
            int? stdCode = 1;
            int? progType = 2;
            int? pwayCode = 3;
            var frameworkApprenticeshipFunding = new List<LARSFrameworkApprenticeshipFunding>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSApprenticeshipFundingFrameworks).Returns(frameworkApprenticeshipFunding);

            NewService(referenceDataCacheMock.Object).LARSFrameworkApprenticeshipFunding(stdCode, progType, pwayCode).Should().BeEquivalentTo(frameworkApprenticeshipFunding);
        }

        [Fact]
        public void LARSFrameworkApprenticeshipFunding_NullStdCode()
        {
            int? progType = 2;
            int? pwayCode = 3;
            var frameworkApprenticeshipFunding = new List<LARSFrameworkApprenticeshipFunding>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSApprenticeshipFundingFrameworks).Returns(frameworkApprenticeshipFunding);

            NewService(referenceDataCacheMock.Object).LARSFrameworkApprenticeshipFunding(null, progType, pwayCode).Should().BeNull();
        }

        [Fact]
        public void LARSFrameworkApprenticeshipFunding_NullProgType()
        {
            int? stdCode = 1;
            int? pwayCode = 3;
            var frameworkApprenticeshipFunding = new List<LARSFrameworkApprenticeshipFunding>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSApprenticeshipFundingFrameworks).Returns(frameworkApprenticeshipFunding);

            NewService(referenceDataCacheMock.Object).LARSFrameworkApprenticeshipFunding(stdCode, null, pwayCode).Should().BeNull();
        }

        [Fact]
        public void LARSFrameworkApprenticeshipFunding_NullPwayCode()
        {
            int? stdCode = 1;
            int? progType = 2;
            var frameworkApprenticeshipFunding = new List<LARSFrameworkApprenticeshipFunding>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSApprenticeshipFundingFrameworks).Returns(frameworkApprenticeshipFunding);

            NewService(referenceDataCacheMock.Object).LARSFrameworkApprenticeshipFunding(stdCode, progType, null).Should().BeNull();
        }

        [Fact]
        public void LARSStandardCommonComponent()
        {
            int? stdCode = 1;
            IEnumerable<LARSStandardCommonComponent> larsStandardCommonComponent = new List<LARSStandardCommonComponent>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSStandardCommonComponent).Returns(new Dictionary<int, IEnumerable<LARSStandardCommonComponent>>()
            {
                { 1, larsStandardCommonComponent },
            });

            NewService(referenceDataCacheMock.Object).LARSStandardCommonComponent(stdCode).Should().BeSameAs(larsStandardCommonComponent);
        }

        [Fact]
        public void LARSStandardCommonComponent_NotExists()
        {
            int? stdCode = 2;
            IEnumerable<LARSStandardCommonComponent> larsStandardCommonComponent = new List<LARSStandardCommonComponent>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSStandardCommonComponent).Returns(new Dictionary<int, IEnumerable<LARSStandardCommonComponent>>()
            {
                { 1, larsStandardCommonComponent },
            });

            NewService(referenceDataCacheMock.Object).LARSStandardCommonComponent(stdCode).Should().BeNull();
        }

        [Fact]
        public void LARSStandardCommonComponent_NotExistsNull()
        {
            IEnumerable<LARSStandardCommonComponent> larsStandardCommonComponent = new List<LARSStandardCommonComponent>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSStandardCommonComponent).Returns(new Dictionary<int, IEnumerable<LARSStandardCommonComponent>>()
            {
                { 1, larsStandardCommonComponent },
            });

            NewService(referenceDataCacheMock.Object).LARSStandardCommonComponent(null).Should().BeNull();
        }

        [Fact]
        public void LARSFrameworkCommonComponent()
        {
            var learnAimRef = "LearnAimRef";
            int? fworkCode = 1;
            int? progType = 2;
            int? pwayCode = 3;
            IEnumerable<LARSFrameworkCommonComponent> larsFrameworkCommonComponent = new List<LARSFrameworkCommonComponent>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSFrameworkCommonComponent).Returns(larsFrameworkCommonComponent);

            NewService(referenceDataCacheMock.Object).LARSFrameworkCommonComponent(learnAimRef, fworkCode, progType, pwayCode).Should().BeEquivalentTo(larsFrameworkCommonComponent);
        }

        [Fact]
        public void LARSFrameworkCommonComponentNull_LearnAimRef()
        {
            int? fworkCode = 1;
            int? progType = 2;
            int? pwayCode = 3;
            IEnumerable<LARSFrameworkCommonComponent> larsFrameworkCommonComponent = new List<LARSFrameworkCommonComponent>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSFrameworkCommonComponent).Returns(larsFrameworkCommonComponent);

            NewService(referenceDataCacheMock.Object).LARSFrameworkCommonComponent(null, fworkCode, progType, pwayCode).Should().BeNull();
        }

        [Fact]
        public void LARSFrameworkCommonComponentNullLearn_FworkCode()
        {
            var learnAimRef = "LearnAimRef";
            int? progType = 2;
            int? pwayCode = 3;
            IEnumerable<LARSFrameworkCommonComponent> larsFrameworkCommonComponent = new List<LARSFrameworkCommonComponent>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSFrameworkCommonComponent).Returns(larsFrameworkCommonComponent);

            NewService(referenceDataCacheMock.Object).LARSFrameworkCommonComponent(learnAimRef, null, progType, pwayCode).Should().BeNull();
        }

        [Fact]
        public void LARSFrameworkCommonComponentNullLearn_ProgType()
        {
            var learnAimRef = "LearnAimRef";
            int? fworkCode = 1;
            int? pwayCode = 3;
            IEnumerable<LARSFrameworkCommonComponent> larsFrameworkCommonComponent = new List<LARSFrameworkCommonComponent>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSFrameworkCommonComponent).Returns(larsFrameworkCommonComponent);

            NewService(referenceDataCacheMock.Object).LARSFrameworkCommonComponent(learnAimRef, fworkCode, null, pwayCode).Should().BeNull();
        }

        [Fact]
        public void LARSFrameworkCommonComponentNullLearn_PwayCode()
        {
            var learnAimRef = "LearnAimRef";
            int? fworkCode = 1;
            int? progType = 2;
            IEnumerable<LARSFrameworkCommonComponent> larsFrameworkCommonComponent = new List<LARSFrameworkCommonComponent>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSFrameworkCommonComponent).Returns(larsFrameworkCommonComponent);

            NewService(referenceDataCacheMock.Object).LARSFrameworkCommonComponent(learnAimRef, fworkCode, progType, null).Should().BeNull();
        }

        [Fact]
        public void LARSStandardFunding()
        {
            int? stdCode = 1;
            IEnumerable<LARSStandardFunding> larsStandardFunding = new List<LARSStandardFunding>
            { new LARSStandardFunding { FundingCategory = "StandardTblazer" } };

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSStandardFundings)
                .Returns(new Dictionary<int, IEnumerable<LARSStandardFunding>>()
                {
                    { 1, larsStandardFunding },
                });

            NewService(referenceDataCacheMock.Object).LARSStandardFunding(stdCode).Should().BeEquivalentTo(larsStandardFunding);
        }

        [Fact]
        public void LARSStandardFunding_NotExists()
        {
            int? stdCode = 1;
            var larsStandardFunding = new List<LARSStandardFunding>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSStandardFundings)
                .Returns(new Dictionary<int, IEnumerable<LARSStandardFunding>>()
                {
                    { 2, larsStandardFunding },
                });

            NewService(referenceDataCacheMock.Object).LARSStandardFunding(stdCode).Should().BeNull();
        }

        [Fact]
        public void LARSStandardFunding_NotExists_Null()
        {
            var stdCode = 1;
            var larsStandardFunding = new List<LARSStandardFunding>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LARSStandardFundings)
                .Returns(new Dictionary<int, IEnumerable<LARSStandardFunding>>()
                {
                    { stdCode, larsStandardFunding },
                });

            NewService(referenceDataCacheMock.Object).LARSStandardFunding(null).Should().BeNull();
        }

        private LARSReferenceDataService NewService(IExternalDataCache referenceDataCache = null)
        {
            return new LARSReferenceDataService(referenceDataCache);
        }
    }
}

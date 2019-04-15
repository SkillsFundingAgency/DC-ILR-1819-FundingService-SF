using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests
{
    public class ExternalDataCachePopulationServiceTests
    {
        [Fact]
        public async Task Populate()
        {
            var message = new TestMessage()
            {
                LearningProviderEntity = new TestLearningProvider
                {
                    UKPRN = 1
                },
                Learners = new List<TestLearner>
                {
                    new TestLearner
                    {
                        LearnRefNumber = "TestLearnRefNumber",
                        ULN = 1234567890
                    }
                }
            };

            var fundingServiceDtoMock = new Mock<IFundingServiceDto>();

            fundingServiceDtoMock.SetupGet(f => f.Message).Returns(message);

            var externalDataCache = new ExternalDataCache();

            var postcodesDataRetrievalServiceMock = new Mock<IPostcodesDataRetrievalService>();

            var postcodesCurrentVersion = "PostcodesVersion";
            var sfaAreaCosts = new Dictionary<string, IEnumerable<SfaAreaCost>>();
            var sfaDisadvantages = new Dictionary<string, IEnumerable<SfaDisadvantage>>();
            var dasDisadvantages = new Dictionary<string, IEnumerable<DasDisadvantage>>();
            var efaDisadvantages = new Dictionary<string, IEnumerable<EfaDisadvantage>>();
            var careerLearningPilots = new Dictionary<string, IEnumerable<CareerLearningPilot>>();
            var postcodeRoots = new Dictionary<string, PostcodeRoot>();

            postcodesDataRetrievalServiceMock.Setup(p => p.UniquePostcodes(message)).Returns(new HashSet<string>()).Verifiable();
            postcodesDataRetrievalServiceMock.Setup(p => p.CurrentVersion()).Returns(postcodesCurrentVersion).Verifiable();
            postcodesDataRetrievalServiceMock.Setup(p => p.PostcodeRootsForPostcodes(It.IsAny<IEnumerable<string>>())).Returns(postcodeRoots);

            var largeEmployersDataRetrievalServiceMock = new Mock<ILargeEmployersDataRetrievalService>();

            var largeEmployers = new Dictionary<int, IEnumerable<LargeEmployers>>();

            largeEmployersDataRetrievalServiceMock.Setup(l => l.UniqueEmployerIds(message)).Returns(new List<int>()).Verifiable();
            largeEmployersDataRetrievalServiceMock.Setup(l => l.LargeEmployersForEmployerIds(It.IsAny<List<int>>())).Returns(largeEmployers).Verifiable();

            var larsDataRetrievalServiceMock = new Mock<ILARSDataRetrievalService>();

            var larsCurrentVersion = "LarsVersion";
            var larsAnnualValues = new Dictionary<string, IEnumerable<LARSAnnualValue>>();
            var larsFrameworkAims = new Dictionary<string, IEnumerable<LARSFrameworkAims>>();
            var larsLearningDeliveryCategories = new Dictionary<string, IEnumerable<LARSLearningDeliveryCategory>>();
            var larsFundings = new Dictionary<string, IEnumerable<LARSFunding>>();
            var larsLearningDeliveries = new Dictionary<string, LARSLearningDelivery>();
            var larsStandardCommonComponents = new Dictionary<int, IEnumerable<LARSStandardCommonComponent>>();
            var larsFrameworkCommonComponents = new List<LARSFrameworkCommonComponent>();
            var larsApprenticeshipFundingStandards = new List<LARSStandardApprenticeshipFunding>();
            var larsApprenticeshipFundingFrameworks = new List<LARSFrameworkApprenticeshipFunding>();
            var larsStandardFundings = new Dictionary<int, IEnumerable<LARSStandardFunding>>();

            larsDataRetrievalServiceMock.Setup(l => l.UniqueLearnAimRefs(message)).Returns(new HashSet<string>()).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.UniqueStandardCodes(message)).Returns(new List<int>()).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.UniqueFrameworkCommonComponents(message)).Returns(new HashSet<LARSFrameworkKey>()).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.UniqueApprenticeshipFundingStandards(message)).Returns(It.IsAny<IEnumerable<LARSApprenticeshipFundingKey>>).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.UniqueApprenticeshipFundingFrameworks(message)).Returns(It.IsAny<IEnumerable<LARSApprenticeshipFundingKey>>).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.CurrentVersion()).Returns(larsCurrentVersion).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSAnnualValuesForLearnAimRefs(It.IsAny<IEnumerable<string>>())).Returns(larsAnnualValues).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSFrameworkAimsForLearnAimRefs(It.IsAny<IEnumerable<string>>())).Returns(larsFrameworkAims).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSLearningDeliveryCategoriesForLearnAimRefs(It.IsAny<HashSet<string>>())).Returns(larsLearningDeliveryCategories).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSFundingsForLearnAimRefs(It.IsAny<IEnumerable<string>>())).Returns(larsFundings).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSLearningDeliveriesForLearnAimRefs(It.IsAny<IEnumerable<string>>())).Returns(larsLearningDeliveries).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSStandardCommonComponentForStandardCode(It.IsAny<List<int>>())).Returns(larsStandardCommonComponents).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSFrameworkCommonComponentForLearnAimRefs(It.IsAny<IEnumerable<LARSFrameworkKey>>())).Returns(larsFrameworkCommonComponents).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSApprenticeshipFundingStandards(It.IsAny<List<LARSApprenticeshipFundingKey>>())).Returns(larsApprenticeshipFundingStandards).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSApprenticeshipFundingFrameworks(It.IsAny<List<LARSApprenticeshipFundingKey>>())).Returns(larsApprenticeshipFundingFrameworks).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSStandardFundingForStandardCodes(It.IsAny<List<int>>())).Returns(larsStandardFundings).Verifiable();

            var organisationDataRetrievalServiceMock = new Mock<IOrganisationDataRetrievalService>();

            var organisationCurrentVersion = "OrganisationVersion";
            var orgFundings = new Dictionary<long, IEnumerable<OrgFunding>>();

            organisationDataRetrievalServiceMock.Setup(o => o.CurrentVersion()).Returns(organisationCurrentVersion).Verifiable();
            organisationDataRetrievalServiceMock.Setup(o => o.OrgFundingsForUkprns(It.IsAny<List<long>>())).Returns(orgFundings).Verifiable();

            var appsEarningsHistoryDataRetrievalServiceMock = new Mock<IAppsEarningsHistoryDataRetrievalService>();

            var aecHistory = new Dictionary<long, IEnumerable<AECEarningsHistory>>();

            appsEarningsHistoryDataRetrievalServiceMock.Setup(a => a.AppsEarningsHistoryForLearners(It.IsAny<int>(), It.IsAny<IEnumerable<LearnRefNumberULNKey>>())).Returns(aecHistory).Verifiable();

            var fcsDataRetrievalServiceMock = new Mock<IFCSDataRetrievalService>();

            var fcsContractAllocations = new List<FCSContractAllocation>();

            fcsDataRetrievalServiceMock.Setup(f => f.UniqueConRefNumbers(message)).Returns(new HashSet<string>()).Verifiable();
            fcsDataRetrievalServiceMock.Setup(f => f.FCSContractsForUKPRN(It.IsAny<int>(), It.IsAny<IEnumerable<string>>())).Returns(fcsContractAllocations).Verifiable();

            await NewService(
                externalDataCache,
                appsEarningsHistoryDataRetrievalServiceMock.Object,
                fcsDataRetrievalServiceMock.Object,
                postcodesDataRetrievalServiceMock.Object,
                largeEmployersDataRetrievalServiceMock.Object,
                larsDataRetrievalServiceMock.Object,
                organisationDataRetrievalServiceMock.Object,
                fundingServiceDtoMock.Object)
                .PopulateAsync(CancellationToken.None);

            postcodesDataRetrievalServiceMock.VerifyAll();
            largeEmployersDataRetrievalServiceMock.VerifyAll();
            larsDataRetrievalServiceMock.VerifyAll();
            organisationDataRetrievalServiceMock.VerifyAll();
            appsEarningsHistoryDataRetrievalServiceMock.VerifyAll();
            fcsDataRetrievalServiceMock.VerifyAll();

            externalDataCache.PostcodeCurrentVersion.Should().Be(postcodesCurrentVersion);
            externalDataCache.PostcodeRoots.Should().BeSameAs(postcodeRoots);

            externalDataCache.LargeEmployers.Should().BeSameAs(largeEmployers);

            externalDataCache.LARSCurrentVersion.Should().Be(larsCurrentVersion);
            externalDataCache.LARSAnnualValue.Should().BeSameAs(larsAnnualValues);
            externalDataCache.LARSFrameworkAims.Should().BeSameAs(larsFrameworkAims);
            externalDataCache.LARSFunding.Should().BeSameAs(larsFundings);
            externalDataCache.LARSLearningDelivery.Should().BeSameAs(larsLearningDeliveries);
            externalDataCache.LARSLearningDeliveryCategory.Should().BeSameAs(larsLearningDeliveryCategories);
            externalDataCache.LARSStandardFundings.Should().BeSameAs(larsStandardFundings);
            externalDataCache.LARSFrameworkCommonComponent.Should().BeSameAs(larsFrameworkCommonComponents);
            externalDataCache.LARSStandardCommonComponent.Should().BeSameAs(larsStandardCommonComponents);
            externalDataCache.LARSApprenticeshipFundingFrameworks.Should().BeSameAs(larsApprenticeshipFundingFrameworks);
            externalDataCache.LARSApprenticeshipFundingStandards.Should().BeSameAs(larsApprenticeshipFundingStandards);

            externalDataCache.OrgVersion.Should().Be(organisationCurrentVersion);
            externalDataCache.OrgFunding.Should().BeSameAs(orgFundings);
        }

        private ExternalDataCachePopulationService NewService(
            IExternalDataCache externalDataCache = null,
            IAppsEarningsHistoryDataRetrievalService appsEarningsHistoryDataRetrievalService = null,
            IFCSDataRetrievalService fcsDataRetrievalService = null,
            IPostcodesDataRetrievalService postcodesDataRetrievalService = null,
            ILargeEmployersDataRetrievalService largeEmployersDataRetrievalService = null,
            ILARSDataRetrievalService larsDataRetrievalService = null,
            IOrganisationDataRetrievalService organisationDataRetrievalService = null,
            IFundingServiceDto fundingServiceDto = null)
        {
            return new ExternalDataCachePopulationService(externalDataCache, appsEarningsHistoryDataRetrievalService, fcsDataRetrievalService, postcodesDataRetrievalService, largeEmployersDataRetrievalService, larsDataRetrievalService, organisationDataRetrievalService, fundingServiceDto);
        }
    }
}

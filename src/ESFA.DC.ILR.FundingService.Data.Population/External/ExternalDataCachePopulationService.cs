using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class ExternalDataCachePopulationService : IExternalDataCachePopulationService
    {
        private readonly IExternalDataCache _externalDataCache;
        private readonly IAppsEarningsHistoryDataRetrievalService _appsEarningsHistoryDataRetrievalService;
        private readonly IFCSDataRetrievalService _fcsDataRetrievalService;
        private readonly IPostcodesDataRetrievalService _postcodesDataRetrievalService;
        private readonly ILargeEmployersDataRetrievalService _largeEmployersDataRetrievalService;
        private readonly ILARSDataRetrievalService _larsDataRetrievalService;
        private readonly IOrganisationDataRetrievalService _organisationDataRetrievalService;
        private readonly IFundingServiceDto _fundingServiceDto;

        public ExternalDataCachePopulationService(
            IExternalDataCache externalDataCache,
            IAppsEarningsHistoryDataRetrievalService appsEarningsHistoryDataRetrievalService,
            IFCSDataRetrievalService fcsDataRetrievalService,
            IPostcodesDataRetrievalService postcodesDataRetrievalService,
            ILargeEmployersDataRetrievalService largeEmployersDataRetrievalService,
            ILARSDataRetrievalService larsDataRetrievalService,
            IOrganisationDataRetrievalService organisationDataRetrievalService,
            IFundingServiceDto fundingServiceDto)
        {
            _externalDataCache = externalDataCache;
            _appsEarningsHistoryDataRetrievalService = appsEarningsHistoryDataRetrievalService;
            _fcsDataRetrievalService = fcsDataRetrievalService;
            _postcodesDataRetrievalService = postcodesDataRetrievalService;
            _largeEmployersDataRetrievalService = largeEmployersDataRetrievalService;
            _larsDataRetrievalService = larsDataRetrievalService;
            _organisationDataRetrievalService = organisationDataRetrievalService;
            _fundingServiceDto = fundingServiceDto;
        }

        public async Task PopulateAsync(CancellationToken cancellationToken)
        {
            var providerUKPRN = _fundingServiceDto.Message.LearningProviderEntity.UKPRN;

            var uniquePostcodes = _postcodesDataRetrievalService.UniquePostcodes(_fundingServiceDto.Message).ToCaseInsensitiveHashSet();
            var learnAimRefs = _larsDataRetrievalService.UniqueLearnAimRefs(_fundingServiceDto.Message).ToCaseInsensitiveHashSet();
            var standardCodes = _larsDataRetrievalService.UniqueStandardCodes(_fundingServiceDto.Message).ToList();
            var frameworks = _larsDataRetrievalService.UniqueFrameworkCommonComponents(_fundingServiceDto.Message);
            var apprenticeshipFundingStandards = _larsDataRetrievalService.UniqueApprenticeshipFundingStandards(_fundingServiceDto.Message);
            var apprenticeshipFundingFrameworks = _larsDataRetrievalService.UniqueApprenticeshipFundingFrameworks(_fundingServiceDto.Message);

            var uniqueEmployerIds = _largeEmployersDataRetrievalService.UniqueEmployerIds(_fundingServiceDto.Message).ToList();

            var ukprns = new List<long>() { providerUKPRN };

            var uniqueFM36Learners = _appsEarningsHistoryDataRetrievalService.UniqueFM36Learners(_fundingServiceDto.Message);

            var conRefNumbers = _fcsDataRetrievalService.UniqueConRefNumbers(_fundingServiceDto.Message).ToCaseInsensitiveHashSet();

            var referenceDataCache = (ExternalDataCache)_externalDataCache;

            referenceDataCache.LARSCurrentVersion = _larsDataRetrievalService.CurrentVersion();
            referenceDataCache.LARSAnnualValue = _larsDataRetrievalService.LARSAnnualValuesForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSLearningDelivery = _larsDataRetrievalService.LARSLearningDeliveriesForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSLearningDeliveryCategory = _larsDataRetrievalService.LARSLearningDeliveryCategoriesForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSFrameworkAims = _larsDataRetrievalService.LARSFrameworkAimsForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSFunding = _larsDataRetrievalService.LARSFundingsForLearnAimRefs(learnAimRefs);
            referenceDataCache.LARSStandardCommonComponent = _larsDataRetrievalService.LARSStandardCommonComponentForStandardCode(standardCodes);
            referenceDataCache.LARSFrameworkCommonComponent = _larsDataRetrievalService.LARSFrameworkCommonComponentForLearnAimRefs(frameworks);
            referenceDataCache.LARSApprenticeshipFundingStandards = _larsDataRetrievalService.LARSApprenticeshipFundingStandards(apprenticeshipFundingStandards);
            referenceDataCache.LARSApprenticeshipFundingFrameworks = _larsDataRetrievalService.LARSApprenticeshipFundingFrameworks(apprenticeshipFundingFrameworks);
            referenceDataCache.LARSStandardFundings = _larsDataRetrievalService.LARSStandardFundingForStandardCodes(standardCodes);

            referenceDataCache.PostcodeRoots = _postcodesDataRetrievalService.PostcodeRootsForPostcodes(uniquePostcodes);
            referenceDataCache.PostcodeCurrentVersion = _postcodesDataRetrievalService.CurrentVersion();

            referenceDataCache.OrgVersion = _organisationDataRetrievalService.CurrentVersion();
            referenceDataCache.OrgFunding = _organisationDataRetrievalService.OrgFundingsForUkprns(ukprns);

            referenceDataCache.LargeEmployers = _largeEmployersDataRetrievalService.LargeEmployersForEmployerIds(uniqueEmployerIds);

            referenceDataCache.FCSContractAllocations = _fcsDataRetrievalService.FCSContractsForUKPRN(providerUKPRN, conRefNumbers);

            referenceDataCache.AECLatestInYearEarningHistory = _appsEarningsHistoryDataRetrievalService.AppsEarningsHistoryForLearners(providerUKPRN, uniqueFM36Learners);
        }
    }
}
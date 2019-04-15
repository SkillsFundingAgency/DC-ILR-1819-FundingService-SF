using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS
{
    public class LARSReferenceDataService : ILARSReferenceDataService
    {
        private readonly IExternalDataCache _referenceDataCache;

        public LARSReferenceDataService(IExternalDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        public string LARSCurrentVersion()
        {
            return _referenceDataCache.LARSCurrentVersion;
        }

        public IEnumerable<LARSFunding> LARSFundingsForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSFunding.TryGetValue(learnAimRef, out IEnumerable<LARSFunding> larsFundings);

            return larsFundings;
        }

        public LARSLearningDelivery LARSLearningDeliveryForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSLearningDelivery.TryGetValue(learnAimRef, out LARSLearningDelivery learningDelivery);

            return learningDelivery;
        }

        public IEnumerable<LARSAnnualValue> LARSAnnualValuesForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSAnnualValue.TryGetValue(learnAimRef, out IEnumerable<LARSAnnualValue> larsAnnualValue);

            return larsAnnualValue;
        }

        public IEnumerable<LARSFrameworkAims> LARSFFrameworkAimsForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSFrameworkAims.TryGetValue(learnAimRef, out IEnumerable<LARSFrameworkAims> larsFrameworkAims);

            return larsFrameworkAims;
        }

        public IEnumerable<LARSLearningDeliveryCategory> LARSLearningDeliveryCategoriesForLearnAimRef(string learnAimRef)
        {
            _referenceDataCache.LARSLearningDeliveryCategory.TryGetValue(learnAimRef, out IEnumerable<LARSLearningDeliveryCategory> larsLearningDeliveryCategory);

            return larsLearningDeliveryCategory;
        }

        public IEnumerable<LARSStandardApprenticeshipFunding> LARSStandardApprenticeshipFunding(int? standardCode, int? progType)
        {
            if (standardCode == null || progType == null)
            {
                return null;
            }

            return _referenceDataCache.LARSApprenticeshipFundingStandards
                        .Where(la =>
                        la.ApprenticeshipCode == standardCode
                        && la.ProgType == progType);
        }

        public IEnumerable<LARSFrameworkApprenticeshipFunding> LARSFrameworkApprenticeshipFunding(int? fworkCode, int? progType, int? pwayCode)
        {
            if (fworkCode == null || progType == null || pwayCode == null)
            {
                return null;
            }

            return _referenceDataCache.LARSApprenticeshipFundingFrameworks
                        .Where(la =>
                        la.ApprenticeshipCode == fworkCode
                        && la.ProgType == progType
                        && la.PwayCode == pwayCode);
        }

        public IEnumerable<LARSFrameworkCommonComponent> LARSFrameworkCommonComponent(string learnAimRef, int? fworkCode, int? progType, int? pwayCode)
        {
            if (learnAimRef == null || fworkCode == null || progType == null || pwayCode == null)
            {
                return null;
            }

            return _referenceDataCache.LARSFrameworkCommonComponent
                        .Where(lf =>
                        lf.LearnAimRef.CaseInsensitiveEquals(learnAimRef)
                        && lf.FworkCode == fworkCode
                        && lf.ProgType == progType
                        && lf.PwayCode == pwayCode);
        }

        public IEnumerable<LARSStandardCommonComponent> LARSStandardCommonComponent(int? standardCode)
        {
            return standardCode == null ? null :
                _referenceDataCache
                .LARSStandardCommonComponent
                .Where(k => k.Key == standardCode)
                .Select(v => v.Value).FirstOrDefault();
        }

        public IEnumerable<LARSStandardFunding> LARSStandardFunding(int? standardCode)
        {
            if (standardCode == null)
            {
                return null;
            }

            _referenceDataCache.LARSStandardFundings.TryGetValue((int)standardCode, out IEnumerable<LARSStandardFunding> larsStandardFunding);

            return larsStandardFunding?.Where(la => la.FundingCategory == "StandardTblazer");
        }
    }
}

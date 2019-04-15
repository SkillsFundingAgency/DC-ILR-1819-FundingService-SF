using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Interface
{
    public interface ILARSReferenceDataService
    {
        string LARSCurrentVersion();

        IEnumerable<LARSFunding> LARSFundingsForLearnAimRef(string learnAimRef);

        LARSLearningDelivery LARSLearningDeliveryForLearnAimRef(string learnAimRef);

        IEnumerable<LARSFrameworkAims> LARSFFrameworkAimsForLearnAimRef(string learnAimRef);

        IEnumerable<LARSAnnualValue> LARSAnnualValuesForLearnAimRef(string learnAimRef);

        IEnumerable<LARSLearningDeliveryCategory> LARSLearningDeliveryCategoriesForLearnAimRef(string learnAimRef);

        IEnumerable<LARSStandardApprenticeshipFunding> LARSStandardApprenticeshipFunding(int? standardCode, int? progType);

        IEnumerable<LARSFrameworkApprenticeshipFunding> LARSFrameworkApprenticeshipFunding(int? fworkCode, int? progType, int? pwayCode);

        IEnumerable<LARSStandardCommonComponent> LARSStandardCommonComponent(int? standardCode);

        IEnumerable<LARSFrameworkCommonComponent> LARSFrameworkCommonComponent(string learnAimRef, int? fworkCode, int? progType, int? pwayCode);

        IEnumerable<LARSStandardFunding> LARSStandardFunding(int? standardCode);
    }
}

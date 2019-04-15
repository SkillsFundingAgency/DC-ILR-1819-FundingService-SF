using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface ILARSDataRetrievalService
    {
        string CurrentVersion();

        IEnumerable<string> UniqueLearnAimRefs(IMessage message);

        IEnumerable<int> UniqueStandardCodes(IMessage message);

        IEnumerable<LARSFrameworkKey> UniqueFrameworkCommonComponents(IMessage message);

        IEnumerable<LARSApprenticeshipFundingKey> UniqueApprenticeshipFundingStandards(IMessage message);

        IEnumerable<LARSApprenticeshipFundingKey> UniqueApprenticeshipFundingFrameworks(IMessage message);

        IDictionary<string, IEnumerable<LARSFunding>> LARSFundingsForLearnAimRefs(IEnumerable<string> learnAimRefs);

        IDictionary<string, LARSLearningDelivery> LARSLearningDeliveriesForLearnAimRefs(IEnumerable<string> learnAimRefs);

        IDictionary<string, IEnumerable<LARSAnnualValue>> LARSAnnualValuesForLearnAimRefs(IEnumerable<string> learnAimRefs);

        IDictionary<string, IEnumerable<LARSLearningDeliveryCategory>> LARSLearningDeliveryCategoriesForLearnAimRefs(IEnumerable<string> learnAimRefs);

        IDictionary<string, IEnumerable<LARSFrameworkAims>> LARSFrameworkAimsForLearnAimRefs(IEnumerable<string> learnAimRefs);

        IDictionary<int, IEnumerable<LARSStandardCommonComponent>> LARSStandardCommonComponentForStandardCode(IEnumerable<int> standardCodes);

        IEnumerable<LARSFrameworkCommonComponent> LARSFrameworkCommonComponentForLearnAimRefs(IEnumerable<LARSFrameworkKey> larsFrameworkKeys);

        IEnumerable<LARSStandardApprenticeshipFunding> LARSApprenticeshipFundingStandards(IEnumerable<LARSApprenticeshipFundingKey> apprenticeshipFundingKeys);

        IEnumerable<LARSFrameworkApprenticeshipFunding> LARSApprenticeshipFundingFrameworks(IEnumerable<LARSApprenticeshipFundingKey> apprenticeshipFundingKeys);

        IDictionary<int, IEnumerable<LARSStandardFunding>> LARSStandardFundingForStandardCodes(IEnumerable<int> standardCodes);
    }
}

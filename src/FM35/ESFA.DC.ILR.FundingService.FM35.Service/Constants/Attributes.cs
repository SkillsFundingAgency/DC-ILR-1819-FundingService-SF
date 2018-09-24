﻿namespace ESFA.DC.ILR.FundingService.FM35.Service.Constants
{
    public static class Attributes
    {
        // FundModels
        public const int FundModel_35 = 35;

        // Entity
        public const string EntityGlobal = "global";
        public const string EntityOrgFunding = "OrgFunding";
        public const string EntityLearner = "Learner";
        public const string EntityLearnerEmploymentStatus = "LearnerEmploymentStatus";
        public const string EntityLargeEmployerReferenceData = "LargeEmployerReferenceData";
        public const string EntitySFA_PostcodeDisadvantage = "SFA_PostcodeDisadvantage";
        public const string EntityLearningDelivery = "LearningDelivery";
        public const string EntityLearningDeliveryFAM = "LearningDeliveryFAM";
        public const string EntityLearningDeliverySFA_PostcodeAreaCost = "SFA_PostcodeAreaCost";
        public const string EntityLearningDeliveryLARS_AnnualValue = "LearningDeliveryAnnualValue";
        public const string EntityLearningDeliveryLARS_Category = "LearningDeliveryLARSCategory";
        public const string EntityLearningDeliveryLARS_Funding = "LearningDeliveryLARS_Funding";

        // Global Values
        public const string YearValue = "1819";
        public const string CollectionPeriodValue = "DefaultPeriod";
        public const string Period1 = "R01";
        public const string Period2 = "R02";
        public const string Period3 = "R03";
        public const string Period4 = "R04";
        public const string Period5 = "R05";
        public const string Period6 = "R06";
        public const string Period7 = "R07";
        public const string Period8 = "R08";
        public const string Period9 = "R09";
        public const string Period10 = "R10";
        public const string Period11 = "R11";
        public const string Period12 = "R12";

        // Global
        public const string LARSVersion = "LARSVersion";
        public const string OrgVersion = "OrgVersion";
        public const string UKPRN = "UKPRN";
        public const string PostcodeDisadvantageVersion = "PostcodeDisadvantageVersion";

        // OrgFunding
        public const string OrgFundEffectiveTo = "OrgFundEffectiveTo";
        public const string OrgFundEffectiveFrom = "OrgFundEffectiveFrom";
        public const string OrgFundFactor = "OrgFundFactor";
        public const string OrgFundFactValue = "OrgFundFactValue";
        public const string OrgFundFactType = "OrgFundFactType";

        // Learner
        public const string LearnRefNumber = "LearnRefNumber";
        public const string DateOfBirth = "DateOfBirth";

        // Learner Employment Status
        public const string EmpId = "EmpId";
        public const string DateEmpStatApp = "DateEmpStatApp";

        // Large Employer
        public const string LargeEmpEffectiveFrom = "LargeEmpEffectiveFrom";
        public const string LargeEmpEffectiveTo = "LargeEmpEffectiveTo";

        // SFA Postcode Disadvantage
        public const string DisUplift = "DisUplift";
        public const string DisUpEffectiveFrom = "DisUpEffectiveFrom";
        public const string DisUpEffectiveTo = "DisUpEffectiveTo";

        // LearningDelivery
        public const string AchDate = "AchDate";
        public const string AddHours = "AddHours";
        public const string AimSeqNumber = "AimSeqNumber";
        public const string AimType = "AimType";
        public const string CompStatus = "CompStatus";
        public const string EmpOutcome = "EmpOutcome";
        public const string EnglandFEHEStatus = "EnglandFEHEStatus";
        public const string EnglPrscID = "EnglPrscID";
        public const string FworkCode = "FworkCode";
        public const string FrameworkCommonComponent = "FrameworkCommonComponent";
        public const string FrameworkComponentType = "FrameworkComponentType";
        public const string LearnActEndDate = "LearnActEndDate";
        public const string LearnPlanEndDate = "LearnPlanEndDate";
        public const string LearnStartDate = "LearnStartDate";
        public const string LrnDelFAM_EEF = "LrnDelFAM_EEF";
        public const string LrnDelFAM_LDM1 = "LrnDelFAM_LDM1";
        public const string LrnDelFAM_LDM2 = "LrnDelFAM_LDM2";
        public const string LrnDelFAM_LDM3 = "LrnDelFAM_LDM3";
        public const string LrnDelFAM_LDM4 = "LrnDelFAM_LDM4";
        public const string LrnDelFAM_FFI = "LrnDelFAM_FFI";
        public const string LrnDelFAM_RES = "LrnDelFAM_RES";
        public const string OrigLearnStartDate = "OrigLearnStartDate";
        public const string OtherFundAdj = "OtherFundAdj";
        public const string Outcome = "Outcome";
        public const string PriorLearnFundAdj = "PriorLearnFundAdj";
        public const string ProgType = "ProgType";
        public const string PwayCode = "PwayCode";

        public const string LearningDeliveryFAMTypeEEF = "EEF";
        public const string LearningDeliveryFAMTypeFFI = "FFI";
        public const string LearningDeliveryFAMTypeRES = "RES";
        public const string LearningDeliveryFAMTypeLDM = "LDM";
        public const string LearningDeliveryFAMTypeLDM1 = "LDM1";
        public const string LearningDeliveryFAMTypeLDM2 = "LDM2";
        public const string LearningDeliveryFAMTypeLDM3 = "LDM3";
        public const string LearningDeliveryFAMTypeLDM4 = "LDM4";

        // LearningDeliveryFAM
        public const string LearnDelFAMCode = "LearnDelFAMCode";
        public const string LearnDelFAMDateFrom = "LearnDelFAMDateFrom";
        public const string LearnDelFAMDateTo = "LearnDelFAMDateTo";
        public const string LearnDelFAMType = "LearnDelFAMType";

        // LearningDeliveryLARSCategory
        public const string LearnDelCatRef = "LearnDelCatRef";
        public const string LearnDelCatDateFrom = "LearnDelCatDateFrom";
        public const string LearnDelCatDateTo = "LearnDelCatDateTo";

        // LearningDeliveryLARSAnnualValue
        public const string LearnDelAnnValBasicSkillsTypeCode = "LearnDelAnnValBasicSkillsTypeCode";
        public const string LearnDelAnnValDateFrom = "LearnDelAnnValDateFrom";
        public const string LearnDelAnnValDateTo = "LearnDelAnnValDateTo";

        // LearningDeliveryLARSFunding
        public const string LARSFundCategory = "LARSFundCategory";
        public const string LARSFundEffectiveFrom = "LARSFundEffectiveFrom";
        public const string LARSFundEffectiveTo = "LARSFundEffectiveTo";
        public const string LARSFundUnweightedRate = "LARSFundUnweightedRate";
        public const string LARSFundWeightedRate = "LARSFundWeightedRate";
        public const string LARSFundWeightingFactor = "LARSFundWeightingFactor";

        // SFAPostcodeAreaCost
        public const string AreaCosEffectiveFrom = "AreaCosEffectiveFrom";
        public const string AreaCosEffectiveTo = "AreaCosEffectiveTo";
        public const string AreaCosFactor = "AreaCosFactor";
    }
}

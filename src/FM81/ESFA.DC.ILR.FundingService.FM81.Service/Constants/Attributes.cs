namespace ESFA.DC.ILR.FundingService.FM81.Service.Constants
{
    public static class Attributes
    {
        // FundModels
        public const int FundModel_81 = 81;
        public const int ProgType_25 = 25;

        // Entity
        public const string EntityGlobal = "global";
        public const string EntityLearner = "Learner";
        public const string EntityLearningDelivery = "LearningDelivery";
        public const string EntityApprenticeshipFinancialRecord = "ApprenticeshipFinancialRecord";
        public const string EntityLearnerEmploymentStatus = "LearnerEmploymentStatus";
        public const string EntityLearningDeliveryFAM = "LearningDeliveryFAM";
        public const string EntityStandardCommonComponent = "LARS_StandardCommonComponent";
        public const string EntityLearningDeliveryLARS_StandardFunding = "LARS_StandardFunding";

        // Global Names
        public const string LARSVersion = "LARSVersion";
        public const string UKPRN = "UKPRN";
        public const string CurFundYr = "CurFundYr";
        public const string RulebaseVersion = "RulebaseVersion";

        // Global Values
        public const string CurFundYrValue = "1819";
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

        // Learner
        public const string LearnRefNumber = "LearnRefNumber";
        public const string DateOfBirth = "DateOfBirth";

        // LearningDelivery
        public const string AchDate = "AchDate";
        public const string AimSeqNumber = "AimSeqNumber";
        public const string AimType = "AimType";
        public const string CompStatus = "CompStatus";
        public const string FrameworkCommonComponent = "FrameworkCommonComponent";
        public const string LearnAimRef = "LearnAimRef";
        public const string LearnActEndDate = "LearnActEndDate";
        public const string LearnPlanEndDate = "LearnPlanEndDate";
        public const string LearnStartDate = "LearnStartDate";
        public const string LrnDelFAM_EEF = "LrnDelFAM_EEF";
        public const string LrnDelFAM_FFI = "LrnDelFAM_FFI";
        public const string LrnDelFAM_LDM1 = "LrnDelFAM_LDM1";
        public const string LrnDelFAM_LDM2 = "LrnDelFAM_LDM2";
        public const string LrnDelFAM_LDM3 = "LrnDelFAM_LDM3";
        public const string LrnDelFAM_LDM4 = "LrnDelFAM_LDM4";
        public const string LrnDelFAM_RES = "LrnDelFAM_RES";
        public const string LrnDelFAM_SOF = "LrnDelFAM_SOF";
        public const string LrnDelFAM_SPP = "LrnDelFAM_SPP";
        public const string OrigLearnStartDate = "OrigLearnStartDate";
        public const string OtherFundAdj = "OtherFundAdj";
        public const string Outcome = "Outcome";
        public const string PriorLearnFundAdj = "PriorLearnFundAdj";
        public const string ProgType = "ProgType";
        public const string STDCode = "STDCode";
        public const string WithdrawReason = "WithdrawReason";

        public const string AchApplicDate = "AchApplicDate";
        public const string AchEligible = "AchEligible";
        public const string Achieved = "Achieved";
        public const string AchievementApplicVal = "AchievementApplicVal";
        public const string AchPayment = "AchPayment";
        public const string ActualDaysIL = "ActualDaysIL";
        public const string ActualNumInstalm = "ActualNumInstalm";
        public const string AdjProgStartDate = "AdjProgStartDate";
        public const string AgeStandardStart = "AgeStandardStart";
        public const string ApplicFundValDate = "ApplicFundValDate";
        public const string CombinedAdjProp = "CombinedAdjProp";
        public const string CoreGovContCapApplicVal = "CoreGovContCapApplicVal";
        public const string CoreGovContPayment = "CoreGovContPayment";
        public const string CoreGovContUncapped = "CoreGovContUncapped";
        public const string EmpIdAchDate = "EmpIdAchDate";
        public const string EmpIdFirstDayStandard = "EmpIdFirstDayStandard";
        public const string EmpIdFirstYoungAppDate = "EmpIdFirstYoungAppDate";
        public const string EmpIdSecondYoungAppDate = "EmpIdSecondYoungAppDate";
        public const string EmpIdSmallBusDate = "EmpIdSmallBusDate";
        public const string FundLine = "FundLine";
        public const string InstPerPeriod = "InstPerPeriod";
        public const string LearnDelDaysIL = "LearnDelDaysIL";
        public const string LearnDelStandardAccDaysIL = "LearnDelStandardAccDaysIL";
        public const string LearnDelStandardPrevAccDaysIL = "LearnDelStandardPrevAccDaysIL";
        public const string LearnDelStandardTotalDaysIL = "LearnDelStandardTotalDaysIL";
        public const string LearnSuppFund = "LearnSuppFund";
        public const string LearnSuppFundCash = "LearnSuppFundCash";
        public const string MathEngAimValue = "MathEngAimValue";
        public const string MathEngBalPayment = "MathEngBalPayment";
        public const string MathEngBalPct = "MathEngBalPct";
        public const string MathEngLSFFundStart = "MathEngLSFFundStart";
        public const string MathEngLSFThresholdDays = "MathEngLSFThresholdDays";
        public const string MathEngOnProgPayment = "MathEngOnProgPayment";
        public const string MathEngOnProgPct = "MathEngOnProgPct";
        public const string OutstandNumOnProgInstalm = "OutstandNumOnProgInstalm";
        public const string PlannedNumOnProgInstalm = "PlannedNumOnProgInstalm";
        public const string PlannedTotalDaysIL = "PlannedTotalDaysIL";
        public const string ProgStandardStartDate = "ProgStandardStartDate";
        public const string SmallBusApplicVal = "SmallBusApplicVal";
        public const string SmallBusEligible = "SmallBusEligible";
        public const string SmallBusPayment = "SmallBusPayment";
        public const string SmallBusStatusFirstDayStandard = "SmallBusStatusFirstDayStandard";
        public const string SmallBusStatusThreshold = "SmallBusStatusThreshold";
        public const string YoungAppApplicVal = "YoungAppApplicVal";
        public const string YoungAppEligible = "YoungAppEligible";
        public const string YoungAppFirstPayment = "YoungAppFirstPayment";
        public const string YoungAppFirstThresholdDate = "YoungAppFirstThresholdDate";
        public const string YoungAppPayment = "YoungAppPayment";
        public const string YoungAppSecondPayment = "YoungAppSecondPayment";
        public const string YoungAppSecondThresholdDate = "YoungAppSecondThresholdDate";

        // LearningDeliveryFAM
        public const string EEF = "EEF";
        public const string FFI = "FFI";
        public const string LDM = "LDM";
        public const string RES = "RES";
        public const string SOF = "SOF";
        public const string SPP = "SPP";

        // ApprenticeshipFinancialRecord
        public const string AFinAmount = "AFinAmount";
        public const string AFinCode = "AFinCode";
        public const string AFinDate = "AFinDate";
        public const string AFinType = "AFinType";

        // LearningDeliveryFAM
        public const string LearnDelFAMCode = "LearnDelFAMCode";
        public const string LearnDelFAMDateFrom = "LearnDelFAMDateFrom";
        public const string LearnDelFAMDateTo = "LearnDelFAMDateTo";
        public const string LearnDelFAMType = "LearnDelFAMType";

        // LARS_StandardCommonComponent
        public const string LARSStandardCommonComponentCode = "LARSCommonComponent";
        public const string LARSStandardCommonComponentStandardCode = "LARSStandardCode";
        public const string LARSStandardCommonComponentEffectiveFrom = "LARSEffectiveFrom";
        public const string LARSStandardCommonComponentEffectiveTo = "LARSEffectiveTo";

        // LARS_StandardFunding
        public const string SFFundableWithoutEmployer = "FundableWithoutEmployer";
        public const string SF1618Incentive = "SF1618Incentive";
        public const string SFAchIncentive = "SFAchIncentive";
        public const string SFCoreGovContCap = "SFCoreGovContCap";
        public const string SFEffectiveFromDate = "SFEffectiveFromDate";
        public const string SFEffectiveToDate = "SFEffectiveToDate";
        public const string SFSmallBusIncentive = "SFSmallBusIncentive";

        // LearnerEmploymentStatus
        public const string DateEmpStatApp = "DateEmpStatApp";
        public const string EmpId = "EmpId";
        public const string EMPStat = "EMPStat";
        public const string EmpStatMon_SEM = "EmpStatMon_SEM";
        public const string SEM = "SEM";
    }
}

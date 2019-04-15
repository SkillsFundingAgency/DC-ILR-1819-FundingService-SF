namespace ESFA.DC.ILR.FundingService.FM36.Service.Constants
{
    public static class Attributes
    {
        // FundModels
        public const int FundModel_36 = 36;

        // Entity
        public const string EntityGlobal = "global";
        public const string EntityLearner = "Learner";
        public const string EntityLearningDelivery = "LearningDelivery";
        public const string EntityApprenticeshipFinancialRecord = "ApprenticeshipFinancialRecord";
        public const string EntityLearnerEmploymentStatus = "LearnerEmploymentStatus";
        public const string EntityLearningDeliveryFAM = "LearningDeliveryFAM";
        public const string EntityStandardLARSApprenticshipFunding = "Standard_LARS_ApprenticshipFunding";
        public const string EntityFrameworkLARSApprenticshipFunding = "Framework_LARS_ApprenticshipFunding";
        public const string EntitySFA_PostcodeDisadvantage = "SFA_PostcodeDisadvantage";
        public const string EntityHistoricEarningInput = "HistoricEarningInput";
        public const string EntityLARSFrameworkCmnComp = "LARS_FrameworkCmnComp";
        public const string EntityStandardCommonComponent = "LARS_StandardCommonComponent";
        public const string EntityLearningDeliveryLARS_Funding = "LearningDeliveryLARS_Funding";
        public const string EntityApprenticeshipPriceEpisode = "ApprenticeshipPriceEpisode";
        public const string EntityHistoricEarningOutput = "HistoricEarningOutput";

        // Global Names
        public const string LARSVersion = "LARSVersion";
        public const string UKPRN = "UKPRN";
        public const string CollectionPeriod = "CollectionPeriod";
        public const string Year = "Year";
        public const string RulebaseVersion = "RulebaseVersion";

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

        // Learner
        public const string LearnRefNumber = "LearnRefNumber";
        public const string DateOfBirth = "DateOfBirth";
        public const string ULN = "ULN";
        public const string PrevUKPRN = "PrevUKPRN";
        public const string PMUKPRN = "PMUKPRN";

        // LearningDelivery
        public const string ActualDaysIL = "ActualDaysIL";
        public const string ActualNumInstalm = "ActualNumInstalm";
        public const string AdjStartDate = "AdjStartDate";
        public const string AgeAtProgStart = "AgeAtProgStart";
        public const string AimSeqNumber = "AimSeqNumber";
        public const string AimType = "AimType";
        public const string AppAdjLearnStartDate = "AppAdjLearnStartDate";
        public const string AppAdjLearnStartDateMatchPathway = "AppAdjLearnStartDateMatchPathway";
        public const string ApplicCompDate = "ApplicCompDate";
        public const string CombinedAdjProp = "CombinedAdjProp";
        public const string Completed = "Completed";
        public const string CompStatus = "CompStatus";
        public const string FirstIncentiveThresholdDate = "FirstIncentiveThresholdDate";
        public const string FrameworkCommonComponent = "FrameworkCommonComponent";
        public const string FundStart = "FundStart";
        public const string FworkCode = "FworkCode";
        public const string LDApplic1618FrameworkUpliftBalancingValue = "LDApplic1618FrameworkUpliftBalancingValue";
        public const string LDApplic1618FrameworkUpliftCompElement = "LDApplic1618FrameworkUpliftCompElement";
        public const string LDApplic1618FRameworkUpliftCompletionValue = "LDApplic1618FRameworkUpliftCompletionValue";
        public const string LDApplic1618FrameworkUpliftMonthInstalVal = "LDApplic1618FrameworkUpliftMonthInstalVal";
        public const string LDApplic1618FrameworkUpliftPrevEarnings = "LDApplic1618FrameworkUpliftPrevEarnings";
        public const string LDApplic1618FrameworkUpliftPrevEarningsStage1 = "LDApplic1618FrameworkUpliftPrevEarningsStage1";
        public const string LDApplic1618FrameworkUpliftRemainingAmount = "LDApplic1618FrameworkUpliftRemainingAmount";
        public const string LDApplic1618FrameworkUpliftTotalActEarnings = "LDApplic1618FrameworkUpliftTotalActEarnings";
        public const string LearnAimRef = "LearnAimRef";
        public const string LearnActEndDate = "LearnActEndDate";
        public const string LearnDel1618AtStart = "LearnDel1618AtStart";
        public const string LearnDelAppAccDaysIL = "LearnDelAppAccDaysIL";
        public const string LearnDelApplicDisadvAmount = "LearnDelApplicDisadvAmount";
        public const string LearnDelApplicEmp1618Incentive = "LearnDelApplicEmp1618Incentive";
        public const string LearnDelApplicEmpDate = "LearnDelApplicEmpDate";
        public const string LearnDelApplicProv1618FrameworkUplift = "LearnDelApplicProv1618FrameworkUplift";
        public const string LearnDelApplicProv1618Incentive = "LearnDelApplicProv1618Incentive";
        public const string LearnDelAppPrevAccDaysIL = "LearnDelAppPrevAccDaysIL";
        public const string LearnDelDaysIL = "LearnDelDaysIL";
        public const string LearnDelDisadAmount = "LearnDelDisadAmount";
        public const string LearnDelEligDisadvPayment = "LearnDelEligDisadvPayment";
        public const string LearnDelEmpIdFirstAdditionalPaymentThreshold = "LearnDelEmpIdFirstAdditionalPaymentThreshold";
        public const string LearnDelEmpIdSecondAdditionalPaymentThreshold = "LearnDelEmpIdSecondAdditionalPaymentThreshold";
        public const string LearnDelHistDaysThisApp = "LearnDelHistDaysThisApp";
        public const string LearnDelHistProgEarnings = "LearnDelHistProgEarnings";
        public const string LearnDelInitialFundLineType = "LearnDelInitialFundLineType";
        public const string LearnDelMathEng = "LearnDelMathEng";
        public const string LearnDelProgEarliestACT2Date = "LearnDelProgEarliestACT2Date";
        public const string LearnDelNonLevyProcured = "LearnDelNonLevyProcured";
        public const string LearnDelApplicCareLeaverIncentive = "LearnDelApplicCareLeaverIncentive";
        public const string LearnDelHistDaysCareLeavers = "LearnDelHistDaysCareLeavers";
        public const string LearnDelAccDaysILCareLeavers = "LearnDelAccDaysILCareLeavers";
        public const string LearnDelPrevAccDaysILCareLeavers = "LearnDelPrevAccDaysILCareLeavers";
        public const string LearnDelLearnerAddPayThresholdDate = "LearnDelLearnerAddPayThresholdDate";
        public const string LearnDelRedCode = "LearnDelRedCode";
        public const string LearnDelRedStartDate = "LearnDelRedStartDate";
        public const string LearnPlanEndDate = "LearnPlanEndDate";
        public const string LearnStartDate = "LearnStartDate";
        public const string LrnDelFAM_EEF = "LrnDelFAM_EEF";
        public const string LrnDelFAM_LDM1 = "LrnDelFAM_LDM1";
        public const string LrnDelFAM_LDM2 = "LrnDelFAM_LDM2";
        public const string LrnDelFAM_LDM3 = "LrnDelFAM_LDM3";
        public const string LrnDelFAM_LDM4 = "LrnDelFAM_LDM4";
        public const string MathEngAimValue = "MathEngAimValue";
        public const string OutstandNumOnProgInstalm = "OutstandNumOnProgInstalm";
        public const string OrigLearnStartDate = "OrigLearnStartDate";
        public const string OtherFundAdj = "OtherFundAdj";
        public const string PlannedNumOnProgInstalm = "PlannedNumOnProgInstalm";
        public const string PlannedTotalDaysIL = "PlannedTotalDaysIL";
        public const string PriorLearnFundAdj = "PriorLearnFundAdj";
        public const string ProgType = "ProgType";
        public const string PwayCode = "PwayCode";
        public const string SecondIncentiveThresholdDate = "SecondIncentiveThresholdDate";
        public const string STDCode = "STDCode";
        public const string ThresholdDays = "ThresholdDays";

        // Learning Delivery Periodised
        public const string DisadvFirstPayment = "DisadvFirstPayment";
        public const string DisadvSecondPayment = "DisadvSecondPayment";
        public const string FundLineType = "FundLineType";
        public const string InstPerPeriod = "InstPerPeriod";
        public const string LDApplic1618FrameworkUpliftBalancingPayment = "LDApplic1618FrameworkUpliftBalancingPayment";
        public const string LDApplic1618FrameworkUpliftCompletionPayment = "LDApplic1618FrameworkUpliftCompletionPayment";
        public const string LDApplic1618FrameworkUpliftOnProgPayment = "LDApplic1618FrameworkUpliftOnProgPayment";
        public const string LearnDelContType = "LearnDelContType";
        public const string LearnDelFirstEmp1618Pay = "LearnDelFirstEmp1618Pay";
        public const string LearnDelFirstProv1618Pay = "LearnDelFirstProv1618Pay";
        public const string LearnDelLevyNonPayInd = "LearnDelLevyNonPayInd";
        public const string LearnDelSecondEmp1618Pay = "LearnDelSecondEmp1618Pay";
        public const string LearnDelSecondProv1618Pay = "LearnDelSecondProv1618Pay";
        public const string LearnDelSEMContWaiver = "LearnDelSEMContWaiver";
        public const string LearnDelSFAContribPct = "LearnDelSFAContribPct";
        public const string LearnSuppFund = "LearnSuppFund";
        public const string LearnSuppFundCash = "LearnSuppFundCash";
        public const string MathEngBalPayment = "MathEngBalPayment";
        public const string MathEngBalPct = "MathEngBalPct";
        public const string MathEngOnProgPayment = "MathEngOnProgPayment";
        public const string MathEngOnProgPct = "MathEngOnProgPct";
        public const string ProgrammeAimBalPayment = "ProgrammeAimBalPayment";
        public const string ProgrammeAimCompletionPayment = "ProgrammeAimCompletionPayment";
        public const string ProgrammeAimOnProgPayment = "ProgrammeAimOnProgPayment";
        public const string ProgrammeAimProgFundIndMaxEmpCont = "ProgrammeAimProgFundIndMaxEmpCont";
        public const string ProgrammeAimProgFundIndMinCoInvest = "ProgrammeAimProgFundIndMinCoInvest";
        public const string ProgrammeAimTotProgFund = "ProgrammeAimTotProgFund";
        public const string LearnDelLearnAddPayment = "LearnDelLearnAddPayment";

        // LearningDeliveryFAM
        public const string EEF = "EEF";
        public const string LDM = "LDM";

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

        // LearnerEmploymentStatus
        public const string AgreeId = "AgreeId";
        public const string DateEmpStatApp = "DateEmpStatApp";
        public const string EmpId = "EmpId";
        public const string EMPStat = "EMPStat";
        public const string EmpStatMon_SEM = "EmpStatMon_SEM";
        public const string SEM = "SEM";

        // Standard_LARS_ApprenticshipFunding
        public const string StandardAF1618EmployerAdditionalPayment = "StandardAF1618EmployerAdditionalPayment";
        public const string StandardAF1618ProviderAdditionalPayment = "StandardAF1618ProviderAdditionalPayment";
        public const string StandardAF1618FrameworkUplift = "StandardAF1618FrameworkUplift";
        public const string StandardAFCareLeaverAdditionalPayment = "StandardAFCareLeaverAdditionalPayment";
        public const string StandardAFEffectiveFrom = "StandardAFEffectiveFrom";
        public const string StandardAFEffectiveTo = "StandardAFEffectiveTo";
        public const string StandardAFFundingCategory = "StandardAFFundingCategory";
        public const string StandardAFMaxEmployerLevyCap = "StandardAFMaxEmployerLevyCap";
        public const string StandardAFReservedValue2 = "StandardAFReservedValue2";
        public const string StandardAFReservedValue3 = "StandardAFReservedValue3";

        // Framework_LARS_ApprenticshipFunding
        public const string FrameworkAF1618EmployerAdditionalPayment = "FrameworkAF1618EmployerAdditionalPayment";
        public const string FrameworkAF1618ProviderAdditionalPayment = "FrameworkAF1618ProviderAdditionalPayment";
        public const string FrameworkAF1618FrameworkUplift = "FrameworkAF1618FrameworkUplift";
        public const string FrameworkAFCareLeaverAdditionalPayment = "FrameworkAFCareLeaverAdditionalPayment";
        public const string FrameworkAFEffectiveFrom = "FrameworkAFEffectiveFrom";
        public const string FrameworkAFEffectiveTo = "FrameworkAFEffectiveTo";
        public const string FrameworkAFFundingCategory = "FrameworkAFFundingCategory";
        public const string FrameworkAFMaxEmployerLevyCap = "FrameworkAFMaxEmployerLevyCap";
        public const string FrameworkAFReservedValue2 = "FrameworkAFReservedValue2";
        public const string FrameworkAFReservedValue3 = "FrameworkAFReservedValue3";

        // SFA_PostcodeDisadvantage
        public const string DisApprenticeshipUplift = "DisApprenticeshipUplift";
        public const string DisUpEffectiveFrom = "DisUpEffectiveFrom";
        public const string DisUpEffectiveTo = "DisUpEffectiveTo";

        // HistoricEarningInput
        public const string AppIdentifierInput = "AppIdentifierInput";
        public const string AppProgCompletedInTheYearInput = "AppProgCompletedInTheYearInput";
        public const string HistoricCollectionReturnInput = "HistoricCollectionReturnInput";
        public const string HistoricCollectionYearInput = "HistoricCollectionYearInput";
        public const string HistoricDaysInYearInput = "HistoricDaysInYearInput";
        public const string HistoricEffectiveTNPStartDateInput = "HistoricEffectiveTNPStartDateInput";
        public const string HistoricEmpIdStartWithinYearInput = "HistoricEmpIdStartWithinYearInput";
        public const string HistoricEmpIdEndWithinYearInput = "HistoricEmpIdEndWithinYearInput";
        public const string HistoricFworkCodeInput = "HistoricFworkCodeInput";
        public const string HistoricLearnDelProgEarliestACT2DateInput = "HistoricLearnDelProgEarliestACT2DateInput";
        public const string HistoricLearner1618AtStartInput = "HistoricLearner1618AtStartInput";
        public const string HistoricLearnRefNumberInput = "HistoricLearnRefNumberInput";
        public const string HistoricPMRAmountInput = "HistoricPMRAmountInput";
        public const string HistoricProgrammeStartDateIgnorePathwayInput = "HistoricProgrammeStartDateIgnorePathwayInput";
        public const string HistoricProgrammeStartDateMatchPathwayInput = "HistoricProgrammeStartDateMatchPathwayInput";
        public const string HistoricProgTypeInput = "HistoricProgTypeInput";
        public const string HistoricPwayCodeInput = "HistoricPwayCodeInput";
        public const string HistoricTotalProgAimPaymentsInTheYearInput = "HistoricTotalProgAimPaymentsInTheYearInput";
        public const string HistoricTotal1618UpliftPaymentsInTheYearInput = "HistoricTotal1618UpliftPaymentsInTheYearInput";
        public const string HistoricSTDCodeInput = "HistoricSTDCodeInput";
        public const string HistoricTNP1Input = "HistoricTNP1Input";
        public const string HistoricTNP2Input = "HistoricTNP2Input";
        public const string HistoricTNP3Input = "HistoricTNP3Input";
        public const string HistoricTNP4Input = "HistoricTNP4Input";
        public const string HistoricUKPRNInput = "HistoricUKPRNInput";
        public const string HistoricULNInput = "HistoricULNInput";
        public const string HistoricUptoEndDateInput = "HistoricUptoEndDateInput";
        public const string HistoricVirtualTNP3EndofTheYearInput = "HistoricVirtualTNP3EndofTheYearInput";
        public const string HistoricVirtualTNP4EndofTheYearInput = "HistoricVirtualTNP4EndofTheYearInput";

        // LARS_FrameworkCmnComp
        public const string LARSFrameworkCommonComponentCode = "LARSFrameworkCommonComponentCode";
        public const string LARSFrameworkCommonComponentEffectiveTo = "LARSFrameworkCommonComponentEffectiveTo";
        public const string LARSFrameworkCommonComponentEffectiveFrom = "LARSFrameworkCommonComponentEffectiveFrom";

        // LARS_StandardCommonComponent
        public const string LARSStandardCommonComponentCode = "LARSStandardCommonComponentCode";
        public const string LARSStandardCommonComponentEffectiveFrom = "LARSStandardCommonComponentEffectiveFrom";
        public const string LARSStandardCommonComponentEffectiveTo = "LARSStandardCommonComponentEffectiveTo";

        // LearningDeliveryLARSFunding
        public const string LARSFundCategory = "LARSFundCategory";
        public const string LARSFundEffectiveFrom = "LARSFundEffectiveFrom";
        public const string LARSFundEffectiveTo = "LARSFundEffectiveTo";
        public const string LARSFundWeightedRate = "LARSFundWeightedRate";

        // Apprenticeship Price Episode
        public const string PriceEpisodeIdentifier = "PriceEpisodeIdentifier";
        public const string EpisodeStartDate = "EpisodeStartDate";
        public const string TNP1 = "TNP1";
        public const string TNP2 = "TNP2";
        public const string TNP3 = "TNP3";
        public const string TNP4 = "TNP4";
        public const string PriceEpisodeUpperBandLimit = "PriceEpisodeUpperBandLimit";
        public const string PriceEpisodePlannedEndDate = "PriceEpisodePlannedEndDate";
        public const string PriceEpisodeActualEndDate = "PriceEpisodeActualEndDate";
        public const string PriceEpisodeTotalTNPPrice = "PriceEpisodeTotalTNPPrice";
        public const string PriceEpisodeUpperLimitAdjustment = "PriceEpisodeUpperLimitAdjustment";
        public const string PriceEpisodePlannedInstalments = "PriceEpisodePlannedInstalments";
        public const string PriceEpisodeActualInstalments = "PriceEpisodeActualInstalments";
        public const string PriceEpisodeInstalmentsThisPeriod = "PriceEpisodeInstalmentsThisPeriod";
        public const string PriceEpisodeCompletionElement = "PriceEpisodeCompletionElement";
        public const string PriceEpisodePreviousEarnings = "PriceEpisodePreviousEarnings";
        public const string PriceEpisodeInstalmentValue = "PriceEpisodeInstalmentValue";
        public const string PriceEpisodeOnProgPayment = "PriceEpisodeOnProgPayment";
        public const string PriceEpisodeTotalEarnings = "PriceEpisodeTotalEarnings";
        public const string PriceEpisodeBalanceValue = "PriceEpisodeBalanceValue";
        public const string PriceEpisodeBalancePayment = "PriceEpisodeBalancePayment";
        public const string PriceEpisodeCompleted = "PriceEpisodeCompleted";
        public const string PriceEpisodeCompletionPayment = "PriceEpisodeCompletionPayment";
        public const string PriceEpisodeRemainingTNPAmount = "PriceEpisodeRemainingTNPAmount";
        public const string PriceEpisodeRemainingAmountWithinUpperLimit = "PriceEpisodeRemainingAmountWithinUpperLimit";
        public const string PriceEpisodeCappedRemainingTNPAmount = "PriceEpisodeCappedRemainingTNPAmount";
        public const string PriceEpisodeExpectedTotalMonthlyValue = "PriceEpisodeExpectedTotalMonthlyValue";
        public const string PriceEpisodeAimSeqNumber = "PriceEpisodeAimSeqNumber";
        public const string PriceEpisodeFirstDisadvantagePayment = "PriceEpisodeFirstDisadvantagePayment";
        public const string PriceEpisodeSecondDisadvantagePayment = "PriceEpisodeSecondDisadvantagePayment";
        public const string PriceEpisodeApplic1618FrameworkUpliftBalancing = "PriceEpisodeApplic1618FrameworkUpliftBalancing";
        public const string PriceEpisodeApplic1618FrameworkUpliftCompletionPayment = "PriceEpisodeApplic1618FrameworkUpliftCompletionPayment";
        public const string PriceEpisodeApplic1618FrameworkUpliftOnProgPayment = "PriceEpisodeApplic1618FrameworkUpliftOnProgPayment";
        public const string PriceEpisodeSecondProv1618Pay = "PriceEpisodeSecondProv1618Pay";
        public const string PriceEpisodeFirstEmp1618Pay = "PriceEpisodeFirstEmp1618Pay";
        public const string PriceEpisodeSecondEmp1618Pay = "PriceEpisodeSecondEmp1618Pay";
        public const string PriceEpisodeFirstProv1618Pay = "PriceEpisodeFirstProv1618Pay";
        public const string PriceEpisodeLSFCash = "PriceEpisodeLSFCash";
        public const string PriceEpisodeFundLineType = "PriceEpisodeFundLineType";
        public const string PriceEpisodeSFAContribPct = "PriceEpisodeSFAContribPct";
        public const string PriceEpisodeLevyNonPayInd = "PriceEpisodeLevyNonPayInd";
        public const string EpisodeEffectiveTNPStartDate = "EpisodeEffectiveTNPStartDate";
        public const string PriceEpisodeFirstAdditionalPaymentThresholdDate = "PriceEpisodeFirstAdditionalPaymentThresholdDate";
        public const string PriceEpisodeSecondAdditionalPaymentThresholdDate = "PriceEpisodeSecondAdditionalPaymentThresholdDate";
        public const string PriceEpisodeContractType = "PriceEpisodeContractType";
        public const string PriceEpisodePreviousEarningsSameProvider = "PriceEpisodePreviousEarningsSameProvider";
        public const string PriceEpisodeTotProgFunding = "PriceEpisodeTotProgFunding";
        public const string PriceEpisodeProgFundIndMinCoInvest = "PriceEpisodeProgFundIndMinCoInvest";
        public const string PriceEpisodeProgFundIndMaxEmpCont = "PriceEpisodeProgFundIndMaxEmpCont";
        public const string PriceEpisodeTotalPMRs = "PriceEpisodeTotalPMRs";
        public const string PriceEpisodeCumulativePMRs = "PriceEpisodeCumulativePMRs";
        public const string PriceEpisodeCompExemCode = "PriceEpisodeCompExemCode";
        public const string PriceEpisodeLearnerAdditionalPaymentThresholdDate = "PriceEpisodeLearnerAdditionalPaymentThresholdDate";
        public const string PriceEpisodeAgreeId = "PriceEpisodeAgreeId";
        public const string PriceEpisodeRedStartDate = "PriceEpisodeRedStartDate";
        public const string PriceEpisodeRedStatusCode = "PriceEpisodeRedStatusCode";
        public const string PriceEpisodeLearnerAdditionalPayment = "PriceEpisodeLearnerAdditionalPayment";

        // Apprenticeship Historic Earnings Output
        public const string AppIdentifierOutput = "AppIdentifierOutput";
        public const string AppProgCompletedInTheYearOutput = "AppProgCompletedInTheYearOutput";
        public const string HistoricBalancingProgAimPaymentsInTheYear = "HistoricBalancingProgAimPaymentsInTheYear";
        public const string HistoricCompletionProgAimPaymentsInTheYear = "HistoricCompletionProgAimPaymentsInTheYear";
        public const string HistoricDaysInYearOutput = "HistoricDaysInYearOutput";
        public const string HistoricEffectiveTNPStartDateOutput = "HistoricEffectiveTNPStartDateOutput";
        public const string HistoricEmpIdEndWithinYearOutput = "HistoricEmpIdEndWithinYearOutput";
        public const string HistoricEmpIdStartWithinYearOutput = "HistoricEmpIdStartWithinYearOutput";
        public const string HistoricFworkCodeOutput = "HistoricFworkCodeOutput";
        public const string HistoricLearner1618AtStartOutput = "HistoricLearner1618AtStartOutput";
        public const string HistoricOnProgProgAimPaymentsInTheYear = "HistoricOnProgProgAimPaymentsInTheYear";
        public const string HistoricPMRAmountOutput = "HistoricPMRAmountOutput";
        public const string HistoricProgrammeStartDateIgnorePathwayOutput = "HistoricProgrammeStartDateIgnorePathwayOutput";
        public const string HistoricProgrammeStartDateMatchPathwayOutput = "HistoricProgrammeStartDateMatchPathwayOutput";
        public const string HistoricProgTypeOutput = "HistoricProgTypeOutput";
        public const string HistoricPwayCodeOutput = "HistoricPwayCodeOutput";
        public const string HistoricSTDCodeOutput = "HistoricSTDCodeOutput";
        public const string HistoricTNP1Output = "HistoricTNP1Output";
        public const string HistoricTNP2Output = "HistoricTNP2Output";
        public const string HistoricTNP3Output = "HistoricTNP3Output";
        public const string HistoricTNP4Output = "HistoricTNP4Output";
        public const string HistoricTotal1618UpliftPaymentsInTheYear = "HistoricTotal1618UpliftPaymentsInTheYear";
        public const string HistoricTotalProgAimPaymentsInTheYear = "HistoricTotalProgAimPaymentsInTheYear";
        public const string HistoricULNOutput = "HistoricULNOutput";
        public const string HistoricUptoEndDateOutput = "HistoricUptoEndDateOutput";
        public const string HistoricVirtualTNP3EndofThisYearOutput = "HistoricVirtualTNP3EndofThisYearOutput";
        public const string HistoricVirtualTNP4EndofThisYearOutput = "HistoricVirtualTNP4EndofThisYearOutput";
        public const string HistoricLearnDelProgEarliestACT2DateOutput = "HistoricLearnDelProgEarliestACT2DateOutput";
    }
}

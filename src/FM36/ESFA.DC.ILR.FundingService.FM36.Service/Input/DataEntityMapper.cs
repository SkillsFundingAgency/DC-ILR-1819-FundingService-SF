using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Interface;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.FM36.Service.Constants;
using ESFA.DC.ILR.FundingService.FM36.Service.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM36.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<ILearner>
    {
        private readonly int _fundModel = Attributes.FundModel_36;

        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;
        private readonly IAppsEarningsHistoryReferenceDataService _appsEarningsHistoryReferenceDataService;
        private readonly IFileDataService _fileDataService;

        public DataEntityMapper(
            ILARSReferenceDataService larsReferenceDataService,
            IPostcodesReferenceDataService postcodesReferenceDataService,
            IAppsEarningsHistoryReferenceDataService appsEarningsHistoryReferenceDataService,
            IFileDataService fileDataService)
        {
            _larsReferenceDataService = larsReferenceDataService;
            _postcodesReferenceDataService = postcodesReferenceDataService;
            _appsEarningsHistoryReferenceDataService = appsEarningsHistoryReferenceDataService;
            _fileDataService = fileDataService;
        }

        public IEnumerable<IDataEntity> MapTo(IEnumerable<ILearner> inputModels)
        {
            var global = BuildGlobal();

            var entities = inputModels.Where(l => l.LearningDeliveries.Any(ld => ld.FundModel == _fundModel)).Select(l => BuildGlobalDataEntity(l, global));

            return entities.Any() ? entities : new List<IDataEntity> { BuildGlobalDataEntity(null, global) };
        }

        public IDataEntity BuildGlobalDataEntity(ILearner learner, Global global)
        {
            return new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                    { Attributes.Year, new AttributeData(global.Year) },
                    { Attributes.CollectionPeriod, new AttributeData(global.CollectionPeriod) },
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                },
                Children = learner != null ? new List<IDataEntity>() { BuildLearnerDataEntity(learner) } : new List<IDataEntity>()
            };
        }

        public IDataEntity BuildLearnerDataEntity(ILearner learner)
        {
            var learnerEmploymentStatusDenormalized = BuildLearnerEmploymentStatusDenormalized(learner.LearnerEmploymentStatuses);
            var dasPostDisadvantage = _postcodesReferenceDataService.DASDisadvantagesForPostcode(learner.PostcodePrior);
            var appsEarningsHistory = _appsEarningsHistoryReferenceDataService.AECEarningsHistory(learner.ULN);

            return new DataEntity(Attributes.EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                    { Attributes.DateOfBirth, new AttributeData(learner.DateOfBirthNullable) },
                    { Attributes.ULN, new AttributeData(learner.ULN) },
                    { Attributes.PrevUKPRN, new AttributeData(learner.PrevUKPRNNullable) },
                    { Attributes.PMUKPRN, new AttributeData(learner.PMUKPRNNullable) }
                },
                Children =
                    (learner
                        .LearningDeliveries?
                        .Where(ld => ld.FundModel == _fundModel)
                        .Select(BuildLearningDeliveryDataEntity) ?? new List<IDataEntity>())
                        .Union(
                            learnerEmploymentStatusDenormalized?
                            .Select(BuildLearnerEmploymentStatus) ?? new List<IDataEntity>())
                        .Union(
                            dasPostDisadvantage?
                            .Select(BuildDASPostcodeDisadvantage) ?? new List<IDataEntity>())
                        .Union(
                            appsEarningsHistory?
                            .Select(BuildApprenticeshipsEarningsHistory) ?? new List<IDataEntity>())
                        .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(ILearningDelivery learningDelivery)
        {
            var learningDeliveryFAMDenormalized = BuildLearningDeliveryFAMDenormalized(learningDelivery.LearningDeliveryFAMs);
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
            var larsStandardAppenticeshipFunding = _larsReferenceDataService.LARSStandardApprenticeshipFunding(learningDelivery.StdCodeNullable, learningDelivery.ProgTypeNullable);
            var larsFrameworkAppenticeshipFunding = _larsReferenceDataService.LARSFrameworkApprenticeshipFunding(learningDelivery.FworkCodeNullable, learningDelivery.ProgTypeNullable, learningDelivery.PwayCodeNullable);
            var larsFrameworkCommonComponent = _larsReferenceDataService.LARSFrameworkCommonComponent(learningDelivery.LearnAimRef, learningDelivery.FworkCodeNullable, learningDelivery.ProgTypeNullable, learningDelivery.PwayCodeNullable);
            var larsStandardCommonComponent = _larsReferenceDataService.LARSStandardCommonComponent(learningDelivery.StdCodeNullable);
            var larsFunding = _larsReferenceDataService.LARSFundingsForLearnAimRef(learningDelivery.LearnAimRef);

            return new DataEntity(Attributes.EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { Attributes.AimType, new AttributeData(learningDelivery.AimType) },
                    { Attributes.CompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { Attributes.FrameworkCommonComponent, new AttributeData(larsLearningDelivery.FrameworkCommonComponent) },
                    { Attributes.FworkCode, new AttributeData(learningDelivery.FworkCodeNullable) },
                    { Attributes.LearnAimRef, new AttributeData(learningDelivery.LearnAimRef) },
                    { Attributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDateNullable) },
                    { Attributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { Attributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { Attributes.LrnDelFAM_EEF, new AttributeData(learningDeliveryFAMDenormalized.EEF) },
                    { Attributes.LrnDelFAM_LDM1, new AttributeData(learningDeliveryFAMDenormalized.LDM1) },
                    { Attributes.LrnDelFAM_LDM2, new AttributeData(learningDeliveryFAMDenormalized.LDM2) },
                    { Attributes.LrnDelFAM_LDM3, new AttributeData(learningDeliveryFAMDenormalized.LDM3) },
                    { Attributes.LrnDelFAM_LDM4, new AttributeData(learningDeliveryFAMDenormalized.LDM4) },
                    { Attributes.OrigLearnStartDate, new AttributeData(learningDelivery.OrigLearnStartDateNullable) },
                    { Attributes.OtherFundAdj, new AttributeData(learningDelivery.OtherFundAdjNullable) },
                    { Attributes.PriorLearnFundAdj, new AttributeData(learningDelivery.PriorLearnFundAdjNullable) },
                    { Attributes.ProgType, new AttributeData(learningDelivery.ProgTypeNullable) },
                    { Attributes.PwayCode, new AttributeData(learningDelivery.PwayCodeNullable) },
                    { Attributes.STDCode, new AttributeData(learningDelivery.StdCodeNullable) },
                },
                Children = (
                            learningDelivery?
                            .LearningDeliveryFAMs?
                            .Select(BuildLearningDeliveryFAM) ?? new List<IDataEntity>())
                            .Union(
                                   learningDelivery?
                                    .AppFinRecords?
                                    .Select(BuildApprenticeshipFinancialRecord) ?? new List<IDataEntity>())
                            .Union(
                                   larsStandardAppenticeshipFunding?
                                    .Select(BuildLARSStandardApprenticeshipFunding) ?? new List<IDataEntity>())
                            .Union(
                                   larsFrameworkAppenticeshipFunding?
                                    .Select(BuildLARSFrameworkApprenticeshipFunding) ?? new List<IDataEntity>())
                            .Union(
                                   larsFrameworkCommonComponent?
                                    .Select(BuildLARSFrameworkCommonComponent) ?? new List<IDataEntity>())
                            .Union(
                                   larsStandardCommonComponent?
                                    .Select(BuildLARSStandardCommonComponent) ?? new List<IDataEntity>())
                            .Union(
                                   larsFunding?
                                    .Select(BuildLARSFunding) ?? new List<IDataEntity>())
                            .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryFAM(ILearningDeliveryFAM learningDeliveryFAM)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryFAM)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnDelFAMCode, new AttributeData(learningDeliveryFAM.LearnDelFAMCode) },
                    { Attributes.LearnDelFAMDateTo, new AttributeData(learningDeliveryFAM.LearnDelFAMDateToNullable) },
                    { Attributes.LearnDelFAMDateFrom, new AttributeData(learningDeliveryFAM.LearnDelFAMDateFromNullable) },
                    { Attributes.LearnDelFAMType, new AttributeData(learningDeliveryFAM.LearnDelFAMType) },
                }
            };
        }

        public IDataEntity BuildLearnerEmploymentStatus(LearnerEmploymentStatusDenormalized learnerEmploymentStatus)
        {
            var l = learnerEmploymentStatus;

            return new DataEntity(Attributes.EntityLearnerEmploymentStatus)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AgreeId, new AttributeData(learnerEmploymentStatus.AgreeId) },
                    { Attributes.DateEmpStatApp, new AttributeData(learnerEmploymentStatus.DateEmpStatApp) },
                    { Attributes.EmpId, new AttributeData(learnerEmploymentStatus.EmpId) },
                    { Attributes.EMPStat, new AttributeData(learnerEmploymentStatus.EMPStat) },
                    { Attributes.EmpStatMon_SEM, new AttributeData(learnerEmploymentStatus.SEM) }
                }
            };
        }

        public IDataEntity BuildDASPostcodeDisadvantage(DasDisadvantage dasDisadvantage)
        {
            return new DataEntity(Attributes.EntitySFA_PostcodeDisadvantage)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.DisApprenticeshipUplift, new AttributeData(dasDisadvantage.Uplift) },
                    { Attributes.DisUpEffectiveFrom, new AttributeData(dasDisadvantage.EffectiveFrom) },
                    { Attributes.DisUpEffectiveTo, new AttributeData(dasDisadvantage.EffectiveTo) }
                }
            };
        }

        public IDataEntity BuildApprenticeshipsEarningsHistory(AECEarningsHistory aecEarningsHistory)
        {
            return new DataEntity(Attributes.EntityHistoricEarningInput)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AppIdentifierInput, new AttributeData(aecEarningsHistory.AppIdentifier) },
                    { Attributes.AppProgCompletedInTheYearInput, new AttributeData(aecEarningsHistory.AppProgCompletedInTheYearInput) },
                    { Attributes.HistoricCollectionReturnInput, new AttributeData(aecEarningsHistory.CollectionReturnCode) },
                    { Attributes.HistoricCollectionYearInput, new AttributeData(aecEarningsHistory.CollectionYear) },
                    { Attributes.HistoricDaysInYearInput, new AttributeData(aecEarningsHistory.DaysInYear) },
                    { Attributes.HistoricEffectiveTNPStartDateInput, new AttributeData(aecEarningsHistory.HistoricEffectiveTNPStartDateInput) },
                    { Attributes.HistoricEmpIdStartWithinYearInput, new AttributeData(aecEarningsHistory.HistoricEmpIdStartWithinYear) },
                    { Attributes.HistoricEmpIdEndWithinYearInput, new AttributeData(aecEarningsHistory.HistoricEmpIdEndWithinYear) },
                    { Attributes.HistoricFworkCodeInput, new AttributeData(aecEarningsHistory.FworkCode) },
                    { Attributes.HistoricLearnDelProgEarliestACT2DateInput, new AttributeData(aecEarningsHistory.HistoricLearnDelProgEarliestACT2DateInput) },
                    { Attributes.HistoricLearner1618AtStartInput, new AttributeData(aecEarningsHistory.HistoricLearner1618StartInput) },
                    { Attributes.HistoricLearnRefNumberInput, new AttributeData(aecEarningsHistory.LearnRefNumber) },
                    { Attributes.HistoricPMRAmountInput, new AttributeData(aecEarningsHistory.HistoricPMRAmount) },
                    { Attributes.HistoricProgrammeStartDateIgnorePathwayInput, new AttributeData(aecEarningsHistory.ProgrammeStartDateIgnorePathway) },
                    { Attributes.HistoricProgrammeStartDateMatchPathwayInput, new AttributeData(aecEarningsHistory.ProgrammeStartDateMatchPathway) },
                    { Attributes.HistoricProgTypeInput, new AttributeData(aecEarningsHistory.ProgType) },
                    { Attributes.HistoricPwayCodeInput, new AttributeData(aecEarningsHistory.PwayCode) },
                    { Attributes.HistoricTotalProgAimPaymentsInTheYearInput, new AttributeData(aecEarningsHistory.TotalProgAimPaymentsInTheYear) },
                    { Attributes.HistoricTotal1618UpliftPaymentsInTheYearInput, new AttributeData(aecEarningsHistory.HistoricTotal1618UpliftPaymentsInTheYearInput) },
                    { Attributes.HistoricSTDCodeInput, new AttributeData(aecEarningsHistory.STDCode) },
                    { Attributes.HistoricTNP1Input, new AttributeData(aecEarningsHistory.HistoricTNP1Input) },
                    { Attributes.HistoricTNP2Input, new AttributeData(aecEarningsHistory.HistoricTNP2Input) },
                    { Attributes.HistoricTNP3Input, new AttributeData(aecEarningsHistory.HistoricTNP3Input) },
                    { Attributes.HistoricTNP4Input, new AttributeData(aecEarningsHistory.HistoricTNP4Input) },
                    { Attributes.HistoricUKPRNInput, new AttributeData(aecEarningsHistory.UKPRN) },
                    { Attributes.HistoricULNInput, new AttributeData(aecEarningsHistory.ULN) },
                    { Attributes.HistoricUptoEndDateInput, new AttributeData(aecEarningsHistory.UptoEndDate) },
                    { Attributes.HistoricVirtualTNP3EndofTheYearInput, new AttributeData(aecEarningsHistory.HistoricVirtualTNP3EndOfTheYearInput) },
                    { Attributes.HistoricVirtualTNP4EndofTheYearInput, new AttributeData(aecEarningsHistory.HistoricVirtualTNP4EndOfTheYearInput) }
                }
            };
        }

        public IDataEntity BuildApprenticeshipFinancialRecord(IAppFinRecord appFinRecord)
        {
            return new DataEntity(Attributes.EntityApprenticeshipFinancialRecord)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AFinAmount, new AttributeData(appFinRecord.AFinAmount) },
                    { Attributes.AFinCode, new AttributeData(appFinRecord.AFinCode) },
                    { Attributes.AFinDate, new AttributeData(appFinRecord.AFinDate) },
                    { Attributes.AFinType, new AttributeData(appFinRecord.AFinType) }
                }
            };
        }

        public IDataEntity BuildLARSStandardApprenticeshipFunding(LARSStandardApprenticeshipFunding larsStandardApprenticeshipFunding)
        {
            return new DataEntity(Attributes.EntityStandardLARSApprenticshipFunding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.StandardAF1618EmployerAdditionalPayment, new AttributeData(larsStandardApprenticeshipFunding.SixteenToEighteenEmployerAdditionalPayment) },
                    { Attributes.StandardAF1618ProviderAdditionalPayment, new AttributeData(larsStandardApprenticeshipFunding.SixteenToEighteenProviderAdditionalPayment) },
                    { Attributes.StandardAF1618FrameworkUplift, new AttributeData(larsStandardApprenticeshipFunding.SixteenToEighteenFrameworkUplift) },
                    { Attributes.StandardAFCareLeaverAdditionalPayment, new AttributeData(larsStandardApprenticeshipFunding.CareLeaverAdditionalPayment) },
                    { Attributes.StandardAFEffectiveFrom, new AttributeData(larsStandardApprenticeshipFunding.EffectiveFrom) },
                    { Attributes.StandardAFEffectiveTo, new AttributeData(larsStandardApprenticeshipFunding.EffectiveTo) },
                    { Attributes.StandardAFFundingCategory, new AttributeData(larsStandardApprenticeshipFunding.FundingCategory) },
                    { Attributes.StandardAFMaxEmployerLevyCap, new AttributeData(larsStandardApprenticeshipFunding.MaxEmployerLevyCap) },
                    { Attributes.StandardAFReservedValue2, new AttributeData(larsStandardApprenticeshipFunding.ReservedValue2) },
                    { Attributes.StandardAFReservedValue3, new AttributeData(larsStandardApprenticeshipFunding.ReservedValue3) }
                }
            };
        }

        public IDataEntity BuildLARSFrameworkApprenticeshipFunding(LARSFrameworkApprenticeshipFunding larsFrameworkApprenticeshipFunding)
        {
            return new DataEntity(Attributes.EntityFrameworkLARSApprenticshipFunding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.FrameworkAF1618EmployerAdditionalPayment, new AttributeData(larsFrameworkApprenticeshipFunding.SixteenToEighteenEmployerAdditionalPayment) },
                    { Attributes.FrameworkAF1618ProviderAdditionalPayment, new AttributeData(larsFrameworkApprenticeshipFunding.SixteenToEighteenProviderAdditionalPayment) },
                    { Attributes.FrameworkAF1618FrameworkUplift, new AttributeData(larsFrameworkApprenticeshipFunding.SixteenToEighteenFrameworkUplift) },
                    { Attributes.FrameworkAFCareLeaverAdditionalPayment, new AttributeData(larsFrameworkApprenticeshipFunding.CareLeaverAdditionalPayment) },
                    { Attributes.FrameworkAFEffectiveFrom, new AttributeData(larsFrameworkApprenticeshipFunding.EffectiveFrom) },
                    { Attributes.FrameworkAFEffectiveTo, new AttributeData(larsFrameworkApprenticeshipFunding.EffectiveTo) },
                    { Attributes.FrameworkAFFundingCategory, new AttributeData(larsFrameworkApprenticeshipFunding.FundingCategory) },
                    { Attributes.FrameworkAFMaxEmployerLevyCap, new AttributeData(larsFrameworkApprenticeshipFunding.MaxEmployerLevyCap) },
                    { Attributes.FrameworkAFReservedValue2, new AttributeData(larsFrameworkApprenticeshipFunding.ReservedValue2) },
                    { Attributes.FrameworkAFReservedValue3, new AttributeData(larsFrameworkApprenticeshipFunding.ReservedValue3) }
                }
            };
        }

        public IDataEntity BuildLARSFrameworkCommonComponent(LARSFrameworkCommonComponent larsFrameworkCommonComponent)
        {
            return new DataEntity(Attributes.EntityLARSFrameworkCmnComp)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSFrameworkCommonComponentCode, new AttributeData(larsFrameworkCommonComponent.CommonComponent) },
                    { Attributes.LARSFrameworkCommonComponentEffectiveFrom, new AttributeData(larsFrameworkCommonComponent.EffectiveFrom) },
                    { Attributes.LARSFrameworkCommonComponentEffectiveTo, new AttributeData(larsFrameworkCommonComponent.EffectiveTo) },
                }
            };
        }

        public IDataEntity BuildLARSStandardCommonComponent(LARSStandardCommonComponent larsStandardCommonComponent)
        {
            return new DataEntity(Attributes.EntityStandardCommonComponent)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSStandardCommonComponentCode, new AttributeData(larsStandardCommonComponent.CommonComponent) },
                    { Attributes.LARSStandardCommonComponentEffectiveFrom, new AttributeData(larsStandardCommonComponent.EffectiveFrom) },
                    { Attributes.LARSStandardCommonComponentEffectiveTo, new AttributeData(larsStandardCommonComponent.EffectiveTo) },
                }
            };
        }

        public IDataEntity BuildLARSFunding(LARSFunding larsFunding)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryLARS_Funding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSFundCategory, new AttributeData(larsFunding.FundingCategory) },
                    { Attributes.LARSFundEffectiveFrom, new AttributeData(larsFunding.EffectiveFrom) },
                    { Attributes.LARSFundEffectiveTo, new AttributeData(larsFunding.EffectiveTo) },
                    { Attributes.LARSFundWeightedRate, new AttributeData(larsFunding.RateWeighted) },
                }
            };
        }

        public Global BuildGlobal()
        {
            return new Global()
            {
                LARSVersion = _larsReferenceDataService.LARSCurrentVersion(),
                Year = Attributes.YearValue,

                // ToDo: implement AcademicYear service over InternalCache for "CollectionPeriod" to calculate value.
                // This attribute is not used by rulebase at present 10/09/18.
                CollectionPeriod = Attributes.CollectionPeriodValue,
                UKPRN = _fileDataService.UKPRN()
            };
        }

        public IEnumerable<LearnerEmploymentStatusDenormalized> BuildLearnerEmploymentStatusDenormalized(IEnumerable<ILearnerEmploymentStatus> learnerEmploymentStatuses)
        {
            return learnerEmploymentStatuses?.Select(les => new LearnerEmploymentStatusDenormalized
            {
                AgreeId = les.AgreeId,
                DateEmpStatApp = les.DateEmpStatApp,
                EmpId = les.EmpIdNullable,
                EMPStat = les.EmpStat,
                SEM = les.EmploymentStatusMonitorings?.Where(e => e.ESMType == Attributes.SEM).Select(e => (int?)e.ESMCode).FirstOrDefault()
            });
        }

        public LearningDeliveryFAMDenormalized BuildLearningDeliveryFAMDenormalized(IEnumerable<ILearningDeliveryFAM> learningDeliveryFams)
        {
            var learningDeliveryFam = new Service.Model.LearningDeliveryFAMDenormalized();

            if (learningDeliveryFams != null)
            {
                learningDeliveryFams = learningDeliveryFams.ToList();

                var ldmArray = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.LDM).Select(f => f.LearnDelFAMCode).ToArray();

                Array.Resize(ref ldmArray, 4);

                learningDeliveryFam.EEF = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.EEF).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.LDM1 = ldmArray[0];
                learningDeliveryFam.LDM2 = ldmArray[1];
                learningDeliveryFam.LDM3 = ldmArray[2];
                learningDeliveryFam.LDM4 = ldmArray[3];
            }

            return learningDeliveryFam;
        }
    }
}
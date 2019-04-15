using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.FM81.Service.Constants;
using ESFA.DC.ILR.FundingService.FM81.Service.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM81.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<ILearner>
    {
        private readonly int _fundModel = Attributes.FundModel_81;
        private readonly int? _progType = Attributes.ProgType_25;

        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IFileDataService _fileDataService;

        public DataEntityMapper(
            ILARSReferenceDataService larsReferenceDataService,
            IFileDataService fileDataService)
        {
            _larsReferenceDataService = larsReferenceDataService;
            _fileDataService = fileDataService;
        }

        public IEnumerable<IDataEntity> MapTo(IEnumerable<ILearner> inputModels)
        {
            var global = BuildGlobal();

            var entities = inputModels.Where(l => l.LearningDeliveries.Any(ld => _fundModel == ld.FundModel && _progType == ld.ProgTypeNullable)).Select(l => BuildGlobalDataEntity(l, global));

            return entities.Any() ? entities : new List<IDataEntity> { BuildGlobalDataEntity(null, global) };
        }

        public IDataEntity BuildGlobalDataEntity(ILearner learner, Global global)
        {
            return new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                },
                Children = learner != null ? new List<IDataEntity>() { BuildLearnerDataEntity(learner) } : new List<IDataEntity>()
            };
        }

        public IDataEntity BuildLearnerDataEntity(ILearner learner)
        {
            var learnerEmploymentStatusDenormalized = BuildLearnerEmploymentStatusDenormalized(learner.LearnerEmploymentStatuses);

            return new DataEntity(Attributes.EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                    { Attributes.DateOfBirth, new AttributeData(learner.DateOfBirthNullable) },
                },
                Children =
                    (learner
                        .LearningDeliveries?
                        .Where(ld => ld.FundModel == _fundModel && ld.ProgTypeNullable == _progType)
                        .Select(BuildLearningDeliveryDataEntity) ?? new List<IDataEntity>())
                        .Union(
                            learnerEmploymentStatusDenormalized?
                            .Select(BuildLearnerEmploymentStatus) ?? new List<IDataEntity>())
                        .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(ILearningDelivery learningDelivery)
        {
            var learningDeliveryFAMDenormalized = BuildLearningDeliveryFAMDenormalized(learningDelivery.LearningDeliveryFAMs);
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
            var larsStandardCommonComponent = _larsReferenceDataService.LARSStandardCommonComponent(learningDelivery.StdCodeNullable);
            var larsStandardFunding = _larsReferenceDataService.LARSStandardFunding(learningDelivery.StdCodeNullable);

            return new DataEntity(Attributes.EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AchDate, new AttributeData(learningDelivery.AchDateNullable) },
                    { Attributes.AimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { Attributes.AimType, new AttributeData(learningDelivery.AimType) },
                    { Attributes.CompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { Attributes.FrameworkCommonComponent, new AttributeData(larsLearningDelivery.FrameworkCommonComponent) },
                    { Attributes.LearnAimRef, new AttributeData(learningDelivery.LearnAimRef) },
                    { Attributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDateNullable) },
                    { Attributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { Attributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { Attributes.LrnDelFAM_EEF, new AttributeData(learningDeliveryFAMDenormalized.EEF) },
                    { Attributes.LrnDelFAM_FFI, new AttributeData(learningDeliveryFAMDenormalized.FFI) },
                    { Attributes.LrnDelFAM_LDM1, new AttributeData(learningDeliveryFAMDenormalized.LDM1) },
                    { Attributes.LrnDelFAM_LDM2, new AttributeData(learningDeliveryFAMDenormalized.LDM2) },
                    { Attributes.LrnDelFAM_LDM3, new AttributeData(learningDeliveryFAMDenormalized.LDM3) },
                    { Attributes.LrnDelFAM_LDM4, new AttributeData(learningDeliveryFAMDenormalized.LDM4) },
                    { Attributes.LrnDelFAM_RES, new AttributeData(learningDeliveryFAMDenormalized.RES) },
                    { Attributes.LrnDelFAM_SOF, new AttributeData(learningDeliveryFAMDenormalized.SOF) },
                    { Attributes.LrnDelFAM_SPP, new AttributeData(learningDeliveryFAMDenormalized.SPP) },
                    { Attributes.OrigLearnStartDate, new AttributeData(learningDelivery.OrigLearnStartDateNullable) },
                    { Attributes.OtherFundAdj, new AttributeData(learningDelivery.OtherFundAdjNullable) },
                    { Attributes.Outcome, new AttributeData(learningDelivery.OutcomeNullable) },
                    { Attributes.PriorLearnFundAdj, new AttributeData(learningDelivery.PriorLearnFundAdjNullable) },
                    { Attributes.ProgType, new AttributeData(learningDelivery.ProgTypeNullable) },
                    { Attributes.STDCode, new AttributeData(learningDelivery.StdCodeNullable) },
                    { Attributes.WithdrawReason, new AttributeData(learningDelivery.WithdrawReasonNullable) },
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
                                   larsStandardCommonComponent?
                                    .Select(BuildLARSStandardCommonComponent) ?? new List<IDataEntity>())
                            .Union(
                                    larsStandardFunding?
                                    .Select(BuildLARSStandardFunding) ?? new List<IDataEntity>())
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
                    { Attributes.DateEmpStatApp, new AttributeData(learnerEmploymentStatus.DateEmpStatApp) },
                    { Attributes.EmpId, new AttributeData(learnerEmploymentStatus.EmpId) },
                    { Attributes.EMPStat, new AttributeData(learnerEmploymentStatus.EMPStat) },
                    { Attributes.EmpStatMon_SEM, new AttributeData(learnerEmploymentStatus.SEM) }
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

        public IDataEntity BuildLARSStandardCommonComponent(LARSStandardCommonComponent larsStandardCommonComponent)
        {
            return new DataEntity(Attributes.EntityStandardCommonComponent)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSStandardCommonComponentCode, new AttributeData(larsStandardCommonComponent.CommonComponent) },
                    { Attributes.LARSStandardCommonComponentStandardCode, new AttributeData(larsStandardCommonComponent.StandardCode) },
                    { Attributes.LARSStandardCommonComponentEffectiveFrom, new AttributeData(larsStandardCommonComponent.EffectiveFrom) },
                    { Attributes.LARSStandardCommonComponentEffectiveTo, new AttributeData(larsStandardCommonComponent.EffectiveTo) },
                }
            };
        }

        public IDataEntity BuildLARSStandardFunding(LARSStandardFunding larsStdFunding)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryLARS_StandardFunding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.SFFundableWithoutEmployer, new AttributeData(larsStdFunding.FundableWithoutEmployer) },
                    { Attributes.SF1618Incentive, new AttributeData(larsStdFunding.SixteenToEighteenIncentive) },
                    { Attributes.SFAchIncentive, new AttributeData(larsStdFunding.AchievementIncentive) },
                    { Attributes.SFCoreGovContCap, new AttributeData(larsStdFunding.CoreGovContributionCap) },
                    { Attributes.SFEffectiveFromDate, new AttributeData(larsStdFunding.EffectiveFrom) },
                    { Attributes.SFEffectiveToDate, new AttributeData(larsStdFunding.EffectiveTo) },
                    { Attributes.SFSmallBusIncentive, new AttributeData(larsStdFunding.SmallBusinessIncentive) },
                }
            };
        }

        public Global BuildGlobal()
        {
            return new Global()
            {
                LARSVersion = _larsReferenceDataService.LARSCurrentVersion(),
                UKPRN = _fileDataService.UKPRN()
            };
        }

        public IEnumerable<LearnerEmploymentStatusDenormalized> BuildLearnerEmploymentStatusDenormalized(IEnumerable<ILearnerEmploymentStatus> learnerEmploymentStatuses)
        {
            return learnerEmploymentStatuses?.Select(les => new LearnerEmploymentStatusDenormalized
            {
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
                learningDeliveryFam.FFI = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.FFI).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.LDM1 = ldmArray[0];
                learningDeliveryFam.LDM2 = ldmArray[1];
                learningDeliveryFam.LDM3 = ldmArray[2];
                learningDeliveryFam.LDM4 = ldmArray[3];
                learningDeliveryFam.RES = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.RES).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.SOF = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.SOF).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.SPP = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.SPP).Select(f => f.LearnDelFAMCode).FirstOrDefault();
            }

            return learningDeliveryFam;
        }
    }
}
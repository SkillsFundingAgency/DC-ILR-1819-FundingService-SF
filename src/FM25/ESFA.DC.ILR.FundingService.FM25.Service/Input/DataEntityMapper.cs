using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.FundingService.FM25.Service.Constants;
using ESFA.DC.ILR.FundingService.FM25.Service.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<ILearner>
    {
        private readonly int _fundModel = Attributes.FundModel_25;
        private readonly DateTime _orgFundingAppliesFrom = new DateTime(2018, 8, 1);

        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IOrganisationReferenceDataService _organisationReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;
        private readonly IFileDataService _fileDataService;

        public DataEntityMapper(ILARSReferenceDataService larsReferenceDataService, IOrganisationReferenceDataService organisationReferenceDataService, IPostcodesReferenceDataService postcodesReferenceDataService, IFileDataService fileDataService)
        {
            _larsReferenceDataService = larsReferenceDataService;
            _organisationReferenceDataService = organisationReferenceDataService;
            _postcodesReferenceDataService = postcodesReferenceDataService;
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
                    { Attributes.AreaCostFactor1618, new AttributeData(global.AreaCostFactor1618) },
                    { Attributes.DisadvantageProportion, new AttributeData(global.DisadvantageProportion) },
                    { Attributes.HistoricLargeProgrammeProportion, new AttributeData(global.HistoricLargeProgrammeProportion) },
                    { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                    { Attributes.OrgVersion, new AttributeData(global.OrgVersion) },
                    { Attributes.PostcodeDisadvantageVersion, new AttributeData(global.PostcodesVersion) },
                    { Attributes.ProgrammeWeighting, new AttributeData(global.ProgrammeWeighting) },
                    { Attributes.RetentionFactor, new AttributeData(global.RetentionFactor) },
                    { Attributes.SpecialistResources, new AttributeData(global.SpecialistResources) },
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                },
                Children = learner != null ? new List<IDataEntity>() { BuildLearnerDataEntity(learner) } : new List<IDataEntity>()
            };
        }

        public IDataEntity BuildLearnerDataEntity(ILearner learner)
        {
            var learnerFamDenormalized = BuildLearnerFAMDenormalized(learner.LearnerFAMs);

            var efaDisadvantageUplift = _postcodesReferenceDataService.LatestEFADisadvantagesUpliftForPostcode(learner.Postcode);

            return new DataEntity(Attributes.EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.DateOfBirth, new AttributeData(learner.DateOfBirthNullable) },
                    { Attributes.EngGrade, new AttributeData(learner.EngGrade) },
                    { Attributes.LearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                    { Attributes.LrnFAM_ECF, new AttributeData(learnerFamDenormalized.ECF) },
                    { Attributes.LrnFAM_EDF1, new AttributeData(learnerFamDenormalized.EDF1) },
                    { Attributes.LrnFAM_EDF2, new AttributeData(learnerFamDenormalized.EDF2) },
                    { Attributes.LrnFAM_EHC, new AttributeData(learnerFamDenormalized.EHC) },
                    { Attributes.LrnFAM_HNS, new AttributeData(learnerFamDenormalized.HNS) },
                    { Attributes.LrnFAM_MCF, new AttributeData(learnerFamDenormalized.MCF) },
                    { Attributes.MathGrade, new AttributeData(learner.MathGrade) },
                    { Attributes.PlanEEPHours, new AttributeData(learner.PlanEEPHoursNullable) },
                    { Attributes.PlanLearnHours, new AttributeData(learner.PlanLearnHoursNullable) },
                    { Attributes.PostcodeDisadvantageUplift, new AttributeData(efaDisadvantageUplift) },
                    { Attributes.ULN, new AttributeData(learner.ULN) },
                },
                Children =
                    (learner
                        .LearningDeliveries?
                        .Select(BuildLearningDeliveryDataEntity) ?? new List<IDataEntity>())
                        .Union(
                            _fileDataService.DPOutcomesForLearnRefNumber(learner.LearnRefNumber)?
                                .Select(BuildDPOutcome) ?? new List<IDataEntity>())
                        .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(ILearningDelivery learningDelivery)
        {
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
            var learningDeliveryFAMDenormalized = BuildLearningDeliveryFAMDenormalized(learningDelivery.LearningDeliveryFAMs);

            return new DataEntity(Attributes.EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { Attributes.AimType, new AttributeData(learningDelivery.AimType) },
                    { Attributes.AwardOrgCode, new AttributeData(larsLearningDelivery.AwardOrgCode) },
                    { Attributes.CompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { Attributes.EFACOFType, new AttributeData(larsLearningDelivery.EFACOFType) },
                    { Attributes.FundModel, new AttributeData(learningDelivery.FundModel) },
                    { Attributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDateNullable) },
                    { Attributes.LearnAimRef, new AttributeData(learningDelivery.LearnAimRef) },
                    { Attributes.LearnAimRefTitle, new AttributeData(larsLearningDelivery.LearnAimRefTitle) },
                    { Attributes.LearnAimRefType, new AttributeData(larsLearningDelivery.LearnAimRefType) },
                    { Attributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { Attributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { Attributes.LrnDelFAM_SOF, new AttributeData(learningDeliveryFAMDenormalized.SOF) },
                    { Attributes.LrnDelFAM_LDM1, new AttributeData(learningDeliveryFAMDenormalized.LDM1) },
                    { Attributes.LrnDelFAM_LDM2, new AttributeData(learningDeliveryFAMDenormalized.LDM2) },
                    { Attributes.LrnDelFAM_LDM3, new AttributeData(learningDeliveryFAMDenormalized.LDM3) },
                    { Attributes.LrnDelFAM_LDM4, new AttributeData(learningDeliveryFAMDenormalized.LDM4) },
                    { Attributes.ProgType, new AttributeData(learningDelivery.ProgTypeNullable) },
                    { Attributes.SectorSubjectAreaTier2, new AttributeData(larsLearningDelivery.SectorSubjectAreaTier2) },
                    { Attributes.WithdrawReason, new AttributeData(learningDelivery.WithdrawReasonNullable) },
                },
                Children = larsLearningDelivery?
                        .LARSValidities?
                        .Select(BuildLearningDeliveryLARSValidity)
                        .ToList()
                    ?? new List<IDataEntity>()
            };
        }

        public IDataEntity BuildDPOutcome(DPOutcome dpOutcome)
        {
            return new DataEntity(Attributes.EntityDPOutcome)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.OutCode, new AttributeData(dpOutcome.OutCode) },
                    { Attributes.OutType, new AttributeData(dpOutcome.OutType) },
                }
            };
        }

        public IDataEntity BuildLearningDeliveryLARSValidity(LARSValidity larsValidity)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryLARSValidity)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.ValidityCategory, new AttributeData(larsValidity.Category) },
                    { Attributes.ValidityLastNewStartDate, new AttributeData(larsValidity.LastNewStartDate) },
                    { Attributes.ValidityStartDate, new AttributeData(larsValidity.StartDate) },
                }
            };
        }

        public Global BuildGlobal()
        {
            var ukprn = _fileDataService.UKPRN();
            var orgFundings = _organisationReferenceDataService.OrganisationFundingForUKPRN(ukprn).Where(f => f.OrgFundFactType == Attributes.OrgFundFactorTypeEFA1619).ToList();

            return new Global()
            {
                AreaCostFactor1618 = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == _orgFundingAppliesFrom && f.OrgFundFactor == Attributes.OrgFundFactorHistoricAreaCost)?.OrgFundFactValue,
                DisadvantageProportion = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == _orgFundingAppliesFrom && f.OrgFundFactor == Attributes.OrgFundFactorHistoricDisadvantageFundingProportion)?.OrgFundFactValue,
                HistoricLargeProgrammeProportion = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == _orgFundingAppliesFrom && f.OrgFundFactor == Attributes.OrgFundFactorHistoricLargeProgProportion)?.OrgFundFactValue,
                LARSVersion = _larsReferenceDataService.LARSCurrentVersion(),
                OrgVersion = _organisationReferenceDataService.OrganisationVersion(),
                PostcodesVersion = _postcodesReferenceDataService.PostcodesCurrentVersion(),
                ProgrammeWeighting = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == _orgFundingAppliesFrom && f.OrgFundFactor == Attributes.OrgFundFactorHistoriProgCostWeigting)?.OrgFundFactValue,
                RetentionFactor = orgFundings.FirstOrDefault(f => f.OrgFundEffectiveFrom == _orgFundingAppliesFrom && f.OrgFundFactor == Attributes.OrgFundFactorHistoricRetention)?.OrgFundFactValue,
                SpecialistResources = orgFundings.FirstOrDefault(f => f.OrgFundFactor == Attributes.OrgFundFactorSpecialistResources)?.OrgFundFactValue == "1" ? true : false,
                UKPRN = ukprn
            };
        }

        public LearnerFAMDenormalized BuildLearnerFAMDenormalized(IEnumerable<ILearnerFAM> learnerFams)
        {
            var learnerFam = new LearnerFAMDenormalized();

            if (learnerFams != null)
            {
                learnerFams = learnerFams.ToList();

                var edfArray = learnerFams.Where(f => f.LearnFAMType == Attributes.EDF).Select(f => (int?)f.LearnFAMCode).ToArray();

                Array.Resize(ref edfArray, 2);

                learnerFam.ECF = learnerFams.Where(f => f.LearnFAMType == Attributes.ECF).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
                learnerFam.EDF1 = edfArray[0];
                learnerFam.EDF2 = edfArray[1];
                learnerFam.EHC = learnerFams.Where(f => f.LearnFAMType == Attributes.EHC).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
                learnerFam.HNS = learnerFams.Where(f => f.LearnFAMType == Attributes.HNS).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
                learnerFam.MCF = learnerFams.Where(f => f.LearnFAMType == Attributes.MCF).Select(f => (int?)f.LearnFAMCode).FirstOrDefault();
            }

            return learnerFam;
        }

        public LearningDeliveryFAMDenormalized BuildLearningDeliveryFAMDenormalized(IEnumerable<ILearningDeliveryFAM> learningDeliveryFams)
        {
            var learningDeliveryFam = new LearningDeliveryFAMDenormalized();

            if (learningDeliveryFams != null)
            {
                learningDeliveryFams = learningDeliveryFams.ToList();

                var ldmArray = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.LDM).Select(f => f.LearnDelFAMCode).ToArray();

                Array.Resize(ref ldmArray, 4);

                learningDeliveryFam.SOF = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.SOF).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.LDM1 = ldmArray[0];
                learningDeliveryFam.LDM2 = ldmArray[1];
                learningDeliveryFam.LDM3 = ldmArray[2];
                learningDeliveryFam.LDM4 = ldmArray[3];
            }

            return learningDeliveryFam;
        }
    }
}
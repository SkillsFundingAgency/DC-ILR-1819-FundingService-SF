using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.FM35.Service.Constants;
using ESFA.DC.ILR.FundingService.FM35.Service.Models;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<ILearner>
    {
        private readonly int _fundModel = Attributes.FundModel_35;

        private readonly ILargeEmployersReferenceDataService _largeEmployersReferenceDataService;
        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IOrganisationReferenceDataService _organisationReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;
        private readonly IFileDataService _fileDataService;

        public DataEntityMapper(
                    ILargeEmployersReferenceDataService largeEmployersReferenceDataService,
                    ILARSReferenceDataService larsReferenceDataService,
                    IOrganisationReferenceDataService organisationReferenceDataService,
                    IPostcodesReferenceDataService postcodesReferenceDataService,
                    IFileDataService fileDataService)
        {
            _largeEmployersReferenceDataService = largeEmployersReferenceDataService;
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
            var orgFunding = _organisationReferenceDataService.OrganisationFundingForUKPRN(global.UKPRN).Where(f => f.OrgFundFactType == Attributes.OrgFundFactorTypeAdultSkills).ToList();

            var globalEntity = new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                    { Attributes.OrgVersion, new AttributeData(global.OrgVersion) },
                    { Attributes.PostcodeDisadvantageVersion, new AttributeData(global.PostcodeDisadvantageVersion) },
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                },
                Children =
                    learner != null ?
                     new List<IDataEntity> { BuildLearnerDataEntity(learner) }
                     .Union(
                         orgFunding?
                         .Select(BuildOrgFundingDataEntity) ?? new List<IDataEntity>())
                         .ToList()
                     : new List<IDataEntity>()
            };

            return globalEntity;
        }

        public IDataEntity BuildLearnerDataEntity(ILearner learner)
        {
            var sfaPostDisadvantage = _postcodesReferenceDataService.SFADisadvantagesForPostcode(learner.PostcodePrior);

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
                        .Where(ld => ld.FundModel == _fundModel)
                        .Select(BuildLearningDeliveryDataEntity) ?? new List<IDataEntity>())
                        .Union(
                            learner.LearnerEmploymentStatuses?
                            .Select(BuildLearnerEmploymentStatus) ?? new List<IDataEntity>())
                        .Union(
                            sfaPostDisadvantage?
                            .Select(BuildSFAPostcodeDisadvantage) ?? new List<IDataEntity>())
                        .ToList()
            };
        }

        public IDataEntity BuildOrgFundingDataEntity(OrgFunding orgFunding)
        {
            return new DataEntity(Attributes.EntityOrgFunding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                    {
                        { Attributes.OrgFundEffectiveFrom,  new AttributeData(orgFunding.OrgFundEffectiveFrom) },
                        { Attributes.OrgFundEffectiveTo, new AttributeData(orgFunding.OrgFundEffectiveTo) },
                        { Attributes.OrgFundFactor, new AttributeData(orgFunding.OrgFundFactor) },
                        { Attributes.OrgFundFactType,  new AttributeData(orgFunding.OrgFundFactType) },
                        { Attributes.OrgFundFactValue,  new AttributeData(orgFunding.OrgFundFactValue) },
                    }
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(ILearningDelivery learningDelivery)
        {
            var learningDeliveryFAMDenormalized = BuildLearningDeliveryFAMDenormalized(learningDelivery.LearningDeliveryFAMs);
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
            var larsFrameworkAims = _larsReferenceDataService.LARSFFrameworkAimsForLearnAimRef(learningDelivery.LearnAimRef);
            var larsFunding = _larsReferenceDataService.LARSFundingsForLearnAimRef(learningDelivery.LearnAimRef);

            var larsAnnualValue = _larsReferenceDataService.LARSAnnualValuesForLearnAimRef(learningDelivery.LearnAimRef);
            var larsLearningDeliveryCategories = _larsReferenceDataService.LARSLearningDeliveryCategoriesForLearnAimRef(learningDelivery.LearnAimRef);
            var sfaAreaCost = _postcodesReferenceDataService.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode);

            var larsFwkAims = larsFrameworkAims?.ToList();

            int? frameworkComponentType = null;

            if (larsFrameworkAims != null
                && learningDelivery.FworkCodeNullable != null
                && learningDelivery.ProgTypeNullable != null
                && learningDelivery.PwayCodeNullable != null)
            {
                frameworkComponentType = larsFrameworkAims
                .Where(fwa =>
                       learningDelivery.FworkCodeNullable == fwa.FworkCode
                    && learningDelivery.ProgTypeNullable == fwa.ProgType
                    && learningDelivery.PwayCodeNullable == fwa.PwayCode)
                .Select(fwct => fwct.FrameworkComponentType).FirstOrDefault();
            }

            return new DataEntity(Attributes.EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AchDate, new AttributeData(learningDelivery.AchDateNullable) },
                    { Attributes.AddHours, new AttributeData(learningDelivery.AddHoursNullable) },
                    { Attributes.AimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { Attributes.AimType, new AttributeData(learningDelivery.AimType) },
                    { Attributes.CompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { Attributes.EmpOutcome, new AttributeData(learningDelivery.EmpOutcomeNullable) },
                    { Attributes.EnglandFEHEStatus, new AttributeData(larsLearningDelivery.EnglandFEHEStatus) },
                    { Attributes.EnglPrscID, new AttributeData(larsLearningDelivery.EnglPrscID) },
                    { Attributes.FworkCode, new AttributeData(learningDelivery.FworkCodeNullable) },
                    { Attributes.FrameworkCommonComponent, new AttributeData(larsLearningDelivery.FrameworkCommonComponent) },
                    { Attributes.FrameworkComponentType, new AttributeData(frameworkComponentType) },
                    { Attributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDateNullable) },
                    { Attributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { Attributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { Attributes.LrnDelFAM_EEF, new AttributeData(learningDeliveryFAMDenormalized.EEF) },
                    { Attributes.LrnDelFAM_LDM1, new AttributeData(learningDeliveryFAMDenormalized.LDM1) },
                    { Attributes.LrnDelFAM_LDM2, new AttributeData(learningDeliveryFAMDenormalized.LDM2) },
                    { Attributes.LrnDelFAM_LDM3, new AttributeData(learningDeliveryFAMDenormalized.LDM3) },
                    { Attributes.LrnDelFAM_LDM4, new AttributeData(learningDeliveryFAMDenormalized.LDM4) },
                    { Attributes.LrnDelFAM_FFI, new AttributeData(learningDeliveryFAMDenormalized.FFI) },
                    { Attributes.LrnDelFAM_RES, new AttributeData(learningDeliveryFAMDenormalized.RES) },
                    { Attributes.OrigLearnStartDate, new AttributeData(learningDelivery.OrigLearnStartDateNullable) },
                    { Attributes.OtherFundAdj, new AttributeData(learningDelivery.OtherFundAdjNullable) },
                    { Attributes.Outcome, new AttributeData(learningDelivery.OutcomeNullable) },
                    { Attributes.PriorLearnFundAdj, new AttributeData(learningDelivery.PriorLearnFundAdjNullable) },
                    { Attributes.ProgType, new AttributeData(learningDelivery.ProgTypeNullable) },
                    { Attributes.PwayCode, new AttributeData(learningDelivery.PwayCodeNullable) }
                },
                Children = (
                            learningDelivery?
                            .LearningDeliveryFAMs?
                            .Select(BuildLearningDeliveryFAM) ?? new List<IDataEntity>())
                            .Union(
                                   larsAnnualValue?
                                   .Select(BuildLARSAnnualValue) ?? new List<IDataEntity>())
                            .Union(
                                   larsLearningDeliveryCategories?
                                   .Select(BuildLARSLearningDeliveryCategories) ?? new List<IDataEntity>())
                            .Union(
                                   sfaAreaCost?
                                   .Select(BuildSFAAreaCost) ?? new List<IDataEntity>())
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

        public IDataEntity BuildLearnerEmploymentStatus(ILearnerEmploymentStatus learnerEmploymentStatus)
        {
            var largeEmployer = _largeEmployersReferenceDataService.LargeEmployersforEmpID(learnerEmploymentStatus.EmpIdNullable);

            return new DataEntity(Attributes.EntityLearnerEmploymentStatus)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.DateEmpStatApp, new AttributeData(learnerEmploymentStatus.DateEmpStatApp) },
                    { Attributes.EmpId, new AttributeData(learnerEmploymentStatus.EmpIdNullable) }
                },
                Children = (
                            largeEmployer?
                            .Select(BuildLargeEmployer) ?? new List<IDataEntity>())
                            .ToList()
            };
        }

        public IDataEntity BuildLargeEmployer(LargeEmployers largeEmployers)
        {
            return new DataEntity(Attributes.EntityLargeEmployerReferenceData)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LargeEmpEffectiveFrom, new AttributeData(largeEmployers.EffectiveFrom) },
                    { Attributes.LargeEmpEffectiveTo, new AttributeData(largeEmployers.EffectiveTo) }
                }
            };
        }

        public IDataEntity BuildSFAPostcodeDisadvantage(SfaDisadvantage sfaDisadvantage)
        {
            return new DataEntity(Attributes.EntitySFA_PostcodeDisadvantage)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.DisUplift, new AttributeData(sfaDisadvantage.Uplift) },
                    { Attributes.DisUpEffectiveFrom, new AttributeData(sfaDisadvantage.EffectiveFrom) },
                    { Attributes.DisUpEffectiveTo, new AttributeData(sfaDisadvantage.EffectiveTo) }
                }
            };
        }

        public IDataEntity BuildLARSAnnualValue(LARSAnnualValue larsAnnualValue)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryLARS_AnnualValue)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnDelAnnValBasicSkillsTypeCode, new AttributeData(larsAnnualValue.BasicSkillsType) },
                    { Attributes.LearnDelAnnValDateFrom, new AttributeData(larsAnnualValue.EffectiveFrom) },
                    { Attributes.LearnDelAnnValDateTo, new AttributeData(larsAnnualValue.EffectiveTo) }
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
                    { Attributes.LARSFundUnweightedRate, new AttributeData(larsFunding.RateUnWeighted) },
                    { Attributes.LARSFundWeightingFactor, new AttributeData(larsFunding.WeightingFactor) },
                }
            };
        }

        public IDataEntity BuildLARSLearningDeliveryCategories(LARSLearningDeliveryCategory larsLearningDeliveryCategory)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryLARS_Category)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnDelCatRef, new AttributeData(larsLearningDeliveryCategory.CategoryRef) },
                    { Attributes.LearnDelCatDateFrom, new AttributeData(larsLearningDeliveryCategory.EffectiveFrom) },
                    { Attributes.LearnDelCatDateTo, new AttributeData(larsLearningDeliveryCategory.EffectiveTo) },
                }
            };
        }

        public IDataEntity BuildSFAAreaCost(SfaAreaCost sfaAreaCost)
        {
            return new DataEntity(Attributes.EntityLearningDeliverySFA_PostcodeAreaCost)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AreaCosFactor, new AttributeData(sfaAreaCost.AreaCostFactor) },
                    { Attributes.AreaCosEffectiveFrom, new AttributeData(sfaAreaCost.EffectiveFrom) },
                    { Attributes.AreaCosEffectiveTo, new AttributeData(sfaAreaCost.EffectiveTo) },
                }
            };
        }

        public Global BuildGlobal()
        {
            return new Global()
            {
                LARSVersion = _larsReferenceDataService.LARSCurrentVersion(),
                OrgVersion = _organisationReferenceDataService.OrganisationVersion(),
                PostcodeDisadvantageVersion = _postcodesReferenceDataService.PostcodesCurrentVersion(),
                UKPRN = _fileDataService.UKPRN()
            };
        }

        public LearningDeliveryFAMDenormalized BuildLearningDeliveryFAMDenormalized(IEnumerable<ILearningDeliveryFAM> learningDeliveryFams)
        {
            var learningDeliveryFam = new LearningDeliveryFAMDenormalized();

            if (learningDeliveryFams != null)
            {
                learningDeliveryFams = learningDeliveryFams.ToList();

                var ldmArray = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.LearningDeliveryFAMTypeLDM).Select(f => f.LearnDelFAMCode).ToArray();

                Array.Resize(ref ldmArray, 4);

                learningDeliveryFam.EEF = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.LearningDeliveryFAMTypeEEF).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.FFI = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.LearningDeliveryFAMTypeFFI).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.RES = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.LearningDeliveryFAMTypeRES).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.LDM1 = ldmArray[0];
                learningDeliveryFam.LDM2 = ldmArray[1];
                learningDeliveryFam.LDM3 = ldmArray[2];
                learningDeliveryFam.LDM4 = ldmArray[3];
            }

            return learningDeliveryFam;
        }
    }
}
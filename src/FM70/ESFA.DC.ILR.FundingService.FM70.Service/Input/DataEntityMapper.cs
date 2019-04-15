using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.FundingService.FM70.Service.Constants;
using ESFA.DC.ILR.FundingService.FM70.Service.Models;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM70.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<ILearner>
    {
        private readonly int _fundModel = Attributes.FundModel_70;

        private readonly IFileDataService _fileDataService;
        private readonly IFCSReferenceDataService _fcsDataReferenceDataService;
        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;

        public DataEntityMapper(IFileDataService fileDataService, IFCSReferenceDataService fcsDataReferenceDataService, ILARSReferenceDataService larsReferenceDataService, IPostcodesReferenceDataService postcodesReferenceDataService)
        {
            _fileDataService = fileDataService;
            _fcsDataReferenceDataService = fcsDataReferenceDataService;
            _larsReferenceDataService = larsReferenceDataService;
            _postcodesReferenceDataService = postcodesReferenceDataService;
        }

        public IEnumerable<IDataEntity> MapTo(IEnumerable<ILearner> inputModels)
        {
            var global = BuildGlobal();

            var entities = inputModels.Where(l => l.LearningDeliveries.Any(ld => _fundModel == ld.FundModel)).Select(l => BuildGlobalDataEntity(l, global));

            return entities.Any() ? entities : new List<IDataEntity> { BuildGlobalDataEntity(null, global) };
        }

        public IDataEntity BuildGlobalDataEntity(ILearner learner, Global global)
        {
            var globalEntity = new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                },
                Children =
                    learner != null ? new List<IDataEntity> { BuildLearnerDataEntity(learner) } : new List<IDataEntity>()
            };

            return globalEntity;
        }

        public IDataEntity BuildLearnerDataEntity(ILearner learner)
        {
            var dpOutcomes = _fileDataService.DPOutcomesForLearnRefNumber(learner.LearnRefNumber);

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
                            dpOutcomes?
                            .Select(BuildDPOutcomes) ?? new List<IDataEntity>())
                        .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(ILearningDelivery learningDelivery)
        {
            var learningDeliveryFAMDenormalized = BuildLearningDeliveryFAMDenormalized(learningDelivery.LearningDeliveryFAMs);
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
            var larsAnnualValue = _larsReferenceDataService.LARSAnnualValuesForLearnAimRef(learningDelivery.LearnAimRef);
            var larsFunding = _larsReferenceDataService.LARSFundingsForLearnAimRef(learningDelivery.LearnAimRef);
            var sfaAreaCost = _postcodesReferenceDataService.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode);

            var fcsContractData = _fcsDataReferenceDataService.FcsContractsForConRef(learningDelivery.ConRefNumber);
            var esfData = BuildEsfDataFromContract(fcsContractData);

            return new DataEntity(Attributes.EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AchDate, new AttributeData(learningDelivery.AchDateNullable) },
                    { Attributes.AddHours, new AttributeData(learningDelivery.AddHoursNullable) },
                    { Attributes.AimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { Attributes.CalcMethod, new AttributeData(esfData.Select(c => c.CalcMethod).FirstOrDefault()) },
                    { Attributes.CompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { Attributes.ConRefNumber, new AttributeData(learningDelivery.ConRefNumber) },
                    { Attributes.GenreCode, new AttributeData(larsLearningDelivery.LearningDeliveryGenre) },
                    { Attributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDateNullable) },
                    { Attributes.LearnAimRef, new AttributeData(learningDelivery.LearnAimRef) },
                    { Attributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { Attributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { Attributes.LrnDelFAM_LDM1, new AttributeData(learningDeliveryFAMDenormalized.LDM1) },
                    { Attributes.LrnDelFAM_LDM2, new AttributeData(learningDeliveryFAMDenormalized.LDM2) },
                    { Attributes.LrnDelFAM_LDM3, new AttributeData(learningDeliveryFAMDenormalized.LDM3) },
                    { Attributes.LrnDelFAM_LDM4, new AttributeData(learningDeliveryFAMDenormalized.LDM4) },
                    { Attributes.LrnDelFAM_RES, new AttributeData(learningDeliveryFAMDenormalized.RES) },
                    { Attributes.OrigLearnStartDate, new AttributeData(learningDelivery.OrigLearnStartDateNullable) },
                    { Attributes.OtherFundAdj, new AttributeData(learningDelivery.OtherFundAdjNullable) },
                    { Attributes.Outcome, new AttributeData(learningDelivery.OutcomeNullable) },
                    { Attributes.PriorLearnFundAdj, new AttributeData(learningDelivery.PriorLearnFundAdjNullable) },
                },
                Children = (
                            larsAnnualValue?
                                   .Select(BuildLARSAnnualValue) ?? new List<IDataEntity>())
                            .Union(
                                   sfaAreaCost?
                                   .Select(BuildSFAAreaCost) ?? new List<IDataEntity>())
                            .Union(
                                    larsFunding?
                                   .Select(BuildLARSFunding) ?? new List<IDataEntity>())
                             .Union(
                                    esfData?
                                    .Select(BuildEsfDataEntity) ?? new List<IDataEntity>())
                            .ToList()
            };
        }

        public IDataEntity BuildLearnerEmploymentStatus(ILearnerEmploymentStatus learnerEmploymentStatus)
        {
            return new DataEntity(Attributes.EntityLearnerEmploymentStatus)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.DateEmpStatApp, new AttributeData(learnerEmploymentStatus.DateEmpStatApp) },
                    { Attributes.EmpStat, new AttributeData(learnerEmploymentStatus.EmpStat) }
                },
            };
        }

        public IDataEntity BuildDPOutcomes(DPOutcome dpOutcome)
        {
            return new DataEntity(Attributes.EntityDPOutcome)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.OutCode,  new AttributeData(dpOutcome.OutCode) },
                    { Attributes.OutType, new AttributeData(dpOutcome.OutType) },
                    { Attributes.OutCollDate, new AttributeData(dpOutcome.OutCollDate) },
                    { Attributes.OutStartDate,  new AttributeData(dpOutcome.OutStartDate) },
                    { Attributes.OutEndDate,  new AttributeData(dpOutcome.OutEndDate) },
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
            return new DataEntity(Attributes.EntityLearningDeliveryLARSFunding)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSFundingCategory, new AttributeData(larsFunding.FundingCategory) },
                    { Attributes.LARSFundingEffectiveFrom, new AttributeData(larsFunding.EffectiveFrom) },
                    { Attributes.LARSFundingEffectiveTo, new AttributeData(larsFunding.EffectiveTo) },
                    { Attributes.LARSFundingWeightedRate, new AttributeData(larsFunding.RateWeighted) },
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

        public IDataEntity BuildEsfDataEntity(EsfData esfData)
        {
            return new DataEntity(Attributes.EntityESFData)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.UnitCost, new AttributeData(esfData.UnitCost) },
                    { Attributes.ESFDeliverableCode, new AttributeData(esfData.ESFDeliverableCode) },
                    { Attributes.ESFDataPremiumFactor, new AttributeData(esfData.ESFDataPremiumFactor) },
                    { Attributes.EffectiveContractStartDate, new AttributeData(esfData.EffectiveContractStartDate) },
                    { Attributes.EffectiveContractEndDate, new AttributeData(esfData.EffectiveContractEndDate) }
                }
            };
        }

        public Global BuildGlobal()
        {
            return new Global()
            {
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

                learningDeliveryFam.RES = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.LearningDeliveryFAMTypeRES).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.LDM1 = ldmArray[0];
                learningDeliveryFam.LDM2 = ldmArray[1];
                learningDeliveryFam.LDM3 = ldmArray[2];
                learningDeliveryFam.LDM4 = ldmArray[3];
            }

            return learningDeliveryFam;
        }

        public IEnumerable<EsfData> BuildEsfDataFromContract(IEnumerable<FCSContractAllocation> fcsContractAllocations)
        {
            return fcsContractAllocations.SelectMany(e => e.FCSContractDeliverables.Select(cd => new EsfData
            {
                ESFDataPremiumFactor = e.LearningRatePremiumFactor,
                EffectiveContractStartDate = e.ContractStartDate,
                EffectiveContractEndDate = e.ContractEndDate,
                UnitCost = cd.UnitCost,
                CalcMethod = e.CalcMethod,
                ESFDeliverableCode = cd.ExternalDeliverableCode,
            }));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.ALB.Service.Constants;
using ESFA.DC.ILR.FundingService.ALB.Service.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.Service.Input
{
    public class DataEntityMapper : IDataEntityMapper<ILearner>
    {
        private readonly HashSet<int> _fundModels = new HashSet<int> { Attributes.FundModel_81, Attributes.FundModel_99 };

        private readonly ILARSReferenceDataService _larsReferenceDataService;
        private readonly IPostcodesReferenceDataService _postcodesReferenceDataService;
        private readonly IFileDataService _fileDataService;

        public DataEntityMapper(ILARSReferenceDataService larsReferenceDataService, IPostcodesReferenceDataService postcodesReferenceDataService, IFileDataService fileDataService)
        {
            _larsReferenceDataService = larsReferenceDataService;
            _postcodesReferenceDataService = postcodesReferenceDataService;
            _fileDataService = fileDataService;
        }

        public IEnumerable<IDataEntity> MapTo(IEnumerable<ILearner> inputModels)
        {
            var global = BuildGlobal();

            var entities = inputModels.Where(l => l.LearningDeliveries.Any(ld => _fundModels.Contains(ld.FundModel))).Select(l => BuildGlobalDataEntity(l, global));

            return entities.Any() ? entities : new List<IDataEntity> { BuildGlobalDataEntity(null, global) };
        }

        public IDataEntity BuildGlobalDataEntity(ILearner learner, Global global)
        {
            return new DataEntity(Attributes.EntityGlobal)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LARSVersion, new AttributeData(global.LARSVersion) },
                    { Attributes.PostcodeAreaCostVersion, new AttributeData(global.PostcodesVersion) },
                    { Attributes.UKPRN, new AttributeData(global.UKPRN) }
                },
                Children = learner != null ? new List<IDataEntity>() { BuildLearnerDataEntity(learner) } : new List<IDataEntity>()
            };
        }

        public IDataEntity BuildLearnerDataEntity(ILearner learner)
        {
            return new DataEntity(Attributes.EntityLearner)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnRefNumber, new AttributeData(learner.LearnRefNumber) },
                },
                Children =
                    (learner
                        .LearningDeliveries?
                        .Where(ld => _fundModels.Contains(ld.FundModel))
                        .Select(BuildLearningDeliveryDataEntity) ?? new List<IDataEntity>())
                        .ToList()
            };
        }

        public IDataEntity BuildLearningDeliveryDataEntity(ILearningDelivery learningDelivery)
        {
            var larsLearningDelivery = _larsReferenceDataService.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef);
            var sfaPostCodeAreaCost = _postcodesReferenceDataService.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode);
            var subsidyPilotPostcodeArea = _postcodesReferenceDataService.CareerLearningPilotsForPostcode(learningDelivery.DelLocPostCode);
            var learningDeliveryFAMDenormalized = BuildLearningDeliveryFAMDenormalized(learningDelivery.LearningDeliveryFAMs);
            var larsFunding = _larsReferenceDataService.LARSFundingsForLearnAimRef(learningDelivery.LearnAimRef);

            return new DataEntity(Attributes.EntityLearningDelivery)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.AimSeqNumber, new AttributeData(learningDelivery.AimSeqNumber) },
                    { Attributes.CompStatus, new AttributeData(learningDelivery.CompStatus) },
                    { Attributes.LearnActEndDate, new AttributeData(learningDelivery.LearnActEndDateNullable) },
                    { Attributes.LearnAimRefType, new AttributeData(larsLearningDelivery.LearnAimRefType) },
                    { Attributes.LearnDelFundModel, new AttributeData(learningDelivery.FundModel) },
                    { Attributes.LearnPlanEndDate, new AttributeData(learningDelivery.LearnPlanEndDate) },
                    { Attributes.LearnStartDate, new AttributeData(learningDelivery.LearnStartDate) },
                    { Attributes.LrnDelFAM_ADL, new AttributeData(learningDeliveryFAMDenormalized.ADL) },
                    { Attributes.LrnDelFAM_LDM1, new AttributeData(learningDeliveryFAMDenormalized.LDM1) },
                    { Attributes.LrnDelFAM_LDM2, new AttributeData(learningDeliveryFAMDenormalized.LDM2) },
                    { Attributes.LrnDelFAM_LDM3, new AttributeData(learningDeliveryFAMDenormalized.LDM3) },
                    { Attributes.LrnDelFAM_LDM4, new AttributeData(learningDeliveryFAMDenormalized.LDM4) },
                    { Attributes.LrnDelFAM_RES, new AttributeData(learningDeliveryFAMDenormalized.RES) },
                    { Attributes.NotionalNVQLevelv2, new AttributeData(larsLearningDelivery.NotionalNVQLevelv2) },
                    { Attributes.OrigLearnStartDate, new AttributeData(learningDelivery.OrigLearnStartDateNullable) },
                    { Attributes.OtherFundAdj, new AttributeData(learningDelivery.OtherFundAdjNullable) },
                    { Attributes.Outcome, new AttributeData(learningDelivery.OutcomeNullable) },
                    { Attributes.PriorLearnFundAdj, new AttributeData(learningDelivery.PriorLearnFundAdjNullable) },
                    { Attributes.RegulatedCreditValue, new AttributeData(larsLearningDelivery.RegulatedCreditValue) },
                },
                Children = (
                            learningDelivery?
                            .LearningDeliveryFAMs?
                            .Select(BuildLearningDeliveryFAM) ?? new List<IDataEntity>())
                            .Union(
                                   larsLearningDelivery?
                                    .LARSCareerLearningPilot?
                                    .Select(BuildLARSCareerLearningPilot) ?? new List<IDataEntity>())
                            .Union(
                                   larsFunding?
                                    .Select(BuildLARSFunding) ?? new List<IDataEntity>())
                             .Union(
                                   _postcodesReferenceDataService
                                    .SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode)
                                    .Select(BuildSFAPostcodeAreaCost) ?? new List<IDataEntity>())
                             .Union(
                                   _postcodesReferenceDataService
                                    .CareerLearningPilotsForPostcode(learningDelivery.DelLocPostCode)
                                    .Select(BuildSubsidyPilotPostcodeArea) ?? new List<IDataEntity>())
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

        public IDataEntity BuildLARSCareerLearningPilot(LARSCareerLearningPilot larsCareerLearningPilot)
        {
            return new DataEntity(Attributes.EntityLearningDeliveryLARS_CareerLearningPilot)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.LearnDelLARSCarPilFundAreaCode, new AttributeData(larsCareerLearningPilot.AreaCode) },
                    { Attributes.LearnDelLARSCarPilFundEffFromDate, new AttributeData(larsCareerLearningPilot.EffectiveFrom) },
                    { Attributes.LearnDelLARSCarPilFundEffToDate, new AttributeData(larsCareerLearningPilot.EffectiveTo) },
                    { Attributes.LearnDelLARSCarPilFundSubsidyRate, new AttributeData(larsCareerLearningPilot.SubsidyRate) },
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
                    { Attributes.LARSFundWeightingFactor, new AttributeData(larsFunding.WeightingFactor) },
                }
            };
        }

        public IDataEntity BuildSFAPostcodeAreaCost(SfaAreaCost sfaAreaCost)
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

        public IDataEntity BuildSubsidyPilotPostcodeArea(CareerLearningPilot larsCareerLearningPilot)
        {
            return new DataEntity(Attributes.EntityLearningDeliverySubsidyPilotPostcodeArea)
            {
                Attributes = new Dictionary<string, IAttributeData>()
                {
                    { Attributes.SubsidyPilotAreaCode, new AttributeData(larsCareerLearningPilot.AreaCode) },
                    { Attributes.SubsidyPilotEffectiveFrom, new AttributeData(larsCareerLearningPilot.EffectiveFrom) },
                    { Attributes.SubsidyPilotEffectiveTo, new AttributeData(larsCareerLearningPilot.EffectiveTo) },
                }
            };
        }

        public Global BuildGlobal()
        {
            return new Global()
            {
                LARSVersion = _larsReferenceDataService.LARSCurrentVersion(),
                PostcodesVersion = _postcodesReferenceDataService.PostcodesCurrentVersion(),
                UKPRN = _fileDataService.UKPRN()
            };
        }

        public LearningDeliveryFAMDenormalized BuildLearningDeliveryFAMDenormalized(IEnumerable<ILearningDeliveryFAM> learningDeliveryFams)
        {
            var learningDeliveryFam = new Service.Model.LearningDeliveryFAMDenormalized();

            if (learningDeliveryFams != null)
            {
                learningDeliveryFams = learningDeliveryFams.ToList();

                var ldmArray = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.LDM).Select(f => f.LearnDelFAMCode).ToArray();

                Array.Resize(ref ldmArray, 4);

                learningDeliveryFam.ADL = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.ADL).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.RES = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.RES).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.LDM1 = ldmArray[0];
                learningDeliveryFam.LDM2 = ldmArray[1];
                learningDeliveryFam.LDM3 = ldmArray[2];
                learningDeliveryFam.LDM4 = ldmArray[3];
            }

            return learningDeliveryFam;
        }
    }
}
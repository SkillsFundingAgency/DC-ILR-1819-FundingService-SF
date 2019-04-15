using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Keys;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class LARSDataRetrievalService : ILARSDataRetrievalService
    {
        private readonly ILARS _lars;

        public LARSDataRetrievalService()
        {
        }

        public LARSDataRetrievalService(ILARS lars)
        {
            _lars = lars;
        }

        public virtual IQueryable<LARS_Funding> LARSFundings => _lars.LARS_Funding;

        public virtual IQueryable<LARS_LearningDelivery> LARSLearningDeliveries => _lars.LARS_LearningDelivery;

        public virtual IQueryable<LARS_Version> LARSVersions => _lars.LARS_Version;

        public virtual IQueryable<LARS_AnnualValue> LARSAnnualValues => _lars.LARS_AnnualValue;

        public virtual IQueryable<LARS_LearningDeliveryCategory> LARSLearningDeliveryCategories => _lars.LARS_LearningDeliveryCategory;

        public virtual IQueryable<LARS_FrameworkAims> LARSFrameworkAims => _lars.LARS_FrameworkAims;

        public virtual IQueryable<LARS_StandardCommonComponent> LARSStandardCommonComponents => _lars.LARS_StandardCommonComponent;

        public virtual IQueryable<LARS_FrameworkCmnComp> LARSFrameworkCommonComponents => _lars.LARS_FrameworkCmnComp;

        public virtual IQueryable<LARS_StandardFunding> LARSStandardFundings => _lars.LARS_StandardFunding;

        public virtual IQueryable<LARS_ApprenticeshipFunding> LARSApprenticeshipFundings => _lars.LARS_ApprenticeshipFunding;

        public string CurrentVersion()
        {
            return LARSVersions.OrderByDescending(v => v.MainDataSchemaName).Select(lv => lv.MainDataSchemaName).FirstOrDefault();
        }

        public IEnumerable<string> UniqueLearnAimRefs(IMessage message)
        {
            return message
                .Learners
                .Where(l => l.LearningDeliveries != null)
                .SelectMany(l => l.LearningDeliveries)
                .Select(ld => ld.LearnAimRef)
                .Distinct();
        }

        public IEnumerable<int> UniqueStandardCodes(IMessage message)
        {
            return message
                .Learners
                .Where(l => l.LearningDeliveries != null)
                .SelectMany(l => l.LearningDeliveries.Where(s => s.StdCodeNullable != null))
                .Select(ld => (int)ld.StdCodeNullable)
                .Distinct();
        }

        public IEnumerable<LARSFrameworkKey> UniqueFrameworkCommonComponents(IMessage message)
        {
            return message
                       .Learners?
                       .Where(l => l.LearningDeliveries != null)
                       .SelectMany(l => l.LearningDeliveries)
                       .Where(ld =>
                           ld.FworkCodeNullable.HasValue
                           && ld.ProgTypeNullable.HasValue
                           && ld.PwayCodeNullable.HasValue)
                       .Select(lf => new LARSFrameworkKey(lf.LearnAimRef, (int)lf.FworkCodeNullable, (int)lf.ProgTypeNullable, (int)lf.PwayCodeNullable)).Distinct();
        }

        public IEnumerable<LARSApprenticeshipFundingKey> UniqueApprenticeshipFundingStandards(IMessage message)
        {
            return message
                 .Learners
                 .Where(l => l.LearningDeliveries != null)
                 .SelectMany(l => l.LearningDeliveries
                 .Where(
                     s => s.StdCodeNullable != null
                     && s.ProgTypeNullable != null))
                 .Select(ld => new LARSApprenticeshipFundingKey((int)ld.StdCodeNullable, (int)ld.ProgTypeNullable, 0))
                 .Distinct();
        }

        public IEnumerable<LARSApprenticeshipFundingKey> UniqueApprenticeshipFundingFrameworks(IMessage message)
        {
            return message
                 .Learners
                 .Where(l => l.LearningDeliveries != null)
                 .SelectMany(l => l.LearningDeliveries
                 .Where(
                     s => s.FworkCodeNullable != null
                     && s.ProgTypeNullable != null
                     && s.PwayCodeNullable != null))
                 .Select(ld => new LARSApprenticeshipFundingKey((int)ld.FworkCodeNullable, (int)ld.ProgTypeNullable, (int)ld.PwayCodeNullable))
                 .Distinct();
        }

        public IDictionary<string, IEnumerable<LARSFunding>> LARSFundingsForLearnAimRefs(IEnumerable<string> learnAimRefs)
        {
            return LARSFundings
                .Where(lf => learnAimRefs.Contains(lf.LearnAimRef))
                .GroupBy(a => a.LearnAimRef)
                .ToCaseInsensitiveDictionary(a => a.Key, a => a.Select(LARSFundingFromEntity).ToList() as IEnumerable<LARSFunding>);
        }

        public LARSFunding LARSFundingFromEntity(LARS_Funding entity)
        {
            return new LARSFunding
            {
                LearnAimRef = entity.LearnAimRef,
                FundingCategory = entity.FundingCategory,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
                RateWeighted = entity.RateWeighted,
                WeightingFactor = entity.WeightingFactor,
                RateUnWeighted = entity.RateUnWeighted,
            };
        }

        public IDictionary<string, LARSLearningDelivery> LARSLearningDeliveriesForLearnAimRefs(IEnumerable<string> learnAimRefs)
        {
            return LARSLearningDeliveries
                    .Where(ld => learnAimRefs.Contains(ld.LearnAimRef))
                    .ToCaseInsensitiveDictionary(a => a.LearnAimRef, LARSLearningDeliveryFromEntity);
        }

        public LARSLearningDelivery LARSLearningDeliveryFromEntity(LARS_LearningDelivery entity)
        {
            return new LARSLearningDelivery
            {
                LearnAimRef = entity.LearnAimRef,
                LearnAimRefTitle = entity.LearnAimRefTitle,
                LearnAimRefType = entity.LearnAimRefType,
                LearningDeliveryGenre = entity.LearningDeliveryGenre,
                NotionalNVQLevelv2 = entity.NotionalNVQLevelv2,
                RegulatedCreditValue = entity.RegulatedCreditValue,
                EnglandFEHEStatus = entity.EnglandFEHEStatus,
                EnglPrscID = entity.EnglPrscID,
                FrameworkCommonComponent = entity.FrameworkCommonComponent,
                AwardOrgCode = entity.AwardOrgCode,
                EFACOFType = entity.EFACOFType,
                SectorSubjectAreaTier2 = entity.SectorSubjectAreaTier2,
                LARSValidities = entity.LARS_Validity.Select(LARSValidityFromEntity).ToList(),
                LARSCareerLearningPilot = entity.LARS_CareerLearningPilot.Select(LARSCareerLearningPilotFromEntity).ToList(),
                LARSFunding = entity.LARS_Funding.Select(LARSFundingFromEntity).ToList()
            };
        }

        public LARSValidity LARSValidityFromEntity(LARS_Validity entity)
        {
            return new LARSValidity()
            {
                Category = entity.ValidityCategory,
                LastNewStartDate = entity.LastNewStartDate,
                StartDate = entity.StartDate,
            };
        }

        public LARSCareerLearningPilot LARSCareerLearningPilotFromEntity(LARS_CareerLearningPilot entity)
        {
            return new LARSCareerLearningPilot()
            {
                AreaCode = entity.AreaCode,
                SubsidyRate = entity.SubsidyRate,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }

        public IDictionary<string, IEnumerable<LARSAnnualValue>> LARSAnnualValuesForLearnAimRefs(IEnumerable<string> learnAimRefs)
        {
            return LARSAnnualValues
                .Where(la => learnAimRefs.Contains(la.LearnAimRef))
                .GroupBy(a => a.LearnAimRef)
                .ToCaseInsensitiveDictionary(a => a.Key, a => a.Select(LARSAnnualValueFromEntity).ToList() as IEnumerable<LARSAnnualValue>);
        }

        public LARSAnnualValue LARSAnnualValueFromEntity(LARS_AnnualValue entity)
        {
            return new LARSAnnualValue()
            {
                LearnAimRef = entity.LearnAimRef,
                BasicSkillsType = entity.BasicSkillsType,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }

        public IDictionary<string, IEnumerable<LARSLearningDeliveryCategory>> LARSLearningDeliveryCategoriesForLearnAimRefs(IEnumerable<string> learnAimRefs)
        {
            return LARSLearningDeliveryCategories
                .Where(ld => learnAimRefs.Contains(ld.LearnAimRef))
                .GroupBy(a => a.LearnAimRef)
                .ToCaseInsensitiveDictionary(a => a.Key, a => a.Select(LARSLearningDeliveryCategoryFromEntity).ToList() as IEnumerable<LARSLearningDeliveryCategory>);
        }

        public LARSLearningDeliveryCategory LARSLearningDeliveryCategoryFromEntity(LARS_LearningDeliveryCategory entity)
        {
            return new LARSLearningDeliveryCategory()
            {
                LearnAimRef = entity.LearnAimRef,
                CategoryRef = entity.CategoryRef,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }

        public IDictionary<string, IEnumerable<LARSFrameworkAims>> LARSFrameworkAimsForLearnAimRefs(IEnumerable<string> learnAimRefs)
        {
            return LARSFrameworkAims
              .Where(lf => learnAimRefs.Contains(lf.LearnAimRef))
              .GroupBy(a => a.LearnAimRef)
              .ToCaseInsensitiveDictionary(a => a.Key, a => a.Select(LARSFrameworkAimsFromEntity).ToList() as IEnumerable<LARSFrameworkAims>);
        }

        public LARSFrameworkAims LARSFrameworkAimsFromEntity(LARS_FrameworkAims entity)
        {
            return new LARSFrameworkAims()
            {
                LearnAimRef = entity.LearnAimRef,
                FworkCode = entity.FworkCode,
                ProgType = entity.ProgType,
                PwayCode = entity.PwayCode,
                FrameworkComponentType = entity.FrameworkComponentType,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }

        public IDictionary<int, IEnumerable<LARSStandardCommonComponent>> LARSStandardCommonComponentForStandardCode(IEnumerable<int> standardCodes)
        {
            return LARSStandardCommonComponents
                .Where(sc => standardCodes.Contains(sc.StandardCode))
                .GroupBy(s => s.StandardCode)
                .ToDictionary(a => a.Key, a => a.Select(LARSStandardCommonComponentFromEntity).ToList() as IEnumerable<LARSStandardCommonComponent>);
        }

        public LARSStandardCommonComponent LARSStandardCommonComponentFromEntity(LARS_StandardCommonComponent entity)
        {
            return new LARSStandardCommonComponent()
            {
                StandardCode = entity.StandardCode,
                CommonComponent = entity.CommonComponent,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }

        public IEnumerable<LARSFrameworkCommonComponent> LARSFrameworkCommonComponentForLearnAimRefs(IEnumerable<LARSFrameworkKey> larsFrameworkKeys)
        {
            var learnAimRefsFromKey = larsFrameworkKeys.Select(lfk => lfk.LearnAimRef).Distinct().ToCaseInsensitiveHashSet();

            var larsFrameworkCommonComponents = new List<LARSFrameworkCommonComponent>();

            var larsFrameworksCmnComponents = LARSLearningDeliveries
                    .Where(ld => learnAimRefsFromKey.Contains(ld.LearnAimRef))
                    .Join(
                    LARSFrameworkCommonComponents,
                    ld => ld.FrameworkCommonComponent,
                    fcc => fcc.CommonComponent,
                    (ld, fcc) => new { LARSFrameworkCommonComponent = fcc, LARSLearningDelivery = ld })
                    .Select(fc => new LARSFrameworkCommonComponent
                    {
                        LearnAimRef = fc.LARSLearningDelivery.LearnAimRef,
                        CommonComponent = fc.LARSFrameworkCommonComponent.CommonComponent,
                        FworkCode = fc.LARSFrameworkCommonComponent.FworkCode,
                        PwayCode = fc.LARSFrameworkCommonComponent.PwayCode,
                        ProgType = fc.LARSFrameworkCommonComponent.ProgType,
                        EffectiveFrom = fc.LARSFrameworkCommonComponent.EffectiveFrom,
                        EffectiveTo = fc.LARSFrameworkCommonComponent.EffectiveTo
                    })
                    .ToList();

            foreach (var larsFrameworkKey in larsFrameworkKeys)
            {
                var larsFramework =
                larsFrameworksCmnComponents?
                    .Where(
                        fc => fc.LearnAimRef.CaseInsensitiveEquals(larsFrameworkKey.LearnAimRef)
                        && fc.FworkCode == larsFrameworkKey.FworkCode
                        && fc.ProgType == larsFrameworkKey.ProgType
                        && fc.PwayCode == larsFrameworkKey.PwayCode)
                        .Select(fcc => fcc).SingleOrDefault();

                if (larsFramework != null)
                {
                    larsFrameworkCommonComponents.Add(larsFramework);
                }
            }

            return larsFrameworkCommonComponents;
        }

        public IDictionary<int, IEnumerable<LARSStandardFunding>> LARSStandardFundingForStandardCodes(IEnumerable<int> standardCodes)
        {
            return LARSStandardFundings
                .Where(sc => standardCodes.Contains(sc.StandardCode))
                .GroupBy(s => s.StandardCode)
                .ToDictionary(a => a.Key, a => a.Select(LARSStandardFundingFromEntity).ToList() as IEnumerable<LARSStandardFunding>);
        }

        public LARSStandardFunding LARSStandardFundingFromEntity(LARS_StandardFunding entity)
        {
            return new LARSStandardFunding()
            {
                StandardCode = entity.StandardCode,
                FundingCategory = entity.FundingCategory,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
                AchievementIncentive = entity.AchievementIncentive,
                BandNumber = entity.BandNumber,
                CoreGovContributionCap = entity.CoreGovContributionCap,
                FundableWithoutEmployer = entity.FundableWithoutEmployer,
                SixteenToEighteenIncentive = entity.C1618Incentive,
                SmallBusinessIncentive = entity.SmallBusinessIncentive
            };
        }

        public IEnumerable<LARSStandardApprenticeshipFunding> LARSApprenticeshipFundingStandards(IEnumerable<LARSApprenticeshipFundingKey> apprenticeshipFundingKeys)
        {
            var apprenticeshipFundings = new List<LARSStandardApprenticeshipFunding>();

            foreach (var apprenticeshipFundingKey in apprenticeshipFundingKeys)
            {
                var appFunding =
                    LARSApprenticeshipFundings?
                    .Where(
                        a => a.ApprenticeshipType == "STD"
                        && a.ApprenticeshipCode == apprenticeshipFundingKey.Code
                        && a.ProgType == apprenticeshipFundingKey.ProgType
                        && a.PwayCode == apprenticeshipFundingKey.PwayCode)
                    .Select(l => l) as IEnumerable<LARS_ApprenticeshipFunding>;

                if (appFunding != null)
                {
                    apprenticeshipFundings.AddRange(
                        appFunding
                        .Select(u => LARSStandardApprenticeshipFundingFromEntity(u)));
                }
            }

            return apprenticeshipFundings.ToCaseInsensitiveHashSet();
        }

        public LARSStandardApprenticeshipFunding LARSStandardApprenticeshipFundingFromEntity(LARS_ApprenticeshipFunding entity)
        {
            return new LARSStandardApprenticeshipFunding
            {
                ApprenticeshipCode = entity.ApprenticeshipCode,
                ApprenticeshipType = entity.ApprenticeshipType,
                BandNumber = entity.BandNumber,
                CareLeaverAdditionalPayment = entity.CareLeaverAdditionalPayment,
                ProgType = entity.ProgType,
                PwayCode = entity.PwayCode,
                FundingCategory = entity.FundingCategory,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
                CoreGovContributionCap = entity.CoreGovContributionCap,
                SixteenToEighteenIncentive = entity.C1618Incentive,
                SixteenToEighteenProviderAdditionalPayment = entity.C1618ProviderAdditionalPayment,
                SixteenToEighteenEmployerAdditionalPayment = entity.C1618EmployerAdditionalPayment,
                SixteenToEighteenFrameworkUplift = entity.C1618FrameworkUplift,
                Duration = entity.Duration,
                ReservedValue2 = entity.ReservedValue2,
                ReservedValue3 = entity.ReservedValue3,
                MaxEmployerLevyCap = entity.MaxEmployerLevyCap,
                FundableWithoutEmployer = entity.FundableWithoutEmployer
            };
        }

        public IEnumerable<LARSFrameworkApprenticeshipFunding> LARSApprenticeshipFundingFrameworks(IEnumerable<LARSApprenticeshipFundingKey> apprenticeshipFundingKeys)
        {
            var apprenticeshipFundings = new List<LARSFrameworkApprenticeshipFunding>();

            foreach (var apprenticeshipFundingKey in apprenticeshipFundingKeys)
            {
                var appFunding =
                    LARSApprenticeshipFundings
                    .Where(
                        a => a.ApprenticeshipType == "FWK"
                        && a.ApprenticeshipCode == apprenticeshipFundingKey.Code
                        && a.ProgType == apprenticeshipFundingKey.ProgType
                        && a.PwayCode == apprenticeshipFundingKey.PwayCode)
                    .Select(l => l) as IEnumerable<LARS_ApprenticeshipFunding>;

                if (appFunding != null)
                {
                    apprenticeshipFundings.AddRange(
                        appFunding
                        .Select(u => LARSFrameworkApprenticeshipFundingFromEntity(u)));
                }
            }

            return apprenticeshipFundings.ToCaseInsensitiveHashSet();
        }

        public LARSFrameworkApprenticeshipFunding LARSFrameworkApprenticeshipFundingFromEntity(LARS_ApprenticeshipFunding entity)
        {
            return new LARSFrameworkApprenticeshipFunding
            {
                ApprenticeshipCode = entity.ApprenticeshipCode,
                ApprenticeshipType = entity.ApprenticeshipType,
                BandNumber = entity.BandNumber,
                CareLeaverAdditionalPayment = entity.CareLeaverAdditionalPayment,
                ProgType = entity.ProgType,
                PwayCode = entity.PwayCode,
                FundingCategory = entity.FundingCategory,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
                CoreGovContributionCap = entity.CoreGovContributionCap,
                SixteenToEighteenIncentive = entity.C1618Incentive,
                SixteenToEighteenProviderAdditionalPayment = entity.C1618ProviderAdditionalPayment,
                SixteenToEighteenEmployerAdditionalPayment = entity.C1618EmployerAdditionalPayment,
                SixteenToEighteenFrameworkUplift = entity.C1618FrameworkUplift,
                Duration = entity.Duration,
                ReservedValue2 = entity.ReservedValue2,
                ReservedValue3 = entity.ReservedValue3,
                MaxEmployerLevyCap = entity.MaxEmployerLevyCap,
                FundableWithoutEmployer = entity.FundableWithoutEmployer
            };
        }
    }
}

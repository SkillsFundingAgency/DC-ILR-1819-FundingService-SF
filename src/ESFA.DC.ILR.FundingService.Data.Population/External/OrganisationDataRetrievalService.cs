using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.Organisatons.Model;
using ESFA.DC.Data.Organisatons.Model.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class OrganisationDataRetrievalService : IOrganisationDataRetrievalService
    {
        private readonly IOrganisations _organisations;

        public OrganisationDataRetrievalService()
        {
        }

        public OrganisationDataRetrievalService(IOrganisations organisations)
        {
            _organisations = organisations;
        }

        public virtual IQueryable<Org_Version> OrgVersions => _organisations.Org_Version;

        public virtual IQueryable<Org_Funding> OrgFundings => _organisations.Org_Funding;

        public string CurrentVersion()
        {
            return OrgVersions.OrderByDescending(v => v.MainDataSchemaName).Select(lv => lv.MainDataSchemaName).FirstOrDefault();
        }

        public IDictionary<long, IEnumerable<OrgFunding>> OrgFundingsForUkprns(IEnumerable<long> ukprns)
        {
            return OrgFundings
                    .Where(o => ukprns.Contains(o.UKPRN))
                    .GroupBy(u => u.UKPRN)
                    .ToDictionary(a => a.Key, a => a.Select(OrgFundingFromEntity).ToList() as IEnumerable<OrgFunding>);
        }

        public OrgFunding OrgFundingFromEntity(Org_Funding entity)
        {
            return new OrgFunding()
            {
                UKPRN = entity.UKPRN,
                OrgFundFactor = entity.FundingFactor,
                OrgFundFactType = entity.FundingFactorType,
                OrgFundFactValue = entity.FundingFactorValue,
                OrgFundEffectiveFrom = entity.EffectiveFrom,
                OrgFundEffectiveTo = entity.EffectiveTo,
            };
        }
    }
}

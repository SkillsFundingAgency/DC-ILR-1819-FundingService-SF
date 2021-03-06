﻿using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;

namespace ESFA.DC.ILR.FundingService.Data.External.Organisation
{
    public class OrganisationReferenceDataService : IOrganisationReferenceDataService
    {
        private readonly IExternalDataCache _referenceDataCache;

        public OrganisationReferenceDataService(IExternalDataCache referenceDataCache)
        {
            _referenceDataCache = referenceDataCache;
        }

        public IEnumerable<OrgFunding> OrganisationFundingForUKPRN(int ukprn)
        {
            _referenceDataCache.OrgFunding.TryGetValue(ukprn, out IEnumerable<OrgFunding> orgFunding);

            return orgFunding?? new List<OrgFunding>();
        }

        public string OrganisationVersion()
        {
            return _referenceDataCache.OrgVersion;
        }
    }
}

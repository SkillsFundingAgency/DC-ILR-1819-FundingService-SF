using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.Organisatons.Model;
using ESFA.DC.Data.Organisatons.Model.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class OrganisationDataRetrievalServiceTests
    {
        [Fact]
        public void OrgVersions()
        {
            var organisationMock = new Mock<IOrganisations>();

            var orgVersions = NewService(organisationMock.Object).OrgVersions;

            organisationMock.VerifyGet(o => o.Org_Version);
        }

        [Fact]
        public void OrgFundings()
        {
            var organisationMock = new Mock<IOrganisations>();

            var orgFundings = NewService(organisationMock.Object).OrgFundings;

            organisationMock.VerifyGet(o => o.Org_Funding);
        }

        [Fact]
        public void CurrentVersion()
        {
            var orgVersions = new List<Org_Version>()
            {
                new Org_Version() { MainDataSchemaName = "001" },
                new Org_Version() { MainDataSchemaName = "002" },
                new Org_Version() { MainDataSchemaName = "003" }
            }.AsQueryable();

            var organisationDataRetrievalServiceMock = NewMock();

            organisationDataRetrievalServiceMock.SetupGet(o => o.OrgVersions).Returns(orgVersions);

            organisationDataRetrievalServiceMock.Object.CurrentVersion().Should().Be("003");
        }

        [Fact]
        public void OrgFundingsForUkprns()
        {
            var org_Fundings = new List<Org_Funding>()
            {
                new Org_Funding
                {
                    UKPRN = 1,
                    FundingFactorType = "Adult Skills"
                },
                new Org_Funding
                {
                    UKPRN = 2,
                    FundingFactorType = "Adult Skills"
                },
                new Org_Funding
                {
                    UKPRN = 2,
                    FundingFactorType = "EFA 16-19"
                },
                new Org_Funding
                {
                    UKPRN = 1,
                    FundingFactorType = "Adult Skills",
                },
                new Org_Funding
                {
                    UKPRN = 3,
                }
            }.AsQueryable();

            var organisationDataRetrievalServiceMock = NewMock();

            organisationDataRetrievalServiceMock.SetupGet(o => o.OrgFundings).Returns(org_Fundings);

            var ukprns = new List<long>() { 1, 2 };

            var orgFundings = organisationDataRetrievalServiceMock.Object.OrgFundingsForUkprns(ukprns);

            orgFundings.Should().HaveCount(2);
            orgFundings.Should().ContainKeys(1, 2);

            orgFundings[1].Should().HaveCount(2);
            orgFundings[2].Should().HaveCount(2);

            orgFundings.SelectMany(o => o.Value).Should().Contain(v => v.OrgFundFactType == "Adult Skills");
            orgFundings.SelectMany(o => o.Value).Should().Contain(v => v.OrgFundFactType == "EFA 16-19");
        }

        private OrganisationDataRetrievalService NewService(IOrganisations organisations = null)
        {
            return new OrganisationDataRetrievalService(organisations);
        }

        private Mock<OrganisationDataRetrievalService> NewMock()
        {
            return new Mock<OrganisationDataRetrievalService>()
            {
                CallBase = true
            };
        }
    }
}

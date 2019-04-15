using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Tests
{
    public class PostcodeFactorsReferenceDataServiceTests
    {
        [Fact]
        public void PostcodeFactorsCurrentVersion_Exists()
        {
            var postcodeFactorsVersion = "version";

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeCurrentVersion).Returns(postcodeFactorsVersion);

            NewService(referenceDataCacheMock.Object).PostcodesCurrentVersion().Should().Be(postcodeFactorsVersion);
        }

        [Fact]
        public void SFA_AreaCost()
        {
            var postcode = "postcode";
            var sfaAreaCosts = new List<SfaAreaCost>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { postcode, new PostcodeRoot() { SfaAreaCosts = sfaAreaCosts } }
                });

            NewService(referenceDataCacheMock.Object).SFAAreaCostsForPostcode(postcode).Should().BeSameAs(sfaAreaCosts);
        }

        [Fact]
        public void SFA_AreaCost_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { "postcode", null }
                });

            NewService(referenceDataCacheMock.Object).SFAAreaCostsForPostcode("notPostcode").Should().BeEmpty();
        }

        [Fact]
        public void SfaDisadvantageCost()
        {
            var postcode = "postcode";
            var sfaDisadvantages = new List<SfaDisadvantage>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { postcode, new PostcodeRoot() { SfaDisadvantages = sfaDisadvantages } }
                });

            NewService(referenceDataCacheMock.Object).SFADisadvantagesForPostcode(postcode).Should().BeSameAs(sfaDisadvantages);
        }

        [Fact]
        public void SfaDisadvantageCost_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { "postcode", null }
                });

            NewService(referenceDataCacheMock.Object).SFADisadvantagesForPostcode("notPostcode").Should().BeEmpty();
        }

        [Fact]
        public void DasDisadvantageCost()
        {
            var postcode = "postcode";
            var dasDisadvantages = new List<DasDisadvantage>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { postcode, new PostcodeRoot() { DasDisadvantages = dasDisadvantages } }
                });

            NewService(referenceDataCacheMock.Object).DASDisadvantagesForPostcode(postcode).Should().BeSameAs(dasDisadvantages);
        }

        [Fact]
        public void DasDisadvantageCost_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { "postcode", null }
                });

            NewService(referenceDataCacheMock.Object).DASDisadvantagesForPostcode("notPostcode").Should().BeEmpty();
        }

        [Fact]
        public void LatestEfaDisadvantage()
        {
            var postcode = "postcode";
            var uplift = 1.1m;

            var efaDisadvantageOne = new EfaDisadvantage
            {
                Postcode = "CV1 2WT",
                Uplift = 1.0m,
                EffectiveFrom = new DateTime(2000, 01, 01),
                EffectiveTo = new DateTime(2015, 07, 31),
            };

            var efaDisadvatageTwo = new EfaDisadvantage
            {
                Postcode = "CV1 2WT",
                Uplift = uplift,
                EffectiveFrom = new DateTime(2015, 08, 01),
                EffectiveTo = new DateTime(2019, 07, 31),
            };

            var efaDisadvantages = new List<EfaDisadvantage>()
            {
               efaDisadvantageOne,
               efaDisadvatageTwo
            };

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { postcode, new PostcodeRoot() { EfaDisadvantages = efaDisadvantages } }
                });

            NewService(referenceDataCacheMock.Object).LatestEFADisadvantagesUpliftForPostcode(postcode).Should().Be(efaDisadvatageTwo.Uplift);
        }

        [Fact]
        public void LatestEfaDisadvantage_ReturnsNullDecimal()
        {
            var postcode = "postcode";

            var efaDisadvantageOne = new EfaDisadvantage
            {
                Postcode = "CV1 2WT",

                EffectiveFrom = new DateTime(2000, 01, 01),
                EffectiveTo = new DateTime(2015, 07, 31),
            };

            var efaDisadvatageTwo = new EfaDisadvantage
            {
                Postcode = "CV1 2WT",

                EffectiveFrom = new DateTime(2015, 08, 01),
                EffectiveTo = new DateTime(2019, 07, 31),
            };

            var efaDisadvantages = new List<EfaDisadvantage>()
            {
               efaDisadvantageOne,
               efaDisadvatageTwo
            };

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { postcode, new PostcodeRoot() { EfaDisadvantages = efaDisadvantages } }
                });

            NewService(referenceDataCacheMock.Object).LatestEFADisadvantagesUpliftForPostcode(postcode).Should().BeNull();
        }

        [Fact]
        public void LatestEfaDisadvantage_ReturensNull_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { "postcode", null }
                });

            NewService(referenceDataCacheMock.Object).LatestEFADisadvantagesUpliftForPostcode("notPostcode").Should().BeNull();
        }

        [Fact]
        public void CareerLearningPilot()
        {
            var postcode = "postcode";
            var careerLearningPilot = new List<CareerLearningPilot>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { postcode, new PostcodeRoot() { CareerLearningPilots = careerLearningPilot } }
                });

            NewService(referenceDataCacheMock.Object).CareerLearningPilotsForPostcode(postcode).Should().BeSameAs(careerLearningPilot);
        }

        [Fact]
        public void CareerLearningPilot_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.PostcodeRoots)
                .Returns(new Dictionary<string, PostcodeRoot>()
                {
                    { "postcode", null }
                });

            NewService(referenceDataCacheMock.Object).CareerLearningPilotsForPostcode("notPostcode").Should().BeEmpty();
        }

        private PostcodesReferenceDataService NewService(IExternalDataCache referenceDataCache = null)
        {
            return new PostcodesReferenceDataService(referenceDataCache);
        }
    }
}

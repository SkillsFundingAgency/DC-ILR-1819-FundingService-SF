using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class PostcodesDataRetrievalServiceTests
    {
        [Fact]
        public void VersionInfos()
        {
            var postcodesMock = new Mock<IPostcodes>();

            var versionInfos = NewService(postcodesMock.Object).VersionInfos;

            postcodesMock.VerifyGet(p => p.VersionInfos);
        }

        [Fact]
        public void SfaPostcodeAreaCosts()
        {
            var postcodesMock = new Mock<IPostcodes>();

            var sfaPostcodeAreaCosts = NewService(postcodesMock.Object).SfaPostcodeAreaCosts;

            postcodesMock.VerifyGet(p => p.SFA_PostcodeAreaCost);
        }

        [Fact]
        public void SfaPostcodeDisadvantages()
        {
            var postcodesMock = new Mock<IPostcodes>();

            var sfaPostcodeDisadvantages = NewService(postcodesMock.Object).SfaPostcodeAreaCosts;

            postcodesMock.VerifyGet(p => p.SFA_PostcodeAreaCost);
        }

        [Fact]
        public void UniquePostcodes()
        {
            var message = new TestMessage()
            {
                Learners = new List<TestLearner>()
                {
                    new TestLearner()
                    {
                        Postcode = "A",
                        PostcodePrior = "B",
                        LearningDeliveries = new List<TestLearningDelivery>()
                        {
                            new TestLearningDelivery()
                            {
                                DelLocPostCode = "ABC",
                            },
                            new TestLearningDelivery()
                            {
                                DelLocPostCode = "DEF",
                            },
                            new TestLearningDelivery()
                            {
                                DelLocPostCode = "ABC",
                            }
                        }
                    },
                    new TestLearner
                    {
                        Postcode = "AB",
                        PostcodePrior = "B",
                    }
                }
            };

            var uniquePostcodes = NewService().UniquePostcodes(message).ToList();

            uniquePostcodes.Should().HaveCount(5);
            uniquePostcodes.Should().Contain("A", "B", "AB", "ABC", "DEF");
        }

        [Fact]
        public void VersionInfo()
        {
            var postcodesDataRetrievalServiceMock = NewMock();

            var versionInfos = new List<VersionInfo>()
            {
                new VersionInfo() { VersionNumber = "001" },
                new VersionInfo() { VersionNumber = "010" },
                new VersionInfo() { VersionNumber = "100" },
            }.AsQueryable();

            postcodesDataRetrievalServiceMock.SetupGet(v => v.VersionInfos).Returns(versionInfos);

            postcodesDataRetrievalServiceMock.Object.CurrentVersion().Should().Be("100");
        }

        [Fact]
        public void PostcodeRootsForPostcodes()
        {
            var sfaPostcodeDisadvantages = new List<SFA_PostcodeDisadvantage>()
            {
                new SFA_PostcodeDisadvantage()
                {
                    Postcode = "CV1 2TT",
                    Uplift = 1.5m,
                    EffectiveFrom = new DateTime(2000, 01, 01),
                    EffectiveTo = new DateTime(2015, 12, 31)
                },
                new SFA_PostcodeDisadvantage()
                {
                    Postcode = "CV1 2TT",
                    Uplift = 2.1m,
                    EffectiveFrom = new DateTime(2016, 01, 01),
                    EffectiveTo = null,
                },
                new SFA_PostcodeDisadvantage()
                {
                    MasterPostcode = new MasterPostcode { Postcode = "CV1 2WT" },
                    Postcode = "CV1 2WT",
                    Uplift = 1.2m,
                    EffectiveFrom = new DateTime(2000, 01, 01),
                    EffectiveTo = null,
                },
            }.AsQueryable();

            var masterPostcodes = new List<MasterPostcode>()
            {
                new MasterPostcode()
                {
                    Postcode = "CV1 2TT"
                },
                new MasterPostcode()
                {
                    Postcode = "CV1 2WT",
                }
            }.AsQueryable();

            var postcodesDataRetrievalServiceMock = NewMock();

            postcodesDataRetrievalServiceMock.SetupGet(p => p.MasterPostcodes).Returns(masterPostcodes);
            postcodesDataRetrievalServiceMock.SetupGet(p => p.CareerLearningPilot_Postcodes).Returns(new List<CareerLearningPilot_Postcode>().AsQueryable());
            postcodesDataRetrievalServiceMock.SetupGet(p => p.DasPostcodeDisadvantages).Returns(new List<DAS_PostcodeDisadvantage>().AsQueryable());
            postcodesDataRetrievalServiceMock.SetupGet(p => p.EfaPostcodeDisadvantages).Returns(new List<EFA_PostcodeDisadvantage>().AsQueryable());
            postcodesDataRetrievalServiceMock.SetupGet(p => p.SfaPostcodeAreaCosts).Returns(new List<SFA_PostcodeAreaCost>().AsQueryable());
            postcodesDataRetrievalServiceMock.SetupGet(p => p.SfaPostcodeDisadvantages).Returns(sfaPostcodeDisadvantages);

            var postcodes = new List<string>() { "CV1 2WT", "CV1 2TT" };

            var postcodeRoots = postcodesDataRetrievalServiceMock.Object.PostcodeRootsForPostcodes(postcodes);

            postcodeRoots.Should().HaveCount(2);
            postcodeRoots.Should().ContainKeys("CV1 2WT", "CV1 2TT");
            postcodeRoots.Should().NotContainKey("Fictional");

            postcodeRoots["CV1 2TT"].SfaDisadvantages.Should().HaveCount(2);
            postcodeRoots["CV1 2WT"].SfaDisadvantages.Should().HaveCount(1);
        }

        private PostcodesDataRetrievalService NewService(IPostcodes postcodes = null)
        {
            return new PostcodesDataRetrievalService(postcodes);
        }

        private Mock<PostcodesDataRetrievalService> NewMock()
        {
            return new Mock<PostcodesDataRetrievalService>
            {
                CallBase = true
            };
        }
    }
}

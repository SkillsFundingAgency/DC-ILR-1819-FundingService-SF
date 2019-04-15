using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.LargeEmployer.Model;
using ESFA.DC.Data.LargeEmployer.Model.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class LargeEmployersDataRetrievalServiceTests
    {
        [Fact]
        public void Employers()
        {
            var largeEmployersMock = new Mock<ILargeEmployer>();

            var employers = NewService(largeEmployersMock.Object).Employers;

            largeEmployersMock.VerifyGet(l => l.LEMP_Employers);
        }

        [Fact]
        public void UniqueEmployerIds()
        {
            var message = new TestMessage()
            {
                Learners = new List<TestLearner>()
                {
                    new TestLearner()
                    {
                        LearnerEmploymentStatuses = new List<TestLearnerEmploymentStatus>()
                        {
                            new TestLearnerEmploymentStatus()
                            {
                                EmpIdNullable = 1,
                            },
                            new TestLearnerEmploymentStatus()
                            {
                                EmpIdNullable = 2,
                            },
                            new TestLearnerEmploymentStatus()
                            {
                                EmpIdNullable = null
                            }
                        }
                    },
                    new TestLearner()
                }
            };

            var employerIds = NewService().UniqueEmployerIds(message).ToList();

            employerIds.Should().HaveCount(2);
            employerIds.Should().Contain(new List<int>() { 1, 2 });
        }

        [Fact]
        public void LargeEmployersForEmployerIds()
        {
            var lemp_Employers = new List<LEMP_Employers>()
            {
                new LEMP_Employers()
                {
                    ERN = 1
                },
                new LEMP_Employers()
                {
                    ERN = 2,
                },
                new LEMP_Employers()
                {
                    ERN = 1,
                },
                new LEMP_Employers()
                {
                    ERN = 3,
                }
            }.AsQueryable();

            var largeEmployersDataRetrievalServiceMock = NewMock();

            largeEmployersDataRetrievalServiceMock.SetupGet(l => l.Employers).Returns(lemp_Employers);

            var employerIds = new List<int>() { 1, 2 };

            var largeEmployers = largeEmployersDataRetrievalServiceMock.Object.LargeEmployersForEmployerIds(employerIds);

            largeEmployers.Should().HaveCount(2);

            largeEmployers.Should().ContainKeys(1, 2);

            largeEmployers[1].Should().HaveCount(2);
            largeEmployers[2].Should().HaveCount(1);
        }

        [Fact]
        public void LargeEmployer_EntityOutputCorrect()
        {
            var employerIds = new List<int>() { 1 };

            var lemp_Employer = new LEMP_Employers()
            {
                ERN = 1,
                EffectiveFrom = new DateTime(2017, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
            };

            var lemp_Employers = new List<LEMP_Employers>()
            {
               lemp_Employer
            }.AsQueryable();

            var largeEmployersDataRetrievalServiceMock = NewMock();

            largeEmployersDataRetrievalServiceMock.SetupGet(l => l.Employers).Returns(lemp_Employers);

            var largeEmployersDictionary = largeEmployersDataRetrievalServiceMock.Object.LargeEmployersForEmployerIds(employerIds);

            var largeEmployer = largeEmployersDictionary[1].FirstOrDefault();

            largeEmployer.ERN.Should().Be(lemp_Employer.ERN);
            largeEmployer.EffectiveFrom.Should().Be(lemp_Employer.EffectiveFrom);
            largeEmployer.EffectiveTo.Should().Be(lemp_Employer.EffectiveTo);
        }

        private LargeEmployersDataRetrievalService NewService(ILargeEmployer largeEmployers = null)
        {
            return new LargeEmployersDataRetrievalService(largeEmployers);
        }

        private Mock<LargeEmployersDataRetrievalService> NewMock()
        {
            return new Mock<LargeEmployersDataRetrievalService>()
            {
                CallBase = true
            };
        }
    }
}

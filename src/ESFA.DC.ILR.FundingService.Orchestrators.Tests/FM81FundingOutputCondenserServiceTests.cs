using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM81.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Orchestrators.Output;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Tests
{
    public class FM81FundingOutputCondenserServiceTests
    {
        [Fact]
        public void Condense()
        {
            var learnerOne = new FM81Learner();
            var learnerTwo = new FM81Learner();
            var learnerThree = new FM81Learner();
            var learnerFour = new FM81Learner();
            var learnerFive = new FM81Learner();
            var learnerSix = new FM81Learner();

            var globalOne = new FM81Global()
            {
                Learners = new List<FM81Learner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new FM81Global()
            {
                Learners = new List<FM81Learner>
                {
                    learnerThree,
                    learnerFour,
                },
            };

            var globalThree = new FM81Global()
            {
                Learners = new List<FM81Learner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<FM81Global>()
            {
                globalOne,
                globalTwo,
                globalThree,
            };

            var fundingOutput = NewService().Condense(fundingOutputs);

            fundingOutput.Should().Be(globalOne);
            fundingOutput.Learners.Should().HaveCount(6);
            fundingOutput.Learners.Should().Contain(new[] { learnerOne, learnerTwo, learnerThree, learnerFour, learnerFive, learnerSix });
        }

        [Fact]
        public void Condense_Null()
        {
            Action action = () => NewService().Condense(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Condense_NullLearners()
        {
            var globalOne = new FM81Global()
            {
                Learners = null
            };

            var globalTwo = new FM81Global()
            {
                Learners = null
            };

            var globalThree = new FM81Global()
            {
                Learners = null
            };

            var fundingOutputs = new List<FM81Global>()
            {
                globalOne,
                globalTwo,
                globalThree,
            };

            var fundingOutput = NewService().Condense(fundingOutputs);

            fundingOutput.Should().Be(globalOne);
            fundingOutput.Learners.Should().BeEmpty();
        }

        [Fact]
        public void Condense_MixedNullLearners()
        {
            var learnerOne = new FM81Learner();
            var learnerTwo = new FM81Learner();
            var learnerFive = new FM81Learner();
            var learnerSix = new FM81Learner();

            var globalOne = new FM81Global()
            {
                Learners = new List<FM81Learner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new FM81Global()
            {
                Learners = null,
            };

            var globalThree = new FM81Global()
            {
                Learners = new List<FM81Learner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<FM81Global>()
            {
                globalOne,
                globalTwo,
                globalThree,
            };

            var fundingOutput = NewService().Condense(fundingOutputs);

            fundingOutput.Should().Be(globalOne);
            fundingOutput.Learners.Should().HaveCount(4);
            fundingOutput.Learners.Should().Contain(new[] { learnerOne, learnerTwo, learnerFive, learnerSix });
        }


        private FM81FundingOutputCondenserService NewService()
        {
            return new FM81FundingOutputCondenserService();
        }
    }
}

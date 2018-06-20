﻿using System;
using System.Globalization;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface.Attribute;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Tests
{
    public class FM35FundingOutputModelTests
    {
        /// <summary>
        /// Return FundingOutput Model
        /// </summary>
        [Fact(DisplayName = "FundingOutputModel - FundingOutputs - Exists"), Trait("FundingOutputModel", "Unit")]
        public void FundingOutputModel_FundingOutputs_Exists()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var fundingOutputs = TestFundingOutputs();

            // ASSERT
            fundingOutputs.Should().NotBeNull();
        }

        #region Test Helpers

        private static IFM35FundingOutputs TestFundingOutputs()
        {
            return new FM35FundingOutputs
            {
                Global = new GlobalAttribute(), // TestGlobalAttribute(),
                Learners = new LearnerAttribute[] { new LearnerAttribute() }, // TestLearnerAttribute(),
            };
        }

        #endregion
    }
}

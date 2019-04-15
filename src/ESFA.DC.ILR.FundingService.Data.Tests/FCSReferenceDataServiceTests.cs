using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.FCS;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Tests
{
    public class FCSReferenceDataServiceTests
    {
        [Fact]
        public void FCSContractAllocation_Exists()
        {
            var conRef = "123";
            IEnumerable<FCSContractAllocation> contractAllocations = new List<FCSContractAllocation>
            {
                new FCSContractAllocation
                {
                    ContractAllocationNumber = "123"
                }
            };

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.FCSContractAllocations).Returns(contractAllocations);

            NewService(referenceDataCacheMock.Object).FcsContractsForConRef(conRef).Should().BeEquivalentTo(contractAllocations);
        }

        [Fact]
        public void FCSContractAllocation_NotExists()
        {
            var conRef = "456";
            var contractAllocations = new List<FCSContractAllocation>
            {
                new FCSContractAllocation
                {
                    ContractAllocationNumber = "123"
                }
            };

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.FCSContractAllocations).Returns(contractAllocations);

            NewService(referenceDataCacheMock.Object).FcsContractsForConRef(conRef).Should().BeNullOrEmpty();
        }

        private FCSReferenceDataService NewService(IExternalDataCache referenceDataCache = null)
        {
            return new FCSReferenceDataService(referenceDataCache);
        }
    }
}

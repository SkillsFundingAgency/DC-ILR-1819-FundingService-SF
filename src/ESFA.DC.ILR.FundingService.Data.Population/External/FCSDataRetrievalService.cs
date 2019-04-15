using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class FCSDataRetrievalService : IFCSDataRetrievalService
    {
        private readonly IFcsContext _fcs;

        public FCSDataRetrievalService()
        {
        }

        public FCSDataRetrievalService(IFcsContext fcs)
        {
            _fcs = fcs;
        }

        public virtual IQueryable<Contractor> Contractors => _fcs.Contractors;

        public virtual IQueryable<EsfEligibilityRule> EsfEligibilityRules => _fcs.EsfEligibilityRules;

        public virtual IQueryable<ContractDeliverableCodeMapping> DeliverableCodeMappings => _fcs.ContractDeliverableCodeMappings;

        public IEnumerable<string> UniqueConRefNumbers(IMessage message)
        {
            return message.Learners
                .Where(l => l.LearningDeliveries != null)
                .SelectMany(l => l.LearningDeliveries)
                .Where(c => c.ConRefNumber != null)
                .Select(ld => ld.ConRefNumber)
                .Distinct();
        }

        public IEnumerable<FCSContractAllocation> FCSContractsForUKPRN(int ukprn, IEnumerable<string> conRefNumbers)
        {
            if (Contractors.Where(c => c.Ukprn == ukprn).Any())
            {
                return Contractors
                     .Where(c => c.Ukprn == ukprn)
                     .SelectMany(c => c.Contracts
                     .SelectMany(ca => ca.ContractAllocations.Where(cr => conRefNumbers.Contains(cr.ContractAllocationNumber))
                     .Select(f => new FCSContractAllocation
                     {
                         ContractAllocationNumber = f.ContractAllocationNumber,
                         ContractStartDate = ca.StartDate,
                         ContractEndDate = ca.EndDate,
                         FundingStreamPeriodCode = f.FundingStreamPeriodCode,
                         LearningRatePremiumFactor = f.LearningRatePremiumFactor,
                         TenderSpecReference = f.TenderSpecReference,
                         LotReference = f.LotReference,
                         CalcMethod = EsfEligibilityRules
                           .Where(e => e.TenderSpecReference == f.TenderSpecReference && e.LotReference == f.LotReference)
                           .Select(cm => cm.CalcMethod).FirstOrDefault(),
                         FCSContractDeliverables = f.ContractDeliverables.Select(cd => new FCSContractDeliverable
                         {
                             DeliverableCode = cd.DeliverableCode,
                             DeliverableDescription = cd.Description,
                             PlannedValue = cd.PlannedValue,
                             PlannedVolume = cd.PlannedVolume,
                             UnitCost = cd.UnitCost,
                             ExternalDeliverableCode = DeliverableCodeMappings
                            .Where(dc =>
                                dc.FundingStreamPeriodCode == f.FundingStreamPeriodCode
                                && dc.FCSDeliverableCode == cd.DeliverableCode.ToString())
                            .Select(e => e.ExternalDeliverableCode).FirstOrDefault()
                         }).ToList()
                     }))).ToList() as IEnumerable<FCSContractAllocation>;
            }

            return null;
        }
    }
}

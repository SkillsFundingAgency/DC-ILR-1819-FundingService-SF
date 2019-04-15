using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Output
{
    public class FM70FundingOutputCondenserService : IFundingOutputCondenserService<FM70Global>
    {
        public FM70Global Condense(IEnumerable<FM70Global> fundingOutputs)
        {
            var firstOutput = fundingOutputs.FirstOrDefault();

            if (firstOutput != null)
            {
                firstOutput.Learners = fundingOutputs.Where(o => o.Learners != null).SelectMany(o => o.Learners).ToList();

                return firstOutput;
            }

            return new FM70Global();
        }
    }
}

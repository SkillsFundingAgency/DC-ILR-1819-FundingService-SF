using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.LargeEmployer.Model;
using ESFA.DC.Data.LargeEmployer.Model.Interface;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class LargeEmployersDataRetrievalService : ILargeEmployersDataRetrievalService
    {
        private readonly ILargeEmployer _largeEmployers;

        public LargeEmployersDataRetrievalService()
        {
        }

        public LargeEmployersDataRetrievalService(ILargeEmployer largeEmployers)
        {
            _largeEmployers = largeEmployers;
        }

        public virtual IQueryable<LEMP_Employers> Employers => _largeEmployers.LEMP_Employers;

        public IEnumerable<int> UniqueEmployerIds(IMessage message)
        {
            return message.Learners
                .Where(l => l.LearnerEmploymentStatuses != null)
                .SelectMany(l => l.LearnerEmploymentStatuses)
                .Where(les => les.EmpIdNullable.HasValue)
                .Select(les => les.EmpIdNullable.Value)
                .Distinct();
        }

        public IDictionary<int, IEnumerable<LargeEmployers>> LargeEmployersForEmployerIds(IEnumerable<int> employerIds)
        {
           var employersList = new List<LargeEmployers>();

            var employerShards = employerIds.SplitList(5000);

            foreach (var shard in employerShards)
            {
                employersList.AddRange(
                    Employers
                    .Where(l => shard.Contains(l.ERN))
                    .Select(lemp => new LargeEmployers
                    {
                        ERN = lemp.ERN,
                        EffectiveFrom = lemp.EffectiveFrom,
                        EffectiveTo = lemp.EffectiveTo,
                    }).ToList());
            }

            return employersList
                .GroupBy(e => e.ERN)
               .ToDictionary(k => k.Key, v => v.ToList() as IEnumerable<LargeEmployers>);
        }
    }
}

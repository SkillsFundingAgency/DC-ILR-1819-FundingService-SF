﻿using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.LargeEmployer.Model;
using ESFA.DC.Data.LargeEmployer.Model.Interface;
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
            return Employers
                .Where(l => employerIds.Contains(l.ERN))
                .GroupBy(e => e.ERN)
                .ToDictionary(a => a.Key, a => a.Select(LargeEmployersFromEntity).ToList() as IEnumerable<LargeEmployers>);
        }

        public LargeEmployers LargeEmployersFromEntity(LEMP_Employers entity)
        {
            return new LargeEmployers
            {
                ERN = entity.ERN,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }
    }
}
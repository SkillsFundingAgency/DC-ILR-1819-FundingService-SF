using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ESFA.DC.Data.Postcodes.Model;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Extensions;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.External
{
    public class PostcodesDataRetrievalService : IPostcodesDataRetrievalService
    {
        private const int ShardSize = 5000;
        private readonly IPostcodes _postcodes;

        public PostcodesDataRetrievalService()
        {
        }

        public PostcodesDataRetrievalService(IPostcodes postcodes)
        {
            _postcodes = postcodes;
        }

        public virtual IQueryable<VersionInfo> VersionInfos => _postcodes.VersionInfos;

        public virtual IQueryable<MasterPostcode> MasterPostcodes => _postcodes.MasterPostcodes;

        public virtual IQueryable<SFA_PostcodeAreaCost> SfaPostcodeAreaCosts => _postcodes.SFA_PostcodeAreaCost;

        public virtual IQueryable<SFA_PostcodeDisadvantage> SfaPostcodeDisadvantages => _postcodes.SFA_PostcodeDisadvantage;

        public virtual IQueryable<DAS_PostcodeDisadvantage> DasPostcodeDisadvantages => _postcodes.DAS_PostcodeDisadvantage;

        public virtual IQueryable<EFA_PostcodeDisadvantage> EfaPostcodeDisadvantages => _postcodes.EFA_PostcodeDisadvantage;

        public virtual IQueryable<CareerLearningPilot_Postcode> CareerLearningPilot_Postcodes => _postcodes.CareerLearningPilot_Postcode;

        public IEnumerable<string> UniquePostcodes(IMessage message)
        {
            return
                message.Learners.Select(l => l.Postcode)
                .Union(message.Learners.Select(l => l.PostcodePrior))
                .Union(message.Learners.Where(l => l.LearningDeliveries != null).SelectMany(l => l.LearningDeliveries).Select(ld => ld.DelLocPostCode))
                .Distinct();
        }

        public string CurrentVersion()
        {
            return VersionInfos.OrderByDescending(v => v.VersionNumber).Select(v => v.VersionNumber).FirstOrDefault();
        }

        public IDictionary<string, PostcodeRoot> PostcodeRootsForPostcodes(IEnumerable<string> postcodes)
        {
            var postcodeRoots = new List<PostcodeRoot>();

            var postcodeShards = postcodes.SplitList(ShardSize);

            foreach (var shard in postcodeShards)
            {
                postcodeRoots.AddRange(
                    MasterPostcodes
                        .Where(p => shard.Contains(p.Postcode))
                        .Select(p =>
                            new PostcodeRoot()
                            {
                                Postcode = p.Postcode,
                            }));
            }

            foreach (var postcodeRootsShard in postcodeRoots.SplitList(ShardSize))
            {
                var shardPostcodes = postcodeRootsShard.Select(p => p.Postcode).ToList();

                var careerLearningPilots = CareerLearningPilot_Postcodes
                    .Where(clp => shardPostcodes.Contains(clp.Postcode))
                    .Select(clp => new CareerLearningPilot()
                    {
                        Postcode = clp.Postcode,
                        AreaCode = clp.AreaCode,
                        EffectiveFrom = clp.EffectiveFrom,
                        EffectiveTo = clp.EffectiveTo,
                    })
                    .ToList()
                    .GroupBy(d => d.Postcode)
                    .ToCaseInsensitiveDictionary(d => d.Key, g => g.ToList());

                var dasDisadvantages = DasPostcodeDisadvantages
                    .Where(d => shardPostcodes.Contains(d.Postcode))
                    .Select(pd => new DasDisadvantage
                    {
                        Postcode = pd.Postcode,
                        Uplift = pd.Uplift,
                        EffectiveFrom = pd.EffectiveFrom,
                        EffectiveTo = pd.EffectiveTo,
                    })
                    .ToList()
                    .GroupBy(d => d.Postcode)
                    .ToCaseInsensitiveDictionary(d => d.Key, g => g.ToList());

                var efaDisadvantages = EfaPostcodeDisadvantages
                    .Where(d => shardPostcodes.Contains(d.Postcode))
                    .Select(ed => new EfaDisadvantage()
                    {
                        Postcode = ed.Postcode,
                        Uplift = ed.Uplift,
                        EffectiveFrom = ed.EffectiveFrom,
                        EffectiveTo = ed.EffectiveTo,
                    })
                    .ToList()
                    .GroupBy(d => d.Postcode)
                    .ToCaseInsensitiveDictionary(d => d.Key, g => g.ToList());

                var sfaAreaCosts = SfaPostcodeAreaCosts
                    .Where(c => shardPostcodes.Contains(c.Postcode))
                    .Select(ac => new SfaAreaCost()
                    {
                        Postcode = ac.Postcode,
                        AreaCostFactor = ac.AreaCostFactor,
                        EffectiveFrom = ac.EffectiveFrom,
                        EffectiveTo = ac.EffectiveTo
                    })
                    .ToList()
                    .GroupBy(d => d.Postcode)
                    .ToCaseInsensitiveDictionary(d => d.Key, g => g.ToList());

                var sfaDisadvantages = SfaPostcodeDisadvantages
                    .Where(d => shardPostcodes.Contains(d.Postcode))
                    .Select(pd => new SfaDisadvantage
                        {
                            Postcode = pd.Postcode,
                            Uplift = pd.Uplift,
                            EffectiveFrom = pd.EffectiveFrom,
                            EffectiveTo = pd.EffectiveTo,
                        })
                    .ToList()
                    .GroupBy(d => d.Postcode)
                    .ToCaseInsensitiveDictionary(d => d.Key, g => g.ToList());

                foreach (var postcodeRoot in postcodeRootsShard)
                {
                    var postcode = postcodeRoot.Postcode;

                    postcodeRoot.CareerLearningPilots = careerLearningPilots.ContainsKey(postcode) ? careerLearningPilots[postcode] : new List<CareerLearningPilot>();
                    postcodeRoot.DasDisadvantages = dasDisadvantages.ContainsKey(postcode) ? dasDisadvantages[postcode] : new List<DasDisadvantage>();
                    postcodeRoot.EfaDisadvantages = efaDisadvantages.ContainsKey(postcode) ? efaDisadvantages[postcode] : new List<EfaDisadvantage>();
                    postcodeRoot.SfaAreaCosts = sfaAreaCosts.ContainsKey(postcode) ? sfaAreaCosts[postcode] : new List<SfaAreaCost>();
                    postcodeRoot.SfaDisadvantages = sfaDisadvantages.ContainsKey(postcode) ? sfaDisadvantages[postcode] : new List<SfaDisadvantage>();
                }
            }

            return postcodeRoots.ToCaseInsensitiveDictionary(k => k.Postcode, v => v);
        }
    }
}

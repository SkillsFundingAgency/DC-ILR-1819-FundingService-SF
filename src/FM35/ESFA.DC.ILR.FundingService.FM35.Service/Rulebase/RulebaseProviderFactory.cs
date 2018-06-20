﻿using System.Linq;
using System.Reflection;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Rulebase
{
    public class RulebaseProviderFactory : IRulebaseProviderFactory
    {
        public IRulebaseProvider Build()
        {
            var rulebaseZipPath =
                Assembly.GetExecutingAssembly().GetManifestResourceNames()
                .Where(n => n.Contains("Rulebase"))
                .Select(r => r).SingleOrDefault();

            return new RulebaseProvider(rulebaseZipPath);
        }
    }
}

﻿using System;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.LARS.Model
{
    public class LARSAnnualValue
    {
        public string LearnAimRef { get; set; }

        public int? BasicSkillsType { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}

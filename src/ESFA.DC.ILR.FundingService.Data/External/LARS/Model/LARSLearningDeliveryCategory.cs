﻿using System;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Model
{
    public class LARSLearningDeliveryCategory
    {
        public string LearnAimRef { get; set; }

        public int CategoryRef { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}

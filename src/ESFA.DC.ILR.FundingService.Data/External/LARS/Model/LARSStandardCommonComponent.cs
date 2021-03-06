﻿using System;

namespace ESFA.DC.ILR.FundingService.Data.External.LARS.Model
{
    public class LARSStandardCommonComponent
    {
        public int CommonComponent { get; set; }

        public int StandardCode { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}

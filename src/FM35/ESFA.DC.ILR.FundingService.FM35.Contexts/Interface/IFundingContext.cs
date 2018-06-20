﻿using System.Collections.Generic;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.Contexts.Interface
{
    public interface IFundingContext
    {
        int UKPRN { get; }

        IList<ILearner> ValidLearners { get; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.JobContext.Interface;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Interfaces
{
    public interface IPreFundingSFOrchestrationService
    {
        Task Execute(IJobContextMessage jobContextMessage);
    }
}

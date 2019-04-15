using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.JobContextManager.Model.Interface;
using ESFA.DC.Logging.Interfaces;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;

namespace ESFA.DC.ILR.FundingService.Stateless.Handlers
{
    public class MessageHandler : IMessageHandler<JobContextMessage>
    {
        private readonly ILifetimeScope _parentLifeTimeScope;
        private readonly StatelessServiceContext _context;

        public MessageHandler(ILifetimeScope parentLifeTimeScope, StatelessServiceContext context)
        {
            _parentLifeTimeScope = parentLifeTimeScope;
            _context = context;
        }

        public async Task<bool> HandleAsync(JobContextMessage jobContextMessage, CancellationToken cancellationToken)
         {
            try
            {
                using (var childLifeTimeScope = _parentLifeTimeScope.BeginLifetimeScope(c =>
                    c.RegisterInstance(jobContextMessage).As<IJobContextMessage>()))
                {
                    // Get logger
                    ExecutionContext executionContext = (ExecutionContext)childLifeTimeScope.Resolve<IExecutionContext>();
                    executionContext.JobId = jobContextMessage.JobId.ToString();
                    ILogger logger = childLifeTimeScope.Resolve<ILogger>();

                    logger.LogDebug("Started Funding Calc Service");
                    var preFundingSfOrchestrationService =
                        childLifeTimeScope.Resolve<IPreFundingSFOrchestrationService>();

                    // Call logic asynchronously, and return on initial context (no .ConfigureAwait(false))
                    await preFundingSfOrchestrationService.ExecuteAsync(jobContextMessage, cancellationToken);

                    logger.LogDebug("Completed Funding Calc Service");
                }

                ServiceEventSource.Current.ServiceMessage(_context, "Completed Funding Calc Service");
                return true;
            }
            catch (OutOfMemoryException oom)
            {
                Environment.FailFast("Funding Service Out Of Memory", oom);
                throw;
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(_context, "Exception-{0}", ex.ToString());
                throw;
            }
        }
    }
}

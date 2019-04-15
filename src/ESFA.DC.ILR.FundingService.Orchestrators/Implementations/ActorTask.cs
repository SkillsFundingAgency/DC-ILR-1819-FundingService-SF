using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;
using ESFA.DC.ILR.FundingService.ServiceFabric.Common.Interfaces;
using ESFA.DC.ILR.FundingService.Stateless.Models;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Implementations
{
    public class ActorTask<TActor, TActorReturn> : IActorTask<TActor, TActorReturn>
        where TActor : IFundingActor
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly ILogger _logger;
        private readonly IActorProvider<TActor> _fundingActorProvider;
        private readonly IKeyValuePersistenceService _keyValuePersistenceService;
        private readonly IFundingOutputCondenserService<TActorReturn> _fundingOutputCondenserService;
        private readonly string _actorName;

        public ActorTask(
            IJsonSerializationService jsonSerializationService,
            IActorProvider<TActor> fundingActorProvider,
            IKeyValuePersistenceService keyValuePersistenceService,
            IFundingOutputCondenserService<TActorReturn> fundingOutputCondenserService,
            ILogger logger,
            string actorName)
        {
            _jsonSerializationService = jsonSerializationService;
            _fundingActorProvider = fundingActorProvider;
            _keyValuePersistenceService = keyValuePersistenceService;
            _fundingOutputCondenserService = fundingOutputCondenserService;
            _logger = logger;
            _actorName = actorName;
        }

        public async Task Execute(
            IEnumerable<FundingActorDto> fundingActorDtos,
            string outputKey,
            CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Starting {_actorName} Actors");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Task<string>> taskList = new List<Task<string>>();
            List<TActor> actors = new List<TActor>();

            foreach (FundingActorDto fundingActorDto in fundingActorDtos)
            {
                TActor actor = _fundingActorProvider.Provide();
                actors.Add(actor);
                taskList.Add(actor.Process(fundingActorDto, cancellationToken));
            }

            await Task.WhenAll(taskList).ConfigureAwait(false);

            IEnumerable<TActorReturn> results = taskList.Select(t => _jsonSerializationService.Deserialize<TActorReturn>(t.Result));

            _logger.LogDebug($"Completed {taskList.Count} {_actorName} Actors - {stopWatch.ElapsedMilliseconds}");

            List<Task> tasksDestroy = new List<Task>();
            foreach (TActor actor in actors)
            {
                tasksDestroy.Add(_fundingActorProvider.DestroyAsync(actor, cancellationToken));
            }

            await Task.WhenAll(tasksDestroy).ConfigureAwait(false);

            _logger.LogDebug($"Destroyed {taskList.Count} {_actorName} Actors - {stopWatch.ElapsedMilliseconds}");

            stopWatch.Restart();

            var output = _jsonSerializationService.Serialize(_fundingOutputCondenserService.Condense(results));
            _logger.LogDebug($"Persisting {_actorName} bytes:{output.Length}");

            await _keyValuePersistenceService.SaveAsync(outputKey, output, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug($"Persisted {_actorName} results - {stopWatch.ElapsedMilliseconds}");
        }
    }
}

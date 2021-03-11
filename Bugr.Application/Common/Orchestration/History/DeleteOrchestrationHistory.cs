using MediatR;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Threading;
using System.Threading.Tasks;

namespace Bugr.Application.Common.Orchestration.History
{
	public class DeleteOrchestrationHistory : IRequest<bool>
	{
		public IDurableOrchestrationClient DurableOrchestrationClient { get; set; }
	}

	public class DeleteOrchestrationHistoryHandler : IRequestHandler<DeleteOrchestrationHistory, bool>
	{

		public async Task<bool> Handle(DeleteOrchestrationHistory request, CancellationToken cancellationToken)
		{
			try
			{
				var client = request.DurableOrchestrationClient;
				var previousDurableOrchestrations = (await client
					.ListInstancesAsync(new OrchestrationStatusQueryCondition(), cancellationToken))
					.DurableOrchestrationState;

				foreach (var durableOrchestrationState in previousDurableOrchestrations)
				{
					await client.PurgeInstanceHistoryAsync(durableOrchestrationState.InstanceId);
					await client.TerminateAsync(durableOrchestrationState.InstanceId, $"{nameof(DeleteOrchestrationHistory)} mediator command has terminated the orchestration");
				}

				return true;

			}
			catch 
			{
				// Fail silently if durable function purging fails
				return false;
			}
		}
	}
}

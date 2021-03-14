using MediatR;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Threading;
using System.Threading.Tasks;

namespace Bugr.Application.Common.Orchestration.History
{
	public class TerminateDurableFunctionOrchestration : IRequest<bool>
	{
		public IDurableOrchestrationClient DurableOrchestrationClient { get; set; }
	}

	public class TerminateDurableFunctionOrchestrationHandler : IRequestHandler<TerminateDurableFunctionOrchestration, bool>
	{
		public async Task<bool> Handle(TerminateDurableFunctionOrchestration request, CancellationToken cancellationToken)
		{
			try
			{
				var client = request.DurableOrchestrationClient;
				var previousDurableOrchestrations = (await client
					.ListInstancesAsync(new OrchestrationStatusQueryCondition(), cancellationToken))
					.DurableOrchestrationState;

				foreach (var durableOrchestrationState in previousDurableOrchestrations)
				{
					await client.TerminateAsync(durableOrchestrationState.InstanceId, $"{nameof(TerminateDurableFunctionOrchestration)} mediator command has terminated the orchestration");
				}

				return true;

			}
			catch 
			{
				// Fail silently if durable function termination fails
				return false;
			}
		}
	}
}

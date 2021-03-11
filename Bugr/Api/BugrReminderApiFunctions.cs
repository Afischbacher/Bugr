using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MediatR;
using Bugr.Application.Messages.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Bugr.Application.Common.Twilio.Authentication.Services;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Bugr.Application.Messages.Models;
using System;
using Bugr.Application.Common.Twilio.Models;
using Bugr.Application.Common.Collections.Extensions;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using Bugr.Application.Common.Orchestration.Naming;

namespace Bugr.Api
{
	public class BugrReminderApiFunctions
	{
		private readonly IMediator _mediator;
		private readonly ITwilioAuthenticationWebHookService _twilioAuthenticationWebHookService;

		public BugrReminderApiFunctions
		(
			IMediator mediator,
			ITwilioAuthenticationWebHookService twilioAuthenticationWebHookService
		)
		{
			_mediator = mediator;
			_twilioAuthenticationWebHookService = twilioAuthenticationWebHookService;
		}

		[FunctionName(BugrReminderApiFunctionNames.ProcessSmsResponseWebHookHttpTriggerOrchestrator)]
		public async Task ProcessSmsResponseWebHookHttpTriggerOrchestrator([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "smsWebHook")] HttpRequest httpRequest, [DurableClient] IDurableOrchestrationClient client, ILogger log)
		{

			var bindingModel = new WebhookBindingModel
			{
				Body = await httpRequest.ReadAsStringAsync(),
				DisplayUrl = UriHelper.GetDisplayUrl(httpRequest),
				Form = httpRequest.Form.ToNameValueCollection().ToDictionary(),
				Headers = httpRequest.Headers.ToDictionary(k => k.Key, v => v.Value.ToString())
			};

			await client.StartNewAsync(BugrReminderApiFunctionNames.ProcessSmsResponseWebHookOrchestrationTrigger, bindingModel);

			log.LogInformation($"Begun {BugrReminderApiFunctionNames.ProcessSmsResponseWebHookOrchestrationTrigger}");

		}

		[FunctionName(BugrReminderApiFunctionNames.BugrReminderTimerTriggerFunctionOrchestrator)]
		public async Task BugrReminderTimerTriggerFunctionOrchestrator([TimerTrigger("0 30 13 * * Thu", RunOnStartup = true)] TimerInfo myTimer, [DurableClient] IDurableOrchestrationClient client, ILogger log)
		{
			

			await client.StartNewAsync(BugrReminderApiFunctionNames.BugrReminderOrchestrationTrigger);

			log.LogInformation($"Begun {BugrReminderApiFunctionNames.BugrReminderTimerTriggerFunctionOrchestrator}");
		}


		[FunctionName(BugrReminderApiFunctionNames.ProcessSmsResponseWebHookOrchestrationTrigger)]
		public async Task<bool> ProcessSmsResponseWebHookHttpTriggerOrchestration([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
		{
			var webHookBindingModel = context.GetInput<WebhookBindingModel>();

			await context.CallActivityAsync(BugrReminderApiFunctionNames.AuthenticateWebHookRequestActivityFunction, webHookBindingModel);

			await context.CallActivityWithRetryAsync(BugrReminderApiFunctionNames.ProcessSmsWebhookResponseActivityFunction, new RetryOptions(firstRetryInterval: TimeSpan.FromSeconds(5), maxNumberOfAttempts: 5), webHookBindingModel);

			log.LogInformation("Processed SMS response");

			return true;
		}

		[FunctionName(BugrReminderApiFunctionNames.BugrReminderOrchestrationTrigger)]
		public async Task BugrReminderOrchestrationTrigger([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
		{
			var bugrMessageViewModel = await context.CallActivityAsync<BugrMessageViewModel>(BugrReminderApiFunctionNames.GenerateBugrMessageActivityFunction, null);

			await context.CallActivityWithRetryAsync(BugrReminderApiFunctionNames.SendBugrMessageActivityFunction, new RetryOptions(firstRetryInterval: TimeSpan.FromSeconds(5), maxNumberOfAttempts: 5), bugrMessageViewModel);

			log.LogInformation($"Issued reminder SMS message");
		}

		[FunctionName(BugrReminderApiFunctionNames.GenerateBugrMessageActivityFunction)]
		public async Task<BugrMessageViewModel> GenerateBugrMessageActivityFunction([ActivityTrigger] IDurableActivityContext context)
		{
			return await _mediator.Send(new GenerateBugrMessage());
		}

		[FunctionName(BugrReminderApiFunctionNames.AuthenticateWebHookRequestActivityFunction)]
		public async Task AuthenticateWebHookRequestActivityFunction([ActivityTrigger] IDurableActivityContext context)
		{
			var bindingModel = context.GetInput<WebhookBindingModel>();
			await _twilioAuthenticationWebHookService.AuthenticateWebHookRequest(bindingModel);
		}

		[FunctionName(BugrReminderApiFunctionNames.SendBugrMessageActivityFunction)]
		public async Task SendBugrMessageActivityFunction([ActivityTrigger] IDurableActivityContext context)
		{
			var bugrMessageViewModel = context.GetInput<BugrMessageViewModel>();

			await _mediator.Send(new SendMessage
			{
				PhoneNumber = bugrMessageViewModel.PhoneNumber,
				Message = bugrMessageViewModel.Message
			});

		}

		[FunctionName(BugrReminderApiFunctionNames.ProcessSmsWebhookResponseActivityFunction)]
		public async Task ProcessSmsWebhookResponseActivityFunction([ActivityTrigger] IDurableActivityContext context)
		{
			var bindingModel = context.GetInput<WebhookBindingModel>();

			await _mediator.Send(new ProcessSmsWebhookResponse
			{
				BindingModel = bindingModel
			});
		}
	}
}

using Azure.Security.KeyVault.Secrets;
using Bugr.Application.Common.Collections.Extensions;
using Bugr.Application.Common.Secrets.Extension;
using Bugr.Application.Common.Twilio.Authentication.Services;
using Bugr.Application.Common.Twilio.Models;
using Bugr.Application.Messages.Models;
using Bugr.Application.Messages.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Bugr.Application.Messages.Commands
{
	public class ProcessSmsWebhookResponse : IRequest<bool>
	{
		public TwilioWebhookBindingModel BindingModel { get; set; }
	}

	public class ProcessSmsResponseWebHookHandler : IRequestHandler<ProcessSmsWebhookResponse, bool>
	{
		private readonly IMediator _mediator;
		private readonly ITwilioAuthenticationWebHookService _twilioAuthenticationWebHookService;
		private readonly SecretClient _secretClient;
		private readonly ISecretKeySettings _secretKeySettings;

		public ProcessSmsResponseWebHookHandler
		(
			IMediator mediator, 
			ITwilioAuthenticationWebHookService twilioAuthenticationWebHookService,
			SecretClient secretClient,
			ISecretKeySettings secretKeySettings
		)
		{
			_mediator = mediator;
			_twilioAuthenticationWebHookService = twilioAuthenticationWebHookService;
			_secretClient = secretClient;
			_secretKeySettings = secretKeySettings;
		}

		public async Task<bool> Handle(ProcessSmsWebhookResponse request, CancellationToken cancellationToken)
		{
			var httpRequest = request.BindingModel;

			var requestBody = HttpUtility.UrlDecode(httpRequest.Body);
			var queryString = HttpUtility.ParseQueryString(requestBody);

			var dictionaryValues = queryString.ToDictionary();
			var bindingModel = dictionaryValues.ToObject<SmsWebHookResponseBindingModel>();

			var recipientResponsePhoneNumber = await _secretClient.GetSecretValueAsync(_secretKeySettings.RecipientResponseMessagePhoneNumber);
			await _mediator.Send(new SendMessage
			{ 
				PhoneNumber = recipientResponsePhoneNumber,
				Message = $"Message Recieved from Auto Bugr Bot - {bindingModel.Body}"
			});

			return true;
		}
	}

}

using MediatR;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Azure.Security.KeyVault.Secrets;
using Azure;
using Bugr.Application.Common.Secrets.Extension;
using Bugr.Application.Messages.Settings;
using Bugr.Application.Common.Exceptions;
using System;

namespace Bugr.Application.Messages.Commands
{
	public class SendMessage : IRequest<bool>
	{
		[Phone]
		public string PhoneNumber { get; set; }

		[Required]
		public string Message { get; set; }

	}

	public class SendMessageHandler : IRequestHandler<SendMessage, bool>
	{
		private readonly SecretClient _client;
		private readonly ISecretKeySettings _secretKeySettings;

		public SendMessageHandler(SecretClient client, ISecretKeySettings secretKeySettings)
		{
			_client = client;
			_secretKeySettings = secretKeySettings;
		}

		public async Task<bool> Handle(SendMessage request, CancellationToken cancellationToken)
		{
			var toPhoneNumber = request.PhoneNumber;
			var messageBody = request.Message;

			await InitializeTwilioClientAsync();

			var fromPhoneNumber = await _client.GetSecretValueAsync(_secretKeySettings.FromPhoneNumber);

			if (fromPhoneNumber == null)
			{
				throw new RequestFailedException("Unable to get to source phone number");
			}

			await MessageResource.CreateAsync
			(
				from: fromPhoneNumber,
				to: toPhoneNumber,
				body: messageBody
			);

			return true;
		}

		private async Task InitializeTwilioClientAsync()
		{
			var accountId = await _client.GetSecretValueAsync(_secretKeySettings.TwilioAccountSid);
			var authToken = await _client.GetSecretValueAsync(_secretKeySettings.TwilioAuthToken);

			if (authToken == null || accountId == null)
			{
				throw new RequestFailedException("Unable to retrieve Azure KeyVault Secrets");
			}

			TwilioClient.Init(accountId, authToken);
		}
	}
}

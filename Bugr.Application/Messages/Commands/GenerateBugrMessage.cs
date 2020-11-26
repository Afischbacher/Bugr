using Azure;
using Azure.Security.KeyVault.Secrets;
using Bugr.Application.Common.Secrets.Extension;
using Bugr.Application.Messages.Models;
using Bugr.Application.Messages.Services;
using Bugr.Application.Messages.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bugr.Application.Messages.Commands
{

	public class GenerateBugrMessage : IRequest<BugrMessageViewModel>
	{

	}

	public class GenerateBugrMessageHandler : IRequestHandler<GenerateBugrMessage, BugrMessageViewModel>
	{
		private readonly SecretClient _secretClient;
		private readonly IMessageService _messageService;
		private readonly ISecretKeySettings _secretKeySettings;

		public GenerateBugrMessageHandler
		(
			SecretClient secretClient,
			IMessageService messageService,
			ISecretKeySettings secretKeySettings
 		)
		{
			_secretClient = secretClient;
			_messageService = messageService;
			_secretKeySettings = secretKeySettings;
		}


		public async Task<BugrMessageViewModel> Handle(GenerateBugrMessage request, CancellationToken cancellationToken)
		{
			var toNumber = await _secretClient.GetSecretValueAsync(_secretKeySettings.RecipientPhoneNumber);
			if (toNumber == null)
			{
				throw new RequestFailedException("Unable to get to destination phone number");
			}

			var message = await _messageService.GetRandomReminderMessageAsync();
			return new BugrMessageViewModel
			{
				PhoneNumber = toNumber,
				Message = message
			};
		}
	}
}

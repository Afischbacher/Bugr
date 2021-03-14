using Bugr.Application.Common.Twilio.Authentication.Services;
using Bugr.Application.Common.Twilio.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bugr.Application.Common.Twilio.Authentication.Mediator.Commands
{
	public class AuthenticateTwilioWebhookRequest : IRequest<bool>
	{
		public TwilioWebhookBindingModel WebhookBindingModel { get; set; }

		public class AuthenticateTwilioWebhookRequestHandler : IRequestHandler<AuthenticateTwilioWebhookRequest, bool>
		{
			private readonly ITwilioAuthenticationWebHookService _twilioAuthenticationWebHookService;

			public AuthenticateTwilioWebhookRequestHandler
			(
				ITwilioAuthenticationWebHookService twilioAuthenticationWebHookService
			)
			{
				_twilioAuthenticationWebHookService = twilioAuthenticationWebHookService;
			}

			public async Task<bool> Handle(AuthenticateTwilioWebhookRequest request, CancellationToken cancellationToken)
			{
				var bindingModel = request.WebhookBindingModel;

				return await _twilioAuthenticationWebHookService.AuthenticateWebHookRequest(bindingModel);
			}
		}
	}
}

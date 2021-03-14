using Twilio.Security;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using Bugr.Application.Common.Secrets.Extension;
using Bugr.Application.Messages.Settings;
using Bugr.Application.Common.Twilio.Authentication.Exceptions;
using Bugr.Application.Common.Twilio.Models;

namespace Bugr.Application.Common.Twilio.Authentication.Services
{
	public interface ITwilioAuthenticationWebHookService
	{
		Task<bool> AuthenticateWebHookRequest(TwilioWebhookBindingModel twilioAuthenticationBindingModel);
	}

	public class TwilioAuthenticationWebHookService : ITwilioAuthenticationWebHookService
	{
		private readonly SecretClient _secretClient;
		private readonly ISecretKeySettings _secretKeySettings;
		private RequestValidator _requestValidator;
		private const string _twilioHeader = "X-Twilio-Signature";

		public TwilioAuthenticationWebHookService(SecretClient secretClient, ISecretKeySettings secretKeySettings)
		{
			_secretClient = secretClient;
			_secretKeySettings = secretKeySettings;
		}

		public async Task<bool> AuthenticateWebHookRequest(TwilioWebhookBindingModel twilioAuthenticationBindingModel)
		{
			var twilioAuthToken = await _secretClient.GetSecretValueAsync(_secretKeySettings.TwilioAuthToken);
			_requestValidator = new RequestValidator(twilioAuthToken);

			var isNotValidRequest = !IsValidRequest(twilioAuthenticationBindingModel);

			if (isNotValidRequest) throw new SmsWebHookAuthenticationException("Unable to authenticate webhook request");

			return true;
		}

		private bool IsValidRequest(TwilioWebhookBindingModel twilioAuthenticationBindingModel)
		{
			var signature = twilioAuthenticationBindingModel.Headers[_twilioHeader];
			var form = twilioAuthenticationBindingModel.Form;

			var requestUri = twilioAuthenticationBindingModel.DisplayUrl;
			return _requestValidator.Validate(requestUri, form, signature);
		}
	}
}

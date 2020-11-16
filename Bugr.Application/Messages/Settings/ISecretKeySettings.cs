namespace Bugr.Application.Messages.Settings
{
	public interface ISecretKeySettings
	{
		string FromPhoneNumber { get; }
		string RecipientPhoneNumber { get; }
		string TwilioAccountSid { get; }
		string TwilioAuthToken { get; }
		string RecipientResponseMessagePhoneNumber { get; }
	}

	public class SecretKeySettings : ISecretKeySettings
	{
		public string FromPhoneNumber => "from-phone-number";

		public string RecipientPhoneNumber => "recipient-phone-number";

		public string TwilioAccountSid => "twilio-account-id";

		public string TwilioAuthToken => "twilio-auth-token";

		public string RecipientResponseMessagePhoneNumber => "recipient-response-message-phone-number";
	}
}

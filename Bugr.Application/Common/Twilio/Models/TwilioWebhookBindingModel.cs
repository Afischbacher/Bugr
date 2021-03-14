using System.Collections.Generic;

namespace Bugr.Application.Common.Twilio.Models
{
	public abstract class WebhookBindingModel
	{
		public string Body { get; set; }
		public Dictionary<string, string> Headers { get; set; }

	}

	public class TwilioWebhookBindingModel : WebhookBindingModel
	{
		public Dictionary<string,string> Form { get; set; }

		public string DisplayUrl { get; set; }

	}
}

using System.Collections.Generic;

namespace Bugr.Application.Common.Twilio.Models
{
	public class WebhookBindingModel 
	{
		public string Body { get; set; }

		public Dictionary<string,string> Form { get; set; }

		public string DisplayUrl { get; set; }

		public Dictionary<string, string> Headers { get; set; }
	}
}

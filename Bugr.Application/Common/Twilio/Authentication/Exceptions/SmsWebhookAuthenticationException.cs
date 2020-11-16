using System;
using System.Collections.Generic;
using System.Text;

namespace Bugr.Application.Common.Twilio.Authentication.Exceptions
{
	public class SmsWebHookAuthenticationException : Exception
	{
		public SmsWebHookAuthenticationException(string message) : base(message)
		{

		}
	}
}

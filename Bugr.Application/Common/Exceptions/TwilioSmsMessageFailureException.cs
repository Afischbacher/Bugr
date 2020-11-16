using System;
using System.Collections.Generic;
using System.Text;

namespace Bugr.Application.Common.Exceptions
{
	public class TwilioSmsMessageFailureException : Exception
	{
		public TwilioSmsMessageFailureException(string message) : base(message)
		{

		}
	}
}

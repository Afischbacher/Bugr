using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Bugr.Application.Common.Twilio.Authentication.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
	{
		public void OnResourceExecuting(ResourceExecutingContext context)
		{
			var factories = context.ValueProviderFactories;
			factories.RemoveType<FormValueProviderFactory>();
			factories.RemoveType<JQueryFormValueProviderFactory>();
		}

		public void OnResourceExecuted(ResourceExecutedContext context)
		{
		}
	}
}
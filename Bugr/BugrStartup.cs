using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using Bugr.Application.Common.Mediatr;
using Microsoft.Extensions.DependencyInjection;
using Bugr.Application.Messages.Services;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using System;
using Bugr.Application.Messages.Settings;
using Bugr.Application.Common.Twilio.Authentication.Services;
using Bugr.Application.Common.Startup.Extensions;

[assembly: FunctionsStartup(typeof(Bugr.BugrStartup))]
namespace Bugr
{
	public class BugrStartup : FunctionsStartup
	{
		private static string AzureKeyVaultEndPoint => "https://bugr-keyvault.vault.azure.net/";
		public override void Configure(IFunctionsHostBuilder builder)
		{

			// Service Dependency Injection Configuration
			builder.Services.AddMediatR(Assembly.GetAssembly(typeof(IMediatrRegistration)));
			builder.Services.AddSingleton(new SecretClient(new Uri(AzureKeyVaultEndPoint), new DefaultAzureCredential()));
			
			// Services and Settings
			builder.Services.AddTransient<ISecretKeySettings, SecretKeySettings>();
			builder.Services.AddTransient<ITwilioAuthenticationWebHookService, TwilioAuthenticationWebHookService>();
			builder.Services.AddTransient<IMessageService, MessageService>();

			// Configure Azure Table Storage
			builder.Services.AddCloudTableStorage();


		}
	}
}

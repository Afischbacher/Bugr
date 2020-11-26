using Azure.Security.KeyVault.Secrets;
using Bugr.Application.Common.Persistance.Storage.Table.Exceptions;
using Bugr.Application.Common.Persistance.Storage.Table.Services;
using Bugr.Application.Common.Secrets.Extension;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bugr.Application.Common.Startup.Extensions
{
	public static class CloudTableStorageExtensions
	{
		/// <summary>
		/// Adds the cloud table storage connection with a Singleton lifetime within IServiceCollection
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddCloudTableStorage(this IServiceCollection services)
		{

			static ICloudTableStorageService CloudTableStorage(IServiceProvider serviceProvider)
			{
				var secretClient = serviceProvider.GetService<SecretClient>();

				var cloudTableConnectionString = secretClient.GetSecretValue("cloud-table-storage-connection-string");

				var canParseCloudStorageAccount = CloudStorageAccount.TryParse(cloudTableConnectionString, out var cloudStorageAccount);

				if (!canParseCloudStorageAccount)
				{
					throw new CloudStorageAccountParseException();
				}

				var tableClient = cloudStorageAccount.CreateCloudTableClient();

				return new CloudTableStorageService(tableClient);
			}

			services.AddSingleton(CloudTableStorage);

			return services;
		}
	}
}

using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bugr.Application.Common.Secrets.Extension
{
	public static class SecretClientExtensions
	{
		public static async Task<string> GetSecretValueAsync(this SecretClient secretClient, string key)
		{
			return (await secretClient.GetSecretAsync(key))?.Value?.Value ?? null;
		}

		public static string GetSecretValue(this SecretClient secretClient, string key)
		{
			return secretClient.GetSecret(key)?.Value?.Value ?? null;
		}
	}
}

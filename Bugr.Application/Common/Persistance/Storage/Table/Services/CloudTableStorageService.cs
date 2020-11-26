using Bugr.Application.Common.Persistance.Storage.Table.Exceptions;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bugr.Application.Common.Persistance.Storage.Table.Services
{

	public interface ICloudTableStorageService
	{
		Task<IEnumerable<T>> QueryTableAsync<T>(string tableName, TableQuery<T> tableQuery) where T : TableEntity, new();
		Task InsertOrMergeAsync<T>(string tableName, T entity) where T : TableEntity, new();

	}

	public class CloudTableStorageService : ICloudTableStorageService
	{
		private readonly CloudTableClient _cloudTableClient;

		public CloudTableStorageService
		(
			CloudTableClient cloudTableClient
		)
		{
			_cloudTableClient = cloudTableClient;
			_cloudTableClient.TableClientConfiguration.UseRestExecutorForCosmosEndpoint = true;

		}

		public async Task InsertOrMergeAsync<T>(string tableName, T entity) where T : TableEntity, new()
		{
			if (entity == null)
			{
				throw new CloudTableEntityNullException();
			}

			var cloudTable = _cloudTableClient.GetTableReference(tableName);
			var tableExists = await cloudTable.ExistsAsync();

			if (!tableExists)
			{
				throw new CloudTableNotExistsException();
			}

			var upsertOperation = TableOperation.InsertOrMerge(entity);
			await cloudTable.ExecuteAsync(upsertOperation);

		}

		public async Task<IEnumerable<T>> QueryTableAsync<T>(string tableName, TableQuery<T> tableQuery) where T : TableEntity, new()
		{
			var cloudTable = _cloudTableClient.GetTableReference(tableName);
			var tableExists = await cloudTable.ExistsAsync();
			if (!tableExists)
			{
				throw new CloudTableNotExistsException();
			}

			return cloudTable.ExecuteQuery(tableQuery);

		}
	}
}

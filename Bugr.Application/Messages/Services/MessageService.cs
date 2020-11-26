using Bugr.Application.Common.Collections.Extensions;
using Bugr.Application.Common.Persistance.Storage.Table.Models;
using Bugr.Application.Common.Persistance.Storage.Table.Services;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugr.Application.Messages.Services
{
	public interface IMessageService
	{
		Task<string> GetRandomReminderMessageAsync();
	}

	public class MessageService : IMessageService
	{
		private readonly ICloudTableStorageService _cloudTableStorageService;

		public MessageService
		(
			ICloudTableStorageService cloudTableStorageService
		)
		{
			_cloudTableStorageService = cloudTableStorageService;
		}

		public async Task<string> GetRandomReminderMessageAsync()
		{
			var reminderMessages = await _cloudTableStorageService.QueryTableAsync("bugrReminderMessages", new TableQuery<BugrReminderMessage>());

			return reminderMessages.GetRandomValue().RowKey;
		}
	}
}

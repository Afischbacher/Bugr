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
		Task<string> GetRandomReminderMessage();
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

		public async Task<string> GetRandomReminderMessage()
		{
			var reminderMessages = await _cloudTableStorageService.QueryTable("bugrReminderMessages", new TableQuery<BugrReminderMessage>());

			return reminderMessages.GetRandomValue().RowKey;
		}
	}
}

using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bugr.Application.Common.Persistance.Storage.Table.Models
{
	public class BugrReminderMessage : TableEntity
	{
		public BugrReminderMessage()
		{

		}

		public BugrReminderMessage(string message)
		{
			PartitionKey = Guid.NewGuid().ToString();
			RowKey = message;
		}

	}
}

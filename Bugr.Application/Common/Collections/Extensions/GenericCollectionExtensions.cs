using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bugr.Application.Common.Collections.Extensions
{
	public static class GenericCollectionExtensions
	{ 
		public static T GetRandomValue<T>(this IEnumerable<T> collection)
		{
			var randomIndex = new Random().Next(minValue: 0, maxValue: collection.Count() - 1);

			return collection.ElementAt(randomIndex);
		}
	}
}

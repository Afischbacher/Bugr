using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bugr.Application.Common.Collections.Extensions
{
	public static class GenericCollectionExtensions
	{ 
		/// <summary>
		/// Selects a returns a random value from a generic collection
		/// </summary>
		public static T GetRandomValue<T>(this IEnumerable<T> collection)
		{
			return collection.ElementAt(new Random().Next(minValue: 0, maxValue: collection.Count() - 1));
		}
	}
}

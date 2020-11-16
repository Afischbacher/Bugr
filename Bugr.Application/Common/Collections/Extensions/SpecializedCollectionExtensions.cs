using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Bugr.Application.Common.Collections.Extensions
{
	public static class SpecializedCollectionExtensions
	{
		public static Dictionary<string, string> ToDictionary(this NameValueCollection nameValueCollection)
		{
			var dictionary = new Dictionary<string, string>();

			foreach (var key in nameValueCollection.Keys)
			{
				dictionary.Add(key.ToString(), nameValueCollection.Get(key.ToString()));
			}

			return dictionary;
		}

		public static NameValueCollection ToNameValueCollection(this IFormCollection formCollection)
		{
			var nameValueCollection = new NameValueCollection();

			foreach (var key in formCollection.Keys)
			{
				nameValueCollection.Add(key, formCollection[key]);
			}

			return nameValueCollection;
		}

		public static T ToObject<T>(this Dictionary<string, string> source)
		where T : class, new()
		{
			var someObject = new T();
			var someObjectType = someObject.GetType();

			foreach (var item in source)
			{
				someObjectType
				.GetProperty(item.Key)
				.SetValue(someObject, item.Value, null);
			}

			return someObject;
		}
	}
}

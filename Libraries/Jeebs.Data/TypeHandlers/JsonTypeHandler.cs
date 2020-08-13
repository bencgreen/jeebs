﻿using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Newtonsoft.Json;

namespace Jeebs.Data.TypeHandlers
{
	/// <summary>
	/// JSON TypeHandler
	/// </summary>
	/// <typeparam name="T">Type to serialise from / deserialise to</typeparam>
	public class JsonTypeHandler<T> : SqlMapper.StringTypeHandler<T>
	{
		/// <summary>
		/// Serialise object to JSON
		/// </summary>
		/// <param name="value">T value</param>
		/// <returns>JSON</returns>
		protected override string Format(T value)
			=> Util.Json.Serialise(value);

		/// <summary>
		/// Deserialise JSON string
		/// </summary>
		/// <param name="json">JSON string</param>
		protected override T Parse(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				throw new ArgumentNullException(nameof(json));
			}

			return Util.Json.Deserialise<T>(json).Unwrap(() => throw new JsonException("Unable to deserialise JSON."));
		}
	}
}

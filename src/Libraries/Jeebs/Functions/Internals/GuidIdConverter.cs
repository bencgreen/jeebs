﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Text.Json;
using Jeebs.Id;

namespace F.Internals
{
	/// <summary>
	/// Converter for <see cref="GuidId"/> types
	/// </summary>
	/// <typeparam name="T">GuidId type</typeparam>
	internal class GuidIdConverter<T> : StrongIdConverter<GuidId, Guid>
		where T : GuidId, new()
	{
		/// <summary>
		/// Create object
		/// </summary>
		public GuidIdConverter() : base(Guid.Empty) { }

		/// <summary>
		/// Read an GuidId type value
		/// </summary>
		/// <param name="reader">Utf8JsonReader</param>
		/// <param name="typeToConvert">GuidId type</param>
		/// <param name="options">JsonSerializerOptions</param>
		public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
			new()
			{
				Value = reader.TokenType switch
				{
					// Handle strings
					JsonTokenType.String =>
						HandleString(ref reader, s => !string.IsNullOrEmpty(s), Guid.TryParse),

					// Handle default
					_ =>
						HandleDefault(ref reader)

				}
			};
	}
}
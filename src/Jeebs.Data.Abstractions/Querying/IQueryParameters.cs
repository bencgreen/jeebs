﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System.Collections.Generic;

namespace Jeebs.Data.Querying
{
	/// <summary>
	/// Shorthand to make working with query parameters easier
	/// </summary>
	public interface IQueryParameters : IDictionary<string, object>
	{
		/// <summary>
		/// Merge another <see cref="IQueryParameters"/> into this one
		/// </summary>
		/// <param name="parameters">Parameters to merge</param>
		bool Merge(IQueryParameters parameters);

		/// <summary>
		/// Add an anonymous object of parameters to the dictionary
		/// Properties must be simple key/value pairs
		/// </summary>
		/// <param name="parameters">Parameters to add</param>
		bool TryAdd(object? parameters);
	}
}

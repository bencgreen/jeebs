﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

namespace Jeebs.WordPress.Data.Querying
{
	/// <summary>
	/// Saves query parts (stage 3) and enables stage 4: get query object
	/// </summary>
	/// <typeparam name="T">Model Type</typeparam>
	public interface IQueryWithParts<T>
	{
		/// <summary>
		/// Query Stage 4: Get query object
		/// </summary>
		IQuery<T> GetQuery();
	}
}

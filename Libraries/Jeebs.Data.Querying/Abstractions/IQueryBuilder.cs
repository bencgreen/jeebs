﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jeebs.Data
{
	/// <summary>
	/// Builds a Query in a fluent manner
	/// </summary>
	public interface IQueryBuilder
	{
		/// <summary>
		/// Query Stage 1: Set the model for this query
		/// </summary>
		/// <typeparam name="T">Model type</typeparam>
		IQueryWithModel<T> WithModel<T>();
	}
}
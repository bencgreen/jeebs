﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Data;

namespace Jeebs.Data.TypeHandlers
{
	/// <summary>
	/// Enumerated TypeHandler
	/// </summary>
	public abstract class EnumeratedTypeHandler<T> : Dapper.SqlMapper.TypeHandler<T>
		where T : Enumerated
	{
		/// <summary>
		/// Parse the Enumerated value
		/// </summary>
		/// <param name="value">Database table value</param>
		protected T Parse(object value, Func<string, T> parse, T ifNull) =>
			value?.ToString() switch
			{
				string taxonomy =>
					parse(taxonomy),

				_ =>
					ifNull
			};

		/// <summary>
		/// Set the Enumerated table value
		/// </summary>
		/// <param name="parameter">IDbDataParameter object</param>
		/// <param name="value">Enumerated value</param>
		public override void SetValue(IDbDataParameter parameter, T value) =>
			parameter.Value = value.ToString();
	}
}
﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Jeebs;

namespace F.DataF
{
	public static partial class QueryF
	{
		/// <summary>
		/// Get a parameter value - if it's a <see cref="StrongId"/>, return <see cref="StrongId.Value"/>
		/// </summary>
		/// <param name="value">Parameter Value</param>
		public static object GetParameterValue(object value) =>
			value switch
			{
				StrongId id =>
					id.Value,

				{ } x =>
					x
			};
	}
}

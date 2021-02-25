﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jeebs
{
	/// <summary>
	/// Strongly Typed ID record type
	/// </summary>
	/// <typeparam name="T">ID Value Type</typeparam>
	/// <param name="Value">ID Value</param>
	public abstract record StrongId<T>(T Value) : IStrongId<T>
	{
		/// <inheritdoc cref="IStrongId.ValueStr"/>
		public string ValueStr =>
			Value?.ToString() ?? "Unknown ID";

		/// <inheritdoc cref="IStrongId.IsDefault"/>
		public abstract bool IsDefault { get; }
	}
}
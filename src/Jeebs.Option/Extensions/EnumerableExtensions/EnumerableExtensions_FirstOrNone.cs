﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Collections.Generic;

namespace Jeebs.Linq
{
	/// <summary>
	/// Enumerable Extensions: FirstOrNone
	/// </summary>
	public static class EnumerableExtensions_FirstOrNone
	{
		/// <inheritdoc cref="F.OptionF.Enumerable.FirstOrNone{T}(IEnumerable{T}, Func{T, bool}?)"/>
		public static Option<T> FirstOrNone<T>(this IEnumerable<T> @this) =>
			F.OptionF.Enumerable.FirstOrNone(@this, null);

		/// <inheritdoc cref="F.OptionF.Enumerable.FirstOrNone{T}(IEnumerable{T}, Func{T, bool}?)"/>
		public static Option<T> FirstOrNone<T>(this IEnumerable<T> @this, Func<T, bool> predicate) =>
			F.OptionF.Enumerable.FirstOrNone(@this, predicate);
	}
}
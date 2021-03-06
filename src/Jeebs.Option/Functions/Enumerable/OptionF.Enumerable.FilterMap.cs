﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using System.Collections.Generic;
using Jeebs;

namespace F
{
	public static partial class OptionF
	{
		public static partial class Enumerable
		{
			/// <summary>
			/// Filter elements to return only <see cref="Some{T}"/> and transform using <paramref name="map"/>
			/// </summary>
			/// <typeparam name="T">Option value type</typeparam>
			/// <typeparam name="U">Next value type</typeparam>
			/// <param name="list">Option list</param>
			/// <param name="map">Mapping function</param>
			/// <param name="predicate">[Optional] Predicate to use with filter</param>
			public static IEnumerable<U> FilterMap<T, U>(IEnumerable<Option<T>> list, Func<T, U> map, Func<T, bool>? predicate)
			{
				foreach (var some in Filter(list, predicate))
				{
					yield return map(some);
				}
			}
		}
	}
}

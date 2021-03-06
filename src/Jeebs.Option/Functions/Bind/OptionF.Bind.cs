﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using Jeebs;

namespace F
{
	public static partial class OptionF
	{
		/// <summary>
		/// Use <paramref name="bind"/> to convert the value of <paramref name="option"/> to type <typeparamref name="U"/>,
		/// if it is a <see cref="Some{T}"/>
		/// </summary>
		/// <typeparam name="T">Option value type</typeparam>
		/// <typeparam name="U">Next value type</typeparam>
		/// <param name="option">Input option</param>
		/// <param name="bind">Binding function - will receive <see cref="Some{T}.Value"/> if this is a <see cref="Some{T}"/></param>
		public static Option<U> Bind<T, U>(Option<T> option, Func<T, Option<U>> bind) =>
			Catch(() =>
				Switch(
					option,
					some: v => bind(v),
					none: r => new None<U>(r)
				),
				DefaultHandler
			);
	}
}

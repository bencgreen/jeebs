﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Threading.Tasks;

namespace Jeebs
{
	public abstract partial record Option<T>
	{
		/// <summary>
		/// Use <paramref name="bind"/> to convert the current Option to a new type - if this is a <see cref="Some{T}"/>
		/// </summary>
		/// <typeparam name="U">Next value type</typeparam>
		/// <param name="bind">Binding function - will receive <see cref="Some{T}.Value"/> if this is a <see cref="Some{T}"/></param>
		/// <param name="handler">[Optional] Exception handler</param>
		private Task<Option<U>> BindAsyncPrivate<U>(Func<T, Task<Option<U>>> bind, Option.Handler? handler = null) =>
			Option.CatchAsync(() =>
				Switch(
					some: x => bind(x),
					none: async x => (Option<U>)Option.None<U>(x)
				),
				handler
			);

		/// <inheritdoc cref="BindAsyncPrivate{U}(Func{T, Task{Option{U}}}, Option.Handler?)"/>
		public Task<Option<U>> BindAsync<U>(Func<T, Task<Option<U>>> bind, Option.Handler? handler = null) =>
			BindAsyncPrivate(bind, handler);
	}
}

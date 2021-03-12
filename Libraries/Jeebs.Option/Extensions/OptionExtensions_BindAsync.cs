﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Threading.Tasks;

namespace JeebsF
{
	/// <summary>
	/// <see cref="Option{T}"/> Extensions: BindAsync
	/// </summary>
	public static partial class OptionExtensions
	{
		/// <inheritdoc cref="Option{T}.DoBindAsync{U}(Func{T, Task{Option{U}}}, OptionF.Handler?)"/>
		internal static Task<Option<U>> DoBindAsync<T, U>(
			Task<Option<T>> @this,
			Func<T, Task<Option<U>>> bind,
			OptionF.Handler? handler = null
		) =>
			OptionF.CatchAsync(async () =>
				await @this switch
				{
					Some<T> some =>
						await bind(some.Value),

					None<T> none =>
						new None<U>(none.Reason),

					_ =>
						throw new Exceptions.UnknownOptionException() // as Option<T> is internal implementation only this should never happen...
				},
				handler
			);

		/// <inheritdoc cref="Option{T}.DoBindAsync{U}(Func{T, Task{Option{U}}}, OptionF.Handler?)"/>
		public static Task<Option<U>> BindAsync<T, U>(
			this Task<Option<T>> @this,
			Func<T, Option<U>> bind,
			OptionF.Handler? handler = null
		) =>
			DoBindAsync(@this, x => Task.FromResult(bind(x)), handler);

		/// <inheritdoc cref="Option{T}.DoBindAsync{U}(Func{T, Task{Option{U}}}, OptionF.Handler?)"/>
		public static Task<Option<U>> BindAsync<T, U>(
			this Task<Option<T>> @this,
			Func<T, Task<Option<U>>> bind,
			OptionF.Handler? handler = null
		) =>
			DoBindAsync(@this, bind, handler);
	}
}

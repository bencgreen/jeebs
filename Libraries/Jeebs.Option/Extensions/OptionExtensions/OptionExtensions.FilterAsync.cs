﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Threading.Tasks;
using static F.OptionF;

namespace Jeebs
{
	/// <summary>
	/// <see cref="Option{T}"/> Extensions: FilterAsync
	/// </summary>
	public static class OptionExtensions_FilterAsync
	{
		/// <inheritdoc cref="Option{T}.DoFilterAsync(Func{T, Task{bool}}, Handler?)"/>
		internal static async Task<Option<T>> DoFilterAsync<T>(Task<Option<T>> @this, Func<T, Task<bool>> predicate, Handler? handler) =>
			await (await @this).DoFilterAsync(predicate, handler);

		/// <inheritdoc cref="DoBindAsync{T, U}(Task{Option{T}}, Func{T, Task{Option{U}}}, Handler?)"/>
		public static Task<Option<T>> FilterAsync<T>(this Task<Option<T>> @this, Func<T, bool> predicate, Handler? handler = null) =>
			DoFilterAsync(@this, x => Task.FromResult(predicate(x)), handler);

		/// <inheritdoc cref="DoBindAsync{T, U}(Task{Option{T}}, Func{T, Task{Option{U}}}, Handler?)"/>
		public static Task<Option<T>> FilterAsync<T>(this Task<Option<T>> @this, Func<T, Task<bool>> predicate, Handler? handler = null) =>
			DoFilterAsync(@this, predicate, handler);
	}
}

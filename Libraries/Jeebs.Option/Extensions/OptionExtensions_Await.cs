﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System.Threading.Tasks;

namespace Jeebs
{
	/// <summary>
	/// <see cref="Option{T}"/> Extensions: Await
	/// </summary>
	public static partial class OptionExtensions
	{
		/// <summary>
		/// Forces the thread to await the current Option value
		/// </summary>
		/// <remarks>Use with EXTREME CAUTION - can cause locks - you should normall use async/await keywords</remarks>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="this">Option (awaitable)</param>
		public static Option<T> Await<T>(this Task<Option<T>> @this) =>
			@this.GetAwaiter().GetResult();
	}
}
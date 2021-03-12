﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

namespace Jeebs
{
	/// <summary>
	/// 'Some' option - wraps value to enable safe non-null returns (see <seealso cref="None{T}"/>)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class Some<T> : Option<T>
	{
		/// <summary>
		/// Option Value - nullability will match the nullability of <typeparamref name="T"/>
		/// </summary>
		public T Value { get; private init; }

		internal Some(T value) =>
			Value = value;

		/// <summary>
		/// Return:
		///    Value (if Value is not null)
		/// </summary>
		public override string ToString() =>
			base.ToString();
	}
}

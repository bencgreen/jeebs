﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;

namespace Jeebs
{
	public static partial class Option
	{
		/// <summary>
		/// Create a <see cref="Some{T}"/> Option, containing a value
		/// <para>If <paramref name="value"/> is null, <see cref="Jeebs.None{T}"/> will be returned instead</para>
		/// </summary>
		/// <typeparam name="T">Option value type</typeparam>
		/// <param name="value">Some value</param>
		/// <param name="allowNull">If true, <see cref="Some{T}"/> will always be returned</param>
		public static Option<T> Wrap<T>(T value, bool allowNull = false) =>
			value switch
			{
				T x =>
					new Some<T>(x),

				_ =>
					allowNull switch
					{
						true =>
							new Some<T>(value),

						false =>
							None<T>(new Jm.Option.SomeValueWasNullMsg())
					}

			};

		/// <summary>
		/// Create <see cref="Some{T}"/> with <paramref name="value"/> if <paramref name="predicate"/> is true
		/// <para>Otherwise, will return <see cref="Jeebs.None{T}"/></para>
		/// </summary>
		/// <typeparam name="T">Option value type</typeparam>
		/// <param name="predicate">Predicate to evaluate</param>
		/// <param name="value">Function to return value</param>
		public static Option<T> WrapIf<T>(Func<bool> predicate, Func<T> value) =>
			predicate() switch
			{
				true =>
					Wrap(value()),

				false =>
					None<T>(new Jm.Option.PredicateWasFalseMsg())
			};

		/// <inheritdoc cref="WrapIf{T}(Func{bool}, Func{T})"/>
		public static Option<T> WrapIf<T>(Func<bool> predicate, T value) =>
			WrapIf(predicate, () => value);
	}
}

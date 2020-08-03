﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jeebs
{
	/// <summary>
	/// Chain Link interface - with value
	/// </summary>
	/// <typeparam name="TValue">Result value type</typeparam>
	public interface ILink<TValue> : IDisposable
	{
		#region Map

		/// <summary>
		/// Map to a new result with a new value type
		/// <para>Any exceptions will be caught and added to <see cref="IR.Messages"/> as a <see cref="Jm.ChainExceptionMsg"/> - and an <see cref="IError{TValue}"/> will be returned</para>
		/// </summary>
		/// <typeparam name="TNext">Next result type</typeparam>
		/// <param name="f">Function which receives the current result (if it's an <see cref="IOk{TValue}"/>) and returns the next result</param>
		IR<TNext> Map<TNext>(Func<IOk<TValue>, IR<TNext>> f);

		/// <inheritdoc cref="Map{TNext}(Func{IOk{TValue}, IR{TNext}})"/>
		Task<IR<TNext>> MapAsync<TNext>(Func<IOk<TValue>, Task<IR<TNext>>> f);

		/// <summary>
		/// Map to a new result with a new value type
		/// <para>Any exceptions will be caught and added to <see cref="IR.Messages"/> as a <see cref="Jm.ChainExceptionMsg"/> - and an <see cref="IError{TValue}"/> will be returned</para>
		/// </summary>
		/// <typeparam name="TNext">Next result type</typeparam>
		/// <param name="f">Function which receives the current result (if it's an <see cref="IOkV{TValue}"/>) and returns the next result</param>
		IR<TNext> Map<TNext>(Func<IOkV<TValue>, IR<TNext>> f);

		/// <inheritdoc cref="Map{TNext}(Func{IOkV{TValue}, IR{TNext}})"/>
		Task<IR<TNext>> MapAsync<TNext>(Func<IOkV<TValue>, Task<IR<TNext>>> f);

		#endregion

		#region Run

		/// <summary>
		/// Run an action and return <see cref="IOk"/>
		/// <para>Any exceptions will be caught and added to <see cref="IR.Messages"/> as a <see cref="Jm.ChainExceptionMsg"/> - and an <see cref="IError"/> will be returned</para>
		/// </summary>
		/// <param name="f">Action to run</param>
		IR<TValue> Run(Action f);

		/// <inheritdoc cref="ILink.RunAsync(Func{Task})"/>
		Task<IR<TValue>> RunAsync(Func<Task> f);

		/// <summary>
		/// Run an action and return <see cref="IOk"/>
		/// <para>The action will receive the current result as an input - if it's an <see cref="IOk"/></para>
		/// <para>Any exceptions will be caught and added to <see cref="IR.Messages"/> as a <see cref="Jm.ChainExceptionMsg"/> - and an <see cref="IError"/> will be returned</para>
		/// </summary>
		/// <param name="f">Action which receives the current result (if it's an <see cref="IOk"/>)</param>
		IR<TValue> Run(Action<IOk> f);

		/// <inheritdoc cref="ILink.RunAsync(Func{IOk, Task})"/>
		Task<IR<TValue>> RunAsync(Func<IOk, Task> f);

		/// <summary>
		/// Run an action and return <see cref="IR{TValue}"/>
		/// <para>The action will receive the current result as an input - if it's an <see cref="IOk{TValue}"/></para>
		/// <para>Any exceptions will be caught and added to <see cref="IR.Messages"/> as a <see cref="Jm.ChainExceptionMsg"/> - and an <see cref="IError{TValue}"/> will be returned</para>
		/// </summary>
		/// <param name="f">Action which receives the current result (if it's an <see cref="IOk{TValue}"/>)</param>
		IR<TValue> Run(Action<IOk<TValue>> f);

		/// <inheritdoc cref="Run(Action{IOk{TValue}})"/>
		Task<IR<TValue>> RunAsync(Func<IOk<TValue>, Task> f);

		/// <summary>
		/// Run an action and return <see cref="IR{TValue}"/>
		/// <para>The action will receive the current result as an input - if it's an <see cref="IOkV{TValue}"/></para>
		/// <para>Any exceptions will be caught and added to <see cref="IR.Messages"/> as a <see cref="Jm.ChainExceptionMsg"/> - and an <see cref="IError{TValue}"/> will be returned</para>
		/// </summary>
		/// <param name="f">Action which receives the current result (if it's an <see cref="IOkV{TValue}"/>)</param>
		IR<TValue> Run(Action<IOkV<TValue>> f);

		/// <inheritdoc cref="Run(Action{IOkV{TValue}})"/>
		Task<IR<TValue>> RunAsync(Func<IOkV<TValue>, Task> f);

		#endregion

		#region Wrap

		/// <summary>
		/// Wrap a value in an <see cref="IOkV{TValue}"/> object
		/// </summary>
		/// <param name="value">Value to wrap</param>
		IR<TNext> Wrap<TNext>(TNext value);

		/// <summary>
		/// Wrap a value in an <see cref="IOkV{TValue}"/> object
		/// </summary>
		/// <param name="f">Function to return the value to wrap</param>
		IR<TNext> Wrap<TNext>(Func<TNext> f);

		/// <inheritdoc cref="Wrap(Func{TValue})"/>
		Task<IR<TNext>> WrapAsync<TNext>(Func<Task<TNext>> f);

		#endregion
	}
}
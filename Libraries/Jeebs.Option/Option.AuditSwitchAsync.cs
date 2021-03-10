﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Threading.Tasks;

namespace Jeebs
{
	public abstract partial record Option<T>
	{
		/// <summary>
		/// Audit the current Option state and return unmodified
		/// Errors will not be returned as they affect the state of the object, but will be written to the console
		/// </summary>
		/// <param name="some">[Optional] Action to run if the current Option is <see cref="Some{T}"/></param>
		/// <param name="none">[Optional] Action to run if the current Option is <see cref="None{T}"/></param>
		private Task<Option<T>> AuditSwitchAsyncPrivate(Func<T, Task>? some = null, Func<IMsg?, Task>? none = null)
		{
			// Do nothing if the user gave us nothing to do!
			if (some == null && none == null)
			{
				return Task.FromResult(this);
			}

			// Work out which audit function to use
			Func<Task> audit = Switch<Func<Task>>(
				some: v => () => some?.Invoke(v) ?? Task.CompletedTask,
				none: r => () => none?.Invoke(r) ?? Task.CompletedTask
			);

			// Perform the audit
			try
			{
				audit();
			}
			catch (Exception e)
			{
				Console.WriteLine("Audit Error: {0}", e);
			}

			// Return the original object
			return Task.FromResult(this);
		}

		/// <inheritdoc cref="AuditSwitchAsyncPrivate(Func{T, Task}?, Func{IMsg?, Task}?)"/>
		public Task<Option<T>> AuditSwitchAsync(Func<T, Task>? some = null, Action<IMsg?>? none = null) =>
			AuditSwitchAsyncPrivate(
				some: some,
				none: r => { none?.Invoke(r); return Task.CompletedTask; }
			);

		/// <inheritdoc cref="AuditSwitchAsyncPrivate(Func{T, Task}?, Func{IMsg?, Task}?)"/>
		public Task<Option<T>> AuditSwitchAsync(Action<T>? some = null, Func<IMsg?, Task>? none = null) =>
			AuditSwitchAsyncPrivate(
				some: v => { some?.Invoke(v); return Task.CompletedTask; },
				none: none
			);

		/// <inheritdoc cref="AuditSwitchAsyncPrivate(Func{T, Task}?, Func{IMsg?, Task}?)"/>
		public Task<Option<T>> AuditSwitchAsync(Func<T, Task>? some = null, Func<IMsg?, Task>? none = null) =>
			AuditSwitchAsyncPrivate(
				some: some,
				none: none
			);
	}
}

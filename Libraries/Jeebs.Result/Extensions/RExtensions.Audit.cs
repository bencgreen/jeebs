﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jeebs
{
	public static partial class RExtensions
	{
		#region Start Async

		/// <summary>
		/// Audit the current result state asynchronously and return unmodified
		/// <para>Any exceptions will be caught and passed down the pipeline as a <see cref="Jm.AuditAsyncException"/> message</para>
		/// </summary>
		/// <param name="this">Current result task (will be awaited before executing <paramref name="audit"/>)</param>
		/// <param name="audit">Audit action</param>
		public static async Task<R<T>> AuditAsync<T>(this Task<R<T>> @this, Func<R<T>, Task> audit)
		{
			var result = await @this.ConfigureAwait(false);

			try
			{
				await audit(result).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				result.Messages.Add(new Jm.AuditAsyncException(ex));
			}

			return result;
		}

		/// <summary>
		/// Audit the current result state asynchronously and return unmodified
		/// <para>Any exceptions will be caught and passed down the pipeline as a <see cref="Jm.AuditAsyncException"/> message</para>
		/// </summary>
		/// <param name="this">Current result task (will be awaited before executing appropriate action)</param>
		/// <param name="isOk">[Optional] Action to run if the current result is <see cref="Ok{T}"/></param>
		/// <param name="isOkV">[Optional] Action to run if the current result is <see cref="OkV{T}"/></param>
		/// <param name="isError">[Optional] Action to run if the current result is <see cref="Error{T}"/></param>
		/// <param name="isUnknown">[Optional] Action to run if the current result is unknown</param>
		public static async Task<R<T>> AuditSwitchAsync<T>(
			this Task<R<T>> @this,
			Func<Ok<T>, Task>? isOk = null,
			Func<OkV<T>, Task>? isOkV = null,
			Func<Error<T>, Task>? isError = null,
			Func<Task>? isUnknown = null
		)
		{
			var result = await @this.ConfigureAwait(false);

			if (isOk == null && isOkV == null && isError == null && isUnknown == null)
			{
				return result;
			}

			Func<Task> audit = result switch
			{
				Error<T> error => () => isError == null ? Task.CompletedTask : isError(error),
				OkV<T> okV => () => isOkV == null ? Task.CompletedTask : isOkV(okV),
				Ok<T> ok => () => isOk == null ? Task.CompletedTask : isOk(ok),
				_ => isUnknown ?? new Func<Task>(() => throw new InvalidOperationException($"Unknown R<> subtype: '{result.GetType()}'."))
			};

			try
			{
				await audit().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				result.Messages.Add(new Jm.AuditAsyncException(ex));
			}

			return result;
		}

		#endregion

		#region Start Sync

		/// <summary>
		/// Audit the current result state asynchronously and return unmodified
		/// <para>Any exceptions will be caught and passed down the pipeline as a <see cref="Jm.AuditAsyncException"/> message</para>
		/// </summary>
		/// <param name="this">Current result</param>
		/// <param name="audit">Audit action</param>
		public static async Task<R<T>> AuditAsync<T>(this R<T> @this, Func<R<T>, Task> audit) => await AuditAsync(Task.Run(() => @this), audit);

		/// <summary>
		/// Audit the current result state asynchronously and return unmodified
		/// <para>Any exceptions will be caught and passed down the pipeline as a <see cref="Jm.AuditAsyncException"/> message</para>
		/// </summary>
		/// <param name="this">Current result</param>
		/// <param name="isOk">[Optional] Action to run if the current result is <see cref="Ok{T}"/></param>
		/// <param name="isOkV">[Optional] Action to run if the current result is <see cref="OkV{T}"/></param>
		/// <param name="isError">[Optional] Action to run if the current result is <see cref="Error{T}"/></param>
		/// <param name="isUnknown">[Optional] Action to run if the current result is unknown</param>
		public static async Task<R<T>> AuditSwitchAsync<T>(
			this R<T> @this,
			Func<Ok<T>, Task>? isOk = null,
			Func<OkV<T>, Task>? isOkV = null,
			Func<Error<T>, Task>? isError = null,
			Func<Task>? isUnknown = null
		) => await AuditSwitchAsync(Task.Run(() => @this), isOk, isOkV, isError, isUnknown);

		#endregion
	}
}

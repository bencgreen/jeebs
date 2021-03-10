﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jeebs.Mvc
{
	/// <summary>
	/// Controller class
	/// </summary>
	public abstract class Controller : Microsoft.AspNetCore.Mvc.Controller
	{
		/// <summary>
		/// ILog
		/// </summary>
		public ILog Log { get; }

		/// <summary>
		/// Current page number
		/// </summary>
		public long Page =>
			long.TryParse(Request.Query["p"], out long p) switch
			{
				true =>
					p,

				false =>
					1
			};

		/// <summary>
		/// Create object
		/// </summary>
		/// <param name="log">ILog</param>
		protected Controller(ILog log) =>
			Log = log;

		/// <summary>
		/// Do something, process the result and return errors if necessary, or perform the success function
		/// </summary>
		/// <typeparam name="T">Result type</typeparam>
		/// <param name="option">Option value</param>
		/// <param name="success">Function to run when the result is successful</param>
		protected async Task<IActionResult> ProcessOptionAsync<T>(Option<T> option, Func<T, Task<IActionResult>> success) =>
			await option.MatchAsyncPrivate(
				some: value =>
					success(value),
				none: reason =>
					this.ExecuteErrorAsync(reason)
			);

		/// <summary>
		/// Do something, process the result and return errors if necessary, or perform the success function
		/// </summary>
		/// <typeparam name="T">Result type</typeparam>
		/// <param name="option">Option value</param>
		/// <param name="success">Function to run when the result is successful</param>
		protected async Task<IActionResult> ProcessOptionAsync<T>(Task<Option<T>> option, Func<T, IActionResult> success) =>
			await option.MatchAsync(
				some: value =>
					success(value),
				none: reason =>
					this.ExecuteErrorAsync(reason)
			);

		/// <summary>
		/// Do something, process the result and return errors if necessary, or perform the success function
		/// </summary>
		/// <typeparam name="T">Result type</typeparam>
		/// <param name="option">Option value</param>
		/// <param name="success">Function to run when the result is successful</param>
		protected async Task<IActionResult> ProcessOptionAsync<T>(Task<Option<T>> option, Func<T, Task<IActionResult>> success) =>
			await option.MatchAsync(
				some: value =>
					success(value),
				none: reason =>
					this.ExecuteErrorAsync(reason)
			);

		/// <inheritdoc cref="ProcessOptionAsync{T}(Option{T}, Func{T, Task{IActionResult}})"/>
		protected IActionResult ProcessOption<T>(Option<T> option, Func<T, IActionResult> success) =>
			option.Match(
				some: value =>
					success(value),
				none: reason =>
					this.ExecuteErrorAsync(reason).GetAwaiter().GetResult()
			);

		/// <summary>
		/// Redirect to error page
		/// </summary>
		/// <param name="code">HTTP Status Code</param>
		protected static RedirectToActionResult RedirectToError(int code = StatusCodes.Status500InternalServerError) =>
			new(nameof(ErrorController.Handle), "Error", new { code });

		/// <summary>
		/// Return a 403 Not Allowed result
		/// </summary>
		protected StatusCodeResult NotAllowed() =>
			StatusCode(403);
	}
}

﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Serilog;
using Serilog.Events;

namespace Jeebs.Apps.WebApps.Middleware
{
	/// <summary>
	/// Logger Middleware
	/// </summary>
	public sealed class LoggerMiddleware : IMiddleware
	{
		/// <summary>
		/// ILogger
		/// </summary>
		private readonly ILogger logger = Serilog.Log.ForContext<LoggerMiddleware>();

		/// <summary>
		/// Log message template
		/// </summary>
		private const string messageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}s";

		/// <summary>
		/// Invoke Middleware
		/// </summary>
		/// <param name="context">HttpContext</param>
		/// <param name="next">Next Middleware</param>
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			// Start stopwatch
			var stopwatch = Stopwatch.StartNew();

			try
			{
				// Call the rest of the pipeline
				await next(context);
				stopwatch.Stop();

				// If the status is HTTP 200 (success), return
				var status = context.Response?.StatusCode;
				if (status == 200)
				{
					return;
				}

				// Get log level based on HTTP status - 500 or over is an HTTP error
				var level = status switch
				{
					int x when x >= 100 && x <= 299 =>
						LogEventLevel.Verbose,

					int x when x >= 300 && x <= 399 =>
						LogEventLevel.Debug,

					int x when x >= 400 && x <= 499 =>
						LogEventLevel.Information,

					_ =>
						LogEventLevel.Error,
				};

				// Write event to log
				logger.Write(level, messageTemplate, context.Request.Method, GetPath(context), status, stopwatch.Elapsed.TotalSeconds);
			}
			catch (Exception ex)
			{
				// Log error
				logger.Error(ex, messageTemplate, context.Request.Method, GetPath(context), 500, stopwatch.Elapsed.TotalSeconds);
			}
		}

		/// <summary>
		/// Get Request Path
		/// </summary>
		/// <param name="context">HttpContext</param>
		private static string GetPath(HttpContext context) =>
			context.Features.Get<IHttpRequestFeature>()?.RawTarget ?? context.Request.Path.ToString();
	}
}

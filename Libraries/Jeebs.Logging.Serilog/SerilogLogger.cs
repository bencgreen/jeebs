﻿using System;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Jeebs.Logging
{
	/// <summary>
	/// Serilog Logger
	/// </summary>
	public sealed class SerilogLogger : ILog
	{
		/// <summary>
		/// Whether or not the log will write for the specified Level
		/// </summary>
		/// <param name="level">LogLevel</param>
		/// <returns>True if the log will write at this level</returns>
		public bool IsEnabled(in LogLevel level) => Log.Logger.IsEnabled(F.EnumF.Convert(level).To<LogEventLevel>());

		/// <summary>
		/// Verbose log message
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="args">Arguments (if message supports string.Format())</param>
		public void Verbose(in string message, params object[] args) => Log.Logger.Verbose(message, args);

		/// <summary>
		/// Debug log message
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="args">Arguments (if message supports string.Format())</param>
		public void Debug(in string message, params object[] args) => Log.Logger.Debug(message, args);

		/// <summary>
		/// Information log message
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="args">Arguments (if message supports string.Format())</param>
		public void Information(in string message, params object[] args) => Log.Logger.Information(message, args);

		/// <summary>
		/// Warning log message
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="args">Arguments (if message supports string.Format())</param>
		public void Warning(in string message, params object[] args) => Log.Logger.Warning(message, args);

		/// <summary>
		/// Error log message
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="args">Arguments (if message supports string.Format())</param>
		public void Error(in string message, params object[] args) => Log.Logger.Error(message, args);

		/// <summary>
		/// Error log message
		/// </summary>
		/// <param name="ex">Exception</param>
		/// <param name="message">Message</param>
		/// <param name="args">Arguments (if message supports string.Format())</param>
		public void Error(in Exception ex, in string message, params object[] args) => Log.Logger.Error(ex, message, args);

		/// <summary>
		/// Fatal log message
		/// </summary>
		/// <param name="message">Message</param>
		/// <param name="args">Arguments (if message supports string.Format())</param>
		public void Fatal(in string message, params object[] args) => Log.Logger.Fatal(message, args);

		/// <summary>
		/// Fatal log message
		/// </summary>
		/// <param name="ex">Exception</param>
		/// <param name="message">Message</param>
		/// <param name="args">Arguments (if message supports string.Format())</param>
		public void Fatal(in Exception ex, in string message, params object[] args) => Log.Logger.Fatal(ex, message, args);
	}
}
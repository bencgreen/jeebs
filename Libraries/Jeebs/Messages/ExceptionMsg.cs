﻿using System;
using System.Collections.Generic;
using System.Text;
using Jeebs;
using Microsoft.Extensions.Logging;

namespace Jm
{
	/// <inheritdoc cref="IExceptionMsg"/>
	public abstract class ExceptionMsg : IExceptionMsg
	{
		/// <inheritdoc/>
		public Exception Exception { get; private set; }

		/// <inheritdoc/>
		public string ExceptionType
			=> Exception.GetType().FullName;

		/// <inheritdoc/>
		public string ExceptionText
			=> Exception.Message;

		/// <inheritdoc/>
		public virtual string Format
			=> "{ExceptionType}: {ExceptionText}";

		/// <inheritdoc/>
		public virtual object[] ParamArray
			=> new object[] { ExceptionType, ExceptionText };

		/// <summary>
		/// Log level - default is <see cref="Jeebs.Defaults.Log.ExceptionLevel"/>
		/// </summary>
		public LogLevel Level
			=> Jeebs.Defaults.Log.ExceptionLevel;

		/// <summary>
		/// Properties must then be set using <see cref="Set(Exception)"/>
		/// </summary>
		protected ExceptionMsg()
			=> Exception = new Exception("Unknown.");

		/// <summary>
		/// Create object from exception
		/// </summary>
		/// <param name="ex">Exception</param>
		protected ExceptionMsg(Exception ex)
			=> Exception = ex;

		/// <inheritdoc/>
		public void Set(Exception ex)
			=> Exception = ex;

		/// <summary>
		/// Output Exception type and message
		/// </summary>
		public override string ToString()
			=> string.Format(Format, ParamArray);
	}
}

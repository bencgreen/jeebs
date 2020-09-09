﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jx.Config
{
	/// <summary>
	/// Named DB Connection Not Found
	/// </summary>
	[Serializable]
	public class NamedDbConnectionNotFoundException : Exception
	{
		/// <summary>
		/// Create exception
		/// </summary>
		public NamedDbConnectionNotFoundException() { }

		/// <summary>
		/// Create exception
		/// </summary>
		/// <param name="connection"></param>
		public NamedDbConnectionNotFoundException(string connection) : base($"Connection '{connection}' was not found in configuration settings.") { }

		/// <summary>
		/// Create exception
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public NamedDbConnectionNotFoundException(string message, Exception inner) : base(message, inner) { }

		/// <summary>
		/// Create exception
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected NamedDbConnectionNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
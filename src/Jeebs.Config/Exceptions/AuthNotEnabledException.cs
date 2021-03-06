﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;

namespace Jx.Config
{
	/// <summary>
	/// Adding auth when auth is not enabled in configuration
	/// </summary>
	public class AuthNotEnabledException : Exception
	{
		/// <summary>
		/// Create exception
		/// </summary>
		public AuthNotEnabledException() : this("You need to enable auth in JSON configuration settings.") { }

		/// <summary>
		/// Create exception
		/// </summary>
		/// <param name="message"></param>
		public AuthNotEnabledException(string message) : base(message) { }

		/// <summary>
		/// Create exception
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public AuthNotEnabledException(string message, Exception inner) : base(message, inner) { }
	}
}

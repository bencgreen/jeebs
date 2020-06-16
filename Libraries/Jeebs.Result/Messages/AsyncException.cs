﻿using System;
using System.Collections.Generic;
using System.Text;
using Jeebs;

namespace Jm
{
	/// <summary>
	/// Async exception handling message
	/// </summary>
	public class AsyncException : Exception
	{
		/// <summary>
		/// Create object
		/// </summary>
		/// <param name="ex">Exception</param>
		public AsyncException(System.Exception ex) : base(ex) { }
	}
}

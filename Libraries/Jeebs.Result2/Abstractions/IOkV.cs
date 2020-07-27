﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jeebs
{
	/// <summary>
	/// Result success with a value
	/// </summary>
	/// <typeparam name="TValue">Result value type</typeparam>
	public interface IOkV<TValue> : IOk, IR<TValue>
	{
		/// <summary>
		/// Result value
		/// </summary>
		TValue Value { get; }
	}
}
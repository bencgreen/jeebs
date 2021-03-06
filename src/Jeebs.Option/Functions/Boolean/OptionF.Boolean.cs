﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Jeebs;

namespace F
{
	public static partial class OptionF
	{
		/// <summary>
		/// Special case for boolean - returns Some{bool}(true)
		/// </summary>
		public static Option<bool> True =>
			true;

		/// <summary>
		/// Special case for boolean - returns Some{bool}(false)
		/// </summary>
		public static Option<bool> False =>
			false;
	}
}

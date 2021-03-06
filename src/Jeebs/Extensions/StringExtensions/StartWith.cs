﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

namespace Jeebs
{
	public static partial class StringExtensions
	{
		/// <summary>
		/// Ensure that an input string starts with a single defined character
		/// </summary>
		/// <param name="this">The input string</param>
		/// <param name="character">The character to start the string with</param>
		/// <returns>The input string starting with a single 'character'</returns>
		public static string StartWith(this string @this, char character) =>
			Modify(@this, () => string.Format("{0}{1}", character, @this.TrimStart(character)));
	}
}

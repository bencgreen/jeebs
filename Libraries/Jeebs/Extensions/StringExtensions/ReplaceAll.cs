﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Jeebs.Reflection;

namespace Jeebs
{
	public static partial class StringExtensions
	{
		/// <summary>
		/// Replace all strings in an array
		/// </summary>
		/// <param name="this">String to perform operation on</param>
		/// <param name="replace">Array of strings to replace</param>
		/// <param name="with">String to replace occurrences with</param>
		/// <returns>String with all strings in the array replaced</returns>
		public static string ReplaceAll(this string @this, string[] replace, string with) =>
			Modify(@this, () =>
			{
				// Copy string and replace values
				string r = @this;
				foreach (string t in replace)
				{
					r = r.Replace(t, with);
				}

				return r;
			});
	}
}
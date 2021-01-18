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
		/// Replace all HTML tags
		/// </summary>
		/// <param name="this">The input string</param>
		/// <param name="replaceWith">String to replace HTML tags with</param>
		/// <returns>Input string with all HTML tags removed</returns>
		public static string ReplaceHtmlTags(this string @this, string? replaceWith) =>
			Modify(@this, () =>
			{
				// Make sure replaceWith isn't null
				if (replaceWith == null)
				{
					replaceWith = string.Empty;
				}

				// Now replace all HTML characters
				Regex re = new Regex("<.*?>");
				return re.Replace(@this, replaceWith);
			});

		/// <inheritdoc cref="ReplaceHtmlTags(string, string?)"/>
		public static string ReplaceHtmlTags(this string @this) =>
			ReplaceHtmlTags(@this, null);
	}
}

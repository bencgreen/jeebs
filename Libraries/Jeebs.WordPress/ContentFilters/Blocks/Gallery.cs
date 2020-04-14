﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Jeebs.Util;

namespace Jeebs.WordPress.ContentFilters.Blocks
{
	/// <summary>
	/// Gallery block
	/// </summary>
	internal sealed class Gallery : ParseBlocks.Block
	{
		/// <summary>
		/// Output format
		/// </summary>
		private const string format = @"<div class=""hide image-gallery"" data-ids=""{0}"" data-cols=""{1}""></div>";

		/// <summary>
		/// Parse content
		/// </summary>
		/// <param name="content">Content to parse</param>
		internal override string Parse(string content)
		{
			// Get Gallery info
			var matches = Regex.Matches(content, "<!-- wp:gallery ({.*?}) -->(.*?)<!-- /wp:gallery -->", RegexOptions.Singleline);
			if (matches.Count == 0)
			{
				return content;
			}

			// Parse each match
			foreach (Match match in matches)
			{
				// Info is encoded as JSON
				var json = match.Groups[1].Value;
				var gallery = Json.Deserialise<GalleryParsed>(json);

				// Replace content using output format
				content = content.Replace(match.Value, string.Format(format, string.Join(",", gallery.Ids), gallery.Columns));
			}

			// Return parsed content
			return content;
		}

		/// <summary>
		/// Used to parse Gallery JSON
		/// </summary>
		private class GalleryParsed
		{
			/// <summary>
			/// Image IDs
			/// </summary>
			public int[] Ids { get; set; } = new int[] { };

			/// <summary>
			/// Number of columns
			/// </summary>
			public int Columns { get; set; }
		}
	}
}
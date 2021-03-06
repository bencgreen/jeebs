﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

namespace Jeebs.WordPress.Data.ContentFilters
{
	public sealed partial class ParseBlocks
	{
		/// <summary>
		/// Parse embedded Vimeo videos
		/// </summary>
		/// <param name="content">Post content</param>
		internal static string ParseVimeo(string content) =>
			ParseEmbed(content, EmbedType.Video, Provider.Vimeo, (id, embed) =>
				string.Format("<div id=\"{0}\" class=\"hide video-vimeo\" data-url=\"{1}\">{2}</div>", id, embed.Url, embed.Url)
			);
	}
}

﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Jeebs;
using Jeebs.Data.Querying;
using Jeebs.WordPress.Data;
using Jeebs.WordPress.Data.Entities;
using static F.OptionF;

namespace F.WordPressF.DataF
{
	public static partial class QueryPostsF
	{
		/// <summary>
		/// Get query parts using the specific options
		/// </summary>
		/// <typeparam name="TModel">Return value type</typeparam>
		/// <param name="db">IWpDb</param>
		/// <param name="opt">Function to return query options</param>
		internal static Option<IQueryParts> GetQueryParts<TModel>(IWpDb db, Query.GetPostsOptions opt)
			where TModel : IWithId<WpPostId> =>
			Return(
				() => opt(new Query.PostsOptions(db.Schema)),
				e => new Msg.ErrorGettingQueryPostsOptionsMsg(e)
			)
			.Bind(
				x => x.ToParts<TModel>()
			);
	}
}

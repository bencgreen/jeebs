﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System.Collections.Generic;
using System.Threading.Tasks;
using Jeebs;
using Jeebs.WordPress.Data;
using Jeebs.WordPress.Data.Entities;
using static F.OptionF;

namespace F.WordPressF.DataF
{
	public static partial class QueryPostsF
	{
		/// <summary>
		/// Process a list of posts, adding meta / taxonomies / custom fields as required
		/// </summary>
		/// <typeparam name="TList">List type</typeparam>
		/// <typeparam name="TModel">Model type</typeparam>
		/// <param name="db">IWpDb</param>
		/// <param name="posts">Posts</param>
		internal static Task<Option<TList>> Process<TList, TModel, TPostMeta, TTerm>(IWpDb db, TList posts)
			where TList : IEnumerable<TModel>
			where TModel : IWithId
			where TPostMeta : WpPostMetaEntity
			where TTerm : WpTermEntity =>
			Return(
				posts
			)
			.BindAsync(
				x => AddMetaAsync<TList, TModel, TPostMeta>(db, x)
			)
			.BindAsync(
				x => AddCustomFieldsAsync<TList, TModel>(db, x)
			)
			.BindAsync(
				x => AddTaxonomiesAsync<TList, TModel, TTerm>(db, x)
			);
	}
}

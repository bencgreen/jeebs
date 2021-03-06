﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System.Collections.Generic;
using System.Linq;
using Jeebs;
using Jeebs.Linq;
using Jeebs.WordPress.Data;
using Jeebs.WordPress.Data.Entities;

namespace F.WordPressF.DataF
{
	public static partial class QueryPostsF
	{
		/// <summary>
		/// Set meta dictionary property for each post
		/// </summary>
		/// <typeparam name="TList">List type</typeparam>
		/// <typeparam name="TModel">Model type</typeparam>
		/// <param name="posts">Posts</param>
		/// <param name="postsMeta">Posts Meta</param>
		/// <param name="metaDict">Meta Dictionary property for <typeparamref name="TModel"/></param>
		internal static Option<TList> SetMeta<TList, TModel>(TList posts, List<PostMeta> postsMeta, Meta<TModel> metaDict)
			where TList : IEnumerable<TModel>
			where TModel : IWithId<WpPostId>
		{
			if (posts.Any() && postsMeta.Count > 0)
			{
				// Add meta to each post
				foreach (var post in posts)
				{
					var postMeta = from m in postsMeta
								   let key = m.Key
								   let value = m.Value
								   where m.PostId == post.Id.Value
								   && !string.IsNullOrEmpty(key)
								   && !string.IsNullOrEmpty(value)
								   select new KeyValuePair<string, string>(key, value);

					if (!postMeta.Any())
					{
						continue;
					}

					// Set the value of the meta property
					metaDict.Set(post, new MetaDictionary(postMeta));
				}
			}

			return posts;
		}
	}
}

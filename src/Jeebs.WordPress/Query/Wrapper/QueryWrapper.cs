﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jeebs.WordPress.Entities;
using static F.OptionF;

namespace Jeebs.WordPress
{
	/// <inheritdoc/>
	public sealed partial class QueryWrapper : Data.Querying.QueryWrapper
	{
		/// <summary>
		/// IWpDb
		/// </summary>
		private readonly IWpDb db;

		/// <summary>
		/// Setup object
		/// </summary>
		/// <param name="db">IWpDb</param>
		public QueryWrapper(IWpDb db) : base(db) =>
			this.db = db;

		#region Caches

		/// <summary>
		/// Meta Dictionary cache
		/// </summary>
		private static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> metaDictionaryCache;

		/// <summary>
		/// Taxonomies cache
		/// </summary>
		private static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> termListsCache;

		/// <summary>
		/// Custom Fields cache
		/// </summary>
		private static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> customFieldsCache;

		/// <summary>
		/// Post Content cache
		/// </summary>
		private static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> contentCache;

		/// <summary>
		/// Create empty cache dictionaries
		/// </summary>
		static QueryWrapper()
		{
			metaDictionaryCache = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();
			termListsCache = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();
			customFieldsCache = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();
			contentCache = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();
		}

		/// <summary>
		/// Get MetaDictionary for specified model
		/// </summary>
		/// <typeparam name="TModel">Model type</typeparam>
		private static Option<PropertyInfo> GetMetaDictionary<TModel>()
		{
			// Get from or Add to the cache
			var metaDictionary = metaDictionaryCache.GetOrAdd(
				typeof(TModel),
				type => from m in type.GetProperties()
						where m.PropertyType.IsEquivalentTo(typeof(MetaDictionary))
						select m
			);

			// Throw an error if there are multiple MetaDictionaries
			if (metaDictionary.Count() > 1)
			{
				return None<PropertyInfo, Msg.OnlyOneMetaDictionaryPropertySupportedMsg<TModel>>();
			}

			// If MetaDictionary is not defined return null
			if (!metaDictionary.Any())
			{
				return None<PropertyInfo, Msg.MetaDictionaryPropertyNotFoundMsg<TModel>>();
			}

			return metaDictionary.Single();
		}

		/// <summary>
		/// Get Term Lists for specified model
		/// </summary>
		/// <typeparam name="TModel">Model type</typeparam>
		private static List<PropertyInfo> GetTermLists<TModel>()
		{
			// Get from or Add to the cache
			var taxonomies = termListsCache.GetOrAdd(
				typeof(TModel),
				type => from t in type.GetProperties()
						where t.PropertyType.IsEquivalentTo(typeof(TermList))
						select t
			);

			// If there aren't any return an empty list
			if (!taxonomies.Any())
			{
				return new List<PropertyInfo>();
			}

			return taxonomies.ToList();
		}

		/// <summary>
		/// Get Custom Fields for specified model
		/// </summary>
		/// <typeparam name="TModel">Model type</typeparam>
		private static List<PropertyInfo> GetCustomFields<TModel>()
		{
			// Get from or Add to the cache
			var customFields = customFieldsCache.GetOrAdd(
				typeof(TModel),
				type => from cf in type.GetProperties()
						where cf.PropertyType.GetInterfaces().Contains(typeof(ICustomField))
						select cf
			);

			// If there aren't any return an empty list
			if (!customFields.Any())
			{
				return new List<PropertyInfo>();
			}

			return customFields.ToList();
		}

		/// <summary>
		/// Get Post Content property for specified model
		/// </summary>
		/// <typeparam name="TModel">Model type</typeparam>
		private static Option<PropertyInfo> GetPostContent<TModel>()
		{
			// Get from or Add to the cache
			var content = contentCache.GetOrAdd(
				typeof(TModel),
				type => from c in type.GetProperties()
						where c.Name == nameof(WpPostEntity.Content)
						select c
			);

			// If content is not defined return null
			if (!content.Any())
			{
				return None<PropertyInfo, Msg.ContentPropertyNotFoundMsg<TModel>>();
			}

			return content.Single();
		}

		#endregion

		/// <summary>Messages</summary>
		public static partial class Msg
		{
			/// <summary>Content property not found on entity</summary>
			/// <typeparam name="T">Post type</typeparam>
			public sealed record ContentPropertyNotFoundMsg<T> : IMsg { }

			/// <summary>MetaDictionary property not found on entity</summary>
			/// <typeparam name="T">Post type</typeparam>
			public sealed record MetaDictionaryPropertyNotFoundMsg<T> : IMsg { }

			/// <summary>Multiple MetaDictionary properties found on entity</summary>
			/// <typeparam name="T">Post type</typeparam>
			public sealed record OnlyOneMetaDictionaryPropertySupportedMsg<T> : IMsg { }
		}
	}
}
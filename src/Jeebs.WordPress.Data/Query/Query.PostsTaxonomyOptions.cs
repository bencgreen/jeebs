﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Linq;
using System.Linq.Expressions;
using Jeebs.Data;
using Jeebs.Data.Enums;
using Jeebs.Data.Mapping;
using Jeebs.Data.Querying;
using Jeebs.Linq;
using Jeebs.WordPress.Data.Entities;
using Jeebs.WordPress.Data.Enums;
using Jeebs.WordPress.Data.Tables;
using static F.DataF.QueryF;

namespace Jeebs.WordPress.Data
{
	public static partial class Query
	{
		/// <inheritdoc cref="IQueryPostsTaxonomyOptions"/>
		public sealed record PostsTaxonomyOptions : Options<WpTermId, TermTable>, IQueryPostsTaxonomyOptions
		{
			/// <inheritdoc/>
			public IImmutableList<Taxonomy>? Taxonomies { get; init; }

			/// <inheritdoc/>
			public IImmutableList<WpPostId>? PostIds { get; init; }

			/// <inheritdoc/>
			public TaxonomySort SortBy { get; init; }

			/// <inheritdoc/>
			protected override Expression<Func<TermTable, string>> IdColumn =>
				table => table.TermId;

			/// <summary>
			/// Internal creation only
			/// </summary>
			/// <param name="db">IWpDb</param>
			internal PostsTaxonomyOptions(IWpDb db) : base(db, db.Schema.Term) =>
				Maximum = null;

			/// <inheritdoc/>
			protected override Option<IColumnList> GetColumns<TModel>(ITable table) =>
				Extract<TModel>.From(table, T.TermRelationship, T.TermTaxonomy);

			/// <inheritdoc/>
			protected override Option<QueryParts> GetParts(ITable table, IColumnList cols, IColumn idColumn) =>
				CreateParts(
					table, cols
				)
				.Bind(
					x => AddInnerJoin(x, T.Term, t => t.TermId, T.TermTaxonomy, tx => tx.TermId)
				)
				.Bind(
					x => AddInnerJoin(x, T.TermTaxonomy, tx => tx.TermTaxonomyId, T.TermRelationship, tr => tr.TermTaxonomyId)
				)
				.SwitchIf(
					_ => Id?.Value > 0 || Ids?.Length > 0,
					x => AddWhereId(x, idColumn)
				)
				.SwitchIf(
					_ => Taxonomies?.Count > 0,
					ifTrue: AddWhereTaxonomies
				)
				.SwitchIf(
					_ => PostIds?.Count > 0,
					ifTrue: AddWherePostIds
				)
				.Bind(
					x => AddSort(x)
				);

			/// <summary>
			/// Add Where Taxonomies
			/// </summary>
			/// <param name="parts">QueryParts</param>
			internal Option<QueryParts> AddWhereTaxonomies(QueryParts parts)
			{
				// Add Taxonomies
				if (Taxonomies?.Count > 0)
				{
					return AddWhere(parts, T.TermTaxonomy, t => t.Taxonomy, Compare.In, Taxonomies);
				}

				// Return
				return parts;
			}

			/// <summary>
			/// Add Where Post IDs
			/// </summary>
			/// <param name="parts">QueryParts</param>
			internal Option<QueryParts> AddWherePostIds(QueryParts parts)
			{
				// Add Post IDs
				if (PostIds?.Count > 0)
				{
					return AddWhere(parts, T.TermRelationship, t => t.PostId, Compare.In, PostIds);
				}

				// Return
				return parts;
			}

			/// <summary>
			/// Add custom Sort or default Sort
			/// </summary>
			/// <param name="parts">QueryParts</param>
			protected override Option<QueryParts> AddSort(QueryParts parts)
			{
				// Add custom Sort options
				if (SortRandom || Sort?.Length > 0)
				{
					return base.AddSort(parts);
				}

				// Add sort
				return from title in GetColumnFromExpression(T.Term, t => t.Title)
					   from count in GetColumnFromExpression(T.TermTaxonomy, tx => tx.Count)
					   let sortRange = SortBy switch
					   {
						   TaxonomySort.CountDescending =>
							   new[] { (count, SortOrder.Descending), (title, SortOrder.Ascending) },

						   _ =>
							   new[] { (title, SortOrder.Ascending) }
					   }
					   select parts with { Sort = parts.Sort.WithRange(sortRange) };
			}
		}
	}
}

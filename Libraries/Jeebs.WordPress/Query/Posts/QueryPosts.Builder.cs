﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jeebs.Data;
using Jeebs.Data.Enums;
using Jeebs.WordPress.Enums;

namespace Jeebs.WordPress
{
	/// <summary>
	/// Query Posts
	/// </summary>
	public partial class QueryPosts
	{
		/// <inheritdoc/>
		internal sealed class Builder<T> : QueryPartsBuilderExtended<T, Options>
		{
			/// <summary>
			/// IWpDb
			/// </summary>
			private readonly IWpDb db;

			/// <summary>
			/// Create object
			/// </summary>
			/// <param name="db">IWpDb</param>
			internal Builder(IWpDb db) : base(db.Adapter)
				=> this.db = db;

			/// <inheritdoc/>
			public override IQueryParts Build(Options opt)
			{
				// SELECT columns
				AddSelect(db.Post);

				// FROM table
				AddFrom(db.Post);

				// WHERE type
				var type = opt.Type;
				AddWhere($"{__(db.Post, p => p.Type)} = @{nameof(type)}", new { type });

				// WHERE status
				var status = opt.Status;
				AddWhere($"{__(db.Post, p => p.Status)} = @{nameof(status)}", new { status });

				// WHERE Id
				if (opt.Id is long postId)
				{
					AddWhere($"{__(db.Post, p => p.PostId)} = @{nameof(postId)}", new { postId });
				}

				// WHERE search
				else if (opt.SearchText is string searchText)
				{
					AddWhereSearch(searchText, opt);
				}

				// WHERE dates
				if (opt.From is DateTime fromBase)
				{
					var from = fromBase.StartOfDay().ToMySqlString();
					AddWhere($"{__(db.Post, p => p.PublishedOn)} >= @{nameof(from)}", new { from });
				}
				if (opt.To is DateTime toBase)
				{
					var to = toBase.EndOfDay().ToMySqlString();
					AddWhere($"{__(db.Post, p => p.PublishedOn)} <= @{nameof(to)}", new { to });
				}

				// WHERE parent ID
				if (opt.ParentId is int parentId)
				{
					AddWhere($"{__(db.Post, p => p.ParentId)} = @{nameof(parentId)}", new { parentId });
				}

				// WHERE taxonomies
				if (opt.Taxonomies is var taxonomiesList && taxonomiesList.Count > 0)
				{
					AddWhereTaxonomies(taxonomiesList);
				}

				// WHERE custom fields
				if (opt.CustomFields is var fields && fields.Count > 0)
				{
					AddWhereCustomFields(fields);
				}

				// Finish and Return
				return FinishBuild(opt, (__(db.Post, p => p.PublishedOn), SortOrder.Descending));
			}

			/// <summary>
			/// Add WHERE for text search
			/// </summary>
			/// <param name="searchText">Search text</param>
			/// <param name="opt">QueryOptions</param>
			private void AddWhereSearch(string searchText, Options opt)
			{
				// Trim search text
				var search = searchText.Trim();
				var where = string.Empty;

				// Set comparison operator and modify search string accordingly
				var comparison = "=";
				if (opt.SearchOperator == SearchOperators.Like)
				{
					// Change the comparison
					comparison = "LIKE";

					// If % has not already been added to the search string, add it
					if (search.IndexOf("%") == -1)
					{
						search = $"%{search}%";
					}
				}

				// Search title
				if ((opt.SearchFields & SearchPostFields.Title) != 0)
				{
					where += $"{__(db.Post, p => p.Title)} {comparison} @{nameof(search)}";
				}

				// Search slug
				if ((opt.SearchFields & SearchPostFields.Slug) != 0)
				{
					if (!string.IsNullOrEmpty(where))
					{
						where += " OR ";
					}

					where += $"{__(db.Post, p => p.Slug)} {comparison} @{nameof(search)}";
				}

				// Search content
				if ((opt.SearchFields & SearchPostFields.Content) != 0)
				{
					if (!string.IsNullOrEmpty(where))
					{
						where += " OR ";
					}

					where += $"{__(db.Post, p => p.Content)} {comparison} @{nameof(search)}";
				}

				// Add to WHERE
				AddWhere($"({where})", new { search });
			}

			/// <summary>
			/// Add WHERE for taxonomies search
			/// </summary>
			/// <param name="taxonomiesList">List of taxonomies to search</param>
			private void AddWhereTaxonomies(IList<(Taxonomy taxonomy, long id)> taxonomiesList)
			{
				// Setup variables
				var taxonomyWhere = string.Empty;
				var taxonomyNameIndex = 0;
				var taxonomyParameters = new QueryParameters();

				// Group taxonomies by taxonomy name
				var taxonomies = from t in taxonomiesList
								 group t by t.taxonomy into g
								 select new
								 {
									 Name = g.Key,
									 Ids = g.Select(x => x.id).ToList()
								 };

				// Add each taxonomy
				foreach (var taxonomy in taxonomies)
				{
					// Add AND if this is not the first conditional clause
					if (!string.IsNullOrEmpty(taxonomyWhere))
					{
						taxonomyWhere += " AND ";
					}

					// Name of the taxonomy parameter
					var taxonomyNameParameter = $"@taxonomy{taxonomyNameIndex}";
					taxonomyParameters.Add(taxonomyNameParameter, taxonomy.Name);

					// Add SQL commands to lookup taxonomy terms
					var subQuery = "SELECT COUNT(1) ";
					subQuery += $"FROM {__(db.TermRelationship)} ";
					subQuery += $"INNER JOIN {__(db.TermTaxonomy)} ON {__(db.TermRelationship, tr => tr.TermTaxonomyId)} = {__(db.TermTaxonomy, tx => tx.TermTaxonomyId)} ";
					subQuery += $"WHERE {__(db.TermTaxonomy, tx => tx.Taxonomy)} = {taxonomyNameParameter} ";
					subQuery += $"AND {__(db.TermRelationship, tr => tr.PostId)} = {__(db.Post, p => p.PostId)} ";
					subQuery += $"AND {__(db.TermTaxonomy, tx => tx.TermId)} IN (";

					// Add the terms for this taxonomy
					var taxonomyIdIndex = 0;
					foreach (var taxonomyId in taxonomy.Ids)
					{
						// Add a comma if this is not the first term
						if (taxonomyIdIndex > 0)
						{
							subQuery += ", ";
						}

						// Add the term parameter and reference
						var taxonomyIdParameter = $"{taxonomyNameParameter}_{taxonomyIdIndex}";

						subQuery += taxonomyIdParameter;
						taxonomyParameters.Add(taxonomyIdParameter, taxonomyId);

						// Increase taxonomy term index
						taxonomyIdIndex++;
					}

					// Close IN function
					subQuery += ")";

					// Add to sub-query, matching the number of terms
					taxonomyWhere += $"({subQuery}) = {taxonomy.Ids.Count}";

					// Increase taxonomy name index
					taxonomyNameIndex++;
				}

				// Add to main WHERE clause
				if (!string.IsNullOrEmpty(taxonomyWhere))
				{
					AddWhere($"({taxonomyWhere})", taxonomyParameters);
				}
			}

			/// <summary>
			/// Add WHERE for custom fields search
			/// </summary>
			/// <param name="fields">List of custom fields to search</param>
			private void AddWhereCustomFields(IList<(ICustomField field, SearchOperators op, object value)> fields)
			{
				// Setup variables
				var customFieldWhere = string.Empty;
				var customFieldIndex = 0;
				var customFieldParameters = new QueryParameters();

				// Add each custom field
				foreach (var (field, op, value) in fields)
				{
					// Add AND if this is not the first conditional clause
					if (!string.IsNullOrEmpty(customFieldWhere))
					{
						customFieldWhere += " AND ";
					}

					// Set comparison operators and modify search string accordingly
					var customFieldComparison = "=";
					var customFieldSearch = value.ToString();
					if (op == SearchOperators.Like)
					{
						// Change the comparison
						customFieldComparison = "LIKE";

						// If % has not already been added to the search string, add it
						if (customFieldSearch.IndexOf("%") == -1)
						{
							customFieldSearch = $"%{customFieldSearch}%";
						}
					}

					// Name of the custom field parameter
					var customFieldKeyParameter = $"@customField{customFieldIndex}_Key";
					var customFieldValueParameter = $"@customField{customFieldIndex}_Value";
					customFieldParameters.Add(customFieldKeyParameter, field.Key);
					customFieldParameters.Add(customFieldValueParameter, customFieldSearch);

					// Add SQL commands to lookup custom field
					var subQuery = "SELECT COUNT(1) ";
					subQuery += $"FROM {__(db.PostMeta)} ";
					subQuery += $"WHERE {__(db.PostMeta, pm => pm.PostId)} = {__(db.Post, p => p.PostId)} ";
					subQuery += $"AND {__(db.PostMeta, pm => pm.Key)} = {customFieldKeyParameter} ";
					subQuery += $"AND {__(db.PostMeta, pm => pm.Value)} {customFieldComparison} {customFieldValueParameter} ";

					// Add sub query to where
					customFieldWhere += $"({subQuery}) = 1";

					// Increase custom field index
					customFieldIndex++;
				}

				// Add to main WHERE clause
				if (!string.IsNullOrEmpty(customFieldWhere))
				{
					AddWhere($"({customFieldWhere})", customFieldParameters);
				}
			}
		}
	}
}

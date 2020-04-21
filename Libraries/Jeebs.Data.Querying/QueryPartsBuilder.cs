﻿using System;
using System.Collections.Generic;
using System.Text;
using Jeebs.Data.Enums;

namespace Jeebs.Data
{
	/// <inheritdoc cref="IQueryPartsBuilder{TModel, TOptions}"/>
	public abstract class QueryPartsBuilder<TModel, TOptions> : IQueryPartsBuilder<TModel, TOptions>
		where TOptions : QueryOptions
	{
		/// <summary>
		/// QueryParts
		/// </summary>
		private readonly IQueryParts parts;

		/// <inheritdoc/>
		public IAdapter Adapter { get; }

		/// <summary>
		/// Create object
		/// </summary>
		/// <param name="adapter">IAdapter</param>
		protected QueryPartsBuilder(IAdapter adapter)
		{
			parts = new QueryParts();
			Adapter = adapter;
		}

		/// <summary>
		/// Build the query
		/// </summary>
		/// <param name="opt">TOptions</param>
		public abstract IQueryParts Build(TOptions opt);

		/// <summary>
		/// Finish Build process by adding ORDER BY, LIMIT and OFFSET values
		/// </summary>
		/// <param name="opt">TOptions</param>
		/// <param name="defaultSort">Default sort columns</param>
		protected IQueryParts FinishBuild(TOptions opt, params (string column, SortOrder order)[] defaultSort)
		{
			// ORDER BY
			AddSort(opt, defaultSort);

			// LIMIT and OFFSET
			AddLimitAndOffset(opt);

			// Return
			return parts;
		}

		/// <summary>
		/// Add Sort
		/// </summary>
		/// <param name="opt">TOptions</param>
		/// <param name="defaultSort">Default sort columns</param>
		private void AddSort(TOptions opt, (string column, SortOrder order)[] defaultSort)
		{
			// Random sort
			if (opt.SortRandom)
			{
				(parts.OrderBy ?? (parts.OrderBy = new List<string>())).Clear();
				parts.OrderBy.Add(Adapter.GetRandomSortOrder());
			}
			// Specified sort
			else if (opt.Sort is (string column, SortOrder order)[] sort && sort.Length > 0)
			{
				Add(sort);
			}
			// Default sort
			else
			{
				Add(defaultSort);
			}

			// Add to ORDER BY
			void Add(params (string column, SortOrder order)[] sort)
			{
				if (sort.Length == 0)
				{
					return;
				}

				if (parts.OrderBy == null)
				{
					parts.OrderBy = new List<string>();
				}

				foreach (var (column, order) in sort)
				{
					parts.OrderBy.Add(Adapter.GetSortOrder(column, order));
				}
			}
		}

		/// <summary>
		/// Add Limit and Offset
		/// </summary>
		/// <param name="opt">TOptions</param>
		private void AddLimitAndOffset(TOptions opt)
		{
			// LIMIT
			if (opt.Limit is long limit)
			{
				parts.Limit = limit;
			}

			// OFFSET
			if (opt.Offset is long offset)
			{
				parts.Offset = offset;
			}
		}

		/// <summary>
		/// Set FROM
		/// </summary>
		/// <param name="from">FROM string</param>
		/// <param name="overwrite">[Optional] If true, will overwrite whatever already exists in FROM</param>
		protected void AddFrom(string from, bool overwrite = false)
		{
			if (string.IsNullOrEmpty(parts.From) || overwrite)
			{
				parts.From = from;
			}
			else
			{
				throw new Jx.Data.QueryException("FROM has already been set.");
			}
		}

		/// <summary>
		/// Set SELECT
		/// </summary>
		/// <param name="select">SELECT string</param>
		/// <param name="overwrite">[Optional] If true, will overwrite whatever already exists in SELECT</param>
		protected void AddSelect(string select, bool overwrite = false)
		{
			if (string.IsNullOrEmpty(parts.Select) || overwrite)
			{
				parts.Select = select;
			}
			else
			{
				throw new Jx.Data.QueryException("SELECT has already been set.");
			}
		}

		/// <summary>
		/// Add JOIN
		/// </summary>
		/// <param name="join">JOINT list</param>
		/// <param name="table">JOIN table</param>
		/// <param name="on">JOIN column - should be a column on the JOIN table</param>
		/// <param name="equals">EQUALS table and column</param>
		private IList<(string table, string on, string equals)> AddJoin(
			IList<(string table, string on, string equals)>? join,
			object table,
			string on,
			(object table, string column) equals
		)
		{
			// Use existing list or create new one
			var joinList = join ?? new List<(string table, string on, string equals)>();

			// Add the join
			joinList.Add((
				Adapter.Escape(table),
				Adapter.EscapeAndJoin(table, on),
				Adapter.EscapeAndJoin(equals.table, equals.column)
			));

			// Return the join list
			return joinList;
		}

		/// <summary>
		/// Set INNER JOIN
		/// </summary>
		/// <param name="table">JOIN table</param>
		/// <param name="on">JOIN column - should be a column on the JOIN table</param>
		/// <param name="equals">EQUALS table and column</param>
		protected void AddInnerJoin(object table, string on, (object table, string column) equals)
		{
			parts.InnerJoin = AddJoin(parts.InnerJoin, table, on, equals);
		}

		/// <summary>
		/// Set LEFT JOIN
		/// </summary>
		/// <param name="table">JOIN table</param>
		/// <param name="on">JOIN column - should be a column on the JOIN table</param>
		/// <param name="equals">EQUALS table and column</param>
		protected void AddLeftJoin(object table, string on, (object table, string column) equals)
		{
			parts.LeftJoin = AddJoin(parts.LeftJoin, table, on, equals);
		}

		/// <summary>
		/// Set RIGHT JOIN
		/// </summary>
		/// <param name="table">JOIN table</param>
		/// <param name="on">JOIN column - should be a column on the JOIN table</param>
		/// <param name="equals">EQUALS table and column</param>
		protected void AddRightJoin(object table, string on, (object table, string column) equals)
		{
			parts.RightJoin = AddJoin(parts.RightJoin, table, on, equals);
		}

		/// <summary>
		/// Add WHERE clause
		/// </summary>
		/// <param name="where">WHERE string</param>
		/// <param name="parameters">[Optional] Parameters to add</param>
		protected void AddWhere(string where, object? parameters = null)
		{
			(parts.Where ?? (parts.Where = new List<string>())).Add(where);

			if (parameters != null)
			{
				parts.Parameters.Add(parameters);
			}
		}
	}
}
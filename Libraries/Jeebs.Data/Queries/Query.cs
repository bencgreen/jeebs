﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jeebs.Data.Enums;

namespace Jeebs.Data
{
	/// <summary>
	/// Query Args
	/// </summary>
	public class Query : IQuery
	{
		/// <summary>
		/// IUnitOfWork
		/// </summary>
		protected IUnitOfWork UnitOfWork { get; }

		/// <summary>
		/// Will be set to true if a UnitOfWork is not passed to the query on creation
		/// </summary>
		private readonly bool disposeUnitOfWork;

		/// <summary>
		/// IAdapter
		/// </summary>
		protected IAdapter Adapter { get => UnitOfWork.Adapter; }

		/// <summary>
		/// From
		/// </summary>
		public string From { get; protected set; }

		/// <summary>
		/// Select
		/// </summary>
		public string? Select { get; protected set; }

		/// <summary>
		/// Inner Join
		/// </summary>
		public List<string>? InnerJoin { get; protected set; }

		/// <summary>
		/// Left Join
		/// </summary>
		public List<string>? LeftJoin { get; protected set; }

		/// <summary>
		/// Right Join
		/// </summary>
		public List<string>? RightJoin { get; protected set; }

		/// <summary>
		/// Where
		/// </summary>
		public List<string>? Where { get; protected set; }

		/// <summary>
		/// Query Parameters
		/// </summary>
		public QueryParameters Parameters { get; }

		/// <summary>
		/// Order By
		/// </summary>
		public List<string>? OrderBy { get; protected set; }

		/// <summary>
		/// Limit
		/// </summary>
		public double Limit { get; protected set; }

		/// <summary>
		/// Offset
		/// </summary>
		public double Offset { get; protected set; }

		/// <summary>
		/// Create object
		/// </summary>
		/// <param name="unitOfWorkFactory">Function to create an IUnitOfWork, if one isn't passed</param>
		/// <param name="unitOfWork">[Optional] Pre-created IUnitOfWork</param>
		protected Query(Func<IUnitOfWork> unitOfWorkFactory, IUnitOfWork? unitOfWork = null)
		{
			if (unitOfWork == null)
			{
				UnitOfWork = unitOfWorkFactory();
				disposeUnitOfWork = true;
			}
			else
			{
				UnitOfWork = unitOfWork;
			}

			From = string.Empty;
			Parameters = new QueryParameters();
		}

		/// <summary>
		/// Set FROM
		/// </summary>
		/// <param name="from">FROM string</param>
		/// <param name="overwrite">[Optional] If true, will overwrite whatever already exists in FROM</param>
		protected void AddFrom(string from, bool overwrite = false)
		{
			if (string.IsNullOrEmpty(From) || overwrite)
			{
				From = from;
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
			if (string.IsNullOrEmpty(Select) || overwrite)
			{
				Select = select;
			}
			else
			{
				throw new Jx.Data.QueryException("SELECT has already been set.");
			}
		}

		/// <summary>
		/// Add WHERE clause
		/// </summary>
		/// <param name="where">WHERE string</param>
		protected void AddWhere(string where, object? parameters = null)
		{
			(Where ?? (Where = new List<string>())).Add(where);

			if (parameters != null)
			{
				Parameters.Add(parameters);
			}
		}

		/// <summary>
		/// Add ORDER BY random - will clear previous order by items
		/// </summary>
		protected void AddOrderByRandom()
		{
			(OrderBy ?? (OrderBy = new List<string>())).Clear();
			OrderBy.Add(Adapter.GetRandomSortOrder());
		}

		/// <summary>
		/// Add ORDER BY columns
		/// </summary>
		/// <param name="sort">Sort columns</param>
		protected void AddOrderBy(params (string selectColumn, SortOrder order)[] sort)
		{
			// Add sort clauses
			if (sort.Length > 0)
			{
				if (OrderBy == null)
				{
					OrderBy = new List<string>();
				}

				foreach (var (column, order) in sort)
				{
					OrderBy.Add(Adapter.GetSortOrder(column, order));
				}
			}
		}

		/// <summary>
		/// Add LIMIT
		/// </summary>
		/// <param name="limit"></param>
		protected void AddLimit(double limit) => Limit = limit;

		/// <summary>
		/// Add OFFSET
		/// </summary>
		protected void AddOffset(double offset) => Offset = offset;

		/// <summary>
		/// Build SELECT query
		/// </summary>
		public string GetQuerySql() => Adapter.Retrieve(this);

		public async Task<Result<int>> GetCountAsync()
		{
			// Store select
			var actualSelect = Select;

			// Get count query
			Select = Adapter.GetSelectCount();
			var countQuery = Adapter.Retrieve(this);

			// Execute
			var count = await UnitOfWork.ExecuteScalarAsync<int>(countQuery, Parameters);

			// Reset select and return
			Select = actualSelect;
			return count;
		}

		/// <summary>
		/// Dispose of the UnitOfWork if we created one just for this query
		/// Otherwise, the UnitOfWork was passed to the query, so will be disposed of in the wider context
		/// </summary>
		public void Dispose()
		{
			if (disposeUnitOfWork)
			{
				UnitOfWork.Dispose();
			}
		}
	}
}

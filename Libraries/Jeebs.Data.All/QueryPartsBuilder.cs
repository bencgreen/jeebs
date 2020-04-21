﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jeebs.Data
{
	/// <summary>
	/// Extended methods for building a query from constituent parts
	/// </summary>
	/// <typeparam name="TModel">Model type</typeparam>
	/// <typeparam name="TOptions">Options type</typeparam>
	public abstract class QueryPartsBuilderExtended<TModel, TOptions>: QueryPartsBuilder<TModel, TOptions>
		where TOptions : QueryOptions
	{
		/// <summary>
		/// Create object
		/// </summary>
		/// <param name="adapter">IAdapter</param>
		protected QueryPartsBuilderExtended(IAdapter adapter) : base(adapter) { }

		/// <summary>
		/// Add FROM shorthand
		/// </summary>
		/// <param name="tables">List of tables</param>
#pragma warning disable IDE1006 // Naming Styles
		protected void AddFrom(params Table[] tables)
#pragma warning restore IDE1006 // Naming Styles
		{
			AddFrom(Adapter.EscapeAndJoin(tables));
		}

		/// <summary>
		/// Add SELECT shorthand
		/// </summary>
		/// <param name="tables">List of tables</param>
#pragma warning disable IDE1006 // Naming Styles
		protected void AddSelect(params Table[] tables)
#pragma warning restore IDE1006 // Naming Styles
		{
			AddSelect(Adapter.Extract<TModel>(tables));
		}

		/// <summary>
		/// Escape shorthand
		/// </summary>
		/// <typeparam name="T">Table type</typeparam>
		/// <param name="table">Table</param>
#pragma warning disable IDE1006 // Naming Styles
		protected string __<T>(T table)
#pragma warning restore IDE1006 // Naming Styles
			where T : Table
		{
			return Adapter.Escape(table);
		}

		/// <summary>
		/// Escape shorthand
		/// </summary>
		/// <typeparam name="T">Table type</typeparam>
		/// <param name="table">Table</param>
		/// <param name="column">Column</param>
#pragma warning disable IDE1006 // Naming Styles
		protected string __<T>(T table, Func<T, string> column)
#pragma warning restore IDE1006 // Naming Styles
			where T : Table
		{
			return Adapter.EscapeAndJoin(table, column(table));
		}

		/// <summary>
		/// Set INNER JOIN
		/// </summary>
		/// <param name="table">The table to JOIN</param>
		/// <param name="on">The column to JOIN (from <paramref name="table"/>)</param>
		/// <param name="equals">The table and colum to join TO (must already be part of the query)</param>
		protected void AddInnerJoin<T1, T2>(T1 table, Func<T1, string> on, (T2 table, Func<T2, string> column) equals)
			where T1 : Table
			where T2 : Table
		{
			AddInnerJoin(table, on(table), (equals.table, equals.column(equals.table)));
		}

		/// <summary>
		/// Set LEFT JOIN
		/// </summary>
		/// <param name="table">The table to JOIN</param>
		/// <param name="on">The column to JOIN (from <paramref name="table"/>)</param>
		/// <param name="equals">The table and colum to join TO (must already be part of the query)</param>
		protected void AddLeftJoin<T1, T2>(T1 table, Func<T1, string> on, (T2 table, Func<T2, string> column) equals)
			where T1 : Table
			where T2 : Table
		{
			AddLeftJoin(table, on(table), (equals.table, equals.column(equals.table)));
		}

		/// <summary>
		/// Set RIGHT JOIN
		/// </summary>
		/// <param name="table">The table to JOIN</param>
		/// <param name="on">The column to JOIN (from <paramref name="table"/>)</param>
		/// <param name="equals">The table and colum to join TO (must already be part of the query)</param>
		protected void AddRightJoin<T1, T2>(T1 table, Func<T1, string> on, (T2 table, Func<T2, string> column) equals)
			where T1 : Table
			where T2 : Table
		{
			AddRightJoin(table, on(table), (equals.table, equals.column(equals.table)));
		}
	}
}
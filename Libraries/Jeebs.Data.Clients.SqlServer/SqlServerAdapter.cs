﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jeebs.Data.Clients.SqlServer
{
	/// <summary>
	/// SqlServer adapter
	/// </summary>
	public sealed class SqlServerAdapter : Adapter
	{
		/// <summary>
		/// Create object
		/// </summary>
		public SqlServerAdapter() : base('.', '[', ']', "AS", '[', ']', "ASC", "DESC") { }

		/// <summary>
		/// Return random sort string
		/// </summary>
		public override string GetRandomSortOrder() => "NEWID()";

		/// <summary>
		/// Query to insert a single row and return the new ID
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		public override string CreateSingleAndReturnId<T>()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Query to retrieve a single row by ID
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		public override string RetrieveSingleById<T>()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Build a SELECT query
		/// </summary>
		/// <param name="parts">QueryParts</param>
		/// <returns>SELECT query</returns>
		public override string Retrieve<T>(QueryParts<T> parts)
		{
			// Start query
			StringBuilder sql = new StringBuilder($"SELECT {parts.Select ?? "*"} FROM {parts.From}");

			// Add INNER JOIN
			if (parts.InnerJoin is List<(string table, string on, string equals)> innerJoinValues)
			{
				foreach (var item in innerJoinValues)
				{
					sql.Append($" INNER JOIN {item.table} ON {item.on} = {item.equals}");
				}
			}

			// Add LEFT JOIN
			if (parts.LeftJoin is List<(string table, string on, string equals)> leftJoinValues)
			{
				foreach (var item in leftJoinValues)
				{
					sql.Append($" LEFT JOIN {item.table} ON {item.on} = {item.equals}");
				}
			}

			// Add RIGHT JOIN
			if (parts.RightJoin is List<(string table, string on, string equals)> rightJoinValues)
			{
				foreach (var item in rightJoinValues)
				{
					sql.Append($" RIGHT JOIN {item.table} ON {item.on} = {item.equals}");
				}
			}

			// Add WHERE
			if (parts.Where is List<string> whereValue)
			{
				sql.Append($" WHERE {string.Join(" AND ", whereValue)}");
			}

			// Add ORDER BY
			if (parts.OrderBy is List<string> orderByValue)
			{
				sql.Append($" ORDER BY {string.Join(", ", orderByValue)}");
			}

			// Add LIMIT
			if (parts.Limit is long limitValue && limitValue > 0)
			{
				sql.Append($" LIMIT {limitValue}");
			}

			// Add OFFSET
			if (parts.Offset is long offsetValue && offsetValue > 0)
			{
				sql.Append($" OFFSET {offsetValue}");
			}

			// Return query string
			return sql.ToString();
		}

		/// <summary>
		/// Query to update a single row
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		public override string UpdateSingle<T>()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Query to delete a single row
		/// </summary>
		/// <typeparam name="T">Entity type</typeparam>
		public override string DeleteSingle<T>()
		{
			throw new NotImplementedException();
		}
	}
}
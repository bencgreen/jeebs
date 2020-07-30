﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jeebs.Data.Clients.SqlServer
{
	/// <inheritdoc/>
	public sealed class SqlServerAdapter : Adapter
	{
		/// <summary>
		/// Create object
		/// </summary>
		public SqlServerAdapter() : base('.', ", ", '[', ']', "AS", '[', ']', "ASC", "DESC") { }

		/// <inheritdoc/>
		public override string GetRandomSortOrder()
			=> "NEWID()";

		/// <inheritdoc/>
		public override string CreateSingleAndReturnId(string table, List<string> columns, List<string> aliases)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public override string Retrieve(IQueryParts parts)
		{
			// Start query
			StringBuilder sql = new StringBuilder($"SELECT {parts.Select ?? "*"} FROM {parts.From}");

			// Add INNER JOIN
			if (parts.InnerJoin is List<(string table, string on, string equals)> innerJoinValues)
			{
				foreach (var (table, on, equals) in innerJoinValues)
				{
					sql.Append($" INNER JOIN {table} ON {on} = {equals}");
				}
			}

			// Add LEFT JOIN
			if (parts.LeftJoin is List<(string table, string on, string equals)> leftJoinValues)
			{
				foreach (var (table, on, equals) in leftJoinValues)
				{
					sql.Append($" LEFT JOIN {table} ON {on} = {equals}");
				}
			}

			// Add RIGHT JOIN
			if (parts.RightJoin is List<(string table, string on, string equals)> rightJoinValues)
			{
				foreach (var (table, on, equals) in rightJoinValues)
				{
					sql.Append($" RIGHT JOIN {table} ON {on} = {equals}");
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

		/// <inheritdoc/>
		public override string RetrieveSingleById(List<string> columns, string table, string idColumn)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public override string UpdateSingle(string table, List<string> columns, List<string> aliases, string idColumn, string idAlias, string? versionColumn = null, string? versionAlias = null)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public override string DeleteSingle(string table, string idColumn, string idAlias, string? versionColumn = null, string? versionAlias = null)
		{
			throw new NotImplementedException();
		}
	}
}
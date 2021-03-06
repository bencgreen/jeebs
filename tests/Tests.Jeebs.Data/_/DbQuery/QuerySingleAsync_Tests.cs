﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System.Data;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace Jeebs.Data.DbQuery_Tests
{
	public class QuerySingleAsync_Tests
	{
		[Theory]
		[InlineData(CommandType.StoredProcedure)]
		[InlineData(CommandType.TableDirect)]
		[InlineData(CommandType.Text)]
		public async Task Calls_Db_QuerySingleAsync_With_CommandType(CommandType input)
		{
			// Arrange
			var (db, _, _, query) = DbQuery_Setup.Get();
			var value = F.Rnd.Str;
			var param = F.Rnd.Int;
			var transaction = Substitute.For<IDbTransaction>();

			// Act
			_ = await query.QuerySingleAsync<int>(value, param, input);
			_ = await query.QuerySingleAsync<int>(value, param, input, transaction);

			// Assert
			await db.Received().QuerySingleAsync<int>(value, param, input, Arg.Any<IDbTransaction>());
			await db.Received().QuerySingleAsync<int>(value, param, input, transaction);
		}

		[Fact]
		public async Task Calls_Db_QuerySingleAsync_As_Text()
		{
			// Arrange
			var (db, _, _, query) = DbQuery_Setup.Get();
			var value = F.Rnd.Str;
			var param = F.Rnd.Int;
			var transaction = Substitute.For<IDbTransaction>();

			// Act
			_ = await query.QuerySingleAsync<int>(value, param);
			_ = await query.QuerySingleAsync<int>(value, param, transaction);

			// Assert
			await db.Received().QuerySingleAsync<int>(value, param, CommandType.Text, Arg.Any<IDbTransaction>());
			await db.Received().QuerySingleAsync<int>(value, param, CommandType.Text, transaction);
		}

		[Fact]
		public async Task With_Parts_Calls_Client_GetQuery_And_Db_QuerySingleAsync()
		{
			// Arrange
			var value = F.Rnd.Str;
			var (parts, param) = DbQuery_Setup.GetParts();
			var (db, client, _, query) = DbQuery_Setup.Get(value, param);
			var transaction = Substitute.For<IDbTransaction>();

			// Act
			_ = await query.QuerySingleAsync<int>(parts);
			_ = await query.QuerySingleAsync<int>(parts, transaction);

			// Assert
			client.Received(2).GetQuery(parts);
			await db.Received().QuerySingleAsync<int>(value, param, CommandType.Text, Arg.Any<IDbTransaction>());
			await db.Received().QuerySingleAsync<int>(value, param, CommandType.Text, transaction);
		}
	}
}

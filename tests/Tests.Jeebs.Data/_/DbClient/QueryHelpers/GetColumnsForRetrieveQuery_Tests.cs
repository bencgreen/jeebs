﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Jeebs.Data.Mapping;
using NSubstitute;
using Xunit;

namespace Jeebs.Data.DbClient_Tests
{
	public class GetColumnsForRetrieveQuery_Tests
	{
		[Fact]
		public void No_Columns_Returns_Empty_List()
		{
			// Arrange
			var client = Substitute.ForPartsOf<DbClient>();
			var list = new ColumnList();

			// Act
			var col = client.GetColumnsForRetrieveQueryTest(list);

			// Assert
			Assert.Empty(col);
		}

		[Fact]
		public void Returns_Escaped_Column_Names()
		{
			// Arrange
			var client = Substitute.ForPartsOf<DbClient>();
			var column = new Column(F.Rnd.Str, F.Rnd.Str, F.Rnd.Str);
			var list = new ColumnList(new[] { column });

			// Act
			client.GetColumnsForRetrieveQueryTest(list);

			// Assert
			client.Received().Escape(column, true);
		}
	}
}

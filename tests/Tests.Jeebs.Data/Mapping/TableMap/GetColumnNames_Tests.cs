﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System.Reflection;
using NSubstitute;
using Xunit;

namespace Jeebs.Data.Mapping.TableMap_Tests
{
	public class GetColumnNames_Tests
	{
		internal static (string name, MappedColumn column) Get()
		{
			var name = F.Rnd.Str;
			var prop = Substitute.For<PropertyInfo>();
			prop.Name.Returns(F.Rnd.Str);
			return (name, new MappedColumn(F.Rnd.Str, name, prop));
		}

		[Fact]
		public void No_Columns_Returns_Empty_List()
		{
			// Arrange
			var table = Substitute.For<ITable>();
			var map = new TableMap(table, new MappedColumnList(), Get().column);

			// Act
			var result = map.GetColumnNames();

			// Assert
			Assert.Empty(result);
		}

		[Fact]
		public void Selects_Column_Names()
		{
			// Arrange
			var (n0, c0) = Get();
			var (n1, c1) = Get();
			var (n2, c2) = Get();
			var (n3, c3) = Get();
			var table = Substitute.For<ITable>();
			var map = new TableMap(table, new MappedColumnList(new[] { c0, c1, c2, c3 }), c0);

			// Act
			var result = map.GetColumnNames();

			// Assert
			Assert.Collection(result,
				x => Assert.Equal(n0, x),
				x => Assert.Equal(n1, x),
				x => Assert.Equal(n2, x),
				x => Assert.Equal(n3, x)
			);
		}
	}
}

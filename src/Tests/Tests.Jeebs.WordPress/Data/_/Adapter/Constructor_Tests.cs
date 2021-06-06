﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Xunit;
using static Jeebs.WordPress.Data.Adapter_Tests.Adapter;

namespace Jeebs.WordPress.Data.Adapter_Tests
{
	public class Constructor_Tests
	{
		[Fact]
		public void Properties_Set()
		{
			// Arrange
			var adapter = GetAdapter();

			// Act

			// Assert
			Assert.Equal(SchemaSeparator, adapter.SchemaSeparator);
			Assert.Equal(ColumnSeparator, adapter.ColumnSeparator);
			Assert.Equal(ListSeparator, adapter.ListSeparator);
			Assert.Equal(EscapeOpen, adapter.EscapeOpen);
			Assert.Equal(EscapeClose, adapter.EscapeClose);
			Assert.Equal(Alias, adapter.Alias);
			Assert.Equal(AliasOpen, adapter.AliasOpen);
			Assert.Equal(AliasClose, adapter.AliasClose);
			Assert.Equal(SortAsc, adapter.SortAsc);
			Assert.Equal(SortDesc, adapter.SortDesc);
		}
	}
}

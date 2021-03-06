﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Jeebs.Data;
using Jeebs.Data.Querying.QueryPartsBuilder_Tests;
using Jeebs.WordPress.Data.Entities;
using Jeebs.WordPress.Data.Tables;
using Xunit;
using static Jeebs.WordPress.Data.Query_Tests.PostsTaxonomyPartsBuilder_Tests.Setup;

namespace Jeebs.WordPress.Data.Query_Tests.PostsTaxonomyPartsBuilder_Tests
{
	public class IdColumn_Tests : QueryPartsBuilder_Tests<Query.PostsTaxonomyPartsBuilder, WpTermId>
	{
		protected override Query.PostsTaxonomyPartsBuilder GetConfiguredBuilder(IExtract extract) =>
			GetBuilder(extract);

		[Fact]
		public void Returns_Id_Column()
		{
			// Arrange
			var (builder, _) = Setup();

			// Act
			var result = builder.IdColumn;

			// Assert
			Assert.Equal(builder.TTest.Term.GetName(), result.Table);
			Assert.Equal(builder.TTest.Term.TermId, result.Name);
			Assert.Equal(nameof(TermTable.TermId), result.Alias);
		}
	}
}

﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using NSubstitute;
using Xunit;

namespace Jeebs.WordPress.Data.Querying.QueryWithParts_Tests
{
	public class Constructor_Tests
	{
		[Fact]
		public void Sets_UnitOfWork_And_Parts()
		{
			// Arrange
			var unitOfWork = Substitute.For<IUnitOfWork>();
			var parts = Substitute.For<IQueryParts>();

			// Act
			var result = new QueryBuilder<string>.QueryWithParts(unitOfWork, parts);

			// Assert
			Assert.Equal(unitOfWork, result.UnitOfWork);
			Assert.Equal(parts, result.Parts);
		}
	}
}
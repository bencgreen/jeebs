﻿using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using Xunit;

namespace Jeebs.Data.Querying.QueryBuilder_Tests
{
	public class WithModel_Tests
	{
		[Fact]
		public void Returns_New_QueryWithModel_With_UnitOfWork()
		{
			// Arrange
			var unitOfWork = Substitute.For<IUnitOfWork>();
			var builder = new QueryBuilder(unitOfWork);

			// Act
			var result = builder.WithModel<string>();

			// Assert
			var withModel = Assert.IsType<QueryBuilder<string>.QueryWithModel>(result);
			Assert.Equal(unitOfWork, withModel.UnitOfWork);
		}
	}
}

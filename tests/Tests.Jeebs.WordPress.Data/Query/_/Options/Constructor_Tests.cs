﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Jeebs.Data.Querying;
using NSubstitute;
using Xunit;

namespace Jeebs.WordPress.Data.Query_Tests.Options_Tests
{
	public class Constructor_Tests
	{
		[Fact]
		public void Sets_T()
		{
			// Arrange
			var schema = Substitute.For<IWpDbSchema>();
			var builder = Substitute.For<IQueryPartsBuilder<TestId>>();

			// Act
			var result = new TestOptions(schema, builder);

			// Assert
			Assert.Same(schema, result.TTest);
		}

		public sealed record TestId(ulong Value) : StrongId(Value);

		public sealed record TestOptions : Query.Options<TestId>
		{
			public TestOptions(IWpDbSchema schema, IQueryPartsBuilder<TestId> builder) : base(schema, builder) { }
		}
	}
}

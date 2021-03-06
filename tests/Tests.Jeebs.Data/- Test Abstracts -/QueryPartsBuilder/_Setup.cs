﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Jeebs.Data.Mapping;
using NSubstitute;

namespace Jeebs.Data.Querying.QueryPartsBuilder_Tests
{
	public abstract class QueryPartsBuilder_Tests<TBuilder, TId>
		where TBuilder : QueryPartsBuilder<TId>
		where TId : StrongId
	{
		protected abstract TBuilder GetConfiguredBuilder(IExtract extract);

		public (TBuilder builder, Vars v) Setup()
		{
			var extract = Substitute.For<IExtract>();

			var builder = GetConfiguredBuilder(extract);

			var parts = new QueryParts(builder.Table);

			return (builder, new(extract, parts));
		}

		public record Vars(
			IExtract Extract,
			QueryParts Parts
		);
	}

	public record TestTable0 : ITable
	{
		private readonly string name;

		public string Foo { get; init; }

		public TestTable0(string name, string foo) =>
			(this.name, Foo) = (name, foo);

		public string GetName() =>
			name;
	}

	public record TestTable1 : ITable
	{
		private readonly string name;

		public string Bar { get; init; }

		public TestTable1(string name, string bar) =>
			(this.name, Bar) = (name, bar);

		public string GetName() =>
			name;
	}
}

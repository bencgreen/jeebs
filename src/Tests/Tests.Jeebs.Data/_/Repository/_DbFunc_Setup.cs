﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Linq.Expressions;
using Jeebs.Data.Enums;
using NSubstitute;

namespace Jeebs.Data.Repository_Tests
{
	public static class Repository_Setup
	{
		public static (IDbClient client, ILog log, Repository<Foo, FooId> repo) Get()
		{
			var client = Substitute.For<IDbClient>();
			client
				.GetQuery<Foo, FooModel>(Arg.Any<(Expression<Func<Foo, object>>, SearchOperator, object)[]>())
				.Returns((F.Rnd.Str, Substitute.For<IQueryParameters>()));
			client.GetCreateQuery<Foo>().Returns(F.Rnd.Str);
			client.GetRetrieveQuery<Foo, FooModel>(Arg.Any<long>()).Returns(F.Rnd.Str);
			client.GetUpdateQuery<Foo, FooModel>(Arg.Any<long>()).Returns(F.Rnd.Str);
			client.GetDeleteQuery<Foo>(Arg.Any<long>()).Returns(F.Rnd.Str);

			var db = Substitute.For<IDb>();
			db.Client.Returns(client);

			var log = Substitute.For<ILog>();

			var repo = Substitute.ForPartsOf<Repository<Foo, FooId>>(db, log);

			return (client, log, repo);
		}

		public sealed record Foo : IEntity<FooId>
		{
			public FooId Id { get; init; } = new();
		}

		public sealed record FooModel : IWithId<FooId>
		{
			public FooId Id { get; init; } = new();
		}

		public sealed record FooId(long Value) : StrongId(Value)
		{
			public FooId() : this(0) { }
		}
	}
}
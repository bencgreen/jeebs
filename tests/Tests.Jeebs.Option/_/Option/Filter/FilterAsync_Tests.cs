﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace Jeebs.Option_Tests
{
	public class FilterAsync_Tests : Jeebs_Tests.FilterAsync_Tests
	{
		[Fact]
		public override async Task Test00_If_Unknown_Option_Returns_None_With_UnhandledExceptionMsg()
		{
			var syncPredicate = Substitute.For<Func<int, bool>>();
			var asyncPredicate = Substitute.For<Func<int, Task<bool>>>();

			await Test00(opt => opt.FilterAsync(syncPredicate));
			await Test00(opt => opt.FilterAsync(asyncPredicate));
		}

		[Fact]
		public override async Task Test01_Exception_Thrown_Returns_None_With_UnhandledExceptionMsg()
		{
			await Test01((opt, predicate) => opt.FilterAsync(x => predicate(x).GetAwaiter().GetResult()));
			await Test01((opt, predicate) => opt.FilterAsync(predicate));
		}

		[Fact]
		public override async Task Test02_When_Some_And_Predicate_True_Returns_Value()
		{
			await Test02((opt, predicate) => opt.FilterAsync(x => predicate(x).GetAwaiter().GetResult()));
			await Test02((opt, predicate) => opt.FilterAsync(predicate));
		}

		[Fact]
		public override async Task Test03_When_Some_And_Predicate_False_Returns_None_With_PredicateWasFalseMsg()
		{
			await Test03((opt, predicate) => opt.FilterAsync(x => predicate(x).GetAwaiter().GetResult()));
			await Test03((opt, predicate) => opt.FilterAsync(predicate));
		}

		[Fact]
		public override async Task Test04_When_None_Returns_None_With_Original_Reason()
		{
			await Test04((opt, predicate) => opt.FilterAsync(x => predicate(x).GetAwaiter().GetResult()));
			await Test04((opt, predicate) => opt.FilterAsync(predicate));
		}
	}
}

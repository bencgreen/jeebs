﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using NSubstitute;
using Xunit;

namespace Jeebs.Option_Tests
{
	public class Filter_Tests : Jeebs_Tests.Filter_Tests
	{
		[Fact]
		public override void Test00_If_Unknown_Option_Returns_None_With_UnhandledExceptionMsg()
		{
			var predicate = Substitute.For<Func<int, bool>>();
			Test00(opt => opt.Filter(predicate));
		}

		[Fact]
		public override void Test01_Exception_Thrown_Returns_None_With_UnhandledExceptionMsg()
		{
			Test01((opt, predicate) => opt.Filter(predicate));
		}

		[Fact]
		public override void Test02_When_Some_And_Predicate_True_Returns_Value()
		{
			Test02((opt, predicate) => opt.Filter(predicate));
		}

		[Fact]
		public override void Test03_When_Some_And_Predicate_False_Returns_None_With_PredicateWasFalseMsg()
		{
			Test03((opt, predicate) => opt.Filter(predicate));
		}

		[Fact]
		public override void Test04_When_None_Returns_None_With_Original_Reason()
		{
			Test04((opt, predicate) => opt.Filter(predicate));
		}
	}
}

﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using NSubstitute;
using Xunit;

namespace Jeebs.Option_Tests
{
	public class Audit_Tests : Jeebs_Tests.Audit_Tests
	{
		#region General

		[Fact]
		public override void Test01_If_Unknown_Option_Throws_UnknownOptionException()
		{
			Test01(opt => opt.Audit(Substitute.For<Action<Option<int>>>()));
			Test01(opt => opt.Audit(Substitute.For<Action<int>>()));
			Test01(opt => opt.Audit(Substitute.For<Action<IMsg>>()));
			Test01(opt => opt.Audit(Substitute.For<Action<int>>(), Substitute.For<Action<IMsg>>()));
		}

		#endregion

		#region Any

		[Fact]
		public override void Test02_Some_Runs_Audit_And_Returns_Original_Option()
		{
			Test02((opt, any) => opt.Audit(any));
		}

		[Fact]
		public override void Test03_None_Runs_Audit_And_Returns_Original_Option()
		{
			Test03((opt, any) => opt.Audit(any));
		}

		[Fact]
		public override void Test04_Some_Catches_Exception_And_Returns_Original_Option()
		{
			Test04((opt, any) => opt.Audit(any));
		}

		[Fact]
		public override void Test05_None_Catches_Exception_And_Returns_Original_Option()
		{
			Test05((opt, any) => opt.Audit(any));
		}

		#endregion

		#region Some / None

		[Fact]
		public override void Test06_Some_Runs_Some_And_Returns_Original_Option()
		{
			Test06((opt, some) => opt.Audit(some));
			Test06((opt, some) => opt.Audit(some, Substitute.For<Action<IMsg>>()));
		}

		[Fact]

		public override void Test07_None_Runs_None_And_Returns_Original_Option()
		{
			Test07((opt, none) => opt.Audit(none));
			Test07((opt, none) => opt.Audit(Substitute.For<Action<int>>(), none));
		}

		[Fact]
		public override void Test08_Some_Catches_Exception_And_Returns_Original_Option()
		{
			Test08((opt, some) => opt.Audit(some));
			Test08((opt, some) => opt.Audit(some, Substitute.For<Action<IMsg>>()));
		}

		[Fact]
		public override void Test09_None_Catches_Exception_And_Returns_Original_Option()
		{
			Test09((opt, none) => opt.Audit(none));
			Test09((opt, none) => opt.Audit(Substitute.For<Action<int>>(), none));
		}

		#endregion

		#region Unused

		[Fact]
		public override void Test00_Null_Args_Returns_Original_Option() { }

		#endregion
	}
}

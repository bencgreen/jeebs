﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using System.Threading.Tasks;
using Jeebs;
using NSubstitute;
using Xunit;
using static F.OptionF;

namespace F.OptionF_Tests
{
	public class SwitchIfAsync_Tests : Jeebs_Tests.SwitchIfAsync_Tests
	{
		[Fact]
		public override async Task Test00_Unknown_Option_Throws_UnknownOptionException()
		{
			var ifFalse = Substitute.For<Func<int, IMsg>>();
			await Test00((opt, check) => SwitchIfAsync(opt, check, null, null));
			await Test00((opt, check) => SwitchIfAsync(opt, check, ifFalse));
		}

		[Fact]
		public override async Task Test01_None_Returns_Original_None()
		{
			var ifFalse = Substitute.For<Func<int, IMsg>>();
			await Test01((opt, check) => SwitchIfAsync(opt, check, null, null));
			await Test01((opt, check) => SwitchIfAsync(opt, check, ifFalse));
		}

		[Fact]
		public override async Task Test02_Check_Func_Throws_Exception_Returns_None_With_SwitchIfFuncExceptionMsg()
		{
			var ifFalse = Substitute.For<Func<int, IMsg>>();
			await Test02((opt, check) => SwitchIfAsync(opt, check, null, null));
			await Test02((opt, check) => SwitchIfAsync(opt, check, ifFalse));
		}

		[Fact]
		public override async Task Test03_Check_Returns_True_And_IfTrue_Is_Null_Returns_Original_Option()
		{
			await Test03((opt, check) => SwitchIfAsync(opt, check, null, null));
		}

		[Fact]
		public override async Task Test04_Check_Returns_False_And_IfFalse_Is_Null_Returns_Original_Option()
		{
			await Test04((opt, check) => SwitchIfAsync(opt, check, null, null));
		}

		[Fact]
		public override async Task Test05_Check_Returns_True_And_IfTrue_Throws_Exception_Returns_None_With_SwitchIfFuncExceptionMsg()
		{
			await Test05((opt, check, ifTrue) => SwitchIfAsync(opt, check, ifTrue, null));
		}

		[Fact]
		public override async Task Test06_Check_Returns_False_And_IfFalse_Throws_Exception_Returns_None_With_SwitchIfFuncExceptionMsg()
		{
			await Test06((opt, check, ifFalse) => SwitchIfAsync(opt, check, null, ifFalse));
			await Test06((opt, check, ifFalse) => SwitchIfAsync(opt, check, x => ifFalse(x).Reason));
		}

		[Fact]
		public override async Task Test07_Check_Returns_True_Runs_IfTrue_Returns_Value()
		{
			await Test07((opt, check, ifTrue) => SwitchIfAsync(opt, check, ifTrue, null));
		}

		[Fact]
		public override async Task Test08_Check_Returns_False_Runs_IfFalse_Returns_Value()
		{
			await Test08((opt, check, ifFalse) => SwitchIfAsync(opt, check, null, ifFalse));
			await Test08((opt, check, ifFalse) => SwitchIfAsync(opt, check, x => ifFalse(x).Reason));
		}
	}
}

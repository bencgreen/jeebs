﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System.Threading.Tasks;
using Xunit;
using static F.OptionF;

namespace F.OptionF_Tests
{
	public class UnwrapSingleAsync_Tests : Jeebs_Tests.UnwrapSingleAsync_Tests
	{
		[Fact]
		public override async Task Test00_If_Unknown_Option_Returns_None_With_UnhandledExceptionMsg()
		{
			await Test00(opt => UnwrapAsync(opt, x => x.Single<int>(null, null, null)));
		}

		[Fact]
		public override async Task Test01_None_Returns_None()
		{
			await Test01(opt => UnwrapAsync(opt, x => x.Single<int>(null, null, null)));
		}

		[Fact]
		public override async Task Test02_None_With_Reason_Returns_None_With_Reason()
		{
			await Test02(opt => UnwrapAsync(opt, x => x.Single<int>(null, null, null)));
		}

		[Fact]
		public override async Task Test03_No_Items_Returns_None_With_UnwrapSingleNoItemsMsg()
		{
			await Test03(opt => UnwrapAsync(opt, x => x.Single<int>(null, null, null)));
		}

		[Fact]
		public override async Task Test04_No_Items_Runs_NoItems()
		{
			await Test04((opt, noItems) => UnwrapAsync(opt, x => x.Single<int>(noItems, null, null)));
		}

		[Fact]
		public override async Task Test05_Too_Many_Items_Returns_None_With_UnwrapSingleTooManyItemsErrorMsg()
		{
			await Test05(opt => UnwrapAsync(opt, x => x.Single<int>(null, null, null)));
		}

		[Fact]
		public override async Task Test06_Too_Many_Items_Runs_TooMany()
		{
			await Test06((opt, tooMany) => UnwrapAsync(opt, x => x.Single<int>(null, tooMany, null)));
		}

		[Fact]
		public override async Task Test07_Not_A_List_Returns_None_With_UnwrapSingleNotAListMsg()
		{
			await Test07(opt => UnwrapAsync(opt, x => x.Single<int>(null, null, null)));
		}

		[Fact]
		public override async Task Test08_Not_A_List_Runs_NotAList()
		{
			await Test08((opt, notAList) => UnwrapAsync(opt, x => x.Single<int>(null, null, notAList)));
		}

		[Fact]
		public override async Task Test09_Incorrect_Type_Returns_None_With_UnwrapSingleIncorrectTypeErrorMsg()
		{
			await Test09(opt => UnwrapAsync(opt, x => x.Single<string>(null, null, null)));
		}

		[Fact]
		public override async Task Test10_List_With_Single_Item_Returns_Single()
		{
			await Test10(opt => UnwrapAsync(opt, x => x.Single<int>(null, null, null)));
		}
	}
}

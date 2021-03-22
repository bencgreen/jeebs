﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System.Threading.Tasks;
using Xunit;

namespace Jeebs.OptionExtensions_Tests
{
	public class AuditAsync_Tests : Jeebs_Tests.AuditAsync_Tests
	{
		[Fact]
		public override async Task Test00_Some_Runs_Audit_Action_And_Returns_Original_Option()
		{
			await Test00((some, audit) => some.AsTask.AuditAsync(audit));
		}

		[Fact]
		public override async Task Test01_None_Runs_Audit_Action_And_Returns_Original_Option()
		{
			await Test01((none, audit) => none.AsTask.AuditAsync(audit));
		}

		[Fact]
		public override async Task Test02_Some_Runs_Audit_Func_And_Returns_Original_Option()
		{
			await Test02((some, audit) => some.AsTask.AuditAsync(audit));
		}

		[Fact]
		public override async Task Test03_None_Runs_Audit_Func_And_Returns_Original_Option()
		{
			await Test03((none, audit) => none.AsTask.AuditAsync(audit));
		}

		[Fact]
		public override async Task Test04_Some_Runs_Audit_Action_Catches_Exception_And_Returns_Original_Option()
		{
			await Test04((some, audit) => some.AsTask.AuditAsync(audit));
		}

		[Fact]
		public override async Task Test05_None_Runs_Audit_Action_Catches_Exception_And_Returns_Original_Option()
		{
			await Test05((none, audit) => none.AsTask.AuditAsync(audit));
		}

		[Fact]
		public override async Task Test06_Some_Runs_Audit_Func_Catches_Exception_And_Returns_Original_Option()
		{
			await Test06((some, audit) => some.AsTask.AuditAsync(audit));
		}

		[Fact]
		public override async Task Test07_None_Runs_Audit_Func_Catches_Exception_And_Returns_Original_Option()
		{
			await Test07((none, audit) => none.AsTask.AuditAsync(audit));
		}
	}
}

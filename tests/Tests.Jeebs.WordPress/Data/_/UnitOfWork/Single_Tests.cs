﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System.Data;
using Xunit;
using static Jeebs.WordPress.Data.UnitOfWork_Tests.UnitOfWork;

namespace Jeebs.WordPress.Data.UnitOfWork_Tests
{
	public class Single_Tests
	{
		[Theory]
		[InlineData(CommandType.StoredProcedure)]
		[InlineData(CommandType.Text)]
		public void Logs_Query(CommandType commandType)
		{
			LogsQuery(
				w => w.Single<int>,
				commandType
			);
		}

		[Theory]
		[InlineData(CommandType.StoredProcedure)]
		[InlineData(CommandType.Text)]
		public void Calls_Driver_QuerySingle(CommandType commandType)
		{
			CallsDriver<int>(
				w => w.Single<int>,
				d => d.QuerySingle<int>,
				commandType
			);
		}

		[Theory]
		[InlineData(CommandType.StoredProcedure)]
		[InlineData(CommandType.Text)]
		public void Catches_Exception_Rolls_Back_Logs_Exception_Returns_Error(CommandType commandType)
		{
			HandlesFailure<int>(
				w => w.Single<int>,
				commandType
			);
		}
	}
}
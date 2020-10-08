﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;
using static Jeebs.Data.UnitOfWork_Tests.UnitOfWork;

namespace Jeebs.Data.UnitOfWork_Tests
{
	public class QueryDynamicAsync_Tests
	{
		[Theory]
		[InlineData(CommandType.StoredProcedure)]
		[InlineData(CommandType.Text)]
		public void Logs_Query(CommandType commandType)
		{
			QueryMethodIsLogged(
				w => w.Query,
				commandType
			);
		}

		[Theory]
		[InlineData(CommandType.StoredProcedure)]
		[InlineData(CommandType.Text)]
		public void Calls_Driver_Query(CommandType commandType)
		{
			QueryMethodCallsDriver<IEnumerable<dynamic>>(
				w => w.Query,
				d => d.Query,
				commandType
			);
		}
	}
}

﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using Jeebs.Data.Mapping;
using NSubstitute;
using Xunit;

namespace Jeebs.WordPress.Data.Query_Tests.Options_Tests
{
	public class Escape_Tests : Options_Tests
	{
		[Fact]
		public void Escape_Table_Calls_Client_Escape()
		{
			// Arrange
			var (options, v) = Setup();

			// Act
			_ = options.EscapeTest(v.Table);

			// Assert
			v.DbClient.Received().Escape(v.Table);
		}

		[Fact]
		public void Escape_Column_Calls_Client_Escape()
		{
			// Arrange
			var (options, v) = Setup();

			// Act
			_ = options.EscapeTest(v.Table, t => t.Id);

			// Assert
			v.DbClient.Received().EscapeWithTable(Arg.Any<IColumn>());
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs.RExtensions_Tests
{
	public class Select_Tests
	{
		[Fact]
		public void Linq_Select_With_OkV_Returns_OkV()
		{
			// Arrange
			const int value = 18;
			var result = Result.OkV(value);

			// Act
			var next = from a in result
					   select a ^ 2;

			// Assert
			var okV = Assert.IsAssignableFrom<IOkV<int>>(next);
			Assert.Equal(value ^ 2, okV.Value);
		}

		[Fact]
		public void Linq_Select_With_Ok_Returns_Error()
		{
			// Arrange
			var result = Result.Ok<int>();

			// Act
			var next = from a in result
					   select a ^ 2;

			// Assert
			Assert.IsAssignableFrom<IError<int>>(next);
		}

		[Fact]
		public void Linq_Select_With_Error_Returns_Error()
		{
			// Arrange
			var result = Result.Error<int>().AddMsg().OfType<InvalidIntegerMsg>();

			// Act
			var next = from a in result
					   select a ^ 2;

			// Assert
			var error = Assert.IsAssignableFrom<IError<int>>(next);
			Assert.Contains(error.Messages, m => m is InvalidIntegerMsg);
		}

		public class InvalidIntegerMsg : IMsg { }
	}
}
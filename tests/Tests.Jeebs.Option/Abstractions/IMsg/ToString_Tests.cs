﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Xunit;

namespace Jeebs.OptionAbstractions.IMsg_Tests
{
	public class ToString_Tests
	{
		[Fact]
		public void Returns_Type_Name()
		{
			// Arrange
			var message = new TestMsg();

			// Act
			var result = message.ToString();

			// Assert
			Assert.Equal($"{typeof(TestMsg).Name} {{ }}", result);
		}

		public record TestMsg : IMsg { }
	}
}

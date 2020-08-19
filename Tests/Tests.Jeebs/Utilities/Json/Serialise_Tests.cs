﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace Jeebs.Util.Json_Tests
{
	public partial class Serialise_Tests
	{
		[Theory]
		[InlineData(null)]
		public void Null_ReturnsEmpty(object input)
		{
			// Arrange

			// Act
			var result = Json.Serialise(input);

			// Assert
			Assert.Equal(Json.Empty, result);
		}

		[Fact]
		public void Object_ReturnsJson()
		{
			// Arrange
			var input = new Test { Foo = "test", Bar = 2 };
			const string expected = "{\"foo\":\"test\",\"bar\":2}";

			// Act
			var result = Json.Serialise(input);

			// Assert
			Assert.Equal(expected, result);
		}

		public class Test
		{
			public string Foo { get; set; } = string.Empty;

			public int Bar { get; set; }

			public string? Empty { get; set; }
		}
	}
}

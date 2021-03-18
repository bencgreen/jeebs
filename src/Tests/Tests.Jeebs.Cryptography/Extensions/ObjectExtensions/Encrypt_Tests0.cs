﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using Xunit;
using static F.CryptoF;
using static F.JsonF;

namespace Jeebs.Cryptography.ObjectExtensions_Tests
{
	public partial class Encrypt_Tests
	{
		private readonly string defaultInputString = "String to encrypt.";
		private readonly Foo defaultInputObject = new() { Bar = "Test string." };

		[Theory]
		[InlineData(null)]
		public void Null_Input_Byte_Key_Returns_Empty(string input)
		{
			// Arrange
			var key = GenerateKey();

			// Act
			var result = input.Encrypt(key);

			// Assert
			Assert.Equal(Empty, result);
		}

		[Fact]
		public void String_Input_Byte_Key_Returns_Encrypted_Json()
		{
			// Arrange
			var key = GenerateKey();

			// Act
			var result = defaultInputString.Encrypt(key);

			// Assert
			Assert.NotEqual(defaultInputString, result);
		}

		[Fact]
		public void Object_Input_Byte_Key_Returns_Encrypted_Json()
		{
			// Arrange
			var key = GenerateKey();

			// Act
			var json = Serialise(defaultInputObject);
			var result = defaultInputObject.Encrypt(key);

			// Assert
			Assert.NotEqual(json, result);
		}

		public class Foo
		{
			public string Bar { get; set; } = string.Empty;
		}
	}
}
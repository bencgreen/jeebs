﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs.Cryptography.StringExtensions_Tests
{
	public sealed class VerifyPassword_Tests
	{
		private readonly string password = "Password to hash.";
		private readonly string passwordHash = "$argon2id$v=19$m=131072,t=6,p=1$hlK04aWKBnSzi2Sx2T3f7Q$YP80Zmw9Ef0oixaawEGtrJtDszIs2VeoCbH0Qmc+fUk\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0";

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void VerifyPassword_NullOrEmpty_ThrowsArgumentNullException(string input)
		{
			// Arrange
			var password = this.password;

			// Act
			Action result = () => input.VerifyPassword(password);

			// Assert
			Assert.Throws<ArgumentNullException>(result);
		}

		[Fact]
		public void VerifyPassword_IncorrectPassword_ReturnsFalse()
		{
			// Arrange
			var password = F.StringF.Random(10);
			var passwordHash = this.passwordHash;

			// Act
			var result = passwordHash.VerifyPassword(password);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public void VerifyPassword_CorrectPassword_ReturnsTrue()
		{
			// Arrange
			var password = this.password;
			var passwordHash = this.passwordHash;

			// Act
			var result = passwordHash.VerifyPassword(password);

			// Assert
			Assert.True(result);
		}
	}
}
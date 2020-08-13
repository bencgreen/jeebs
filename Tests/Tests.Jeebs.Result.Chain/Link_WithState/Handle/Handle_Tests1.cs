﻿using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using Xunit;

namespace Jeebs.LinkTests.WithState
{
	public partial class Handle_Tests
	{
		[Fact]
		public void Specific_Handler_Runs_For_That_Exception()
		{
			// Arrange
			const int state = 7;
			var chain = Chain.Create(state);
			var sideEffect = 1;
			void h0(IR<bool> _, DivideByZeroException __) => sideEffect++;
			void h1(IR<bool, int> _, DivideByZeroException __) => sideEffect++;
			static void throwException() => throw new DivideByZeroException();

			// Act
			chain.Link().Handle<DivideByZeroException>().With(h0).Run(throwException);
			chain.Link().Handle<DivideByZeroException>().With(h1).Run(throwException);

			// Assert
			Assert.Equal(3, sideEffect);
		}

		[Fact]
		public void Specific_Handler_Does_Not_Run_For_Other_Exceptions()
		{
			// Arrange
			const int state = 7;
			var chain = Chain.Create(state);
			var sideEffect = 1;
			void h0(IR<bool> _, DivideByZeroException __) => sideEffect++;
			void h1(IR<bool, int> _, DivideByZeroException __) => sideEffect++;
			static void throwException() => throw new ArithmeticException();

			// Act
			chain.Link().Handle<DivideByZeroException>().With(h0).Run(throwException);
			chain.Link().Handle<DivideByZeroException>().With(h1).Run(throwException);

			// Assert
			Assert.Equal(1, sideEffect);
		}
	}
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Jeebs.UnwrapTests.WithState
{
	public partial class UnwrapTests
	{
		[Fact]
		public void Not_IEnumerable_Or_Same_Type_Input_Returns_IError()
		{
			// Arrange
			const int value = 18;
			const int state = 7;
			var chain = Chain.CreateV(value, state);

			// Act
			var result = chain.Link().Unwrap<string>();
			var msg = result.Messages.Get<Jm.Link.Single.NotIEnumerableMsg>();

			// Assert
			var e = Assert.IsAssignableFrom<IError<string, int>>(result);
			Assert.Equal(state, e.State);
			Assert.NotEmpty(msg);
		}
	}
}
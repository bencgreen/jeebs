﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Jeebs.Link_Tests
{
	public partial class UnwrapSingle_Tests
	{
		[Fact]
		public void Not_IEnumerable_Or_Same_Type_Input_Returns_IError()
		{
			// Arrange
			var value = F.Rnd.Int;
			var chain = Chain.CreateV(value);

			// Act
			var result = chain.Link().UnwrapSingle<string>();
			var msg = result.Messages.Get<Jm.Link.Unwrap.NotIEnumerableMsg>();

			// Assert
			Assert.IsAssignableFrom<IError<string>>(result);
			Assert.NotEmpty(msg);
		}
	}
}

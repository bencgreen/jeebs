﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Jeebs.MsgList_Tests
{
	public class ToString_Tests
	{
		[Fact]
		public void Returns_TypeName_When_Empty()
		{
			// Arrange
			var l = new MsgList();
			var t = typeof(MsgList).FullName;

			// Act

			// Assert
			Assert.Equal(t, l.ToString());
		}

		[Fact]
		public void Returns_Message_Strings_On_NewLines()
		{
			// Arrange
			var l = new MsgList();

			var m0 = new StringMsg("zero");
			var m1 = new StringMsg("one");
			const string str = "zero\none";

			// Act
			l.AddRange(m0, m1);

			// Assert
			Assert.Equal(str, l.ToString());
		}

		public class StringMsg : Jm.WithValueMsg<string>
		{
			public StringMsg(string value) : base(value) { }
		}
	}
}
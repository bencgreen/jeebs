﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Jeebs.MsgList_Tests
{
	public class AddRange_Tests
	{
		[Fact]
		public void Does_Nothing_When_No_Messages_Given()
		{
			// Arrange
			var l = new MsgList();

			// Act
			var exception = Record.Exception(() => l.AddRange());

			// Assert
			Assert.Null(exception);
			Assert.Equal(0, l.Count);
		}

		[Fact]
		public void Adds_Messages_To_List()
		{
			// Arrange
			var l = new MsgList();

			var m0 = new StringMsg("zero");
			var m1 = new StringMsg("one");
			var ms = new List<string>(new[] { "zero", "one" });

			// Act
			l.AddRange(m0, m1);

			// Assert
			Assert.Equal(2, l.Count);
			Assert.Equal(ms, l.GetAll());
		}

		public class StringMsg : Jm.WithValueMsg<string>
		{
			public StringMsg(string value) : base(value) { }
		}
	}
}

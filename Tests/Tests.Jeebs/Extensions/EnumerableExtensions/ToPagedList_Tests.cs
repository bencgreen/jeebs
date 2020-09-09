﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs.EnumerableExtensions_Tests
{
	public class ToPagedList_Tests
	{
		[Fact]
		public void Empty_ReturnsEmpty()
		{
			// Arrange
			var list = new List<string>();

			// Act
			var newList = list.ToPagedList(1);

			// Assert
			Assert.Empty(newList);
		}

		[Fact]
		public void ReturnsValuesAndItems()
		{
			// Arrange
			var items = new List<string>();
			for (int i = 1; i <= 25; i++)
			{
				items.Add($"Item {i}");
			}
			var list = new List<string>(items);

			// Act
			var newList = list.ToPagedList(3, 5);

			// Assert
			Assert.Equal(3, newList.Values.Page);
			Assert.Equal(5, newList.Values.Pages);
			Assert.Equal(25, newList.Values.Items);
			Assert.Equal(5, newList.Count);
			Assert.Equal("Item 11", newList[0]);
			Assert.Equal("Item 15", newList[4]);
		}
	}
}
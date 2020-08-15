﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Jeebs.PagingValues_Tests
{
	public class Constructor_Tests
	{
		[Fact]
		public void InvalidPage_ThrowsInvalidException()
		{
			// Arrange
			const int invalidPage = -1;

			// Act
			Action createPagingValuesWithInvalidPage = () => new PagingValues(items: 0, page: invalidPage);

			// Assert
			Assert.Throws<InvalidOperationException>(createPagingValuesWithInvalidPage);
		}

		[Fact]
		public void NoPageOrItems_CurrentPage_Equals_1()
		{
			// Arrange
			var pageNoItems = new PagingValues(items: 0, page: 1);
			var itemsNoPage = new PagingValues(items: 1, page: 0);

			// Act

			// Assert
			Assert.Equal(1, pageNoItems.Page);
			Assert.Equal(1, itemsNoPage.Page);
		}

		[Theory]
		[InlineData(1, 1, 10)]
		[InlineData(2, 11, 20)]
		[InlineData(3, 21, 25)]
		public void FirstAndLastItems(int page, int firstItem, int lastItem)
		{
			// Arrange
			var values = new PagingValues(items: 25, page: page);

			// Act

			// Assert
			Assert.Equal(firstItem, values.FirstItem);
			Assert.Equal(lastItem, values.LastItem);
		}

		[Theory]
		[InlineData(2, 25, 1, 3)]
		[InlineData(4, 478, 1, 10)]
		[InlineData(17, 478, 11, 20)]
		[InlineData(45, 478, 41, 48)]
		public void UpperAndLowerPages(int page, int items, int lowerPage, int upperPage)
		{
			// Arrange
			var values = new PagingValues(items: items, page: page);

			// Act

			// Assert
			Assert.Equal(lowerPage, values.LowerPage);
			Assert.Equal(upperPage, values.UpperPage);
		}
	}
}
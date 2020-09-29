﻿using System;
using System.Collections.Generic;
using System.Text;
using Jx.Data.Mapping;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;

namespace Jeebs.Data.Mapping.Map_Tests
{
	public class To_Tests
	{
		[Fact]
		public void Checks_If_Entity_Already_Mapped()
		{
			// Arrange
			var maps = Substitute.For<ITableMaps>();
			maps.Exists<Foo>().Returns(true);
			var table = new FooTable();

			// Act
			Map<Foo>.To(table, maps);

			// Assert
			maps.Received().Exists<Foo>();
		}

		[Fact]
		public void Missing_Id_Property_Attribute_Throws_MissingAttributeException()
		{
			// Arrange
			var maps = Substitute.For<ITableMaps>();
			var table = new FooTable();

			// Act
			void action() => Map<FooWithoutIdAttribute>.To(table, maps);

			// Assert
			var ex = Assert.Throws<MissingAttributeException>(action);
			Assert.Equal(string.Format(MissingAttributeException.Format, typeof(FooWithoutIdAttribute), "Id"), ex.Message);
		}

		[Fact]
		public void Multiple_Id_Properties_Throws_MultipleAttributesException()
		{
			// Arrange
			var maps = Substitute.For<ITableMaps>();
			var table = new FooTable();

			// Act
			void action() => Map<FooWithMultipleIdAttributes>.To(table, maps);

			// Assert
			var ex = Assert.Throws<MultipleAttributesException>(action);
			Assert.Equal(string.Format(MultipleAttributesException.Format, typeof(FooWithMultipleIdAttributes), "Id"), ex.Message);
		}

		[Fact]
		public void Missing_Version_Property_Attribute_Throws_MissingAttributeException()
		{
			// Arrange
			var maps = Substitute.For<ITableMaps>();
			var table = new FooWithVersionTable();

			// Act
			void action() => Map<FooWithoutVersionAttribute>.To(table, maps);

			// Assert
			var ex = Assert.Throws<MissingAttributeException>(action);
			Assert.Equal(string.Format(MissingAttributeException.Format, typeof(FooWithoutVersionAttribute), "Version"), ex.Message);
		}

		[Fact]
		public void Multiple_Version_Properties_Throws_MultipleAttributesException()
		{
			// Arrange
			var maps = Substitute.For<ITableMaps>();
			var table = new FooWithVersionTable();

			// Act
			void action() => Map<FooWithMultipleVersionAttributes>.To(table, maps);

			// Assert
			var ex = Assert.Throws<MultipleAttributesException>(action);
			Assert.Equal(string.Format(MultipleAttributesException.Format, typeof(FooWithMultipleVersionAttributes), "Version"), ex.Message);
		}

		[Fact]
		public void TryAdd_TableMap_To_TableMaps()
		{
			// Arrange
			var maps = Substitute.For<ITableMaps>();
			var table = new FooWithVersionTable();

			// Act
			Map<FooWithVersion>.To(table, maps);

			// Assert
			maps.Received().TryAdd<FooWithVersion>(
				Arg.Is<TableMap>(x =>
					x.Name == table.ToString()
					&& x.IdColumn.Name == table.Id
					&& x.IdColumn.Alias == nameof(table.Id)
					&& x.VersionColumn != null
					&& x.VersionColumn.Name == table.Version
					&& x.VersionColumn.Alias == nameof(table.Version)
				)
			);
		}
	}
}

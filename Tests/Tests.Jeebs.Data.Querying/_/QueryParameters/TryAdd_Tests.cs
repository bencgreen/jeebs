﻿using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using Xunit;

namespace Jeebs.Data.Querying.QueryParameters_Tests
{
	public class TryAdd_Tests
	{
		[Theory]
		[InlineData(null)]
		[InlineData(42)]
		[InlineData(true)]
		[InlineData('c')]
		public void Ignores_Null_And_Primitive_Types(object input)
		{
			// Arrange
			var parameters = new QueryParameters();

			// Act
			var result = parameters.TryAdd(input);

			// Assert
			Assert.False(result);
			Assert.Empty(parameters);
		}

		[Fact]
		public void Adds_QueryParameters_To_Dictionary()
		{
			// Arrange
			var p0 = F.Rnd.Str;
			var p1 = F.Rnd.Str;
			var p2 = F.Rnd.Str;
			var parametersToAdd = new QueryParameters
			{
				{ nameof(p0), p0 },
				{ nameof(p1), p1 },
				{ nameof(p2), p2 }
			};

			var parameters = new QueryParameters();

			// Act
			var result = parameters.TryAdd(parametersToAdd);

			// Assert
			Assert.True(result);
			Assert.Collection(parameters,
				x => { Assert.Equal(nameof(p0), x.Key); Assert.Equal(p0, x.Value); },
				x => { Assert.Equal(nameof(p1), x.Key); Assert.Equal(p1, x.Value); },
				x => { Assert.Equal(nameof(p2), x.Key); Assert.Equal(p2, x.Value); }
			);
		}

		[Fact]
		public void Adds_AnonymousType_To_Dictionary()
		{
			// Arrange
			var p0 = F.Rnd.Str;
			var p1 = F.Rnd.Str;
			var p2 = F.Rnd.Str;
			var parametersToAdd = new { p0, p1, p2 };

			var parameters = new QueryParameters();

			// Act
			var result = parameters.TryAdd(parametersToAdd);

			// Assert
			Assert.True(result);
			Assert.Collection(parameters,
				x => { Assert.Equal(nameof(p0), x.Key); Assert.Equal(p0, x.Value); },
				x => { Assert.Equal(nameof(p1), x.Key); Assert.Equal(p1, x.Value); },
				x => { Assert.Equal(nameof(p2), x.Key); Assert.Equal(p2, x.Value); }
			);
		}

		[Fact]
		public void Adds_Type_With_Public_Properties_To_Dictionary()
		{
			// Arrange
			var parameters = new QueryParameters();
			var foo = new Foo();

			// Act
			var result = parameters.TryAdd(foo);

			// Assert
			Assert.True(result);
			Assert.Collection(parameters,
				x => { Assert.Equal(nameof(Foo.Bar0), x.Key); Assert.Equal(42, x.Value); }
			);
		}

		public class Foo
		{
			public int Bar0 { get; set; } = 42;

			public string Bar1 { private get; set; } = F.Rnd.Str;

			public readonly bool Bar2 = false;
		}
	}
}

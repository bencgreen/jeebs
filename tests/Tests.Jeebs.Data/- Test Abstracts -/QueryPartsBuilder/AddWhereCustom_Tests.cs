﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System.Collections.Generic;
using Xunit;
using static Jeebs.Data.Querying.QueryPartsBuilder.Msg;

namespace Jeebs.Data.Querying.QueryPartsBuilder_Tests
{
	public abstract class AddWhereCustom_Tests<TBuilder, TId> : QueryPartsBuilder_Tests<TBuilder, TId>
		where TBuilder : QueryPartsBuilder<TId>
		where TId : StrongId
	{
		public abstract void Test00_Clause_Null_Or_Empty_Returns_None_With_TryingToAddEmptyClauseToWhereCustomMsg(string input);

		public static IEnumerable<string?[]> Test00_Data()
		{
			yield return new string?[] { null };
			yield return new string[] { "" };
			yield return new string[] { " " };
		}

		protected void Test00(string input)
		{
			// Arrange
			var (builder, v) = Setup();

			// Act
			var result = builder.AddWhereCustom(v.Parts, input, new());

			// Assert
			var none = result.AssertNone();
			Assert.IsType<TryingToAddEmptyClauseToWhereCustomMsg>(none);
		}

		public abstract void Test01_Invalid_Parameters_Returns_None_With_UnableToAddParametersToWhereCustomMsg(object input);

		public static IEnumerable<object?[]> Test01_Data()
		{
			yield return new object?[] { null };
			yield return new object[] { 42 };
			yield return new object[] { true };
			yield return new object[] { 'c' };
		}

		protected void Test01(object input)
		{
			// Arrange
			var (builder, v) = Setup();
			var clause = F.Rnd.Str;

			// Act
			var result = builder.AddWhereCustom(v.Parts, clause, input);

			// Assert
			var none = result.AssertNone();
			Assert.IsType<UnableToAddParametersToWhereCustomMsg>(none);
		}

		public abstract void Test02_Returns_New_Parts_With_Clause_And_Parameters();

		protected void Test02()
		{
			// Arrange
			var (builder, v) = Setup();
			var clause = F.Rnd.Str;
			var value = F.Rnd.Lng;
			var parameters = new { value };

			// Act
			var result = builder.AddWhereCustom(v.Parts, clause, parameters);

			// Assert
			var some = result.AssertSome();
			Assert.NotSame(v.Parts, some);
			Assert.Collection(some.WhereCustom,
				x =>
				{
					Assert.Equal(clause, x.clause);
					Assert.Collection(x.parameters,
						y =>
						{
							Assert.Equal(nameof(value), y.Key);
							Assert.Equal(value, y.Value);
						}
					);
				}
			);
		}
	}
}

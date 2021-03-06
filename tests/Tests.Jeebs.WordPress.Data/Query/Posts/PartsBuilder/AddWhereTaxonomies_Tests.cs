﻿// Jeebs Unit Tests
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Jeebs.Data;
using Jeebs.Data.Querying.QueryPartsBuilder_Tests;
using Jeebs.WordPress.Data.Entities;
using Jeebs.WordPress.Data.Enums;
using NSubstitute;
using Xunit;
using static Jeebs.WordPress.Data.Query_Tests.PostsPartsBuilder_Tests.Setup;

namespace Jeebs.WordPress.Data.Query_Tests.PostsPartsBuilder_Tests
{
	public class AddWhereTaxonomies_Tests : QueryPartsBuilder_Tests<Query.PostsPartsBuilder, WpPostId>
	{
		protected override Query.PostsPartsBuilder GetConfiguredBuilder(IExtract extract) =>
			GetBuilder(extract);

		[Fact]
		public void No_Taxonomies_Does_Nothing()
		{
			// Arrange
			var (builder, v) = Setup();

			// Act
			var result = builder.AddWhereTaxonomies(v.Parts, Substitute.For<IImmutableList<(Taxonomy, WpTermId)>>());

			// Assert
			var some = result.AssertSome();
			Assert.Same(v.Parts, some);
		}

		[Fact]
		public void Single_Taxonomy_Adds_Taxonomy_Clause()
		{
			// Arrange
			var (builder, v) = Setup();
			var taxonomy = Taxonomy.PostCategory;
			var id = new WpTermId(F.Rnd.Ulng);
			var taxonomies = ImmutableList.Create((taxonomy, id));

			var tt = builder.TTest.TermTaxonomy.GetName();
			var tr = builder.TTest.TermRelationship.GetName();
			var p = builder.TTest.Post.GetName();

			// Act
			var result = builder.AddWhereTaxonomies(v.Parts, taxonomies);

			// Assert
			var some = result.AssertSome();
			Assert.Collection(some.WhereCustom,
				x =>
				{
					Assert.Equal("(" +
						$"SELECT COUNT(1) FROM `{tr}` " +
						$"INNER JOIN `{tt}` ON `{tr}`.`term_taxonomy_id` = `{tt}`.`term_taxonomy_id` " +
						$"WHERE `{tt}`.`taxonomy` = @taxonomy0 " +
						$"AND `{tr}`.`object_id` = `{p}`.`ID` " +
						$"AND `{tt}`.`term_id` IN (@taxonomy0_0)" +
						") = 1",
						x.clause
					);
				}
			);
		}

		[Fact]
		public void Single_Taxonomy_Adds_Parameters()
		{
			// Arrange
			var (builder, v) = Setup();
			var taxonomy = Taxonomy.PostCategory;
			var id = new WpTermId(F.Rnd.Ulng);
			var taxonomies = ImmutableList.Create((taxonomy, id));

			// Act
			var result = builder.AddWhereTaxonomies(v.Parts, taxonomies);

			// Assert
			var some = result.AssertSome();
			Assert.Collection(some.WhereCustom,
				x =>
				{
					Assert.Collection(x.parameters,
						y =>
						{
							Assert.Equal("@taxonomy0", y.Key);
							Assert.Equal(taxonomy, y.Value);
						},
						y =>
						{
							Assert.Equal("@taxonomy0_0", y.Key);
							Assert.Equal(id.Value, y.Value);
						}
					);
				}
			);
		}

		[Fact]
		public void Multiple_Taxonomies_Adds_Taxonomy_Clause()
		{
			// Arrange
			var (builder, v) = Setup();
			var t0 = Taxonomy.PostCategory;
			var id0 = new WpTermId(F.Rnd.Ulng);
			var t1 = Taxonomy.NavMenu;
			var id1 = new WpTermId(F.Rnd.Ulng);
			var id2 = new WpTermId(F.Rnd.Ulng);
			var taxonomies = ImmutableList.Create((t0, id0), (t1, id1), (t1, id2));

			var tt = builder.TTest.TermTaxonomy.GetName();
			var tr = builder.TTest.TermRelationship.GetName();
			var p = builder.TTest.Post.GetName();

			// Act
			var result = builder.AddWhereTaxonomies(v.Parts, taxonomies);

			// Assert
			var some = result.AssertSome();
			Assert.Collection(some.WhereCustom,
				x =>
				{
					Assert.Equal("(" +
						$"SELECT COUNT(1) FROM `{tr}` " +
						$"INNER JOIN `{tt}` ON `{tr}`.`term_taxonomy_id` = `{tt}`.`term_taxonomy_id` " +
						$"WHERE `{tt}`.`taxonomy` = @taxonomy0 " +
						$"AND `{tr}`.`object_id` = `{p}`.`ID` " +
						$"AND `{tt}`.`term_id` IN (@taxonomy0_0)" +
						") = 1 AND (" +
						$"SELECT COUNT(1) FROM `{tr}` " +
						$"INNER JOIN `{tt}` ON `{tr}`.`term_taxonomy_id` = `{tt}`.`term_taxonomy_id` " +
						$"WHERE `{tt}`.`taxonomy` = @taxonomy1 " +
						$"AND `{tr}`.`object_id` = `{p}`.`ID` " +
						$"AND `{tt}`.`term_id` IN (@taxonomy1_0, @taxonomy1_1)" +
						") = 2",
						x.clause
					);
				}
			);
		}

		[Fact]
		public void Multiple_Taxonomies_Adds_Parameters()
		{
			// Arrange
			var (builder, v) = Setup();
			var t0 = Taxonomy.PostCategory;
			var id0 = new WpTermId(F.Rnd.Ulng);
			var id1 = new WpTermId(F.Rnd.Ulng);
			var t1 = Taxonomy.LinkCategory;
			var id2 = new WpTermId(F.Rnd.Ulng);
			var taxonomies = ImmutableList.Create((t0, id0), (t0, id1), (t1, id2));

			// Act
			var result = builder.AddWhereTaxonomies(v.Parts, taxonomies);

			// Assert
			var some = result.AssertSome();
			Assert.Collection(some.WhereCustom,
				x =>
				{
					Assert.Collection(x.parameters,
						y =>
						{
							Assert.Equal("@taxonomy0", y.Key);
							Assert.Equal(t0, y.Value);
						},
						y =>
						{
							Assert.Equal("@taxonomy0_0", y.Key);
							Assert.Equal(id0.Value, y.Value);
						},
						y =>
						{
							Assert.Equal("@taxonomy0_1", y.Key);
							Assert.Equal(id1.Value, y.Value);
						},
						y =>
						{
							Assert.Equal("@taxonomy1", y.Key);
							Assert.Equal(t1, y.Value);
						},
						y =>
						{
							Assert.Equal("@taxonomy1_0", y.Key);
							Assert.Equal(id2.Value, y.Value);
						}
					);
				}
			);
		}
	}
}

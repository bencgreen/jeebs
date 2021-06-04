﻿// Jeebs Unit Tests
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using Jeebs.Data;

namespace Jeebs.WordPress.Data.Query_Tests.PostsTaxonomyPartsBuilder_Tests
{
	public static class Setup
	{
		private readonly static WpDbSchema schema =
			new(F.Rnd.Str);

		public static Query.PostsTaxonomyPartsBuilder GetBuilder(IExtract extract) =>
			new(extract, schema);
	}
}
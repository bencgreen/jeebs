﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jeebs.Data.Mapping.MapService_Tests
{
	public class FooTableWithBar234 : Table
	{
		public string FooId { get; } = "foo_id";

		public string Bar0 { get; } = "foo_bar0";

		public string Bar1 { get; } = "foo_bar1";

		public string Bar2 { get; } = "foo_bar2";

		public string Bar3 { get; } = "foo_bar3";

		public string Bar4 { get; } = "foo_bar4";

		public FooTableWithBar234() : base("foo_with_bar2") { }
	}
}

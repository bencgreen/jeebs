﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jeebs.Data.Mapping.MapService_Tests
{
	public class FooWithoutVersionAttribute : IEntityWithVersion
	{
		[Ignore]
		public long Id
		{
			get => FooId;
			set => FooId = value;
		}

		[Id]
		public long FooId { get; set; }

		public long Version { get; set; }

		public string Bar0 { get; set; } = string.Empty;

		public string Bar1 { get; set; } = string.Empty;
	}
}

﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Jeebs.Data.Querying;

namespace Jeebs.WordPress.Data
{
	public static partial class Query
	{
		/// <summary>
		/// WordPress Query Options
		/// </summary>
		/// <typeparam name="TId">Entity ID type</typeparam>
		public abstract record Options<TId> : QueryOptions<TId>
			where TId : StrongId
		{
			/// <summary>
			/// IWpDbSchema shorthand
			/// </summary>
			protected IWpDbSchema T { get; private init; }

			internal IWpDbSchema TTest =>
				T;

			/// <summary>
			/// Inject dependencies
			/// </summary>
			/// <param name="schema">IWpDbSchema</param>
			/// <param name="builder">IQueryPartsBuilder</param>
			internal Options(IWpDbSchema schema, IQueryPartsBuilder<TId> builder) : base(builder) =>
				T = schema;
		}
	}
}

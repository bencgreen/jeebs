﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System.Collections.Generic;

namespace Jeebs.Data.Mapping
{
	/// <inheritdoc cref="IColumn"/>
	/// <param name="Table">Table Name</param>
	/// <param name="Name">Column Name</param>
	/// <param name="Alias">Column Alias</param>
	public partial record Column(string Table, string Name, string Alias) : IColumn
	{
		/// <summary>
		/// Column Alias Comparer
		/// </summary>
		public class AliasComparer : IEqualityComparer<IColumn>
		{
			/// <summary>
			/// Returns true if the two aliases are identical
			/// </summary>
			/// <param name="x">IColumn 1</param>
			/// <param name="y">IColumn 2</param>
			/// <returns>True if the aliases of the two columns are identical</returns>
			public bool Equals(IColumn? x, IColumn? y) =>
				x?.Alias == y?.Alias;

			/// <summary>
			/// Return object's hash code
			/// </summary>
			/// <param name="obj">IColumn</param>
			/// <returns>Hash code</returns>
			public int GetHashCode(IColumn obj) =>
				obj.Alias.GetHashCode();
		}
	}
}

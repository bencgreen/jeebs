﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Microsoft.Extensions.DependencyInjection;

namespace Jeebs.Data
{
	/// <summary>
	/// IServiceCollection extension methods
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Configure data
		/// </summary>
		/// <param name="this">IServiceCollection</param>
		public static IServiceCollection AddData(this IServiceCollection @this)
		{
			// Return
			return @this;
		}
	}
}

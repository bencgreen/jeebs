﻿using System;
using System.Collections.Generic;
using System.Text;
using Jeebs.Apps;
using Jeebs.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jeebs.Data
{
	/// <summary>
	/// IServiceCollection extension methods
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Register data configuration
		/// </summary>
		/// <param name="services">IServiceCollection</param>
		/// <param name="section">[Optional] Section Key for retrieving database configuration</param>
		public static FluentData AddData(this IServiceCollection services, in string section = DbConfig.Key)
		{
			return new FluentData(ref services, section);
		}

		/// <summary>
		/// Fluently configure data registration
		/// </summary>
		public sealed class FluentData
		{
			/// <summary>
			/// IServiceCollection
			/// </summary>
			private IServiceCollection Services { get; }

			/// <summary>
			/// Configuration Section Key
			/// </summary>
			private readonly string section;

			/// <summary>
			/// Start configuring data
			/// </summary>
			/// <param name="services">IServiceCollection</param>
			/// <param name="section">Configuration Section Key</param>
			public FluentData(ref IServiceCollection services, in string section)
			{
				Services = services;
				this.section = section;
			}

			/// <summary>
			/// Register data configuration
			/// </summary>
			/// <param name="config">IConfiguration</param>
			public IServiceCollection Using(IConfiguration config)
			{
				Services.Bind<DbConfig>().To(section).Using(config);
				return Services;
			}
		}
	}
}
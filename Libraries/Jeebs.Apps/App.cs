﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jeebs.Config;
using Jeebs.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Jeebs.Apps
{
	/// <summary>
	/// Application bootstrapped using IHost
	/// </summary>
	public abstract class App
	{
		/// <summary>
		/// Create IHost
		/// </summary>
		/// <param name="args">Command Line Arguments</param>
		public virtual IHost CreateHost(string[] args)
		{
			// Create Default Host Builder
			return Host.CreateDefaultBuilder(args)

				// Configure Host
				.ConfigureHostConfiguration(config =>
				{
					ConfigureHost(config);
				})

				// Configure App
				.ConfigureAppConfiguration((host, config) =>
				{
					ConfigureApp(host.HostingEnvironment, config);
				})

				// Configure Serilog
				.UseSerilog((host, logger) =>
				{
					ConfigureSerilog(host.Configuration, logger);
				})

				// Configure Services
				.ConfigureServices((host, services) =>
				{
					ConfigureServices(host.HostingEnvironment, host.Configuration, services);
				})

				// Build Host
				.Build()

			;
		}

		/// <summary>
		/// Configure Host
		/// </summary>
		/// <param name="config">IConfigurationBuilder</param>
		protected virtual void ConfigureHost(IConfigurationBuilder config)
		{
			config.SetBasePath(Directory.GetCurrentDirectory());
		}

		/// <summary>
		/// Configure App
		/// </summary>
		/// <param name="env">IHostEnvironment</param>
		/// <param name="config">IConfigurationBuilder</param>
		protected virtual void ConfigureApp(IHostEnvironment env, IConfigurationBuilder config)
		{
			config.AddJeebsConfig(env);
		}

		/// <summary>
		/// Configure Serilog
		/// </summary>
		/// <param name="config">IConfiguration</param>
		/// <param name="logger">LoggerConfiguration</param>
		protected virtual void ConfigureSerilog(IConfiguration config, LoggerConfiguration logger)
		{
			// Load Serilog config
			var jeebs = config.GetJeebsConfig();
			logger.LoadFromJeebsConfig(jeebs);
		}

		/// <summary>
		/// Configure Services
		/// </summary>
		/// <param name="env">IHostEnvironment</param>
		/// <param name="config">IConfiguration</param>
		/// <param name="services">IServiceCollection</param>
		protected virtual void ConfigureServices(IHostEnvironment env, IConfiguration config, IServiceCollection services)
		{
			// Bind JeebsConfig
			services.Bind<JeebsConfig>().To(JeebsConfig.Key).Using(config);

			// Register Serilog Logger
			services.AddTransient<ILog, SerilogLogger>();
		}
	}
}

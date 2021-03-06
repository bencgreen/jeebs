﻿// Jeebs Test Applications
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System;
using AppMvc.EfCore;
using Jeebs;
using Jeebs.Auth;
using Jeebs.Auth.Data.Clients.MySql;
using Jeebs.Mvc.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AppMvc
{
	public sealed class App : Jeebs.Apps.MvcAppWithData
	{
		public App() : base(false) { }

		protected override void ConfigureServices(IHostEnvironment env, IConfiguration config, IServiceCollection services)
		{
			base.ConfigureServices(env, config, services);

			services.AddAuth(config)
				.WithData<MySqlDbClient>()
				.WithJwt();

			services.AddDbContext<EfCoreContext>(
				options => options.UseMySQL("server=192.168.1.104;port=18793;user id=test;password=test;database=test;convert zero datetime=True;sslmode=none")
			);
		}

		public override void Ready(IServiceProvider services, ILog log)
		{
			base.Ready(services, log);

			var db = services.GetRequiredService<AuthDb>();
			db.MigrateToLatest();
		}
	}
}

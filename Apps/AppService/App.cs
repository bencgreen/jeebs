﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jeebs;
using Microsoft.Extensions.Hosting;

namespace ServiceApp
{
	public class App : Jeebs.Apps.ServiceApp<AppService>
	{
	}

	public class AppService : IHostedService
	{
		private readonly ILog log;

		public AppService(ILog log)
		{
			this.log = log;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			log.Debug("Hello, world!");

			var response = Console.ReadLine();
			log.Debug("Response: {Response}", response);

			Console.Read();

			return Task.Delay(2000);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}

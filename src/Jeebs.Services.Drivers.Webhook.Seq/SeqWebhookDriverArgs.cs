﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System.Net.Http;
using Jeebs.Config;
using Microsoft.Extensions.Options;

namespace Jeebs.Services.Drivers.Webhook.Seq
{
	/// <summary>
	/// Seq Webhook Driver arguments
	/// </summary>
	public sealed class SeqWebhookDriverArgs : WebhookDriverArgs<SeqConfig>
	{
		/// <summary>
		/// Create object
		/// </summary>
		/// <param name="factory">IHttpClientFactory</param>
		/// <param name="log">ILog</param>
		/// <param name="jeebsConfig">JeebsConfig</param>
		public SeqWebhookDriverArgs(
			IHttpClientFactory factory,
			ILog log,
			IOptions<JeebsConfig> jeebsConfig
		) : base(factory, log, jeebsConfig, c => c.Seq) { }
	}
}

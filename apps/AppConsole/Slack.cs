﻿// Jeebs Test Applications
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Jeebs;
using Jeebs.Services.Drivers.Webhook.Slack;

namespace AppConsole
{
	internal class Slack : SlackWebhookDriver, INotificationListener
	{
		public Slack(SlackWebhookDriverArgs args) : base("bcgdesign", args) { }
	}
}

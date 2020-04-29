﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jeebs.Config.Logging
{
	/// <summary>
	/// Slack Provider
	/// </summary>
	public sealed class SlackProvider : LoggingProvider
	{
		/// <summary>
		/// Web hook URI
		/// </summary>
		public string Webhook { get; set; } = string.Empty;

		/// <summary>
		/// Whether or not to add attachments to a message (error type and timestamp)
		/// </summary>
		public bool ShowAttachments { get; set; }

		/// <inheritdoc/>
		public override bool IsValid() => !string.IsNullOrEmpty(Webhook);
	}
}

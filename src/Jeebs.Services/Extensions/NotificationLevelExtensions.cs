﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Jeebs.Logging;

namespace Jeebs.Services
{
	/// <summary>
	/// LogLevel Extensions - ToMessageLevel
	/// </summary>
	public static class NotificationLevelExtensions
	{
		/// <summary>
		/// Convert a <see cref="LogLevel"/> to a <see cref="NotificationLevel"/>
		/// </summary>
		/// <param name="this"></param>
		public static NotificationLevel ToNotificationLevel(this LogLevel @this) =>
			@this switch
			{
				LogLevel.Warning =>
					NotificationLevel.Warning,

				LogLevel.Error =>
					NotificationLevel.Error,

				LogLevel.Fatal =>
					NotificationLevel.Error,

				_ =>
					NotificationLevel.Information
			};
	}
}

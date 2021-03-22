﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using Jeebs.Data;

namespace Jeebs.Auth.Data.Entities
{
	/// <inheritdoc cref="IUser"/>
	internal sealed record UserEntity : IUser
	{
		/// <inheritdoc/>
		[Ignore]
		StrongId IEntity.Id
		{
			get =>
				UserId;

			init =>
				UserId = new UserId(value.Value);
		}

		/// <inheritdoc/>
		[Id]
		public UserId UserId { get; init; } = new UserId();

		/// <inheritdoc/>
		[Version]
		public long Version { get; init; }

		/// <inheritdoc/>
		public string EmailAddress { get; init; } = string.Empty;

		/// <inheritdoc/>
		public string PasswordHash { get; init; } = string.Empty;

		/// <inheritdoc/>
		public string FriendlyName { get; init; } = string.Empty;

		/// <inheritdoc/>
		public string? GivenName { get; init; }

		/// <inheritdoc/>
		public string? FamilyName { get; init; }

		/// <inheritdoc/>
		public bool IsEnabled { get; init; }

		/// <inheritdoc/>
		public bool IsSuper { get; init; }

		/// <inheritdoc/>
		public DateTimeOffset? LastSignedIn { get; init; }
	}
}

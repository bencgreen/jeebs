﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using Jeebs.Auth.Data.Entities;
using Jeebs.Auth.Data.Models;
using Jeebs.Data;

namespace Jeebs.Auth.Data.Tables
{
	/// <summary>
	/// Authentication Role Table
	/// </summary>
	internal sealed record AuthRoleTable() : Table("auth_role")
	{
		/// <summary>
		/// Prefix added before all columns
		/// </summary>
		public const string ColumnPrefix = "Role";

		#region From AuthRoleModel

		/// <inheritdoc cref="AuthRoleModel.Id"/>
		public string Id =>
			ColumnPrefix + nameof(Id);

		/// <inheritdoc cref="AuthRoleModel.Name"/>
		public string Name =>
			ColumnPrefix + nameof(Name);

		#endregion

		#region From AuthRoleEntity

		/// <inheritdoc cref="AuthRoleEntity.Description"/>
		public string Description =>
			ColumnPrefix + nameof(Description);

		#endregion
	}
}
﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System.Threading.Tasks;
using Jeebs.Data;

namespace Jeebs.Auth.Data
{
	/// <summary>
	/// Provides Authentication functions for interacting with Roles
	/// </summary>
	/// <typeparam name="TRoleEntity">Role Entity type</typeparam>
	public interface IAuthRoleFunc<TRoleEntity> : IDbFunc<TRoleEntity, AuthRoleId>
		where TRoleEntity : IAuthRole, IEntity
	{
		/// <summary>
		/// Create a new Role
		/// </summary>
		/// <param name="name">Role name</param>
		Task<Option<AuthRoleId>> CreateAsync(string name);
	}
}
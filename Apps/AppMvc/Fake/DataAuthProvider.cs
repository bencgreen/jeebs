﻿// Jeebs Test Applications
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System.Collections.Generic;
using System.Threading.Tasks;
using Jeebs.Auth;
using Jeebs.Auth.Data;
using JeebsF;

namespace AppMvc.Fake
{
	public class DataAuthProvider : IDataAuthProvider
	{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		public async Task<Option<TUserModel>> ValidateUserAsync<TUserModel>(string email, string password)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
			where TUserModel : IUserModel, new()
		{
			if (password == "fail")
			{
				return OptionF.None<TUserModel>(true);
			}

			return new TUserModel
			{
				UserId = new(1),
				EmailAddress = "ben@bcgdesign.com",
				FriendlyName = "Ben",
				IsSuper = true
			};
		}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		public async Task<Option<TUserModel>> ValidateUserAsync<TUserModel, TRoleModel>(string email, string password)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
			where TUserModel : IUserModel<TRoleModel>, new()
			where TRoleModel : IRoleModel, new()
		{
			if (password == "fail")
			{
				return OptionF.None<TUserModel>(true);
			}

			return new TUserModel
			{
				UserId = new(1),
				EmailAddress = "ben@bcgdesign.com",
				FriendlyName = "Ben",
				IsSuper = true,
				Roles = new List<TRoleModel>(new[]
				{
					new TRoleModel { Name = "One" },
					new TRoleModel { Name = "Two" },
				})
			};
		}
	}
}

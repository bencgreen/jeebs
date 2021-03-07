﻿using Jeebs.Auth.Data;

namespace AppMvc.Models
{
	public record UserModel : IUserModel
	{
		public UserId UserId { get; init; } = new UserId();
		public string EmailAddress { get; init; } = string.Empty;
		public string FriendlyName { get; init; } = string.Empty;
		public string FullName { get; init; } = string.Empty;
		public bool IsSuper { get; init; }

		public UserModel() { }
	}
}
﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Sodium;

namespace Jeebs.Cryptography
{
	/// <summary>
	/// String Extensions: HashPassword
	/// </summary>
	public static class StringExtensions_HashPassword
	{
		/// <summary>
		/// Hash a password using argon2id - returns hash containing the salt (of length 128 bytes)
		/// </summary>
		/// <param name="this">Password to hash</param>
		/// <returns>Password hash</returns>
		public static string HashPassword(this string @this)
		{
			if (string.IsNullOrEmpty(@this))
			{
				return string.Empty;
			}

			return PasswordHash.ArgonHashString(@this, PasswordHash.StrengthArgon.Moderate);
		}
	}
}

﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using Jeebs.Linq;

namespace Jeebs.Cryptography
{
	/// <summary>
	/// Encryption Extensions
	/// </summary>
	public static class ObjectExtensions_Encrypt
	{
		/// <summary>
		/// Encrypt an object using the specified key and return it serialised as JSON
		/// </summary>
		/// <typeparam name="T">Type of object being encrypted</typeparam>
		/// <param name="this">Value to encrypt</param>
		/// <param name="key">Encryption Key (must be 32 bytes)</param>
		public static Option<string> Encrypt<T>(this T @this, byte[] key) =>
			@this switch
			{
				T x =>
					from l in new Lockable<T>(x).Lock(key)
					select l.Serialise(),

				_ =>
					F.JsonF.Empty
			};

		/// <summary>
		/// Encrypt an object using the specified key and return it serialised as JSON
		/// </summary>
		/// <typeparam name="T">Type of object being encrypted</typeparam>
		/// <param name="this">Value to encrypt</param>
		/// <param name="key">Encryption key</param>
		public static string Encrypt<T>(this T @this, string key) =>
			@this switch
			{
				T x =>
					new Lockable<T>(x).Lock(key).Serialise(),

				_ =>
					F.JsonF.Empty
			};
	}
}
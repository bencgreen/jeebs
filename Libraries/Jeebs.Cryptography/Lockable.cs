﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jeebs.Cryptography
{
	/// <summary>
	/// Constants relating to <see cref="Lockable{T}"/> and <see cref="Locked{T}"/>
	/// </summary>
	public static class Lockable
	{
		/// <summary>
		/// Length of encryption key (if it's a byte array)
		/// </summary>
		public const int KeyLength = 32;
	}

	/// <summary>
	/// Contains contents that can been encrypted
	/// </summary>
	/// <typeparam name="T">Value type</typeparam>
	public sealed class Lockable<T>
	{
		/// <summary>
		/// Contents
		/// </summary>
		public T Contents { get; }

		/// <summary>
		/// Create object
		/// </summary>
		/// <param name="contents">Contents</param>
		public Lockable(T contents)
			=> Contents = contents;

		/// <summary>
		/// Lock object
		/// </summary>
		/// <param name="key">Encryption key - must be <see cref="KeyLength"/> bytes</param>
		public Locked<T> Lock(byte[] key)
			=> key.Length switch
			{
				Lockable.KeyLength => new Locked<T>(Contents, key),
				_ => throw new NotSupportedException($"Key length must be {Lockable.KeyLength} bytes.")
			};

		/// <summary>
		/// Lock object
		/// </summary>
		/// <param name="key">Encryption key</param>
		public Locked<T> Lock(string key)
			=> new Locked<T>(Contents, key);
	}
}
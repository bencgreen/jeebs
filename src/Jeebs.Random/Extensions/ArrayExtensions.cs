﻿// Jeebs Rapid Application Development
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using System.Linq;
using System.Security.Cryptography;

namespace Jeebs
{
	/// <summary>
	/// Array Extensions
	/// </summary>
	public static class ArrayExtensions
	{
		/// <summary>
		/// Create a copy of an array and shuffle the elements using a Fisher-Yates Shuffle
		/// See http://www.dotnetperls.com/fisher-yates-shuffle
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="this">Array to shuffle</param>
		public static T[] Shuffle<T>(this T[] @this)
		{
			// Don't alter the original array
			var shuffled = @this.ToArray();

			// For speed share the random number generator
			using var rng = new RNGCryptoServiceProvider();

			for (int i = shuffled.Length; i > 1; i--)
			{
				int j = F.Rnd.NumberF.GetInt32(max: i - 1, generator: rng);
				var tmp = shuffled[j];
				shuffled[j] = shuffled[i - 1];
				shuffled[i - 1] = tmp;
			}

			return shuffled;
		}
	}
}

﻿using System;
using System.Collections;
using System.Text;
using Jeebs.Reflection;

namespace Jeebs.Util
{
	/// <summary>
	/// PHP Serialiser Class
	/// </summary>
	public static class Php
	{
		// Value types
		public const char Array = 'a';
		public const char Boolean = 'b';
		public const char Double = 'd';
		public const char Integer = 'i';
		public const char String = 's';
		public const char Null = 'N';

		/// <summary>
		/// UTF8Encoding
		/// </summary>
		private static readonly Encoding enc = new UTF8Encoding();

		/// <summary>
		/// Serialise object
		/// </summary>
		/// <param name="obj">Object</param>
		/// <returns>Serialised object</returns>
		public static string Serialise<T>(T obj)
			=> Serialise(obj, new StringBuilder()).ToString();

		private static StringBuilder Serialise<T>(T obj, StringBuilder sb)
		{
			return obj switch
			{
				string x => appendString(x),
				bool x => append(Boolean, x ? "1" : "0"),
				int x => append(Integer, x),
				long x => append(Integer, x),
				float x => append(Double, x),
				double x => append(Double, x),
				IList x => appendList(x),
				IDictionary x => appendDictionary(x),
				{ } x => sb,
				_ => sb.Append(Null).Append(";")
			};

			// Append a value to the StringBuilder
			StringBuilder append<U>(char type, U value)
				=> sb.Append($"{type}:{value};");

			// Append a string to the StringBuilder
			StringBuilder appendString(string str)
				=> sb.Append($"{String}:{enc.GetByteCount(str)}:\"{str}\";");

			// Append a Hashtable to the StringBuilder
			// Enables arrays of different key / value pairs
			StringBuilder appendHashtable(Hashtable hashtable)
			{
				sb.Append($"{Array}:{hashtable.Count}:{{");
				foreach (DictionaryEntry item in hashtable)
				{
					Serialise(item.Key, sb);
					Serialise(item.Value, sb);
				}
				return sb.Append("}");
			}

			// Append a List to the StringBuilder
			StringBuilder appendList(IList list)
			{
				var htb = new Hashtable();
				for (int i = 0; i < list.Count; i++)
				{
					htb.Add(i, list[i]);
				}

				return appendHashtable(htb);
			}

			// Append a Dictionary to the StringBuilder
			StringBuilder appendDictionary(IDictionary d)
				=> appendHashtable(new Hashtable(d));
		}

		private static int pointer;

		/// <summary>
		/// Deserialise object
		/// </summary>
		/// <param name="str">Serialised string</param>
		/// <returns>Deserialised object</returns>
		public static object Deserialise(string str)
		{
			if (string.IsNullOrWhiteSpace(str) || str.Length <= pointer)
			{
				return new object();
			}

			return str[pointer] switch
			{
				Array => getHashtable(),
				Boolean => getBoolean(),
				Double => getNumber(double.Parse, 0d),
				Integer => getNumber(long.Parse, 0u),
				String => getString(),
				Null => getNull(),
				_ => string.Empty
			};

			// Get null object
			static object getNull()
			{
				pointer += 2;
				return new object();
			}

			// Get boolean
			bool getBoolean()
			{
				char b = str[pointer + 2];
				pointer += 4;
				return b == '1';
			}

			// Get a number (long or double)
			T getNumber<T>(Func<string, T> parse, T ifError)
			{
				// Get string value
				var colon = str.IndexOf(':', pointer) + 1;
				var semicolon = str.IndexOf(';', colon);
				var num = str[colon..semicolon]; // the number as a string
				pointer += 3 + num.Length;

				// Attempt to parse number value
				try
				{
					return parse(num);
				}
				catch (Exception)
				{
					return ifError;
				}
			}

			// Get string
			string getString()
			{
				// Get start and end positions
				var colon0 = str.IndexOf(':', pointer) + 1;
				var colon1 = str.IndexOf(':', colon0);
				var semicolon = str.IndexOf(';', pointer);
				pointer = semicolon + 1;

				// Calculate length
				var from = colon1 + 2; // start two characters after the second colon
				var to = semicolon - 1; // end one character before the semicolon

				// Get substring
				return str[from..to];
			}

			// Get a Hashtable
			Hashtable getHashtable()
			{
				// Get start and end positions
				var colon0 = str.IndexOf(':', pointer) + 1;
				var colon1 = str.IndexOf(':', colon0);
				var num = str[colon0..colon1]; // the number of items in the array
				var len = int.Parse(num);
				pointer += 4 + num.Length;

				// Get each key and value, and add them to a hashtable
				var table = new Hashtable();
				for (int i = 0; i < len; i++)
				{
					var key = Deserialise(str);
					table[key] = Deserialise(str);
				}

				return table;
			}
		}
	}
}

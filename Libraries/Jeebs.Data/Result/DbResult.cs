﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jeebs.Data
{
	public static partial class DbResult
	{
		#region Success

		/// <summary>
		/// Simple success (value is bool true)
		/// </summary>
		public static DbSuccess Success() => new DbSuccess();

		/// <summary>
		/// Success with value
		/// </summary>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="value">Value</param>
		public static DbSuccess<T> Success<T>(in T value) => new DbSuccess<T>(value);

		/// <summary>
		/// Success with message and value
		/// </summary>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="message">Message</param>
		/// <param name="value">Value</param>
		public static DbSuccess<T> Success<T>(in string message, in T value) => new DbSuccess<T>(message, value);

		#endregion

		#region Failure

		/// <summary>
		///	Simple failure
		/// </summary>
		/// <param name="errors">Errors</param>
		public static DbFailure Failure(params string[] errors) => new DbFailure(errors);

		/// <summary>
		///	Simple failure
		/// </summary>
		/// <param name="errors">List of errors</param>
		public static DbFailure Failure(in List<string> errors) => new DbFailure(errors);

		/// <summary>
		///	Failure with value type
		/// </summary>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="errors">Errors</param>
		public static DbFailure<T> Failure<T>(params string[] errors) => new DbFailure<T>(errors);

		/// <summary>
		///	Failure with value type
		/// </summary>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="errors">List of errors</param>
		public static DbFailure<T> Failure<T>(in List<string> errors) => new DbFailure<T>(errors);

		/// <summary>
		///	Concurrency Failure
		/// </summary>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="errors">Errors</param>
		public static DbFailureConcurrency<T> ConcurrencyFailure<T>(params string[] errors) => new DbFailureConcurrency<T>(errors);

		/// <summary>
		///	Concurrency Failure
		/// </summary>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="errors">List of errors</param>
		public static DbFailureConcurrency<T> ConcurrencyFailure<T>(in List<string> errors) => new DbFailureConcurrency<T>(errors);

		#endregion
	}
}
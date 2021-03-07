﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;

namespace Jm.Data
{
	/// <summary>
	/// Message about an error that has occurred during a Create operation
	/// </summary>
	public class CreateErrorMsg : CreateMsg
	{
		/// <summary>
		/// Create object
		/// </summary>
		/// <param name="type">POCO type</param>
		public CreateErrorMsg(Type type) : base(type, 0) { }

		/// <summary>
		/// Output error message
		/// </summary>
		public override string ToString() =>
			$"Unable to Create '{type}'.";
	}
}

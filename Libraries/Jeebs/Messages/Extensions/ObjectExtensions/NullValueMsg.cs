﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;

namespace Jm.Extensions.ObjectExtensions
{
	/// <summary>
	/// See <see cref="Jeebs.Reflection.ObjectExtensions"/>
	/// </summary>
	public sealed class NullValueMsg : WithValueMsg<(Type type, string property)>
	{
		/// <summary>
		/// Create message
		/// </summary>
		/// <param name="type">Object type</param>
		/// <param name="property">Property name</param>
		public NullValueMsg(Type type, string property) : base((type, property)) { }
	}
}

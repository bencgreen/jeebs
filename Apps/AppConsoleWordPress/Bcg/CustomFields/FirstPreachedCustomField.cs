﻿// Jeebs Test Applications
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using Jeebs.WordPress;

namespace AppConsoleWordPress.Bcg
{
	/// <summary>
	/// The place a sermon was first preached
	/// </summary>
	public sealed class FirstPreachedCustomField : TermCustomField
	{
		/// <summary>
		/// This is a required field
		/// </summary>
		public FirstPreachedCustomField() : base("first_preached", true) { }
	}
}

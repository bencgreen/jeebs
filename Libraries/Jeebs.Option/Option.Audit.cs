﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;

namespace JeebsF
{
	public abstract partial class Option<T>
	{
		/// <summary>
		/// Audit the current Option state and return unmodified
		/// Errors will not be returned as they affect the state of the object, but will be written to the console
		/// </summary>
		/// <param name="audit">Audit function</param>
		internal Option<T> DoAudit(Action<Option<T>> audit)
		{
			// Perform the audit
			try
			{
				audit(this);
			}
			catch (Exception e)
			{
				OptionF.HandleAuditException(e);
			}

			// Return the original object
			return this;
		}

		/// <inheritdoc cref="DoAudit(Action{Option{T}})"/>
		public Option<T> Audit(Action<Option<T>> audit) =>
			DoAudit(audit);
	}
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jeebs.Result.Chain.Fluent;

namespace Jeebs
{
	public partial class Link<TValue, TState>
	{
		/// <inheritdoc/>
		new public Handle<TValue, TState, Exception> Handle()
			=> new Handle<TValue, TState, Exception>(this);

		/// <inheritdoc/>
		new public Handle<TValue, TState, TException> Handle<TException>()
			where TException : Exception
			=> new Handle<TValue, TState, TException>(this);
	}
}
﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jeebs.Result.Chain.Fluent;

namespace Jeebs
{
	public partial class Link<TValue>
	{
		/// <inheritdoc/>
		public Handle<TValue, Exception> Handle()
			=> new Handle<TValue, Exception>(this);

		/// <inheritdoc/>
		public Handle<TValue, TException> Handle<TException>()
			where TException : Exception
			=> new Handle<TValue, TException>(this);
	}
}
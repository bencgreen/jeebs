﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jeebs
{
	/// <inheritdoc cref="IOk{TValue}"/>
	public class ROk<TValue> : R<TValue>, IOk<TValue>
	{
		internal ROk() { }

		/// <inheritdoc cref="IOk{TValue}.Ok"/>
		public IOk<TValue> Ok() => Ok<TValue>();

		/// <inheritdoc cref="IOk.Ok{TNext}"/>
		public IOk<TNext> Ok<TNext>() => this switch
		{
			IOk<TNext> ok => ok,
			_ => new ROk<TNext> { Messages = Messages }
		};

		/// <inheritdoc cref="IOk.OkV{TNext}(TNext)"/>
		public IOkV<TNext> OkV<TNext>(TNext value) => new ROkV<TNext>(value) { Messages = Messages };

		#region Explicit implementations

		IOk IOk.Ok() => Ok<TValue>();

		IOk<TNext> IOk.Ok<TNext>() => Ok<TNext>();

		IOk<TValue> IOk<TValue>.Ok() => Ok<TValue>();

		IOkV<TNext> IOk.OkV<TNext>(TNext value) => OkV(value);

		#endregion
	}
}

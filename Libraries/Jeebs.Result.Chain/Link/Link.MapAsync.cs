﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jeebs
{
	public partial class Link<TValue>
	{
		private async Task<IR<TNext>> PrivateMapAsync<TResult, TNext>(Func<TResult, Task<IR<TNext>>> f)
			where TResult : IOk<TValue>
			=> result switch
			{
				TResult x => Catch(async () => await f(x)),
				_ => result.Error<TNext>()
			};

		/// <inheritdoc/>
		public Task<IR<TNext>> MapAsync<TNext>(Func<IOk<TValue>, Task<IR<TNext>>> f)
			=> PrivateMapAsync(f);

		/// <inheritdoc/>
		public Task<IR<TNext>> MapAsync<TNext>(Func<IOkV<TValue>, Task<IR<TNext>>> f)
			=> PrivateMapAsync(f);
	}
}
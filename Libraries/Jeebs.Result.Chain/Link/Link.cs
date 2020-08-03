﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jeebs
{
	/// <inheritdoc cref="ILink{TValue}"/>
	public partial class Link<TValue> : ILink<TValue>
	{
		private readonly IR<TValue> result;

		internal Link(IR<TValue> result)
			=> this.result = result;

		private IR<TNext> Catch<TNext>(Func<IR<TNext>> f)
		{
			try
			{
				return f();
			}
			catch (Exception ex)
			{
				return result.Error<TNext>().AddMsg(new Jm.ChainExceptionMsg(ex));
			}
		}

		private IR<TNext> Catch<TNext>(Func<Task<IR<TNext>>> f)
		{
			try
			{
				return f().Await();
			}
			catch (Exception ex)
			{
				return result.Error<TNext>().AddMsg(new Jm.ChainExceptionMsg(ex));
			}
		}

		public void Dispose()
			=> result.Dispose();
	}
}
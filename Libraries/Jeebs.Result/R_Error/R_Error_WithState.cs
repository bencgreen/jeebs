﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Jeebs
{
	public abstract partial class R<TResult, TState>
	{
		#region Keep Result Type

		/// <inheritdoc/>
		public IError<TResult, TState> Error(params IMessage[] messages)
		{
			Messages.AddRange(messages);
			return SkipAhead();
		}

		/// <inheritdoc/>
		public IError<TResult, TState> Error<TMessage>() where TMessage : IMessage, new()
		{
			Messages.Add<TMessage>();
			return SkipAhead();
		}

		/// <inheritdoc/>
		public IError<TResult, TState> Error<TMessage>(TMessage message) where TMessage : IMessage
		{
			Messages.Add(message);
			return SkipAhead();
		}

		#endregion

		#region Change Result Type

		/// <inheritdoc/>
		public IError<TNext, TState> ErrorNew<TNext>(params IMessage[] messages)
		{
			Messages.AddRange(messages);
			return SkipAhead<TNext>();
		}

		/// <inheritdoc/>
		public IError<TNext, TState> ErrorNew<TNext, TMessage>() where TMessage : IMessage, new()
		{
			Messages.Add<TMessage>();
			return SkipAhead<TNext>();
		}

		/// <inheritdoc/>
		public IError<TNext, TState> ErrorNew<TNext, TMessage>(TMessage message) where TMessage : IMessage, new()
		{
			Messages.Add(message);
			return SkipAhead<TNext>();
		}

		#endregion
	}
}
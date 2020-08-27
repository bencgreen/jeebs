﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jeebs
{
	/// <summary>
	/// <see cref="IMsg"/> Extensions
	/// </summary>
	public static class MsgExtensions
	{
		/// <summary>
		/// Prepare an IMsg object for sending
		/// </summary>
		/// <param name="this"></param>
		public static (string text, object[] args) Prepare(this IMsg @this)
		{
			// Get message type
			var type = @this.GetType().FullName;

			// Check for simple message type return
			if (@this.ToString() == type)
			{
				return (type, new object[] { });
			}

			// Add message type to the message
			var text = "{MsgType} - " + @this switch
			{
				ILoggableMsg x => x.Format,
				{ } x => x.ToString()
			};

			// Add message type to the argument array
			var args = @this switch
			{
				ILoggableMsg x => x.ParamArray.Prepend(type).ToArray(),
				_ => new object[] { type }
			};

			return (text, args);
		}
	}
}
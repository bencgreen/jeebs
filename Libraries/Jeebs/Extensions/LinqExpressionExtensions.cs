﻿// Jeebs Rapid Application Development
// Copyright (c) bcg|design - licensed under https://mit.bcgdesign.com/2013

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Jeebs
{
	/// <summary>
	/// LinqExpression Extensions
	/// </summary>
	public static class LinqExpressionExtensions
	{
		/// <summary>
		/// Prepare a Linq Expression for use as property setter / getter
		/// </summary>
		/// <typeparam name="TObject">Object type</typeparam>
		/// <typeparam name="TProperty">Property type</typeparam>
		/// <param name="this">Expression to get property</param>
		public static PropertyInfo<TObject, TProperty> GetPropertyInfo<TObject, TProperty>(this Expression<Func<TObject, TProperty>> @this)
		{
			var body = (MemberExpression)@this.Body;
			return new PropertyInfo<TObject, TProperty>((PropertyInfo)body.Member);
		}
	}
}

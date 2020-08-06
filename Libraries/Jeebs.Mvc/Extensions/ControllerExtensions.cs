﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;

namespace Jeebs.Mvc.Extensions
{
	/// <summary>
	/// Controller extension methods
	/// </summary>
	public static class ControllerExtensions
	{
		/// <summary>
		/// Execute an error result and return the View
		/// </summary>
		/// <param name="this">Controller</param>
		/// <param name="code">[Optional] HTTP Status Code</param>
		public async static Task<IActionResult> ExecuteErrorAsync(this Controller @this, int? code = null)
		{
			// Look for a view
			var viewName = $"Error{code}";
			if ((findView(viewName) ?? findView("Default")) is string view)
			{
				return @this.View(view);
			}

			// If response has stared we can't do anything
			if (@this.Response.HasStarted)
			{
				@this.Log.Warning("Unable to execute Error view: output already started.");
			}
			else
			{
				@this.Response.Clear();
				@this.Response.StatusCode = StatusCodes.Status500InternalServerError;
				await @this.Response.WriteAsync($"Unable to find views '{viewName}' or 'Default'.").ConfigureAwait(false);
			}

			// Return empty result
			return new EmptyResult();

			// Find a view
			string? findView(string viewName)
			{
				// Get View Engine
				var viewEngine = @this.HttpContext.RequestServices.GetService<ICompositeViewEngine>();

				// Find View
				var viewPath = $"Error/{viewName}";
				var result = viewEngine.FindView(@this.ControllerContext, viewPath, true);

				// Return result
				return result.Success ? viewPath : null;
			}
		}
	}
}
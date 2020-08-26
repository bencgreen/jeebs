﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Jeebs.Mvc
{
	/// <summary>
	/// Simple Controller
	/// </summary>
	public abstract class SimpleController : Microsoft.AspNetCore.Mvc.Controller
	{
		/// <summary>
		/// Disable favicon.ico
		/// </summary>
		[Route("favicon.ico")]
		public EmptyResult Favicon()
			=> new EmptyResult();

		/// <summary>
		/// Google Webmaster Tools site verification
		/// </summary>
		[Route("google5cfca8ea82e65ef1.html")]
		public ContentResult GoogleVerification()
			=> Content("google-site-verification: google5cfca8ea82e65ef1.html");

		/// <summary>
		/// Keep alive page
		/// </summary>
		[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
		[Route("keep-alive")]
		public ContentResult KeepAlive()
			=> Content(DateTime.UtcNow.ToString("u"), "text/plain");

		/// <summary>
		/// Robots.txt file
		/// </summary>
		[Route("robots.txt")]
		public ContentResult RobotsTxt()
			=> Content("User-agent: * Allow: /", "text/plain");
	}
}

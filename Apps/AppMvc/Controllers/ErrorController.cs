﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jeebs;
using Jeebs.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace AppMvc.Controllers
{
	public class ErrorController : Jeebs.Mvc.ErrorController
	{
		public ErrorController(ILog log) : base(log) { }

		public IActionResult Throw_Exception()
			=> throw new Exception("Something");

		public async Task<IActionResult> Return_Error()
			=> await this.ExecuteErrorAsync(R.Error().AddMsg().OfType<Jm.Mvc.Controller_ProcessResult_Unknown_IR>());

		public IActionResult Return_NotFound()
			=> NotFound();

		public IActionResult Return_Unauthorised()
			=> Unauthorized();

		public IActionResult Return_Forbidden()
			=> NotAllowed();

		public async Task<IActionResult> Return_Error404()
			=> await this.ExecuteErrorAsync(R.Error().AddMsg().OfType<Jm.NotFoundMsg>());
	}
}
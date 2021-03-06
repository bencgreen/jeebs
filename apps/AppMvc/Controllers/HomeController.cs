﻿// Jeebs Test Applications
// Copyright (c) bfren.uk - licensed under https://mit.bfren.uk/2013

using Jeebs;
using Microsoft.AspNetCore.Mvc;

namespace MvcApp.Controllers
{
	public class HomeController : Jeebs.Mvc.Controller
	{
		public HomeController(ILog<HomeController> log) : base(log) { }

		public IActionResult Index()
		{
			Log.Information("Hello, world!");
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}
	}
}

﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Scheduler.Web.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : BaseController<HomeController>
    {
        public IActionResult Index()
        {
            return View();
        }

        // Template actions
        public IActionResult About() => View();
        public IActionResult TOS() => View();
        public IActionResult Privacy() => View();
    }
}
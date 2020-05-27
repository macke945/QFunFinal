using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QFun.Models;

namespace QFun.Controllers
{
    public class ChallengesController : Controller
    {
        private readonly ILogger<ChallengesController> _logger;

        public ChallengesController(ILogger<ChallengesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

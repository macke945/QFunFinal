using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QFun.Models;

namespace QFun.Controllers
{
    public class MyPageController : Controller
    {
        private readonly ILogger<MyPageController> _logger;

        public MyPageController(ILogger<MyPageController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult _Chart()
        {
            List<UserData> newlist = new List<UserData>();

            var tempModel = (from a in appc.Challenge
                             join b in appc.Contribution on a.Id equals
                             b.ChallengeId
                             join c in appc.Vote on b.Id equals c.ContributionId
                             select new { a.ChName, b.CoName, c.Votes, b.IdentityUserId });

            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var item in tempModel)
            {

                var userdata = new UserData();

                if (item.IdentityUserId == identity)
                {
                    userdata.ChName = item.ChName;
                    userdata.CoName = item.CoName;
                    userdata.Votes = item.Votes;
                    newlist.Add(userdata);

                }

            }

            return View(newlist);
        }
      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

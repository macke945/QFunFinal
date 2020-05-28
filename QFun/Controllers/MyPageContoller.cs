using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QFun.Data;
using QFun.Models;

namespace QFun.Controllers
{
    public class MyPageController : Controller
    {
        private readonly ILogger<MyPageController> _logger;
        private readonly ApplicationDbContext appc;
        public MyPageController(ILogger<MyPageController> logger, ApplicationDbContext appc)
        {
            _logger = logger;
            this.appc = appc;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult _Chart()
        {
            List<UserData> newlist = new List<UserData>();
            
            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tempModel = (from a in appc.Challenge
                             join b in appc.Contribution on a.Id equals
                             b.ChallengeId
                             join c in appc.Vote on b.Id equals c.ContributionId where identity == b.UserId
                             select new { a.Title, c.Contribution.Votes.Count  }).Distinct();

            foreach (var item in tempModel)
            {
                var userdata = new UserData();
                
                userdata.ChallengeName = item.Title;
                userdata.Votes = item.Count;
                newlist.Add(userdata);

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

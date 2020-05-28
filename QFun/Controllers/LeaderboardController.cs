using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QFun.Models;
using QFun.Models.LeaderboardVm;
using QFun.Services;

namespace QFun.Controllers
{
    public class LeaderboardController : Controller
    {
        private readonly ContributionServices contributionServices;
        private readonly ILogger<LeaderboardController> _logger;

        public LeaderboardController(ILogger<LeaderboardController> logger, ContributionServices contributionServices)
        {
            this.contributionServices = contributionServices;
            _logger = logger;
        }

        public IActionResult Index()
        {

            

            var boardVm = new BoardVm();

            var userIds = contributionServices.GetAllUsersId();

            foreach (var id in userIds)
            {
                var userToAdd = new LeaderboardUserData();

                userToAdd.UserName = contributionServices.GetUserNameById(id);
                userToAdd.Votes = contributionServices.GetUserVotes(id);

                //Debug.WriteLine(userToAdd.UserName);
                //Debug.WriteLine(userToAdd.Votes);
                //Debug.WriteLine(userToAdd);

                boardVm.UserVoteData.Add(userToAdd);
            }

            boardVm.UserVoteData = boardVm.UserVoteData.OrderByDescending(u => u.Votes).ThenBy(u => u.UserName).ToList();

            return View(boardVm);
        }

      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

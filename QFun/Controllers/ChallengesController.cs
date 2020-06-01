using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using QFun.Data.DbTables;
using QFun.Models;
using QFun.Services;

namespace QFun.Controllers
{
    public class ChallengesController : Controller
    {

        private readonly ChallengeServices challengeService;
        private readonly ContributionServices contributionServices;
        private readonly VoteServices voteServices;
        private readonly IHostingEnvironment hostingEnvironment;

        public ChallengesController(ChallengeServices challengeService, ContributionServices contributionServices, IHostingEnvironment hostingEnvironment)
        {
            this.challengeService = challengeService;
            this.contributionServices = contributionServices;
            this.voteServices = voteServices;
            this.hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index()
        {
            var vm = new ChallengeVm();
            vm.Challenges = challengeService.GetAllChallenges();
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ChallengeVm vm)
        {
            if (ModelState.IsValid)
            {
                var challenge = new Challenge();

                challenge.Title = vm.Title;
                challenge.Description = vm.Description;

                challengeService.AddChallenge(challenge);

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Error", "Home", "");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Contributions(int id)
        {
            var contributionDb = new Contribution();
            var vm = new ContributionsVm();
            vm.Path = contributionDb.Path;
            vm.Description = contributionDb.Description;
            vm.TimeOfUpload = contributionDb.TimeOfUpload;
            vm.ChallengeId = id;
            vm.Contributions = contributionServices.GetAllContributionsByChallengeId(id);
            return View(vm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contributions(ContributionsVm vm, int id, string VoteButton)
        {
            if (ModelState.IsValid)
            {
                // var contribution = new Contribution();
                // contribution.Path = vm.Path;
                // contribution.Description = vm.Description;
                // contribution.TimeOfUpload = DateTime.UtcNow.ToLocalTime();
                // contribution.ChallengeId = id;

               

                string uniqueFileName = null;

                if (vm.Image != null)
                {

                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    vm.Image.CopyTo(new FileStream(filePath, FileMode.Create));

                    var contribution = new Contribution();

                    contribution.Path = filePath;
                    contribution.Description = vm.Description;
                    contribution.ChallengeId = id;
                     var userIds = contributionServices.GetAllUsersId();
                    foreach (var userId in userIds)
                    {
                        vm.UserId = contributionServices.GetUserIdById(userId);
                    }
                    contribution.UserId = vm.UserId;
                    var UserName = contributionServices.GetUserNameById(vm.UserId);
                    UserName = vm.UserName;
                    contributionServices.AddContribution(contribution);
                    if (!string.IsNullOrEmpty(VoteButton))
                    {
                        var vote = new Vote();
                        vote.UserId = vm.UserId;
                        vote.ContributionId = vm.ContributionsId;
                        voteServices.AddVote(vote);
                    }

                }


                return RedirectToAction(nameof(Contributions));
            }

            return RedirectToAction("Error", "Home", "");
        }
    }
}

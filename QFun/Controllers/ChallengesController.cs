using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public ChallengesController(ChallengeServices challengeService, ContributionServices contributionServices)
        {
            this.challengeService = challengeService;
            this.contributionServices = contributionServices;
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
            vm.UserId = contributionDb.UserId;
            vm.Contributions = contributionServices.GetAllContributionsByChallengeId(id);
            return View(vm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contributions(ContributionsVm vm, int id)
        {
            if (ModelState.IsValid)
            {
                var contribution = new Contribution();

                contribution.User = vm.User;
                contribution.Path = vm.Path;
                contribution.Description = vm.Description;
                contribution.TimeOfUpload = DateTime.UtcNow.ToLocalTime();
                contribution.ChallengeId = id;
                contribution.UserId = vm.UserId;
                contributionServices.AddContribution(contribution);

                return RedirectToAction(nameof(Contributions));
            }

            return RedirectToAction("Error", "Home", "");
        }



        int i = 0;

        public IActionResult Vote()
        {
            var vm = new ContributionsVm();
            i++;
            vm.Votes = i;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vote(ContributionsVm vm)
        {
            if (ModelState.IsValid)
            {
                i = vm.Votes;
                return RedirectToAction(nameof(Contributions));
            }

            return RedirectToAction("Error", "Home", "");
        }

    }
}

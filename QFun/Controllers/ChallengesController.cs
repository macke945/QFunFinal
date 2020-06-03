using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> userManager;

        public ChallengesController(ChallengeServices challengeService, ContributionServices contributionServices, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, VoteServices voteServices)
        {
            this.challengeService = challengeService;
            this.contributionServices = contributionServices;
            this.voteServices = voteServices;
            this.hostingEnvironment = hostingEnvironment;
            this.userManager = userManager;
        }

        public IActionResult AddContributions()
        {
            var vm = new AddContributionsVm();
            return View(vm);
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

        public IActionResult EditContributions(int id)
        {
            var vm = new contributionEditVm();
            var contriToEdit = contributionServices.GetContributionById(id);
            vm.Id = contriToEdit.Id;
            vm.ChallengeId = contriToEdit.ChallengeId;
            vm.Description = contriToEdit.Description;
            return View(vm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContributions(contributionEditVm vm)
        {

            if (ModelState.IsValid)
            {

                var contriToEdit = contributionServices.GetContributionById(vm.Id);
                string url = "https://localhost:44384/Challenges/Contributions";
                url += "/" + contriToEdit.ChallengeId;
                ClaimsPrincipal currentUser = this.User;
                var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                var UserName = contributionServices.GetUserNameById(currentUserId);
                UserName = vm.UserName;
                if (User.Identity.Name == vm.UserName)
                {

                    contriToEdit.Description = vm.Description;

                    contributionServices.EditContribution(contriToEdit);

                    return Redirect(url);
                }
                else
                {
                    return Redirect(url);
                }

            }


            return RedirectToAction("Error", "Home", "");


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddContributions(AddContributionsVm vm, int id)
        {

            string url = "https://localhost:44384/Challenges/Contributions";
            url += "/" + id;
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                vm.ChallengeId = id;
                ClaimsPrincipal currentUser = this.User;
                var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (currentUser.Identity.IsAuthenticated == false)
                {
                    vm.ChallengeId = id;
                    return Redirect(url);

                }
                if (currentUserId != null)
                {

                    if (vm.Image != null)
                    {

                        if (contributionServices.IsImage(vm.Image) && vm.Image.Length < (5 * 1024 * 1024))
                        {
                            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                            uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.Image.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                vm.Image.CopyTo(fileStream);
                            }

                            //vm.Image.CopyTo(new FileStream(filePath, FileMode.Create));

                            var contribution = new Contribution();

                            contribution.Path = uniqueFileName;
                            contribution.Description = vm.Description;
                            contribution.ChallengeId = id;

                            contribution.UserId = currentUserId;
                            var UserName = contributionServices.GetUserNameById(currentUserId);
                            UserName = vm.UserName;
                            contributionServices.AddContribution(contribution);
                        }

                    }
                }
                else
                {
                    vm.ChallengeId = id;
                    return Redirect(url);
                }


                return Redirect(url);
            }


            else if (vm.Image == null || vm.Image.Length > (5 * 1024 * 1024) || !contributionServices.IsImage(vm.Image))
            {
                vm.ChallengeId = id;
                vm.ShowImageError = false;
                return Redirect(url);
            }


            return RedirectToAction("Error", "Home", "");
        }

        public async Task<IActionResult> Votes(int id1, int id2)
        {

            if (ModelState.IsValid)
            {


                ClaimsPrincipal currentUser = this.User;
                var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;


                var vote = new Vote();
                vote.UserId = currentUserId;
                vote.ContributionId = id1;

                if (voteServices.UserAbleToVote(vote))
                {
                    voteServices.AddVote(vote);
                }
                else
                {
                    voteServices.RemoveVote(vote);
                }
            }

            Debug.WriteLine(id2);
            //return Redirect(Request.UrlReferrer.PathAndQuery);
            string url = "https://localhost:44384/Challenges/Contributions";
            url += "/" + id2;

            return Redirect(url);


            //return RedirectToAction(@"Contributions/" + id2);

            //return RedirectToAction(nameof(Contributions));
        }


        public async Task<IActionResult> DeleteContributionAdmin(int id1, int id2)
        {

            if (ModelState.IsValid)
            {
                string fileFolder = Path.Combine(hostingEnvironment.WebRootPath, @"images\");
                var contributionToDelete = contributionServices.GetContributionById(id1);
                string fileToDelete = fileFolder + contributionToDelete.Path;


                if (System.IO.File.Exists(fileToDelete))
                {
                    System.IO.File.Delete(fileToDelete);
                }
                else
                {
                    Debug.WriteLine("file doesn't exists");
                }


                contributionServices.RemoveContributionById(id1);
            }

            string url = "https://localhost:44384/Challenges/Contributions";
            url += "/" + id2;

            return Redirect(url);

        }

        public async Task<IActionResult> DeleteContributionUser(int id1, int id2)
        {

            if (ModelState.IsValid)
            {
                string fileFolder = Path.Combine(hostingEnvironment.WebRootPath, @"images\");
                var contributionToDelete = contributionServices.GetContributionById(id1);
                string fileToDelete = fileFolder + contributionToDelete.Path;

                if (System.IO.File.Exists(fileToDelete))
                {
                    System.IO.File.Delete(fileToDelete);
                }
                else
                {
                    Debug.WriteLine("file doesn't exists");
                }

                contributionServices.RemoveContributionById(id1);
            }

            string url = "https://localhost:44384/Challenges/Contributions";
            url += "/" + id2;

            return Redirect(url);

        }
        public async Task<IActionResult> DeleteChallengeAdmin(int id)
        {

            if (ModelState.IsValid)
            {
                challengeService.RemoveChallengeById(id);
            }


            string url = "https://localhost:44384/Challenges/Index";

            return Redirect(url);
        }
    }
}

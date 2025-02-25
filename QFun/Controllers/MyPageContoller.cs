﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QFun.Data;
using QFun.Models;
using QFun.Services;

namespace QFun.Controllers
{
    public class MyPageController : Controller
    {
        private readonly ILogger<MyPageController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ContributionServices _contributionService;
        public MyPageController(ILogger<MyPageController> logger, ApplicationDbContext context, ContributionServices contributionService)
        {
            _logger = logger;
            _context = context;
            _contributionService = contributionService;
        }

        public IActionResult Index()
        {
            var vm = new MyPageUserVm();
            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            vm.Contributions = _contributionService.GetAllContributionsByUserId(identity);
            return View(vm);
        }

        public IActionResult _User()
        {
            var vm = new MyPageUserVm();
            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            vm.Contributions = _contributionService.GetAllContributionsByUserId(identity);
            return View(vm);
        }

        public IActionResult _Chart()
        {
            List<UserData> newlist = new List<UserData>();

            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var tempModel = (from a in _context.Challenge
                             join b in _context.Contribution on a.Id equals
                             b.ChallengeId
                             join c in _context.Vote on b.Id equals c.ContributionId
                             where identity == b.UserId
                             select new { a.Title, c.Contribution.Votes.Count }).Distinct();

            foreach (var item in tempModel)
            {
                var userdata = new UserData();

                userdata.ChallengeName = item.Title;
                userdata.Votes = item.Count;
                newlist.Add(userdata);

            }

            return View(newlist);
        }

        // GET: About me
        public async Task<IActionResult> _AboutMe()
        {
            var vm = new AboutMeVm();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            vm.AboutMe = user.AboutMe;
            return View(vm);
        }

        // POST: About me
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _AboutMe(AboutMeVm vm)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {

                user.AboutMe = vm.AboutMe;
                _context.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

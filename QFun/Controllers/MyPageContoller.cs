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

namespace QFun.Controllers
{
    public class MyPageController : Controller
    {
        private readonly ILogger<MyPageController> _logger;
        private readonly ApplicationDbContext _context;
        public MyPageController(ILogger<MyPageController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult _Chart()
        {
            List<UserData> newlist = new List<UserData>();
            
            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var tempModel = (from a in _context.Challenge
                             join b in _context.Contribution on a.Id equals
                             b.ChallengeId
                             join c in _context.Vote on b.Id equals c.ContributionId where identity == b.UserId
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

        // GET: Movies/Edit/5
        public async Task<IActionResult> _AboutMe()
        {
            var vm = new AboutMeVm();
            return View(vm);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _AboutMe(AboutMeVm vm)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            user.AboutMe = vm.AboutMe;
            _context.Update(user);
            _context.SaveChanges();
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

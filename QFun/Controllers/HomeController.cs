using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QFun.Models;
using QFun.Models.HomePageVm;
using QFun.Services;

namespace QFun.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ContributionServices contributionServices;

        public HomeController(ILogger<HomeController> logger, ContributionServices contributionServices)
        {
            _logger = logger;
            this.contributionServices = contributionServices;
        }

        public IActionResult Index()
        {

            var vm = new HomeIndexVm();

            foreach (var contri in contributionServices.GetAllContributions())
            {
                var contriToAdd = new HomeIndexData();
                
                contriToAdd.Challenge = contri.Challenge;
                contriToAdd.User = contri.User;
                contriToAdd.Votes = contri.Votes.Count();
                contriToAdd.Path = contri.Path;
                contriToAdd.Description = contri.Description;

                vm.Contributions.Add(contriToAdd);
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

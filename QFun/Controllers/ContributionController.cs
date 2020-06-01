using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QFun.Models;

namespace QFun.Controllers
{
    public class ContributionController : Controller
    {
        // GET: Contribution
        public ActionResult Index()
        {
            return View();
        }

        // GET: Contribution/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Contribution/Create
        public ActionResult Create(ContributionsVm vm)
        {
            return View();
        }

        // POST: Contribution/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contribution/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Contribution/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Contribution/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Contribution/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
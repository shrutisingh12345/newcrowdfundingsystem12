using Crowd_Funding.Database;
using Crowd_Funding.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowd_Funding.Controllers
{
    public class DonationsController : Controller
    {

        private readonly MyDBContext db;

        public DonationsController(MyDBContext context)
        {
            db = context;
        }
        // GET: DonationsController
        public ActionResult Index(int CID=0)
        {
            List<Donation> Donations = new List<Donation>();
            if (CID > 0) Donations = db.Donations.Where(x => x.CID == CID).ToList();
            else
            {
                Donations = db.Donations.ToList();
            }
            return View(Donations);
        }

        // GET: DonationsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DonationsController/Create
        public ActionResult Create(int id)
        {
            if (id == 0) return null;
            ViewBag.CID = id;
            ViewBag.UID = db.Campaigns.FirstOrDefault(x=>x.CID==id).UID;

            return View();
        }

        // POST: DonationsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Donation donation)
        {
            if(ModelState.IsValid)
            {
                db.Donations.Add(donation);
                db.SaveChanges();
                return View("DonationSuccessful");
            }
            return View();
        }

        // GET: DonationsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DonationsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DonationsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DonationsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        
       
    }
}

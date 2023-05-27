using Crowd_Funding.Database;
using Crowd_Funding.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Crowd_Funding.Controllers
{
    public class CampaignsController : Controller
    {
        private readonly MyDBContext db;

        public CampaignsController(MyDBContext context)
        {
            db = context;
        }
        public ActionResult Index(string Keyword=null)
        {
            var UID = int.Parse(HttpContext.Session.GetString("UID"));
            if (string.IsNullOrEmpty(Keyword)) return View(db.Campaigns.Where(x => x.UID == UID).ToList());

            var campaigns = db.Campaigns.Where(x => x.Title.Contains(Keyword) && x.Description.Contains(Keyword) && x.UID == UID).ToList();
            return View(campaigns);
        }

        // GET: CampaignsController/Details/5
        public ActionResult Details(int CID)
        {
            ViewBag.CID = CID;
            var campaign = db.Campaigns.Find(CID);
            campaign.User = db.Users.Find(campaign.UID);
            ViewBag.Campaign = campaign;
            return View();
        }

        // GET: CampaignsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CampaignsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Campaign campaign)
        {
            var UID = int.Parse(HttpContext.Session.GetString("UID"));
            campaign.UID = UID;
            if (ModelState.IsValid)
            {
                db.Campaigns.Add(campaign);
                db.SaveChanges();


                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    var ID = campaign.CID;
                    if (file != null && file.Length > 0)
                    {

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Campaigns\\" + ID + ".jpg");

                        using (var stream = new FileStream(path, FileMode.OpenOrCreate))
                        {
                            file.CopyTo(stream);
                            stream.Close();
                            stream.Dispose();
                        }

                    }

                }




                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: CampaignsController/Edit/5
        public ActionResult Edit(int id)
        {
            var campaign = db.Campaigns.Find(id);
            return View(campaign);
        }

        // POST: CampaignsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Campaign campaign)
        {


            if (ModelState.IsValid)
            {
                var Existing_Campaign = db.Campaigns.Find(campaign.CID);

                Existing_Campaign.Title = campaign.Title;
                Existing_Campaign.Description = campaign.Description;
                db.SaveChanges();

                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    var ID = campaign.CID;
                    if (file != null && file.Length > 0)
                    {

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Campaigns\\" + ID + ".jpg");

                        using (var stream = new FileStream(path, FileMode.OpenOrCreate))
                        {
                            file.CopyTo(stream);
                            stream.Close();
                            stream.Dispose();
                        }

                    }

                }
               
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: CampaignsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CampaignsController/Delete/5
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
        public ActionResult ChangeStatus(int id = 0)
        {
            var campaign = db.Campaigns.Find(id);
            var UID = int.Parse(HttpContext.Session.GetString("UID"));
            if (!campaign.Status.Equals("Enabled")) campaign.Status = "Enabled";
            else campaign.Status = "Disabled";

            db.SaveChanges();
            var campaigns = db.Campaigns.Where(x => x.UID== UID).ToList();
            return View("Index", campaigns);
        }
        public ActionResult Search(string Keyword = null)
        {
            ViewBag.Keyword = Keyword;
            var Users = db.Users.ToList();
            List<Campaign> campaigns = null;
            if (string.IsNullOrEmpty(Keyword)) campaigns= db.Campaigns.Where(x=> x.AdminAction.Contains("Enabled") && x.Status=="Enabled").ToList();
            else  campaigns = db.Campaigns.Where(x => (x.Title.Contains(Keyword) || x.Description.Contains(Keyword) )&& x.AdminAction.Contains("Enabled") && x.Status == "Enabled").ToList();

            foreach (var campaign in campaigns)
            {
                campaign.User = Users.FirstOrDefault(x => x.UID == campaign.UID);
            }


            return View(campaigns);
        }

        public ActionResult AdminAllCampaigns(string Keyword = null)
        {
           
            return View(GetCampaigns(Keyword));
        }
        public ActionResult ChangeAdminStatus(int id,string Keyword)
        {
            var campaign = db.Campaigns.Find(id);
            
            if (!campaign.AdminAction.Equals("Enabled")) campaign.AdminAction = "Enabled";
            else campaign.AdminAction = "Disabled";

            db.SaveChanges();
           
            return RedirectToAction("AdminAllCampaigns", GetCampaigns(Keyword));
        }
        List<Campaign> GetCampaigns(string Keyword = null)
        {
            var Users = db.Users.ToList();
            List<Campaign> campaigns = null;
            if (string.IsNullOrEmpty(Keyword)) campaigns = db.Campaigns.ToList();
            else campaigns = db.Campaigns.Where(x => x.Title.Contains(Keyword) && x.Description.Contains(Keyword)).ToList();

            foreach (var campaign in campaigns)
            {
                campaign.User = Users.FirstOrDefault(x => x.UID == campaign.UID);
            }


            return campaigns;
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEWITender.Models;
using KEWITender.NAVWS;

namespace KEWITender.Controllers
{
    public class DashboardController : Controller
    {
        Procurement webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult Index()
        {
            if (Session["KRAPin"] == null)return RedirectToAction("index", "login");
            Account account = new Account();
            try
            {
                string path = Server.MapPath("~/Downloads/");
                if(Directory.Exists(path))Directory.CreateDirectory(path);
                string kraPin = Session["KRAPin"].ToString();
                string response = webportals.GetBidderDetails(kraPin);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters,StringSplitOptions.None);
                    account.KRAPin = responseArr[0];
                    account.CompanyName = responseArr[1];
                    account.EmailAddress = responseArr[2];
                    account.ContactPerson = responseArr[3];
                    account.ContactEmail = responseArr[4];
                    account.Address = responseArr[5];
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(account);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("index", "login");
        }
    }
}
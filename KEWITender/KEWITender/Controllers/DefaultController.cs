using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEWITender.Models;
using KEWITender.NAVWS;

namespace KEWITender.Controllers
{
    public class DefaultController : Controller
    {
        Procurement webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult Index()
        {
            Tender tender = new Tender();
            try
            {
                var list = new List<Tender>();
                string openTenders = webportals.GetOpenBidderTenders();
                if (!string.IsNullOrEmpty(openTenders))
                {
                    string[] openTendersArr = openTenders.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach(var openTender in openTendersArr)
                    {
                        string[] responseArr = openTender.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Tender()
                        {
                            TenderNo = responseArr[0],
                            RequisitionNo = responseArr[1],
                            Description = responseArr[2],
                            OpeningDate = Convert.ToDateTime(responseArr[3]),
                            ClosingDate = Convert.ToDateTime(responseArr[4])
                        });
                    }
                }
                tender.OpenTenders = list;
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(tender);
        }

        public ActionResult TenderLines(string tenderNo)
        {
            if (string.IsNullOrEmpty(tenderNo)) return RedirectToAction("index", "default");
            Tender tender = new Tender();
            try
            {
                var list = new List<Tender>();
                string tenderLines = webportals.GetOpenTenderLines(tenderNo);
                if (!string.IsNullOrEmpty(tenderLines))
                {
                    string[] tenderLinesArr = tenderLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach(var  tenderLine in tenderLinesArr)
                    {
                        string[] responseArr = tenderLine.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Tender()
                        {
                            ItemNo = responseArr[0],
                            Description = responseArr[1],
                            UnitOfMeasure = responseArr[2],
                            Quantity = Convert.ToDecimal(responseArr[3])
                        });
                    }
                }
                tender.TenderLines = list;
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(tender);
        }
    }
}
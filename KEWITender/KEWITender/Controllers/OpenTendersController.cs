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
    public class OpenTendersController : Controller
    {
        Procurement webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult Index()
        {
            if (Session["KRAPin"] == null) return RedirectToAction("index", "login");
            Tender tender = new Tender();
            try
            {
                var list = new List<Tender>();
                string openTenders = webportals.GetOpenBidderTenders();
                if (!string.IsNullOrEmpty(openTenders))
                {
                    string[] openTendersArr = openTenders.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in openTendersArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        DateTime closingDate = Convert.ToDateTime(responseArr[4]).AddDays(1);
                        if (closingDate < DateTime.Today) continue;
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
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(tender);
        }

        public ActionResult TenderLines(string tenderNo)
        {
            if (Session["KRAPin"] == null) return RedirectToAction("index", "login");
            Tender tender = new Tender();
            try
            {
                var list = GetTenderLines(tenderNo);
                tender.TenderLines = list;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(tender);
        }

        public ActionResult ApplyTender(string tenderNo)
        {
            if (Session["KRAPin"] == null) return RedirectToAction("index", "login");
            Tender tender = new Tender();
            try
            {
                string vatNo = Session["KRAPin"].ToString();
                if (webportals.TenderApplied(vatNo, tenderNo))
                {
                    TempData["Error"] = $"You have already submitted your bid for tender number {tenderNo}!";
                    return RedirectToAction("appliedtenders", "opentenders");
                }
                var list = GetTenderLines(tenderNo);
                tender.TenderLines = list;
                ViewBag.TenderNo = tenderNo;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(tender);
        }

        private List<Tender> GetTenderLines(string tenderNo)
        {
            var list = new List<Tender>();
            try
            {
                string openTenderLines = webportals.GetOpenTenderLines(tenderNo);
                if (!string.IsNullOrEmpty(openTenderLines))
                {
                    string[] openTenderLinesArr = openTenderLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in openTenderLinesArr)
                    {
                        string[] responseArr = line.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Tender()
                        {
                            ItemNo = responseArr[0],
                            Description = responseArr[1],
                            UnitOfMeasure = responseArr[2],
                            Quantity = Convert.ToDecimal(responseArr[3])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return list;
        }

        public ActionResult SubmitTenderApplication(Tender tender)
        {
            string tenderNo = tender.TenderNo;
            try
            {
                string vatNo = Session["KRAPin"].ToString();
                string categories = tender.SelectedCategories;
                string bidNo = webportals.CreateTenderSubmissionHeader(vatNo, tenderNo);
                if (!string.IsNullOrEmpty(bidNo))
                {
                    string[] categoriesArr = categories.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string category in categoriesArr)
                    {
                        int entryNo = webportals.GetLastEntry() + 1;
                        string[] responseArr = category.Split(strLimiters, StringSplitOptions.None);
                        string itemNo = responseArr[0];
                        decimal amount = Convert.ToDecimal(responseArr[1]);
                        webportals.InsertTenderSubmissionLine(vatNo,tenderNo,bidNo,itemNo,amount);
                    }
                    webportals.SubmitBid(vatNo, bidNo);

                    string path = Server.MapPath("~/Attachments/");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    if (tender.RFQForm != null && tender.RFQForm.ContentLength > 0)
                    {
                        string filename = bidNo.Replace("/", "") + tender.RFQForm.FileName;
                        string filepath = path + filename;
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }
                        tender.RFQForm.SaveAs(filepath);
                        System.IO.Stream fs = tender.RFQForm.InputStream;
                        System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                        Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        webportals.DocumentAttachment(bidNo, filename, base64String, 52178793);
                    }
                    TempData["Success"] = "Bid has been submitted successfuly";
                }
                return RedirectToAction("appliedtenders", "opentenders");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("applytender", "opentenders", new { tenderNo });
            }
        }

        public ActionResult AppliedTenders()
        {
            if (Session["KRAPin"] == null) return RedirectToAction("index", "login");
            Tender tender = new Tender();
            try
            {
                var list = GetMyQuotes();
                tender.AppliedTenders = list;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(tender);
        }

        public ActionResult BidLines(string bidNo)
        {
            if (Session["KRAPin"] == null) return RedirectToAction("index", "login");
            Tender tender = new Tender();
            try
            {
                string vatNo = Session["KRAPin"].ToString();
                var list = GetBidLines(vatNo, bidNo);
                tender.BidLines = list;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(tender);
        }

        private List<Tender> GetMyQuotes()
        {
            var list = new List<Tender>();
            try
            {
                string vatNo = Session["KRAPin"].ToString();
                string quotes = webportals.GetMyBids(vatNo);
                if (!string.IsNullOrEmpty(quotes))
                {
                    string[] quotesArr = quotes.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string quote in quotesArr)
                    {
                        string[] responseArr = quote.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Tender()
                        {
                            BidNo = responseArr[0],
                            TenderNo = responseArr[1],
                            Description = responseArr[2],
                            Date = Convert.ToDateTime(responseArr[3]),
                            OpeningDate = Convert.ToDateTime(responseArr[4]),
                            ClosingDate = Convert.ToDateTime(responseArr[5]),
                            Status = responseArr[6]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return list;
        }

        private List<Tender> GetBidLines(string vendorNo, string bidNo)
        {
            var list = new List<Tender>();
            try
            {
                string bidLines = webportals.GetMyBidLines(vendorNo, bidNo);
                if (!string.IsNullOrEmpty(bidLines))
                {
                    string[] bidLinesArr = bidLines.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string bidLine in bidLinesArr)
                    {
                        string[] responseArr = bidLine.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Tender()
                        {
                            ItemNo = responseArr[0],
                            Description = responseArr[1],
                            UnitOfMeasure = responseArr[2],
                            UnitCost = Convert.ToDecimal(responseArr[3]),
                            Quantity = Convert.ToDecimal(responseArr[4]),
                            Amount = Convert.ToDecimal(responseArr[5])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return list;
        }
    }
}
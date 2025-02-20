using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;
using KEWITender.Models;
using KEWITender.NAVWS;

namespace KEWITender.Controllers
{
    public class PrequalificationController : Controller
    {
        Procurement webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult PrequalificationApplication()
        {
            if (Session["KRAPin"] == null) return RedirectToAction("index", "login");
            Prequalification prequalification = new Prequalification();
            try
            {
                string vatNo = Session["KRAPin"].ToString();
                string bidderEmail = Session["CompanyEmail"].ToString();

                // Check if prequalification applied
                if (webportals.HasAppliedForPrequalification(vatNo, bidderEmail))
                {
                    TempData["Error"] = "You have already applied for prequalification in the current period!";
                    return RedirectToAction("prequalificationapplications", "prequalification");
                }

                var unselectedCategories = new List<Prequalification>();
                var selectedCategories = new List<Prequalification>();

                // Unapplied categories
                string unappliedCategories = webportals.GetUnAppliedCategories(vatNo);
                if (!string.IsNullOrEmpty(unappliedCategories))
                {
                    string[] unappliedCategoriesArr = unappliedCategories.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in unappliedCategoriesArr)
                    {
                        string[] itemArr = item.Split(strLimiters, StringSplitOptions.None);
                        unselectedCategories.Add(new Prequalification { Code = itemArr[0], Description = itemArr[1] });
                    }
                }

                // Applied categories
                string appliedCategories = webportals.GetAppliedCategories(vatNo);
                if (!string.IsNullOrEmpty(appliedCategories))
                {
                    string[] appliedCategoriesArr = appliedCategories.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in appliedCategoriesArr)
                    {
                        string[] itemArr = item.Split(strLimiters, StringSplitOptions.None);
                        selectedCategories.Add(new Prequalification { Code = itemArr[0], Description = itemArr[1] });
                    }
                }

                prequalification.UnAppliedCategories = unselectedCategories;
                prequalification.AppliedCategories = selectedCategories;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(prequalification);
        }

        public ActionResult AddCategory(string category)
        {
            if (Session["KRAPin"] == null) return RedirectToAction("index", "login");
            Prequalification prequalification = new Prequalification();
            try
            {
                string vatNo = Session["KRAPin"].ToString();
                var requirements = new List<Prequalification>();
                string categoryRequirements = webportals.GetCategoryRequirements(vatNo, category);
                if (!string.IsNullOrEmpty(categoryRequirements))
                {
                    string[] categoryRequirementsArr = categoryRequirements.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in categoryRequirementsArr)
                    {
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        requirements.Add(new Prequalification()
                        {
                            Description = responseArr[0],
                            Mandatory = responseArr[1],
                            Attached = responseArr[2]
                        });
                    }
                }
                prequalification.CategoryRequirements = requirements;
                ViewBag.Category = category;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(prequalification);
        }

        [HttpPost]
        public ActionResult SubmitCategoryDocument(Prequalification prequalification)
        {
            string category = prequalification.Category;
            try
            {
                string vatNo = Session["KRAPin"].ToString();
                string bidderEmail = Session["CompanyEmail"].ToString();
                string path = Server.MapPath("~/Attachments/");
                if (!webportals.PrequalificationApplied(vatNo))
                {
                    webportals.CreateTenderPrequalificationHeader(vatNo);
                }
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                if (prequalification.AttachmentFile != null && prequalification.AttachmentFile.ContentLength > 0)
                {
                    string filename = vatNo.Replace("/","") + prequalification.DocName;
                    string filepath = path + filename;
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                    prequalification.AttachmentFile.SaveAs(filepath);
                    System.IO.Stream fs = prequalification.AttachmentFile.InputStream;
                    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    string name = prequalification.DocName + ".pdf";
                    bool submitted = webportals.DocumentAttachment(vatNo, name, base64String, 52178541);
                }
                TempData["Success"] = $"{prequalification.DocName} has been uploaded successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("addcategory", "prequalification", new { category });
        }

        [HttpPost]
        public ActionResult SubmitCategory(Prequalification prequalification)
        {
            string category = prequalification.Category;
            try
            {
                string vatNo = Session["KRAPin"].ToString();
                string bidderEmail = Session["CompanyEmail"].ToString();
                if (webportals.NotAllMandatoryDocumentsUploaded(vatNo, category))
                {
                    TempData["Error"] = "Please upload all mandatory documents";
                    return RedirectToAction("addcategory", "prequalification", new { category });
                }
                if (!webportals.PrequalificationApplied(vatNo))
                {
                    webportals.CreateTenderPrequalificationHeader(vatNo);
                }
                webportals.CreatePrequalificationLine(vatNo, bidderEmail, category);
                TempData["Success"] = "Category added successfully!";
                return RedirectToAction("prequalificationapplication", "prequalification");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("addcategory", "prequalification", new { category });
            }
        }

        public ActionResult SubmitApplication()
        {
            try
            {
                string vatNo = Session["KRAPin"].ToString();
                string bidderEmail = Session["CompanyEmail"].ToString();
                if (!webportals.AddedCategory(vatNo))
                {
                    TempData["Error"] = "You have not added any prequalification category to your application!";
                    return RedirectToAction("prequalificationapplication", "prequalification");
                }
                webportals.SubmitPrequalificationApplication(vatNo, bidderEmail);
                TempData["Success"] = "Prequalification application submittedd successfully";
                return RedirectToAction("prequalificationapplications", "prequalification");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("prequalificationapplication", "prequalification");
            }
        }

        public ActionResult PrequalificationApplications()
        {
            if (Session["KRAPin"] == null) return RedirectToAction("index", "login");
            Prequalification prequalification = new Prequalification();
            try
            {
                string vatNo = Session["KRAPin"].ToString();
                var list = new List<Prequalification>();
                var myPrequalifications = webportals.GetMyPrequalificationApplications(vatNo);
                if (!string.IsNullOrEmpty(myPrequalifications))
                {
                    int counter = 0;
                    string[] myPrequalificationsArr = myPrequalifications.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in myPrequalificationsArr)
                    {
                        counter++;
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Prequalification()
                        {
                            Counter = counter,
                            Period = responseArr[0],
                            Prequalified = responseArr[1]
                        });
                    }
                }
                prequalification.AppliedCategories = list;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(prequalification);
        }

        public ActionResult PeriodCategories(string period)
        {
            if (Session["KRAPin"] == null) return RedirectToAction("index", "login");
            Prequalification prequalification = new Prequalification();
            try
            {
                string vatNo = Session["KRAPin"].ToString();
                var list = new List<Prequalification>();
                string categories = webportals.GetMyAppliedCategories(vatNo, period);
                if (!string.IsNullOrEmpty(categories))
                {
                    int counter = 0;
                    string[] categoriesArr = categories.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in categoriesArr)
                    {
                        counter++;
                        string[] responseArr = item.Split(strLimiters, StringSplitOptions.None);
                        list.Add(new Prequalification()
                        {
                            Counter = counter,
                            Period = responseArr[0],
                            CategoryName = responseArr[1],
                            CategoryApproved = responseArr[2],
                            Prequalified = responseArr[3]
                        });
                    }
                }
                prequalification.AppliedCategories = list;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(prequalification);
        }
    }
}
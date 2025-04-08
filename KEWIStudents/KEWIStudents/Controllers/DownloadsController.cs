using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEWIStudents.NAVWS;

namespace KEWIStudents.Controllers
{
    public class DownloadsController : Controller
    {
        Studentportal webportals = Components.ObjNav;
        public FileResult DownloadReceipt(string receiptNo)
        {
            string filename = receiptNo.Replace("/", "");
            string pdfFile = $"Receipt-{filename}.pdf";
            try
            {
                webportals.GenerateReceipt(receiptNo, pdfFile);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            string path = Server.MapPath("~/Downloads/" + pdfFile);
            return File(path, "application/pdf", pdfFile);
        }
    }
}
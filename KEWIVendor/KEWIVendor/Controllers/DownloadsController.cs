using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEWIVendor.NAVWS;

namespace KEWIVendor.Controllers
{
    public class DownloadsController : Controller
    {
        Procurement webportals = Components.ObjNav;
        public FileResult RfqReport(string tenderNo)
        {
            string filename = tenderNo.Replace("/", "");
            string pdfFile = $"RFQReport-{filename}.pdf";
            try
            {
                webportals.GenerateRFQReport(tenderNo, pdfFile);
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
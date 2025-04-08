using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using KEWIStudents.Models;
using KEWIStudents.NAVWS;
using Newtonsoft.Json;

namespace KEWIStudents.Controllers
{
    public class FinanceController : Controller
    {
        Studentportal webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        public ActionResult FeeStatement()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            return View();
        }

        public ActionResult PaymentReceipts()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            return View();
        }

        public ActionResult FeesPDF()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            try
            {
                string username = Session["username"].ToString();
                string filename = username.Replace("/", "");
                string pdfFile = $"FeeStatement-{filename}.pdf";
                webportals.GenerateFeesStatement(username, pdfFile);
                string filepath = Url.Content($"~/Downloads/{pdfFile}");
                ViewBag.filepath = filepath;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View();
        }

        public ActionResult FeesPayment()
        {
            if (Session["username"] == null) return RedirectToAction("index", "login");
            return View();
        }

        [HttpPost]
        public ActionResult FeesPayment(Finance finance)
        {
            try
            {
                decimal amount = finance.Amount;
                if (amount < 51)
                {
                    TempData["Error"] = "Amount plus convinience fees must be greater than 50KSHs.";
                    return RedirectToAction("feespayment", "finance");
                }
                string username = Session["username"].ToString();
                string email = string.Empty;
                string phoneno = string.Empty;
                string idno = string.Empty  ;
                string name = string.Empty;

                string response = webportals.GetStudentDetails(username);
                if(!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters,StringSplitOptions.None);
                    name = responseArr[1];
                    email = responseArr[2];
                    idno = responseArr[3];
                    phoneno = responseArr[4];
                }

                
                string secret = "qx9OsV5VsQ8f1KrQIp/qSnExyPqu09JJ";
                string key = "w2eb0OeDQ57V0+Ar";
                string apiClientID = "223";
                string serviceID = "5916454";
                string billDesc = "Fee Payment";
                string billRefNumber = webportals.GenerateBillRefNo();
                string currency = "KES";
                string clientMSISDN = phoneno;
                string clientName = name;
                string clientIDNumber = idno;
                string clientEmail = email;
                string callBackURLOnSuccess = "http://student.kewi.go.ke/dashboard";
                string pictureURL = "http://student.kewi.go.ke/images/logo.png";
                string notificationURL = "http://api.kewi.go.ke/api/b2b/feeadvice";
                string amountExpected = amount.ToString();
                string format = "json";
                string data = apiClientID + amountExpected + serviceID + clientIDNumber + currency + billRefNumber + billDesc + clientName + secret;
                string hash = GetHash(data, key);
                string secureHash = Base64Encode(hash);

                using (var client = new WebClient())
                {

                    EnableTrustedHosts();
                    client.Headers.Add("Content-Type", "application/json");
                    if (clientMSISDN.StartsWith("0"))
                    {
                        var mobileBuilder = new StringBuilder(clientMSISDN);
                        mobileBuilder.Remove(0, 1); //Trim one character from position 1
                        mobileBuilder.Insert(0, "254"); // replace position 0 with 254
                        clientMSISDN = mobileBuilder.ToString();
                    }
                    if (clientMSISDN.StartsWith("7"))
                    {
                        var mobileBuilder = new StringBuilder(clientMSISDN);
                        //mobileBuilder.Remove(0, 1); //Trim one character from position 1
                        mobileBuilder.Insert(0, "254"); // replace position 0 with 254
                        clientMSISDN = mobileBuilder.ToString();
                    }
                    var vm = new { apiClientID = apiClientID, secureHash = secureHash, serviceID = serviceID, billDesc = billDesc, billRefNumber = billRefNumber, currency = currency, clientMSISDN = clientMSISDN, clientName = clientName, clientIDNumber = clientIDNumber, clientEmail = clientEmail, callBackURLOnSuccess = callBackURLOnSuccess, notificationURL = notificationURL, pictureURL = pictureURL, amountExpected = amountExpected, format = format };

                    var dataString = JsonConvert.SerializeObject(vm);
                    string result = "";
                    result = client.UploadString("https://payments.ecitizen.go.ke/PaymentAPI/iframev2.1.php", "POST", dataString);

                    Stkresult details = null;


                    details = JsonConvert.DeserializeObject<Stkresult>(result);

                    if (!string.IsNullOrEmpty(details.invoice_number) && !string.IsNullOrEmpty(details.invoice_link))
                    {
                        webportals.SavePesaFlowInvoice(billRefNumber, details.invoice_number, username, clientName, Convert.ToDecimal(amountExpected), serviceID, billDesc, secureHash, details.invoice_link);
                        return Redirect(details.invoice_link);
                    }
                    else
                    {
                        TempData["Error"] = "An Error has occured. Payment checkout not processed";
                        return RedirectToAction("feespayment", "finance");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("feespayment", "finance");
        }

        private static void EnableTrustedHosts()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                 ((sender, certificate, chain, sslPolicyErrors) => true);
        }
        private static String GetHash(String text, String key)
        {
            // change according to your needs, an UTF8Encoding
            // could be more suitable in certain situations
            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private partial class Stkresult
        {
            public string invoice_number { get; set; }
            public string invoice_link { get; set; }
        }
    }
}
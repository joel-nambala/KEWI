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
    public class LoginController : Controller
    {
        Procurement webportals = Components.ObjNav;
        string[] strLimiters = new string[] { "::" };
        string[] strLimiters2 = new string[] { "[]" };
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account account)
        {
            try
            {
                string kraPin = account.KRAPin.ToUpper();
                string password = account.Password;
                if (!webportals.ValidBidderNo(kraPin))
                {
                    TempData["Error"] = "Ivalid KRA Pin number. Please try again later!";
                    return RedirectToAction("index", "login");
                }

                if (!webportals.HasActivatedAccount(kraPin))
                {
                    TempData["Error"] = "Account not activated. Please follow the link sent to your email address to activate your account!";
                    return RedirectToAction("index", "login");
                }

                if (!webportals.CheckPasswordChanged(kraPin))
                {
                    TempData["Error"] = "Please update your password before you continue!";
                    return RedirectToAction("resetpassword", "login", new { kraPin });
                }

                string response = webportals.BidderLogin(kraPin, password);
                if (!string.IsNullOrEmpty(response))
                {
                    string[] responseArr = response.Split(strLimiters, StringSplitOptions.None);
                    string pin = responseArr[0];
                    string name = responseArr[1];
                    string email = responseArr[2];
                    Session["KRAPin"] = pin;
                    Session["CompanyName"] = name;
                    Session["CompanyEmail"] = email;
                    return RedirectToAction("index", "dashboard");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("index", "login");
        }

        public ActionResult CreateAccount()
        {
            try
            {
                ViewBag.cities = GetCities();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View();
        }

        private List<Account> GetCities()
        {
            List<Account> cities = new List<Account>();
            try
            {
                string strCities = webportals.GetCities();
                string[] strCity = strCities.Split(strLimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string city in strCity)
                {
                    cities.Add(new Account { City = city });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return cities;
        }

        [HttpPost]
        public ActionResult CreateAccount(Account account)
        {
            try
            {
                string kraPin = account.KRAPin;
                string companyName = account.CompanyName;
                string address = account.Address;
                string companyPhone = account.CompanyPhone;
                string city = account.City;
                string companyEmail = account.CompanyEmail;
                string contactPerson = account.ContactPerson;
                string contactPhone = account.ContactPhone;
                string contactEmail = account.ContactEmail;
                string sessionKey = Guid.NewGuid().ToString();
                string path = Server.MapPath("~/Attachments/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (account.KRACert != null && account.KRACert.ContentLength > 0)
                {
                    string filename = $"{account.KRAPin} KRA Certificate.pdf";
                    string filepath = path + filename;
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                    account.KRACert.SaveAs(filepath);
                    System.IO.Stream fs = account.KRACert.InputStream;
                    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    string name = "KRA Certificate.pdf";
                    webportals.DocumentAttachment(account.KRAPin, name, base64String, 52178793);
                }
                if (account.IncopCert != null && account.IncopCert.ContentLength > 0)
                {
                    string filename = account.KRAPin + " Certificate of Incorporation.pdf";
                    string filepath = path + filename;
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                    account.IncopCert.SaveAs(filepath);
                    System.IO.Stream fs = account.IncopCert.InputStream;
                    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    string name = "Certificate of Incorporation.pdf";
                    webportals.DocumentAttachment(account.KRAPin, name, base64String, 52178793);
                }
                if (account.CompCert != null && account.CompCert.ContentLength > 0)
                {
                    string filename = account.KRAPin + " KRA Compliance Certificate.pdf";
                    string filepath = path + filename;
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                    account.CompCert.SaveAs(filepath);
                    System.IO.Stream fs = account.CompCert.InputStream;
                    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    string name = "KRA Compliance Certificate.pdf";
                    webportals.DocumentAttachment(account.KRAPin, name, base64String, 52178793);
                }
                webportals.CreateBidderAccount(kraPin, companyName, address, companyPhone, companyEmail, contactPerson, contactEmail, city, sessionKey);

                // Send account confirmation email
                string subject = "Kenya Water Institute Tender Portal Account Confirmation";
                string message = $"Dear {companyName}" +
                    $"<br/><br/>" +
                    $"Please follow the link below to confirm your account creation." +
                    $"<br/><br/>" +
                    $"<a href='{Request.Url.Scheme}://{Request.Url.Authority}/login/activateaccount?kraPin={kraPin}'>Click here</a>" +
                    $"<br/><br/>" +
                    $"This email is system generated. Do not reply!";
                Components.SentEmailAlerts(companyEmail, subject, message);
                TempData["Success"] = "Account has been created successfully. Please check your email to activate your account.";
                return RedirectToAction("index", "login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("createaccount", "login");
            }
        }

        public ActionResult ActivateAccount(string kraPin)
        {
            try
            {
                webportals.ActivateBidderAccount(kraPin);
                TempData["Success"] = "Account has been activated successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
                return RedirectToAction("resetpassword", "login", new { kraPin });
        }

        public ActionResult ResetPassword(string kraPin)
        {
            try
            {
                if (webportals.HasActivatedAccount(kraPin))
                {
                    Session["KRAPin"] = kraPin;
                }
                else
                {
                    TempData["Error"] = "Account not activated. Please try again!";
                    return RedirectToAction("index", "login");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(Account account)
        {
            try
            {
                string password = account.Password;
                string kraPin = Session["KRAPin"].ToString();
                webportals.UpdateBidderPassword(kraPin, password);
                TempData["Success"] = "Password has been updated successfully";
                return RedirectToAction("index", "login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("resetpassword", "login");
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(Account account)
        {
            try
            {
                string kraPin = account.KRAPin;
                if (!webportals.ValidBidderNo(kraPin))
                {
                    TempData["Error"] = "Ivalid KRA Pin number. Please try again later!";
                    return RedirectToAction("forgotpassword", "login");
                }
                return RedirectToAction("resetpassword", "login", new { kraPin });
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("forgotpassword", "login");
            }
        }
    }
}
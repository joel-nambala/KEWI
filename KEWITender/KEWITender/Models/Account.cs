using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEWITender.Models
{
	public class Account
	{
        public string KRAPin { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string City { get; set; }
        public HttpPostedFileBase KRACert { get; set; }
        public HttpPostedFileBase IncopCert { get; set; }
        public HttpPostedFileBase CompCert { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEWIVendor.Models
{
	public class Account
	{
        public string VATNo { get; set; }
        public string VendorNo { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string VendorName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonPhone { get; set; }
    }
}
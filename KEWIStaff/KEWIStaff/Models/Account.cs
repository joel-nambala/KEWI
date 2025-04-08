using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEWIStaff.Models
{
	public class Account
	{
        public string Username { get; set; }
        public string Password { get; set; }
        public string StaffNo { get; set; }
        public string StaffName { get; set; }
        public string JobTitle { get; set; }
        public string Gender { get; set; }
        public string IdNumber { get; set; }
        public string EmailAddress { get; set; }
        public string PostalAddress { get; set; }
        public string PhoneNumber { get; set; }        
    }
}
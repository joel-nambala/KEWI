using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEWIStudents.Models
{
	public class Account
	{
        public string Username { get; set; }
        public string Password { get; set; }
        public string StudentNo { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string IdNumber { get; set; }
        public string PhoneNo { get; set; }
        public string PostalAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Program { get; set; }
        public string ProgramName { get; set; }
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public string SettlementType { get; set; }
        public string Stage { get; set; }
        public string YearOfStudy { get; set; }
    }
}
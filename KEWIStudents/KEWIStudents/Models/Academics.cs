using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEWIStudents.Models
{
	public class Academics
	{
        public int Counter { get; set; }
        public string StudentNo { get; set; }
        public string StudentName { get; set; }
        public string Program { get; set; }
        public string ProgramName { get; set; }
        public string Stage { get; set; }
        public string SettlementType { get; set; }
        public string NextStage { get; set; }
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public int YearOfStudy { get; set; }
    }
}
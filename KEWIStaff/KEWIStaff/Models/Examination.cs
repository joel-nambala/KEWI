using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEWIStaff.Models
{
	public class Examination
	{
        public string StaffNo { get; set; }
        public string StaffName { get; set; }
        public string StudentNo { get; set; }
        public string StudentName { get; set; }
        public string Programme { get; set; }
        public string ProgramName { get; set; }
        public string Unit { get; set; }
        public string UnitDescription { get; set; }
        public string Semester { get; set; }
        public string Stage { get; set; }
        public string Campus { get; set; }
        public string TransactionId { get; set; }
        public string MaxPracticalScore { get; set; }
        public string MaxCatScore { get; set; }
        public string MaxExamScore { get; set; }
        public string LockCatEditing { get; set; }
        public string LockExamEditing { get; set; }
        public string CatMarks { get; set; }
        public string PracticalMarks { get; set; }
        public string ExamMarks { get; set; }
        public decimal Score { get; set; }
        public List<Examination> LecturerStudents { get; set; }
    }
}
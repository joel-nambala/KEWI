using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEWIStudents.Models
{
	public class Finance
	{
        public string DocumentNo { get; set; }
        public string PostingDate { get; set; }
        public string Description { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal Balance { get; set; }
        public string ReceiptNo { get; set; }
        public string PaymentMode { get; set; }
        public string StudentName { get; set; }
        public string StudentNo { get; set; }
        public decimal Amount { get; set; }
    }
}
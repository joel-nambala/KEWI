using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEWIVendor.Models
{
	public class Tender
	{
        public string BidNo { get; set; }
        public string TenderNo { get; set; }
        public string RequisitionNo { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public string ItemNo { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal Quantity { get; set; }
        public string SelectedCategories { get; set; }
        public string Status { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Amount { get; set; }
        public HttpPostedFileBase RFQForm { get; set; }
        public List<Tender> OpenTenders { get; set; }
        public List<Tender> TenderLines { get; set; }
        public List<Tender> AppliedTenders { get; set; }
        public List<Tender> BidLines { get; set; }
    }
}
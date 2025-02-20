using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEWITender.Models
{
	public class Prequalification
	{
        public int Counter { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Mandatory { get; set; }
        public string Attached { get; set; }
        public string Category { get; set; }
        public string DocName { get; set; }
        public string Period { get; set; }
        public string CategoryName { get; set; }
        public string CategoryApproved { get; set; }
        public string Prequalified { get; set; }
        public HttpPostedFileBase AttachmentFile { get; set; }
        public List<Prequalification> UnAppliedCategories { get; set; }
        public List<Prequalification> AppliedCategories { get; set; }
        public List<Prequalification> CategoryRequirements { get; set; }
    }
}
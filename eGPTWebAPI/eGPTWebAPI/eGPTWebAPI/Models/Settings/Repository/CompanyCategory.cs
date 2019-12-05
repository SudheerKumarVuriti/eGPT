using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGPTWebAPI.Models.Settings.Repository
{
    public class CompanyCategory
    {

        public int CompanyId { get; set; }

        public string CompanyCategoryName { get; set; }

        public string CompanyCategoryDescription { get; set; }

        public string NodeType { get; set; } 

        public bool Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int LastUpdatedBy { get; set; }

        public DateTime LastUpdatedOn { get; set; }

    }
}
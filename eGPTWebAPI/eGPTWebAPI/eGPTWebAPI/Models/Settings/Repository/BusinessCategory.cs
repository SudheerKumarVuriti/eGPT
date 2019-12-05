using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGPTWebAPI.Models.Settings.Repository
{
    public class BusinessCategory
    {

        public int BizId { get; set; }

        public string BizCategoryName { get; set; }

        public string BizCategoryDescription { get; set; }

        public bool Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int LastUpdatedBy { get; set; }

        public DateTime LastUpdatedOn { get; set; }

    }
}
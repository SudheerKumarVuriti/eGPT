using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGPTWebAPI.Models.Settings.Repository
{
    public class Designation
    {
        public string DesignationName { get; set; }

        public string DesignationDescription { get; set; }

        public int LastUpdatedBy { get; set; }

        public DateTime LastUpdated { get; set; }

        public int DesignationId { get; set; }

        public bool Status { get; set; }
    }
}
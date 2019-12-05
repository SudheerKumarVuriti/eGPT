using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGPTWebAPI.Models.Settings.Repository.Common
{
    public class BloodGroup
    {
        public string BloodGroupName { get; set; }

        public string BloodGroupDescription { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime LastUpdated { get; set; }

        public int LastUpdatedBy { get; set; }

        public int BloodGroupId { get; set; }

        public bool Status { get; set; }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGPTWebAPI.Models.Settings.Repository.Common
{
    public class VisaType
    {
        public string TypeOfVisa { get; set; }

        public string VisaTypeDescription { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime LastUpdated { get; set; }

        public int LastUpdatedBy { get; set; }

        public int VisaId { get; set; }

        public int Status { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGPTWebAPI.Models.Settings.Repository.Common
{
    public class Country
    {
        public string CountryName { get; set; }

        public string CountryDescription { get; set; }

        public DateTime LastUpdated { get; set; }

        public int LastUpdatedBy { get; set; }

        public DateTime CountryId { get; set; }

        public bool Status { get; set; }
    }
}
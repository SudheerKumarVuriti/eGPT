using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGPTWebAPI.Models.Settings.Repository.Common
{
    public class Currency
    {
        public string CurrencyName { get; set; }

        public string CurrencyDescription { get; set; }

        public DateTime LastUpdated { get; set; }

        public int LastUpdatedBy { get; set; }

        public int CurrencyId { get; set; }

        public bool Status { get; set; }
       
    }
}
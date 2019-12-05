using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGPTWebAPI.Models.Settings.Repository.Common
{
    public class UnitOfMeasure
    {
        public string UnitOfMeasureName { get; set; }

        public string UnitOfMeasureDescription { get; set; }

        public DateTime LastUpdated { get; set; }

        public int LastUpdatedBy { get; set; }

        public int UnitId { get; set; }

        public bool Status { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGPTWebAPI.Models
{
    public class EGPTStaticMethods
    {
        /// <summary>
        /// Replaces With Special Characters.
        /// </summary>
        /// <param name="str">string.</param>
        /// <returns>Replaces With Special Characters.</returns>
        public static string replaceWithSplChars(string str)
        {
            string res = str;

            //Note : Do not change the order. Here the order is So Important.
            if (!String.IsNullOrEmpty(str))
            {
                str = str.TrimStart(' ');
                str = str.TrimEnd(' ');
                res = res.Replace("\\", "\\\\");
                res = res.Replace("_", "\\_");
                res = res.Replace("[", "\\[");
                res = res.Replace("]", "\\]");
                res = res.Replace("%", "\\%");
                res = res.Replace("^", "\\^");
                res = res.Replace("'", "''");
                res = res.Replace(":", "\\"); //If i search with : it is giving error                

            }
            return res;
        }
    }
}
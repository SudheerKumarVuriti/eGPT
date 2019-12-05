using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace eGPTWebAPI
{
    /// <summary>
    /// GlobalObjects class
    /// </summary>
    public class GlobalObjects
    {
        # region Log File Details
        /// <summary>
        /// logFilePath variable
        /// </summary>
        public string logFilePath = @"D:\eGPT\Log\AppSLog\";
        //public string logFilePath = ConfigurationManager.AppSettings["errorLogFolderPath"].ToString();D:\SmartFSMforJio\Log\Jio_smartFSMWebSLog
        //public string logFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\Jio_smartFSM\";
        /// logFileName variable
        /// </summary>
        public string logFileName = "AppSLog";
        //public string logFileName = ConfigurationManager.AppSettings["logFileFolder"].ToString();
        /// <summary>
        /// logFileSize variable
        /// </summary>
        public long logFileSize = 1000000;
        /// <summary>
        /// noOfLogBackUp variable
        /// </summary>
        public int noOfLogBackUp = 50;
        /// <summary>
        /// hostName variable
        /// </summary>
        public string hostName = "";
        /// <summary>
        /// lnkNXTVersion variable
        /// </summary>
        public string FoS = "1.0";
        #endregion

        /// <summary>
        /// GlobalObjects method
        /// </summary>
        /// <param name="_logFilePath">a string _logFilePath value</param>
        /// <param name="_logFileName">a string _logFileName value</param>
        //public GlobalObjects(string _logFilePath, string _logFileName)
        //{
        //    logFilePath = _logFilePath;
        //    logFileName = _logFileName;
        //}


    }
}

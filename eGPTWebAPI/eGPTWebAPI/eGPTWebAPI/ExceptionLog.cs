
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eGPTWebAPI
{
    /// <summary>
    /// ExceptionLog class
    /// </summary>
    public static class ExceptionLog
    {

        /// <summary>
        /// Function to Write Error Logs to txt file
        /// Parameters  :2)function: Function Name from which you are passing this log message
        ///              3)description: Log description
        /// </summary>
        public static void WriteErrorLog(string function, string description, string stackDescription)
        {
            try
            {
                GlobalObjects globalObjects = new GlobalObjects();
                
                //string szLogFolder = "WhatsAppAPIServer\\";

                if (Directory.Exists(globalObjects.logFilePath) == false)
                {
                    Directory.CreateDirectory(globalObjects.logFilePath);
                }

                StreamWriter FileWriteFun = new StreamWriter(globalObjects.logFilePath + globalObjects.logFileName + ".Log", true);
                String szWriteLine;
                string szDate;
                szDate = DateTime.Now.ToString("ddMMMyyyy HH:mm:ss.fff tt");

                szWriteLine = szDate + " | [Error] | [" + globalObjects.logFileName + "] | " + function + " | " + description + " : [StackTrace] :  " + stackDescription;
                FileWriteFun.WriteLine(szWriteLine);
                FileWriteFun.Close();

                //check file size and take back up
                CheckFileSize(globalObjects.logFilePath, globalObjects.logFileName, globalObjects.logFileSize, globalObjects);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Function to Write Informative Logs to txt file
        ///             1)function: Function Name from which you are passing this log message
        ///             2)description: Log description
        /// </summary>
        public static void WriteInfoLog(string function, string description)
        {
            try
            {
                GlobalObjects globalObjects = new GlobalObjects();
                if (Directory.Exists(globalObjects.logFilePath) == false)
                {
                    Directory.CreateDirectory(globalObjects.logFilePath);
                }

                StreamWriter FileWriteFun = new StreamWriter(globalObjects.logFilePath + globalObjects.logFileName + ".Log", true);
                String szWriteLine;
                string szDate;
                szDate = DateTime.Now.ToString("ddMMMyyyy HH:mm:ss.fff tt");

                szWriteLine = szDate + " | [Info]  | [" + globalObjects.logFileName + "] | " + function + " | " + description;
                FileWriteFun.WriteLine(szWriteLine);
                FileWriteFun.Close();

                //check file size and take back up
                CheckFileSize(globalObjects.logFilePath, globalObjects.logFileName, globalObjects.logFileSize, globalObjects);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Function to Check the All logs file size, as configured in XML file
        /// </summary>
        /// <param name="strFilePath"> Path of the file from where you can read the file size</param>
        /// <param name="strFileName">Name of the file to generate txt file</param>
        /// <param name="nMaxFileSize"> max File size to be checked as given in Config XML File</param>
        /// <param name="globalObjects">Object of the GlobalObjects to get global variables  </param>
        public static void CheckFileSize(String strFilePath, String strFileName, long nMaxFileSize, GlobalObjects globalObjects)
        {
            try
            {
                long nSize = 0;
                Boolean bMoved = false;
                string szOldFileName;
                String szNewFileName;

                if (File.Exists((strFilePath + strFileName + ".Log")) == true)
                {
                    szOldFileName = strFilePath + strFileName + ".Log";
                    nSize = File.ReadAllBytes(szOldFileName).Length;
                }
                else
                {
                    szOldFileName = "";
                }

                //'check file size for more than 3 MB.
                if (nSize >= nMaxFileSize)
                {
                    //Check Files
                    bMoved = false;

                    //loop to check last 5 files
                    for (int nRec = 1; nRec < globalObjects.noOfLogBackUp + 1; nRec++)
                    {
                        szNewFileName = strFilePath + "Backup" + nRec + "_" + strFileName + ".Log";
                        if (File.Exists(szNewFileName) == false)
                        {
                            File.Copy(szOldFileName, szNewFileName);
                            File.Delete(szOldFileName);
                            bMoved = true;
                            break;
                        }
                    }

                    string sSourceFile;
                    string sDestFile;
                    //check if file moved or not
                    if (bMoved == false)
                    {
                        File.Delete(strFilePath + "Backup1_" + strFileName + ".Log");

                        //loop to check no of back up
                        for (int nRec = 1; nRec < globalObjects.noOfLogBackUp; nRec++)
                        {
                            if (nRec == globalObjects.noOfLogBackUp)
                            {
                                sSourceFile = strFilePath + strFileName + ".Log";
                                sDestFile = strFilePath + "Backup" + nRec + "_" + strFileName + ".Log";
                                File.Copy(sSourceFile, sDestFile);
                                File.Delete(sSourceFile);
                            }
                            else
                            {
                                sSourceFile = strFilePath + "Backup" + (nRec + 1) + "_" + strFileName + ".Log";
                                sDestFile = strFilePath + "Backup" + nRec + "_" + strFileName + ".Log";
                                File.Copy(sSourceFile, sDestFile);
                                File.Delete(sSourceFile);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteErrorLog("CheckFileSize", ex.Message, ex.StackTrace);
            }
        }


        /// <summary>
        /// writes message to the text file.
        /// </summary>
        /// <param name="message"></param>
        public static void WriteMessage(string message)
        {
            StreamWriter sw = null;
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Log.txt";
                //sw = new StreamWriter("c:/tmp.txt",true);
                sw = new StreamWriter(path, true);
                //sw.WriteLine("\r\n");
                DateTime dt = DateTime.Now;
                sw.WriteLine("Date and Time : " + dt);
                //sw.WriteLine("Help Link : "+ex.HelpLink);

                sw.WriteLine(message);

            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }
    }
}

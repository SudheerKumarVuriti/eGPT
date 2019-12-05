using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace eGPTWebAPI.Models
{
    public class ConOperation
    {

        //public static String strReg = "^([0-9a-zA-Z]([-\\.\\w])*@([0-9a-zA-Z][-\\w]*\\.)+[a-zA-Z]{2,9})$";
        public static String strReg = "^([0-9a-zA-Z .])";
        private static byte[] KEY_128 = { 42, 1, 52, 67, 231, 13, 94, 101, 123, 6, 0, 12, 32, 91, 4, 111, 31, 70, 21, 141, 123, 142, 234, 82, 95, 129, 187, 162, 12, 55, 98, 23 };
        private static byte[] IV_128 = { 234, 12, 52, 44, 214, 222, 200, 109, 2, 98, 45, 76, 88, 53, 23, 78 };
        public ConOperation()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static string ConString()
        {
            string connectionString = Decrypt(ConfigurationManager.ConnectionStrings["eGPTDB"].ConnectionString);
            return connectionString;
        }

        /// <summary>
        /// A Static method to decrypt the plain text
        /// </summary>
        /// <param name="encryptedText">encryptedText</param>
        /// <returns>returns the decrypted value</returns>
        public static string Decrypt(string encryptedText)
        {
            string sDecryptedValue = "";
            using (RijndaelManaged oDecryptor = new RijndaelManaged())
            {
                try
                {
                    oDecryptor.Key = KEY_128; oDecryptor.IV = IV_128;
                    using (MemoryStream msDecrypt = new MemoryStream(System.Convert.FromBase64String(encryptedText)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, oDecryptor.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                sDecryptedValue = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                finally
                {
                    if (oDecryptor != null)
                    {
                        oDecryptor.Clear();
                    }
                }
            }
            return sDecryptedValue;
        }

        public static bool IsRegexValid(string st, string format)
        {
            bool isValid = false;
            if (st == "" || st == null)
            {
                isValid = true;
            }
            else
            {
                if (!String.IsNullOrEmpty(st) && !String.IsNullOrEmpty(format))
                {
                    if (Regex.IsMatch(st, format))
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
        }

        public static Hashtable ConvertDataTableToHashtable(DataTable dt)
        {
            Hashtable ht = new Hashtable();
            int i = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                ht.Add(dt.Columns[i].ToString(), dt.Rows[0][dt.Columns[i].ToString()]);
                i++;
            }
            return ht;
        }

        public static string strErrorMsgForUser = "Error...";
        public string base64EEncode(string sData)
        {
            try
            {
                return sData;

            }
            catch (Exception ex)
            {
                ExceptionLog.WriteErrorLog("base64Encode", ex.Message, ex.StackTrace);
                throw new Exception(strErrorMsgForUser);
            }
        }

        public static string RandomString(int length)
        {
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return res.ToString();
        }

        public object ConvertData(object obj)
        {
            var list = JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return list;
        }

        public object DataTableToJSONWithJSONNet(object obj)
        {
            var list = JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return list;
        }

        public string GetResultString(string strFuncName, object inputData, string strinputType)
        {
            string strResult = String.Empty;
            try
            {
                switch (strinputType)
                {
                    case "Datatable":
                        strResult = Convert.ToString(DataTableToJSONWithJSONNet(inputData));
                        break;
                    case "DataSet":
                        strResult = Convert.ToString(ConvertData(inputData));
                        break;

                    case "ArrayList":
                    case "Hashtable":
                        strResult = (new JavaScriptSerializer()).Serialize(inputData);
                        break;

                    case "String":
                        strResult = Convert.ToString(inputData);
                        break;
                }
            }
            catch (Exception e)
            {
                ExceptionLog.WriteErrorLog(strFuncName, e.Message, e.StackTrace);
                throw e;
            }
            strResult = strResult.Replace(@"\r\n", "");
            return strResult;
        }
    }
}
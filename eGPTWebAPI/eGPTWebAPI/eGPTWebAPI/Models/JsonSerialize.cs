using System;
using System.Collections.Generic;
using System.Collections;
using System.Web.Script.Serialization;


namespace eGPTWebAPI.Models
{
    /// <summary>
    /// Summary description for JsonSerialize
    /// </summary>
    public class JsonSerialize
    {
        #region Dictionary convert
        public List<Dictionary<String, String>> DictionaryToList(Dictionary<String, String> result, String value)
        {
            List<Dictionary<String, String>> lstResult = new List<Dictionary<string, string>>();
            Dictionary<String, String> dicResults;
            if (result != null)
            {
                foreach (KeyValuePair<String, String> dicResult in result)
                {
                    dicResults = new Dictionary<string, string>();
                    dicResults.Add(value + "Code", dicResult.Key.ToString());
                    dicResults.Add(value + "Name", dicResult.Value.ToString());
                    lstResult.Add(dicResults);
                }
            }
            return lstResult;
        }
        #endregion
        #region Serliaze the Data
        public String JsonSerializer(String status, Object result, String token)
        {

            String resultStr = String.Empty;
            Hashtable ht = new Hashtable();
            JavaScriptSerializer sr = new JavaScriptSerializer();
            if (token != null)
            {

                if (token.Equals(ContentValues.INVALID_SESSION))
                {
                    resultStr = "{\"Status\":\"" + token + "\",\"Result\": {}}";
                }
                else if (token.Equals(ContentValues.INVALID_CREDENTIALS))
                {
                    resultStr = "{\"Status\":\"" + token + "\",\"Result\": {}}";
                }
                else
                {
                    if (result != null)
                    {
                        ht["Status"] = status;
                        ht["Result"] = result;
                        ht["SessionId"] = token;
                        resultStr = sr.Serialize(ht);
                    }
                    else
                    {
                        resultStr = "{\"Status\":\"" + status + "\",\"Result\": {},\"SessionId\":\"" + token + "\"}";
                    }
                }
            }
            else
                resultStr = "{\"Status\":\"" + status + "\",\"Result\": {},\"SessionId\":\"" + ContentValues.UNRECOGNIZED_PROCESS + "\"}";

            return resultStr;
        }

        public String JsonSerializer(String status, Object result, String type, String typeOfMethod, String token)
        {
            String resultStr = String.Empty;
            Hashtable ht = new Hashtable();
            JavaScriptSerializer sr = new JavaScriptSerializer();
            try
            {

                if (token.Equals(ContentValues.INVALID_SESSION))
                {
                    resultStr = "{\"Status\":\"" + token + "\",\"Result\": {}}";
                }
                else if (token.Equals(ContentValues.INVALID_CREDENTIALS))
                {
                    resultStr = "{\"Status\":\"" + token + "\",\"Result\": {}}";
                }
                else
                {

                    if (typeOfMethod.Equals("Get"))
                    {
                        if (type.Equals("Dic"))
                        {
                            if (resultStr != null)
                            {

                                ht["Status"] = status;
                                ht["Result"] = result;
                                ht["SessionId"] = token;
                                resultStr = sr.Serialize(ht);
                            }
                            else
                            {
                                resultStr = "{\"Status\":\"" + status + "\",\"Result\": {},\"SessionId\":\"" + token + "\"}";
                            }
                        }
                        else if (type.Equals("Array"))
                        {
                            if (resultStr != null)
                            {
                                ht["Status"] = status;
                                ht["Results"] = result;
                                ht["SessionId"] = token;
                                resultStr = sr.Serialize(ht);
                            }
                            else
                            {
                                resultStr = "{\"Status\":\"" + status + "\",\"Result\": []}";
                            }
                        }
                    }
                    else if (typeOfMethod.Equals("Save"))
                    {
                        if (status.Equals(ContentValues.INSERTION_FAILED)) //if insertion failed i.e., web service is successful and transaction to db failed
                        {
                            resultStr = "{\"Status\":\"" + status + "\",\"Result\": {},\"SessionId\":\"" + token + "\"}";
                        }
                        else if (status.Equals(ContentValues.Ok))//insertion Successful
                        {
                            ht["Status"] = status;
                            ht["SessionId"] = token;
                            ht["Result"] = result;
                            resultStr = sr.Serialize(ht);
                        }
                        else//Any error other than Insertion Failure
                        {
                            ht["Status"] = status;
                            ht["SessionId"] = token;
                            ht["Result"] = result;
                            resultStr = sr.Serialize(ht);
                        }

                    }
                }
            }
            catch (Exception ex)
            { }
            return resultStr;
        }
        #endregion
    }
}
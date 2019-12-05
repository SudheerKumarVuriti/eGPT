using System.Web.Http;
using System.Configuration;
using eGPTWebAPI.Models;
using System.Collections;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Data;
using System;
using eGPTWebAPI.Models.Settings.Repository;
using System.Text;
using eGPTWebAPI.Models.Settings.Repository.Common;

namespace eGPTWebAPI.Controllers.Settings.Repository.Common
{
    public class CurrencyController : ApiController
    {
        SqlConnection _conn = new SqlConnection(
         ConfigurationManager.ConnectionStrings["eGPTDBB"].ToString());
        Hashtable htResult = new Hashtable();
        public string strErrorMsgForUser = "Error...";
        JavaScriptSerializer ser = new JavaScriptSerializer();
        JsonSerialize js = new JsonSerialize();
        string Result = "";

        // GET api/Currency
        public string GetCurrencySummary()
        {
            ExceptionLog.WriteInfoLog("Get", "GetCurrencySummary");

            #region GetDesignation
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            try
            {
                DataSet dsCurrency = new DataSet();
                var qryCurrnecy = "select * from TBL_EFW_Currnecy";

                dsCurrency = dbOperations.GetDataSetforQuery(qryCurrnecy);

                if (dsCurrency != null && dsCurrency.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetCurrency", dsCurrency, "DataSet")));
                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "OK");
                }
                else
                {
                    htResult.Add("Result", "");
                    htResult.Add("Status", "Failed");
                    htResult.Add("Message", "No Records Found");
                }
            }
            catch (Exception e)
            {
                htResult = new Hashtable();
                htResult.Add("Result", "");
                htResult.Add("Status", "Error");
                htResult.Add("Message", strErrorMsgForUser);
                ExceptionLog.WriteErrorLog("GetCurrencySummary", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }



        
        
        [Route("api/Currency/{Id}")]
        public string GetCurrencyDetails(int Id)
        {
            ExceptionLog.WriteInfoLog("GetCurrencyDetails", "GetCurrencyDetails");
            #region GetCurrencyDetails
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();
            try
            {
                DataSet dsCurrncyDetails = new DataSet();
                var qryCurrencyDetails = "Select * from TBL_EFW_Currnecy where CurrencyId=" + Id;

                dsCurrncyDetails = dbOperations.GetDataSetforQuery(qryCurrencyDetails);

                if (dsCurrncyDetails != null && dsCurrncyDetails.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetCurrencyDetails", dsCurrncyDetails, "DataSet")));
                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "OK");
                }
                else
                {
                    htResult.Add("Result", "");
                    htResult.Add("Status", "Failed");
                    htResult.Add("Message", "No Records Found");
                }
            }
            catch (Exception e)
            {
                htResult = new Hashtable();
                htResult.Add("Result", "");
                htResult.Add("Status", "Error");
                htResult.Add("Message", strErrorMsgForUser);
                ExceptionLog.WriteErrorLog("GetCurrencyDetails", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        [Route("api/Currency/{strName}/{strDesctptn}")] //Pending
        public string GetCurrency(string strName, string strDesctptn)
        {
            ExceptionLog.WriteInfoLog("GetCurrency", "GetCurrency");
            #region GetCurrency
            string errMessage = string.Empty;
            string strEntrd = "";
            DBOperations dbOperations = new DBOperations();
            try
            {
                DataSet dsCurrencyDet = new DataSet();
                StringBuilder StrQuery = new StringBuilder();
                StrQuery.Append("Select * from TBL_EFW_Currnecy");
                if (!string.IsNullOrEmpty(strName))
                {
                    strEntrd = "Exist";
                    StrQuery.Append(" where CurrencyName='" + strName + "'");
                }
                    
                if (!string.IsNullOrEmpty(strDesctptn))
                {
                    if (!string.IsNullOrEmpty(strEntrd))
                        StrQuery.Append(" and CurrencyDescription = '" + strDesctptn + "'");
                    else
                        StrQuery.Append(" where CurrencyDescription = '" + strDesctptn + "'");
                    
                }
                   
                dsCurrencyDet = dbOperations.GetDataSetforQuery(StrQuery);

                if (dsCurrencyDet != null && dsCurrencyDet.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetCurrencyDetails", dsCurrencyDet, "DataSet")));
                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "OK");
                }
                else
                {
                    htResult.Add("Result", "");
                    htResult.Add("Status", "Failed");
                    htResult.Add("Message", "No Records Found");
                }
            }
            catch (Exception e)
            {
                htResult = new Hashtable();
                htResult.Add("Result", "");
                htResult.Add("Status", "Error");
                htResult.Add("Message", strErrorMsgForUser);
                ExceptionLog.WriteErrorLog("GetCurrency", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }



        [HttpPost]
        public string AddCurrency(Currency currency)
        {
            ExceptionLog.WriteInfoLog("AddCurrency", "AddCurrency");
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            #region 
            try
            {
                int desgCount = GetIsExists(currency.CurrencyName);

                if (desgCount <= 0)
                {

                    if (ConOperation.IsRegexValid(currency.CurrencyName, ConOperation.strReg))
                    {
                        string strStoreCurrency = "insert into TBL_EFW_Currnecy values(@CurrencyName, @CurrencyDescription,@LastUpdated, @LastUpdatedBy, @Status)";

                        SqlCommand cmdStoreCurrency = new SqlCommand();
                        cmdStoreCurrency.Parameters.Add("@CurrencyName", SqlDbType.VarChar, 30);
                        cmdStoreCurrency.Parameters["@CurrencyName"].Value = Convert.ToString(currency.CurrencyName);

                        SqlParameter currncyDescParam = new SqlParameter("@CurrencyDescription", SqlDbType.VarChar, 10);
                        currncyDescParam.Value = currency.CurrencyDescription;
                        cmdStoreCurrency.Parameters.Add(currncyDescParam);

                        SqlParameter LastUpdatedByparam = new SqlParameter("@LastUpdatedBy", SqlDbType.Int, 10);
                        LastUpdatedByparam.Value = currency.LastUpdatedBy;
                        cmdStoreCurrency.Parameters.Add(LastUpdatedByparam);

                        SqlParameter LastUpdatedOnparam = new SqlParameter("@LastUpdated", SqlDbType.DateTime, 10);
                        LastUpdatedOnparam.Value = currency.LastUpdated;
                        cmdStoreCurrency.Parameters.Add(LastUpdatedOnparam);

                        SqlParameter Statusparam = new SqlParameter("@Status", SqlDbType.Bit, 10);
                        Statusparam.Value = currency.Status;
                        cmdStoreCurrency.Parameters.Add(Statusparam);

                        Hashtable htUserData = new Hashtable();
                        htUserData.Add("CurrencyId", Convert.ToString(currency.CurrencyId));


                        if (htUserData != null)
                        {
                            dbOperations.ExecuteNonQueryWithCommand(strStoreCurrency, cmdStoreCurrency);
                            htResult.Add("Status", "OK");
                            htResult.Add("Message", "Inserted");
                            htResult.Add("CurrencyId", Convert.ToString(currency.CurrencyId));
                        }
                    }
                    else
                    {
                        htResult.Add("Status", "Failed");
                        htResult.Add("Message", "NotInserted");
                        htResult.Add("Result", "");
                    }
                }
                else
                {
                    htResult.Add("Status", "Failed");
                    htResult.Add("Message", "Already Exists");
                    htResult.Add("Result", "");
                }
            }
            catch (Exception e)
            {
                htResult = new Hashtable();
                htResult.Add("Result", "");
                htResult.Add("Status", "Error");
                htResult.Add("Message", strErrorMsgForUser);
                ExceptionLog.WriteErrorLog("AddCurrency", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }


        [HttpPut]
        public string UpdateCurrency(Currency currency)
        {
            ExceptionLog.WriteInfoLog("UpdateCurrency", "UpdateCurrency");

            #region Update Currency

            string result = "";
            try
            {
                if (ConOperation.IsRegexValid(currency.CurrencyName, ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_Currnecy SET CurrencyName=@CurrencyName,CurrencyDescription=@CurrencyDescription,LastUpdated=@LastUpdated,LastUpdatedBy=@LastUpdatedBy,Status=@Status WHERE CurrencyId=@CurrencyId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@CurrencyId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@CurrencyId"].Value = currency.CurrencyId;

                    sqlCmd.Parameters.Add("@CurrencyName", SqlDbType.VarChar, 30);
                    sqlCmd.Parameters["@CurrencyName"].Value = currency.CurrencyName;

                    sqlCmd.Parameters.Add("@CurrencyDescription", SqlDbType.VarChar, 30);
                    sqlCmd.Parameters["@CurrencyDescription"].Value = currency.CurrencyDescription;

                    

                    sqlCmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime, 8);
                    sqlCmd.Parameters["@LastUpdated"].Value = currency.LastUpdated;

                    sqlCmd.Parameters.Add("@LastUpdatedBy", SqlDbType.Int, 4);
                    sqlCmd.Parameters["@LastUpdatedBy"].Value = currency.LastUpdatedBy;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 4);
                    sqlCmd.Parameters["@Status"].Value = currency.Status;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();
                    result = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "Updated");
                    htResult.Add("CurrencyId", Convert.ToString(currency.CurrencyId));
                }
                else
                {
                    htResult.Add("Status", "Failed");
                    htResult.Add("Message", "NotUpdated");
                    htResult.Add("Result", "");
                }
            }
            catch (Exception e)
            {
                htResult = new Hashtable();
                htResult.Add("Result", "");
                htResult.Add("Status", "Error");
                htResult.Add("Message", strErrorMsgForUser);
                ExceptionLog.WriteErrorLog("UpdateCurrency", e.Message, e.StackTrace);
            }
            finally
            {
                _conn.Close();
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }


        
        [HttpDelete]
        [Route("api/currency/{Id}")]
        public string DeleteCurrencyByID(int id)
        {
            ExceptionLog.WriteInfoLog("DeleteCurrencyID", "DeleteCurrencyID");

            #region DeleteBizCatByID
            try
            {
                if (ConOperation.IsRegexValid(Convert.ToString(id), ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_Currnecy SET Status=@Status where CurrencyId=@CurrencyId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@CurrencyId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@CurrencyId"].Value = id;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 8);
                    sqlCmd.Parameters["@Status"].Value = 0;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();

                    string rowDeleted = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "deleted");
                    htResult.Add("CurrencyId", Convert.ToString(id));
                }
                else
                {
                    htResult.Add("Status", "Failed");
                    htResult.Add("Message", "Could not delete");
                    htResult.Add("Result", "");
                }
            }
            catch (Exception e)
            {
                htResult = new Hashtable();
                htResult.Add("Result", "");
                htResult.Add("Status", "Error");
                htResult.Add("Message", strErrorMsgForUser);
                ExceptionLog.WriteErrorLog("DeleteCurrencyByID", e.Message, e.StackTrace);
            }
            finally
            {
                _conn.Close();
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion

        }

        #region IsCurrencyExist
        public int GetIsExists(string curncyName)
        {
            ExceptionLog.WriteInfoLog("Get", "GetIsExists");

            #region GetIsExists

            DBOperations dbOperations = new DBOperations();
            int TotalBizCat = 0;
            try
            {
                DataSet dsCurrency = new DataSet();
                var qryCurncy = "select * from TBL_EFW_Currnecy where CurrencyName =@CurrencyName and Status=@Status";

                SqlCommand cmdTotalCurrncyCount = new SqlCommand();
                cmdTotalCurrncyCount.Parameters.Add("@CurrencyName", SqlDbType.VarChar, 30);
                cmdTotalCurrncyCount.Parameters["@CurrencyName"].Value = Convert.ToString(curncyName);

                cmdTotalCurrncyCount.Parameters.Add("@Status", SqlDbType.Bit, 4);
                cmdTotalCurrncyCount.Parameters["@Status"].Value = 1;

                try
                {
                    {
                        TotalBizCat = Convert.ToInt32(dbOperations.ExecuteScalarWithCommand(qryCurncy, cmdTotalCurrncyCount));
                    }
                }
                catch (Exception e)
                {
                    ExceptionLog.WriteErrorLog("GetIsExists", e.Message, e.StackTrace);
                    TotalBizCat = 0;
                }
            }
            catch (Exception e)
            {
                ExceptionLog.WriteErrorLog("GetIsExists", e.Message, e.StackTrace);
            }
            return TotalBizCat;
            #endregion
        }

        #endregion
    }
}

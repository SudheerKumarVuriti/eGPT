using System.Web.Http;
using System.Configuration;
using eGPTWebAPI.Models;
using System.Collections;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Data;
using System;
using eGPTWebAPI.Models.Settings.Repository;
using eGPTWebAPI.Models.Settings.Repository.Common;

namespace eGPTWebAPI.Controllers.Settings.Repository.Common
{
    public class CountryController : ApiController
    {
        SqlConnection _conn = new SqlConnection(
          ConfigurationManager.ConnectionStrings["eGPTDBB"].ToString());
        Hashtable htResult = new Hashtable();
        public string strErrorMsgForUser = "Error...";
        JavaScriptSerializer ser = new JavaScriptSerializer();
        JsonSerialize js = new JsonSerialize();
        string Result = "";

        // GET api/getCntry
        public string GetCountrySummary()
        {
            ExceptionLog.WriteInfoLog("Get", "GetCountrySummary");

            #region Get Country
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            try
            {
                DataSet dsCntry = new DataSet();
                var qryCountry = "select * from TBL_EFW_Country";

                dsCntry = dbOperations.GetDataSetforQuery(qryCountry);

                if (dsCntry != null && dsCntry.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetCountrySummary", dsCntry, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetCountrySummary", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        [Route("api/Country/{strName}/{strDesctptn}")]
        public string GetCountryDetails(string strName, string strDesctptn)
        {
            ExceptionLog.WriteInfoLog("GetCountryDetails", "GetCountryDetails");
            #region GetCountryDetails
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();
            try
            {
                DataSet dsCountryDet = new DataSet();
                var qryCntryDet = "Select * from TBL_EFW_Country where CountryName='" + strName + "' and CountryDescription = '" + strDesctptn + "'" ;

                dsCountryDet = dbOperations.GetDataSetforQuery(qryCntryDet);

                if (dsCountryDet != null && dsCountryDet.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetCountryDetails", dsCountryDet, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetCountryDetails", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        [HttpPost]
        public string AddCountry(Country country)
        {
            ExceptionLog.WriteInfoLog("AddCountry", "AddCountry");
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            #region 
            try
            {
                int cntryCount = GetIsExists(country.CountryName);

                if (cntryCount <= 0)
                {

                    if (ConOperation.IsRegexValid(country.CountryName, ConOperation.strReg))
                    {
                        string strStoreCntryCat = "insert into TBL_EFW_Country values(@CountryName, @CountryDescription,@LastUpdatedBy, @LastUpdated, @Status)";

                        SqlCommand cmdStoreCountryCat = new SqlCommand();
                        cmdStoreCountryCat.Parameters.Add("@CountryName", SqlDbType.VarChar, 30);
                        cmdStoreCountryCat.Parameters["@CountryName"].Value = Convert.ToString(country.CountryName);

                        SqlParameter bizCountyDescparam = new SqlParameter("@DesignationDescription", SqlDbType.VarChar, 10);
                        bizCountyDescparam.Value = country.CountryDescription;
                        cmdStoreCountryCat.Parameters.Add(bizCountyDescparam);

                        SqlParameter LastUpdatedByparam = new SqlParameter("@LastUpdatedBy", SqlDbType.Int, 10);
                        LastUpdatedByparam.Value = country.LastUpdatedBy;
                        cmdStoreCountryCat.Parameters.Add(LastUpdatedByparam);

                        SqlParameter LastUpdatedOnparam = new SqlParameter("@LastUpdated", SqlDbType.DateTime, 10);
                        LastUpdatedOnparam.Value = country.LastUpdated;
                        cmdStoreCountryCat.Parameters.Add(LastUpdatedOnparam);

                        SqlParameter Statusparam = new SqlParameter("@Status", SqlDbType.Bit, 10);
                        Statusparam.Value = country.Status;
                        cmdStoreCountryCat.Parameters.Add(Statusparam);

                        Hashtable htUserData = new Hashtable();
                        htUserData.Add("CountryId", Convert.ToString(country.CountryId));


                        if (htUserData != null)
                        {
                            dbOperations.ExecuteNonQueryWithCommand(strStoreCntryCat, cmdStoreCountryCat);
                            htResult.Add("Status", "OK");
                            htResult.Add("Message", "Inserted");
                            htResult.Add("CountryId", Convert.ToString(country.CountryId));
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
                ExceptionLog.WriteErrorLog("AddCountry", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        [HttpPut]
        public string UpdateCountry(Country country)
        {
            ExceptionLog.WriteInfoLog("UpdateCountry", "UpdateCountry");

            #region UpdateDesignation

            string result = "";
            try
            {
                if (ConOperation.IsRegexValid(country.CountryName, ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_Country SET CountryName=@CountryName,CountryDescription=@CountryDescription,LastUpdatedBy=@LastUpdatedBy,LastUpdated=@LastUpdated,Status=@Status WHERE CountryId=@CountryId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@CountryId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@CountryId"].Value = country.CountryId;

                    sqlCmd.Parameters.Add("@CountryName", SqlDbType.VarChar, 8);
                    sqlCmd.Parameters["@CountryName"].Value = country.CountryName;

                    sqlCmd.Parameters.Add("@CountryDescription", SqlDbType.VarChar, 8);
                    sqlCmd.Parameters["@CountryDescription"].Value = country.CountryDescription;


                    sqlCmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime, 8);
                    sqlCmd.Parameters["@LastUpdatedOn"].Value = country.LastUpdated;

                    sqlCmd.Parameters.Add("@LastUpdatedBy", SqlDbType.Int, 4);
                    sqlCmd.Parameters["@LastUpdatedBy"].Value = country.LastUpdatedBy;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 4);
                    sqlCmd.Parameters["@Status"].Value = country.Status;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();
                    result = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "Updated");
                    htResult.Add("CountryId", Convert.ToString(country.CountryId));
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
                ExceptionLog.WriteErrorLog("UpdateCountry", e.Message, e.StackTrace);
            }
            finally
            {
                _conn.Close();
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        [ActionName("DeleteCountry")]
        [HttpDelete]
        [Route("api/Designation/{Id}")]
        public string DeleteCountryByID(int id)
        {
            ExceptionLog.WriteInfoLog("DeleteCountryByID", "DeleteCountryByID");

            #region DeleteCountryByID
            try
            {
                if (ConOperation.IsRegexValid(Convert.ToString(id), ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_Country SET Status=@Status where CountryId=@CountryId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@CountryId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@CountryId"].Value = id;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 8);
                    sqlCmd.Parameters["@Status"].Value = 0;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();

                    string rowDeleted = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "deleted");
                    htResult.Add("CountryId", Convert.ToString(id));
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
                ExceptionLog.WriteErrorLog("DeleteCountryByID", e.Message, e.StackTrace);
            }
            finally
            {
                _conn.Close();
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion

        }


        #region IsDesigExist
        public int GetIsExists(string strCntryName)
        {
            ExceptionLog.WriteInfoLog("Get", "GetIsExists");

            #region GetIsExists

            DBOperations dbOperations = new DBOperations();
            int totalCntryCnt = 0;
            try
            {
                DataSet dsBizCat = new DataSet();
                var queryBizCat = "select * from TBL_EFW_Country where CountryName =@CountryName and Status=@Status";

                SqlCommand cmdTotalCntryCount = new SqlCommand();
                cmdTotalCntryCount.Parameters.Add("@CountryName", SqlDbType.VarChar, 30);
                cmdTotalCntryCount.Parameters["@CountryName"].Value = Convert.ToString(strCntryName);

                cmdTotalCntryCount.Parameters.Add("@Status", SqlDbType.Bit, 4);
                cmdTotalCntryCount.Parameters["@Status"].Value = 1;

                try
                {
                    {
                        totalCntryCnt = Convert.ToInt32(dbOperations.ExecuteScalarWithCommand(queryBizCat, cmdTotalCntryCount));
                    }
                }
                catch (Exception e)
                {
                    ExceptionLog.WriteErrorLog("GetIsExists", e.Message, e.StackTrace);
                    totalCntryCnt = 0;
                }
            }
            catch (Exception e)
            {
                ExceptionLog.WriteErrorLog("GetIsExists", e.Message, e.StackTrace);
            }
            return totalCntryCnt;
            #endregion
        }

        #endregion
    }
}

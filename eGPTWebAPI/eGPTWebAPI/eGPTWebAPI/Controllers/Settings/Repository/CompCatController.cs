using System.Web.Http;
using System.Configuration;
using eGPTWebAPI.Models;
using System.Collections;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Data;
using System;
using eGPTWebAPI.Models.Settings.Repository;

namespace eGPTWebAPI.Controllers.Settings.Repository
{
    public class CompCatController : ApiController
    {
        SqlConnection _conn = new SqlConnection(
          ConfigurationManager.ConnectionStrings["eGPTDBB"].ToString());
        Hashtable htResult = new Hashtable();
        public string strErrorMsgForUser = "Error...";
        JavaScriptSerializer ser = new JavaScriptSerializer();
        JsonSerialize js = new JsonSerialize();
        string Result = "";


        // GET api/CompCat
        public string Get()
        {
            ExceptionLog.WriteInfoLog("Get", "GetBizCat");

            #region GetCompCat
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            try
            {
                DataSet dsCompCatgry = new DataSet();
                var queryBizCat = "select * from TBL_EFW_CompanyCategory";

                dsCompCatgry = dbOperations.GetDataSetforQuery(queryBizCat);

                if (dsCompCatgry != null && dsCompCatgry.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    //htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetBizCat", dsBizCat.Tables[0], "Datatable")));
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetCompCatgry", dsCompCatgry, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetCompCatgry", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        
        // GET api/CompCat/Id
        
        [Route("api/CompCat/{Id}")]
        public string Get(int Id)
        {
            ExceptionLog.WriteInfoLog("GetBasedonId", "GetCompCatBasedonId");
            #region GetCompCatBasedonId
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();
            try
            {
                DataSet dsCompCatgry = new DataSet();
                var qryCompCatgry = "Select * from TBL_EFW_CompanyCategory where CompanyId=" + Id;

                dsCompCatgry = dbOperations.GetDataSetforQuery(qryCompCatgry);

                if (dsCompCatgry != null && dsCompCatgry.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetCompCatgryBasedonId", dsCompCatgry, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetCompCatgryBasedonId", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        [HttpPost]
        public string AddCompanyCategory(CompanyCategory compCatgry)
        {
            ExceptionLog.WriteInfoLog("AddCompanyCategory", "AddCompanyCategory");
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            #region 
            try
            {
                int compCatgryCount = GetIsExists(compCatgry.CompanyCategoryName);

                if (compCatgryCount <= 0)
                {

                    if (ConOperation.IsRegexValid(compCatgry.CompanyCategoryName, ConOperation.strReg))
                    {
                        string strStoreBizCat = "insert into TBL_EFW_CompanyCategory values(@CompanyCategoryName, @CompanyCategoryDescription, @CreatedBy, @CreatedOn, @LastUpdatedBy, @LastUpdatedOn, @Status)";

                        SqlCommand cmdStoreCompCat = new SqlCommand();
                        cmdStoreCompCat.Parameters.Add("@CompanyCategoryName", SqlDbType.VarChar, 30);
                        cmdStoreCompCat.Parameters["@CompanyCategoryName"].Value = Convert.ToString(compCatgry.CompanyCategoryName);

                        SqlParameter bizCompCatDescparam = new SqlParameter("@CompanyCategoryDescription", SqlDbType.VarChar, 10);
                        bizCompCatDescparam.Value = compCatgry.CompanyCategoryDescription;
                        cmdStoreCompCat.Parameters.Add(bizCompCatDescparam);


                        SqlParameter CreatedByparam = new SqlParameter("@CreatedBy", SqlDbType.Int, 4);
                        CreatedByparam.Value = compCatgry.CreatedBy;
                        cmdStoreCompCat.Parameters.Add(CreatedByparam);

                        SqlParameter CreatedOnparam = new SqlParameter("@CreatedOn", SqlDbType.DateTime, 10);
                        CreatedOnparam.Value = compCatgry.CreatedOn;
                        cmdStoreCompCat.Parameters.Add(CreatedOnparam);

                        SqlParameter LastUpdatedByparam = new SqlParameter("@LastUpdatedBy", SqlDbType.Int, 10);
                        LastUpdatedByparam.Value = compCatgry.LastUpdatedBy;
                        cmdStoreCompCat.Parameters.Add(LastUpdatedByparam);

                        SqlParameter LastUpdatedOnparam = new SqlParameter("@LastUpdatedOn", SqlDbType.DateTime, 10);
                        LastUpdatedOnparam.Value = compCatgry.CreatedOn;
                        cmdStoreCompCat.Parameters.Add(LastUpdatedOnparam);

                        SqlParameter Statusparam = new SqlParameter("@Status", SqlDbType.Bit, 10);
                        Statusparam.Value = compCatgry.Status;
                        cmdStoreCompCat.Parameters.Add(Statusparam);

                        Hashtable htUserData = new Hashtable();
                        htUserData.Add("CompanyId", Convert.ToString(compCatgry.CompanyId));


                        if (htUserData != null)
                        {
                            dbOperations.ExecuteNonQueryWithCommand(strStoreBizCat, cmdStoreCompCat);
                            htResult.Add("Status", "OK");
                            htResult.Add("Message", "Inserted");
                            htResult.Add("BizId", Convert.ToString(compCatgry.CompanyId));
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
                ExceptionLog.WriteErrorLog("AddCompanyCategory", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        [HttpPut]
        public string UpdateCompanyCategory(CompanyCategory compCatgry)
        {
            ExceptionLog.WriteInfoLog("UpdateCompanyCategory", "UpdateCompanyCategory");

            #region UpdateBusinessCategory

            string result = "";
            try
            {
                if (ConOperation.IsRegexValid(compCatgry.CompanyCategoryName, ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_CompanyCategory SET CompanyCategoryName=@CompanyCategoryName,CompanyCategoryDescription=@CompanyCategoryDescription,CreatedBy=@CreatedBy,CreatedOn=@CreatedOn,LastUpdatedBy=@LastUpdatedBy,LastUpdatedOn=@LastUpdatedOn,Status=@Status WHERE CompanyId=@CompanyId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@CompanyId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@CompanyId"].Value = compCatgry.CompanyId;

                    sqlCmd.Parameters.Add("@CompanyCategoryName", SqlDbType.VarChar, 8);
                    sqlCmd.Parameters["@CompanyCategoryName"].Value = compCatgry.CompanyCategoryName;

                    sqlCmd.Parameters.Add("@CompanyCategoryDescription", SqlDbType.VarChar, 8);
                    sqlCmd.Parameters["@CompanyCategoryDescription"].Value = compCatgry.CompanyCategoryDescription;

                    sqlCmd.Parameters.Add("@CreatedOn", SqlDbType.DateTime, 8);
                    sqlCmd.Parameters["@CreatedOn"].Value = compCatgry.CreatedOn;

                    sqlCmd.Parameters.Add("@CreatedBy", SqlDbType.Int, 4);
                    sqlCmd.Parameters["@CreatedBy"].Value = compCatgry.CreatedBy;

                    sqlCmd.Parameters.Add("@LastUpdatedOn", SqlDbType.DateTime, 8);
                    sqlCmd.Parameters["@LastUpdatedOn"].Value = compCatgry.LastUpdatedOn;

                    sqlCmd.Parameters.Add("@LastUpdatedBy", SqlDbType.Int, 4);
                    sqlCmd.Parameters["@LastUpdatedBy"].Value = compCatgry.LastUpdatedBy;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 4);
                    sqlCmd.Parameters["@Status"].Value = compCatgry.Status;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();
                    result = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "Updated");
                    htResult.Add("BizId", Convert.ToString(compCatgry.CompanyId));
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
                ExceptionLog.WriteErrorLog("UpdateCompanyCategory", e.Message, e.StackTrace);
            }
            finally
            {
                _conn.Close();
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }



        [ActionName("DeleteCompCatgry")]
        [HttpDelete]
        [Route("api/CompCatgry/{Id}")]
        public string DeleteCompCatByID(int id)
        {
            ExceptionLog.WriteInfoLog("DeleteCompCatByID", "DeleteCompCatByID");

            #region DeleteBizCatByID
            try
            {
                if (ConOperation.IsRegexValid(Convert.ToString(id), ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_CompanyCategory SET Status=@Status where CompanyId=@CompanyId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@CompanyId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@CompanyId"].Value = id;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 8);
                    sqlCmd.Parameters["@Status"].Value = 0;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();

                    string rowDeleted = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "deleted");
                    htResult.Add("BizId", Convert.ToString(id));
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
                ExceptionLog.WriteErrorLog("DeleteCompCatByID", e.Message, e.StackTrace);
            }
            finally
            {
                _conn.Close();
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion

        }

        #region IsCompCatgryExist
        public int GetIsExists(string compCatName)
        {
            ExceptionLog.WriteInfoLog("Get", "GetIsExists");

            #region GetIsExists

            DBOperations dbOperations = new DBOperations();
            int TotalBizCat = 0;
            try
            {
                DataSet dsBizCat = new DataSet();
                var queryBizCat = "select * from TBL_EFW_CompanyCategory where CompanyCategoryName =@CompanyCategoryName and Status=@Status";

                SqlCommand cmdTotalBizCatCount = new SqlCommand();
                cmdTotalBizCatCount.Parameters.Add("@CompanyCategoryName", SqlDbType.VarChar, 30);
                cmdTotalBizCatCount.Parameters["@CompanyCategoryName"].Value = Convert.ToString(compCatName);

                cmdTotalBizCatCount.Parameters.Add("@Status", SqlDbType.Bit, 4);
                cmdTotalBizCatCount.Parameters["@Status"].Value = 1;

                try
                {
                    {
                        TotalBizCat = Convert.ToInt32(dbOperations.ExecuteScalarWithCommand(queryBizCat, cmdTotalBizCatCount));
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

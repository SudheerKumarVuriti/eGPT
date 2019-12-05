using eGPTWebAPI.Models.Settings.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using eGPTWebAPI.Models;
using System.Collections;
using System.Web.Script.Serialization;

namespace eGPTWebAPI.Controllers.Settings.Repository
{
    public class BizCatController : ApiController
    {
        SqlConnection _conn = new SqlConnection(
          ConfigurationManager.ConnectionStrings["eGPTDBB"].ToString());
        Hashtable htResult = new Hashtable();
        public string strErrorMsgForUser = "Error...";
        JavaScriptSerializer ser = new JavaScriptSerializer();
        JsonSerialize js = new JsonSerialize();
        string Result = "";

        // GET api/BizCat
        
        public string Get()
        {
            ExceptionLog.WriteInfoLog("Get", "GetBizCat");

            #region GetBizCat
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            try
            {
                DataSet dsBizCat = new DataSet();
                var queryBizCat = "select * from TBL_EFW_BusinessCategory";

                dsBizCat = dbOperations.GetDataSetforQuery(queryBizCat);

                if (dsBizCat != null && dsBizCat.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    //htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetBizCat", dsBizCat.Tables[0], "Datatable")));
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetBizCat", dsBizCat, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetBizCat", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        // GET api/BizCat/5
        // [ActionName("GetBizCatBsdOnId")]
        [Route("api/BizCat/{Id}")]
        public string Get(int Id)
        {
            ExceptionLog.WriteInfoLog("GetBasedonId", "GetBizCatBasedonId");
            #region GetBizCatBasedonId
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();
            try
            {
                DataSet dsBizCat = new DataSet();
                var queryBizCat = "Select * from TBL_EFW_BusinessCategory where BizId=" + Id;

                dsBizCat = dbOperations.GetDataSetforQuery(queryBizCat);

                if (dsBizCat != null && dsBizCat.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetBizCatBasedonId", dsBizCat, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetBizCatBasedonId", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        [HttpPost]
        public string AddBusinessCategory(BusinessCategory BizCat)
        {
            ExceptionLog.WriteInfoLog("AddBusinessCategory", "AddBusinessCategory");
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            #region 
            try
            {
                int BizCatCount = GetIsExists(BizCat.BizCategoryName);

                if (BizCatCount <= 0)
                {

                    if (ConOperation.IsRegexValid(BizCat.BizCategoryName, ConOperation.strReg))
                    {
                        string strStoreBizCat = "insert into TBL_EFW_BusinessCategory values(@BizCategoryName, @BizCategoryDescription, @CreatedBy, @CreatedOn, @LastUpdatedBy, @LastUpdatedOn, @Status)";

                        SqlCommand cmdStoreBizCat = new SqlCommand();
                        cmdStoreBizCat.Parameters.Add("@BizCategoryName", SqlDbType.VarChar, 30);
                        cmdStoreBizCat.Parameters["@BizCategoryName"].Value = Convert.ToString(BizCat.BizCategoryName);

                        SqlParameter bizcatDescparam = new SqlParameter("@BizCategoryDescription", SqlDbType.VarChar, 10);
                        bizcatDescparam.Value = BizCat.BizCategoryDescription;
                        cmdStoreBizCat.Parameters.Add(bizcatDescparam);


                        SqlParameter CreatedByparam = new SqlParameter("@CreatedBy", SqlDbType.Int, 4);
                        CreatedByparam.Value = BizCat.CreatedBy;
                        cmdStoreBizCat.Parameters.Add(CreatedByparam);

                        SqlParameter CreatedOnparam = new SqlParameter("@CreatedOn", SqlDbType.DateTime, 10);
                        CreatedOnparam.Value = BizCat.CreatedOn;
                        cmdStoreBizCat.Parameters.Add(CreatedOnparam);

                        SqlParameter LastUpdatedByparam = new SqlParameter("@LastUpdatedBy", SqlDbType.Int, 10);
                        LastUpdatedByparam.Value = BizCat.LastUpdatedBy;
                        cmdStoreBizCat.Parameters.Add(LastUpdatedByparam);

                        SqlParameter LastUpdatedOnparam = new SqlParameter("@LastUpdatedOn", SqlDbType.DateTime, 10);
                        LastUpdatedOnparam.Value = BizCat.CreatedOn;
                        cmdStoreBizCat.Parameters.Add(LastUpdatedOnparam);

                        SqlParameter Statusparam = new SqlParameter("@Status", SqlDbType.Bit, 10);
                        Statusparam.Value = BizCat.Status;
                        cmdStoreBizCat.Parameters.Add(Statusparam);

                        Hashtable htUserData = new Hashtable();
                        htUserData.Add("BizId", Convert.ToString(BizCat.BizId));


                        if (htUserData != null)
                        {
                            dbOperations.ExecuteNonQueryWithCommand(strStoreBizCat, cmdStoreBizCat);
                            htResult.Add("Status", "OK");
                            htResult.Add("Message", "Inserted");
                            htResult.Add("BizId", Convert.ToString(BizCat.BizId));
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
                ExceptionLog.WriteErrorLog("AddBusinessCategory", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        [HttpPut]
        public string UpdateBusinessCategory(BusinessCategory BizCat)
        {
            ExceptionLog.WriteInfoLog("UpdateBusinessCategory", "UpdateBusinessCategory");

            #region UpdateBusinessCategory

            string result = "";
            try
            {
                if (ConOperation.IsRegexValid(BizCat.BizCategoryName, ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_BusinessCategory SET BizCategoryName=@BizCategoryName,BizCategoryDescription=@BizCategoryDescription,CreatedBy=@CreatedBy,CreatedOn=@CreatedOn,LastUpdatedBy=@LastUpdatedBy,LastUpdatedOn=@LastUpdatedOn,Status=@Status WHERE BizId=@BizId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@BizId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@BizId"].Value = BizCat.BizId;

                    sqlCmd.Parameters.Add("@BizCategoryName", SqlDbType.VarChar, 8);
                    sqlCmd.Parameters["@BizCategoryName"].Value = BizCat.BizCategoryName;

                    sqlCmd.Parameters.Add("@BizCategoryDescription", SqlDbType.VarChar, 8);
                    sqlCmd.Parameters["@BizCategoryDescription"].Value = BizCat.BizCategoryDescription;

                    sqlCmd.Parameters.Add("@CreatedOn", SqlDbType.DateTime, 8);
                    sqlCmd.Parameters["@CreatedOn"].Value = BizCat.CreatedOn;

                    sqlCmd.Parameters.Add("@CreatedBy", SqlDbType.Int, 4);
                    sqlCmd.Parameters["@CreatedBy"].Value = BizCat.CreatedBy;

                    sqlCmd.Parameters.Add("@LastUpdatedOn", SqlDbType.DateTime, 8);
                    sqlCmd.Parameters["@LastUpdatedOn"].Value = BizCat.LastUpdatedOn;

                    sqlCmd.Parameters.Add("@LastUpdatedBy", SqlDbType.Int, 4);
                    sqlCmd.Parameters["@LastUpdatedBy"].Value = BizCat.LastUpdatedBy;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 4);
                    sqlCmd.Parameters["@Status"].Value = BizCat.Status;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();
                    result = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "Updated");
                    htResult.Add("BizId", Convert.ToString(BizCat.BizId));
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
                ExceptionLog.WriteErrorLog("UpdateBusinessCategory", e.Message, e.StackTrace);
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
        [Route("api/BizCat/{Id}")]
        public string DeleteBizCatByID(int id)
        {
            ExceptionLog.WriteInfoLog("DeleteBizCatByID", "DeleteBizCatByID");

            #region DeleteBizCatByID
            try
            {
                if (ConOperation.IsRegexValid(Convert.ToString(id), ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_BusinessCategory SET Status=@Status where BizId=@BizId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@BizId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@BizId"].Value = id;

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
                ExceptionLog.WriteErrorLog("DeleteBizCatByID", e.Message, e.StackTrace);
            }
            finally
            {
                _conn.Close();
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion

        }

        public int GetIsExists(string BizCatName)
        {
            ExceptionLog.WriteInfoLog("Get", "GetIsExists");

            #region GetIsExists

            DBOperations dbOperations = new DBOperations();
            int TotalBizCat = 0;
            try
            {
                DataSet dsBizCat = new DataSet();
                var queryBizCat = "select * from TBL_EFW_BusinessCategory where BizCategoryName =@BizCategoryName and Status=@status";

                SqlCommand cmdTotalBizCatCount = new SqlCommand();
                cmdTotalBizCatCount.Parameters.Add("@BizCategoryName", SqlDbType.VarChar, 30);
                cmdTotalBizCatCount.Parameters["@BizCategoryName"].Value = Convert.ToString(BizCatName);

                cmdTotalBizCatCount.Parameters.Add("@status", SqlDbType.Bit, 4);
                cmdTotalBizCatCount.Parameters["@status"].Value = 1;

                try
                {
                    {
                        TotalBizCat = Convert.ToInt32(dbOperations.ExecuteScalarWithCommand(queryBizCat, cmdTotalBizCatCount));
                    }
                }
                catch(Exception e)
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
    }
}

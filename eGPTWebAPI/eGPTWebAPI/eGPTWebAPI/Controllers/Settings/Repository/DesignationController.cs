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
    public class DesignationController : ApiController
    {
        SqlConnection _conn = new SqlConnection(
          ConfigurationManager.ConnectionStrings["eGPTDBB"].ToString());
        Hashtable htResult = new Hashtable();
        public string strErrorMsgForUser = "Error...";
        JavaScriptSerializer ser = new JavaScriptSerializer();
        JsonSerialize js = new JsonSerialize();
        string Result = "";

        // GET api/Designation
        public string GetDesignationSummary()
        {
            ExceptionLog.WriteInfoLog("Get", "GetDesignation");

            #region GetDesignation
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            try
            {
                DataSet dsDesg = new DataSet();
                var qryDesg = "select * from TBL_EFW_Designation";

                dsDesg = dbOperations.GetDataSetforQuery(qryDesg);

                if (dsDesg != null && dsDesg.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetDesignationSumm", dsDesg, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetDesignationSumm", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        // GET api/Designation/Id

        [Route("api/Designation/{Id}")]
        public string GetDesignationDetailsById(int Id)
        {
            ExceptionLog.WriteInfoLog("GetBasedonId", "GetDesignationDetailsById");
            #region GetDesignationDetailsById
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();
            try
            {
                DataSet dsDesgDetails = new DataSet();
                var qryDesgDetails = "Select * from TBL_EFW_Designation where DesignationId=" + Id;

                dsDesgDetails = dbOperations.GetDataSetforQuery(qryDesgDetails);

                if (dsDesgDetails != null && dsDesgDetails.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetDesignationDetailsById", dsDesgDetails, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetDesignationDetailsById", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        [HttpPost]
        public string AddDesignation(Designation designation)
        {
            ExceptionLog.WriteInfoLog("AddDesignation", "AddDesignation");
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            #region 
            try
            {
                int desgCount = GetIsExists(designation.DesignationName);

                if (desgCount <= 0)
                {

                    if (ConOperation.IsRegexValid(designation.DesignationName, ConOperation.strReg))
                    {
                        string strStoreBizCat = "insert into TBL_EFW_Designation values(@DesignationName, @DesignationDescription,@LastUpdatedBy, @LastUpdated, @Status)";

                        SqlCommand cmdStoreCompCat = new SqlCommand();
                        cmdStoreCompCat.Parameters.Add("@DesignationName", SqlDbType.VarChar, 30);
                        cmdStoreCompCat.Parameters["@DesignationName"].Value = Convert.ToString(designation.DesignationName);

                        SqlParameter bizCompCatDescparam = new SqlParameter("@DesignationDescription", SqlDbType.VarChar, 10);
                        bizCompCatDescparam.Value = designation.DesignationDescription;
                        cmdStoreCompCat.Parameters.Add(bizCompCatDescparam);

                        SqlParameter LastUpdatedByparam = new SqlParameter("@LastUpdatedBy", SqlDbType.Int, 10);
                        LastUpdatedByparam.Value = designation.LastUpdatedBy;
                        cmdStoreCompCat.Parameters.Add(LastUpdatedByparam);

                        SqlParameter LastUpdatedOnparam = new SqlParameter("@LastUpdated", SqlDbType.DateTime, 10);
                        LastUpdatedOnparam.Value = designation.LastUpdated;
                        cmdStoreCompCat.Parameters.Add(LastUpdatedOnparam);

                        SqlParameter Statusparam = new SqlParameter("@Status", SqlDbType.Bit, 10);
                        Statusparam.Value = designation.Status;
                        cmdStoreCompCat.Parameters.Add(Statusparam);

                        Hashtable htUserData = new Hashtable();
                        htUserData.Add("DesignationId", Convert.ToString(designation.DesignationId));


                        if (htUserData != null)
                        {
                            dbOperations.ExecuteNonQueryWithCommand(strStoreBizCat, cmdStoreCompCat);
                            htResult.Add("Status", "OK");
                            htResult.Add("Message", "Inserted");
                            htResult.Add("BizId", Convert.ToString(designation.DesignationId));
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
                ExceptionLog.WriteErrorLog("AddDesignation", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }


        [HttpPut]
        public string UpdateDesignation(Designation designation)
        {
            ExceptionLog.WriteInfoLog("UpdateDesignation", "UpdateDesignation");

            #region UpdateDesignation

            string result = "";
            try
            {
                if (ConOperation.IsRegexValid(designation.DesignationName, ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_Designation SET DesignationName=@DesignationName,DesignationDescription=@DesignationDescription,LastUpdatedBy=@LastUpdatedBy,LastUpdated=@LastUpdated,Status=@Status WHERE DesignationId=@DesignationId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@DesignationId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@DesignationId"].Value = designation.DesignationId;

                    sqlCmd.Parameters.Add("@DesignationName", SqlDbType.VarChar, 8);
                    sqlCmd.Parameters["@DesignationName"].Value = designation.DesignationName;

                    sqlCmd.Parameters.Add("@DesignationDescription", SqlDbType.VarChar, 8);
                    sqlCmd.Parameters["@DesignationDescription"].Value = designation.DesignationDescription;

                    
                    sqlCmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime, 8);
                    sqlCmd.Parameters["@LastUpdatedOn"].Value = designation.LastUpdated;

                    sqlCmd.Parameters.Add("@LastUpdatedBy", SqlDbType.Int, 4);
                    sqlCmd.Parameters["@LastUpdatedBy"].Value = designation.LastUpdatedBy;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 4);
                    sqlCmd.Parameters["@Status"].Value = designation.Status;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();
                    result = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "Updated");
                    htResult.Add("DesignationId", Convert.ToString(designation.DesignationId));
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
                ExceptionLog.WriteErrorLog("UpdateDesignation", e.Message, e.StackTrace);
            }
            finally
            {
                _conn.Close();
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        [ActionName("DeleteDesignation")]
        [HttpDelete]
        [Route("api/Designation/{Id}")]
        public string DeleteDesignationByID(int id)
        {
            ExceptionLog.WriteInfoLog("DeleteDesignationByID", "DeleteDesignationByID");

            #region DeleteDesignationByID
            try
            {
                if (ConOperation.IsRegexValid(Convert.ToString(id), ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_Designation SET Status=@Status where DesignationId=@DesignationId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@DesignationId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@DesignationId"].Value = id;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 8);
                    sqlCmd.Parameters["@Status"].Value = 0;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();

                    string rowDeleted = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "deleted");
                    htResult.Add("DesignationId", Convert.ToString(id));
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
                ExceptionLog.WriteErrorLog("DeleteDesignationByID", e.Message, e.StackTrace);
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
        public int GetIsExists(string desgName)
        {
            ExceptionLog.WriteInfoLog("Get", "GetIsExists");

            #region GetIsExists

            DBOperations dbOperations = new DBOperations();
            int TotalBizCat = 0;
            try
            {
                DataSet dsBizCat = new DataSet();
                var queryBizCat = "select * from TBL_EFW_Designation where DesignationName =@DesignationName and Status=@Status";

                SqlCommand cmdTotalDesigCount = new SqlCommand();
                cmdTotalDesigCount.Parameters.Add("@CompanyCategoryName", SqlDbType.VarChar, 30);
                cmdTotalDesigCount.Parameters["@CompanyCategoryName"].Value = Convert.ToString(desgName);

                cmdTotalDesigCount.Parameters.Add("@Status", SqlDbType.Bit, 4);
                cmdTotalDesigCount.Parameters["@Status"].Value = 1;

                try
                {
                    {
                        TotalBizCat = Convert.ToInt32(dbOperations.ExecuteScalarWithCommand(queryBizCat, cmdTotalDesigCount));
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

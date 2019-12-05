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
    public class VisaTypeController : ApiController
    {
        SqlConnection _conn = new SqlConnection(
        ConfigurationManager.ConnectionStrings["eGPTDBB"].ToString());
        Hashtable htResult = new Hashtable();
        public string strErrorMsgForUser = "Error...";
        JavaScriptSerializer ser = new JavaScriptSerializer();
        JsonSerialize js = new JsonSerialize();
        string Result = "";

        // GET api/VisaType
        public string GetVisaTypeSummary()
        {
            ExceptionLog.WriteInfoLog("Get", "GetVisaTypeSummary");

            #region GetVisaTypeSummary
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            try
            {
                DataSet dsVisaType = new DataSet();
                var qryVisaType = "select * from TBL_EFW_VisaType";

                dsVisaType = dbOperations.GetDataSetforQuery(qryVisaType);

                if (dsVisaType != null && dsVisaType.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetVisaTypeSummary", dsVisaType, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetVisaTypeSummary", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        [Route("api/VisaType/{Id}")]
        public string GetVisaDetails(int Id)
        {
            ExceptionLog.WriteInfoLog("GetVisaDetails", "GetVisaDetails");
            #region GetVisaDetails
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();
            try
            {
                DataSet dsVisaDetails = new DataSet();
                var qryVisaDetails = "Select * from TBL_EFW_VisaType where VisaId=" + Id;
                dsVisaDetails = dbOperations.GetDataSetforQuery(qryVisaDetails);
                if (dsVisaDetails != null && dsVisaDetails.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetVisaDetails", dsVisaDetails, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetVisaDetails", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }

        [Route("api/BloodGroup/{strName}/{strDescptn}")]
        #region Blood Group Search
        public string GetVisaType(string strName, string strDescptn)
        {
            ExceptionLog.WriteInfoLog("Get", "GetVisaType");

            #region GetBloodGroupSummary    
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            try
            {
                DataSet dsBldGrp = new DataSet();
                StringBuilder sbBldGrp = new StringBuilder();
                sbBldGrp.Append(" select * from TBL_EFW_VisaType  where Status=1  ");
                if (!string.IsNullOrEmpty(strName))
                {
                    sbBldGrp.Append(" and b.TypeOfVisa like '%" + EGPTStaticMethods.replaceWithSplChars(strName) + "%' ESCAPE '\\' ");
                }
                if (!string.IsNullOrEmpty(strDescptn))
                {
                    sbBldGrp.Append(" and b.VisaTypeDescription like '%" + EGPTStaticMethods.replaceWithSplChars(strDescptn) + "%' ESCAPE '\\' ");
                }

                sbBldGrp.Append(" order by b.LastUpdated desc ");
                dsBldGrp = dbOperations.GetDataSetforQuery(sbBldGrp);
                if (dsBldGrp != null && dsBldGrp.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetVisaType", dsBldGrp, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetVisaType", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }
        #endregion

        [HttpPost]
        public string AddVisaType(VisaType visaType)
        {
            ExceptionLog.WriteInfoLog("AddVisaType", "AddVisaType");
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            #region AddVisaType
            try
            {
                int desgCount = GetIsExists(visaType.TypeOfVisa);

                if (desgCount <= 0)
                {

                    if (ConOperation.IsRegexValid(visaType.TypeOfVisa, ConOperation.strReg))
                    {
                        string strStoreCurrency = "insert into TBL_EFW_VisaType values(@TypeOfVisa, @VisaTypeDescription,@CreatedOn,@CreatedBy,@LastUpdated, @LastUpdatedBy, @Status)";

                        SqlCommand cmdVisaStore = new SqlCommand();
                        cmdVisaStore.Parameters.Add("@TypeOfVisa", SqlDbType.VarChar, 30);
                        cmdVisaStore.Parameters["@TypeOfVisa"].Value = Convert.ToString(visaType.TypeOfVisa);

                        SqlParameter visaDescParam = new SqlParameter("@VisaTypeDescription", SqlDbType.VarChar, 10);
                        visaDescParam.Value = visaType.VisaTypeDescription;
                        cmdVisaStore.Parameters.Add(visaDescParam);

                        SqlParameter LastUpdatedByparam = new SqlParameter("@LastUpdatedBy", SqlDbType.Int, 10);
                        LastUpdatedByparam.Value = visaType.LastUpdatedBy;
                        cmdVisaStore.Parameters.Add(LastUpdatedByparam);

                        SqlParameter LastUpdatedOnparam = new SqlParameter("@LastUpdated", SqlDbType.DateTime, 10);
                        LastUpdatedOnparam.Value = visaType.LastUpdated;
                        cmdVisaStore.Parameters.Add(LastUpdatedOnparam);

                        SqlParameter Statusparam = new SqlParameter("@Status", SqlDbType.Bit, 10);
                        Statusparam.Value = visaType.Status;
                        cmdVisaStore.Parameters.Add(Statusparam);

                        Hashtable htUserData = new Hashtable();
                        htUserData.Add("VisaId", Convert.ToString(visaType.VisaId));


                        if (htUserData != null)
                        {
                            dbOperations.ExecuteNonQueryWithCommand(strStoreCurrency, cmdVisaStore);
                            htResult.Add("Status", "OK");
                            htResult.Add("Message", "Inserted");
                            htResult.Add("VisaId", Convert.ToString(visaType.VisaId));
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
        public string UpdateVisaType(VisaType visaType)
        {
            ExceptionLog.WriteInfoLog("UpdateCurrency", "UpdateCurrency");

            #region Update Currency

            string result = "";
            try
            {
                if (ConOperation.IsRegexValid(visaType.TypeOfVisa, ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_VisaType SET TypeOfVisa=@TypeOfVisa,VisaTypeDescription=@VisaTypeDescription,CreatedOn = @CreatedOn,CreatedBy=@CreatedBy;LastUpdated=@LastUpdated,LastUpdatedBy=@LastUpdatedBy,Status=@Status WHERE VisaId=@VisaId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@VisaId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@VisaId"].Value = visaType.VisaId;

                    sqlCmd.Parameters.Add("@TypeOfVisa", SqlDbType.VarChar, 30);
                    sqlCmd.Parameters["@TypeOfVisa"].Value = visaType.TypeOfVisa;

                    sqlCmd.Parameters.Add("@VisaTypeDescription", SqlDbType.VarChar, 30);
                    sqlCmd.Parameters["@VisaTypeDescription"].Value = visaType.VisaTypeDescription;



                    sqlCmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime, 8);
                    sqlCmd.Parameters["@LastUpdated"].Value = visaType.LastUpdated;

                    sqlCmd.Parameters.Add("@LastUpdatedBy", SqlDbType.Int, 4);
                    sqlCmd.Parameters["@LastUpdatedBy"].Value = visaType.LastUpdatedBy;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 4);
                    sqlCmd.Parameters["@Status"].Value = visaType.Status;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();
                    result = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "Updated");
                    htResult.Add("VisaId", Convert.ToString(visaType.VisaId));
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
                ExceptionLog.WriteErrorLog("UpdateVisaType", e.Message, e.StackTrace);
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
        [Route("api/VisaType/{Id}")]
        public string DeleteVisaTypeByID(int id)
        {
            ExceptionLog.WriteInfoLog("DeleteVisaTypeByID", "DeleteVisaTypeByID");

            #region DeleteBizCatByID
            try
            {
                if (ConOperation.IsRegexValid(Convert.ToString(id), ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_VisaType SET Status=@Status where VisaId=@VisaId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@VisaId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@VisaId"].Value = id;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 8);
                    sqlCmd.Parameters["@Status"].Value = 0;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();

                    string rowDeleted = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "deleted");
                    htResult.Add("VisaId", Convert.ToString(id));
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
                ExceptionLog.WriteErrorLog("DeleteVisaTypeByID", e.Message, e.StackTrace);
            }
            finally
            {
                _conn.Close();
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion

        }


        #region IsExist
        public int GetIsExists(string visaType)
        {
            ExceptionLog.WriteInfoLog("Get", "GetIsExists");

            #region GetIsExists

            DBOperations dbOperations = new DBOperations();
            int ttlVisaType = 0;
            try
            {
                DataSet dsCurrency = new DataSet();
                var qryCurncy = "select * from TBL_EFW_VisaType where TypeOfVisa =@TypeOfVisa and Status=@Status";

                SqlCommand cmdTotalVisaCount = new SqlCommand();
                cmdTotalVisaCount.Parameters.Add("@TypeOfVisa", SqlDbType.VarChar, 30);
                cmdTotalVisaCount.Parameters["@TypeOfVisa"].Value = Convert.ToString(visaType);

                cmdTotalVisaCount.Parameters.Add("@Status", SqlDbType.Bit, 4);
                cmdTotalVisaCount.Parameters["@Status"].Value = 1;

                try
                {
                    {
                        ttlVisaType = Convert.ToInt32(dbOperations.ExecuteScalarWithCommand(qryCurncy, cmdTotalVisaCount));
                    }
                }
                catch (Exception e)
                {
                    ExceptionLog.WriteErrorLog("GetIsExists", e.Message, e.StackTrace);
                    ttlVisaType = 0;
                }
            }
            catch (Exception e)
            {
                ExceptionLog.WriteErrorLog("GetIsExists", e.Message, e.StackTrace);
            }
            return ttlVisaType;
            #endregion
        }

        #endregion


    }
}

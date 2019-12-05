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
    public class BloodGroupController : ApiController
    {
        SqlConnection _conn = new SqlConnection(
        ConfigurationManager.ConnectionStrings["eGPTDBB"].ToString());
        Hashtable htResult = new Hashtable();
        public string strErrorMsgForUser = "Error...";
        JavaScriptSerializer ser = new JavaScriptSerializer();
        JsonSerialize js = new JsonSerialize();
        string Result = "";

        #region Blood Group
        //GET api/Blood Group
        #region Blood Group Summary
        public string GetBloodGroup()
        {
            ExceptionLog.WriteInfoLog("GetBloodGroup", "GetBloodGroup");

            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();
            try
            {
                DataSet dsBldGrp = new DataSet();
                var qryBldGrp = "Select * from TBL_EFW_BloodGroup where Status = 1";

                dsBldGrp = dbOperations.GetDataSetforQuery(qryBldGrp);

                if (dsBldGrp != null && dsBldGrp.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetBloodGroup", dsBldGrp, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetBloodGroup", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;

        }
        #endregion

        [Route("api/BloodGroup/{Id}")]
        #region Blood Group Detail
        public string GetBloodGroupDetails(int Id)
        {
            ExceptionLog.WriteInfoLog("GetBloodGroupDetails", "GetBloodGroupDetails");
            #region GetCurrencyDetails
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();
            try
            {
                DataSet dsBldGrp = new DataSet();
                var qryBldGrp = "Select * from TBL_EFW_BloodGroup where BloodGroupId=" + Id;

                dsBldGrp = dbOperations.GetDataSetforQuery(qryBldGrp);

                if (dsBldGrp != null && dsBldGrp.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetBloodGroupDetails", dsBldGrp, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetBloodGroupDetails", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }
        #endregion

        [Route("api/BloodGroup/{strName}/{strDescptn}")]
        #region Blood Group Search
        public string GetBloodGroupSummary(string strName, string strDescptn)
        {
            ExceptionLog.WriteInfoLog("Get", "GetBloodGroupSummary");

            #region GetBloodGroupSummary    
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            try
            {
                DataSet dsBldGrp = new DataSet();
                StringBuilder sbBldGrp = new StringBuilder();
                sbBldGrp.Append(" select * from TBL_EFW_BloodGroup  where Status=1  ");
                if (!string.IsNullOrEmpty(strName))
                {
                    sbBldGrp.Append(" and b.BloodGroupName like '%" + EGPTStaticMethods.replaceWithSplChars(strName) + "%' ESCAPE '\\' ");
                }
                if (!string.IsNullOrEmpty(strDescptn))
                {
                    sbBldGrp.Append(" and b.BloodGroupDescription like '%" + EGPTStaticMethods.replaceWithSplChars(strDescptn) + "%' ESCAPE '\\' ");
                }

                sbBldGrp.Append(" order by b.LastUpdated desc ");
                dsBldGrp = dbOperations.GetDataSetforQuery(sbBldGrp);
                if (dsBldGrp != null && dsBldGrp.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetBloodGroupSummary", dsBldGrp, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetBloodGroupSummary", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            #endregion
        }
        #endregion

        [HttpPost]
        #region Add Blood Group
        public string AddBloodGroup(BloodGroup bloodGroup)
        {
            ExceptionLog.WriteInfoLog("AddBloodGroup", "AddBloodGroup");
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();
            try
            {
                int bldGrpCnt = GetIsExists(bloodGroup.BloodGroupName);

                if (bldGrpCnt <= 0)
                {

                    if (ConOperation.IsRegexValid(bloodGroup.BloodGroupName, ConOperation.strReg))
                    {
                        string strStoreCurrency = "insert into TBL_EFW_BloodGroup values(@BloodGroupName, @BloodGroupDescription,@CreatedOn, @CreatedBy, @LastUpdated,@LastUpdatedBy,@Status)";

                        SqlCommand cmdStoreBldGrp = new SqlCommand();
                        cmdStoreBldGrp.Parameters.Add("@BloodGroupName", SqlDbType.VarChar, 30);
                        cmdStoreBldGrp.Parameters["@BloodGroupName"].Value = Convert.ToString(bloodGroup.BloodGroupName);

                        SqlParameter bldGrpDesc = new SqlParameter("@BloodGroupDescription", SqlDbType.VarChar, 30);
                        bldGrpDesc.Value = bloodGroup.BloodGroupDescription;
                        cmdStoreBldGrp.Parameters.Add(bldGrpDesc);

                        SqlParameter createdOnParam = new SqlParameter("@CreatedOn", SqlDbType.DateTime, 10);
                        createdOnParam.Value = bloodGroup.CreatedOn;
                        cmdStoreBldGrp.Parameters.Add(createdOnParam);

                        SqlParameter createdByParam = new SqlParameter("@CreatedBy", SqlDbType.Int, 4);
                        createdByParam.Value = bloodGroup.CreatedBy;
                        cmdStoreBldGrp.Parameters.Add(createdByParam);

                        SqlParameter lastUpdatedByParam = new SqlParameter("@LastUpdatedBy", SqlDbType.Int, 10);
                        lastUpdatedByParam.Value = bloodGroup.LastUpdatedBy;
                        cmdStoreBldGrp.Parameters.Add(lastUpdatedByParam);

                        SqlParameter lastUpdatedParam = new SqlParameter("@LastUpdated", SqlDbType.DateTime, 10);
                        lastUpdatedParam.Value = bloodGroup.LastUpdated;
                        cmdStoreBldGrp.Parameters.Add(lastUpdatedParam);

                        SqlParameter statusParam = new SqlParameter("@Status", SqlDbType.Bit, 10);
                        statusParam.Value = bloodGroup.Status;
                        cmdStoreBldGrp.Parameters.Add(statusParam);

                        Hashtable htUserData = new Hashtable();
                        htUserData.Add("BloodGroupId", Convert.ToString(bloodGroup.BloodGroupId));


                        if (htUserData != null)
                        {
                            dbOperations.ExecuteNonQueryWithCommand(strStoreCurrency, cmdStoreBldGrp);
                            htResult.Add("Status", "OK");
                            htResult.Add("Message", "Inserted");
                            htResult.Add("BloodGroupId", Convert.ToString(bloodGroup.BloodGroupId));
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
                ExceptionLog.WriteErrorLog("AddBloodGroup", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;

        }
        #endregion

        [HttpPut]
        #region Update Blood Group
        public string UpdateBloodGroup(BloodGroup bloodGroup)
        {
            ExceptionLog.WriteInfoLog("UpdateBloodGroup", "UpdateBloodGroup");
            string result = "";
            try
            {
                if (ConOperation.IsRegexValid(bloodGroup.BloodGroupName, ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_BloodGroup SET BloodGroupName=@BloodGroupName,BloodGroupDescription=@BloodGroupDescription,CreatedOn=@CreatedOn,CreatedBy=@CreatedBy,LastUpdated=@LastUpdated,LastUpdatedBy=@LastUpdatedBy,Status=@Status WHERE BloodGroupId=@BloodGroupId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@BloodGroupId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@BloodGroupId"].Value = bloodGroup.BloodGroupId;

                    sqlCmd.Parameters.Add("@BloodGroupName", SqlDbType.VarChar, 30);
                    sqlCmd.Parameters["@BloodGroupName"].Value = bloodGroup.BloodGroupName;

                    sqlCmd.Parameters.Add("@BloodGroupDescription", SqlDbType.VarChar, 30);
                    sqlCmd.Parameters["@BloodGroupDescription"].Value = bloodGroup.BloodGroupDescription;

                    sqlCmd.Parameters.Add("@CreatedOn", SqlDbType.DateTime, 8);
                    sqlCmd.Parameters["@CreatedOn"].Value = bloodGroup.CreatedOn;

                    sqlCmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 4);
                    sqlCmd.Parameters["@CreatedBy"].Value = bloodGroup.CreatedBy;

                    sqlCmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime, 8);
                    sqlCmd.Parameters["@LastUpdated"].Value = bloodGroup.LastUpdated;

                    sqlCmd.Parameters.Add("@LastUpdatedBy", SqlDbType.Int, 4);
                    sqlCmd.Parameters["@LastUpdatedBy"].Value = bloodGroup.LastUpdatedBy;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 4);
                    sqlCmd.Parameters["@Status"].Value = bloodGroup.Status;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();
                    result = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "Updated");
                    htResult.Add("BloodGroupId", Convert.ToString(bloodGroup.BloodGroupId));
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
                ExceptionLog.WriteErrorLog("UpdateBloodGroup", e.Message, e.StackTrace);
            }
            finally
            {
                _conn.Close();
            }
            Result = ser.Serialize(htResult);
            return Result;

        }

        #endregion[HttpDelete]

        [Route("api/BloodGroup/{Id}")]
        #region Delete Blood Group
        public string DeleteBloodGroupByID(int id)
        {
            ExceptionLog.WriteInfoLog("DeleteBloodGroupByID", "DeleteBloodGroupByID");
            try
            {
                if (ConOperation.IsRegexValid(Convert.ToString(id), ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_BloodGroup SET Status=@Status where BloodGroupId=@BloodGroupId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@BloodGroupId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@BloodGroupId"].Value = id;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 8);
                    sqlCmd.Parameters["@Status"].Value = 0;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();

                    string rowDeleted = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "deleted");
                    htResult.Add("BloodGroupId", Convert.ToString(id));
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
                ExceptionLog.WriteErrorLog("DeleteBloodGroupByID", e.Message, e.StackTrace);
            }
            finally
            {
                _conn.Close();
            }
            Result = ser.Serialize(htResult);
            return Result;


        }
        #endregion


        #region IsCurrencyExist
        public int GetIsExists(string bloodGrpName)
        {
            ExceptionLog.WriteInfoLog("Get", "GetIsExists");

            #region GetIsExists

            DBOperations dbOperations = new DBOperations();
            int ttlBldGrpCnt = 0;
            try
            {
                DataSet dsCurrency = new DataSet();
                var qryCurncy = "select * from TBL_EFW_BloodGroup where BloodGroupName =@BloodGroupName and Status=@Status";

                SqlCommand cmdTotalBldGrpCount = new SqlCommand();
                cmdTotalBldGrpCount.Parameters.Add("@BloodGroupName", SqlDbType.VarChar, 30);
                cmdTotalBldGrpCount.Parameters["@BloodGroupName"].Value = Convert.ToString(bloodGrpName);

                cmdTotalBldGrpCount.Parameters.Add("@Status", SqlDbType.Bit, 4);
                cmdTotalBldGrpCount.Parameters["@Status"].Value = 1;

                try
                {
                    {
                        ttlBldGrpCnt = Convert.ToInt32(dbOperations.ExecuteScalarWithCommand(qryCurncy, cmdTotalBldGrpCount));
                    }
                }
                catch (Exception e)
                {
                    ExceptionLog.WriteErrorLog("GetIsExists", e.Message, e.StackTrace);
                    ttlBldGrpCnt = 0;
                }
            }
            catch (Exception e)
            {
                ExceptionLog.WriteErrorLog("GetIsExists", e.Message, e.StackTrace);
            }
            return ttlBldGrpCnt;
            #endregion
        }

        #endregion
        #endregion
    }
}


    




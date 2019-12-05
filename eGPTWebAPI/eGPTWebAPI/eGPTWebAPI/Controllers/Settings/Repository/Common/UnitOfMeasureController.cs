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
using eGPTWebAPI.Models.Settings.Repository.Common;

namespace eGPTWebAPI.Controllers.Settings.Repository.Common
{
    public class UnitOfMeasureController : ApiController
    {
        SqlConnection _conn = new SqlConnection(
        ConfigurationManager.ConnectionStrings["eGPTDBB"].ToString());
        Hashtable htResult = new Hashtable();
        public string strErrorMsgForUser = "Error...";
        JavaScriptSerializer ser = new JavaScriptSerializer();
        JsonSerialize js = new JsonSerialize();
        string Result = "";

        #region Unit Of Measure
        // GET api/UnitOfMeasure
        #region GetUnitOfMeasure
        public string GetUnitOfMeasure()
        {
            ExceptionLog.WriteInfoLog("GetUnitOfMeasure", "GetUnitOfMeasure");

           
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();

            try
            {
                DataSet dsUOM = new DataSet();
                var qryUOM = "select * from TBL_EFW_UnitOfMeasure";

                dsUOM = dbOperations.GetDataSetforQuery(qryUOM);

                if (dsUOM != null && dsUOM.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetUnitOfMeasure", dsUOM, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetUnitOfMeasure", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;
            
        }
        #endregion

        // GET api/UnitOfMeasure/Id
        [Route("api/UnitOfMeasure/{Id}")]
        #region UnitOfMeasure By ID
            public string GetUOMById(int Id)
        {
            ExceptionLog.WriteInfoLog("GetUOMById", "GetUOMById");
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();
            try
            {
                DataSet dsUOM = new DataSet();
                var qryUOM = "Select * from TBL_EFW_UnitOfMeasure where UnitId=" + Id;

                dsUOM = dbOperations.GetDataSetforQuery(qryUOM);

                if (dsUOM != null && dsUOM.Tables[0].Rows.Count > 0)
                {
                    ConOperation Conop = new ConOperation();
                    htResult.Add("Result", Conop.base64EEncode(Conop.GetResultString("GetUOMById", dsUOM, "DataSet")));
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
                ExceptionLog.WriteErrorLog("GetUOMById", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;

        }
        #endregion

        //GET api/UnitOfMeasure/Id
        [HttpPost]
        #region Add Unit Of Measure
        public string AddUnitOfMeasure(UnitOfMeasure uom)
        {
            ExceptionLog.WriteInfoLog("AddUnitOfMeasure", "AddUnitOfMeasure");
            string errMessage = string.Empty;
            DBOperations dbOperations = new DBOperations();
            try
            {
                int uomCnt = GetIsExists(uom.UnitOfMeasureName);

                if (uomCnt <= 0)
                {
                     if (ConOperation.IsRegexValid(uom.UnitOfMeasureName, ConOperation.strReg))
                     {
                        string strStoreUom = "insert into TBL_EFW_UnitOfMeasure values(@UnitOfMeasureName, @UnitOfMeasureDescription, @LastUpdated, @LastUpdatedBy, @Status)";

                        SqlCommand cmdStoreUom = new SqlCommand();
                        cmdStoreUom.Parameters.Add("@UnitOfMeasureName", SqlDbType.VarChar, 30);
                        cmdStoreUom.Parameters["@UnitOfMeasureName"].Value = Convert.ToString(uom.UnitOfMeasureName);

                        SqlParameter uomDesc = new SqlParameter("@UnitOfMeasureDescription", SqlDbType.VarChar, 30);
                        uomDesc.Value = uom.UnitOfMeasureDescription;
                        cmdStoreUom.Parameters.Add(uomDesc);

                        SqlParameter lastUpdatedParam = new SqlParameter("@LastUpdated", SqlDbType.DateTime, 8);
                        lastUpdatedParam.Value = uom.LastUpdated;
                        cmdStoreUom.Parameters.Add(lastUpdatedParam);

                        SqlParameter lastUpdatedByparam = new SqlParameter("@LastUpdatedBy", SqlDbType.Int, 10);
                        lastUpdatedByparam.Value = uom.LastUpdatedBy;
                        cmdStoreUom.Parameters.Add(lastUpdatedByparam);

                        SqlParameter statusParam = new SqlParameter("@Status", SqlDbType.Bit, 10);
                        statusParam.Value = uom.Status;
                        cmdStoreUom.Parameters.Add(statusParam);

                        Hashtable htUserData = new Hashtable();
                        htUserData.Add("UnitId", Convert.ToString(uom.UnitId));


                        if (htUserData != null)
                        {
                            dbOperations.ExecuteNonQueryWithCommand(strStoreUom, cmdStoreUom);
                            htResult.Add("Status", "OK");
                            htResult.Add("Message", "Inserted");
                            htResult.Add("BizId", Convert.ToString(uom.UnitId));
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
                ExceptionLog.WriteErrorLog("AddUnitOfMeasure", e.Message, e.StackTrace);
            }
            Result = ser.Serialize(htResult);
            return Result;

        }
        #endregion

        [HttpPut]
        #region Update UOM
        public string UpdateUnitOfMeasure(UnitOfMeasure unitOfMeasure)
        { 
       
            ExceptionLog.WriteInfoLog("UpdateUnitOfMeasure", "UpdateUnitOfMeasure");
            string result = "";
            try
            {
                if (ConOperation.IsRegexValid(unitOfMeasure.UnitOfMeasureName, ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_UnitOfMeasure SET UnitOfMeasureName=@UnitOfMeasureName,UnitOfMeasureDescription=@UnitOfMeasureDescription,LastUpdated=@LastUpdated,LastUpdatedBy=@LastUpdatedBy,Status=@Status where  UnitId=@UnitId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@UnitId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@UnitId"].Value = unitOfMeasure.UnitId;

                    sqlCmd.Parameters.Add("@UnitOfMeasureName", SqlDbType.VarChar, 30);
                    sqlCmd.Parameters["@UnitOfMeasureName"].Value = unitOfMeasure.UnitOfMeasureName;

                    sqlCmd.Parameters.Add("@UnitOfMeasureDescription", SqlDbType.VarChar, 30);
                    sqlCmd.Parameters["@UnitOfMeasureDescription"].Value = unitOfMeasure.UnitOfMeasureDescription;

                    sqlCmd.Parameters.Add("@LastUpdated", SqlDbType.DateTime, 8);
                    sqlCmd.Parameters["@LastUpdated"].Value = unitOfMeasure.LastUpdated;

                    sqlCmd.Parameters.Add("@LastUpdatedBy", SqlDbType.Int, 4);
                    sqlCmd.Parameters["@LastUpdatedBy"].Value = unitOfMeasure.LastUpdatedBy;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 4);
                    sqlCmd.Parameters["@Status"].Value = unitOfMeasure.Status;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();
                    result = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "Updated");
                    htResult.Add("BloodGroupId", Convert.ToString(unitOfMeasure.UnitId));
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
                ExceptionLog.WriteErrorLog("UpdateUnitOfMeasure", e.Message, e.StackTrace);
            }
            finally
            {
                _conn.Close();
            }
            Result = ser.Serialize(htResult);
            return Result;

        }
        #endregion

        [HttpDelete]
        [Route("api/UnitOfMeasure/{Id}")]
        #region DeleteUnitOfMeasure
        public string DeleteUnitOfMeasure(int id)
        {
            ExceptionLog.WriteInfoLog("DeleteUnitOfMeasure", "DeleteUnitOfMeasure");

            
            try
            {
                if (ConOperation.IsRegexValid(Convert.ToString(id), ConOperation.strReg))
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "UPDATE TBL_EFW_UnitOfMeasure SET Status=@Status where UnitId=@UnitId";
                    sqlCmd.Connection = _conn;

                    sqlCmd.Parameters.Add("@UnitId", SqlDbType.Int, 8);
                    sqlCmd.Parameters["@UnitId"].Value = id;

                    sqlCmd.Parameters.Add("@Status", SqlDbType.Bit, 8);
                    sqlCmd.Parameters["@Status"].Value = 0;

                    if (_conn.State == ConnectionState.Closed)
                        _conn.Open();

                    string rowDeleted = sqlCmd.ExecuteNonQuery().ToString();

                    htResult.Add("Status", "OK");
                    htResult.Add("Message", "deleted");
                    htResult.Add("UnitId", Convert.ToString(id));
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
                ExceptionLog.WriteErrorLog("DeleteUnitOfMeasure", e.Message, e.StackTrace);
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
        public int GetIsExists(string uomName)
        {
            ExceptionLog.WriteInfoLog("Get", "GetIsExists");

            #region GetIsExists

            DBOperations dbOperations = new DBOperations();
            int ttlUOMCnt = 0;
            try
            {
                DataSet dsUOM = new DataSet();
                var qryUOM = "select * from TBL_EFW_UnitOfMeasure where UnitOfMeasureName =@UnitOfMeasureName and Status=@Status";

                SqlCommand cmdTotalUomCount = new SqlCommand();
                cmdTotalUomCount.Parameters.Add("@UnitOfMeasureName", SqlDbType.VarChar, 30);
                cmdTotalUomCount.Parameters["@UnitOfMeasureName"].Value = Convert.ToString(uomName);

                cmdTotalUomCount.Parameters.Add("@Status", SqlDbType.Bit, 4);
                cmdTotalUomCount.Parameters["@Status"].Value = 1;

                try
                {
                    {
                        ttlUOMCnt = Convert.ToInt32(dbOperations.ExecuteScalarWithCommand(qryUOM, cmdTotalUomCount));
                    }
                }
                catch (Exception e)
                {
                    ExceptionLog.WriteErrorLog("GetIsExists", e.Message, e.StackTrace);
                    ttlUOMCnt = 0;
                }
            }
            catch (Exception e)
            {
                ExceptionLog.WriteErrorLog("GetIsExists", e.Message, e.StackTrace);
            }
            return ttlUOMCnt;
            #endregion
        }

        #endregion

        #endregion
    }
}

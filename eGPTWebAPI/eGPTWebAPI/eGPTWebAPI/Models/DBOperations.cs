using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace eGPTWebAPI.Models
{
    public class DBOperations
    {
            private static readonly ILog log = LogManager.GetLogger(typeof(DBOperations));
            //Current Day Start Time
            public static DateTime dtDayStart = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");
            //Current Day End Time
            public static DateTime dtDayEnd = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");

            public static string strConStr = String.Empty;

            string queryValidation = "^([0-9a-zA-Z@=, .])";
            public DBOperations()
            {
                log4net.Config.XmlConfigurator.Configure();
                strConStr = ConOperation.ConString();
            }
            public string GetConString()
            {
                log.Debug("Inside Get Connection String");
                string strConString = String.Empty;
                strConString = ConOperation.ConString();
                //log.Debug("Inside Connection String");
                return strConString;
            }

            /// <summary>
            /// A Method to return DataTable for the Specified Stored Procedure Name
            /// </summary>
            /// <param name="strSPName">The Name of the Stored Prcedure</param>
            /// <returns>A Datatable</returns>
            public DataTable GetDataTableforSP(string strSPName)
            {
                DataTable dt = new DataTable();
                try
                {
                    //string strConStr = GetConString();
                    using (SqlConnection con = new SqlConnection(strConStr))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = strSPName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.SelectCommand.Prepare();
                        da.Fill(dt);
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(strSPName + "  " + ex.Message + "   " + ex.StackTrace);
                    throw ex;
                }
                return dt;
            }


            /// <summary>
            /// A Method to return DataTable for the Specified Stored Procedure Name
            /// </summary>
            /// <param name="strSPName">The Name of the Stored Prcedure</param>
            /// <returns>A Datatable</returns>
            public DataSet GetDataSetforSP(string strSPName)
            {
                DataSet dt = new DataSet();
                try
                {
                    //string strConStr = GetConString();
                    using (SqlConnection con = new SqlConnection(strConStr))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = strSPName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.SelectCommand.Prepare();
                        da.Fill(dt);
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(strSPName + "  " + ex.Message + "   " + ex.StackTrace);
                    throw ex;
                }
                return dt;
            }


            /// <summary>
            /// A Method to return DataTable for the Specified Query
            /// </summary>
            /// <param name="strQuery">The Query to be executed</param>
            /// <returns>A Datatable</returns>
            public DataTable GetDataTableforQuery(string strQuery)
            {
                DataTable dt = new DataTable();
                try
                {
                    using (SqlConnection con = new SqlConnection(strConStr))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = strQuery;
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.SelectCommand.Prepare();
                        da.Fill(dt);
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(strQuery + "  " + ex.Message + "  " + ex.StackTrace);
                    throw ex;
                }
                return dt;
            }

            public DataTable GetDataTableforQueryWithCommand(string strQuery, SqlCommand cmd)
            {
                DataTable dt = new DataTable();
                try
                {
                    if (ConOperation.IsRegexValid(strQuery, queryValidation))
                    {
                        using (SqlConnection con = new SqlConnection(strConStr))
                        {
                            con.Open();
                            cmd.Connection = con;
                            cmd.CommandText = strQuery;
                            cmd.CommandType = CommandType.Text;
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.SelectCommand.Prepare();
                            da.Fill(dt);
                            con.Close();
                        }
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    log.Error(strQuery + " , " + ex.Message + " , " + ex.StackTrace);
                    throw ex;
                }
                return dt;
            }

            public DataSet GetDataSetforQuery(string strQuery)
            {
                DataSet ds = new DataSet();
                try
                {
                    using (SqlConnection con = new SqlConnection(strConStr))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = strQuery;
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.SelectCommand.Prepare();
                        da.Fill(ds);
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(strQuery + " , " + ex.Message + " , " + ex.StackTrace);
                    throw ex;
                }
                return ds;
            }

        public DataSet GetDataSetforQuery(StringBuilder strQry)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(strConStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = Convert.ToString(strQry);
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.Prepare();
                    da.Fill(ds);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(strQry + " , " + ex.Message + " , " + ex.StackTrace);
                throw ex;
            }
            return ds;
        }

        public DataSet GetDataSetforQueryWithCommand(string strQuery, SqlCommand cmd)
            {
                DataSet ds = new DataSet();
                try
                {
                    using (SqlConnection con = new SqlConnection(strConStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = strQuery;
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.SelectCommand.Prepare();
                        da.Fill(ds);
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(strQuery + " , " + ex.Message + " , " + ex.StackTrace);
                    throw ex;
                }
                return ds;
            }

            /// <summary>
            /// A Method to return DataTable for the Specified Query
            /// </summary>
            /// <param name="strQuery">The Query to be executed</param>
            /// <param name="strParams">Parameters</param>
            /// <returns>A Datatable</returns>
            public DataTable GetDataTableforQueryUsingParams(string strQuery, string strParams)
            {
                DataTable dt = new DataTable();
                try
                {
                    using (SqlConnection con = new SqlConnection(strConStr))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = strQuery;
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);

                        string[] p = strParams.Split('|');

                        foreach (string ps in p)
                        {
                            string[] paramss = ps.Split(',');

                            cmd.Parameters.Add(ps.Split(',')[0], GetSqlDBType(ps.Split(',')[1]), Convert.ToInt32(ps.Split(',')[2]));
                            cmd.Parameters[ps.Split(',')[0]].Value = ps.Split(',')[3];
                        }

                        da.SelectCommand.Prepare();
                        da.Fill(dt);
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return dt;
            }

            public SqlDbType GetSqlDBType(string strDBType)
            {
                SqlDbType dbType = new SqlDbType();
                switch (strDBType)
                {
                    case "VarChar":
                        dbType = SqlDbType.VarChar;
                        break;

                    case "Bit":
                        dbType = SqlDbType.Bit;
                        break;

                    case "Date":
                        dbType = SqlDbType.Date;
                        break;

                    case "DateTime":
                        dbType = SqlDbType.DateTime;
                        break;

                    case "Int":
                        dbType = SqlDbType.Int;
                        break;
                }
                return dbType;
            }

            public string GetSingleValueFromQuery(string strQuery)
            {
                string strValue = String.Empty;
                try
                {
                    using (SqlConnection con = new SqlConnection(strConStr))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = strQuery;
                        cmd.CommandType = CommandType.Text;

                        strValue = Convert.ToString(cmd.ExecuteScalar());

                        con.Close();
                    }
                }
                catch
                {
                    throw;
                }
                return strValue;
            }

            public int ExecuteNonQuery(string strQuery)
            {
                int intResult = 0;
                try
                {
                    using (SqlConnection con = new SqlConnection(strConStr))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = strQuery;
                        cmd.CommandType = CommandType.Text;

                        intResult = cmd.ExecuteNonQuery();

                        con.Close();
                    }
                }
                catch
                {
                    throw;
                }
                return intResult;

            }

            public int ExecuteNonQueryWithParams(string strQuery, string strParams)
            {
                int intResult = 0;
                try
                {
                    using (SqlConnection con = new SqlConnection(strConStr))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = strQuery;
                        cmd.CommandType = CommandType.Text;

                        string[] p = strParams.Split('|');

                        foreach (string ps in p)
                        {
                            string[] paramss = ps.Split(',');

                            cmd.Parameters.Add(ps.Split(',')[0], GetSqlDBType(ps.Split(',')[1]), Convert.ToInt32(ps.Split(',')[2]));
                            cmd.Parameters[ps.Split(',')[0]].Value = ps.Split(',')[3];
                        }

                        intResult = cmd.ExecuteNonQuery();

                        con.Close();
                    }
                }
                catch
                {
                    throw;
                }
                return intResult;

            }

            public int ExecuteNonQueryWithCommand(string strQuery, SqlCommand cmd)
            {
                int intResult = 0;
                try
                {
                    using (SqlConnection con = new SqlConnection(strConStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = strQuery;
                        cmd.CommandType = CommandType.Text;
                        cmd.Prepare();
                        intResult = cmd.ExecuteNonQuery();

                        con.Close();
                    }
                }
                catch
                {
                    throw;
                }
                return intResult;

            }

            public int ExecuteNonQueryWithCommandAndTransaction(List<SqlCommand> lstCmd)
            {
                int intResult = 0;
                try
                {
                    using (SqlConnection con = new SqlConnection(strConStr))
                    {
                        SqlTransaction transaction;
                        con.Open();
                        transaction = con.BeginTransaction();

                        foreach (SqlCommand cmd in lstCmd)
                        {
                            //ExceptionLog.WriteInfoLog("ExecuteNonQueryWithCommandAndTransaction",cmd.CommandText);
                            cmd.Connection = con;
                            cmd.CommandType = CommandType.Text;
                            cmd.Transaction = transaction;
                            cmd.Prepare();
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        con.Close();
                    }
                }
                catch (Exception e)
                {
                    log.Error("ExecuteNonQueryWithCommandAndTransaction" + " , " + e.Message + " , " + e.StackTrace);
                    throw e;
                }
                return intResult;

            }

            public string ExecuteScalarWithCommand(string strQuery, SqlCommand cmd)
            {
                string strResult = String.Empty;
                try
                {
                    using (SqlConnection con = new SqlConnection(strConStr))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = strQuery;
                        cmd.CommandType = CommandType.Text;
                        cmd.Prepare();
                        strResult = Convert.ToString(cmd.ExecuteScalar());

                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return strResult;
            }

            public void Dispose()
            {
                try
                {
                    //this.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }        
    }
}
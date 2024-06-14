/********************************************************************************************
 * Project Name - DataAccess Handler
 * Description  - Setting up SQL Connection and managing the SQL operations
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        01-Dec-2015   Kiran          Created 
 *********************************************************************************************
 *1.00        18-Aug-2016   Raghuveera     Modified
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
//using ParafaitUtils;
using Semnox.Parafait.logging;
using Semnox.Core.Utilities;

namespace Semnox.Core.DBUtils
{
    public class DataAccessHandler
    {
        private SqlDataAdapter dataAdapter;
        private SqlConnection conn;
        private  int commandTimeout ;
        private string tempConnectionString;

        Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataAccessHandler()
        {
            try
            {
                log.LogMethodEntry();
                dataAdapter = new SqlDataAdapter();
                //conn = new SqlConnection(ConfigurationManager.ConnectionStrings
                //        ["ParafaitConnectionString"].ConnectionString);
                //Below change done on 25-Jan-2016 to handle connection string name
                string connString = "";
            
                commandTimeout = -1;
                try
                {
                    connString = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
                }
                catch { }
                if (string.IsNullOrEmpty(connString))
                    connString = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ConnectionString;
                string connectionString = StaticUtils.getParafaitConnectionString(connString);
                tempConnectionString = connString;  // Addded to get Connection String for Generic Method 
                conn = new SqlConnection(connectionString);
                //End modification on 25-Jan-2016
                
            }
            catch (SqlException)
            {
                log.LogMethodExit(null, "Fatal error inside DataAccessHandler constructor");
                log.Fatal("Fatal error inside DataAccessHandler constructor");
                throw;
            }
            log.LogMethodExit(null);
        }


        /// <summary>
        /// DataAccessHandler Constructor with params
        /// </summary>
        /// <param name="connectionString">string connectionString</param>
        public DataAccessHandler(string connectionString)
        {
            try
            {
                log.LogMethodEntry("connectionString");
                dataAdapter = new SqlDataAdapter();
                commandTimeout = -1;
                string connString = StaticUtils.getParafaitConnectionString(connectionString);
                conn = new SqlConnection(connString);
                log.Debug("Exiting DataAccessHandler constructor after getting sqlConnection");
            }
            catch (SqlException)
            {
                log.LogMethodExit(null, "Fatal error inside DataAccessHandler constructor");
                log.Fatal("Fatal error inside DataAccessHandler constructor");
                throw;
            }
            log.LogMethodExit(null);
        }




        /// <summary>
        /// Get/Set for MessageText field
        /// </summary>
        public string ConnectionString { get { return tempConnectionString; } }


        /// <summary>
        ///  Get/Set for CommandTimeOut field
        /// </summary>
        public int CommandTimeOut { get { return commandTimeout; } set { commandTimeout = value; } }


        private SqlConnection openConnection()
        {
            log.LogMethodEntry();
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
            {
                conn.Open();
            }
            log.LogMethodExit(null);
            return conn;
        }

        public DataTable executeSelectQuery(String _query, SqlParameter[] sqlParameter)
        {
            return executeSelectQuery(_query, sqlParameter, null);
        }
        /// <method>
        /// Select Query
        /// </method>
        public DataTable executeSelectQuery(String _query, SqlParameter[] sqlParameter, SqlTransaction sqlTrxn)//Modification on 18-Aug-2016: added SqlTransaction
        {
            log.LogMethodEntry(_query, sqlParameter, sqlTrxn);
            SqlCommand sqlCommand = new SqlCommand();
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet ds = new DataSet();
            try
            {
                if (sqlTrxn == null)//Starts:Modification on 18-Aug-2016: added SqlTransaction
                {
                    sqlCommand.Connection = openConnection();
                }
                else
                {
                    sqlCommand.Connection = sqlTrxn.Connection;
                    if (commandTimeout >= 0)
                    {
                        sqlCommand.CommandTimeout = commandTimeout;
                    }
                    sqlCommand.Transaction = sqlTrxn;
                }//Ends:Modification on 18-Aug-2016: added SqlTransaction

                sqlCommand.CommandText = _query;
                if (sqlParameter != null)
                    sqlCommand.Parameters.AddRange(sqlParameter);
                //sqlCommand.ExecuteNonQuery();                
                dataAdapter.SelectCommand = sqlCommand;
                dataAdapter.Fill(ds);
                dataTable = ds.Tables[0];
            }
            catch (SqlException ex)
            {
                log.Error("Error occured while executing the select query", ex);
                log.LogMethodExit(null);
                throw;
            }
            finally
            {
                if (sqlTrxn == null)//Modification on 18-Aug-2016: added SqlTransaction
                    sqlCommand.Connection.Close();
            }
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <method>
        /// Execute scalar
        /// </method>
        public object executeScalar(String _query, SqlParameter[] sqlParameter, SqlTransaction sqlTrxn)//Modification on 18-Aug-2016: added SqlTransaction
        {
            log.LogMethodEntry(_query, sqlParameter, sqlTrxn);
            SqlCommand sqlCommand = new SqlCommand();
            object returnValue = null;
            try
            {
                if (sqlTrxn == null)//Starts:Modification on 18-Aug-2016: added SqlTransaction
                {
                    sqlCommand.Connection = openConnection();
                }
                else
                {
                    sqlCommand.Connection = sqlTrxn.Connection;
                    if (commandTimeout >= 0)
                    {
                        sqlCommand.CommandTimeout = commandTimeout;
                    }
                    sqlCommand.Transaction = sqlTrxn;
                }//Ends:Modification on 18-Aug-2016: added SqlTransaction

                sqlCommand.CommandText = _query;
                if (sqlParameter != null)
                    sqlCommand.Parameters.AddRange(sqlParameter);
                returnValue = sqlCommand.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                log.Error("Error occured while executing the select query", ex);
                log.LogMethodExit(null);
                throw;
            }
            finally
            {
                if (sqlTrxn == null)//Modification on 18-Aug-2016: added SqlTransaction
                    sqlCommand.Connection.Close();
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public int executeInsertQuery(String _query, SqlParameter[] sqlParameter)//Modification on 18-Aug-2016: added SqlTransaction
        {
            return executeInsertQuery(_query, sqlParameter, null);
        }//Modification on 18-Aug-2016: added SqlTransaction

        /// <method>
        /// Insert Query
        /// </method>
        public int executeInsertQuery(String _query, SqlParameter[] sqlParameter, SqlTransaction sqlTrxn)//Modification on 18-Aug-2016: added SqlTransaction
        {
            log.LogMethodEntry(_query, sqlParameter, sqlTrxn);
            SqlCommand insertCommand = new SqlCommand();
            try
            {
                if (sqlTrxn == null)//Starts:Modification on 18-Aug-2016: added SqlTransaction
                {
                    insertCommand.Connection = openConnection();
                }
                else
                {
                    insertCommand.Connection = sqlTrxn.Connection;
                    if (commandTimeout >= 0)
                    {
                        insertCommand.CommandTimeout = commandTimeout;
                    }
                    insertCommand.Transaction = sqlTrxn;
                }//Ends:Modification on 18-Aug-2016: added SqlTransaction

                insertCommand.CommandText = _query;
                insertCommand.Parameters.AddRange(sqlParameter);
                dataAdapter.InsertCommand = insertCommand;
                int insertRecordId = (int)insertCommand.ExecuteScalar();
                log.LogMethodExit(insertRecordId);
                return insertRecordId;
            }
            catch (SqlException ex)
            {
                log.Error("Error occured while executing the insert query", ex);
                log.LogMethodExit(null);
                throw;
            }
            finally
            {
                if (sqlTrxn == null)//Modification on 18-Aug-2016: added SqlTransaction
                    insertCommand.Connection.Close();
            }
        }

        /// <summary>
        /// Returns the sql parameter from the parametername and value passed
        /// if isForeignKey should be passed true if the column is a forgien key column
        /// </summary>
        /// <param name="parameterName">name of the parameter</param>
        /// <param name="value">value of the parameter</param>
        /// <param name="isForeignKey">is a forign key value</param>
        /// <returns></returns>
        public SqlParameter GetSQLParameter(string parameterName, object value, bool isForeignKey = false)
        {
            log.LogMethodEntry(parameterName, value, isForeignKey);
            SqlParameter parameter = null;
            if (value is int )
            {
                if (isForeignKey && ((int)value) < 0)
                {
                    parameter = new SqlParameter(parameterName, DBNull.Value);
                }
                else
                {
                    parameter = new SqlParameter(parameterName, value);
                }
            }
            else if (value is long)
            {
                if (isForeignKey && ((long)value) < 0)
                {
                    parameter = new SqlParameter(parameterName, DBNull.Value);
                }
                else
                {
                    parameter = new SqlParameter(parameterName, value);
                }
            }
            else if (value is string)
            {
                if (string.IsNullOrEmpty(value as string))
                {
                    parameter = new SqlParameter(parameterName, DBNull.Value);
                }
                else
                {
                    parameter = new SqlParameter(parameterName, value);
                }
            }
            else
            {
                if (value == null)
                {
                    parameter = new SqlParameter(parameterName, DBNull.Value);
                }
                else
                {
                    parameter = new SqlParameter(parameterName, value);
                }
            }
            log.LogMethodExit(parameter);
            return parameter;
        }

        public int executeUpdateQuery(String _query, SqlParameter[] sqlParameter)//Modification on 18-Aug-2016: added SqlTransaction
        {
            return executeUpdateQuery(_query, sqlParameter, null);//Modification on 18-Aug-2016: added SqlTransaction
        }//Modification on 18-Aug-2016: added SqlTransaction

        /// <method>
        /// Update Query
        /// </method>
        public int executeUpdateQuery(String _query, SqlParameter[] sqlParameter, SqlTransaction sqlTrxn)//Modification on 18-Aug-2016: added SqlTransaction
        {
            log.LogMethodEntry(_query, sqlParameter, sqlTrxn);
            SqlCommand updateCommand = new SqlCommand();
            try
            {
                if (sqlTrxn == null)//Starts:Modification on 18-Aug-2016: added SqlTransaction
                {
                    updateCommand.Connection = openConnection();
                }
                else
                {

                    updateCommand.Connection = sqlTrxn.Connection;
                    if (commandTimeout >= 0)
                    {
                        updateCommand.CommandTimeout = commandTimeout;
                    }
                    updateCommand.Transaction = sqlTrxn;
                }//Ends:Modification on 18-Aug-2016: added SqlTransaction

                updateCommand.CommandText = _query;
                updateCommand.Parameters.AddRange(sqlParameter);
                dataAdapter.UpdateCommand = updateCommand;
                int numberOfRecords = updateCommand.ExecuteNonQuery();
                log.LogMethodExit(numberOfRecords);
                return numberOfRecords;
            }
            catch (SqlException ex)
            {
                log.Error("Error occured while executing the update query", ex);
                log.LogMethodExit(null);
                throw;
            }
            finally
            {
                if (sqlTrxn == null)//Modification on 18-Aug-2016: added SqlTransaction
                    updateCommand.Connection.Close();
            }
        }
        //Defined destructor to close sqlConnection
        ~DataAccessHandler()
        {
            log.LogMethodEntry();
            if (conn != null)
            {
                try
                {
                    conn.Close();
                }
                catch { }
            }
            log.LogMethodExit(null);
        }
    }
}

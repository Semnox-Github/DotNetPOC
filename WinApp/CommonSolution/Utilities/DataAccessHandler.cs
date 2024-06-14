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
 *2.60.2      29-May-2019   Jagan Mohana   Code merge from Development to WebManagementStudio
 *2.90.0      29-Jul-2020   Girish Kundar  Modified: BatchSelect() - issue fix when more than one batch exists sql parameter duplicating 
 *2.120.0     09-Oct-2020   Guru S A       Membership engine sql session issue
 *2.130.0     12-Jul-2021   Lakshminarayana Modified : Static menu enhancement
 *2.130.0     31-Aug-2021   Guru S A       Enable Serial number based card load
 *2.140.1     28-Feb-2022   Nitin Pai      Fix: Gateway website crashes with app pool corruption. Enclosing connections in using.
 *2.150.2     20-Jun-2023   Nitin Pai      Security Testing Fix: Catch SQL excpeption and throw as generic exception.
 *2.150.2     20-Jun-2023   Nitin Pai      App pool crashing issues - Check connection status before opening
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Reflection;
//using ParafaitUtils;
using Semnox.Parafait.logging;
using Semnox.Core.Utilities;
using Microsoft.SqlServer.Server;
using System.Drawing;

namespace Semnox.Core.Utilities
{
    public class DataAccessHandler
    {
        private SqlDataAdapter dataAdapter;
        private SqlConnection conn;
        private int commandTimeout;
        private string tempConnectionString;
        private string sqlConnectionString;
        private string genericErrorMessage = "SQL exception encountered. Check log files to get more details.";
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataAccessHandler()
        {
            log.LogMethodEntry();
            try
            {
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
                catch (Exception ex)
                {
                    //log.Error(ex);
                }
                if (string.IsNullOrEmpty(connString))
                {
                    try
                    {
                        connString = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ConnectionString;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
                if (string.IsNullOrWhiteSpace(connString))
                {
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    fileMap.ExeConfigFilename = Assembly.GetExecutingAssembly().Location + ".config";

                    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                    ConnectionStringSettingsCollection conStrings = config.ConnectionStrings.ConnectionStrings;//Get collection of connection strings
                    try
                    {
                        connString = conStrings["ParafaitConnectionString"].ConnectionString;
                    }
                    catch { }
                }

                string connectionString = StaticUtils.getParafaitConnectionString(connString);
                tempConnectionString = connString;  // Addded to get Connection String for Generic Method 
                sqlConnectionString = connectionString;
                //conn = new SqlConnection(connectionString);
                //End modification on 25-Jan-2016

            }
            catch (SqlException ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Fatal error inside DataAccessHandler constructor");
                log.Fatal("Fatal error inside DataAccessHandler constructor");
                throw new Exception(genericErrorMessage);
            }
            log.LogMethodExit(null);
        }


        /// <summary>
        /// DataAccessHandler Constructor with params
        /// </summary>
        /// <param name="connectionString">string connectionString</param>
        public DataAccessHandler(string connectionString)
        {
            log.LogMethodEntry("connectionString");
            try
            {
                dataAdapter = new SqlDataAdapter();
                commandTimeout = -1;
                string connString = StaticUtils.getParafaitConnectionString(connectionString);
                //conn = new SqlConnection(connString);
                sqlConnectionString = connString;
                log.Debug("Exiting DataAccessHandler constructor after getting sqlConnection");
            }
            catch (SqlException ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Fatal error inside DataAccessHandler constructor");
                log.Fatal("Fatal error inside DataAccessHandler constructor");
                throw new Exception(genericErrorMessage);
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
            if (conn == null)
            {
                conn = new SqlConnection(sqlConnectionString);
            }
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            else if (conn.State == System.Data.ConnectionState.Broken)
            {
                conn.Close();
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
        public DataTable executeSelectQuery(String _query, SqlParameter[] sqlParameter, SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(_query, sqlParameter, sqlTrxn);
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet ds = new DataSet();
            if (sqlTrxn == null)
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        sqlCommand.Connection = openConnection();
                        if (commandTimeout >= 0)
                        {
                            sqlCommand.CommandTimeout = commandTimeout;
                        }
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
                        throw new Exception(genericErrorMessage);
                    }
                    finally
                    {
                        sqlCommand.Connection.Close();
                    }
                }
            }
            else
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlTrxn.Connection;
                    sqlCommand.Transaction = sqlTrxn;
                    if (commandTimeout >= 0)
                    {
                        sqlCommand.CommandTimeout = commandTimeout;
                    }
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
                    throw new Exception(genericErrorMessage);
                }
            }
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        public T GetDataFromReader<T>(String _query, Func<SqlDataReader, T> ReadDataFromSqlReader)
        {
            return GetDataFromReader(_query, null, null, ReadDataFromSqlReader);
        }

        public T GetDataFromReader<T>(String _query, SqlParameter[] sqlParameter, Func<SqlDataReader, T> ReadDataFromSqlReader)
        {
            return GetDataFromReader(_query, sqlParameter, null, ReadDataFromSqlReader);
        }
        public T GetDataFromReader<T>(String _query, SqlParameter[] sqlParameter, SqlTransaction sqlTrxn, Func<SqlDataReader, T> ReadDataFromSqlReader)
        {
            log.LogMethodEntry(_query, sqlParameter, sqlTrxn);
            T result = default(T);

            if (sqlTrxn == null)
            {
                using (SqlCommand sqlCommand = new SqlCommand(_query))
                {
                    try
                    {
                        sqlCommand.Connection = openConnection();
                        if (commandTimeout >= 0)
                        {
                            sqlCommand.CommandTimeout = commandTimeout;
                        }
                        if (sqlParameter != null)
                            sqlCommand.Parameters.AddRange(sqlParameter);
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                result = ReadDataFromSqlReader(reader);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        log.Error("Error occured while executing the select query", ex);
                        log.LogMethodExit(null);
                        throw new Exception(genericErrorMessage);
                    }
                    finally
                    {
                        sqlCommand.Connection.Close();
                    }
                }
            }
            else
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(_query);
                    sqlCommand.Connection = sqlTrxn.Connection;
                    sqlCommand.Transaction = sqlTrxn;
                    if (commandTimeout >= 0)
                    {
                        sqlCommand.CommandTimeout = commandTimeout;
                    }
                    if (sqlParameter != null)
                        sqlCommand.Parameters.AddRange(sqlParameter);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            result = ReadDataFromSqlReader(reader);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    log.Error("Error occured while executing the select query", ex);
                    log.LogMethodExit(null);
                    throw new Exception(genericErrorMessage);
                }
            }
            return result;
        }

        /// <method>
        /// Execute scalar
        /// </method>
        public object executeScalar(String _query, SqlParameter[] sqlParameter, SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(_query, sqlParameter, sqlTrxn);
            object returnValue = null;

            if (sqlTrxn == null)
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        sqlCommand.Connection = openConnection();
                        if (commandTimeout >= 0)
                        {
                            sqlCommand.CommandTimeout = commandTimeout;
                        }

                        sqlCommand.CommandText = _query;
                        if (sqlParameter != null)
                            sqlCommand.Parameters.AddRange(sqlParameter);
                        returnValue = sqlCommand.ExecuteScalar();
                    }
                    catch (SqlException ex)
                    {
                        log.Error("Error occured while executing the select query", ex);
                        log.LogMethodExit(null);
                        throw new Exception(genericErrorMessage);
                    }
                    finally
                    {
                        sqlCommand.Connection.Close();
                    }
                }
            }
            else
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlTrxn.Connection;
                    sqlCommand.Transaction = sqlTrxn;
                    if (commandTimeout >= 0)
                    {
                        sqlCommand.CommandTimeout = commandTimeout;
                    }

                    sqlCommand.CommandText = _query;
                    if (sqlParameter != null)
                        sqlCommand.Parameters.AddRange(sqlParameter);
                    returnValue = sqlCommand.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    log.Error("Error occured while executing the select query", ex);
                    log.LogMethodExit(null);
                    throw new Exception(genericErrorMessage);
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public int executeInsertQuery(String _query, SqlParameter[] sqlParameter)
        {
            return executeInsertQuery(_query, sqlParameter, null);
        }

        /// <method>
        /// Insert Query
        /// </method>
        public int executeInsertQuery(String _query, SqlParameter[] sqlParameter, SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(_query, sqlParameter, sqlTrxn);
            if (sqlTrxn == null)
            {
                using (SqlCommand insertCommand = new SqlCommand())
                {
                    try
                    {
                        insertCommand.Connection = openConnection();
                        if (commandTimeout >= 0)
                        {
                            insertCommand.CommandTimeout = commandTimeout;
                        }
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
                        throw new Exception(genericErrorMessage);
                    }
                    finally
                    {
                        insertCommand.Connection.Close();
                    }
                }
            }
            else
            {
                try
                {
                    SqlCommand insertCommand = new SqlCommand();
                    insertCommand.Connection = sqlTrxn.Connection;
                    insertCommand.Transaction = sqlTrxn;
                    if (commandTimeout >= 0)
                    {
                        insertCommand.CommandTimeout = commandTimeout;
                    }
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
                    throw new Exception(genericErrorMessage);
                }
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

        public SqlParameter GetSecureSQLParameter(string parameterName, object value, bool isForeignKey = false)
        {
            log.LogMethodEntry(parameterName, "value", isForeignKey);
            object parameterValue = GetSecureParameterValue(value, isForeignKey);
            SqlParameter parameter = new SqlParameter(parameterName, parameterValue);
            log.LogMethodExit("parameter");
            return parameter;
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
            object parameterValue = GetParameterValue(value, isForeignKey);
            SqlParameter parameter = new SqlParameter(parameterName, parameterValue);
            log.LogMethodExit(parameter);
            return parameter;
        }

        public object GetParameterValue(object input, bool isForeignKey = false)
        {
            log.LogMethodEntry(input, isForeignKey);
            var result = GetSecureParameterValue(input, isForeignKey);
            log.LogMethodExit(result);
            return result;
        }

        public object GetSecureParameterValue(object input, bool isForeignKey = false)
        {
            log.LogMethodEntry("input", isForeignKey);

            object result = DBNull.Value;
            if (input is int)
            {
                if (!isForeignKey || ((int)input) >= 0)
                {
                    result = input;
                }
            }
            else if (input is Guid)
            {
                if (Guid.Empty.Equals(input) == false)
                {
                    result = input;
                }
            }
            else if (input is long)
            {
                if (!isForeignKey || ((long)input) >= 0)
                {
                    result = input;
                }
            }
            else if (input is string)
            {
                if (string.IsNullOrEmpty(input as string) == false)
                {
                    result = input;
                }
            }
            else if (input is DateTime)
            {
                if (((DateTime)input) != DateTime.MinValue)
                {
                    result = input;
                }
            }
            else if (input is double)
            {
                if (double.IsNaN((double)input) == false)
                {
                    result = input;
                }
            }
            else if (input != null)
            {
                result = input;
            }
            log.LogMethodExit("result");
            return result;
        }

        public int executeUpdateQuery(String _query, SqlParameter[] sqlParameter)
        {
            return executeUpdateQuery(_query, sqlParameter, null);
        }

        /// <method>
        /// Update Query
        /// </method>
        public int executeUpdateQuery(String _query, SqlParameter[] sqlParameter, SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(_query, sqlParameter, sqlTrxn);
            if (sqlTrxn == null)
            {
                using (SqlCommand updateCommand = new SqlCommand())
                {
                    try
                    {
                        updateCommand.Connection = openConnection();
                        if (commandTimeout >= 0)
                        {
                            updateCommand.CommandTimeout = commandTimeout;
                        }
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
                        throw new Exception(genericErrorMessage);
                    }
                    finally
                    {
                        updateCommand.Connection.Close();
                    }
                }
            }
            else
            {
                try
                {
                    SqlCommand updateCommand = new SqlCommand();
                    updateCommand.Connection = sqlTrxn.Connection;
                    updateCommand.Transaction = sqlTrxn;
                    if (commandTimeout >= 0)
                    {
                        updateCommand.CommandTimeout = commandTimeout;
                    }
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
                    throw new Exception(genericErrorMessage);
                }
            }
        }

        public DataTable BatchSave(List<SqlDataRecord> sqlDataRecordList,
                                   SqlTransaction sqlTransaction,
                                   string query,
                                   string tableType,
                                   string parameterName)
        {
            log.LogMethodEntry(sqlDataRecordList, sqlTransaction, query, tableType, parameterName);
            SqlConnection sqlConnection = null;
            DataTable dataTable = new DataTable();
            if (sqlDataRecordList == null ||
                sqlDataRecordList.Any() == false)
            {
                throw new ArgumentException("sqlDataRecordList is empty.");
            }
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("query is empty.");
            }
            if (string.IsNullOrWhiteSpace(tableType))
            {
                throw new ArgumentException("tableType is empty.");
            }
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                throw new ArgumentException("parameterName is empty.");
            }
            SqlTransaction trx = sqlTransaction;
            try
            {
                if (sqlTransaction == null)
                {
                    sqlConnection = openConnection();
                    trx = sqlConnection.BeginTransaction();
                }
                int batchSize = 5000;
                int totalNoOfRecords = sqlDataRecordList.Count;
                int noOfBatches = totalNoOfRecords / batchSize;
                if (totalNoOfRecords % batchSize > 0)
                {
                    noOfBatches++;
                }
                for (int i = 0; i < noOfBatches; i++)
                {
                    int index = i * batchSize;
                    int count = batchSize;
                    if (index + count > totalNoOfRecords)
                    {
                        count = totalNoOfRecords - index;
                    }
                    if (count <= 0)
                    {
                        continue;
                    }
                    List<SqlDataRecord> subset = sqlDataRecordList.GetRange(index, count);
                    SqlParameter parameter = new SqlParameter(parameterName, SqlDbType.Structured);
                    parameter.TypeName = tableType;
                    parameter.Value = subset;
                    DataTable subsetDataTable = executeSelectQuery(query, new SqlParameter[] { parameter }, trx);
                    dataTable.Merge(subsetDataTable);
                }
                if (sqlTransaction == null)
                {
                    trx.Commit();
                }
            }
            finally
            {
                if (sqlTransaction == null)
                {
                    trx.Dispose();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        public SqlParameter GetListSqlParameter<T>(string parameterName, List<T> list)
        {
            log.LogMethodEntry(parameterName, list);
            if (list == null ||
               list.Any() == false)
            {
                log.LogMethodExit(null, "Throwing ArgumentException - List is empty");
                throw new ArgumentException("integerList is empty");
            }
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                log.LogMethodExit(null, "Throwing ArgumentException - parameterName is empty");
                throw new ArgumentException("parameterName is empty");
            }
            SqlParameter sqlParameter = new SqlParameter(parameterName, SqlDbType.Structured);
            if(typeof(T) == typeof(int))
            {
                sqlParameter.TypeName = "IntegerType";
            }
            else if(typeof(T) == typeof(string))
            {
                sqlParameter.TypeName = "StringType";
            }
            else
            {
                throw new ArgumentException("Invalid data type. Only Integer/String type are supported");
            }
            sqlParameter.Value = GetSqlDataRecords(list);
            log.LogMethodExit(sqlParameter);
            return sqlParameter;
        }

        private IEnumerable<SqlDataRecord> GetSqlDataRecords<T>(List<T> list)
        {
            SqlMetaData[] columnStructures = new SqlMetaData[1];
            if (typeof(T) == typeof(int))
            {
                columnStructures[0] = new SqlMetaData("Id", SqlDbType.Int);
            }
            else if (typeof(T) == typeof(string))
            {
                columnStructures[0] = new SqlMetaData("Value", SqlDbType.NVarChar, SqlMetaData.Max);
            }
            else
            {
                throw new ArgumentException("Invalid data type. Only Integer/String type are supported");
            }
            for (int i = 0; i < list.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, GetParameterValue(list[i]));
                yield return dataRecord;
            }
        }

        public DataTable BatchSelect<T>(string _query,
                                     string listParameterName,
                                     List<T> list,
                                     SqlParameter[] sqlParameter = null,
                                     SqlTransaction sqlTrxn = null)
        {
            log.LogMethodEntry(_query, listParameterName, list, sqlParameter, sqlTrxn);
            DataTable result = new DataTable();
            int batchSize = 5000;
            int totalNoOfRecords = list.Count;
            int noOfBatches = totalNoOfRecords / batchSize;
            if (totalNoOfRecords % batchSize > 0)
            {
                noOfBatches++;
            }
            for (int i = 0; i < noOfBatches; i++)
            {
                int index = i * batchSize;
                int count = batchSize;
                if (index + count > totalNoOfRecords)
                {
                    count = totalNoOfRecords - index;
                }
                if (count <= 0)
                {
                    continue;
                }
                List<T> subset = list.GetRange(index, count);
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(GetListSqlParameter(listParameterName, subset));
                if (sqlParameter != null &&
                    sqlParameter.Any())
                {
                    foreach (SqlParameter spp in sqlParameter)
                    {
                        SqlParameter nameParam = new SqlParameter(spp.ParameterName, spp.SqlValue);
                        parameterList.Add(nameParam);
                    }
                }
                DataTable subsetDataTable = executeSelectQuery(_query, parameterList.ToArray(), sqlTrxn);
                result.Merge(subsetDataTable);
            }
            return result;
        }

        public string GetParameterName(Enum key)
        {
            return "@" + key.ToString();
        }

        private List<string> GetInClauseParameterNameList(Enum key, string commaSeperatedValues)
        {
            log.LogMethodEntry(key, commaSeperatedValues);
            List<string> result = new List<string>();
            string[] values = commaSeperatedValues.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < values.Length; i++)
            {
                result.Add("@" + key.ToString() + i);
            }
            log.LogMethodExit(result);
            return result;
        }

        public string GetInClauseParameterName(Enum key, string commaSeperatedValues)
        {
            log.LogMethodEntry(key, commaSeperatedValues);
            List<string> parameterNameList = GetInClauseParameterNameList(key, commaSeperatedValues);
            string result = string.Join(",", parameterNameList);
            log.LogMethodExit(result);
            return result;
        }

        public List<SqlParameter> GetSqlParametersForInClause(Enum key, string commaSeperatedValues)
        {
            string[] values = commaSeperatedValues.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> parameterNameList = GetInClauseParameterNameList(key, commaSeperatedValues);
            List<SqlParameter> result = new List<SqlParameter>();
            for (int i = 0; i < parameterNameList.Count; i++)
            {
                result.Add(new SqlParameter(parameterNameList[i], values[i]));
            }
            log.LogMethodExit(result);
            return result;
        }

        public List<SqlParameter> GetSqlParametersForInClause<T>(Enum key, string commaSeperatedValues) where T : struct
        {
            string[] values = commaSeperatedValues.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> parameterNameList = GetInClauseParameterNameList(key, commaSeperatedValues);
            List<SqlParameter> result = new List<SqlParameter>();
            for (int i = 0; i < parameterNameList.Count; i++)
            {
                result.Add(new SqlParameter(parameterNameList[i], (T)ConvertStringToType<T>(values[i])));
            }
            log.LogMethodExit(result);
            return result;
        }

        private object ConvertStringToType<T>(string value) where T : struct
        {
            log.LogMethodEntry(value);
            object convertedValue = null;
            if (typeof(T) == typeof(Color))
            {
                convertedValue = ColorTranslator.FromHtml(value); ;
            }
            else if (typeof(T) == typeof(bool))
            {
                if (value == "Y" || value == "1")
                {
                    convertedValue = true;
                }
                else
                {
                    convertedValue = false;
                }
            }
            else
            {
                convertedValue = Convert.ChangeType(value, typeof(T));
            }
            log.LogMethodExit(convertedValue);
            return convertedValue;
        }
        public string GetInClauseParameterNameWithSHA2256Command(Enum key, string commaSeperatedValues)
        {
            log.LogMethodEntry(key, commaSeperatedValues);
            List<string> parameterNameList = GetInClauseParameterNameListSHA2256Command(key, commaSeperatedValues);
            string result = string.Join(",", parameterNameList);
            log.LogMethodExit(result);
            return result;
        }
        private List<string> GetInClauseParameterNameListSHA2256Command(Enum key, string commaSeperatedValues)
        {
            log.LogMethodEntry(key, commaSeperatedValues);
            List<string> result = new List<string>();
            string[] values = commaSeperatedValues.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < values.Length; i++)
            {
                result.Add(" hashbytes('SHA2_256',convert(nvarchar(max), upper(@" + key.ToString() + i + "))) ");

            }
            log.LogMethodExit(result);
            return result;
        }
        public object GetHashedValue(string stringValue, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(stringValue, sqlTrx);
            object returnValue = string.Empty;
            if (string.IsNullOrWhiteSpace(stringValue) == false)
            {
                string qry = "SELECT hashbytes('SHA2_256',convert(nvarchar(max), upper(@stringValue)))  as hashValue ";
                SqlParameter sqlParameter = new SqlParameter("@stringValue", stringValue);
                DataTable dt = executeSelectQuery(qry, new SqlParameter[] { sqlParameter }, sqlTrx);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["hashValue"] != DBNull.Value)
                {
                    returnValue = dt.Rows[0]["hashValue"];
                }
            }
            log.LogMethodExit(stringValue);
            return stringValue;
        }
        public string GetParameterNameWithSHA256HashByteCommand(Enum key)
        {
            log.LogMethodEntry(key);
            string returnValue = string.Empty;
            if (key != null)
            {
                returnValue = " hashbytes('SHA2_256',convert(nvarchar(max), upper(@"+ key.ToString() + "))) ";
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public string GetDateTimeParameterNameWithSHA256HashByteCommand(Enum key)
        {
            log.LogMethodEntry(key);
            string returnValue = string.Empty;
            if (key != null)
            {
                returnValue = " hashbytes('SHA2_256', upper(Convert(nvarchar(100), convert(datetime, @"+ key.ToString() + " ,120), 120))) ";
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// BatchUpdate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_query"></param>
        /// <param name="listParameterName"></param>
        /// <param name="list"></param>
        /// <param name="sqlParameter"></param>
        /// <param name="sqlTrxn"></param>
        /// <returns></returns>
        public int BatchUpdate<T>(string _query,
                                     string listParameterName,
                                     List<T> list,
                                     SqlParameter[] sqlParameter = null,
                                     SqlTransaction sqlTrxn = null)
        {
            log.LogMethodEntry(_query, listParameterName, list, sqlParameter, sqlTrxn);
            int resultCount = 0;
            int batchSize = 5000;
            int totalNoOfRecords = list.Count;
            int noOfBatches = totalNoOfRecords / batchSize;
            if (totalNoOfRecords % batchSize > 0)
            {
                noOfBatches++;
            }
            for (int i = 0; i < noOfBatches; i++)
            {
                int index = i * batchSize;
                int count = batchSize;
                if (index + count > totalNoOfRecords)
                {
                    count = totalNoOfRecords - index;
                }
                if (count <= 0)
                {
                    continue;
                }
                List<T> subset = list.GetRange(index, count);
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(GetListSqlParameter(listParameterName, subset));
                if (sqlParameter != null &&
                    sqlParameter.Any())
                {
                    foreach (SqlParameter spp in sqlParameter)
                    {
                        SqlParameter nameParam = new SqlParameter(spp.ParameterName, spp.SqlValue);
                        parameterList.Add(nameParam);
                    }
                }
                int numberOfRecords = executeUpdateQuery(_query, parameterList.ToArray(), sqlTrxn);
                resultCount = resultCount + numberOfRecords;
            }
            log.LogMethodExit(resultCount);
            return resultCount;
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

        public void CheckAndRemoveBadConnections()
        {
            log.LogMethodEntry();
            for (int i = 0; i < 100; i++) //100 is the default pool size
            {
                SqlConnection connection = openConnection();
                try
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED;"; // Default, or use whatever you think your connections should be at
                        cmd.ExecuteNonQuery();
                    }
                    break;
                }
                catch (SqlException ex)
                {
                    log.Error("Error encountered while trying to clear bad connections");
                    connection.Dispose();
                }
            }
            log.LogMethodExit();
        }
    }
}

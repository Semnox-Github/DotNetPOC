/********************************************************************************************
 * Project Name - DBUtils
 * Description  - DBUtils
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.120.0     09-Oct-2020   Guru S A       Membership engine sql session issue
 *2.130.11    07-Nov-2022   Deeksha        Sales Report changes for AFM report five- timeout issue Fix
 *2.150.2     20-Jun-2023   Nitin Pai      Security Testing Fix: Catch SQL excpeption and throw as generic exception.
 *2.150.2     20-Jun-2023   Nitin Pai      App pool crashing issues - Check connection status before opening
 *****************************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Drawing;
using System.Configuration;
using System.Collections.Concurrent;

namespace Semnox.Core.Utilities
{
    public class DBUtils : IDisposable
    {
        public SqlConnection sqlConnection;
        string _ConnectionString = "";
        private string genericErrorMessage = "SQL exception encountered. Check log files to get more details.";
        private readonly PropertyInfo ConnectionInfo = typeof(SqlConnection).GetProperty("InnerConnection", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<string, string> connectionStringDictionary = new ConcurrentDictionary<string, string>();
        public DBUtils(SqlConnection sqlConnection)
        {
            log.LogMethodEntry(sqlConnection);
            this.sqlConnection = sqlConnection;
            _ConnectionString = sqlConnection.ConnectionString;
            log.LogMethodExit();
        }

        public DBUtils(string sqlConnectionString)
        {
            try
            {
                _ConnectionString = getParafaitConnectionString(sqlConnectionString);
            }
            catch
            {
                _ConnectionString = sqlConnectionString;
            }
            this.sqlConnection = getConnection(sqlConnectionString);
        }

        public DBUtils()
        {
        }

        public SqlConnection getConnection()
        {
            return getConnection(null);
        }

        SqlConnection getConnection(string conString)
        {
            try
            {
                if (sqlConnection == null)
                    sqlConnection = createConnection(conString);

                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                    sqlConnection.Open();
                else if (sqlConnection.State == System.Data.ConnectionState.Broken)
                {
                    sqlConnection.Close();
                    sqlConnection.Open();
                }
            }
            catch (SqlException ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Fatal error inside DataAccessHandler constructor");
                log.Fatal("Fatal error inside DataAccessHandler constructor");
                throw new Exception(genericErrorMessage);
            }
            return sqlConnection;
        }

        public SqlCommand getCommand()
        {
            SqlCommand Command = new SqlCommand();
            Command.Connection = getConnection();
            Command.CommandTimeout = 900;
            return Command;
        }

        public SqlCommand getCommand(SqlTransaction SQLTrx)
        {
            if (SQLTrx == null)
                return getCommand();

            SqlCommand Command = new SqlCommand();
            Command.Connection = SQLTrx.Connection;
            Command.Transaction = SQLTrx;
            Command.CommandTimeout = 900;
            return Command;
        }

        public SqlCommand getCommand(SqlConnection cnn)
        {
            SqlCommand Command = new SqlCommand();
            Command.Connection = cnn;
            Command.CommandTimeout = 900;
            return Command;
        }

        public SqlConnection createConnection()
        {
            return createConnection(null);
        }

        public SqlConnection createConnection(string conString)
        {
            if (string.IsNullOrEmpty(conString))
            {
                if (string.IsNullOrEmpty(_ConnectionString))
                    conString = _ConnectionString = DBUtils.getParafaitConnectionString(getConStringFromConfig());
                else
                    conString = _ConnectionString;
            }
            else
            {
                try
                {
                    conString = DBUtils.getParafaitConnectionString(conString);
                }
                catch (Exception)
                {
                }
                if (string.IsNullOrEmpty(_ConnectionString))
                    _ConnectionString = conString;
            }
            SqlConnection lclConnection = new SqlConnection();
            try
            {
                lclConnection = new System.Data.SqlClient.SqlConnection(conString);
                lclConnection.Open();
            }
            catch (SqlException ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Fatal error inside DataAccessHandler constructor");
                log.Fatal("Fatal error inside DataAccessHandler constructor");
                throw new Exception(genericErrorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lclConnection;
        }

        private SqlTransaction GetTransaction(IDbConnection conn)
        {
            var internalConn = ConnectionInfo.GetValue(conn, null);
            var currentTransactionProperty = internalConn.GetType().GetProperty("CurrentTransaction", BindingFlags.NonPublic | BindingFlags.Instance);
            var currentTransaction = currentTransactionProperty.GetValue(internalConn, null);
            var realTransactionProperty = currentTransaction.GetType().GetProperty("Parent", BindingFlags.NonPublic | BindingFlags.Instance);
            var realTransaction = realTransactionProperty.GetValue(currentTransaction, null);
            return (SqlTransaction)realTransaction;
        }

        public static string getParafaitConnectionString(string ConnectionString)
        {
            log.LogMethodEntry();
            if (string.IsNullOrWhiteSpace(ConnectionString) == false &&
                connectionStringDictionary.ContainsKey(ConnectionString))
            {
                return connectionStringDictionary[ConnectionString];
            }
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);

            builder.Password = Encoding.UTF8.GetString(EncryptionAES.Decrypt(Convert.FromBase64String(builder.Password), getKey(builder.DataSource))).TrimEnd();
            builder.PersistSecurityInfo = true;
            log.LogMethodExit();
            string result = builder.ToString();
            if(string.IsNullOrWhiteSpace(ConnectionString) == false)
            {
                connectionStringDictionary[ConnectionString] = result;
            }
            return result;
        }

        string getConStringFromConfig()
        {
            log.LogMethodEntry();
            string connectionString = null;
            foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
            {
                if (String.Compare(item.Name, "ParafaitUtils.Properties.Settings.ParafaitConnectionString", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    connectionString = item.ConnectionString;
                    break;
                }
            }

            // In case "ParafaitUtils.Properties.Settings.ParafaitConnectionString" entry is not present in the config manager search for "ParafaitConnectionString"
            if (String.IsNullOrEmpty(connectionString))
            {
                foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
                {
                    if (String.Compare(item.Name, "ParafaitConnectionString", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        connectionString = item.ConnectionString;
                        break;
                    }
                }
            }

            if (connectionString == null)
            {
                connectionString = Semnox.Core.Utilities.Properties.Settings.Default.ParafaitConnectionString;

            }
            log.LogMethodExit();
            return connectionString;
        }



        public object executeScalar(string command, SqlTransaction SQLTrx, params SqlParameter[] spc)
        {
            log.LogMethodEntry(command, SQLTrx, spc);
            if (SQLTrx == null)
            {
                using (SqlConnection conn = createConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = command;
                            foreach (SqlParameter sp in spc)
                                cmd.Parameters.Add(sp);
                            object o = cmd.ExecuteScalar();
                            log.LogVariableState("o", o);
                            log.LogMethodExit();
                            return o;
                        }
                        catch (SqlException ex)
                        {
                            log.Error("Error occured while executing the select query", ex);
                            log.LogMethodExit(null);
                            throw new Exception(genericErrorMessage);
                        }
                        finally
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLTrx.Connection;
                    cmd.Transaction = SQLTrx;
                    cmd.CommandText = command;
                    foreach (SqlParameter sp in spc)
                        cmd.Parameters.Add(sp);
                    object o = cmd.ExecuteScalar();
                    cmd = null;
                    log.LogVariableState("o", o);
                    log.LogMethodExit();
                    return o;
                }
                catch (SqlException ex)
                {
                    log.Error("Error occured while executing the select query", ex);
                    log.LogMethodExit(null);
                    throw new Exception(genericErrorMessage);
                }
            }
        }

        public object executeScalar(string command, params SqlParameter[] spc)
        {
            log.LogMethodEntry(command, spc);
            object retValue = executeScalar(command, null, spc);
            log.LogVariableState("retValue", retValue);
            log.LogMethodExit();
            return retValue;
        }

        public int executeNonQuery(string command, SqlTransaction SQLTrx, params SqlParameter[] spc)
        {
            log.LogMethodEntry(command, SQLTrx, spc);
            if (SQLTrx == null)
            {
                using (SqlConnection conn = createConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = command;
                            foreach (SqlParameter sp in spc)
                                cmd.Parameters.Add(sp);
                            int ret = cmd.ExecuteNonQuery();
                            log.LogVariableState("ret", ret);
                            log.LogMethodExit();
                            return (ret);
                        }
                        catch (SqlException ex)
                        {
                            log.Error("Error occured while executing the select query", ex);
                            log.LogMethodExit(null);
                            throw new Exception(genericErrorMessage);
                        }
                        finally
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLTrx.Connection;
                    cmd.Transaction = SQLTrx;
                    cmd.CommandText = command;
                    foreach (SqlParameter sp in spc)
                        cmd.Parameters.Add(sp);
                    int ret = cmd.ExecuteNonQuery();
                    cmd = null;
                    log.LogVariableState("ret", ret);
                    log.LogMethodExit();
                    return (ret);
                }
                catch (SqlException ex)
                {
                    log.Error("Error occured while executing the select query", ex);
                    log.LogMethodExit(null);
                    throw new Exception(genericErrorMessage);
                }

            }
        }

        public int executeNonQuery(string command, params SqlParameter[] spc)
        {
            log.LogMethodEntry(command, spc);
            int retValue = executeNonQuery(command, null, spc);
            log.LogVariableState("retValue", retValue);
            log.LogMethodExit();
            return retValue;
        }



        public DataTable executeDataTable(string command, SqlTransaction SQLTrx, int commandTimeOut, params SqlParameter[] spc)
        {
            log.LogMethodEntry(command, SQLTrx, spc);
            if (SQLTrx == null)
            {
                using (SqlConnection conn = createConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = command;
                            cmd.CommandTimeout = commandTimeOut;
                            foreach (SqlParameter sp in spc)
                                cmd.Parameters.Add(sp);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            log.LogVariableState("dt", dt);
                            log.LogMethodExit();
                            return (dt);
                        }
                        catch (SqlException ex)
                        {
                            log.Error("Error occured while executing the select query", ex);
                            log.LogMethodExit(null);
                            throw new Exception(genericErrorMessage);
                        }
                        finally
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLTrx.Connection;
                    cmd.Transaction = SQLTrx;
                    cmd.CommandText = command;
                    cmd.CommandTimeout = commandTimeOut;
                    foreach (SqlParameter sp in spc)
                        cmd.Parameters.Add(sp);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmd = null;
                    log.LogVariableState("dt", dt);
                    log.LogMethodExit();
                    return (dt);
                }
                catch (SqlException ex)
                {
                    log.Error("Error occured while executing the select query", ex);
                    log.LogMethodExit(null);
                    throw new Exception(genericErrorMessage);
                }
            }
        }

        public DataTable executeDataTable(string command, SqlTransaction SQLTrx, params SqlParameter[] spc)
        {
            log.LogMethodEntry(command, SQLTrx, spc);
            DataTable dt = executeDataTable(command, SQLTrx, 30, spc);
            log.LogVariableState("dt", dt);
            log.LogMethodExit();
            return (dt);
        }

        public DataTable executeDataTable(string command, params SqlParameter[] spc)
        {
            log.LogMethodEntry(command, spc);
            DataTable dt = executeDataTable(command, null, spc);
            log.LogVariableState("dt", dt);
            log.LogMethodExit();
            return dt;
        }


        public static byte[] getKey(string insert)
        {
            log.LogMethodEntry();
            string encryptionKey = "46A97988SEMNOX!1CCCC9D1C581D86EE";
            byte[] key = Encoding.UTF8.GetBytes(encryptionKey);
            byte[] insertBytes = Encoding.UTF8.GetBytes(insert.PadRight(4, 'X').Substring(0, 4));
            key[16] = insertBytes[0];
            key[17] = insertBytes[1];
            key[18] = insertBytes[2];
            key[19] = insertBytes[3];
            log.LogMethodExit();
            return key;
        }

        public byte[] ConvertToByteArray(System.Drawing.Image image)
        {
            log.LogMethodEntry();
            log.LogVariableState("image", image);

            if (image == null)
            {
                log.LogMethodExit("image == null");
                return null;
            }
            else
            {
                ImageConverter converter = new ImageConverter();
                Byte[] imageByteArray = (byte[])converter.ConvertTo(image, typeof(byte[]));
                log.LogVariableState("imageByteArray", imageByteArray);
                log.LogMethodExit();
                return imageByteArray;
            }
        }



        public void Dispose()
        {
            if (sqlConnection != null)
            {
                try
                {
                    sqlConnection.Close();
                }
                catch { }
            }
        }

        ~DBUtils()
        {
            Dispose();
        }


    }
}

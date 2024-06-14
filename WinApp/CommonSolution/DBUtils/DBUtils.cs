using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
//using Semnox.Parafait.EncryptionUtils;
using System.Drawing;
using System.Configuration;
using Semnox.Core.Security;

//namespaParafaitUtils

namespace Semnox.Core.DBUtils
{
    public class DBUtils : IDisposable
    {
        public SqlConnection sqlConnection;
        string _ConnectionString = "";
        private readonly PropertyInfo ConnectionInfo = typeof(SqlConnection).GetProperty("InnerConnection", BindingFlags.NonPublic | BindingFlags.Instance);
        public DBUtils(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
            _ConnectionString = sqlConnection.ConnectionString;

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
            if (sqlConnection == null)
                sqlConnection = createConnection(conString);

            if (sqlConnection.State == System.Data.ConnectionState.Closed)
                sqlConnection.Open();
            else if (sqlConnection.State == System.Data.ConnectionState.Broken)
            {
                sqlConnection.Close();
                sqlConnection.Open();
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
            SqlConnection lclConnection=new SqlConnection();
            try
            { 
              lclConnection = new System.Data.SqlClient.SqlConnection(conString);
            lclConnection.Open();
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
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);

            builder.Password = Encoding.UTF8.GetString(EncryptionAES.Decrypt(Convert.FromBase64String(builder.Password), getKey(builder.DataSource))).TrimEnd();
            builder.PersistSecurityInfo = true;

            return builder.ToString();
        }

        string getConStringFromConfig()
        {
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
                connectionString = Settings.Default.ParafaitConnectionString;
            }
            return connectionString;
        }

      

        public object executeScalar(string command, SqlTransaction SQLTrx, params SqlParameter[] spc)
        {
            SqlCommand cmd = getCommand(SQLTrx);
            if (SQLTrx == null)
                cmd.Connection = createConnection();

            cmd.CommandText = command;
            foreach (SqlParameter sp in spc)
                cmd.Parameters.Add(sp);
            object o = cmd.ExecuteScalar();
            if (SQLTrx == null)
                cmd.Connection.Close();
            cmd.Dispose();
            return o;
        }

        public object executeScalar(string command, params SqlParameter[] spc)
        {
            return executeScalar(command, null, spc);
        }

        public int executeNonQuery(string command, SqlTransaction SQLTrx, params SqlParameter[] spc)
        {
            SqlCommand cmd = getCommand(SQLTrx);
            if (SQLTrx == null)
                cmd.Connection = createConnection();

            cmd.CommandText = command;
            foreach (SqlParameter sp in spc)
                cmd.Parameters.Add(sp);
            int ret = cmd.ExecuteNonQuery();
            if (SQLTrx == null)
                cmd.Connection.Close();
            cmd.Dispose();
            return (ret);
        }

        public int executeNonQuery(string command, params SqlParameter[] spc)
        {
            return executeNonQuery(command, null, spc);
        }

        public DataTable executeDataTable(string command, SqlTransaction SQLTrx, params SqlParameter[] spc)
        {
            SqlCommand cmd = getCommand(SQLTrx);
            if (SQLTrx == null)
                cmd.Connection = createConnection();

            cmd.CommandText = command;
            foreach (SqlParameter sp in spc)
                cmd.Parameters.Add(sp);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (SQLTrx == null)
                cmd.Connection.Close();

            cmd.Dispose();

            return (dt);
        }

        public DataTable executeDataTable(string command, params SqlParameter[] spc)
        {
            return executeDataTable(command, null, spc);
        }

        public static byte[] getKey(string insert)
        {
            string encryptionKey = "46A97988SEMNOX!1CCCC9D1C581D86EE";
            byte[] key = Encoding.UTF8.GetBytes(encryptionKey);
            byte[] insertBytes = Encoding.UTF8.GetBytes(insert.PadRight(4, 'X').Substring(0, 4));
            key[16] = insertBytes[0];
            key[17] = insertBytes[1];
            key[18] = insertBytes[2];
            key[19] = insertBytes[3];

            return key;
        }

        public byte[] ConvertToByteArray(System.Drawing.Image image)
        {
            if (image == null)
                return null;
            else
            {
                ImageConverter converter = new ImageConverter();
                Byte[] imageByteArray = (byte[])converter.ConvertTo(image, typeof(byte[]));
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

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using ParafaitUtils.Authentication;
using Semnox.Parafait.logging;
using Semnox.Core.Utilities;

namespace Semnox.Core.Utilities
{
    public class ParafaitDBTransaction : IDisposable
    {
        private SqlConnection conn;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private string errorMessage;
        public SqlTransaction SQLTrx { get; set; }

        public ParafaitDBTransaction()
        {
            try
            {
                log.Debug("Entering ParafaitDBTransaction constructor");
                string connString = "";
                try
                {
                    connString = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
                }
                catch { }
                if (string.IsNullOrEmpty(connString))
                    connString = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ConnectionString;

                string connectionString = StaticUtils.getParafaitConnectionString(connString);
                conn = new SqlConnection(connectionString);
                log.Debug("Exiting ParafaitDBTransaction constructor after getting sqlConnection");
            }
            catch (SqlException)
            {
                log.Fatal("Fatal error inside ParafaitDBTransaction constructor");
                throw;
            }
        }

        public ParafaitDBTransaction(string connString)
        {
            log.LogMethodEntry(connString);
            try
            {
                log.Debug("Entering ParafaitDBTransaction constructor with parameter");
                if (string.IsNullOrEmpty(connString))
                {
                    log.Fatal("Connection string is not specified");
                    throw new Exception("Connection string is not specified");
                }
                else
                {
                    string connectionString = StaticUtils.getParafaitConnectionString(connString);
                    conn = new SqlConnection(connectionString);
                    log.Debug("Exiting ParafaitDBTransaction constructor after getting sqlConnection");
                }
            }
            catch (SqlException)
            {
                log.Fatal("Fatal error inside ParafaitDBTransaction constructor");
                throw;
            }
            log.LogMethodExit();
        }


        public void BeginTransaction()
        {
            log.Debug("Entering openConnection method");
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
            {
                conn.Open();
            }
            log.Debug("Exiting openConnection method");
            SQLTrx = conn.BeginTransaction();
        }

        public SqlConnection GetConnection()
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
            {
                conn.Open();
            }
            return conn;
        }

        public bool EndTransaction()
        {
            try
            {
                SQLTrx.Commit();
                Dispose();
            }
            catch (Exception dbEx)
            {
                errorMessage = "Fatal error inside ParafaitDBTransaction - EndTransaction";
                log.Fatal(errorMessage);
                throw new Exception(errorMessage, dbEx);
            }
            return true;
        }

        public void RollBack()
        {
            try
            {
                SQLTrx.Rollback();
                Dispose();
            }
            catch (Exception dbEx)
            {
                errorMessage = "Fatal error inside ParafaitDBTransaction - EndTransaction";
                log.Fatal(errorMessage);
                throw new Exception(errorMessage, dbEx);
            }
        }

        public void Dispose()
        {
            if (conn != null)
            {
                try
                {
                    conn.Close();
                    log.Debug("Connection Closed- ParafaitDBTransaction");
                }
                catch { }
            }

            if (SQLTrx != null)
            SQLTrx.Dispose();

            log.Debug("Disposed Connection - ParafaitDBTransaction");

        }

    }


}

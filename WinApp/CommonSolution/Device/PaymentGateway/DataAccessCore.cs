using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Configuration;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class DataAccessCore
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataSet ExecuteSP(string SPName, List<SqlParameter> parameters)
        {
            log.LogMethodEntry(SPName, parameters);

            try
            {
                DataSet myDS = new DataSet();
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.Parameters.AddRange(parameters.ToArray());
                myDS = executesp(SPName, sqlcommand);

                log.LogMethodExit(myDS);
                return myDS;
            }
            catch (Exception ex)
            {
                log.Error("Error occure while executing the stored procedure", ex);
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw ex;
            }
        }

        private DataSet executesp(string SPName, SqlCommand sqlcommand)
        {
            log.LogMethodEntry(SPName, sqlcommand);

            DataSet myDS = new DataSet();
            SqlConnection sqlconn = WSCore._utilities.createConnection();
            try
            {
                sqlcommand.Connection = sqlconn;
                sqlcommand.CommandText = SPName;
                sqlcommand.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                adapter.Fill(myDS);
                WSCore._utilities.logSQLCommand("MercuryGateway", sqlcommand);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while executing the stored procedure", ex);
                log.LogMethodExit(null, "Throwing Exception");
                throw ex;
            }
            finally
            {
                sqlconn.Close();
            }

            log.LogMethodExit(myDS);
            return myDS;
        }

        public object sqlExecuteNonQueryReturnVal(string SPName, List<SqlParameter> parameters, bool RetResultrequired = false)
        {
            log.LogMethodEntry(SPName, parameters, RetResultrequired);

            SqlConnection sqlconn = WSCore._utilities.createConnection();
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.Connection = sqlconn;
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = SPName;
                sqlcommand.Parameters.AddRange(parameters.ToArray());
                if (RetResultrequired == true) sqlcommand.Parameters["@Result"].Direction = ParameterDirection.Output;
                sqlcommand.ExecuteNonQuery();
                WSCore._utilities.logSQLCommand("MercuryGateway", sqlcommand);
                sqlcommand.Dispose();

                object returnValueNew = sqlcommand.Parameters["@Result"].Value;
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while executing the non query", ex);
                log.LogMethodExit(null, "Throwing Exception");
                throw ex;
            }
            finally
            {
                sqlconn.Close();
            }
        }

        public void sqlExecuteNonQuery(string sql, List<SqlParameter> parameters)
        {
            log.LogMethodEntry(sql, parameters);

            SqlConnection sqlconn = WSCore._utilities.createConnection();
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.Connection = sqlconn;
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.CommandText = sql;
                sqlcommand.Parameters.AddRange(parameters.ToArray());
                sqlcommand.ExecuteNonQuery();
                WSCore._utilities.logSQLCommand("MercuryGateway", sqlcommand);
                sqlcommand.Dispose();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while executing the non query", ex);
                log.LogMethodExit(null, "Throwing Exception" + ex);
                throw ex;
            }
            finally
            {
                sqlconn.Close();
            }

            log.LogMethodExit(null);
        }
    }
}

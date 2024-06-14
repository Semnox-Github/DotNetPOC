/********************************************************************************************
 * Project Name - Printer 
 * Description  - Query Management.
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        18-Jul-2019      Deeksha        Modified:Added logger methods.
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Printer
{
    public class QueryManagement
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Utilities _Utilites;
        public QueryManagement (Utilities Utilities)
        {
            log.LogMethodEntry(Utilities);
            _Utilites = Utilities;
            log.LogMethodExit();
        }

        public string getQueryString(string reportKey)
        {
            log.LogMethodEntry("reportKey");
            string queryString = "";

            object objquery = _Utilites.executeScalar(@"SELECT DBQuery 
                                                          FROM Reports
                                                         WHERE report_key = @reportKey", 
                                                      new SqlParameter("@reportKey", reportKey));
            if (objquery != null && objquery != DBNull.Value)
                queryString = objquery.ToString();
            log.LogMethodExit(queryString);
            return queryString;
        }

        public string getResultString(string queryString, params SqlParameter[] sqlQueryParameter)
        {
            log.LogMethodEntry(queryString, sqlQueryParameter);
            string resultString = "";

            DataTable dtQueryResult = _Utilites.executeDataTable(queryString, sqlQueryParameter);

            foreach (DataRow dr in dtQueryResult.Rows)
            {
                resultString += Environment.NewLine + dr[0].ToString();
            }
            log.LogMethodExit(resultString);
            return resultString;
        }

        public DataTable getDTResultString(string queryString, params SqlParameter[] sqlQueryParameter)
        {
            log.LogMethodEntry(queryString, sqlQueryParameter);
            DataTable dtResult;
            dtResult = _Utilites.executeDataTable(queryString, sqlQueryParameter);
            log.LogMethodExit(dtResult);
            return dtResult;
        }
    }
}

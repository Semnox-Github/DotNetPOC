/********************************************************************************************
 * Project Name - Job Utils
 * Description  - Visiting details Query
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// the class implements Iquery interface, this will be handle all Visiting details Template related queries
    /// </summary>
    public class VisitingDetailsQuery : IQuery
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Property to hold filename
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Property to hold from date
        /// </summary>
        public DateTime FromDate { get; set; }
        /// <summary>
        /// Property to hold To date
        /// </summary>
        public DateTime ToDate { get; set; }

        Utilities Utilities;

        /// <summary>
        /// Parameterised constructor to set class properties
        /// </summary>
        /// <param name="_Utilities">parafait utility object</param>
        /// <param name="fromDate">from when data should be select in query</param>
        /// <param name="toDate">till when data should be select in query</param>
        public VisitingDetailsQuery(Utilities _Utilities, DateTime fromDate, DateTime toDate)
        {
            log.LogMethodEntry(_Utilities, fromDate, toDate);
            Utilities = _Utilities;
            FromDate = fromDate;
            ToDate = toDate;
            FileName = ConfigurationManager.AppSettings["VisitingDetailsFileName"];
            log.LogMethodExit();
        }

        /// <summary>
        /// To get visiting details for the day
        /// </summary>
        /// <returns>Execute Query and load into datatable</returns>
        public DataTable GetQueryResult()
        {
            log.LogMethodEntry();
            DataTable dt = Utilities.executeDataTable("exec usp_VisitingDetails @FromDate, @ToDate",
                                                        new SqlParameter("@FromDate", FromDate),
                                                        new SqlParameter("@ToDate", ToDate));
            log.LogMethodExit(dt);
            return dt;
        }
    }
}

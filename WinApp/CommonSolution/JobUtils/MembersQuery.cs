/********************************************************************************************
 * Project Name - Job Utils
 * Description  - Member Query
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// the class implements Iquery interface, this will be handle all Members Template File related queries
    /// </summary>
    public class MembersQuery : IQuery
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
        public MembersQuery(Utilities _Utilities, DateTime fromDate, DateTime toDate)
        {
            log.LogMethodEntry(_Utilities, fromDate, toDate);
            Utilities = _Utilities;
            FromDate = fromDate;
            ToDate = toDate;
            FileName = ConfigurationManager.AppSettings["MembersFileName"];
            log.LogMethodExit();
        }

        /// <summary>
        /// To get Members list for the date specified
        /// </summary>
        /// <returns>Execute Query and load into datatable</returns>
        public DataTable GetQueryResult()
        {
            log.LogMethodEntry();
            DataTable dt = Utilities.executeDataTable("exec usp_Members @FromDate, @ToDate",
                                                        new SqlParameter("@FromDate", FromDate),
                                                        new SqlParameter("@ToDate", ToDate));

            log.LogMethodExit(dt);
            return dt;
        }
    }
}

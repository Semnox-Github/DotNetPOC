/********************************************************************************************
 * Project Name - Job Utils
 * Description  - Order Line Query
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
    /// the class implements Iquery interface, this will be handle all OrderLine related queries
    /// </summary>
    public class OrderLineQuery : IQuery
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// private property to hold filename
        /// </summary>
        private string fileName;
        /// <summary>
        /// Property to hold from date
        /// </summary>
        public DateTime FromDate { get; set; }
        /// <summary>
        /// Property to hold To date
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Interface member to hold filename
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                value = fileName;
            }
        }

        Utilities Utilities;

        /// <summary>
        /// Parameterised constructor to set class properties
        /// </summary>
        /// <param name="_Utilities">parafait utility object</param>
        /// <param name="fromDate">from when data should be select in query</param>
        /// <param name="toDate">till when data should be select in query</param>
        public OrderLineQuery(Utilities _Utilities, DateTime fromDate, DateTime toDate)
        {
            log.LogMethodEntry(_Utilities, fromDate, toDate);
            Utilities = _Utilities;
            FromDate = fromDate;
            ToDate = toDate;
            this.fileName = ConfigurationManager.AppSettings["OrderLineFileName"].ToString();
            log.LogMethodExit();
        }

        /// <summary>
        /// To get OrderLine query result
        /// </summary>
        /// <returns>Execute Query and load into datatable</returns>
        public DataTable GetQueryResult()
        {
            log.LogMethodEntry();
            DataTable dt = Utilities.executeDataTable("exec usp_GetOrderLine @FromDate, @ToDate",
                                                        new SqlParameter("@FromDate", FromDate),
                                                        new SqlParameter("@ToDate", ToDate));
            log.LogMethodExit(dt);
            return dt;
        }
    }
}

/********************************************************************************************
 * Project Name - Transaction
 * Description  - Summary Data transfer object of DB Synch Log
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.150.0     16-Dec-2021      Lakshminarayana           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DBSynch
{
    /// <summary>
    /// Summary Data transfer object of DB Synch Log
    /// </summary>
    public class DBSynchLogSummaryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by TABLE_NAME field
            /// </summary>
            TABLE_NAME,
            /// <summary>
            /// Search by IS_PROCESSED field
            /// </summary>
            IS_PROCESSED,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DBSynchLogSummaryDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DBSynchLogSummaryDTO(string tableName, int siteId, int count)
        {
            log.LogMethodEntry(tableName, siteId, count);
            TableName = tableName;
            SiteId = siteId;
            Count = count;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of TableName
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Get/Set method of SiteId
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Get/Set method of Count
        /// </summary>
        public int Count { get; set; }
    }
}

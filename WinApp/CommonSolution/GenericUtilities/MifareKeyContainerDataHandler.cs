/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Data access handler class for the mifare key container
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         17-Nov-2020       Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/using System;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    internal class MifareKeyContainerDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        /// <summary>
        /// Parameterized constructor of MifareKeyContainer DataHandler class
        /// </summary>
        public MifareKeyContainerDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        public DateTime? GetMifareKeyModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate
                             FROM (
                             select MAX(LastUpdateDate) LastUpdatedDate from site WHERE (site_id = @siteId or @siteId = -1)
                             union all
                             select MAX(LastUpdateDate) LastUpdatedDate from ProductKey WHERE (site_id = @siteId or @siteId = -1)
                             union all
                             select MAX(LastUpdateDate) LastUpdatedDate from SystemOptions where OptionName IN ('NonMifareAuthorization', 'MifareAuthorization', 'CustomerMifareUltraLightCKey','DefaultMifareUltraLightCKeys') and (site_id = @siteId or @siteId = -1)
                             union all
                             select MAX(LastUpdatedDate) LastUpdatedDate from parafait_defaults where default_value_name = 'MIFARE_CARD' and (site_id = @siteId or @siteId = -1)) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}

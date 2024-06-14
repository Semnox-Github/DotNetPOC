/********************************************************************************************
 * Project Name - MonitorView Datahandler Programs
 * Description  - Data object of MonitorView Datahandler  
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *2.150.0     11-Jul-2022   Prajwal S       Created.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.logger
{
    /// <summary>
    ///  MonitorViewDatahandler Class
    /// </summary>
    public class MonitorViewDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT mv.MonitorId,  mv.MonitorName, mv.ModuleName, mv.ApplicationName, mv.MonitorType, mv.AssetName, mv.AssetHostname, mv.AssetType, mv.Priority, mv.site_id, ma.IPAddress, ma.MacAddress FROM MonitorView mv, MonitorAsset ma
                                             Where mv.AssetHostname = ma.Hostname
                                             AND mv.AssetName = ma.Name ";

        /// <summary>
        /// Parameterized constructor of MonitorViewDatahandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public MonitorViewDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the MonitorViewDTO  matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of  MonitorViewDTO matching the search criteria</returns>
        public List<MonitorViewDTO> GetMonitorViewList(int siteId)
        {
            log.LogMethodEntry();
            List<MonitorViewDTO> monitorViewDTOList = new List<MonitorViewDTO>();

            SqlParameter[] selectMonitorViewParameters = new SqlParameter[1];
            string selectMonitorViewsQuery = SELECT_QUERY;
            selectMonitorViewsQuery = selectMonitorViewsQuery + "and (mv.site_id = @siteId or @siteId = -1)";
            selectMonitorViewParameters[0] = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectMonitorViewsQuery, selectMonitorViewParameters, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow paymentRow in dataTable.Rows)
                {
                    MonitorViewDTO monitorViewDTO = GetMonitorViewDTO(paymentRow);
                    monitorViewDTOList.Add(monitorViewDTO);
                }
            }
            log.LogMethodExit();
            return monitorViewDTOList;
        }

        /// <summary>
        /// return the record from the database
        /// Convert the datarow to MonitorViewDTO object
        /// </summary>
        /// <returns>return the MonitorViewDTO object</returns>
        private MonitorViewDTO GetMonitorViewDTO(DataRow paymentDataRow)
        {
            log.LogMethodEntry(paymentDataRow);
            MonitorViewDTO monitorViewDTO = new MonitorViewDTO(
                                                    paymentDataRow["MonitorId"] == DBNull.Value ? -1 : Convert.ToInt32(paymentDataRow["MonitorId"]),
                                                    paymentDataRow["MonitorName"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["MonitorName"]),
                                                    paymentDataRow["ApplicationName"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["ApplicationName"]),
                                                    paymentDataRow["ModuleName"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["ModuleName"]),
                                                    paymentDataRow["MonitorType"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["MonitorType"]),
                                                    paymentDataRow["AssetName"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["AssetName"]),
                                                    paymentDataRow["AssetHostname"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["AssetHostname"]),
                                                    paymentDataRow["AssetType"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["AssetType"]),
                                                    paymentDataRow["Priority"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["Priority"]),
                                                    paymentDataRow["MacAddress"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["MacAddress"]),
                                                    paymentDataRow["IPAddress"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["IPAddress"]),
                                                    paymentDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(paymentDataRow["site_id"])

                                                 );
            log.LogMethodExit(monitorViewDTO);
            return monitorViewDTO;

        }


        /// <summary>
        /// Gets the latest update time
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        internal DateTime? GetMonitorViewModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdateDate) LastUpdateDate 
                            FROM (
                            select max(LastUpdatedDate) LastUpdateDate from Monitor WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdateDate from MonitorAsset WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdateDate from MonitorApplication WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdateDate from MonitorAppModule WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdateDate from MonitorPriority WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdateDate from MonitorType WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastupdateDate from MonitorAssetType WHERE  (site_id = @siteId or @siteId = -1)
                            ) a";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@siteId", siteId));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdateDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdateDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}

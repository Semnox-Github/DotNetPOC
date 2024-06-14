using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DashBoard
{
    public class MonitorDashBoardDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of MonitorDashBoardDataHandler class
        /// </summary>
        public MonitorDashBoardDataHandler()
        {
            log.Debug("Starts-MonitorDashBoardDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-MonitorDashBoardDataHandler() default constructor.");
        }



        /// <summary>
        /// Gets the MonitorAssetSummaryReportDTO list based on the parameters
        /// </summary>
        /// <param name="blnShowErrorSites">bool type parameter</param>
        /// <param name="strPriority">string type parameter</param>
        /// <returns>Returns the list of MonitorAssetLogReportDTO </returns>
        public List<MonitorAssetSummaryReportDTO> GetMonitorSiteAssetSummaryList(bool showErrorSites, string priority)
        {
            log.Debug("Starts-GetMonitorSiteAssetSummaryList() Method.");

            string strShowSitesFilter = string.Empty;

            if (showErrorSites == true)
            {
                strShowSitesFilter = " having sum(case when mev.MonitorId  is not null and (mev.Status in ('WARNING','ERROR') or mev.status is null) then 1 else 0 end) >0 "; 
            }

            if (priority == null || priority == "")
            {
                priority = "-1";
            }

            string selectMonitorSummaryQuery = @"select s.site_id,s.SiteCode,s.site_name,
                                                        count(msv.Monitorid) TotalAssets,
                                                        sum(case when mev.MonitorId is not null and (mev.Status in ('WARNING','ERROR') or mev.status is null) then 1 else 0 end) ErrorAssets
                                                from MonitorStatusView msv
                                                left outer join MonitorErrorView mev  on msv.MonitorId = mev.MonitorId
                                                inner join site s on s.site_id = msv.site_id  
                                                where s.SiteCode is not null 
                                                and notes is null
                                                and (msv.Priority = @Priority or @Priority='-1')
                                                group by s.site_id,s.SiteCode,s.site_name " + strShowSitesFilter + " order by s.site_id desc" ;

            List<SqlParameter> monitorRepParameters = new List<SqlParameter>();
            monitorRepParameters.Add(new SqlParameter("@Priority", priority));

            DataTable monitorRepData = dataAccessHandler.executeSelectQuery(selectMonitorSummaryQuery, monitorRepParameters.ToArray());

            if (monitorRepData.Rows.Count > 0)
            {
                List<MonitorAssetSummaryReportDTO> monitorAssetSummaryReportDTOList = new List<MonitorAssetSummaryReportDTO>();
                foreach (DataRow monitorRepDataRow in monitorRepData.Rows)
                {

                    monitorAssetSummaryReportDTOList.Add(new MonitorAssetSummaryReportDTO(
                                                            Convert.ToInt32(monitorRepDataRow["site_id"]),
                                                            Convert.ToInt32(monitorRepDataRow["siteCode"]),
                                                            monitorRepDataRow["site_name"].ToString(),
                                                            Convert.ToInt32(monitorRepDataRow["TotalAssets"]),
                                                            Convert.ToInt32(monitorRepDataRow["ErrorAssets"]))
                                                     );

                }
                log.Debug("Ends-GetMonitorSiteAssetSummaryList() Method by returning monitorAssetSummaryReportDTOList.");
                return monitorAssetSummaryReportDTOList;
            }
            else
            {
                log.Debug("Ends-GetMonitorSiteAssetSummaryList() Method by returning null.");
                return null;
            }
        }

        /// <summary>
        /// Gets the MonitorAssetsReportDTO list based on the parameters
        /// </summary>
        /// <param name="siteId">integer type parameter</param>
        /// <param name="priority">string type parameter</param>
        /// <param name="showErrorSites">bool type parameter</param>
        /// <returns>Returns the list of MonitorAssetsReportDTO </returns>
        public List<MonitorAssetsReportDTO> GetMonitorAssetsList(int siteId, string priority, bool showErrorSites)
        {
            log.Debug("Starts-GetMonitorAssetsList() Method.");

            string strPriorityFilter = string.Empty;
            string strShowSitesFilter = string.Empty;
            StringBuilder stbFilter = new StringBuilder();

            if (siteId <= 0)
            {
                siteId = -1;
            }

            if (priority == null || priority == "")
            {
                priority = "-1";
            }

            if (showErrorSites == true)
            {
               stbFilter.Append(@" and ( case when mev.MonitorId is not null and (mev.Status in ('WARNING','ERROR') or mev.status is null) 
                                                            then isnull(mev.Status,'No Data') 
                                                            else isnull(msv.Status,'No Data') end  IN ('WARNING','ERROR','No Data')) ");
            }

            string selectMonitorRepQuery = @"select msv.MonitorId, s.site_id,s.SiteCode,s.site_name,
                                                        msv.AssetName,msv.AssetHostname,msv.AssetType,
                                                        case when mev.MonitorId is not null and (mev.Status in ('WARNING','ERROR') or mev.status is null) 
                                                            then isnull(mev.Status,'No Data') 
                                                            else isnull(msv.Status,'No Data') end as Status,
                                                        msv.Priority,msv.Timestamp,
                                                        msv.MonitorName,msv.ApplicationName,
                                                        msv.ModuleName,msv.MonitorType,
		                                                msv.LogText,msv.LogKey,msv.LogValue
		                                    from MonitorStatusView msv
                                            left outer join MonitorErrorView mev  on msv.MonitorId = mev.MonitorId
                                            inner join site s on s.site_id = msv.site_id 
                                            where s.siteCode is not null and (msv.site_id=@SiteId or @SiteId=-1)
                                            and s.notes is null 
                                            and (msv.Priority = @Priority or @Priority='-1') " + stbFilter.ToString() +
                                            " order BY s.site_id DESC,msv.MonitorName,msv.ApplicationName,msv.ModuleName ";

            List<SqlParameter> monitorRepParameters = new List<SqlParameter>();
            monitorRepParameters.Add(new SqlParameter("@SiteId", siteId));
            monitorRepParameters.Add(new SqlParameter("@Priority", priority));

            DataTable monitorRepData = dataAccessHandler.executeSelectQuery(selectMonitorRepQuery, monitorRepParameters.ToArray());

            if (monitorRepData.Rows.Count > 0)
            {
                List<MonitorAssetsReportDTO> monitorAssetsReportDTOList = new List<MonitorAssetsReportDTO>();
                foreach (DataRow monitorRepDataRow in monitorRepData.Rows)
                {
                    monitorAssetsReportDTOList.Add(new MonitorAssetsReportDTO(
                                                            Convert.ToInt32(monitorRepDataRow["MonitorId"]),
                                                            Convert.ToInt32(monitorRepDataRow["site_id"]),
                                                            Convert.ToInt32(monitorRepDataRow["siteCode"]),
                                                            monitorRepDataRow["site_name"].ToString(),
                                                            monitorRepDataRow["assetName"].ToString(),
                                                            monitorRepDataRow["assetHostname"].ToString(),
                                                            monitorRepDataRow["assetType"].ToString(),
                                                            monitorRepDataRow["status"].ToString(),
                                                            monitorRepDataRow["priority"].ToString(),
                                                            monitorRepDataRow["monitorName"].ToString(),    
                                                            monitorRepDataRow["applicationName"].ToString(),
                                                            monitorRepDataRow["moduleName"].ToString(),
                                                            monitorRepDataRow["monitorType"].ToString(),
                                                            monitorRepDataRow["logText"].ToString(),
                                                            monitorRepDataRow["logKey"].ToString(),
                                                            monitorRepDataRow["logValue"].ToString(),
                                                            monitorRepDataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(monitorRepDataRow["Timestamp"]))
                                                     );

                }
                log.Debug("Ends-GetMonitorAssetsList() Method by returning monitorAssetSummaryReportDTOList.");
                return monitorAssetsReportDTOList;
            }
            else
            {
                log.Debug("Ends-GetMonitorAssetsList() Method by returning null.");
                return null;
            }
        }


        /// <summary>
        /// Gets the MonitorAssetLogReportDTO list based on the parameters
        /// </summary>
        /// <param name="intMonitorId">integer type parameter</param>
        /// <param name="intMaxRows">integer type parameter</param>
        /// <returns>Returns the list of MonitorAssetLogReportDTO </returns>
        public List<MonitorAssetLogReportDTO> GetMonitorAssetLogList(int monitorId, int maxRows)
        {
            log.Debug("Starts-GetMonitorAssetLogList() Method.");

            string selectMonitorLogQuery = @"SELECT top (@maxRows) Timestamp,Status,Priority,
		                                                    LogText,LogKey,LogValue
		                                    FROM MonitorLogView
                                            WHERE MonitorId=@monitorId  
                                            and Timestamp >= (select max(CAST(Timestamp AS DATE)) 
					                                            FROM MonitorLogView
					                                            WHERE MonitorId=@monitorId) 
                                            ORDER BY Timestamp DESC,MonitorName,ApplicationName,ModuleName ";

            List<SqlParameter> monitorLogParameters = new List<SqlParameter>();
            monitorLogParameters.Add(new SqlParameter("@monitorId", monitorId));
            monitorLogParameters.Add(new SqlParameter("@maxRows", maxRows));

            DataTable monitorLogData = dataAccessHandler.executeSelectQuery(selectMonitorLogQuery, monitorLogParameters.ToArray());

            if (monitorLogData.Rows.Count > 0)
            {
                List<MonitorAssetLogReportDTO> monitorAssetLogReportDTOList = new List<MonitorAssetLogReportDTO>();
                foreach (DataRow assetLogDataRow in monitorLogData.Rows)
                {

                    monitorAssetLogReportDTOList.Add(new MonitorAssetLogReportDTO(
                                                            Convert.ToDateTime(assetLogDataRow["Timestamp"]),
                                                            assetLogDataRow["status"].ToString(),
                                                            assetLogDataRow["priority"].ToString(),
                                                            assetLogDataRow["logText"].ToString(),
                                                            assetLogDataRow["logKey"].ToString(),
                                                            assetLogDataRow["logValue"].ToString())
                                                     );

                }
                log.Debug("Ends-GetMonitorAssetLogList() Method by returning monitorAssetLogReportDTOList.");
                return monitorAssetLogReportDTOList;
            }
            else
            {
                log.Debug("Ends-GetMonitorAssetLogList() Method by returning null.");
                return null;
            }
        }



        /// <summary>
        /// Gets the Monitor Asset Cross Tab Data list based on the parameters
        /// </summary>
        /// <param name="strPriority">string type parameter</param>
        /// <returns>Returns the list of MonitorAssetLogReportDTO </returns>
        public DataTable GetMonitorAssetSummaryCrossTab(string strPriority)
        {
            log.Debug("Starts-GetMonitorAssetSummaryCrossTab(strPriority) Method.");

            string selectAssetSummaryCrossTabQuery = @"SELECT stuff((select distinct ',isnull(' + quotename(ma.ApplicationName)  + ',''No Data'') as '+  ma.ApplicationName
                                                                from MonitorApplication  ma  
                                                                where ma.ApplicationName 
                                                                in ('PRIMARY_SERVER','EXSYS_SERVER','DATA_UPLOAD_SERVER')
                                                         FOR XML PATH(''), TYPE
                                                        ).value('.', 'NVARCHAR(MAX)') 
                                                        ,1,1,'') ";

            string pivotColumns = dataAccessHandler.executeSelectQuery(selectAssetSummaryCrossTabQuery, null).Rows[0][0].ToString();

            selectAssetSummaryCrossTabQuery = @"SELECT site_id as SiteId,SiteCode,
                                                        site_name as SiteName," + pivotColumns + @",Remarks 
                                                from 
                                                (
		                                                SELECT s.site_id,s.SiteCode,
							                                    s.site_name, msv.ApplicationName,
							                                    STUFF((SELECT ',' + QUOTENAME(mev1.MonitorName + '-' + isnull(mev1.Status,'No Data') + ' - ' + isnull(mev1.LogText,''))
										                                from MonitorErrorView  mev1 
										                                where mev1.site_id = msv.site_id  
										                                and  (mev1.Status in ('WARNING','ERROR') or mev1.status is null) 
										                                and mev1.ApplicationName  not in ('PRIMARY_SERVER','EXSYS_SERVER','DATA_UPLOAD_SERVER','MAIN_INSTALLER','WRAPPER_INSTALLER') 
									                                        FOR XML PATH(''), TYPE
								                                                    ).value('.', 'NVARCHAR(MAX)') ,1,1,'') 
							                                            as Remarks,
						                                    case when (mev.MonitorId is not null and (mev.Status in ('WARNING','ERROR') or mev.status is null))
						                                    then isnull(mev.Status,'No Data') 
						                                    else 'OK' end as AssetStatus
					                                    from MonitorStatusView  msv 
					                                    inner join MonitorApplication ma on ma.ApplicationName = msv.ApplicationName
					                                    left outer join MonitorErrorView mev on msv.MonitorId= mev.MonitorId 
					                                    inner join site s on s.site_id = msv.site_id 
					                                    WHERE s.SiteCode is not null and notes is null
			                                    ) x
                                                pivot 
                                                (
                                                    max(AssetStatus) 
                                                    for ApplicationName in ([PRIMARY_SERVER],[EXSYS_SERVER],[DATA_UPLOAD_SERVER])
                                                ) p ";

            DataTable monitorAssetSummaryData = dataAccessHandler.executeSelectQuery(selectAssetSummaryCrossTabQuery,null);

            log.Debug("Ends-GetMonitorAssetLogList() Method by returning null.");
            return monitorAssetSummaryData;
        }

    }
}

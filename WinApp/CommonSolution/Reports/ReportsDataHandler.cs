/********************************************************************************************
 * Project Name - Reports Data Handler
 * Description  - Data handler of the reports class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00          14-Apr-2017   Amaresh          Created 
 *2.70.2        12-Jul-2019   Dakshakh raj     Modified : added GetSQLParameters() and
 *                                                      SQL injection Issue Fix
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query   
 * 2.80         15-Jun-2020   Laster Menezes   updated Datahandler with new RepeatBreakcolumns column
 * 2.100        28-Sep-2020   Laster Menezes   updated Datahandler with new columns HeaderBackgroundColor, HeaderTextColor
 * 2.100        15-Oct-2020   Laster Menezes   updated Datahandler with new column RowCountPerPage
 * 2.110        20-Dec-2020   Nitin Pai        Dashboard Changes - Added new variable IsDashboard
 * 2.110        22-Dec-2020   Laster Menezes   Added new methods  GetPOSListByPaymentDate, GetUsersListByPaymentDate
 * 2.110        02-Dec-2020   Laster menezes   IsDashboard column related changes. Modified method GetUsersListByPaymentDate
 *                                             Modified GetUsersListFromTransaction,GetPOSList methods to fetch the details based on paymentdate along with trxdate
 *                                             Modified InventoryReportQuery to include, PriceInTickets column.
 * 2.110        26-Feb-2020   Laster menezes   Modified InventoryAdjustmentsReportQuery, ReceivedReportQuery to eliminate duplicate records                         
 * 2.120        26-Apr-2021   Laster Menezes   updated Datahandler with new columns PageSize, PageWidth, PageHeight, IsPortrait, IsReceipt
 * 2.120        03-May-2021   Laster Menezes   Modified GetMessageList method to include site filter to fix mesaage transalation issues in HQ scenarios
 * 2.130        29-Jun-2021   Laster Menezes   Modified Inventory report queries to include offset in report columns
 * 2.130        01-Jul-2021   Laster Menezes   updated Datahandler with new columns DashboardType, IsActive
 * 2.130        20-Sep-2021   Laster Menezes   Updated GetGameProfileList method to get the  game profile list not based role access
 * 2.130        04-Oct-2021   Laster Menezes   Updated Inventory report query to include site name column, 
 *                                             Fixed issue of loading Inactive products when 'show active products' only checkbox was selected
 * 2.140        20-Oct-2021   Laster Menezes   Modified GetMessageList method to update the query to handle null cases in translated messages                                    
 * 2.140.0      06-Dec-2021   Laster Menezes   Modified method GetReportsByGroup to include only reports under management form access main menu 'Run Reports'
 *                                             Modified GetUsersListFromTransaction, GetPOSList methods to load the POS and Users when deposit collection is involved in the transaction
 * 2.150        03-Sep-2022   Rakshith Shetty  Added GetHomePageReports Method to return list of Home Page Dashboard Reports.   
 * 2.150.1      06-Mar-2023   Rakshith Shetty  Modified GetHomePageReports Method to return list of Home Page Dashboard Reports with siteid and userrole as inputs to return the list based on management form access. 
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// Reports Data Handler - Handles insert, update and select of Reports Data objects
    /// </summary>
    public class ReportsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Reports";

        /// <summary>
        /// Dictionary for searching Parameters for the Reports object.
        /// </summary>
        private static readonly Dictionary<ReportsDTO.SearchByReportsParameters, string> DBSearchParameters = new Dictionary<ReportsDTO.SearchByReportsParameters, string>
            {
                {ReportsDTO.SearchByReportsParameters.REPORT_ID, "report_id"},
                {ReportsDTO.SearchByReportsParameters.REPORT_KEY, "report_key"},
                {ReportsDTO.SearchByReportsParameters.REPORT_NAME, "report_name"},
                {ReportsDTO.SearchByReportsParameters.SITE_ID, "site_id"},
                {ReportsDTO.SearchByReportsParameters.MASTERENTITYID, "Masterentityid"},
                {ReportsDTO.SearchByReportsParameters.CUSTOMFLAG, "Customflag"},
                {ReportsDTO.SearchByReportsParameters.REPORT_GROUP, "report_group"},
                {ReportsDTO.SearchByReportsParameters.IS_DASHBOARD, "isDashboard"},
                {ReportsDTO.SearchByReportsParameters.IS_RECEIPT,"IsReceipt"},
                {ReportsDTO.SearchByReportsParameters.DASHBOARD_TYPE,"DashboardType"},
                {ReportsDTO.SearchByReportsParameters.IS_ACTIVE,"IsActive"},
            };


        /// <summary>
        ///  Default constructor of ReportsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }


        public ReportsDataHandler(string connectionString, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(connectionString, sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            if (string.IsNullOrEmpty(connectionString))
            {
                dataAccessHandler = new DataAccessHandler();
            }
            else
            {
                dataAccessHandler = new DataAccessHandler(connectionString);
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor of ReportsDataHandler class
        /// </summary>
        /// <param name="commandTimeout">timeOutSet</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportsDataHandler(int commandTimeout, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(commandTimeout, sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            dataAccessHandler.CommandTimeOut = commandTimeout;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Report parameters Record.
        /// </summary>
        /// <param name="reportsDTO">reportsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ReportsDTO reportsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportId", reportsDTO.ReportId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportName", string.IsNullOrEmpty(reportsDTO.ReportName) ? DBNull.Value : (object)reportsDTO.ReportName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportKey", string.IsNullOrEmpty(reportsDTO.ReportKey) ? DBNull.Value : (object)reportsDTO.ReportKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@customFlag", string.IsNullOrEmpty(reportsDTO.CustomFlag) ? "N" : (object)reportsDTO.CustomFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@outputFormat", string.IsNullOrEmpty(reportsDTO.OutputFormat) ? DBNull.Value : (object)reportsDTO.OutputFormat));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dbQuery", string.IsNullOrEmpty(reportsDTO.DBQuery) ? DBNull.Value : (object)reportsDTO.DBQuery));
            parameters.Add(dataAccessHandler.GetSQLParameter("@breakColumn", reportsDTO.BreakColumn == "" ? DBNull.Value : (object)reportsDTO.BreakColumn));
            parameters.Add(dataAccessHandler.GetSQLParameter("@hideBreakColumn", string.IsNullOrEmpty(reportsDTO.HideBreakColumn) ? DBNull.Value : (object)reportsDTO.HideBreakColumn));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportGroup", string.IsNullOrEmpty(reportsDTO.ReportGroup) ? DBNull.Value : (object)reportsDTO.ReportGroup));
            parameters.Add(dataAccessHandler.GetSQLParameter("@aggregateColumns", string.IsNullOrEmpty(reportsDTO.AggregateColumns) ? DBNull.Value : (object)reportsDTO.AggregateColumns));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@hideGridLines", string.IsNullOrEmpty(reportsDTO.HideGridLines) ? DBNull.Value : (object)reportsDTO.HideGridLines));
            parameters.Add(dataAccessHandler.GetSQLParameter("@showGrandTotal", string.IsNullOrEmpty(reportsDTO.ShowGrandTotal) ? DBNull.Value : (object)reportsDTO.ShowGrandTotal));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", reportsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@printContinuous", string.IsNullOrEmpty(reportsDTO.PrintContinuous) ? DBNull.Value : (object)reportsDTO.PrintContinuous));
            parameters.Add(dataAccessHandler.GetSQLParameter("@repeatBreakColumns", string.IsNullOrEmpty(reportsDTO.RepeatBreakColumns) ? DBNull.Value : (object)reportsDTO.RepeatBreakColumns));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maxDateRange", reportsDTO.MaxDateRange == -1 ? DBNull.Value : (object)reportsDTO.MaxDateRange));
            parameters.Add(dataAccessHandler.GetSQLParameter("@HeaderBackgroundColor", string.IsNullOrEmpty(reportsDTO.HeaderBackgroundColor) ? DBNull.Value : (object)reportsDTO.HeaderBackgroundColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@HeaderTextColor", string.IsNullOrEmpty(reportsDTO.HeaderTextColor) ? DBNull.Value : (object)reportsDTO.HeaderTextColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RowCountPerPage", reportsDTO.RowCountPerPage == -1 ? DBNull.Value : (object)reportsDTO.RowCountPerPage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isDashboard", reportsDTO.IsDashboard));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pageSize", reportsDTO.PageSize == -1 ? DBNull.Value : (object)reportsDTO.PageSize));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pageWidth", reportsDTO.PageWidth == 0 ? DBNull.Value : (object)reportsDTO.PageWidth));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pageHeight", reportsDTO.PageHeight == 0 ? DBNull.Value : (object)reportsDTO.PageHeight));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isPortrait", reportsDTO.IsPortrait));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isReceipt", reportsDTO.IsReceipt));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dashboardType", reportsDTO.DashboardType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", reportsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }



        /// <summary>
        /// Inserts the reports record to the database
        /// </summary>
        /// <param name="reportsDTO">ReportsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ReportsDTO InsertReports(ReportsDTO reportsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportsDTO, loginId, siteId);
            string insertReportsQuery = @"INSERT INTO [dbo].[Reports] 
                                                        (                                                         
                                                        report_name,
                                                        report_key,
                                                        CustomFlag,
                                                        OutputFormat,
                                                        DBQuery,
                                                        BreakColumn,
                                                        HideBreakColumn,
                                                        report_group,
                                                        AggregateColumns,
                                                        Guid,
                                                        site_id,
                                                        HideGridLines,
                                                        MasterEntityId,
                                                        ShowGrandTotal,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdateDate,
                                                        PrintContinuous, 
                                                        RepeatBreakColumns,
                                                        MaxDateRange,                                                        
                                                        HeaderBackgroundColor,
                                                        HeaderTextColor,
                                                        RowCountPerPage,
                                                        IsDashboard,
                                                        PageSize,
                                                        PageWidth,
                                                        PageHeight,
                                                        IsPortrait,
                                                        IsReceipt,
                                                        DashboardType,
                                                        IsActive
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @reportName,
                                                        @reportKey,
                                                        @customFlag,
                                                        @outputFormat,
                                                        @dbQuery,                                                        
                                                        @breakColumn,
                                                        @hideBreakColumn,
                                                        @reportGroup,
                                                        @aggregateColumns,
                                                        NewId(),
                                                        @siteId,
                                                        @hideGridLines,
                                                        @masterEntityId,
                                                        @showGrandTotal,                                                       
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        @printContinuous,
                                                        @repeatBreakColumns,
                                                        @maxDateRange,                                                        
                                                        @headerBackgroundColor,
                                                        @headerTextColor,
                                                        @rowCountPerPage,
                                                        @isDashboard,
                                                        @pageSize,
                                                        @pageWidth,
                                                        @pageHeight,
                                                        @isPortrait,
                                                        @isReceipt,
                                                        @dashboardType,
                                                        @isActive
                                                        )SELECT * FROM Reports WHERE report_Id = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertReportsQuery, GetSQLParameters(reportsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportsDTO(reportsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ReportsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportsDTO);
            return reportsDTO;
        }

        /// <summary>
        /// Inserts the reports record to the database.
        /// </summary>
        /// <param name="reportID">reportID</param>
        /// <param name="start_time">start_time</param>
        /// <param name="end_time">end_time</param>
        /// <param name="param_list">param_list</param>
        /// <param name="username">username</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertReportAuditRecord(string reportID, DateTime start_time, DateTime end_time, string param_list, string username, int siteId)
        {
            log.LogMethodEntry(reportID, start_time, end_time, param_list, username, siteId);
            string insertReportsQuery = @"INSERT INTO [dbo].[RunReportAudit] ([report_id],[start_time],[end_time],[parameter_list], creation_date, created_by, last_update_date, last_updated_by, Guid, site_id) 
                                          VALUES (@report_id,@start_time,@end_time,@parameter_list, getdate(), @user, getdate(), @user, newid(), @site_id)";

            List<SqlParameter> insertReportsParameters = new List<SqlParameter>();

            insertReportsParameters.Add(new SqlParameter("@report_id", reportID));
            insertReportsParameters.Add(new SqlParameter("@start_time", start_time));
            insertReportsParameters.Add(new SqlParameter("@end_time", end_time));
            insertReportsParameters.Add(new SqlParameter("@parameter_list", param_list));
            insertReportsParameters.Add(new SqlParameter("@user", username));
            if (siteId == -1)
                insertReportsParameters.Add(new SqlParameter("@site_id", DBNull.Value));
            else
                insertReportsParameters.Add(new SqlParameter("@site_id", siteId));
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertReportsQuery, insertReportsParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }


        /// <summary>
        /// Inserts the managementformaccess record to the database
        /// </summary>
        /// <param name="formName">form name</param>
        /// <param name="reportId">reportId</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertManagementformaccess(string formName, int reportId, int siteId)
        {
            log.LogMethodEntry(formName, reportId, siteId);
            string insertManagementformaccessQuery = @"insert into ManagementFormAccess (role_id, main_menu, form_name, access_allowed, FunctionGUID, site_id) 
                                                    select role_id, 'Run Reports', @form_name, manager_flag ,( select guid from Reports where report_id = @reportId), @site_id  
                                                    from user_roles 
                                                    where site_id = @site_id or @site_id is null";

            List<SqlParameter> insertManagementformaccessParameters = new List<SqlParameter>();

            insertManagementformaccessParameters.Add(new SqlParameter("@form_name", formName));
            insertManagementformaccessParameters.Add(new SqlParameter("@reportId", reportId));
            if (siteId == -1)
                insertManagementformaccessParameters.Add(new SqlParameter("@site_id", DBNull.Value));
            else
                insertManagementformaccessParameters.Add(new SqlParameter("@site_id", siteId));
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertManagementformaccessQuery, insertManagementformaccessParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Inserts the managementformaccess record to the database
        /// </summary>
        /// <param name="GlobalReportScheduleReportId">report ID</param>
        /// <returns>Returns report output format</returns>
        public string getReportOutputFormat(int GlobalReportScheduleReportId)
        {
            log.LogMethodEntry(GlobalReportScheduleReportId);
            string selectQuery = @"select isnull(OutputFormat, 'D') 
                                                        from Report_Schedule_Reports 
                                                        where report_schedule_report_id = @report_schedule_report_id";

            List<SqlParameter> selectParameters = new List<SqlParameter>();

            selectParameters.Add(new SqlParameter("@report_schedule_report_id", GlobalReportScheduleReportId));
            DataTable dtOutputFormat = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(dtOutputFormat.Rows[0][0].ToString());
            return dtOutputFormat.Rows[0][0].ToString();
        }

        /// <summary>
        /// Updates the user record
        /// </summary>
        /// <param name="reportsDTO">ReportsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ReportsDTO UpdateReports(ReportsDTO reportsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportsDTO, loginId, siteId);

            string updateReportsQuery = @"update Reports 
                                           set  report_name = @reportName,
                                                report_key = @reportKey,
                                                CustomFlag = @customFlag,
                                                OutputFormat = @outputFormat,
                                                DBQuery = @dbQuery,
                                                BreakColumn = @breakColumn,
                                                HideBreakColumn = @hideBreakColumn,
                                                report_group = @reportGroup,
                                                AggregateColumns = @aggregateColumns,
                                                -- site_id = @siteId,
                                                HideGridLines = @hideGridLines,
                                                MasterEntityId = @masterEntityId,
                                                ShowGrandTotal = @showGrandTotal,                                               
                                                LastUpdatedBy = @lastUpdatedBy,
                                                LastUpdateDate = GETDATE(),
                                                PrintContinuous = @printContinuous, 
                                                RepeatBreakColumns = @repeatBreakColumns,
                                                MaxDateRange = @maxDateRange,                                                
                                                HeaderBackgroundColor = @headerBackgroundColor,
                                                HeaderTextColor = @headerTextColor,
                                                RowCountPerPage = @rowCountPerPage,
                                                IsDashboard = @isDashboard,
                                                PageSize = @pageSize,
                                                PageWidth = @pageWidth,
                                                PageHeight = @pageHeight,
                                                IsPortrait = @isPortrait,
                                                IsReceipt = @isReceipt,
                                                DashboardType = @dashboardType,
                                                IsActive = @isActive
                                          where report_id = @reportId 
                                          SELECT* FROM Reports WHERE  report_id = @reportId  ";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateReportsQuery, GetSQLParameters(reportsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportsDTO(reportsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ReportsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportsDTO);
            return reportsDTO;
        }

        /// <summary>
        /// Updates the user record
        /// </summary>
        /// <param name="reportsDTO">ReportsDTO type parameter</param>
        /// <returns>Returns the count of updated rows</returns>
        public int DeleteReport(ReportsDTO reportsDTO)
        {
            log.LogMethodEntry(reportsDTO);

            string deleteReportsQuery = @"Delete Reports 
                                          where report_id = @reportId";

            List<SqlParameter> deleteReportsParameters = new List<SqlParameter>();
            deleteReportsParameters.Add(new SqlParameter("@reportId", reportsDTO.ReportId));
            int rowsDeleted = dataAccessHandler.executeUpdateQuery(deleteReportsQuery, deleteReportsParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(rowsDeleted);
            return rowsDeleted;
        }

        /// <summary>
        ///Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="reportsDTO">reportsDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshReportsDTO(ReportsDTO reportsDTO, DataTable dt)
        {
            log.LogMethodEntry(reportsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                reportsDTO.ReportId = Convert.ToInt32(dt.Rows[0]["report_Id"]);
                reportsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                reportsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                reportsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                reportsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                reportsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                reportsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ReportsDTO class type
        /// </summary>
        /// <param name="reportsDataRow">ReportsDTO DataRow</param>
        /// <returns>Returns ReportsDTO</returns>
        private ReportsDTO GetReportsDTO(DataRow reportsDataRow)
        {
            log.LogMethodEntry(reportsDataRow);
            ReportsDTO reportsDataObject = new ReportsDTO(Convert.ToInt32(reportsDataRow["report_id"]),
                                                    reportsDataRow["report_name"].ToString(),
                                                    reportsDataRow["report_key"].ToString(),
                                                    reportsDataRow["CustomFlag"] == DBNull.Value ? "N" : reportsDataRow["CustomFlag"].ToString(),
                                                    reportsDataRow["OutputFormat"].ToString(),
                                                    reportsDataRow["DisplayOutputFormat"].ToString(),
                                                    reportsDataRow["DBQuery"].ToString(),
                                                    reportsDataRow["BreakColumn"] == DBNull.Value ? "" : (reportsDataRow["BreakColumn"].ToString()),
                                                    reportsDataRow["HideBreakColumn"].ToString(),
                                                    reportsDataRow["report_group"].ToString(),
                                                    reportsDataRow["AggregateColumns"].ToString(),
                                                    reportsDataRow["Guid"].ToString(),
                                                    reportsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(reportsDataRow["site_id"]),
                                                    reportsDataRow["HideGridLines"].ToString(),
                                                    reportsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(reportsDataRow["SynchStatus"]),
                                                    reportsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(reportsDataRow["MasterEntityId"]),
                                                    reportsDataRow["ShowGrandTotal"] == DBNull.Value ? "N" : reportsDataRow["ShowGrandTotal"].ToString(),
                                                    reportsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(reportsDataRow["CreatedBy"]),
                                                    reportsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportsDataRow["CreationDate"]),
                                                    reportsDataRow["LastUpdatedBy"].ToString(),
                                                    reportsDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportsDataRow["LastUpdateDate"]),
                                                    reportsDataRow["PrintContinuous"] == DBNull.Value ? "N" : reportsDataRow["PrintContinuous"].ToString(),
                                                    reportsDataRow["RepeatBreakColumns"] == DBNull.Value ? "" : reportsDataRow["RepeatBreakColumns"].ToString(),
                                                    reportsDataRow["MaxDateRange"] == DBNull.Value ? -1 : Convert.ToInt32(reportsDataRow["MaxDateRange"]),
                                                    reportsDataRow["HeaderBackgroundColor"] == DBNull.Value ? string.Empty : reportsDataRow["HeaderBackgroundColor"].ToString(),
                                                    reportsDataRow["HeaderTextColor"] == DBNull.Value ? string.Empty : reportsDataRow["HeaderTextColor"].ToString(),
                                                    reportsDataRow["RowCountPerPage"] == DBNull.Value ? -1 : Convert.ToInt32(reportsDataRow["RowCountPerPage"]),
                                                    reportsDataRow["IsDashboard"] == DBNull.Value ? false : Convert.ToBoolean(reportsDataRow["IsDashboard"]),
                                                    reportsDataRow["PageSize"] == DBNull.Value ? -1 : Convert.ToInt32(reportsDataRow["PageSize"]),
                                                    reportsDataRow["PageWidth"] == DBNull.Value ? 0 : Convert.ToDouble(reportsDataRow["PageWidth"]),
                                                    reportsDataRow["PageHeight"] == DBNull.Value ? 0 : Convert.ToDouble(reportsDataRow["PageHeight"]),
                                                    reportsDataRow["IsPortrait"] == DBNull.Value ? false : Convert.ToBoolean(reportsDataRow["IsPortrait"]),
                                                    reportsDataRow["IsReceipt"] == DBNull.Value ? false : Convert.ToBoolean(reportsDataRow["IsReceipt"]),
                                                    reportsDataRow["DashboardType"] == DBNull.Value ? string.Empty : reportsDataRow["DashboardType"].ToString(),
                                                    reportsDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(reportsDataRow["IsActive"]));
            log.LogMethodExit(reportsDataObject);
            return reportsDataObject;
        }

        /// <summary>
        /// Gets the user data of passed userId
        /// </summary>
        /// <param name="reportId">integer type parameter</param>
        /// <returns>Returns ReportsDTO</returns>
        public ReportsDTO GetReports(int reportId)
        {
            log.LogMethodEntry(reportId);
            string selectReportQuery = @"select *, case OutputFormat when 'E' then 'Excel' when 'C' then 'Chart' when 'V' then 'CSV' else 'PDF' end DisplayOutputFormat
                                         from Reports
                                        where report_id = @reportId";
            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@reportId", reportId);
            DataTable reports = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (reports.Rows.Count > 0)
            {
                DataRow reportRow = reports.Rows[0];
                ReportsDTO reportDataObject = GetReportsDTO(reportRow);
                log.LogMethodExit(reportDataObject);
                return reportDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }


        /// <summary>
        /// GetReportsByReportKey Method
        /// </summary>
        /// <param name="reportKey">reportKey</param>
        /// <returns></returns>
        public ReportsDTO GetReportsByReportKey(string reportKey)
        {
            log.LogMethodEntry(reportKey);
            string selectReportQuery = @"select *, case OutputFormat when 'E' then 'Excel' when 'C' then 'Chart' when 'V' then 'CSV' else 'PDF' end DisplayOutputFormat
                                         from Reports
                                        where report_key = @reportKey";
            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@reportKey", reportKey);
            DataTable reports = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (reports.Rows.Count > 0)
            {
                DataRow reportRow = reports.Rows[0];
                ReportsDTO reportDataObject = GetReportsDTO(reportRow);
                log.LogMethodExit(reportDataObject);
                return reportDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }


        /// <summary>
        /// Gets the ReportsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ReportsDTO matching the search criteria</returns>
        public List<ReportsDTO> GetReportsList(List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ReportsDTO> reportsList = new List<ReportsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @"select *, case OutputFormat when 'E' then 'Excel' when 'C' then 'Chart' when 'V' then 'CSV' else 'PDF' end DisplayOutputFormat
                                         from Reports ";

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ReportsDTO.SearchByReportsParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ReportsDTO.SearchByReportsParameters.REPORT_ID
                            || searchParameter.Key == ReportsDTO.SearchByReportsParameters.MASTERENTITYID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ReportsDTO.SearchByReportsParameters.REPORT_NAME
                                 || searchParameter.Key == ReportsDTO.SearchByReportsParameters.REPORT_KEY
                                 || searchParameter.Key == ReportsDTO.SearchByReportsParameters.DASHBOARD_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ReportsDTO.SearchByReportsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ReportsDTO.SearchByReportsParameters.CUSTOMFLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ReportsDTO.SearchByReportsParameters.IS_DASHBOARD || searchParameter.Key == ReportsDTO.SearchByReportsParameters.IS_RECEIPT)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ", 0) =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ReportsDTO.SearchByReportsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ", 1) =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }

                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query + " Order by report_group, report_name";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ReportsDTO reportsDTO = GetReportsDTO(dataRow);
                    reportsList.Add(reportsDTO);
                }
            }
            log.LogMethodExit(reportsList);
            return reportsList;
        }

        /// <summary>
        /// Gets report output formats
        /// </summary>
        /// <returns>Returns ReportList</returns>
        public List<ReportsDTO> GetReportsByGroup(int site_id, int role_id, string group)
        {
            log.LogMethodEntry(site_id, role_id, group);
            List<ReportsDTO> reportsList = null;
            string selectReportQuery = @"select r.*, case OutputFormat when 'E' then 'Excel' when 'C' then 'Chart' when 'V' then 'CSV' else 'PDF' end DisplayOutputFormat
                                        from reports r, managementFormAccess a
                                        where r.report_name = a.form_name 
                                            and a.main_menu = 'Run Reports' 
                                            and a.access_allowed = 'Y' 
                                            and (r.site_id = @site_id or @site_id = -1) 
                                            and a.role_id = @role_id 
                                            and (report_group = @group or @group is null or (@group = 'Others' and (report_group is null or report_group = '')))
                                            and (r.IsActive is null or r.IsActive = 1)
                                            and r.report_group!='HomePage'
                                        order by report_group, report_name";
            SqlParameter[] selectReportParameters = new SqlParameter[3];
            selectReportParameters[0] = new SqlParameter("@site_id", site_id);
            selectReportParameters[1] = new SqlParameter("@role_id", role_id);
            if (group == "All")
                selectReportParameters[2] = new SqlParameter("@group", DBNull.Value);
            else
                selectReportParameters[2] = new SqlParameter("@group", group);
            DataTable reportsData = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);
            if (reportsData.Rows.Count > 0)
            {
                reportsList = new List<ReportsDTO>();
                foreach (DataRow reportsDataRow in reportsData.Rows)
                {
                    ReportsDTO reportsDataObject = GetReportsDTO(reportsDataRow);
                    reportsList.Add(reportsDataObject);
                }
            }
            log.LogMethodExit(reportsList);
            return reportsList;

        }

        /// <summary>
        /// Gets accessible functions for the RoleID that is passed
        /// </summary>
        /// <param name="roleID">integer type parameter</param>
        /// <returns>Returns reports</returns>
        public DataTable GetAccessibleFunctions(int roleID)
        {
            log.LogMethodEntry(roleID);
            string selectReportQuery = @"select form_name, access_allowed 
                                         from ManagementFormAccess a 
                                         where a.role_id = @roleID 
                                            and main_menu in ('Reports', 'Manage Reports') 
                                            and form_name in ('Run Reports', 'Management', 'Schedule')";
            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@roleID", roleID);
            DataTable reports = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (reports.Rows.Count > 0)
            {
                log.LogMethodExit(reports);
                return reports;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets report output formats
        /// </summary>
        /// <returns>Returns reportFormats</returns>
        public DataTable GetReportOutputFormats()
        {
            log.LogMethodEntry();
            string selectReportQuery = @"select 'D' Code, 'Default' Name
                                        union all
                                        select 'P' Code, 'PDF' Name
                                       -- union all
                                       -- select 'H' Code, 'HTML' Name
                                        union all
                                        select 'E' Code, 'Excel' Name
                                        union all
                                         select 'C' Code, 'Chart' Name
                                        union all
                                        select 'V' Code, 'CSV' Name
                                       --union all
                                       --  select 'B' Code, 'DBF' Name
                                      --union all
                                       --  select 'I' Code, 'IIF' Name
                                        ";
            DataTable reportFormats = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);

            if (reportFormats.Rows.Count > 0)
            {
                log.LogMethodExit(reportFormats);
                return reportFormats;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets custom report data
        /// </summary>
        /// <param name="DBQuery">DBQuery</param>
        /// <param name="OtherParams">OtherParams</param>
        /// <returns>reportFormat</returns>
        public DataTable GetCustomReportData(string DBQuery, List<SqlParameter> OtherParams)
        {
            log.LogMethodEntry(DBQuery, OtherParams);
            DataTable customReportData = new DataTable();

            if (OtherParams == null)
                customReportData = dataAccessHandler.executeSelectQuery(DBQuery, null);
            else
                customReportData = dataAccessHandler.executeSelectQuery(DBQuery, OtherParams.ToArray(), sqlTransaction);

            if (customReportData.Rows.Count > 0)
            {
                log.LogMethodExit(customReportData);
                return customReportData;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        ///  Gets report group list
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="role_id">role_id</param>
        /// <returns>report group list</returns>
        public DataTable GetReportGroupList(int site_id, int role_id)
        {
            log.LogMethodEntry(site_id, role_id);
            string selectReportQuery = @"select distinct case when report_group is null then 'Others' when report_group = '' then 'Others' else report_group end report_group, 1 sort 
                                        from reports r, managementFormAccess a 
                                        where r.report_name = a.form_name 
                                        and (r.site_id in (@site_id) or @site_id = -1) 
                                        and a.role_id = @role_id
                                        and a.access_allowed = 'Y' 
                                        and (r.IsActive is null or r.IsActive = 1)
					and r.report_group!='HomePage'
                                        union all select 'All', 2 order by sort, report_group ";
            SqlParameter[] selectReportParameters = new SqlParameter[2];
            selectReportParameters[0] = new SqlParameter("@role_id", role_id);
            selectReportParameters[1] = new SqlParameter("@site_id", site_id);
            DataTable reportGroup = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (reportGroup.Rows.Count > 0)
            {
                log.LogMethodExit(reportGroup);
                return reportGroup;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets report group list.
        /// </summary>
        /// <param name="LanguageID">LanguageID</param>
        /// <returns>Returns languageList</returns>
        public DataTable GetMessageList(long LanguageID)
        {
            log.LogMethodEntry(LanguageID);
            string selectReportQuery = @"select m.message, isnull(tl.Message, m.Message) TranslatedMessage
                                         from Messages m 
                                            left outer join MessagesTranslated tl on m.MessageId = tl.MessageId and tl.LanguageId = @langId ";
            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@langId", LanguageID);
            DataTable languageList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (languageList.Rows.Count > 0)
            {
                log.LogMethodExit(languageList);
                return languageList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets report group list
        /// </summary>
        /// <returns>Returns languageList</returns>
        /// <param name="language">language</param>
        /// <param name="siteId">siteId</param>
        public DataTable GetMessageList(string language, int siteId)
        {
            log.LogMethodEntry(language, siteId);
            string selectReportQuery = @"select isnull(tl.Message, m.Message) 'TranslatedMessage' , m.Message message  
                                                 from Messages m left outer join MessagesTranslated tl
                                                      on m.MessageId = tl.MessageId and( tl.site_id= @siteId or -1= @siteId)                                                    
                                                 where 
                                                 tl.LanguageId in (
						                         select LanguageId from  Languages  
                                                 where LanguageName= @lang 
                                                 and (site_id= @siteId or -1= @siteId )
						                         ) 
                                                 and m.MessageNo >= 10000 ";

            SqlParameter[] selectReportParameters = new SqlParameter[2];
            selectReportParameters[0] = new SqlParameter("@lang", language);
            selectReportParameters[1] = new SqlParameter("@siteId", siteId);
            DataTable languageList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (languageList.Rows.Count > 0)
            {
                log.LogMethodExit(languageList);
                return languageList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }



        /// <summary>
        /// Gets report game profile list.
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="role_id">role_id<param>
        /// <returns>Returns languageList</returns>
        public DataTable GetGameProfileList(string site_id, int role_id)
        {
            log.LogMethodEntry(site_id, role_id);
            string selectReportQuery = @"select distinct profile_name,game_profile_id
                                        from game_profile where
                                        site_id in (@site_id) or -1 in (@site_id)                                
                                        order by 1 ";
            selectReportQuery = selectReportQuery.Replace("@site_id", site_id);
            DataTable GameProfileList = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);

            if (GameProfileList.Rows.Count > 0)
            {
                log.LogMethodExit(GameProfileList);
                return GameProfileList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets report game machine list
        /// </summary>
        /// <param name="profile_id">profile_id</param>
        /// <returns>returns language list</returns>
        public DataTable GetMachineList(string profile_id)
        {
            log.LogMethodExit(profile_id);
            string selectReportQuery = @"select machine_name, machine_id 
                                        from machines m, games g, game_profile gp 
                                        where gp.game_profile_id in " + profile_id +
                                            @" and gp.game_profile_id = g.game_profile_id
                                            and g.game_id = m.game_id  ";
            DataTable GameMachineList = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);

            if (GameMachineList.Rows.Count > 0)
            {
                log.LogMethodExit(GameMachineList);
                return GameMachineList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets Category list
        /// </summary>
        /// <param name="site_id_array">site_id_array</param>
        /// <returns>Returns categoryList</returns>
        public DataTable GetCategories(string site_id_array)
        {
            log.LogMethodExit(site_id_array);
            //string condition = " site_id is null";
            //if (!string.IsNullOrEmpty(site_id_array) && site_id_array!="-1")
            //{
            //    condition = "site_id in (" + site_id_array + ")";
            //}

            string selectReportQuery = @"select -1 categoryid, '-All-' name
                                         union all
                                         select categoryid, name
                                         from category 
                                        where  site_id in (@site_id) or (-1 in (@site_id))  
                                         order by 1 ";
            selectReportQuery = selectReportQuery.Replace("@site_id", site_id_array);
            DataTable categoryList = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);

            if (categoryList.Rows.Count > 0)
            {
                log.LogMethodExit(categoryList);
                return categoryList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets Users list from transaction.
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="from">from</param>
        /// <param name="to">to</param>
        /// <returns>Returns userList</returns>
        public DataTable GetUsersListFromTransaction(string site_id, DateTime from, DateTime to)
        {
            log.LogMethodEntry(site_id, from, to);
            string selectReportQuery = string.Empty;
            if (site_id == "-1" || site_id == null)
            {
                selectReportQuery = @"select  distinct u.user_id,
                                        (case when u.active_flag!='Y' then u.username+' inactive' else u.username end)username from users u,trx_header h
                                        where h.user_id=u.user_id and (u.site_id in (@site) or (-1 in (@site)))
                                        and trxdate >= @fromDate
                                        AND trxdate < @toDate
                                        union
                                        select  distinct u.user_id,
                                        (case when u.active_flag!='Y' then u.username+' inactive' else u.username end)username  from users u,TrxPayments tp
                                        where tp.CreatedBy=u.loginid  and (tp.site_id=u.site_id or tp.site_id is null) and (u.site_id in (@site) or (-1 in (@site)))
                                        and PaymentDate >= @fromDate AND PaymentDate < @toDate
										union
										select distinct u.user_id,
                                        (case when u.active_flag!='Y' then u.username+' inactive' else u.username end)username  from users u,TrxPayments tp,trx_header h
                                        where tp.trxid=h.trxid and tp.CreatedBy=u.loginid and (tp.site_id=u.site_id or tp.site_id is null) 
                                        AND (u.site_id IN (@site) OR (-1 IN (@site)))
                                        and trxdate >= @fromDate AND trxdate < @toDate";
            }
            else
            {


                selectReportQuery = @"declare @dFromDate datetime=dbo.ConvertToServerTime(@fromDate,@offSet)
declare @dToDate datetime=dbo.ConvertToServerTime(@toDate,@offSet)
select  distinct u.user_id,
                                        (case when u.active_flag!='Y' then u.username+' inactive' else u.username end)username from users u,trx_header h
                                        where h.user_id=u.user_id and (u.site_id in (@site) or (-1 in (@site)))
                                        and trxdate >= @dFromDate
                                        AND trxdate <  @dToDate
                                        union
                                        select  distinct u.user_id,
                                        (case when u.active_flag!='Y' then u.username+' inactive' else u.username end)username  from users u,TrxPayments tp
                                        where tp.CreatedBy=u.loginid  and (tp.site_id=u.site_id or tp.site_id is null) and (u.site_id in (@site) or (-1 in (@site)))
                                        and PaymentDate >= @dFromDate AND PaymentDate <  @dToDate
										union all
										select distinct u.user_id,
                                        (case when u.active_flag!='Y' then u.username+' inactive' else u.username end)username  from users u,TrxPayments tp,trx_header h
                                        where tp.trxid=h.trxid and tp.CreatedBy=u.loginid and (tp.site_id=u.site_id or tp.site_id is null) 
                                        AND (u.site_id IN (@site) OR (-1 IN (@site)))
                                        and trxdate >= @dFromDate AND trxdate < @dToDate";
            }
            selectReportQuery = selectReportQuery.Replace("@site", site_id);
            SqlParameter[] selectReportParameters = new SqlParameter[3];
            selectReportParameters[0] = new SqlParameter("@todate", to);
            selectReportParameters[1] = new SqlParameter("@fromdate", from);
            selectReportParameters[2] = new SqlParameter("@offSet", Common.Offset);


            DataTable userList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (userList.Rows.Count > 0)
            {
                log.LogMethodExit(userList);
                return userList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets Users list from transaction By LastUpdateTime
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="from">from</param>
        /// <param name="to">to</param>
        /// <returns>Returns userList</returns>
        public DataTable GetUsersListFromTransactionByLastupdated(string site_id, DateTime from, DateTime to)
        {
            log.LogMethodEntry(site_id, from, to);
            string selectReportQuery = @"select  distinct u.user_id, 
                                        (case when u.active_flag!='Y' then u.username+' inactive' else u.username end)username   from users u,trx_header h 
                                        where h.user_id=u.user_id and   (u.site_id in (@site) or (-1 in (@site)))
                                        and h.LastUpdateTime between @fromdate and @todate ";
            selectReportQuery = selectReportQuery.Replace("@site", site_id);

            SqlParameter[] selectReportParameters = new SqlParameter[2];
            selectReportParameters[0] = new SqlParameter("@todate", to);
            selectReportParameters[1] = new SqlParameter("@fromdate", from);


            DataTable userList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (userList.Rows.Count > 0)
            {
                log.LogMethodExit(userList);
                return userList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets Users list 
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <returns>Returns userList</returns>
        public DataTable GetUsersList(string site_id)
        {
            log.LogMethodEntry(site_id);
            string selectReportQuery = @"SELECT user_id, username 
                                        FROM users 
                                        WHERE  active_flag = 'Y' and ((site_id in (@site)) or (-1 in (@site)))
                                        UNION ALL 
                                        (SELECT user_id, username + '(Inactive)' 
                                        FROM users 
                                        WHERE active_flag != 'Y' and ((site_id in (@site)) or (-1 in (@site))))
                                        ORDER BY 2 ";
            selectReportQuery = selectReportQuery.Replace("@site", site_id);
            DataTable userList = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);

            if (userList.Rows.Count > 0)
            {
                log.LogMethodExit(userList);
                return userList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets Users list
        /// </summary>
        /// <param name="user_id">user_id</param>
        /// <returns> returns the userList</returns>
        public DataTable GetUsersList(int user_id)
        {
            log.LogMethodEntry(user_id);
            string selectReportQuery = @"select u.user_id, username  
                                from users u, (select user_id, us.role_id, DataAccessLevel  
                                                from users us, user_roles ur  
                                                  where ur.role_id = us.role_id  
                                                  and us.user_id = @user_id) v  
                                where DataAccessLevel = 'S'  
                                    or (DataAccessLevel = 'U' and u.user_id = v.user_id)  
                                    or (DataAccessLevel = 'R' and u.role_id in (select role_id from ManagementFormAccess mfa 
								                                                    where mfa.role_id = v.role_id
                                                                                    and mfa.access_allowed = 'Y'
								                                                    and mfa.FunctionGroup = 'Data Access'	
								                                                    and mfa.main_menu = 'User Roles'))  
                             union all select -1, ' - All - ' order by 2";
            selectReportQuery = selectReportQuery.Replace("@user_id", user_id.ToString());
            DataTable userList = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);

            if (userList.Rows.Count > 0)
            {
                log.LogMethodExit(userList);
                return userList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets POS list.
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="role_id">role_id</param>
        /// <param name="ENABLE_POS_FILTER_IN_TRX_REPORT">ENABLE_POS_FILTER_IN_TRX_REPORT</param>
        /// <param name="Mode">Mode</param>
        /// <param name="FromDate">FromDate</param>
        /// <param name="ToDate">ToDate</param>
        /// <param name="offset">offset</param>
        /// <returns>Returns categoryList</returns>
        public DataTable GetPOSList(string site_id, int role_id, string ENABLE_POS_FILTER_IN_TRX_REPORT, string Mode, DateTime FromDate, DateTime ToDate, int offset)
        {
            log.LogMethodEntry(site_id, role_id, ENABLE_POS_FILTER_IN_TRX_REPORT, Mode, FromDate, ToDate, offset);
            string selectReportQuery = "";

            if (ENABLE_POS_FILTER_IN_TRX_REPORT == "Y")
                selectReportQuery = @"SELECT distinct POSname
	                                    FROM POSMachines pm, ManagementFormAccess ma, user_roles u 
	                                    WHERE pm.POSName = ma.form_name 
	                                    AND ma.main_menu = 'POS Machine'
	                                    AND ma.access_allowed = 'Y' 
	                                    AND ma.role_id = u.role_id
	                                    AND (u.role_id = @role or @role = -1)
	                                    and ((pm.site_id in (@site)) or (-1 in (@site))) ";
            else   if (site_id == "-1" || site_id == null)
            {
                selectReportQuery = @"if exists(SELECT DISTINCT pos_machine as POSname
                                        FROM trx_header  WITH (index (trx_date))
                                        WHERE trxdate >= @fromDate
                                        AND trxdate < @toDate 
                                        AND ((site_id in (@site)) or (-1 in (@site)))
                                        UNION 
                                        SELECT distinct PosMachine from TrxPayments 
                                        WHERE PaymentDate >= @fromDate
                                        AND PaymentDate < @toDate 
                                        AND ((site_id in (@site)) or (-1 in (@site)))
                                        UNION 
		                                SELECT distinct tp.PosMachine from TrxPayments tp,trx_header h
                                        WHERE tp.trxid=h.trxid 
		                                AND (( h.site_id IN (@site)) OR (-1 IN ( @site )))						
                                        AND trxdate >= @fromDate 
                                        AND trxdate < @toDate
                                        )
                                        SELECT DISTINCT pos_machine as POSname
                                        from trx_header  WITH (index (trx_date))
                                        WHERE trxdate >= @fromDate
                                        AND trxdate < @toDate 
                                        AND ((site_id in (@site)) or (-1 in (@site)))
                                        UNION 
                                        SELECT distinct PosMachine from TrxPayments 
                                        WHERE PaymentDate >= @fromDate
                                        AND PaymentDate < @toDate 
                                        AND ((site_id in (@site)) or (-1 in (@site)))
                                        UNION 
		                                SELECT distinct tp.PosMachine  from TrxPayments tp,trx_header h
                                        WHERE tp.trxid=h.trxid 
		                                AND ((h.site_id IN (@site)) OR (-1 IN (@site)))						
                                        AND trxdate >= @fromDate 
                                        AND trxdate < @toDate
                                        else
                                        select 'No Trx' as POSname";
            }
            else
            {
                selectReportQuery = @"
declare @dFromdate datetime= dbo.ConvertToServerTime(@fromDate,@offSet)
declare @dTodate datetime=dbo.ConvertToServerTime(@toDate,@offSet)
if exists(SELECT DISTINCT pos_machine as POSname
                                        FROM trx_header  WITH (index (trx_date))
                                        WHERE trxdate >=@dFromdate
                                        AND trxdate <  @dTodate
                                        AND ((site_id in (@site)) or (-1 in (@site)))
                                        UNION 
                                        SELECT distinct PosMachine from TrxPayments 
                                        WHERE PaymentDate >= @dFromdate
                                        AND PaymentDate < @dTodate
                                        AND ((site_id in (@site)) or (-1 in (@site)))
                                        UNION 
		                                SELECT distinct tp.PosMachine from TrxPayments tp,trx_header h
                                        WHERE tp.trxid=h.trxid 
		                                AND (( h.site_id IN (@site)) OR (-1 IN ( @site )))						
                                        AND trxdate >= @dFromdate 
                                        AND trxdate <  @dTodate
                                        )
                                        SELECT DISTINCT pos_machine as POSname
                                        from trx_header  WITH (index (trx_date))
                                        WHERE trxdate >= @dFromdate
                                        AND trxdate <  @dTodate
                                        AND ((site_id in (@site)) or (-1 in (@site)))
                                        UNION 
                                        SELECT distinct PosMachine from TrxPayments 
                                        WHERE PaymentDate >= @dFromdate
                                        AND PaymentDate <  @dTodate
                                        AND ((site_id in (@site)) or (-1 in (@site)))
                                        UNION 
		                                SELECT distinct tp.PosMachine  from TrxPayments tp,trx_header h
                                        WHERE tp.trxid=h.trxid 
		                                AND ((h.site_id IN (@site)) OR (-1 IN (@site)))						
                                        AND trxdate >=  @dFromdate
                                        AND trxdate <@dTodate 
                                        else
                                        select 'No Trx' as POSname";
            }
            selectReportQuery = selectReportQuery.Replace("@site", site_id);
            SqlParameter[] selectReportParameters = new SqlParameter[4];
            selectReportParameters[0] = new SqlParameter("@role", role_id);
            selectReportParameters[1] = new SqlParameter("@fromDate", FromDate);
            selectReportParameters[2] = new SqlParameter("@toDate", ToDate);
            selectReportParameters[3] = new SqlParameter("@offSet", offset);
            DataTable posList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (posList.Rows.Count > 0)
            {
                log.LogMethodExit(posList);
                return posList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }


        /// <summary>
        /// Gets POS list
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="role_id">role_id</param>
        /// <param name="ENABLE_POS_FILTER_IN_TRX_REPORT">ENABLE_POS_FILTER_IN_TRX_REPORT</param>
        /// <param name="FromDate">FromDate</param>
        /// <param name="ToDate">ToDate</param>
        /// <param name="offset">offset</param>
        /// <param name="userId">userId</param>
        /// <returns>Returns PostList</returns>
        public DataTable GetPOSListByUserId(string site_id, int role_id, string ENABLE_POS_FILTER_IN_TRX_REPORT, DateTime FromDate, DateTime ToDate, int offset, int userId)
        {
            log.LogMethodEntry(site_id, role_id, ENABLE_POS_FILTER_IN_TRX_REPORT, FromDate, ToDate, offset, userId);
            string selectReportQuery = "";

            if (ENABLE_POS_FILTER_IN_TRX_REPORT == "Y")
                selectReportQuery = @"SELECT distinct POSname
	                                    FROM POSMachines pm, ManagementFormAccess ma, user_roles u 
	                                    WHERE pm.POSName = ma.form_name 
	                                    AND ma.main_menu = 'POS Machine'
	                                    AND ma.access_allowed = 'Y' 
	                                    AND ma.role_id = u.role_id
	                                    AND (u.role_id = @role or @role = -1)
	                                    and ((pm.site_id in (@site)) or (-1 in (@site))) ";
            else if (site_id == "-1" || site_id == null)
                selectReportQuery = @"if exists(SELECT DISTINCT pos_machine as POSname 
				                                from trx_header 
				                                WHERE trxdate >= @fromDate 
				                                AND trxdate <  @toDate
				                                and ((site_id in (@site)) or (-1 in (@site)))
	                                            and (user_id=@userid or @userid=-1)
				                                )
	                                SELECT DISTINCT pos_machine as POSname 
	                                from trx_header  
	                                WHERE trxdate >= @fromDate 
	                                AND trxdate <  @toDate
	                                and ((site_id in (@site)) or (-1 in (@site)))
                                   	and (user_id=@userid or @userid=-1)
	                                else
		                                select 'No Trx' as POSname";

            else
                selectReportQuery = @"declare @dFromDate datetime=dbo.ConvertToServerTime(@fromDate,@offSet)
declare @dToDate datetime=dbo.ConvertToServerTime(@toDate,@offSet)
if exists(SELECT DISTINCT pos_machine as POSname 
				                                from trx_header 
				                                WHERE trxdate >= @dFromDate 
				                                AND trxdate <  @dToDate
				                                and ((site_id in (@site)) or (-1 in (@site)))
	                                            and (user_id=@userid or @userid=-1)
				                                )
	                                SELECT DISTINCT pos_machine as POSname 
	                                from trx_header  
	                                WHERE trxdate >= @dFromDate 
	                                AND trxdate <  @dToDate
	                                and ((site_id in (@site)) or (-1 in (@site)))
                                   	and (user_id=@userid or @userid=-1)
	                                else
		                                select 'No Trx' as POSname";
            selectReportQuery = selectReportQuery.Replace("@site", site_id);
            SqlParameter[] selectReportParameters = new SqlParameter[5];
            selectReportParameters[0] = new SqlParameter("@role", role_id);
            selectReportParameters[1] = new SqlParameter("@fromDate", FromDate);
            selectReportParameters[2] = new SqlParameter("@toDate", ToDate);
            selectReportParameters[3] = new SqlParameter("@offSet", offset);
            selectReportParameters[4] = new SqlParameter("@userid", userId);
            DataTable categoryList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (categoryList.Rows.Count > 0)
            {
                log.LogMethodExit(categoryList);
                return categoryList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        ///Returns the Purchase Order types 
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <returns></returns>
        public DataTable GetPurchaseOrderTypes(string site_id)
        {
            log.LogMethodEntry(site_id);
            string selectReportQuery = @"SELECT DocumentTypeId,Name  FROM InventoryDocumentType 
                                        WHERE Applicability like 'PO'
                                        and (site_id in (@site_id) or -1 in (@site_id))
                                        union all 
                                        select -1, ' - All - ' 
                                        order by 1";
            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@site_id", site_id);
            DataTable locationList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (locationList.Rows.Count > 0)
            {
                log.LogMethodExit(locationList);
                return locationList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        //Ends: Added on 25-Sep-2017

        /// <summary>
        /// Gets Location list
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <returns>Returns locationList</returns>
        public DataTable GetInventoryLocations(string site_id)
        {
            log.LogMethodEntry(site_id);
            string selectReportQuery = @"select LocationID ID, name Location
                                        from Location l, locationtype lt
                                   where     (l.site_id in (SUBSTRING(@site_id, 0, PATINDEX('%,%', @site_id)) ) 
OR -1= SUBSTRING(@site_id, 1, PATINDEX('%1%', @site_id) ))
                                            and l.locationtypeid = lt.locationtypeid
                                            and locationtype <> 'Department'
                                        union all 
                                        select -1, ' - All - ' 
                                        order by 1";
            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@site_id", site_id);
            DataTable locationList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (locationList.Rows.Count > 0)
            {
                log.LogMethodExit(locationList);
                return locationList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets Pivot column list
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <returns>Returns pivotColumnList</returns>
        public DataTable GetPivotColumns(string SiteID)
        {
            log.LogMethodEntry(SiteID);
            string selectReportQuery = @"declare @pivotColumns varchar(max)
                                        set @pivotColumns = ''
                                        select @pivotColumns = @pivotColumns + ', [' + segmentname + ']'
                                           from (select segmentname 
		                                         from segment_definition
		                                         where applicableentity = 'PRODUCT' and isactive = 'Y' and (site_id in (@SiteId) or -1 in(@SiteId))) vv
                                        select @pivotColumns";
            selectReportQuery = selectReportQuery.Replace("@SiteId", SiteID);
            DataTable pivotColumnList = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);


            if (pivotColumnList.Rows.Count > 0)
            {
                log.LogMethodExit(pivotColumnList);
                return pivotColumnList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets Site list
        /// </summary>
        /// <param name="Role_id">Role_id</param>
        /// <returns>Returns siteList</returns>
        public DataTable GetAccessibleSites(int Role_id)
        {
            log.LogMethodEntry(Role_id);
            string selectReportQuery = @"select distinct s.site_name as SiteName, s.site_id as Id 
                                          from site s,ManagementFormAccess ma
                                          where ma.FunctionGUID=s.Guid and (ma.role_id = @role or -1 = @role)
                                          and ma.FunctionGroup = 'Data Access' 
                                          and ma.access_allowed = 'Y' and ma.main_menu = 'Sites'
                                          and  s.site_name!='Master' order by 2 asc";
            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@role", Role_id);
            DataTable siteList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (siteList.Rows.Count > 0)
            {
                log.LogMethodExit(siteList);
                return siteList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets Store list
        /// </summary>
        /// <param name="SiteID">SiteID</param>
        /// <returns>Returns storeList</returns>
        public DataTable GetStoreLocations(string SiteID)
        {
            log.LogMethodEntry(SiteID);
            string selectReportQuery = @"select LocationId ID, l.Name Location
                                        from location l, locationtype lt
                                        where l.locationtypeid = lt.LocationTypeId
	                                    and lt.LocationType = 'Store'
										and (l.site_id in (SUBSTRING(@site_id, 0, PATINDEX('%,%', @site_id)) ) 
                                        OR -1= SUBSTRING(@site_id, 1, PATINDEX('%1%', @site_id) ))                                        
                                        union all 
                                        select -1, ' - All - ' 
                                        order by 1";
            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@site_id", SiteID);
            DataTable storeList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (storeList.Rows.Count > 0)
            {
                log.LogMethodExit(storeList);
                return storeList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets report Open to buy report data
        /// </summary>
        /// <param name="profile_id">profile_id</param>
        /// <returns>Returns dtData</returns>
        public DataTable GetOpenToBuyReportQuery(string profile_id)
        {
            log.LogMethodEntry(profile_id);
            string selectReportQuery = @"select machine_name, machine_id 
                                        from machines m, games g, game_profile gp 
                                        where gp.game_profile_id in " + profile_id +
                                            @" and gp.game_profile_id = g.game_profile_id
                                            and g.game_id = m.game_id  ";
            DataTable dtData = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);

            if (dtData.Rows.Count > 0)
            {
                log.LogMethodExit(dtData);
                return dtData;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets Segment list
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <returns>Returns segmentList</returns>
        public DataTable GetSegments(string site_id)
        {
            log.LogMethodEntry(site_id);
            string selectReportQuery = @"select sd.SegmentDefinitionId, segmentname, sm.DataSourceType, sm.DataSourceEntity, sm.DataSourceColumn, SegmentDefinitionSourceId
                                        from Segment_Definition sd, Segment_Definition_Source_Mapping sm
                                            --left outer join Segment_Definition_Source_Values dsv on sm.SegmentDefinitionSourceId = dsv.SegmentDefinitionSourceId and dsv.IsActive = 'Y'
                                        where sd.SegmentDefinitionId = sm.SegmentDefinitionId
	                                        and sd.isactive = 'Y'
	                                        and sm.isactive = 'Y' 
                                            and (sd.site_id in (@site_id) or   -1 in (@site_id)) 
                                        order by sd.SequenceOrder ";
            selectReportQuery = selectReportQuery.Replace("@site_id", site_id);
            DataTable segmentList = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);

            if (segmentList.Rows.Count > 0)
            {
                log.LogMethodExit(segmentList);
                return segmentList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets SegmentDefinitionSourceValue list
        /// </summary>
        /// <param name="SegmentDefinitionId">SegmentDefinitionId</param>
        /// <returns>Returns segmentDefinitionSourceValueList</returns>
        public DataTable GetSegmentDefinitionSourceValues(int SegmentDefinitionId)
        {
            log.LogMethodEntry(SegmentDefinitionId);
            string selectReportQuery = @"select -1 segmentdefinitionsourcevalueid, '-All-' listvalue, '-All-' description
                                        union all
                                        select segmentdefinitionsourcevalueid, listvalue, isnull(listvalue, '') + ' - ' + isnull(description, '') description
                                        from Segment_Definition_Source_Values sv, Segment_Definition_Source_Mapping sm
                                        where sv.SegmentDefinitionSourceId = sm.SegmentDefinitionSourceId
	                                        and sm.SegmentDefinitionId = @SegmentDefinitionId
                                            and sv.isactive = 'Y'
                                        order by 1 ";
            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@SegmentDefinitionId", SegmentDefinitionId);
            DataTable segmentDefinitionSourceValuesList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (segmentDefinitionSourceValuesList.Rows.Count > 0)
            {
                log.LogMethodExit(segmentDefinitionSourceValuesList);
                return segmentDefinitionSourceValuesList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets SegmentDefinitionSourceValue list
        /// </summary>
        /// <param name="SegmentDefinitionSourceId">SegmentDefinitionSourceId</param>
        /// <returns>Returns segmentDefinitionSourceValueList</returns>
        public DataTable GetSegmentDefinitionSourceValuesDBQuery(int SegmentDefinitionSourceId)
        {
            log.LogMethodEntry(SegmentDefinitionSourceId);
            string selectReportQuery = @"select dbquery 
                                         from Segment_Definition_Source_Values 
                                         where SegmentDefinitionSourceId = @SegmentDefinitionSourceId and isactive = 'Y' ";
            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@SegmentDefinitionSourceId", SegmentDefinitionSourceId);
            DataTable segmentDefinitionSourceValuesList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (segmentDefinitionSourceValuesList.Rows.Count > 0)
            {
                log.LogMethodExit(segmentDefinitionSourceValuesList);
                return segmentDefinitionSourceValuesList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets Custom report data
        /// </summary>
        /// <param name="DBQuery">DBQuery</param>
        /// <param name="selectReportParameters">selectReportParameters</param>
        /// <returns>Returns segmentDefinitionSourceValueList</returns>
        public DataTable GetCustomReportData(string DBQuery, params SqlParameter[] selectReportParameters)
        {
            log.LogMethodEntry(DBQuery, selectReportParameters);
            DataTable customReportData = dataAccessHandler.executeSelectQuery(DBQuery, selectReportParameters, sqlTransaction);

            if (customReportData.Rows.Count > 0)
            {
                log.LogMethodExit(customReportData);
                return customReportData;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets OpenToBuy report data.
        /// </summary>
        /// <param name="strSelect">strSelect</param>
        /// <param name="strFrom">strFrom</param>
        /// <param name="strWhere">strWhere</param>
        /// <param name="strGroupBy">strGroupBy</param>
        /// <param name="fromdate">fromdate</param>
        /// <param name="toDate">toDate</param>
        /// <param name="Site">Site</param>
        /// <param name="CurrencySymbol">CurrencySymbol</param>
        /// <returns>Returns selectReportQuery</returns>
        public string OpenToBuyReportQuery(string strSelect, string strFrom, string strWhere, string strGroupBy, DateTime fromdate, DateTime toDate, string Site, string CurrencySymbol)
        {
            log.LogMethodEntry(strSelect, strFrom, strWhere, strGroupBy, fromdate, toDate, Site, CurrencySymbol);
            string selectReportQuery = (strSelect != "" ? "select " + strSelect + ", " : "select p.code Item, ") + @"sum((i.quantity +  
                                        isnull(-1 * Receipt.quantity_open, 0) +  
                                        isnull(-1 * Adjust.quantity_open, 0)  +  
                                        isnull(-1 * sold.quantity_open, 0) )) Opening_Balance, 
	                                    cast(sum((i.quantity +  
                                        isnull(-1 * Receipt.quantity_open, 0) +  
                                        isnull(-1 * Adjust.quantity_open, 0)  +  
                                        isnull(-1 * sold.quantity_open, 0) )*p.SalePrice) as numeric(18,3)) [Opening Value (in " + CurrencySymbol + @")],
	                                    cast(sum(Receipt.quantity) as numeric(18,2)) Received,
							            cast(sum(Receipt.quantity * p.SalePrice) as numeric(18,2)) [Received Value (in " + CurrencySymbol + @")],
	                                    cast(sum(Adjust.quantity) as numeric(18,2)) Adjusted,
	                                    sum(Adjust.quantity * p.SalePrice) [Adjusted Value (in " + CurrencySymbol + @")],
	                                    cast(sum(sold.Markdown_Value) as numeric(18,2)) [MarkDown Value (in " + CurrencySymbol + @")],
	                                    cast(sum(sold.quantity) as numeric(18,2)) Sold,
	                                    cast(sum(sold.SalesVolume) as numeric(18,2)) SalesVolume,
							            cast(sum(
										            (i.quantity +   
										            isnull(-1 * Receipt.quantity_open, 0) +  
										            isnull(-1 * Adjust.quantity_open, 0)  +  
										            isnull(-1 * sold.quantity_open, 0) +
										            isnull(Receipt.quantity, 0) +  
										            isnull(Adjust.quantity, 0)  +  
										            isnull(sold.quantity, 0))) as numeric(18,2)) [Closing Balance],
																            cast(sum(
										            (i.quantity +   
											            isnull(-1 * Receipt.quantity_open, 0) +  
											            isnull(-1 * Adjust.quantity_open, 0)  +  
											            isnull(-1 * sold.quantity_open, 0) +
											            isnull(Receipt.quantity, 0) +  
											            isnull(Adjust.quantity, 0)  +  
											            isnull(sold.quantity, 0))*p.SalePrice) as numeric(18,2)) [Closing Value (in " + CurrencySymbol + @")],
											            cast((sum(sold.SalesVolume) - sum(p.Cost*sold.quantity))/sum(case when sold.SalesVolume = 0 then null else sold.SalesVolume end) * 100 as numeric(18,2)) [Gross Margin %]
                        from " + strFrom +
                                    @" Product p
	              JOIN Inventory i on i.ProductId = p.ProductId --and i.LocationId = p.DefaultLocationId
                  and i.site_id in(@SiteId) or -1 in (@SiteId)
	              LEFT OUTER JOIN (SELECT rl.ProductId
	                                      , sum(case when ReceiveDate <= @toDate then isnull(quantity,0) else 0 end) quantity 
	                                      , sum(case when ReceiveDate <= @toDate then isnull(quantity,0) * isnull(p.SalePrice,0) else 0 end) RMB_Quantity 
	                                      ,sum(isnull(quantity,0)) quantity_open
							              ,sum(isnull(quantity,0)*p.SalePrice) RMB_Open
	                                 FROM PurchaseOrderReceive_Line rl, Receipt R, product P
	                                WHERE RL.ReceiptId = r.ReceiptId
						              and ReceiveDate >= @fromdate
						              and rl.ProductId = p.ProductId
						              and IsReceived ='Y'
                                      and (R.site_id in (@SiteId) or (-1 in (@SiteId)))
						              group by rl.ProductId) Receipt on Receipt.ProductId = p.ProductId
	              LEFT OUTER JOIN (select ia.productid, 
							            sum(case when timestamp <= @toDate then isnull(adjustmentquantity, 0) else 0 end) quantity, 
							            sum(case when timestamp <= @toDate then isnull(adjustmentquantity, 0) else 0 end) RMB_Quantity, 
							            sum(isnull(adjustmentquantity, 0)) quantity_open ,
						                sum(isnull(adjustmentquantity, 0)* isnull(p.SalePrice,0)) RMB_open 
						            from inventoryadjustments ia, product p
						            where adjustmenttype in ('Adjustment', 'Trade-In') 
						            and timestamp >= @fromDate
						            and p.ProductId = ia.ProductId
                                    and (p.site_id in (@SiteId) or (-1 in (@SiteId)))
						            group by ia.productid) Adjust on Adjust.productid = P.productid
	              LEFT OUTER JOIN (select r.productid, 
							            sum(case when r.TrxDate <= @toDate then isnull(r.quantity,0)*-1 else 0 end) quantity, 
							            cast(sum(case when r.TrxDate <= @toDate 
							                     then isnull(r.quantity,0) *-1* isnull(case when r.quantity < 0 then -1 * l.amount else l.amount end * case when isnull(Disc.DiscPerc,0) > 0 then (1 - isnull(Disc.DiscPerc,0)/100) else 1 end,0) 
									             else 0 end) as numeric(18,2)) SalesVolume, 
							            sum(case when r.TrxDate <= @toDate then isnull(r.quantity,0)*-1 * isnull(p.SalePrice,0) else 0 end) RMB_quantity, 
							            sum(isnull(r.quantity,0)*-1) quantity_open,
						                sum(isnull(r.quantity,0) * isnull(p.SalePrice,0) * -1) RMB_open,
							            cast(sum(case when r.TrxDate <= @toDate then isnull(l.amount * case when isnull(Disc.DiscPerc,0) > 0 then (1 - isnull(Disc.DiscPerc,0)/100) else 0 end,0)else 0 end)as numeric(18,2)) Markdown_Value
						            from InventoryTransaction r , 
						                 Product P, 
							             trx_header h, 
							             trx_lines l
						                 left outer join (select sum(discountPercentage) DiscPerc, trxid, lineId 
							                                from trxdiscounts 
											              group by trxid, lineid) disc on l.trxid = disc.trxid and l.lineid=disc.lineid
						            where r.TrxDate  >= @fromDate
						            and r.ParafaitTrxId = h.trxid
						            and r.ParafaitTrxId = l.trxid
						            and r.LineId = l.LineId
						            and r.ProductId = p.ProductId
                                    and (r.site_id in (@SiteId) or (-1 in (@SiteId)))
						            group by r.productid) sold 
						            on sold.productid = i.productid   "
                                + strWhere + " "
                                + strGroupBy;

            log.LogMethodExit(selectReportQuery);
            return selectReportQuery;
        }

        /// <summary>
        ///  Gets query output
        /// </summary>
        /// <param name="Query">Query</param>
        /// <param name="parameters">parameters</param>
        /// <returns>Returns dtData</returns>
        public DataTable GetQueryOutput(string Query, List<SqlParameter> parameters = null)
        {
            log.LogMethodEntry(Query, parameters);
            DataTable dtData;
            if (parameters == null)
                dtData = dataAccessHandler.executeSelectQuery(Query, null);
            else
                dtData = dataAccessHandler.executeSelectQuery(Query, parameters.ToArray(), sqlTransaction);

            if (dtData.Rows.Count > 0)
            {
                log.LogMethodExit(dtData);
                return dtData;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        ///Gets InventoryAgingReport Query
        /// </summary>
        /// <param name="strSelect">strSelect</param>
        /// <param name="strFrom">strFrom</param>
        /// <param name="strWhere">strWhere</param>
        /// <param name="fromdate">fromdate</param>
        /// <param name="toDate">toDate</param>
        /// <param name="Site">Site</param>
        /// <param name="CurrencySymbol">CurrencySymbol</param>
        /// <param name="vendorWhere">vendorWhere</param>
        /// <returns>Returns selectReportQuery</returns>
        public string InventoryAgingReportQuery(string strSelect, string strFrom, string strWhere, DateTime fromdate, DateTime toDate, string Site, string CurrencySymbol, string vendorWhere)
        {
            log.LogMethodEntry(strSelect, strFrom, strWhere, fromdate, toDate, Site, CurrencySymbol, vendorWhere);
            string selectReportQuery = @"declare @initialDate datetime = (select DateAdd(MI,@OffSet*-1,min(trxdate)) from trx_header where (site_id in (@SiteId) or (-1 in (@SiteId)))) " +
                        (strSelect != "" ? "select " + strSelect + ", " : "select ") +
                           @" Convert(Varchar, p.Code) [Product Code],P.Description [Product Description],
	                           ISNULL(a.PurchaseDate, ISNULL(ADJ.Timestamp,@initialDate)) PurchaseDate,
	                       i.quantity, p.cost, a.inventoryPrice,
                           vn.Name [Vendor Name],p.PriceInTickets [Price In Tickets],p.ItemMarkupPercent [Product Markup %],
                           vn.VendorMarkupPercent [Vendor Markup %], c.Name [category Name],
	                       CASE 
	                       WHEN DATEDIFF (DD,ISNULL(a.receiveDate, ISNULL(ADJ.Timestamp,@initialDate)), @todate) between 0 and 30
	                       THEN cast(I.Quantity * (case when a.InventoryPrice = 0 or a.InventoryPrice is null then p.cost else a.InventoryPrice end) as numeric(18,2))
	                       ELSE 0
	                        END [0-30],
	                       CASE 
	                       WHEN DATEDIFF (DD,ISNULL(a.receiveDate, ISNULL(ADJ.Timestamp,@initialDate)), @todate) between 31 and 60
	                       THEN cast(I.Quantity * (case when a.InventoryPrice = 0 or a.InventoryPrice is null then p.cost else a.InventoryPrice end) as numeric(18,2))
	                       ELSE 0
	                        END [31-60],
	                       CASE 
	                       WHEN DATEDIFF (DD,ISNULL(a.receiveDate, ISNULL(ADJ.Timestamp,@initialDate)), @todate) between 61 and 90
	                       THEN cast(I.Quantity * (case when a.InventoryPrice = 0 or a.InventoryPrice is null then p.cost else a.InventoryPrice end) as numeric(18,2))
	                       ELSE 0
	                        END [61-90],
	                       CASE 
	                       WHEN DATEDIFF (DD,ISNULL(a.receiveDate, ISNULL(ADJ.Timestamp,@initialDate)), @todate) between 91 and 120
	                       THEN cast(I.Quantity * (case when a.InventoryPrice = 0 or a.InventoryPrice is null then p.cost else a.InventoryPrice end) as numeric(18,2))
	                       ELSE 0
	                        END [91-120],
	                       CASE 
	                       WHEN DATEDIFF (DD,ISNULL(a.receiveDate, ISNULL(ADJ.Timestamp,@initialDate)), @todate) between 121 and 150
	                       THEN cast(I.Quantity * (case when a.InventoryPrice = 0 or a.InventoryPrice is null then p.cost else a.InventoryPrice end) as numeric(18,2))
	                       ELSE 0
	                        END [121-150],
	                       CASE 
	                       WHEN DATEDIFF (DD,ISNULL(a.receiveDate, ISNULL(ADJ.Timestamp,@initialDate)), @todate) between 151 and 180
	                       THEN cast(I.Quantity * (case when a.InventoryPrice = 0 or a.InventoryPrice is null then p.cost else a.InventoryPrice end) as numeric(18,2))
	                       ELSE 0
	                        END [151-180],
	                       CASE 
	                       WHEN DATEDIFF (DD,ISNULL(a.receiveDate, ISNULL(ADJ.Timestamp,@initialDate)), @todate) between 181 and 210
	                       THEN cast(I.Quantity * (case when a.InventoryPrice = 0 or a.InventoryPrice is null then p.cost else a.InventoryPrice end) as numeric(18,2))
	                       ELSE 0
	                        END [181-210],
	                       CASE 
	                       WHEN DATEDIFF (DD,ISNULL(a.receiveDate, ISNULL(ADJ.Timestamp,@initialDate)), @todate) between 211 and 240
	                       THEN cast(I.Quantity * (case when a.InventoryPrice = 0 or a.InventoryPrice is null then p.cost else a.InventoryPrice end) as numeric(18,2))
	                       ELSE 0
	                        END [211-240],
	                       CASE 
	                       WHEN DATEDIFF (DD,ISNULL(a.receiveDate, ISNULL(ADJ.Timestamp,@initialDate)), @todate) between 241 and 270
	                       THEN cast(I.Quantity * (case when a.InventoryPrice = 0 or a.InventoryPrice is null then p.cost else a.InventoryPrice end) as numeric(18,2))
	                       ELSE 0
	                        END [241-270],
	                       CASE 
	                       WHEN DATEDIFF (DD,ISNULL(a.receiveDate, ISNULL(ADJ.Timestamp,@initialDate)), @todate) between 271 and 300
	                       THEN cast(I.Quantity * (case when a.InventoryPrice = 0 or a.InventoryPrice is null then p.cost else a.InventoryPrice end) as numeric(18,2))
	                       ELSE 0
	                        END [271-300],
	                       CASE 
	                       WHEN DATEDIFF (DD,ISNULL(a.receiveDate, ISNULL(ADJ.Timestamp,@initialDate)), @todate) between 301 and 330
	                       THEN cast(I.Quantity * (case when a.InventoryPrice = 0 or a.InventoryPrice is null then p.cost else a.InventoryPrice end) as numeric(18,2))
	                       ELSE 0
	                        END [301-330],
	                       CASE 
	                       WHEN DATEDIFF (DD,ISNULL(a.receiveDate, ISNULL(ADJ.Timestamp,@initialDate)), @todate) between 331 and 360
	                       THEN cast(I.Quantity * (case when a.InventoryPrice = 0 or a.InventoryPrice is null then p.cost else a.InventoryPrice end) as numeric(18,2))
	                       ELSE 0
	                        END [331-360],	
                           CASE 
	                       WHEN DATEDIFF (DD,ISNULL(a.receiveDate, ISNULL(ADJ.Timestamp,@initialDate)), @todate) > 360
	                       THEN cast(I.Quantity * (case when a.InventoryPrice = 0 or a.InventoryPrice is null then p.cost else a.InventoryPrice end) as numeric(18,2))
	                       ELSE 0
	                        END [>360]	
             from " + strFrom +
                        @" (SELECT i.Quantity - isnull(Receipt.quantity_open,0) - isnull(Adjust.quantity_open,0)
	                                            + isnull(sold.quantity_Open,0) * -1 Quantity, 
			                        i.ProductId
	                        FROM INVENTORY I
		                    LEFT OUTER JOIN (SELECT rl.ProductId
	                                                ,sum(isnull(quantity,0)) quantity_open
	                                            FROM PurchaseOrderReceive_Line rl, Receipt R, product P
	                                        WHERE RL.ReceiptId = r.ReceiptId
						                        and ReceiveDate > @Todate
						                        and rl.ProductId = p.ProductId
						                        and IsReceived ='Y'
                                                and (R.site_id in (@SiteId) or (-1 in (@SiteId)))
						                        group by rl.ProductId) Receipt on Receipt.ProductId = I.ProductId and (I.site_id in (@SiteId) or (-1 in (@SiteId)))
	                        LEFT OUTER JOIN (select ia.productid, 
							                    sum(isnull(adjustmentquantity, 0)) quantity_open 
						                    from inventoryadjustments ia, product p
						                    where adjustmenttype in ('Adjustment', 'Trade-In') 
						                    and timestamp > @Todate
						                    and p.ProductId = ia.ProductId
                                            and (ia.site_id in (@SiteId) or (-1 in (@SiteId)))
						                    group by ia.productid) Adjust on Adjust.productid = I.productid
	                        LEFT OUTER JOIN (select r.productid,
							                    sum(isnull(r.quantity,0)*-1) quantity_open
						                    from InventoryTransaction r 
						                    where r.TrxDate > @Todate
                                            and (r.site_id in (@SiteId) or (-1 in (@SiteId)))
						                        and (InventoryTransactionTypeID IS NULL
						                            OR InventoryTransactionTypeID = (select top 1 lookupValueId 
						                                                            from lookupValues lv, lookups L
															                        where lv.LookupId = l.LookupId
															                        and l.LookupName = 'INVENTORY_TRANSACTION_TYPE'
															                        and LookupValue = 'Sales'
                                                                                    and (l.site_id in (@SiteId) or (-1 in (@SiteId)))
														                        )
							                        )
						                    group by r.productid) sold 
						                    on sold.productid = i.productid) i,
	                        Product p	   
                            left outer join Vendor vn on vn.VendorId = p.DefaultVendorId   
                            left outer join 
					                    (SELECT C.CATEGORYID, ISNULL(PC.NAME,C.NAME) NAME, C.IsActive
						                    FROM CATEGORY C LEFT OUTER JOIN CATEGORY PC ON C.ParentCategoryId = PC.CategoryId
					                        WHERE C.IsActive = 'Y' and (C.site_id in (@SiteId) or (-1 in (@SiteId)))) c
						                    on p.CategoryId = c.CategoryId
				                        left outer join
				                        (select p.ProductId, prl.quantity, cast((prl.price * (1 + (tax_percentage/100))) as numeric(18,2)) InventoryPrice, 
						                        convert(date, DateAdd(MI,@OffSet*-1,r.ReceiveDate)) ReceiveDate,
						                        datediff (DD, DateAdd(MI,@OffSet*-1,receivedate), @todate) Age,
						                        convert(date, DateAdd(MI,@OffSet*-1,PO.OrderDate)) PurchaseDate,
						                        DENSE_RANK() over (partition by pRL.Productid order by r.ReceiveDate desc) rnk
					                        from product p, PurchaseOrderReceive_Line Prl, Receipt R, PurchaseOrder PO
					                        where p.ProductId = Prl.ProductId
					                        and prl.ReceiptId = R.ReceiptId
					                        and r.ReceiveDate <= @ToDate 
					                        and prl.IsReceived = 'Y'
					                        and prl.PurchaseOrderId = PO.PurchaseOrderId
					                        and PO.OrderStatus != 'Cancelled' and (PO.site_id in (@SiteId) or (-1 in (@SiteId))) ) a on p.ProductId = a.ProductId and a.rnk = 1
					                    left outer join
					                        (select ProductId, DateAdd(MI,@OffSet*-1,Timestamp) Timestamp,
						                        DENSE_RANK() over (partition by Productid order by Timestamp desc) rnk
					                        from InventoryAdjustments ia
					                        where timestamp <= @todate
					                        and remarks like 'Bulk Adjustment%'  and (ia.site_id in (@SiteId) or (-1 in (@SiteId))) ) Adj on Adj.ProductId = p.ProductId and Adj.rnk = 1  "
                                + strWhere + "  " + vendorWhere;
            log.LogMethodExit(selectReportQuery);
            return selectReportQuery;
        }

        /// <summary>
        /// Gets Top15WeeklyUnitSales report query
        /// </summary>
        /// <returns>Returns selectReportQuery</returns>
        public string Top15WeeklyUnitSalesReportQuery(string strSelect, string strFrom, string strWhere, string strGroupBy, string strOrderBy, DateTime fromdate, DateTime toDate, string Site, string CurrencySymbol)
        {
            log.LogMethodEntry(strSelect, strFrom, strWhere, strGroupBy, strOrderBy, fromdate, toDate, Site, CurrencySymbol);
            string selectReportQuery = (strSelect != "" ? "select top 15 " + strSelect + ", " : "select top 15 p.code Item, ") + @"sum(i.Quantity) OnHand
	                   ,sum(ISNULL(PO.Quantity, 0)) OnOrder
	                   ,sum(iT.quantity) UnitSales
	                   ,round(sum(it.SalePrice), 2)[TotalSales(in " + CurrencySymbol + @")]
                       from " + strFrom +
                        @" Product p
	                      JOIN Inventory i on i.ProductId = p.ProductId and i.LocationId = p.DefaultLocationId and (p.site_id in (@SiteId) or (-1 in (@SiteId)))
	                      JOIN (select productId, LocationId, sum(quantity) quantity, sum(Saleprice) SalePrice
	                              from InventoryTransaction iT 
			                     where trxdate between @FromDate and @ToDate and (iT.site_id in (@SiteId) or (-1 in (@SiteId)))
	                             group by productId, locationId) it on IT.ProductId = p.ProductId and it.LocationId = p.DefaultLocationId
	                      LEFT OUTER JOIN (select POL.ProductId, POL.Quantity, PO.OrderNumber , POL.CancelledDate, PO.OrderDate
	                                         from PurchaseOrder PO, PurchaseOrder_Line POL 
						                    WHERE POL.purchaseOrderId = PO.PurchaseOrderId
						                      AND PO.OrderStatus != 'Received'
						                      and po.OrderStatus != 'CANCELLED'
						                      and POL.CancelledDate is null and (po.site_id in (@SiteId) or (-1 in (@SiteId)))) po on po.ProductId = p.ProductId  "
                        + strWhere + " "
                        + strGroupBy + " "
                        + strOrderBy;

            log.LogMethodExit(selectReportQuery);
            return selectReportQuery;
        }

        /// <summary>
        /// Gets PurchaseOrder report query.
        /// </summary>
        /// <param name="strSelect">strSelect</param>
        /// <param name="s_line">s_line</param>
        /// <param name="categoryWhere">categoryWhere</param>
        /// <param name="strPONumber">strPONumber</param>
        /// <param name="poType">poType</param>
        /// <param name="vendor">vendor</param>
        /// <param name="creditCashPO">creditCashPO</param>
        /// <param name="strpivot">strpivot</param>
        /// <param name="segmentCategory">segmentCategory</param>
        /// <param name="strOrderBy">strOrderBy</param>
        /// <param name="Site">Site</param>
        /// <returns>Returns segmentDefinitionSourceValueList</returns>
        public string PurchaseOrderReportQuery(string strSelect, string s_line, string categoryWhere, string strPONumber, string poType, string vendor, string creditCashPO, string strpivot, string segmentCategory, string strOrderBy, string Site)
        {
            log.LogMethodEntry(strSelect, s_line, categoryWhere, strPONumber, poType, vendor, creditCashPO, strpivot, segmentCategory, strOrderBy, Site);
            string selectReportQuery = strSelect + @" select OrderNumber [Order Number],OrderStatus [Order Status], isnull(iscreditpo, 'N') [Is Credit PO?], isnull(dt.name, 
                              'Regular Purchase Order') 'Order Type',DateAdd(MI,@OffSet*-1,OrderDate) [Order Date],ItemCode [Item Code],p.Description, UOM ,
                              c.Name category, segmentname, valuechar,
                               Quantity, D.UnitPrice [Unit Price], TaxAmount [Tax Amount],SubTotal [Sub Total],V.Name as [Vendor Name], V.TaxRegistrationNumber as 'VAT/Tax No',
                               OrderRemarks 'Order Remarks',DateAdd(MI,@OffSet*-1,ReceivedDate) 'Received Date',ReceiveRemarks 'Receive Remarks',  DateAdd(MI,@OffSet*-1, h.CancelledDate) 'Order Cancelled Date', 
                               DateAdd(MI,@OffSet*-1,d.CancelledDate) 'Line Cancelled Date',
                               d.PriceInTickets  [PriceInTickets]	,
							    (case when (isnull(d.PriceInTickets,0)*@ticketCost) =0 then 0 else
							    (isnull(d.PriceInTickets,0)*@ticketCost)- 
								(
									  (isnull(d.UnitPrice,0)+isnull(d.TaxAmount,0)+isnull(d.UnitLogisticsCost,0))/(isnull(d.PriceInTickets,0)*@ticketCost)   
								)
						        end )MarkUp
                                 from PurchaseOrder H left outer join inventoryDocumentType dt on dt.documentTypeId = H.documentTypeId, PurchaseOrder_Line D, Vendor V, product p 
                                 left outer join Category c on p.categoryid = c.categoryid 
                                 left outer join uom u on p.uomid = u.uomid 
                                 left outer join segmentdataview sdv on sdv.segmentcategoryid = p.segmentcategoryid 
                                 
                                 where " + s_line + @" H.PurchaseOrderId = D.PurchaseOrderId AND H.VendorId = V.VendorId 
                                and p.productId = D.productId  and (H.site_id in (@SiteId) or (-1 in (@SiteId)))
                                AND ((H.ReceivedDate >= @fromdate AND H.ReceivedDate < @todate) 
                                     or (H.OrderDate >= @fromdate AND H.OrderDate < @todate)) " +
                                     categoryWhere +
                                     strPONumber +
                                     poType + " " +
                                     vendor +
                                     creditCashPO +
                                     strpivot +
                                     segmentCategory + //Updated sequence of string in query 11-Aug-2016 
                                     strOrderBy;
            log.LogMethodExit(selectReportQuery);
            return selectReportQuery;
        }

        /// <summary>
        /// Gets ReceivedReportQuery report query
        /// </summary>
        /// <param name="pivotColumns">pivotColumns</param>
        /// <param name="categoryWhere">categoryWhere</param>
        /// <param name="poType">poType</param>
        /// <param name="recvLocation">recvLocation</param>
        /// <param name="vendor">vendor</param>
        /// <param name="creditCashPO">creditCashPO</param>
        /// <param name="strpivot">strpivot</param>
        /// <param name="segmentCategory">segmentCategory</param>
        /// <param name="strOrderby">strOrderby</param>
        /// <param name="Site">Site</param>
        /// <returns>Returns selectReportQuery</returns>
        public string ReceivedReportQuery(string pivotColumns, string categoryWhere, string poType, string recvLocation, string vendor, string creditCashPO, string strpivot, string segmentCategory, string strOrderby, string Site)
        {
            log.LogMethodEntry(pivotColumns, categoryWhere, poType, recvLocation, vendor, creditCashPO, strpivot, segmentCategory, strOrderby, Site);
            string selectReportQuery = "select  [Order Number], [Vendor Name], [Order Type], [Is Credit PO?], [Recv Location],ProductId,Code,Description, GRN, UOM, Category" +
                             pivotColumns +
                            @",case when isnull(BarCode, '') != '' then substring(BarCode, 3, len(BarCode)) else BarCode end BarCode, [Order Quantity], [Receive Quantity], [Unit Price], [Sale Price], Amount, 
                               DateAdd(MI,@OffSet*-1,[Order Date]) [Order Date], DateAdd(MI,@OffSet*-1,[Received Date]) [Received Date],[Receive Remarks] ,PriceInTickets [Price In Tickets]
                                from(
		                                select  H.OrderNumber [Order Number], V.Name as [Vendor Name], dt.name [Order Type], iscreditPO [Is Credit PO?], GRN, loc.name [Recv Location], D.ProductId,p.Code,D.Description, UOM, c.Name Category,
													(select case cnt when 0 then '' else stuff((select ' | '+ cast(BarCode as nvarchar(250))
														FROM ProductBarcode
														WHERE productid = p.ProductId and isactive = 'Y' and (p.site_id in (@SiteId) or (-1 in (@SiteId)))
														group by BarCode
														FOR XML PATH('')
														
														)
														, 1, 3, '') end
	 from (select count(*) cnt
		   from ProductBarcode 
		   where ProductId = p.ProductId and isactive = 'Y' and  (p.site_id in (@SiteId) or (-1 in (@SiteId)))
		   group by productid)v) BarCode, segmentname, valuechar, pol.quantity [Order Quantity],
                                           p.PriceInTickets,
                                           D.Quantity [Receive Quantity], pol.UnitPrice [Unit Price], D.Quantity * pol.UnitPrice Amount, pt.price [Sale Price], H.OrderDate [Order Date],H.ReceivedDate [Received Date],r.remarks [Receive Remarks]
										from PurchaseOrder H left outer join inventorydocumenttype dt on H.documentTypeId = dt.documentTypeId, receipt r, PurchaseOrderReceive_Line D left outer join location loc on D.LocationId = loc.LocationId, Vendor V, purchaseOrder_line pol, product p left outer join category c on p.categoryid = c.categoryid
                                            left outer join uom u on u.uomid = p.uomid
                                            left outer join segmentdataview sdv on p.segmentcategoryid = sdv.segmentcategoryid
											left outer join products pt on p.ManualProductId = pt.product_id
                                        where D.PurchaseOrderId =  H.PurchaseOrderId
											    AND H.ReceivedDate >= @fromdate AND H.ReceivedDate < @todate
                                                and (H.site_id in (@SiteId) or (-1 in (@SiteId)))
											    and p.productId = D.productId 
											    and V.vendorid = H.vendorid 
                                                and D.ReceiptId = r.ReceiptId
                                                and r.purchaseOrderId = H.PurchaseOrderId
											    and D.PurchaseOrderLineId = pol.PurchaseOrderLineId " +
                                        categoryWhere +
                                        poType + " " +
                                        vendor +
                                        recvLocation +
                                        creditCashPO +
                                @")v " +
                                strpivot +
                                segmentCategory +
                                strOrderby;
            log.LogMethodExit(selectReportQuery);
            return selectReportQuery;
        }

        /// <summary>
        /// Gets DepartmentSellingYTDReport report Query
        /// </summary>
        /// <param name="strSelect">strSelect</param>
        /// <param name="strFrom">strFrom</param>
        /// <param name="strWhere">strWhere</param>
        /// <param name="strGroupBy">strGroupBy</param>
        /// <param name="strOrderBy">strOrderBy</param>
        /// <param name="fromdate">fromdate</param>
        /// <param name="toDate">toDate</param>
        /// <param name="Site">Site</param>
        /// <param name="CurrencySymbol">CurrencySymbol</param>
        /// <returns>Returns selectReportQuery</returns>
        public string DepartmentSellingYTDReportQuery(string strSelect, string strFrom, string strWhere, string strGroupBy, string strOrderBy, DateTime fromdate, DateTime toDate, string Site, string CurrencySymbol)
        {
            log.LogMethodEntry(strSelect, strFrom, strWhere, strGroupBy, strOrderBy, fromdate, toDate, Site, CurrencySymbol);
            string selectReportQuery = (strSelect != "" ? "select " + strSelect + ", " : "select p.code Item, ") + @"sum(MTDTY.quantity) [MTD UnitSales]
	                                   ,round(sum(MTDTY.SalePrice),2) [MTD TotalSales]
	                                   ,cast((sum(MTDTY.SalesVolume) - sum(p.Cost*MTDTY.quantity))/sum(case when MTDTY.SalesVolume = 0 then null else MTDTY.SalesVolume end) * 100 as numeric(18,2)) [Gross Margin % MTD]
                                       ,sum(MTDLY.quantity) [MTD UnitSales LY]
	                                   ,round(sum(MTDLY.SalePrice),2) [MTD TotalSales LY]
                                       ,sum(YTDTY.quantity) [YTD UnitSales]
	                                   ,round(sum(YTDTY.SalePrice),2) [YTD TotalSales]
                                       ,cast((sum(YTDTY.SalesVolume) - sum(p.Cost*YTDTY.quantity))/sum(case when YTDTY.SalesVolume = 0 then null else YTDTY.SalesVolume end) * 100 as numeric(18,2)) [Gross Margin % YTD]
                                       ,sum(YTDLY.quantity) [YTD UnitSales LY]
	                                   ,round(sum(YTDLY.SalePrice),2) [YTD TotalSales LY] 
                             from " + strFrom +
                                        @" Product p
	                                      left outer JOIN (select productId, LocationId, sum(it.quantity) quantity, sum(Saleprice) SalePrice
	                                                              , cast(sum(isnull(it.quantity,0) * isnull(case when it.quantity < 0 then -1 * l.amount else l.amount end * case when isnull(Disc.DiscPerc,0) > 0 then (1 - isnull(Disc.DiscPerc,0)/100) else 1 end,0) 
									                                     ) as numeric(18,2)) SalesVolume
	                                              from InventoryTransaction iT , trx_lines l
						                                         left outer join (select sum(discountPercentage) DiscPerc, trxid, lineId 
							                                                        from trxdiscounts 
											                                      group by trxid, lineid) disc on l.trxid = disc.trxid and l.lineid=disc.lineid
			                                     where trxdate between dateadd(hh,6,convert(datetime,dateadd(day,1-day(@ToDate),convert(date,@ToDate))))
		                                                          AND @ToDate
						                                    and it.ParafaitTrxId = l.trxid
						                                    and it.LineId = l.LineId and (it.site_id in (@SiteId) or (-1 in (@SiteId)))
	                                             group by productId, locationId) MTDTY on MTDTY.ProductId = p.ProductId and MTDTY.LocationId = p.DefaultLocationId
	                                      left outer JOIN (select productId, LocationId, sum(quantity) quantity, sum(Saleprice) SalePrice
	                                              from InventoryTransaction iT 
			                                     where trxdate between dateadd(hh,6,convert(datetime,dateadd(day,1-day(dateadd(Year, -1, @ToDate)),convert(date,dateadd(Year, -1, @ToDate)))))
			                                                       and dateadd(Year, -1, @ToDate)
	                                             group by productId, locationId) MTDLY on MTDLY.ProductId = p.ProductId and MTDLY.LocationId = p.DefaultLocationId
	                                      left outer JOIN (select productId, LocationId, sum(IT.quantity) quantity, sum(Saleprice) SalePrice
	                                                              , cast(sum(isnull(it.quantity,0) * isnull(case when it.quantity < 0 then -1 * l.amount else l.amount end * case when isnull(Disc.DiscPerc,0) > 0 then (1 - isnull(Disc.DiscPerc,0)/100) else 1 end,0) 
									                                     ) as numeric(18,2)) SalesVolume
	                                              from InventoryTransaction iT , trx_lines l
						                                         left outer join (select sum(discountPercentage) DiscPerc, trxid, lineId 
							                                                        from trxdiscounts 
											                                      group by trxid, lineid) disc on l.trxid = disc.trxid and l.lineid=disc.lineid
			                                     where trxdate between case when datepart(mm,@ToDate) between 1 and 3
														                                    then  dateadd(hh,6,dateadd(mm,3,DATEADD(yy, DATEDIFF(yy,0,@ToDate)-1, 0)))
														                                    else dateadd(hh,6,dateadd(mm,3,DATEADD(yy, DATEDIFF(yy,0,@ToDate), 0)))
														                                    end  and @ToDate
						                                    and it.ParafaitTrxId = l.trxid
						                                    and it.LineId = l.LineId and (it.site_id in (@SiteId) or (-1 in (@SiteId)))
	                                             group by productId, locationId) YTDTY on YTDTY.ProductId = p.ProductId and YTDTY.LocationId = p.DefaultLocationId
	                                      left outer JOIN (select productId, LocationId, sum(quantity) quantity, sum(Saleprice) SalePrice
	                                              from InventoryTransaction iT 
			                                     where trxdate between case when datepart(mm,dateadd(Year, -1, @ToDate)) between 1 and 3
									                                    then  dateadd(hh,6,dateadd(mm,3,DATEADD(yy, DATEDIFF(yy,0,dateadd(Year, -1, @ToDate))-1, 0)))
									                                    else dateadd(hh,6,dateadd(mm,3,DATEADD(yy, DATEDIFF(yy,0,dateadd(Year, -1, @ToDate)), 0)))
									                                    end and dateadd(Year, -1, @ToDate)
	                                             group by productId, locationId) YTDLY on YTDLY.ProductId = p.ProductId and YTDLY.LocationId = p.DefaultLocationId  "
                                        + strWhere + " "
                                        + strGroupBy + " "
                                        + strOrderBy;
            log.LogMethodExit(selectReportQuery);
            return selectReportQuery;
        }

        /// <summary>
        /// Gets InventoryAdjustmentsReportData report data
        /// </summary>
        /// <param name="strSelect">strSelect</param>
        /// <param name="prodTypeWhere">prodTypeWhere</param>
        /// <param name="categoryWhere">categoryWhere</param>
        /// <param name="strpivot">strpivot</param>
        /// <param name="segmentCategory">segmentCategory</param>
        /// <param name="strOrderby">strOrderby</param>
        /// <param name="LocationID">LocationID</param>
        /// <param name="vendorWhere">vendorWhere</param>
        /// <returns>Returns selectReportQuery</returns>
        public string InventoryAdjustmentsReportQuery(string strSelect, string prodTypeWhere, string categoryWhere, string strpivot, string segmentCategory, string strOrderby, string LocationID, string vendorWhere)
        {
            log.LogMethodEntry(strSelect, prodTypeWhere, categoryWhere, strpivot, segmentCategory, strOrderby, LocationID, vendorWhere);
            string selectReportQuery = strSelect +
               @"select isnull((select v.lookupvalue 
		                        from lookups l, lookupvalues v
		                        where l.lookupid = v.lookupid
			                        and Lookupname = 'INVENTORY_ADJUSTMENT_TYPE'
			                        and v.lookupvalueid = inv.adjustmenttypeid), inv.AdjustmentType) Adjustment_Type,
                Code Product, Description, " +
                "c.Name category, segmentname, valuechar, AdjustmentQuantity Adj_Quantity, " +
                "fromLocation.name From_Location, tolocation.name To_Location, Cost * AdjustmentQuantity total_cost, " +
                "p.taxinclusivecost 'tax inclusive cost'," +
                 "inv.Remarks, DateAdd(MI,@OffSet*-1,timestamp) Date, userid UserId ," +
                " vn.Name vendorName, vn.VendorId,p.PriceInTickets, DateAdd(MI,@OffSet*-1,recDate.receiveDate) LastModDttm  " + //Added Column
                "from inventoryadjustments inv left outer join location tolocation " +
                "on tolocationid = tolocation.locationid " +
                ", location fromlocation, product p left outer join Category c on p.categoryid = c.categoryid " +
                  " left outer join Vendor vn on vn.VendorId = p.DefaultVendorId" +
                "  left outer join " +
                "  (select distinct invrh.receiveDate, invrl.productid pid " +
                    " from PurchaseOrderReceive_Line invrl, " +
                    " Receipt invrh " +
                    "where " +
                    " invrl.receiptId = invrh.receiptId " +
                    " and invrh.receiveDate = (SELECT Max(receiveDate) " +
                    " FROM Receipt invrhin, " +
                    " PurchaseOrderReceive_Line invrlin " +
                    " WHERE invrhin.receiptId = invrlin.receiptId " +
                    " and invrlin.productId = invrl.productid and (invrhin.site_id in (@SiteId) or (-1 in (@SiteId))) )" +
                    " )recDate on  recDate.pid = p.ProductId " +
                " left outer join SegmentDataView sdv on p.SegmentCategoryId = sdv.SegmentCategoryId " +
                "where p.productid = inv.productid " +
                "and (@locationid =-1 or (tolocationid = @locationid or adjustmentType = 'Adjustment' and fromLocationId = @locationid)) " +
                "  and  (inv.site_id in (@SiteId) or (-1 in (@SiteId))) and fromlocationid = fromlocation.locationid " +
                "and timestamp >= @fromdate and timestamp <@todate " +
                prodTypeWhere +
                vendorWhere +
                categoryWhere +
                strpivot +
                segmentCategory +
                strOrderby;

            log.LogMethodExit(selectReportQuery);
            return selectReportQuery;
        }

        /// <summary>
        /// Gets consumptionLocationList report data
        /// </summary>
        /// <param name="SiteID">SiteID</param>
        /// <param name="LocationType">LocationType</param>
        /// <returns>Returns consumptionLocationList</returns>
        public DataTable GetLocationListByLocationType(string SiteID, string LocationType)
        {
            log.LogMethodEntry(SiteID, LocationType);
            string selectReportQuery = @"select locationid ID, l.name Value
                                        from location l, locationtype lt
                                        where l.locationtypeid = lt.LocationTypeId
	                                        and lt.LocationType = '" + LocationType + "' " +
                                            "and (l.site_id in (@site) or -1 in (@site))";
            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@site", SiteID);
            DataTable consumptionLocationList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (consumptionLocationList.Rows.Count > 0)
            {
                log.LogMethodExit(consumptionLocationList);
                return consumptionLocationList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets list of vendors
        /// </summary>
        /// <param name="SIteID">SIteID</param>
        /// <returns>Returns Daylookup</returns>
        public DataTable GetVendors(string SIteID)
        {
            log.LogMethodEntry(SIteID);
            string selectReportQuery = @"select -1 VendorId,'All' Name
                                        union all
                                        select VendorId, Name
                                        from Vendor 
                                        where ((site_id in (@site)) or (-1 in (@site)))
                                        
										
										  ";
            selectReportQuery = selectReportQuery.Replace("@site", SIteID);
            DataTable dtVendors = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);

            if (dtVendors.Rows.Count > 0)
            {
                log.LogMethodExit(dtVendors);
                return dtVendors;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets list of technician cards
        /// </summary>
        /// <param name="SIteID">SIteID</param>
        /// <returns>Returns Technician card list</returns>
        public DataTable GetTechnicianCardList(string SIteID)
        {
            log.LogMethodEntry(SIteID);
            string selectReportQuery = @"SELECT c.card_number + case isnull(isnull(u.username, c.notes), '') when '' then '' else ' - ' + isnull(u.username, c.notes) end card_number_label, c.card_id
                                        FROM cards c left outer join UserIdentificationTags ut on c.card_id = ut.CardId
	                                        left outer join users u on u.user_id = ut.userid  
                                        WHERE valid_flag = 'Y' 
                                        AND technician_card = 'Y'
                                        AND ((c.site_id in (@site)) or (-1 in (@site)))
                                        order by 2, 1";
            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportQuery = selectReportQuery.Replace("@site", SIteID);
            DataTable dtVendors = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);

            if (dtVendors.Rows.Count > 0)
            {
                log.LogMethodExit(dtVendors);
                return dtVendors;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets count of transactionsReturns count of transactions
        /// </summary>
        /// <param name="SiteID">SiteID</param>
        /// <param name="FromDate">FromDate</param>
        /// <param name="ToDate">ToDate</param>
        /// <returns></returns>
        public DataTable GetTransactionCount(string SiteID, DateTime FromDate, DateTime ToDate)
        {
            log.LogMethodEntry(SiteID, FromDate, ToDate);
            string selectReportQuery = @"SELECT count(*)
                                         from trx_header
                                        where trxdate between @fromdate and @todate
                                        AND ((site_id in (@site)) or (-1 in (@site)))
                                        order by 1";//Modified on 14-Sep-2017
                                                    //order by 2, 1";
            selectReportQuery = selectReportQuery.Replace("@site", SiteID);
            SqlParameter[] selectReportParameters = new SqlParameter[2];
            selectReportParameters[0] = new SqlParameter("@fromdate", FromDate);
            selectReportParameters[1] = new SqlParameter("@todate", ToDate);
            DataTable dtTrxCount = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (dtTrxCount.Rows.Count > 0)
            {
                log.LogMethodExit(dtTrxCount);
                return dtTrxCount;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets count of transactions
        /// </summary>
        /// <returns>Returns count of transactions</returns>
        public DataTable GetTransactionTotalCount()
        {
            log.LogMethodEntry();
            string selectReportQuery = @"SELECT SCHEMA_NAME(tbl.schema_id) as [Schema]
                                        , tbl.Name
                                        , Coalesce((Select pr.name
                                                From sys.database_principals pr
                                                Where pr.principal_id = tbl.principal_id)
                                            , SCHEMA_NAME(tbl.schema_id)) as [Owner]
                                        , tbl.max_column_id_used as [Columns]
                                        , CAST(CASE idx.index_id WHEN 1 THEN 1 ELSE 0 END AS bit) AS [HasClusIdx]
                                        , Coalesce( (Select sum (spart.rows) from sys.partitions spart
                                            Where spart.object_id = tbl.object_id and spart.index_id < 2), 0) AS [RowCount]
 
                                        , Coalesce( (Select Cast(v.low/1024.0 as float)
                                            * SUM(a.used_pages - CASE WHEN a.type <> 1 THEN a.used_pages WHEN p.index_id < 2 THEN a.data_pages ELSE 0 END)
                                                FROM sys.indexes as i
                                                 JOIN sys.partitions as p ON p.object_id = i.object_id and p.index_id = i.index_id
                                                 JOIN sys.allocation_units as a ON a.container_id = p.partition_id
                                                Where i.object_id = tbl.object_id  )
                                            , 0.0) AS [IndexKB]
 
                                        , Coalesce( (Select Cast(v.low/1024.0 as float)
                                            * SUM(CASE WHEN a.type <> 1 THEN a.used_pages WHEN p.index_id < 2 THEN a.data_pages ELSE 0 END)
                                                FROM sys.indexes as i
                                                 JOIN sys.partitions as p ON p.object_id = i.object_id and p.index_id = i.index_id
                                                 JOIN sys.allocation_units as a ON a.container_id = p.partition_id
                                                Where i.object_id = tbl.object_id)
                                            , 0.0) AS [DataKB]
                                        , tbl.create_date, tbl.modify_date
 
                                        FROM sys.tables AS tbl
                                          INNER JOIN sys.indexes AS idx ON (idx.object_id = tbl.object_id and idx.index_id < 2)
                                          INNER JOIN master.dbo.spt_values v ON (v.number=1 and v.type='E')
                                        WHERE TBL.name = 'TRX_LINES'
                                    ";

            DataTable dtTrxCount = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);

            if (dtTrxCount.Rows.Count > 0)
            {
                log.LogMethodExit(dtTrxCount);
                return dtTrxCount;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets count of Customer Gameplays
        /// </summary>
        /// <param name="SIteID">SIteID</param>
        /// <param name="FromDate">FromDate</param>
        /// <param name="ToDate">ToDate</param>
        /// <returns>Returns count of Customer Gameplays</returns>
        public DataTable GetCustomerGameplayCount(string SIteID, DateTime FromDate, DateTime ToDate)
        {
            log.LogMethodEntry(SIteID, FromDate, ToDate);
            string selectReportQuery = @"SELECT count(*)
                                         from gameplay
                                        where play_date between @fromdate and @todate
                                        AND ((site_id in (@site)) or (-1 in (@site)))
                                        and notes <> 'Technician Play'";
            selectReportQuery = selectReportQuery.Replace("@site", SIteID);
            SqlParameter[] selectReportParameters = new SqlParameter[2];
            selectReportParameters[0] = new SqlParameter("@fromdate", FromDate);
            selectReportParameters[1] = new SqlParameter("@todate", ToDate);
            DataTable dtTrxCount = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (dtTrxCount.Rows.Count > 0)
            {
                log.LogMethodExit(dtTrxCount);
                return dtTrxCount;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets InventoryReportData report data
        /// </summary>
        /// <param name="pivotColumns">pivotColumns</param>
        /// <param name="locationWhere">locationWhere</param>
        /// <param name="prodTypeWhere">prodTypeWhere</param>
        /// <param name="categoryWhere">categoryWhere</param>
        /// <param name="strpivot">strpivot</param>
        /// <param name="segmentCategory">segmentCategory</param>
        /// <param name="strOrderBy">strOrderBy</param>
        /// <param name="LocationID">LocationID</param>
        /// <returns>Returns selectReportQuery</returns>
        public string InventoryReportQuery(string pivotColumns, string locationWhere, string prodTypeWhere, string categoryWhere, string strpivot, string segmentCategory, string strOrderBy, string LocationID)
        {
            log.LogMethodEntry(pivotColumns, locationWhere, prodTypeWhere, categoryWhere, strpivot, segmentCategory, strOrderBy, LocationID);
            string selectReportQuery = "select site_name, ProductId, Code, Category, case when isnull(BarCode, '') <> '' then SUBSTRING(BarCode, 3, len(BarCode)) else BarCode end BarCode, Description " +
                                    pivotColumns +
                                    @",IsActive, 
                                    (Quantity) Quantity, Location,  
	                                isnull(Cost, ProductCost) Cost, 
                                    TaxInclusiveCost [Tax Inclv. Cost], 
                                    SalePrice [Sale Price], PriceInTickets, 
	                                VendorName [Vendor Name], LocationId  
                                from (
		                                select s.site_name, P.ProductId, Code, C.Name as Category, (select case cnt when 0 then '' else stuff((select ' | '+ cast(BarCode as nvarchar(250))
														FROM ProductBarcode
														WHERE productid = p.ProductId and isactive = 'Y'
														group by BarCode
														FOR XML PATH('')
														
														)
														, 1, 1, '') end
	 from (select count(*) cnt
		   from ProductBarcode 
		   where ProductId = p.ProductId and isactive = 'Y'
		   group by productid)v) BarCode, p.Description, PriceInTickets,
                                            segmentname, valuechar,
			                                P.IsActive,p.cost ProductCost,
                                            sum(i.Quantity - isnull(Receipt.quantity_open,0) - isnull(Adjust.quantity_open,0) + isnull(Transfer.quantity_open,0) + isnull(sold.quantity_Open,0) * -1) Quantity, L.Name as Location,  
			                                cast(p.cost as numeric(18,2)) Cost, 
                                            TaxInclusiveCost, SalePrice, 
			                                V.Name as VendorName, I.LocationId 
		                                from Product P left outer join Vendor V on P.DefaultVendorId = V.VendorID 
			                                left outer join Inventory I on P.ProductId = I.ProductId 
			                                left outer join Location L on I.LocationId = L.LocationId
                                            left outer join InventoryLot il on I.LotID = il.LotID
                                            LEFT OUTER JOIN (SELECT rl.ProductId,
								rl.locationid,
								invl.lotid,
								sum(isnull(invl.originalquantity, isnull(rl.quantity,0))) quantity_open
							 FROM Receipt R, product P, PurchaseOrderReceive_Line rl left outer join inventorylot invl on rl.PurchaseOrderReceiveLineId = invl.PurchaseOrderReceiveLineId
							 WHERE RL.ReceiptId = r.ReceiptId
								and ReceiveDate > @Todate
								and rl.ProductId = p.ProductId
								and IsReceived ='Y'
                                and (R.site_id in (@SiteId) or (-1 in (@SiteId)))
							group by rl.ProductId, rl.locationid, invl.LotId) Receipt on Receipt.ProductId = I.ProductId and Receipt.LocationId = I.LocationId and ISNULL(Receipt.lotid, -1) = isnull(I.lotid, -1)
			LEFT OUTER JOIN (
								select ia.productid, 
									ia.fromlocationid locationid,
									ia.lotid,
									sum(isnull(adjustmentquantity, 0)) quantity_open 
								from inventoryadjustments ia, product p
								where adjustmenttype in ('Adjustment', 'Trade-In') 
									and timestamp > @Todate
									and p.ProductId = ia.ProductId
                                    and (ia.site_id in (@SiteId) or (-1 in (@SiteId)))
								group by ia.productid,ia.fromlocationid,ia.lotid
								) Adjust on Adjust.productid = I.productid and Adjust.LocationId = i.LocationId and  isnull(Adjust.LotID, -1) = isnull(I.lotid, -1)
			left outer join (select productid, 
								locationid,
								lotid,
								sum(isnull(quantity_open, 0)) quantity_open
							from (
									select ia.productid, 
										ia.fromlocationid locationid,
										ia.lotid,
										sum(isnull(adjustmentquantity, 0)) quantity_open 
									from inventoryadjustments ia, product p
									where adjustmenttype in ('Transfer') 
										and timestamp > @Todate
										and p.ProductId = ia.ProductId
                                        and (ia.site_id in (@SiteId) or (-1 in (@SiteId)))
									group by ia.productid,ia.fromlocationid,ia.lotid
									union all
									select ia.productid, 
										ia.ToLocationId locationid,
										ia.lotid,
										sum(isnull(-1 * adjustmentquantity, 0)) quantity_open 
									from inventoryadjustments ia, product p
									where adjustmenttype in ('Transfer') 
										and timestamp > @Todate
										and p.ProductId = ia.ProductId
                                        and (ia.site_id in (@SiteId) or (-1 in (@SiteId)))
									group by ia.productid,ia.ToLocationId,ia.lotid
								)trf
								group by productid,locationid,lotid) Transfer on Transfer.productid = I.productid and Transfer.LocationId = i.LocationId and  isnull(Transfer.LotID, -1) = isnull(I.lotid, -1)
			LEFT OUTER JOIN (select r.productid,
								r.locationid,
								r.lotid,
								sum(isnull(r.quantity,0)*-1) quantity_open
							from InventoryTransaction r 
							where r.TrxDate > @Todate
								and (InventoryTransactionTypeID IS NULL
								OR InventoryTransactionTypeID = (select top 1 lookupValueId 
																 from lookupValues lv, lookups L
																 where lv.LookupId = l.LookupId
																   and l.LookupName = 'INVENTORY_TRANSACTION_TYPE'
																   and LookupValue = 'Sales'
                                                                   and (L.site_id in (@SiteId) or (-1 in (@SiteId))))
							  )
							group by r.productid, r.locationid, r.LotID) sold on sold.productid = i.productid and sold.LocationId = i.LocationId and isnull(sold.lotid, -1) = isnull(i.LotId, -1)
			left outer join SegmentDataView sdv on sdv.SegmentCategoryId = p.segmentcategoryid,
			                                Category C, site s 
		                                where P.CategoryId = C.CategoryID 
                                        AND (s.site_id = p.site_id OR p.site_id IS NULL)
                                        and (p.site_id in (@SiteId) or (-1 in (@SiteId))) and isPurchaseable = 'Y' " +
                                        prodTypeWhere +
                                        categoryWhere + //Added 30-Jul-2016
                                        locationWhere +
                                    //" group by p.productid, p.code, c.name, p.description, segmentname, valuechar, p.isactive, l.name, p.TaxInclusiveCost, p.SalePrice, v.name, i.LocationId " +
                                    @"  group by site_name, p.productid, p.code, p.cost, c.name, p.description, PriceInTickets, segmentname, valuechar, p.isactive, l.name, p.TaxInclusiveCost, p.SalePrice, v.name, i.LocationId )v " +
                                    strpivot +
                                    segmentCategory + //Updated sequence of string in query 11-Aug-2016 
                                    @" group by site_name, ProductId, Code, Category,PriceInTickets,
	                                    case when isnull(BarCode, '') <> '' then SUBSTRING(BarCode, 3, len(BarCode)) else BarCode end, 
	                                    Description " +
                                        pivotColumns +
                                       @" ,IsActive,TaxInclusiveCost, SalePrice, 
	                                    VendorName, LocationId, Location, isnull(Cost, ProductCost), Quantity  order by site_name, Code";

            log.LogMethodExit(selectReportQuery);
            return selectReportQuery;
        }


        /// <summary>
        /// IsManager method
        /// </summary>
        /// <param name="userid">userid</param>
        /// <returns>returns bool</returns>
        public bool IsManager(string userid)
        {
            log.LogMethodEntry(userid);
            string selectReportQuery = @"select u.user_id,u.username,u.loginid,r.manager_flag 
                                            from users u ,user_roles r
                                            where u.role_id=r.role_id and r.manager_flag='Y'
                                            and user_id=@userid";

            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@userid", userid);

            DataTable dtusers = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (dtusers.Rows.Count > 0)
            {
                log.LogMethodExit(true);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns To Time from PosMachinereportLog
        /// </summary>
        /// <param name="POSMachineName">POSMachineName</param>
        /// <param name="site_id">site_id</param>
        /// <param name="role_id">role_id</param>
        /// <param name="reportid">reportid</param>
        /// <returns>Time from PosMachinereportLog</returns>
        public DataTable GetToTimeByMachineName(string POSMachineName, string site_id, int role_id, int reportid)
        {

            log.LogMethodEntry(POSMachineName, site_id, role_id, reportid);
            string selectReportQuery = "";

            selectReportQuery = @"SELECT distinct id, startTime,endTime,reportid 
                                       FROM PosMachinereportLog pml, POSMachines pm, ManagementFormAccess ma, user_roles u 
                                   WHERE pm.POSName = ma.form_name 
                                   AND ma.main_menu = 'POS Machine'
                                   AND pml.POSMachineName = pm.POSName
                                   AND ma.access_allowed = 'Y'
                                   AND pml.POSMachineName=@posMachineName 
                                   AND ma.role_id = u.role_id
                                   AND  pml.reportid=@reportid
                                   AND (u.role_id = @role or @role = -1)
                                   and ((pm.site_id in (@site)) or (-1 in (@site))) order by endTime desc";

            selectReportQuery = selectReportQuery.Replace("@site", site_id);
            SqlParameter[] selectReportParameters = new SqlParameter[3];
            selectReportParameters[0] = new SqlParameter("@role", role_id);
            selectReportParameters[1] = new SqlParameter("@posMachineName", POSMachineName);
            selectReportParameters[2] = new SqlParameter("@reportid", reportid);
            DataTable ToTimeList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (ToTimeList.Rows.Count > 0)
            {
                log.LogMethodExit(ToTimeList);
                return ToTimeList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }

        }
        /// <summary>
        ///  Returns POSZ report time by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="site_id">site_id</param>
        /// <param name="role_id">role_id</param>
        /// <returns>POSZ report time by Id</returns>
        public DataTable GetPOSReportTimeByID(int id, string site_id, int role_id)
        {

            log.LogMethodEntry(id, site_id, role_id);
            string selectReportQuery = "";

            selectReportQuery = @"SELECT distinct id, startTime,endTime,reportid 
                                       FROM PosMachinereportLog pml, POSMachines pm, ManagementFormAccess ma, user_roles u 
                                   WHERE pm.POSName = ma.form_name 
                                   AND ma.main_menu = 'POS Machine'
                                       AND pml.POSMachineName = pm.POSName
                                   AND ma.access_allowed = 'Y' 
                                       AND Id = @id
                                   AND ma.role_id = u.role_id
                                   AND (u.role_id = @role or @role = -1)
                                   and ((pm.site_id in (@site)) or (-1 in (@site))) ";

            selectReportQuery = selectReportQuery.Replace("@site", site_id);
            SqlParameter[] selectReportParameters = new SqlParameter[2];
            selectReportParameters[0] = new SqlParameter("@role", role_id);
            selectReportParameters[1] = new SqlParameter("@id", id);
            DataTable ToTimeList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (ToTimeList.Rows.Count > 0)
            {
                log.LogMethodExit(ToTimeList);
                return ToTimeList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }

        }

        /// <summary>
        /// GetSiteListByOrgID method
        /// </summary>
        /// <param name="orgID">orgID</param>
        /// <param name="siteIds">siteIds</param>
        /// <returns>ListByOrgID</returns>
        public DataTable GetSiteListByOrgID(int orgID, string siteIds)
        {
            log.LogMethodEntry(orgID, siteIds);
            string selectSiteListQuery = @"select * from site where OrgId = @orgId and site_id in (@accessibleSites) order by site_name";
            selectSiteListQuery = selectSiteListQuery.Replace("@accessibleSites", siteIds);
            SqlParameter[] selectSiteListParameters = new SqlParameter[1];
            selectSiteListParameters[0] = new SqlParameter("@orgId", orgID);
            DataTable siteList = dataAccessHandler.executeSelectQuery(selectSiteListQuery, selectSiteListParameters, sqlTransaction);

            if (siteList.Rows.Count > 0)
            {
                log.LogMethodExit(siteList);
                return siteList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// GetConceptTypes method
        /// </summary>
        /// <param name="segmnetName">segmnetName</param>
        /// <returns>ConceptTypes</returns>
        public DataTable GetConceptTypes(string segmnetName)
        {
            log.LogMethodEntry(segmnetName);

            string selectConceptTypeQuery = @"select distinct isnull(ValueChar,'UnassignedConcept') ConceptType 
                                        from SegmentDataView where 
                                           SegmentName=@segmentName order by 1 asc";
            SqlParameter[] selectConceptTypeParameters = new SqlParameter[1];
            selectConceptTypeParameters[0] = new SqlParameter("@segmentName", segmnetName);
            DataTable conceptTypeList = dataAccessHandler.executeSelectQuery(selectConceptTypeQuery, selectConceptTypeParameters, sqlTransaction);

            if (conceptTypeList.Rows.Count > 0)
            {
                log.LogMethodExit(conceptTypeList);
                return conceptTypeList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }


        /// <summary>
        /// GetSiteDescription method
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns>SiteDescription</returns>
        public DataTable GetSiteDescription(string siteId)
        {
            log.LogMethodEntry(siteId);

            string selectSiteDescriptionQuery = @" select distinct Description from site where Description is not null and (site_id in (@siteID) or -1 in(@siteID))";
            selectSiteDescriptionQuery = selectSiteDescriptionQuery.Replace("@siteID", siteId);
            DataTable siteDescriptionList = dataAccessHandler.executeSelectQuery(selectSiteDescriptionQuery, null, sqlTransaction);

            if (siteDescriptionList.Rows.Count > 0)
            {
                log.LogMethodExit(siteDescriptionList);
                return siteDescriptionList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }


        public ReportsDTO GetReportsById(int reportId)
        {
            log.LogMethodEntry(reportId);
            ReportsDTO reportsDTO = null;
            string selectReportQuery = @"select *, case OutputFormat when 'E' then 'Excel' when 'C' then 'Chart' when 'V' then 'CSV' else 'PDF' end DisplayOutputFormat from Reports
                                                        where report_id= @reportId";

            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@reportId", reportId);
            DataTable reportData = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);

            if (reportData.Rows.Count > 0)
            {
                reportsDTO = new ReportsDTO();
                DataRow reportRow = reportData.Rows[0];
                reportsDTO = GetReportsDTO(reportRow);
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
            log.LogMethodExit(reportsDTO);
            return reportsDTO;
        }


        /// <summary>
        /// GetPOSListByPaymentDate
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="roleId"></param>
        /// <param name="ENABLE_POS_FILTER_IN_TRX_REPORT"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="offset"></param>
        /// <returns>POS Machines by PaymentDate</returns>
        public DataTable GetPOSListByPaymentDate(string siteId, int roleId, string ENABLE_POS_FILTER_IN_TRX_REPORT, DateTime fromDate, DateTime toDate, int offset)
        {
            log.LogMethodEntry(siteId, roleId, ENABLE_POS_FILTER_IN_TRX_REPORT, fromDate, toDate, offset);
            string selectReportQuery = string.Empty;
            DataTable posMachineList = null;

            if (ENABLE_POS_FILTER_IN_TRX_REPORT == "Y")
            {
                selectReportQuery = @"SELECT distinct POSname
	                                    FROM POSMachines pm, ManagementFormAccess ma, user_roles u 
	                                    WHERE pm.POSName = ma.form_name 
	                                    AND ma.main_menu = 'POS Machine'
	                                    AND ma.access_allowed = 'Y' 
	                                    AND ma.role_id = u.role_id
	                                    AND (u.role_id = @role or @role = -1)
	                                    and ((pm.site_id in (@site)) or (-1 in (@site))) ";
            }
            else
            {
                if (siteId == "-1" || siteId == null)
                {
                    selectReportQuery = @"SELECT DISTINCT PosMachine as POSname 
                                    from TrxPayments 
                                    WHERE PaymentDate >=@fromDate 
                                    AND PaymentDate <@toDate
                                    and ((site_id in (@site)) or (-1 in (@site)))";
                }
                else
                {
                    selectReportQuery = @"declare @dFromDate datetime=dbo.ConvertToServerTime(@fromDate,@offSet)
declare @dToDate datetime=dbo.ConvertToServerTime(@toDate,@offSet)
SELECT DISTINCT PosMachine as POSname 
                                    from TrxPayments 
                                    WHERE PaymentDate >= @dFromDate 
                                    AND PaymentDate < @dToDate
                                    and ((site_id in (@site)) or (-1 in (@site)))";
                }
            }

            selectReportQuery = selectReportQuery.Replace("@site", siteId);
            SqlParameter[] selectReportParameters = new SqlParameter[4];
            selectReportParameters[0] = new SqlParameter("@role", roleId);
            selectReportParameters[1] = new SqlParameter("@fromDate", fromDate);
            selectReportParameters[2] = new SqlParameter("@toDate", toDate);
            selectReportParameters[3] = new SqlParameter("@offSet", offset);
            posMachineList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);
            log.LogMethodExit(posMachineList);
            return posMachineList;
        }

        /// <summary>
        /// GetUsersListByPaymentDate
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns>Users List by PaymentDate</returns>
        public DataTable GetUsersListByPaymentDate(string siteId, DateTime fromDate, DateTime toDate)
        {
            log.LogMethodEntry(siteId, fromDate, toDate);
            string selectReportQuery = string.Empty;
            DataTable userList = null;
            
            if (siteId == "-1" || siteId == null)
            {
                selectReportQuery = @"select  distinct u.user_id, 
                                        (case when u.active_flag!='Y' then u.username+' inactive' else u.username end) username from users u,TrxPayments tp 
                                        where tp.CreatedBy=u.loginid and (u.site_id in (@site) or (-1 in (@site)))
                                        and tp.PaymentDate >=@fromDate
                                        and tp.PaymentDate <@toDate";

            }
            else
            {
                selectReportQuery = @"declare @dFromDate datetime=dbo.ConvertToServerTime(@fromDate,@offSet)
declare @dToDate datetime=dbo.ConvertToServerTime(@toDate,@offSet)
select  distinct u.user_id, 
                                        (case when u.active_flag!='Y' then u.username+' inactive' else u.username end) username from users u,TrxPayments tp 
                                        where tp.CreatedBy=u.loginid and (u.site_id in (@site) or (-1 in (@site)))
                                        and tp.PaymentDate >= @dFromDate 
                                        and tp.PaymentDate < @dToDate";
            }
            selectReportQuery = selectReportQuery.Replace("@site", siteId);
            SqlParameter[] selectReportParameters = new SqlParameter[3];
            selectReportParameters[0] = new SqlParameter("@todate", toDate);
            selectReportParameters[1] = new SqlParameter("@fromdate", fromDate);
            selectReportParameters[2] = new SqlParameter("@offSet", Common.Offset);

            userList = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);
            log.LogMethodExit(userList);
            return userList;
        }
        /// <summary>
        /// Gets HomePage Dashboard Reports
        /// </summary>
        /// <returns>Returns ReportList</returns>
        public List<ReportsDTO> GetHomePageReports(int site_id, int role_id)
        {
            log.LogMethodEntry(site_id);
            List<ReportsDTO> reportsList = null;
            string selectReportQuery = @"select r.*,case OutputFormat when 'E' then 'Excel' when 'C' then 'Chart' when 'V' then 'CSV' else 'PDF' end DisplayOutputFormat
                                        from reports r, managementformaccess m
                                        where (r.site_id = @site_id or @site_id = -1) 
                                         and (r.IsActive is null or r.IsActive = 1)
                                            and r.report_group='HomePage'
										and (m.access_allowed='Y' 
										and m.role_id=@role_id
                                        and (m.site_id = @site_id or @site_id = -1)
                                        and m.main_menu = 'Run Reports' 
										and m.form_name=r.report_name )
                                        order by report_group, report_name";
            SqlParameter[] selectReportParameters = new SqlParameter[2];
            selectReportParameters[0] = new SqlParameter("@site_id", site_id);
            selectReportParameters[1] = new SqlParameter("@role_id", role_id);

            DataTable reportsData = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters, sqlTransaction);
            if (reportsData.Rows.Count > 0)
            {
                reportsList = new List<ReportsDTO>();
                foreach (DataRow reportsDataRow in reportsData.Rows)
                {
                    ReportsDTO reportsDataObject = GetReportsDTO(reportsDataRow);
                    reportsList.Add(reportsDataObject);
                }
            }
            log.LogMethodExit(reportsList);
            return reportsList;

        }
        /// <summary>
        /// Inserts the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="functionalGuid">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void AddManagementFormAccess(string formName, string functionGuid, int siteId, bool isActive)
        {
            log.LogMethodEntry(formName, functionGuid, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'Run Reports',@formName,'Reports',@siteId,@functionGuid,@isActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", isActive));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Rename the managementFormAccess record to the database
        /// </summary>
        /// <param name="newFormName">string type object</param>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void RenameManagementFormAccess(string newFormName, string formName, int siteId, string functionGuid)
        {
            log.LogMethodEntry(newFormName, formName, siteId);
            string query = @"exec RenameManagementFormAccess @newFormName,'Run Reports',@formName,'Reports',@siteId,@functionGuid";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@newFormName", newFormName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Update the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="updatedIsActive">Site to which the record belongs</param>
        /// <param name="functionGuid">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void UpdateManagementFormAccess(string formName, int siteId, bool updatedIsActive, string functionGuid)
        {
            log.LogMethodEntry(formName, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'Run Reports',@formName,'Reports',@siteId,@functionGuid,@updatedIsActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@updatedIsActive", updatedIsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
    }
}

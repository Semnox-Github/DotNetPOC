/********************************************************************************************************
 * Project Name - Reports
 * Description  - Bussiness logic of Reports
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************************
 *1.00        14-Apr-2017   Amaresh        Created 
 *2.70.2      12-Jul-2019   Dakshakh raj   Modified : Save() method Insert/Update method returns DTO.
 *2.110       22-Dec-2020   Laster Menezes Added new methods GetPOSListByPaymentDate, GetUsersListByPaymentDate
 *2.110       28-Dec-2020   Laster Menezes Added parameterized constructor with executioncontext for Reports and ReportsList class
 *2.110       02-Feb-2021   Laster Menezes Added method GetReportDTOWithCustomKey
 *2.130       03-Sep-2021   Laster Menezes Added method GetTableauDashboards
 *2.150       03-Sep-2022   Rakshith Shetty Added method GetHomePageReports.
 *2.150.1     06-Mar-2023   Rakshith Shetty Modified GetHomePageReports Method to return list of Home Page Dashboard Reports with siteid and userrole as inputs to return the list based on management form access. 
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 *********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// report class
    /// </summary>
    public class Reports
    {
        private ReportsDTO reportsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Reports()
        {
            log.LogMethodEntry();
            reportsDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Reports DTO parameter
        /// </summary>
        /// <param name="reportsDTO">Parameter of the type ReportsDTO</param>
        public Reports(ReportsDTO reportsDTO)
        {
            log.LogMethodEntry(reportsDTO);
            this.reportsDTO = reportsDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor of Reports class
        /// </summary>
        /// <param name="executionContext"></param>
        public Reports(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the Reports DTO parameter
        /// </summary>
        /// <param name="reportsDTO">Parameter of the type ReportsDTO</param>
        public Reports(ExecutionContext executionContext, int reportId)
        {
            log.LogMethodEntry(reportsDTO);
            this.executionContext = executionContext;
            ReportsDataHandler reportsDataHandler = new ReportsDataHandler();
            this.reportsDTO = reportsDataHandler.GetReportsById(reportId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Reports  
        /// Reports   will be inserted if  Id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ExecutionContext executionUserContext = null;

            if (this.executionContext != null)
                executionUserContext = this.executionContext;
            else
                executionUserContext = ExecutionContext.GetExecutionContext();

            ReportsDataHandler reportsDataHandler = new ReportsDataHandler(sqlTransaction);

            if (reportsDTO.ReportId < 0)
            {
                reportsDTO = reportsDataHandler.InsertReports(reportsDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                reportsDTO.AcceptChanges();
                AddManagementFormAccess(executionUserContext,sqlTransaction);
            }
            else
            {
                if (reportsDTO.IsChanged)
                {
                    ReportsDTO existingReportsDTO = new Reports(executionUserContext, reportsDTO.ReportId).GetReportsDTO;
                    reportsDTO = reportsDataHandler.UpdateReports(reportsDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                    reportsDTO.AcceptChanges();
                    if (existingReportsDTO.ReportName.ToLower().ToString() != reportsDTO.ReportName.ToLower().ToString())
                    {
                        RenameManagementFormAccess(existingReportsDTO.ReportName, executionUserContext, sqlTransaction);
                    }
                    if (existingReportsDTO.IsActive != reportsDTO.IsActive)
                    {
                        UpdateManagementFormAccess(reportsDTO.ReportName, reportsDTO.IsActive, reportsDTO.Guid, executionUserContext, sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }
        public void AddManagementFormAccess(ExecutionContext executionContext, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(executionContext, sqlTransaction);
            if (reportsDTO.ReportId > -1)
            {
                ReportsDataHandler reportsDataHandler = new ReportsDataHandler(sqlTransaction);
                reportsDataHandler.AddManagementFormAccess(reportsDTO.ReportName, reportsDTO.Guid, executionContext.GetSiteId(), reportsDTO.IsActive);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        public void RenameManagementFormAccess(string existingFormName, ExecutionContext executionContext, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (reportsDTO.ReportId > -1)
            {
                ReportsDataHandler reportsDataHandler = new ReportsDataHandler(sqlTransaction);
                reportsDataHandler.RenameManagementFormAccess(reportsDTO.ReportName, existingFormName, executionContext.GetSiteId(), reportsDTO.Guid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        public void UpdateManagementFormAccess(string formName, bool updatedIsActive, string functionGuid, ExecutionContext executionContext, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (reportsDTO.ReportId > -1)
            {
                ReportsDataHandler reportsDataHandler = new ReportsDataHandler(sqlTransaction);
                reportsDataHandler.UpdateManagementFormAccess(formName, executionContext.GetSiteId(), updatedIsActive, functionGuid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the tableau token for logged in user
        /// </summary>
        /// <param name="deviceGuid"></param>
        /// <returns></returns>
        public string GetTableauToken(string deviceGuid)
        {
            log.LogMethodEntry(deviceGuid);
            String token = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TABLEAU_TOKENIZATION_URL"));
                var postData = "username=" + executionContext.GetUserId();
                postData += "&client_ip=" + Dns.GetHostAddresses(Environment.MachineName)[0].ToString();
                log.LogVariableState("postData", postData);
                var data = Encoding.ASCII.GetBytes(postData);
                log.LogVariableState("data", data);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                token = responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            log.LogMethodExit(token);
            return token;
        }

        #region Properties
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ReportsDTO GetReportsDTO
        {
            get { return reportsDTO; }
        }
        #endregion
    }

    /// <summary>
    /// Manages the list of Reports List
    /// </summary>
    public class ReportsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        string connectionString = string.Empty;

        /// <summary>
        /// Returns the ReportsDTO
        /// </summary>
        /// <param name="reportsId">reportsId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>ReportsDTO</returns>
        public ReportsDTO GetReports(int reportsId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reportsId);
            ReportsDataHandler reportsDataHandler = new ReportsDataHandler(sqlTransaction);
            ReportsDTO reportsDTO = reportsDataHandler.GetReports(reportsId);
            log.LogMethodExit(reportsDTO);
            return reportsDTO;
        }

        public ReportsList()
        {

        }

        public ReportsList(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Parameterized constructor of ReportsList class
        /// </summary>
        /// <param name="executionContext"></param>
        public ReportsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ReportsDTO
        /// </summary>
        /// <param name="reportKey">reportKey</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>ReportsDTO</returns>
        public ReportsDTO GetReportsByReportKey(string reportKey, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reportKey, sqlTransaction);
            ReportsDataHandler reportsDataHandler = new ReportsDataHandler(sqlTransaction);
            ReportsDTO reportsDTO = reportsDataHandler.GetReportsByReportKey(reportKey);
            log.LogMethodExit(reportsDTO);
            return reportsDTO;
        }

        /// <summary>
        /// Returns the List of ReportsDTO
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> List of ReportsDTO</returns>
        public List<ReportsDTO> GetAllReports(List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            List<ReportsDTO> reportsDTOList = ReportsDataHandler.GetReportsList(searchParameters);
            log.LogMethodExit(reportsDTOList);
            return reportsDTOList;
        }

        /// <summary>
        ///  Returns the List of ReportsDTO
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="role_id">role_id</param>
        /// <param name="group">group</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of ReportsDTO</returns>
        public List<ReportsDTO> GetReportsByGroup(int site_id, int role_id, string group, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(site_id, role_id, group, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            List<ReportsDTO> reportsDTOList = ReportsDataHandler.GetReportsByGroup(site_id, role_id, group);
            log.LogMethodExit(reportsDTOList);
            return reportsDTOList;
        }

        /// <summary>
        ///  Deletes report
        /// </summary>
        /// <param name="reportsDTO">reportsDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Deletes report</returns>
        public int DeleteReport(ReportsDTO reportsDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reportsDTO, sqlTransaction);
            ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            ReportsDataHandler.UpdateManagementFormAccess(reportsDTO.ReportName,executionContext.GetSiteId(), false, reportsDTO.Guid);
            int delreps = ReportsDataHandler.DeleteReport(reportsDTO);
            log.LogMethodExit(delreps);
            return delreps;
        }

        /// <summary>
        /// Returns the List of ReportsDTO
        /// </summary>
        /// <param name="roleID">roleID</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of ReportsDTO</returns>
        public DataTable GetAccessibleFunctions(int roleID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(roleID, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetAccessibleFunctions(roleID);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the List of Report formats
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Report formats</returns>
        public DataTable GetReportFormats(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetReportOutputFormats();
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the List of Report Groups
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="role_id">role_id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> List of Report Groups</returns>
        public DataTable GetReportGroupList(int site_id, int role_id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(site_id, role_id, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetReportGroupList(site_id, role_id);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the List of Report Groups
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="role_id">role_id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> List of Report Groups</returns>
        public DataTable GetGameProfileList(string site_id, int role_id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(site_id, role_id, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetGameProfileList(site_id, role_id);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the List of Report Groups
        /// </summary>
        /// <param name="profile_id">profile_id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns the List of Report Groups</returns>
        public DataTable GetMachineList(string profile_id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(profile_id, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetMachineList(profile_id);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the List of Categories
        /// </summary>
        /// <param name="site_id_array">site_id_array</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of Categories</returns>
        public DataTable GetCategories(string site_id_array, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(site_id_array, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetCategories(site_id_array);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the list of Purchase order Types
        /// </summary>
        /// <param name="site_id"></param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>list of Purchase order Types</returns>
        public DataTable GetPurchaseOrderTypes(string site_id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(site_id, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetPurchaseOrderTypes(site_id);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the List of Locations
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of Locations</returns>
        public DataTable GetInventoryLocations(string site_id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(site_id, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetInventoryLocations(site_id);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the List of Sites
        /// </summary>
        /// <param name="Role_id">Role_id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> List of Sites</returns>
        public DataTable GetAccessibleSites(int Role_id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(Role_id, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetAccessibleSites(Role_id);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        ///  Returns the List of Segments
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> List of Segments</returns>
        public DataTable GetSegments(string site_id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(site_id, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetSegments(site_id);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        ///  Returns the List of Users
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of Users</returns>
        public DataTable GetUsersList(string site_id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(site_id, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetUsersList(site_id);
            log.LogMethodExit(dataTable);
            return dataTable;
        }


        /// <summary>
        /// Gets Users list
        /// </summary>
        /// <param name="user_id">user_id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Users list</returns>
        public DataTable GetUsersList(int user_id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(user_id, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetUsersList(user_id);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the List of Users from transaction
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="from">from</param>
        /// <param name="to">to</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of Users from transaction</returns>
        public DataTable GetUsersListFromTransaction(string site_id, DateTime from, DateTime to, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(site_id, from, to, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetUsersListFromTransaction(site_id, from, to);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Gets Users list from transaction By LastUpdateTime
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="from">from</param>
        /// <param name="to">to</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns Userlist</returns>
        public DataTable GetUsersListFromTransactionByLastupdated(string site_id, DateTime from, DateTime to, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(site_id, from, to, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetUsersListFromTransactionByLastupdated(site_id, from, to);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the List of POS machines
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="role_id">role_id</param>
        /// <param name="ENABLE_POS_FILTER_IN_TRX_REPORT">ENABLE_POS_FILTER_IN_TRX_REPORT</param>
        /// <param name="Mode">Mode</param>
        /// <param name="FromDate">FromDate</param>
        /// <param name="ToDate">ToDate</param>
        /// <param name="offset">offset</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of POS machines</returns>
        public DataTable GetPOSList(string site_id, int role_id, string ENABLE_POS_FILTER_IN_TRX_REPORT, string Mode, DateTime FromDate, DateTime ToDate, int offset, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(site_id, role_id, ENABLE_POS_FILTER_IN_TRX_REPORT, Mode, FromDate, ToDate, offset, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetPOSList(site_id, role_id, ENABLE_POS_FILTER_IN_TRX_REPORT, Mode, FromDate, ToDate, offset);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the List of POS machines 
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="role_id">role_id</param>
        /// <param name="ENABLE_POS_FILTER_IN_TRX_REPORT">ENABLE_POS_FILTER_IN_TRX_REPORT</param>
        /// <param name="FromDate">FromDate</param>
        /// <param name="ToDate">ToDate</param>
        /// <param name="offset">offset</param>
        /// <param name="userId">userId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of POS machines</returns>
        public DataTable GetPOSListByUserId(string site_id, int role_id, string ENABLE_POS_FILTER_IN_TRX_REPORT, DateTime FromDate, DateTime ToDate, int offset, int userId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(site_id, role_id, ENABLE_POS_FILTER_IN_TRX_REPORT, FromDate, ToDate, offset, userId, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetPOSListByUserId(site_id, role_id, ENABLE_POS_FILTER_IN_TRX_REPORT, FromDate, ToDate, offset, userId);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        ///  Returns the List of SegmentDefinitionSourceValue
        /// </summary>
        /// <param name="SegmentDefinitionId">SegmentDefinitionId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of SegmentDefinitionSourceValue</returns>
        public DataTable GetSegmentDefinitionSourceValues(int SegmentDefinitionId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(SegmentDefinitionId, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetSegmentDefinitionSourceValues(SegmentDefinitionId);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the List of SegmentDefinitionSourceValue
        /// </summary>
        /// <param name="SegmentDefinitionSourceId">SegmentDefinitionSourceId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of SegmentDefinitionSourceValue</returns>
        public DataTable GetSegmentDefinitionSourceValuesDBQuery(int SegmentDefinitionSourceId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(SegmentDefinitionSourceId, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetSegmentDefinitionSourceValuesDBQuery(SegmentDefinitionSourceId);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the custom reports data
        /// </summary>
        /// <param name="DBQuery">DBQuery</param>
        /// <param name="OtherParams">OtherParams</param>
        /// <param name="timeOutSet">timeOutSet</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>custom reports data</returns>
        public DataTable GetCustomReportData(string DBQuery, List<SqlParameter> OtherParams, bool timeOutSet = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(DBQuery, OtherParams, sqlTransaction);
            int commandTimeout = 900;
            try
            {
                commandTimeout = Convert.ToInt32(Common.Utilities.getParafaitDefaults("REPORT_COMMAND_TIMEOUT"));
            }
            catch
            {
                commandTimeout = 900;
            }
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(commandTimeout, sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetCustomReportData(DBQuery, OtherParams);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns OpenToBuy query
        /// </summary>
        /// <param name="strSelect">strSelect</param>
        /// <param name="strFrom">strFrom</param>
        /// <param name="strWhere">strWhere</param>
        /// <param name="strGroupBy">strGroupBy</param>
        /// <param name="fromdate">fromdate</param>
        /// <param name="toDate">toDate</param>
        /// <param name="Site">Site<param>
        /// <param name="CurrencySymbol">CurrencySymbol</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>OpenToBuy query</returns>
        public string OpenToBuyReportQuery(string strSelect, string strFrom, string strWhere, string strGroupBy, DateTime fromdate, DateTime toDate, string Site, string CurrencySymbol, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(strSelect, strFrom, strWhere, strGroupBy, fromdate, toDate, Site, CurrencySymbol, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            string que = ReportsDataHandler.OpenToBuyReportQuery(strSelect, strFrom, strWhere, strGroupBy, fromdate, toDate, Site, CurrencySymbol);
            log.LogMethodExit(que);
            return que;
        }

        /// <summary>
        ///  Returns InventoryAgingReport Query 
        /// </summary>
        /// <param name="strSelect">strSelect</param>
        /// <param name="strFrom">strFrom</param>
        /// <param name="strWhere">strWhere</param>
        /// <param name="strGroupBy">strGroupBy</param>
        /// <param name="fromdate">fromdate</param>
        /// <param name="toDate">toDate</param>
        /// <param name="Site">Site</param>
        /// <param name="CurrencySymbol">CurrencySymbol</param>
        /// <param name="vendorWhere">vendorWhere</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>InventoryAgingReport Query </returns>
        public string InventoryAgingReportQuery(string strSelect, string strFrom, string strWhere, string strGroupBy, DateTime fromdate, DateTime toDate, string Site, string CurrencySymbol, string vendorWhere, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(strSelect, strFrom, strWhere, strGroupBy, fromdate, toDate, Site, CurrencySymbol, vendorWhere, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            string que = ReportsDataHandler.InventoryAgingReportQuery(strSelect, strFrom, strWhere, fromdate, toDate, Site, CurrencySymbol, vendorWhere);
            log.LogMethodExit(que);
            return que;
        }

        /// <summary>
        /// Returns Top15WeeklyUnitSalesReportData query
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
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Top15WeeklyUnitSalesReportData query</returns>
        public string Top15WeeklyUnitSalesReportQuery(string strSelect, string strFrom, string strWhere, string strGroupBy, string strOrderBy, DateTime fromdate, DateTime toDate, string Site, string CurrencySymbol, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(strSelect, strFrom, strWhere, strGroupBy, strOrderBy, fromdate, toDate, Site, CurrencySymbol, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            string que = ReportsDataHandler.Top15WeeklyUnitSalesReportQuery(strSelect, strFrom, strWhere, strGroupBy, strOrderBy, fromdate, toDate, Site, CurrencySymbol);
            log.LogMethodExit(que);
            return que;
        }

        /// <summary>
        ///  Returns DepartmentSellingYTD query
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
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>DepartmentSellingYTD query</returns>
        public string DepartmentSellingYTDReportQuery(string strSelect, string strFrom, string strWhere, string strGroupBy, string strOrderBy, DateTime fromdate, DateTime toDate, string Site, string CurrencySymbol, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(strSelect, strFrom, strWhere, strGroupBy, strOrderBy, fromdate, toDate, Site, CurrencySymbol, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            string que = ReportsDataHandler.DepartmentSellingYTDReportQuery(strSelect, strFrom, strWhere, strGroupBy, strOrderBy, fromdate, toDate, Site, CurrencySymbol);
            log.LogMethodExit(que);
            return que;
        }

        /// <summary>
        /// Returns GetPivotColumns data
        /// </summary>
        /// <param name="SiteID">SiteID</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>GetPivotColumns data</returns>
        public DataTable GetPivotColumns(string SiteID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(SiteID, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetPivotColumns(SiteID);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        ///  Returns Store location list
        /// </summary>
        /// <param name="SiteID">SiteID</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Store location list</returns>
        public DataTable GetStoreLocations(string SiteID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(SiteID, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetStoreLocations(SiteID);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns InventoryAdjustmentsReport data
        /// </summary>
        /// <param name="strSelect">strSelect</param>
        /// <param name="prodTypeWhere">prodTypeWhere</param>
        /// <param name="categoryWhere">categoryWhere</param>
        /// <param name="strpivot">strpivot</param>
        /// <param name="segmentCategory">segmentCategory</param>
        /// <param name="strOrderby">strOrderby</param>
        /// <param name="LocationID">LocationID</param>
        /// <param name="vendorWhere">vendorWhere</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>InventoryAdjustmentsReport data</returns>
        public string InventoryAdjustmentsReportQuery(string strSelect, string prodTypeWhere, string categoryWhere, string strpivot, string segmentCategory, string strOrderby, string LocationID, string vendorWhere, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(strSelect, prodTypeWhere, categoryWhere, strpivot, segmentCategory, strOrderby, LocationID, vendorWhere, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            string dat = ReportsDataHandler.InventoryAdjustmentsReportQuery(strSelect, prodTypeWhere, categoryWhere, strpivot, segmentCategory, strOrderby, LocationID, vendorWhere);
            log.LogMethodExit(dat);
            return dat;
        }

        /// <summary>
        /// Returns InventoryReport data
        /// </summary>
        /// <param name="pivotColumns">pivotColumns</param>
        /// <param name="locationWhere">locationWhere</param>
        /// <param name="prodTypeWhere">prodTypeWhere</param>
        /// <param name="categoryWhere">categoryWhere</param>
        /// <param name="strpivot">strpivot</param>
        /// <param name="segmentCategory">segmentCategory</param>
        /// <param name="strOrderBy">strOrderBy</param>
        /// <param name="LocationID">LocationID</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>InventoryReport data</returns>
        public string InventoryReportQuery(string pivotColumns, string locationWhere, string prodTypeWhere, string categoryWhere, string strpivot, string segmentCategory, string strOrderBy, string LocationID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pivotColumns, locationWhere, prodTypeWhere, categoryWhere, strpivot, segmentCategory, strOrderBy, LocationID, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            string dat = ReportsDataHandler.InventoryReportQuery(pivotColumns, locationWhere, prodTypeWhere, categoryWhere, strpivot, segmentCategory, strOrderBy, LocationID);
            log.LogMethodExit(dat);
            return dat;
        }

        /// <summary>
        /// Returns Purchaseorder Report data
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
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Purchaseorder Report data</returns>
        public string PurchaseOrderReportQuery(string strSelect, string s_line, string categoryWhere, string strPONumber, string poType, string vendor, string creditCashPO, string strpivot, string segmentCategory, string strOrderBy, string Site, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(strSelect, s_line, categoryWhere, strPONumber, poType, vendor, creditCashPO, strpivot, segmentCategory, strOrderBy, Site, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            string dat = ReportsDataHandler.PurchaseOrderReportQuery(strSelect, s_line, categoryWhere, strPONumber, poType, vendor, creditCashPO, strpivot, segmentCategory, strOrderBy, Site);
            log.LogMethodExit(dat);
            return dat;
        }

        /// <summary>
        ///  Returns Received Report data
        /// </summary>
        /// <param name="pivotColumns">pivotColumns</param>
        /// <param name="categoryWhere">categoryWhere</param>
        /// <param name="poType">poType</param>
        /// <param name="recvLocation">recvLocation</param>
        /// <param name="vendor">vendor</param>
        /// <param name="creditCashPO">creditCashPO</param>
        /// <param name="strpivot">strpivot</param>
        /// <param name="segmentCategory">segmentCategoryparam>
        /// <param name="strOrderby">strOrderby</param>
        /// <param name="Site">Site</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Received Report data</returns>
        public string ReceivedReportQuery(string pivotColumns, string categoryWhere, string poType, string recvLocation, string vendor, string creditCashPO, string strpivot, string segmentCategory, string strOrderby, string Site, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pivotColumns, categoryWhere, poType, recvLocation, vendor, creditCashPO, strpivot, segmentCategory, strOrderby, Site, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            string dat = ReportsDataHandler.ReceivedReportQuery(pivotColumns, categoryWhere, poType, recvLocation, vendor, creditCashPO, strpivot, segmentCategory, strOrderby, Site);
            log.LogMethodExit(dat);
            return dat;
        }

        /// <summary>
        /// Inserts report run audit record
        /// </summary>
        /// <param name="reportID">reportID</param>
        /// <param name="start_time">start_time</param>
        /// <param name="end_time">end_time</param>
        /// <param name="param_list">param_list</param>
        /// <param name="username">username</param>
        /// <param name="siteId">siteId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>report run audit record</returns>
        public int InsertReportAuditRecord(string reportID, DateTime start_time, DateTime end_time, string param_list, string username, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reportID, start_time, end_time, param_list, username, siteId, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            int ins = ReportsDataHandler.InsertReportAuditRecord(reportID, start_time, end_time, param_list, username, siteId);
            log.LogMethodExit(ins);
            return ins;
        }

        /// <summary>
        ///  Inserts report run audit record
        /// </summary>
        /// <param name="Query">Query</param>
        /// <param name="parameters">parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>report run audit record</returns>
        public DataTable GetQueryOutput(string Query, List<SqlParameter> parameters = null, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(Query, parameters, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetQueryOutput(Query, parameters);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Inserts report run audit record
        /// </summary>
        /// <param name="SiteID">SiteID</param>
        /// <param name="LocationType">LocationType</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> report run audit record</returns>
        public DataTable GetLocationListByLocationType(string SiteID, string LocationType, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(SiteID, LocationType, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetLocationListByLocationType(SiteID, LocationType);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Get vendor list
        /// </summary>
        /// <param name="SiteID">SiteID</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>vendor list</returns>
        public DataTable GetVendors(string SiteID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(SiteID, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetVendors(SiteID);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Get technician card list
        /// </summary>
        /// <param name="SiteID">SiteID</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>technician card list</returns>
        public DataTable GetTechnicianCardList(string SiteID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(SiteID, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetTechnicianCardList(SiteID);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        ///  Get transaction count
        /// </summary>
        /// <param name="SiteID">SiteID</param>
        /// <param name="FromDate">FromDate</param>
        /// <param name="ToDate">ToDate</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>transaction count</returns>
        public DataTable GetTransactionCount(string SiteID, DateTime FromDate, DateTime ToDate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(SiteID, FromDate, ToDate, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetTransactionCount(SiteID, FromDate, ToDate);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        ///  Gets count of transactions
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>count of transactions</returns>
        public DataTable GetTransactionTotalCount(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetTransactionTotalCount();
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Get customer Gameplay count
        /// </summary>
        /// <param name="SiteID">SiteID</param>
        /// <param name="FromDate">FromDate</param>
        /// <param name="ToDate">ToDate</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>customer Gameplay count</returns>
        public DataTable GetCustomerGameplayCount(string SiteID, DateTime FromDate, DateTime ToDate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(SiteID, FromDate, ToDate, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetCustomerGameplayCount(SiteID, FromDate, ToDate);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        ///  Get customer Gameplay count
        /// </summary>
        /// <param name="formName">formName</param>
        /// <param name="reportId">reportId</param>
        /// <param name="siteId">siteId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>customer Gameplay count</returns>
        public int InsertManagementformaccess(string formName, int reportId, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(formName, reportId, siteId, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            int count = ReportsDataHandler.InsertManagementformaccess(formName, reportId, siteId);
            log.LogMethodExit(count);
            return count;
        }

        /// <summary>
        /// Gets report group list
        /// </summary>
        /// <param name="language">language</param>
        /// <param name="siteId">siteId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>report group list</returns>
        public DataTable GetMessageList(string language, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(language, siteId, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetMessageList(language, siteId);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        ///  IsManager method
        /// </summary>
        /// <param name="userid">userid</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> IsManager method</returns>
        public bool IsManager(string userid, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(userid, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            bool im = ReportsDataHandler.IsManager(userid);
            log.LogMethodExit(im);
            return im;
        }

        /// <summary>
        /// Returns Record from PosMachinereportLog
        /// </summary>
        /// <param name="POSMachineName">POSMachineName</param>
        /// <param name="site_id">site_id</param>
        /// <param name="role_id">role_id</param>
        /// <param name="reportid">reportid</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Record from PosMachinereportLog</returns>
        public DataTable GetToTimeByMachineName(string POSMachineName, string site_id, int role_id, int reportid, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(POSMachineName, site_id, role_id, reportid, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetToTimeByMachineName(POSMachineName, site_id, role_id, reportid);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns ToTime from PosMachinereportLog
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="site_id">site_id</param>
        /// <param name="role_id">role_id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public DataTable GetPOSReportTimeByID(int id, string site_id, int role_id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, site_id, role_id, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetPOSReportTimeByID(id, site_id, role_id);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// GetSiteListByOrgID method
        /// </summary>
        /// <param name="orgID">orgID</param>
        /// <param name="siteIds">siteIds</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>GetSiteListByOrgID method</returns>
        public DataTable GetSiteListByOrgID(int orgID, string siteIds, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(orgID, siteIds, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetSiteListByOrgID(orgID, siteIds);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        ///  GetConceptTypes Method
        /// </summary>
        /// <param name="segmentName">segmentName</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>GetConceptTypes Method</returns>
        public DataTable GetConceptTypes(string segmentName, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(segmentName, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetConceptTypes(segmentName);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// GetSiteDescription
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>GetSiteDescription</returns>
        public DataTable GetSiteDescription(string siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetSiteDescription(siteId);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        public bool IsReportMaxDateRangeValid(DateTime fromdate, DateTime todate, int maxDaterange)
        {
            bool isValid = true;
            if (maxDaterange != -1)
            {
                int dateRange = Common.GetTimeSpanInDays(fromdate, todate);
                if (dateRange > maxDaterange)
                {
                    isValid = false;
                }
            }
            return isValid;
        }


        public ReportsDTO GetReportsById(int reportId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reportId);
            ReportsDataHandler reportsDataHandler = new ReportsDataHandler(connectionString, sqlTransaction);
            ReportsDTO reportsDTO = reportsDataHandler.GetReportsById(reportId);
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
        /// <param name="sqlTransaction"></param>
        /// <returns>POS Machines by PaymentDate</returns>
        public DataTable GetPOSListByPaymentDate(string siteId, int roleId, string ENABLE_POS_FILTER_IN_TRX_REPORT, DateTime fromDate, DateTime toDate, int offset, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, roleId, ENABLE_POS_FILTER_IN_TRX_REPORT, fromDate, toDate, offset, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetPOSListByPaymentDate(siteId, roleId, ENABLE_POS_FILTER_IN_TRX_REPORT, fromDate, toDate, offset);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// GetUsersListFromTransaction
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>Users List by PaymentDate</returns>
        public DataTable GetUsersListByPaymentDate(string siteId, DateTime fromDate, DateTime toDate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, fromDate, toDate, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            DataTable dataTable = ReportsDataHandler.GetUsersListByPaymentDate(siteId, fromDate, toDate);
            log.LogMethodExit(dataTable);
            return dataTable;
        }


        /// <summary>
        /// GetReportDTOWithCustomKey
        /// </summary>
        /// <param name="reportKey"></param>
        /// <returns>ReportsDTO list with Custom/Non Custom Key.</returns>
        public List<ReportsDTO> GetReportDTOWithCustomKey(string reportKey)
        {
            log.LogMethodEntry(reportKey);
            List<ReportsDTO> ReportsDTOList;
            string customReportKey = reportKey + "Custom";
            List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> customSearchParameters = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
            customSearchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.REPORT_KEY, customReportKey));
            customSearchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, executionContext.SiteId.ToString()));
            ReportsDTOList = GetAllReports(customSearchParameters);

            if (ReportsDTOList != null && ReportsDTOList.Any() == false)
            {
                List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> reportSearchParameters = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
                reportSearchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.REPORT_KEY, reportKey));
                reportSearchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, executionContext.SiteId.ToString()));
                ReportsDTOList = GetAllReports(reportSearchParameters);
            }
            log.LogMethodExit(ReportsDTOList);
            return ReportsDTOList;
        }


        /// <summary>
        /// GetTableauDashboards 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>List of Tableau Reports DTO</returns>
        public List<ReportsDTO> GetTableauDashboards(int roleId)
        {
            log.LogMethodEntry();
            ReportsList reportsListBL = new ReportsList(executionContext);
            List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> searchParameters = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
            searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.IS_DASHBOARD, "true"));
            searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.IS_ACTIVE, "true"));
            searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, executionContext.GetIsCorporate() ? executionContext.GetSiteId().ToString() : "-1"));
            searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.DASHBOARD_TYPE, Enum.GetName(typeof(ReportsDTO.DashboardReportType), ReportsDTO.DashboardReportType.T).ToString()));
            List<ReportsDTO> reportsDTOList = reportsListBL.GetAllReports(searchParameters);

            List<ReportsDTO> allowedReportsDTOList = new List<ReportsDTO>();

            if (reportsDTOList != null && reportsDTOList.Any())
            {
                ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(executionContext);
                List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, roleId.ToString()));
                searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ISACTIVE, "1"));
                List<ManagementFormAccessDTO> managementFormAccessList = managementFormAccessListBL.GetManagementFormAccessDTOList(searchParams);
                List<ManagementFormAccessDTO> managementFormAccessDTOList = new List<ManagementFormAccessDTO>();

                if (reportsDTOList != null && reportsDTOList.Any())
                {
                    reportsDTOList = reportsDTOList.OrderBy(m => m.ReportName).ToList();
                    for (int i = 0; i < reportsDTOList.Count; i++)
                    {
                        var isreportsNotExist = managementFormAccessList.Find(m => m.FormName == reportsDTOList[i].ReportName && m.MainMenu == "Run Reports" && m.FunctionGroup == "Reports" && m.RoleId == roleId);
                        if (isreportsNotExist != null)
                        {
                            reportsDTOList[i].DBQuery = reportsDTOList[i].DBQuery.Replace("@tableauURL", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TABLEAU_DASHBOARDS_URL"));
                            allowedReportsDTOList.Add(reportsDTOList[i]);
                        }
                    }
                }
            }
            log.LogMethodExit(allowedReportsDTOList);
            return allowedReportsDTOList;
        }
        /// <summary>
        ///  Returns the List of ReportsDTO
        /// </summary>
        /// <param name="site_id">site_id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of ReportsDTO</returns>
        public List<ReportsDTO> GetHomePageReports(int site_id,int role_id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(site_id, sqlTransaction);
            ReportsDataHandler ReportsDataHandler = new ReportsDataHandler(sqlTransaction);
            List<ReportsDTO> reportsDTOList = ReportsDataHandler.GetHomePageReports(site_id,role_id);
            log.LogMethodExit(reportsDTOList);
            return reportsDTOList;
        }
    }
}

/********************************************************************************************
 * Project Name - Maintenance Job Data Handler
 * Description  - Data handler of the maintenance job class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        14-Jan-2016   Raghuveera          Created 
 *1.00        18-Jul-2016   Raghuveera          Modified 
 *2.70        07-Jul-2019   Dakshakh raj        Modified (Added SELECT_QUERY,GetSQLParameters)
 ********************************************************************************************/

using System;
using System.Text;
using System.Data;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Globalization;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Maintenance Job Data Handler - Handles insert, update and select of Maintenance Job Data objects
    /// </summary>

    public class MaintenanceJobDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Maint_ChecklistDetails ";
       
        /// <summary>
        /// Dictionary for searching Parameters for the Category object.
        /// </summary>


        private static readonly Dictionary<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string> DBSearchParameters = new Dictionary<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>
            {                
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.JOB_NAME, "MaintJobName"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.MAINT_JOB_ID, "MaintChklstdetId"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.TASK_ID, "MaintTaskId"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.TASK_NAME, "TaskName"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.ASSIGNED_TO, "AssignedUserId"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.ASSET_ID, "AssetId"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.ASSET_NAME, "AssetName"},                
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.SCHEDULE_FROM_DATE, "ChklstScheduleTime"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.SCHEDULE_TO_DATE, "ChklstScheduleTime"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.PAST_DUE_DATE, "DATEADD(D,DurationToComplete,ChklstScheduleTime)"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.STATUS, "Status"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.ACTIVE_FLAG, "IsActive"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.MAINT_SCHEDULE_ID, "MaintScheduleId"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.LAST_UPDATED_DATE, "LastUpdatedDate"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_TYPE_ID, "RequestType"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.PRIORITY, "Priority"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_FROM_DATE, "RequestDate"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_TO_DATE, "RequestDate"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.SITE_ID, "site_id"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUESTED_BY, "RequestedBy"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.JOB_TYPE, "MaintJobType"},
                {MaintenanceJobDTO.SearchByMaintenanceJobParameters.MASTER_ENTITY_ID,"MasterEntityId"},//Modification on 18-Jul-2016 for publish feature
            };

        /// <summary>
        /// Parameterized Constructor for MaintenanceJobDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>

        public MaintenanceJobDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MaintenanceJob Record.
        /// </summary>
        /// <param name="CategoryDTO">CategoryDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>

        private List<SqlParameter> GetSQLParameters(MaintenanceJobDTO maintenanceJobDTO, string loginId, int siteId)
        {
            
            log.LogMethodEntry(maintenanceJobDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@MaintChklstdetId", maintenanceJobDTO.MaintChklstdetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maintScheduleId", maintenanceJobDTO.MaintScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maintTaskId", maintenanceJobDTO.MaintTaskId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maintJobType", maintenanceJobDTO.MaintJobType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maintJobName", maintenanceJobDTO.MaintJobName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@chklstScheduleTime", string.IsNullOrEmpty(maintenanceJobDTO.ChklstScheduleTime) ? DateTime.MinValue : DateTime.ParseExact(maintenanceJobDTO.ChklstScheduleTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assignedTo", maintenanceJobDTO.AssignedTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assignedUserId", maintenanceJobDTO.AssignedUserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@departmentId", maintenanceJobDTO.DepartmentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", maintenanceJobDTO.Status,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@checklistCloseDate", string.IsNullOrEmpty(maintenanceJobDTO.ChecklistCloseDate) ? DateTime.MinValue : DateTime.ParseExact(maintenanceJobDTO.ChecklistCloseDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taskName", maintenanceJobDTO.TaskName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@validateTag", maintenanceJobDTO.ValidateTag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardNumber", maintenanceJobDTO.CardNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardId", maintenanceJobDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taskCardNumber", maintenanceJobDTO.TaskCardNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarksMandatory", maintenanceJobDTO.RemarksMandatory));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetId", maintenanceJobDTO.AssetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetName", maintenanceJobDTO.AssetName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetType", maintenanceJobDTO.AssetType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@assetGroupName", maintenanceJobDTO.AssetGroupName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@chklistValue", maintenanceJobDTO.ChklistValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@chklistRemarks", maintenanceJobDTO.ChklistRemarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sourceSystemId", maintenanceJobDTO.SourceSystemId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@durationToComplete", maintenanceJobDTO.DurationToComplete, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requestType", maintenanceJobDTO.RequestType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requestDate", string.IsNullOrEmpty(maintenanceJobDTO.RequestDate) ? DateTime.MinValue : DateTime.ParseExact(maintenanceJobDTO.RequestDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@priority", maintenanceJobDTO.Priority));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requestDetail", maintenanceJobDTO.RequestDetail));
            parameters.Add(dataAccessHandler.GetSQLParameter("@imageName", maintenanceJobDTO.ImageName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requestedBy", maintenanceJobDTO.RequestedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contactPhone", maintenanceJobDTO.ContactPhone));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contactEmailId", maintenanceJobDTO.ContactEmailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@resolution", maintenanceJobDTO.Resolution));
            parameters.Add(dataAccessHandler.GetSQLParameter("@comments", maintenanceJobDTO.Comments));
            parameters.Add(dataAccessHandler.GetSQLParameter("@repairCost", maintenanceJobDTO.RepairCost));
            parameters.Add(dataAccessHandler.GetSQLParameter("@docFileName", maintenanceJobDTO.DocFileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@attribute1", maintenanceJobDTO.Attribute1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", maintenanceJobDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", maintenanceJobDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@creationDate", string.IsNullOrEmpty(maintenanceJobDTO.CreationDate) ? DateTime.MinValue : DateTime.ParseExact(maintenanceJobDTO.CreationDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedDate", string.IsNullOrEmpty(maintenanceJobDTO.LastUpdatedDate) ? DateTime.MinValue : DateTime.ParseExact(maintenanceJobDTO.LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));

            
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the maintenance job record to the database
        /// </summary>
        /// <param name="maintenanceJobDTO">MaintenanceJobDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>

        public MaintenanceJobDTO InsertMaintenanceJob(MaintenanceJobDTO maintenanceJobDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(maintenanceJobDTO, loginId, siteId);
            string insertMaintenanceJobQuery = @"INSERT INTO[dbo].[Maint_ChecklistDetails]
                                             ( 
                                              MaintTaskId,
                                              MaintScheduleId,
                                              MaintJobName,
                                              MaintJobType,
                                              ChklstScheduleTime,
                                              AssignedTo,
                                              AssignedUserId,
                                              DepartmentId,
                                              Status,
                                              ChecklistCloseDate,
                                              TaskName,
                                              ValidateTag,
                                              CardNumber,
                                              CardId,
                                              TaskCardNumber,
                                              RemarksMandatory,
                                              AssetId,
                                              AssetName,
                                              AssetType,
                                              AssetGroupName,
                                              ChklistValue,
                                              ChklistRemarks,
                                              SourceSystemId,
                                              DurationToComplete,
                                              RequestType,
                                              RequestDate,
                                              Priority,
                                              RequestDetail,
                                              ImageName,
                                              RequestedBy,
                                              ContactPhone,
                                              ContactEmailId,
                                              Resolution,
                                              Comments,
                                              RepairCost,
                                              DocFileName,
                                              Attribute1,
                                              IsActive,
                                              CreatedBy,
                                              CreationDate,
                                              LastUpdatedBy,
                                              LastupdatedDate,
                                              Guid,
                                              site_id,
                                              MasterEntityId
                                              ) 
                                                values 
                                              (
                                              @maintTaskId,
                                              @maintScheduleId,
                                              @maintJobName,
                                              @maintJobType,
                                              @chklstScheduleTime,
                                              @assignedTo,
                                              @assignedUserId,
                                              @departmentId,
                                              @status,
                                              @checklistCloseDate,
                                              @taskName,
                                              @validateTag,
                                              @cardNumber,
                                              @cardId,
                                              @taskCardNumber,
                                              @remarksMandatory,
                                              @assetId,
                                              @assetName,
                                              @assetType,
                                              @assetGroupName,
                                              @chklistValue,
                                              @chklistRemarks,
                                              @sourceSystemId,
                                              @durationToComplete,
                                              @requestType,
                                              @requestDate,
                                              @priority,
                                              @requestDetail,
                                              @imageName,
                                              @requestedBy,
                                              @contactPhone,
                                              @contactEmailId,
                                              @resolution,
                                              @comments,
                                              @repairCost,
                                              @docFileName,
                                              @attribute1,
                                              @isActive,
                                              @createdBy,
                                              isNull(@creationDate,Getdate()),
                                              @lastUpdatedBy,
                                              isNull(@lastUpdatedDate,Getdate()),
                                              NewId(),
                                              @siteid,
                                              @masterEntityId
                                              )SELECT * FROM Maint_ChecklistDetails WHERE MaintChklstdetId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMaintenanceJobQuery, GetSQLParameters(maintenanceJobDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMaintenanceJobDTO(maintenanceJobDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting maintenanceJobDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(maintenanceJobDTO);
            return maintenanceJobDTO;
        }
        /// <summary>
        /// Updates the maintenance job record
        /// </summary>
        /// <param name="maintenanceJobDTO">MaintenanceJobDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>

        public MaintenanceJobDTO UpdateMaintenanceJob(MaintenanceJobDTO maintenanceJobDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(maintenanceJobDTO, loginId, siteId);
            string updateMaintenanceJobQuery = @"update Maint_ChecklistDetails 
                                         set MaintTaskId=@maintTaskId,
                                              MaintScheduleId=@maintScheduleId,
                                              MaintJobName=@maintJobName,
                                              MaintJobType=@maintJobType,
                                              ChklstScheduleTime=@chklstScheduleTime,
                                              AssignedTo=@assignedTo,
                                              AssignedUserId=@assignedUserId,
                                              DepartmentId=@departmentId,
                                              Status=@status,
                                              ChecklistCloseDate=@checklistCloseDate,
                                              TaskName=@taskName,
                                              ValidateTag=@validateTag,
                                              CardNumber=@cardNumber,
                                              CardId=@cardId,
                                              TaskCardNumber=@taskCardNumber,
                                              RemarksMandatory=@remarksMandatory,
                                              AssetId=@assetId,
                                              AssetName=@assetName,
                                              AssetType=@assetType,
                                              AssetGroupName=@assetGroupName,
                                              ChklistValue=@chklistValue,
                                              ChklistRemarks=@chklistRemarks,
                                              SourceSystemId=@sourceSystemId,
                                              DurationToComplete=@durationToComplete,
                                              RequestDate=@requestDate,
                                              RequestType=@requestType,
                                              Priority=@priority,
                                              RequestDetail=@requestDetail,
                                              ImageName=@imageName,
                                              RequestedBy=@requestedBy,
                                              ContactPhone=@contactPhone,
                                              ContactEmailId=@contactEmailId,
                                              Resolution=@resolution,
                                              Comments=@comments,
                                              RepairCost=@repairCost,
                                              DocFileName=@docFileName,
                                              Attribute1=@attribute1,
                                              IsActive = @isActive,
                                              LastUpdatedBy = @lastUpdatedBy, 
                                              LastupdatedDate = isnull(@lastUpdatedDate,Getdate()),
                                             -- site_id=@siteid,
                                             MasterEntityId=@masterEntityId
                                       where MaintChklstdetId = @MaintChklstdetId
                                       SELECT * FROM Maint_ChecklistDetails WHERE MaintChklstdetId = @MaintChklstdetId";
                                       
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMaintenanceJobQuery, GetSQLParameters(maintenanceJobDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMaintenanceJobDTO(maintenanceJobDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating mMintenanceJobDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(maintenanceJobDTO);
            return maintenanceJobDTO;
        }



        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="maintenanceJobDTO">MaintenanceJobDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        private void RefreshMaintenanceJobDTO(MaintenanceJobDTO maintenanceJobDTO, DataTable dt)
        {
            log.LogMethodEntry(maintenanceJobDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                maintenanceJobDTO.MaintChklstdetId = Convert.ToInt32(dt.Rows[0]["MaintChklstdetId"]);
                maintenanceJobDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? "" : Convert.ToDateTime(dataRow["LastupdatedDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                maintenanceJobDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? "" : Convert.ToDateTime(dataRow["CreationDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                maintenanceJobDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                maintenanceJobDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                maintenanceJobDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                maintenanceJobDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to MaintenanceJobDTO class type
        /// </summary>
        /// <param name="maintenanceJobDataRow">MaintenanceJobDTO DataRow</param>
        /// <returns>Returns MaintenanceJobDTO</returns>

        private MaintenanceJobDTO GetMaintenanceJobDTO(DataRow maintenanceJobDataRow)
        {
            log.LogMethodEntry(maintenanceJobDataRow);
            MaintenanceJobDTO maintenanceJobDataObject = new MaintenanceJobDTO(Convert.ToInt32(maintenanceJobDataRow["MaintChklstdetId"]),
                                            maintenanceJobDataRow["MaintScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobDataRow["MaintScheduleId"]),
                                            maintenanceJobDataRow["MaintTaskId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobDataRow["MaintTaskId"]),
                                            maintenanceJobDataRow["MaintJobName"].ToString(),
                                            maintenanceJobDataRow["MaintJobType"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobDataRow["MaintJobType"]),
                                            maintenanceJobDataRow["ChklstScheduleTime"] == DBNull.Value ? "" :Convert.ToDateTime(maintenanceJobDataRow["ChklstScheduleTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                            maintenanceJobDataRow["AssignedTo"] == DBNull.Value ? null : maintenanceJobDataRow["AssignedTo"].ToString(),
                                            maintenanceJobDataRow["AssignedUserId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobDataRow["AssignedUserId"]),
                                            maintenanceJobDataRow["DepartmentId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobDataRow["DepartmentId"]),
                                            maintenanceJobDataRow["Status"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobDataRow["Status"]),
                                            maintenanceJobDataRow["ChecklistCloseDate"] == DBNull.Value ? "" : Convert.ToDateTime(maintenanceJobDataRow["ChecklistCloseDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                            maintenanceJobDataRow["TaskName"].ToString(),
                                            maintenanceJobDataRow["ValidateTag"].ToString(),
                                            maintenanceJobDataRow["CardNumber"].ToString(),
                                            maintenanceJobDataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobDataRow["CardId"]),
                                            maintenanceJobDataRow["TaskCardNumber"].ToString(),
                                            maintenanceJobDataRow["RemarksMandatory"].ToString(),
                                            maintenanceJobDataRow["AssetId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobDataRow["AssetId"]),
                                            maintenanceJobDataRow["AssetName"].ToString(),
                                            maintenanceJobDataRow["AssetType"].ToString(),
                                            maintenanceJobDataRow["AssetGroupName"].ToString(),
                                            maintenanceJobDataRow["ChklistValue"].ToString(),
                                            maintenanceJobDataRow["ChklistRemarks"].ToString(),
                                            maintenanceJobDataRow["SourceSystemId"].ToString(),
                                            maintenanceJobDataRow["DurationToComplete"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobDataRow["DurationToComplete"]),
                                            maintenanceJobDataRow["RequestType"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobDataRow["RequestType"]),
                                            maintenanceJobDataRow["RequestDate"]== DBNull.Value ? "" :Convert.ToDateTime(maintenanceJobDataRow["RequestDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                            maintenanceJobDataRow["Priority"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobDataRow["Priority"]),
                                            maintenanceJobDataRow["RequestDetail"].ToString(),
                                            maintenanceJobDataRow["ImageName"].ToString(),
                                            maintenanceJobDataRow["RequestedBy"].ToString(),
                                            maintenanceJobDataRow["ContactPhone"].ToString(),
                                            maintenanceJobDataRow["ContactEmailId"].ToString(),
                                            maintenanceJobDataRow["Resolution"].ToString(),
                                            maintenanceJobDataRow["Comments"].ToString(),
                                            maintenanceJobDataRow["RepairCost"] == DBNull.Value ? -1 : Convert.ToDouble(maintenanceJobDataRow["RepairCost"]),
                                            maintenanceJobDataRow["DocFileName"].ToString(),
                                            maintenanceJobDataRow["Attribute1"].ToString(),
                                            maintenanceJobDataRow["IsActive"].ToString(),
                                            maintenanceJobDataRow["CreatedBy"].ToString(),
                                            maintenanceJobDataRow["CreationDate"] == DBNull.Value ? "" : Convert.ToDateTime(maintenanceJobDataRow["CreationDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                            maintenanceJobDataRow["LastUpdatedBy"].ToString(),
                                            maintenanceJobDataRow["LastupdatedDate"] == DBNull.Value ? "" : Convert.ToDateTime(maintenanceJobDataRow["LastupdatedDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                            maintenanceJobDataRow["Guid"].ToString(),
                                            maintenanceJobDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobDataRow["site_id"]),
                                            maintenanceJobDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(maintenanceJobDataRow["SynchStatus"]),
                                            maintenanceJobDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(maintenanceJobDataRow["MasterEntityId"]));//Modification on 18-Jul-2016 for adding publish to site
            log.LogMethodExit(maintenanceJobDataObject);
            return maintenanceJobDataObject;
        }

        /// <summary>
        /// Gets the maintenance job data of passed maintenance job Id
        /// </summary>
        /// <param name="MaintTaskId">integer type parameter</param>
        /// <returns>Returns MaintenanceJobDTO</returns>

        public MaintenanceJobDTO GetMaintenanceJob(int maintJobId)
        {
            log.LogMethodEntry(maintJobId);
            MaintenanceJobDTO result = null;
            string selectMaintenanceJobQuery = SELECT_QUERY + @" WHERE MaintChklstdetId = @MaintChklstdetId";
            SqlParameter parameter = new SqlParameter("@MaintChklstdetId", maintJobId);
            DataTable maintenanceJob = dataAccessHandler.executeSelectQuery(selectMaintenanceJobQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (maintenanceJob.Rows.Count > 0)
            {
                result = GetMaintenanceJobDTO(maintenanceJob.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Returns the List of CategoryDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of CategoryDTO</returns>

        public List<MaintenanceJobDTO> GetMaintenanceJobList(List<KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<MaintenanceJobDTO> maintenanceJobDTOList = new List<MaintenanceJobDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.MAINT_JOB_ID
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.MAINT_JOB_ID
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.ASSET_ID
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_TYPE_ID
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.TASK_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.TASK_NAME
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.ASSET_NAME
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.JOB_NAME
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUESTED_BY
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.ASSIGNED_TO)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));// == "1" ? 'Y' : 'N'));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.STATUS
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.PRIORITY
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.JOB_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.LAST_UPDATED_DATE
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.PAST_DUE_DATE
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_FROM_DATE
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_TO_DATE
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SCHEDULE_FROM_DATE
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SCHEDULE_TO_DATE)
                        
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MaintenanceJobDTO maintenanceJobDTO = GetMaintenanceJobDTO(dataRow);
                    maintenanceJobDTOList.Add(maintenanceJobDTO);
                }
            }
            log.LogMethodExit(maintenanceJobDTOList);
            return maintenanceJobDTOList;
        }


        /// <summary>
        /// Gets the MaintenanceJobDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="userId"></param>
        /// <returns>Returns the list of MaintenanceJobDTO matching the search criteria</returns>

        public List<MaintenanceJobDTO> GetMaintenanceJobList(List<KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>> searchParameters, int userId)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();

            string selectMaintenanceJobQuery = @"SELECT Job.*  FROM Maint_ChecklistDetails Job ,
                                                   (SELECT user_id,loginid,role_id FROM users WHERE (USER_ID=@UserId
                                                    OR managerId = @UserId ) AND active_flag = 'Y') Users
                                                    WHERE ((Users.user_id = Job.AssignedUserId and users.user_id = @UserId)
                                                    OR (Job.CreatedBy=Users.loginid and users.user_id = @UserId)or role_id=(select role_id 
                                                        from user_roles where role='System Administrator' and (site_id= @siteId OR @siteId =-1)))";
            //SqlParameter[] maintenanceJobParameter = new SqlParameter[2];
            //maintenanceJobParameter[0] = new SqlParameter("@UserId", userId);
            //maintenanceJobParameter[1] = new SqlParameter("@siteId", -1);
            parameters.Add(new SqlParameter("@UserId", userId));
            //parameters.Add(new SqlParameter("@siteId", -1));

            if (searchParameters != null)
            {
                string joiner = " ";//Modification on 18-Jul-2016 for adding masterEntityId
                StringBuilder query = new StringBuilder();
                count = 1;
                foreach (KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.ASSET_ID || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.MAINT_JOB_ID
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.MAINT_SCHEDULE_ID || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_TYPE_ID || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.TASK_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SCHEDULE_FROM_DATE || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_FROM_DATE)
                        {
                           query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                           parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.LAST_UPDATED_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) > " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SCHEDULE_TO_DATE || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.PAST_DUE_DATE)
                        {
                             query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), "GETDATE()"));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SITE_ID)
                        {
                            parameters.Add(new SqlParameter("@siteId", searchParameter.Value));
                            //query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                query.Append(" ORDER BY AssetName");
                if (searchParameters.Count > 0)
                    selectMaintenanceJobQuery = selectMaintenanceJobQuery + query;
            }

            DataTable maintenanceJobData = dataAccessHandler.executeSelectQuery(selectMaintenanceJobQuery, parameters.ToArray(),sqlTransaction);
            if (maintenanceJobData.Rows.Count > 0)
            {
                List<MaintenanceJobDTO> maintenanceJobList = new List<MaintenanceJobDTO>();
                foreach (DataRow maintenanceJobDataRow in maintenanceJobData.Rows)
                {
                    MaintenanceJobDTO maintenanceJobDataObject = GetMaintenanceJobDTO(maintenanceJobDataRow);
                    maintenanceJobList.Add(maintenanceJobDataObject);
                }
                log.LogMethodExit(maintenanceJobList);
                return maintenanceJobList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the MaintenanceJobDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MaintenanceJobDTO matching the search criteria</returns>

        public List<MaintenanceJobDTO> GetAllMaintenanceWithHQPublishedJobList(List<KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectMaintenanceJobQuery = @"SELECT Srvreq.MaintChklstdetId,Srvreq.MaintScheduleId,Srvreq.MaintTaskId,Srvreq.MaintJobType,Srvreq.
                                                    MaintJobName,Srvreq.ChklstScheduleTime,Srvreq.AssignedTo, (usr.MasterEntityId) as AssignedUserId,
                                                    Srvreq.DepartmentId,Srvreq.Status,Srvreq.ChecklistCloseDate,Srvreq.TaskName,
                                                    Srvreq.ValidateTag,Srvreq.CardNumber,Srvreq.CardId,Srvreq.TaskCardNumber,Srvreq.RemarksMandatory,
                                                    Srvreq.AssetId,Srvreq.AssetName,Srvreq.AssetType,Srvreq.AssetGroupName,Srvreq.ChklistValue,
                                                    Srvreq.ChklistRemarks,Srvreq.SourceSystemId,Srvreq.DurationToComplete,Srvreq.RequestType,Srvreq.RequestDate,
                                                    Srvreq.Priority,Srvreq.RequestDetail,Srvreq.ImageName,Srvreq.RequestedBy,Srvreq.ContactPhone,Srvreq.ContactEmailId,
                                                    Srvreq.Resolution,Srvreq.Comments,Srvreq.RepairCost,Srvreq.DocFileName,Srvreq.Attribute1,Srvreq.IsActive,
                                                    Srvreq.CreatedBy,Srvreq.CreationDate,Srvreq.LastUpdatedBy,Srvreq.LastupdatedDate,Srvreq.Guid,Srvreq.site_id,
                                                    Srvreq.SynchStatus,Srvreq.MasterEntityId  
                                                    FROM Maint_ChecklistDetails Srvreq join users usr on AssignedUserId=user_id where usr.masterEntityId is not null ";

            if (searchParameters != null)
            {
                string joiner = " ";//Modification on 18-Jul-2016 for adding masterEntityId
                StringBuilder query = new StringBuilder();//" Where "
                //count = 1;
                foreach (KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " and " : " and ";
                        if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.ASSET_ID || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.MAINT_JOB_ID
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.MAINT_SCHEDULE_ID || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_TYPE_ID || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.TASK_ID)
                        {
                            query.Append(joiner + "Srvreq." + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SCHEDULE_FROM_DATE || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_FROM_DATE)
                        {
                            query.Append(joiner + "Srvreq." + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.LAST_UPDATED_DATE)
                        {
                            query.Append(joiner + "Srvreq." + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) > " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SCHEDULE_TO_DATE || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_TO_DATE)
                        {
                            query.Append(joiner + "Srvreq." + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.PAST_DUE_DATE)
                        {
                            query.Append(joiner + "Srvreq." + DBSearchParameters[searchParameter.Key] + "< GetDate()");
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + "Srvreq." + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));

                            //query.Append(joiner + "(" + "Srvreq." + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + "Srvreq." + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }


                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                query.Append(" ORDER BY Srvreq.AssetName");
                if (searchParameters.Count > 0)
                    selectMaintenanceJobQuery = selectMaintenanceJobQuery + query;
            }

            DataTable maintenanceJobData = dataAccessHandler.executeSelectQuery(selectMaintenanceJobQuery, parameters.ToArray(), sqlTransaction);
            if (maintenanceJobData.Rows.Count > 0)
            {
                List<MaintenanceJobDTO> maintenanceJobList = new List<MaintenanceJobDTO>();
                foreach (DataRow maintenanceJobDataRow in maintenanceJobData.Rows)
                {
                    MaintenanceJobDTO maintenanceJobDataObject = GetMaintenanceJobDTO(maintenanceJobDataRow);
                    maintenanceJobList.Add(maintenanceJobDataObject);
                }
                log.LogMethodExit(maintenanceJobList);
                return maintenanceJobList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }


        /// <summary>
        /// Gets the MaintenanceJobDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MaintenanceJobDTO matching the search criteria</returns>

        public List<MaintenanceJobDTO> GetAllMaintenanceJobList(List<KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectMaintenanceJobQuery = @"SELECT *  FROM Maint_ChecklistDetails";
            
            if (searchParameters != null)
            {
                string joiner = " ";//Modification on 18-Jul-2016 for adding masterEntityId
                StringBuilder query = new StringBuilder(" Where ");
                //count = 1;
                foreach (KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.ASSET_ID || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.MAINT_JOB_ID
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.MAINT_SCHEDULE_ID || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_TYPE_ID || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.TASK_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SCHEDULE_FROM_DATE || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.LAST_UPDATED_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) > " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SCHEDULE_TO_DATE || searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.REQUEST_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.PAST_DUE_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), "GETDATE()"));
                        }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }


                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                query.Append(" ORDER BY AssetName");
                if (searchParameters.Count > 0)
                    selectMaintenanceJobQuery = selectMaintenanceJobQuery + query;
            }

            DataTable maintenanceJobData = dataAccessHandler.executeSelectQuery(selectMaintenanceJobQuery,parameters.ToArray(),sqlTransaction);
            if (maintenanceJobData.Rows.Count > 0)
            {
                List<MaintenanceJobDTO> maintenanceJobList = new List<MaintenanceJobDTO>();
                foreach (DataRow maintenanceJobDataRow in maintenanceJobData.Rows)
                {
                    MaintenanceJobDTO maintenanceJobDataObject = GetMaintenanceJobDTO(maintenanceJobDataRow);
                    maintenanceJobList.Add(maintenanceJobDataObject);
                }
                log.LogMethodEntry(maintenanceJobList);
                return maintenanceJobList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the MaintenanceJobDTO list in a batch matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="userId">Logged in User Id</param>
        /// <param name="maxRows">Maximum rows to be fetched in one call</param>
        /// <returns>Returns the list of MaintenanceJobDTO in batch matching the search criteria</returns>

        public List<MaintenanceJobDTO> GetMaintenanceJobListBatch(List<KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>> searchParameters, int maxRows)
        {
            log.Debug("Starts-GetMaintenanceJobListBatch(searchParameters) Method.");
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectMaintenanceJobQuery = @"SELECT TOP (@maxRows)
                                                        Job.*  
                                                   FROM Maint_ChecklistDetails Job";
            SqlParameter[] maintenanceJobParameter = new SqlParameter[1];
            maintenanceJobParameter[0] = new SqlParameter("@maxRows", maxRows);
            parameters.Add(new SqlParameter("@maxRows", maxRows));
            if (searchParameters != null)
            {
                string joiner;
                StringBuilder query = new StringBuilder(" Where ");
                foreach (KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        
                        
                            if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SCHEDULE_FROM_DATE)
                            {
                                query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            }
                            else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.LAST_UPDATED_DATE)
                            {
                                query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) > " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            }
                            else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.MAINT_JOB_ID)
                            {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                        else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.SCHEDULE_TO_DATE)
                            {
                                query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            }
                            else if (searchParameter.Key == MaintenanceJobDTO.SearchByMaintenanceJobParameters.PAST_DUE_DATE)
                            {
                                query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), "GETDATE()"));
                            }
                            else
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }

                        
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                query.Append(" ORDER BY " + DBSearchParameters[MaintenanceJobDTO.SearchByMaintenanceJobParameters.MAINT_JOB_ID]);
                if (searchParameters.Count > 0)
                    selectMaintenanceJobQuery = selectMaintenanceJobQuery + query;
            }

            DataTable maintenanceJobData = dataAccessHandler.executeSelectQuery(selectMaintenanceJobQuery,parameters.ToArray(),sqlTransaction);
            if (maintenanceJobData.Rows.Count > 0)
            {
                List<MaintenanceJobDTO> maintenanceJobList = new List<MaintenanceJobDTO>();
                foreach (DataRow maintenanceJobDataRow in maintenanceJobData.Rows)
                {
                    MaintenanceJobDTO maintenanceJobDataObject = GetMaintenanceJobDTO(maintenanceJobDataRow);
                    maintenanceJobList.Add(maintenanceJobDataObject);
                }
                log.LogMethodExit(maintenanceJobList);
                return maintenanceJobList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
    }
}

/********************************************************************************************
 * Project Name - User Job Items Data Handler
 * Description  - Data handler of the User job items class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Jan-2016   Raghuveera     Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera     Modified 
 *2.70        08-Mar-2019   Guru S A       Renamed MaintenanceJobDataHandler as UserJobItemsDatahandler
 *2.70.2        13-Nov-2019   Guru S A       Waiver phase 2 changes
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query                                                          
 *2.100.0     30-Sept-2020  Mushahid Faizan Modified for Service Request Enhancement.
 ********************************************************************************************/

 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// User Job Data Handler - Handles insert, update and select of User Job Data objects
    /// </summary>
    public class UserJobItemsDatahandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<UserJobItemsDTO.SearchByUserJobItemsParameters, string> DBSearchParameters = new Dictionary<UserJobItemsDTO.SearchByUserJobItemsParameters, string>
            {
                {UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_NAME, "job.MaintJobName"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.USER_JOB_ID, "job.MaintChklstdetId"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.TASK_ID, "job.MaintTaskId"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.TASK_NAME, "job.TaskName"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.ASSIGNED_TO, "job.AssignedUserId"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.ASSET_ID, "job.AssetId"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.ASSET_NAME, "job.AssetName"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_FROM_DATE, "job.ChklstScheduleTime"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_TO_DATE, "job.ChklstScheduleTime"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.PAST_DUE_DATE, "DATEADD(D,job.DurationToComplete,job.ChklstScheduleTime)"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.STATUS, "job.Status"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.IS_ACTIVE, "job.IsActive"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_SCHEDULE_ID, "job.MaintScheduleId"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.LAST_UPDATED_DATE, "job.LastUpdatedDate"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TYPE_ID, "job.RequestType"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.PRIORITY, "job.Priority"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_FROM_DATE, "job.RequestDate"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TO_DATE, "job.RequestDate"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID, "job.site_id"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.REQUESTED_BY, "job.RequestedBy"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_TYPE, "job.MaintJobType"},
                {UserJobItemsDTO.SearchByUserJobItemsParameters.MASTER_ENTITY_ID,"job.MasterEntityId"},//Modification on 18-Jul-2016 for publish feature
                {UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_NUMBER,"job.MaintJobNumber"}//Modification on 18-Jul-2016 for publish feature
            };
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private string selectQry = @"select job.*, mst.BookingId, mst.BookingCheckListId
                                       from Maint_ChecklistDetails job 
	                                        left outer join Maint_SchAssetTasks mst on mst.MaintSchAssetTaskId = job.MaintSchAssetTaskId";
        /// <summary>
        /// Default constructor of UserJobItemDatahandler class
        /// </summary>
        public UserJobItemsDatahandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Inserts the user job record to the database
        /// </summary>
        /// <param name="userJobItemDTO">UserJobItemsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertUserJobItems(UserJobItemsDTO userJobItemDTO, string userId, int siteId)
        {
            log.LogMethodEntry(userJobItemDTO, userId, siteId);
            string insertUserJobQuery = @"insert into Maint_ChecklistDetails 
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
                                              MasterEntityId,
                                              MaintSchAssetTaskId,
                                              MaintJobNumber
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
                                              Getdate(),
                                              @lastUpdatedBy,
                                              Getdate(),
                                              NewId(),
                                              @siteid, 
                                              @masterEntityId,
                                              @jobScheduleTaskId,
                                              @maintJobNumber
                                              )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateMaintenanceJobParameters = new List<SqlParameter>();

            if (userJobItemDTO.JobTaskId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintTaskId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintTaskId", userJobItemDTO.JobTaskId));
            }
            if (userJobItemDTO.JobScheduleId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintScheduleId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintScheduleId", userJobItemDTO.JobScheduleId));
            }

            updateMaintenanceJobParameters.Add(new SqlParameter("@maintJobName", userJobItemDTO.MaintJobName));
            if (userJobItemDTO.MaintJobType == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintJobType", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintJobType", userJobItemDTO.MaintJobType));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.ChklstScheduleTime))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@chklstScheduleTime", DBNull.Value));
            }
            else
            {

                updateMaintenanceJobParameters.Add(new SqlParameter("@chklstScheduleTime", DateTime.Parse(userJobItemDTO.ChklstScheduleTime, CultureInfo.InvariantCulture)));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.AssignedTo))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assignedTo", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assignedTo", userJobItemDTO.AssignedTo));
            }
            if (userJobItemDTO.AssignedUserId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assignedUserId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assignedUserId", userJobItemDTO.AssignedUserId));
            }
            if (userJobItemDTO.DepartmentId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@departmentId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@departmentId", userJobItemDTO.DepartmentId));
            }
            if (userJobItemDTO.Status == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@status", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@status", userJobItemDTO.Status));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.ChecklistCloseDate))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@checklistCloseDate", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@checklistCloseDate", DateTime.Parse(userJobItemDTO.ChecklistCloseDate, CultureInfo.InvariantCulture)));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.TaskName))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@taskName", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@taskName", userJobItemDTO.TaskName));
            }
            updateMaintenanceJobParameters.Add(new SqlParameter("@validateTag", (userJobItemDTO.ValidateTag == true ? "Y" : "N")));
            if (string.IsNullOrEmpty(userJobItemDTO.CardNumber))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@cardNumber", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@cardNumber", userJobItemDTO.CardNumber));
            }
            if (userJobItemDTO.CardId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@cardId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@cardId", userJobItemDTO.CardId));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.TaskCardNumber))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@taskCardNumber", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@taskCardNumber", userJobItemDTO.TaskCardNumber));
            }
            updateMaintenanceJobParameters.Add(new SqlParameter("@remarksMandatory", (userJobItemDTO.RemarksMandatory == true ? "Y" : "N")));
            if (userJobItemDTO.AssetId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetId", userJobItemDTO.AssetId));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.AssetName))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetName", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetName", userJobItemDTO.AssetName));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.AssetType))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetType", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetType", userJobItemDTO.AssetType));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.AssetGroupName))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetGroupName", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetGroupName", userJobItemDTO.AssetGroupName));
            }

            updateMaintenanceJobParameters.Add(new SqlParameter("@chklistValue", (userJobItemDTO.ChklistValue == true ? "Y" : "N")));

            if (string.IsNullOrEmpty(userJobItemDTO.ChklistRemarks))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@chklistRemarks", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@chklistRemarks", userJobItemDTO.ChklistRemarks));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.SourceSystemId))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@sourceSystemId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@sourceSystemId", userJobItemDTO.SourceSystemId));
            }
            if (userJobItemDTO.DurationToComplete == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@durationToComplete", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@durationToComplete", userJobItemDTO.DurationToComplete));
            }
            if (userJobItemDTO.RequestType == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestType", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestType", userJobItemDTO.RequestType));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.RequestDate))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestDate", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestDate", DateTime.Parse(userJobItemDTO.RequestDate, CultureInfo.InvariantCulture)));
            }

            if (userJobItemDTO.Priority == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@priority", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@priority", userJobItemDTO.Priority));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.RequestDetail))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestDetail", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestDetail", userJobItemDTO.RequestDetail));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.ImageName))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@imageName", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@imageName", userJobItemDTO.ImageName));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.RequestedBy))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestedBy", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestedBy", userJobItemDTO.RequestedBy));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.ContactPhone))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@contactPhone", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@contactPhone", userJobItemDTO.ContactPhone));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.ContactEmailId))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@contactEmailId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@contactEmailId", userJobItemDTO.ContactEmailId));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.Resolution))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@resolution", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@resolution", userJobItemDTO.Resolution));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.Comments))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@comments", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@comments", userJobItemDTO.Comments));
            }

            if (userJobItemDTO.RepairCost == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@repairCost", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@repairCost", userJobItemDTO.RepairCost));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.DocFileName))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@docFileName", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@docFileName", userJobItemDTO.DocFileName));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.Attribute1))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@attribute1", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@attribute1", userJobItemDTO.Attribute1));
            }
            updateMaintenanceJobParameters.Add(new SqlParameter("@isActive", (userJobItemDTO.IsActive == true ? "Y" : "N")));
            if (string.IsNullOrEmpty(userJobItemDTO.CreatedBy))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@createdBy", userId));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@createdBy", userJobItemDTO.CreatedBy));
            }

            if (string.IsNullOrEmpty(userJobItemDTO.LastUpdatedBy))
            {

                updateMaintenanceJobParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@lastUpdatedBy", userJobItemDTO.LastUpdatedBy));
            }
            //if (maintenanceJob.Siteid != -1)
            //{
            //    siteId = maintenanceJob.Siteid;
            //}
            if (siteId == -1)
                updateMaintenanceJobParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateMaintenanceJobParameters.Add(new SqlParameter("@siteId", siteId));
            if (userJobItemDTO.MasterEntityId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@masterEntityId", userJobItemDTO.MasterEntityId));
            }
            if (userJobItemDTO.JobScheduleTaskId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@jobScheduleTaskId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@jobScheduleTaskId", userJobItemDTO.JobScheduleTaskId));
            }
            if (string.IsNullOrEmpty(userJobItemDTO.MaintJobNumber))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintJobNumber", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintJobNumber", userJobItemDTO.MaintJobNumber));
            }
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertUserJobQuery, updateMaintenanceJobParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the user job items record
        /// </summary>
        /// <param name="userJobItemsDTO">userJobItemsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateUserJobItems(UserJobItemsDTO userJobItemsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(userJobItemsDTO, userId, siteId);
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
                                              LastupdatedDate = getdate(),
                                              MaintJobNumber =@maintJobNumber,
                                             -- site_id=@siteid, 
                                             MasterEntityId=@masterEntityId,
                                             MaintschAssetTaskId = @jobScheduleTaskId
                                       where MaintChklstdetId = @maintChklstdetId";
            List<SqlParameter> updateMaintenanceJobParameters = new List<SqlParameter>();
            updateMaintenanceJobParameters.Add(new SqlParameter("@maintChklstdetId", userJobItemsDTO.MaintChklstdetId));
            if (userJobItemsDTO.JobTaskId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintTaskId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintTaskId", userJobItemsDTO.JobTaskId));
            }
            if (userJobItemsDTO.JobScheduleId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintScheduleId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintScheduleId", userJobItemsDTO.JobScheduleId));
            }
            updateMaintenanceJobParameters.Add(new SqlParameter("@maintJobName", userJobItemsDTO.MaintJobName));
            if (userJobItemsDTO.MaintJobType == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintJobType", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintJobType", userJobItemsDTO.MaintJobType));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.ChklstScheduleTime))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@chklstScheduleTime", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@chklstScheduleTime", DateTime.Parse(userJobItemsDTO.ChklstScheduleTime, CultureInfo.InvariantCulture)));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.AssignedTo))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assignedTo", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assignedTo", userJobItemsDTO.AssignedTo));
            }
            if (userJobItemsDTO.AssignedUserId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assignedUserId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assignedUserId", userJobItemsDTO.AssignedUserId));
            }
            if (userJobItemsDTO.DepartmentId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@departmentId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@departmentId", userJobItemsDTO.DepartmentId));
            }
            if (userJobItemsDTO.Status == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@status", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@status", userJobItemsDTO.Status));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.ChecklistCloseDate))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@checklistCloseDate", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@checklistCloseDate", DateTime.Parse(userJobItemsDTO.ChecklistCloseDate, CultureInfo.InvariantCulture)));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.TaskName))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@taskName", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@taskName", userJobItemsDTO.TaskName));
            }
            updateMaintenanceJobParameters.Add(new SqlParameter("@validateTag", (userJobItemsDTO.ValidateTag == true ? "Y" : "N")));
            if (string.IsNullOrEmpty(userJobItemsDTO.CardNumber))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@cardNumber", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@cardNumber", userJobItemsDTO.CardNumber));
            }
            if (userJobItemsDTO.CardId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@cardId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@cardId", userJobItemsDTO.CardId));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.TaskCardNumber))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@taskCardNumber", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@taskCardNumber", userJobItemsDTO.TaskCardNumber));
            }
            updateMaintenanceJobParameters.Add(new SqlParameter("@remarksMandatory", (userJobItemsDTO.RemarksMandatory == true ? "Y" : "N")));

            if (userJobItemsDTO.AssetId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetId", userJobItemsDTO.AssetId));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.AssetName))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetName", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetName", userJobItemsDTO.AssetName));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.AssetType))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetType", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetType", userJobItemsDTO.AssetType));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.AssetGroupName))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetGroupName", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@assetGroupName", userJobItemsDTO.AssetGroupName));
            }
            updateMaintenanceJobParameters.Add(new SqlParameter("@chklistValue", (userJobItemsDTO.ChklistValue == true ? "Y" : "N")));
            if (string.IsNullOrEmpty(userJobItemsDTO.ChklistRemarks))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@chklistRemarks", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@chklistRemarks", userJobItemsDTO.ChklistRemarks));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.SourceSystemId))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@sourceSystemId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@sourceSystemId", userJobItemsDTO.SourceSystemId));
            }
            if (userJobItemsDTO.DurationToComplete == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@durationToComplete", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@durationToComplete", userJobItemsDTO.DurationToComplete));
            }
            if (userJobItemsDTO.RequestType == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestType", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestType", userJobItemsDTO.RequestType));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.RequestDate))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestDate", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestDate", DateTime.Parse(userJobItemsDTO.RequestDate, CultureInfo.InvariantCulture)));
            }

            if (userJobItemsDTO.Priority == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@priority", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@priority", userJobItemsDTO.Priority));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.RequestDetail))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestDetail", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestDetail", userJobItemsDTO.RequestDetail));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.ImageName))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@imageName", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@imageName", userJobItemsDTO.ImageName));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.RequestedBy))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestedBy", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@requestedBy", userJobItemsDTO.RequestedBy));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.ContactPhone))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@contactPhone", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@contactPhone", userJobItemsDTO.ContactPhone));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.ContactEmailId))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@contactEmailId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@contactEmailId", userJobItemsDTO.ContactEmailId));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.Resolution))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@resolution", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@resolution", userJobItemsDTO.Resolution));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.Comments))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@comments", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@comments", userJobItemsDTO.Comments));
            }
            if (userJobItemsDTO.RepairCost == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@repairCost", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@repairCost", userJobItemsDTO.RepairCost));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.DocFileName))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@docFileName", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@docFileName", userJobItemsDTO.DocFileName));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.Attribute1))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@attribute1", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@attribute1", userJobItemsDTO.Attribute1));
            }
            updateMaintenanceJobParameters.Add(new SqlParameter("@isActive", (userJobItemsDTO.IsActive == true ? "Y" : "N")));
            updateMaintenanceJobParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateMaintenanceJobParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateMaintenanceJobParameters.Add(new SqlParameter("@siteId", siteId));

            if (userJobItemsDTO.MasterEntityId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@masterEntityId", userJobItemsDTO.MasterEntityId));
            }
            if (userJobItemsDTO.JobScheduleTaskId == -1)
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@jobScheduleTaskId", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@jobScheduleTaskId", userJobItemsDTO.JobScheduleTaskId));
            }
            if (string.IsNullOrEmpty(userJobItemsDTO.MaintJobNumber))
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintJobNumber", DBNull.Value));
            }
            else
            {
                updateMaintenanceJobParameters.Add(new SqlParameter("@maintJobNumber", userJobItemsDTO.MaintJobNumber));
            }
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateMaintenanceJobQuery, updateMaintenanceJobParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }
        /// <summary>
        /// Converts the Data row object to UserJobItemsDTO class type
        /// </summary>
        /// <param name="userJobItemsDataRow">UserJobItemsDTO DataRow</param>
        /// <returns>Returns UserJobItemsDTO</returns>
        private UserJobItemsDTO GetUserJobItemsDTO(DataRow userJobItemsDataRow)
        {
            log.LogMethodEntry(userJobItemsDataRow);
            UserJobItemsDTO userJobItemsDataObject = new UserJobItemsDTO(Convert.ToInt32(userJobItemsDataRow["MaintChklstdetId"]),
                                            userJobItemsDataRow["MaintScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["MaintScheduleId"]),
                                            userJobItemsDataRow["MaintTaskId"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["MaintTaskId"]),
                                            userJobItemsDataRow["MaintJobName"].ToString(),
                                            userJobItemsDataRow["MaintJobType"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["MaintJobType"]),
                                            userJobItemsDataRow["ChklstScheduleTime"] == DBNull.Value ? "" : Convert.ToDateTime(userJobItemsDataRow["ChklstScheduleTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                            userJobItemsDataRow["AssignedTo"] == DBNull.Value ? null : userJobItemsDataRow["AssignedTo"].ToString(),
                                            userJobItemsDataRow["AssignedUserId"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["AssignedUserId"]),
                                            userJobItemsDataRow["DepartmentId"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["DepartmentId"]),
                                            userJobItemsDataRow["Status"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["Status"]),
                                            userJobItemsDataRow["ChecklistCloseDate"] == DBNull.Value ? "" : Convert.ToDateTime(userJobItemsDataRow["ChecklistCloseDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                            userJobItemsDataRow["TaskName"].ToString(),
                                            (userJobItemsDataRow["ValidateTag"] == DBNull.Value ? false : (userJobItemsDataRow["ValidateTag"].ToString() == "Y" ? true : false)),
                                            userJobItemsDataRow["CardNumber"].ToString(),
                                            userJobItemsDataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["CardId"]),
                                            userJobItemsDataRow["TaskCardNumber"].ToString(),
                                            (userJobItemsDataRow["RemarksMandatory"] == DBNull.Value ? false : (userJobItemsDataRow["RemarksMandatory"].ToString() == "Y" ? true : false)),
                                            userJobItemsDataRow["AssetId"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["AssetId"]),
                                            userJobItemsDataRow["AssetName"].ToString(),
                                            userJobItemsDataRow["AssetType"].ToString(),
                                            userJobItemsDataRow["AssetGroupName"].ToString(),
                                            (userJobItemsDataRow["ChklistValue"] == DBNull.Value ? false : (userJobItemsDataRow["ChklistValue"].ToString() == "Y" ? true : false)),
                                            userJobItemsDataRow["ChklistRemarks"].ToString(),
                                            userJobItemsDataRow["SourceSystemId"].ToString(),
                                            userJobItemsDataRow["DurationToComplete"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["DurationToComplete"]),
                                            userJobItemsDataRow["RequestType"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["RequestType"]),
                                            userJobItemsDataRow["RequestDate"] == DBNull.Value ? "" : Convert.ToDateTime(userJobItemsDataRow["RequestDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                            userJobItemsDataRow["Priority"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["Priority"]),
                                            userJobItemsDataRow["RequestDetail"].ToString(),
                                            userJobItemsDataRow["ImageName"].ToString(),
                                            userJobItemsDataRow["RequestedBy"].ToString(),
                                            userJobItemsDataRow["ContactPhone"].ToString(),
                                            userJobItemsDataRow["ContactEmailId"].ToString(),
                                            userJobItemsDataRow["Resolution"].ToString(),
                                            userJobItemsDataRow["Comments"].ToString(),
                                            userJobItemsDataRow["RepairCost"] == DBNull.Value ? -1 : Convert.ToDouble(userJobItemsDataRow["RepairCost"]),
                                            userJobItemsDataRow["DocFileName"].ToString(),
                                            userJobItemsDataRow["Attribute1"].ToString(),
                                            (userJobItemsDataRow["IsActive"] == DBNull.Value ? false : (userJobItemsDataRow["IsActive"].ToString() == "Y" ? true : false)),
                                            userJobItemsDataRow["CreatedBy"].ToString(),
                                            userJobItemsDataRow["CreationDate"] == DBNull.Value ? "" : Convert.ToDateTime(userJobItemsDataRow["CreationDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                            userJobItemsDataRow["LastUpdatedBy"].ToString(),
                                            userJobItemsDataRow["LastupdatedDate"] == DBNull.Value ? "" : Convert.ToDateTime(userJobItemsDataRow["LastupdatedDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                            userJobItemsDataRow["Guid"].ToString(),
                                            userJobItemsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["site_id"]),
                                            userJobItemsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(userJobItemsDataRow["SynchStatus"]),
                                            userJobItemsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["MasterEntityId"]),
                                            userJobItemsDataRow["BookingId"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["BookingId"]),
                                            userJobItemsDataRow["BookingCheckListId"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["BookingCheckListId"]),
                                            userJobItemsDataRow["MaintschAssetTaskId"] == DBNull.Value ? -1 : Convert.ToInt32(userJobItemsDataRow["MaintschAssetTaskId"]),
                                            userJobItemsDataRow["MaintJobNumber"] == DBNull.Value ? null : Convert.ToString(userJobItemsDataRow["MaintJobNumber"])
                                            );
            log.LogMethodExit(userJobItemsDataObject);
            return userJobItemsDataObject;
        }
        /// <summary>
        /// Gets the user job item data of passed user job item Id
        /// </summary>
        /// <param name="userJobItemId">integer type parameter</param>
        /// <returns>Returns UserJobItemsDTO</returns>
        public UserJobItemsDTO GetUserJobItemsDTO(int userJobItemId)
        {
            log.LogMethodEntry(userJobItemId);
            string selectUserJobItemsQuery = selectQry + "  where job.MaintChklstdetId = @maintChklstdetId";
            SqlParameter[] selectUserJobItemsParameters = new SqlParameter[1];
            selectUserJobItemsParameters[0] = new SqlParameter("@maintChklstdetId", userJobItemId);
            DataTable userJObItems = dataAccessHandler.executeSelectQuery(selectUserJobItemsQuery, selectUserJobItemsParameters, sqlTransaction);
            UserJobItemsDTO userJobItemsDataObject = null;
            if (userJObItems.Rows.Count > 0)
            {
                DataRow maintenanceJobRow = userJObItems.Rows[0];
                userJobItemsDataObject = GetUserJobItemsDTO(maintenanceJobRow);
            }
            log.LogMethodExit(userJobItemsDataObject);
            return userJobItemsDataObject;
        }
        /// <summary>
        /// Gets the UserJobItemsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of UserJobItemsDTO matching the search criteria</returns>
        public List<UserJobItemsDTO> GetUserJobItemsDTOList(List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> searchParameters, int userId)
        {
            log.LogMethodEntry(searchParameters, userId);
            int count = 0;
            string selectUserJobItemsQuery = selectQry + @", (SELECT user_id,loginid,role_id 
                                                              FROM users WHERE (USER_ID=@UserId
                                                                               OR managerId = @UserId ) AND active_flag = 'Y') Users
                                                                      WHERE ((Users.user_id = job.AssignedUserId and users.user_id = @UserId)
                                                                             OR (job.CreatedBy=Users.loginid and users.user_id = @UserId)
                                                                             or role_id=(select role_id from user_roles 
                                                                                     where role='System Administrator' and (site_id= @SiteId OR @SiteId =-1)))";
            SqlParameter[] userJobItemsParameter = new SqlParameter[2];
            userJobItemsParameter[0] = new SqlParameter("@UserId", userId);
            userJobItemsParameter[1] = new SqlParameter("@SiteId", -1);
            if (searchParameters != null)
            {
                string joiner = " ";//Modification on 18-Jul-2016 for adding masterEntityId
                StringBuilder query = new StringBuilder();
                count = 1;
                foreach (KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string> searchParameter in searchParameters)
                {

                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.ASSET_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.USER_JOB_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_SCHEDULE_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TYPE_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.ASSIGNED_TO
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.TASK_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_TYPE
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.STATUS
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.PRIORITY
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_NUMBER
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_FROM_DATE
                               || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_FROM_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">= CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.LAST_UPDATED_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "> CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_TO_DATE
                                || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TO_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<= CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.PAST_DUE_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "< GetDate()");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID)
                        {
                            userJobItemsParameter[1] = new SqlParameter("@SiteId", searchParameter.Value);
                            //query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + (searchParameter.Value == "1" ? "'Y'" : "'N'"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                query.Append(" ORDER BY AssetName");
                if (searchParameters.Count > 0)
                    selectUserJobItemsQuery = selectUserJobItemsQuery + query;
            }

            DataTable userJobItemsData = dataAccessHandler.executeSelectQuery(selectUserJobItemsQuery, userJobItemsParameter, sqlTransaction);
            List<UserJobItemsDTO> userJobItemsDTOList = null;
            if (userJobItemsData.Rows.Count > 0)
            {
                userJobItemsDTOList = new List<UserJobItemsDTO>();
                foreach (DataRow maintenanceJobDataRow in userJobItemsData.Rows)
                {
                    UserJobItemsDTO userJobItemsDataObject = GetUserJobItemsDTO(maintenanceJobDataRow);
                    userJobItemsDTOList.Add(userJobItemsDataObject);
                };
            }
            log.LogMethodExit(userJobItemsDTOList);
            return userJobItemsDTOList;
        }

        /// <summary>
        /// Gets the UserJobItemsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of UserJobItemsDTO matching the search criteria</returns>
        public List<UserJobItemsDTO> GetAllUserJobItemsWithHQPublishedJobList(List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectMaintenanceJobQuery = @"SELECT job.MaintChklstdetId,job.MaintScheduleId,job.MaintTaskId,job.MaintJobType,job.
                                                    MaintJobName,job.ChklstScheduleTime,job.AssignedTo, (usr.MasterEntityId) as AssignedUserId,
                                                    job.DepartmentId,job.Status,job.ChecklistCloseDate,job.TaskName,
                                                    job.ValidateTag,job.CardNumber,job.CardId,job.TaskCardNumber,job.RemarksMandatory,
                                                    job.AssetId,job.AssetName,job.AssetType,job.AssetGroupName,job.ChklistValue,
                                                    job.ChklistRemarks,job.SourceSystemId,job.DurationToComplete,job.RequestType,job.RequestDate,
                                                    job.Priority,job.RequestDetail,job.ImageName,job.RequestedBy,job.ContactPhone,job.ContactEmailId,
                                                    job.Resolution,job.Comments,job.RepairCost,job.DocFileName,job.Attribute1,job.IsActive,
                                                    job.CreatedBy,job.CreationDate,job.LastUpdatedBy,job.LastupdatedDate,job.Guid,job.site_id,
                                                    job.SynchStatus,job.MasterEntityId, job.MaintschAssetTaskId, mst.BookingId, mst.BookingCheckListId
                                                    FROM Maint_ChecklistDetails job 
                                                        join users usr on AssignedUserId=user_id where usr.masterEntityId is not null 
                                                        left outer join Maint_SchAssetTasks mst on mst.MaintschAssetTaskId = job.MaintschAssetTaskId ";

            if (searchParameters != null)
            {
                string joiner = " ";//Modification on 18-Jul-2016 for adding masterEntityId
                StringBuilder query = new StringBuilder();//" Where "
                //count = 1;
                foreach (KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " and " : " and ";
                        if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.ASSET_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.USER_JOB_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_SCHEDULE_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TYPE_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.ASSIGNED_TO
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.TASK_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_TYPE
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.STATUS
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.PRIORITY)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_FROM_DATE
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_FROM_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">= CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.LAST_UPDATED_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "> CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_TO_DATE
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TO_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<= CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.PAST_DUE_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "< GetDate()");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + (searchParameter.Value == "1" ? "'Y'" : "'N'"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }


                        count++;
                    }
                    else
                    {
                        log.Debug("throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                query.Append(" ORDER BY job.AssetName");
                if (searchParameters.Count > 0)
                    selectMaintenanceJobQuery = selectMaintenanceJobQuery + query;
            }

            DataTable maintenanceJobData = dataAccessHandler.executeSelectQuery(selectMaintenanceJobQuery, null, sqlTransaction);
            List<UserJobItemsDTO> userJobItemsDTOList = null;
            if (maintenanceJobData.Rows.Count > 0)
            {
                userJobItemsDTOList = new List<UserJobItemsDTO>();
                foreach (DataRow maintenanceJobDataRow in maintenanceJobData.Rows)
                {
                    UserJobItemsDTO maintenanceJobDataObject = GetUserJobItemsDTO(maintenanceJobDataRow);
                    userJobItemsDTOList.Add(maintenanceJobDataObject);
                }
            }
            log.LogMethodExit(userJobItemsDTOList);
            return userJobItemsDTOList;
        }


        /// <summary>
        /// Gets the UserJobItemsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of UserJobItemsDTO matching the search criteria</returns>
        public List<UserJobItemsDTO> GetAllUserJobItemsDTOList(List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectMaintenanceJobQuery = selectQry;

            if (searchParameters != null)
            {
                string joiner = " ";//Modification on 18-Jul-2016 for adding masterEntityId
                StringBuilder query = new StringBuilder(" Where ");
                //count = 1;
                foreach (KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.ASSET_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.USER_JOB_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_SCHEDULE_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TYPE_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.ASSIGNED_TO
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.TASK_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_TYPE
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.STATUS
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.PRIORITY)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_FROM_DATE
                                 || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_FROM_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">= CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.LAST_UPDATED_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "> CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_TO_DATE
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TO_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<= CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.PAST_DUE_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "< GetDate()");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID
                            || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + (searchParameter.Value == "1" ? "'Y'" : "'N'"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }


                        count++;
                    }
                    else
                    {
                        log.Debug("throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                query.Append(" ORDER BY AssetName");
                if (searchParameters.Count > 0)
                    selectMaintenanceJobQuery = selectMaintenanceJobQuery + query;
            }

            DataTable maintenanceJobData = dataAccessHandler.executeSelectQuery(selectMaintenanceJobQuery, null, sqlTransaction);
            List<UserJobItemsDTO> userJobItemsDTOList = null;
            if (maintenanceJobData.Rows.Count > 0)
            {
                userJobItemsDTOList = new List<UserJobItemsDTO>();
                foreach (DataRow maintenanceJobDataRow in maintenanceJobData.Rows)
                {
                    UserJobItemsDTO maintenanceJobDataObject = GetUserJobItemsDTO(maintenanceJobDataRow);
                    userJobItemsDTOList.Add(maintenanceJobDataObject);
                }
            }
            log.LogMethodExit(userJobItemsDTOList);
            return userJobItemsDTOList;

        }

        /// <summary>
        /// Gets the UserJobItemsDTO list in a batch matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="userId">Logged in User Id</param>
        /// <param name="maxRows">Maximum rows to be fetched in one call</param>
        /// <returns>Returns the list of UserJobItemsDTO in batch matching the search criteria</returns>
        public List<UserJobItemsDTO> GetUserJobItemsListBatch(List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> searchParameters, int maxRows)
        {
            log.LogMethodEntry(searchParameters, maxRows);
            int count = 0;
            string selectMaintenanceJobQuery = @"SELECT TOP (@maxRows)
                                                        Job.* , mst.BookingId, mst.BookingCheckListId
                                                   FROM Maint_ChecklistDetails Job
                                                        left outer join Maint_SchAssetTasks mst on mst.MaintschAssetTaskId = Job.MaintschAssetTaskId ";
            SqlParameter[] maintenanceJobParameter = new SqlParameter[1];
            maintenanceJobParameter[0] = new SqlParameter("@maxRows", maxRows);

            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" Where ");
                foreach (KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_FROM_DATE)
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + ">= CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                            }
                            else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.LAST_UPDATED_DATE)
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + "> CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                            }
                            else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.USER_JOB_ID)
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + "> CAST('" + searchParameter.Value + "'AS NUMERIC) ");
                            }
                            else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_TO_DATE)
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + "<= CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                            }
                            else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.PAST_DUE_DATE)
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + "< GetDate()");
                            }
                            else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.IS_ACTIVE)
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + (searchParameter.Value == "1" ? "'Y'" : "'N'"));
                            }
                            else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID
                                || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.MASTER_ENTITY_ID)
                            {
                                query.Append("(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                            }

                        }
                        else
                        {
                            if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_FROM_DATE
                                || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_FROM_DATE)
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + ">= CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                            }
                            else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.LAST_UPDATED_DATE)
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + "> CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                            }
                            else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.USER_JOB_ID)
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + "> CAST('" + searchParameter.Value + "'AS NUMERIC) ");
                            }
                            else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_TO_DATE
                                || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TO_DATE)
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + "<= CAST('" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'AS DATETIME) ");
                            }
                            else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.PAST_DUE_DATE)
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + "< GetDate()");
                            }
                            else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.IS_ACTIVE)
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + (searchParameter.Value == "1" ? "'Y'" : "'N'"));
                            }
                            else if (searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID
                                || searchParameter.Key == UserJobItemsDTO.SearchByUserJobItemsParameters.MASTER_ENTITY_ID)
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                query.Append(" ORDER BY " + DBSearchParameters[UserJobItemsDTO.SearchByUserJobItemsParameters.USER_JOB_ID]);
                if (searchParameters.Count > 0)
                    selectMaintenanceJobQuery = selectMaintenanceJobQuery + query;
            }

            DataTable maintenanceJobData = dataAccessHandler.executeSelectQuery(selectMaintenanceJobQuery, maintenanceJobParameter, sqlTransaction);
            List<UserJobItemsDTO> userJobItemsDTOList = null;
            if (maintenanceJobData.Rows.Count > 0)
            {
                userJobItemsDTOList = new List<UserJobItemsDTO>();
                foreach (DataRow maintenanceJobDataRow in maintenanceJobData.Rows)
                {
                    UserJobItemsDTO maintenanceJobDataObject = GetUserJobItemsDTO(maintenanceJobDataRow);
                    userJobItemsDTOList.Add(maintenanceJobDataObject);
                }
            }
            log.LogMethodExit(userJobItemsDTOList);
            return userJobItemsDTOList;
        }


        /// <summary>
        /// Gets number for Servcie Request
        /// </summary>
        /// <param name="sequenceName">string</param>
        /// <returns>Returns string</returns>
        public string GetNextSeqNo(string sequenceName)
        {
            log.LogMethodEntry(sequenceName);
            DataTable dTable = dataAccessHandler.executeSelectQuery(@"declare @value varchar(20)
                                exec GetNextSeqValue N'" + sequenceName + "', @value out, -1 "
                                   + " select @value", null, sqlTransaction);
            try
            {
                if (dTable != null && dTable.Rows.Count > 0)
                {
                    object o = dTable.Rows[0][0];
                    if (o != null)
                    {
                        log.LogMethodExit(o);
                        return (o.ToString());
                    }
                    else
                    {
                        log.LogMethodExit(-1);
                        return "-1";
                    }
                }

            }
            catch
            {
                log.LogMethodExit(-1);
                return "-1";
            }
            log.LogMethodExit("-1");
            return "-1";
        }

    }
}

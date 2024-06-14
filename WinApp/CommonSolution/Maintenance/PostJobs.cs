/********************************************************************************************
 * Project Name - Post Job
 * Description  - Bussiness logic for creating jobs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Feb-2016   Raghuveera     Created
 *2.70        12-Mar-2019   Guru S A       Modified for schedule class renaming as par of booking phase2
              31-May-2019   Jagan Mohan    Code merge from Development to WebManagementStudio
 *2.70.2        13-Nov-2019   Guru S A       Waiver phase 2 changes
 *2.80        10-May-2020   Girish Kundar  Modified: REST API Changes merge from WMS  
 ********************************************************************************************/
//using Semnox.Core.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Creates the Jobs
    /// </summary>
    public class PostJobs
    {
        Semnox.Parafait.logger.EventLogDataHandler eventLogHandler=new Semnox.Parafait.logger.EventLogDataHandler();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Updates the status message
        /// </summary>
        public static string messages = "";
        /// <summary>
        /// Creates the job from scheduled jobs
        /// </summary>
        /// <param name="scheduleTime"> Schedule time from configuration</param>
        /// <returns></returns>
        public bool ProcessPostJob(DateTime scheduleTime)
        {
            log.LogMethodEntry(scheduleTime);             
            int status;
            int counter;
            int jobType;
            ScheduledJobList scheduledJobList = new ScheduledJobList();
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            AssetGroupAssetMapperList assetGroupAssetMapperList = new AssetGroupAssetMapperList(machineUserContext);
            List<AssetGroupAssetDTO> assetGroupAssetDTOList;
            AssetList assetList = new AssetList(machineUserContext);
            List<GenericAssetDTO> assetDTOList;
            GenericAssetDTO genericAssetDTO; 
            JobTaskList jobTaskList = new JobTaskList(machineUserContext);
            JobTaskDTO jobTaskDTO;
            List<JobTaskDTO> jobTaskDTOList;
            UserJobItemsDTO userJobItemsDTO;
            UserJobItemsBL userJobItemsBL;
            JobScheduleTasksListBL jobScheduleTasksListBL = new JobScheduleTasksListBL(machineUserContext);
            List<JobScheduleTasksDTO> jobScheduleTasksDTOList;
            List<ScheduledJobDTO> scheduledJobDTOList;
            List<LookupValuesDTO> lookupOpenValuesDTOList;
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            Core.GenericUtilities.JobScheduleDTO jobScheduleDTO;  
            
            List<KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>> jobScheduleTaskearchParams;
            List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>> scheduleAssetGroupAssetSearchParams;
            List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> assetSearchParams;
            List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> searchByJobTaskParameters;
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupOpenValuesSearchParams;
            try
            {
                lookupOpenValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Open"));
                lookupOpenValuesDTOList = lookupValuesList.GetAllLookupValues(lookupOpenValuesSearchParams);

                if (lookupOpenValuesDTOList != null)
                {
                    status = lookupOpenValuesDTOList[0].LookupValueId;
                }
                else
                {
                    messages += "MAINT_JOB_STATUS Lookup values are not found. " + Environment.NewLine;
                    eventLogHandler.InsertEventLog("Job Scheduler", "E", "Semnox", Environment.MachineName, "MAINT_JOB_STATUS", "MAINT_JOB_STATUS Lookup values are not found", "Job Creation", 0, "Job scheduler", "", false, "Semnox", -1);
                    log.Info("End-ProcessPostJob(scheduleTime) method because MAINT_JOB_STATUS Lookup values are not found");
                    log.LogVariableState("messages", messages);
                    log.LogMethodExit(false);
                    return false;
                }

                lookupOpenValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_TYPE"));
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Job"));
                lookupOpenValuesDTOList = lookupValuesList.GetAllLookupValues(lookupOpenValuesSearchParams);

                if (lookupOpenValuesDTOList != null)
                {
                    jobType = lookupOpenValuesDTOList[0].LookupValueId;
                }
                else
                {
                    messages += "MAINT_JOB_TYPE Lookup values are not found. " + Environment.NewLine;
                    eventLogHandler.InsertEventLog("Job Scheduler", "E", "Semnox", Environment.MachineName, "MAINT_JOB_TYPE", "MAINT_JOB_TYPE Lookup values are not found", "Job Creation", 0, "Job scheduler", "", false, "Semnox", -1);
                    log.Info("End-ProcessPostJob(scheduleTime) method because MAINT_JOB_TYPE Lookup values are not found");
                    log.LogVariableState("messages", messages);
                    log.LogMethodExit(false);
                    return false;
                }                
                scheduledJobDTOList = scheduledJobList.GetAllScheduledJobs(scheduleTime);
                if (scheduledJobDTOList != null)
                {
                    counter = 0;
                    foreach (ScheduledJobDTO scheduledJobDTO in scheduledJobDTOList)//Loop on scheduledjobView
                    {
                        JobScheduleBL jobScheduleBL = new JobScheduleBL(machineUserContext, scheduledJobDTO.MaintScheduleId);
                        jobScheduleDTO = jobScheduleBL.JobScheduleDTO;
                        if (jobScheduleDTO != null)
                        {
                            jobScheduleTasksDTOList = new List<JobScheduleTasksDTO>();
                            jobScheduleTaskearchParams = new List<KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>>();
                            jobScheduleTaskearchParams.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.IS_ACTIVE, "1"));
                            jobScheduleTaskearchParams.Add(new KeyValuePair<JobScheduleTasksDTO.SearchByJobScheduleTaskParameters, string>(JobScheduleTasksDTO.SearchByJobScheduleTaskParameters.JOB_SCHEDULE_ID, scheduledJobDTO.MaintScheduleId.ToString()));
                            jobScheduleTasksDTOList = jobScheduleTasksListBL.GetAllJobScheduleTaskDTOList(jobScheduleTaskearchParams);
                            if (jobScheduleTasksDTOList != null)
                            {
                                assetDTOList = new List<GenericAssetDTO>();
                                jobTaskDTOList = new List<JobTaskDTO>();
                                foreach (JobScheduleTasksDTO jobScheduleTasksDTOItem in jobScheduleTasksDTOList)//Loop on schedule asset task to fetch asset
                                {
                                    if (jobScheduleTasksDTOItem.AssetGroupId != -1)//selecting all the assets belong to the AssetGroupId
                                    {
                                        assetGroupAssetDTOList = new List<AssetGroupAssetDTO>();
                                        scheduleAssetGroupAssetSearchParams = new List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>>();
                                        scheduleAssetGroupAssetSearchParams.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ACTIVE_FLAG, "Y"));
                                        scheduleAssetGroupAssetSearchParams.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_GROUP_ID, jobScheduleTasksDTOItem.AssetGroupId.ToString()));
                                        assetGroupAssetDTOList = assetGroupAssetMapperList.GetAllAssetGroupAsset(scheduleAssetGroupAssetSearchParams);
                                        if (assetGroupAssetDTOList != null)
                                        {
                                            foreach (AssetGroupAssetDTO assetGroupAssetDTO in assetGroupAssetDTOList)
                                            {
                                                genericAssetDTO = assetList.GetAsset( assetGroupAssetDTO.AssetId);
                                                if (genericAssetDTO != null)
                                                {
                                                    assetDTOList.Add(genericAssetDTO);
                                                }
                                                else
                                                {
                                                    eventLogHandler.InsertEventLog("Job Scheduler", "I", "Semnox", Environment.MachineName, jobScheduleTasksDTOItem.AssetGroupId.ToString(), "Asset is not found for the group id:" + jobScheduleTasksDTOItem.AssetGroupId, "Job Creation", 0, "Job scheduler", "", false, "Semnox", -1);
                                                }
                                            }
                                        }
                                    }
                                    else if (jobScheduleTasksDTOItem.AssetTypeId != -1)//selecting all the assets belong to the AssetTypeId
                                    {
                                        assetSearchParams = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
                                        assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ACTIVE_FLAG, "Y"));
                                        assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ASSET_TYPE_ID, jobScheduleTasksDTOItem.AssetTypeId.ToString()));
                                        var assetsDTOList = assetList.GetAllAssets(assetSearchParams);
                                        if (assetsDTOList != null)
                                        {
                                            foreach (GenericAssetDTO assetDTO in (List<GenericAssetDTO>)assetsDTOList)
                                            {
                                                assetDTOList.Add(assetDTO);
                                            }
                                        }
                                        else
                                        {
                                            eventLogHandler.InsertEventLog("Job Scheduler", "I", "Semnox", Environment.MachineName, jobScheduleTasksDTOItem.AssetTypeId.ToString(), "Asset is not found for the type id:" + jobScheduleTasksDTOItem.AssetTypeId, "Job Creation", 0, "Job scheduler", "", false, "Semnox", -1);
                                        }
                                    }
                                    else if (jobScheduleTasksDTOItem.AssetID != -1)//selecting the asset using asset id
                                    {
                                        genericAssetDTO = assetList.GetAsset(jobScheduleTasksDTOItem.AssetID);
                                        if (genericAssetDTO != null)
                                        {
                                            assetDTOList.Add(genericAssetDTO);
                                        }
                                        else
                                        {
                                            eventLogHandler.InsertEventLog("Job Scheduler", "I", "Semnox", Environment.MachineName, jobScheduleTasksDTOItem.AssetID.ToString(), "Asset is not found for the assset id:" + jobScheduleTasksDTOItem.AssetID, "Job Creation", 0, "Job scheduler", "", false, "Semnox", -1);
                                        }
                                    }
                                    if (jobScheduleTasksDTOItem.JObTaskGroupId != -1)//selecting the task belongs to the particular group id 
                                    {
                                        searchByJobTaskParameters = new List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>>();
                                        searchByJobTaskParameters.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.IS_ACTIVE, "1"));
                                        searchByJobTaskParameters.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.JOB_TASK_GROUP_ID, jobScheduleTasksDTOItem.JObTaskGroupId.ToString()));
                                        if (jobScheduleTasksDTOItem.JobTaskId != -1)
                                        {
                                            searchByJobTaskParameters.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.JOB_TASK_ID, jobScheduleTasksDTOItem.JobTaskId.ToString()));
                                        }
                                        var jobTasksDTOList = jobTaskList.GetAllJobTasks(searchByJobTaskParameters);
                                        if (jobTasksDTOList != null)
                                        {
                                            foreach (JobTaskDTO jobTaskDTOItem in (List<JobTaskDTO>)jobTasksDTOList)
                                            {
                                                jobTaskDTOList.Add(jobTaskDTOItem);
                                            }
                                        }
                                        else
                                        {
                                            eventLogHandler.InsertEventLog("Job Scheduler", "I", "Semnox", Environment.MachineName, jobScheduleTasksDTOItem.JObTaskGroupId.ToString(), "Job task is not found for the Job group id:" + jobScheduleTasksDTOItem.JObTaskGroupId, "Job Creation", 0, "Job scheduler", "", false, "Semnox", -1);
                                        }
                                    }
                                    else if (jobScheduleTasksDTOItem.JobTaskId != -1)//selecting the task using task id
                                    {
                                        JobTaskBL jobTaskBL = new JobTaskBL(machineUserContext, jobScheduleTasksDTOItem.JobTaskId);
                                        jobTaskDTO = jobTaskBL.JobTaskDTO;
                                        if (jobTaskDTO != null)
                                        {
                                            jobTaskDTOList.Add(jobTaskDTO);
                                        }
                                        else
                                        {
                                            eventLogHandler.InsertEventLog("Job Scheduler", "I", "Semnox", Environment.MachineName, jobScheduleTasksDTOItem.JobTaskId.ToString(), "JOb task is not found for the JOb task id:" + jobScheduleTasksDTOItem.JobTaskId, "Job Creation", 0, "Job scheduler", "", false, "Semnox", -1);
                                        }
                                    }
                                    if (assetDTOList != null && assetDTOList.Count > 0) //for jobs of asset maintenance
                                    {
                                        foreach (GenericAssetDTO assetDTO in assetDTOList)
                                        {
                                            if (jobTaskDTOList != null)
                                            {
                                                foreach (JobTaskDTO TaskDTO in jobTaskDTOList)
                                                {
                                                    //From view
                                                    userJobItemsDTO = new UserJobItemsDTO();
                                                    userJobItemsDTO.MaintJobName = scheduledJobDTO.MaintJobName;
                                                    userJobItemsDTO.AssignedTo = scheduledJobDTO.AssignedTo;
                                                    userJobItemsDTO.AssignedUserId = scheduledJobDTO.AssignedUserId;
                                                    userJobItemsDTO.ChklstScheduleTime = scheduledJobDTO.ChklstScheduleTime.ToString("yyyy-MM-dd HH:mm:ss");
                                                    userJobItemsDTO.DepartmentId = scheduledJobDTO.DepartmentId;
                                                    userJobItemsDTO.DurationToComplete = scheduledJobDTO.DurationToComplete;
                                                    userJobItemsDTO.JobScheduleId = scheduledJobDTO.MaintScheduleId;
                                                    machineUserContext.SetUserId(scheduledJobDTO.LastUpdatedBy);
                                                    //From assets
                                                    userJobItemsDTO.AssetId = assetDTO.AssetId;
                                                    userJobItemsDTO.AssetName = assetDTO.Name;
                                                    userJobItemsDTO.Siteid = assetDTO.Siteid;
                                                    //From JobTask
                                                    userJobItemsDTO.JobTaskId = TaskDTO.JobTaskId;
                                                    userJobItemsDTO.TaskName = TaskDTO.TaskName;
                                                    userJobItemsDTO.ValidateTag = TaskDTO.ValidateTag;
                                                    userJobItemsDTO.TaskCardNumber = TaskDTO.CardNumber;
                                                    userJobItemsDTO.RemarksMandatory = TaskDTO.RemarksMandatory;
                                                    //From Lookups
                                                    userJobItemsDTO.Status = status;
                                                    userJobItemsDTO.MaintJobType = jobType;
                                                    userJobItemsDTO.JobScheduleTaskId = jobScheduleTasksDTOItem.JobScheduleTaskId;
                                                    //Saving
                                                    userJobItemsBL = new UserJobItemsBL(machineUserContext, userJobItemsDTO);
                                                    userJobItemsBL.Save();
                                                    counter++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (jobTaskDTOList != null)
                                        {
                                            foreach (JobTaskDTO TaskDTO in jobTaskDTOList)
                                            {
                                                //From view
                                                userJobItemsDTO = new UserJobItemsDTO();
                                                userJobItemsDTO.MaintJobName = scheduledJobDTO.MaintJobName;
                                                userJobItemsDTO.AssignedTo = scheduledJobDTO.AssignedTo;
                                                userJobItemsDTO.AssignedUserId = scheduledJobDTO.AssignedUserId;
                                                userJobItemsDTO.ChklstScheduleTime = scheduledJobDTO.ChklstScheduleTime.ToString("yyyy-MM-dd HH:mm:ss");
                                                userJobItemsDTO.DepartmentId = scheduledJobDTO.DepartmentId;
                                                userJobItemsDTO.DurationToComplete = scheduledJobDTO.DurationToComplete;
                                                userJobItemsDTO.JobScheduleId = scheduledJobDTO.MaintScheduleId;
                                                machineUserContext.SetUserId(scheduledJobDTO.LastUpdatedBy);
                                                //for asset
                                                userJobItemsDTO.AssetName = "Booking";
                                                //From JobTask
                                                userJobItemsDTO.JobTaskId = TaskDTO.JobTaskId;
                                                userJobItemsDTO.TaskName = TaskDTO.TaskName;
                                                userJobItemsDTO.ValidateTag = TaskDTO.ValidateTag;
                                                userJobItemsDTO.TaskCardNumber = TaskDTO.CardNumber;
                                                userJobItemsDTO.RemarksMandatory = TaskDTO.RemarksMandatory;
                                                //From Lookups
                                                userJobItemsDTO.Status = status;
                                                userJobItemsDTO.MaintJobType = jobType;
                                                userJobItemsDTO.JobScheduleTaskId = jobScheduleTasksDTOItem.JobScheduleTaskId;
                                                //Saving
                                                userJobItemsBL = new UserJobItemsBL(machineUserContext, userJobItemsDTO);
                                                userJobItemsBL.Save();
                                                counter++;
                                            }
                                        }
                                    }
                                    assetDTOList = new List<GenericAssetDTO>();
                                    jobTaskDTOList = new List<JobTaskDTO>();
                                }
                            }
                            else
                            {
                                eventLogHandler.InsertEventLog("Job Scheduler", "I", "Semnox", Environment.MachineName, scheduledJobDTO.MaintJobName, "Schedule-Asset/booking-Task maping is not found for job:" + scheduledJobDTO.MaintJobName, "Job Creation", 0, "Job scheduler", "", false, "Semnox", -1);
                                messages += "Schedule-Asset/Booking-Task maping is not found for job : " + scheduledJobDTO.MaintJobName + Environment.NewLine;
                                continue;
                            }                            
                            jobScheduleDTO.MaxValueJobCreated = scheduledJobDTO.ChklstScheduleTime;
                            jobScheduleBL = new JobScheduleBL(machineUserContext, jobScheduleDTO);
                            jobScheduleBL.Save();

                        }
                        else
                        {
                            eventLogHandler.InsertEventLog("Job Scheduler", "I", "Semnox", Environment.MachineName, scheduledJobDTO.MaintScheduleId.ToString(), "Job schedule not found", "Job Creation", 0, "Job scheduler", "", false, "Semnox", -1);

                        }
                    }
                    if (counter > 0)
                    {                        
                        messages += counter + " jobs Created." + Environment.NewLine;
                        eventLogHandler.InsertEventLog("Job Scheduler", "I", "Semnox", Environment.MachineName, "", counter + " jobs Created.", "Job Creation", 0, "Job scheduler", "", false, "Semnox", -1);                        
                    }
                }
                else
                {
                    eventLogHandler.InsertEventLog("Job Scheduler", "I", "Semnox", Environment.MachineName, "", "No new jobs to schedule.", "Job Creation", 0, "Job scheduler", "", false, "Semnox", -1);
                    messages += "No new jobs to schedule." + Environment.NewLine;
                    log.LogVariableState("messages", messages);
                    log.LogMethodExit(true);
                    return true;
                }
                log.LogVariableState("messages", messages);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception e)
            {
                eventLogHandler.InsertEventLog("Job Scheduler", "E", "Semnox", Environment.MachineName, "", "Exception occured:" + e.Message, "Job Creation", 0, "Job scheduler", "", false, "Semnox", -1);
                messages += "Exception occured : " + e.Message + Environment.NewLine;
                log.Error(e);
                log.LogMethodExit(false);
                return false;
            }
        }
        
    }
}

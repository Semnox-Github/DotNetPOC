/********************************************************************************************
 * Project Name - Task Types
 * Description  - Bussiness logic of Task Types
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.70        12-Mar-2016   Jagan Mohana       Created 
              08-Apr-2019   Mushahid Faizan    Modified- SaveUpdateTaskTypesList(),Added LogMethodEntry & LogMethodExit,Removed unused namespaces.
 *2.120.0     25-Mar-2021   Fiona              Container changes 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Core.Utilities
{
    public class TaskTypes
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TaskTypesDTO taskTypesDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public TaskTypes(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.taskTypesDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="taskTypesDTO"></param>
        public TaskTypes(ExecutionContext executionContext, TaskTypesDTO taskTypesDTO)
        {
            log.LogMethodEntry(executionContext, taskTypesDTO);
            this.executionContext = executionContext;
            this.taskTypesDTO = taskTypesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Task Types
        /// Checks if the task type id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            TaskTypesDataHandler taskTypesDataHandler = new TaskTypesDataHandler();
            if (taskTypesDTO.TaskTypeId < 0)
            {
                int taskTypeId = taskTypesDataHandler.InsertTaskTypes(taskTypesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                taskTypesDTO.TaskTypeId = taskTypeId;
            }
            else
            {
                if (taskTypesDTO.IsChanged)
                {
                    taskTypesDataHandler.UpdateTaskTypes(taskTypesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    taskTypesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        public TaskTypesDTO GetTaskTypesDTO
        {
            get { return taskTypesDTO; }
        }
    }
    /// <summary>
    /// Manages the list of TaskTypes
    /// </summary>
    public class TaskTypesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<TaskTypesDTO> taskTypesDTOList;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public TaskTypesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.taskTypesDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="taskTypesDTOList"></param>
        public TaskTypesList(ExecutionContext executionContext, List<TaskTypesDTO> taskTypesDTOList)
        {
            log.LogMethodEntry(executionContext, taskTypesDTOList);
            this.taskTypesDTOList = taskTypesDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the task types list
        /// </summary>
        public List<TaskTypesDTO> GetAllTaskTypes(List<KeyValuePair<TaskTypesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            TaskTypesDataHandler taskTypesDataHandler = new TaskTypesDataHandler();
            log.LogMethodExit();
            return taskTypesDataHandler.GetTaskTypes(searchParameters);
        }

        /// <summary>
        /// This method should be used to Save and Update the Task Types details for Web Management Studio.
        /// </summary>
        public void SaveUpdateTaskTypesList()
        {
            log.LogMethodEntry();
            try
            {
                if (taskTypesDTOList != null)
                {
                    foreach (TaskTypesDTO taskTypesDto in taskTypesDTOList)
                    {
                        TaskTypes taskTypes = new TaskTypes(executionContext, taskTypesDto);
                        taskTypes.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        public DateTime? GetTaskTypesLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            TaskTypesDataHandler taskTypesDataHandler = new TaskTypesDataHandler(sqlTransaction);
            DateTime? result = taskTypesDataHandler.GetTaskTypesLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}

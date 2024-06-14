/********************************************************************************************
 * Project Name - Utilities
 * Description  - TaskTypesContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
2.120.00    02-Mar-2021       Roshan Devadiga         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace Semnox.Core.Utilities
{
    public class TaskTypesContainer : BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<TaskTypesDTO> taskTypesDTOList;
        private readonly DateTime? taskTypesLastUpdateTime;
        private readonly int siteId;
        private readonly ConcurrentDictionary<int, TaskTypesDTO> taskTypesDTODictionary;
        private readonly ConcurrentDictionary<string, TaskTypesDTO> taskTypeNameDTODictionary;

        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        internal TaskTypesContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            taskTypesDTODictionary = new ConcurrentDictionary<int, TaskTypesDTO>();
            taskTypesDTOList = new List<TaskTypesDTO>();
            TaskTypesList taskTypesListBL = new TaskTypesList(executionContext);
            taskTypesLastUpdateTime = taskTypesListBL.GetTaskTypesLastUpdateTime(siteId);

            List<KeyValuePair<TaskTypesDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<TaskTypesDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<TaskTypesDTO.SearchByParameters, string>(TaskTypesDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            taskTypesDTOList = taskTypesListBL.GetAllTaskTypes(SearchParameters);
            if (taskTypesDTOList != null && taskTypesDTOList.Any())
            {
                foreach (TaskTypesDTO taskTypesDTO in taskTypesDTOList)
                {
                    taskTypesDTODictionary[taskTypesDTO.TaskTypeId] = taskTypesDTO;
                }
            }
            else
            {
                taskTypesDTOList = new List<TaskTypesDTO>();
                taskTypesDTODictionary = new ConcurrentDictionary<int, TaskTypesDTO>();
            }
            log.LogMethodExit();
        }
        public List<TaskTypesContainerDTO> GetTaskTypesContainerDTOList()
        {
            log.LogMethodEntry();
            List<TaskTypesContainerDTO> taskTypesViewDTOList = new List<TaskTypesContainerDTO>();
            foreach (TaskTypesDTO taskTypesDTO in taskTypesDTOList)
            {

                TaskTypesContainerDTO taskTypesViewDTO = new TaskTypesContainerDTO(taskTypesDTO.TaskTypeId,
                                                                                taskTypesDTO.TaskType,
                                                                                taskTypesDTO.RequiresManagerApproval,
                                                                                taskTypesDTO.DisplayInPOS,
                                                                                taskTypesDTO.TaskTypeName  
                                                                                );

                taskTypesViewDTOList.Add(taskTypesViewDTO);
            }
            log.LogMethodExit(taskTypesViewDTOList);
            return taskTypesViewDTOList;
        }
        public TaskTypesContainer Refresh()
        {
            log.LogMethodEntry();
            TaskTypesList taskTypesListBL = new TaskTypesList(executionContext);
            DateTime? updateTime = taskTypesListBL.GetTaskTypesLastUpdateTime(siteId);
            if (taskTypesLastUpdateTime.HasValue
                && taskTypesLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in TaskTypes since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            TaskTypesContainer result = new TaskTypesContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}

/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - TaskTypesViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    02-Mar-2021      Roshan Devadiga          Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Semnox.Parafait.ViewContainer
{
    class TaskTypesViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly TaskTypesContainerDTOCollection taskTypesContainerDTOCollection;
        private readonly ConcurrentDictionary<int, TaskTypesContainerDTO> taskTypesContainerDTODictionary = new ConcurrentDictionary<int, TaskTypesContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="taskTypesContainerDTOCollection">taskTypesContainerDTOCollection</param>
        internal TaskTypesViewContainer(int siteId, TaskTypesContainerDTOCollection taskTypesContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, taskTypesContainerDTOCollection);
            this.siteId = siteId;
            this.taskTypesContainerDTOCollection = taskTypesContainerDTOCollection;
            if (taskTypesContainerDTOCollection != null &&
                taskTypesContainerDTOCollection.TaskTypesContainerDTOList != null &&
               taskTypesContainerDTOCollection.TaskTypesContainerDTOList.Any())
            {
                foreach (var taskTypesContainerDTO in taskTypesContainerDTOCollection.TaskTypesContainerDTOList)
                {
                    taskTypesContainerDTODictionary[taskTypesContainerDTO.TaskTypeId] = taskTypesContainerDTO;
                }
            }
            log.LogMethodExit();
        }
        internal TaskTypesViewContainer(int siteId)
              : this(siteId, GetTaskTypesContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        private static TaskTypesContainerDTOCollection GetTaskTypesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            TaskTypesContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ITaskTypesUseCases taskTypesUseCases = TaskTypesUseCaseFactory.GetTaskTypesUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<TaskTypesContainerDTOCollection> task = taskTypesUseCases.GetTaskTypesContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving TaskTypesContainerDTOCollection.", ex);
                result = new TaskTypesContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns the latest in TaskTypesContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal TaskTypesContainerDTOCollection GetTaskTypesContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (taskTypesContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(taskTypesContainerDTOCollection);
            return taskTypesContainerDTOCollection;
        }

        internal List<TaskTypesContainerDTO> GetTaskTypesContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(taskTypesContainerDTOCollection.TaskTypesContainerDTOList);
            return taskTypesContainerDTOCollection.TaskTypesContainerDTOList;
        }
        internal TaskTypesViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            TaskTypesContainerDTOCollection latestTaskTypesContainerDTOCollection = GetTaskTypesContainerDTOCollection(siteId, taskTypesContainerDTOCollection.Hash, rebuildCache);
            if (latestTaskTypesContainerDTOCollection == null ||
                latestTaskTypesContainerDTOCollection.TaskTypesContainerDTOList == null ||
                latestTaskTypesContainerDTOCollection.TaskTypesContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            TaskTypesViewContainer result = new TaskTypesViewContainer(siteId, latestTaskTypesContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}

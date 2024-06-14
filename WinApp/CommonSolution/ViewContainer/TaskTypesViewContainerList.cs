/********************************************************************************************
 * Project Name - ContainerView
 * Description  - TaskTypesViewContainerList holds multiple  TaskTypesView containers based on siteId, userId and POSMachineId
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    02-Mar-2021      Roshan Devadiga           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Timers;

namespace Semnox.Parafait.ViewContainer
{
    // <summary>
    /// TaskTypesViewContainerList holds multiple  TaskTypesView containers based on siteId, userId and POSMachineId
    /// <summary>
    public class TaskTypesViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, TaskTypesViewContainer> taskTypesViewContainerCache = new Cache<int, TaskTypesViewContainer>();
        private static Timer refreshTimer;

        static TaskTypesViewContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }
        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = taskTypesViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                TaskTypesViewContainer taskTypesViewContainer;
                if (taskTypesViewContainerCache.TryGetValue(uniqueKey, out taskTypesViewContainer))
                {
                    taskTypesViewContainerCache[uniqueKey] = taskTypesViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static TaskTypesViewContainer GetTaskTypesViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = taskTypesViewContainerCache.GetOrAdd(siteId, (k) => new TaskTypesViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the POSMachineContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<TaskTypesContainerDTO> GetTaskTypesContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            TaskTypesViewContainer taskTypesViewContainer = GetTaskTypesViewContainer(executionContext.SiteId);
            List<TaskTypesContainerDTO> result = taskTypesViewContainer.GetTaskTypesContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public static TaskTypesContainerDTOCollection GetTaskTypesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            TaskTypesViewContainer container = GetTaskTypesViewContainer(siteId);
            TaskTypesContainerDTOCollection taskTypesContainerDTOCollection = container.GetTaskTypesContainerDTOCollection(hash);
            return taskTypesContainerDTOCollection;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            TaskTypesViewContainer container = GetTaskTypesViewContainer(siteId);
            taskTypesViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}

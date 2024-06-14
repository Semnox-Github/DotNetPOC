/********************************************************************************************
 * Project Name - Utilities
 * Description  - TaskTypesMasterList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00     02-Mar-2021      Roshan Devadiga         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Timers;


namespace Semnox.Core.Utilities
{
    public  class TaskTypesContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, TaskTypesContainer> taskTypesContainerDictionary = new Cache<int, TaskTypesContainer>();
        private static Timer refreshTimer;

        static TaskTypesContainerList()
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
            var uniqueKeyList = taskTypesContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                TaskTypesContainer taskTypesContainer;
                if (taskTypesContainerDictionary.TryGetValue(uniqueKey, out taskTypesContainer))
                {
                    taskTypesContainerDictionary[uniqueKey] = taskTypesContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static TaskTypesContainer GetTaskTypesContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            TaskTypesContainer result = taskTypesContainerDictionary.GetOrAdd(siteId, (k) => new TaskTypesContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        public static List<TaskTypesContainerDTO> GetTaskTypesContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            TaskTypesContainer container = GetTaskTypesContainer(siteId);
            List<TaskTypesContainerDTO> taskTypesContainerDTOList = container.GetTaskTypesContainerDTOList();
            log.LogMethodExit(taskTypesContainerDTOList);
            return taskTypesContainerDTOList;
        }
        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            TaskTypesContainer redemptionCurrencyContainer = GetTaskTypesContainer(siteId);
            taskTypesContainerDictionary[siteId] = redemptionCurrencyContainer.Refresh();
            log.LogMethodExit();
        }
    }
}

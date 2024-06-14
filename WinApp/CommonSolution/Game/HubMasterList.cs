/********************************************************************************************
 * Project Name - Games
 * Description  - HubMasterList class to get the List of games from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Game
{
    public static class HubMasterList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, HubContainer> hubContainerDictionary = new ConcurrentDictionary<int, HubContainer>();
        private static readonly object locker = new object();

        private static HubContainer GetHubContainer(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            if (hubContainerDictionary.ContainsKey(executionContext.GetSiteId()) == false)
            {
                hubContainerDictionary[executionContext.GetSiteId()] = new HubContainer(executionContext.GetSiteId());
            }
            HubContainer hubContainer = hubContainerDictionary[executionContext.GetSiteId()];
            log.LogMethodExit(hubContainer);
            return hubContainer;
        }
        //public static List<HubDTO> GetHubsList(ExecutionContext executionContext, DateTime? maxLastUpdatedDate, string hash)
        //{
        //    log.LogMethodEntry(executionContext, maxLastUpdatedDate, hash);
        //    List<HubDTO> hubDTOList;
        //    lock (locker)
        //    {
        //        HubContainer hubContainer = GetHubContainer(executionContext);
        //        hubDTOList = hubContainer.GetHubDTOListModifiedAfter(maxLastUpdatedDate, hash);
        //    }
        //    log.LogMethodExit(hubDTOList);
        //    return hubDTOList;
        //}
        //public static HubDTO GetHubDTO(ExecutionContext executionContext, int masterId)
        //{
        //    log.LogMethodEntry(executionContext, masterId);
        //    HubDTO result;
        //    lock (locker)
        //    {
        //        lock (locker)
        //        {
        //            HubContainer hubContainer = GetHubContainer(executionContext);
        //            result = hubContainer.GetHubDTO(masterId);
        //        }
        //    }
        //    log.LogMethodExit(result);
        //    return result;
        //}

        //public static HubDTO GetHubDTO(ExecutionContext executionContext, string masterName)
        //{
        //    log.LogMethodEntry(executionContext, masterName);
        //    HubDTO result;
        //    lock (locker)
        //    {
        //        lock (locker)
        //        {
        //            HubContainer hubContainer = GetHubContainer(executionContext);
        //            result = hubContainer.GetHubDTO(masterName);
        //        }
        //    }
        //    log.LogMethodExit(result);
        //    return result;
        //}
        //public static HubGetBL GetHubBL(ExecutionContext executionContext, int hubId)
        //{
        //    log.LogMethodEntry(executionContext, hubId);
        //    HubGetBL hubGetBL;
        //    lock (locker)
        //    {
        //        HubContainer hubContainer = GetHubContainer(executionContext);
        //        hubGetBL = hubContainer.GetHubBL(hubId);
        //    }
        //    log.LogMethodExit(hubGetBL);
        //    return hubGetBL;
        //}

        /// <summary>
        /// clears the master list
        /// </summary>
        public static void Clear()
        {
            log.LogMethodEntry();
            lock (locker)
            {
                foreach (var hubContainer in hubContainerDictionary.Values)
                {
                    hubContainer.RebuildCache();
                }
            }
            log.LogMethodExit();
        }
    }
}

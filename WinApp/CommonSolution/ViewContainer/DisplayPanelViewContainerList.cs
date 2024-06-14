/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - DisplayPanelViewContainerList holds multiple DisplayPanelView containers based on siteId.
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.150.2    06-Dec-2022      Abhishek           Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.DigitalSignage;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// DisplayPanelViewContainerList holds multiple  DisplayPanelView containers based on siteId.
    /// </summary>
    public class DisplayPanelViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, DisplayPanelViewContainer> displayPanelViewContainerCache = new Cache<int, DisplayPanelViewContainer>();
        private static Timer refreshTimer;

        static DisplayPanelViewContainerList()
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
            var uniqueKeyList = displayPanelViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                DisplayPanelViewContainer displayPanelViewContainer;
                if (displayPanelViewContainerCache.TryGetValue(uniqueKey, out displayPanelViewContainer))
                {
                    displayPanelViewContainerCache[uniqueKey] = displayPanelViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static DisplayPanelViewContainer GetDisplayPanelViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = displayPanelViewContainerCache.GetOrAdd(siteId, (k)=> new DisplayPanelViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the DisplayPanelContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// <returns></returns>
        public static DisplayPanelContainerDTOCollection GetDisplayPanelContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, hash, rebuildCache);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            DisplayPanelViewContainer container = GetDisplayPanelViewContainer(siteId);
            DisplayPanelContainerDTOCollection displayPanelContainerDTOCollection = container.GetDisplayPanelContainerDTOCollection(hash);
            return displayPanelContainerDTOCollection;
        }

        /// <summary>
        /// Returns the DisplayPanelContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<DisplayPanelContainerDTO> GetDisplayPanelContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            DisplayPanelViewContainer displayPanelViewContainer = GetDisplayPanelViewContainer(executionContext.SiteId);
            List<DisplayPanelContainerDTO> result = displayPanelViewContainer.GetDisplayPanelContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            DisplayPanelViewContainer displayPanelViewContainer = GetDisplayPanelViewContainer(siteId);
            displayPanelViewContainerCache[siteId] = displayPanelViewContainer.Refresh(true);
            log.LogMethodExit();
        }
    }
}

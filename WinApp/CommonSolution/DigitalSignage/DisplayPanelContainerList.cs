/********************************************************************************************
* Project Name -DigitalSignage
* Description  - DisplayPanelContainerList class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
*2.150.2    06-Dec-2022      Abhishek           Created - Game Server Cloud Movement.
********************************************************************************************/
using System;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// DisplayPanelContainerList class
    /// </summary>
    public class DisplayPanelContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, DisplayPanelContainer> displayPanelContainerDictionary = new Cache<int, DisplayPanelContainer>();
        private static Timer refreshTimer;

        static DisplayPanelContainerList()
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
            var uniqueKeyList = displayPanelContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                DisplayPanelContainer displayPanelContainer;
                if (displayPanelContainerDictionary.TryGetValue(uniqueKey, out displayPanelContainer))
                {
                    displayPanelContainerDictionary[uniqueKey] = displayPanelContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static DisplayPanelContainer GetDisplayPanelContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            DisplayPanelContainer result = displayPanelContainerDictionary.GetOrAdd(siteId, (k) => new DisplayPanelContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static DisplayPanelContainerDTOCollection GetDisplayPanelContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            DisplayPanelContainer container = GetDisplayPanelContainer(siteId);
            DisplayPanelContainerDTOCollection result = container.GetDisplayPanelContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            DisplayPanelContainer displayPanelContainer = GetDisplayPanelContainer(siteId);
            displayPanelContainerDictionary[siteId] = displayPanelContainer.Refresh();
            log.LogMethodExit();
        }
    }
}

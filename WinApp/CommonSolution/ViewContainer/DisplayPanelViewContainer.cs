/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - DisplayPanelViewContainer holds the Display Panel values for a given siteId.
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
  *2.150.2    06-Dec-2022      Abhishek           Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.DigitalSignage;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// DisplayPanelViewContainer holds the Display Panel values for a given siteId.
    /// </summary>
    public class DisplayPanelViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DisplayPanelContainerDTOCollection displayPanelContainerDTOCollection;
        private readonly ConcurrentDictionary<int, DisplayPanelContainerDTO> displayPanelContainerDTODictionary = new ConcurrentDictionary<int, DisplayPanelContainerDTO>();
        private readonly int siteId;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="displayPanelContainerDTOCollection">displayPanelContainerDTOCollection</param>
        internal DisplayPanelViewContainer(int siteId, DisplayPanelContainerDTOCollection displayPanelContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, displayPanelContainerDTOCollection);
            this.siteId = siteId;
            this.displayPanelContainerDTOCollection = displayPanelContainerDTOCollection;
            if (displayPanelContainerDTOCollection != null &&
                displayPanelContainerDTOCollection.DisplayPanelContainerDTOList != null &&
               displayPanelContainerDTOCollection.DisplayPanelContainerDTOList.Any())
            {
                foreach (var displayPanelContainerDTO in displayPanelContainerDTOCollection.DisplayPanelContainerDTOList)
                {
                    displayPanelContainerDTODictionary[displayPanelContainerDTO.PanelId] = displayPanelContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal DisplayPanelViewContainer(int siteId)
              : this(siteId, GetDisplayPanelContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static DisplayPanelContainerDTOCollection GetDisplayPanelContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            DisplayPanelContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IDisplayPanelUseCases displayPanelUseCases = DigitalSignageUseCaseFactory.GetDisplayPanelUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<DisplayPanelContainerDTOCollection> task = displayPanelUseCases.GetDisplayPanelContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving DisplayPanelContainerDTOCollection.", ex);
                result = new DisplayPanelContainerDTOCollection();
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the latest in DisplayPanelContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal DisplayPanelContainerDTOCollection GetDisplayPanelContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (displayPanelContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(displayPanelContainerDTOCollection);
            return displayPanelContainerDTOCollection;
        }

        internal List<DisplayPanelContainerDTO> GetDisplayPanelContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(displayPanelContainerDTOCollection.DisplayPanelContainerDTOList);
            return displayPanelContainerDTOCollection.DisplayPanelContainerDTOList;
        }

        internal DisplayPanelViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            DisplayPanelContainerDTOCollection latestDisplayPanelContainerDTOCollection = GetDisplayPanelContainerDTOCollection(siteId, displayPanelContainerDTOCollection.Hash, rebuildCache);
            if (latestDisplayPanelContainerDTOCollection == null ||
                latestDisplayPanelContainerDTOCollection.DisplayPanelContainerDTOList == null ||
                latestDisplayPanelContainerDTOCollection.DisplayPanelContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            DisplayPanelViewContainer result = new DisplayPanelViewContainer(siteId, latestDisplayPanelContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}

/********************************************************************************************
* Project Name -DigitalSignage
* Description  - DisplayPanelContainer class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
*2.150.2    06-Dec-2022      Abhishek           Created - Game Server Cloud Movement.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// DisplayPanelContainer class
    /// </summary>
    public class DisplayPanelContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<DisplayPanelDTO> displayPanelDTOList;
        private readonly DateTime? displayPanelModuleLastUpdateTime;
        private readonly int siteId;
        private readonly DisplayPanelContainerDTOCollection displayPanelContainerDTOCollection;
        private readonly Dictionary<int, DisplayPanelContainerDTO> panelIdDisplayPanelDTODictionary = new Dictionary<int, DisplayPanelContainerDTO>();

        /// <summary>
        /// Default Container Constructor
        /// </summary>
        /// <param name="siteId"></param>
        public DisplayPanelContainer(int siteId) : this(siteId, GetDisplayPanelDTOList(siteId), GetDisplayPanelModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameters siteId, displayPanelDTOList, displayPanelModuleLastUpdateTime
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="displayPanelDTOList"></param>
        /// <param name="displayPanelModuleLastUpdateTime"></param>
        public DisplayPanelContainer(int siteId, List<DisplayPanelDTO> displayPanelDTOList, DateTime? displayPanelModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId, displayPanelDTOList, displayPanelModuleLastUpdateTime);
            this.siteId = siteId;
            this.displayPanelDTOList = displayPanelDTOList;
            this.displayPanelModuleLastUpdateTime = displayPanelModuleLastUpdateTime;          
            List<DisplayPanelContainerDTO> displayPanelContainerDTOList = new List<DisplayPanelContainerDTO>();
            foreach (DisplayPanelDTO displayPanelDTO in displayPanelDTOList)
            {
                if (panelIdDisplayPanelDTODictionary.ContainsKey(displayPanelDTO.PanelId))
                {
                    continue;
                }
                DisplayPanelContainerDTO displayPanelContainerDTO = new DisplayPanelContainerDTO(displayPanelDTO.PanelId, displayPanelDTO.PanelName, displayPanelDTO.PCName,
                    displayPanelDTO.DisplayGroup, displayPanelDTO.Location, displayPanelDTO.MACAddress, displayPanelDTO.Description, displayPanelDTO.StartTime,
                    displayPanelDTO.EndTime, displayPanelDTO.ResolutionX, displayPanelDTO.ResolutionY, displayPanelDTO.LocalFolder, displayPanelDTO.RestartFlag,
                    displayPanelDTO.LastRestartTime, displayPanelDTO.ShutdownSec, displayPanelDTO.IsActive);
                displayPanelContainerDTOList.Add(displayPanelContainerDTO);
                panelIdDisplayPanelDTODictionary.Add(displayPanelContainerDTO.PanelId, displayPanelContainerDTO);
            }
            displayPanelContainerDTOCollection = new DisplayPanelContainerDTOCollection(displayPanelContainerDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get the latest update time of Display Panel table from DB.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private static DateTime? GetDisplayPanelModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                DisplayPanelListBL displayPanelListBL = new DisplayPanelListBL();
                result = displayPanelListBL.GetDisplayPanelModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the Display Panel max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get all the active Display Panel records for the given siteId.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private static List<DisplayPanelDTO> GetDisplayPanelDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<DisplayPanelDTO> displayPanelDTOList = null;
            try
            {
                DisplayPanelListBL displayPanelListBL = new DisplayPanelListBL();
                List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.IS_ACTIVE, "1"));
                displayPanelDTOList = displayPanelListBL.GetDisplayPanelDTOList(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the Display Panel.", ex);
            }

            if (displayPanelDTOList == null)
            {
                displayPanelDTOList = new List<DisplayPanelDTO>();
            }
            log.LogMethodExit(displayPanelDTOList);
            return displayPanelDTOList;
        }

        /// <summary>
        /// Returns displayPanelContainerDTOCollection.
        /// </summary>
        /// <returns></returns>
        public DisplayPanelContainerDTOCollection GetDisplayPanelContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(displayPanelContainerDTOCollection);
            return displayPanelContainerDTOCollection;
        }

        /// <summary>
        /// Refresh the container if there is any update in Db.
        /// </summary>
        /// <returns></returns>
        public DisplayPanelContainer Refresh()
        {
            log.LogMethodEntry();
            DisplayPanelListBL displayPanelListBL = new DisplayPanelListBL();
            DateTime? updateTime = displayPanelListBL.GetDisplayPanelModuleLastUpdateTime(siteId);
            if (displayPanelModuleLastUpdateTime.HasValue
                && displayPanelModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Display Panel since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            DisplayPanelContainer result = new DisplayPanelContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}

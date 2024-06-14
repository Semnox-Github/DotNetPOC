/********************************************************************************************
* Project Name -DigitalSignage
* Description  - DisplayPanelContainerDTO class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
 *2.150.2    06-Dec-2022      Abhishek           Created - Game Server Cloud Movement.
********************************************************************************************/
using System;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// DisplayPanelContainerDTO Class
    /// </summary>
    public class DisplayPanelContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int panelId;
        private string panelName;
        private string pCName;
        private string displayGroup;
        private string location;
        private string mACAddress;
        private string description;
        private decimal startTime;
        private decimal endTime;
        private int resolutionX;
        private int resolutionY;
        private string localFolder;
        private string restartFlag;
        private DateTime? lastRestartTime;
        private int? shutdownSec;
        private bool isActive;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DisplayPanelContainerDTO()
        {
            log.LogMethodEntry();
            panelId = -1;
            panelName = string.Empty;
            pCName = string.Empty;
            displayGroup = string.Empty;
            location = string.Empty;
            mACAddress = string.Empty;
            description = string.Empty;
            resolutionX = -1;
            resolutionY = -1;
            localFolder = string.Empty;
            restartFlag = "N";
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        ///
        public DisplayPanelContainerDTO(int panelId, string panelName, string pCName, string displayGroup, string location,
                                        string mACAddress, string description, decimal startTime, decimal endTime, int resolutionX,
                                        int resolutionY, string localFolder, string restartFlag, DateTime? lastRestartTime,
                                        int? shutdownSec, bool isActive)
            : this()
        {
            log.LogMethodEntry(panelId, panelName, pCName, displayGroup, location, mACAddress, description, startTime, endTime,
                               resolutionX, resolutionY, localFolder, restartFlag, lastRestartTime, shutdownSec, isActive);
            this.panelId = panelId;
            this.panelName = panelName;
            this.pCName = pCName;
            this.displayGroup = displayGroup;
            this.location = location;
            this.mACAddress = mACAddress;
            this.description = description;
            this.startTime = startTime;
            this.endTime = endTime;
            this.resolutionX = resolutionX;
            this.resolutionY = resolutionY;
            this.localFolder = localFolder;
            this.restartFlag = restartFlag;
            this.lastRestartTime = lastRestartTime;
            this.shutdownSec = shutdownSec;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PanelId field
        /// </summary>
        public int PanelId { get { return panelId; } set { panelId = value; } }

        /// <summary>
        /// Get/Set method of the PanelName field
        /// </summary>
        public string PanelName { get { return panelName; } set { panelName = value; } }

        /// <summary>
        /// Get/Set method of the PCName field
        /// </summary>
        public string PCName { get { return pCName; } set { pCName = value; } }

        /// <summary>
        /// Get/Set method of the DisplayGroup field
        /// </summary>
        public string DisplayGroup { get { return displayGroup; } set { displayGroup = value; } }

        /// <summary>
        /// Get/Set method of the Location field
        /// </summary>
        public string Location { get { return location; } set { location = value; } }

        /// <summary>
        /// Get/Set method of the MACAddress field
        /// </summary>
        public string MACAddress { get { return mACAddress; } set { mACAddress = value; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description { get { return description; } set { description = value; } }

        /// <summary>
        /// Get/Set method of the StartTime field
        /// </summary>
        public decimal StartTime { get { return startTime; } set { startTime = value; } }

        /// <summary>
        /// Get/Set method of the EndTime field
        /// </summary>
        public decimal EndTime { get { return endTime; } set { endTime = value; } }

        /// <summary>
        /// Get/Set method of the ResolutionX field
        /// </summary>
        public int ResolutionX { get { return resolutionX; } set { resolutionX = value; } }

        /// <summary>
        /// Get/Set method of the ResolutionY field
        /// </summary>
        public int ResolutionY { get { return resolutionY; } set { resolutionY = value; } }

        /// <summary>
        /// Get/Set method of the LocalFolder field
        /// </summary>
        public string LocalFolder { get { return localFolder; } set { localFolder = value; } }

        /// <summary>
        /// Get/Set method of the RestartFlag field
        /// </summary>
        public string RestartFlag { get { return restartFlag; } set { restartFlag = value; } }

        /// <summary>
        /// Get/Set method of the LastRestartTime field
        /// </summary>
        public DateTime? LastRestartTime { get { return lastRestartTime; } set { lastRestartTime = value; } }

        /// <summary>
        /// Get/Set method of the ShutdownSec field
        /// </summary>
        public int? ShutdownSec { get { return shutdownSec; } set { shutdownSec = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; } }
    }
}

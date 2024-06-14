﻿/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu panel exclusion data transfer object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By           Remarks          
 ********************************************************************************************* 
 *2.130.0     19-Jul-2021      Lakshminarayana       Created
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Product
{
    public class ProductMenuPanelExclusionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        private int id;
        private int panelId;
        private int pOSMachineId;
        private int userRoleId;
        private int posTypeId;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;

        public ProductMenuPanelExclusionDTO()
        {
            log.LogMethodEntry();
            id = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            pOSMachineId = -1;
            panelId = -1;
            userRoleId = -1;
            posTypeId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public ProductMenuPanelExclusionDTO(int id, int panelId, int pOSMachineId, int userRoleId, int posTypeId, bool isActive)
            : this()
        {
            log.LogMethodEntry(id, panelId, pOSMachineId, userRoleId, posTypeId, isActive);
            this.id = id;
            this.panelId = panelId;
            this.pOSMachineId = pOSMachineId;
            this.userRoleId = userRoleId;
            this.posTypeId = posTypeId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductMenuPanelExclusionDTO(int id, int panelId, int pOSMachineId, int userRoleId, int posTypeId, string guid, int siteId, bool synchStatus, int masterEntityId, string createdBy,
                             DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, bool isActive)
            : this(id, panelId, pOSMachineId, userRoleId, posTypeId, isActive)
        {
            log.LogMethodEntry(id, panelId, pOSMachineId, userRoleId, posTypeId, guid, siteId, synchStatus, masterEntityId, creationDate, lastUpdatedBy, lastUpdateDate, isActive);
            this.masterEntityId = masterEntityId;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { this.IsChanged = true; id = value; } }

        /// <summary>
        /// Get/Set method of the panelId field
        /// </summary>
        public int PanelId { get { return panelId; } set { this.IsChanged = true; panelId = value; } }
        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        public int POSMachineId { get { return pOSMachineId; } set { this.IsChanged = true; pOSMachineId = value; } }

        /// <summary>
        /// Get/Set method of the userRoleId field
        /// </summary>
        public int UserRoleId { get { return userRoleId; } set { this.IsChanged = true; userRoleId = value; } }

        /// <summary>
        /// Get/Set method of the posTypeId field
        /// </summary>
        public int POSTypeId { get { return posTypeId; } set { this.IsChanged = true; posTypeId = value; } }

        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

    }
}
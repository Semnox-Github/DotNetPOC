/********************************************************************************************
 * Project Name - MachineGroups DTO
 * Description  - Data object of MachineGroups
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************
 *2.60        13-May-2019   Jagan Mohana Rao        Created 
 *2.70.2        31-Jul-2019   Deeksha                 Modified:Added a new constructor with required fields.
 *                                                  Added a recursive function for the list DTO.
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Game
{
    public class MachineGroupsDTO
    {
        /// <summary>
        /// Data object for MachineGroups
        /// </summary>
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by MACHINE GROUP ID field
            /// </summary>
            MACHINE_GROUP_ID,
            /// <summary>
            /// Search by NAME field
            /// </summary>
            GROUP_NAME ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            ISACTIVE ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
        }

        private int machineGroupId;
        private string groupName;
        private string remarks;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private bool isActive;
        private List<MachineGroupMachinesDTO> machineGroupMachinesDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineGroupsDTO()
        {
            log.LogMethodEntry();
            machineGroupId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            machineGroupMachinesDTOList = new List<MachineGroupMachinesDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public MachineGroupsDTO(int machineGroupId, string groupName, string remarks, bool isActive)
            :this()
        {
            log.LogMethodEntry(machineGroupId, groupName, remarks, isActive);
            this.machineGroupId = machineGroupId;
            this.groupName = groupName;
            this.remarks = remarks;            
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MachineGroupsDTO(int machineGroupId, string groupName, string remarks, string guid, int siteId, bool synchStatus, int masterEntityId,
                           string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, bool isActive)
            :this(machineGroupId, groupName, remarks, isActive)
        {
            log.LogMethodEntry(machineGroupId, groupName, remarks, siteId, guid, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, isActive);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MachineGroupId field
        /// </summary>
        [DisplayName("Group Id")]
        public int MachineGroupId { get { return machineGroupId; } set { machineGroupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the GroupName field
        /// </summary>
        [DisplayName("Group Name")]
        public string GroupName { get { return groupName; } set { groupName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("Site id")]
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Is Active")]
        [Browsable(false)]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MachineGroupMachinesList field
        /// </summary>
        [DisplayName("MachineGroupMachines")]
        public List<MachineGroupMachinesDTO> MachineGroupMachinesList { get { return machineGroupMachinesDTOList; } set { machineGroupMachinesDTOList = value; } }

        /// <summary>
        /// Returns whether the MachineGroupMachinesDTO changed or any of its Lists  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (machineGroupMachinesDTOList != null &&
                   machineGroupMachinesDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || machineGroupId < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
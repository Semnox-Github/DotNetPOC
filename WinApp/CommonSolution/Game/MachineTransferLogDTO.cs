/********************************************************************************************
 * Project Name - Games
 * Description  - DTO of Machine transfer log
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60        27-Mar-2019   Jagan Mohana          Created 
 *2.70.2        18-Jun-2019   Girish Kundar  Modified:Added List <MachineDTO> as a Data Member.
 *2.70.2        29-Jul-2019   Deeksha        Modifications as per 3 tier standard.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Game
{
    public class MachineTransferLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by MACHINE ID field
            /// </summary>
            MACHINE_ID,
            /// <summary>
            /// Search by FROM SITE ID field
            /// </summary>
            FROM_SITE_ID,
            /// <summary>
            /// Search by TO SITE ID field
            /// </summary>
            TO_SITE_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTIY_ID
        }

       private int machineTransferLogId;
       private int machineId;
       private int fromSiteId;
       private int toSiteId;
       private DateTime transferDate;
       private string transferedBy;
       private string fromMachineStatus;
       private string toMachineStatus;
       private string sourceMachineGUID;
       private string targetMachineGUID;
       private string remarks;
       private int siteId;
       private string guid;
       private bool synchStatus;
       private int masterEntityId;
       private string createdBy;
       private DateTime creationDate;
       private string lastUpdatedBy;
       private DateTime lastUpdateDate;
       private bool isActive;
       private List<MachineDTO> machineDTOList;  

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineTransferLogDTO()
        {
            log.LogMethodEntry();
            this.machineTransferLogId = -1;
            this.masterEntityId = -1;
            this.isActive = true;
            this.siteId = -1;
            this.machineId = -1;
            this.fromSiteId = -1;
            this.toSiteId = -1;
            machineDTOList = new List<MachineDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public MachineTransferLogDTO(int machineTransferLogId, int machineId, int fromSiteId, int toSiteId, DateTime transferDate, string transferedBy, string fromMachineStatus,
                             string toMachineStatus, string sourceMachineGUID, string targetMachineGUID, string remarks, bool isActive)
            :this()
        {
            log.LogMethodEntry(machineTransferLogId, machineId, fromSiteId, toSiteId, transferDate, transferedBy, fromMachineStatus, toMachineStatus, sourceMachineGUID, targetMachineGUID,
                               remarks,isActive);
            this.machineTransferLogId = machineTransferLogId;
            this.machineId = machineId;
            this.fromSiteId = fromSiteId;
            this.toSiteId = toSiteId;
            this.transferDate = transferDate;
            this.transferedBy = transferedBy;
            this.fromMachineStatus = fromMachineStatus;
            this.toMachineStatus = toMachineStatus;
            this.sourceMachineGUID = sourceMachineGUID;
            this.targetMachineGUID = targetMachineGUID;
            this.remarks = remarks;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MachineTransferLogDTO(int machineTransferLogId, int machineId, int fromSiteId, int toSiteId, DateTime transferDate,string transferedBy,string fromMachineStatus,
                             string toMachineStatus, string sourceMachineGUID, string targetMachineGUID,string remarks, int siteId,string guid, bool synchStatus,
                            int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,bool isActive)
            :this(machineTransferLogId, machineId, fromSiteId, toSiteId, transferDate, transferedBy, fromMachineStatus, toMachineStatus, sourceMachineGUID, targetMachineGUID,
                               remarks, isActive)
        {
            log.LogMethodEntry(machineTransferLogId, machineId, fromSiteId, toSiteId, transferDate, transferedBy, fromMachineStatus, toMachineStatus, sourceMachineGUID, targetMachineGUID, siteId, guid,
                               synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MachineTransferLogId field
        /// </summary>
        [DisplayName("MachineTransferLogId")]
        [ReadOnly(true)]
        public int MachineTransferLogId { get { return machineTransferLogId; } set { machineTransferLogId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MachineId field
        /// </summary>
        [DisplayName("MachineId")]
        public int MachineId { get { return machineId; } set { machineId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FromSiteId field
        /// </summary>
        [DisplayName("FromSiteId")]
        public int FromSiteId { get { return fromSiteId; } set { fromSiteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ToSiteId field
        /// </summary>
        [DisplayName("ToSiteId")]
        public int ToSiteId { get { return toSiteId; } set { toSiteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TransferDate field
        /// </summary>
        [DisplayName("TransferDate")]
        public DateTime TransferDate { get { return transferDate; } set { transferDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TransferedBy field
        /// </summary>
        [DisplayName("TransferedBy")]
        public string TransferedBy { get { return transferedBy; } set { transferedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FromMachineStatus field
        /// </summary>
        [DisplayName("FromMachineStatus")]
        public string FromMachineStatus { get { return fromMachineStatus; } set { fromMachineStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ToMachineStatus field
        /// </summary>
        [DisplayName("ToMachineStatus")]
        public string ToMachineStatus { get { return toMachineStatus; } set { toMachineStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SourceMachineGUID field
        /// </summary>
        [DisplayName("SourceMachineGUID")]
        public string SourceMachineGUID { get { return sourceMachineGUID; } set { sourceMachineGUID = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TargetMachineGUID field
        /// </summary>
        [DisplayName("TargetMachineGUID")]
        public string TargetMachineGUID { get { return targetMachineGUID; } set { targetMachineGUID = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value;  } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SyncStatus field
        /// </summary>
        [DisplayName("SyncStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;  } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value;  } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MachineDTOList field
        /// </summary>
        public List<MachineDTO> MachineDTOList
        {
            get { return machineDTOList; }
            set { machineDTOList = value; }
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
                    return notifyingObjectIsChanged || machineTransferLogId < 0;
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
        /// Returns whether the AppUIPanelsDTO changed or any of its childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (machineDTOList != null &&
                   machineDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
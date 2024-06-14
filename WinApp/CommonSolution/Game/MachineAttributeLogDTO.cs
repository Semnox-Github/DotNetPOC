/********************************************************************************************
 * Project Name - Games                                                                         
 * Description  - MachineAttributeLogDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.100       05-Sep-2020   Girish Kundar        Created
 *2.130       27-Aug-2021   Abhishek             Modified: Added new field UpdateType
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Game
{
    public class MachineAttributeLogDTO
    {

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// FromType Enum
        /// </summary>
        public enum UpdateTypes
        {
            ///<summary>
            ///ADD_MACHINE
            ///</summary>
            NONE = -1,
            ///<summary>
            ///ADD_MACHINE
            ///</summary>
            ADD_MACHINE = 0,

            ///<summary>
            ///EDIT_MACHINE
            ///</summary>
            EDIT_MACHINE = 1,

            ///<summary>
            ///IN_TO_SERVICE
            ///</summary>
            IN_TO_SERVICE = 2,

            ///<summary>
            ///OUT_OF_SERVICE
            ///</summary>
            OUT_OF_SERVICE = 3,

            ///<summary>
            ///Management Studio Update
            ///</summary>
            MANAGEMENT_STUDIO_UPDATE = 4

        }
        public enum SearchByParameters
        {
            /// <summary>
            /// ID search field
            /// </summary>
            ID,
            /// <summary>
            /// POS_NAME search field
            /// </summary>
            POS_NAME,
            /// <summary>
            /// MACHINE ID search field
            /// </summary>
            MACHINE_ID,
            // <summary>
            /// SITE_ID search field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            ///POS MACHINE ID search field
            /// </summary>
            POS_MACHINEID,
            /// <summary>
            ///UPDATE_TYPE search field
            /// </summary>
            UPDATE_TYPE,
            /// <summary>
            ///STATUS search field
            /// </summary>
            STATUS
        }
        private int id;
        private int machineId;
        private int posMachineId;
        private string posMachineName;
        private string message;
        private string userReason;
        private string userRemarks;
        private string updateType;
        private bool status;
        private DateTime? timeStamp;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastupdatedBy;
        private DateTime lastupdateDate;
        private int dataRate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineAttributeLogDTO()
        {
            log.LogMethodEntry();
            id = -1;
            machineId = -1;
            posMachineId = -1;
            siteId = -1;
            masterEntityId = -1;
            timeStamp = null;
            status = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required the data fields
        /// </summary>
        public MachineAttributeLogDTO(int id, int machineId, int posMachineId, string posName, string message,
                          string userReason, string userRemarks, bool status, DateTime? timeStamp, string updateType)
            : this()
        {
            log.LogMethodEntry(id, machineId, posMachineId, posName, message, userReason, userRemarks, status, timeStamp, updateType);
            this.id = id;
            this.machineId = machineId;
            this.posMachineId = posMachineId;
            this.posMachineName = posName;
            this.message = message;
            this.userReason = userReason;
            this.userRemarks = userRemarks;
            this.status = status;
            this.timeStamp = timeStamp;
            this.updateType = updateType;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MachineAttributeLogDTO(int id, int machineId, int posMachineId, string posName, string message,
                          string userReason, string userRemarks, bool status, DateTime? timeStamp, string updateType,
                          string guid, int siteId, bool synchStatus, int masterEntityId, string createdBy,
                          DateTime creationDate, string lastupdatedBy, DateTime lastupdateDate)
            : this(id, machineId, posMachineId, posName, message, userReason, userRemarks, status, timeStamp, updateType)
        {
            log.LogMethodEntry(id, machineId, posMachineId, posName, message, userReason, userRemarks, status, timeStamp, updateType, guid, siteId,
                              synchStatus, masterEntityId, createdBy, creationDate, lastupdatedBy,
                              lastupdateDate);

            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastupdatedBy = lastupdatedBy;
            this.lastupdateDate = lastupdateDate;
            log.LogMethodExit();
        }


        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }
        public int MachineId { get { return machineId; } set { machineId = value; this.IsChanged = true; } }
        public int POSMachineId { get { return posMachineId; } set { posMachineId = value; this.IsChanged = true; } }
        public string POSMachineName { get { return posMachineName; } set { posMachineName = value; this.IsChanged = true; } }
        public string Message { get { return message; } set { message = value; this.IsChanged = true; } }
        public string UserReason { get { return userReason; } set { userReason = value; this.IsChanged = true; } }
        public string UserRemarks { get { return userRemarks; } set { userRemarks = value; this.IsChanged = true; } }
        public bool Status { get { return status; } set { status = value; this.IsChanged = true; } }
        public DateTime? Timestamp { get { return timeStamp; } set { timeStamp = value; this.IsChanged = true; } }
        public string UpdateType { get { return updateType; } set { updateType = value; this.IsChanged = true; } }
        public string Guid { get { return guid; } set { guid = value; } }
        public int SiteId { get { return siteId; } set { siteId = value; } }
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        public string LastupdatedBy { get { return lastupdatedBy; } set { lastupdatedBy = value; } }
        public DateTime LastupdateDate { get { return lastupdateDate; } set { lastupdateDate = value; } }
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

/********************************************************************************************
 * Project Name - FacilityPOSAssignmentDTO
 * Description  - DTO class for FacilityPOSAssignment
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.70        08-Mar-2019   Guru S A       Moved to POS namespace
 *2.70        07-May-2019   Akshay G       Added isActive
 ********************************************************************************************/
using System;
using System.ComponentModel; 

namespace Semnox.Parafait.POS
{
    public class FacilityPOSAssignmentDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by Id
            /// </summary>
            ID,
            /// <summary>
            /// Search by POSMachineId
            /// </summary>
            POS_MACHINE_ID,
            /// <summary>
            /// Search by FacilityId
            /// </summary>
            FACILITY_ID,
            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by isActive
            /// </summary>
            IS_ACTIVE
        }

        private int id;
        private int pOSMachineId;
        private int facilityId;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;

        public FacilityPOSAssignmentDTO()
        {
            log.LogMethodEntry();
            id = -1;
            pOSMachineId = -1;
            facilityId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        public FacilityPOSAssignmentDTO(int id, int pOSMachineId, int facilityId, DateTime creationDate,
                            string createdBy, DateTime lastUpdateDate, string lastUpdatedBy, int site_id,
                            string guid, bool synchStatus, int masterEntityId, bool isActive)
        {
            log.LogMethodEntry(id, pOSMachineId, facilityId,
                               creationDate, createdBy, lastUpdateDate,
                               lastUpdatedBy, site_id, guid, synchStatus,
                               masterEntityId, isActive);
            this.id = id;
            this.pOSMachineId = pOSMachineId;
            this.facilityId = facilityId;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the id field
        /// </summary>
        [Browsable(false)]
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the pOSMachineId field
        /// </summary>
        [DisplayName("POSMachine")]
        public int POSMachineId
        {
            get
            {
                return pOSMachineId;
            }
            set
            {
                pOSMachineId = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the facilityId field
        /// </summary>
        [DisplayName("Facility")]
        public int FacilityId
        {
            get
            {
                return facilityId;
            }
            set
            {
                facilityId = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                this.IsChanged = true;
                lastUpdateDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                this.IsChanged = true;
                lastUpdatedBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return site_id;
            }
            set
            {
                this.IsChanged = true;
                site_id = value;
            }
        }
        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }
        /// <summary>
        /// Get/Set method of the synchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                this.IsChanged = true;
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        [Browsable(false)]
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                this.IsChanged = true;
                isActive = value;
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

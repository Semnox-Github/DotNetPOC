/********************************************************************************************
 * Project Name - FacilityMapDetailsDTO
 * Description  - Data object of FacilityMapDetails
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        14-Jun-2019   Guru S A                Created 
 *2.70.2      18-Oct-2019   Akshay G                ClubSpeed enhancement changes
 *2.80.0      26-Feb-2020   Girish Kundar           Modified : 3 Tier Changes for API
 *********************************************************************************************/

using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the FacilityMapDetails data object class. This acts as data holder for the FacilityMapDetails business object
    /// </summary>
    public class FacilityMapDetailsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {

            /// <summary>
            /// Search by  FACILITY_MAP_DETAIL_ID  field
            /// </summary>
            FACILITY_MAP_DETAIL_ID,
            /// <summary>
            /// Search by  FACILITY_MAP_ID  field
            /// </summary>
            FACILITY_MAP_ID,
            /// <summary>
            /// Search by  FACILITY_MAP_ID  field
            /// </summary>
            FACILITY_MAP_ID_LIST,
            /// <summary>
            /// Search by  FACILITY_ID field
            /// </summary>
            FACILITY_ID,
            ///<summary>
            /// Search by  IS_ACTIVE field
            /// <summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int facilityMapDetailId;
        private int facilityMapId;
        private int facilityId;
        private string facilityName;
        private bool isActive;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private List<FacilityDTO> facilityDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public FacilityMapDetailsDTO()
        {
            log.LogMethodEntry();
            facilityMapDetailId = -1;
            facilityMapId = -1;
            facilityId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            facilityDTOList = new List<FacilityDTO>(); ;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public FacilityMapDetailsDTO(int facilityMapDetailId, int facilityMapId, int facilityId, bool isActive)
            : this()
        {
            log.LogMethodEntry(facilityMapDetailId, facilityMapId, facilityId, isActive);
            this.facilityMapDetailId = facilityMapDetailId;
            this.facilityMapId = facilityMapId;
            this.facilityId = facilityId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public FacilityMapDetailsDTO(int facilityMapDetailId, int facilityMapId, int facilityId, string facilityName, bool isActive,
                                  string guid, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, int siteId,
                                  bool synchStatus, int masterEntityId) :
            this(facilityMapDetailId, facilityMapId, facilityId, isActive)
        {
            log.LogMethodEntry(facilityMapDetailId, facilityMapId, facilityId, isActive,
                                guid, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.facilityName = facilityName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the facilityMapDetailId field
        /// </summary>
        public int FacilityMapDetailId
        {
            get { return facilityMapDetailId; }
            set { facilityMapDetailId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the facilityMapId field
        /// </summary>
        public int FacilityMapId
        {
            get { return facilityMapId; }
            set { facilityMapId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the facilityId field
        /// </summary>
        public int FacilityId
        {
            get { return facilityId; }
            set { facilityId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the FacilityName field
        /// </summary>
        public string FacilityName
        {
            get { return facilityName; }
            set { facilityName = value; }
        }
        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the FacilityDTO (in memory only) field
        /// </summary>
        public List<FacilityDTO> FacilityDTOList
        {
            get { return facilityDTOList; }
            set { facilityDTOList = value; }
        }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || facilityMapId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}

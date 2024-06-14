/********************************************************************************************
 * Project Name - FacilityMapDTO
 * Description  - Data object of FacilityMap
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        14-Jun-2019   Guru S A           Created 
 *2.70.2      18-Oct-2019   Akshay G           ClubSpeed enhancement changes - Added searchParameters 
 *            01-Jan-2020   Akshay G           Added searchParameters - ALLOWED_PRODUCTS_WITH_EXTERNAL_SYSTEM_REFERENCE and ALLOWED_PRODUCTS_IS_COMBO_CHILD
 *2.80.0      26-Feb-2020   Girish Kundar       Modified : 3 Tier Changes for API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the FacilityMap data object class. This acts as data holder for the FacilityMap business object
    /// </summary>
    public class FacilityMapDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {

            /// <summary>
            /// Search by  FACILITY_MAP_ID  field
            /// </summary>
            FACILITY_MAP_ID,
            /// <summary>
            /// Search by  FACILITY_MAP_ID  field
            /// </summary>
            FACILITY_MAP_ID_LIST,
            /// <summary>
            /// Search by  MASTER_SCHEDULE_ID field
            /// </summary>
            MASTER_SCHEDULE_ID,
            /// <summary>
            /// Search by  CANCELLATION_PRODUCT_ID field
            /// </summary>
            CANCELLATION_PRODUCT_ID,
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
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by HAVING_PRODUCT_TYPES_IN field
            /// </summary>
            HAVING_PRODUCT_TYPES_IN,
            /// <summary>
            /// Search by  FACILITY_MAP_IDS_IN  field
            /// </summary>
            FACILITY_MAP_IDS_IN,
            /// <summary>
            /// Search by  ALLOWED_PRODUCT_ID  field
            /// </summary>
            ALLOWED_PRODUCT_ID,
            /// <summary>
            /// Search by  ALLOWED_PRODUCT_IDS_IN  field
            /// </summary>
            ALLOWED_PRODUCT_IDS_IN,
            /// <summary>
            /// Search by FACILITY_ID field
            /// </summary>
            FACILITY_ID,
            /// <summary>
            /// Search by  FACILITY_IDS_IN  field
            /// </summary>
            FACILITY_IDS_IN,
            /// <summary>
            /// Search by LAST_UPDATED_FROM_DATE field
            /// </summary>
            LAST_UPDATED_FROM_DATE,
            /// <summary>
            /// Search by LAST_UPDATED_TO_DATE field
            /// </summary>
            LAST_UPDATED_TO_DATE,
            /// <summary>
            /// Search by ALLOWED_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE field
            /// </summary>
            ALLOWED_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE,
            /// <summary>
            /// Search by ALLOWED_PRODUCTS_IS_COMBO_CHILD field
            /// </summary>
            ALLOWED_PRODUCTS_IS_COMBO_CHILD,
            /// <summary>
            /// Search by FACILITY_MAP_NAME field
            /// </summary>
            FACILITY_MAP_NAME

        }
        private int facilityMapId;
        private string facilityMapName;
        private int masterScheduleId;
        private int cancellationProductId;
        private bool isActive;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private int? facilityCapacity;
        private int? graceTime;
        private List<FacilityMapDetailsDTO> facilityMapDetailsDTOList;
        private List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public FacilityMapDTO()
        {
            log.LogMethodEntry();
            facilityMapId = -1;
            masterScheduleId = -1;
            cancellationProductId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            facilityMapDetailsDTOList = new List<FacilityMapDetailsDTO>();
            productsAllowedInFacilityDTOList = new List<ProductsAllowedInFacilityMapDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public FacilityMapDTO(int facilityMapId, string facilityMapName, int masterScheduleId, int cancellationProductId, int? graceTime, bool isActive)
            : this()
        {
            log.LogMethodEntry(facilityMapId, facilityMapName, masterScheduleId, graceTime, isActive);
            this.facilityMapId = facilityMapId;
            this.facilityMapName = facilityMapName;
            this.masterScheduleId = masterScheduleId;
            this.cancellationProductId = cancellationProductId;
            this.graceTime = graceTime;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public FacilityMapDTO(int facilityMapId, string facilityMapName, int masterScheduleId, int cancellationProductId, int? graceTime, bool isActive,
                                  string guid, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, int siteId,
                                  bool synchStatus, int masterEntityId, int? facilityCapacity) :
            this(facilityMapId, facilityMapName, masterScheduleId, cancellationProductId, graceTime, isActive)
        {
            log.LogMethodEntry(facilityMapId, facilityMapName, masterScheduleId, cancellationProductId, graceTime, isActive,
                                guid, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, siteId, synchStatus, masterEntityId, facilityCapacity);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.facilityCapacity = facilityCapacity;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the FacilityMapId field
        /// </summary>
        public int FacilityMapId
        {
            get { return facilityMapId; }
            set { facilityMapId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the FacilityMapName field
        /// </summary>
        public string FacilityMapName
        {
            get { return facilityMapName; }
            set { facilityMapName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the masterScheduleId field
        /// </summary>
        public int MasterScheduleId
        {
            get { return masterScheduleId; }
            set { masterScheduleId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the cancellationProductId field
        /// </summary>
        public int CancellationProductId
        {
            get { return cancellationProductId; }
            set { cancellationProductId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the GraceTime field
        /// </summary> 
        public int? GraceTime { get { return graceTime; } set { graceTime = value; this.IsChanged = true; } }

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
        /// Get/Set method of the facilityCapacity field
        /// </summary>
        public int? FacilityCapacity
        {
            get { return facilityCapacity; }
            set { facilityCapacity = value; }
        }

        /// <summary>
        /// Get/Set method of the facilityMapDetailsDTOList field
        /// </summary>
        public List<FacilityMapDetailsDTO> FacilityMapDetailsDTOList
        {
            get { return facilityMapDetailsDTOList; }
            set { facilityMapDetailsDTOList = value; }
        }
        /// <summary>
        /// Get/Set method of the productsAllowedInFacilityDTOList field
        /// </summary>
        public List<ProductsAllowedInFacilityMapDTO> ProductsAllowedInFacilityDTOList
        {
            get { return productsAllowedInFacilityDTOList; }
            set { productsAllowedInFacilityDTOList = value; }
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
                if (facilityMapDetailsDTOList != null &&
                   facilityMapDetailsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (productsAllowedInFacilityDTOList != null &&
                  productsAllowedInFacilityDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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

/********************************************************************************************
 * Project Name - Waiver
 * Description  - Data object of the WaiverSet
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        21-Sep-2016    Amaresh          Created 
 *2.70        01-Jul -2019   Girish Kundar    Modified : Added Parametrized Constructor with required fields.
 *2.70.2        23-Sep-2019    Deeksha          Waiver phase 2 changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Waiver
{
    /// <summary>
    ///  This is the user data object class. This acts as data holder for the user business object
    /// </summary>   
    public class WaiverSetDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByWaiverParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 

        public enum SearchByWaiverParameters
        {
            /// <summary>
            /// Search by WAIVERSET ID field
            /// </summary>
            WAIVER_SET_ID,
            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE,
            /// <summary>
            /// Search by LAST UPDATED BY field
            /// </summary>
            LAST_UPDATED_BY,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by SYNCH STATUS field
            /// </summary>
            SYNCH_STATUS,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by WAIVER_SET_ID_LIST field
            /// </summary>
            WAIVER_SET_ID_LIST
        }
        private int waiverSetId;
        private string name;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private int site_id;
        private bool synchStatus;
        private int masterEntityId;
        private List<WaiversDTO> waiverSetDetailDTOList;
        private List<WaiverSetSigningOptionsDTO> waiverSetSigningOptionDTOList;
        private string description;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WaiverSetDTO()
        {
            log.LogMethodEntry();
            waiverSetId = -1;
            masterEntityId = -1;
            isActive = true;
            site_id = -1;
            waiverSetDetailDTOList = new List<WaiversDTO>();
            waiverSetSigningOptionDTOList = new List<WaiverSetSigningOptionsDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public WaiverSetDTO(int waiverSetId, string name, bool isActive, string description)
            : this()
        {
            log.LogMethodEntry(waiverSetId, name, isActive, description);
            this.waiverSetId = waiverSetId;
            this.name = name;
            this.isActive = isActive;
            this.description = description;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public WaiverSetDTO(int waiverSetId, string name, bool isActive, DateTime creationDate, string createdBy, DateTime lastUpdatedDate, string lastUpdatedBy, string guid, int site_id,
                        bool synchStatus, int masterEntityId, string description)
            : this(waiverSetId, name, isActive, description)
        {
            log.LogMethodEntry(waiverSetId, name, isActive, creationDate, createdBy, lastUpdatedDate, lastUpdatedBy, guid, site_id, synchStatus, masterEntityId);
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the WaiverSetId field
        /// </summary>
        [DisplayName("WaiverSet Id")]
        //[ReadOnly(true)]
        public int WaiverSetId { get { return waiverSetId; } set { waiverSetId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }


        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the GUID field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site Id")]
        public int Site_id { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }


        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Name")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the WaiverSetDetailDTOList field
        /// </summary>
        [Browsable(false)]
        public List<WaiversDTO> WaiverSetDetailDTOList
        {
            get { return waiverSetDetailDTOList; }
            set { waiverSetDetailDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of the WaiverSetSigningOptionDTOList field
        /// </summary>
        [Browsable(false)]
        public List<WaiverSetSigningOptionsDTO> WaiverSetSigningOptionDTOList
        {
            get { return waiverSetSigningOptionDTOList; }
            set { waiverSetSigningOptionDTOList = value; }
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
                    return notifyingObjectIsChanged || waiverSetId < 0;
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
        /// Returns whether the CheckInDTO changed or any of its  children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (waiverSetDetailDTOList != null &&
                    waiverSetDetailDTOList.Any(x => x.IsChanged))
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
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}

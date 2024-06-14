/********************************************************************************************
* Project Name - CMSBanners DTO Programs 
* Description  - Data object of the CMSBannersDTO
* 
**************
**Version Log
**************
*Version     Date          Modified By        Remarks          
*********************************************************************************************
*1.00        05-Apr-2016   Rakshith           Created 
*2.70        09-Jul-2019   Girish Kundar      Modified : Added constructor with required parameters.
*                                                        Added masterEntity  field.
*2.80        08-May-2020   Indrajeet Kumar    Modified : Added a property - frequency, CMSBannerItemsDTO & IsChangedRecursive                                      
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.WebCMS
{
    /// <summary>
    /// This is the Banner data object class. This acts as data holder for the Banner business object
    /// </summary>
    public class CMSBannersDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int bannerId;
        private string name;
        private bool active;
        private string guid;
        private bool synchStatus;
        private int site_id;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int masterEntityId;
        private int frequency;
        private List<CMSBannerItemsDTO> cMSBannerItemsDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSBannersDTO()
        {
            log.LogMethodEntry();
            this.bannerId = -1;
            this.site_id = -1;
            this.name = string.Empty;
            this.guid = string.Empty;
            this.masterEntityId = -1;
            this.frequency = -1;
            this.cMSBannerItemsDTOList = new List<CMSBannerItemsDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CMSBannersDTO(int bannerId, string name, bool active)
            : this()
        {
            log.LogMethodEntry(bannerId, name, active);
            this.bannerId = bannerId;
            this.name = name;
            this.active = active;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CMSBannersDTO(int bannerId, string name, bool active, string guid, bool synchStatus, int site_id,
                             string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, int masterEntityId, int frequency)
            : this(bannerId, name, active)
        {
            log.LogMethodEntry(bannerId, name, active, guid, synchStatus, site_id,
                               createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, masterEntityId, frequency);
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.site_id = site_id;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.masterEntityId = masterEntityId;
            this.frequency = frequency;
            log.LogMethodExit();
        }

        /// <summary>
        /// SearchByRequestParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByRequestParameters
        {
            /// <summary>
            /// Search by BANNER ID field
            /// </summary>
            BANNER_ID,
            /// <summary>
            /// Search by ACTIVE field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        /// <summary>
        /// Get/Set method of the BannerId field
        /// </summary>
        [DisplayName("Banner Id")]
        [DefaultValue(-1)]
        public int BannerId { get { return bannerId; } set { bannerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid ")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site id ")]
        [DefaultValue(-1)]
        public int Site_id { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Frequency field
        /// </summary>
        public int Frequency { get { return frequency; } set { frequency = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CMSBannerItemsDTO field
        /// </summary>
        public List<CMSBannerItemsDTO> CMSBannerItemsDTOList { get { return cMSBannerItemsDTOList; } set { cMSBannerItemsDTOList = value; } }


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
                    return notifyingObjectIsChanged;
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
        /// Returns whether CMSBannerDTO changes or any of its child record is changed
        /// </summary>
        [Browsable(false)]

        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (cMSBannerItemsDTOList != null && cMSBannerItemsDTOList.Any(x => x.IsChanged))
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

/********************************************************************************************
 * Project Name - Ads DTO
 * Description  - Data object of Ads
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************
 *2.60        16-May-2019   Jagan Mohana Rao        Created 
 *2.70.2      26-Jan-2020   Girish Kundar           Modified : Changed to Standard format 
 *2.80       20-May-2020    Mushahid Faizan         Modified : 3 tier changes for Rest API. 
 *2.90       20-Aug-2020    Girish Kundar           Modified : Added base64Image field to DTO
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.ServerCore
{
    public class AdsDTO
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
            /// Search by AD_ID field
            /// </summary>
            AD_ID,
            /// <summary>
            /// Search by AD_NAME field
            /// </summary>
            AD_NAME,
            /// <summary>
            /// Search by AD_TYPE field
            /// </summary>
            AD_TYPE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int adId;
        private string adName;
        private string adImageFileUser;
        private byte[] img;
        private string adImageFileSystem;
        private string adText;
        private bool isActive;
        private string adType;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private List<AdBroadcastDTO> adBroadcastDTOList;
        private string base64ImageString;
        /// <summary>
        /// Default constructor
        /// </summary>
        public AdsDTO()
        {
            log.LogMethodEntry();
            adId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            adBroadcastDTOList = new List<AdBroadcastDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public AdsDTO(int adId, string adName, string adImageFileUser, string adImageFileSystem, string adText, bool isActive, string adType)
            : this()
        {
            log.LogMethodEntry(adId, adName, adImageFileUser, adImageFileSystem, adText, isActive, adType);
            this.adId = adId;
            this.adName = adName;
            this.adImageFileUser = adImageFileUser;
            this.adImageFileSystem = adImageFileSystem;
            this.adText = adText;
            this.isActive = isActive;
            this.adType = adType;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AdsDTO(int adId, string adName, string adImageFileUser, string adImageFileSystem, string adText, bool isActive, string adType, string guid, int siteId, bool synchStatus, int masterEntityId,
                           string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate)
            : this(adId, adName, adImageFileUser, adImageFileSystem, adText, isActive, adType)
        {
            log.LogMethodEntry(adId, adName, adImageFileUser, adImageFileSystem, adText, isActive, adType, siteId,
                               guid, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy,
                               lastUpdatedDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the AdId field
        /// </summary>

        public int AdId { get { return adId; } set { adId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AdName field
        /// </summary>

        public string AdName { get { return adName; } set { adName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AdImageFileUser field
        /// </summary>

        public string AdImageFileUser { get { return adImageFileUser; } set { adImageFileUser = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Image field
        /// </summary>

        public byte[] Image { get { return img; } set { img = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AdImageFileSystem field
        /// </summary>

        public string AdImageFileSystem { get { return adImageFileSystem; } set { adImageFileSystem = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AdText field
        /// </summary>

        public string AdText { get { return adText; } set { adText = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AdType field
        /// </summary>

        public string AdType { get { return adType; } set { adType = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>

        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>

        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>

        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>

        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>

        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>

        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>

        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>

        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>

        [Browsable(false)]
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        public string Base64ImageString { get { return base64ImageString; } set { base64ImageString = value; } }
        /// <summary>
        /// Get/Set method of the AdBroadcastDTOList field
        /// </summary>

        public List<AdBroadcastDTO> AdBroadcastDTOList { get { return adBroadcastDTOList; } set { adBroadcastDTOList = value; } }

        /// <summary>
        /// Returns whether the AdsDTO changed or any of its AdBroadcastDTO DTO  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (adBroadcastDTOList != null &&
                  adBroadcastDTOList.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || adId < 0;
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
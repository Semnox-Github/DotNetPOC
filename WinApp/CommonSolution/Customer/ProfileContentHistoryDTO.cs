/********************************************************************************************
 * Project Name - ProfileContentHistory DTO
 * Description  - Data object of ProfileContentHistory
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        02-Feb-2017   Lakshminarayana          Created 
 *2.70.2        22-Jul-2019   Girish Kundar            Modified : Added Constructor with required Parameter
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
	public class ProfileContentHistoryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID ,

			/// <summary>
			/// Search by PROFILE ID field
			/// </summary>
			PROFILE_ID ,

            /// <summary>
            /// Search by RICH CONTENT ID field
            /// </summary>
            RICH_CONTENT_ID,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by PROFILE ID LIST field
            /// </summary>
            PROFILE_ID_LIST,
            /// <summary>
            /// Search by MASTER ENTITY OD field
            /// </summary>
            MASTER_ENTITY_ID
        }

      private int id;
      private int profileId;
      private int richContentId;
      private DateTime contentSignedDate;
      private bool isActive;
      private DateTime creationDate;
      private string createdBy;
      private string lastUpdatedBy;
      private DateTime lastUpdateDate;
      private int siteId;
      private bool synchStatus;
      private string guid;
      private int masterEntityId;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProfileContentHistoryDTO()
        {
            log.LogMethodEntry();
            id = -1;
            profileId = -1;
            richContentId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with required  parameters
        /// </summary>
        public ProfileContentHistoryDTO(int id, int profileId, int richContentId, DateTime contentSignedDate, bool isActive)
            :this()
        {
            log.LogMethodEntry(id, profileId, richContentId, contentSignedDate, isActive);
            this.id = id;
            this.profileId = profileId;
            this.richContentId = richContentId;
            this.contentSignedDate = contentSignedDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// constructor with all the parameters
        /// </summary>
        public ProfileContentHistoryDTO(int id, int profileId, int richContentId, DateTime contentSignedDate,bool isActive,
            string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
            int siteId, bool synchStatus, string guid, int masterEntityId)
            :this(id, profileId, richContentId, contentSignedDate, isActive)
        {
            log.LogMethodEntry(id, profileId, richContentId, contentSignedDate, isActive, createdBy,
                creationDate, lastUpdatedBy, lastUpdateDate, siteId, synchStatus, guid, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }


		/// <summary>
		/// Get/Set method of the ProfileId field
		/// </summary>
		[DisplayName("ProfileId")]
        public int ProfileId { get { return profileId; } set { profileId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RichContentId field
        /// </summary>
        [DisplayName("RichContentId")]
        public int RichContentId { get { return richContentId; } set { richContentId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ContentSignedDate field
        /// </summary>
        [DisplayName("ContentSignedDate")]
        public DateTime ContentSignedDate { get { return contentSignedDate; } set { contentSignedDate = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }


        /// <summary>
        ///  Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdateDate; } set { lastUpdateDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        ///  Get/Set method of the Guid field
        /// </summary>
		[Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

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

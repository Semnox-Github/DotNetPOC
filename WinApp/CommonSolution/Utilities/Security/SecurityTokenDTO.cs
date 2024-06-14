/********************************************************************************************
 * Project Name - Security
 * Description  - SecurityTokenDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.60        22-Mar-2019   Jagan Mohan            Modify: Removed index enumerations and added log method entry and method exit
 *2.80        15-Mar-2020   Nitin Pai              Added MachineId in SecurityTokenDTO
 *2.110       09-Feb-2021   Girish Kundar          Modified: Added SessionId in SecurityTokenDTO
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.Utilities
{
    public class SecurityTokenDTO
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
			/// Search by TOKENID field
			/// </summary>
			TOKENID,

			/// <summary>
			/// Search by TOKEN field
			/// </summary>
			TOKEN,

			/// <summary>
			/// Search by OBJECT field
			/// </summary>
			OBJECT,

			/// <summary>
			/// Search by OBJECT_GUID field
			/// </summary>
			OBJECT_GUID,

            /// <summary>
            /// Search by START_TIME field
            /// </summary>
            START_TIME,

            /// <summary>
            /// Search by EXPIRY_TIME field
            /// </summary>
            EXPIRY_TIME,

			/// <summary>
			/// Search by INVALID_ATTEMPTS field
			/// </summary>
			INVALID_ATTEMPTS,

			/// Search by IsActive field
			/// </summary>
			ACTIVE_FLAG,

			/// <summary>
			/// Search by SITE_ID field
			/// </summary>
			SITE_ID,

            /// <summary>
            /// Search by IS_EXPIRED field
            /// </summary>
            IS_EXPIRED,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by USER_SESSION_ID field
            /// </summary>
            USER_SESSION_ID
        }

      private  int tokenId;
      private  string token;
      private  string tableObject;
      private  string objectGuid;
      private  DateTime startTime;
      private  DateTime expiryTime;
      private  DateTime lastActivityTime;
      private  int invalidAttempts;
	  private  string isActive;
      private  string createdBy;
      private  DateTime creationDate;
      private  string lastUpdatedBy;
      private  DateTime lastUpdatedDate;
      private  int site_id;
      private  int masterEntityId;
      private  bool synchStatus;
      private  string guid;
      private  string loginId;
      private  int languageId;
      private  int roleId;
      private  int machineId;
      private  string sessionId;
                
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SecurityTokenDTO()
        {
            log.LogMethodEntry();
			tokenId = -1;
            token = "";
            objectGuid = "";
            site_id = -1;
            masterEntityId = -1;
            roleId = -1;
            TokenValidated = false;
			TokenValidationMessage = "";
            log.LogMethodExit();
        }


        public SecurityTokenDTO(int tokenId, string token, string tableObject, string objectGuid, DateTime startTime,
                              DateTime expiryTime, DateTime lastActivityTime, int invalidAttempts, string isActive,string sessionId )
        {
            log.LogMethodEntry(tokenId, token, tableObject, objectGuid, startTime, expiryTime, lastActivityTime, invalidAttempts, isActive, sessionId);
            this.tokenId = tokenId;
            this.token = token;
            this.tableObject = tableObject;
            this.objectGuid = objectGuid;
            this.startTime = startTime;
            this.expiryTime = expiryTime;
            this.lastActivityTime = lastActivityTime;
            this.invalidAttempts = invalidAttempts;
            this.isActive = isActive;
            this.sessionId = sessionId;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameters
        /// </summary>

        public SecurityTokenDTO(int tokenId, string token, string tableObject, string objectGuid, DateTime startTime,
                                DateTime expiryTime, DateTime lastActivityTime, int invalidAttempts, string isActive, string createdBy,
                                DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, int site_id, 
                                int masterEntityId, bool synchStatus, string guid, string loginid = "",string sessionId=null)
            :this(tokenId, token, tableObject, objectGuid, startTime, expiryTime, lastActivityTime, invalidAttempts, isActive, sessionId)
        {
            log.LogMethodEntry(tokenId, token, tableObject, objectGuid, startTime, expiryTime, lastActivityTime, invalidAttempts, isActive, createdBy,
               creationDate, lastUpdatedBy, lastUpdatedDate, site_id, masterEntityId, synchStatus, guid, loginid, sessionId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.site_id = site_id;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.loginId = loginid;
            log.LogMethodExit();
        }


		/// <summary>
		/// Get/Set method of the TokenId field
		/// </summary>
		[DisplayName("TokenId")]
		public int TokenId { get { return tokenId; } set { tokenId = value; this.IsChanged = true; } }

		/// <summary>
		/// Get/Set method of the Token field
		/// </summary>
		[DisplayName("Token")]
        public string Token { get { return token; } set { token = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Object field
        /// </summary>
        [DisplayName("TableObject")]
        public string TableObject { get { return tableObject; } set { tableObject = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ObjectGuid field
        /// </summary>
        [DisplayName("ObjectGuid")]
        public string ObjectGuid { get { return objectGuid; } set { objectGuid = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the StartTime field
        /// </summary>
        [DisplayName("StartTime")]
        public DateTime StartTime { get { return startTime; } set { startTime = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ExpiryTime field
        /// </summary>
        [DisplayName("ExpiryTime")]
        public DateTime ExpiryTime { get { return expiryTime; } set { expiryTime = value; this.IsChanged = true; } }


		/// <summary>
		///  Get/Set method of the LastActivityTime field
		/// </summary>
		[DisplayName("LastActivityTime")]
        public DateTime LastActivityTime { get { return lastActivityTime; } set { lastActivityTime = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the InvalidAttempts field
        /// </summary>
        [DisplayName("InvalidAttempts")]
        public int InvalidAttempts { get { return invalidAttempts; } set { invalidAttempts = value; this.IsChanged = true; } }


        /// <summary>
        ///  Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public string IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }


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
		public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }


		/// <summary>
		/// Get/Set method of the SiteId field
		/// </summary>
		[Browsable(false)]
		public int SiteId { get { return site_id; } set { site_id = value; this.IsChanged = true; } }


		/// <summary>
		/// Get/Set method of the MasterEntityId field
		/// </summary>
		[Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }


        /// <summary>
        ///  Get/Set method of the SynchStatus field
        /// </summary>
		[Browsable(false)]
		public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method of the Guid field
        /// </summary>
		[Browsable(false)]
		public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

		/// <summary>
		///  Get/Set method of the TokenValidated field
		/// </summary>
		[Browsable(false)]
		public bool TokenValidated { get ;  set ; }

		/// <summary>
		///  Get/Set method of the TokenValidationMessage field
		/// </summary>
		[Browsable(false)]
		public string TokenValidationMessage { get; set; }


        /// <summary>
        /// Get/Set method of the InvalidAttempts field
        /// Manoj - 02/Oct/2018 - Login ID will retrieved from the token and passed via this object
        /// </summary>
        [DisplayName("LoginId")]
        public string LoginId { get { return loginId; } set { loginId = value; } }

        /// <summary>
        /// Get/Set method of the InvalidAttempts field
        /// Manoj - 02/Oct/2018 - Language Id will retrieved from the token and passed via this object
        /// </summary>
        [DisplayName("LanguageId")]
        public int LanguageId { get { return languageId; } set { languageId = value; } }

        /// <summary>
        /// Get/Set method
        /// Jagan - 26/Feb/2019 - Role Id will retrieved from the token and passed via this object
        /// </summary>
        [DisplayName("RoleId")]
        public int RoleId { get { return roleId; } set { roleId = value; } }

        /// <summary>
        /// Get/Set method of the MachineId field
        /// Manoj - 02/Oct/2018 - MachineId Id will retrieved from the token and passed via this object
        /// </summary>
        [DisplayName("MachineId")]
        public int MachineId { get { return machineId; } set { machineId = value; } }

        public string UserSessionId { get { return sessionId; } set { sessionId = value; this.IsChanged = true; } }


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
                    return notifyingObjectIsChanged || tokenId < 0;
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
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }
    }
}

/********************************************************************************************
 * Project Name - Utilities
 * Description  - Data object of ApplicationRequestLogDetail
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.120.10    05-Jul-2021   Abhishek                Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// This is the  Application Request Log Detail data object class. This acts as data holder for the   Application Request Log Details business object
    /// </summary>
    public class ApplicationRequestLogDetailDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by  APPLICATION_REQUEST_LOG_ID field 
            /// </summary>
            APPLICATION_REQUEST_LOG_ID,
            /// <summary>
            /// Search by  ENTITY_GUID field 
            /// </summary>
            ENTITY_GUID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by ID_LIST field
            /// </summary>
            ID_LIST
        }

        private int id;
        private int applicationRequestLogId;
        private string entityGuid;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationRequestLogDetailDTO()
        {
            log.LogMethodEntry();
            id = -1;
            applicationRequestLogId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with required data fields.
        /// </summary>
        public ApplicationRequestLogDetailDTO(int id, int applicationRequestLogId, string entityGuid, bool isActive)
            : this()
        {
            log.LogMethodEntry(id, applicationRequestLogId, entityGuid, isActive);
            this.id = id;
            this.applicationRequestLogId = applicationRequestLogId;
            this.entityGuid = entityGuid;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with all data fields
        /// </summary>
        public ApplicationRequestLogDetailDTO(int id, int applicationRequestLogId, string entityGuid, 
                                        bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                        DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus, int masterEntityId)
            : this(id, applicationRequestLogId, entityGuid, isActive)
        {
            log.LogMethodEntry(id, applicationRequestLogId, entityGuid, isActive, createdBy, creationDate, lastUpdatedBy,
                               lastUpdatedDate, siteId, guid, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with all data fields
        /// </summary>
        public ApplicationRequestLogDetailDTO(ApplicationRequestLogDetailDTO applicationRequestLogDetailDTO)
            : this(applicationRequestLogDetailDTO.id, applicationRequestLogDetailDTO.applicationRequestLogId, applicationRequestLogDetailDTO.entityGuid,
                  applicationRequestLogDetailDTO.isActive, applicationRequestLogDetailDTO.createdBy, applicationRequestLogDetailDTO.creationDate,
                  applicationRequestLogDetailDTO.lastUpdatedBy, applicationRequestLogDetailDTO.lastUpdatedDate, applicationRequestLogDetailDTO.siteId,
                  applicationRequestLogDetailDTO.guid, applicationRequestLogDetailDTO.synchStatus, applicationRequestLogDetailDTO.masterEntityId)
        {
            log.LogMethodEntry(applicationRequestLogDetailDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { this.IsChanged = true; id = value; } }

        /// <summary>
        /// Get/Set method of the ApplicationRequestLogId field
        /// </summary>
        public int ApplicationRequestLogId { get { return applicationRequestLogId; } set { this.IsChanged = true; applicationRequestLogId = value; } }

        /// <summary>
        /// Get/Set method of the EntityGuid field
        /// </summary>
        public string EntityGuid { get { return entityGuid; } set { this.IsChanged = true; entityGuid = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || Id < 0;
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

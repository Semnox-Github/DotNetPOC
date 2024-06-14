﻿/********************************************************************************************
 * Project Name - Utilities
 * Description  - Data object of ApplicationRequestLog
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
    /// This is the  Application Request Log data object class. This acts as data holder for the   Application Request Logs business object
    /// </summary>
    public class ApplicationRequestLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by  REQUEST_GUID field 
            /// </summary>
            REQUEST_GUID,
            /// <summary>
            /// Search by  MODULE field 
            /// </summary>
            MODULE,
            /// <summary>
            /// Search by  USECASE field 
            /// </summary>
            USECASE,
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
        private string requestGuid;
        private string module;
        private string usecase;
        private DateTime timestamp;
        private string loginId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private List<ApplicationRequestLogDetailDTO> applicationRequestLogDetailDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationRequestLogDTO()
        {
            log.LogMethodEntry();
            id = -1;
            module = string.Empty;
            usecase = string.Empty;
            loginId = string.Empty;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            applicationRequestLogDetailDTOList = new List<ApplicationRequestLogDetailDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with required data fields.
        /// </summary>
        public ApplicationRequestLogDTO(int id, string requestGuid, string module, string usecase, DateTime timestamp, string loginId,
                                        bool isActive)
            : this()
        {
            log.LogMethodEntry(id, requestGuid, module, usecase, timestamp, loginId, isActive);
            this.id = id;
            this.requestGuid = requestGuid;
            this.module = module;
            this.usecase = usecase;
            this.timestamp = timestamp;
            this.loginId = loginId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with all data fields
        /// </summary>
        public ApplicationRequestLogDTO(int id, string requestGuid, string module, string usecase, DateTime timestamp, string loginId,
                                        bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                        DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus, int masterEntityId)
            : this(id, requestGuid, module, usecase, timestamp, loginId, isActive)
        {
            log.LogMethodEntry(id, requestGuid, module, usecase, timestamp, loginId, isActive, createdBy, creationDate, lastUpdatedBy,
                               lastUpdatedDate, siteId, guid, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.applicationRequestLogDetailDTOList = new List<ApplicationRequestLogDetailDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with all data fields
        /// </summary>
        public ApplicationRequestLogDTO(ApplicationRequestLogDTO applicationRequestLogDTO)
            : this(applicationRequestLogDTO.id, applicationRequestLogDTO.requestGuid, applicationRequestLogDTO.module, 
                  applicationRequestLogDTO.usecase, applicationRequestLogDTO.timestamp, applicationRequestLogDTO.loginId, 
                  applicationRequestLogDTO.isActive, applicationRequestLogDTO.createdBy, applicationRequestLogDTO.creationDate, 
                  applicationRequestLogDTO.lastUpdatedBy, applicationRequestLogDTO.lastUpdatedDate, applicationRequestLogDTO.siteId, 
                  applicationRequestLogDTO.guid, applicationRequestLogDTO.synchStatus, applicationRequestLogDTO.masterEntityId)
        {
            log.LogMethodEntry(applicationRequestLogDTO);
            if(applicationRequestLogDTO.applicationRequestLogDetailDTOList != null)
            {
                foreach (var applicationRequestLogDetailDTO in applicationRequestLogDTO.applicationRequestLogDetailDTOList)
                {
                    applicationRequestLogDetailDTOList.Add(new ApplicationRequestLogDetailDTO(applicationRequestLogDetailDTO));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { this.IsChanged = true; id = value; } }

        /// <summary>
        /// Get/Set method of the RequestGuid field
        /// </summary>
        public string RequestGuid { get { return requestGuid; } set { this.IsChanged = true; requestGuid = value; } }

        /// <summary>
        /// Get/Set method of the Module field
        /// </summary>
        public string Module { get { return module; } set { this.IsChanged = true; module = value; } }

        /// <summary>
        /// Get/Set method of the Usecase field
        /// </summary>
        public string Usecase { get { return usecase; } set { this.IsChanged = true; usecase = value; } }

        /// <summary>
        /// Get/Set method of the Timestamp field
        /// </summary>
        public DateTime Timestamp { get { return timestamp; } set { this.IsChanged = true; timestamp = value; } }

        /// <summary>
        /// Get/Set method of the LoginId field
        /// </summary>
        public string LoginId { get { return loginId; } set { this.IsChanged = true; loginId = value; } }

        /// <summary>
        /// Get/Set method of the ApplicationRequestLogDetailDTOList field
        /// </summary>
        public List<ApplicationRequestLogDetailDTO> ApplicationRequestLogDetailDTOList
        {
            get { return applicationRequestLogDetailDTOList; }
            set { applicationRequestLogDetailDTOList = value; }
        }

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
        /// Returns whether product or any child record is changed
        /// </summary> 
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (applicationRequestLogDetailDTOList != null &&
                    applicationRequestLogDetailDTOList.Any(x => x.IsChanged))
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

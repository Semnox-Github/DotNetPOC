/********************************************************************************************
 * Project Name - Concurrent Programs Parameter In List Values DTO
 * Description  - DTO class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.120.1       26-Apr-2021   Deeksha             Created as part of AWS Concurrent Programs enhancements
 ********************************************************************************************/
using System;


namespace Semnox.Parafait.JobUtils
{
    public class ProgramParameterInListValuesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by Program Parameter Value Id
            /// </summary>
            PROGRAM_PARAMETER_VALUE_ID,

            /// <summary>
            /// Search by Program Parameter In List Value Id
            /// </summary>
            PROGRAM_PARAMETER_IN_LIST_VALUE_ID,
            
            /// <summary>
            /// Search by is active
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int programParameterInListValueId;
        private int programParameterValueId;
        private string inListValue;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <returns></returns>
        public ProgramParameterInListValuesDTO()
        {
            log.LogMethodEntry();
            programParameterValueId = -1;
            programParameterInListValueId = -1;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameter
        /// </summary>
        public ProgramParameterInListValuesDTO(int programParameterInListValueId, int programParameterValueId, string inListValue, 
                                                bool isActive)
            : this()
        {
            log.LogMethodEntry(programParameterInListValueId, programParameterValueId, inListValue, isActive);
            this.programParameterInListValueId = programParameterInListValueId;
            this.programParameterValueId = programParameterValueId;
            this.inListValue = inListValue;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProgramParameterInListValuesDTO(int programParameterInListValueId, int programParameterValueId, string inListValue,
                                                bool isActive, string createdBy, DateTime creationDate,
                                               string lastUpdatedBy, DateTime lastUpdatedDate, string guid, bool synchStatus,
                                               int siteId, int masterEntityId)
            : this(programParameterInListValueId, programParameterValueId, inListValue, isActive)
        {
            log.LogMethodEntry(programParameterInListValueId, programParameterValueId, inListValue, isActive, createdBy, creationDate,
                               lastUpdatedBy, lastUpdatedDate, guid, siteId, masterEntityId, synchStatus);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ProgramParameterInListValueId field
        /// </summary>
        public int ProgramParameterInListValueId { get { return programParameterInListValueId; } set { this.IsChanged = true; programParameterInListValueId = value; } }


        /// <summary>
        /// Get/Set method of the ProgramId field
        /// </summary>
        public int ProgramParameterValueId { get { return programParameterValueId; } set { this.IsChanged = true; programParameterValueId = value; } }

        /// <summary>
        /// Get/Set method of the ParameterName field
        /// </summary>
        public string InListValue { get { return inListValue; } set { this.IsChanged = true; inListValue = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

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
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || programParameterInListValueId < 0;
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
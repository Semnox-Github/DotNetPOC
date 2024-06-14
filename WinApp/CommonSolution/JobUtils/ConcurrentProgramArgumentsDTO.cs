/********************************************************************************************
 * Project Name - Concurrent Program Arguments DTO
 * Description  - Data object of the concurrent Programs Arguments
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        7-Mar-2016    Amaresh          Created 
 *2.70.2        24-Jul-2019   Dakshakh raj     Modified : Added Parameterized costrustor,
 *                                                      who columns
 *2.140       14-Sep-2021      Fiona          Modified: Added ARGUMENT_ID in Search Parameters
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the Concurrent Programs Arguments data object class. This acts as data holder for the Concurrent Program Argument business object
    /// </summary>

    public class ConcurrentProgramArgumentsDTO : IChangeTracking
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByProgramsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByProgramArgumentsParameters
        {
            /// <summary>
            /// Search by ProgramId field
            /// </summary>
            ARGUMENT_ID,
            /// <summary>
            /// Search by ProgramId field
            /// </summary>
            PROGRAM_ID,
            /// <summary>
            /// Search by ProgramId field
            /// </summary>
            PROGRAM_ID_LIST,

            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int argumentId;
        private int programId;
        private string argumentValue;
        private string argumentType;
        private bool isActive;
        private string guid;
        private bool synchStatus;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int masterEntityId;
        private int siteId;
        private string createdBy;
        private DateTime creationDate;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

         /// <summary>
        /// Default constructor
        /// </summary>
        public ConcurrentProgramArgumentsDTO()
        {
            log.LogMethodEntry();
            argumentId = -1;
            programId = -1;
            argumentType = "S";
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

         /// <summary>
        /// Constructor with required fields
        /// </summary>
        public ConcurrentProgramArgumentsDTO(int argumentId, int programId, string argumentValue, string argumentType, bool isActive)
            :this()
        {
            log.LogMethodEntry(argumentId, programId, argumentValue, argumentType, isActive);
            this.argumentId = argumentId;
            this.programId = programId;
            this.argumentValue = argumentValue;
            this.argumentType = argumentType;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ConcurrentProgramArgumentsDTO(int argumentId, int programId, string argumentValue, string argumentType, bool isActive,
                                             int siteId, string guid, bool synchStatus, DateTime lastUpdateDate, string lastUpdatedBy,
                                             int masterEntityId, string createdBy, DateTime creationDate)
            :this(argumentId, programId, argumentValue, argumentType, isActive)
        {
            log.LogMethodEntry(siteId, guid, synchStatus, lastUpdateDate, lastUpdatedBy,
                               masterEntityId,  createdBy,  creationDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ProgramId field
        /// </summary>
        [Browsable(false)]
        [DisplayName("Program Id")]
        public int ProgramId { get { return programId; } set { programId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ArgumentId field
        /// </summary>
        [DisplayName("Argument Id")]
        public int ArgumentId { get { return argumentId; } set { argumentId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ArgumentValue field
        /// </summary>
        [DisplayName("Argument Value")]
        public string ArgumentValue { get { return argumentValue; } set { argumentValue = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ArgumentType field
        /// </summary>
        [DisplayName("Argument Type")]
        public string ArgumentType { get { return argumentType; } set { argumentType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("Last Updated User")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; IsChanged = true; } }

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
                    return notifyingObjectIsChanged || argumentId < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}

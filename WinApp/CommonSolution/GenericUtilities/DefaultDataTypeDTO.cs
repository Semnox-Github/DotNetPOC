/********************************************************************************************
 * Project Name - DefaultDataType DTO
 * Description  - Data object of DefaultDataType
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        03-May-2019   Mushahid Faizan          Created 
 *2.70.2        26-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor                                            
 ********************************************************************************************/
using System;
using System.ComponentModel;


namespace Semnox.Core.GenericUtilities
{
    public class DefaultDataTypeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Id field
            /// </summary>
            DATATYPE_ID,
            /// <summary>
            /// Search by  data type field
            /// </summary>
            DATA_TYPE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int datatypeId;
        private string datatype;
        private string datavalues;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        /// <summary>
        /// Default constructor
        /// </summary>
        public DefaultDataTypeDTO()
        {
            log.LogMethodEntry();
            datatypeId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public DefaultDataTypeDTO(int datatypeId, string datatype, string datavalues)
            :this()
        {
            log.LogMethodEntry(datatypeId, datatype, datavalues);

            this.datatypeId = datatypeId;
            this.datatype = datatype;
            this.datavalues = datavalues;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DefaultDataTypeDTO(int datatypeId, string datatype, string datavalues, string createdBy, DateTime creationDate,
                                  string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid)
            :this(datatypeId, datatype, datavalues)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Datatype_id field
        /// </summary>
        [DisplayName("Datatype Id")]
        public int DatatypeId
        {
            get { return datatypeId; }
            set { this.IsChanged = true; datatypeId = value; }
        }
        /// <summary>
        /// Get/Set method of the Datatype field
        /// </summary>
        [DisplayName("Datatype")]
        public string Datatype
        {
            get { return datatype; }
            set { this.IsChanged = true; datatype = value; }
        }

        /// <summary>
        /// Get/Set method of the Datavalues field
        /// </summary>
        [DisplayName("Datavalues")]
        public string Datavalues
        {
            get { return datavalues; }
            set { this.IsChanged = true; datavalues = value; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set { creationDate = value; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set { lastUpdateDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }

            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                this.IsChanged = true;
                guid = value;
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
                    return notifyingObjectIsChanged || datatypeId < 0;
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

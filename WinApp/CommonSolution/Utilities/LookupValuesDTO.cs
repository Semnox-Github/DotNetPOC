/********************************************************************************************
 * Project Name - Lookup Values DTO
 * Description  - Data object of Lookup Values
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        25-Jan-2016   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 *2.70        03-Jul-2019   Girish Kundar       Modified : Added constructor and Missing Who columns. 
 *            29-Jul-2019   Mushahid Faizan     Added isActive to delete the record.
 ********************************************************************************************/

using System;
using System.ComponentModel;
namespace Semnox.Core.Utilities
{
    /// <summary>
    ///  This is the lookup value data object class. This acts as data holder for the lookup value business object
    /// </summary>   
    public class LookupValuesDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByLookupValuesParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByLookupValuesParameters
        {
            /// <summary>
            /// Search by LOOKUP VALUE ID field
            /// </summary>
            LOOKUP_VALUE_ID,
            /// <summary>
            /// Search by LOOKUP VALUE ID LIST field
            /// </summary>
            LOOKUP_ID_LIST,
            /// <summary>
            /// Search by LOOKUP ID field
            /// </summary>
            LOOKUP_ID,
            /// <summary>
            /// Search by LOOKUP VALUE field
            /// </summary>
            LOOKUP_VALUE,
            /// <summary>
            /// Search by LOOKUP NAME field
            /// </summary>
            LOOKUP_NAME,
            /// <summary>
            /// Search by DESCRIPTION field
            /// </summary>
            DESCRIPTION,
            /// <summary>//starts:Modification on 18-Jul-2016 for publish feature
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID, //Ends:Modification on 18-Jul-2016 for publish feature
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            IS_ACTIVE
        }

        private int lookupValueId;
        private int lookupId;
        private string lookupName;
        private string lookupValue;
        private string description;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        bool isActive;

        /// <summary>
        /// Default constructor
        /// </summary>
        public LookupValuesDTO()
        {
            log.LogMethodEntry();
            lookupValueId = -1;
            lookupId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required  data fields
        /// </summary>
        public LookupValuesDTO(int lookupValueId, int lookupId, string lookupName, string lookupValue,
                               string description,bool isActive)
            : this()
        {
            log.LogMethodEntry(lookupValueId, lookupId, lookupName, lookupValue, description);
            this.lookupValueId = lookupValueId;
            this.lookupId = lookupId;
            this.lookupName = lookupName;
            this.lookupValue = lookupValue;
            this.description = description;
            this.isActive = isActive;
            log.LogMethodExit();
        }



        /// <summary>
        /// Constructor with All the  data fields
        /// </summary>
        public LookupValuesDTO(int lookupValueId, int lookupId, string lookupName, string lookupValue,
                               string description, string guid, bool synchStatus, int siteId, int masterEntityId,
                               string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,bool isActive )
            : this(lookupValueId, lookupId, lookupName, lookupValue, description, isActive)
        {
            log.LogMethodEntry(lookupValueId, lookupId, lookupName, lookupValue,
                                description, guid, synchStatus, siteId, masterEntityId,
                                createdBy, creationDate, lastUpdatedBy, lastUpdateDate);

            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public LookupValuesDTO(int lookupValueId, int lookupId, string lookupName, string lookupValue,
                               string description, string guid, bool synchStatus, int siteId, int masterEntityId)//Modification on 18-Jul-2016 for publish feature
        {
            log.LogMethodEntry();
            this.lookupValueId = lookupValueId;
            this.lookupId = lookupId;
            this.lookupName = lookupName;
            this.lookupValue = lookupValue;
            this.description = description;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;//Modification on 18-Jul-2016 for publish feature
            this.isActive = true;
            log.LogMethodExit();
        }
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="lookupValuesDTO"></param>
        public LookupValuesDTO(LookupValuesDTO lookupValuesDTO):
            this(lookupValuesDTO.lookupValueId,
                lookupValuesDTO.lookupId,
                lookupValuesDTO.lookupName,
                lookupValuesDTO.lookupValue,
                lookupValuesDTO.description,
                lookupValuesDTO.guid,
                lookupValuesDTO.synchStatus,
                lookupValuesDTO.siteId,
                lookupValuesDTO.masterEntityId)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LookupValueId field
        /// </summary>
        [DisplayName("Lookup Value Id")]
        [ReadOnly(true)]
        public int LookupValueId { get { return lookupValueId; } set { lookupValueId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LookupId field
        /// </summary>
        [DisplayName("Lookup Id")]
        public int LookupId { get { return lookupId; } set { lookupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LookupName field
        /// </summary>
        [DisplayName("Lookup Name")]
        public string LookupName { get { return lookupName; } set { lookupName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LookupValue field
        /// </summary>
        [DisplayName("Lookup Value")]
        public string LookupValue { get { return lookupValue; } set { lookupValue = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>//starts:Modification on 18-Jul-2016 for publish feature
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }//Ends:Modification on 18-Jul-2016 for publish feature

        [Browsable(false)]
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
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
                    return notifyingObjectIsChanged || lookupValueId < 0;
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

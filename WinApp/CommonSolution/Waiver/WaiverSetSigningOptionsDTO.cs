/********************************************************************************************
 * Project Name - Waiver
 * Description  - Data object of the WaiverSetSigningOption
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.70        01-Jul -2019   Girish Kundar    Modified : Added Parametrized Constructor with required fields.
 *2.70.2        15-Oct-2019    GUru S A         Waiver phase 2 changes
 *2.70.2      06-Feb-2020      Divya A          Changes for WMS 
 ********************************************************************************************* */
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Waiver
{
    /// <summary>
    ///  WaiverSetSigningOptionsDTO Class
    /// </summary>
    public class WaiverSetSigningOptionsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int id;
        private int waiverSetId;
        private int lookupValueId;
        private string optionName;
        private string optionDescription;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID
            /// </summary>
            ID ,
            /// <summary>
            /// Search by WAIVER SET ID
            /// </summary>
            WAIVER_SET_ID ,
            /// <summary>
            /// Search by LOOKUP VALUE ID
            /// </summary>
            LOOKUP_VALUE_ID ,
            /// <summary>
            /// Search by SITE ID
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by WAIVERSET_ID_LIST field
            /// </summary>
            WAIVERSET_ID_LIST,
            /// <summary>
            /// Search by WAIVERSET_ID_LIST field
            /// </summary>
            WAIVERSET_SIGNING_OPTIONS_LIST,
        }

        public enum WaiverSigningOptions
        {
            /// <summary>
            ///Sign on device 
            /// </summary>
            DEVICE,
            /// <summary>
            /// Maunal signing
            /// </summary>
            MANUAL,
            /// <summary>
            ///  ONLINE
            /// </summary>
            ONLINE,
        }

        /// <summary>
        /// Default constructor
        /// </summary> 
        public WaiverSetSigningOptionsDTO()
        {
            log.LogMethodEntry();
            this.id = -1;
            this.waiverSetId = -1;
            this.lookupValueId = -1;
            this.createdBy = string.Empty;
            this.creationDate = DateTime.MinValue;
            this.lastUpdatedDate = DateTime.MinValue;
            this.lastUpdatedBy = string.Empty;
            this.site_id = -1;
            this.guid = string.Empty;
            this.synchStatus = false;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        ///  constructor with required data fields.
        /// </summary>
        public WaiverSetSigningOptionsDTO(int id, int waiverSetId, int lookupValueId)
            :this()
        {
            log.LogMethodEntry(id, waiverSetId, lookupValueId);
            this.id = id;
            this.waiverSetId = waiverSetId;
            this.lookupValueId = lookupValueId;
            log.LogMethodExit();
        }
        /// <summary>
        ///  constructor with Parameter
        /// </summary>
        public WaiverSetSigningOptionsDTO(int id, int waiverSetId, int lookupValueId, string createdBy, DateTime creationDate, DateTime lastUpdatedDate,
                                    string lastUpdatedBy, int site_id, string guid, bool synchStatus, int masterEntityId, string optionName, string optionDescription)
            :this(id, waiverSetId, lookupValueId)
        {
            log.LogMethodEntry(id, waiverSetId, lookupValueId, createdBy, creationDate, lastUpdatedDate,lastUpdatedBy,
                               site_id, guid, synchStatus, masterEntityId, optionName, optionDescription);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.optionName = optionName;
            this.optionDescription = optionDescription;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the WaiverSigningOptionId field
        /// </summary>
        [DisplayName("WaiverSigningOptionId")]
        [DefaultValue(-1)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the WaiverSetDetailId field
        /// </summary>
        [DisplayName("WaiverSetId")]
        [DefaultValue(-1)]
        public int WaiverSetId { get { return waiverSetId; } set { waiverSetId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LookupValueId field
        /// </summary>
        [DisplayName("LookupValueId")]
        [DefaultValue(-1)]
        public int LookupValueId { get { return lookupValueId; } set { lookupValueId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the OptionName field
        /// </summary>
        [DisplayName("OptionName")]
        [DefaultValue("")]
        public string OptionName { get { return optionName; } set { optionName = value; } }

        /// <summary>
        /// Get/Set method of the optionDescription field
        /// </summary>
        [DisplayName("optionDescription")]
        [DefaultValue("")]
        public string OptionDescription { get { return optionDescription; } set { optionDescription = value; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [DefaultValue("")]
        public string CreatedBy { get { return createdBy; } }


        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } }


        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        [DefaultValue("")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } }


        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [DefaultValue(-1)]
        public int SiteId { get { return site_id; } set { site_id = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [DefaultValue("")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }


        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [DefaultValue(-1)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
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
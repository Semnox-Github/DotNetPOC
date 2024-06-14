/********************************************************************************************
 * Project Name - Utilities
 * Description  - Data object of ParafaitDefaults
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        3-Jul-2019  Girish Kundar             Modified : Added Constructor with required Parameter
 *                                                             Added Active Flag as Search Parameter 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.Utilities
{
    public class SystemOptionsDTO
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
            /// Search by OPTION ID field
            /// </summary>
            OPTION_ID,
            /// <summary>
            /// Search by OPTION TYPE field
            /// </summary>
            OPTION_TYPE ,
            /// <summary>
            /// Search by OPTION NAME field
            /// </summary>
            OPTION_NAME ,
            /// <summary>
            /// Search by OPTION VALUE field
            /// </summary>
            OPTION_VALUE ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID ,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE
        }

       private int optionId;
       private string optionType;
       private string optionName;
       private string optionValue;
       private int siteId;
       private string guid;
       private bool synchStatus;
       private bool isActive;
       private int masterEntityId;
       private string createdBy;
       private DateTime creationDate;
       private string lastUpdatedBy;
       private DateTime lastUpdateDate;
    
        /// Default constructor
        /// </summary>
        public SystemOptionsDTO()
        {
            log.LogMethodEntry();
            optionId = -1;            
            siteId = -1;
            synchStatus = false;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with required parameters
        /// </summary>
        public SystemOptionsDTO(int optionId, string optionType, string optionName, string optionValue, bool isActive)
            :this()
        {
            log.LogMethodEntry(optionId, optionType, optionName, optionValue, isActive);
            this.optionId = optionId;
            this.optionType = optionType;
            this.optionName = optionName;
            this.optionValue = optionValue;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameters
        /// </summary>
        public SystemOptionsDTO(int optionId, string optionType, string optionName, string optionValue, int siteId, string guid, bool synchStatus, bool isActive, int masterEntityId,
                          string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            :this(optionId, optionType, optionName, optionValue, isActive)
        {
            log.LogMethodEntry(optionId,  optionType,  optionName,  optionValue, siteId,  guid,  synchStatus, isActive,  masterEntityId,
                                createdBy, creationDate, lastUpdatedBy, lastUpdateDate);

            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the OptionId field
        /// </summary>
        [DisplayName("OptionId")]
        public int OptionId { get { return optionId; } set { optionId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the OptionType field
        /// </summary>
        [DisplayName("Option Type")]
        public string OptionType { get { return optionType; } set { optionType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the OptionName field
        /// </summary>
        [DisplayName("Option Name")]
        public string OptionName { get { return optionName; } set { optionName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the OptionValue field
        /// </summary>
        [DisplayName("Option Value")]
        public string OptionValue { get { return optionValue; } set { optionValue = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || optionId < 0 ;
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

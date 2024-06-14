/********************************************************************************************
 * Project Name - Object Translations DTO
 * Description  - Data object of  object translations 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        30-Dec-2016   Raghuveera          Created 
 *2.70.2        26-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor,
 *                                                         CreatedBy and CreationDate field.
 *2.80        03-Mar-2020     Mushahid Faizan   Modified : 3 tier Changes for REST API                                                         
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the object translation data object class. This acts as data holder for the object translation business object
    /// </summary>
    public class ObjectTranslationsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByObjectTranslationsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByObjectTranslationsParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            
            /// <summary>
            /// Search by LANGUAGE ID field
            /// </summary>
            LANGUAGE_ID,
            
            /// <summary>
            /// Search by ELEMENT GUID field
            /// </summary>
            ELEMENT_GUID,
            
            /// <summary>
            /// Search by OBJECT field
            /// </summary>
            OBJECT,
            
            /// <summary>
            /// Search by ELEMENT field
            /// </summary>
            ELEMENT,
            
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by MASTERENTITYID field
            /// </summary>
            MASTERENTITYID
        }

        private int id;
        private int languageId;
        private string elementGuid;
        private string tableObject;
        private string element;
        private string translation;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private bool synchStatus;
        private string guid;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool isActive;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ObjectTranslationsDTO()
        {
            log.LogMethodEntry();
            id = -1;
            languageId = -1;
            masterEntityId = -1;
            siteId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ObjectTranslationsDTO(int id, int languageId, string elementGuid, string tableObject, string element, string translation, bool isActive)
            : this()
        {
            log.LogMethodEntry(id, languageId, elementGuid, tableObject, element, translation);
            this.id = id;
            this.languageId = languageId;
            this.elementGuid = elementGuid;
            this.tableObject = tableObject;
            this.element = element;
            this.translation = translation;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ObjectTranslationsDTO(int id, int languageId, string elementGuid, string tableObject, string element, string translation,
         string lastUpdatedBy, DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, bool isActive)
            : this(id, languageId, elementGuid, tableObject, element, translation, isActive)
        {
            log.LogMethodEntry(lastUpdatedBy, lastUpdatedDate, siteId, guid, synchStatus, masterEntityId, createdBy, creationDate, isActive);
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LanguageId field
        /// </summary>
        [DisplayName("Language")]
        public int LanguageId { get { return languageId; } set { languageId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ElementGuid field
        /// </summary>
        [DisplayName("ElementGuid")]
        public string ElementGuid { get { return elementGuid; } set { elementGuid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TableObject field
        /// </summary>
        [DisplayName("Table")]
        public string TableObject { get { return tableObject; } set { tableObject = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Element field
        /// </summary>
        [DisplayName("Element")]
        public string Element { get { return element; } set { element = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Translation field
        /// </summary>
        [DisplayName("Translation")]
        public string Translation { get { return translation; } set { translation = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        [Browsable(false)]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit(null);
        }

    }
}

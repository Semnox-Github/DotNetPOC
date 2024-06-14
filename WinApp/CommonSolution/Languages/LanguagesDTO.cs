/********************************************************************************************
 * Project Name - LanguagesDTO
 * Description  - LanguagesDTO object of user
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        24-June-2016   Rakshith          Created 
 *2.70.2        29-Jul -2019   Girish Kundar    Modified :  Added Who columns and parameterized with required Constructor
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Languages
{
    /// <summary>
    /// LanguagesDTO Class
    /// </summary> 
    public class LanguagesDTO
    {
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private int languageId;
        private string languageName;
        private string languageCode;
        private string fontName;
        private int fontSize;
        private int readerLanguageNo;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private string remarks;
        private string cultureCode;
        private bool active;
        private int masterEntityId;
        private string createdBy;
        private string lastUpdatedBy;
        private DateTime creationDate;
        private DateTime lastUpdateDate;

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by LANGUAGE_NAME field
            /// </summary>
            LANGUAGE_NAME,
            /// <summary>
            /// Search by LANGUAGE_CODE field
            /// </summary>
            LANGUAGE_CODE,
            /// <summary>
            /// Search by LANGUAGE_ID field
            /// </summary>
            LANGUAGE_ID,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by READER_LANGUAGE_NO field
            /// </summary>
            READER_LANGUAGE_NO,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }


        /// <summary>
        /// Default Constructor
        /// </summary>
        public LanguagesDTO()
        {
            log.LogMethodEntry();
            this.languageId = -1;
            this.languageName = string.Empty;
            this.languageCode = string.Empty;
            this.siteId = -1;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with required Parameters
        /// </summary>
        public LanguagesDTO(int languageId, string languageName, string languageCode, int readerLanguageNo,
                             string remarks, string cultureCode, bool active)
            : this()
        {
            log.LogMethodEntry(languageId, languageName, languageCode, readerLanguageNo,
                               remarks, cultureCode, active);
            this.languageId = languageId;
            this.languageName = languageName;
            this.languageCode = languageCode;
            this.readerLanguageNo = readerLanguageNo;            
            this.remarks = remarks;
            this.cultureCode = cultureCode;
            this.active = active;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with All the fields
        /// </summary>
        public LanguagesDTO(int languageId, string languageName, string languageCode,string fontName, int fontSize,  int readerLanguageNo,
                            int siteId, bool synchStatus, string remarks, string cultureCode, bool active, string guid,
                            int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy,
                            DateTime lastUpdateDate)
            : this(languageId, languageName, languageCode, readerLanguageNo, remarks, cultureCode, active)
        {
            log.LogMethodEntry(languageId, languageName, languageCode, readerLanguageNo,
                             siteId, synchStatus, remarks, cultureCode, active, guid,
                             masterEntityId, createdBy, creationDate, lastUpdatedBy,
                             lastUpdateDate);
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.fontSize = fontSize;
            this.fontName = fontName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LanguageId field
        /// </summary>
        [DisplayName("LanguageId")]
        public int LanguageId { get { return languageId; } set { languageId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LanguageName field
        /// </summary>
        [DisplayName("LanguageName")]
        public string LanguageName { get { return languageName; } set { languageName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LanguageCode field
        /// </summary>
        [DisplayName("LanguageCode")]
        public string LanguageCode { get { return languageCode; } set { languageCode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FontName field
        /// </summary>
        [DisplayName("FontName")]
        public string FontName { get { return fontName; } set { fontName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FontSize field
        /// </summary>
        [DisplayName("FontSize")]
        public int FontSize { get { return fontSize; } set { fontSize = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReaderLanguageNo field
        /// </summary>
        [DisplayName("ReaderLanguageNo")]
        public int ReaderLanguageNo { get { return readerLanguageNo; } set { readerLanguageNo = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        public int Site_id { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the CultureCode field
        /// </summary>
        [DisplayName("CultureCode")]
        public string CultureCode { get { return cultureCode; } set { cultureCode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CultureCode field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

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
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
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
                    return notifyingObjectIsChanged || languageId < 0;
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

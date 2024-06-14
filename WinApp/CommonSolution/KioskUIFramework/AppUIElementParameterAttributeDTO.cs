/********************************************************************************************
* Project Name - KioskUIFramework
* Description  - Data Object
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.70        15-May-2019   Girish Kundar           Created 
*2.80      04-Jun-2020   Mushahid Faizan          Modified : 3 Tier Changes for Rest API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// This is the AppUIElementParameterAttribute data object class. This acts as data holder for the AppUIElementParameterAttribute business object
    /// </summary>
    public class AppUIElementParameterAttributeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  UI_PARAMETER_ATTRIBUTE_ID field
            /// </summary>
            UI_PARAMETER_ATTRIBUTE_ID,
            /// <summary>
            /// Search by   PARAMETER_ID, field
            /// </summary>
            PARAMETER_ID,
            /// <summary>
            /// Search by   PARAMETER_ID LIST, field
            /// </summary>
            PARAMETER_ID_LIST,
            /// <summary>
            /// Search by   LANGUAGE_ID field
            /// </summary>
            LANGUAGE_ID,
            /// <summary>
            /// Search by   DISPLAY_TEXT_1 field
            /// </summary>
            DISPLAY_TEXT_1,
            /// <summary>
            /// Search by   DISPLAY_TEXT_2 field
            /// </summary>
            DISPLAY_TEXT_2,
            /// <summary>
            /// Search by   FILE_NAME field
            /// </summary>
            FILE_NAME,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by siteId field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int uiParameterAttributeId;
        private int parameterId;
        private string displayText1;
        private string displayText2;
        private int languageId;
        private bool synchStatus;
        private int masterEntityId;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private byte[] displayImage;
        private string fileName;
        private string createdBy;
        private DateTime creationDate;
        private bool activeFlag;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public AppUIElementParameterAttributeDTO()
        {
            log.LogMethodEntry();
            uiParameterAttributeId = -1;
            parameterId = -1;
            languageId = -1;
            siteId = -1;
            masterEntityId = -1;
            activeFlag = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AppUIElementParameterAttributeDTO(int uiParameterAttributeId, int parameterId, string displayText1, string displayText2, int languageId,
               byte[] displayImage,string fileName,bool activeFlag)
            :this()
        {
            log.LogMethodEntry(uiParameterAttributeId, parameterId, displayText1, displayText2, languageId, displayImage, fileName, activeFlag);
            this.uiParameterAttributeId = uiParameterAttributeId;
            this.parameterId = parameterId;
            this.displayText1 = displayText1;
            this.displayText2 = displayText2;
            this.languageId = languageId;
            this.displayImage = displayImage;
            this.fileName = fileName;
            this.activeFlag = activeFlag;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AppUIElementParameterAttributeDTO(int uiParameterAttributeId, int parameterId, string displayText1, string displayText2, int languageId,
              bool synchStatus, int masterEntityId, string lastUpdatedBy, DateTime lastUpdatedDate, int siteId, string guid, byte[] displayImage,
              string fileName, string createdBy, DateTime creationDate, bool activeFlag)
            :this(uiParameterAttributeId, parameterId, displayText1, displayText2, languageId, displayImage, fileName, activeFlag)
        {
            log.LogMethodEntry(uiParameterAttributeId, parameterId, displayText1, displayText2, languageId, synchStatus, masterEntityId,
                lastUpdatedBy, lastUpdatedDate, siteId, guid, displayImage, fileName, createdBy, creationDate,activeFlag);
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the UIParameterAttributeId field
        /// </summary>
        public int UIParameterAttributeId
        {
            get { return uiParameterAttributeId; }
            set { this.IsChanged = true; uiParameterAttributeId = value; }
        }
        /// <summary>
        /// Get/Set method of the ParameterId field
        /// </summary>
        public int ParameterId
        {
            get { return parameterId; }
            set { this.IsChanged = true; parameterId = value; }
        }
        /// <summary>
        /// Get/Set method of the DisplayText1 field
        /// </summary>
        public string DisplayText1
        {
            get { return displayText1; }
            set { this.IsChanged = true; displayText1 = value; }
        }
        /// <summary>
        /// Get/Set method of the DisplayText2 field
        /// </summary>
        public string DisplayText2
        {
            get { return displayText2; }
            set { this.IsChanged = true; displayText2 = value; }
        }
        /// <summary>
        /// Get/Set method of the LanguageId field
        /// </summary>
        public int LanguageId
        {
            get { return languageId; }
            set { this.IsChanged = true; languageId = value; }
        }
        /// <summary>
        /// Get/Set method of the DisplayImage field
        /// </summary>
        public byte[] DisplayImage
        {
            get { return displayImage; }
            set { this.IsChanged = true; displayImage = value; }
        }
        /// <summary>
        /// Get/Set method of the FileName field
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { this.IsChanged = true; fileName = value; }
        }
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
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set {  lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set {  siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { this.IsChanged = true; guid = value; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set {  synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { this.IsChanged = true; masterEntityId = value; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set {  createdBy = value; }
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
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set { this.IsChanged = true; activeFlag = value; }
        }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || uiParameterAttributeId< 0;
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

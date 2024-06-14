/********************************************************************************************
 * Project Name - CMSContentTemplateDTO   
 * Description  - Data object of the CMSContentTemplate
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.70        09-Jul-2019   Girish Kundar      Modified : Added constructor with required parameters.
 *                                                        Added masterEntity field.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.WebCMS
{
    /// <summary>
    ///  This is the user data object class. This acts as data holder for the user business object
    /// </summary>   
    public class CMSContentTemplateDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByCMSContentTemplateParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 

        public enum SearchByCMSContentTemplateParameters
        {
            /// <summary>
            /// Search by CONTENT_TEMPLATE_ID field
            /// </summary>
            CONTENT_TEMPLATE_ID ,
            /// <summary>
            /// Search by CONTENT_TEMPLATE_NAME field
            /// </summary>
            CONTENT_TEMPLATE_NAME ,
            /// <summary>
            /// Search by CONTROL_FILE_NAME field
            /// </summary>
            CONTROL_FILE_NAME ,
            /// <summary>
            /// Search by TEMPLATE_FILE_NAME field
            /// </summary>
            TEMPLATE_FILE_NAME ,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE ,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID ,
            /// <summary>
            /// Search by CREATED_DATE field
            /// </summary>
            CREATED_DATE ,
            /// <summary>
            /// Search by CREATED_BY field
            /// </summary>
            CREATED_BY ,
            /// <summary>
            /// Search by LAST_UPDATED_DATE field
            /// </summary>
            LAST_UPDATED_DATE,
            /// <summary>
            /// Search by LAST_UPDATED_BY field
            /// </summary>
            LAST_UPDATED_BY ,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by SYNCH_STATUS field
            /// </summary>
            SYNCH_STATUS ,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID ,
        }


       private int contentTemplateId;
       private string contentTemplateName;
       private string controlFileName;
       private string templateFileName;
       private bool isActive;
       private string guid;
       private DateTime creationDate;
       private string createdBy;
       private DateTime lastUpdatedDate;
       private string lastUpdatedBy;
       private int site_id;
       private bool synchStatus;
       private int masterEntityId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSContentTemplateDTO()
        {
            log.LogMethodEntry();
            contentTemplateId = -1;
            masterEntityId = -1;
            site_id = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CMSContentTemplateDTO(int contentTemplateId, string contentTemplateName, string controlFileName, string templateFileName, bool isActive)
            :this()
        {
            log.LogMethodEntry(contentTemplateId, contentTemplateName, controlFileName, templateFileName, isActive);
            this.contentTemplateId = contentTemplateId;
            this.contentTemplateName = contentTemplateName;
            this.controlFileName = controlFileName;
            this.templateFileName = templateFileName;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CMSContentTemplateDTO(int contentTemplateId, string contentTemplateName, string controlFileName, 
                                     string templateFileName,  bool isActive, DateTime creationDate, string createdBy, 
                                     DateTime lastUpdatedDate, string lastUpdatedBy, string guid, int site_id,
                                     bool synchStatus, int masterEntityId)
            :this(contentTemplateId, contentTemplateName, controlFileName, templateFileName, isActive)
        {
            log.LogMethodEntry(contentTemplateId, contentTemplateName, controlFileName, templateFileName, isActive, 
                               creationDate, createdBy, lastUpdatedDate, lastUpdatedBy, guid,  site_id,
                               synchStatus,  masterEntityId);
           
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ContentTemplateId field
        /// </summary>
        [DisplayName("ContentTemplate Id ")]
        //[ReadOnly(true)]
        public int ContentTemplateId { get { return contentTemplateId; } set { contentTemplateId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ContentTemplate Name field
        /// </summary>
        [DisplayName("ContentTemplate Name ")]
        public string ContentTemplateName { get { return contentTemplateName; } set { contentTemplateName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Control FileName field
        /// </summary>
        [DisplayName("Control FileName ")]
        public string ControlFileName { get { return controlFileName; } set { controlFileName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Template FileName  field
        /// </summary>
        [DisplayName("Template FileName ")]
        public string TemplateFileName { get { return templateFileName; } set { templateFileName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the GUID field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site Id")]
        public int Site_id { get { return site_id; } set { site_id = value;  } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged;
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

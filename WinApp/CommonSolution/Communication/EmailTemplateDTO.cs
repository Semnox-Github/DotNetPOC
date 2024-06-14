/********************************************************************************************
 * Project Name - EmailTemplate DTO Programs 
 * Description  - Data object of the EmailTemplateDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By                        Remarks          
 *********************************************************************************************
 *1.00        14-April-2016   Rakshith                            Created 
 *2.60.0      07-Feb-2019     Flavia  Jyothi Dsouza               modified
 *2.70.2.0      17-Jul-2019     Girish Kundar                       modified : Added Parametrized Constructor with required fields
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Communication
{

    /// <summary>
    /// This is the EmailTemplate data object class. This acts as data holder for the EmailTemplate business object
    /// </summary>
    public class EmailTemplateDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// <summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  EmailTemplateId
            /// <summary>
            EMAIL_TEMPLATE_ID,
            /// <summary>
            /// Search by Name 
            /// </summary>
            NAME,
            /// </summary>
            /// Search by Site Id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search By IsActive Field
            /// </summary>
            ISACTIVE
        }

        private int emailTemplateId;
        private string name;
        private string description;
        private DateTime startDate;
        private DateTime endDate;
        private string emailTemplate;
        private string lastUpdatedUser;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private String createdBy;
        private DateTime creationDate;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EmailTemplateDTO()
        {
            log.LogMethodEntry();
            this.emailTemplateId = -1;
            this.startDate = DateTime.MinValue;
            this.endDate = DateTime.MinValue;
            this.lastUpdatedDate = DateTime.MinValue;
            this.siteId = -1;
            this.synchStatus = false;
            this.masterEntityId = -1;
            this.isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with required fields.
        /// </summaryCustomersDTO>
        public EmailTemplateDTO(int emailTemplateId, string name, string description, DateTime startDate, DateTime endDate, string emailTemplate)
            : this()
        {
            log.LogMethodEntry(emailTemplateId, name, description, startDate, endDate, emailTemplate);
            this.emailTemplateId = emailTemplateId;
            this.name = name;
            this.description = description;
            this.startDate = startDate;
            this.endDate = endDate;
            this.emailTemplate = emailTemplate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the fields.
        /// </summaryCustomersDTO>
        public EmailTemplateDTO(int emailTemplateId, string name, string description, DateTime startDate, DateTime endDate, string emailTemplate,
                                string lastUpdatedUser, DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus, int masterEntityId, String createdBy,
                                   DateTime creationDate, bool isActive)
            : this(emailTemplateId, name, description, startDate, endDate, emailTemplate)
        {
            log.LogMethodEntry(emailTemplateId, name, description, startDate, endDate, emailTemplate,
                               lastUpdatedUser, lastUpdatedDate, siteId, guid, synchStatus, masterEntityId, createdBy,
                               creationDate);
            this.lastUpdatedUser = lastUpdatedUser;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the EmailTemplateId field
        /// </summary>
        [DisplayName("EmailTemplateId")]
        [DefaultValue(-1)]
        public int EmailTemplateId
        {
            get
            {
                return emailTemplateId;
            }
            set
            {
                this.IsChanged = true;
                emailTemplateId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.IsChanged = true;
                name = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                this.IsChanged = true;
                description = value;
            }
        }

        /// <summary>
        /// Get/Set method of the StartDate field
        /// </summary>
        [DisplayName("StartDate")]
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                this.IsChanged = true;
                startDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the EndDate field
        /// </summary>
        [DisplayName("EndDate")]
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                this.IsChanged = true;
                endDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the EmailTemplate field
        /// </summary>
        [DisplayName("EmailTemplate")]
        public string EmailTemplate
        {
            get
            {
                return emailTemplate;
            }
            set
            {
                this.IsChanged = true;
                emailTemplate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("LastUpdatedUser")]
        public string LastUpdatedUser
        {
            get
            {
                return lastUpdatedUser;
            }
            set
            {
                lastUpdatedUser = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdatedDate;
            }
            set
            {
                lastUpdatedDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [DefaultValue(-1)]
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                this.IsChanged = true;
                siteId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
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
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
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
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
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
        ///Get/Set for CreatedBy
        /// <summary>
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
            }
        }

        /// <summary>
        ///Get/Set for CreationDate
        /// <summary>
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Allows to accept the changes to the value 
        /// <summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set  for IsChanged 
        /// <summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || emailTemplateId < 0;
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

    }
}

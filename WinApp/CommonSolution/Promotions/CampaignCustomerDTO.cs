/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data object of CampaignCustomers
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        23-May-2019   Girish Kundar           Created 
 *2.80        22-Nov-2019    Rakesh                 Added name,email,contacthone1,
 *                                                  birthDate,cardNumber properties 
 *2.100.0     15-Sep-2020   Nitin Pai               Push Notification changes to hold notification sent date and time                                                  
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// This is the CampaignCustomerDTO data object class. This acts as data holder for the CampaignCustomers business object
    /// </summary>
    public class CampaignCustomerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by   CampaignCustomers ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by   CAMPAIGN ID field
            /// </summary>
            CAMPAIGN_ID,
            /// <summary>
            /// Search by   CAMPAIGN ID List field
            /// </summary>
            CAMPAIGN_ID_LIST,
            /// <summary>
            /// Search by CUSTOMER ID field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by  CARD ID field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by  EMAIL SENT STATUS field
            /// </summary>
            EMAIL_STATUS,
            /// <summary>
            /// Search by  SMS SENT STATUS field
            /// </summary>
            SMS_STATUS,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by NOTIFICATION_STATUS field
            /// </summary>
            NOTIFICATION_STATUS
        }

        private int id;
        private int campaignId;
        private int customerId;
        private int cardId;
        private DateTime? emailSentDate;
        private DateTime? smsSentDate;
        private DateTime? notificationSentDate;
        private string emailStatus;
        private string smsStatus;
        private string notificationStatus;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string createdBy;
        private DateTime creationDate;
        private string name;
        private string email;
        private string contactPhone1;
        private string notificationToken;
        private DateTime? birthDate;
        private string cardNumber;
        private decimal? credit;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CampaignCustomerDTO()
        {
            log.LogMethodEntry();
            id = -1;
            campaignId = -1;
            customerId = -1;
            cardId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            emailSentDate = null;
            smsSentDate = null;
            credit = null;
            birthDate = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public CampaignCustomerDTO(int id, int campaignId, int customerId, int cardId, DateTime? emailSentDate, DateTime? smsSentDate, DateTime? notificationSentDate,
                                   string emailStatus, string smsStatus, string notificationStatus, bool isActive)
            :this()
        {

            log.LogMethodEntry(id, campaignId, customerId, cardId, emailSentDate, smsSentDate, notificationSentDate, emailStatus, smsStatus, notificationStatus);
            this.id = id;
            this.campaignId = campaignId;
            this.customerId = customerId;
            this.cardId = cardId;
            this.emailSentDate = emailSentDate;
            this.smsSentDate = smsSentDate;
            this.emailStatus = emailStatus;
            this.smsStatus = smsStatus;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public CampaignCustomerDTO(int id, int campaignId, int customerId, int cardId, DateTime? emailSentDate, DateTime? smsSentDate, DateTime? notificationSentDate,
                                   string emailStatus, string smsStatus, string notificationStatus, string guid, int siteId, bool synchStatus, int masterEntityId,
                                   DateTime lastUpdatedDate, string lastUpdatedBy, string createdBy, DateTime creationDate, bool isActive)
            : this(id, campaignId, customerId, cardId, emailSentDate, smsSentDate, notificationSentDate, emailStatus, notificationStatus, smsStatus, isActive)
        {

            log.LogMethodEntry(id, campaignId, customerId, cardId, emailSentDate, smsSentDate, emailStatus, smsStatus, guid,
                               siteId, synchStatus, masterEntityId, lastUpdatedDate, lastUpdatedBy, createdBy, creationDate, isActive);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        public CampaignCustomerDTO(int id, int campaignId, int customerId, int cardId, string name, string cardNumber, string email, string contactPhone1,
                                           DateTime? birthDate, DateTime? emailSentDate, DateTime? smsSentDate, DateTime? notificationSentDate, string emailStatus, 
                                           string notificationStatus, string smsStatus, decimal? credit, string notificationToken)
                   : this()
        {
            log.LogMethodEntry(id, customerId, cardId, name, cardNumber, email, contactPhone1, birthDate,
                               emailSentDate, smsSentDate, emailStatus, smsStatus, credit, notificationToken);
            this.id = id;
            this.campaignId = campaignId;
            this.customerId = customerId;
            this.cardId = cardId;
            this.name = name;
            this.cardNumber = cardNumber;
            this.email = email;
            this.contactPhone1 = contactPhone1;
            this.birthDate = birthDate;
            this.emailSentDate = emailSentDate;
            this.smsSentDate = smsSentDate;
            this.emailStatus = emailStatus;
            this.smsStatus = smsStatus;
            this.credit = credit;
            this.notificationToken = notificationToken;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        public int CustomerId
        {
            get { return customerId; }
            set { customerId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CampaignId field
        /// </summary>
        public int CampaignId
        {
            get { return campaignId; }
            set { campaignId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public int CardId
        {
            get { return cardId; }
            set { cardId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the EmailSentDate field
        /// </summary>
        public DateTime? EmailSentDate
        {
            get { return emailSentDate; }
            set { emailSentDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SMSentDate field
        /// </summary>
        public DateTime? SMSSentDate
        {
            get { return smsSentDate; }
            set { smsSentDate = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SMSentDate field
        /// </summary>
        public DateTime? NotificationSentDate
        {
            get { return notificationSentDate; }
            set { notificationSentDate = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the EmailStatus field
        /// </summary>
        public string EmailStatus
        {
            get { return emailStatus; }
            set { emailStatus = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SMSStatus field
        /// </summary>
        public string SMSStatus
        {
            get { return smsStatus; }
            set { smsStatus = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SMSStatus field
        /// </summary>
        public string NotificationStatus
        {
            get { return notificationStatus; }
            set { notificationStatus = value; this.IsChanged = true; }
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
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
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
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { name = value; } }
        /// <summary>
        /// Get/Set method of the Email field
        /// </summary>
        public string Email { get { return email; } set { email = value; } }
        /// <summary>
        /// Get/Set method of the ContactPhone1 field
        /// </summary>
        public string ContactPhone1 { get { return contactPhone1; } set { contactPhone1 = value; } }
        /// <summary>
        /// Get/Set method of the pushNotificationToken field
        /// </summary>
        public string NotificationToken { get { return notificationToken; } set { notificationToken = value; } }
        /// <summary>
        /// Get/Set method of the BirthDate field
        /// </summary>
        public DateTime? BirthDate { get { return birthDate; } set { birthDate = value; } }
        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; } }
        /// <summary>
        /// Get/Set method of the Credit field
        /// </summary>
        public decimal? Credit { get { return credit; } set { credit = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}

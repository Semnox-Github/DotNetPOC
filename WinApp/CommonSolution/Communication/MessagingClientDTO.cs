/********************************************************************************************
 * Project Name - MessagingClientDTO
 * Description  - Data object of the messaing client
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.80.0      23-Jun-2020   Jinto Thomas   Created.
 *2.100.0     22-Aug-2020   Vikas Dwivedi  Modified as per 3-Tier Standard CheckList
 *2.120.6     20-Feb-2022   Nitin Pai      SendGrid Email Change - From Email and Domain Name
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Communication
{
    public class MessagingClientDTO
    {
        private static readonly logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by CLIENT ID field
            /// </summary>
            CLIENT_ID,
            /// <summary>
            /// Search by CLIENT ID LIST field
            /// </summary>
            CLIENT_ID_LIST,
            /// <summary>
            /// Search by CLIENT NAME field
            /// </summary>
            CLIENT_NAME,
            /// <summary>
            /// Search by MESSAGING CHANEL CODE field
            /// </summary>
            MESSAGING_CHANNEL_CODE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary> 
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by MESSAGING CHANEL CODE field
            /// </summary>
            MESSAGING_SUB_CHANNEL_TYPE,
        }

        private int clientId;
        private string clientName;
        private string messagingChannelCode;
        private string messagingSubChannelType;
        private string sender;
        private string hostUrl;
        private int smtpPort;
        private string userName;
        private string password;
        private bool enableSsl;
        private bool synchStatus;
        private string guid;
        private int siteId;
        private bool isActive;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private string fromEmail;
        private string domain;


        public enum MessagingChanelType
        {
            ///<summary>
            ///NONE
            ///</summary>
            [Description("NONE")] NONE,
            ///<summary>
            ///EMAIL
            ///</summary>
            [Description("Email")] EMAIL,

            ///<summary>
            ///SMS
            ///</summary>
            [Description("SMS")] SMS,

            /// <summary>
            /// APP NOTIFICATION
            /// </summary>
            [Description("App Notification")] APP_NOTIFICATION,
            /// <summary>
            /// APP NOTIFICATION
            /// </summary>
            [Description("WhatsApp Message")] WHATSAPP_MESSAGE
        }

        public static string SourceEnumToString(MessagingChanelType status)
        {
            String returnString = "";
            switch (status)
            {
                case MessagingChanelType.EMAIL:
                    returnString = "E";
                    break;
                case MessagingChanelType.SMS:
                    returnString = "S";
                    break;
                case MessagingChanelType.APP_NOTIFICATION:
                    returnString = "A";
                    break;
                case MessagingChanelType.WHATSAPP_MESSAGE:
                    returnString = "W";
                    break;
                default:
                    returnString = "";
                    break;
            }
            return returnString;
        }

        public static MessagingChanelType SourceEnumFromString(String status)
        {
            MessagingChanelType returnValue = MessagingChanelType.NONE;
            switch (status)
            {
                case "E":
                    returnValue = MessagingChanelType.EMAIL;
                    break;
                case "S":
                    returnValue = MessagingChanelType.SMS;
                    break;
                case "A":
                    returnValue = MessagingChanelType.APP_NOTIFICATION;
                    break;
                case "W":
                    returnValue = MessagingChanelType.WHATSAPP_MESSAGE;
                    break;
                default:
                    returnValue = MessagingChanelType.NONE;
                    break;
            }
            return returnValue;
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MessagingClientDTO()
        {
            log.LogMethodEntry();
            clientId = -1;
            clientName = "";
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>

        public MessagingClientDTO(int clientId, string clientName, string messagingChannelCode, string sender, string hostUrl, int smtpPort, string userName, string password,
                                    bool enableSsl, string messagingSubChannelType, string fromEmail, string domain)
            : this()
        {
            log.LogMethodEntry(clientId, clientName, messagingChannelCode, sender, hostUrl, smtpPort, userName, password, enableSsl);
            this.clientId = clientId;
            this.clientName = clientName;
            this.messagingChannelCode = messagingChannelCode;
            this.sender = sender;
            this.hostUrl = hostUrl;
            this.smtpPort = smtpPort;
            this.userName = userName;
            this.password = password;
            this.enableSsl = enableSsl;
            this.messagingSubChannelType = messagingSubChannelType;
            this.fromEmail = fromEmail;
            this.domain = domain;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>

        public MessagingClientDTO(int clientId, string clientName, string messagingChannelCode, string host, string hostUrl, int smtpPort, string userName, string password,
                                    bool enableSsl, bool synchStatus, string guid, int siteId, bool isActive, int masterEntityId, string createdBy, DateTime creationDate,
                                    string lastUpdatedBy, DateTime lastUpdatedDate, string messagingSubChannelType, string fromEmail, string domain)
            : this(clientId, clientName, messagingChannelCode, host, hostUrl, smtpPort, userName, password, enableSsl, messagingSubChannelType, fromEmail, domain)
        {
            log.LogMethodEntry(clientId, clientName, messagingChannelCode, host, hostUrl, smtpPort, userName, password, enableSsl, synchStatus, guid, siteId, isActive,
                                masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate);
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.siteId = siteId;
            this.isActive = isActive;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ClientId  field
        /// </summary>

        public int ClientId
        {
            get { return clientId; }
            set { clientId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ClientName  field
        /// </summary>

        public string ClientName
        {
            get { return clientName; }
            set { clientName = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the MessagingChanelCode  field
        /// </summary>

        public string MessagingChannelCode
        {
            get { return messagingChannelCode; }
            set { messagingChannelCode = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the MessagingSubChannelType  field
        /// </summary>

        public string MessagingSubChannelType
        {
            get { return messagingSubChannelType; }
            set { messagingSubChannelType = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Host  field
        /// </summary>

        public string Sender
        {
            get { return sender; }
            set { sender = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Host  field
        /// </summary>

        public string HostUrl
        {
            get { return hostUrl; }
            set { hostUrl = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Port  field
        /// </summary>

        public int SmtpPort
        {
            get { return smtpPort; }
            set { smtpPort = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the UserName  field
        /// </summary>

        public string UserName
        {
            get { return userName; }
            set { userName = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Password  field
        /// </summary>

        public string Password
        {
            get { return password; }
            set { password = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the EnableSsl  field
        /// </summary>

        public bool EnableSsl
        {
            get { return enableSsl; }
            set { enableSsl = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SysnchStatus  field
        /// </summary>

        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }

        /// <summary>
        /// Get/Set method of the Guid  field
        /// </summary>

        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SiteId  field
        /// </summary>

        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
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
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>

        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate  field
        /// </summary>

        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }

        /// <summary>
        /// Get/Set method of the FromEmail field
        /// </summary>

        public string FromEmail
        {
            get { return fromEmail; }
            set { fromEmail = value; }
        }

        /// <summary>
        /// Get/Set method of the Domain field
        /// </summary>

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
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
                    return notifyingObjectIsChanged || clientId < 0;
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

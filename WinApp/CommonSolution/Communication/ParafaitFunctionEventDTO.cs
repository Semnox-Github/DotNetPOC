/********************************************************************************************
* Project Name - ParafaitFunctionEventDTO
* Description - DTO for ParafaitFunctionEvent 
*
**************
**Version Log 
**************
*Version    Date        Modified By     Remarks
*********************************************************************************************
*2.110.0    10-Dec-2020  Fiona          Created for subscription feature
*2.130.7    13-Apr-2022  Guru S A       Payment mode OTP validation changes
*2.130.11   18-Aug-2022  Yashodhara C H Added new OTP function events
*2.140.5    10-Jan-2023  Muaaz Musthafa Added Reservation Purchase Event
*********************************************************************************************/
using System;
using System.Collections.Generic; 

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// Parafait Function Events
    /// </summary>
    public enum ParafaitFunctionEvents
    {
        //NONE
        NONE,
        //Customer function Events
        NEW_REGISTRATION_EVENT,
        REGISTRATION_LINK_EVENT,
        CUSTOMER_VERIFICATION_EVENT,
        RESET_PASSWORD_EVENT,
        LINK_CUSTOMER_ACCOUNT_EVENT,
        CARD_EXPIRY_MESSAGE_TRIGGER_EVENT,
        CAMPAIGN_MESSAGE_EVENT,
        //Subscription Events
        SUBSCRIPTION_PURCHASE_EVENT,
        CANCEL_SUBSCRIPTION_EVENT,
        RENEW_SUBSCRIPTION_EVENT,
        RENEWAL_REMINDER_EVENT,
        SUBSCRIPTION_PAYMENT_FAILURE_EVENT,
        PAUSE_SUBSCRIPTION_EVENT,
        UNPAUSE_SUBSCRIPTION_EVENT,
        SUBSCRIPTION_CARD_EXPIRY_EVENT,
        REACTIVATE_SUBSCRIPTION_EVENT,
        BILL_SUBSCRIPTION_EVENT,
        //Transaction function Events
        PURCHASE_EVENT,
        PAYMENT_LINK_EVENT,
        EXECUTE_ONLINE_TRANSACTION_EVENT,
        PURCHASE_MESSAGE_TRIGGER_EVENT,
        PAYMENT_MODE_OTP_EVENT,
        REDEEM_TOKEN_TRANSACTION_EVENT,
        ABORT_REDEEM_TOKEN_TRANSACTION_EVENT,
        ABORT_TRANSACTION_EVENT,
        KIOSK_CARD_DISPENSER_ERROR_EVENT,
        KiOSK_WRISTBAND_PRINT_ERROR,
        //Redemption function Events
        REDEMPTION_MESSAGE_TRIGGER_EVENT,
        //Maintenance function Events
        SERVICE_REQUEST_STATUS_CHANGE_EVENT,
        PAYMENT_REFUND_EVENT,
        TRANSACTION_CANCELLATION_EVENT,
        //OTP function events
        GENERIC_EVENT,
        LOGIN_OTP_EVENT,
        CUSTOMER_DELETE_OTP_EVENT,
        //Reservation function events
        RESERVATION_PURCHASE_EVENT
    }
    public class ParafaitFunctionEventDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int parafaitFunctionEventId;
        private ParafaitFunctionEvents parafaitFunctionEventName;
        private string parafaitFunctionEventNameString;
        private string parafaitFunctionEventDescription;
        private int parafaitFunctionId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTOList;
        //private bool notifyingObjectIsChanged;
        //private readonly object notifyingObjectIsChangedSyncRoot = new Object(); 

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            PARAFAIT_FUNCTION_EVENT_ID,
            /// <summary>
            /// Search by  ParafaitFunctionEventId field
            /// </summary>
            PARAFAIT_FUNCTION_ID,
            /// <summary>
            /// Search by  ID field
            /// </summary>
            PARAFAIT_FUNCTION_EVENT_NAME,
            /// <summary>
            /// Search by  IsActive field
            /// </summary> 
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID
        }

        /// <summary>
        /// Gets the ParafaitFunctionEventName 
        /// </summary>
        /// <param name="parafaitFunctionEvents"></param>
        /// <returns></returns>
        private string GetParafaitFunctionEventNameString(ParafaitFunctionEvents parafaitFunctionEvents)
        {
            log.LogMethodEntry();
            String parafaitFunctionEventNameString = "";
            parafaitFunctionEventNameString = Enum.GetName(typeof(ParafaitFunctionEvents), parafaitFunctionEvents);
            log.LogMethodExit(parafaitFunctionEventNameString);
            return parafaitFunctionEventNameString;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ParafaitFunctionEventDTO()
        {
            log.LogMethodEntry();
            parafaitFunctionEventId = -1;
            parafaitFunctionId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            messagingClientFunctionLookUpDTOList = new List<MessagingClientFunctionLookUpDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        ///Constructor with Required Fields
        /// </summary>
        public ParafaitFunctionEventDTO(int parafaitFunctionEventId, ParafaitFunctionEvents parafaitFunctionEventName, string parafaitFunctionEventDescription, int parafaitFunctionId)
            :this()
        {
            log.LogMethodEntry( parafaitFunctionEventId,  parafaitFunctionEventName,  parafaitFunctionEventDescription,  parafaitFunctionId);
            this.parafaitFunctionEventId = parafaitFunctionEventId;
            this.parafaitFunctionEventName = parafaitFunctionEventName;
            this.parafaitFunctionEventDescription = parafaitFunctionEventDescription;
            this.parafaitFunctionId = parafaitFunctionId;
            log.LogMethodExit();
        }
        /// <summary>
        ///Constructor with All Fields
        /// </summary>
        public ParafaitFunctionEventDTO(int parafaitFunctionEventId, ParafaitFunctionEvents parafaitFunctionEventName, 
            string parafaitFunctionEventDescription, int parafaitFunctionId, bool isActive, string createdBy, DateTime creationDate, 
            string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid) : 
            this(parafaitFunctionEventId, parafaitFunctionEventName, parafaitFunctionEventDescription, parafaitFunctionId)
        {
            log.LogMethodEntry( parafaitFunctionEventId,  parafaitFunctionEventName, 
             parafaitFunctionEventDescription,  parafaitFunctionId,  isActive,  createdBy,  creationDate,
             lastUpdatedBy,  lastUpdateDate,  siteId,  masterEntityId,  synchStatus,  guid);
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the ParafaitFunctionEventId field
        /// </summary>
        public int ParafaitFunctionEventId
        {
            get { return parafaitFunctionEventId; }
            set { parafaitFunctionEventId = value; //this.IsChanged = true; 
            }
        }
        /// <summary>
        /// Get/Set method of the ParafaitFunctionEventName field
        /// </summary>
        public ParafaitFunctionEvents ParafaitFunctionEventName
        {
            get { return parafaitFunctionEventName; }
            set { parafaitFunctionEventName = value; //this.IsChanged = true; 
            }
        }

        /// <summary>
        /// Get/Set method of the ParafaitFunctionEventNameString field
        /// </summary>
        public string ParafaitFunctionEventNameString
        {
            get
            {
                return GetParafaitFunctionEventNameString(parafaitFunctionEventName);
            }
        }

        /// <summary>
        /// Get/Set method of the ParafaitFunctionEventDescription field
        /// </summary>
        public string ParafaitFunctionEventDescription
        {
            get { return parafaitFunctionEventDescription; }
            set { parafaitFunctionEventDescription = value; //this.IsChanged = true; 
            }
        }
        /// <summary>
        /// Get/Set method of the ParafaitFunctionId field
        /// </summary>
        public int ParafaitFunctionId
        {
            get { return parafaitFunctionId; }
            set { parafaitFunctionId = value; //this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                //this.IsChanged = true;
                isActive = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
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
        /// Get/Set method of the CreationDate field
        /// </summary>
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
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {

                lastUpdatedBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {

                lastUpdateDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get
            {
                return siteId;
            }

            set
            {

                siteId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }

            set
            {
               // this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
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
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }

            set
            {
                //this.IsChanged = true;
                guid = value;
            }
        }
        /// <summary>
        /// Get/Set method of MessagingClientFunctionLookUpDTOList field
        /// </summary>
        public List<MessagingClientFunctionLookUpDTO> MessagingClientFunctionLookUpDTOList
        {
            get { return messagingClientFunctionLookUpDTOList; }
            set { messagingClientFunctionLookUpDTOList = value; }
        }

        ///// <summary>
        ///// Get/Set method to track changes to the object
        ///// </summary>
        //public bool IsChanged
        //{
        //    get
        //    {
        //        lock (notifyingObjectIsChangedSyncRoot)
        //        {
        //            return notifyingObjectIsChanged || parafaitFunctionEventId < 0;
        //        }
        //    }

        //    set
        //    {
        //        lock (notifyingObjectIsChangedSyncRoot)
        //        {
        //            if (!Boolean.Equals(notifyingObjectIsChanged, value))
        //            {
        //                notifyingObjectIsChanged = value;
        //            }
        //        }
        //    }
        //}
        ///// <summary>
        ///// Allows to accept the changes
        ///// </summary>
        //public void AcceptChanges()
        //{
        //    log.LogMethodEntry();
        //    this.IsChanged = false;
        //    log.LogMethodExit();
        //}
    }
}

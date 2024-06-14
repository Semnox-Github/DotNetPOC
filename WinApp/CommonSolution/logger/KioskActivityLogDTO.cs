/********************************************************************************************
 * Project Name - Kiosk Activity Log DTO
 * Description  - Data object of Kiosk Activity Log
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70        16-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor,
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// This is the KioskActivityLog data object class. This acts as data holder for the KioskActivityLog business object
    /// </summary>
    public class KioskActivityLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DateTime timeStamp;
        private string noteCoinFlag;
        private string activity;
        private double? value;
        private string cardNumber;
        private string message;
        private int kioskId;
        private string kioskName;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int trxId;
        private int kioskTrxId;
        private int masterEntityId;
        private int kioskActivityLogId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;


        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by kioskActivityLogId field
            /// </summary>
            KIOSK_ACTIVITY_LOG_ID,
            /// <summary>
            /// Search by kioskId field
            /// </summary>
            KIOSK_ID,
            /// <summary>
            /// Search by cardNumber field
            /// </summary>
            CARD_NUMBER,
            /// <summary>
            /// Search by kioskName field
            /// </summary>
            KIOSK_NAME,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ACTIVITY field
            /// </summary>
            ACTIVITY,
            /// <summary>
            /// Search by FROM_TIME_STAMP field
            /// </summary>
            FROM_TIME_STAMP,
            /// <summary>
            /// Search by TO_TIME_STAMP field
            /// </summary>
            TO_TIME_STAMP
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public KioskActivityLogDTO()
        {
            log.LogMethodEntry();
            siteId = -1;
            trxId = -1;
            kioskId = -1;
            kioskTrxId = -1;
            cardNumber = "";
            message = "";
            kioskName = "";
            masterEntityId = -1;
            kioskActivityLogId = -1;
            value = null;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public KioskActivityLogDTO(string noteCoinFlag, DateTime timeStamp, string activity, double? value,
                                                  string cardNumber, string message, int kioskId,
                                                  string kioskName, string guid, bool synchStatus,
                                                  int trxId, int siteId, int kioskTrxId, int masterEntityId) : this()
        {
            log.LogMethodEntry(noteCoinFlag, timeStamp, activity, value, cardNumber, message, kioskId,
                               kioskName, guid, synchStatus, trxId, siteId, kioskTrxId, masterEntityId);
            this.timeStamp = timeStamp;
            this.noteCoinFlag = noteCoinFlag;
            this.activity = activity;
            this.value = value;
            this.cardNumber = cardNumber;
            this.message = message;
            this.kioskId = kioskId;
            this.kioskName = kioskName;
            this.trxId = trxId;
            this.kioskTrxId = kioskTrxId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public KioskActivityLogDTO(int kioskActivityLogId, string noteCoinFlag, DateTime timeStamp, string activity, double? value,
                                                  string cardNumber, string message, int kioskId,
                                                  string kioskName, string guid, bool synchStatus,
                                                  int trxId, int siteId, int kioskTrxId, int masterEntityId, string createdBy, DateTime creationDate,
                                                  string lastUpdatedBy, DateTime lastUpdateDate)
            : this(noteCoinFlag, timeStamp, activity, value, cardNumber, message, kioskId,
                               kioskName, guid, synchStatus, trxId, siteId, kioskTrxId, masterEntityId)
        {
            log.LogMethodEntry(noteCoinFlag, timeStamp, activity, value, cardNumber, message, kioskId,
                               kioskName, guid, synchStatus, trxId, siteId, kioskTrxId, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.kioskActivityLogId = kioskActivityLogId;
            this.createdBy = createdBy;
            this.lastUpdatedBy = lastUpdatedBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the TimeStamp field
        /// </summary>
        public DateTime TimeStamp
        {
            get
            {
                return timeStamp;
            }

            set
            {
                IsChanged = true;
                timeStamp = value;
            }
        }

        /// <summary>
        /// Get/Set method of the NoteCoinFlag field
        /// </summary>

        public string NoteCoinFlag
        {
            get
            {
                return noteCoinFlag;
            }
            set
            {
                IsChanged = true;
                noteCoinFlag = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Value  field
        /// </summary>
        public double? Value
        {
            get
            {
                return this.value;
            }
            set
            {
                IsChanged = true;
                this.value = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Activity field
        /// </summary>
        public string Activity
        {
            get
            {
                return activity;
            }
            set
            {
                IsChanged = true;
                activity = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        public string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                IsChanged = true;
                cardNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Message field
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                IsChanged = true;
                message = value;
            }
        }

        /// <summary>
        /// Get/Set method of the KioskId field
        /// </summary>
        public int KioskId
        {
            get
            {
                return kioskId;
            }
            set
            {
                IsChanged = true;
                kioskId = value;
            }
        }


        /// <summary>
        /// Get/Set method of the KioskName field
        /// </summary>
        public string KioskName
        {
            get
            {
                return kioskName;
            }
            set
            {
                IsChanged = true;
                kioskName = value;
            }
        }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                IsChanged = true;
                guid = value;
            }

        }

        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                IsChanged = true;
                synchStatus = value;
            }
        }

        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        public int site_id
        {
            get
            {
                return siteId;
            }
            set
            {
                IsChanged = true;
                siteId = value;

            }
        }

        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        public int TrxId
        {
            get
            {
                return trxId;
            }
            set
            {
                IsChanged = true;
                trxId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the kioskActivityLogId field
        /// </summary>
        public int KioskTrxId
        {
            get
            {
                return kioskTrxId;
            }
            set
            {
                IsChanged = true;
                kioskTrxId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        public int KioskActivityLogId
        {
            get
            {
                return kioskActivityLogId;
            }
            set
            {
                IsChanged = true;
                kioskActivityLogId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the createdBy fields
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreatedDate fields
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the lastUpdatedBy fields
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the lastUpdatedAt fields
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }



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
                IsChanged = true;
                masterEntityId = value;

            }
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
                    return notifyingObjectIsChanged || kioskActivityLogId < 0;
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
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns a string that represents the current KioskActivityLogDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.Debug("Starts-ToString() method.");
            StringBuilder returnValue = new StringBuilder("\n-----------------------KioskActivityLogDTO-----------------------------\n");
            returnValue.Append(" TimeStamp : " + TimeStamp);
            returnValue.Append(" NoteCoinFlag : " + NoteCoinFlag);
            returnValue.Append(" Value : " + Value);
            returnValue.Append(" Activity : " + Activity);
            returnValue.Append(" CardNumber : " + CardNumber);
            returnValue.Append(" Message : " + Message);
            returnValue.Append(" KioskId : " + KioskId);
            returnValue.Append(" KioskName : " + KioskName);
            returnValue.Append(" Guid : " + Guid);
            returnValue.Append(" SynchStatus : " + SynchStatus);
            returnValue.Append(" site_id : " + site_id);
            returnValue.Append(" TrxId : " + TrxId);
            returnValue.Append(" KioskTrxId : " + KioskTrxId);
            returnValue.Append(" MasterEntityId : " + MasterEntityId);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.Debug("Ends-ToString() Method");
            return returnValue.ToString();

        }
    }
}

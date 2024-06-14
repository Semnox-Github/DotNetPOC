/********************************************************************************************
 * Project Name - CheckInDTO
 * Description  - Data object of CheckIns
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        22-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Customer;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Enum which holds the check in statuses
    /// </summary>
    public enum CheckInStatus
    {
        /// <summary>
        /// PENDING
        /// </summary>
        PENDING,
        /// <summary>
        /// ORDERED
        /// </summary>
        ORDERED,
        /// <summary>
        /// CHECKEDIN
        /// </summary>
        CHECKEDIN,
        /// <summary>
        /// PAUSED
        /// </summary>
        PAUSED,
        /// <summary>
        /// CHECKEDOUT
        /// </summary>
        CHECKEDOUT

    }
    public enum CheckInStatusFilter
    {
        /// <summary>
        /// CHECKEDIN
        /// </summary>
        CHECKEDIN,
        /// <summary>
        /// PENDING
        /// </summary>
        PENDING,
        /// <summary>
        /// PAUSED
        /// </summary>
        PAUSED,
        /// <summary>
        /// CHECKEDOUT
        /// </summary>
        CHECKEDOUT,
        /// <summary>
        /// ORDERED
        /// </summary>
        ORDERED,
        /// <summary>
        /// ALL
        /// </summary>
        ALL
    }
    public class CheckInStatusConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<string, string> attributeDictionary = new ConcurrentDictionary<string, string>();
        /// <summary>
        /// Converts CheckInStatus from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CheckInStatus FromString(string value)
        {
            log.LogMethodEntry("value");
            switch (value.ToUpper())
            {
                case "PENDING":
                    {
                        return CheckInStatus.PENDING;
                    }
                case "CHECKEDIN":
                    {
                        return CheckInStatus.CHECKEDIN;
                    }
                case "PAUSED":
                    {
                        return CheckInStatus.PAUSED;
                    }
                case "CHECKEDOUT":
                    {
                        return CheckInStatus.CHECKEDOUT;
                    }
                case "ORDERED":
                    {
                        return CheckInStatus.ORDERED;
                    }

                default:
                    {
                        log.Error("Error :Not a valid CheckInStatus ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid CheckInStatus");
                    }
            }
        }


        /// <summary>
        /// Converts CheckInStatus to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(CheckInStatus value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case CheckInStatus.PENDING:
                    {
                        return "PENDING";
                    }
                case CheckInStatus.CHECKEDIN:
                    {
                        return "CHECKEDIN";
                    }
                case CheckInStatus.PAUSED:
                    {
                        return "PAUSED";
                    }
                case CheckInStatus.CHECKEDOUT:
                    {
                        return "CHECKEDOUT";
                    }
                case CheckInStatus.ORDERED:
                    {
                        return "ORDERED";
                    }
                default:
                    {
                        log.Error("Error :Not a valid CheckInStatus ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid credit plus type");
                    }
            }
        }
    }


    /// <summary>
    /// This is the CheckInDTO data object class. This acts as data holder for the CheckIns business object
    /// </summary>
    public class CheckInDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string StandardTableType = "Standard";
        public const string PoolTableType = "Pool / Snooker";
        public const string KaraokeTableType = "Karaoke";
        public const string SnKInterface = "SnK";
        public const string KMTronicInterface = "KMTronic";

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by    CHECK IN ID field
            /// </summary>
            CHECK_IN_ID,
            /// <summary>
            /// Search by CUSTOMER ID field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by  CARD ID field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by  TIME_SLAB field
            /// </summary>
            CHECK_IN_TRX_ID,
            /// <summary>
            /// Search by  CHECK IN FECILITY ID field
            /// </summary>
            CHECK_IN_FACILITY_ID,
            /// <summary>
            /// Search by  TABLE ID field
            /// </summary>
            TABLE_ID,
            /// <summary>
            /// Search by  TRX LINE ID field
            /// </summary>
            TRX_LINE_ID,
            /// <summary>
            /// Search by  CHECK IN TIME field
            /// </summary>
            CHECK_IN_TIME,
            /// Search by  ACTIVE_FLAG field
            /// <summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            IS_ACTIVE
        }
        private int checkInId;
        private int customerId;
        private DateTime? checkInTime;
        private string photoFileName;
        private byte[] fingerPrint;
        private byte[] fpTemplate;
        private int cardId;
        private int checkInTrxId;
        private int allowedTimeInMinutes;
        private DateTime lastUpdatedDate;
        private int checkInFacilityId;
        private string lastUpdatedBy;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int tableId;
        private int? trxLineId;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string accountNumber;
        private List<CheckInDetailDTO> checkInDetailDTOList;
        private CustomerDTO customerDTO;
        private bool userTime = false;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CheckInDTO()
        {
            log.LogMethodEntry();
            checkInId = -1;
            customerId = -1;
            cardId = -1;
            checkInTrxId = -1;
            checkInFacilityId = -1;
            tableId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            checkInDetailDTOList = new List<CheckInDetailDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public CheckInDTO(int checkInId, int customerId, DateTime? checkInTime, string photoFileName, byte[] fingerPrint,
                          byte[] fpTemplate, int cardId, int checkInTrxId, int allowedTimeInMinutes, int checkInFacilityId,
                         int tableId, int? trxLineId, CustomerDTO customerDTO, bool isActive)
            : this()
        {
            log.LogMethodEntry(checkInId, customerId, checkInTime, photoFileName, fingerPrint, fpTemplate, cardId,
                               checkInTrxId, allowedTimeInMinutes, checkInFacilityId, tableId, trxLineId, isActive);
            this.checkInId = checkInId;
            this.customerId = customerId;
            this.checkInTime = checkInTime;
            this.photoFileName = photoFileName;
            this.fingerPrint = fingerPrint;
            this.fpTemplate = fpTemplate;
            this.cardId = cardId;
            this.checkInTrxId = checkInTrxId;
            this.allowedTimeInMinutes = allowedTimeInMinutes;
            this.checkInFacilityId = checkInFacilityId;
            this.tableId = tableId;
            this.trxLineId = trxLineId;
            this.customerDTO = customerDTO;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public CheckInDTO(int checkInId, int customerId, DateTime? checkInTime, string photoFileName, byte[] fingerPrint,
                          byte[] fpTemplate, int cardId, int checkInTrxId, int allowedTimeInMinutes, string lastUpdatedBy, DateTime lastUpdatedDate,
                          int checkInFacilityId, string guid, int siteId, bool synchStatus, int tableId,
                          int? trxLineId, CustomerDTO customerDTO, int masterEntityId, string createdBy, DateTime creationDate,
                          bool isActive)
            : this(checkInId, customerId, checkInTime, photoFileName, fingerPrint, fpTemplate, cardId,
                               checkInTrxId, allowedTimeInMinutes, checkInFacilityId, tableId, trxLineId, customerDTO,
                                isActive)
        {
            log.LogMethodEntry(checkInId, customerId, checkInTime, photoFileName, fingerPrint, fpTemplate, cardId,
                               checkInTrxId, allowedTimeInMinutes, lastUpdatedBy, lastUpdatedDate, checkInFacilityId, guid,
                               siteId, synchStatus, tableId, trxLineId, masterEntityId, createdBy, creationDate, isActive);
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CheckInId field
        /// </summary>
        public int CheckInId
        {
            get { return checkInId; }
            set { checkInId = value; this.IsChanged = true; }
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
        /// Get/Set method of the CustomerDTO field
        /// </summary>
        public CustomerDTO CustomerDTO
        {
            get { return customerDTO; }
            set { customerDTO = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the userTime field
        /// </summary>
        public bool UserTime
        {
            get { return userTime; }
            set { userTime = value; }
        }
        /// <summary>
        /// Get/Set method of the CheckInTime field
        /// </summary>
        public DateTime? CheckInTime
        {
            get { return checkInTime; }
            set { checkInTime = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PhotoFileName field
        /// </summary>
        public string PhotoFileName
        {
            get { return photoFileName; }
            set { photoFileName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the FingerPrint field
        /// </summary>
        public byte[] FingerPrint
        {
            get { return fingerPrint; }
            set { fingerPrint = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the FPTemplate field
        /// </summary>
        public byte[] FPTemplate
        {
            get { return fpTemplate; }
            set { fpTemplate = value; this.IsChanged = true; }
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
        /// Get method of the AccountDTO field
        /// </summary>
        public string AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }

        /// <summary>
        /// Get/Set method of the CheckInTrxId field
        /// </summary>
        public int CheckInTrxId
        {
            get { return checkInTrxId; }
            set { checkInTrxId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the AllowedTimeInMinutes field
        /// </summary>
        public int AllowedTimeInMinutes
        {
            get { return allowedTimeInMinutes; }
            set { allowedTimeInMinutes = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CheckInFacilityId field
        /// </summary>
        public int CheckInFacilityId
        {
            get { return checkInFacilityId; }
            set { checkInFacilityId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the TableId field
        /// </summary>
        public int TableId
        {
            get { return tableId; }
            set { tableId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TrxLineId field
        /// </summary>
        public int? TrxLineId
        {
            get { return trxLineId; }
            set { trxLineId = value; this.IsChanged = true; }
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
        /// Get/Set method of the CheckInDetailDTOList field
        /// </summary>
        public List<CheckInDetailDTO> CheckInDetailDTOList
        {
            get { return checkInDetailDTOList; }
            set { checkInDetailDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
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
        /// Returns whether the CheckInDTO changed or any of its CheckInDetailDTO childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (checkInDetailDTOList != null &&
                   checkInDetailDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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

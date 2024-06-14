/********************************************************************************************
 * Project Name - CheckInDetailDTO
 * Description  - Data object of CheckInDetails
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        22-May-2019   Girish Kundar           Created 
 *2.70        22-Jun-2019   Mathew Ninan            Added CheckInTrxId and CheckInTrxLineId fields 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the CheckInDetailDTO data object class. This acts as data holder for the CheckInDetails business object
    /// </summary>
    public class CheckInDetailDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by   CHECK IN DETAIL ID field
            /// </summary>
            CHECK_IN_DETAIL_ID,
            /// <summary>
            /// Search by   CHECKIN ID field
            /// </summary>
            CHECK_IN_ID,
            /// <summary>
            /// Search by   CHECKIN ID List field
            /// </summary>
            CHECKIN_ID_LIST,
            /// <summary>
            /// Search by  NAME field
            /// </summary>
            NAME,
            /// <summary>
            /// Search by  CARD ID field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by  VEHICHLE NUMBER field
            /// </summary>
            VEHICHLE_NUMBER,
            /// <summary>
            /// Search by CHECK OUT TRANSACTION ID field
            /// </summary>
            CHECK_OUT_TRX_ID,
            /// <summary>
            /// Search by CHECK OUT TRANSACTION LINE ID field
            /// </summary>
            TRX_LINE_ID,
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
            /// <summary>
            /// Search by CHECK_IN_TRX_ID field
            /// </summary>
            CHECK_IN_TRX_ID,
            /// <summary>
            /// Search by CHECK_IN_TRX_LINE_ID field
            /// </summary>
            CHECK_IN_TRX_LINE_ID,
            /// <summary>
            /// Search by ACTIVE_CHECK_INS field
            /// </summary>
            ACTIVE_CHECK_IN_DETAILS,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by CHECKIN_STATUS field
            /// </summary>
            CHECKIN_STATUS,
            CHECKIN_STATUS_LIST
        }
        private int checkInDetailId;
        private int checkInId;
        private string name;
        private int cardId;
        private string vehicleNumber;
        private string vehicleModel;
        private string vehicleColor;
        private DateTime? dateOfBirth;
        private decimal age;
        private string specialNeeds;
        private string allergies;
        private string remarks;
        private DateTime? checkOutTime;
        private int checkOutTrxId;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int? trxLineId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int siteId;
        private string createdBy;
        private DateTime creationDate;
        private int checkInTrxLineId;
        private int checkInTrxId;
        private List<CheckInPauseLogDTO> checkInPauseLogDTOList;
        private string detail;
        private string accountNumber;
        private DateTime? checkInTime;
        private CheckInStatus status;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CheckInDetailDTO()
        {
            log.LogMethodEntry();
            checkInDetailId = -1;
            checkInId = -1;
            cardId = -1;
            checkOutTrxId = -1;
            checkInTrxId = -1;
            checkInTrxLineId = -1;
            siteId = -1;
            masterEntityId = -1;
            checkInPauseLogDTOList = new List<CheckInPauseLogDTO>();
            checkInTime = null;
            status = CheckInStatus.PENDING;
            isActive = true;
            log.LogMethodExit();
        }


        /// <summary>
        ///  Parameterized constructor with Required fields
        /// </summary>
        public CheckInDetailDTO(int checkInDetailId, int checkInId, string name, int cardId, string vehicleNumber,
                                 string vehicleModel, string vehicleColor, DateTime? dateOfBirth, decimal age,
                                 string specialNeeds, string allergies, string remarks, DateTime? checkOutTime, int checkOutTrxId,
                                 int? trxLineId, int checkInTrxId, int checkInTrxLineId,DateTime? checkInTime, CheckInStatus status,bool isActive)
            : this()
        {

            log.LogMethodEntry(checkInDetailId, checkInId, name, cardId, vehicleNumber, vehicleModel, vehicleColor, dateOfBirth,
                                age, specialNeeds, allergies, remarks, checkOutTime, checkOutTrxId, trxLineId, checkInTrxId,
                                checkInTrxLineId, checkInTime, status, isActive);
            this.checkInDetailId = checkInDetailId;
            this.checkInId = checkInId;
            this.name = name;
            this.cardId = cardId;
            this.vehicleNumber = vehicleNumber;
            this.vehicleModel = vehicleModel;
            this.vehicleColor = vehicleColor;
            this.dateOfBirth = dateOfBirth;
            this.age = age;
            this.specialNeeds = specialNeeds;
            this.allergies = allergies;
            this.remarks = remarks;
            this.checkOutTime = checkOutTime;
            this.checkOutTrxId = checkOutTrxId;
            this.trxLineId = trxLineId;
            this.checkInTrxId = checkInTrxId;
            this.checkInTrxLineId = checkInTrxLineId;
            this.checkInTime = checkInTime;
            this.status = status;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Parameterized constructor with all the fields
        /// </summary>
        public CheckInDetailDTO(int checkInDetailId, int checkInId, string name, int cardId, string vehicleNumber,
                                 string vehicleModel, string vehicleColor, DateTime? dateOfBirth, decimal age,
                                 string specialNeeds, string allergies, string remarks, DateTime? checkOutTime, int checkOutTrxId,
                                 DateTime lastUpdatedDate, string lastUpdatedBy, int? trxLineId, string guid, bool synchStatus,
                                 int masterEntityId, int siteId, string createdBy, DateTime creationDate, int checkInTrxId,
                                 int checkInTrxLineId, List<CheckInPauseLogDTO> checkInPauseLogDTOList,
                                 DateTime? checkInTime , CheckInStatus status,bool isActive)
            : this(checkInDetailId, checkInId, name, cardId, vehicleNumber, vehicleModel, vehicleColor, dateOfBirth,
                                age, specialNeeds, allergies, remarks, checkOutTime, checkOutTrxId, trxLineId, checkInTrxId, checkInTrxLineId,
                                checkInTime, status, isActive)
        {

            log.LogMethodEntry(checkInDetailId, checkInId, name, cardId, vehicleNumber, vehicleModel, vehicleColor, dateOfBirth,
                                age, specialNeeds, allergies, remarks, checkOutTime, checkOutTrxId, lastUpdatedDate, lastUpdatedBy,
                                trxLineId, guid, synchStatus, masterEntityId, siteId, createdBy, creationDate, checkInTrxId, 
                                checkInTrxLineId, checkInPauseLogDTOList, checkInTime, status, isActive);
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.lastUpdatedDate = lastUpdatedDate;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.masterEntityId = masterEntityId;
            this.siteId = siteId;
            if (checkInPauseLogDTOList == null)
                this.checkInPauseLogDTOList = new List<CheckInPauseLogDTO>();
            else
                this.checkInPauseLogDTOList = checkInPauseLogDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CheckInDetailId field
        /// </summary>
        public int CheckInDetailId
        {
            get { return checkInDetailId; }
            set { checkInDetailId = value; this.IsChanged = true; }
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
        /// Get/Set method of the Name field
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; this.IsChanged = true; }
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
        /// Get/Set method of the VehicleNumber field
        /// </summary>
        public string VehicleNumber
        {
            get { return vehicleNumber; }
            set { vehicleNumber = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the VehicleModel field
        /// </summary>
        public string VehicleModel
        {
            get { return vehicleModel; }
            set { vehicleModel = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the VehicleColor field
        /// </summary>
        public string VehicleColor
        {
            get { return vehicleColor; }
            set { vehicleColor = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the DateOfBirth field
        /// </summary>
        public DateTime? DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Age field
        /// </summary>
        public decimal Age
        {
            get { return age; }
            set { age = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SpecialNeeds field
        /// </summary>
        public string SpecialNeeds
        {
            get { return specialNeeds; }
            set { specialNeeds = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Allergies field
        /// </summary>
        public string Allergies
        {
            get { return allergies; }
            set { allergies = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CheckOutTime field
        /// </summary>
        public DateTime? CheckOutTime
        {
            get { return checkOutTime; }
            set { checkOutTime = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CheckOutTrxId field
        /// </summary>
        public int CheckOutTrxId
        {
            get { return checkOutTrxId; }
            set { checkOutTrxId = value; this.IsChanged = true; }
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
        /// Get method of the Detail field
        /// </summary>
        public string Detail
        {
            get { return detail; }
            set { detail = value; }
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
        /// Get/Set method of the CheckInTrxId field
        /// </summary>
        public int CheckInTrxId
        {
            get { return checkInTrxId; }
            set { checkInTrxId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CheckInTrxLineId field
        /// </summary>
        public int CheckInTrxLineId
        {
            get { return checkInTrxLineId; }
            set { checkInTrxLineId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CheckInPauseLogDTOList field
        /// </summary>
        public List<CheckInPauseLogDTO> CheckInPauseLogDTOList
        {
            get { return checkInPauseLogDTOList; }
            set { checkInPauseLogDTOList = value; }
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
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || checkInDetailId < 0;
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
        /// Returns whether the CheckInDetailDTO changed or any of its CheckInPauseLogDTO are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (checkInPauseLogDTOList != null &&
                   checkInPauseLogDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

        public DateTime? CheckInTime
        {
            get { return checkInTime; }
            set { checkInTime = value; this.IsChanged = true; }
        }

        public CheckInStatus Status
        {
            get { return status; }
            set { status = value; this.IsChanged = true; }
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

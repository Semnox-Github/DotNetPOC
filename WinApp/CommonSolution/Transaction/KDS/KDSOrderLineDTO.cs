/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of KDSOrderEntry
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        22-May-2019   Girish Kundar           Created
 *2.140.0     01-Jun-2021   Fiona Lishal           Modified for Delivery Order enhancements for F&B
 ********************************************************************************************/

using System;

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// This is the KDSOrderLineDTO data object class. This acts as data holder for the KDSOrderEntry business object
    /// </summary>
    public class KDSOrderLineDTO
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  KDSOrderEntry ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by  KDSOrderEntry ID LIST field
            /// </summary>
            ID_LIST,
            /// <summary>
            /// Search by TERMINAL ID field
            /// </summary>
            TERMINAL_ID,
            /// <summary>
            /// Search by  LINE ID field
            /// </summary>
            LINE_ID,
            /// <summary>
            /// Search by  TRANSACTION ID field
            /// </summary>
            TRX_ID,
            /// <summary>
            /// Search by  DISPLAY_TEMPLATE ID field
            /// </summary>
            DISPLAY_TEMPLATE_ID,
            /// <summary>
            /// Search by  ORDERED TIME field
            /// </summary>
            ORDERED_TIME,
            /// <summary>
            /// Search by  DELIVERED TIME field
            /// </summary>
            DELIVERED_TIME,
            /// <summary>
            /// Search by  DISPLAY Batch ID field
            /// </summary>
            DISPLAY_BATCH_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by KDSKOT_ENTRY_TYPE field
            /// </summary>
            KDSKOT_ENTRY_TYPE
        }
        private int id;
        private int terminalId;
        private int trxId;
        private int lineId;
        private DateTime? orderedTime;
        private DateTime? deliveredTime;
        private int displayTemplateId;
        private int displayBatchId;
        private DateTime? preparedTime;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private DateTime? prepareStartTime;
        private int productId;
        private string productType;
        private string productName;
        private string productDescription;
        private decimal quantity;
        private string transactionLineRemarks;
        private int parentLineId;
        private DateTime? lineCancelledTime;
        private DateTime? scheduleTime;
        private KDSKOTEntryType entryType;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public KDSOrderLineDTO()
        {
            log.LogMethodEntry();
            id = -1;
            terminalId = -1;
            trxId = -1;
            lineId = -1;
            parentLineId = -1;
            siteId = -1;
            displayTemplateId = -1;
            masterEntityId = -1;
            productId = -1;
            transactionLineRemarks = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public KDSOrderLineDTO(int id, int terminalId, int trxId, int lineId, DateTime? orderedTime, DateTime? deliveredTime,
                                int displayTemplateId, int displayBatchId, DateTime? preparedTime,
                                DateTime? prepareStartTime, int productId, string productName, string productType,
                                string productDescription, decimal quantity, string transactionLineRemarks, int parentLineId, DateTime? lineCancelledTime,DateTime? scheduleTime, KDSKOTEntryType entryType)
            : this()
        {
            log.LogMethodEntry(id, terminalId, trxId, lineId, orderedTime, deliveredTime, displayTemplateId, displayBatchId,
                               preparedTime, prepareStartTime, productId, productName, productType, productDescription, quantity, transactionLineRemarks, 
                               parentLineId, lineCancelledTime, scheduleTime, entryType);
            this.id = id;
            this.terminalId = terminalId;
            this.trxId = trxId;
            this.lineId = lineId;
            this.orderedTime = orderedTime;
            this.deliveredTime = deliveredTime;
            this.displayTemplateId = displayTemplateId;
            this.displayBatchId = displayBatchId;
            this.preparedTime = preparedTime;
            this.prepareStartTime = prepareStartTime;
            this.productId = productId;
            this.productName = productName;
            this.productType = productType;
            this.productDescription = productDescription;
            this.quantity = quantity;
            this.transactionLineRemarks = transactionLineRemarks;
            this.parentLineId = parentLineId;
            this.lineCancelledTime = lineCancelledTime;
            this.scheduleTime = scheduleTime;
            this.entryType = entryType;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public KDSOrderLineDTO(int id, int terminalId, int trxId, int lineId, DateTime? orderedTime, DateTime? deliveredTime,
                                int displayTemplateId, int displayBatchId, DateTime? preparedTime, string guid, bool synchStatus,
                                int siteId, DateTime? prepareStartTime, int productId, string productName,string productType,
                                string productDescription, decimal quantity, string transactionLineRemarks, int parentLineId, DateTime? lineCancelledTime,DateTime? scheduleTime, KDSKOTEntryType entryType, int masterEntityId, string createdBy, DateTime creationDate,
                                string lastUpdatedBy, DateTime lastUpdatedDate)
            : this(id, terminalId, trxId, lineId, orderedTime, deliveredTime, displayTemplateId, displayBatchId,
                               preparedTime, prepareStartTime, productId, productName, productType, productDescription, quantity, transactionLineRemarks, parentLineId, lineCancelledTime, scheduleTime, entryType)
        {
            log.LogMethodEntry(id, terminalId, trxId, lineId, orderedTime, deliveredTime, displayTemplateId, displayBatchId,
                               preparedTime, guid, synchStatus, siteId, prepareStartTime, productId, productName, productType, productDescription, quantity, 
                               transactionLineRemarks, parentLineId, lineCancelledTime, scheduleTime, masterEntityId, createdBy,
                               creationDate, lastUpdatedBy, lastUpdatedDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the TerminalId field
        /// </summary>
        public int TerminalId
        {
            get
            {
                return terminalId;
            }
            set
            {
                terminalId = value;
                IsChanged = true;
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
                trxId = value;
                IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the LineId field
        /// </summary>
        public int LineId
        {
            get
            {
                return lineId;
            }
            set
            {
                lineId = value;
                IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the OrderedTime field
        /// </summary>
        public DateTime? OrderedTime
        {
            get
            {
                return orderedTime;
            }
            set
            {
                orderedTime = value;
                IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the DeliveredTime field
        /// </summary>
        public DateTime? DeliveredTime
        {
            get
            {
                return deliveredTime;
            }
            set
            {
                deliveredTime = value;
                IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the DisplayTemplateId field
        /// </summary>
        public int DisplayTemplateId
        {
            get
            {
                return displayTemplateId;
            }
            set
            {
                displayTemplateId = value;
                IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the DisplayBatchId field
        /// </summary>
        public int DisplayBatchId
        {
            get
            {
                return displayBatchId;
            }
            set
            {
                displayBatchId = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the PreparedTime field
        /// </summary>
        public DateTime? PreparedTime
        {
            get
            {
                return preparedTime;
            }
            set
            {
                preparedTime = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId
        {
            get
            {
                return productId;
            }
            set
            {
                productId = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the productName field
        /// </summary>
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        /// <summary>
        /// Get/Set method of the productDescription field
        /// </summary>
        public string ProductDescription
        {
            get { return productDescription; }
            set { productDescription = value; }
        }

        /// <summary>
        /// Get/Set method of the quantity field
        /// </summary>
        public decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// Get/Set method of the transactionLineRemarks field
        /// </summary>
        public string TransactionLineRemarks
        {
            get { return transactionLineRemarks; }
            set { transactionLineRemarks = value; }
        }

        /// <summary>
        /// Get/Set method of the PrepareStartTime field
        /// </summary>
        public DateTime? PrepareStartTime
        {
            get
            {
                return prepareStartTime;
            }
            set
            {
                prepareStartTime = value;
                IsChanged = true;
            }
        }
        /// <summary>
        ///  Get/Set method of the ScheduleTime field
        /// </summary>
        public DateTime? ScheduleTime
        {
            get
            {
                return scheduleTime;
            }
            set
            {
                scheduleTime = value;
                IsChanged = true;
            }
        }
        /// <summary>
        /// Get method of the LastUpdatedBy field
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
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
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
                guid = value;
                IsChanged = true;
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
                masterEntityId = value;
                IsChanged = true;
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
        ///// <summary>
        ///// Get/Set method of the ActiveFlag field
        ///// </summary>
        //public bool ActiveFlag
        //{
        //    get { return activeFlag; }
        //    set { activeFlag = value; IsChanged = true; }
        //}

        /// <summary>
        /// Get/Set method of the parentLineId field
        /// </summary>
        public int ParentLineId
        {
            get { return parentLineId; }
            set { parentLineId = value; }
        }

        /// <summary>
        /// Get/Set method of the lineCancelledTime field
        /// </summary>
        public DateTime? LineCancelledTime
        {
            get { return lineCancelledTime; }
            set { lineCancelledTime = value; }
        }

        /// <summary>
        /// Get/Set method of the productType field
        /// </summary>
        public string ProductType
        {
            get { return productType; }
            set { productType = value; }
        }
        /// <summary>
        /// Get/Set method of the EntryType field
        /// </summary>
        public KDSKOTEntryType EntryType
        {
            get { return entryType; }
            set { entryType = value; }
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
                    return notifyingObjectIsChanged || id == -1;
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


        /// <summary>
        /// Order Types
        /// </summary>
        public enum KDSKOTEntryType
        { 
            /// <summary>
            /// KDS
            /// </summary>
            KDS,
            /// <summary>
            /// Cancelled
            /// </summary>
            KOT,
        }
        /// <summary>
        /// GetStringFromKDSKOTEntryType
        /// </summary>
        /// <param name="entryTypeValue"></param>
        /// <returns></returns>
        public static string GetStringFromKDSKOTEntryType(KDSKOTEntryType entryTypeValue)
        {
            log.LogMethodEntry(entryTypeValue);
            string returnValue = "KDS";
            switch (entryTypeValue)
            { 
                case KDSKOTEntryType.KOT:
                    returnValue = KDSKOTEntryType.KOT.ToString();
                    break;
                default:
                    returnValue = KDSKOTEntryType.KDS.ToString();
                    break;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// GetKDSKOTEntryTypeFromString
        /// </summary>
        /// <param name="entryTypeValue"></param>
        /// <returns></returns>
        public static KDSKOTEntryType GetKDSKOTEntryTypeFromString(string entryTypeValue)
        {
            log.LogMethodEntry(entryTypeValue);
            KDSKOTEntryType returnValue = KDSKOTEntryType.KDS;
            switch (entryTypeValue)
            { 
                case "KOT":
                    returnValue = KDSKOTEntryType.KOT;
                    break;
                default:
                    returnValue = KDSKOTEntryType.KDS;
                    break;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
    
}

/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of KDSOrderEntry
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        22-May-2019   Lakshminarayana           Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// This is the KDSOrderDTO data object class. This acts as data holder for the KDSOrder business object
    /// </summary>
    public class KDSOrderDTO
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  DISPLAY Batch ID field
            /// </summary>
            DISPLAY_BATCH_ID,
            /// <summary>
            /// Search by  DISPLAY BATCH ID LIST field
            /// </summary>
            DISPLAY_BATCH_ID_LIST,
            /// <summary>
            /// Search by TERMINAL ID field
            /// </summary>
            TERMINAL_ID,
            /// <summary>
            /// Search by  TRANSACTION ID field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search by TABLE_NUMBER field
            /// </summary>
            TABLE_NUMBER,
            /// <summary>
            /// Search by POS_MACHINE_ID field
            /// </summary>
            POS_MACHINE_ID,
            /// <summary>
            /// Search by  DISPLAY_TEMPLATE ID field
            /// </summary>
            DISPLAY_TEMPLATE_ID,
            /// <summary>
            /// Search by  ORDERED TIME field
            /// </summary>
            ORDERED_TIME_EQUAL_TO,
            /// <summary>
            /// Search by PREPARED_TIME field
            /// </summary>
            PREPARED_TIME_GREATER_THAN,
            /// <summary>
            /// Search by DELIVERED_TIME field
            /// </summary>
            DELIVERED_TIME_GREATER_THAN,
            /// <summary>
            /// Search by PREPARED_TIME_NOT_NULL  field
            /// </summary>
            PREPARED_TIME_NOT_NULL,
            /// <summary>
            /// Search by DELIVERED_TIME_NOT_NULL field
            /// </summary>
            DELIVERED_TIME_NOT_NULL,
            /// <summary>
            /// Search by PREPARED_TIME_NULL  field
            /// </summary>
            PREPARED_TIME_NULL,
            /// <summary>
            /// Search by DELIVERED_TIME_NULL field
            /// </summary>
            DELIVERED_TIME_NULL,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by KDSKOT_ENTRY_TYPE field
            /// </summary>
            KDSKOT_ENTRY_TYPE, 
            /// <summary>
            /// Search by HAS_SCHEDULED_TIME  field
            /// </summary>
            HAS_SCHEDULED_TIME,
            /// <summary>
            /// Search by SCHEDULED_TIME_GREATER_THAN field
            /// </summary>
            SCHEDULED_TIME_GREATER_THAN,
            /// <summary>
            /// Search by SCHEDULED_TIME_LESS_THAN field
            /// </summary>
            SCHEDULED_TIME_LESS_THAN,
        }
        private int displayBatchId;
        private int terminalId;
        private int transactionId;
        private DateTime? orderedTime;
        private int displayTemplateId;
        private string tableNumber;
        private string transactionProfileName;
        private string transactionNumber;
        private DateTime? deliveredTime;
        private DateTime? preparedTime;
        private List<KDSOrderLineDTO> kdsOrderLineDtoList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default Constructor
        /// </summary>
        public KDSOrderDTO()
        {
            log.LogMethodEntry();
            terminalId = -1;
            transactionId = -1;
            displayTemplateId = -1;
            displayBatchId = -1;
            kdsOrderLineDtoList = new List<KDSOrderLineDTO>(); 
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public KDSOrderDTO(int displayBatchId, int terminalId, int transactionId, DateTime? orderedTime, int displayTemplateId, string tableNumber, string transactionProfileName,
             string transactionNumber, DateTime? deliveredTime, DateTime? preparedTime)
            :this()
        {
            log.LogMethodEntry(displayBatchId, terminalId, transactionId, orderedTime, displayTemplateId, 
                tableNumber,transactionProfileName, transactionNumber, deliveredTime, preparedTime);
            this.terminalId = terminalId;
            this.transactionId = transactionId;
            this.orderedTime = orderedTime;
            this.displayTemplateId = displayTemplateId;
            this.displayBatchId = displayBatchId;
            this.tableNumber = tableNumber;
            this.transactionProfileName = transactionProfileName;
            this.transactionNumber = transactionNumber;
            this.deliveredTime = deliveredTime;
            this.preparedTime = preparedTime;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Get/Set method of the TerminalId field
        /// </summary>
        public int TerminalId
        {
            get { return terminalId; }
            set { terminalId = value; IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public int TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; IsChanged = true; }
        }
        
        /// <summary>
        /// Get/Set method of the OrderedTime field
        /// </summary>
        public DateTime? OrderedTime
        {
            get { return orderedTime; }
            set { orderedTime = value; IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the TableNumber field
        /// </summary>
        public string TransactionProfileName
        {
            get { return transactionProfileName; }
            set { transactionProfileName = value; IsChanged = true; }
        }


        /// <summary>
        /// Get/Set method of the TableNumber field
        /// </summary>
        public string TransactionNumber
        {
            get { return transactionNumber; }
            set { transactionNumber = value; IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the TableNumber field
        /// </summary>
        public string TableNumber
        {
            get { return tableNumber; }
            set { tableNumber = value; IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the PreparedTime field
        /// </summary>
        public DateTime? PreparedTime
        {
            get { return preparedTime; }
            set { preparedTime = value; IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the DeliveredTime field
        /// </summary>
        public DateTime? DeliveredTime
        {
            get { return deliveredTime; }
            set { deliveredTime = value; IsChanged = true; }
        }
        
        /// <summary>
        /// Get/Set method of the DisplayTemplateId field
        /// </summary>
        public int DisplayTemplateId
        {
            get { return displayTemplateId; }
            set { displayTemplateId = value; IsChanged = true; }
        }
        
        /// <summary>
        /// Get/Set method of the DisplayBatchId field
        /// </summary>
        public int DisplayBatchId
        {
            get { return displayBatchId; }
            set { displayBatchId = value; IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the KDSOrderLineDtoList field
        /// </summary>
        public List<KDSOrderLineDTO> KDSOrderLineDtoList
        {
            get { return kdsOrderLineDtoList; }
            set { kdsOrderLineDtoList = value;}
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
                    if (!bool.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Returns whether KDSOrder or any child record is changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (kdsOrderLineDtoList != null &&
                    kdsOrderLineDtoList.Any(x => x.IsChanged))
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

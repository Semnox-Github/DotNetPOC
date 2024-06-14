/********************************************************************************************
 * Project Name - OrderHeaderDTO
 * Description  - OrderHeaderDTO  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.110.0     01-Feb-2021    Girish Kundar       Modified : Urban Piper changes
 *2.130.0     01-Jun-2021    Fiona Lishal        Modified for Delivery Order enhancements for F&B
  *2.140.0     01-Dec-2021    Girish Kundar       Modified : View Open order Ui changes
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Semnox.Parafait.Transaction.Order;

namespace Semnox.Parafait.Transaction
{
    public class OrderHeaderDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by orderId
            /// </summary>
            ORDER_ID,
            /// <summary>
            /// Search by transactionId
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search by tableId
            /// </summary>
            TABLE_ID,
            /// <summary>
            /// Search by transactionNumber
            /// </summary>
            TRANSACTION_NUMBER,
            /// <summary>
            /// Search by reservationCode
            /// </summary>
            RESERVATION_CODE,
            /// <summary>
            /// Search by phoneNumber
            /// </summary>
            PHONE_NUMBER,
            /// <summary>
            /// Search by CustomerName
            /// </summary>
            CUSTOMER_NAME,
            /// <summary>
            /// Search by TableNumber
            /// </summary>
            TABLE_NUMBER,
            /// <summary>
            /// Search by CardNumber
            /// </summary>
            CARD_NUMBER,
            /// <summary>
            /// Search by isactive
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by transaction Status
            /// </summary>
            TRANSACTION_STATUS,
            /// <summary>
            /// Search by transaction Status list
            /// </summary>
            TRANSACTION_STATUS_LIST,
            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE,
            /// <summary>
            /// Search by LAST ORDER_GUID field
            /// </summary>
            ORDER_GUID

        }

        private int orderId;
        private string tableNumber;
        private string facilityTableNumber;
        private string customerName;
        private string waiterName;
        private string remarks;
        private OrderStatus status;
        private int pOSMachineId;
        private string pOSMachineName;
        private int userId;
        private DateTime orderDate;
        private int cardId;
        private string cardNumber;
        private int tableId;
        private int guestCount;
        private decimal amount;
        private int orderStatusId;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int transactionOrderTypeId;
        private List<int> transactionIdList; 
        private string reservationCode; 
        private string trxNumber;
        private List<OrderDetailDTO> orderDetailsDTOList;
        private List<string> transactionNoList;
        public OrderHeaderDTO()
        {
            log.LogMethodEntry();
            orderId = -1;
            pOSMachineId = -1;
            userId = -1;
            cardId = -1;
            tableId = -1;
            orderStatusId = -1;
            isActive = true;
            masterEntityId = -1;
            transactionOrderTypeId = -1;
            tableNumber = string.Empty;
            facilityTableNumber = string.Empty;
            status = OrderStatus.OPEN;
            transactionIdList = new List<int>();
            transactionNoList = new List<string>(); 
            trxNumber = string.Empty;
            reservationCode = string.Empty;
            log.LogMethodExit();
        }

        public OrderHeaderDTO(int orderId, string tableNumber, string facilityTableNumber, string customerName, string waiterName,
                            string remarks, OrderStatus status, int pOSMachineId, string pOSMachineName,
                            int userId, DateTime orderDate, int cardId, string cardNumber, decimal amount, int tableId,
                            int guestCount, int orderStatusId, bool isActive, DateTime creationDate, string createdBy,
                            DateTime lastUpdateDate, string lastUpdatedBy, int site_id, string guid,
                            bool synchStatus, int masterEntityId, int transactionOrderTypeId, string reservationCode)
        {
            log.LogMethodEntry(orderId, tableNumber, facilityTableNumber, customerName,
                               waiterName, remarks, status, pOSMachineId, pOSMachineName, userId, orderDate,
                               cardId, cardNumber, amount, tableId, guestCount, orderStatusId, isActive,
                               creationDate, createdBy, lastUpdateDate,lastUpdatedBy, site_id, guid, synchStatus,
                               masterEntityId, transactionOrderTypeId, reservationCode);
            this.orderId = orderId;
            this.tableNumber = tableNumber;
            this.facilityTableNumber = facilityTableNumber;
            this.customerName = customerName;
            this.waiterName = waiterName;
            this.remarks = remarks;
            this.status = status;
            this.pOSMachineId = pOSMachineId;
            this.pOSMachineName = pOSMachineName;
            this.userId = userId;
            this.orderDate = orderDate;
            this.cardId = cardId;
            this.cardNumber = cardNumber;
            this.amount = amount;
            this.tableId = tableId;
            this.guestCount = guestCount;
            this.orderStatusId = orderStatusId;
            this.isActive = isActive;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.transactionOrderTypeId = transactionOrderTypeId;
            this.reservationCode = reservationCode;
            transactionIdList = new List<int>();
            transactionNoList = new List<string>();
            log.LogMethodExit();
        }

        

        /// <summary>
        /// Get/Set method of the orderId field
        /// </summary>
        [Browsable(false)]
        public int OrderId
        {
            get
            {
                return orderId;
            }
            set
            {
                orderId = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the tableNumber field
        /// </summary>
        [Browsable(false)]
        public string TableNumber
        {
            get
            {
                return tableNumber;
            }
            set
            {
                tableNumber = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the facilityTableNumber field
        /// </summary>
        [DisplayName("Table#")]
        public string FacilityTableNumber
        {
            get
            {
                return facilityTableNumber;
            }
            set
            {
                facilityTableNumber = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the waiterName field
        /// </summary>
        [DisplayName("Waiter")]
        public string WaiterName
        {
            get
            {
                return waiterName;
            }
            set
            {
                waiterName = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the customerName field
        /// </summary>
        [DisplayName("Customer")]
        public string CustomerName
        {
            get
            {
                return customerName;
            }
            set
            {
                customerName = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the card number field
        /// </summary>
        [DisplayName("Card Number")]
        public string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                cardNumber = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the pOSMachineName field
        /// </summary>
        [DisplayName("POS")]
        public string POSMachineName
        {
            get
            {
                return pOSMachineName;
            }
            set
            {
                pOSMachineName = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the orderDate field
        /// </summary>
        [DisplayName("Date")]
        public DateTime OrderDate
        {
            get
            {
                return orderDate;
            }
            set
            {
                orderDate = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the amount field
        /// </summary>
        [DisplayName("Amount")]
        public decimal Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get method to list all the transaction id of the order
        /// </summary>
        [DisplayName("TrxId(s)")]
        public string TransactionIDs
        {
            get
            {
                StringBuilder trxids = new StringBuilder();
                string joiner = string.Empty;
                foreach (var transactionId in transactionIdList)
                {
                    trxids.Append(joiner);
                    trxids.Append(transactionId.ToString());
                    joiner = ", ";
                }
                return trxids.ToString();
            }
        } 

        /// <summary>
        /// Get method to list all the transaction id of the order
        /// </summary>
        [DisplayName("TrxNo")]
        public string TrxNo
        {
            get
            {
                StringBuilder trxNos = new StringBuilder();
                string joiner = string.Empty;
                foreach (var transactionNo in transactionNoList)
                {
                    trxNos.Append(joiner);
                    trxNos.Append(transactionNo.ToString());
                    joiner = ", ";
                }
                return trxNos.ToString();
            }
        }
        /// <summary>
        /// Get/Set method of the remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks
        {
            get
            {
                return remarks;
            }
            set
            {
                remarks = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the status field
        /// </summary>
        [Browsable(false)]
        public OrderStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the pOSMachineId field
        /// </summary>
        [Browsable(false)]
        public int POSMachineId
        {
            get
            {
                return pOSMachineId;
            }
            set
            {
                pOSMachineId = value;
                this.IsChanged = true;
            }
        }




        /// <summary>
        /// Get/Set method of the userId field
        /// </summary>
        [Browsable(false)]
        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the cardId field
        /// </summary>
        [Browsable(false)]
        public int CardId
        {
            get
            {
                return cardId;
            }
            set
            {
                cardId = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the tableId field
        /// </summary>
        [Browsable(false)]
        public int TableId
        {
            get
            {
                return tableId;
            }
            set
            {
                tableId = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the guestCount field
        /// </summary>
        [Browsable(false)]
        public int GuestCount
        {
            get
            {
                return guestCount;
            }
            set
            {
                guestCount = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the OrderStatusId field
        /// </summary>
        public int OrderStatusId
        {
            get
            {
                return orderStatusId;
            }
            set
            {
                orderStatusId = value;
                this.IsChanged = true;
            }
        }


        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        [Browsable(false)]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                this.IsChanged = true;
                lastUpdateDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                this.IsChanged = true;
                lastUpdatedBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return site_id;
            }
            set
            {
                this.IsChanged = true;
                site_id = value;
            }
        }
        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set method of the synchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                this.IsChanged = true;
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set method of the TransactionOrderTypeId field
        /// </summary>
        [Browsable(false)]
        public int TransactionOrderTypeId
        {
            get
            {
                return transactionOrderTypeId;
            }
            set
            {
                transactionOrderTypeId = value;
                this.IsChanged = true;
            }
        }


        /// <summary>
        /// Get/Set method of the reservationCode field
        /// </summary>
        [DisplayName("Reservation Code")]
        public string ReservationCode
        {
            get
            {
                return reservationCode;
            }
            set
            {
                reservationCode = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the transactionIdList field
        /// </summary>
        [Browsable(false)]
        public List<int> TransactionIdList
        {
            get
            {
                return transactionIdList;
            }
            set
            {
                transactionIdList = value;
            }
        } 
        /// <summary>
        /// Get/Set method of the transactionNoList field
        /// </summary>
        [Browsable(false)]
        public List<string> TransactionNoList
        {
            get
            {
                return transactionNoList;
            }
            set
            {
                transactionNoList = value;
            }
        }
        /// <summary>
        /// Get/Set Method for select field
        /// Used in orderlistview for row selection 
        /// will not be saved to the db.
        /// </summary>
        public bool Selected
        {
            get; set;
        }

        public List<OrderDetailDTO> OrderDetailsDTOList { get { return orderDetailsDTOList; } set { orderDetailsDTOList = value; } }
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
                    return notifyingObjectIsChanged || orderId < 0;
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
        /// Returns whether the OrderHeader changed or any of its Child  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (orderDetailsDTOList != null &&
                   orderDetailsDTOList.Any(x => x.IsChanged))
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
            this.IsChanged = false;
            log.LogMethodExit();
        }

    }
}

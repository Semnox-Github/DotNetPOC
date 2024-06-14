/********************************************************************************************
 * Project Name - POS
 * Description  - Data object of ShiftLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        27-May-2019   Divya A                 Created
 *2.90        26-May-2020   Vikas Dwivedi           Modified as per the Standard CheckList
 *2.140.0     16-Aug-2021   Deeksha                 Modified : Provisional Shift changes
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    public class ShiftLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByShiftParameters
        {
            /// <summary>
            /// Search by SHIFT_KEY field
            /// </summary>
            SHIFT_KEY,
            /// <summary>
            /// Search by SHIFT_KEY_LIST field
            /// </summary>
            SHIFT_KEY_LIST,
            /// <summary>
            /// Search by SHIFT_LOGIN_ID field
            /// </summary>
            SHIFT_LOG_ID,
            ///<summary>
            ///Search by SHIFT_ACTION field
            ///</summary>
            SHIFT_ACTION,
            ///<summary>
            ///Search by SHIFT_TIME field
            ///</summary>
            SHIFT_FROM_TIME,
            ///<summary>
            ///Search by SHIFT_TIME field
            ///</summary>
            SHIFT_TO_TIME,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int shiftLogId;
        private int shiftKey;
        private DateTime shiftTime;
        private string shiftAction;
        private double? shiftAmount;
        private int? cardCount;
        private int? shiftTicketNumber;
        private string shiftRemarks;
        private decimal? actualAmount;
        private int? actualCards;
        private int? actualTickets;
        private decimal? gameCardAmount;
        private decimal? creditCardamount;
        private decimal? chequeAmount;
        private decimal? couponAmount;
        private decimal? actualGameCardamount;
        private decimal? actualCreditCardamount;
        private decimal? actualChequeAmount;
        private decimal? actualCouponAmount;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string approverId;
        private DateTime? approvalTime;
        private string shiftReason;
        private string externalReference;
        private int shiftId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public ShiftLogDTO()
        {
            log.LogMethodEntry();
            shiftLogId = -1;
            shiftKey = -1;
            siteId = -1;
            masterEntityId = -1;
            gameCardAmount = null;
            creditCardamount= null;
            chequeAmount= null;
            couponAmount= null;
            actualGameCardamount= null;
            actualCreditCardamount= null;
            actualChequeAmount= null;
            actualCouponAmount= null;
            shiftAmount = null;
            cardCount = null;
            shiftTicketNumber = null;
            actualAmount = null;
            actualCards = null;
            actualTickets = null;
            approvalTime = null;
            externalReference = null;
            shiftId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields.
        /// </summary>
        public ShiftLogDTO(int shiftLogId, int shiftKey, DateTime shiftTime, string shiftAction, double? shiftAmount, 
                        int? cardCount, int? shiftTicketNumber, string shiftRemarks, decimal? actualAmount, int? actualCards, 
                        int? actualTickets, decimal? gameCardAmount, decimal? creditCardamount, decimal? chequeAmount, 
                        decimal? couponAmount, decimal? actualGameCardamount, decimal? actualCreditCardamount, decimal? actualChequeAmount, 
                        decimal? actualCouponAmount, string approverId, DateTime? approvalTime, string shiftReason,string externalReference,
                        int shiftId)
            : this()
        {
            log.LogMethodEntry(shiftLogId, shiftKey, shiftTime, shiftAction, shiftAmount, cardCount, shiftTicketNumber, shiftRemarks,
                  actualAmount, actualCards, actualTickets, gameCardAmount, creditCardamount, chequeAmount, couponAmount,
                  actualGameCardamount, actualCreditCardamount, actualChequeAmount, actualCouponAmount, approverId, approvalTime,
                  shiftReason, externalReference, shiftId);
            this.shiftLogId = shiftLogId;
            this.shiftKey = shiftKey;
            this.shiftTime = shiftTime;
            this.shiftAction  = shiftAction;
            this.shiftAmount = shiftAmount;
            this.cardCount = cardCount;
            this.shiftTicketNumber = shiftTicketNumber;
            this.shiftRemarks = shiftRemarks;
            this.actualAmount = actualAmount;
            this.actualCards = actualCards;
            this.actualTickets = actualTickets;
            this.gameCardAmount = gameCardAmount;
            this.creditCardamount = creditCardamount;
            this.chequeAmount = chequeAmount;
            this.couponAmount = couponAmount;
            this.actualGameCardamount = actualGameCardamount;
            this.actualCreditCardamount = actualCreditCardamount;
            this.actualChequeAmount = actualChequeAmount;
            this.actualCouponAmount = actualCouponAmount;
            this.approverId = approverId;
            this.approvalTime = approvalTime;
            this.shiftReason = shiftReason;
            this.externalReference = externalReference;
            this.shiftId = shiftId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields.
        /// </summary>
        public ShiftLogDTO(int shiftLogId, int shiftKey, DateTime shiftTime, string shiftAction, double? shiftAmount, 
                    int? cardCount, int? shiftTicketNumber, string shiftRemarks, decimal? actualAmount, int? actualCards, 
                    int? actualTickets, decimal? gameCardAmount, decimal? creditCardamount, decimal? chequeAmount, decimal? couponAmount, 
                    decimal? actualGameCardamount, decimal? actualCreditCardamount, decimal? actualChequeAmount, 
                    decimal? actualCouponAmount, string guid, int siteId, bool synchStatus, int masterEntityId, 
                    string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, 
                    string approverId, DateTime? approvalTime, string shiftReason,string externalReference,int shiftId)
            : this(shiftLogId, shiftKey, shiftTime, shiftAction, shiftAmount, cardCount, shiftTicketNumber, shiftRemarks,
                  actualAmount, actualCards, actualTickets, gameCardAmount, creditCardamount, chequeAmount, couponAmount,
                  actualGameCardamount, actualCreditCardamount, actualChequeAmount, actualCouponAmount, approverId, approvalTime, shiftReason, externalReference, shiftId)
        {
            log.LogMethodEntry(shiftLogId, shiftKey, shiftTime, shiftAction, shiftAmount, cardCount, shiftTicketNumber, shiftRemarks,
                  actualAmount, actualCards, actualTickets, gameCardAmount, creditCardamount, chequeAmount, couponAmount,
                  actualGameCardamount, actualCreditCardamount, actualChequeAmount, actualCouponAmount, guid, siteId, synchStatus, 
                  masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, approverId, approvalTime, shiftReason, externalReference);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ShiftLogId field
        /// </summary>        
        public int ShiftLogId { get { return shiftLogId; } set { shiftLogId = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShiftKey field
        /// </summary>
        public int ShiftKey { get { return shiftKey; } set { shiftKey = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShiftId field
        /// </summary>
        public int ShiftId { get { return shiftId; } set { shiftId = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShiftTime field
        /// </summary>
        public DateTime ShiftTime { get { return shiftTime; } set { shiftTime = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShiftAction field
        /// </summary>
        public string ShiftAction { get { return shiftAction; } set { shiftAction = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShiftAmount field
        /// </summary>
        public double? ShiftAmount { get { return shiftAmount; } set { shiftAmount = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardCount field
        /// </summary>
        public int? CardCount { get { return cardCount; } set { cardCount = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShiftTicketNumber field
        /// </summary>
        public int? ShiftTicketNumber { get { return shiftTicketNumber; } set { shiftTicketNumber = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShiftRemarks field
        /// </summary>
        public string ShiftRemarks { get { return shiftRemarks; } set { shiftRemarks = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActualAmount field
        /// </summary>
        public decimal? ActualAmount { get { return actualAmount; } set { actualAmount = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActualCards field
        /// </summary>
        public int? ActualCards { get { return actualCards; } set { actualCards = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActualTickets field
        /// </summary>
        public int? ActualTickets { get { return actualTickets; } set { actualTickets = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GameCardAmount field
        /// </summary>
        public decimal? GameCardAmount { get { return gameCardAmount; } set { gameCardAmount = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreditCardamount field
        /// </summary>
        public decimal? CreditCardamount { get { return creditCardamount; } set { creditCardamount = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ChequeAmount field
        /// </summary>
        public decimal? ChequeAmount { get { return chequeAmount; } set { chequeAmount = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CouponAmount field
        /// </summary>
        public decimal? CouponAmount { get { return couponAmount; } set { couponAmount = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActualGameCardamount field
        /// </summary>
        public decimal? ActualGameCardamount { get { return actualGameCardamount; } set { actualGameCardamount = value; IsChanged = true;} }

        /// <summary>
        /// Get/Set method of the ActualCreditCardamount field
        /// </summary>
        public decimal? ActualCreditCardAmount { get { return actualCreditCardamount; } set { actualCreditCardamount = value; IsChanged = true;} }

        /// <summary>
        /// Get/Set method of the ActualChequeAmount field
        /// </summary>
        public decimal? ActualChequeAmount { get { return actualChequeAmount; } set { actualChequeAmount = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActualCouponAmount field
        /// </summary>
        public decimal? ActualCouponAmount { get { return actualCouponAmount; } set { actualCouponAmount = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value;} }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the ApproverID field
        /// </summary>
        public string ApproverID { get { return approverId; } set { approverId = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ApprovalTime field
        /// </summary>
        public DateTime? ApprovalTime { get { return approvalTime; } set { approvalTime = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShiftReason field
        /// </summary>
        public string ShiftReason { get { return shiftReason; } set { shiftReason = value; IsChanged = true; } }
        public string ExternalReference { get { return externalReference; } set { externalReference = value; IsChanged = true; } }

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
                    return notifyingObjectIsChanged || shiftLogId < 0;
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

    }
}
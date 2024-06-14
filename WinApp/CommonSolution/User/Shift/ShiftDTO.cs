/********************************************************************************************
 * Project Name - Shift DTO
 * Description  - Data object of Shift
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        04-Mar-2019   Indhu          Created 
 *2.60        21-May-2019   Nitin Pai      Added search by timestamp field
 *2.90        26-May-2020   Vikas Dwivedi  Modified as per the Standard CheckList
 *2.140.0     16-Aug-2021   Deeksha        Modified : Provisional Shift changes
 *2.140.0     16-Aug-2021   Girish         Modified : Multicash drawer changes, Added cashdrawerId column
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public class ShiftDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

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
            /// Search by POS_MACHINE field
            /// </summary>
            POS_MACHINE,
            /// <summary>
            /// Search by SHIFT_LOGIN_ID field
            /// </summary>
            SHIFT_LOGIN_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            ///<summary>
            ///Search by SHIFT_USERNAME field
            ///</summary>
            SHIFT_USERNAME,
            ///<summary>
            ///Search by SHIFT_USERTYPE field
            ///</summary>
            SHIFT_USERTYPE,
            ///<summary>
            ///Order by ShiftTime field
            ///</summary>
            ORDER_BY_TIMESTAMP,
            ///<summary>
            ///Filter by ShiftTime field
            ///</summary>
            TIMESTAMP,
            ///<summary>
            ///Filter by ShiftTime field
            ///</summary>
            SHIFT_FROM_TIME,
            ///<summary>
            ///Filter by ShiftTime field
            ///</summary>
            SHIFT_TO_TIME,
            ///<summary>
            ///Filter by ShiftAction field
            ///</summary>
            SHIFT_ACTION,
            /// <summary>
            /// Search by Last X days Login
            /// </summary>
            LAST_X_DAYS_LOGIN,
            // <summary>
            /// Search by CASHDRAWER_ID
            /// </summary>
            CASHDRAWER_ID,
            // <summary>
            /// Search by SHIFT_ACTION_IN
            /// </summary>
            SHIFT_ACTION_IN
        }
        /// <summary>
        /// ShiftActionType enum
        /// </summary>
        /// 
        public enum ShiftActionType
        {
            ///<summary>
            ///Open
            ///</summary>
            [Description("Open")] Open,
            ///<summary>
            ///Close
            ///</summary>
            [Description("Close")] Close,
            ///<summary>
            ///BClose
            ///</summary>
            [Description("PClose")] PClose,
            ///<summary>
            ///ROpen
            ///</summary>
            [Description("ROpen")] ROpen
        }

        private int shiftKey;
        private string shiftUsername;
        private DateTime shiftTime;
        private string shiftUsertype;
        private string shiftAction;
        private double shiftAmount;
        private double cardCount;
        private string shiftTicketnumber;
        private string shiftRemarks;
        private string posMachine;
        private double actualAmount;
        private double? actualCards;
        private double actualTickets;
        private double gameCardamount;
        private double creditCardamount;
        private double chequeAmount;
        private double couponAmount;
        private double actualGameCardamount;
        private double actualCreditCardamount;
        private double actualChequeAmount;
        private double actualCouponAmount;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string shiftLoginId;
        private List<ShiftLogDTO> shiftLogDTOList;
        private string receipt;
        private int cashdrawerId;

        public ShiftDTO()
        {
            log.LogMethodEntry();
            shiftKey = -1;
            siteId = -1;
            masterEntityId = -1;
            cashdrawerId = -1;
            actualCards = null;
            shiftLogDTOList = new List<ShiftLogDTO>();
            log.LogMethodExit();
        }

        ///<summary>
        ///Parameterized constructor
        ///</summary>
        public ShiftDTO(int shiftKey,string shiftUsername, DateTime shiftTime,string shiftUsertype,
                        string shiftAction,double shiftAmount,double cardCount,string shiftTicketnumber,
                        string shiftRemarks,string posMachine,double actualAmount, double? actualCards,
                        double actualTickets,double gameCardamount,double creditCardamount,
                        double chequeAmount,double couponAmount,double actualGameCardamount,
                        double actualCreditCardamount,double actualChequeAmount,double actualCouponAmount,
                          string shiftLoginId, int cashdrawerId)
         : this()
        {
            this.shiftKey = shiftKey;
            this.shiftUsername = shiftUsername;
            this.shiftTime = shiftTime;
            this.shiftUsertype = shiftUsertype;
            this.shiftAction = shiftAction;
            this.shiftAmount = shiftAmount;
            this.cardCount = cardCount;
            this.shiftTicketnumber = shiftTicketnumber;
            this.shiftRemarks = shiftRemarks;
            this.posMachine = posMachine;
            this.actualAmount = actualAmount;
            this.actualCards = actualCards;
            this.actualTickets = actualTickets;
            this.gameCardamount = gameCardamount;
            this.creditCardamount = creditCardamount;
            this.chequeAmount = chequeAmount;
            this.couponAmount = couponAmount;
            this.actualGameCardamount = actualGameCardamount;
            this.actualCreditCardamount = actualCreditCardamount;
            this.actualChequeAmount = actualChequeAmount;
            this.actualCouponAmount = actualCouponAmount;
            this.shiftLoginId = shiftLoginId;
            this.cashdrawerId = cashdrawerId;
        }

        ///<summary>
        ///Parameterized constructor
        ///</summary>
        public ShiftDTO(int shiftKey, string shiftUsername, DateTime shiftTime, string shiftUsertype,
                      string shiftAction, double shiftAmount, double cardCount, string shiftTicketnumber,
                      string shiftRemarks, string posMachine, double actualAmount, double? actualCards,
                      double actualTickets, double gameCardamount, double creditCardamount,
                      double chequeAmount, double couponAmount, double actualGameCardamount,
                      double actualCreditCardamount, double actualChequeAmount, double actualCouponAmount,
                      string guid, int siteId, bool synchStatus, int masterEntityId, string createdBy,
                      DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                      string shiftLoginId, int cashdrawerId)
          : this(shiftKey, shiftUsername, shiftTime, shiftUsertype, shiftAction, shiftAmount, cardCount, shiftTicketnumber,
                 shiftRemarks, posMachine, actualAmount, actualCards, actualTickets, gameCardamount, creditCardamount, chequeAmount,
                 couponAmount, actualGameCardamount, actualCreditCardamount, actualChequeAmount, actualCouponAmount, shiftLoginId, cashdrawerId)
        {
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
        }

        /// <summary>
        /// Get/Set method of the Shift Key field
        /// </summary>
        [DisplayName("Shift Key")]
        [ReadOnly(true)]
        public int ShiftKey { get { return shiftKey; } set { shiftKey = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ShiftUserName field
        /// </summary>
        [DisplayName("Shift User Name")]
        public string ShiftUserName { get { return shiftUsername; } set { shiftUsername = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ShiftTime field
        /// </summary>
        [DisplayName("Shift Time")]
        public DateTime ShiftTime { get { return shiftTime; } set { shiftTime = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the ShiftUsertype field
        /// </summary>
        [DisplayName("Shift User Type")]
        public string ShiftUserType { get { return shiftUsertype; } set { shiftUsertype = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShiftAction field
        /// </summary>
        [DisplayName("Shift Action")]
        public string ShiftAction { get { return shiftAction; } set { shiftAction = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShiftAmount field
        /// </summary>
        [DisplayName("Shift Amount")]
        public double ShiftAmount { get { return shiftAmount; } set { shiftAmount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardCount field
        /// </summary>
        [DisplayName("Card Count")]
        public double CardCount { get { return cardCount; } set { cardCount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShiftTicketnumber field
        /// </summary>
        [DisplayName("Shift Ticket Number")]
        public string ShiftTicketNumber { get { return shiftTicketnumber; } set { shiftTicketnumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShiftRemarks field
        /// </summary>
        [DisplayName("Shift Remarks")]
        public string ShiftRemarks { get { return shiftRemarks; } set { shiftRemarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the POSMachine field
        /// </summary>
        [DisplayName("POS Machine")]
        public string POSMachine { get { return posMachine; } set { posMachine = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the actualAmount field
        /// </summary>
        [DisplayName("Actual Amount")]
        public double ActualAmount { get { return actualAmount; } set { actualAmount = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ActualCards field
        /// </summary>
        [DisplayName("Actual Cards")]
        public double? ActualCards { get { return actualCards; } set { actualCards = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActualTickets field
        /// </summary>
        [DisplayName("Actual Tickets")]
        public double ActualTickets { get { return actualTickets; } set { actualTickets = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the gameCardamount field
        /// </summary>
        [DisplayName("Game Card amount")]
        public double GameCardamount { get { return gameCardamount; } set { gameCardamount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreditCardamount field
        /// </summary>
        [DisplayName("CreditCard amount")]
        public double CreditCardamount { get { return creditCardamount; } set { creditCardamount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ChequeAmount field
        /// </summary>
        [DisplayName("Cheque Amount")]
        public double ChequeAmount { get { return chequeAmount; } set { chequeAmount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CouponAmount field
        /// </summary>
        [DisplayName("Coupon Amount")]
        public double CouponAmount { get { return couponAmount; } set { couponAmount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActualGameCardamount field
        /// </summary>
        [DisplayName("Actual GameCard amount")]
        public double ActualGameCardamount { get { return actualGameCardamount; } set { actualGameCardamount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActualCreditCardamount field
        /// </summary>
        [DisplayName("Actual CreditCard Amount")]
        public double ActualCreditCardamount { get { return actualCreditCardamount; } set { actualCreditCardamount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActualChequeAmount field
        /// </summary>
        [DisplayName("Actual Cheque Amount")]
        public double ActualChequeAmount { get { return actualChequeAmount; } set { actualChequeAmount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActualCouponAmount field
        /// </summary>
        [DisplayName("Actual Coupon Amount")]
        public double ActualCouponAmount { get { return actualCouponAmount; } set { actualCouponAmount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Shift Login Id field
        /// </summary>
        [DisplayName("Shift Login Id")]
        public string ShiftLoginId { get { return shiftLoginId; } set { shiftLoginId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdateDate; } set { lastUpdateDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the List of ShiftLogDTO
        /// </summary>
        [DisplayName("ShiftLogDTO List")]
        [Browsable(false)]
        public List<ShiftLogDTO> ShiftLogDTOList { get { return shiftLogDTOList; } set { shiftLogDTOList = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the  field
        /// </summary>
        [DisplayName("Receipt")]
        public string Receipt { get { return receipt; } set { receipt = value; } }

        [DisplayName("CashdrawerId")]
        public int CashdrawerId { get { return cashdrawerId; } set { cashdrawerId = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || shiftKey < 0;
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
        /// Returns true or false whether the ShiftDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (shiftLogDTOList != null &&
                   shiftLogDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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

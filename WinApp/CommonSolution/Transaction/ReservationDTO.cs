/********************************************************************************************
 * Project Name - Reservations
 * Description  - DTO class for reservations
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      28-Nov-2018      Guru S A       Booking enhancements
 *2.70        25-Mar-2019      Guru S A       Booking Phase 2 enhancements
 *2.110.00    25-Nov-2020      Girish Kundar  Modified:  Paymemnt link enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    public class ReservationDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by BOOKING_ID field
            /// </summary>
            BOOKING_ID,
            /// <summary>
            /// Search by TRX_ID field
            /// </summary>
            TRX_ID,
            /// <summary>
            /// Search by STATUS field
            /// </summary>
            STATUS,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by STATUS list field
            /// </summary>
            STATUS_LIST_IN,
            /// <summary>
            /// Search by STATUS list field
            /// </summary>
            STATUS_LIST_NOT_IN,
            /// <summary>
            /// Search by bookingProductId field
            /// </summary>
            BOOKING_PRODUCT_ID,
            /// <summary>
            /// Search by FACILITY_MAP_ID  field
            /// </summary>
            FACILITY_MAP_ID,
            /// <summary>
            /// Search by card number field
            /// </summary>
            CARD_NUMBER_LIKE,
            /// <summary>
            /// Search by Reservation code field
            /// </summary>
            RESERVATION_CODE_LIKE,
            /// <summary>
            /// Search by from date field
            /// </summary>
            RESERVATION_FROM_DATE,
            /// <summary>
            /// Search by to date field
            /// </summary>
            RESERVATION_TO_DATE,
            /// <summary>
            /// Search by customer name field
            /// </summary>
            CUSTOMER_NAME_LIKE,
            /// <summary>
            /// Search by Reservation code field
            /// </summary>
            RESERVATION_CODE_EXACT,
            /// <summary>
            /// Search by CHECKLIST_TASK_ASSIGNEE_ID field
            /// </summary>
            CHECKLIST_TASK_ASSIGNEE_ID,
            /// <summary>
            /// Search by TRANSACTION_GUID field
            /// </summary>
            TRANSACTION_GUID
        }

        public enum ReservationStatus
        {
            BOOKED,
            BLOCKED,
            WIP,
            CANCELLED,
            COMPLETE,
            CONFIRMED,
            NEW,
            SYSTEMABANDONED
        };

        public enum DefaultBookingSetup
        {
            DEFAULT_BOOKINGS_CHANNEL,
            BOOKING_PRINT_TYPE,
            FIXED_SCHEDULE_BOOKING_GRACE_PERIOD,
            BLOCK_BOOKING_FOR_X_MINUTES,
            CALENDAR_TIME_SLOT_GAP
        }
        public class SelectedCategoryProducts
        {
            public int parentComboProductId;
            public int productId;
            public int quantity;
            public double productPrice;
        }

        private int bookingId;
        private int bookingClassId;
        private string bookingName;
        private DateTime fromDate;
        private string recur_flag;
        private string recur_frequency;
        private DateTime? recur_end_date;
        private int quantity;
        private string reservationCode;
        private string status;
        private int cardId;
        private string cardNumber;
        private int customerId;
        private string customerName;
        private DateTime? expiryTime;
        private string channel;
        private string remarks;
        private string contactNo;
        private string alternateContactNo;
        private string email;
        private int isEmailSent;
        private DateTime toDate;
        private int trxId;
        private int age;
        private string gender;
        private string postalAddress;
        private int bookingProductId;
        private int attractionScheduleId;
        private int extraGuests;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private string trxNumber;
        private string trxStatus;
        private decimal? trxNetAmount;
        //int facilityId;
        private string facilityName;
        private int facilityMapId;
        private string facilityMapName;
        private string bookingProductName;
        private double serviceChargeAmount;
        private double serviceChargePercentage;
        //private int eventHostId;
        // private string eventHostName;
        //private string checkListTaskGroupName;
        // private int checkListTaskGroupId;
        //private List<BookingAttendeeDTO> bookingAttendeeList;
        private List<BookingCheckListDTO> bookingCheckListDTOList;

        bool notifyingObjectIsChanged;
        readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReservationDTO()
        {
            log.LogMethodEntry();
            bookingId = -1;
            bookingClassId = -1;
            bookingProductId = -1;
            attractionScheduleId = -1;
            trxId = -1;
            customerId = -1;
            cardId = -1;
            siteId = -1;
            recur_flag = "N";
            //eventHostId = -1;
            //checkListTaskGroupId = -1;
            masterEntityId = -1;
            //facilityId = -1;
            facilityMapId = -1;
            status = ReservationDTO.ReservationStatus.NEW.ToString();
            bookingCheckListDTOList = new List<BookingCheckListDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor With Parameter
        /// </summary>
        public ReservationDTO(int bookingId, string bookingName, DateTime fromDate, int age, string gender,
                            string contactNo, string email,
                            int siteId, string status)
        {
            log.LogMethodEntry(bookingId, bookingName, fromDate, age, gender,
                             contactNo, email,
                             siteId, status);
            this.bookingId = bookingId;
            this.fromDate = fromDate;
            this.bookingName = bookingName;
            this.age = age;
            this.gender = gender;
            this.contactNo = contactNo;
            this.email = email;
            this.status = status;
            this.bookingClassId = -1;
            this.bookingProductId = -1;
            this.attractionScheduleId = -1;
            this.trxId = -1;
            this.customerId = -1;
            this.cardId = -1;
            this.siteId = -1;
            this.recur_flag = "N";
            //this.eventHostId = -1;
            //this.checkListTaskGroupId = -1;  
            this.masterEntityId = -1;
            //this.facilityId = -1;
            this.facilityMapId = -1;
            log.LogMethodExit();
        }
        public ReservationDTO(int bookingId, int bookingClassId, string bookingName, DateTime fromDate, string recur_flag, string recur_frequency, DateTime? recur_end_date,
                            int quantity, string reservationCode, string status, int cardId, string cardNumber, int customerId, string customerName, DateTime? expiryTime,
                            string channel, string remarks, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid,
                            bool synchStatus, int siteId, string contactNo, string alternateContactNo, string email, int isEmailSent, DateTime toDate, int trxId, int age,
                            string gender, string postalAddress, int bookingProductId, int attractionScheduleId, int extraGuests, int masterEntityId, string trxNumber,
                            string trxStatus, decimal? trxNetAmount, //int facilityId, 
                            string bookingProductName, double serviceChargeAmount, double serviceChargePercentage, 
                            int facilityMapId, string facilityMapName, string facilityName)
        {
            log.LogMethodEntry(bookingId, bookingClassId, bookingName, fromDate, recur_flag, recur_frequency, recur_end_date,
                             quantity, reservationCode, status, cardId, cardNumber, customerId, customerName, expiryTime,
                             channel, remarks, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, guid,
                             synchStatus, siteId, contactNo, alternateContactNo, email, isEmailSent, toDate, trxId, age,
                             gender, postalAddress, bookingProductId, attractionScheduleId, extraGuests, masterEntityId, trxNumber, trxStatus, trxNetAmount,
                             //facilityId, 
                             bookingProductName, serviceChargeAmount, serviceChargePercentage,
                             facilityMapId, facilityMapName, facilityName);
            this.bookingId = bookingId;
            this.bookingClassId = bookingClassId;
            this.bookingName = bookingName;
            this.fromDate = fromDate;
            this.recur_flag = recur_flag;
            this.recur_frequency = recur_frequency;
            this.recur_end_date = recur_end_date;
            this.quantity = quantity;
            this.reservationCode = reservationCode;
            this.status = status;
            this.cardId = cardId;
            this.cardNumber = cardNumber;
            this.customerId = customerId;
            this.customerName = customerName;
            this.expiryTime = expiryTime;
            this.channel = channel;
            this.remarks = remarks;
            this.contactNo = contactNo;
            this.alternateContactNo = alternateContactNo;
            this.email = email;
            this.isEmailSent = isEmailSent;
            this.toDate = toDate;
            this.trxId = trxId;
            this.age = age;
            this.gender = gender;
            this.postalAddress = postalAddress;
            this.bookingProductId = bookingProductId;
            this.attractionScheduleId = attractionScheduleId;
            this.extraGuests = extraGuests;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.trxNumber = trxNumber;
            this.trxStatus = trxStatus;
            this.trxNetAmount = trxNetAmount;
            //this.facilityId = facilityId;
            this.facilityMapId = facilityMapId;
            this.facilityMapName = facilityMapName;
            this.facilityName = facilityName;
            this.bookingProductName = bookingProductName;
            //this.eventHostId = eventHostId;
            //this.eventHostName = eventHostName;
            //this.checkListTaskGroupId = checkListTaskGroupId;
            //this.checkListTaskGroupName = checkListTaskGroupName;
            this.serviceChargeAmount = serviceChargeAmount;
            this.serviceChargePercentage = serviceChargePercentage;
            log.LogMethodExit();
        }

        /// Get/Set method of the BookingId field
        /// </summary>
        [DisplayName("BookingId")]
        public int BookingId { get { return bookingId; } set { bookingId = value; this.IsChanged = true; } }

        /// Get/Set method of the BookingClassId field
        /// </summary>
        [DisplayName("BookingClassId")]
        public int BookingClassId { get { return bookingClassId; } set { bookingClassId = value; this.IsChanged = true; } }

        /// Get/Set method of the BookingName field
        /// </summary>
        [DisplayName("BookingName")]
        public string BookingName { get { return bookingName; } set { bookingName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FromDate field
        /// </summary>
        [DisplayName("From Date Time")]
        public DateTime FromDate { get { return fromDate; ; } set { fromDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RecurFlag field
        /// </summary>
        [DisplayName("Recur Flag")]
        public string RecurFlag { get { return recur_flag; ; } set { recur_flag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RecurFrequency field
        /// </summary>
        [DisplayName("Recur Frequency")]
        public string RecurFrequency { get { return recur_frequency; } set { recur_frequency = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FromDate field
        /// </summary>
        [DisplayName("Recur End Date;")]
        public DateTime? RecurEndDate { get { return recur_end_date; } set { recur_end_date = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public int Quantity { get { return quantity; } set { quantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReservationCode field
        /// </summary>
        [DisplayName("Reservation Code")]
        public string ReservationCode { get { return reservationCode; } set { reservationCode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        [DisplayName("Status")]
        public string Status { get { return status; } set { status = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("CardId")]
        public int CardId { get { return cardId; } set { cardId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("Card Number")]
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustomeId field
        /// </summary>
        [DisplayName("Customer Id")]
        public int CustomerId { get { return customerId; } set { customerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the UserName field
        /// </summary>
        [DisplayName("Customer Name")]
        public string CustomerName { get { return customerName; } set { customerName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ExpiryTime field
        /// </summary>
        [DisplayName("Expiry Time")]
        public DateTime? ExpiryTime { get { return expiryTime; } set { expiryTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Channel field
        /// </summary>
        [DisplayName("Channel")]
        public string Channel { get { return channel; } set { channel = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ContactNo field
        /// </summary>
        [DisplayName("Contact No")]
        public string ContactNo { get { return contactNo; } set { contactNo = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AlternateContactNo field
        /// </summary>
        [DisplayName("Alternate Contact No")]
        public string AlternateContactNo { get { return alternateContactNo; } set { alternateContactNo = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Email field
        /// </summary>
        [DisplayName("Email")]
        public string Email { get { return email; } set { email = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsEmailSent field
        /// </summary>
        [DisplayName("Is Email Sent")]
        public int IsEmailSent { get { return isEmailSent; } set { isEmailSent = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ToDate field
        /// </summary>
        [DisplayName("To Date Time")]
        public DateTime ToDate { get { return toDate; } set { toDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        [DisplayName("Trx Id")]
        public int TrxId { get { return trxId; } set { trxId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TrxNumber field
        /// </summary>
        [DisplayName("Trx Number")]
        public string TrxNumber { get { return trxNumber; } set { trxNumber = value; } }

        /// <summary>
        /// Get/Set method of the trxStatus field
        /// </summary>
        [DisplayName("Trx Status")]
        public string TrxStatus { get { return trxStatus; } set { trxStatus = value; } }

        /// <summary>
        /// Get/Set method of the TrxNetAmount field
        /// </summary>
        [DisplayName("Trx Net Amount")]
        public decimal? TrxNetAmount { get { return trxNetAmount; } set { trxNetAmount = value; } }
        /// <summary>
        /// Get/Set method of the Age field
        /// </summary>
        [DisplayName("Age")]
        public int Age { get { return age; } set { age = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Gender field
        /// </summary>
        [DisplayName("Gender")]
        public string Gender { get { return gender; } set { gender = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PostalAddress field
        /// </summary>
        [DisplayName("Postal Address")]
        public string PostalAddress { get { return postalAddress; } set { postalAddress = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the BookingProductId field
        /// </summary>
        [DisplayName("Booking Product Id")]
        public int BookingProductId { get { return bookingProductId; } set { bookingProductId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the BookingProductName field
        /// </summary>
        [DisplayName("Booking Product Name")]
        public string BookingProductName { get { return bookingProductName; } set { bookingProductName = value; } }

        ///// <summary>
        ///// Get/Set method of the facilityId field
        ///// </summary>
        //[DisplayName("Facility Id")] 
        //public int FacilityId { get { return facilityId; } set { facilityId = value;  } }

        /// <summary>
        /// Get/Set method of the facilityName field
        /// </summary>
        [DisplayName("Facility Name")]
        public string FacilityName { get { return facilityName; } set { facilityName = value; } }

        /// <summary>
        /// Get/Set method of the FacilityMapId field
        /// </summary>
        [DisplayName("Facility Map Id")]
        public int FacilityMapId { get { return facilityMapId; } set { facilityMapId = value; } }

        /// <summary>
        /// Get/Set method of the FacilityMappName field
        /// </summary>
        [DisplayName("Facility Map Name")]
        public string FacilityMapName { get { return facilityMapName; } set { facilityMapName = value; } }

        /// <summary>
        /// Get/Set method of the BookingProductId field
        /// </summary>
        [DisplayName("Attraction Schedule Id")]
        public int AttractionScheduleId { get { return attractionScheduleId; } set { attractionScheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ExtraGuests field
        /// </summary>
        [DisplayName("Extra Guests")]
        public int ExtraGuests { get { return extraGuests; } set { extraGuests = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        public DateTime LastupdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SeatId field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

        ///// Get/Set method of the EventHostId field
        ///// </summary>
        //[DisplayName("Event Host Id")] 
        //public int EventHostId { get { return eventHostId; } set { eventHostId = value; this.IsChanged = true; } }

        ///// Get/Set method of the EventHostName field
        ///// </summary>
        //[DisplayName("Event Host Name")]
        //public string EventHostName { get { return eventHostName; } set { eventHostName = value; this.IsChanged = true; } }

        ///// Get/Set method of the CheckListTaskGroupId field
        ///// </summary>
        //[DisplayName("Check List Id")] 
        //public int CheckListTaskGroupId { get { return checkListTaskGroupId; } set { checkListTaskGroupId = value; this.IsChanged = true; } }

        ///// Get/Set method of the CheckListTaskGroupName field
        ///// </summary>
        //[DisplayName("Check List Name")]
        //public string CheckListTaskGroupName { get { return checkListTaskGroupName; } set { checkListTaskGroupName = value; this.IsChanged = true; } }

        ///// Get/Set method of the BookingAttendeeList field
        ///// </summary>
        //[DisplayName("Booking Attendee List")]
        //public List<BookingAttendeeDTO> BookingAttendeeList { get { return bookingAttendeeList; } set { bookingAttendeeList = value; this.IsChanged = true; } }
        /// Get/Set method of the bookingCheckListDTOList field
        /// </summary>
        [DisplayName("Booking Check List")]
        public List<BookingCheckListDTO> BookingCheckListDTOList { get { return bookingCheckListDTOList; } set { bookingCheckListDTOList = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the serviceChargeAmount field
        /// </summary>
        [DisplayName("ServiceChargeAmount")]
        public double ServiceChargeAmount { get { return serviceChargeAmount; } set { serviceChargeAmount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the serviceChargePercentage field
        /// </summary>
        [DisplayName("ServiceChargePercentage")]
        public double ServiceChargePercentage { get { return serviceChargePercentage; } set { serviceChargePercentage = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || bookingId < 0;
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

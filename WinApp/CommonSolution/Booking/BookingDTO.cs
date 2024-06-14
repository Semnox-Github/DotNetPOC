using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.Booking
{
    public class BookingDTO
    {
        //    int bookingId;
        //    int bookingClassId;
        //    string bookingName;
        //    DateTime fromDate;
        //    string recur_flag;
        //    string recur_frequency;
        //    DateTime recur_end_date;
        //    int quantity;
        //    string reservationCode;
        //    string status;
        //    int cardId;
        //    string cardNumber;
        //    int customerId;
        //    string customerName;
        //    DateTime expiryTime;
        //    string channel;
        //    string remarks;
        //    string contactNo;
        //    string alternateContactNo;
        //    string email;
        //    int isEmailSent;
        //    DateTime toDate;
        //    int trxId;
        //    int age;
        //    string gender;
        //    string postalAddress;
        //    int bookingProductId;
        //    int attractionScheduleId;
        //    int extraGuests;
        //    string createdBy;
        //    DateTime creationDate;
        //    string lastUpdatedBy;
        //    DateTime lastUpdatedDate;
        //    string guid;
        //    bool synchStatus;
        //    int siteId;
        //    int masterEntityId;


        //    bool notifyingObjectIsChanged;
        //    readonly object notifyingObjectIsChangedSyncRoot = new Object();

        //    Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //    /// <summary>
        //    /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        //    /// </summary>
        //    public enum SearchByParameters
        //    {
        //        /// <summary>
        //        /// Search by BOOKING_ID field
        //        /// </summary>
        //        BOOKING_ID = 0,
        //        /// <summary>
        //        /// Search by TRX_ID field
        //        /// </summary>
        //        TRX_ID = 1
        //    }

        //        /// <summary>
        //    /// Default constructor
        //    /// </summary>
        //    public BookingDTO()
        //    {
        //        bookingId = -1;
        //        bookingClassId = -1;
        //        bookingProductId = -1;
        //        attractionScheduleId = -1;
        //        trxId = -1;
        //        customerId = -1;
        //        cardId = -1;
        //        siteId = -1;
        //        recur_flag = "N";
        //    }


        //         /// <summary>
        //    /// Constructor With Parameter
        //    /// </summary>
        //    public BookingDTO(int bookingId, string bookingName, DateTime fromDate, int age, string gender,
        //                        string contactNo, string email,
        //                        int siteId,string status)
        //    {
        //        this.bookingId = bookingId;
        //        this.fromDate = fromDate;
        //        this.bookingName = bookingName;
        //        this.age = age;
        //        this.gender = gender;
        //        this.contactNo = contactNo;
        //        this.email = email;
        //        this.status = status;
        //    }
        //    public BookingDTO(int bookingId,int bookingClassId,string bookingName,DateTime fromDate,string recur_flag,string recur_frequency,DateTime recur_end_date,
        //                        int quantity,string reservationCode,string status,int cardId,string cardNumber,int customerId,string customerName,DateTime expiryTime,
        //                        string channel,string remarks, string createdBy,DateTime creationDate,string lastUpdatedBy,DateTime lastUpdatedDate,string guid,
        //                        bool synchStatus, int siteId, string contactNo, string alternateContactNo, string email, int isEmailSent, DateTime toDate, int trxId, int age,
        //                        string gender,string postalAddress,int bookingProductId,int attractionScheduleId,int extraGuests,int masterEntityId)
        //    {

        //        this.bookingId=bookingId;
        //        this.bookingClassId=bookingClassId;
        //        this.bookingName=bookingName;
        //        this.fromDate=fromDate;
        //        this.recur_flag=recur_flag;
        //        this.recur_frequency=recur_frequency;
        //        this.recur_end_date=recur_end_date;
        //        this.quantity=quantity;
        //        this.reservationCode=reservationCode;
        //        this.status=status;
        //        this.cardId=cardId;
        //        this.cardNumber=cardNumber;
        //        this.customerId=customerId;
        //        this.customerName=customerName;
        //        this.expiryTime=expiryTime;
        //        this.channel=channel;
        //        this.remarks=remarks;
        //        this.contactNo=contactNo;
        //        this.alternateContactNo=alternateContactNo;
        //        this.email=email;
        //        this.isEmailSent=isEmailSent;
        //        this.toDate=toDate;
        //        this.trxId=trxId;
        //        this.age=age;
        //        this.gender=gender;
        //        this.postalAddress=postalAddress;
        //        this.bookingProductId=bookingProductId;
        //        this.attractionScheduleId=attractionScheduleId;
        //        this.extraGuests=extraGuests;
        //        this.createdBy=createdBy;
        //        this.creationDate=creationDate;
        //        this.lastUpdatedBy=lastUpdatedBy;
        //        this.lastUpdatedDate=lastUpdatedDate;
        //        this.guid=guid;
        //        this.synchStatus=synchStatus;
        //        this.siteId=siteId;
        //        this.masterEntityId=masterEntityId;
        //    }

        //    /// Get/Set method of the BookingId field
        //    /// </summary>
        //    [DisplayName("BookingId")]
        //    [DefaultValue(-1)]
        //    public int BookingId { get { return bookingId; } set { bookingId = value; this.IsChanged = true; } }


        //    /// Get/Set method of the BookingClassId field
        //    /// </summary>
        //    [DisplayName("BookingClassId")]
        //    [DefaultValue(-1)]
        //    public int BookingClassId { get { return bookingClassId; } set { bookingClassId = value; this.IsChanged = true; } }


        //    /// Get/Set method of the BookingName field
        //    /// </summary>
        //    [DisplayName("BookingName")]
        //    public string BookingName { get { return bookingName; } set { bookingName = value; this.IsChanged = true; } }


        //    /// <summary>
        //    /// Get/Set method of the FromDate field
        //    /// </summary>
        //    [DisplayName("FromDate")]
        //    [DefaultValue(typeof(DateTime), "")]
        //    public DateTime FromDate { get { return fromDate; ; } set { fromDate = value; this.IsChanged = true; } }


        //    /// <summary>
        //    /// Get/Set method of the RecurFlag field
        //    /// </summary>
        //    [DisplayName("RecurFlag")]
        //    public string RecurFlag { get { return recur_flag; ; } set { recur_flag = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the RecurFrequency field
        //    /// </summary>
        //    [DisplayName("RecurFrequency")]
        //    public string RecurFrequency { get { return recur_frequency; } set { recur_frequency = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the FromDate field
        //    /// </summary>
        //    [DisplayName("RecurEndDate;")]
        //    [DefaultValue(typeof(DateTime), "")]
        //    public DateTime RecurEndDate { get { return recur_end_date ; } set { recur_end_date = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the Quantity field
        //    /// </summary>
        //    [DisplayName("Quantity")]
        //    [DefaultValue(0)]
        //    public int Quantity { get { return quantity; } set { quantity = value; this.IsChanged = true; } }


        //    /// <summary>
        //    /// Get/Set method of the ReservationCode field
        //    /// </summary>
        //    [DisplayName("ReservationCode")]
        //    public string ReservationCode { get { return reservationCode; } set { reservationCode = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the Status field
        //    /// </summary>
        //    [DisplayName("Status")]
        //    public string Status { get { return status; } set { status = value; this.IsChanged = true; } }


        //    /// <summary>
        //    /// Get/Set method of the CardId field
        //    /// </summary>
        //    [DisplayName("CardId")]
        //    [DefaultValue(-1)]
        //    public int CardId { get { return cardId; } set { cardId = value; this.IsChanged = true; } }


        //    /// <summary>
        //    /// Get/Set method of the CardNumber field
        //    /// </summary>
        //    [DisplayName("Card Number")]
        //    public string CardNumber { get { return cardNumber; } set { cardNumber = value; this.IsChanged = true; } }


        //    /// <summary>
        //    /// Get/Set method of the CustomeId field
        //    /// </summary>
        //    [DisplayName("Customer Id")]
        //    [DefaultValue(-1)]
        //    public int CustomerId { get { return customerId; } set { customerId = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the UserName field
        //    /// </summary>
        //    [DisplayName("Customer Name")]
        //    public string CustomerName { get { return customerName; } set { customerName = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the ExpiryTime field
        //    /// </summary>
        //    [DisplayName("Expiry Time")]
        //    [DefaultValue(typeof(DateTime), "")]
        //    public DateTime ExpiryTime { get { return expiryTime; } set { expiryTime = value; this.IsChanged = true; } }


        //    /// <summary>
        //    /// Get/Set method of the Channel field
        //    /// </summary>
        //    [DisplayName("Channel")]
        //    public string Channel { get { return channel; } set { channel = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the Remarks field
        //    /// </summary>
        //    [DisplayName("Remarks")]
        //    public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the ContactNo field
        //    /// </summary>
        //    [DisplayName("ContactNo")]
        //    public string ContactNo { get { return contactNo; } set { contactNo = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the AlternateContactNo field
        //    /// </summary>
        //    [DisplayName("AlternateContactNo")]
        //    public string AlternateContactNo { get { return alternateContactNo; } set { alternateContactNo = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the Email field
        //    /// </summary>
        //    [DisplayName("Email")]
        //    public string Email { get { return email; } set { email = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the IsEmailSent field
        //    /// </summary>
        //    [DisplayName("IsEmailSent")]
        //    public int IsEmailSent { get { return isEmailSent; } set { isEmailSent = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the ToDate field
        //    /// </summary>
        //    [DisplayName("ToDate")]
        //    [DefaultValue(typeof(DateTime), "")]
        //    public DateTime ToDate { get { return toDate; } set { toDate = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the TrxId field
        //    /// </summary>
        //    [DisplayName("TrxId")]
        //    [DefaultValue(-1)]
        //    public int TrxId { get { return trxId; } set { trxId = value; this.IsChanged = true; } }


        //    /// <summary>
        //    /// Get/Set method of the Age field
        //    /// </summary>
        //    [DisplayName("Age")]
        //    [DefaultValue(0)]
        //    public int Age { get { return age; } set { age = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the Gender field
        //    /// </summary>
        //    [DisplayName("Gender")]
        //    public string Gender { get { return gender; } set { gender = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get/Set method of the PostalAddress field
        //    /// </summary>
        //    [DisplayName("PostalAddress")]
        //    public string PostalAddress { get { return postalAddress; } set { postalAddress = value; this.IsChanged = true; } }


        //    /// <summary>
        //    /// Get/Set method of the BookingProductId field
        //    /// </summary>
        //    [DisplayName("BookingProductId")]
        //    [DefaultValue(-1)]
        //    public int BookingProductId { get { return bookingProductId; } set { bookingProductId = value; this.IsChanged = true; } }


        //    /// <summary>
        //    /// Get/Set method of the BookingProductId field
        //    /// </summary>
        //    [DisplayName("AttractionScheduleId")]
        //    [DefaultValue(-1)]
        //    public int AttractionScheduleId { get { return attractionScheduleId; } set { attractionScheduleId = value; this.IsChanged = true; } }


        //    /// <summary>
        //    /// Get/Set method of the ExtraGuests field
        //    /// </summary>
        //    [DisplayName("ExtraGuests")]
        //    [DefaultValue(0)]
        //    public int ExtraGuests { get { return extraGuests; } set { extraGuests = value; this.IsChanged = true; } }

        //    /// <summary>
        //    /// Get method of the CreatedBy field
        //    /// </summary>
        //    [DisplayName("Created By")]
        //    public string CreatedBy { get { return createdBy; } }

        //    /// <summary>
        //    /// Get method of the CreationDate field
        //    /// </summary>
        //    [DisplayName("Created Date")]
        //    public DateTime CreationDate { get { return creationDate; } }

        //    /// <summary>
        //    /// Get method of the LastUpdatedBy field
        //    /// </summary>
        //    [DisplayName("Updated By")]
        //    public string LastUpdatedBy { get { return lastUpdatedBy; } }

        //    /// <summary>
        //    /// Get method of the LastupdatedDate field
        //    /// </summary>
        //    [DisplayName("Updated Date")]
        //    public DateTime LastupdatedDate { get { return lastUpdatedDate; } }


        //    /// <summary>
        //    /// Get/Set method of the SeatId field
        //    /// </summary>
        //    [DisplayName("Guid")]
        //    public string Guid { get { return guid; } set { guid = value; } }

        //    /// <summary>
        //    /// Get/Set method of the SiteId field
        //    /// </summary>
        //    [DisplayName("SiteId")]
        //    [DefaultValue(-1)]
        //    public int SiteId { get { return siteId; } set { siteId = value; } }

        //    /// <summary>
        //    /// Get/Set method of the SynchStatus field
        //    /// </summary>
        //    [DisplayName("SynchStatus")]
        //    [DefaultValue(false)]
        //    public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        //    /// <summary>
        //    /// Get/Set method of the MasterEntityId field
        //    /// </summary>
        //    [DisplayName("MasterEntityId")]
        //    [DefaultValue(-1)]
        //    public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }


        //    /// <summary>
        //    /// Get/Set method to track changes to the object
        //    /// </summary>
        //    [Browsable(false)]
        //    [DefaultValue(false)]
        //    public bool IsChanged
        //    {
        //        get
        //        {
        //            lock (notifyingObjectIsChangedSyncRoot)
        //            {
        //                return notifyingObjectIsChanged;
        //            }
        //        }

        //        set
        //        {
        //            lock (notifyingObjectIsChangedSyncRoot)
        //            {
        //                if (!Boolean.Equals(notifyingObjectIsChanged, value))
        //                {
        //                    notifyingObjectIsChanged = value;
        //                }
        //            }
        //        }
        //    }

        //    /// <summary>
        //    /// Allowes to accept the changes
        //    /// </summary>
        //    public void AcceptChanges()
        //    {
        //        log.Debug("Starts-AcceptChanges() Method.");
        //        this.IsChanged = false;
        //        log.Debug("Ends-AcceptChanges() Method.");
        //    }

    }
}

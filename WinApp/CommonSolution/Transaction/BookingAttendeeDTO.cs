/********************************************************************************************
 * Project Name - BookingAttendeeDTO
 * Description  - Data object of BookingAttendee    
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.70        14-Mar-2019    Guru S A      Booking phase 2 enhancement changes 
 *2.70.2      25-Sep-2019    Deeksha       Added new fields customerId & TrxId ID.
 *2.80.0      28-Apr-2020    Guru S A      Send sign waiver email changes
 *2.130.10    08-Sep-2022    Nitin Pai     Modified as part of customer delete enhancement.
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.Transaction
{
    public class BookingAttendeeDTO
    {
        private int id;
        private int bookingId; 
        private string name;
        private int? age;
        private string gender;
        private string specialRequest;
        private string remarks;
        private string guid;
        private string phoneNumber;
        private string email;
        private bool partyInNameOf;
        private DateTime? dateofBirth;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int customerId;
        private int trxId;
        private DateTime? signWaiverEmailLastSentOn;
        private int signWaiverEmailSentCount;

        bool notifyingObjectIsChanged;
        readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by BOOKING ID field
            /// </summary>
            BOOKING_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by TRX ID field
            /// </summary>
            TRX_ID,
            /// <summary>
            /// Search by CUSTOMER_ID_LIST field
            /// </summary>
            CUSTOMER_ID_LIST,
            /// <summary>
            /// Search by TRX_ID_LIST field
            /// </summary>
            TRX_ID_LIST,
        }

            /// <summary>
        /// Default constructor
        /// </summary>
        public BookingAttendeeDTO()
        {
            log.LogMethodEntry();
            this.id = -1;
            this.bookingId = -1;
            this.partyInNameOf = false;
            this.synchStatus = false;
            this.siteId = -1; 
            this.isActive = true;
            this.masterEntityId = -1;
            this.customerId = -1;
            this.trxId = -1;
            this.signWaiverEmailSentCount = 0;
            this.signWaiverEmailLastSentOn = null;
            log.LogMethodExit();
           
        }
        
        /// <summary>
        /// Constructor With required parameter
        /// </summary>
        public BookingAttendeeDTO(int id, int bookingId, string name, int? age, string gender, string specialRequest,
                                    string phoneNumber, string email, bool partyInNameOf, DateTime? dateofBirth, bool isActive, string remarks, int customerId, int trxId)
            : this()
        {
            log.LogMethodEntry(id, bookingId, name, age, gender, specialRequest,
                                    phoneNumber, email, partyInNameOf, dateofBirth, isActive, remarks, customerId, trxId);
            this.id = id;
            this.bookingId = bookingId;
            this.name = name;
            this.age = age;
            this.gender = gender;
            this.specialRequest = specialRequest;
            this.phoneNumber = phoneNumber;
            this.email = email;
            this.partyInNameOf = partyInNameOf;
            this.dateofBirth = dateofBirth;
            this.remarks = remarks;
            this.customerId = customerId;
            this.trxId = trxId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor With all the parameter.
        /// </summary>
        public BookingAttendeeDTO(int id, int bookingId, string name, int? age, string gender,string specialRequest,
                                    string phoneNumber, string email, bool partyInNameOf, DateTime? dateofBirth,
                                     int siteId, int masterEntityId, string guid, bool synchStatus, bool isActive, string createdBy, 
                                     DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string remarks,int customerId, int trxId, DateTime? signWaiverEmailLastSentOn, int signWaiverEmailSentCount)
            : this(id, bookingId, name, age, gender, specialRequest, phoneNumber, email, partyInNameOf, dateofBirth, isActive, remarks,customerId, trxId)
        {
            log.LogMethodEntry(id, bookingId, name, age, gender, specialRequest,
                                    phoneNumber, email, partyInNameOf, dateofBirth,
                                     siteId, masterEntityId, guid, synchStatus, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, remarks, signWaiverEmailLastSentOn, signWaiverEmailSentCount);
            this.guid = guid;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.signWaiverEmailLastSentOn = signWaiverEmailLastSentOn;
            this.signWaiverEmailSentCount = signWaiverEmailSentCount;
            log.LogMethodExit();
        }


        /// Get/Set method of the id field
        /// </summary>
        [DisplayName("Id")]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// Get/Set method of the BookingId field
        /// </summary>
        [DisplayName("BookingId")] 
        public int BookingId { get { return bookingId; } set { bookingId = value; this.IsChanged = true; } }

        /// Get/Set method of the Email field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Age field
        /// </summary>
        [DisplayName("Age")] 
        public int? Age { get { return age; } set { age = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Gender field
        /// </summary>
        [DisplayName("Gender")]
        public string Gender { get { return gender; } set { gender = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SpecialRequest field
        /// </summary>
        [DisplayName("SpecialRequest")]
        public string SpecialRequest { get { return specialRequest; } set { specialRequest = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PhoneNumber field
        /// </summary>
        [DisplayName("PhoneNumber")]
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Email field
        /// </summary>
        [DisplayName("Email")]
        public string Email { get { return email; } set { email = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Email field
        /// </summary>
        [DisplayName("PartyInNameOf")]
        public bool PartyInNameOf { get { return partyInNameOf; } set { partyInNameOf = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DateofBirth field
        /// </summary>
        [DisplayName("DateofBirth")] 
        public DateTime? DateofBirth { get { return dateofBirth; } set { dateofBirth = value; this.IsChanged = true; } }

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
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Update Date")]
        [Browsable(false)]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the id field
        /// </summary>
        [DisplayName("CustomerId")]
        public int CustomerId { get { return customerId; } set { customerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the id field
        /// </summary>
        [DisplayName("TrxId")]
        public int TrxId { get { return trxId; } set { trxId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SignWaiverEmailLastSentOn field
        /// </summary>
        [DisplayName("Waiver Email Last Sent On")]
        public DateTime? SignWaiverEmailLastSentOn { get { return signWaiverEmailLastSentOn; } set { signWaiverEmailLastSentOn = value; this.IsChanged = true; } }
        /// Get/Set method of the SignWaiverEmailSentCount field
        /// </summary>
        [DisplayName("Waiver Email Sent Count")]
        public int SignWaiverEmailSentCount { get { return signWaiverEmailSentCount; } set { signWaiverEmailSentCount = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || id < 0;
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
            this.IsChanged = false;
            log.LogMethodExit();
        }

    } 
}

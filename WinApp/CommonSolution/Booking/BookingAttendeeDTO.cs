using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.Booking
{
    public class BookingAttendeeDTO
    {
        int id;
        int bookingId;
        string name;
        int age;
        string gender;
        string specialRequest;
        string remarks;
        string guid;
        string phoneNumber;
        string email;
        bool partyInNameOf;
        DateTime dateofBirth;
        int siteId;
        bool synchStatus;

        bool notifyingObjectIsChanged;
        readonly object notifyingObjectIsChangedSyncRoot = new Object();

        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID = 0,
            /// <summary>
            /// Search by USER ID field
            /// </summary>
            BOOKING_ID = 1
        }

            /// <summary>
        /// Default constructor
        /// </summary>
        public BookingAttendeeDTO()
        {
            this.Id = -1;
            this.BookingId = -1;
            this.partyInNameOf = false;
            this.SynchStatus = false;
            this.siteId = -1;
            this.age = 0;
           
        }


             /// <summary>
        /// Constructor With Parameter
        /// </summary>
        public BookingAttendeeDTO(int id, int bookingId, string name, int age, string gender,string specialRequest,
                                    string phoneNumber, string email, bool partyInNameOf, DateTime dateofBirth,
                                     int site_id, string guid, bool synchStatus)
        {

          
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
            this.guid = guid;
            this.siteId = site_id;
            this.synchStatus = synchStatus;
        }


        /// Get/Set method of the id field
        /// </summary>
        [DisplayName("Id")]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// Get/Set method of the BookingId field
        /// </summary>
        [DisplayName("BookingId")]
        [DefaultValue(-1)]
        public int BookingId { get { return bookingId; } set { bookingId = value; this.IsChanged = true; } }

        /// Get/Set method of the Email field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Age field
        /// </summary>
        [DisplayName("Age")]
        [DefaultValue(-1)]
        public int Age { get { return age; } set { age = value; this.IsChanged = true; } }

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
        [DefaultValue(typeof(DateTime), "")]
        public DateTime DateofBirth { get { return dateofBirth; } set { dateofBirth = value; this.IsChanged = true; } }


        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }

    }




}

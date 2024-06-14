/********************************************************************************************
 * Project Name - Customers  DTO Programs 
 * Description  - Data object of the CustomersDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        08-June-2016   Rakshith           Created 
 *2.70.2        19-Jul-2019    Girish Kundar            Modified : Added Constructor with required Parameter
 *******************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// CustomersDTO Class
    /// </summary>
    public class CustomersDTO
    {
        private static readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int customerId;
        private string userName;
        private int siteId;
        private string firstName;
        private string middleName;
        private string lastName;
        private string emailId;
        private DateTime dateOfBirth;
        private DateTime anniversaryDate;
        private string address1;
        private string address2;
        private string address3; //added
        private string contact_phone1;//added
        private string contact_phone2;//added
        private string notes;
        private string city;
        private string state;
        private string country;
        private string postalCode;
        private string gender;
        private string passWord;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string photoFileName;
        private string guid;
        private string title;
        private string channel;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CustomersDTO()
        {
            this.customerId = -1;
            this.userName = string.Empty;
            this.siteId = -1;
            this.firstName = string.Empty;
            this.middleName = string.Empty;
            this.lastName = string.Empty;
            this.emailId = string.Empty;
            this.gender = "N";
            this.channel = string.Empty;
            this.dateOfBirth = DateTime.MinValue;
            this.anniversaryDate = DateTime.MinValue;
            this.address1 = string.Empty;
            this.address2 = string.Empty;
            this.address3 = string.Empty;
            this.contact_phone1 = string.Empty;
            this.contact_phone2 = string.Empty;
            this.notes = string.Empty;
            this.city = string.Empty;
            this.state = string.Empty;
            this.country = string.Empty;
            this.postalCode = string.Empty;
            this.gender = string.Empty;
            this.passWord = string.Empty;
            this.lastUpdatedBy = string.Empty;
            this.lastUpdatedDate = DateTime.MinValue;
            this.photoFileName = string.Empty;
            this.postalCode = string.Empty;
            this.guid = string.Empty;
            this.title = string.Empty;
            this.channel = string.Empty;
            
        }



        /// <summary>
        /// Parameterized Constructor with required fields
        /// </summaryCustomersDTO>
        public CustomersDTO(int customer_id, string userName, string firstName, string middleName, string lastName,
                            string emailId, DateTime dateOfBirth, DateTime anniversaryDate, string address1, string address2,
                            string address3, string contact_phone1, string contact_phone2, string notes, string city, string state, string country, string postalCode,
                            string gender, string passWord, string photoFileName, string title, string channel)
            : this()
        {
            log.LogMethodEntry(customer_id, userName, firstName, middleName, lastName,
                              emailId, dateOfBirth, anniversaryDate, address1, address2,
                             address3, contact_phone1, contact_phone2, notes, city, state, country, postalCode,
                             gender, passWord, photoFileName, title, channel);
            this.customerId = customer_id;
            this.userName = userName;
            this.firstName = firstName;
            this.middleName = middleName;
            this.lastName = lastName;
            this.emailId = emailId;
            this.dateOfBirth = dateOfBirth;
            this.anniversaryDate = anniversaryDate;
            this.address1 = address1;
            this.address2 = address2;
            this.address3 = address3;
            this.contact_phone1 = contact_phone1;
            this.contact_phone2 = contact_phone2;
            this.city = city;
            this.notes = notes;
            this.state = state;
            this.country = country;
            this.postalCode = postalCode;
            this.gender = gender;
            this.passWord = passWord;
            this.photoFileName = photoFileName;
            this.title = title;
            this.channel = channel;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized Constructor
        /// </summaryCustomersDTO>
        public CustomersDTO(int customer_id ,string userName,int siteId, string firstName, string middleName, string lastName,
                             string emailId, DateTime dateOfBirth, DateTime anniversaryDate, string address1, string address2,
                            string address3,  string contact_phone1, string contact_phone2, string notes, string city, string state, string country, string postalCode,
                            string gender, string passWord, string lastUpdatedBy, DateTime lastUpdatedDate, string photoFileName, string guid, string title, string channel)
            :this(customer_id, userName, firstName, middleName, lastName,
                              emailId, dateOfBirth, anniversaryDate, address1, address2,
                             address3, contact_phone1, contact_phone2, notes, city, state, country, postalCode,
                             gender, passWord, photoFileName, title, channel)
        {
            log.LogMethodEntry( customer_id,  userName, siteId,firstName,  middleName,  lastName,
                              emailId,  dateOfBirth,  anniversaryDate,  address1,  address2,
                             address3,  contact_phone1,  contact_phone2,  notes,  city,  state,  country,  postalCode,
                             gender,  passWord,  lastUpdatedBy,  lastUpdatedDate,  photoFileName,  guid,  title, channel);
           
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            log.LogMethodExit();
        }
    
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by USER_NAME field
            /// </summary>
            USER_NAME = 0,
            /// <summary>
            /// Search by FNAME field
            /// </summary>
            FNAME = 1,
            /// <summary>
            /// Search by MNAME field
            /// </summary>
            MNAME = 2,
            /// <summary>
            /// Search by LNAME field
            /// </summary>
            LNAME = 3,
            /// <summary>
            /// Search by EMAIL field
            /// </summary>
            EMAIL=4 ,
            /// <summary>
            /// Search by PHONE field
            /// </summary>
            PHONE=5 ,
            /// <summary>
            /// Search by ORDER_BY_USERNAME field
            /// </summary>
            ORDER_BY_USERNAME = 6,
            /// <summary>
            /// Search by ORDER_BY_LAST_UPDATED_DATE field
            /// </summary>
            ORDER_BY_LAST_UPDATED_DATE = 7,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 8,
            /// <summary>
            /// Search by PASSWORD field
            /// </summary>
            PASSWORD = 9,

           
        }

        /// <summary>
        /// Get/Set method of the Customer_id field
        /// </summary>
        [DisplayName("Customer id")]
        [DefaultValue(-1)]
        public int CustomerId { get { return customerId; } set { customerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the UserName field
        /// </summary>
        [DisplayName("User Name")]
        public string UserName { get { return userName; } set { userName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [DefaultValue(-1)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FirstName field
        /// </summary>
        [DisplayName("First Name")]
        public string FirstName { get { return firstName; } set { firstName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MiddleName field
        /// </summary>
        [DisplayName("MiddleName")]
        public string MiddleName { get { return middleName; } set { middleName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastName field
        /// </summary>
        [DisplayName("Last Name")]
        public string LastName { get { return lastName; } set { lastName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EmailId field
        /// </summary>
        [DisplayName("EmailId")]
        public string EmailId { get { return emailId; } set { emailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the City field
        /// </summary>
        [DisplayName("City")]
        public string City { get { return city; } set { city = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the State field
        /// </summary>
        [DisplayName("State")]
        public string State { get { return state; } set { state = value; this.IsChanged = true; } }
 
        /// <summary>
        /// Get/Set method of the DateOfBirth field
        /// </summary>
        [DisplayName("DateOfBirth")]
        [DefaultValue(typeof(DateTime), "")]
        public DateTime DateOfBirth
        {
            get
            {
                if (dateOfBirth.Year == 1)
                {
                    dateOfBirth = new DateTime(1900, 1, 1);
                }
                return dateOfBirth;

            }
            set { dateOfBirth = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the AnniversaryDate field
        /// </summary>
        [DisplayName("AnniversaryDate")]
        [DefaultValue(typeof(DateTime),"")]
        public DateTime AnniversaryDate
        {
            get
            {
                if (anniversaryDate.Year == 1)
                {
                    anniversaryDate = new DateTime(1900, 1, 1);
                }
                return anniversaryDate;

            }
            set { anniversaryDate = value; this.IsChanged = true; }
        }



        /// <summary>
        /// Get/Set method of the Address1 field
        /// </summary>
        [DisplayName("Address 1")]
        public string Address1 { get { return address1; } set { address1 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Address2 field
        /// </summary>
        [DisplayName("Address 2")]
        public string Address2 { get { return address2; } set { address2 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Address3 field
        /// </summary>
        [DisplayName("Address 3")]
        public string Address3 { get { return address3; } set { address3 = value; } }

        /// <summary>
        /// Get/Set method of the Contact_phone1 field
        /// </summary>
        [DisplayName("Contact phone1")]
        public string Contact_phone1 { get { return contact_phone1; } set { contact_phone1 = value; } }

        /// <summary>
        /// Get/Set method of the Contact_phone2 field
        /// </summary>
        [DisplayName("Contact phone2")]
        public string Contact_phone2 { get { return contact_phone2; } set { contact_phone2 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Notes field
        /// </summary>
        [DisplayName("Notes")]
        public string Notes { get { return notes; } set { notes = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Country field
        /// </summary>
        [DisplayName("Country")]
        public string Country { get { return country; } set { country = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PostalCode field
        /// </summary>
        [DisplayName("PostalCode")]
        public string PostalCode { get { return postalCode; } set { postalCode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Gender field
        /// </summary>
        [DisplayName("Gender")]
        [DefaultValue("N")]
        public string Gender { get {   return gender; } set { gender = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PassWord field
        /// </summary>
        [DisplayName("PassWord")]
        public string PassWord { get { return passWord; } set { passWord = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        [DefaultValue(typeof(DateTime), "")]
        public DateTime LastUpdatedDate
        {
            get
            {
                if (lastUpdatedDate.Year == 1)
                {
                    lastUpdatedDate = DateTime.Now;
                }
                return lastUpdatedDate;

            }
            set { lastUpdatedDate = value;  }
        }
        

        /// <summary>
        /// Get/Set method of the PhotoFileName field
        /// </summary>
        [DisplayName("Photo FileName")]
        public string PhotoFileName { get { return photoFileName; } set { photoFileName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the Title field
        /// </summary>
        [DisplayName("Title")]
        public string Title { get { return title; } set { title = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Channel field
        /// </summary>
        [DisplayName("Channel")]
        public string Channel { get { return channel; } set { channel = value; this.IsChanged = true; } }


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
                    return notifyingObjectIsChanged || customerId < 0;
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
 

/********************************************************************************************
 * Project Name - Vendor DTO
 * Description  - Data object of vendor DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Apr-2016   Raghuveera          Created 
 ********************************************************************************************
 *1.00        10-Aug-2016   Soumya              Updated 
 ********************************************************************************************
 *2.60        11-Apr-2019    Girish Kundar      Updated : Adding field PurchaseTaxId and Get/Set
 *2.70        19-Jun-2019    Akshay Gulaganji   Modified isActive property (from string to bool)
 *2.70.2        25-Jul-2019    Deeksha            Modifications as per three tier changes.
 *2.100.0     14-Oct-2020   Mushahid Faizan   modified IsChanged property.
 *3.0        02-Nov-2020   Mushahid Faizan      Web inventory enhancement.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Vendor
{
    /// <summary>
    /// This is the vendor data object class. This acts as data holder for the vendor business object
    /// </summary>
    public class VendorDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByVendorParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByVendorParameters
        {
            /// <summary>
            /// Search by VENDOR ID field
            /// </summary>
            VENDOR_ID,
            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME,
            /// <summary>
            /// Search by ADDRESS field
            /// </summary>
            ADDRESS,
            /// <summary>
            /// Search by CITY field
            /// </summary>
            CITY,
            /// <summary>
            /// Search by STATE field
            /// </summary>
            STATE,
            /// <summary>
            /// Search by COUNTRY field
            /// </summary>
            COUNTRY,
            /// <summary>
            /// Search by POSTAL CODE field
            /// </summary>
            POSTAL_CODE,
            /// <summary>
            /// Search by CONTACT NAME field
            /// </summary>
            CONTACT_NAME,
            /// <summary>
            /// Search by PHONE field
            /// </summary>
            PHONE,
            /// <summary>
            /// Search by EMAIL field
            /// </summary>
            EMAIL,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITEID field
            /// </summary>
            SITEID,
            /// <summary>
            /// Search by VENDOR CODE field
            /// </summary>
            VENDORCODE,
            /// <summary>
            /// Search by VENDOR MARKUP PERCENT field
            /// </summary>
            VENDORMARKUPPERCENT,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTERENTITYID,
            VENDOR_ID_LIST
        }

        private int vendorId;
        private string name;
        private string remarks;
        private int defaultPaymentTermsId;
        private string address1;
        private string address2;
        private string city;
        private string state;
        private string country;
        private string postalCode;
        private string addressRemarks;
        private string contactName;
        private string phone;
        private string fax;
        private string email;
        private string lastModUserId;
        private DateTime lastModDttm;
        private bool isActive;
        private string website;
        private string taxRegistrationNumber;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private string vendorCode;
        private int masterEntityId; //Added 10-Aug-2016
        //int taxId;
        private double vendorMarkupPercent;
        private int countryId;
        private int stateId;
        private int purchaseTaxId;
        private string createdBy;
        private DateTime? creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        private string paymentTerms;
        private string goodsReturnPolicy;

        /// <summary>
        /// Default constructor
        /// </summary>
        public VendorDTO()
        {
            log.LogMethodEntry();
            vendorId = -1;
            defaultPaymentTermsId = -1;
            siteId = -1;
            isActive = true;
            masterEntityId = -1;
            countryId = -1;
            stateId = -1;
            purchaseTaxId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public VendorDTO(int vendorId, string name, string remarks, int defaultPaymentTermsId, string address1, string address2,
                         string city, string state, string country, string postalCode, string addressRemarks, string contactName,
                         string phone, string fax, string email, string lastModUserId, DateTime lastModDttm, bool isActive,
                         string website, string taxRegistrationNumber, string vendorCode, double vendorMarkupPercent, int countryId,
                         int stateId, int purchaseTaxId, string paymentTerms, string goodsReturnPolicy)
            : this()
        {
            log.LogMethodEntry(vendorId, name, remarks, defaultPaymentTermsId, address1, address2, city, state, country, postalCode, addressRemarks, contactName,
                          phone, fax, email, lastModUserId, lastModDttm, isActive, website, taxRegistrationNumber, vendorCode,
                          vendorMarkupPercent, countryId, stateId, purchaseTaxId, paymentTerms, goodsReturnPolicy);
            this.vendorId = vendorId;
            this.name = name;
            this.remarks = remarks;
            this.defaultPaymentTermsId = defaultPaymentTermsId;
            this.address1 = address1;
            this.address2 = address2;
            this.city = city;
            this.state = state;
            this.country = country;
            this.stateId = stateId;
            this.countryId = countryId;
            this.postalCode = postalCode;
            this.addressRemarks = addressRemarks;
            this.contactName = contactName;
            this.phone = phone;
            this.fax = fax;
            this.email = email;
            this.lastModUserId = lastModUserId;
            this.lastModDttm = lastModDttm;
            this.isActive = isActive;
            this.website = website;
            this.taxRegistrationNumber = taxRegistrationNumber;
            this.vendorCode = vendorCode;
            this.vendorMarkupPercent = vendorMarkupPercent;
            this.purchaseTaxId = purchaseTaxId;
            this.paymentTerms = paymentTerms;
            this.goodsReturnPolicy = goodsReturnPolicy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public VendorDTO(int vendorId, string name, string remarks, int defaultPaymentTermsId, string address1, string address2, string city,
                         string state, string country, string postalCode, string addressRemarks, string contactName, string phone,
                         string fax, string email, string lastModUserId, DateTime lastModDttm, bool isActive, string website,
                         string taxRegistrationNumber, int siteId, string guid, bool synchStatus, string vendorCode, int masterEntityId,
                         double vendorMarkupPercent, int countryId, int stateId, int purchaseTaxId, string createdBy, DateTime creationDate,
                         string lastUpdatedBy, DateTime lastUpdateDate, string paymentTerms, string goodsReturnPolicy)
            : this(vendorId, name, remarks, defaultPaymentTermsId, address1, address2, city, state, country, postalCode, addressRemarks, contactName,
                          phone, fax, email, lastModUserId, lastModDttm, isActive, website, taxRegistrationNumber, vendorCode,
                          vendorMarkupPercent, countryId, stateId, purchaseTaxId, paymentTerms, goodsReturnPolicy)
        {
            log.LogMethodEntry(vendorId, name, remarks, defaultPaymentTermsId, address1, address2, city, state, country, postalCode, addressRemarks, contactName,
                          phone, fax, email, lastModUserId, lastModDttm, isActive, website, taxRegistrationNumber, siteId, guid, synchStatus, vendorCode, masterEntityId,
                          vendorMarkupPercent, countryId, stateId, purchaseTaxId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId; //Added 10-Aug-2016
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get method of the VendorId field
        /// </summary>
        [DisplayName("VendorId")]
        [ReadOnly(true)]
        public int VendorId { get { return vendorId; } set { vendorId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the DefaultPaymentTermsId field
        /// </summary>
        [DisplayName("DefaultPaymentTermsId")]
        public int DefaultPaymentTermsId { get { return defaultPaymentTermsId; } set { defaultPaymentTermsId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Address1 field
        /// </summary>
        [DisplayName("Address1")]
        public string Address1 { get { return address1; } set { address1 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Address2 field
        /// </summary>
        [DisplayName("Address2")]
        public string Address2 { get { return address2; } set { address2 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the City field
        /// </summary>
        [DisplayName("City")]
        public string City { get { return city; } set { city = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the State field
        /// </summary>
        [DisplayName("State")]
        public string State { get { return state; } set { state = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the StateId field
        /// </summary>
        [DisplayName("StateId")]
        public int StateId { get { return stateId; } set { stateId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Country field
        /// </summary>
        [DisplayName("Country")]
        public string Country { get { return country; } set { country = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CountryId field
        /// </summary>
        [DisplayName("CountryId")]
        public int CountryId { get { return countryId; } set { countryId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the PostalCode field
        /// </summary>
        [DisplayName("PostalCode")]
        public string PostalCode { get { return postalCode; } set { postalCode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the AddressRemarks field
        /// </summary>
        [DisplayName("AddressRemarks")]
        public string AddressRemarks { get { return addressRemarks; } set { addressRemarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ContactName field
        /// </summary>
        [DisplayName("ContactName")]
        public string ContactName { get { return contactName; } set { contactName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Phone field
        /// </summary>
        [DisplayName("Phone")]
        public string Phone { get { return phone; } set { phone = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Fax field
        /// </summary>
        [DisplayName("Fax")]
        public string Fax { get { return fax; } set { fax = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Email field
        /// </summary>
        [DisplayName("Email")]
        public string Email { get { return email; } set { email = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastModUserId field
        /// </summary>
        [DisplayName("LastModUserId")]
        public string LastModUserId { get { return lastModUserId; } set { lastModUserId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastModDttm field
        /// </summary>
        [DisplayName("LastModDttm")]
        public DateTime LastModDttm { get { return lastModDttm; } set { lastModDttm = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the createdBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        public DateTime? CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Website field
        /// </summary>
        [DisplayName("Website")]
        public string Website { get { return website; } set { website = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the TaxRegistrationNumber field
        /// </summary>
        [DisplayName("TaxRegistrationNumber")]
        public string TaxRegistrationNumber { get { return taxRegistrationNumber; } set { taxRegistrationNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get method of the VendorCode field
        /// </summary>
        [DisplayName("VendorCode")]
        public string VendorCode { get { return vendorCode; } set { vendorCode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        // Start update 10-Aug-2016
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the VendorMarkupPercent field
        /// </summary>
        [DisplayName("VendorMarkupPercent")]
        public double VendorMarkupPercent { get { return vendorMarkupPercent; } set { vendorMarkupPercent = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the   PurchaseTax Id   field -01-04-2019
        /// </summary>
        [DisplayName("PurchaseTaxId ")]
        public int PurchaseTaxId { get { return purchaseTaxId; } set { purchaseTaxId = value; IsChanged = true; } }
        // End update 10-Aug-2016

        /// <summary>
        /// Get/Set method of the   LastUpdatedBy Id   field -25-07-2019
        /// </summary>
        [DisplayName("LastUpdatedBy ")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the   LastUpdateDate   field -25-07-2019
        /// </summary>
        [DisplayName("LastUpdateDate ")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        public string GoodsReturnPolicy { get { return goodsReturnPolicy; } set { goodsReturnPolicy = value; this.IsChanged = true; } }
        public string PaymentTerms { get { return paymentTerms; } set { paymentTerms = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || vendorId < 0;
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

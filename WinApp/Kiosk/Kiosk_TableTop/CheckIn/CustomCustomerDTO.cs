/********************************************************************************************
 * Project Name - CustomCustomerDTO
 * Description  - Data object of CustomCustomerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the CustomCustomer data object class. This acts as data holder for the CustomCustomer business object
    /// </summary>
    public class CustomCustomerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities = KioskStatic.Utilities;
        CustomerRelationshipDTO customerRelationshipDTO = new CustomerRelationshipDTO();

        public CustomCustomerDTO(int customerId)
        {
            log.LogMethodEntry();
            this.ParentCustomerId = customerId;
            this.RelatedCustomerId = -1;
            this.RelatedCustomerName = string.Empty;
            this.CustomerRelationshipTypeId = -1;
            this.DOB = null;
            this.ContactPhone = string.Empty;
            this.EmailAddress = string.Empty;
            this.UniqueIdentifier = string.Empty;
            this.Gender = string.Empty;
            this.Address1 = string.Empty;
            this.Address2 = string.Empty;
            this.Address3 = string.Empty;
            this.City = string.Empty;
            this.State = string.Empty;
            this.Country = string.Empty;
            this.PostalCode = string.Empty;
            this.Anniversary = Utilities.getDateFormat();
            this.LastName = string.Empty;
            this.Title = string.Empty;
            this.MembershipId = -1;
            this.MiddleName = string.Empty;
            this.AddressType = AddressType.NONE;
            this.CustomerType = CustomerType.UNREGISTERED;
            this.OptInPromotionsMode = "None";
            this.PhotoURL = string.Empty;
            this.customerRelationshipDTO = new CustomerRelationshipDTO();
            this.customerRelationshipDTO.RelatedCustomerDTO = new CustomerDTO();
            this.customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList = new List<AddressDTO>();
            log.LogMethodExit();
        }

        public CustomCustomerDTO(CustomerRelationshipDTO inputCustomerRelationshipDTO)
        {
            log.LogMethodEntry(inputCustomerRelationshipDTO);
            this.customerRelationshipDTO = inputCustomerRelationshipDTO;
            this.ParentCustomerId = inputCustomerRelationshipDTO.CustomerDTO.Id;
            this.RelatedCustomerId = inputCustomerRelationshipDTO.RelatedCustomerId;
            this.CustomerRelationshipTypeId = inputCustomerRelationshipDTO.CustomerRelationshipTypeId;
            this.RelatedCustomerName = inputCustomerRelationshipDTO.RelatedCustomerName;
            this.DOB = inputCustomerRelationshipDTO.RelatedCustomerDTO.DateOfBirth;
            this.ContactPhone = inputCustomerRelationshipDTO.RelatedCustomerDTO.PhoneNumber;
            this.EmailAddress = inputCustomerRelationshipDTO.RelatedCustomerDTO.Email;
            this.UniqueIdentifier = inputCustomerRelationshipDTO.RelatedCustomerDTO.UniqueIdentifier;
            this.Gender = inputCustomerRelationshipDTO.RelatedCustomerDTO.Gender;
            this.Address1 = inputCustomerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.Line1;
            this.Address2 = inputCustomerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.Line2;
            this.Address3 = inputCustomerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.Line3;
            this.City = inputCustomerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.City;
            this.State = inputCustomerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.StateId.ToString();
            this.Country = inputCustomerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.CountryName;
            this.PostalCode = inputCustomerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.PostalCode;
            this.Anniversary = inputCustomerRelationshipDTO.RelatedCustomerDTO.Anniversary.ToString();
            this.LastName = inputCustomerRelationshipDTO.RelatedCustomerDTO.LastName;
            this.Title = inputCustomerRelationshipDTO.RelatedCustomerDTO.Title;
            this.MembershipId = inputCustomerRelationshipDTO.RelatedCustomerDTO.MembershipId;
            this.MiddleName = inputCustomerRelationshipDTO.RelatedCustomerDTO.MiddleName;
            this.AddressType = inputCustomerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.AddressType;
            this.CustomerType = inputCustomerRelationshipDTO.RelatedCustomerDTO.CustomerType;
            this.OptInPromotionsMode = inputCustomerRelationshipDTO.RelatedCustomerDTO.OptInPromotionsMode;
            this.PhotoURL = inputCustomerRelationshipDTO.RelatedCustomerDTO.PhotoURL;
            log.LogMethodExit();
        }

        public CustomCustomerDTO(CustomerDTO inputCustomerDTO)
        {
            log.LogMethodEntry(inputCustomerDTO);

            this.ParentCustomerId = -1;
            this.RelatedCustomerId = -1;
            this.CustomerRelationshipTypeId = -1;
            this.RelatedCustomerName = inputCustomerDTO.FirstName;
            this.DOB = inputCustomerDTO.DateOfBirth;
            this.ContactPhone = inputCustomerDTO.PhoneNumber;
            this.EmailAddress = inputCustomerDTO.Email;
            this.UniqueIdentifier = inputCustomerDTO.UniqueIdentifier;
            this.Gender = inputCustomerDTO.Gender;
            this.Address1 = inputCustomerDTO.LatestAddressDTO.Line1;
            this.Address2 = inputCustomerDTO.LatestAddressDTO.Line2;
            this.Address3 = inputCustomerDTO.LatestAddressDTO.Line3;
            this.City = inputCustomerDTO.LatestAddressDTO.City;
            this.State = inputCustomerDTO.LatestAddressDTO.StateId.ToString();
            this.Country = inputCustomerDTO.LatestAddressDTO.CountryName;
            this.PostalCode = inputCustomerDTO.LatestAddressDTO.PostalCode;
            this.Anniversary = inputCustomerDTO.Anniversary.ToString();
            this.LastName = inputCustomerDTO.LastName;
            this.Title = inputCustomerDTO.Title;
            this.MembershipId = inputCustomerDTO.MembershipId;
            this.MiddleName = inputCustomerDTO.MiddleName;
            this.AddressType = inputCustomerDTO.LatestAddressDTO.AddressType;
            this.CustomerType = inputCustomerDTO.CustomerType;
            this.OptInPromotionsMode = inputCustomerDTO.OptInPromotionsMode;
            this.PhotoURL = inputCustomerDTO.PhotoURL;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the FirstName Text field
        /// </summary>
        [DisplayName("Name")]
        public string RelatedCustomerName
        {
            get
            {
                return customerRelationshipDTO.RelatedCustomerDTO.FirstName == null ? "" : customerRelationshipDTO.RelatedCustomerDTO.FirstName;
            }

            set
            {
                customerRelationshipDTO.RelatedCustomerDTO.FirstName = value;
            }
        }
        // <summary>
        // Get/Set method of the Relationship Type Name field
        // </summary>
        [DisplayName("Relationship Type Name")]
        public string RelationshipTypeName
        {
            get
            {
                string relationshipName;
                CustomerRelationshipTypeBL customerRelationshipTypeBL = new CustomerRelationshipTypeBL(Utilities.ExecutionContext, customerRelationshipDTO.CustomerRelationshipTypeId, null);
                relationshipName = customerRelationshipTypeBL.CustomerRelationshipTypeDTO.Name;
                return relationshipName;
            }
            set { }
        }
        /// <summary>
        /// Get/Set method of the DateOfBirth Text field
        /// </summary>
        [DisplayName("Date Of Birth")]
        public DateTime? DOB
        {
            get
            {
                return CustomerRelationshipDTO.RelatedCustomerDTO.DateOfBirth;
            }

            set
            {
                customerRelationshipDTO.RelatedCustomerDTO.DateOfBirth = value;
            }
        }
        /// <summary>
        /// Get/Set method of the DateOfBirth Text field
        /// </summary>
        [DisplayName("Age")]
        public int Age
        {
            get
            {
                CustomerBL customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, CustomerRelationshipDTO.RelatedCustomerDTO);
                int Age = customerBL.GetAge();
                return Age < 0 ? 0 : Age;
            }
        }
        /// <summary>
        /// Get/Set method of the Phone Number field
        /// </summary>
        [DisplayName("Phone Num")]
        public string ContactPhone
        {
            get
            {
                return customerRelationshipDTO.RelatedCustomerDTO.PhoneNumber;
            }
            set
            {
                ContactDTO contactPhone1DTO = null;
                if (customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList != null
                    && customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList.Any())
                {
                    var orderedContactList = customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList.OrderByDescending((x) => x.LastUpdateDate);
                    var orderedContactPhone = orderedContactList.Where(s => s.ContactType == ContactType.PHONE);
                    if (orderedContactPhone.Any())
                    {
                        contactPhone1DTO = orderedContactPhone.First();
                        contactPhone1DTO.Attribute1 = value;
                    }
                    else
                    { 
                        if (string.IsNullOrWhiteSpace(value) == false)
                        {
                            contactPhone1DTO = new ContactDTO();
                            contactPhone1DTO.ContactType = ContactType.PHONE;
                            contactPhone1DTO.Attribute1 = value;
                            if (customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList == null)
                            {
                                customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList = new List<ContactDTO>();
                            }
                            customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList.Add(contactPhone1DTO);
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(value) == false)
                    {
                        contactPhone1DTO = new ContactDTO();
                        contactPhone1DTO.ContactType = ContactType.PHONE;
                        contactPhone1DTO.Attribute1 = value;
                        if (customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList == null)
                        {
                            customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList = new List<ContactDTO>();
                        }
                        customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList.Add(contactPhone1DTO);
                    }                        
                }
            }
        }

        /// <summary>
        /// Get/Set method of the Email field
        /// </summary>
        [DisplayName("Email")]
        public string EmailAddress
        {
            get
            {
                return customerRelationshipDTO.RelatedCustomerDTO.Email;
            }
            set
            {
                ContactDTO contactEmailDTO = null;
                if (customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList != null
                    && customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList.Any())
                {
                    var orderedContactList = customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList.OrderByDescending((x) => x.LastUpdateDate);
                    var orderedContactEmail = orderedContactList.Where(s => s.ContactType == ContactType.EMAIL);
                    if (orderedContactEmail.Any())
                    {
                        contactEmailDTO = orderedContactEmail.First();
                        contactEmailDTO.Attribute1 = value;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(value) == false)
                        {
                            contactEmailDTO = new ContactDTO();
                            contactEmailDTO.ContactType = ContactType.EMAIL;
                            contactEmailDTO.Attribute1 = value;
                            if (customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList == null)
                            {
                                customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList = new List<ContactDTO>();
                            }
                            customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList.Add(contactEmailDTO);
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(value) == false)
                    {
                        contactEmailDTO = new ContactDTO();
                        contactEmailDTO.ContactType = ContactType.EMAIL;
                        contactEmailDTO.Attribute1 = value;
                        if (customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList == null)
                        {
                            customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList = new List<ContactDTO>();
                        }
                        customerRelationshipDTO.RelatedCustomerDTO.ContactDTOList.Add(contactEmailDTO);
                    }
                }
            }
        }

        /// <summary>
        /// Get/Set method of the UniqueIdentifier Text field
        /// </summary>
        [DisplayName("Unique Id")]
        public string UniqueIdentifier
        {
            get
            {
                if (customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO != null)
                {
                    return customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.UniqueIdentifier == null ? "" : customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.UniqueIdentifier;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO != null)
                {
                    customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.UniqueIdentifier = value;
                }
            }
        }
        /// <summary>
        /// Get/Set method of the Gender Text field
        /// </summary>
        [DisplayName("Gender")]
        public string Gender
        {
            get
            {
                return customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO == null ? "" : customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.Gender;
            }

            set
            {
                if (customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO != null)
                {
                    switch (value)
                    {
                        case "Male":
                            customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.Gender = "M";
                            break;
                        case "Female":
                            customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.Gender = "F";
                            break;
                        case "Not Set":
                            customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.Gender = "N";
                            break;
                        default: break;
                    }
                }
            }
        }
        /// <summary>
        /// Returns the Address1
        /// </summary>
        [DisplayName("Address1")]
        public string Address1
        {
            get
            {
                string value = customerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.Line1;
                return value == null ? "" : value;
            }
            set
            {
                AddressDTO addressDTO = null;
                if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList != null
                    && customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Any())
                {
                    var orderedAddressList = customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.OrderByDescending((x) => x.LastUpdateDate);
                    addressDTO = orderedAddressList.First();
                    addressDTO.Line1 = value;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(value) == false)
                    {
                        addressDTO = new AddressDTO();
                        if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList == null)
                        {
                            customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList = new List<AddressDTO>();
                        }
                        customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Add(addressDTO);
                        addressDTO.Line1 = value;
                    }
                }
            }
        }
        /// <summary>
        /// Returns Address2
        /// </summary>
        [DisplayName("Address2")]
        public string Address2
        {
            get
            {
                string value = customerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.Line2;
                return value == null ? "" : value;
            }
            set
            {
                AddressDTO addressDTO = null;
                if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList != null
                    && customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Any())
                {
                    var orderedAddressList = customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.OrderByDescending((x) => x.LastUpdateDate);
                    addressDTO = orderedAddressList.First();
                    addressDTO.Line2 = value;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(value) == false)
                    {
                        addressDTO = new AddressDTO();
                        if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList == null)
                        {
                            customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList = new List<AddressDTO>();
                        }
                        customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Add(addressDTO);
                        addressDTO.Line2 = value;
                    }
                }
            }
        }
        /// <summary>
        /// Returns Address3
        /// </summary>
        [DisplayName("Address3")]
        public string Address3
        {
            get
            {
                string value = customerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.Line3;
                return value == null ? "" : value;
            }
            set
            {
                AddressDTO addressDTO = null;
                if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList != null
                    && customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Any())
                {
                    var orderedAddressList = customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.OrderByDescending((x) => x.LastUpdateDate);
                    addressDTO = orderedAddressList.First();
                    addressDTO.Line3 = value;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(value) == false)
                    {
                        addressDTO = new AddressDTO();
                        if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList == null)
                        {
                            customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList = new List<AddressDTO>();
                        }
                        customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Add(addressDTO);
                        addressDTO.Line3 = value;
                    }
                }
            }
        }
        /// <summary>
        /// Returns City from AddressDTO
        /// </summary>
        [DisplayName("City")]
        public string City
        {
            get
            {
                string value = customerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.City;
                return value == null ? "" : value;
            }
            set
            {
                AddressDTO addressDTO = null;
                if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList != null
                    && customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Any())
                {
                    var orderedAddressList = customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.OrderByDescending((x) => x.LastUpdateDate);
                    addressDTO = orderedAddressList.First();
                    addressDTO.City = value;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(value) == false)
                    {
                        addressDTO = new AddressDTO();
                        if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList == null)
                        {
                            customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList = new List<AddressDTO>();
                        }
                        customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Add(addressDTO);
                        addressDTO.City = value;
                    }
                }
            }
        }
        /// <summary>
        /// Returns State
        /// </summary>
        [DisplayName("State")]
        public string State
        {
            get
            {
                string stateName = null;
                List<CountryContainerDTO> countryContainerList = CountryContainerList.GetCountryContainerDTOList(KioskStatic.Utilities.ExecutionContext.SiteId);
                string stateCountryId = ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext.GetExecutionContext(), "STATE_LOOKUP_FOR_COUNTRY");
                if (stateCountryId == "-1")
                    stateCountryId = customerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.CountryId.ToString();
                if (!string.IsNullOrWhiteSpace(stateCountryId) && stateCountryId != "-1")
                {
                    int countryId = -1;
                    if ((int.TryParse(stateCountryId, out countryId) == true) && (countryContainerList.Exists(c => c.CountryId == countryId) == true))
                    {
                        List<StateContainerDTO> StateContainerDTOList = countryContainerList.Where(x => x.CountryId == countryId).FirstOrDefault().StateContainerDTOList;
                        var stateDTO = StateContainerDTOList.Where(x => x.StateId == customerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.StateId).FirstOrDefault();
                        if (stateDTO != null)
                        {
                            stateName = stateDTO.Description;
                        }
                    }
                }
                return stateName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    AddressDTO addressDTO = null;
                    if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList != null
                        && customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Any())
                    {
                        var orderedAddressList = customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.OrderByDescending((x) => x.LastUpdateDate);
                        addressDTO = orderedAddressList.First();
                        if (Convert.ToInt32(value) > -1)
                            addressDTO.StateId = Convert.ToInt32(value);
                    }
                    else
                    {
                        addressDTO = new AddressDTO();
                        if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList == null)
                        {
                            customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList = new List<AddressDTO>();
                        }
                        customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Add(addressDTO);
                        if (Convert.ToInt32(value) > -1)
                            addressDTO.StateId = Convert.ToInt32(value);
                    }
                }
            }
        }
        /// <summary>
        /// Returns Country
        /// </summary>
        [DisplayName("Country")]
        public string Country
        {
            get
            {
                string countryName = null;
                int countryId = customerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.CountryId;
                List<CountryContainerDTO> countryContainerList = CountryContainerList.GetCountryContainerDTOList(KioskStatic.Utilities.ExecutionContext.SiteId);
                var countryDTO = countryContainerList.Where(c => c.CountryId == countryId).FirstOrDefault();
                {
                    if (countryDTO != null)
                    {
                        countryName = countryDTO.CountryName;
                    }
                }
                return countryName;
            }
            set
            {
                AddressDTO addressDTO = null;
                if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList != null
                    && customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Any())
                {
                    var orderedAddressList = customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.OrderByDescending((x) => x.LastUpdateDate);
                    addressDTO = orderedAddressList.First();

                    List<CountryContainerDTO> countryContainerList = CountryContainerList.GetCountryContainerDTOList(Utilities.ExecutionContext.SiteId);
                    var countryDTO = countryContainerList.Where(c => c.CountryName == value).FirstOrDefault();
                    {
                        if (countryDTO != null)
                        {
                            addressDTO.CountryId = countryDTO.CountryId;
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(value) == false)
                    {
                        addressDTO = new AddressDTO();
                        if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList == null)
                        {
                            customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList = new List<AddressDTO>();
                        }
                        customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Add(addressDTO);
                        if (Convert.ToInt32(value) > -1)
                            addressDTO.CountryId = Convert.ToInt32(value);
                    }
                }
            }
        }
        /// <summary>
        /// Returns Postal Code
        /// </summary>
        [DisplayName("Postal Code")]
        public string PostalCode
        {
            get
            {
                string value = customerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.PostalCode;
                return value == null ? "" : value;
            }
            set
            {
                AddressDTO addressDTO = null;
                if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList != null
                    && customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Any())
                {
                    var orderedAddressList = customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.OrderByDescending((x) => x.LastUpdateDate);
                    addressDTO = orderedAddressList.First();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(value) == false)
                    {
                        addressDTO = new AddressDTO();
                        if (customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList == null)
                        {
                            customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList = new List<AddressDTO>();
                        }
                        customerRelationshipDTO.RelatedCustomerDTO.AddressDTOList.Add(addressDTO);
                    }
                }
                if (addressDTO != null)
                    addressDTO.PostalCode = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Anniversary Text field
        /// </summary>
        [DisplayName("Anniversary")]
        public string Anniversary
        {
            get
            {
                return CustomerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.Anniversary == null ? null : customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.Anniversary.Value.ToString(Utilities.getDateFormat());
            }

            set
            {
                if (value != Utilities.getDateFormat() && value != "")
                {
                    try
                    {
                        if (customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO != null)
                        {
                            customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.Anniversary = Convert.ToDateTime(value);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit();
                    }
                }
            }
        }
        /// <summary>
        /// Get/Set method of the LastName Text field
        /// </summary>
        [DisplayName("LastName")]
        public string LastName
        {
            get
            {
                return customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO == null ? "" : customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.LastName;
            }

            set
            {
                if (customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO != null)
                {
                    customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.LastName = value;
                }
            }
        }
        /// <summary>
        /// Get/Set method of the Title Text field
        /// </summary>
        [DisplayName("Title")]
        public string Title
        {
            get
            {
                return customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO == null ? "" : customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.Title;
            }

            set
            {
                if (customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO != null)
                {
                    customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.Title = value;
                }
            }
        }
        /// <summary>
        /// Get/Set method of the MembershipId field
        /// </summary>
        [DisplayName("Membership Id")]
        public int MembershipId
        {
            get
            {
                return customerRelationshipDTO.RelatedCustomerDTO.MembershipId;
            }

            set
            {
                customerRelationshipDTO.RelatedCustomerDTO.MembershipId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the MiddleName Text field
        /// </summary>
        [DisplayName("Middle Name")]
        public string MiddleName
        {
            get
            {
                return customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.MiddleName;
            }

            set
            {
                if (customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO != null)
                {
                    customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.MiddleName = value;
                }
            }
        }
        /// <summary>
        /// Get/Set method of the AddressType Text field
        /// </summary>
        [DisplayName("Address Type")]
        public AddressType AddressType
        {
            get
            {
                return customerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.AddressType;
            }
            set
            {
                customerRelationshipDTO.RelatedCustomerDTO.LatestAddressDTO.AddressType = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CustomerType Text field
        /// </summary>
        [DisplayName("Type")]
        public CustomerType CustomerType
        {
            get
            {
                return customerRelationshipDTO.RelatedCustomerDTO.CustomerType;
            }

            set
            {
                customerRelationshipDTO.RelatedCustomerDTO.CustomerType = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Parent Customer Id Text field
        /// </summary>
        [DisplayName("Parent Customer id")]
        public int ParentCustomerId
        {
            get
            {
                return customerRelationshipDTO.CustomerDTO.Id;
            }

            set
            {
                customerRelationshipDTO.CustomerDTO.Id = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Related Customer Id Text field
        /// </summary>
        [DisplayName("Related Customer id")]
        public int RelatedCustomerId
        {
            get
            {
                return customerRelationshipDTO.RelatedCustomerId;
            }

            set
            {
                customerRelationshipDTO.RelatedCustomerId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Customer Relationship Type Id Text field
        /// </summary>
        [DisplayName("Relationship")]
        public int CustomerRelationshipTypeId
        {
            get
            {
                return customerRelationshipDTO.CustomerRelationshipTypeId;
            }

            set
            {
                customerRelationshipDTO.CustomerRelationshipTypeId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Promotion mode field
        /// </summary>
        [DisplayName("Promotion mode")]
        public string OptInPromotionsMode
        {
            get
            {
                return customerRelationshipDTO.RelatedCustomerDTO.OptInPromotionsMode == null ? "None" : customerRelationshipDTO.RelatedCustomerDTO.OptInPromotionsMode;
            }

            set
            {
                customerRelationshipDTO.RelatedCustomerDTO.OptInPromotionsMode = value;
            }
        }
        /// <summary>
        /// Get/Set method of the PhotoURL Text field
        /// </summary>
        [DisplayName("Customer Photo")]
        public string PhotoURL
        {
            get
            {
                return customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.PhotoURL == null ? "" : customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.PhotoURL;
            }

            set
            {
                if (customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO != null)
                {
                    customerRelationshipDTO.RelatedCustomerDTO.ProfileDTO.PhotoURL = value;
                }
            }
        }
        /// <summary>
        /// Get/Set method of the customerRelationshipDTO field
        /// </summary>
        public CustomerRelationshipDTO CustomerRelationshipDTO
        {
            get
            {
                return customerRelationshipDTO;
            }

            set
            {
                customerRelationshipDTO = value;
            }
        }

        //everytime customer has to be fetched from BL because customerDTO may keep changing through editDOB option
        internal CustomerDTO GetCustomerDTO(int id)
        {
            log.LogMethodEntry();
            CustomerDTO customerDTO = null;
            try
            {
                if (id > 0)
                {
                    CustomerBL customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, id);
                    customerDTO = customerBL.CustomerDTO;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error: Error while getting parent customerDTO in PurchaseProductDTO: " + ex.Message);
            }
            log.LogMethodExit();
            return customerDTO;
        }
    }
}



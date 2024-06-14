/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the customer game play level results details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   Vignesh Bhat             Created : External  REST API.
 *2.150.5    31-Oct-2023   Abhishek                 Modified : Addition of CustomerImage field to return customer image.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.External
{
    
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Address
    {
        public string AddressType { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string PostalCode { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }

        public Address()
        {
            AddressType = String.Empty;
            Line1 = String.Empty;
            Line2 = String.Empty;
            Line3 = String.Empty;
            City = String.Empty;
            StateId = -1;
            CountryId = -1;
            PostalCode = String.Empty;
            StateCode = String.Empty;
            StateName = String.Empty;
            CountryName = String.Empty;
        }
        public Address(string AddressType, string Line1, string Line2, string Line3, string City, int StateId, int CountryId, string PostalCode,
            string StateCode, string StateName, string CountryName)
        {
            this.AddressType = AddressType;
            this.Line1 = Line1;
            this.Line2 = Line2;
            this.Line3 = Line3;
            this.City = City;
            this.StateId = StateId;
            this.CountryId = CountryId;
            this.PostalCode = PostalCode;
            this.StateCode = StateCode;
            this.StateName = StateName;
            this.CountryName = CountryName;
        }
    }

    public class Contact
    {
        public string Email { get; set; }
        public string Phone { get; set; }
       
        public Contact()
        {
            Email = String.Empty;
            Phone = String.Empty;

        }
        public Contact(string Email, string Phone)
        {
            this.Email = Email;
            this.Phone = Phone;

        }
    }

    public class ExternalCustomersDTO
    {
        public int Id { get; set; }
        public int MembershipId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string TaxCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public DateTime? Anniversary { get; set; }
        public string CardNumber { get; set; }
        public int SiteId { get; set; }
        public string NickName { get; set; }
        public string CustomerImage { get; set; }
        public List<Address> Address { get; set; }
        public List<Contact> Contact { get; set; }


        
        public ExternalCustomersDTO()
        {
            Id = -1;
            MembershipId = -1;
            Title = String.Empty;
            FirstName = String.Empty;
            MiddleName = String.Empty;
            LastName = String.Empty;
            TaxCode = String.Empty;
            DateOfBirth =null ;
            Gender = String.Empty;
            Anniversary = null;
            CardNumber = String.Empty;
            CustomerImage = String.Empty;
            SiteId = -1;
            Address = new List<Address>();
            Contact = new List<Contact>();
        }

        public ExternalCustomersDTO(int Id, int MembershipId, string Title, string FirstName, string MiddleName, string LastName,
           string TaxCode, DateTime? DateOfBirth, string Gender, DateTime? Anniversary, string CardNumber, int SiteId, string nickName, string customerImage,
           List<Address> Address, List<Contact> Contact)
        {
            this.Id = Id;
            this.MembershipId = MembershipId;
            this.Title = Title;
            this.FirstName = FirstName;
            this.MiddleName = MiddleName;
            this.LastName = LastName;
            this.TaxCode = TaxCode;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Anniversary = Anniversary;
            this.CardNumber = CardNumber;
            this.SiteId = SiteId;
            this.NickName = nickName;
            this.CustomerImage = customerImage;
            this.Address = Address;
            this.Contact = Contact;
        }
    }
}

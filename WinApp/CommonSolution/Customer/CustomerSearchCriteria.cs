/********************************************************************************************
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.60        04-Mar-2019     Mushahid Faizan     Modified - Added    "ISACTIVE" ,CustomerSearchByParameters
 *2.60        20-Jun-2019     Nitin Pai           Guest App Changes
 *2.70.2      19-Jul-2019     Girish Kundar       Modified : Added Master Entity Id as search Parameter 
 *2.70.2      01-Nov-2019     Akshay G            Modified : ClubSpeed enhancement changes - Added Profile_ExternalSystemReference as searchParameters
 *2.70.2      13-Dec-2019     Akshay G            Modified : ClubSpeed enhancement changes - Added CUSTOMER_ENTITY_LAST_UPDATE_DATE_GREATER_THAN as searchParameters
 *2.80        19-Jul-2019     Girish Kundar       Modified : Added Master Entity Id as search Parameter 
 *2.80        01-Nov-2019     Akshay G            Modified : ClubSpeed enhancement changes - Added Profile_ExternalSystemReference as searchParameters
 *2.80.0      25-Nov-2019     Girish kundar       Modified : Customer unique attribute search
 *2.70.3      14-Feb-2020     Lakshminarayana     Modified: Creating unregistered customer during check-in process
 *2.140.0     14-Sep-2021     Guru S A            Waiver mapping UI enhancements
 *2.150.0     22-Dec-2022     Abhishek            Added parameters waivercode, channel as part of waiver.
 ********************************************************************************************/
using System;
using System.Collections.Generic; 
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
    /// </summary>
    /// 
    public enum CustomerSearchByParameters
    {
        /// <summary>
        /// Search by  id field
        /// </summary>
        CUSTOMER_ID,

        /// <summary>
        /// Search by profile id field
        /// </summary>
        CUSTOMER_PROFILE_ID,
        /// <summary>
        /// Search by membership id field
        /// </summary>
        CUSTOMER_MEMBERSHIP_ID,
        /// <summary>
        /// search by channel field
        /// </summary>
        CUSTOMER_CHANNEL,
        /// <summary>
        /// search by external system reference field
        /// </summary>
        CUSTOMER_EXTERNAL_SYSTEM_REFERENCE,
        /// <summary>
        /// search by custom dataset field
        /// </summary>
        CUSTOMER_CUSTOM_DATA_SET_ID,
        /// <summary>
        /// search by verified field
        /// </summary>
        CUSTOMER_VERIFIED,
        /// <summary>
        /// search by created by field
        /// </summary>
        CUSTOMER_CREATED_BY,
        /// <summary>
        /// search by creation date field
        /// </summary>
        CUSTOMER_CREATION_DATE,
        /// <summary>
        /// search by last updated by field
        /// </summary>
        CUSTOMER_LAST_UPDATED_BY,
        /// <summary>
        /// search by last updated date field
        /// </summary>
        CUSTOMER_LAST_UPDATE_DATE,
        /// <summary>
        /// search by site id field
        /// </summary>
        CUSTOMER_SITE_ID,
        /// <summary>
        /// search by customer type field
        /// </summary>
        CUSTOMER_TYPE,
        /// <summary>
        /// search by profile type
        /// </summary>
        PROFILE_PROFILE_TYPE,
        /// <summary>
        /// search by first name field
        /// </summary>
        PROFILE_FIRST_NAME,
        /// <summary>
        /// search by middle name field
        /// </summary>
        PROFILE_MIDDLE_NAME,
        /// <summary>
        /// search by last name field
        /// </summary>
        PROFILE_LAST_NAME,
        /// <summary>
        /// search by title field
        /// </summary>
        PROFILE_TITLE,
        /// <summary>
        /// search by notes field
        /// </summary>
        PROFILE_NOTES,
        /// <summary>
        /// search by date of birth fields
        /// </summary>
        PROFILE_DATE_OF_BIRTH,
        /// <summary>
        /// search by gender field
        /// </summary>
        PROFILE_GENDER,
        /// <summary>
        /// search by anniversary field
        /// </summary>
        PROFILE_ANNIVERSARY,
        /// <summary>
        /// search by unique identifier field
        /// </summary>
        PROFILE_UNIQUE_IDENTIFIER,
        /// <summary>
        /// search by photo URL field
        /// </summary>
        PROFILE_PHOTO_URL,
        /// <summary>
        /// search by tax code field
        /// </summary>
        PROFILE_TAX_CODE,
        /// <summary>
        /// search by company field
        /// </summary>
        PROFILE_COMPANY,
        /// <summary>
        /// search by designation field
        /// </summary>
        PROFILE_DESIGNATION,
        /// <summary>
        /// search by user name field
        /// </summary>
        PROFILE_USER_NAME,
        /// <summary>
        /// search by password field
        /// </summary>
        PROFILE_PASSWORD,
        /// <summary>
        /// search by last login time field
        /// </summary>
        PROFILE_LAST_LOGIN_TIME,

        /// <summary>
        /// search by address type field
        /// </summary>
        ADDRESS_ADDRESS_TYPE,
        /// <summary>
        /// search by address line 1 field
        /// </summary>
        ADDRESS_LINE1,
        /// <summary>
        /// search by address line 2 field
        /// </summary>
        ADDRESS_LINE2,
        /// <summary>
        /// search by address line 3 field
        /// </summary>
        ADDRESS_LINE3,
        /// <summary>
        /// search by address city field
        /// </summary>
        ADDRESS_CITY,
        /// <summary>
        /// search by address country id field
        /// </summary>
        ADDRESS_COUNTRY_ID,
        /// <summary>
        /// search by address site id field
        /// </summary>
        ADDRESS_STATE_ID,
        /// <summary>
        /// search by address postal code field
        /// </summary>
        ADDRESS_POSTAL_CODE,

        /// <summary>
        /// search by contact type field
        /// </summary>
        CONTACT_CONTACT_TYPE,
        /// <summary>
        /// search by address attribute 1 field 
        /// </summary>
        CONTACT_ATTRIBUTE1,
        /// <summary>
        /// search by address attribute 2 field
        /// </summary>
        CONTACT_ATTRIBUTE2,
        /// <summary>
        ///   search by isActive Parameters
        /// </summary>
        ISACTIVE,
        /// <summary>
        ///   search by phone number list Parameters
        /// </summary>
        PHONE_NUMBER_LIST,
        /// <summary>
        ///   search by email list  Parameters
        /// </summary>
        EMAIL_LIST,
        /// <summary>
        ///   search by we chat token list   Parameters
        /// </summary>
        WECHAT_ACCESS_TOKEN_LIST,
        /// <summary>
        ///   search by Master Entity Id Parameters
        /// </summary>
        MASTER_ENTITY_ID,
        /// <summary>
        ///   search by Profile ExternalSystemReference Parameters
        /// </summary>
        PROFILE_EXTERNAL_SYSTEM_REFERENCE,
        /// <summary>
        /// Search by  GUID field
        /// </summary>
        CUSTOMER_GUID,
        /// <summary>
        /// search by contact is active field
        /// </summary>
        CONTACT_IS_ACTIVE,
        /// <summary>
        /// search by address is active field
        /// </summary>
        ADDRESS_IS_ACTIVE,
        /// <summary>
        /// Search by  CUSTOMER_ID_IN field
        /// </summary>
        CUSTOMER_ID_IN,
        /// <summary>
        /// Search by CUSTOMER_LAST_UPDATE_FROM_DATE field
        /// </summary>
        CUSTOMER_LAST_UPDATE_FROM_DATE,
        /// <summary>
        /// Search by CUSTOMER_LAST_UPDATE_TO_DATE field
        /// </summary>
        CUSTOMER_LAST_UPDATE_TO_DATE,
        /// <summary>
        /// Search by CUSTOMER_ENTITY_LAST_UPDATE_DATE_GREATER_THAN field // Customer entity lastupdatedate greater than
        /// </summary>
        CUSTOMER_ENTITY_LAST_UPDATE_DATE_GREATER_THAN,
        /// <summary>
        /// search by user name or Email field
        /// </summary>
        PROFILE_USER_NAME_OR_EMAIL,
        /// <summary>
        /// search by address is active field
        /// </summary>
        CONTACT_PHONE_OR_EMAIL,
        /// <summary>
        /// Search by  FB USERID field
        /// </summary>
        FB_USERID_LIST,
        /// <summary>
        /// Search by  TW ACCESS TOKEN field
        /// </summary>
        TW_ACCESS_TOKEN_LIST,
        /// <summary>
        /// Search by  FB_ACCESS_TOKEN_LIST
        /// </summary>
        FB_ACCESS_TOKEN_LIST,
        /// <summary>
        /// Search by TW ACCESS SECRET field
        /// </summary>
        TW_ACCESS_SECRET_LIST,
        /// <summary>
        /// search by date of birth field
        /// </summary>
        IS_ADULT,
        /// <summary>
        /// search by address attribute 1 masked field 
        /// </summary>
        CONTACT_ATTRIBUTE1_MASKED,
        /// <summary>
        /// search by has signed waivers field
        /// </summary>
        HAS_SIGNED_WAIVERS,
        /// <summary>
        /// search by waiver is mapped to trx field
        /// </summary>
        WAIVER_IS_MAPPED_TO_TRX,
        /// <summary>
        /// search by latest customer to sign waiver field
        /// </summary>
        LATEST_TO_SIGN_WAIVER,
        /// <summary>
        /// search by channel used to sign waiver field
        /// </summary>
        CHANNEL_USED_TO_SIGN_WAIVER,
        /// <summary>
        /// search by waiver id list field
        /// </summary>
        HAS_SIGNED_WAIVER_ID_LIST,
        /// <summary>
        /// search by first name field
        /// </summary>
        PROFILE_FIRST_NAME_LIKE,
        /// <summary>
        /// search by middle name field
        /// </summary>
        PROFILE_MIDDLE_NAME_LIKE,
        /// <summary>
        /// search by last name field
        /// </summary>
        PROFILE_LAST_NAME_LIKE,
        /// <summary>
        /// search by Waiver Code field
        /// </summary>
        WAIVER_CODE,
        /// <summary>
        /// search by Channel field
        /// </summary>
        CHANNEL

    }
    class CustomerColumnProvider : ColumnProvider
    {
        internal CustomerColumnProvider()
        {
            columnDictionary = new Dictionary<Enum, Column>() { 
                    { CustomerSearchByParameters.CUSTOMER_ID, new NumberColumn("customers.customer_id", "Customer Id") },
                    { CustomerSearchByParameters.CUSTOMER_PROFILE_ID, new NumberColumn("customers.ProfileId", "Profile Id")},
                    { CustomerSearchByParameters.CUSTOMER_MEMBERSHIP_ID, new NumberColumn("customers.MembershipId", "Membership Id")},
                    { CustomerSearchByParameters.CUSTOMER_CHANNEL, new TextColumn("customers.channel", "Channel")},
                    { CustomerSearchByParameters.CUSTOMER_EXTERNAL_SYSTEM_REFERENCE, new TextColumn("customers.ExternalSystemRef", "External System Reference")},
                    { CustomerSearchByParameters.CUSTOMER_CUSTOM_DATA_SET_ID, new NumberColumn("customers.CustomDataSetId", "Custom Data Set Id")},
                    { CustomerSearchByParameters.CUSTOMER_VERIFIED, new TextColumn("customers.Verified", "Verified", "'N'")},
                    { CustomerSearchByParameters.CUSTOMER_CREATED_BY, new TextColumn("customers.CreatedBy", "Created By")},
                    { CustomerSearchByParameters.CUSTOMER_CREATION_DATE, new DateTimeColumn("customers.CreationDate", "Creation Date")},
                    { CustomerSearchByParameters.CUSTOMER_LAST_UPDATED_BY, new TextColumn("customers.last_updated_user", "Last Updated By")},
                    { CustomerSearchByParameters.CUSTOMER_LAST_UPDATE_DATE, new DateTimeColumn("customers.last_updated_date", "Last Updated Date")},
                    { CustomerSearchByParameters.CUSTOMER_SITE_ID, new NumberColumn("customers.site_id", "Site Id")},
                    { CustomerSearchByParameters.PROFILE_PROFILE_TYPE, new TextColumn("ProfileType.Name", "Profile Type")},
                    { CustomerSearchByParameters.PROFILE_FIRST_NAME, new TextColumn("Profile.FirstName", "First Name")},
                    { CustomerSearchByParameters.PROFILE_MIDDLE_NAME, new TextColumn("Profile.MiddleName","Middle Name")},
                    { CustomerSearchByParameters.PROFILE_LAST_NAME, new TextColumn("Profile.LastName", "Last Name")},
                    { CustomerSearchByParameters.PROFILE_TITLE, new TextColumn("Profile.Title", "Title")},
                    { CustomerSearchByParameters.PROFILE_NOTES, new TextColumn("Profile.Notes", "Notes")},
                   //{ CustomerSearchByParameters.PROFILE_DATE_OF_BIRTH, new DateTimeColumn("CONVERT(DateTime,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.DateOfBirth)))", "Date Of Birth")},
                    { CustomerSearchByParameters.PROFILE_DATE_OF_BIRTH, new EncryptedDateTimeColumn("CONVERT(DateTime,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.DateOfBirth)))",
                                                                            "Profile.HashDateOfBirth", "Date Of Birth")},
                    { CustomerSearchByParameters.PROFILE_GENDER, new TextColumn("Profile.Gender", "Gender")},
                   //{CustomerSearchByParameters.PROFILE_ANNIVERSARY, new DateTimeColumn("CONVERT(DateTime,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.Anniversary)))", "Anniversary")},
                    { CustomerSearchByParameters.PROFILE_ANNIVERSARY, new EncryptedDateTimeColumn("CONVERT(DateTime,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.Anniversary)))",
                       "Profile.HashAnniversary", "Anniversary")},
                   // { CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.UniqueId))", "Unique Identifier")},
                    { CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, new EncryptedTextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.UniqueId))",
                    "Profile.HashUniqueId", "Unique Identifier")},
                    { CustomerSearchByParameters.PROFILE_PHOTO_URL, new TextColumn("Profile.PhotoURL", "Photo URL")},
                    //{ CustomerSearchByParameters.PROFILE_TAX_CODE, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.TaxCode))", "Tax Code")},
                    { CustomerSearchByParameters.PROFILE_TAX_CODE, new EncryptedTextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.TaxCode))",
                    "Profile.HashTaxCode","Tax Code")},
                    { CustomerSearchByParameters.PROFILE_COMPANY, new TextColumn("Profile.Company", "Company")},
                    { CustomerSearchByParameters.PROFILE_DESIGNATION, new TextColumn("Profile.Designation", "Designation")},
                    //{ CustomerSearchByParameters.PROFILE_USER_NAME, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.Username))", "User name")},
                    { CustomerSearchByParameters.PROFILE_USER_NAME, new EncryptedTextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.Username))",
                    "Profile.HashUsername","User name")},
                    //{ CustomerSearchByParameters.PROFILE_USER_NAME_OR_EMAIL, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.Username))", "User name or EMail ")},
                    { CustomerSearchByParameters.PROFILE_USER_NAME_OR_EMAIL, new EncryptedTextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.Username))",
                    "Profile.HashUsername", "User name or EMail ")},
                    { CustomerSearchByParameters.PROFILE_PASSWORD, new TextColumn("Profile.Password", "Password")},
                    { CustomerSearchByParameters.PROFILE_LAST_LOGIN_TIME, new DateTimeColumn("Profile.LastLoginTime", "Last Login Time")},
                    { CustomerSearchByParameters.ADDRESS_ADDRESS_TYPE, new TextColumn("AddressType.Name", "Address Type")},
                    //{ CustomerSearchByParameters.ADDRESS_LINE1, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line1))", "Address Line 1")},
                    { CustomerSearchByParameters.ADDRESS_LINE1, new EncryptedTextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line1))", "Address.HashLine1",
                     "Address Line 1")},
                    //{ CustomerSearchByParameters.ADDRESS_LINE2, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line2))", "Address Line 2")},
                    { CustomerSearchByParameters.ADDRESS_LINE2, new EncryptedTextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line2))","Address.HashLine2", 
                    "Address Line 2")},
                    //{ CustomerSearchByParameters.ADDRESS_LINE3, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line3))", "Address Line 3")},
                    { CustomerSearchByParameters.ADDRESS_LINE3, new EncryptedTextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line3))","Address.HashLine3",
                    "Address Line 3")},
                    { CustomerSearchByParameters.ADDRESS_CITY, new TextColumn("Address.City", "City")},
                    { CustomerSearchByParameters.ADDRESS_STATE_ID, new NumberColumn("Address.StateId", "StateId")},
                    { CustomerSearchByParameters.ADDRESS_COUNTRY_ID, new NumberColumn("Address.CountryId", "Country Id")},
                    { CustomerSearchByParameters.ADDRESS_POSTAL_CODE, new TextColumn("Address.PostalCode", "Postal Code")},
                    { CustomerSearchByParameters.CONTACT_CONTACT_TYPE, new TextColumn("ContactType.Name", "Contact Type")},
                   //{ CustomerSearchByParameters.CONTACT_ATTRIBUTE1, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute1))", "Contact Attribute 1")},
                    { CustomerSearchByParameters.CONTACT_ATTRIBUTE1, new EncryptedTextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute1))","Contact.HashAttribute1", "Contact Attribute 1")},
                   //{ CustomerSearchByParameters.CONTACT_ATTRIBUTE2, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute2))", "Contact Attribute 2")},
                    { CustomerSearchByParameters.CONTACT_ATTRIBUTE2, new EncryptedTextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute2))","Contact.HashAttribute2", "Contact Attribute 2")},
                    { CustomerSearchByParameters.ISACTIVE, new NumberColumn("isnull(customers.isactive,'1')", "isActive")}, // MD 04-Mar-2019
                    { CustomerSearchByParameters.CUSTOMER_GUID, new TextColumn("customers.guid", "Customer Guid")},
                    { CustomerSearchByParameters.PROFILE_EXTERNAL_SYSTEM_REFERENCE, new TextColumn("Profile.ExternalSystemReference", "ExternalSystemReference")},
                    { CustomerSearchByParameters.CONTACT_IS_ACTIVE, new NumberColumn("isnull(Contact.IsActive,'1')", "Contact Is Active")},
                    { CustomerSearchByParameters.ADDRESS_IS_ACTIVE, new NumberColumn("isnull(Address.IsActive,'1')", "Address Is Active")},
                    { CustomerSearchByParameters.CUSTOMER_ID_IN, new TextColumn("customers.customer_id", "Customer Id")},
                    { CustomerSearchByParameters.CUSTOMER_LAST_UPDATE_FROM_DATE, new DateTimeColumn("customers.last_updated_date", "Last Updated Date")},
                    { CustomerSearchByParameters.CUSTOMER_LAST_UPDATE_TO_DATE, new DateTimeColumn("customers.last_updated_date", "Last Updated Date")},
                    //{ CustomerSearchByParameters.IS_ADULT, new DateTimeColumn("CONVERT(DateTime,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.DateOfBirth)))", "Date Of Birth")},
                    { CustomerSearchByParameters.IS_ADULT, new DateTimeColumn("Isnull(CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.DateOfBirth)),CONVERT(varchar, getdate(), 23))", "Date Of Birth",null,false)},
                    { CustomerSearchByParameters.CUSTOMER_TYPE, new TextColumn("customers.CustomerType", "Type", "'R'")},
                    { CustomerSearchByParameters.CONTACT_PHONE_OR_EMAIL, new EncryptedTextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute1))", "Contact.HashAttribute1", "Contact Attribute 1")},
                    { CustomerSearchByParameters.WAIVER_CODE, new TextColumn("CustomerSignedWaiverHeader.WaiverCode", "Waiver Code")},
                    { CustomerSearchByParameters.CHANNEL, new TextColumn("CustomerSignedWaiverHeader.Channel", "Channel")},
            };
        }
    }

    /// <summary>
    /// customer search criteria
    /// </summary>
    public class CustomerSearchCriteria : SearchCriteria
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// default constructor
        /// </summary>
        public CustomerSearchCriteria() : base(new CustomerColumnProvider())
        {

        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="columnIdentifier"></param>
        /// <param name="operator"></param>
        /// <param name="parameters"></param>
        public CustomerSearchCriteria(Enum columnIdentifier, Operator @operator, params object[] parameters) :
            base(new CustomerColumnProvider(), columnIdentifier, @operator, parameters)
        {

        }         
    }
}

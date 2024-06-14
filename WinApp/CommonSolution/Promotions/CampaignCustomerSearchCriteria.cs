/********************************************************************************************
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *3.0      28-Oct-2020   Mushahid Faizan         Created
 *********************************************************************************************/
using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
    /// </summary>
    /// 
    public enum CampaignCustomerSearchByParameters
    {
        ADDRESS1,
        ADDRESS2,
        ADDRESS3,
        ANNIVERSARY,
        BIRTH_DATE,
        CHANNEL,
        CITY,
        COMPANY,
        CONTACT_PHONE1,
        CONTACT_PHONE2,
        COUNTRY,
        CREATEDBY,
        CREATIONDATE,
        CUSTOMER,
        CUSTOMER_ID,
        CUSTOMER_NAME,
        CUSTOMERTYPE,
        DESIGNATION,
        DOWNLOADBATCHID,
        EMAIL,
        EXTERNALSYSTEMREF,
        FBACCESSTOKEN,
        FBUSERID,
        GENDER,
        IDPROOFFILENAME,
        ISACTIVE,
        LAST_NAME,
        LAST_UPDATED_DATE,
        LAST_UPDATED_USER,
        LASTLOGINTIME,
        MIDDLE_NAME,
        NOTES,
        PASSWORD,
        PHOTOFILENAME,
        PIN,
        PROFILEID,
        RIGHTHANDED,
        SAMEER,
        SITE_ID,
        STATE,
        TAXCODE,
        TEAMUSER,
        TITLE,
        TWACCESSSECRET,
        TWACCESSTOKEN,
        UNIQUE_ID,
        USERNAME,
        VERIFIED,
        WECHATACCESSTOKEN,
        CUSTOMER_CUSTOM_DATA_SET_ID,
        CUSTOMER_ID_IN

    }
    public class CampaignCustomerColumnProvider : ColumnProvider
    {
        internal CampaignCustomerColumnProvider()
        {
             columnDictionary = new Dictionary<Enum, Column>() {
                    { CampaignCustomerSearchByParameters.CUSTOMER_ID, new NumberColumn("customers.customer_id", "Customer Id") },
                    { CampaignCustomerSearchByParameters.PROFILEID, new NumberColumn("customers.ProfileId", "Profile Id")},
                    { CampaignCustomerSearchByParameters.CHANNEL, new TextColumn("customers.channel", "Channel")},
                    { CampaignCustomerSearchByParameters.EXTERNALSYSTEMREF, new TextColumn("Profile.ExternalSystemRef", "External System Reference")},
                    { CampaignCustomerSearchByParameters.VERIFIED, new TextColumn("customers.Verified", "Verified", "'N'")},
                    { CampaignCustomerSearchByParameters.CREATEDBY, new TextColumn("customers.CreatedBy", "Created By")},
                    { CampaignCustomerSearchByParameters.CREATIONDATE, new DateTimeColumn("customers.CreationDate", "Creation Date")},
                    { CampaignCustomerSearchByParameters.LAST_UPDATED_USER, new TextColumn("customers.last_updated_user", "Last Updated By")},
                    { CampaignCustomerSearchByParameters.LAST_UPDATED_DATE, new DateTimeColumn("customers.last_updated_date", "Last Updated Date")},
                    { CampaignCustomerSearchByParameters.SITE_ID, new NumberColumn("customers.site_id", "Site Id")},
                    { CampaignCustomerSearchByParameters.CUSTOMER_NAME, new TextColumn("Profile.FirstName", "Customer Name")},
                    { CampaignCustomerSearchByParameters.MIDDLE_NAME, new TextColumn("Profile.Middle_Name","Middle Name")},
                    { CampaignCustomerSearchByParameters.LAST_NAME, new TextColumn("Profile.LastName", "Last Name")},
                    { CampaignCustomerSearchByParameters.TITLE, new TextColumn("Profile.Title", "Title")},
                    { CampaignCustomerSearchByParameters.NOTES, new TextColumn("Profile.Notes", "Notes")},
                    { CampaignCustomerSearchByParameters.BIRTH_DATE, new DateTimeColumn("CONVERT(DateTime,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.DateOfBirth)))", "Date Of Birth")},
                    { CampaignCustomerSearchByParameters.GENDER, new TextColumn("Profile.Gender", "Gender")},
                    { CampaignCustomerSearchByParameters.ANNIVERSARY, new DateTimeColumn("CONVERT(DateTime,CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.Anniversary)))", "Anniversary")},
                    { CampaignCustomerSearchByParameters.UNIQUE_ID, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.UniqueId))", "Unique Identifier")},
                    { CampaignCustomerSearchByParameters.TAXCODE, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.TaxCode))", "Tax Code")},
                    { CampaignCustomerSearchByParameters.COMPANY, new TextColumn("Profile.Company", "Company")},
                    { CampaignCustomerSearchByParameters.DESIGNATION, new TextColumn("Profile.Designation", "Designation")},
                    { CampaignCustomerSearchByParameters.USERNAME, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.Username))", "User name")},
                    { CampaignCustomerSearchByParameters.EMAIL, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute1))", "Email ")},
                    { CampaignCustomerSearchByParameters.PASSWORD, new TextColumn("Profile.Password", "Password")},
                    { CampaignCustomerSearchByParameters.LASTLOGINTIME, new DateTimeColumn("Profile.LastLoginTime", "Last Login Time")},
                    { CampaignCustomerSearchByParameters.ADDRESS1, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line1))", "Address 1")},
                    { CampaignCustomerSearchByParameters.ADDRESS2, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line2))", "Address 2")},
                    { CampaignCustomerSearchByParameters.ADDRESS3, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line3))", "Address 3")},
                    { CampaignCustomerSearchByParameters.CITY, new TextColumn("Address.City", "City")},
                    { CampaignCustomerSearchByParameters.STATE, new TextColumn("State.State", "State")},
                    { CampaignCustomerSearchByParameters.COUNTRY, new TextColumn("Country.CountryName", "Country")},
                    { CampaignCustomerSearchByParameters.PIN, new TextColumn("Address.PostalCode", "Postal Code")},
                     { CampaignCustomerSearchByParameters.CONTACT_PHONE1, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute1))", "Contact Attribute 1")},
                    //{ CampaignCustomerSearchByParameters.CONTACT_PHONE1, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Customers.contact_phone1))", "Contact Attribute 1")},
                    //{ CampaignCustomerSearchByParameters.CONTACT_PHONE2, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Customers.contact_phone2))", "Contact Attribute 2")},
                    { CampaignCustomerSearchByParameters.CONTACT_PHONE2, new TextColumn("CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute2))", "Contact Attribute 2")},
                       { CampaignCustomerSearchByParameters.ISACTIVE, new NumberColumn("isnull(customers.isactive,'1')", "isActive")},
                   { CampaignCustomerSearchByParameters.CUSTOMER_ID_IN, new TextColumn("customers.customer_id", "Customer")},
                    { CampaignCustomerSearchByParameters.CUSTOMERTYPE, new TextColumn("customers.CustomerType", "Customer Type", "'R'")},
                    { CampaignCustomerSearchByParameters.TWACCESSSECRET, new TextColumn("Customers.TWAccessSecret", "TWAccessSecret")},
                    { CampaignCustomerSearchByParameters.TWACCESSTOKEN, new TextColumn("Customers.TWAccessToken", "TWAccessToken")},
                    { CampaignCustomerSearchByParameters.FBACCESSTOKEN, new TextColumn("Customers.FBAccessToken", "FBAccessToken")},
                    { CampaignCustomerSearchByParameters.FBUSERID, new TextColumn("Customers.FBUserId", "FBUserId")},
                    { CampaignCustomerSearchByParameters.WECHATACCESSTOKEN, new TextColumn("Customers.WeChatAccessToken", "WeChatAccessToken")},
                    { CampaignCustomerSearchByParameters.DOWNLOADBATCHID, new NumberColumn("Customers.DownloadBatchId", "DownloadBatchId")},
                    { CampaignCustomerSearchByParameters.IDPROOFFILENAME, new TextColumn("Customers.IDProofFileName", "IDProofFileName")},
                    { CampaignCustomerSearchByParameters.TEAMUSER, new TextColumn("Customers.TeamUser", "TeamUser")},
                    { CampaignCustomerSearchByParameters.RIGHTHANDED, new TextColumn("Customers.RightHanded", "RightHanded")},
                    { CampaignCustomerSearchByParameters.PHOTOFILENAME, new TextColumn("Customers.PhotoFileName", "PhotoFileName")},
                    { CampaignCustomerSearchByParameters.CUSTOMER_CUSTOM_DATA_SET_ID, new NumberColumn("customers.CustomDataSetId", "Custom Data Set Id")},
            };
        }
    }

    /// <summary>
    /// Campaign customer search criteria
    /// </summary>
    public class CampaignCustomerSearchCriteria : SearchCriteria
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public CampaignCustomerSearchCriteria() : base(new CampaignCustomerColumnProvider())
        {

        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="columnIdentifier"></param>
        /// <param name="operator"></param>
        /// <param name="parameters"></param>
        public CampaignCustomerSearchCriteria(Enum columnIdentifier, Operator @operator, params object[] parameters) :
            base(new CampaignCustomerColumnProvider(), columnIdentifier, @operator, parameters)
        {

        }
    }
}

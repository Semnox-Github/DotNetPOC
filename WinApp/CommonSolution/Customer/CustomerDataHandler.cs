/********************************************************************************************
 * Project Name - Customer Data Handler
 * Description  - Data handler of the Customer class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Lakshminarayana     Created 
 *2.60        04-Mar-2019   Mushahid Faizan     Added "ISACTIVE" in DBSearchParameters and in GetCustomerFilterQuery().
 *2.60        20-Jun-2019     Nitin Pai         Guest App Changes
 *2.70.2      19-Jul-2019    Girish Kundar      Modified : Structure of data Handler - insert /Update methods
 *                                                          Fix for SQL Injection Issue  
 *2.70.2      30-Sep-2019    Deeksha            Created GetCustomerDTOList() method with list<int> as parameter
 *2.70.2      13-Dec-2019    Akshay G           Added searchParameters in CUSTOMER_ENTITY_LAST_UPDATE_DATE_GREATER_THAN for ClubSpeed related changes
 *2.80.0      30-Sep-2019    Deeksha            Created GetCustomerDTOList() method with list<int> as parameter
 *2.80.0      25-Nov-2019    Girish kundar      Modified : Customer unique attribute search
 *2.70.2      06-feb-2020    Nitin Pai          Moved Customer API to Search Params, added additional params for guest app change
 *2.70.3      14-Feb-2020    Lakshminarayana    Modified: Creating unregistered customer during check-in process
 *2.140.0     14-Sep-2021    Guru S A           Waiver mapping UI enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    ///  Customer Data Handler - Handles insert, update and select of  Customer objects
    /// </summary>
    public class CustomerDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string passPhrase;
        private SqlTransaction sqlTransaction;
        private List<SqlParameter> parameters;
        private static readonly Dictionary<CustomerSearchByParameters, string> DBSearchParameters = new Dictionary<CustomerSearchByParameters, string>
            {
                {CustomerSearchByParameters.CUSTOMER_ID, "customers.customer_id"},
                {CustomerSearchByParameters.CUSTOMER_MEMBERSHIP_ID, "customers.MembershipId"},
                {CustomerSearchByParameters.CUSTOMER_CHANNEL, "customers.channel"},
                {CustomerSearchByParameters.CUSTOMER_EXTERNAL_SYSTEM_REFERENCE, "customers.ExternalSystemRef"},
                {CustomerSearchByParameters.CUSTOMER_CUSTOM_DATA_SET_ID, "customers.CustomDataSetId"},
                {CustomerSearchByParameters.CUSTOMER_VERIFIED, "customers.Verified"},
                {CustomerSearchByParameters.CUSTOMER_CREATED_BY, "customers.CreatedBy"},
                {CustomerSearchByParameters.CUSTOMER_CREATION_DATE, "customers.CreationDate"},
                {CustomerSearchByParameters.CUSTOMER_LAST_UPDATED_BY, "customers.last_updated_user"},
                {CustomerSearchByParameters.CUSTOMER_LAST_UPDATE_DATE, "customers.last_updated_date"},
                {CustomerSearchByParameters.CUSTOMER_SITE_ID, "customers.site_id"},
                {CustomerSearchByParameters.CUSTOMER_PROFILE_ID, "customers.ProfileId"},
                {CustomerSearchByParameters.CUSTOMER_TYPE, "customers.CustomerType"},
                {CustomerSearchByParameters.PROFILE_PROFILE_TYPE,"ProfileType.Name"},
                {CustomerSearchByParameters.PROFILE_FIRST_NAME,"Profile.FirstName"},
                {CustomerSearchByParameters.PROFILE_MIDDLE_NAME, "Profile.MiddleName"},
                {CustomerSearchByParameters.PROFILE_LAST_NAME, "Profile.LastName"},
                {CustomerSearchByParameters.PROFILE_TITLE, "Profile.Title"},
                {CustomerSearchByParameters.PROFILE_NOTES, "Profile.Notes"},
                //{CustomerSearchByParameters.PROFILE_DATE_OF_BIRTH, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.DateOfBirth))"},
                {CustomerSearchByParameters.PROFILE_DATE_OF_BIRTH, "Profile.HashDateOfBirth"},
                {CustomerSearchByParameters.PROFILE_GENDER, "Profile.Gender"},
                //{CustomerSearchByParameters.PROFILE_ANNIVERSARY, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.Anniversary))"},
                {CustomerSearchByParameters.PROFILE_ANNIVERSARY, "Profile.HashAnniversary"},
                //{CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.UniqueId))"},
                {CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, "Profile.HashUniqueId"},
                {CustomerSearchByParameters.PROFILE_PHOTO_URL, "Profile.PhotoURL"},
                //{CustomerSearchByParameters.PROFILE_TAX_CODE, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.TaxCode))"},
                {CustomerSearchByParameters.PROFILE_TAX_CODE, "Profile.HashTaxCode"},
                {CustomerSearchByParameters.PROFILE_COMPANY, "Profile.Company"},
                {CustomerSearchByParameters.PROFILE_DESIGNATION, "Profile.Designation"},
                //{CustomerSearchByParameters.PROFILE_USER_NAME, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.Username))"},
                {CustomerSearchByParameters.PROFILE_USER_NAME, "Profile.HashUsername"},
                //{CustomerSearchByParameters.PROFILE_USER_NAME_OR_EMAIL, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.Username))"},
                {CustomerSearchByParameters.PROFILE_USER_NAME_OR_EMAIL, "Profile.HashUsername"},
                {CustomerSearchByParameters.PROFILE_PASSWORD, "Profile.Password"},
                {CustomerSearchByParameters.PROFILE_LAST_LOGIN_TIME, "Profile.LastLoginTime"},
                {CustomerSearchByParameters.ADDRESS_ADDRESS_TYPE, "AddressType.Name"},
                //{CustomerSearchByParameters.ADDRESS_LINE1, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line1))"},
                {CustomerSearchByParameters.ADDRESS_LINE1, " Address.HashLine1"},
                //{CustomerSearchByParameters.ADDRESS_LINE2, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line2))"},
                {CustomerSearchByParameters.ADDRESS_LINE2, " Address.HashLine2"},
                //{CustomerSearchByParameters.ADDRESS_LINE3, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line3))"},
                {CustomerSearchByParameters.ADDRESS_LINE3, " Address.HashLine3"},
                {CustomerSearchByParameters.ADDRESS_CITY, "Address.City"},
                {CustomerSearchByParameters.ADDRESS_STATE_ID, "Address.StateId"},
                {CustomerSearchByParameters.ADDRESS_POSTAL_CODE, "Address.PostalCode"},
                {CustomerSearchByParameters.CONTACT_CONTACT_TYPE, "ContactType.Name"},
                //{CustomerSearchByParameters.CONTACT_ATTRIBUTE1, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute1))"},
                {CustomerSearchByParameters.CONTACT_ATTRIBUTE1, "Contact.HashAttribute1"},
                {CustomerSearchByParameters.CONTACT_ATTRIBUTE2, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute2))"},
                {CustomerSearchByParameters.ISACTIVE, "customers.isactive"},
                {CustomerSearchByParameters.CUSTOMER_GUID, "customers.Guid"},
                {CustomerSearchByParameters.PROFILE_EXTERNAL_SYSTEM_REFERENCE,"Profile.ExternalSystemReference" },
                {CustomerSearchByParameters.CONTACT_IS_ACTIVE, "Contact.isactive"},
                {CustomerSearchByParameters.ADDRESS_IS_ACTIVE, "Address.isactive"},
                {CustomerSearchByParameters.CUSTOMER_ID_IN,"customers.customer_id"},
                {CustomerSearchByParameters.CUSTOMER_LAST_UPDATE_FROM_DATE, "customers.last_updated_date"},
                {CustomerSearchByParameters.CUSTOMER_LAST_UPDATE_TO_DATE, "customers.last_updated_date"},
                {CustomerSearchByParameters.CUSTOMER_ENTITY_LAST_UPDATE_DATE_GREATER_THAN, "customers.last_updated_date"},
                //{CustomerSearchByParameters.CONTACT_PHONE_OR_EMAIL, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute1))"},
                {CustomerSearchByParameters.CONTACT_PHONE_OR_EMAIL, " Contact.HashAttribute1"},
                //// Added as part of unique attributes search
                //{CustomerSearchByParameters.PHONE_NUMBER_LIST, " CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,contacts.Attribute1))"},
                {CustomerSearchByParameters.PHONE_NUMBER_LIST, " Contacts.HashAttribute1"},
                //{CustomerSearchByParameters.EMAIL_LIST, " CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,contacts.Attribute1))"},
                {CustomerSearchByParameters.EMAIL_LIST, " Contacts.HashAttribute1"},
                //{CustomerSearchByParameters.WECHAT_ACCESS_TOKEN_LIST, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,contacts.Attribute1))"},
                {CustomerSearchByParameters.WECHAT_ACCESS_TOKEN_LIST, " Contacts.HashAttribute1"},
                //{CustomerSearchByParameters.FB_USERID_LIST, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,contacts.Attribute1))"},
                {CustomerSearchByParameters.FB_USERID_LIST, " Contacts.HashAttribute1"},
                //{CustomerSearchByParameters.TW_ACCESS_TOKEN_LIST, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,contacts.Attribute1))"},
                {CustomerSearchByParameters.TW_ACCESS_TOKEN_LIST, " Contacts.HashAttribute1"},
                //{CustomerSearchByParameters.TW_ACCESS_SECRET_LIST, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,contacts.Attribute2))"},
                {CustomerSearchByParameters.TW_ACCESS_SECRET_LIST, " Contacts.HashAttribute2"},
                //{CustomerSearchByParameters.FB_ACCESS_TOKEN_LIST, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,contacts.Attribute2))"},
                {CustomerSearchByParameters.FB_ACCESS_TOKEN_LIST, " Contacts.HashAttribute2"},
                {CustomerSearchByParameters.IS_ADULT, "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.DateOfBirth))"},
                {CustomerSearchByParameters.HAS_SIGNED_WAIVERS, ""},
                {CustomerSearchByParameters.WAIVER_IS_MAPPED_TO_TRX, ""},
                {CustomerSearchByParameters.LATEST_TO_SIGN_WAIVER, ""},
                {CustomerSearchByParameters.CHANNEL_USED_TO_SIGN_WAIVER, "cswh.Channel"},
                {CustomerSearchByParameters.HAS_SIGNED_WAIVER_ID_LIST, "csw.WaiverSetDetailId"},
                {CustomerSearchByParameters.PROFILE_FIRST_NAME_LIKE,"Profile.FirstName"},
                {CustomerSearchByParameters.PROFILE_MIDDLE_NAME_LIKE, "Profile.MiddleName"},
                {CustomerSearchByParameters.PROFILE_LAST_NAME_LIKE, "Profile.LastName"},
            };
        private static readonly string CUSTOMER_SELECT_QUERY = @"SELECT DISTINCT Customers.*, card.CardNumber
                                                                 FROM Customers
                                                                 INNER JOIN Profile ON Profile.Id = Customers.ProfileId
                                                                 LEFT OUTER JOIN ProfileType ON Profile.ProfileTypeId = ProfileType.Id
                                                                 LEFT OUTER JOIN Contact ON Contact.ProfileId = Profile.Id
                                                                 LEFT OUTER JOIN ContactType ON ContactType.Id = Contact.ContactTypeId
                                                                 LEFT OUTER JOIN Address ON Address.ProfileId = Profile.Id
                                                                 LEFT OUTER JOIN AddressType ON AddressType.Id = Address.AddressTypeId 
                                                                 LEFT OUTER JOIN CustomerSignedWaiverHeader ON CustomerSignedWaiverHeader.SignedBy = Customers.customer_id
                                                                 OUTER APPLY (SELECT TOP 1 CASE WHEN cards.valid_flag = 'Y' THEN cards.card_number ELSE cards.card_number + '[Inactive]' END CardNumber
                                                                 FROM cards WHERE Customers.customer_id = cards.customer_id
                                                                 ORDER BY primarycard desc, valid_flag DESC, last_update_time desc) card";

        private static readonly string CUSTOMER_MEMBERSHIP_SELECT_QUERY = @"SELECT *, 
                                                                                  (SELECT TOP 1 CASE WHEN cc.valid_flag = 'Y' THEN
                                                                                                          cc.card_number 
                                                                                                     ELSE cc.card_number + '[Inactive]' END CardNumber 
                                                                                     from cards cc where cc.customer_id = cu.customer_id ) as CardNumber 
                                                                              from customers cu  ";
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of CustomerDataHandler class
        /// </summary>
        public CustomerDataHandler(string passPhrase, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry("passPhrase", sqlTransaction);
            this.passPhrase = passPhrase;
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Customer Record.
        /// </summary>
        /// <param name="customerDTO">CustomerDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerDTO customerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry("customerDTO", loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            customerDTO.ExternalSystemReference = customerDTO.ExternalSystemReference != null ? customerDTO.ExternalSystemReference.Trim() : string.Empty;
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", customerDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProfileId", customerDTO.ProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipId", customerDTO.MembershipId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Channel", customerDTO.Channel));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomDataSetId", customerDTO.CustomDataSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Verified", customerDTO.Verified ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isactive", customerDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerType", CustomerTypeConverter.ToString(customerDTO.CustomerType)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSystemReference", customerDTO.ExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customerDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", customerDTO.Guid));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Customer record to the database
        /// </summary>
        /// <param name="customerDTO">CustomerDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerDTO</returns>
        public CustomerDTO InsertCustomer(CustomerDTO customerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry("customerDTO", loginId, siteId);
            string query = @"INSERT INTO Customers 
                                        ( 
                                            ProfileId,
                                            MembershipId,
                                            Channel,
                                            CustomDataSetId,
                                            Verified,
                                            ExternalSystemRef,
                                            Isactive,
                                            CustomerType,
                                            CreatedBy,
                                            CreationDate,
                                            last_updated_user,
                                            last_updated_date,
                                            site_id,
                                            MasterEntityId

                                        ) 
                                VALUES  
                                        (
                                            @ProfileId,
                                            @MembershipId,
                                            @Channel,
                                            @CustomDataSetId,
                                            @Verified,
                                            @ExternalSystemReference,
                                            @isactive,
                                            @CustomerType,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                        ) SELECT * FROM Customers WHERE customer_id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerDTO(customerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerDTO);
            return customerDTO;
        }

        /// <summary>
        /// Updates the Customer record
        /// </summary>
        /// <param name="customerDTO">CustomerDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the CustomerDTO</returns>
        public CustomerDTO UpdateCustomer(CustomerDTO customerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry("customerDTO", loginId, siteId);
            string query = @"UPDATE Customers
                             SET ProfileId=@ProfileId,
                                 MembershipId=@MembershipId,
                                 Channel=@Channel,
                                 CustomDataSetId=@CustomDataSetId,
                                 Verified=@Verified,
                                 ExternalSystemRef=@ExternalSystemReference,
                                 IsActive=@isactive,
                                 CustomerType=@CustomerType,
                                 last_updated_user = @LastUpdatedBy,
                                 last_updated_date=GETDATE(),
                                 MasterEntityId=@MasterEntityId
                             WHERE customer_id = @Id 
                            SELECT * FROM Customers WHERE customer_id  = @Id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerDTO(customerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating customerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerDTO);
            return customerDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerDTO">CustomerDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustomerDTO(CustomerDTO customerDTO, DataTable dt)
        {
            log.LogMethodEntry("customerDTO", dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerDTO.Id = Convert.ToInt32(dt.Rows[0]["customer_id"]);
                customerDTO.LastUpdateDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                customerDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                customerDTO.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                customerDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to CustomerDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CustomerDTO</returns>
        private CustomerDTO GetCustomerDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomerDTO customerDTO = new CustomerDTO(Convert.ToInt32(dataRow["customer_id"]),
                                            dataRow["ProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProfileId"]),
                                            dataRow["MembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipId"]),
                                            dataRow["Channel"] == DBNull.Value ? string.Empty : dataRow["Channel"].ToString(),
                                            dataRow["CustomDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomDataSetId"]),
                                            dataRow["Verified"] == DBNull.Value ? false : Convert.ToString(dataRow["Verified"]).Equals("Y"),
                                            dataRow["ExternalSystemRef"] == DBNull.Value ? string.Empty : dataRow["ExternalSystemRef"].ToString().Trim(),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CardNumber"] == DBNull.Value ? string.Empty : dataRow["CardNumber"].ToString(),
                                            CustomerTypeConverter.FromStringValue(dataRow["CustomerType"] == DBNull.Value ? "R" : dataRow["CustomerType"].ToString()),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["last_updated_user"] == DBNull.Value ? string.Empty : dataRow["last_updated_user"].ToString(),
                                            dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(customerDTO);
            return customerDTO;
        }

        /// <summary>
        /// Gets the Customer data of passed Customer Id
        /// </summary>
        /// <param name="customerId">integer type parameter</param>
        /// <returns>Returns CustomerDTO</returns>
        public CustomerDTO GetCustomerDTO(int customerId)
        {
            log.LogMethodEntry(customerId);
            CustomerDTO returnValue = null;
            string query = CUSTOMER_SELECT_QUERY +
                            @" WHERE Customers.customer_id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", customerId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetCustomerDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the no of customers matching the search criteria
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of customers matching the criteria</returns>
        public int GetCustomerCount(List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int customerCount = 0;
            string selectQuery = @" SELECT COUNT(DISTINCT Customers.customer_id) AS TotalCount
                                    FROM Customers
                                    INNER JOIN Profile ON Profile.Id = Customers.ProfileId
                                    LEFT OUTER JOIN ProfileType ON Profile.ProfileTypeId = ProfileType.Id
                                    LEFT OUTER JOIN Contact ON Contact.ProfileId = Profile.Id
                                    LEFT OUTER JOIN ContactType ON ContactType.Id = Contact.ContactTypeId
                                    LEFT OUTER JOIN Address ON Address.ProfileId = Profile.Id
                                    LEFT OUTER JOIN AddressType ON AddressType.Id = Address.AddressTypeId";
            selectQuery += GetCustomerFilterQuery(searchParameters);
            if(parameters == null)
                parameters = new List<SqlParameter>();

            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                customerCount = Convert.ToInt32(dataTable.Rows[0]["TotalCount"]);
            }
            log.LogMethodExit(customerCount);
            return customerCount;
        }


        /// <summary>
        /// GetCustomerFilterQuery
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>Query string from customer search option</returns>
        private String GetCustomerFilterQuery(List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            string joiner = string.Empty;
            int count = 0;
            StringBuilder query = new StringBuilder(" ");
            parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                query.Append(" WHERE ");
                
                foreach (KeyValuePair<CustomerSearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomerSearchByParameters.CUSTOMER_ID ||
                            searchParameter.Key == CustomerSearchByParameters.CUSTOMER_MEMBERSHIP_ID ||
                            searchParameter.Key == CustomerSearchByParameters.CUSTOMER_PROFILE_ID ||
                            searchParameter.Key == CustomerSearchByParameters.CUSTOMER_CUSTOM_DATA_SET_ID ||
                            searchParameter.Key == CustomerSearchByParameters.ADDRESS_STATE_ID ||
                            searchParameter.Key == CustomerSearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == CustomerSearchByParameters.ADDRESS_COUNTRY_ID)
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.CUSTOMER_GUID ||
                            searchParameter.Key == CustomerSearchByParameters.PROFILE_FIRST_NAME ||
                            searchParameter.Key == CustomerSearchByParameters.PROFILE_LAST_NAME ||
                            searchParameter.Key == CustomerSearchByParameters.PROFILE_MIDDLE_NAME ||
                            searchParameter.Key == CustomerSearchByParameters.PROFILE_NOTES ||
                            searchParameter.Key == CustomerSearchByParameters.PROFILE_PASSWORD ||
                           // searchParameter.Key == CustomerSearchByParameters.PROFILE_USER_NAME ||
                           // searchParameter.Key == CustomerSearchByParameters.PROFILE_TAX_CODE ||
                            searchParameter.Key == CustomerSearchByParameters.PROFILE_DESIGNATION ||
                            searchParameter.Key == CustomerSearchByParameters.CUSTOMER_CHANNEL ||
                            searchParameter.Key == CustomerSearchByParameters.PROFILE_COMPANY ||
                           // searchParameter.Key == CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER ||
                            searchParameter.Key == CustomerSearchByParameters.PROFILE_EXTERNAL_SYSTEM_REFERENCE)
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if ( searchParameter.Key == CustomerSearchByParameters.PROFILE_USER_NAME ||
                            searchParameter.Key == CustomerSearchByParameters.PROFILE_TAX_CODE || 
                            searchParameter.Key == CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER )
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterNameWithSHA256HashByteCommand(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.CUSTOMER_SITE_ID)
                        {
                            
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
			            else if ((searchParameter.Key == CustomerSearchByParameters.ISACTIVE) || 
                            (searchParameter.Key == CustomerSearchByParameters.CONTACT_IS_ACTIVE) ||
                            (searchParameter.Key == CustomerSearchByParameters.ADDRESS_IS_ACTIVE))//char
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.CUSTOMER_TYPE)//char
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'R')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.CUSTOMER_VERIFIED) //char
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.CUSTOMER_GUID) //char
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.CUSTOMER_ID_IN)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.CUSTOMER_LAST_UPDATE_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.CUSTOMER_LAST_UPDATE_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if ((searchParameter.Key == CustomerSearchByParameters.PHONE_NUMBER_LIST)
                            || (searchParameter.Key == CustomerSearchByParameters.EMAIL_LIST)
                            || (searchParameter.Key == CustomerSearchByParameters.WECHAT_ACCESS_TOKEN_LIST) 
                            || (searchParameter.Key == CustomerSearchByParameters.FB_USERID_LIST)
                            || (searchParameter.Key == CustomerSearchByParameters.TW_ACCESS_TOKEN_LIST)
                            || (searchParameter.Key == CustomerSearchByParameters.TW_ACCESS_SECRET_LIST)
                            || (searchParameter.Key == CustomerSearchByParameters.FB_ACCESS_TOKEN_LIST))
                        {
                            query.Append(joiner  + " EXISTS (SELECT 1 FROM Contact contacts WHERE " + DBSearchParameters[searchParameter.Key] +
                                " IN(" + dataAccessHandler.GetInClauseParameterNameWithSHA2256Command(searchParameter.Key, searchParameter.Value) + ") and contacts.ProfileId = Profile.id)");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.ADDRESS_POSTAL_CODE) 
                        {
                            query.Append(joiner + " EXISTS (SELECT 1 FROM Contact contacts WHERE " + DBSearchParameters[searchParameter.Key] +
                                " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") and contacts.ProfileId = Profile.id)");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if ((searchParameter.Key == CustomerSearchByParameters.ADDRESS_CITY)
                            || (searchParameter.Key == CustomerSearchByParameters.ADDRESS_ADDRESS_TYPE)
                            || (searchParameter.Key == CustomerSearchByParameters.CONTACT_CONTACT_TYPE))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if ((searchParameter.Key == CustomerSearchByParameters.ADDRESS_LINE1)
                            || (searchParameter.Key == CustomerSearchByParameters.ADDRESS_LINE2)
                            || (searchParameter.Key == CustomerSearchByParameters.ADDRESS_LINE3) )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterNameWithSHA2256Command(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.PROFILE_DATE_OF_BIRTH
                            || searchParameter.Key == CustomerSearchByParameters.PROFILE_ANNIVERSARY) 
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetDateTimeParameterNameWithSHA256HashByteCommand(searchParameter.Key));
                            // parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))); 
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.CUSTOMER_ENTITY_LAST_UPDATE_DATE_GREATER_THAN) // Customer entity lastupdatedate greater than LastUpdateDate
                        {
                            query.Append(joiner + @" exists (SELECT 1 from Address aa where Profile.Id = aa.ProfileId and aa.LastUpdateDate > " + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                + @"  union all
                                                        SELECT 1 from Contact cc where Profile.Id = cc.ProfileId and cc.LastUpdateDate > " + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                + @"  union all 
                                                        SELECT 1 from CustomData cd where cd.CustomDataSetId = Customers.CustomDataSetId and cd.LastUpdateDate > " + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                + @"  or Profile.LastUpdateDate > " + dataAccessHandler.GetParameterName(searchParameter.Key) + "  OR customers.last_updated_date > " + dataAccessHandler.GetParameterName(searchParameter.Key) + ") ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.IS_ADULT)
                        {
                            query.Append(joiner + "( Isnull(" + DBSearchParameters[CustomerSearchByParameters.IS_ADULT] + ",CONVERT(varchar, getdate(), 23))" + " <= DATEADD(year," + dataAccessHandler.GetParameterName(searchParameter.Key) + " ,GETDATE()))");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        //else if (searchParameter.Key == CustomerSearchByParameters.PROFILE_USER_NAME)
                        //{
                        //    query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        //    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        //}
                        else if (searchParameter.Key == CustomerSearchByParameters.PROFILE_USER_NAME_OR_EMAIL)
                        {
                            query.Append(joiner + @"( Isnull(" + DBSearchParameters[CustomerSearchByParameters.PROFILE_USER_NAME] + ",'N')= "+
                                                                dataAccessHandler.GetParameterNameWithSHA256HashByteCommand(searchParameter.Key) + 
                                  "  OR  ( Isnull(" + DBSearchParameters[CustomerSearchByParameters.CONTACT_CONTACT_TYPE] + ", 'N') ='" + ContactType.EMAIL.ToString() + "' and " +
                                        " Isnull(" + DBSearchParameters[CustomerSearchByParameters.CONTACT_ATTRIBUTE1] + ", 'N') = " + dataAccessHandler.GetParameterNameWithSHA256HashByteCommand(searchParameter.Key) + ")) "
                             );

                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.CONTACT_PHONE_OR_EMAIL)
                        {
                            query.Append(joiner + " ( Isnull(" + DBSearchParameters[CustomerSearchByParameters.CONTACT_CONTACT_TYPE] + ", 'N') ='" + ContactType.EMAIL.ToString()
                                                     + "' OR Isnull(" + DBSearchParameters[CustomerSearchByParameters.CONTACT_CONTACT_TYPE] + ", 'N') ='" + ContactType.PHONE.ToString() 
                                                     + "' ) AND " + DBSearchParameters[CustomerSearchByParameters.CONTACT_ATTRIBUTE1] + " IN (" + dataAccessHandler.GetInClauseParameterNameWithSHA2256Command(searchParameter.Key, searchParameter.Value) + ")");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.HAS_SIGNED_WAIVERS) 
                        {
                            query.Append(joiner + @" ISNULL((SELECT top 1 1 
                                                               from CustomerSignedWaiver csw
                                                              where csw.SignedFor = customers.customer_id
                                                                and csw.IsActive = 1
                                                                and ISNULL(csw.ExpiryDate, getdate() + 1) >= getdate()),0) = "
                                                + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key),
                                                            ((searchParameter.Value == "Y"|| searchParameter.Value == "1") ? "1":"0")));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.WAIVER_IS_MAPPED_TO_TRX)
                        {
                            query.Append(joiner + @"  ISNULL((SELECT top 1 1 
                                                                from trx_header th, trx_lines tl, WaiversSigned ws, CustomerSignedWaiver csw
                                                               where csw.SignedFor = customers.customer_id
                                                                 and csw.IsActive = 1
                                                                 and csw.CustomerSignedWaiverId = ws.CustomerSignedWaiverId
                                                                 and ws.TrxId = tl.TrxId
                                                                 and ws.LineId = tl.LineId
                                                                 and tl.CancelledTime is null
                                                                 and tl.TrxId = th.TrxId
                                                                 and th.Status not in ('CANCELLED')
                                                                 and th.TrxDate >= getdate()-1
                                                                 and ISNULL(csw.ExpiryDate, getdate()+1) >= getdate()),0) =  "
                                               + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key),
                                                            ((searchParameter.Value == "Y" || searchParameter.Value == "1") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.LATEST_TO_SIGN_WAIVER)
                        {
                            string topCount = string.Empty;
                            string waiverIdList = string.Empty;
                            try
                            {
                                string[] inputData = searchParameter.Value.Split('|');
                                if (inputData != null && inputData.Length > 1)
                                {
                                    topCount = inputData[0];
                                    if (inputData.Length == 2)
                                    {
                                        waiverIdList = inputData[1];
                                    }
                                    if (string.IsNullOrWhiteSpace(topCount))
                                    {
                                        throw new ValidationException("Invalid parameter value for " + searchParameter.Key);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                throw new Exception("Invalid parameter value for " + searchParameter.Key);
                            }

                            query.Append(joiner + @" EXISTS (SELECT * FROM 
                                                                        (SELECT top " + Convert.ToInt32(topCount).ToString() + @" * 
                                                                           FROM (SELECT csw.SignedFor, csw.CreationDate,
                                                                                        RANK() OVER(PARTITION BY  csw.SignedFor ORDER BY csw.CreationDate DESC) RankVal 
                                                                                   FROM CustomerSignedWaiver csw 
                                                                                  WHERE csw.IsActive=1
                                                                                    AND ISNULL(csw.ExpiryDate,getdate()+1) >= getdate()"+
                                                                                    (string.IsNullOrWhiteSpace(waiverIdList) 
                                                                                      ? " "
                                                                                      : "AND csw.WaiverSetDetailId IN ( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, waiverIdList) + ") ")
                                                                                + @" AND NOT exists (select 1 
                                                                                                      from WaiversSigned ws 
                                                                                                     where ws.CustomerSignedWaiverId = csw.CustomerSignedWaiverId 
                                                                                                       and ws.TrxId is not null)
                                                                                                    ) as tab1 where RankVal = 1 order by CreationDate desc 
                                                                         ) as tab2 where tab2.SignedFor = customers.customer_id )  " );
                            if (string.IsNullOrWhiteSpace(waiverIdList) == false)
                            {
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, waiverIdList));
                            }
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.CHANNEL_USED_TO_SIGN_WAIVER)
                        {
                            query.Append(joiner + @"  EXISTS ( SELECT 1
                                                                 FROM CustomerSignedWaiverHeader cswh, CustomerSignedWaiver csw
                                                                WHERE cswh.CustomerSignedWaiverHeaderId = csw.CustomerSignedWaiverHeaderId
                                                                  AND csw.SignedFor = customers.customer_id
                                                                  AND csw.IsActive=1
                                                                  AND ISNULL(csw.ExpiryDate,getdate()+1) >= getdate()
                                                                  AND Upper(cswh.Channel) = Upper(" + dataAccessHandler.GetParameterName(searchParameter.Key) + ") ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerSearchByParameters.HAS_SIGNED_WAIVER_ID_LIST)
                        {
                            query.Append(joiner + @"  EXISTS ( SELECT 1
                                                                 FROM CustomerSignedWaiverHeader cswh, CustomerSignedWaiver csw
                                                                WHERE cswh.CustomerSignedWaiverHeaderId = csw.CustomerSignedWaiverHeaderId
                                                                  AND csw.SignedFor = customers.customer_id
                                                                  AND csw.IsActive=1
                                                                  AND ISNULL(csw.ExpiryDate,getdate()+1) >= getdate()
                                                                  AND csw.WaiverSetDetailId IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + " ) ) ");
                             parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            string searchKey = DBSearchParameters[searchParameter.Key];
                            if (searchParameter.Value.Contains("EXACT_SEARCH"))
                            {
                                if (searchParameter.Key == CustomerSearchByParameters.CONTACT_ATTRIBUTE1)
                                {
                                    query.Append(joiner + "Isnull(" + searchKey + ",'') = " + dataAccessHandler.GetParameterNameWithSHA256HashByteCommand(searchParameter.Key) + "");
                                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value.Replace("EXACT_SEARCH", "")));
                                }
                                else
                                {
                                    query.Append(joiner + "Isnull(" + searchKey + ",'') = " + dataAccessHandler.GetParameterName(searchParameter.Key) + "");
                                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value.Replace("EXACT_SEARCH", "")));
                                }
                            }
                            else
                            {
                                if (searchParameter.Key == CustomerSearchByParameters.CONTACT_ATTRIBUTE1)
                                {
                                    searchKey = "CONVERT(NVARCHAR(MAX), DECRYPTBYPASSPHRASE(@PassPhrase, Contact.Attribute1))";
                                }
                                query.Append(joiner + "Isnull(" + searchKey + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }

                }
            }
            log.LogMethodExit(query.ToString());
            return query.ToString();
        }

        /// <summary>
        /// Gets the CustomerDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerDTO matching the search criteria</returns>
        public List<CustomerDTO> GetCustomerDTOList(List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomerDTO> list = null;
            string selectQuery = CUSTOMER_SELECT_QUERY;
            selectQuery += GetCustomerFilterQuery(searchParameters);

            if (parameters == null)
                parameters = new List<SqlParameter>();

            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<CustomerDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerDTO customerDTO = GetCustomerDTO(dataRow);
                    list.Add(customerDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the CustomerDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="pageNumber">current page number</param>
        /// <param name="pageSize">page size</param>
        /// <returns>Returns the list of CustomerDTO matching the search criteria</returns>
        public List<CustomerDTO> GetCustomerDTOList(List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters, int pageNumber, int pageSize)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomerDTO> list = null;
            string selectQuery = CUSTOMER_SELECT_QUERY;
            selectQuery += GetCustomerFilterQuery(searchParameters);
            selectQuery += " ORDER BY Customers.customer_id  OFFSET " + (pageNumber * pageSize).ToString() + " ROWS";
            selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";

            if (parameters == null)
                parameters = new List<SqlParameter>();

            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<CustomerDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerDTO customerDTO = GetCustomerDTO(dataRow);
                    list.Add(customerDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Returns the no of customers matching the search criteria
        /// </summary>
        /// <param name="searchCriteria">customer search criteria</param>
        /// <returns>no of customers matching the criteria</returns>
        public int GetCustomerCount(CustomerSearchCriteria searchCriteria)
        {
            log.LogMethodEntry(searchCriteria);
            int customerCount = 0;
            string selectQuery = @" SELECT COUNT(DISTINCT Customers.customer_id) AS TotalCount
                                    FROM Customers
                                    INNER JOIN Profile ON Profile.Id = Customers.ProfileId
                                    LEFT OUTER JOIN ProfileType ON Profile.ProfileTypeId = ProfileType.Id
                                    LEFT OUTER JOIN Contact ON Contact.ProfileId = Profile.Id
                                    LEFT OUTER JOIN ContactType ON ContactType.Id = Contact.ContactTypeId
                                    LEFT OUTER JOIN Address ON Address.ProfileId = Profile.Id
                                    LEFT OUTER JOIN AddressType ON AddressType.Id = Address.AddressTypeId
                                    LEFT OUTER JOIN CustomerSignedWaiverHeader ON CustomerSignedWaiverHeader.SignedBy = Customers.customer_id";
            if (searchCriteria.ContainsCondition)
            {
                selectQuery += (" WHERE " + searchCriteria.GetWhereClause());
            }
            List<SqlParameter> parameterList = new List<SqlParameter>(searchCriteria.GetSqlParameters());
            parameterList.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameterList.ToArray(), sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                customerCount = Convert.ToInt32(dataTable.Rows[0]["TotalCount"]);
            }
            log.LogMethodExit(customerCount);
            return customerCount;
        }

        /// <summary>
        /// Gets the CustomerDTO list matching the search key
        /// </summary>
        /// <param name="searchCriteria">customer search criteria</param>
        /// <returns>Returns the list of CustomerDTO matching the search criteria</returns>
        public List<CustomerDTO> GetCustomerDTOList(CustomerSearchCriteria searchCriteria ,int pageNumber =-1 , int pageSize = -1)
        {
            log.LogMethodEntry(searchCriteria);
            List<CustomerDTO> list = null;
            string selectQuery = CUSTOMER_SELECT_QUERY;
            selectQuery += searchCriteria.GetQuery();
            if (pageNumber > -1 && pageSize > -1)
            {
                selectQuery += " ORDER BY Customers.customer_id  OFFSET " + (pageNumber * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            if (parameters == null)
                parameters = new List<SqlParameter>();
            List<SqlParameter> parameterList = new List<SqlParameter>(searchCriteria.GetSqlParameters());
            parameterList.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameterList.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<CustomerDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerDTO customerDTO = GetCustomerDTO(dataRow);
                    list.Add(customerDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the eligible CustomerDTO list matching parameter conditions
        /// </summary>
        /// <param name="fromDate">fromDate</param>
        /// <param name="toDate">toDate</param>
        /// <param name="qualifyingPoints">qualifyingPoints</param>
        /// <param name="lastRunDate">lastRunDate</param>
        /// <returns>Returns the list of CustomerDTO matching the search criteria</returns>
        public List<int> GetEligibleNewCustomerIDList(DateTime? fromDate, DateTime? toDate, int qualifyingPoints, DateTime lastRunDate, DateTime tillDateTime)
        {
            log.LogMethodEntry(fromDate, toDate, qualifyingPoints, lastRunDate, tillDateTime);
            List<int> list = null;
            string selectQuery = @"SELECT cu.customer_id 
                                     from customers cu 
                                    where cu.MembershipId IS NULL AND ISNULL(cu.CustomerType, 'R') = 'R' 
                                      AND cu.ProfileId is not null
                                      AND Exists (SELECT 1
                                                   FROM CardCreditPlus ccp , 
                                                        cards cardsIn 
                                                   WHERE cu.customer_id = cardsIn.customer_id 
                                                     AND  (cardsIn.valid_flag = 'Y' 
                                                           OR (cardsIn.valid_flag = 'N' AND cardsIn.refund_flag = 'N' 
                                                                 AND cardsIn.ExpiryDate >=  @LastRunDate))
                                                     AND ccp.Card_id = cardsIn.card_id
                                                     AND ( (ccp.LastupdatedDate >= @LastRunDate and ccp.LastupdatedDate < @TillDateTime) 
                                                            OR (cardsIn.last_update_time > =  @LastRunDate AND cardsIn.last_update_time < @TillDateTime) ) 
                                      AND ISNULL( ( SELECT sum(ccpin.CreditPlus) 
                                              FROM CardCreditPlus ccpin,
                                                    cards cardsIn
                                              WHERE ccpin.CreditPLusTYpe='L' 
                                               AND  ISNULL(ccpin.PeriodFrom, ccpin.creationDate)>= " + (fromDate == null ? " ISNULL(cu.CreationDate, cu.Last_Updated_Date) "
                                                                                             : " @FromDate ") +
                                           @"  AND ISNULL(ccpin.PeriodFrom,@toDate) <= @ToDate
                                               AND ccpin.Card_id = cardsIn.card_id
                                               AND cu.customer_id = cardsIn.customer_id
                                               AND cardsIn.valid_flag = 'Y' 
                                              ),0) >= @QualifyingPoints ) ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@FromDate", fromDate));
            if (toDate == null)
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@ToDate", DateTime.Now));
            }
            else
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@ToDate", toDate));
            }
            parameters.Add(dataAccessHandler.GetSQLParameter("@QualifyingPoints", qualifyingPoints));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastRunDate", lastRunDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TillDateTime", tillDateTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            dataAccessHandler.CommandTimeOut = 600;
            log.Debug("dataAccessHandler.CommandTimeOut: " + dataAccessHandler.CommandTimeOut.ToString());
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<int>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int customerId = Convert.ToInt32(dataRow["customer_id"]);
                    list.Add(customerId);
                }
            }
            log.LogMethodExit(list);
            return list;
        } 

        /// <summary>
        /// Gets the eligible CustomerDTO list matching conditions
        /// </summary>        
        /// <param name="lastRunDate">lastRunDate</param>
        /// <returns>Returns the list of CustomerDTO matching the search criteria</returns>
        public List<int> GetEligibleExistingCustomerIDList(DateTime lastRunDate, DateTime tillDateTime)
        {
            log.LogMethodEntry(lastRunDate, tillDateTime);
            List<int> list = null;
            string selectQuery = @"select distinct * 
                                    from (
                                        SELECT cu.customer_id
                                        from customers cu 
                                         WHERE ISNULL(cu.CustomerType, 'R') = 'R' 
                                           AND cu.ProfileId is not null
                                           AND  EXISTS (SELECT 1 
                                                        FROM MembershipProgression mp 
                                                        WHERE mp.CustomerId = cu.customer_id 
                                                            AND mp.MembershipId = cu.MembershipId
                                                            AND mp.EffectiveToDate >= @LastRunDate)
                                           AND (
                                                (Exists (SELECT 1 
                                                        FROM CardCreditPlus cp, cards 
                                                        WHERE cp.Card_id = cards.card_id 
                                                            AND (cards.valid_flag = 'Y' 
                                                                OR 
                                                                (cards.valid_flag = 'N' AND cards.refund_flag = 'N'
                                                                  AND Cards.ExpiryDate >=  @LastRunDate))
                                                            AND cards.customer_id = cu.customer_id
                                                            AND cp.CreditPlusType = 'L'
                                                            AND cp.CreationDate >=  @LastRunDate 
                                                            AND cp.CreationDate < @TillDateTime)
                                                )                                      
                                            )
                                        union all
                                        SELECT cu.customer_id
                                        from customers cu 
                                         WHERE ISNULL(cu.CustomerType, 'R') = 'R' 
                                           AND cu.ProfileId is not null
                                           AND EXISTS (SELECT 1 
                                                        FROM MembershipProgression mp 
                                                        WHERE mp.CustomerId = cu.customer_id 
                                                            AND mp.MembershipId = cu.MembershipId
                                                            AND mp.EffectiveToDate >= @LastRunDate)
                                           AND (								  
                                                (EXISTS ( SELECT 1 
                                                            FROM membershiprewards mr
                                                                OUTER APPLY (SELECT max(ISNULL(AppliedDate,creationDate)) as appliedDate 
                                                                                FROM MembershipRewardsLog ml 
                                                                                WHERE ml.MembershipId =  cu.MembershipId
                                                                                AND ml.MembershipRewardsId = mr.MembershipRewardsId
                                                                                AND ml.customerId = cu.customer_Id) mll
                                                            WHERE mr.membershipId = cu.MembershipId 
                                                            AND mr.IsActive = 1
                                                            AND mr.RewardFrequency > 0
                                                            AND (
                                                                    mll.appliedDate is null 
                                                                    OR
                                                                    (mll.appliedDate <= (CASE mr.UnitOfRewardFrequency 
                                                                                            WHEN 'D' THEN
                                                                                                    DATEADD(DAY, mr.RewardFrequency*-1, @TillDateTime)
                                                                                            WHEN 'M' THEN
                                                                                                    DATEADD(MONTH, mr.RewardFrequency*-1, @TillDateTime)
                                                                                            WHEN 'Y' THEN
                                                                                                    DATEADD(YEAR, mr.RewardFrequency*-1, @TillDateTime)
                                                                                                END ) )
                                                                )
                                                )
                                            )
								            )
                                        ) as custTab";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastRunDate", lastRunDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TillDateTime", tillDateTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            dataAccessHandler.CommandTimeOut = 600;
            log.Debug("dataAccessHandler.CommandTimeOut: " + dataAccessHandler.CommandTimeOut.ToString());
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<int>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int customerId = Convert.ToInt32(dataRow["customer_id"]);
                    list.Add(customerId);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// Gets the eligible CustomerDTO list matching conditions
        /// </summary>         
        /// <returns>Returns the list of CustomerDTO matching conditions</returns>
        public List<int> GetExpiredMembershipCustomerIDList(DateTime tillDateTime)
        {
            log.LogMethodEntry(tillDateTime);
            List<int> list = null;
            string selectQuery = @"SELECT cu.customer_id 
                                     from customers cu 
                                    WHERE EXISTS (SELECT 1 
                                               FROM MembershipProgression mp 
                                              WHERE mp.CustomerId = cu.customer_id 
                                                AND mp.MembershipId = cu.MembershipId 
                                                AND mp.EffectiveToDate = (SELECT MAX(mpin.EffectiveToDate) 
                                                                            FROM MembershipProgression mpin 
                                                                           WHERE mpin.CustomerId = mp.CustomerId
                                                                             AND mpin.MembershipId = mp.MembershipId ) 
                                                AND mp.EffectiveToDate <= @tillDateTime
                                             ) ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@tillDateTime", tillDateTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            dataAccessHandler.CommandTimeOut = 600;
            log.Debug("dataAccessHandler.CommandTimeOut: " + dataAccessHandler.CommandTimeOut.ToString());
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<int>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int customerId = Convert.ToInt32(dataRow["customer_id"]);
                    list.Add(customerId);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the eligible CustomerDTO list matching conditions
        /// </summary>         
        /// <returns>Returns the list of CustomerDTO matching conditions</returns>
        public List<CustomerDTO> GetCustomerForDailyCardBalance()
        {
            log.LogMethodEntry();
            List<CustomerDTO> list = null;
            string selectQuery = CUSTOMER_MEMBERSHIP_SELECT_QUERY;
            selectQuery += @"WHERE EXISTS (SELECT 1 
                                              FROM MembershipRewards MR 
                                             WHERE MR.MembershipId = cu.MembershipId 
                                               AND MR.RewardFunction = 'DTAVP'
                                               AND MR.IsActive = 1 ) 
                                 ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<CustomerDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerDTO customerDTO = GetCustomerDTO(dataRow);
                    list.Add(customerDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        public SortedDictionary<int, string> GetActiveCustomersInDateRangeList(DateTime fromDate, DateTime toDate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(fromDate, toDate, sqlTransaction);
            SortedDictionary<int, string> activeCustomerList = new SortedDictionary<int, string>();

            String query = @"select distinct customers.customer_id, CONCAT(profile.FirstName, ' ', profile.LastName) as customerName from 
                            trx_header th
                            left outer join trx_lines tl on th.TrxId = tl.TrxId
                            left outer join Cards on (th.PrimaryCardId = Cards.card_id or tl.card_id = Cards.card_id) and Cards.customer_id is not null 
                            inner join customers on (th.customerId = customers.customer_id or customers.customer_id = Cards.customer_id)
                            inner join profile on customers.ProfileId = Profile.Id
                            where th.TrxDate >= @FROM_DATE and th.TrxDate <= @TO_DATE
                            UNION
                            select distinct customers.customer_id, CONCAT(profile.FirstName, ' ', profile.LastName) as customerName from 
                            gameplay gp
                            left outer join Cards on gp.card_id = Cards.card_id and Cards.customer_id is not null 
                            inner join customers on customers.customer_id = Cards.customer_id
                            inner join profile on customers.ProfileId = Profile.Id
                            where gp.play_date >= @FROM_DATE and gp.play_date <= @TO_DATE
                            UNION
                            select distinct customers.customer_id, CONCAT(profile.FirstName, ' ', profile.LastName) as customerName from 
                            tasks tk 
                            left outer join Cards on tk.card_id = Cards.card_id and Cards.customer_id is not null 
                            inner join customers on customers.customer_id = Cards.customer_id
                            inner join profile on customers.ProfileId = Profile.Id
                            where tk.task_date >= @FROM_DATE and tk.task_date <= @TO_DATE
                            UNION
                            select distinct Cards.customer_id, CONCAT(profile.FirstName, ' ', profile.LastName) as customerName from 
                            Redemption rd 
                            left outer join Cards on rd.card_id = Cards.card_id and Cards.customer_id is not null 
                            inner join customers on customers.customer_id = Cards.customer_id
                            inner join profile on customers.ProfileId = Profile.Id
                            where rd.redeemed_date >= @FROM_DATE and rd.redeemed_date <= @TO_DATE";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@FROM_DATE", fromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            parameters.Add(new SqlParameter("@TO_DATE", toDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int id = dataRow["customer_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["customer_id"]);
                    string name = dataRow["customerName"] == DBNull.Value ? "" : Convert.ToString(dataRow["customerName"]);
                    if (!activeCustomerList.ContainsKey(id))
                    {
                        activeCustomerList.Add(id, name);
                    }
                }
            }

            log.LogMethodExit(activeCustomerList);
            return activeCustomerList;
        }

        /// <summary>
        /// Gets the List of CustomerDTO DTO
        /// </summary>
        /// <param name="customerIdList"></param>
        /// <returns>CustomerDTOList</returns>
        public List<CustomerDTO> GetCustomerDTOList(List<int> customerIdList)
        {
            log.LogMethodEntry(customerIdList);
            string query = CUSTOMER_SELECT_QUERY + " INNER JOIN @CustomerIdList List ON Customers.customer_id = List.Id ";
            DataTable dataTable = dataAccessHandler.BatchSelect(query, "@CustomerIdList", customerIdList, null, sqlTransaction);
            List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerDTO customerDTO = GetCustomerDTO(dataRow);
                    customerDTOList.Add(customerDTO);
                }
            }
            log.LogMethodExit(customerDTOList);
            return customerDTOList;
        }
    }
}

/********************************************************************************************
 * Project Name - Customers Datahandler Programs 
 * Description  - Data object of the CustomersDatahandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        08-June-2016   Rakshith           Created 
 *2.70.2       19-Jul-2019    Girish Kundar   Modified :Structure of data Handler - insert /Update methods
*                                                     Fix for SQL Injection Issue  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.Customer
{
    public class CustomersDatahandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        string connstring;

        /// <summary>
        /// Default constructor of  CustomersDatahandler class
        /// </summary>
        public CustomersDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            connstring = dataAccessHandler.ConnectionString;
            log.LogMethodExit();

        }
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from customers AS c";
        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<CustomersDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomersDTO.SearchByParameters, string>
        {
            {CustomersDTO.SearchByParameters.EMAIL, "emailEncrypted"},
            {CustomersDTO.SearchByParameters.FNAME, "customer_name"} ,
            {CustomersDTO.SearchByParameters.MNAME, "middle_name"} ,
            {CustomersDTO.SearchByParameters.LNAME, "last_name"} ,
            {CustomersDTO.SearchByParameters.USER_NAME, "Username"},
            {CustomersDTO.SearchByParameters.PHONE, "contactPhone1Encrypted"},
            {CustomersDTO.SearchByParameters.ORDER_BY_LAST_UPDATED_DATE, "last_updated_date"},
            {CustomersDTO.SearchByParameters.ORDER_BY_USERNAME, "Username"},
            {CustomersDTO.SearchByParameters.SITE_ID, "site_id"},
            {CustomersDTO.SearchByParameters.PASSWORD, "Password"}
        };


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Customers Record.
        /// </summary>
        /// <param name="customersDTO">CustomersDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="passPhrase"></param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomersDTO customersDTO, string loginId, int siteId, string passPhrase)
        {
            log.LogMethodEntry(customersDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@customer_id", customersDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userName", string.IsNullOrEmpty(customersDTO.UserName) ? DBNull.Value : (object)customersDTO.UserName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@firstName", string.IsNullOrEmpty(customersDTO.FirstName) ? DBNull.Value : (object)customersDTO.FirstName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@middleName", string.IsNullOrEmpty(customersDTO.MiddleName) ? DBNull.Value : (object)customersDTO.MiddleName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastName", string.IsNullOrEmpty(customersDTO.LastName) ? DBNull.Value : (object)customersDTO.LastName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@emailId", string.IsNullOrEmpty(customersDTO.EmailId) ? DBNull.Value : (object)customersDTO.EmailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@address1", string.IsNullOrEmpty(customersDTO.Address1) ? DBNull.Value : (object)customersDTO.Address1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@address2", string.IsNullOrEmpty(customersDTO.Address2) ? DBNull.Value : (object)customersDTO.Address2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@address3", string.IsNullOrEmpty(customersDTO.Address3) ? DBNull.Value : (object)customersDTO.Address3));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contact_phone1", string.IsNullOrEmpty(customersDTO.Contact_phone1) ? DBNull.Value : (object)customersDTO.Contact_phone1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contact_phone2", string.IsNullOrEmpty(customersDTO.Contact_phone2) ? DBNull.Value : (object)customersDTO.Contact_phone2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@city", string.IsNullOrEmpty(customersDTO.City) ? DBNull.Value : (object)customersDTO.City));
            parameters.Add(dataAccessHandler.GetSQLParameter("@state", string.IsNullOrEmpty(customersDTO.State) ? DBNull.Value : (object)customersDTO.State));
            parameters.Add(dataAccessHandler.GetSQLParameter("@country", string.IsNullOrEmpty(customersDTO.Country) ? DBNull.Value : (object)customersDTO.Country));
            parameters.Add(dataAccessHandler.GetSQLParameter("@postalCode", string.IsNullOrEmpty(customersDTO.PostalCode) ? DBNull.Value : (object)customersDTO.PostalCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@passWord", string.IsNullOrEmpty(customersDTO.PassWord) ? DBNull.Value : (object)customersDTO.PassWord));
            parameters.Add(dataAccessHandler.GetSQLParameter("@photoFileName", string.IsNullOrEmpty(customersDTO.PhotoFileName) ? DBNull.Value : (object)customersDTO.PhotoFileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@notes", string.IsNullOrEmpty(customersDTO.Notes) ? DBNull.Value : (object)customersDTO.Notes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@title", string.IsNullOrEmpty(customersDTO.Title) ? DBNull.Value : (object)customersDTO.Title));
            parameters.Add(dataAccessHandler.GetSQLParameter("@channel", string.IsNullOrEmpty(customersDTO.Channel) ? DBNull.Value : (object)customersDTO.Channel));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dateOfBirth", customersDTO.DateOfBirth.Year == 1900 ? DBNull.Value : (object)customersDTO.DateOfBirth.Year));
            parameters.Add(dataAccessHandler.GetSQLParameter("@anniversaryDate", customersDTO.AnniversaryDate.Year == 1900 ? DBNull.Value : (object)customersDTO.AnniversaryDate.Year));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gender", customersDTO.Gender == "N" ? DBNull.Value : (object)customersDTO.Gender));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(new SqlParameter("@PassphraseEnteredByUser", passPhrase));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CustomersDTO record to the database
        /// </summary>
        /// <param name="customersDTO">CustomersDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="passPhrase">passPhrase</param>
        /// <returns>Returns CustomersDTO</returns>
        public CustomersDTO InsertCustomer(CustomersDTO customersDTO, string loginId, int siteId, string passPhrase)
        {
            log.LogMethodEntry(customersDTO, loginId, siteId);
            string insertCustomersQuery = @"insert into customers 
                                                        (  
                                                            userName,
                                                            customer_name,
                                                            middle_name,
                                                            last_name,
                                                            email,
                                                            birth_date,
                                                            anniversary,
                                                            address1,
                                                            address2,
                                                            address3,
                                                            contact_phone1,
                                                            contact_phone2,
                                                            city,
                                                            state,
                                                            country,
                                                            pin,
                                                            gender,
                                                            passWord,
                                                            last_updated_user,
                                                            last_updated_date,
                                                            photoFileName ,
                                                            Guid,
                                                            site_id,
                                                            notes,
                                                            channel
                                                        ) 
                                                values 
                                                        (
                                                            @userName,
                                                            @firstName,
                                                            @middleName,
                                                            @lastName,
                                                            EncryptByPassPhrase(@PassphraseEnteredByUser, @emailId ),
                                                            EncryptByPassPhrase(@PassphraseEnteredByUser, convert(nvarchar(max), @dateOfBirth,121) ),
                                                            EncryptByPassPhrase(@PassphraseEnteredByUser, convert(nvarchar(max), @anniversaryDate,121) ),
                                                            EncryptByPassPhrase(@PassphraseEnteredByUser, @address1 ),
                                                            EncryptByPassPhrase(@PassphraseEnteredByUser, @address2 ),
                                                            EncryptByPassPhrase(@PassphraseEnteredByUser, @address3 ),
                                                            EncryptByPassPhrase(@PassphraseEnteredByUser, @contact_phone1 ),
                                                            EncryptByPassPhrase(@PassphraseEnteredByUser, @contact_phone2 ),
                                                            @city,
                                                            @state,
                                                            @country,
                                                            @postalCode,
                                                            @gender,
                                                            @passWord,
                                                            @lastUpdatedBy,
                                                            GetDate(),
                                                            @photoFileName ,
                                                            NEWID(),
                                                            @siteId,
                                                            @notes,
                                                            @channel
                                                         ) SELECT * FROM customers WHERE customer_id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertCustomersQuery, GetSQLParameters(customersDTO, loginId, siteId, passPhrase).ToArray(), sqlTransaction);
                RefreshCustomersDTO(customersDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customersDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customersDTO);
            return customersDTO;
        }

        /// <summary>
        /// update the CustomersDTO record to the database
        /// </summary>
        /// <param name="customersDTO">CustomersDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="passPhrase"></param>
        /// <returns>Returns CustomersDTO</returns>
        public CustomersDTO UpdateCustomer(CustomersDTO customersDTO, string loginId, int siteId, string passPhrase)
        {
            log.LogMethodEntry(customersDTO, loginId, siteId);
            string updateCustomerQuery = @"update customers 
                                                          set 
                                                            userName=@userName,
                                                            customer_name=@firstName,
                                                            middle_name=@middleName,
                                                            last_name=@lastName,
                                                            emailEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @emailId ),
                                                            birthDateEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser,convert(nvarchar(max), @dateOfBirth,121) ),
                                                            anniversaryEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, convert(nvarchar(max), @anniversaryDate,121) ),
                                                            address1Encrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @address1 ),
                                                            address2Encrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @address2 ),
                                                            address3Encrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @address3 ),
                                                            contactPhone1Encrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @contact_phone1 ),
                                                            contactPhone2Encrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @contact_phone2 ),
                                                            city=@city,
                                                            state=@state,
                                                            country=@country,
                                                            pin=@postalCode,
                                                            gender=@gender,
                                                            Password = @passWord,
                                                            last_updated_user=@lastUpdatedBy,
                                                            last_updated_date=GetDate(),
                                                            photoFileName =@photoFileName,
                                                            notes=@notes, 
                                                            site_id=@siteId,
                                                            channel=@channel
                                                            where customer_id= @customer_id 
                                        SELECT * FROM customers WHERE customer_id  = @customer_id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateCustomerQuery, GetSQLParameters(customersDTO, loginId, siteId, passPhrase).ToArray(), sqlTransaction);
                RefreshCustomersDTO(customersDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating customersDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customersDTO);
            return customersDTO;

        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customersDTO">CustomersDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustomersDTO(CustomersDTO customersDTO, DataTable dt)
        {
            log.LogMethodEntry(customersDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customersDTO.CustomerId = Convert.ToInt32(dt.Rows[0]["customer_id"]);
                customersDTO.LastUpdatedDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                customersDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                customersDTO.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// return the record from the database
        /// Convert the datarow to CustomersDTO object
        /// </summary>
        /// <returns>return the CustomersDTO object</returns>
        private CustomersDTO GetCustomersDTO(DataRow customerDataRow)
        {
            log.LogMethodEntry(customerDataRow);
            CustomersDTO customersDTO = new CustomersDTO(
                                                    customerDataRow["customer_id"] == DBNull.Value ? -1 : Convert.ToInt32(customerDataRow["customer_id"]),
                                                    customerDataRow["Username"].ToString(),
                                                    customerDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(customerDataRow["site_id"]),
                                                    customerDataRow["customer_name"].ToString(),
                                                    customerDataRow["middle_name"].ToString(),
                                                    customerDataRow["last_name"].ToString(),
                                                    customerDataRow["email"].ToString(),
                                                    customerDataRow["birth_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerDataRow["birth_date"]),
                                                    customerDataRow["anniversary"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerDataRow["anniversary"]),
                                                    customerDataRow["address1"].ToString(),
                                                    customerDataRow["address2"].ToString(),
                                                    customerDataRow["address3"].ToString(),
                                                    customerDataRow["contact_phone1"].ToString(),
                                                    customerDataRow["contact_phone2"].ToString(),
                                                    customerDataRow["notes"].ToString(),
                                                    customerDataRow["city"].ToString(),
                                                    customerDataRow["state"].ToString(),
                                                    customerDataRow["country"].ToString(),
                                                    customerDataRow["pin"].ToString(),
                                                    customerDataRow["gender"].ToString(),
                                                    customerDataRow["Password"].ToString(),
                                                    customerDataRow["last_updated_user"].ToString(),
                                                    customerDataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerDataRow["last_updated_date"]),
                                                    customerDataRow["PhotoFileName"].ToString(),
                                                    customerDataRow["Guid"].ToString(),
                                                    customerDataRow["Title"].ToString(),
                                                    customerDataRow["Channel"].ToString()
                                                 );
            log.LogMethodExit(customersDTO);
            return customersDTO;
        }


        /// <summary>
        /// return the record from the database based on  customerId
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <param name="passPhrase">passPhrase</param>
        /// <returns>return the CustomersDTO object</returns>
        /// or empty CustomersDTO
        public CustomersDTO GetCustomersDTO(int customerId, string passPhrase)
        {
            log.LogMethodEntry(customerId, passPhrase);
            string customersDTOQuery = @"select customer_id,customer_name,city,state,country,pin,gender,notes,Title,company,middle_name,last_name,last_updated_date,last_updated_user,
                                    Designation,PhotoFileName,ExternalSystemRef,CustomDataSetId,RightHanded,TeamUser,Verified,Username,Password,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, address1Encrypted)) as address1,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, address2Encrypted)) as address2,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, address3Encrypted)) as address3,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, emailEncrypted)) as email,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, WeChatAccessTokenEncrypted)) as WeChatAccessToken,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, IDProofFileNameEncrypted)) as IDProofFileName,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,birthDateEncrypted),121) as birth_date,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,anniversaryEncrypted),121) as anniversary,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,contactPhone1Encrypted)) as contact_phone1 ,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,contactPhone2Encrypted)) as contact_phone2,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,FBUserIdEncrypted)) as FBUserId,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,FBAccessTokenEncrypted)) as FBAccessToken,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,TWAccessSecretEncrypted)) as TWAccessSecret,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,TWAccessTokenEncrypted)) as TWAccessToken,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,TaxCodeEncrypted)) as TaxCode, 
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,UniqueIDEncrypted)) as Unique_ID  
                                    from customers
                                    where customer_id = @customerId ";

            SqlParameter[] CustomersDTOparameters = new SqlParameter[1];
            CustomersDTOparameters[0] = new SqlParameter("@customerId", customerId);
            CustomersDTOparameters[1] = new SqlParameter("@PassphraseEnteredByUser", passPhrase);
            DataTable dtCustomersDTO = dataAccessHandler.executeSelectQuery(customersDTOQuery, CustomersDTOparameters, sqlTransaction);
            CustomersDTO customersDTO = new CustomersDTO(); ;
            if (dtCustomersDTO.Rows.Count > 0)
            {
                DataRow customersDTORow = dtCustomersDTO.Rows[0];
                customersDTO = GetCustomersDTO(customersDTORow);
            }
            log.LogMethodExit(customersDTO);
            return customersDTO;
        }

        /// <summary>
        /// Gets the CustomersDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic CustomersDTO matching the search criteria</returns>
        public List<CustomersDTO> GetAllCustomersList(List<KeyValuePair<CustomersDTO.SearchByParameters, string>> searchParameters, string passPhrase)
        {
            log.LogMethodEntry(searchParameters, passPhrase);
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            try
            {
                string selectCustomersDTOQuery = @"select customer_id,customer_name,city,state,country,pin,gender,notes,Title,company,middle_name,last_name,last_updated_date,last_updated_user,
                                    Designation,PhotoFileName,ExternalSystemRef,CustomDataSetId,RightHanded,TeamUser,Verified,Username,Password,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, address1Encrypted)) as address1,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, address2Encrypted)) as address2,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, address3Encrypted)) as address3,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, emailEncrypted)) as email,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, WeChatAccessTokenEncrypted)) as WeChatAccessToken,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, IDProofFileNameEncrypted)) as IDProofFileName,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,birthDateEncrypted),121) as birth_date,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,anniversaryEncrypted),121) as anniversary,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,contactPhone1Encrypted)) as contact_phone1 ,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,contactPhone2Encrypted)) as contact_phone2,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,FBUserIdEncrypted)) as FBUserId,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,FBAccessTokenEncrypted)) as FBAccessToken,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,TWAccessSecretEncrypted)) as TWAccessSecret,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,TWAccessTokenEncrypted)) as TWAccessToken,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,TaxCodeEncrypted)) as TaxCode, 
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,UniqueIDEncrypted)) as Unique_ID
                                    from customers";
                parameters.Add(new SqlParameter("@PassphraseEnteredByUser", passPhrase));

                if ((searchParameters != null) && (searchParameters.Count > 0))
                {
                    StringBuilder query = new StringBuilder(" where ");
                    foreach (KeyValuePair<CustomersDTO.SearchByParameters, string> searchParameter in searchParameters)
                    {
                        if (DBSearchParameters.ContainsKey(searchParameter.Key))
                        {
                            string joinOperartor = (count == 0) ? " " : " and ";
                            string joinOperartorOr = (count == 0) ? " " : " or ";

                            if (searchParameter.Key.Equals(CustomersDTO.SearchByParameters.USER_NAME))
                            {
                                query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if ((searchParameter.Key.Equals(CustomersDTO.SearchByParameters.EMAIL)))
                            {
                                query.Append(joinOperartor + "convert(nvarchar(max), DECRYPTBYPASSPHRASE ('" + passPhrase + "'," + DBSearchParameters[searchParameter.Key] + ")) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if (searchParameter.Key.Equals(CustomersDTO.SearchByParameters.PHONE))
                            {
                                query.Append(joinOperartor + "convert(nvarchar(max), DECRYPTBYPASSPHRASE (" + passPhrase + "," + DBSearchParameters[searchParameter.Key] + ")) =" + dataAccessHandler.GetParameterName(searchParameter.Key) +
                                    "or convert(nvarchar(max), DECRYPTBYPASSPHRASE (" + passPhrase + ", contactPhone2Encrypted)) =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if (searchParameter.Key.Equals(CustomersDTO.SearchByParameters.SITE_ID))
                            {
                                query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key.Equals(CustomersDTO.SearchByParameters.ORDER_BY_LAST_UPDATED_DATE) ||
                               searchParameter.Key.Equals(CustomersDTO.SearchByParameters.ORDER_BY_USERNAME))
                            {
                                //Do nothing -> Skip ->
                                //Avoid executing to else part
                            }
                            else
                            {
                                query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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

                    count = 0;
                    bool orderByExist = false;
                    StringBuilder orderQUery = new StringBuilder();
                    foreach (KeyValuePair<CustomersDTO.SearchByParameters, string> searchParameter in searchParameters)
                    {
                        string comma = (count == 0) ? " " : " , ";

                        if (searchParameter.Key.Equals(CustomersDTO.SearchByParameters.ORDER_BY_LAST_UPDATED_DATE) ||
                            searchParameter.Key.Equals(CustomersDTO.SearchByParameters.ORDER_BY_USERNAME))
                        {
                            orderByExist = true;
                            orderQUery.Append(comma + DBSearchParameters[searchParameter.Key] + " desc ");
                            count++;
                        }
                    }
                    if (orderByExist)
                    {
                        query.Append(" order by " + orderQUery);
                    }
                    if (searchParameters.Count > 0)
                        selectCustomersDTOQuery = selectCustomersDTOQuery + query;

                }
                DataTable dtCustomersDTO = dataAccessHandler.executeSelectQuery(selectCustomersDTOQuery, parameters.ToArray() ,sqlTransaction);

                List<CustomersDTO> cutomersDTOList = new List<CustomersDTO>();
                if (dtCustomersDTO.Rows.Count > 0)
                {
                    foreach (DataRow customersRow in dtCustomersDTO.Rows)
                    {
                        CustomersDTO customersDTO = GetCustomersDTO(customersRow);
                        cutomersDTOList.Add(customersDTO);
                    }

                }
                log.LogMethodExit(cutomersDTOList);
                return cutomersDTOList;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception("At GetAllCustomersList  " + expn.Message.ToString());
            }
        }


        /// <summary>
        ///  GetCardNumber  method
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <returns>returns string cardnumber</returns>
        public string GetCardNumber(int customerId)
        {
            log.LogMethodEntry(customerId);
            try
            {
                string customersDTOQuery = @"select card_number from cards where customer_id=@customerId and valid_flag=@valid_flag";
                SqlParameter[] customersParameters = new SqlParameter[2];
                customersParameters[0] = new SqlParameter("@customerId", customerId);
                customersParameters[1] = new SqlParameter("@valid_flag", 'Y');

                DataTable dtCustomer = dataAccessHandler.executeSelectQuery(customersDTOQuery, customersParameters ,sqlTransaction);
                if (dtCustomer.Rows.Count > 0)
                {
                    DataRow customersDTORow = dtCustomer.Rows[0];
                    log.LogMethodExit(customersDTORow[0].ToString());
                    return customersDTORow[0].ToString();
                }
                else
                {
                    log.LogMethodExit();
                    return null;
                }
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        ///  EnableRoaming(int customerId) Method
        /// </summary>
        /// <param name="customerId">int customerId</param>
        public void EnableRoaming(int customerId)
        {

            log.LogMethodEntry(customerId);
            using (Utilities parafaitUtility = new Utilities(connstring))
            {
                SqlConnection cnn = parafaitUtility.createConnection();
                SqlTransaction SQLTrx = cnn.BeginTransaction();

                try
                {

                    string customersDTOQuery = @"select guid, site_id from customers where customer_id = @customerId";
                    SqlParameter[] customersParameters = new SqlParameter[1];
                    customersParameters[0] = new SqlParameter("@customerId", customerId);

                    DataTable custDT = dataAccessHandler.executeSelectQuery(customersDTOQuery, customersParameters,sqlTransaction);
                    int TopmostAutoRoamOrg; // Updated the Latest logic to get roaming sites 
                    if (custDT.Rows[0]["site_id"] == DBNull.Value)
                        return;

                    string RoamingSites = Semnox.Core.GenericUtilities.DBSynch.getRoamingSites(SQLTrx.Connection, SQLTrx, Convert.ToInt32(custDT.Rows[0]["site_id"]), out TopmostAutoRoamOrg);

                    custDT.TableName = "customers";
                    Semnox.Core.GenericUtilities.DBSynch.CreateRoamingData(custDT, Convert.ToInt32(custDT.Rows[0]["site_id"]), RoamingSites, DateTime.Now, SQLTrx);
                    log.Debug("Ends- EnableRoaming(int customerId)Method.");

                    SQLTrx.Commit();
                }
                catch (Exception ex)
                {
                    SQLTrx.Rollback();
                    throw new Exception("Error Creating Roaming Data " + ex.ToString());
                }
                finally
                {
                    cnn.Close();
                }

            }


        }

        /// <summary>
        /// GetTopCustomer Method
        /// </summary>
        /// <param name="number">number</param>
        /// <param name="email">email</param>
        /// <param name="passPhrase">passPhrase</param>
        /// <returns>returns CustomersDTO object</returns>
        public CustomersDTO GetTopCustomer(string number, string email, string passPhrase)
        {
            log.LogMethodEntry(number, email, passPhrase);
            try
            {
                string customerQuery = @"select TOP 1 *   from customers where";
                string exist = "";
                if (!string.IsNullOrEmpty(number))
                {
                    exist = "and";
                    customerQuery += "    (convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,contactPhone1Encrypted)) = @number  or convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,contactPhone2Encrypted)) =@number)  ";
                }
                if (!string.IsNullOrEmpty(email))
                {

                    customerQuery += exist + "  convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, emailEncrypted)) = @email ";
                }
                if (string.IsNullOrEmpty(number) && string.IsNullOrEmpty(email))
                {
                    return new CustomersDTO();
                }

                SqlParameter[] Customerparameters = new SqlParameter[2];
                Customerparameters[0] = new SqlParameter("@number", number);
                Customerparameters[1] = new SqlParameter("@email", email);
                Customerparameters[1] = new SqlParameter("@PassphraseEnteredByUser", passPhrase);

                DataTable dtCustomersDTO = dataAccessHandler.executeSelectQuery(customerQuery, Customerparameters ,sqlTransaction);
                CustomersDTO customersDTO;
                if (dtCustomersDTO.Rows.Count > 0)
                {
                    DataRow customersDTORow = dtCustomersDTO.Rows[0];
                    customersDTO = GetCustomersDTO(customersDTORow);
                }
                else
                {
                    customersDTO = new CustomersDTO();
                }
                log.LogMethodExit(customersDTO);
                return customersDTO;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// LoginCustomer(CustomerParams customerParams) method
        /// </summary>
        /// <param name="customerParams">customerParams</param>
        /// <param name="passPhrase"></param>
        /// <returns>returns CustomersDTO object</returns>
        public CustomersDTO LoginCustomer(CustomerParams customerParams, string passPhrase)
        {
            CustomersDTO customersDTO = new CustomersDTO();
            if (string.IsNullOrEmpty(customerParams.UserName) || string.IsNullOrEmpty(customerParams.Password))
            {
                throw new Exception("Enter Username/Password.");
            }

            try
            {
                List<KeyValuePair<CustomersDTO.SearchByParameters, string>> customerSearchParams = new List<KeyValuePair<CustomersDTO.SearchByParameters, string>>();
                customerSearchParams.Add(new KeyValuePair<CustomersDTO.SearchByParameters, string>(CustomersDTO.SearchByParameters.USER_NAME, customerParams.UserName));
                customerSearchParams.Add(new KeyValuePair<CustomersDTO.SearchByParameters, string>(CustomersDTO.SearchByParameters.PASSWORD, Encryption.Encrypt(customerParams.Password)));

                List<CustomersDTO> customersDTOList = GetAllCustomersList(customerSearchParams, passPhrase);
                if (customersDTOList.Count == 0)
                {
                    throw new Exception("Incorrect Username or Password");

                }
                else if (customersDTOList.Count == 1)
                {
                    customersDTO = customersDTOList[0];
                    ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
                    UpdateCustomer(customersDTO, "Semnox", customersDTO.SiteId, passPhrase);
                }
                else
                {

                    throw new Exception("Invalid Customer/Too many Customer");
                }
                return customersDTO;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// ForgotPassword(CustomerParams customerParams) Method
        /// </summary>
        /// <param name="customerParams">CustomerParams customerParams</param>
        /// <param name="passPhrase"></param>
        /// <returns>return bools</returns>
        public bool ForgotPassword(CustomerParams customerParams, string passPhrase)
        {
            try
            {
                if (string.IsNullOrEmpty(customerParams.UserName))
                {
                    throw new Exception("Enter Username");
                }

                List<KeyValuePair<CustomersDTO.SearchByParameters, string>> customerSearchParams = new List<KeyValuePair<CustomersDTO.SearchByParameters, string>>();
                customerSearchParams.Add(new KeyValuePair<CustomersDTO.SearchByParameters, string>(CustomersDTO.SearchByParameters.USER_NAME, customerParams.UserName));

                List<CustomersDTO> customersDTOList = GetAllCustomersList(customerSearchParams, passPhrase);
                if (customersDTOList.Count == 0)
                {
                    throw new Exception("Username / Email does not exist");

                }
                else if (customersDTOList.Count == 1)
                {
                    CustomersDTO customersDTO = customersDTOList[0];
                    if (string.IsNullOrEmpty(customersDTO.PassWord.Trim()))
                    {
                        customersDTO.PassWord = Encryption.Encrypt(Guid.NewGuid().ToString().Substring(0, 6));
                        UpdateCustomer(customersDTO, "Semnox", customersDTO.SiteId, passPhrase);
                    }

                    if (string.IsNullOrEmpty(customersDTO.EmailId))
                    {
                        throw new Exception("Email Id does not exist");
                    }

                    EmailTemplateDTO emailTemplateDTO = new EmailTemplate(ExecutionContext.GetExecutionContext()).GetEmailTemplate("Online Reset Password", customerParams.SiteId);

                    string emailContent = "";
                    if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > 0)
                    {
                        emailContent = emailTemplateDTO.EmailTemplate;
                        emailContent.Replace("@password", Encryption.Decrypt(customerParams.Password));
                    }
                    else
                    {
                        emailContent = " Hi " + customersDTO.FirstName + Environment.NewLine + Environment.NewLine +
                        "You have requested to send your password to your registered email address. Your password is: " + Encryption.Decrypt(customersDTO.PassWord) + Environment.NewLine + Environment.NewLine +
                        "Thank you";
                    }
                    using (Utilities parafaitUtility = new Utilities(connstring))
                    {

                        SendEmailUI email = new SendEmailUI(customersDTO.EmailId, "", customersDTO.EmailId, "Account Information", emailContent, "", "", true, parafaitUtility);
                    }
                }
                else
                {

                }
                return true;

            }
            catch
            {
                throw;
            }
        }




    }


}

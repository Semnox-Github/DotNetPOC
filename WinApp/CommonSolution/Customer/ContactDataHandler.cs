/********************************************************************************************
 * Project Name - Contact Data Handler
 * Description  - Data handler of the Contact class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Lakshminarayana     Created 
 *2.60        08-May-2019   Nitin Pai           Added UUID parameter for Guest App
 *2.70.2      19-Jul-2019   Girish Kundar       Modified :Structure of data Handler - insert /Update methods
 *                                                        Fix for SQL Injection Issue
 *2.70.2      05-Dec-2019   Jinto Thomas        Removed siteid from update query
 *2.110.0     10-Dec-2020    Guru S A           For Subscription changes                   
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Customer
{
    /// <summary>
    ///  Contact Data Handler - Handles insert, update and select of  Contact objects
    /// </summary>
    public class ContactDataHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string passPhrase;
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private static readonly Dictionary<ContactDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ContactDTO.SearchByParameters, string>
            {
                {ContactDTO.SearchByParameters.ID, "Contact.Id"},
                {ContactDTO.SearchByParameters.PROFILE_ID, "Contact.ProfileId"},
                {ContactDTO.SearchByParameters.PROFILE_ID_LIST, "Contact.ProfileId"},
                {ContactDTO.SearchByParameters.CONTACT_TYPE_ID, "Contact.ContactTypeId"},
                {ContactDTO.SearchByParameters.CONTACT_TYPE, "ContactType.Name"},
                {ContactDTO.SearchByParameters.ATTRIBUTE1, "Contact.HashAttribute1"},
                {ContactDTO.SearchByParameters.ATTRIBUTE2, "Contact.HashAttribute2"},
                {ContactDTO.SearchByParameters.IS_ACTIVE,"Contact.IsActive"},
                {ContactDTO.SearchByParameters.SITE_ID, "Contact.site_id"},
                {ContactDTO.SearchByParameters.UUID, "Contact.HashUUID"},
                {ContactDTO.SearchByParameters.WHATSAPP_ENABLED, "Contact.WhatsAppEnabled"},
                {ContactDTO.SearchByParameters.CUSTOMER_ID, "CU.Customer_id"},
            };
        
        private static readonly string CONTACT_SELECT_QUERY = @"SELECT Contact.Id, Contact.ContactTypeId, Contact.ProfileId, 
                                                                CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute1)) AS Attribute1, 
                                                                CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.Attribute2)) AS Attribute2, 
                                                                ContactType.Name AS ContactType, Contact.IsActive, Contact.CreatedBy, Contact.CreationDate, 
                                                                Contact.LastUpdatedBy, Contact.LastUpdateDate,
                                                                Contact.site_id, Contact.Guid, Contact.SynchStatus, Contact.MasterEntityId,
                                                                CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Contact.UUID)) AS UUID,
                                                                Contact.WhatsAppEnabled,Contact.AddressId,Contact.CountryId
                                                                FROM Contact
                                                                LEFT OUTER JOIN ContactType ON Contact.ContactTypeId = ContactType.Id ";

        /// <summary>
        /// Default constructor of ContactDataHandler class
        /// </summary>
        public ContactDataHandler(string passPhrase, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry("passPhrase", sqlTransaction);
            this.passPhrase = passPhrase;
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new  DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Contact Record.
        /// </summary>
        /// <param name="contactDTO">ContactDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ContactDTO contactDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(contactDTO, loginId, siteId);
            contactDTO.Attribute1 = contactDTO.Attribute1 != null ? contactDTO.Attribute1.Trim() : string.Empty;
            contactDTO.Attribute2 = contactDTO.Attribute2 != null ? contactDTO.Attribute2.Trim() : string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", contactDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ContactType", contactDTO.ContactType.ToString()));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProfileId", contactDTO.ProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute1", contactDTO.Attribute1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute2", contactDTO.Attribute2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", contactDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", contactDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UUID", contactDTO.UUID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@WhatsAppEnabled", contactDTO.WhatsAppEnabled, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AddressId", contactDTO.AddressId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CountryId", contactDTO.CountryId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Contact record to the database
        /// </summary>
        /// <param name="contactDTO">ContactDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns ContactDTO</returns>
        public ContactDTO InsertContact(ContactDTO contactDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(contactDTO, loginId, siteId);
            string query = @"INSERT INTO Contact 
                                        ( 
                                            ContactTypeId,
                                            ProfileId,
                                            Attribute1,
                                            Attribute2,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            site_id,
                                            MasterEntityId,
                                            UUID,
                                            WhatsAppEnabled,
                                            CountryId,
                                            AddressId,
                                            HashAttribute1,
                                            HashAttribute2,
                                            HashUUID
                                        ) 
                                VALUES 
                                        (
                                            (SELECT Id FROM ContactType WHERE Name = @ContactType),
                                            @ProfileId,
                                            ENCRYPTBYPASSPHRASE(@PassPhrase, @Attribute1),
                                            ENCRYPTBYPASSPHRASE(@PassPhrase, @Attribute2),
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId,
                                            ENCRYPTBYPASSPHRASE(@PassPhrase, @UUID),
                                            @WhatsAppEnabled,
                                            @CountryId,
                                            @AddressId,
                                            hashbytes('SHA2_256',convert(nvarchar(max), upper(@Attribute1))),
                                            hashbytes('SHA2_256',convert(nvarchar(max), upper(@Attribute2))),
                                            hashbytes('SHA2_256',convert(nvarchar(max), upper(@UUID)))
                                        ) SELECT * FROM Contact WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(contactDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshContactDTO(contactDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting contactDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(contactDTO);
            return contactDTO;
        }

        /// <summary>
        /// Updates the Contact record
        /// </summary>
        /// <param name="contactDTO">ContactDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ContactDTO UpdateContact(ContactDTO contactDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(contactDTO, loginId, siteId);
            string query = @"UPDATE Contact 
                             SET ContactTypeId=(SELECT Id FROM ContactType WHERE Name = @ContactType),
                                 ProfileId=@ProfileId,
                                 Attribute1=ENCRYPTBYPASSPHRASE(@PassPhrase, @Attribute1),
                                 Attribute2=ENCRYPTBYPASSPHRASE(@PassPhrase, @Attribute2),
                                 IsActive = @IsActive,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate=GETDATE(),
                                 MasterEntityId=@MasterEntityId,
                                 --site_id = @site_id,
                                HashUUID =CASE                                
                                            WHEN UUID IS NULL 
                                            THEN hashbytes('SHA2_256',convert(nvarchar(max), upper(@UUID)))
                                            ELSE HashUUID
                                        END,
                                 UUID = CASE                                
                                            WHEN UUID IS NULL
                                            THEN ENCRYPTBYPASSPHRASE(@PassPhrase, @UUID)
                                            ELSE UUID
                                        END,
                                WhatsAppEnabled = @WhatsAppEnabled,
                                CountryId = @CountryId,
                                AddressId = @AddressId,
                                HashAttribute1 = hashbytes('SHA2_256',convert(nvarchar(max), upper(@Attribute1))),
                                HashAttribute2 = hashbytes('SHA2_256',convert(nvarchar(max), upper(@Attribute2)))
                             WHERE Id = @Id
                             SELECT * FROM Contact WHERE Id  = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(contactDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshContactDTO(contactDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating contactDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(contactDTO);
            return contactDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="contactDTO">ContactDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshContactDTO(ContactDTO contactDTO, DataTable dt)
        {
            log.LogMethodEntry(contactDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                contactDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                contactDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                contactDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                contactDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                contactDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                contactDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                contactDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Contact type by passing contact type string
        /// </summary>
        /// <param name="contactTypeString">contactTypeString</param>
        /// <returns>ContactType</returns>
        private ContactType GetContactType(string contactTypeString)
        {
            log.LogMethodEntry(contactTypeString);
            ContactType contactType = ContactType.NONE;
            try
            {
                contactType = (ContactType)Enum.Parse(typeof(ContactType), contactTypeString, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while parsing the contact type", ex);
                throw ex;
            }
            log.LogMethodExit(contactType);
            return contactType;
        }

        /// <summary>
        /// Converts the Data row object to ContactDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ContactDTO</returns>
        private ContactDTO GetContactDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ContactDTO contactDTO = new ContactDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["ProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProfileId"]),
                                            dataRow["ContactTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ContactTypeId"]),
                                            dataRow["ContactType"] == DBNull.Value ? GetContactType("NONE") : GetContactType(dataRow["ContactType"].ToString()),
                                            dataRow["Attribute1"] == DBNull.Value ? string.Empty : dataRow["Attribute1"].ToString().Trim(),
                                            dataRow["Attribute2"] == DBNull.Value ? string.Empty : dataRow["Attribute2"].ToString().Trim(),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["UUID"] == DBNull.Value ? string.Empty : dataRow["UUID"].ToString(),
                                            dataRow["WhatsAppEnabled"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["WhatsAppEnabled"]),
                                            dataRow["AddressId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AddressId"]),
                                            dataRow["CountryId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CountryId"])
                                            );
            log.LogMethodExit(contactDTO);
            return contactDTO;
        }

        /// <summary>
        /// Gets the Contact data of passed Contact Id
        /// </summary>
        /// <param name="contactId">integer type parameter</param>
        /// <returns>Returns ContactDTO</returns>
        public ContactDTO GetContactDTO(int contactId)
        {
            log.LogMethodEntry(contactId);
            ContactDTO returnValue = null;
            string query = CONTACT_SELECT_QUERY + " WHERE Contact.Id = @Id";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@Id", contactId, true), dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase) }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetContactDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the ContactDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ContactDTO matching the search criteria</returns>
        public List<ContactDTO> GetContactDTOList(List<KeyValuePair<ContactDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ContactDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = CONTACT_SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ContactDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == ContactDTO.SearchByParameters.ID ||
                            searchParameter.Key == ContactDTO.SearchByParameters.CONTACT_TYPE_ID ||
                            searchParameter.Key == ContactDTO.SearchByParameters.PROFILE_ID ||
                            searchParameter.Key == ContactDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ContactDTO.SearchByParameters.CONTACT_TYPE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'NONE') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ContactDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ContactDTO.SearchByParameters.PROFILE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ContactDTO.SearchByParameters.ATTRIBUTE1 ||
                                 searchParameter.Key == ContactDTO.SearchByParameters.ATTRIBUTE2 ||
                                 searchParameter.Key == ContactDTO.SearchByParameters.UUID)
                        {
                            // query.Append(joiner + "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase," + DBSearchParameters[searchParameter.Key] + ")) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            // parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            query.Append(joiner + " " + DBSearchParameters[searchParameter.Key] 
                                               + "  = " + dataAccessHandler.GetParameterNameWithSHA256HashByteCommand(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ContactDTO.SearchByParameters.IS_ACTIVE || searchParameter.Key == ContactDTO.SearchByParameters.WHATSAPP_ENABLED)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == ContactDTO.SearchByParameters.CUSTOMER_ID  )
                        {
                            query.Append(joiner + @" EXISTS (SELECT 1 
                                                               FROM customers cu, profile pf 
                                                              where cu.profileId = pf.Id
                                                                 and pf.Id = Contact.ProfileId
                                                                 and " + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                selectQuery = selectQuery + query;
            }
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<ContactDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ContactDTO contactDTO = GetContactDTO(dataRow);
                    list.Add(contactDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the ContactDTO List for profileId List
        /// </summary>
        /// <param name="profileIdList">integer list parameter</param>
        /// <returns>Returns List of ContactDTO</returns>
        public List<ContactDTO> GetContactDTOList(List<int> profileIdList, bool activeRecords)
        {
            log.LogMethodEntry(profileIdList);
            List<ContactDTO> list = null;
            string query = CONTACT_SELECT_QUERY + @" INNER JOIN @ProfileIdList List ON Contact.ProfileId = List.Id";
            if (activeRecords)
            {
                query += " WHERE Contact.IsActive = 1 ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@ProfileIdList", profileIdList, new []{ dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase)}, sqlTransaction);
            if (table.Rows.Count > 0)
            {
                list = new List<ContactDTO>();
                foreach (DataRow dataRow in table.Rows)
                {
                    ContactDTO contactDTO = GetContactDTO(dataRow);
                    list.Add(contactDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        public List<ContactDTO> GetAddressContactDTOList(List<int> addressIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(addressIdList);
            List<ContactDTO> contactDTOList = new List<ContactDTO>();
            string query = CONTACT_SELECT_QUERY + @" INNER JOIN @addressIdList List ON Contact.AddressId = List.Id";
            if (activeRecords)
            {
                query += " WHERE Contact.IsActive = 1 ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@addressIdList", addressIdList, new[] { dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase) }, sqlTransaction);
            if (table.Rows.Count > 0)
            {
                contactDTOList = new List<ContactDTO>();
                foreach (DataRow dataRow in table.Rows)
                {
                    ContactDTO contactDTO = GetContactDTO(dataRow);
                    contactDTOList.Add(contactDTO);
                }
            }
            log.LogMethodExit(contactDTOList);
            return contactDTOList;
        }
    }
}

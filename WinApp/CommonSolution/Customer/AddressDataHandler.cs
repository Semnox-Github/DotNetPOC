/********************************************************************************************
 * Project Name - Address Data Handler
 * Description  - Data handler of the Address class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Lakshminarayana     Created 
 *2.70.2       19-Jul-2019    Girish Kundar          Modified :Structure of data Handler - insert /Update methods
 *                                                          Fix for SQL Injection Issue  
 *2.70.2        05-Dec-2019   Jinto Thomas            Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Customer
{
    /// <summary>
    ///  Address Data Handler - Handles insert, update and select of  Address objects
    /// </summary>
    public class AddressDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string passPhrase;
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private static readonly Dictionary<AddressDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AddressDTO.SearchByParameters, string>
            {
                {AddressDTO.SearchByParameters.ID, "Address.Id"},
                {AddressDTO.SearchByParameters.PROFILE_ID, "Address.ProfileId"},
                {AddressDTO.SearchByParameters.PROFILE_ID_LIST, "Address.ProfileId"},
                {AddressDTO.SearchByParameters.ADDRESS_TYPE_ID, "Address.AddressTypeId"},
                {AddressDTO.SearchByParameters.ADDRESS_TYPE, "AddressType.Name"},
                {AddressDTO.SearchByParameters.IS_ACTIVE,"Address.IsActive"},
                {AddressDTO.SearchByParameters.SITE_ID, "Address.site_id"},
                {AddressDTO.SearchByParameters.MASTER_ENTITY_ID, "Address.MasterEntityId"}
            };
        
        private static readonly string ADDRESS_SELECT_QUERY = @"SELECT Address.Id, Address.AddressTypeId, Address.ProfileId, 
                                                                CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line1)) AS Line1, 
                                                                CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line2)) AS Line2, 
                                                                CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Address.Line3)) AS Line3, 
                                                                Address.City, Address.StateId, Address.PostalCode, Address.CountryId, AddressType.Name AS AddressType,
                                                                Address.IsActive, Address.CreatedBy, Address.CreationDate, 
                                                                Address.LastUpdatedBy, Address.LastUpdateDate, Address.site_id, Address.Guid, Address.SynchStatus, Address.MasterEntityId, Address.IsDefault,
                                                                State.State, State.Description AS StateName, Country.CountryName, Address.IsDefault
                                                                FROM Address
                                                                LEFT OUTER JOIN AddressType ON Address.AddressTypeId = AddressType.Id 
                                                                LEFT OUTER JOIN State ON State.StateId = Address.StateId 
                                                                LEFT OUTER JOIN Country ON Country.CountryId = Address.CountryId ";

        /// <summary>
        /// Default constructor of AddressDataHandler class
        /// </summary>
        public AddressDataHandler(string passPhrase, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry("passPhrase", sqlTransaction);
            this.passPhrase = passPhrase;
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Address Record.
        /// </summary>
        /// <param name="addressDTO">AddressDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AddressDTO addressDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(addressDTO, loginId, siteId);
            addressDTO.Line1 = addressDTO.Line1 != null ? addressDTO.Line1.Trim() : string.Empty;
            addressDTO.Line2 = addressDTO.Line2 != null ? addressDTO.Line2.Trim() : string.Empty;
            addressDTO.Line3 = addressDTO.Line3 != null ? addressDTO.Line3.Trim() : string.Empty;
            addressDTO.City = addressDTO.City != null ? addressDTO.City.Trim() : string.Empty;
            addressDTO.PostalCode = addressDTO.PostalCode != null ? addressDTO.PostalCode.Trim() : string.Empty;

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", addressDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AddressType", addressDTO.AddressType.ToString()));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProfileId", addressDTO.ProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Line1", addressDTO.Line1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Line2", addressDTO.Line2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Line3", addressDTO.Line3));
            parameters.Add(dataAccessHandler.GetSQLParameter("@City", addressDTO.City));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StateId", addressDTO.StateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PostalCode", addressDTO.PostalCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CountryId", addressDTO.CountryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsDefault", addressDTO.IsDefault));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", addressDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", addressDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Address record to the database
        /// </summary>
        /// <param name="addressDTO">AddressDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns AddressDTO</returns>
        public AddressDTO InsertAddress(AddressDTO addressDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(addressDTO, loginId, siteId);
            string query = @"INSERT INTO Address 
                                        ( 
                                            AddressTypeId,
                                            ProfileId,
                                            Line1,
                                            Line2,
                                            Line3,
                                            City,
                                            StateId,
                                            PostalCode,
                                            CountryId,
                                            IsDefault,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            site_id,
                                            MasterEntityId,
                                            HashLine1,
                                            HashLine2,
                                            HashLine3
                                        ) 
                                VALUES 
                                        (
                                            (SELECT Id FROM AddressType WHERE Name = @AddressType),
                                            @ProfileId,
                                            ENCRYPTBYPASSPHRASE(@PassPhrase, @Line1),
                                            ENCRYPTBYPASSPHRASE(@PassPhrase, @Line2),
                                            ENCRYPTBYPASSPHRASE(@PassPhrase, @Line3),
                                            @City,
                                            @StateId,
                                            @PostalCode,
                                            @CountryId,
                                            @IsDefault,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId,
                                            hashbytes('SHA2_256',convert(nvarchar(max), upper(@Line1))),
                                            hashbytes('SHA2_256',convert(nvarchar(max), upper(@Line2))),
                                            hashbytes('SHA2_256',convert(nvarchar(max), upper(@Line3)))
                                        ) SELECT * FROM Address WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(addressDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAddressDTO(addressDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting addressDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(addressDTO);
            return addressDTO;
        }

        /// <summary>
        /// Updates the Address record
        /// </summary>
        /// <param name="addressDTO">AddressDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns theAddressDTO</returns>
        public AddressDTO UpdateAddress(AddressDTO addressDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(addressDTO, loginId, siteId);
            string query = @"UPDATE Address 
                             SET AddressTypeId=(SELECT Id FROM AddressType WHERE Name = @AddressType),
                                 ProfileId=@ProfileId,
                                 Line1=ENCRYPTBYPASSPHRASE(@PassPhrase, @Line1),
                                 Line2=ENCRYPTBYPASSPHRASE(@PassPhrase, @Line2),
                                 Line3=ENCRYPTBYPASSPHRASE(@PassPhrase, @Line3),
                                 City=@City,
                                 StateId=@StateId,
                                 PostalCode=@PostalCode,
                                 CountryId=@CountryId,
                                 IsDefault=@IsDefault,
                                 IsActive = @IsActive,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate=GETDATE(),
                                 MasterEntityId=@MasterEntityId,
                                 --site_id = @site_id
                                HashLine1 = hashbytes('SHA2_256',convert(nvarchar(max), upper(@Line1))),
                                HashLine2 = hashbytes('SHA2_256',convert(nvarchar(max), upper(@Line2))),
                                HashLine3 = hashbytes('SHA2_256',convert(nvarchar(max), upper(@Line3)))
                             WHERE Id = @Id
                             SELECT * FROM Address WHERE Id  = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(addressDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAddressDTO(addressDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating addressDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(addressDTO);
            return addressDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="addressDTO">AddressDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAddressDTO(AddressDTO addressDTO, DataTable dt)
        {
            log.LogMethodEntry(addressDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                addressDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                addressDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                addressDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                addressDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                addressDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                addressDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                addressDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Gets the Address Type by passing address Type string
        /// </summary>
        /// <param name="addressTypeString">addressTypeString</param>
        /// <returns>AddressType</returns>
        private AddressType GetAddressType(string addressTypeString)
        {
            log.LogMethodEntry(addressTypeString);
            AddressType addressType = AddressType.NONE;
            try
            {
                addressType = (AddressType)Enum.Parse(typeof(AddressType), addressTypeString, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while parsing the address type", ex);
                throw ex;
            }
            log.LogMethodExit(addressType);
            return addressType;
        }
        /// <summary>
        /// Converts the Data row object to AddressDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AddressDTO</returns>
        private AddressDTO GetAddressDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AddressDTO addressDTO = new AddressDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["ProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProfileId"]),
                                            dataRow["AddressTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AddressTypeId"]),
                                            dataRow["AddressType"] == DBNull.Value ? GetAddressType("NONE") : GetAddressType(dataRow["AddressType"].ToString()),
                                            dataRow["Line1"] == DBNull.Value ? string.Empty : dataRow["Line1"].ToString().Trim(),
                                            dataRow["Line2"] == DBNull.Value ? string.Empty : dataRow["Line2"].ToString().Trim(),
                                            dataRow["Line3"] == DBNull.Value ? string.Empty : dataRow["Line3"].ToString().Trim(),
                                            dataRow["City"] == DBNull.Value ? string.Empty : dataRow["City"].ToString().Trim(),
                                            dataRow["StateId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["StateId"]),
                                            dataRow["PostalCode"] == DBNull.Value ? string.Empty : dataRow["PostalCode"].ToString().Trim(),
                                            dataRow["CountryId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CountryId"]),
                                            dataRow["State"] == DBNull.Value ? string.Empty : dataRow["State"].ToString().Trim(),
                                            dataRow["StateName"] == DBNull.Value ? string.Empty : dataRow["StateName"].ToString().Trim(),
                                            dataRow["CountryName"] == DBNull.Value ? string.Empty : dataRow["CountryName"].ToString().Trim(),
                                            dataRow["IsDefault"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsDefault"]),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(addressDTO);
            return addressDTO;
        }

        /// <summary>
        /// Gets the Address data of passed Address Id
        /// </summary>
        /// <param name="addressId">integer type parameter</param>
        /// <returns>Returns AddressDTO</returns>
        public AddressDTO GetAddressDTO(int addressId)
        {
            log.LogMethodEntry(addressId);
            AddressDTO returnValue = null;
            string query = ADDRESS_SELECT_QUERY + " WHERE Address.Id = @Id";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@Id", addressId, true), dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase) }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetAddressDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the AddressDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AddressDTO matching the search criteria</returns>
        public List<AddressDTO> GetAddressDTOList(List<KeyValuePair<AddressDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AddressDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = ADDRESS_SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AddressDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == AddressDTO.SearchByParameters.ID ||
                            searchParameter.Key == AddressDTO.SearchByParameters.ADDRESS_TYPE_ID ||
                            searchParameter.Key == AddressDTO.SearchByParameters.PROFILE_ID ||
                            searchParameter.Key == AddressDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AddressDTO.SearchByParameters.ADDRESS_TYPE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'NONE') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AddressDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AddressDTO.SearchByParameters.PROFILE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AddressDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                list = new List<AddressDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AddressDTO addressDTO = GetAddressDTO(dataRow);
                    list.Add(addressDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the AddressDTO List for profileId List
        /// </summary>
        /// <param name="profileIdList">integer list parameter</param>
        /// <returns>Returns List of AddressDTO</returns>
        public List<AddressDTO> GetAddressDTOList(List<int> profileIdList, bool activeRecords)
        {
            log.LogMethodEntry(profileIdList);
            List<AddressDTO> list = null;
            string query = ADDRESS_SELECT_QUERY + @" INNER JOIN @ProfileIdList List ON Address.ProfileId = List.Id";
            if (activeRecords)
            {
                query += " WHERE Address.IsActive = 1 ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@ProfileIdList", profileIdList, new []{ dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase)}, sqlTransaction);
            if (table.Rows.Count > 0)
            {
                list = new List<AddressDTO>();
                foreach (DataRow dataRow in table.Rows)
                {
                    AddressDTO addressDTO = GetAddressDTO(dataRow);
                    list.Add(addressDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

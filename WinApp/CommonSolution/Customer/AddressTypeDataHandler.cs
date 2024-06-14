/********************************************************************************************
 * Project Name - AddressType Data Handler
 * Description  - Data handler of the AddressType class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Lakshminarayana     Created 
 *2.70.2       19-Jul-2019     Girish Kundar       Modified :Structure of data Handler - insert /Update methods
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
    ///  AddressType Data Handler - Handles insert, update and select of  AddressType objects
    /// </summary>
    public class AddressTypeDataHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from AddressType AS at";
        private static readonly Dictionary<AddressTypeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AddressTypeDTO.SearchByParameters, string>
            {
                {AddressTypeDTO.SearchByParameters.ID, "at.Id"},
                {AddressTypeDTO.SearchByParameters.NAME, "at.Name"},
                {AddressTypeDTO.SearchByParameters.IS_ACTIVE,"at.IsActive"},
                {AddressTypeDTO.SearchByParameters.SITE_ID, "at.site_id"},
                {AddressTypeDTO.SearchByParameters.MASTER_ENTITY_ID, "at.MasterEntityId"}
            };
         

        /// <summary>
        /// Default constructor of AddressTypeDataHandler class
        /// </summary>
        public AddressTypeDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new  DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AddressType Record.
        /// </summary>
        /// <param name="addressTypeDTO">AddressTypeDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AddressTypeDTO addressTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(addressTypeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", addressTypeDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", addressTypeDTO.Name, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", addressTypeDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", addressTypeDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", addressTypeDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AddressType record to the database
        /// </summary>
        /// <param name="addressTypeDTO">AddressTypeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns AddressTypeDTO</returns>
        public AddressTypeDTO InsertAddressType(AddressTypeDTO addressTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(addressTypeDTO, loginId, siteId);
            string query = @"INSERT INTO AddressType 
                                        ( 
                                            Name,
                                            Description,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            site_id,
                                            MasterEntityId

                                        ) 
                                VALUES 
                                        (
                                            @Name,
                                            @Description,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                        ) SELECT * FROM AddressType WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(addressTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAddressTypeDTO(addressTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting addressTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(addressTypeDTO);
            return addressTypeDTO;
        }

        /// <summary>
        /// Updates the AddressType record
        /// </summary>
        /// <param name="addressTypeDTO">AddressTypeDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the AddressTypeDTO</returns>
        public AddressTypeDTO UpdateAddressType(AddressTypeDTO addressTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(addressTypeDTO, loginId, siteId);
            string query = @"UPDATE AddressType 
                             SET Name=@Name,
                                 Description=@Description,
                                 IsActive = @IsActive,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate=GETDATE(),
                                 MasterEntityId=@MasterEntityId
                                 --site_id = @site_id
                             WHERE Id = @Id 
                       SELECT * FROM AddressType WHERE Id  = @Id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(addressTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAddressTypeDTO(addressTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating addressTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(addressTypeDTO);
            return addressTypeDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="addressTypeDTO">AddressTypeDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAddressTypeDTO(AddressTypeDTO addressTypeDTO, DataTable dt)
        {
            log.LogMethodEntry(addressTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                addressTypeDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                addressTypeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                addressTypeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                addressTypeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                addressTypeDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                addressTypeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                addressTypeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AddressTypeDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AddressTypeDTO</returns>
        private AddressTypeDTO GetAddressTypeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AddressTypeDTO addressTypeDTO = new AddressTypeDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                            dataRow["Description"] == DBNull.Value ? string.Empty : dataRow["Description"].ToString(),
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
            log.LogMethodExit(addressTypeDTO);
            return addressTypeDTO;
        }

        /// <summary>
        /// Gets the AddressType data of passed AddressType Id
        /// </summary>
        /// <param name="addressTypeId">integer type parameter</param>
        /// <returns>Returns AddressTypeDTO</returns>
        public AddressTypeDTO GetAddressTypeDTO(int addressTypeId)
        {
            log.LogMethodEntry(addressTypeId);
            AddressTypeDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE at.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", addressTypeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetAddressTypeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        

        /// <summary>
        /// Gets the AddressTypeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AddressTypeDTO matching the search criteria</returns>
        public List<AddressTypeDTO> GetAddressTypeDTOList(List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AddressTypeDTO> list = null;
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AddressTypeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == AddressTypeDTO.SearchByParameters.ID
                            || searchParameter.Key == AddressTypeDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                           
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AddressTypeDTO.SearchByParameters.SITE_ID)
                        {
                            
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AddressTypeDTO.SearchByParameters.IS_ACTIVE)
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
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<AddressTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AddressTypeDTO addressTypeDTO = GetAddressTypeDTO(dataRow);
                    list.Add(addressTypeDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        internal DateTime? GetAddressTypeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdateDate) LastUpdateDate
                            FROM (
                            select max(LastUpdateDate) LastUpdateDate from AddressType WHERE (site_id = @siteId or @siteId = -1)
                             ) modefierSetlastupdate";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdateDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdateDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}

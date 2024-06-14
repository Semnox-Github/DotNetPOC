/********************************************************************************************
 * Project Name - ContactType Data Handler
 * Description  - Data handler of the ContactType class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Lakshminarayana     Created 
 *2.70.2       19-Jul-2019    Girish Kundar        Modified :Structure of data Handler - insert /Update methods
 *                                                        Fix for SQL Injection Issue 
 *2.70.2        05-Dec-2019   Jinto Thomas            Removed siteid from update query
 *2.70.2        18-Dec-2019   Jinto Thomas            added parameter executioncontext for userrole object declaration
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
    ///  ContactType Data Handler - Handles insert, update and select of  ContactType objects
    /// </summary>
    public class ContactTypeDataHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * from ContactType AS ct";
        private DataAccessHandler dataAccessHandler;
        private static readonly Dictionary<ContactTypeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ContactTypeDTO.SearchByParameters, string>
            {
                {ContactTypeDTO.SearchByParameters.ID, "ct.Id"},
                {ContactTypeDTO.SearchByParameters.NAME, "ct.Name"},
                {ContactTypeDTO.SearchByParameters.IS_ACTIVE,"ct.IsActive"},
                {ContactTypeDTO.SearchByParameters.MASTER_ENTITY_ID,"ct.MasterEntityId"},
                {ContactTypeDTO.SearchByParameters.SITE_ID, "ct.site_id"}
            };
         

        /// <summary>
        /// Default constructor of ContactTypeDataHandler class
        /// </summary>
        public ContactTypeDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new  DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ContactType Record.
        /// </summary>
        /// <param name="contactTypeDTO">ContactTypeDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ContactTypeDTO contactTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(contactTypeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", contactTypeDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", contactTypeDTO.Name, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", contactTypeDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", contactTypeDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", contactTypeDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ContactType record to the database
        /// </summary>
        /// <param name="contactTypeDTO">ContactTypeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns ContactTypeDTO</returns>
        public ContactTypeDTO InsertContactType(ContactTypeDTO contactTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(contactTypeDTO, loginId, siteId);
            string query = @"INSERT INTO ContactType 
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
                                        ) SELECT * FROM ContactType WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(contactTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshContactTypeDTO(contactTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting contactTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(contactTypeDTO);
            return contactTypeDTO;
        }

        /// <summary>
        /// Updates the ContactType record
        /// </summary>
        /// <param name="contactTypeDTO">ContactTypeDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns ContactTypeDTO</returns>
        public ContactTypeDTO UpdateContactType(ContactTypeDTO contactTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(contactTypeDTO, loginId, siteId);
            string query = @"UPDATE ContactType 
                             SET Name=@Name,
                                 Description=@Description,
                                 IsActive = @IsActive,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate=GETDATE(),
                                 MasterEntityId=@MasterEntityId
                                 --site_id = @site_id
                           WHERE Id = @Id
                           SELECT * FROM ContactType WHERE Id  =  @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(contactTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshContactTypeDTO(contactTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating contactTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(contactTypeDTO);
            return contactTypeDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="contactTypeDTO">ContactTypeDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshContactTypeDTO(ContactTypeDTO contactTypeDTO, DataTable dt)
        {
            log.LogMethodEntry(contactTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                contactTypeDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                contactTypeDTO.CreatedBy =  dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                contactTypeDTO.CreationDate =  dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                contactTypeDTO.LastUpdatedBy =  dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                contactTypeDTO.LastUpdateDate =  dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                contactTypeDTO.SiteId =  dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                contactTypeDTO.Guid =  dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to ContactTypeDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ContactTypeDTO</returns>
        private ContactTypeDTO GetContactTypeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ContactTypeDTO contactTypeDTO = new ContactTypeDTO(Convert.ToInt32(dataRow["Id"]),
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
            log.LogMethodExit(contactTypeDTO);
            return contactTypeDTO;
        }

        /// <summary>
        /// Gets the ContactType data of passed ContactType Id
        /// </summary>
        /// <param name="contactTypeId">integer type parameter</param>
        /// <returns>Returns ContactTypeDTO</returns>
        public ContactTypeDTO GetContactTypeDTO(int contactTypeId)
        {
            log.LogMethodEntry(contactTypeId);
            ContactTypeDTO returnValue = null;
            string query =SELECT_QUERY + "   WHERE ct.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", contactTypeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetContactTypeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the ContactTypeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ContactTypeDTO matching the search criteria</returns>
        public List<ContactTypeDTO> GetContactTypeDTOList(List<KeyValuePair<ContactTypeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ContactTypeDTO> list = null;
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ContactTypeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == ContactTypeDTO.SearchByParameters.ID
                            || searchParameter.Key == ContactTypeDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                           
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ContactTypeDTO.SearchByParameters.SITE_ID)
                        {
                            
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ContactTypeDTO.SearchByParameters.IS_ACTIVE)
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
                list = new List<ContactTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ContactTypeDTO contactTypeDTO = GetContactTypeDTO(dataRow);
                    list.Add(contactTypeDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

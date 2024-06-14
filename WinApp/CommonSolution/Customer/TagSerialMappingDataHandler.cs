/********************************************************************************************
 * Project Name - Tag Serial Mapping Data Handler
 * Description  - Data handler of the Tag Serial Mapping class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       19-Jul-2019    Girish Kundar       Modified : Fix for SQL Injection Issue  
 *2.140.0      12-Dec-2021    Guru S A            Booking execute process performance fixes
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
    ///  TagSerialMapping Data Handler - Handles insert, update and select of  TagSerialMapping objects
    /// </summary>
    public class TagSerialMappingDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<TagSerialMappingDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TagSerialMappingDTO.SearchByParameters, string>
            {
                {TagSerialMappingDTO.SearchByParameters.SERIAL_NUMBER, "csm.SerialNumber"},
                {TagSerialMappingDTO.SearchByParameters.TAG_NUMBER, "csm.CardNumber"},
                {TagSerialMappingDTO.SearchByParameters.TAG_SERIAL_MAPPING_ID, "csm.Id"},
                {TagSerialMappingDTO.SearchByParameters.MASTER_ENTITY_ID, "csm.MasterEntityId"},
                {TagSerialMappingDTO.SearchByParameters.SITE_ID, "csm.site_id"},
                {TagSerialMappingDTO.SearchByParameters.ALREADY_ISSUED, ""},
                {TagSerialMappingDTO.SearchByParameters.SERIAL_NUMBER_FROM, "csm.SerialNumber"},
                {TagSerialMappingDTO.SearchByParameters.SERIAL_NUMBER_TO, "csm.SerialNumber"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CardSerialMapping AS csm";
        /// <summary>
        /// Default constructor of TagSerialMappingDataHandler class
        /// </summary>
        public TagSerialMappingDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating TagSerialMapping Record.
        /// </summary>
        /// <param name="tagSerialMappingDTO">TagSerialMappingDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(TagSerialMappingDTO tagSerialMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tagSerialMappingDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", tagSerialMappingDTO.TagSerialMappingId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SerialNumber", tagSerialMappingDTO.SerialNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardNumber", tagSerialMappingDTO.TagNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", tagSerialMappingDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the TagSerialMapping record to the database
        /// </summary>
        /// <param name="tagSerialMappingDTO">TagSerialMappingDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns TagSerialMappingDTO</returns>
        public TagSerialMappingDTO InsertTagSerialMapping(TagSerialMappingDTO tagSerialMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tagSerialMappingDTO, loginId, siteId);
            string query = @"INSERT INTO CardSerialMapping 
                                        ( 
                                            SerialNumber,
                                            CardNumber,
                                            CreationDate,
                                            CreatedBy,
                                            site_id,
                                            MasterEntityId,
                                            LastUpdatedBy,
                                            LastUpdateDate 
                                        ) 
                                VALUES 
                                        (
                                            @SerialNumber,
                                            @CardNumber,
                                            GETDATE(),
                                            @CreatedBy,
                                            @site_id,
                                            @MasterEntityId,
                                            @LastUpdatedBy,
                                            GetDate()
                                        )
                                        SELECT * FROM CardSerialMapping WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(tagSerialMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTagSerialMappingDTO(tagSerialMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting tagSerialMappingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(tagSerialMappingDTO);
            return tagSerialMappingDTO;
        }

        /// <summary>
        /// Updates the TagSerialMapping record
        /// </summary>
        /// <param name="tagSerialMappingDTO">TagSerialMappingDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns updated TagSerialMapping record</returns>
        public TagSerialMappingDTO UpdateTagSerialMapping(TagSerialMappingDTO tagSerialMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tagSerialMappingDTO, loginId, siteId);
            string query = @"UPDATE CardSerialMapping 
                             SET SerialNumber = @SerialNumber,
                                 CardNumber = @CardNumber,
                                 MasterEntityId = @MasterEntityId,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate = GetDate()
                             WHERE Id = @Id 
                             SELECT * FROM CardSerialMapping WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(tagSerialMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTagSerialMappingDTO(tagSerialMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating tagSerialMappingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(tagSerialMappingDTO);
            return tagSerialMappingDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="tagSerialMappingDTO">TagSerialMappingDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshTagSerialMappingDTO(TagSerialMappingDTO tagSerialMappingDTO, DataTable dt)
        {
            log.LogMethodEntry(tagSerialMappingDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                tagSerialMappingDTO.TagSerialMappingId = Convert.ToInt32(dt.Rows[0]["Id"]);
                tagSerialMappingDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                tagSerialMappingDTO.Guid = dataRow["guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["guid"]);
                tagSerialMappingDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                tagSerialMappingDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                tagSerialMappingDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                tagSerialMappingDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Deletes the TagSerialMapping record of passed TagSerialMapping Id
        /// </summary>
        /// <param name="tagSerialMappingId">integer type parameter</param>
        public void DeleteTagSerialMapping(int tagSerialMappingId)
        {
            log.LogMethodEntry(tagSerialMappingId);
            string query = @"DELETE  
                             FROM CardSerialMapping
                             WHERE CardSerialMapping.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", tagSerialMappingId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to TagSerialMappingDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns TagSerialMappingDTO</returns>
        private TagSerialMappingDTO GetTagSerialMappingDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TagSerialMappingDTO tagSerialMappingDTO = new TagSerialMappingDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["SerialNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SerialNumber"]),
                                            dataRow["CardNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CardNumber"]),
                                            dataRow["CreationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(tagSerialMappingDTO);
            return tagSerialMappingDTO;
        }

        /// <summary>
        /// Gets the TagSerialMapping data of passed TagSerialMapping Id
        /// </summary>
        /// <param name="tagSerialMappingId">integer type parameter</param>
        /// <returns>Returns TagSerialMappingDTO</returns>
        public TagSerialMappingDTO GetTagSerialMappingDTO(int tagSerialMappingId)
        {
            log.LogMethodEntry(tagSerialMappingId);
            TagSerialMappingDTO returnValue = null;
            string query = SELECT_QUERY + " WHERE csm.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", tagSerialMappingId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetTagSerialMappingDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the TagSerialMappingDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TagSerialMappingDTO matching the search criteria</returns>
        public List<TagSerialMappingDTO> GetTagSerialMappingDTOList(List<KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<TagSerialMappingDTO> list = null;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TagSerialMappingDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TagSerialMappingDTO.SearchByParameters.TAG_SERIAL_MAPPING_ID
                            || searchParameter.Key == TagSerialMappingDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TagSerialMappingDTO.SearchByParameters.SERIAL_NUMBER ||
                                 searchParameter.Key == TagSerialMappingDTO.SearchByParameters.TAG_NUMBER)
                        {
                            query.Append(joiner + " " + DBSearchParameters[searchParameter.Key] + " =" + dataAccessHandler.GetParameterName(searchParameter.Key) );
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TagSerialMappingDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TagSerialMappingDTO.SearchByParameters.ALREADY_ISSUED)
                        {
                            query.Append(joiner + ((searchParameter.Value == "1" || searchParameter.Value == "Y")
                                                        ?  " EXISTS (SELECT top 1 1 FROM cards c where c.card_number = csm.cardNumber) "
                                                        :  " NOT EXISTS (SELECT top 1 1 FROM cards c where c.card_number = csm.cardNumber) "));
                        }
                        else if (searchParameter.Key == TagSerialMappingDTO.SearchByParameters.SERIAL_NUMBER_FROM)
                        {
                            query.Append(joiner + " " + DBSearchParameters[searchParameter.Key] + " >=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TagSerialMappingDTO.SearchByParameters.SERIAL_NUMBER_TO)
                        {
                            query.Append(joiner + " " + DBSearchParameters[searchParameter.Key] + "  <=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query + " order by csm.SerialNumber";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<TagSerialMappingDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TagSerialMappingDTO tagSerialMappingDTO = GetTagSerialMappingDTO(dataRow);
                    list.Add(tagSerialMappingDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

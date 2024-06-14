/********************************************************************************************
 * Project Name -Inventory Notes DataHandler
 * Description  -Data object of inventory Notes
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Aug-2016   Amaresh          Created
 *2.70.2        14-jul-2019   Deeksha          Modifications as per three tier standard
 *2.70.2        09-Dec-2019   Jinto Thomas     Removed siteid from update query 
 *2.120.0      07-Apr-2021   Mushahid Faizan   Web Inventory Changes
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Inventory Notes - Handles insert, update and select of inventory Notes objects
    /// </summary>
    public class InventoryNotesDataHandler
    {
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private List<SqlParameter> parameters = new List<SqlParameter>();
        private const string SELECT_QUERY = @"SELECT * FROM InventoryNotes AS inote ";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Dictionary for searching Parameters for the Inventory Notes object.
        /// </summary>
        private static readonly Dictionary<InventoryNotesDTO.SearchByInventoryNotesParameters, string> DBSearchParameters = new Dictionary<InventoryNotesDTO.SearchByInventoryNotesParameters, string>
            {
                {InventoryNotesDTO.SearchByInventoryNotesParameters.INVENTORY_NOTE_ID, "inote.InventoryNoteId"},
                {InventoryNotesDTO.SearchByInventoryNotesParameters.INVENTORY_NOTE_ID_LIST, "inote.InventoryNoteId"},
                {InventoryNotesDTO.SearchByInventoryNotesParameters.NOTE_TYPE_ID, "inote.NoteTypeId"},
                {InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_ID, "inote.ParafaitObjectId"},
                {InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_ID_LIST, "inote.ParafaitObjectId"},
                {InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_NAME,"inote.ParafaitObjectName"},
                {InventoryNotesDTO.SearchByInventoryNotesParameters.SITE_ID, "inote.site_id"},
                {InventoryNotesDTO.SearchByInventoryNotesParameters.MASTER_ENTITY_ID, "inote.MasterEntityId"}
            };

        /// <summary>
        /// Parameterized Constructor for InventoryNotesDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public InventoryNotesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryNotes Record.
        /// </summary>
        /// <param name="inventoryNotesDTO">InventoryNotesDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(InventoryNotesDTO inventoryNotesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryNotesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@inventoryNoteId", inventoryNotesDTO.InventoryNoteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@noteTypeId", inventoryNotesDTO.NoteTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parafaitObjectName", string.IsNullOrEmpty(inventoryNotesDTO.ParafaitObjectName) ? string.Empty : (object)inventoryNotesDTO.ParafaitObjectName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parafaitObjectId", inventoryNotesDTO.ParafaitObjectId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@notes", string.IsNullOrEmpty(inventoryNotesDTO.Notes) ? DBNull.Value : (object)inventoryNotesDTO.Notes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", inventoryNotesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the inventory Notes record to the database
        /// </summary>
        /// <param name="inventoryNotesDTO">InventoryNotesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public InventoryNotesDTO InsertInventoryNotes(InventoryNotesDTO inventoryNotesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryNotesDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[InventoryNotes]
                                                        (
                                                        NoteTypeId,
                                                        ParafaitObjectName,
                                                        ParafaitObjectId,
                                                        Notes,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdatedDate,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy
                                                        ) 
                                                values 
                                                        ( 
                                                         @noteTypeId,
                                                         @parafaitObjectName,
                                                         @parafaitObjectId,
                                                         @notes,
                                                         Getdate(),
                                                         @lastUpdatedBy,
                                                         Getdate(),
                                                         NEWID(),
                                                         @siteId,
                                                         @masterEntityId,
                                                         @createdBy) 
                                            SELECT * FROM InventoryNotes WHERE InventoryNoteId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryNotesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryNotesDTO(inventoryNotesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting InventoryNotesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryNotesDTO);
            return inventoryNotesDTO;
        }

        /// <summary>
        /// Updates the Inventory Notes record
        /// </summary>
        /// <param name="inventoryNotesDTO">InventoryNotesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public InventoryNotesDTO UpdateInventoryNotes(InventoryNotesDTO inventoryNotesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryNotesDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[InventoryNotes]
                                    SET         NoteTypeId=@noteTypeId,
                                                ParafaitObjectName=@parafaitObjectName,
                                                ParafaitObjectId=@parafaitObjectId,
                                                Notes=@notes,
                                                CreationDate= GetDate(),
                                                LastUpdatedBy=@lastUpdatedBy,
                                                LastupdatedDate = Getdate(),
                                                --site_id=@siteId,
                                                MasterEntityId=@masterEntityId
                                              WHERE InventoryNoteId =@inventoryNoteId 
                                    SELECT * FROM InventoryNotes WHERE InventoryNoteId = @inventoryNoteId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(inventoryNotesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryNotesDTO(inventoryNotesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating inventoryNotesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryNotesDTO);
            return inventoryNotesDTO;
        }

        /// <summary>
        /// Converts the Data row object to InventoryNotesDTO class type
        /// </summary>
        /// <param name="inventoryNotesDataRow">InventoryNotes DataRow</param>
        /// <returns>Returns inventoryNotes</returns>
        private InventoryNotesDTO GetInventoryNotesDTO(DataRow inventoryNotesDataRow)
        {
            log.LogMethodEntry(inventoryNotesDataRow);
            InventoryNotesDTO inventoryNotesDataObject = new InventoryNotesDTO(
                                            inventoryNotesDataRow["InventoryNoteId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryNotesDataRow["InventoryNoteId"]),
                                            inventoryNotesDataRow["NoteTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryNotesDataRow["NoteTypeId"]),
                                            inventoryNotesDataRow["ParafaitObjectName"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryNotesDataRow["ParafaitObjectName"]),
                                            inventoryNotesDataRow["ParafaitObjectId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryNotesDataRow["ParafaitObjectId"]),
                                            inventoryNotesDataRow["Notes"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryNotesDataRow["Notes"]),
                                            inventoryNotesDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryNotesDataRow["CreationDate"]),
                                            inventoryNotesDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryNotesDataRow["LastUpdatedBy"]),
                                            inventoryNotesDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryNotesDataRow["LastupdatedDate"]),
                                            inventoryNotesDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryNotesDataRow["Guid"]),
                                            inventoryNotesDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryNotesDataRow["site_id"]),
                                            inventoryNotesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(inventoryNotesDataRow["SynchStatus"]),
                                            inventoryNotesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryNotesDataRow["MasterEntityId"]),
                                            inventoryNotesDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryNotesDataRow["CreatedBy"])
                                            );
            log.LogMethodExit(inventoryNotesDataObject);
            return inventoryNotesDataObject;
        }

        /// <summary>
        /// Gets the Inventory Notes data of passed Id
        /// </summary>
        /// <param name="inventoryNoteId">Int type parameter</param>
        /// <returns>Returns InventoryNotesDTO</returns>
        public InventoryNotesDTO GetInventoryNotes(int inventoryNoteId)
        {
            log.LogMethodEntry(inventoryNoteId);
            InventoryNotesDTO result = null;
            string query = SELECT_QUERY + @" WHERE inote.InventoryNoteId= @inventoryNoteId";
            SqlParameter parameter = new SqlParameter("@inventoryNoteId", inventoryNoteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetInventoryNotesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Delete the record from the inventoryNote database based on inventoryNoteId
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int inventoryNoteId)
        {
            log.LogMethodEntry(inventoryNoteId);
            string query = @"DELETE  
                             FROM InventoryNotes
                             WHERE InventoryNotes.InventoryNoteId = @inventoryNoteId";
            SqlParameter parameter = new SqlParameter("@inventoryNoteId", inventoryNoteId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="inventoryNotesDTO">inventoryNotesDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshInventoryNotesDTO(InventoryNotesDTO inventoryNotesDTO, DataTable dt)
        {
            log.LogMethodEntry(inventoryNotesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                inventoryNotesDTO.InventoryNoteId = Convert.ToInt32(dt.Rows[0]["InventoryNoteId"]);
                inventoryNotesDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                inventoryNotesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                inventoryNotesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                inventoryNotesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                inventoryNotesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                inventoryNotesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        string GetLookuValueName(int lookupValueId)
        {
            string selectLookUpValueQuery = @"SELECT LookupValue FROM LookupValues WHERE LookupValueId = @lookupValueId";
            SqlParameter[] selectLookupValueParameters = new SqlParameter[1];
            selectLookupValueParameters[0] = new SqlParameter("@lookupValueId", lookupValueId);
            DataTable lookupName = dataAccessHandler.executeSelectQuery(selectLookUpValueQuery, selectLookupValueParameters, sqlTransaction);
            if (lookupName != null && lookupName.Rows.Count == 1)
            {
                log.LogMethodExit(lookupName.Rows[0]["LookupValue"].ToString());
                return lookupName.Rows[0]["LookupValue"].ToString();
            }
            else
            {
                log.LogMethodExit();
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the List of InventoryNotesDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Build the Query parameters.</returns>
        public string GetFilterQuery(List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == InventoryNotesDTO.SearchByInventoryNotesParameters.INVENTORY_NOTE_ID
                            || searchParameter.Key == InventoryNotesDTO.SearchByInventoryNotesParameters.NOTE_TYPE_ID
                            || searchParameter.Key == InventoryNotesDTO.SearchByInventoryNotesParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryNotesDTO.SearchByInventoryNotesParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == InventoryNotesDTO.SearchByInventoryNotesParameters.INVENTORY_NOTE_ID_LIST
                             || searchParameter.Key == InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
            }
            log.LogMethodExit(query);
            return query.ToString();
        }


        /// <summary>
        /// Gets the InventoryNotesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of InventoryNotesDTO matching the search criteria</returns>
        public List<InventoryNotesDTO> GetInventoryNotesList(List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>> searchParameters,
                                                             int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters, currentPage, pageSize);
            List<InventoryNotesDTO> inventoryNotesDTOList = new List<InventoryNotesDTO>();
            parameters.Clear();
            string selectInventoryNotesQuery = SELECT_QUERY;

            selectInventoryNotesQuery += GetFilterQuery(searchParameters);

            if (currentPage > 0 && pageSize > 0)
            {
                selectInventoryNotesQuery += " ORDER BY inote.InventoryNoteId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectInventoryNotesQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }

        DataTable dataTable = dataAccessHandler.executeSelectQuery(selectInventoryNotesQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryNotesDTOList = new List<InventoryNotesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryNotesDTO inventoryNotesDTO = GetInventoryNotesDTO(dataRow);
                    inventoryNotesDTOList.Add(inventoryNotesDTO);
                }
            }
            log.LogMethodExit(inventoryNotesDTOList);
            return inventoryNotesDTOList;
        }

        /// <summary>
        /// Returns the no of InventoryNotes matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of Inventory Notes matching the criteria</returns>
        public int GetInventoryNotesCount(List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                count = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(count);
            return count;
        }
    }
}

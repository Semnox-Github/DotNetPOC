/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data Handler - InventoryActivityLogDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70     03-Jun-2019   Girish Kundar           Created 
 *2.110.0  01-Jan-2021   Mushahid Faizan         Modified for Inventory UI Redesign changes 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the InventoryActivityLogDataHandler handles Insert, update and Search for the InventoryActivityLog  objects
    /// </summary>
    public class InventoryActivityLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        List<SqlParameter> parameters = new List<SqlParameter>();
        private const string SELECT_QUERY = @"SELECT * FROM InventoryActivityLog AS ial";
        /// <summary>
        /// Dictionary for searching Parameters for the InventoryActivityLog object.
        /// </summary>
        private static readonly Dictionary<InventoryActivityLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<InventoryActivityLogDTO.SearchByParameters, string>
        {
            { InventoryActivityLogDTO.SearchByParameters.INV_TABLE_KEY , "ial.InvTableKey"},
            { InventoryActivityLogDTO.SearchByParameters.MESSAGE,"ial.Message"},
            { InventoryActivityLogDTO.SearchByParameters.SOURCE_SYSTEM_ID,"ial.SourceSystemId"},
            { InventoryActivityLogDTO.SearchByParameters.SOURCE_TABLE_NAME,"ial.SourceTableName"},
            { InventoryActivityLogDTO.SearchByParameters.SITE_ID,"ial.site_id"},
            { InventoryActivityLogDTO.SearchByParameters.MASTER_ENTITY_ID,"ial.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for InventoryActivityLogDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public InventoryActivityLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryActivityLog Record.
        /// </summary>
        /// <param name="inventoryActivityLogDTO">inventoryActivityLogDTO object passed as the Parameter</param>
        /// <param name="loginId">login id of the user </param>
        /// <param name="siteId">site id of the user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(InventoryActivityLogDTO inventoryActivityLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryActivityLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@InvTableKey", inventoryActivityLogDTO.InvTableKey == -1 ? DBNull.Value : (object)inventoryActivityLogDTO.InvTableKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Message", inventoryActivityLogDTO.Message));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SourceSystemId", inventoryActivityLogDTO.SourceSystemId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SourceTableName", inventoryActivityLogDTO.SourceTableName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeStamp", inventoryActivityLogDTO.TimeStamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", inventoryActivityLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", inventoryActivityLogDTO.Guid));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to InventoryActivityLogDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the InventoryActivityLogDTO</returns>
        private InventoryActivityLogDTO GetInventoryActivityLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            InventoryActivityLogDTO inventoryActivityLogDTO = new InventoryActivityLogDTO(dataRow["TimeStamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["TimeStamp"]),
                                            dataRow["Message"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Message"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["SourceTableName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SourceTableName"]),
                                            dataRow["InvTableKey"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["InvTableKey"]),
                                            dataRow["SourceSystemId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SourceSystemId"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                          );
            log.LogMethodExit(inventoryActivityLogDTO);
            return inventoryActivityLogDTO;
        }

        /// <summary>
        ///  Inserts the record to the InventoryActivityLogDTO Table.
        /// </summary>
        /// <param name="inventoryActivityLogDTO">inventoryActivityLogDTO object passed as the Parameter</param>
        /// <param name="loginId">login id of the user </param>
        /// <param name="siteId">site id of the user</param>
        /// <returns> returns the InventoryActivityLogDTO</returns>
        public int Insert(InventoryActivityLogDTO inventoryActivityLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryActivityLogDTO, loginId, siteId);
            int rowInserted = 0;
            string query = @"INSERT INTO [dbo].[InventoryActivityLog]
                           (TimeStamp,
                            Message,
                            Guid,
                            site_id,
                            SourceTableName,
                            InvTableKey,
                            SourceSystemId,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (@TimeStamp,
                            @Message,
                            @Guid,
                            @site_id,
                            @SourceTableName,
                            @InvTableKey,
                            @SourceSystemId,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE())";

            try
            {
                rowInserted = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(inventoryActivityLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                //RefreshInventoryActivityLogDTO(inventoryActivityLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting InventoryActivityLog ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryActivityLogDTO);
            return rowInserted;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="inventoryActivityLogDTO">InventoryActivityLogDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        private void RefreshInventoryActivityLogDTO(InventoryActivityLogDTO inventoryActivityLogDTO, DataTable dt)
        {
            log.LogMethodEntry(inventoryActivityLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                inventoryActivityLogDTO.TimeStamp = Convert.ToDateTime(dt.Rows[0]["TimeStamp"]);
                inventoryActivityLogDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                inventoryActivityLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                inventoryActivityLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                inventoryActivityLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                inventoryActivityLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                inventoryActivityLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of InventoryActivityLogDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>returns the List of InventoryActivityLogDTO</returns>
        public List<InventoryActivityLogDTO> GetInventoryActivityLogDTOList(List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<InventoryActivityLogDTO> inventoryActivityLogDTOList = new List<InventoryActivityLogDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters);
            if (currentPage > 0 && pageSize > 0)
            {
                selectQuery += " Order by ial.TimeStamp desc OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    InventoryActivityLogDTO inventoryActivityLogDTO = GetInventoryActivityLogDTO(dataRow);
                    inventoryActivityLogDTOList.Add(inventoryActivityLogDTO);
                }
            }
            log.LogMethodExit(inventoryActivityLogDTOList);
            return inventoryActivityLogDTOList;
        }

        private string GetFilterQuery(List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            StringBuilder query = new StringBuilder("");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == InventoryActivityLogDTO.SearchByParameters.INV_TABLE_KEY ||
                            searchParameter.Key == InventoryActivityLogDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == InventoryActivityLogDTO.SearchByParameters.SOURCE_TABLE_NAME ||
                            searchParameter.Key == InventoryActivityLogDTO.SearchByParameters.SOURCE_SYSTEM_ID ||
                            searchParameter.Key == InventoryActivityLogDTO.SearchByParameters.MESSAGE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == InventoryActivityLogDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
            }
            log.LogMethodExit();
            return query.ToString();
        }

        /// <summary>
        /// Returns the no of Inventory Activities matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetInventoryActivityLogCount(List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int inventoryWastagesDTOCount = 0;
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                inventoryWastagesDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(inventoryWastagesDTOCount);
            return inventoryWastagesDTOCount;
        }
    }
}

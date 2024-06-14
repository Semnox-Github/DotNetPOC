/********************************************************************************************
* Project Name -InventoryPhysicalCount DataHandler
* Description  -Data object of InventoryPhysicalCount 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        6-Jan-2017    Amaresh          Created 
*2.80        18-Aug-2019   Deeksha          Modifications as per 3 tier standard.
*2.80        11-Dec-2019   Jinto Thomas     Removed siteid from update query
*2.110.0     04-Jan-2021   Mushahid Faizan  Modified : Web Inventory Changes
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;
using System.Globalization;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// class of InventoryPhysicalCountDataHandler
    /// </summary>
    public class InventoryPhysicalCountDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private List<SqlParameter> parameters = new List<SqlParameter>();
        private const string SELECT_QUERY = @"SELECT * FROM invphysicalcount AS ipc ";
        private static readonly Dictionary<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string> DBSearchParameters = new Dictionary<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>
        {
                {InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.PHYSICAL_COUNT_ID, "ipc.PhysicalCountID"},
                {InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.NAME, "ipc.name"},
                {InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.STATUS, "ipc.status"},
                {InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.START_DATE, "ipc.startDate"},
                {InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.END_DATE, "ipc.endDate"},
                {InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.SITE_ID, "ipc.site_id"},
                {InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.MASTER_ENTITY_ID, "ipc.MasterEntityId"},
                {InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.SCHEDULED_DATE, "ipc.ScheduledDate"},
                {InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.FREQUENCY, "ipc.Frequency"},
                {InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.LOCATIONID, "ipc.LocationID"}
        };
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of InventoryPhysicalCountDataHandler class
        /// </summary>
        public InventoryPhysicalCountDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating inventoryPhysicalCountdatahandler Record.
        /// </summary>
        /// <param name="InventoryPhysicalCountDTO">InventoryPhysicalCountDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(InventoryPhysicalCountDTO inventoryPhysicalCountDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryPhysicalCountDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PhysicalCountID", inventoryPhysicalCountDTO.PhysicalCountID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", string.IsNullOrEmpty(inventoryPhysicalCountDTO.Name) ? DBNull.Value : (object)inventoryPhysicalCountDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", string.IsNullOrEmpty(inventoryPhysicalCountDTO.Status) ? DBNull.Value : (object)inventoryPhysicalCountDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@initiatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LocationID", inventoryPhysicalCountDTO.LocationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@endDate", DBNull.Value));
            parameters.Add(dataAccessHandler.GetSQLParameter("@closedBy", DBNull.Value));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", inventoryPhysicalCountDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastupdatedUserid", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduledDate", inventoryPhysicalCountDTO.ScheduledDate.Equals(DateTime.MinValue) ? DBNull.Value : (object)inventoryPhysicalCountDTO.ScheduledDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@frequency", string.IsNullOrEmpty(inventoryPhysicalCountDTO.Frequency) ? DBNull.Value : (object)inventoryPhysicalCountDTO.Frequency));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the InventoryPhysicalCount record to the database
        /// </summary>
        /// <param name="inventoryPhysicalCountDTO">InventoryPhysicalCountDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">SQLTrx </param>
        /// <returns>Returns inserted record id</returns>
        public InventoryPhysicalCountDTO InsertInventoryPhysicalCount(InventoryPhysicalCountDTO inventoryPhysicalCountDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryPhysicalCountDTO, loginId, siteId);
            string insertInventoryPhysicalCountQuery = @"insert into invphysicalcount 
                                                        (                                                         
                                                         name,
                                                         status,
                                                         startDate,
                                                         endDate,
                                                         InitiatedBy,
                                                         ClosedBy,
                                                         site_id,
                                                         Guid,
                                                         MasterEntityId,
                                                         LocationID,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdatedBy,
                                                         LastUpdateDate,
                                                         ScheduledDate,
                                                         Frequency
                                                        ) 
                                                values 
                                                        (                                                        
                                                         @name,
                                                         @status,
                                                         Getdate(),
                                                         @endDate,
                                                         @initiatedBy,
                                                         @closedBy,
                                                         @siteId,
                                                         NewId(),
                                                         @masterEntityId,
                                                         @LocationID,
                                                         @createdBy,
                                                         getdate(),
                                                         @lastupdatedUserid,
                                                         getdate(),
                                                         @scheduledDate,
                                                         @frequency
                                                        ) SELECT * FROM invphysicalcount WHERE PhysicalCountID = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertInventoryPhysicalCountQuery, GetSQLParameters(inventoryPhysicalCountDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryPhysicalCountDTO(inventoryPhysicalCountDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting inventoryPhysicalCountDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(inventoryPhysicalCountDTO);
            return inventoryPhysicalCountDTO;
        }



        /// <summary>
        /// Updates the InventoryPhysicalCount record
        /// </summary>
        /// <param name="inventoryPhysicalCountDTO">InventoryPhysicalCountDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">SQLTrx </param>
        /// <returns>Returns the count of updated rows</returns>
        public InventoryPhysicalCountDTO UpdateInventoryPhysicalCount(InventoryPhysicalCountDTO inventoryPhysicalCountDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(inventoryPhysicalCountDTO, loginId, siteId);
            string updateInventoryPhysicalCountQuery = @"update invphysicalcount 
                                                    set  name = @name,
                                                         status =@status,
                                                         endDate =Getdate(),
                                                         ClosedBy =@closedBy,
                                                         -- site_id = @siteId,        
                                                         MasterEntityId =@MasterEntityId,
                                                         LastUpdatedBy =@lastupdatedUserid,
                                                         LocationID = @LocationID,
                                                         ScheduledDate = @scheduledDate,
                                                         Frequency = @frequency
                                                    where PhysicalCountID = @physicalCountID
                            SELECT * FROM invphysicalcount WHERE PhysicalCountID = @PhysicalCountID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateInventoryPhysicalCountQuery, GetSQLParameters(inventoryPhysicalCountDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInventoryPhysicalCountDTO(inventoryPhysicalCountDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating inventoryPhysicalCountDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(inventoryPhysicalCountDTO);
            return inventoryPhysicalCountDTO;
        }

        /// <summary>
        /// Converts the Data row object to InventoryPhysicalCountDTO class type
        /// </summary>
        /// <param name="inventoryPhysicalCountDataRow">InventoryPhysicalCountDTO DataRow</param>
        /// <returns>Returns InventoryPhysicalCountDTO</returns>
        private InventoryPhysicalCountDTO GetInventoryPhysicalCountDTO(DataRow inventoryPhysicalCountDataRow)
        {
            log.LogMethodEntry(inventoryPhysicalCountDataRow);
            InventoryPhysicalCountDTO inventoryPhysicalCountDataObject = new InventoryPhysicalCountDTO(Convert.ToInt32(inventoryPhysicalCountDataRow["PhysicalCountID"]),
                                                    inventoryPhysicalCountDataRow["name"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryPhysicalCountDataRow["name"]),
                                                    inventoryPhysicalCountDataRow["status"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryPhysicalCountDataRow["status"]),
                                                    inventoryPhysicalCountDataRow["startDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryPhysicalCountDataRow["startDate"]),
                                                    inventoryPhysicalCountDataRow["endDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryPhysicalCountDataRow["endDate"]),
                                                    inventoryPhysicalCountDataRow["InitiatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryPhysicalCountDataRow["InitiatedBy"]),
                                                    inventoryPhysicalCountDataRow["ClosedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryPhysicalCountDataRow["ClosedBy"]),
                                                    inventoryPhysicalCountDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryPhysicalCountDataRow["site_id"]),
                                                    inventoryPhysicalCountDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryPhysicalCountDataRow["Guid"]),
                                                    inventoryPhysicalCountDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(inventoryPhysicalCountDataRow["SynchStatus"]),
                                                    inventoryPhysicalCountDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryPhysicalCountDataRow["MasterEntityId"]),
                                                    inventoryPhysicalCountDataRow["LocationId"] == DBNull.Value ? -1 : Convert.ToInt32(inventoryPhysicalCountDataRow["LocationId"]),
                                                    inventoryPhysicalCountDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryPhysicalCountDataRow["CreatedBy"]),
                                                    inventoryPhysicalCountDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryPhysicalCountDataRow["CreationDate"]),
                                                    inventoryPhysicalCountDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryPhysicalCountDataRow["LastUpdatedBy"]),
                                                    inventoryPhysicalCountDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryPhysicalCountDataRow["LastUpdateDate"]),
                                                    inventoryPhysicalCountDataRow["ScheduledDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(inventoryPhysicalCountDataRow["ScheduledDate"]),
                                                    inventoryPhysicalCountDataRow["Frequency"] == DBNull.Value ? string.Empty : Convert.ToString(inventoryPhysicalCountDataRow["Frequency"])


                                                    );
            log.LogMethodExit(inventoryPhysicalCountDataObject);
            return inventoryPhysicalCountDataObject;
        }

        /// <summary>
        /// Delete the record from the PhysicalCount database based on PhysicalCountID
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int physicalCountID)
        {
            log.LogMethodEntry(physicalCountID);
            string query = @"DELETE  
                             FROM InvPhysicalCount
                             WHERE InvPhysicalCount.PhysicalCountID = @PhysicalCountID";
            SqlParameter parameter = new SqlParameter("@PhysicalCountID", physicalCountID);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="InventoryPhysicalCountDTO">InventoryPhysicalCountDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshInventoryPhysicalCountDTO(InventoryPhysicalCountDTO inventoryPhysicalCountDTO, DataTable dt)
        {
            log.LogMethodEntry(inventoryPhysicalCountDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                inventoryPhysicalCountDTO.PhysicalCountID = Convert.ToInt32(dt.Rows[0]["PhysicalCountID"]);
                inventoryPhysicalCountDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                inventoryPhysicalCountDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                inventoryPhysicalCountDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                inventoryPhysicalCountDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                inventoryPhysicalCountDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                inventoryPhysicalCountDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                inventoryPhysicalCountDTO.StartDate = dataRow["startDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["startDate"]);
                inventoryPhysicalCountDTO.InitiatedBy = dataRow["InitiatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["InitiatedBy"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the InventoryPhysicalCount data of passed physicalCountID
        /// </summary>
        /// <param name="physicalCountID">integer type parameter</param>
        /// <param name="SQLTrx">SQLTrx type parameter</param>
        /// <returns>Returns InventoryPhysicalCountDTO</returns>
        public InventoryPhysicalCountDTO GetInventoryPhysicalCount(int physicalCountID, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(physicalCountID, SQLTrx);
            string selectInventoryPhysicalCountQuery = @"select *
                                                            from invphysicalcount
                                                            where PhysicalCountID = @physicalCountID";
            SqlParameter[] selectInventoryPhysicalCountParameters = new SqlParameter[1];
            selectInventoryPhysicalCountParameters[0] = new SqlParameter("@physicalCountID", physicalCountID);
            DataTable inventoryPhysicalCountDt = dataAccessHandler.executeSelectQuery(selectInventoryPhysicalCountQuery, selectInventoryPhysicalCountParameters);
            if (inventoryPhysicalCountDt.Rows.Count > 0)
            {
                DataRow inventoryPhysicalCountRow = inventoryPhysicalCountDt.Rows[0];
                InventoryPhysicalCountDTO inventoryPhysicalCountDataObject = GetInventoryPhysicalCountDTO(inventoryPhysicalCountRow);
                log.LogMethodExit(inventoryPhysicalCountDataObject);
                return inventoryPhysicalCountDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the InventoryPhysicalCount data of passed physicalCountID
        /// </summary>
        /// <param name="SiteID">SiteID</param>
        /// <param name="SQLTrx">SQLTrx</param>
        /// <returns>Returns true if there is open physical count for all locations</returns>
        public bool AllLocationsPhysicalCountExists(int SiteID, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SiteID, SQLTrx);
            string selectInventoryPhysicalCountQuery = @"select *
                                                            from invphysicalcount
                                                            where locationid is null
                                                                and (site_id = @SiteID or @SiteID = -1)
                                                                and status = 'Open'";
            SqlParameter[] selectInventoryPhysicalCountParameters = new SqlParameter[1];
            selectInventoryPhysicalCountParameters[0] = new SqlParameter("@SiteID", SiteID);
            DataTable inventoryPhysicalCountDt = dataAccessHandler.executeSelectQuery(selectInventoryPhysicalCountQuery, selectInventoryPhysicalCountParameters);
            if (inventoryPhysicalCountDt.Rows.Count > 0)
            {
                DataRow inventoryPhysicalCountRow = inventoryPhysicalCountDt.Rows[0];
                InventoryPhysicalCountDTO inventoryPhysicalCountDataObject = GetInventoryPhysicalCountDTO(inventoryPhysicalCountRow);
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Gets the InventoryPhysicalCount data of passed physicalCountID
        /// </summary>
        /// <param name="LocationID">LocationID</param>
        /// <param name="SQLTrx">sql trx</param>
        /// <returns>Returns true if there is open physical count for all locations</returns>
        public bool OpenPhysicalCountExistsByLocation(int LocationID, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(LocationID, SQLTrx);
            string selectInventoryPhysicalCountQuery = @"select *
                                                            from invphysicalcount
                                                            where locationid = @LocationID
                                                                and (site_id = @SiteID or @SiteID = -1)
                                                                and status = 'Open'";
            SqlParameter[] selectInventoryPhysicalCountParameters = new SqlParameter[1];
            selectInventoryPhysicalCountParameters[0] = new SqlParameter("@LocationID", LocationID);
            DataTable inventoryPhysicalCountDt = dataAccessHandler.executeSelectQuery(selectInventoryPhysicalCountQuery, selectInventoryPhysicalCountParameters);
            if (inventoryPhysicalCountDt.Rows.Count > 0)
            {
                DataRow inventoryPhysicalCountRow = inventoryPhysicalCountDt.Rows[0];
                InventoryPhysicalCountDTO inventoryPhysicalCountDataObject = GetInventoryPhysicalCountDTO(inventoryPhysicalCountRow);
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Gets the InventoryPhysicalCountDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of InventoryPhysicalCountDTO matching the search criteria</returns>
        public List<InventoryPhysicalCountDTO> GetInventoryPhysicalCountList(List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters);
            List<InventoryPhysicalCountDTO> inventoryPhysicalCountList = null;
            parameters.Clear();
            string selectInventoryPhysicalCountQuery = SELECT_QUERY + GetFilterQuery(searchParameters);
            if (currentPage > 0 && pageSize > 0)
            {
                selectInventoryPhysicalCountQuery += " OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectInventoryPhysicalCountQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable inventoryPhysicalCountData = dataAccessHandler.executeSelectQuery(selectInventoryPhysicalCountQuery, parameters.ToArray(), sqlTransaction);
            if (inventoryPhysicalCountData.Rows.Count > 0)
            {
                inventoryPhysicalCountList = new List<InventoryPhysicalCountDTO>();
                foreach (DataRow inventoryPhysicalCountDataRow in inventoryPhysicalCountData.Rows)
                {
                    InventoryPhysicalCountDTO inventoryPhysicalCountDataObject = GetInventoryPhysicalCountDTO(inventoryPhysicalCountDataRow);
                    inventoryPhysicalCountList.Add(inventoryPhysicalCountDataObject);
                }
            }
            log.LogMethodExit(inventoryPhysicalCountList);
            return inventoryPhysicalCountList;

        }

        private string GetFilterQuery(List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            StringBuilder query = new StringBuilder("");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                query = new StringBuilder(" where ");
                foreach (KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.PHYSICAL_COUNT_ID) ||
                            searchParameter.Key.Equals(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.LOCATIONID) ||
                            searchParameter.Key.Equals(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.START_DATE))
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >=  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.SCHEDULED_DATE))
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.END_DATE))
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.NAME) ||
                            searchParameter.Key.Equals(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.FREQUENCY) ||
                                  searchParameter.Key.Equals(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.STATUS))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    count++;
                }
                if (searchParameters.Count > 0)
                    query.Append(" Order by ipc.PhysicalCountID ");
            }
            log.LogMethodExit();
            return query.ToString();
        }

        /// <summary>
        /// Returns the no of Inventory Physical Count matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetInventoryPhysicalCountsCount(List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int requisitionDTOCount = 0;
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                requisitionDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(requisitionDTOCount);
            return requisitionDTOCount;
        }
    }
}

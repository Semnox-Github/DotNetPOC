/********************************************************************************************
 * Project Name - FacilityTableDataHandler
 * Description  - Facility Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        14-May-2019  Mushahid Faizan          Added -  GetAllFacilityTableLayout(), GetAllFacilityTableDTO(), GetSQLParameters()
 *                                                         DeleteFacilityTables(), UpdateFacilityTables() and InsertFacilityTables() methods.
 *2.70        24-Jun-2019   Mathew Ninan            Added remaining fields from FacilityTables
 * 2.70.2        05-Dec-2019   Jinto Thomas            Removed siteid from update query
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Booking
{
    /// <summary>
    /// FacilityTableDataHandler class
    /// </summary>
    public class FacilityTableDataHandler
    {

        DataAccessHandler dataAccessHandler;
        private readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<FacilityTableDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<FacilityTableDTO.SearchByParameters, string>
        {
                {FacilityTableDTO.SearchByParameters.FACILITY_ID,"ft.FacilityId"},
                {FacilityTableDTO.SearchByParameters.SITE_ID, "ft.site_id"},
                { FacilityTableDTO.SearchByParameters.ISACTIVE, "ft.Active"}
        };
        string connstring;

        /// <summary>
        /// Default constructor of  FacilityTableDataHandler class
        /// </summary>
        public FacilityTableDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            connstring = dataAccessHandler.ConnectionString;
            this.sqlTransaction = sqlTransaction;            
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Facility data of passed facility Id
        /// </summary>
        /// <param name="facilityId">integer type parameter</param>
        /// <returns>Returns FacilityDTO</returns>
        public FacilityTableDTO GetFacilityTableDTO(int tableId)
        {
            log.LogMethodEntry(tableId);
            string selectCheckInFacilityQuery = "SELECT ft.*, null trxId FROM FacilityTables ft WHERE TableId = @TableId";
            SqlParameter[] selectFacilityTableParameters = new SqlParameter[1];
            selectFacilityTableParameters[0] = new SqlParameter("@TableId", tableId);
            DataTable facilityTable = dataAccessHandler.executeSelectQuery(selectCheckInFacilityQuery, selectFacilityTableParameters, sqlTransaction);
            FacilityTableDTO facilityTableObject = new FacilityTableDTO();
            if (facilityTable.Rows.Count > 0)
            {
                DataRow FacilityTableRow = facilityTable.Rows[0];
                facilityTableObject = GetFacilityTableDTO(FacilityTableRow);
            }
            log.LogMethodExit(facilityTableObject);
            return facilityTableObject;
        }

        /// <summary>
        /// GetTableStatus method returns list of facility tables based on matching facilityTableParams details
        /// </summary>
        /// 
        public List<FacilityTableDTO> GetTableStatus(FacilityTableParams facilityTableParams)
        {
            log.LogMethodEntry(facilityTableParams);
            List<FacilityTableDTO> facilityTablesList = new List<FacilityTableDTO>();
            try
            {
                string getTablesQuery = @"select X.RowIndex,
		                                        X.ColumnIndex, 
		                                        isnull(X.trxId, -1) TrxId, 
		                                        X.TableName,
                                                X.TableId,
		                                        CASE 
	                                                WHEN X.trxId IS NULL
	                                                THEN 'Vacant'
	                                                ELSE 'Occupied'
	                                            END TableStatus, 
		                                        isnull(OH.OrderId, -1) OrderId,
	                                            isnull(OH.CustomerName, '') CustomerName,
	                                            isnull(OH.Remarks, '') Remarks,
                                                isnull(TXH.user_id, -1) UserId 
		                                        from (select RowIndex, ColumnIndex, TableName, ft.TableId, 
                                                        (select top 1 th.trxid 
                                                        from OrderHeader oh, Trx_header th 
                                                        where oh.TableId = ft.TableId
                                                        and th.OrderId = oh.OrderId
                                                        and th.Status = 'OPEN'
                                                        and (@enableOrderShareAcrossPOSCounters = 1 or isnull(th.POSTypeId, -1) = @POSTypeId)
                                                        and (@enableOrderShareAcrossUsers = 1 or th.user_id = @userId)
                                                        and (@enableOrderShareAcrossPOS = 1 or (th.POSMachineId = @POSMachineId or th.POS_Machine = @POSMachineName))
                                                        ) trxId,
                                                    (select top 1 ci.CheckInTime
                                                        from CheckIns ci, CheckInDetails cd 
                                                        where ci.CheckInId = cd.CheckInId
	                                                    and ci.TableId = ft.TableId
                                                        and (cd.CheckOutTime is null or cd.CheckOutTime > getdate())) CheckInTime,
                                                    (select top 1 ci.AllowedTimeInMinutes
                                                        from CheckIns ci, CheckInDetails cd 
                                                        where ci.CheckInId = cd.CheckInId
	                                                    and ci.TableId = ft.TableId
                                                        and (cd.CheckOutTime is null or cd.CheckOutTime > getdate())) AllowedTimeInMinutes,
                                                    (select top 1 cd.CheckOutTime
                                                        from CheckIns ci, CheckInDetails cd 
                                                        where ci.CheckInId = cd.CheckInId
	                                                    and ci.TableId = ft.TableId
                                                        and (cd.CheckOutTime is null or cd.CheckOutTime > getdate())) CheckOutTime
                                                from FacilityTables ft 
                                                    where active = 'Y' 
                                                and FacilityId = @facilityId
                                                and (exists (select 1 
                                                                from FacilityPOSAssignment fpa 
                                                                where fpa.FacilityId = ft.FacilityId
                                                                and fpa.POSMachineId = @POSMachineId))
		                                        ) as X
		                                        LEFT OUTER JOIN TRX_HEADER TXH ON TXH.TrxId = X.trxId
		                                        LEFT OUTER JOIN OrderHeader OH on OH.OrderId = TXH.OrderId and OH.OrderStatus='OPEN'";

                List<SqlParameter> queryParameters = new List<SqlParameter>();
                queryParameters.Add(new SqlParameter("@facilityId", facilityTableParams.FacilityId));
                queryParameters.Add(new SqlParameter("@enableOrderShareAcrossPOSCounters", facilityTableParams.EnableOrderShareAcrossPosCounters));
                queryParameters.Add(new SqlParameter("@enableOrderShareAcrossUsers", facilityTableParams.EnableOrderShareAcrossUsers));
                queryParameters.Add(new SqlParameter("@enableOrderShareAcrossPOS", facilityTableParams.EnableOrderShareAcrossPOS));
                queryParameters.Add(new SqlParameter("@POSTypeId", facilityTableParams.POSMachine.POSTypeId));
                queryParameters.Add(new SqlParameter("@userId", facilityTableParams.UserDTO.UserId));
                queryParameters.Add(new SqlParameter("@posMachineId", facilityTableParams.POSMachine.POSMachineId));
                queryParameters.Add(new SqlParameter("@POSMachineName", facilityTableParams.POSMachine.POSName));

                DataTable dtTables = dataAccessHandler.executeSelectQuery(getTablesQuery, queryParameters.ToArray(), sqlTransaction);
                if (dtTables.Rows.Count > 0)
                {
                    FacilityTableDTO facilityTable = null;
                    foreach (DataRow tableDataRow in dtTables.Rows)
                    {
                        facilityTable = new FacilityTableDTO(
                                        Convert.ToInt32(tableDataRow["TableId"]),
                                        tableDataRow["TableName"].ToString(),
                                        Convert.ToInt32(tableDataRow["RowIndex"]),
                                        Convert.ToInt32(tableDataRow["ColumnIndex"]),
                                        tableDataRow["TableStatus"].ToString(),
                                        Convert.ToInt32(tableDataRow["OrderId"]),
                                        tableDataRow["CustomerName"].ToString(),
                                        Convert.ToInt32(tableDataRow["TrxId"]),
                                        tableDataRow["Remarks"].ToString(),
                                        Convert.ToInt32(tableDataRow["UserId"]));

                        facilityTablesList.Add(facilityTable);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit();
                throw new Exception("Error at GetTableStatus() -> " + ex.Message);
            }
            log.LogMethodExit(facilityTablesList);
            return facilityTablesList;
        }

        /// <summary>
        /// Converts the Data row object toFacilityTableDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>ReturnsFacilityTableDTO</returns>
        private FacilityTableDTO GetFacilityTableDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            FacilityTableDTO facilityTableDTO = new FacilityTableDTO(Convert.ToInt32(dataRow["TableId"]),
                                             dataRow["TableName"] == DBNull.Value ? "" : dataRow["TableName"].ToString(),
                                             dataRow["RowIndex"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RowIndex"]),
                                             dataRow["ColumnIndex"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ColumnIndex"]),
                                             dataRow["trxId"] == DBNull.Value ? FacilityTableDTO.VACANCT: FacilityTableDTO.OCCUPIED,
                                             -1,
                                             "",
                                             dataRow["trxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["trxId"]),
                                             dataRow["Remarks"] == DBNull.Value ? "" : dataRow["Remarks"].ToString(),
                                             -1,
                                             dataRow["facilityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["facilityId"])
                                             , dataRow["TableType"] == DBNull.Value ? "" : dataRow["TableType"].ToString()
                                             , dataRow["InterfaceInfo1"] == DBNull.Value ? "" : dataRow["InterfaceInfo1"].ToString()
                                             , dataRow["InterfaceInfo2"] == DBNull.Value ? "" : dataRow["InterfaceInfo2"].ToString()
                                             , dataRow["InterfaceInfo3"] == DBNull.Value ? "" : dataRow["InterfaceInfo3"].ToString()
                                             );
            log.LogMethodExit(facilityTableDTO);
            return facilityTableDTO;
        }

        public List<FacilityTableDTO> GetOpenOrderFacilityTableDTOList(List<KeyValuePair<FacilityTableDTO.SearchByParameters, string>> searchParameters,
                                                                      int POSTypeId,
                                                                      int userId,
                                                                      int POSMachineId,
                                                                      string POSMachineName,
                                                                      bool enableOrderShareAcrossPOSCounters,
                                                                      bool enableOrderShareAcrossUsers,
                                                                      bool enableOrderShareAcrossPOS)
        {
            log.LogMethodEntry();
            List<FacilityTableDTO> list = new List<FacilityTableDTO>();
            string selectQuery = @"select ft.*, (select top 1 th.TrxId from OrderHeader oh, Trx_header th 
                                                where TableId = ft.TableId
                                                and th.OrderId = oh.OrderId
                                                and th.Status = 'OPEN'
                                                and (@enableOrderShareAcrossPOSCounters = 1 or isnull(th.POSTypeId, -1) = @POSTypeId)
                                                and (@enableOrderShareAcrossUsers = 1 or th.user_id = @userId)
                                                and (@enableOrderShareAcrossPOS = 1 or (th.POSMachineId = @posMachineId or th.POS_Machine = @POSMachineName))
                                                ) trxId                                                                               
                                    from FacilityTables ft 
                                    where active = 'Y' and FacilityId in (select FacilityId from FacilityPOSAssignment where POSMachineId=@posMachineId)
                                    order by ft.TableName";
            selectQuery = selectQuery + GetWhereClause(searchParameters);
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@POSTypeId", POSTypeId),
                                                            new SqlParameter("@userId", userId),
                                                            new SqlParameter("@POSMachineId", POSMachineId),
                                                            new SqlParameter("@POSMachineName", POSMachineName),
                                                            new SqlParameter("@enableOrderShareAcrossPOSCounters", enableOrderShareAcrossPOSCounters),
                                                            new SqlParameter("@enableOrderShareAcrossUsers", enableOrderShareAcrossUsers),
                                                            new SqlParameter("@enableOrderShareAcrossPOS", enableOrderShareAcrossPOS)};
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<FacilityTableDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    FacilityTableDTO facilityTableDTO = GetFacilityTableDTO(dataRow);
                    list.Add(facilityTableDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        private string GetWhereClause(List<KeyValuePair<FacilityTableDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string whereClause = string.Empty;
            if (searchParameters == null || searchParameters.Count == 0)
            {
                log.LogMethodExit(string.Empty, "search parameters is empty");
                return whereClause;
            }
            string joiner = "";
            StringBuilder query = new StringBuilder(" where ");
            foreach (KeyValuePair<FacilityTableDTO.SearchByParameters, string> searchParameter in searchParameters)
            {
                if (DBSearchParameters.ContainsKey(searchParameter.Key))
                {
                    joiner = (count == 0) ? " " : " and ";
                    {
                        if (searchParameter.Key.Equals(FacilityTableDTO.SearchByParameters.FACILITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                        }
                        else if (searchParameter.Key == FacilityTableDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == FacilityTableDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + "'" + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N") + "'");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%" + searchParameter.Value + "%'");
                        }
                    }
                    count++;
                }
                else
                {
                    log.Error("Ends-GetAllOrderHeaderList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                    log.LogMethodExit("Throwing exception- The query parameter does not exist ");
                    throw new Exception("The query parameter does not exist");
                }
            }
            whereClause = query.ToString();
            log.LogMethodExit(whereClause);
            return whereClause;
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Facility Table Record.
        /// </summary>
        /// <param name="facilityTableDTO">facilityTableDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(FacilityTableDTO facilityTableDTO, string userId, int siteId)
        {
            log.LogMethodEntry(facilityTableDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@tableId", facilityTableDTO.TableId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tableName", facilityTableDTO.TableName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@rowIndex", facilityTableDTO.RowIndex));
            parameters.Add(dataAccessHandler.GetSQLParameter("@columnIndex", facilityTableDTO.ColumnIndex));
            parameters.Add(dataAccessHandler.GetSQLParameter("@facilityId", facilityTableDTO.FacilityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tableType", facilityTableDTO.TableType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@interfaceInfo1", facilityTableDTO.InterfaceInfo1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@interfaceInfo2", facilityTableDTO.InterfaceInfo2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@interfaceInfo3", facilityTableDTO.InterfaceInfo3));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", (facilityTableDTO.Active == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", facilityTableDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maxCheckIns", facilityTableDTO.MaxCheckIns));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", facilityTableDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Inserts the Facility Tables record to the database
        /// </summary>
        /// <param name="facilityTableDTO">facilityTableDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertFacilityTables(FacilityTableDTO facilityTableDTO, string userId, int siteId)
        {
            log.LogMethodEntry(facilityTableDTO, userId, siteId);
            int idOfRowInserted;
            string insertFacilityTableQuery = @"insert into FacilityTables 
                                                                    (TableName,
                                                                     TableType,
                                                                     RowIndex,
                                                                     ColumnIndex,
                                                                     FacilityId,
                                                                     InterfaceInfo1,
                                                                     InterfaceInfo2, 
                                                                     InterfaceInfo3,
                                                                     Active, 
                                                                     Remarks,
                                                                     MaxCheckIns,
                                                                      site_id,
                                                                      Guid,                                                      
                                                                    MasterEntityId,
                                                                    CreatedBy,
                                                                    CreationDate,
                                                                    LastUpdatedBy,
                                                                    LastUpdateDate   )
                                                            values  
                                                                    (@tableName,
                                                                     @tableType,
                                                                     @rowIndex,
                                                                     @columnIndex,
                                                                     @facilityId,
                                                                     @interfaceInfo1,
                                                                     @interfaceInfo2,
                                                                     @interfaceInfo3,
                                                                     @active,
                                                                     @remarks,
                                                                     @maxCheckIns,
                                                                     @siteId,
                                                                      NewId(),
                                                                     @masterEntityId,
                                                                     @createdBy,
                                                                     GETDATE(),                                                        
                                                                     @lastUpdatedBy,                                                        
                                                                     GetDate()
                                            )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(insertFacilityTableQuery, GetSQLParameters(facilityTableDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the Facility Table record
        /// </summary>
        /// <param name="facilityTableDTO">facilityTableDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateFacilityTables(FacilityTableDTO facilityTableDTO, string userId, int siteId)
        {
            log.LogMethodEntry(facilityTableDTO, userId, siteId);
            int rowsUpdated;
            string updateFacilityTableQuery = @"update FacilityTables 
                                                                    set TableName = @tableName, TableType = @tableType, RowIndex = @rowIndex, ColumnIndex = @columnIndex, 
                                                                        InterfaceInfo1 = @interfaceInfo1, InterfaceInfo2 = @interfaceInfo2, InterfaceInfo3 = @interfaceInfo3, 
                                                                        Active = @active, Remarks = @remarks, MaxCheckIns = @maxCheckIns,   --site_id = @siteId,
                                                                        MasterEntityId = @masterEntityId,LastUpdatedBy = @lastUpdatedBy,LastUpdateDate = GETDATE()
                                                                        where TableId = @tableId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(updateFacilityTableQuery, GetSQLParameters(facilityTableDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Deletes the Facility Tables record of passed Table Id
        /// </summary>
        /// <param name="tableId">integer type parameter</param>
        public void DeleteFacilityTables(int tableId)
        {
            log.LogMethodEntry(tableId);
            string query = @"delete from FacilityTables where TableId = @tableId";
            SqlParameter parameter = new SqlParameter("@tableId", tableId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to FacilityTableDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>ReturnsFacilityTableDTO</returns>
        private FacilityTableDTO GetAllFacilityTableDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            FacilityTableDTO facilityTableDTO = new FacilityTableDTO(Convert.ToInt32(dataRow["TableId"]),
                                             dataRow["TableName"] == DBNull.Value ? "" : dataRow["TableName"].ToString(),
                                             dataRow["RowIndex"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RowIndex"]),
                                             dataRow["ColumnIndex"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ColumnIndex"]),                                             
                                             dataRow["FacilityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["FacilityId"]),
                                             dataRow["TableType"] == DBNull.Value ? "" : dataRow["TableType"].ToString(),                                             
                                             dataRow["InterfaceInfo1"] == DBNull.Value ? "" : dataRow["InterfaceInfo1"].ToString(),
                                             dataRow["InterfaceInfo2"] == DBNull.Value ? "" : dataRow["InterfaceInfo2"].ToString(),
                                             dataRow["InterfaceInfo3"] == DBNull.Value ? "" : dataRow["InterfaceInfo3"].ToString(),
                                             dataRow["Active"] == DBNull.Value ? true : (dataRow["Active"].ToString() == "Y" ? true : false),
                                             dataRow["Remarks"] == DBNull.Value ? "" : dataRow["Remarks"].ToString(),
                                             dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                             dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                             dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                             dataRow["MaxCheckIns"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MaxCheckIns"]),
                                             dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]));

            log.LogMethodExit(facilityTableDTO);
            return facilityTableDTO;
        }
        public List<FacilityTableDTO> GetAllFacilityTableLayout(List<KeyValuePair<FacilityTableDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = @"select * from FacilityTables ft";
            if (searchParameters != null || searchParameters.Count != 0)
            {

                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<FacilityTableDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        {
                            if (searchParameter.Key.Equals(FacilityTableDTO.SearchByParameters.FACILITY_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key == FacilityTableDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key == FacilityTableDTO.SearchByParameters.ISACTIVE)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + "'" + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N") + "'");
                            }
                            else
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%" + searchParameter.Value + "%'");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.Error("Ends-GetAllFacilityTableLayout(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        log.LogMethodExit("Throwing exception- The query parameter does not exist ");
                        throw new Exception("The query parameter does not exist");
                    }
                }
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }

            DataTable dt = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dt.Rows.Count > 0)
            {
                List<FacilityTableDTO> facilityTableDTOList = new List<FacilityTableDTO>();
                foreach (DataRow facilityDataRow in dt.Rows)
                {
                    FacilityTableDTO facilityDTOList = GetAllFacilityTableDTO(facilityDataRow);
                    facilityTableDTOList.Add(facilityDTOList);
                }
                log.LogMethodExit(facilityTableDTOList);
                return facilityTableDTOList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
    }
}

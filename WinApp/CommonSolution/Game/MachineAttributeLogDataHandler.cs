/********************************************************************************************
 * Project Name - Games                                                                         
 * Description  - MachineAttributeLogDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.100       05-Sep-2020   Girish Kundar        Created
 *2.130.0     06-Aug-2021   Abhishek             Modified for new field UpdateType
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    public class MachineAttributeLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MachineAttributeUpdateLog AS maul ";
        /// <summary>
        /// Dictionary for searching Parameters for the MachineGroupMachines object.
        /// </summary>
        private static readonly Dictionary<MachineAttributeLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MachineAttributeLogDTO.SearchByParameters, string>
               {
                    {MachineAttributeLogDTO.SearchByParameters.ID, "maul.MachineGroupId"},
                    {MachineAttributeLogDTO.SearchByParameters.POS_MACHINEID, "maul.MachineGroupId"},
                    {MachineAttributeLogDTO.SearchByParameters.MACHINE_ID, "maul.MachineId"},
                    {MachineAttributeLogDTO.SearchByParameters.SITE_ID,"maul.site_id"},
                    {MachineAttributeLogDTO.SearchByParameters.POS_NAME,"maul.POSMachineName"},
                    {MachineAttributeLogDTO.SearchByParameters.UPDATE_TYPE,"maul.UpdateType"},
                    {MachineAttributeLogDTO.SearchByParameters.STATUS,"maul.Status"},
                    {MachineAttributeLogDTO.SearchByParameters.MASTER_ENTITY_ID,"maul.MasterEntityId"}
               };
        private readonly SqlTransaction sqlTransaction;

        /// <summary>
        /// Default constructor of MachineAttributeLogDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public MachineAttributeLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MachineAttributeLogDTO Record.
        /// </summary>
        /// <param name="machineAttributeLogDTO">MachineAttributeLogDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MachineAttributeLogDTO machineAttributeLogDTO, int userPkId, int siteId)
        {
            log.LogMethodEntry(machineAttributeLogDTO, userPkId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", machineAttributeLogDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineId", machineAttributeLogDTO.MachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posmachineId", machineAttributeLogDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posmachineName", machineAttributeLogDTO.POSMachineName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@message", machineAttributeLogDTO.Message));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", machineAttributeLogDTO.UserRemarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reasons", machineAttributeLogDTO.UserReason));
            parameters.Add(dataAccessHandler.GetSQLParameter("@updateType", machineAttributeLogDTO.UpdateType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", machineAttributeLogDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@timestamp", machineAttributeLogDTO.Timestamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", machineAttributeLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userPkId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userPkId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the MachineGroupMachines record to the database
        /// </summary>
        /// <param name="machineAttributeLogDTO">machineAttributeLogDTO type object</param>
        /// <param name="userPkId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public MachineAttributeLogDTO Insert(MachineAttributeLogDTO machineAttributeLogDTO, int userPkId, int siteId)
        {
            log.LogMethodEntry(machineAttributeLogDTO, userPkId, siteId);
            string query = @"insert into MachineAttributeUpdateLog 
                                                        (
                                                        MachineId,
                                                        POSMachineId,
                                                        POSMachineName,
                                                        Message,
                                                        UserReason,
                                                        UserRemarks,
                                                        Status,
                                                        TimeStamp,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        UpdateType
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @machineId,
                                                        @posmachineId,
                                                        @posmachineName,
                                                        @message,
                                                        @reasons,
                                                        @remarks,
                                                        @status,
                                                        getDate(),
                                                        NewId(),
                                                        @site_id,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        @updateType
                                                        )  SELECT * FROM MachineAttributeUpdateLog WHERE id = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(machineAttributeLogDTO, userPkId, siteId).ToArray(), sqlTransaction);
                RefreshMachineAttributeLogDTO(machineAttributeLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting machineAttributeLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(machineAttributeLogDTO);
            return machineAttributeLogDTO;
        }

        public MachineAttributeLogDTO Update(MachineAttributeLogDTO machineAttributeLogDTO, int userPkId, int siteId)
        {
            log.LogMethodEntry(machineAttributeLogDTO, userPkId, siteId);
            string updateMachineGroupsQuery = @"update MachineAttributeUpdateLog 
                                         set MachineId        = @machineId,
                                             POSMachineId     =@posmachineId,
                                             POSMachineName   =@posmachineName,
                                             Message        =@message,
                                             UserReason       =@reasons,
                                             UserRemarks      =@remarks,
                                             Status          =@status,
                                             LastUpdateDate = getDate(),
                                             LastUpdatedBy = @lastUpdatedBy
                                       where Id = @id
                         SELECT * FROM MachineAttributeUpdateLog WHERE Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMachineGroupsQuery, GetSQLParameters(machineAttributeLogDTO, userPkId, siteId).ToArray(), sqlTransaction);
                RefreshMachineAttributeLogDTO(machineAttributeLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating machineAttributeLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(machineAttributeLogDTO);
            return machineAttributeLogDTO;
        }

        private void RefreshMachineAttributeLogDTO(MachineAttributeLogDTO machineAttributeLogDTO, DataTable dt)
        {
            log.LogMethodEntry(machineAttributeLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                machineAttributeLogDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                machineAttributeLogDTO.LastupdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                machineAttributeLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                machineAttributeLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                machineAttributeLogDTO.LastupdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                machineAttributeLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                machineAttributeLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to MachineGroupMachines class type
        /// </summary>
        /// <param name="machineAttributeLogDataRow">machineAttributeLogDTO DataRow</param>
        /// <returns>Returns machineAttributeLogDTO</returns>
        private MachineAttributeLogDTO GetMachineAttributeLogDTO(DataRow machineAttributeLogDataRow)
        {
            log.LogMethodEntry(machineAttributeLogDataRow);
            MachineAttributeLogDTO machineAttributeLogDTO = new MachineAttributeLogDTO(Convert.ToInt32(machineAttributeLogDataRow["Id"]),
                                            machineAttributeLogDataRow["MachineId"] == DBNull.Value ? -1 : Convert.ToInt32(machineAttributeLogDataRow["MachineId"]),
                                            machineAttributeLogDataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(machineAttributeLogDataRow["POSMachineId"]),
                                            machineAttributeLogDataRow["POSMachineName"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeLogDataRow["POSMachineName"]),
                                            machineAttributeLogDataRow["Message"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeLogDataRow["Message"]),
                                            machineAttributeLogDataRow["UserReason"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeLogDataRow["UserReason"]),
                                            machineAttributeLogDataRow["UserRemarks"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeLogDataRow["UserRemarks"]),
                                            machineAttributeLogDataRow["Status"] == DBNull.Value ? true : Convert.ToBoolean(machineAttributeLogDataRow["Status"]),
                                            machineAttributeLogDataRow["TimeStamp"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(machineAttributeLogDataRow["TimeStamp"]),
                                            machineAttributeLogDataRow["UpdateType"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeLogDataRow["UpdateType"]),
                                            machineAttributeLogDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeLogDataRow["Guid"]),
                                            machineAttributeLogDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(machineAttributeLogDataRow["site_id"]),
                                            machineAttributeLogDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(machineAttributeLogDataRow["SynchStatus"]),
                                            machineAttributeLogDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(machineAttributeLogDataRow["MasterEntityId"]),
                                            machineAttributeLogDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeLogDataRow["CreatedBy"]),
                                            machineAttributeLogDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineAttributeLogDataRow["CreationDate"]),
                                            machineAttributeLogDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(machineAttributeLogDataRow["LastUpdatedBy"]),
                                            machineAttributeLogDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineAttributeLogDataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(machineAttributeLogDTO);
            return machineAttributeLogDTO;
        }

        /// <summary>
        /// Gets the MachineGroupMachines data of passed machineGroupMachineId id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns machineAttributeLogDTO</returns>
        public MachineAttributeLogDTO GetMachineAttributeLogDTO(int id)
        {
            log.LogMethodEntry(id);
            MachineAttributeLogDTO result = null;
            string selectMachineGroupMachinesQuery = SELECT_QUERY + @" WHERE maul.Id= @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectMachineGroupMachinesQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMachineAttributeLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        public List<MachineAttributeLogDTO> GetMachineAttributeLogs(List<KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<MachineAttributeLogDTO> machineAttributeLogDTOist = null;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(MachineAttributeLogDTO.SearchByParameters.ID) ||
                            searchParameter.Key.Equals(MachineAttributeLogDTO.SearchByParameters.MACHINE_ID) ||
                            searchParameter.Key.Equals(MachineAttributeLogDTO.SearchByParameters.POS_MACHINEID) ||
                            searchParameter.Key.Equals(MachineAttributeLogDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                       
                        else if (searchParameter.Key.Equals(MachineAttributeLogDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MachineAttributeLogDTO.SearchByParameters.POS_NAME))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MachineAttributeLogDTO.SearchByParameters.STATUS)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                    count++;
                }
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }

            DataTable machineGroupMachinesData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (machineGroupMachinesData.Rows.Count > 0)
            {
                machineAttributeLogDTOist = new List<MachineAttributeLogDTO>();
                foreach (DataRow machineGroupMachinesDataRow in machineGroupMachinesData.Rows)
                {
                    MachineAttributeLogDTO dto = GetMachineAttributeLogDTO(machineGroupMachinesDataRow);
                    machineAttributeLogDTOist.Add(dto);
                }
            }
            log.LogMethodExit(machineAttributeLogDTOist);
            return machineAttributeLogDTOist;
        }
    }
}

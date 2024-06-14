/*******************************************************************************************
 * Project Name - MachineGroups Data Handler
 * Description  - Data handler of the MachineGroups handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        13-May-2019   Jagan Mohana Rao        Created 
 *2.70.2        30-Jul-2019   Deeksha                 Modified :Insert/Update function returns DTO.Added RefreshmachineGroupMachines().
 *                                                            SQL injection issue Fix.
 *2.90        03-Jun-2020    Mushahid Faizan          Modified: 3 tier changes for Rest API.
  *2.110.0      21-Dec-2020   Prajwal S                Modified. 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Datahandler for MachineGroupMachines
    /// </summary>
    public class MachineGroupMachinesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MachineGroupMachines AS mg ";
        /// <summary>
        /// Dictionary for searching Parameters for the MachineGroupMachines object.
        /// </summary>
        private static readonly Dictionary<MachineGroupMachinesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MachineGroupMachinesDTO.SearchByParameters, string>
               {
                    {MachineGroupMachinesDTO.SearchByParameters.MACHINE_GROUP_ID, "mg.MachineGroupId"},
                    {MachineGroupMachinesDTO.SearchByParameters.MACHINE_GROUP_ID_LIST, "mg.MachineGroupId"},
                    {MachineGroupMachinesDTO.SearchByParameters.MACHINE_ID, "mg.MachineId"},
                    {MachineGroupMachinesDTO.SearchByParameters.SITE_ID,"mg.site_id"},
                    {MachineGroupMachinesDTO.SearchByParameters.ISACTIVE,"mg.IsActive"},
                    {MachineGroupMachinesDTO.SearchByParameters.ID,"mg.Id"},  //added
                    {MachineGroupMachinesDTO.SearchByParameters.MASTER_ENTITY_ID,"mg.MasterEntityId"}
               };
        private readonly SqlTransaction sqlTransaction;

        /// <summary>
        /// Default constructor of MachineGroupMachinesDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public MachineGroupMachinesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MachineGroups Record.
        /// </summary>
        /// <param name="machineGroupMachinesDTO">MachineGroupMachinesDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MachineGroupMachinesDTO machineGroupMachinesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(machineGroupMachinesDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", machineGroupMachinesDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineGroupId", machineGroupMachinesDTO.MachineGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineId", machineGroupMachinesDTO.MachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", machineGroupMachinesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", machineGroupMachinesDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the MachineGroupMachines record to the database
        /// </summary>
        /// <param name="machineGroupMachinesDTO">MachineGroupMachinesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public MachineGroupMachinesDTO InsertMachineGroupMachines(MachineGroupMachinesDTO machineGroupMachinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineGroupMachinesDTO, loginId, siteId);
            string insertMachineGroupMachinesQuery = @"insert into MachineGroupMachines 
                                                        (
                                                        MachineGroupId,
                                                        MachineId,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        IsActive
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @machineGroupId,
                                                        @machineId,
                                                        NewId(),
                                                        @site_id,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        @isActive
                                                        )  SELECT * FROM MachineGroupMachines WHERE Id = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMachineGroupMachinesQuery, GetSQLParameters(machineGroupMachinesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMachineGroupMachinesDTO(machineGroupMachinesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting machineGroupMachinesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(machineGroupMachinesDTO);
            return machineGroupMachinesDTO;
        }

        /// <summary>
        /// Updates the MachineGroupMachines record
        /// </summary>
        /// <param name="machineGroupMachinesDTO">MachineGroupMachinesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public MachineGroupMachinesDTO UpdateMachineGroupMachines(MachineGroupMachinesDTO machineGroupMachinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineGroupMachinesDTO, loginId, siteId);
            string updateMachineGroupsQuery = @"update MachineGroupMachines 
                                         set GroupName=@groupName,
                                             Remarks=@remarks,
                                             --site_id=@site_id,
                                             --SynchStatus = @synchStatus,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastUpdateDate = Getdate(),
                                             IsActive = @isActive
                                       where MachineGroupId = @machineGroupId
                         SELECT * FROM MachineGroupMachines WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMachineGroupsQuery, GetSQLParameters(machineGroupMachinesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMachineGroupMachinesDTO(machineGroupMachinesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating machineGroupMachinesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(machineGroupMachinesDTO);
            return machineGroupMachinesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="machineGroupMachinesDTO">machineGroupMachinesDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshMachineGroupMachinesDTO(MachineGroupMachinesDTO machineGroupMachinesDTO, DataTable dt)
        {
            log.LogMethodEntry(machineGroupMachinesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                machineGroupMachinesDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                machineGroupMachinesDTO.LastupdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                machineGroupMachinesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                machineGroupMachinesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                machineGroupMachinesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                machineGroupMachinesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                machineGroupMachinesDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to MachineGroupMachines class type
        /// </summary>
        /// <param name="machineGroupMachinesDataRow">MachineGroupMachinesDTO DataRow</param>
        /// <returns>Returns MachineGroupMachinesDTO</returns>
        private MachineGroupMachinesDTO GetMachineGroupMachinesDTO(DataRow machineGroupMachinesDataRow)
        {
            log.LogMethodEntry(machineGroupMachinesDataRow);
            MachineGroupMachinesDTO machineGroupMachinesDataObject = new MachineGroupMachinesDTO(Convert.ToInt32(machineGroupMachinesDataRow["Id"]),
                                            Convert.ToInt32(machineGroupMachinesDataRow["MachineGroupId"]),
                                            Convert.ToInt32(machineGroupMachinesDataRow["MachineId"]),
                                            machineGroupMachinesDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(machineGroupMachinesDataRow["Guid"]),
                                            machineGroupMachinesDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(machineGroupMachinesDataRow["site_id"]),
                                            machineGroupMachinesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(machineGroupMachinesDataRow["SynchStatus"]),
                                            machineGroupMachinesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(machineGroupMachinesDataRow["MasterEntityId"]),
                                            machineGroupMachinesDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(machineGroupMachinesDataRow["CreatedBy"]),
                                            machineGroupMachinesDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineGroupMachinesDataRow["CreationDate"]),
                                            machineGroupMachinesDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(machineGroupMachinesDataRow["LastUpdatedBy"]),
                                            machineGroupMachinesDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineGroupMachinesDataRow["LastUpdateDate"]),
                                            machineGroupMachinesDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(machineGroupMachinesDataRow["IsActive"])
                                            );
            log.LogMethodExit(machineGroupMachinesDataObject);
            return machineGroupMachinesDataObject;
        }

        /// <summary>
        /// Gets the MachineGroupMachines data of passed machineGroupMachineId id
        /// </summary>
        /// <param name="machineGroupMachineId">integer type parameter</param>
        /// <returns>Returns MachineGroupMachinesDTO</returns>
        public MachineGroupMachinesDTO GetMachineGroupMachines(int machineGroupMachineId)
        {
            log.LogMethodEntry(machineGroupMachineId);
            MachineGroupMachinesDTO result = null;
            string selectMachineGroupMachinesQuery = SELECT_QUERY + @" WHERE mg.Id= @machineGroupMachineId";
            SqlParameter parameter = new SqlParameter("@machineGroupMachineId", machineGroupMachineId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectMachineGroupMachinesQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMachineGroupMachinesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Gets the MachineGroupMachinesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MachineGroupMachinesDTO matching the search criteria</returns>
        public List<MachineGroupMachinesDTO> GetMachineGroupMachinesList(List<KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<MachineGroupMachinesDTO> machineGroupMachinesList = null;
            string selectMachineGroupMachinesQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                         if (searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.MACHINE_GROUP_ID) ||
                            searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.ID) ||
                            searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.ISACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));

                        }
                        else if (searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.MACHINE_GROUP_ID_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + ") is Not Null and " + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                    selectMachineGroupMachinesQuery = selectMachineGroupMachinesQuery + query;
            }

            DataTable machineGroupMachinesData = dataAccessHandler.executeSelectQuery(selectMachineGroupMachinesQuery, parameters.ToArray(), sqlTransaction);
            if (machineGroupMachinesData.Rows.Count > 0)
            {
                machineGroupMachinesList = new List<MachineGroupMachinesDTO>();
                foreach (DataRow machineGroupMachinesDataRow in machineGroupMachinesData.Rows)
                {
                    MachineGroupMachinesDTO machineGroupMachinesDataObject = GetMachineGroupMachinesDTO(machineGroupMachinesDataRow);
                    machineGroupMachinesList.Add(machineGroupMachinesDataObject);
                }
            }
            log.LogMethodExit(machineGroupMachinesList);
            return machineGroupMachinesList;
        }

        /// <summary>
        ///  Deletes the MachineGroupMachinesDTO record
        /// </summary>
        /// <param name="MachineGroupMachinesDTO">MachineGroupMachinesDTO is passed as parameter</param>
        internal void Delete(MachineGroupMachinesDTO machineGroupMachinesDTO)
        {
            log.LogMethodEntry(machineGroupMachinesDTO);
            string query = @"DELETE  
                             FROM MachineGroupMachines
                             WHERE MachineGroupMachines.Id= @Id";
            SqlParameter parameter = new SqlParameter("@Id", machineGroupMachinesDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            machineGroupMachinesDTO.AcceptChanges();
            log.LogMethodExit();
        }

    }

}


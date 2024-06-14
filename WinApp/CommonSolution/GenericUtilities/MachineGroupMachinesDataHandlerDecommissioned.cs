/********************************************************************************************
 * Project Name - MachineGroups Data Handler
 * Description  - Data handler of the MachineGroups handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        13-May-2019   Jagan Mohana Rao        Created 
 *2.70.2        25-Jul-2019   Dakshakh Raj            Modified : added GetSQLParameters(), 
 *                                                          SQL injection Issue Fix
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class MachineGroupMachinesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MachineGroupMachines as mgm ";

        /// <summary>
        /// Dictionary for searching Parameters for the ApplicationContent object.
        /// </summary>
        private static readonly Dictionary<MachineGroupMachinesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MachineGroupMachinesDTO.SearchByParameters, string>
               {
                    {MachineGroupMachinesDTO.SearchByParameters.MACHINE_GROUP_ID, "mgm.MachineGroupId"},
                    {MachineGroupMachinesDTO.SearchByParameters.MACHINE_ID, "mgm.MachineId"},
                    {MachineGroupMachinesDTO.SearchByParameters.SITE_ID,"mgm.site_id"},
                    {MachineGroupMachinesDTO.SearchByParameters.ISACTIVE,"mgm.IsActive"},
                    {MachineGroupMachinesDTO.SearchByParameters.MASTER_ENTITY_ID,"mgm.MasterEntityId"}
               };
        /// <summary>
        /// Default constructor of MachineGroupMachinesDataHandler class
        /// </summary>
        public MachineGroupMachinesDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MachineGroups Record.
        /// </summary>
        /// <param name="machineGroupMachinesDTO">MachineGroupMachinesDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MachineGroupMachinesDTO machineGroupMachinesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineGroupMachinesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", machineGroupMachinesDTO.Id,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineGroupId", machineGroupMachinesDTO.MachineGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineId", machineGroupMachinesDTO.MachineId,true));            
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", machineGroupMachinesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", machineGroupMachinesDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the MachineGroupMachines record to the database
        /// </summary>
        /// <param name="machineGroupMachinesDTO">MachineGroupMachinesDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertMachineGroupMachines(MachineGroupMachinesDTO machineGroupMachinesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(machineGroupMachinesDTO, userId, siteId);
            int idOfRowInserted;
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
                                                        )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(insertMachineGroupMachinesQuery, GetSQLParameters(machineGroupMachinesDTO, userId, siteId).ToArray(), null);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the MachineGroupMachines record
        /// </summary>
        /// <param name="machineGroupMachinesDTO">MachineGroupMachinesDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateMachineGroupMachines(MachineGroupMachinesDTO machineGroupMachinesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(machineGroupMachinesDTO, userId, siteId);
            int rowsUpdated;
            string updateMachineGroupsQuery = @"update MachineGroups 
                                         set GroupName=@groupName,
                                             Remarks=@remarks,
                                             --site_id=@site_id,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastUpdateDate = Getdate(),
                                             IsActive = @isActive
                                       where MachineGroupId = @machineGroupId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(updateMachineGroupsQuery, GetSQLParameters(machineGroupMachinesDTO, userId, siteId).ToArray(), null);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
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
                                            machineGroupMachinesDataRow["Guid"].ToString(),
                                            machineGroupMachinesDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(machineGroupMachinesDataRow["site_id"]),
                                            machineGroupMachinesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(machineGroupMachinesDataRow["SynchStatus"]),
                                            machineGroupMachinesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(machineGroupMachinesDataRow["MasterEntityId"]),
                                            machineGroupMachinesDataRow["CreatedBy"].ToString(),
                                            machineGroupMachinesDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineGroupMachinesDataRow["CreationDate"]),
                                            machineGroupMachinesDataRow["LastUpdatedBy"].ToString(),
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
            string selectMachineGroupMachinesQuery = @"select * from MachineGroupMachines where Id = @machineGroupMachineId";
            SqlParameter[] selectMachineGroupMachinesParameters = new SqlParameter[1];
            selectMachineGroupMachinesParameters[0] = new SqlParameter("@machineGroupMachineId", machineGroupMachineId);
            DataTable machineGroupMachines = dataAccessHandler.executeSelectQuery(selectMachineGroupMachinesQuery, selectMachineGroupMachinesParameters);
            if (machineGroupMachines.Rows.Count > 0)
            {
                DataRow machineGroupMachinesRow = machineGroupMachines.Rows[0];
                MachineGroupMachinesDTO machineGroupMachinesDataObject = GetMachineGroupMachinesDTO(machineGroupMachinesRow);
                log.LogMethodExit(machineGroupMachinesDataObject);
                return machineGroupMachinesDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Gets the MachineGroupMachinesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MachineGroupMachinesDTO matching the search criteria</returns>
        public List<MachineGroupMachinesDTO> GetMachineGroupMachinesList(List<KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectMachineGroupMachinesQuery = @"select * from MachineGroupMachines";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.MACHINE_GROUP_ID) ||
                                searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.MACHINE_ID) ||
                                searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.ISACTIVE))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.SITE_ID))
                            {
                                query.Append("(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else
                            {
                                query.Append("(" + DBSearchParameters[searchParameter.Key] + " is Not Null and  " + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "')");
                            }
                        }
                        else
                        {
                            if (searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.MACHINE_GROUP_ID) ||
                                searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.MACHINE_ID) ||
                                searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.ISACTIVE))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(MachineGroupMachinesDTO.SearchByParameters.SITE_ID))
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else
                            {
                                query.Append(" or (" + DBSearchParameters[searchParameter.Key] + " is Not Null and  " + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "')");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit();
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectMachineGroupMachinesQuery = selectMachineGroupMachinesQuery + query;
            }

            DataTable machineGroupMachinesData = dataAccessHandler.executeSelectQuery(selectMachineGroupMachinesQuery, null);
            if (machineGroupMachinesData.Rows.Count > 0)
            {
                List<MachineGroupMachinesDTO> machineGroupMachinesList = new List<MachineGroupMachinesDTO>();
                foreach (DataRow machineGroupMachinesDataRow in machineGroupMachinesData.Rows)
                {
                    MachineGroupMachinesDTO machineGroupMachinesDataObject = GetMachineGroupMachinesDTO(machineGroupMachinesDataRow);
                    machineGroupMachinesList.Add(machineGroupMachinesDataObject);
                }
                log.LogMethodExit(machineGroupMachinesList);
                return machineGroupMachinesList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
    }
}
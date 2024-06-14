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
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Core.GenericUtilities
{
    public class MachineGroupsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private static readonly Dictionary<MachineGroupsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MachineGroupsDTO.SearchByParameters, string>
               {
                    {MachineGroupsDTO.SearchByParameters.MACHINE_GROUP_ID, "MachineGroupId"},
                    {MachineGroupsDTO.SearchByParameters.GROUP_NAME, "GroupName"},
                    {MachineGroupsDTO.SearchByParameters.SITE_ID,"site_id"},
                    {MachineGroupsDTO.SearchByParameters.ISACTIVE,"IsActive"}
               };
        /// <summary>
        /// Default constructor of MachineGroupsDataHandler class
        /// </summary>
        public MachineGroupsDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MachineGroups Record.
        /// </summary>
        /// <param name="machineGroupsDTO">MachineGroupsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MachineGroupsDTO machineGroupsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(machineGroupsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineGroupId", machineGroupsDTO.MachineGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@groupName", machineGroupsDTO.GroupName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", machineGroupsDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", machineGroupsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", machineGroupsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the MachineGroups record to the database
        /// </summary>
        /// <param name="machineGroupsDTO">MachineGroupsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertMachineGroups(MachineGroupsDTO machineGroupsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(machineGroupsDTO, userId, siteId);
            int idOfRowInserted;
            string insertMachineGroupsQuery = @"insert into MachineGroups 
                                                        (
                                                        GroupName,
                                                        Remarks,
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
                                                        @groupName,
                                                        @remarks,
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
                idOfRowInserted = dataAccessHandler.executeInsertQuery(insertMachineGroupsQuery, GetSQLParameters(machineGroupsDTO, userId, siteId).ToArray(), null);
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
        /// Updates the Machine Groups record
        /// </summary>
        /// <param name="machineGroupsDTO">MachineGroupsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateMachineGroups(MachineGroupsDTO machineGroupsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(machineGroupsDTO, userId, siteId);
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
                rowsUpdated = dataAccessHandler.executeUpdateQuery(updateMachineGroupsQuery, GetSQLParameters(machineGroupsDTO, userId, siteId).ToArray(), null);
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
        /// Converts the Data row object to MachineGroupsDTO class type
        /// </summary>
        /// <param name="machineGroupsDataRow">MachineGroupsDTO DataRow</param>
        /// <returns>Returns MachineGroupsDTO</returns>
        private MachineGroupsDTO GetMachineGroupsDTO(DataRow machineGroupsDataRow)
        {
            log.LogMethodEntry(machineGroupsDataRow);
            MachineGroupsDTO machineGroupsDataObject = new MachineGroupsDTO(Convert.ToInt32(machineGroupsDataRow["MachineGroupId"]),
                                            machineGroupsDataRow["GroupName"].ToString(),
                                            machineGroupsDataRow["Remarks"].ToString(),
                                            machineGroupsDataRow["Guid"].ToString(),
                                            machineGroupsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(machineGroupsDataRow["site_id"]),
                                            machineGroupsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(machineGroupsDataRow["SynchStatus"]),
                                            machineGroupsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(machineGroupsDataRow["MasterEntityId"]),
                                            machineGroupsDataRow["CreatedBy"].ToString(),
                                            machineGroupsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineGroupsDataRow["CreationDate"]),
                                            machineGroupsDataRow["LastUpdatedBy"].ToString(),
                                            machineGroupsDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineGroupsDataRow["LastUpdateDate"]),
                                            machineGroupsDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(machineGroupsDataRow["IsActive"])
                                            );
            log.LogMethodExit();
            return machineGroupsDataObject;
        }

        /// <summary>
        /// Gets the MachineGroups data of passed machineGroupId id
        /// </summary>
        /// <param name="machineGroupId">integer type parameter</param>
        /// <returns>Returns MachineGroupsDTO</returns>
        public MachineGroupsDTO GetMachineGroups(int machineGroupId)
        {
            log.LogMethodEntry(machineGroupId);
            string selectMachineGroupsQuery = @"select * from MachineGroups where MachineGroupId = @machineGroupId";
            SqlParameter[] selectMachineGroupsParameters = new SqlParameter[1];
            selectMachineGroupsParameters[0] = new SqlParameter("@machineGroupId", machineGroupId);
            DataTable machineGroups = dataAccessHandler.executeSelectQuery(selectMachineGroupsQuery, selectMachineGroupsParameters);
            if (machineGroups.Rows.Count > 0)
            {
                DataRow machineGroupsRow = machineGroups.Rows[0];
                MachineGroupsDTO machineGroupsDataObject = GetMachineGroupsDTO(machineGroupsRow);
                log.LogMethodExit(machineGroupsDataObject);
                return machineGroupsDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Gets the MachineGroupsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MachineGroupsDTO matching the search criteria</returns>
        public List<MachineGroupsDTO> GetMachineGroupsList(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectMachineGroupsQuery = @"select * from MachineGroups";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MachineGroupsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? " " : " and ";
                        if (searchParameter.Key.Equals(MachineGroupsDTO.SearchByParameters.MACHINE_GROUP_ID) ||
                              searchParameter.Key.Equals(MachineGroupsDTO.SearchByParameters.GROUP_NAME) ||
                              searchParameter.Key.Equals(MachineGroupsDTO.SearchByParameters.ISACTIVE))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key.Equals(MachineGroupsDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
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
                    selectMachineGroupsQuery = selectMachineGroupsQuery + query;
            }

            DataTable machineGroupsData = dataAccessHandler.executeSelectQuery(selectMachineGroupsQuery, null);
            if (machineGroupsData.Rows.Count > 0)
            {
                List<MachineGroupsDTO> machineGroupsList = new List<MachineGroupsDTO>();
                foreach (DataRow machineGroupsDataRow in machineGroupsData.Rows)
                {
                    MachineGroupsDTO machineGroupsDataObject = GetMachineGroupsDTO(machineGroupsDataRow);
                    machineGroupsList.Add(machineGroupsDataObject);
                }
                log.LogMethodExit(machineGroupsList);
                return machineGroupsList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
    }
}
/********************************************************************************************
 * Project Name - MachineGroups Data Handler
 * Description  - Data handler of the MachineGroups handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        13-May-2019   Jagan Mohana Rao        Created 
 *2.70.2        31-Jul-2019   Deeksha                 Modified:SQL injection Issue Fix.Insert/Update method returns DTO. 
 *2.90        03-Jun-2020    Mushahid Faizan          Modified: 3 tier changes for Rest API.
 *2.110.0     21-Dec-2020    Prajwal S              Modified GetMachineList using searchParameters. 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Game
{
    public class MachineGroupsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MachineGroups AS mg ";
        private List<SqlParameter> parameters = new List<SqlParameter>();
        private static readonly Dictionary<MachineGroupsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MachineGroupsDTO.SearchByParameters, string>
               {
                    {MachineGroupsDTO.SearchByParameters.MACHINE_GROUP_ID, "mg.MachineGroupId"},
                    {MachineGroupsDTO.SearchByParameters.GROUP_NAME, "mg.GroupName"},
                    {MachineGroupsDTO.SearchByParameters.SITE_ID,"mg.site_id"},
                    {MachineGroupsDTO.SearchByParameters.ISACTIVE,"mg.IsActive"},
                    {MachineGroupsDTO.SearchByParameters.MASTER_ENTITY_ID,"mg.MasterEntityId"}
               };
        private readonly SqlTransaction sqlTransaction;

        /// <summary>
        /// Default constructor of MachineGroupsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public MachineGroupsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MachineGroups Record.
        /// </summary>
        /// <param name="machineGroupsDTO">MachineGroupsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MachineGroupsDTO machineGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineGroupsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineGroupId", machineGroupsDTO.MachineGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@groupName", machineGroupsDTO.GroupName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", machineGroupsDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", machineGroupsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", machineGroupsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the MachineGroups record to the database
        /// </summary>
        /// <param name="machineGroupsDTO">MachineGroupsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public MachineGroupsDTO InsertMachineGroups(MachineGroupsDTO machineGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineGroupsDTO, loginId, siteId);
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
                                                        ) SELECT * FROM MachineGroups WHERE MachineGroupId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMachineGroupsQuery, GetSQLParameters(machineGroupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMachineGroupsDTO(machineGroupsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting machineGroupsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(machineGroupsDTO);
            return machineGroupsDTO;
        }

        /// <summary>
        /// Updates the Machine Groups record
        /// </summary>
        /// <param name="machineGroupsDTO">MachineGroupsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public MachineGroupsDTO UpdateMachineGroups(MachineGroupsDTO machineGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(machineGroupsDTO, loginId, siteId);
            string updateMachineGroupsQuery = @"update MachineGroups 
                                         set GroupName=@groupName,
                                             Remarks=@remarks,
                                             --site_id=@site_id,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastUpdateDate = Getdate(),
                                             IsActive = @isActive
                                       where MachineGroupId = @machineGroupId
                                     SELECT * FROM MachineGroups WHERE MachineGroupId = @machineGroupId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMachineGroupsQuery, GetSQLParameters(machineGroupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMachineGroupsDTO(machineGroupsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating machineGroupsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(machineGroupsDTO);
            return machineGroupsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="machineGroupsDTO">machineGroupsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshMachineGroupsDTO(MachineGroupsDTO machineGroupsDTO, DataTable dt)
        {
            log.LogMethodEntry(machineGroupsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                machineGroupsDTO.MachineGroupId = Convert.ToInt32(dt.Rows[0]["MachineGroupId"]);
                machineGroupsDTO.LastupdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                machineGroupsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                machineGroupsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                machineGroupsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                machineGroupsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                machineGroupsDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
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
                                            machineGroupsDataRow["GroupName"] == DBNull.Value ? string.Empty : Convert.ToString(machineGroupsDataRow["GroupName"]),
                                            machineGroupsDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(machineGroupsDataRow["Remarks"]),
                                            machineGroupsDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(machineGroupsDataRow["Guid"]),
                                            machineGroupsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(machineGroupsDataRow["site_id"]),
                                            machineGroupsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(machineGroupsDataRow["SynchStatus"]),
                                            machineGroupsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(machineGroupsDataRow["MasterEntityId"]),
                                            machineGroupsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(machineGroupsDataRow["CreatedBy"]),
                                            machineGroupsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineGroupsDataRow["CreationDate"]),
                                            machineGroupsDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(machineGroupsDataRow["LastUpdatedBy"]),
                                            machineGroupsDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(machineGroupsDataRow["LastUpdateDate"]),
                                            machineGroupsDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(machineGroupsDataRow["IsActive"])
                                            );
            log.LogMethodExit(machineGroupsDataObject);
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
            MachineGroupsDTO result = null;
            string query = SELECT_QUERY + @" WHERE mg.MachineGroupId= @machineGroupId";
            SqlParameter parameter = new SqlParameter("@machineGroupId", machineGroupId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMachineGroupsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        public int GetMachineGroupsCount(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int machineGroupsCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                machineGroupsCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(machineGroupsCount);
            return machineGroupsCount;
        }
        public string GetFilterQuery(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters)
        {
            int count = 0;
            StringBuilder query = new StringBuilder(" ");
            parameters = new List<SqlParameter>();
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<MachineGroupsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key.Equals(MachineGroupsDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MachineGroupsDTO.SearchByParameters.MACHINE_GROUP_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MachineGroupsDTO.SearchByParameters.GROUP_NAME))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(MachineGroupsDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MachineGroupsDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MachineGroupsDTO.SearchByParameters.ISACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));

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
        /// Gets the CategoryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MachineGroupsDTO matching the search criteria</returns>
        public List<MachineGroupsDTO> GetMachineGroupsList(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters);
            List<MachineGroupsDTO> machineGroupsDTOList = new List<MachineGroupsDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            if (currentPage >= 0 && pageSize > 0)
            {
                selectQuery += " ORDER BY mg.MachineGroupId desc OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                machineGroupsDTOList = new List<MachineGroupsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MachineGroupsDTO machineGroupsDTO = GetMachineGroupsDTO(dataRow);
                    machineGroupsDTOList.Add(machineGroupsDTO);
                }
            }
            log.LogMethodExit(machineGroupsDTOList);
            return machineGroupsDTOList;
        }
        /// <summary>
        /// Gets the MachineGroupsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MachineGroupsDTO matching the search criteria</returns>
        public List<MachineGroupsDTO> GetMachineGroupsList(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<MachineGroupsDTO> machineGroupsDTOList = new List<MachineGroupsDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable machineGroupsData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (machineGroupsData.Rows.Count > 0)
            {
                foreach (DataRow machineGroupsDataRow in machineGroupsData.Rows)
                {
                    MachineGroupsDTO machineGroupsDataObject = GetMachineGroupsDTO(machineGroupsDataRow);
                    machineGroupsDTOList.Add(machineGroupsDataObject);
                }
            }
            log.LogMethodExit(machineGroupsDTOList);
            return machineGroupsDTOList;
        }

        /// <summary>
        ///  Deletes the MachineGroups record
        /// </summary>
        /// <param name="MachineGroupsDTO">MachineGroupsDTO is passed as parameter</param>
        internal void Delete(MachineGroupsDTO machineGroupsDTO)
        {
            log.LogMethodEntry(machineGroupsDTO);
            string query = @"DELETE  
                             FROM MachineGroups
                             WHERE MachineGroups.MachineGroupId= @machineGroupId";
            SqlParameter parameter = new SqlParameter("@machineGroupId", machineGroupsDTO.MachineGroupId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            machineGroupsDTO.AcceptChanges();
            log.LogMethodExit();
        }
    }
}
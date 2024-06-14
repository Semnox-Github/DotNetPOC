/********************************************************************************************
 * Project Name - Utilities
 * Description  - Get and Insert or update methods for Task Types details.
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        11-Mar-2019   Jagan Mohana          Created 
              08-Apr-2019   Mushahid Faizan       Modified- GetTaskTypeDTO(),GetSQLParameters,Insert and Update Method, Added logMethodEntry/logMethodExit.
 *2.70.2      11-Dec-2019   Jinto Thomas          Removed siteid from update query
 *2.120.0     25-Mar-2021   Fiona                 Added GetTaskTypesLastUpdateTime for container refresh
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Core.Utilities
{
    public class TaskTypesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<TaskTypesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TaskTypesDTO.SearchByParameters, string>
        {
            { TaskTypesDTO.SearchByParameters.TASK_TYPE_ID,"task_type_id"},
            { TaskTypesDTO.SearchByParameters.TASK_TYPE, "task_type"},
            { TaskTypesDTO.SearchByParameters.SITE_ID, "site_id"}
        };

        /// <summary>
        /// Default constructor of TaskTypesDataHandler class
        /// </summary>
        public TaskTypesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to TaskTypesDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private TaskTypesDTO GetTaskTypeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TaskTypesDTO taskTypeDTO = new TaskTypesDTO(Convert.ToInt32(dataRow["task_type_id"]),
                                            dataRow["task_type"] == DBNull.Value ? "" : dataRow["task_type"].ToString(),
                                            dataRow["requires_manager_approval"] == DBNull.Value ? "" : dataRow["requires_manager_approval"].ToString(),
                                            dataRow["display_in_pos"] == DBNull.Value ? "" : dataRow["display_in_pos"].ToString(),
                                            dataRow["task_type_name"] == DBNull.Value ? "" : dataRow["task_type_name"].ToString(),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(taskTypeDTO);
            return taskTypeDTO;
        }

        /// <summary>
        /// Gets the TaskTypesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TaskTypesDTO matching the search criteria</returns>
        public List<TaskTypesDTO> GetTaskTypes(List<KeyValuePair<TaskTypesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int count = 0;
            string selectQuery = @"select * from task_type";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<TaskTypesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        {
                            if (searchParameter.Key.Equals(TaskTypesDTO.SearchByParameters.TASK_TYPE_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == TaskTypesDTO.SearchByParameters.SITE_ID)
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
                        count++;
                    }
                    else
                    {
                        log.Error("Ends-GetTaskTypes(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception();
                    }
                }
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }

            DataTable taskTypeDataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (taskTypeDataTable.Rows.Count > 0)
            {
                List<TaskTypesDTO> taskTypesDTOList = new List<TaskTypesDTO>();
                foreach (DataRow taskTypeDataRow in taskTypeDataTable.Rows)
                {
                    TaskTypesDTO taskTypeDataObject = GetTaskTypeDTO(taskTypeDataRow);
                    taskTypesDTOList.Add(taskTypeDataObject);
                }
                log.LogMethodExit(taskTypesDTOList);
                return taskTypesDTOList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating task types Record.
        /// </summary>
        /// <param name="taskTypesDTO">TaskTypesDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        /// Modified by Mushahid Faizan on 08-Apr-2019 to call DataAccessHandler GetSQLParameter, added SynchStatus Column
        private List<SqlParameter> GetSQLParameters(TaskTypesDTO taskTypesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(taskTypesDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@taskTypeId", taskTypesDTO.TaskTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taskType", taskTypesDTO.TaskType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requiresManagerApproval", taskTypesDTO.RequiresManagerApproval));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayInPos", taskTypesDTO.DisplayInPOS));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taskTypeName", taskTypesDTO.TaskTypeName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", taskTypesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the task types record to the database
        /// </summary>
        /// <param name="taskTypesDTO">taskTypesDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        /// LastUpdatedDate column name mismatch modified by Mushahid Faizan on 08-Apr-2019
        public int InsertTaskTypes(TaskTypesDTO taskTypesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(taskTypesDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"insert into task_type 
                                                        (                                                         
                                                        task_type,
                                                        requires_manager_approval,
                                                        display_in_pos,
                                                        task_type_name,                                                        
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate                                                        
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @taskType,
                                                        @requiresManagerApproval,
                                                        @displayInPos,
                                                        @taskTypeName,                                                        
                                                        NewId(),
                                                        @siteId,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        GETDATE(),                                                        
                                                        @lastUpdatedBy,                                                        
                                                        GetDate()
                                            )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(taskTypesDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the task types record
        /// </summary>
        /// <param name="taskTypesDTO">taskTypesDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        /// /// LastUpdatedDate column name mismatch modified by Mushahid Faizan on 08-Apr-2019
        public int UpdateTaskTypes(TaskTypesDTO taskTypesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(taskTypesDTO, userId, siteId);
            int rowsUpdated;
            string query = @"update task_type 
                                         set task_type=@taskType,
                                             requires_manager_approval= @requiresManagerApproval,                                                                                                  
                                             display_in_pos = @displayInPos,
                                             task_type_name = @taskTypeName,                                             
                                             -- site_id = @siteId,                                             
                                             MasterEntityId = @masterEntityId,
                                             LastUpdatedBy = @lastUpdatedBy,
                                             LastUpdateDate = GETDATE()
                                       where task_type_id = @taskTypeId";

            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(taskTypesDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }
        internal DateTime? GetTaskTypesLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdateDate) LastUpdateDate 
                            FROM (
                            select max(LastUpdateDate) LastUpdateDate from task_type WHERE (site_id = @siteId or @siteId = -1)
                            )a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdateDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdateDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}

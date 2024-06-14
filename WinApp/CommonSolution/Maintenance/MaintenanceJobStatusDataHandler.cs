/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Data Handler of the MaintenanceJobStatus class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.100.0    22-Sept-2020   Mushahid Faizan         Created.
 *2.110.0    13-March-2020   Gururaja Kanjan        Changed to site_id.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Maintenance
{
    public class MaintenanceJobStatusDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<MaintenanceJobStatusDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MaintenanceJobStatusDTO.SearchByParameters, string>
               {
                    {MaintenanceJobStatusDTO.SearchByParameters.JOB_STATUS_ID, "JobStatusId"},
                    {MaintenanceJobStatusDTO.SearchByParameters.JOB_STATUS, "JobStatus"},
                    {MaintenanceJobStatusDTO.SearchByParameters.MAINT_CHKLST_DETAIL_ID, "MaintChklstdetId"},
                    {MaintenanceJobStatusDTO.SearchByParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                    {MaintenanceJobStatusDTO.SearchByParameters.SITE_ID,"Site_Id"},
                    {MaintenanceJobStatusDTO.SearchByParameters.IS_ACTIVE,"IsActive"}
               };

        /// <summary>
        /// Default constructor of MaintenanceJobStatusDataHandler class
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public MaintenanceJobStatusDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to MaintenanceJobStatusDTO class type
        /// </summary>
        /// <param name="MaintenanceJobStatusDataRow">MaintenanceJobStatusDTO DataRow</param>
        /// <returns>Returns MaintenanceJobStatusDTO</returns>
        private MaintenanceJobStatusDTO GetMaintenanceJobStatusDTO(DataRow MaintenanceJobStatusDataRow)
        {
            log.LogMethodEntry(MaintenanceJobStatusDataRow);
            MaintenanceJobStatusDTO MaintenanceJobStatusDataObject = new MaintenanceJobStatusDTO(Convert.ToInt32(MaintenanceJobStatusDataRow["JobStatusId"]),
                                            MaintenanceJobStatusDataRow["MaintChklstdetId"] == DBNull.Value ? -1 : Convert.ToInt32(MaintenanceJobStatusDataRow["MaintChklstdetId"]),
                                            MaintenanceJobStatusDataRow["JobStatus"] == DBNull.Value ? string.Empty : Convert.ToString(MaintenanceJobStatusDataRow["JobStatus"]),
                                            MaintenanceJobStatusDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(MaintenanceJobStatusDataRow["IsActive"]),
                                            MaintenanceJobStatusDataRow["CreatedBy"].ToString(),
                                            MaintenanceJobStatusDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(MaintenanceJobStatusDataRow["CreationDate"]),
                                            MaintenanceJobStatusDataRow["LastUpdatedBy"].ToString(),
                                            MaintenanceJobStatusDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(MaintenanceJobStatusDataRow["LastUpdateDate"]),
                                            MaintenanceJobStatusDataRow["Site_Id"] == DBNull.Value ? -1 : Convert.ToInt32(MaintenanceJobStatusDataRow["Site_Id"]),
                                            MaintenanceJobStatusDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(MaintenanceJobStatusDataRow["MasterEntityId"]),
                                            MaintenanceJobStatusDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(MaintenanceJobStatusDataRow["SynchStatus"]),
                                            MaintenanceJobStatusDataRow["Guid"].ToString()
                                            );
            log.LogMethodExit();
            return MaintenanceJobStatusDataObject;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MaintenanceJobStatusDTO Record.
        /// </summary>
        /// <param name="MaintenanceJobStatusDTO">MaintenanceJobStatusDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MaintenanceJobStatusDTO MaintenanceJobStatusDTO, string userId, int siteId)
        {
            log.LogMethodEntry(MaintenanceJobStatusDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@jobStatusId", MaintenanceJobStatusDTO.JobStatusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maintChklstdetId", MaintenanceJobStatusDTO.MaintChklstdetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@jobStatus", MaintenanceJobStatusDTO.JobStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", MaintenanceJobStatusDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", MaintenanceJobStatusDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }
        private void RefreshMaintenanceJobStatusDTO(MaintenanceJobStatusDTO MaintenanceJobStatusDTO, DataTable dt)
        {
            log.LogMethodEntry(MaintenanceJobStatusDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                MaintenanceJobStatusDTO.JobStatusId = Convert.ToInt32(dt.Rows[0]["JobStatusId"]);
                MaintenanceJobStatusDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                MaintenanceJobStatusDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                MaintenanceJobStatusDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                MaintenanceJobStatusDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                MaintenanceJobStatusDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                MaintenanceJobStatusDTO.SiteId = dataRow["Site_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_Id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the MaintenanceJobStatus record to the database
        /// </summary>
        /// <param name="MaintenanceJobStatusDTO">MaintenanceJobStatusDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public MaintenanceJobStatusDTO InsertMaintenanceJobStatus(MaintenanceJobStatusDTO MaintenanceJobStatusDTO, string userId, int siteId)
        {
            log.LogMethodEntry(MaintenanceJobStatusDTO, userId, siteId);
            string insertQuery = @"insert into MaintenanceJobStatus 
                                                        (
                                                           MaintChklstdetId,
                                                           JobStatus,
                                                           IsActive,
                                                           CreatedBy,
                                                           CreationDate,
                                                           LastUpdatedBy,
                                                           LastUpdateDate,
                                                           Site_Id,
                                                           Guid,
                                                           MasterEntityId
                                                        ) 
                                                values 
                                                        ( 
                                                           @maintChklstdetId,
                                                           @jobStatus,
                                                           @isActive,
                                                           @createdBy,
                                                           GetDate(),
                                                           @lastUpdatedBy,
                                                           GetDate(),
                                                           @site_id,
                                                           NewId(),
                                                           @masterEntityId
                                                        )SELECT * FROM MaintenanceJobStatus WHERE JobStatusId = scope_identity() ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, GetSQLParameters(MaintenanceJobStatusDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshMaintenanceJobStatusDTO(MaintenanceJobStatusDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(MaintenanceJobStatusDTO);
            return MaintenanceJobStatusDTO;
        }

        /// <summary>
        /// Updates the MaintenanceJobStatusDTO record
        /// </summary>
        /// <param name="MaintenanceJobStatusDTO">MaintenanceJobStatusDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public MaintenanceJobStatusDTO UpdateMaintenanceJobStatus(MaintenanceJobStatusDTO MaintenanceJobStatusDTO, string userId, int siteId)
        {
            log.LogMethodEntry(MaintenanceJobStatusDTO, userId, siteId);
            string updateQuery = @"update MaintenanceJobStatus  set 
                                              MaintChklstdetId= @maintChklstdetId,
                                              JobStatus  = @jobStatus,
                                              IsActive                  = @isActive,
                                              LastUpdatedBy             = @lastUpdatedBy,
                                              LastUpdateDate            = GetDate(),
                                              MasterEntityId            = @masterEntityId
                                       where JobStatusId = @jobStatusId
                                       select * from MaintenanceJobStatus WHERE JobStatusId = @jobStatusId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, GetSQLParameters(MaintenanceJobStatusDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshMaintenanceJobStatusDTO(MaintenanceJobStatusDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(MaintenanceJobStatusDTO);
            return MaintenanceJobStatusDTO;
        }

        /// <summary>
        /// Deletes the MaintenanceJobStatus record of passed  jobStatusId
        /// </summary>
        /// <param name="jobStatusId">integer type parameter</param>
        public void Delete(int jobStatusId)
        {
            log.LogMethodEntry(jobStatusId);
            string query = @"DELETE  
                             FROM MaintenanceJobStatus
                             WHERE JobStatusId = @JobStatusId";
            SqlParameter parameter = new SqlParameter("@JobStatusId", jobStatusId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter });
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the MaintenanceJobStatus data of passed JobStatusId
        /// </summary>
        /// <param name="jobStatusId">integer type parameter</param>
        /// <returns>Returns MaintenanceJobStatusDTO</returns>
        public MaintenanceJobStatusDTO GetMaintenanceJobStatusDTO(int jobStatusId)
        {
            log.LogMethodEntry(jobStatusId);
            string selectQuery = @"select * from MaintenanceJobStatus where JobStatusId = @JobStatusId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@JobStatusId", jobStatusId);
            DataTable maintenanceJobStatus = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters);
            if (maintenanceJobStatus.Rows.Count > 0)
            {
                DataRow dataRow = maintenanceJobStatus.Rows[0];
                MaintenanceJobStatusDTO MaintenanceJobStatusDTO = GetMaintenanceJobStatusDTO(dataRow);
                log.LogMethodExit(MaintenanceJobStatusDTO);
                return MaintenanceJobStatusDTO;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the MaintenanceJobStatusDTO List for maintChklstdet Id List
        /// </summary>
        /// <param name="maintChklstdetIdList">integer list parameter</param>
        /// <returns>Returns List of MaintenanceJobStatusDTOList</returns>
        public List<MaintenanceJobStatusDTO> GetMaintenanceJobStatusDTOList(List<int> maintChklstdetIdList, bool activeRecords)
        {
            log.LogMethodEntry(maintChklstdetIdList);
            List<MaintenanceJobStatusDTO> list = new List<MaintenanceJobStatusDTO>();
            string query = @"SELECT MaintenanceJobStatus.*
                            FROM MaintenanceJobStatus, @maintChklstdetIdList List
                            WHERE MaintChklstdetId = List.id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@maintChklstdetIdList", maintChklstdetIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetMaintenanceJobStatusDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }


        /// <summary>
        /// Gets the MaintenanceJobStatusDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MaintenanceJobStatusDTO matching the search criteria</returns>
        public List<MaintenanceJobStatusDTO> GetMaintenanceJobStatusDTOList(List<KeyValuePair<MaintenanceJobStatusDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<MaintenanceJobStatusDTO> MaintenanceJobStatusDTOList = new List<MaintenanceJobStatusDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            string selectQuery = @"select * from MaintenanceJobStatus";
            if (searchParameters != null)
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MaintenanceJobStatusDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ((searchParameter.Key.Equals(MaintenanceJobStatusDTO.SearchByParameters.JOB_STATUS_ID) ||
                            searchParameter.Key.Equals(MaintenanceJobStatusDTO.SearchByParameters.MAINT_CHKLST_DETAIL_ID) ||
                            searchParameter.Key.Equals(MaintenanceJobStatusDTO.SearchByParameters.MASTER_ENTITY_ID)))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MaintenanceJobStatusDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(MaintenanceJobStatusDTO.SearchByParameters.IS_ACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
                        }
                        else if (searchParameter.Key.Equals(MaintenanceJobStatusDTO.SearchByParameters.JOB_STATUS))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }

            DataTable maintenanceJobStatusDataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (maintenanceJobStatusDataTable.Rows.Count > 0)
            {
                foreach (DataRow maintenanceJobStatusDataRow in maintenanceJobStatusDataTable.Rows)
                {
                    MaintenanceJobStatusDTO MaintenanceJobStatusDataObject = GetMaintenanceJobStatusDTO(maintenanceJobStatusDataRow);
                    MaintenanceJobStatusDTOList.Add(MaintenanceJobStatusDataObject);
                }
            }
            log.LogMethodExit(MaintenanceJobStatusDTOList);
            return MaintenanceJobStatusDTOList;
        }
    }
}

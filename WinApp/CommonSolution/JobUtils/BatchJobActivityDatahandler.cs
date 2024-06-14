/********************************************************************************************
 * Project Name - Batch Job Activity Data Handler
 * Description  - Data handler of the Batch Job Activity class
 * 
 **************
 **Version Logss
 **************
 *Version     Date           Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        24-Jul-2019    Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                          SQL injection Issue Fix
 *2.70.2        09-Dec-2019   Jinto Thomas         Removed siteid from update query                                                          
 *2.100.0       31-Aug-2020   Mushahid Faizan      siteId changes in GetSQLParameters().
 *2.140         14-Sep-2021      Fiona             Modified: Issue fixes 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the BatchJobActivity datahandler object class. This acts as data handler for the BatchJobActivity business object
    /// </summary>
    public class BatchJobActivityDatahandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT a.* FROM BatchJobActivity as a ";

        /// <summary>
        /// Dictionary for searching Parameters for the BatchJobActivity object.
        /// </summary>
        private static readonly Dictionary<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string> DBSearchParameters = new Dictionary<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>
            {
                {BatchJobActivityDTO.SearchByBatchJobActivityParameters.BATCHJOBACTIVITY_ID, "a.BatchJobActivityId"},
                {BatchJobActivityDTO.SearchByBatchJobActivityParameters.MODULE_ID, "a.ModuleId"},
                {BatchJobActivityDTO.SearchByBatchJobActivityParameters.ENTITYNAME, "a.EntityName"},
                {BatchJobActivityDTO.SearchByBatchJobActivityParameters.ENTITYCOLUMN, "a.EntityColumn"},
                {BatchJobActivityDTO.SearchByBatchJobActivityParameters.ACTION_ID, "a.ActionId"},
                {BatchJobActivityDTO.SearchByBatchJobActivityParameters.SITE_ID,"a.site_id"},
                {BatchJobActivityDTO.SearchByBatchJobActivityParameters.MASTER_ENTITY_ID,"a.MasterEntityId"}
            };
        //Utilities utilities;

        /// <summary>
        /// Default constructor of BatchJobActivityDatahandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public BatchJobActivityDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            //utilities = new Utilities();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating BatchJobActivity parameters Record.
        /// </summary>
        /// <param name="batchJobActivityDTO">batchJobActivityDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(BatchJobActivityDTO batchJobActivityDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(batchJobActivityDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@batchJobActivityId", batchJobActivityDTO.BatchJobActivityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@moduleId", batchJobActivityDTO.ModuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@entityName", batchJobActivityDTO.EntityName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@entityColumn", batchJobActivityDTO.EntityColumn));
            parameters.Add(dataAccessHandler.GetSQLParameter("@actionId", batchJobActivityDTO.ActionId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@actionQuery", batchJobActivityDTO.ActionQuery));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", batchJobActivityDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastModUserId", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            return parameters;
        }

        /// <summary>
        /// Inserts the BatchJobActivity record to the database
        /// </summary>
        /// <param name="batchJobActivityDTO">BatchJobActivityDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">sql transaction</param>
        /// <returns>Returns inserted record id</returns>
        public BatchJobActivityDTO InsertBatchJobActivity(BatchJobActivityDTO batchJobActivityDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(batchJobActivityDTO, loginId, siteId, sqlTransaction);
            string InsertBatchJobActivityQuery = @"INSERT INTO [dbo].[BatchJobActivity]  
                                                        (
                                                          ModuleId,
                                                          EntityName,
                                                          EntityColumn,
                                                          ActionId,
                                                          ActionQuery ,
                                                          Guid,
                                                          site_id,
                                                          MasterEntityId,
                                                          LastModUserId,
                                                          LastModDttm,  
                                                          CreatedBy,
                                                          CreationDate
                                                        ) 
                                                values 
                                                        (
                                                          @moduleId,
                                                          @entityName,
                                                          @entityColumn,
                                                          @actionId,
                                                          @actionQuery,
                                                          NewId(),
                                                          @siteId,
                                                          @MasterEntityId,
                                                          @lastModUserId,
                                                          getdate(),
                                                          @createdBy,
                                                          Getdate()
                                                        )SELECT * FROM BatchJobActivity WHERE BatchJobActivityID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(InsertBatchJobActivityQuery, GetSQLParameters(batchJobActivityDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshBatchJobActivityDTO(batchJobActivityDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting batchJobActivity", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(batchJobActivityDTO);
            return batchJobActivityDTO;
        }

        /// <summary>
        /// Updates the BatchJobActivity record
        /// </summary>
        /// <param name="batchJobActivityDTO">BatchJobActivityDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">sql transaction</param>
        /// <returns>Returns the count of updated rows</returns>
        public BatchJobActivityDTO UpdateBatchJobActivity(BatchJobActivityDTO batchJobActivityDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(batchJobActivityDTO, loginId, siteId, sqlTransaction);
            string updateBatchJobActivityQuery = @"update BatchJobActivity 
                                                          set ModuleId=@moduleId,
                                                          EntityName=@entityName,
                                                          EntityColumn=@entityColumn,
                                                          ActionId=@actionId,
                                                          ActionQuery=@actionQuery,
                                                          --site_id=@siteId,                                                          
                                                          MasterEntityId=@MasterEntityId,
                                                          LastModUserId=@lastModUserId,
                                                          LastModDttm=getdate()
                                       where BatchJobActivityId = @batchJobActivityID
                                       SELECT* FROM BatchJobActivity WHERE BatchJobActivityId = @batchJobActivityID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateBatchJobActivityQuery, GetSQLParameters(batchJobActivityDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshBatchJobActivityDTO(batchJobActivityDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating batchJobActivity", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(batchJobActivityDTO);
            return batchJobActivityDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="batchJobActivityDTO">batchJobActivityDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshBatchJobActivityDTO(BatchJobActivityDTO batchJobActivityDTO, DataTable dt)
        {
            log.LogMethodEntry(batchJobActivityDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                batchJobActivityDTO.BatchJobActivityId = Convert.ToInt32(dt.Rows[0]["BatchJobActivityId"]);
                batchJobActivityDTO.LastModDttm = dataRow["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastModDttm"]);
                batchJobActivityDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                batchJobActivityDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                batchJobActivityDTO.LastModUserId = dataRow["LastModUserId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastModUserId"]);
                batchJobActivityDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                batchJobActivityDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to batchJobActivityDTO class type
        /// </summary>
        /// <param name="batchJobActivityDataRow">batchJobActivityDTO DataRow</param>
        /// <returns>Returns batchJobActivityDTO</returns>
        private BatchJobActivityDTO GetBatchJobActivityDTO(DataRow batchJobActivityDataRow)
        {
            log.LogMethodEntry(batchJobActivityDataRow);
            BatchJobActivityDTO BatchJobActivityDataObject = new BatchJobActivityDTO(Convert.ToInt32(batchJobActivityDataRow["BatchJobActivityId"]),
                                                          Convert.ToInt32(batchJobActivityDataRow["ModuleId"]),
                                                          batchJobActivityDataRow["EntityName"].ToString(),
                                                          batchJobActivityDataRow["EntityColumn"].ToString(),
                                                          Convert.ToInt32(batchJobActivityDataRow["ActionId"]),
                                                          batchJobActivityDataRow["ActionQuery"].ToString(),
                                                          batchJobActivityDataRow["Guid"].ToString(),
                                                          batchJobActivityDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(batchJobActivityDataRow["site_id"]),
                                                          batchJobActivityDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(batchJobActivityDataRow["SynchStatus"]),
                                                          batchJobActivityDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(batchJobActivityDataRow["MasterEntityId"]),
                                                          batchJobActivityDataRow["LastModUserId"].ToString(),
                                                          batchJobActivityDataRow["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(batchJobActivityDataRow["LastModDttm"]),
                                                          batchJobActivityDataRow["CreatedBy"].ToString(),
                                                          batchJobActivityDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(batchJobActivityDataRow["CreationDate"])

                                                         );
            log.LogMethodExit(BatchJobActivityDataObject);
            return BatchJobActivityDataObject;
        }

        /// <summary>
        /// Gets the BatchJobActivity data of passed patch asset application id
        /// </summary>
        /// <param name="batchJobActivityID">integer type parameter</param>
        /// <returns>Returns batchJobActivityDTO</returns>
        public BatchJobActivityDTO GetBatchJobActivity(int batchJobActivityID)
        {
            log.LogMethodEntry(batchJobActivityID);
            BatchJobActivityDTO result = null;
            string selectProductQuery =SELECT_QUERY + " where a.BatchJobActivityId = @batchJobActivityId ";
            SqlParameter parameter = new SqlParameter("@batchJobActivityId", batchJobActivityID);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectProductQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetBatchJobActivityDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the BatchJobActivityDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of batchJobActivityDTO matching the search criteria</returns>
        public List<BatchJobActivityDTO> GetAllBatchJobActivityList(List<KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<BatchJobActivityDTO> batchJobActivityDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == BatchJobActivityDTO.SearchByBatchJobActivityParameters.BATCHJOBACTIVITY_ID
                            || searchParameter.Key == BatchJobActivityDTO.SearchByBatchJobActivityParameters.MODULE_ID
                            || searchParameter.Key == BatchJobActivityDTO.SearchByBatchJobActivityParameters.ACTION_ID
                            || searchParameter.Key == BatchJobActivityDTO.SearchByBatchJobActivityParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == BatchJobActivityDTO.SearchByBatchJobActivityParameters.SITE_ID)
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                batchJobActivityDTOList = new List<BatchJobActivityDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    BatchJobActivityDTO batchJobActivityDTO = GetBatchJobActivityDTO(dataRow);
                    batchJobActivityDTOList.Add(batchJobActivityDTO);
                }
            }
            log.LogMethodExit(batchJobActivityDTOList);
            return batchJobActivityDTOList;
        }
    }
}

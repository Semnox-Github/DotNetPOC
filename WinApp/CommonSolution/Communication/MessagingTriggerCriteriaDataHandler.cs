/********************************************************************************************
 * Project Name - Communication
 * Description  - Data Handler object -MessagingTriggerCriteriaDataHandler
 *
 **************
 ** Version Log
  **************
  * Version     Date        Modified By             Remarks
 *********************************************************************************************
 *2.70.2        11-Jun-2019   Girish Kundar           Created
 *2.80        06-Apr-2020   Mushahid Faizan         Modified 3 tier for Rest Api Changes. 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    ///  MessagingTriggerCriteriaDataHandler object for Insert ,Update and Search for  MessagingTriggerCriteria Object
    /// </summary>
    public class MessagingTriggerCriteriaDataHandler
    {
        private static readonly logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MessagingTriggerCriteria  AS mtc";
        /// <summary>
        /// Dictionary for searching Parameters for the MessagingTriggerCriteria object.
        /// </summary>
        private static readonly Dictionary<MessagingTriggerCriteriaDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MessagingTriggerCriteriaDTO.SearchByParameters, string>
        {
            {MessagingTriggerCriteriaDTO.SearchByParameters.ID,"mtc.Id"},
            {MessagingTriggerCriteriaDTO.SearchByParameters.TRIGGER_ID,"mtc.TriggerId"},
            {MessagingTriggerCriteriaDTO.SearchByParameters.TRIGGER_ID_LIST,"mtc.TriggerId"},
            {MessagingTriggerCriteriaDTO.SearchByParameters.APPLICABLE_PRODUCT_ID,"mtc.ApplicableProductId"},
            {MessagingTriggerCriteriaDTO.SearchByParameters.APPLICABLE_REDMP_PROD_ID,"mtc.ApplicableRedemptionProductId"},
            {MessagingTriggerCriteriaDTO.SearchByParameters.EXCLUDE_FLAG,"mtc.ExcludeFlag"},
            {MessagingTriggerCriteriaDTO.SearchByParameters.IS_ACTIVE,"mtc.IsActive"},
            {MessagingTriggerCriteriaDTO.SearchByParameters.SITE_ID,"mtc.site_id"},
            {MessagingTriggerCriteriaDTO.SearchByParameters.MASTER_ENTITY_ID,"mtc.MasterEntityId"}
        };
        /// <summary>
        /// Parameterized Constructor for MessagingTriggerCriteriaDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object </param>
        public MessagingTriggerCriteriaDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MessagingTriggerCriteria Record.
        /// </summary>
        /// <param name="messagingTriggerCriteriaDTO">MessagingTriggerCriteriaDTO object is passed as  Parameter</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MessagingTriggerCriteriaDTO messagingTriggerCriteriaDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingTriggerCriteriaDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", messagingTriggerCriteriaDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TriggerId", messagingTriggerCriteriaDTO.TriggerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApplicableProductId", messagingTriggerCriteriaDTO.ApplicableProductId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApplicableRedemptionProductId", messagingTriggerCriteriaDTO.ApplicableRedemptionProductId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExcludeFlag", messagingTriggerCriteriaDTO.ExcludeFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TriggerEvent", messagingTriggerCriteriaDTO.TriggerEvent));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", messagingTriggerCriteriaDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", messagingTriggerCriteriaDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Converts the Data row object to MessagingTriggerCriteriaDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object </param>
        /// <returns>Returns the MessagingTriggerCriteriaDTO</returns>
        private MessagingTriggerCriteriaDTO GetMessagingTriggerCriteriaDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MessagingTriggerCriteriaDTO messagingTriggerCriteriaDTO = new MessagingTriggerCriteriaDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                         dataRow["TriggerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TriggerId"]),
                                                         dataRow["ApplicableProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ApplicableProductId"]),
                                                         dataRow["ApplicableRedemptionProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ApplicableRedemptionProductId"]),
                                                         dataRow["ExcludeFlag"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ExcludeFlag"]),
                                                         dataRow["TriggerEvent"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TriggerEvent"]),  // To be checked for default message Type
                                                         dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"])
                                                       );
            log.LogMethodExit(messagingTriggerCriteriaDTO);
            return messagingTriggerCriteriaDTO;
        }

        /// <summary>
        /// Gets the MessagingTriggerCriteriaDTO data of passed id 
        /// </summary>
        /// <param name="id">id is passed</param>
        /// <returns>Returns MessagingTriggerCriteriaDTO</returns>
        public MessagingTriggerCriteriaDTO GetMessagingTriggerCriteriaDTO(int id)
        {
            log.LogMethodEntry(id);
            MessagingTriggerCriteriaDTO result = null;
            string query = SELECT_QUERY + @" WHERE mtc.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMessagingTriggerCriteriaDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the MessagingTriggerCriteria Table.
        /// </summary>
        /// <param name="messagingTriggerCriteriaDTO">MessagingTriggerCriteriaDTO object is passed as  Parameter</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        /// <returns>returns the MessagingTriggerCriteriaDTO</returns>
        public MessagingTriggerCriteriaDTO Insert(MessagingTriggerCriteriaDTO messagingTriggerCriteriaDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingTriggerCriteriaDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[MessagingTriggerCriteria]
                           ([TriggerId],
                            [ApplicableProductId],
                            [ApplicableRedemptionProductId],
                            [ExcludeFlag],
                            [LastUpdatedDate],
                            [LastUpdatedBy],
                            [site_id],
                            [Guid],
                            [MasterEntityId],
                            [triggerEvent],
                            [CreatedBy],
                            [IsActive],
                            [CreationDate])
                     VALUES
                           (@TriggerId,
                            @ApplicableProductId,
                            @ApplicableRedemptionProductId,
                            @ExcludeFlag,
                            GETDATE(),
                            @LastUpdatedBy,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @triggerEvent,
                            @CreatedBy,
                            @IsActive,
                            GETDATE())
                            SELECT * FROM MessagingTriggerCriteria WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(messagingTriggerCriteriaDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMessagingTriggerCriteriaDTO(messagingTriggerCriteriaDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting MessagingTriggerCriteriaDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(messagingTriggerCriteriaDTO);
            return messagingTriggerCriteriaDTO;
        }

        /// <summary>
        ///  Updates the record to the MessagingTriggerCriteria Table.
        /// </summary>
        /// <param name="messagingTriggerCriteriaDTO">MessagingTriggerCriteriaDTO object is passed as  Parameter</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        /// <returns>MessagingTriggerCriteriaDTO</returns>
        public MessagingTriggerCriteriaDTO Update(MessagingTriggerCriteriaDTO messagingTriggerCriteriaDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingTriggerCriteriaDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[MessagingTriggerCriteria]
                           SET
                            [TriggerId]                    = @TriggerId,
                            [ApplicableProductId]          = @ApplicableProductId,
                            [ApplicableRedemptionProductId]= @ApplicableRedemptionProductId,
                            [ExcludeFlag]                  = @ExcludeFlag,
                            [LastUpdatedDate]              = GETDATE(),
                            [LastUpdatedBy]                = @LastUpdatedBy,
                           -- [site_id]                      = @site_id,
                            [MasterEntityId]               = @MasterEntityId,
                            [triggerEvent]                 = @triggerEvent,
                            [IsActive]   = @IsActive
                            WHERE Id = @Id
                            SELECT * FROM MessagingTriggerCriteria WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(messagingTriggerCriteriaDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMessagingTriggerCriteriaDTO(messagingTriggerCriteriaDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating MessagingTriggerCriteriaDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(messagingTriggerCriteriaDTO);
            return messagingTriggerCriteriaDTO;
        }


        /// <summary>
        /// Deletes the MessagingTriggerCriteriaDTO record
        /// </summary>
        /// <param name="messagingTriggerCriteriaDTO"></param>
        internal void Delete(MessagingTriggerCriteriaDTO messagingTriggerCriteriaDTO)
        {
            log.LogMethodEntry(messagingTriggerCriteriaDTO);
            string query = @"DELETE  
                             FROM MessagingTriggerCriteria
                             WHERE MessagingTriggerCriteria.Id = @TriggerId";
            SqlParameter parameter = new SqlParameter("@TriggerId", messagingTriggerCriteriaDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            messagingTriggerCriteriaDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="messagingTriggerCriteriaDTO">MessagingTriggerCriteriaDTO object is passed as  Parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="loginId">login Id of user</param>
        /// <param name="siteId">site  Id  of user</param>
        private void RefreshMessagingTriggerCriteriaDTO(MessagingTriggerCriteriaDTO messagingTriggerCriteriaDTO, DataTable dt)
        {
            log.LogMethodEntry(messagingTriggerCriteriaDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                messagingTriggerCriteriaDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                messagingTriggerCriteriaDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                messagingTriggerCriteriaDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                messagingTriggerCriteriaDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                messagingTriggerCriteriaDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                messagingTriggerCriteriaDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                messagingTriggerCriteriaDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of MessagingTriggerCriteriaDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of MessagingTriggerCriteriaDTO</returns>
        public List<MessagingTriggerCriteriaDTO> GetMessagingTriggerCriteriaDTOList(List<KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<MessagingTriggerCriteriaDTO> messagingTriggerCriteriaDTOList = new List<MessagingTriggerCriteriaDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MessagingTriggerCriteriaDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MessagingTriggerCriteriaDTO.SearchByParameters.ID
                            || searchParameter.Key == MessagingTriggerCriteriaDTO.SearchByParameters.TRIGGER_ID
                            || searchParameter.Key == MessagingTriggerCriteriaDTO.SearchByParameters.APPLICABLE_PRODUCT_ID
                            || searchParameter.Key == MessagingTriggerCriteriaDTO.SearchByParameters.APPLICABLE_REDMP_PROD_ID
                            || searchParameter.Key == MessagingTriggerCriteriaDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagingTriggerCriteriaDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == MessagingTriggerCriteriaDTO.SearchByParameters.TRIGGER_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingTriggerCriteriaDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagingTriggerCriteriaDTO.SearchByParameters.EXCLUDE_FLAG) // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MessagingTriggerCriteriaDTO messagingTriggerCriteriaDTO = GetMessagingTriggerCriteriaDTO(dataRow);
                    messagingTriggerCriteriaDTOList.Add(messagingTriggerCriteriaDTO);
                }
            }
            log.LogMethodExit(messagingTriggerCriteriaDTOList);
            return messagingTriggerCriteriaDTOList;
        }
    }
}

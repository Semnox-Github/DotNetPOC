/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler -TaskDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      04-Jun-2019    Girish Kundar           Created 
 *2.80.0    19-Mar-2020    Jinto Thomas            Modified: Added column trxid
 *2.130.0   19-July-2021   Girish Kundar           Modified : VirtualPoints column added part of Arcade changes
 *2.130.2   12-Dec-2021    Deeksha                 Handled additional fields as part of Transfer Entitlement enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TaskDataHandler Data Handler - Handles insert, update and select of  Task object.
    /// </summary>
    public  class TaskDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM tasks AS t";
        /// <summary>
        /// Dictionary for searching Parameters for the task object.
        /// </summary>
        private static readonly Dictionary<TaskDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TaskDTO.SearchByParameters, string>
        {
            { TaskDTO.SearchByParameters.TASK_ID,"t.task_id"},
            { TaskDTO.SearchByParameters.TASK_ID_LIST,"t.task_id"},
            { TaskDTO.SearchByParameters.CARD_ID,"t.card_id"},
            { TaskDTO.SearchByParameters.TASK_TYPE_ID,"t.task_type_id"},
            { TaskDTO.SearchByParameters.TRANSFERRED_TO_CARD_ID,"t.transfer_to_card_id"},
            { TaskDTO.SearchByParameters.USER_ID,"t.user_id"},
            { TaskDTO.SearchByParameters.CONSOLIDATED_CARD_ID1,"t.consolidate_card1"},
            { TaskDTO.SearchByParameters.POS_MACHINE,"t.pos_machine"},
            { TaskDTO.SearchByParameters.TASK_DATE,"t.task_date"},
            { TaskDTO.SearchByParameters.SITE_ID,"t.site_id"},
            { TaskDTO.SearchByParameters.MASTER_ENTITY_ID,"t.MasterEntityId"},
            { TaskDTO.SearchByParameters.TRX_ID,"t.TrxId"}
        };

        /// <summary>
        /// Parameterized Constructor for TaskDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public TaskDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating tasks Record.
        /// </summary>
        /// <param name="taskDTO">TaskDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the List of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(TaskDTO taskDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(taskDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaskId", taskDTO.TaskId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@card_id", taskDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaskTypeId", taskDTO.TaskTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransferToCardId", taskDTO.TransferToCardId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@user_id", taskDTO.UserId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApprovedBy", taskDTO.ApprovedBy,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@attribute1", taskDTO.Attribute1 < 0 ? (int?)null : taskDTO.Attribute1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute2", taskDTO.Attribute2 < 0 ? (int?)null : taskDTO.Attribute2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@bonus", taskDTO.Bonus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@consolidate_card1", taskDTO.ConsolidateCard1,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@consolidate_card2", taskDTO.ConsolidateCard2, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@consolidate_card3", taskDTO.ConsolidateCard3, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@consolidate_card4", taskDTO.ConsolidateCard4, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@consolidate_card5", taskDTO.ConsolidateCard5, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@courtesy", taskDTO.Courtesy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@credits", taskDTO.Credits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pos_machine", taskDTO.POSMachine));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", taskDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", taskDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@task_date", taskDTO.Taskdate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tickets", taskDTO.Tickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tokens_exchanged", taskDTO.TokensExchanged));
            parameters.Add(dataAccessHandler.GetSQLParameter("@value_loaded", taskDTO.ValueLoaded));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", taskDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@credits_exchanged", taskDTO.CreditsExchanged));
            parameters.Add(dataAccessHandler.GetSQLParameter("@trxId", taskDTO.TrxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@virtualPoints", taskDTO.VirtualPoints));
            parameters.Add(dataAccessHandler.GetSQLParameter("@counterItems", taskDTO.CounterItems));
            parameters.Add(dataAccessHandler.GetSQLParameter("@playCredits", taskDTO.PlayCredits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@time", taskDTO.Time));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        ///  Converts the Data row object to TaskDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the TaskDTO</returns>
        private TaskDTO GetTaskDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TaskDTO taskDTO = new TaskDTO(dataRow["task_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["task_id"]),
                                                         dataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["card_id"]),
                                                         dataRow["task_type_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["task_type_id"]),
                                                         dataRow["value_loaded"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["value_loaded"]),
                                                         dataRow["transfer_to_card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["transfer_to_card_id"]),
                                                         dataRow["credits_exchanged"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["credits_exchanged"]),
                                                         dataRow["tokens_exchanged"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["tokens_exchanged"]),
                                                         dataRow["consolidate_card1"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["consolidate_card1"]),
                                                         dataRow["consolidate_card2"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["consolidate_card2"]),
                                                         dataRow["consolidate_card3"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["consolidate_card3"]),
                                                         dataRow["consolidate_card4"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["consolidate_card4"]),
                                                         dataRow["consolidate_card5"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["consolidate_card5"]),
                                                         dataRow["task_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["task_date"]),
                                                         dataRow["user_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["user_id"]),
                                                         dataRow["pos_machine"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["pos_machine"]),
                                                         dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                                         dataRow["ApprovedBy"] == DBNull.Value ?  -1 : Convert.ToInt32(dataRow["ApprovedBy"]),
                                                         dataRow["attribute1"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["attribute1"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["Attribute2"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Attribute2"]),
                                                         dataRow["credits"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["credits"]),
                                                         dataRow["courtesy"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["courtesy"]),
                                                         dataRow["bonus"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["bonus"]),
                                                         dataRow["tickets"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["tickets"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                         dataRow["trxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["trxId"]),
                                                        dataRow["VirtualPoints"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["VirtualPoints"]),
                                                         dataRow["CounterItems"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["CounterItems"]),
                                                         dataRow["PlayCredits"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["PlayCredits"]),
                                                         dataRow["Time"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Time"])
                                                        );
            log.LogMethodExit(taskDTO);
            return taskDTO;
        }

        /// <summary>
        /// Gets the Task data of passed id 
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns TaskDTO</returns>
        public TaskDTO GetTaskDTO(int id)
        {
            log.LogMethodEntry(id);
            TaskDTO result = null;
            string query = SELECT_QUERY + @" WHERE t.task_id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTaskDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the Task record
        /// </summary>
        /// <param name="taskDTO">TaskDTO is passed as parameter</param>
        internal void Delete(TaskDTO taskDTO)
        {
            log.LogMethodEntry(taskDTO);
            string query = @"DELETE  
                             FROM tasks
                             WHERE tasks.task_id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", taskDTO.TaskId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            taskDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the tasks Table.
        /// </summary>
        /// <param name="taskDTO">TaskDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the TaskDTO</returns>
        public TaskDTO Insert(TaskDTO taskDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(taskDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[tasks]
                           (card_id,
                            task_type_id,
                            value_loaded,
                            transfer_to_card_id,
                            credits_exchanged,
                            tokens_exchanged,
                            consolidate_card1,
                            consolidate_card2,
                            consolidate_card3,
                            consolidate_card4,
                            consolidate_card5,
                            task_date,
                            user_id,
                            pos_machine,
                            Remarks,
                            ApprovedBy,
                            attribute1,
                            Guid,
                            site_id,
                            Attribute2,
                            credits,
                            courtesy,
                            bonus,
                            tickets,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate,
                           trxId,
                            VirtualPoints,
                            CounterItems,
                            PlayCredits,
                            Time
                            )
                     VALUES
                           (@card_id,
                            @TaskTypeId,
                            @value_loaded,
                            @TransferToCardId,
                            @credits_exchanged,
                            @tokens_exchanged,
                            @consolidate_card1,
                            @consolidate_card2,
                            @consolidate_card3,
                            @consolidate_card4,
                            @consolidate_card5,
                            @task_date,
                            @user_id,
                            @pos_machine,
                            @Remarks,
                            @ApprovedBy,
                            @attribute1,
                            NEWID(),
                            @site_id,
                            @Attribute2,
                            @credits,
                            @courtesy,
                            @bonus,
                            @tickets,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(),
                            @trxId ,
                            @virtualPoints,
                            @counterItems,
                            @playCredits,
                            @time)
                                    SELECT * FROM tasks WHERE task_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(taskDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTaskDTO(taskDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting tasks", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(taskDTO);
            return taskDTO;
        }

        /// <summary>
        ///  Updates the record to the tasks Table.
        /// </summary>
        /// <param name="taskDTO">TaskDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the TaskDTO</returns>
        public TaskDTO Update(TaskDTO taskDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(taskDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[tasks]
                           SET 
                           card_id              = @card_id,
                           task_type_id         = @TaskTypeId,
                           value_loaded         = @value_loaded,
                           transfer_to_card_id  = @TransferToCardId,
                           credits_exchanged    = @credits_exchanged,
                           tokens_exchanged     = @tokens_exchanged,
                           consolidate_card1    = @consolidate_card1,
                           consolidate_card2    = @consolidate_card2,
                           consolidate_card3    = @consolidate_card3,
                           consolidate_card4    = @consolidate_card4,
                           consolidate_card5    = @consolidate_card5,
                           task_date            = @task_date,
                           user_id              = @user_id,
                           pos_machine          = @pos_machine,
                           Remarks              = @Remarks,
                           ApprovedBy           = @ApprovedBy,
                           attribute1           = @attribute1,
                           Attribute2           = @Attribute2,
                           credits              = @credits,
                           courtesy             = @courtesy,
                           bonus                = @bonus,
                           tickets              = @tickets,
                           MasterEntityId       = @MasterEntityId,
                           LastUpdatedBy        = @LastUpdatedBy,
                           LastUpdateDate       = GETDATE(),
                           trxId                = @trxId,
                            VirtualPoints        = @virtualPoints,
                           CounterItems         = @counterItems,
                           PlayCredits          = @playCredits,
                           Time                 = @time
                           where task_id = @TaskId
                           SELECT * FROM tasks WHERE task_id = @TaskId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(taskDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTaskDTO(taskDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating tasks", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(taskDTO);
            return taskDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="taskDTO">TaskDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
      
        private void RefreshTaskDTO(TaskDTO taskDTO, DataTable dt)
        {
            log.LogMethodEntry(taskDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                taskDTO.TaskId = Convert.ToInt32(dt.Rows[0]["task_id"]);
                taskDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                taskDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                taskDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                taskDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                taskDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                taskDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// 
        /// </summary>
        /// <param name="bonusLoadLimit"></param>
        /// <param name="CardId"></param>
        /// <returns></returns>
        public bool checkBonusLoadLimit(int bonusLoadLimit, int CardId,int BusinessStart)
        {
            log.LogMethodEntry(bonusLoadLimit, CardId, BusinessStart);
            if (bonusLoadLimit > 0)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(dataAccessHandler.GetSQLParameter("@AccountId", CardId, true));
                parameters.Add(dataAccessHandler.GetSQLParameter("@BusinessStart", BusinessStart, true));

                DataTable bonusDT = dataAccessHandler.executeSelectQuery("select * from tasks " +
                                                                    "where task_type_id in (select task_type_id from task_type where task_type in('LOADBONUS','REDEEMTICKETSFORBONUS'))" +
                                                                    "and task_date >=  DATEADD(HOUR, @BusinessStart, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) " +
                                                                    "and task_date < 1 + DATEADD(HOUR, @BusinessStart, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) " +
                                                                    "and card_id = @AccountId", parameters.ToArray());
                if (bonusDT != null && bonusDT.Rows.Count > 0)
                {
                    if (bonusDT.Rows.Count >= Convert.ToInt32(bonusLoadLimit))
                    {
                        log.LogVariableState("bonusDT.Rows.Count" , bonusDT.Rows.Count);
                        log.LogMethodExit(true);
                        return true;
                    }
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// Returns the List of TaskDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of TaskDTO </returns>
        public List<TaskDTO> GetTaskDTOList(List<KeyValuePair<TaskDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TaskDTO> taskDTOList = new List<TaskDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TaskDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TaskDTO.SearchByParameters.TASK_ID
                            || searchParameter.Key == TaskDTO.SearchByParameters.TASK_TYPE_ID
                            || searchParameter.Key == TaskDTO.SearchByParameters.USER_ID
                            || searchParameter.Key == TaskDTO.SearchByParameters.CARD_ID
                            || searchParameter.Key == TaskDTO.SearchByParameters.TRANSFERRED_TO_CARD_ID
                            || searchParameter.Key == TaskDTO.SearchByParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == TaskDTO.SearchByParameters.TRX_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TaskDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TaskDTO.SearchByParameters.TASK_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == TaskDTO.SearchByParameters.POS_MACHINE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TaskDTO.SearchByParameters.TASK_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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
                    TaskDTO taskDTO = GetTaskDTO(dataRow);
                    taskDTOList.Add(taskDTO);
                }
            }
            log.LogMethodExit(taskDTOList);
            return taskDTOList;
        }
    }
}

/********************************************************************************************
 * Project Name - Account Relationship Data Handler
 * Description  - Data handler of the Account Relationship class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       19-Jul-2019    Girish Kundar          Modified :Structure of data Handler - insert /Update methods
 *                                                            Fix for SQL Injection Issue 
 *2.140       14-Sep-2021      Fiona                 Modified: Issue fixes
 *2.140.0     23-June-2021     Prashanth            Modified : GetSQLParameters,InsertAccountRelationship, UpdateAccountRelationship, GetAccountRelationshipDTO to include DailyLimit field
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    ///  AccountRelationship Data Handler - Handles insert, update and select of  AccountRelationship objects
    /// </summary>
    public class AccountRelationshipDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<AccountRelationshipDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountRelationshipDTO.SearchByParameters, string>
            {
                {AccountRelationshipDTO.SearchByParameters.ACCOUNT_RELATIONSHIP_ID, "pcc.Id"},
                {AccountRelationshipDTO.SearchByParameters.ACCOUNT_ID, "pcc.ParentCardId"},
                {AccountRelationshipDTO.SearchByParameters.ACCOUNT_ID_LIST, "pcc.ParentCardId"},
                {AccountRelationshipDTO.SearchByParameters.RELATED_ACCOUNT_ID, "pcc.ChildCardId"},
                {AccountRelationshipDTO.SearchByParameters.RELATED_ACCOUNT_ID_LIST, "pcc.ChildCardId"},
                {AccountRelationshipDTO.SearchByParameters.VALID_ACCOUNTS, "pcc.ValidAccounts"},
                {AccountRelationshipDTO.SearchByParameters.EITHER_ACCOUNT_ID_OR_RELATED_ACCOUNT_ID, "pcc.EitherAccountOrRelatedAccountID"},
                {AccountRelationshipDTO.SearchByParameters.EITHER_ACCOUNT_ID_OR_RELATED_ACCOUNT_ID_LIST, "pcc.EitherAccountOrRelatedAccountIDList"},
                {AccountRelationshipDTO.SearchByParameters.IS_ACTIVE,"pcc.ActiveFlag"},
                {AccountRelationshipDTO.SearchByParameters.MASTER_ENTITY_ID,"pcc.MasterEntityId"},
                {AccountRelationshipDTO.SearchByParameters.SITE_ID, "pcc.site_id"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from ParentChildCards AS pcc ";
        /// <summary>
        /// Default constructor of AccountRelationshipDataHandler class
        /// </summary>
        public AccountRelationshipDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AccountRelationship Record.
        /// </summary>
        /// <param name="accountRelationshipDTO">AccountRelationshipDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AccountRelationshipDTO accountRelationshipDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountRelationshipDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", accountRelationshipDTO.AccountRelationshipId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentCardId", accountRelationshipDTO.AccountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ChildCardId", accountRelationshipDTO.RelatedAccountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", accountRelationshipDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", accountRelationshipDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DailyLimit", accountRelationshipDTO.DailyLimitPercentage));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AccountRelationship record to the database
        /// </summary>
        /// <param name="accountRelationshipDTO">AccountRelationshipDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted AccountRelationship record</returns>
        public AccountRelationshipDTO InsertAccountRelationship(AccountRelationshipDTO accountRelationshipDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountRelationshipDTO, loginId, siteId);
            string query = @"INSERT INTO ParentChildCards 
                                        ( 
                                            ParentCardId,
                                            ChildCardId,
                                            ActiveFlag,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            site_id,
                                            MasterEntityId,
                                            DailyLimitPercentage
                                        ) 
                                VALUES 
                                        (
                                            @ParentCardId,
                                            @ChildCardId,
                                            @ActiveFlag,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId,
                                            @DailyLimit
                                        )
                                        SELECT * FROM ParentChildCards WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountRelationshipDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountRelationshipDTO(accountRelationshipDTO,dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting accountRelationshipDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(accountRelationshipDTO);
            return accountRelationshipDTO;
        }

        /// <summary>
        /// Updates the AccountRelationship record
        /// </summary>
        /// <param name="accountRelationshipDTO">AccountRelationshipDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns updated AccountRelationship record</returns>
        public AccountRelationshipDTO UpdateAccountRelationship(AccountRelationshipDTO accountRelationshipDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountRelationshipDTO, loginId, siteId);
            string query = @"UPDATE ParentChildCards 
                             SET ParentCardId = @ParentCardId,
                                 ChildCardId = @ChildCardId,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdatedDate = GETDATE(),
                                 MasterEntityId = @MasterEntityId,
                                 DailyLimitPercentage = @DailyLimit,
                                 ActiveFlag = @ActiveFlag
                             WHERE Id = @Id
                             SELECT * FROM ParentChildCards WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountRelationshipDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountRelationshipDTO(accountRelationshipDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating accountRelationshipDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(accountRelationshipDTO);
            return accountRelationshipDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="accountRelationshipDTO">AccountRelationshipDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAccountRelationshipDTO(AccountRelationshipDTO accountRelationshipDTO, DataTable dt)
        {
            log.LogMethodEntry(accountRelationshipDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                accountRelationshipDTO.AccountRelationshipId = Convert.ToInt32(dt.Rows[0]["Id"]);
                accountRelationshipDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                accountRelationshipDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                accountRelationshipDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                accountRelationshipDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                accountRelationshipDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                accountRelationshipDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);               
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to AccountRelationshipDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AccountRelationshipDTO</returns>
        private AccountRelationshipDTO GetAccountRelationshipDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountRelationshipDTO accountRelationshipDTO = new AccountRelationshipDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["ParentCardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentCardId"]),
                                            dataRow["ChildCardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ChildCardId"]),
                                            dataRow["ActiveFlag"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["ActiveFlag"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["DailyLimitPercentage"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["DailyLimitPercentage"])
                                            );
            log.LogMethodExit(accountRelationshipDTO);
            return accountRelationshipDTO;
        }

        /// <summary>
        /// Gets the AccountRelationship data of passed AccountRelationship Id
        /// </summary>
        /// <param name="accountRelationshipId">integer type parameter</param>
        /// <returns>Returns AccountRelationshipDTO</returns>
        public AccountRelationshipDTO GetAccountRelationshipDTO(int accountRelationshipId)
        {
            log.LogMethodEntry(accountRelationshipId);
            AccountRelationshipDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE pcc.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", accountRelationshipId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetAccountRelationshipDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the AccountRelationshipDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AccountRelationshipDTO matching the search criteria</returns>
        public List<AccountRelationshipDTO> GetAccountRelationshipDTOList(List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AccountRelationshipDTO> list = null;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;

                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AccountRelationshipDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AccountRelationshipDTO.SearchByParameters.ACCOUNT_RELATIONSHIP_ID ||
                            searchParameter.Key == AccountRelationshipDTO.SearchByParameters.ACCOUNT_ID ||
                            searchParameter.Key == AccountRelationshipDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == AccountRelationshipDTO.SearchByParameters.RELATED_ACCOUNT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountRelationshipDTO.SearchByParameters.ACCOUNT_ID_LIST ||
                            searchParameter.Key == AccountRelationshipDTO.SearchByParameters.RELATED_ACCOUNT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountRelationshipDTO.SearchByParameters.EITHER_ACCOUNT_ID_OR_RELATED_ACCOUNT_ID)
                        {
                            query.Append(joiner + " (ParentCardId= " + dataAccessHandler.GetParameterName(searchParameter.Key) + " OR ChildCardId= " + dataAccessHandler.GetParameterName(searchParameter.Key) + " )");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountRelationshipDTO.SearchByParameters.EITHER_ACCOUNT_ID_OR_RELATED_ACCOUNT_ID_LIST)
                        {
                            query.Append(joiner + " (ParentCardId IN( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") OR ChildCardId IN( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + " ))");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountRelationshipDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " =-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountRelationshipDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key == AccountRelationshipDTO.SearchByParameters.VALID_ACCOUNTS)
                        {
                            if(searchParameter.Value == "Y")
                            {
                                query.Append(joiner + " EXISTS(SELECT 1 FROM Cards WHERE card_id = ParentCardId AND valid_flag = 'Y') AND EXISTS(SELECT 1 FROM Cards WHERE card_id = ChildCardId AND valid_flag = 'Y') ");
                            }
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<AccountRelationshipDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountRelationshipDTO accountRelationshipDTO = GetAccountRelationshipDTO(dataRow);
                    list.Add(accountRelationshipDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// GetAccountRelationshipDTOListByAccountIdList
        /// </summary>
        /// <param name="accountIdList"></param>
        /// <returns></returns>
        public List<AccountRelationshipDTO> GetAccountRelationshipDTOListByAccountIdList(List<int> accountIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(accountIdList);
            List<AccountRelationshipDTO> list = new List<AccountRelationshipDTO>();
            string query = SELECT_QUERY + @" , @accountIdList List WHERE pcc.ParentCardId = List.Id ";
            if (activeChildRecords)
            {
                query = query + @" AND Isnull(pcc.ActiveFlag,'0') = '1' 
                                   AND EXISTS(SELECT 1 FROM Cards WHERE card_id = ParentCardId AND valid_flag = 'Y') 
                                      AND EXISTS(SELECT 1 FROM Cards WHERE card_id = ChildCardId AND valid_flag = 'Y') ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@accountIdList", accountIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetAccountRelationshipDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// GetAccountRelationshipDTOListByRelatedAccountIdList
        /// </summary>
        /// <param name="accountIdList"></param>
        /// <returns></returns>
        public List<AccountRelationshipDTO> GetAccountRelationshipDTOListByRelatedAccountIdList(List<int> accountIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(accountIdList);
            List<AccountRelationshipDTO> list = new List<AccountRelationshipDTO>();
            string query = SELECT_QUERY + @" , @accountIdList List WHERE pcc.ChildCardId = List.Id ";
            if (activeChildRecords)
            {
                query = query + @" AND Isnull(pcc.ActiveFlag,'0') = '1' 
                                   AND EXISTS(SELECT 1 FROM Cards WHERE card_id = ParentCardId AND valid_flag = 'Y') 
                                      AND EXISTS(SELECT 1 FROM Cards WHERE card_id = ChildCardId AND valid_flag = 'Y') ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@accountIdList", accountIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetAccountRelationshipDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

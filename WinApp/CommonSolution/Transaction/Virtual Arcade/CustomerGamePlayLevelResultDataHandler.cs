/********************************************************************************************
 * Project Name - CustomerGamePlayLevelResultDataHandler                                                                          
 * Description  - CustomerGamePlayLevelResultDataHandler class to manipulate game machine result level details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Transaction.VirtualArcade
{
    /// <summary>
    /// CustomerGamePlayLevelResultDataHandler
    /// </summary>
    public class CustomerGamePlayLevelResultDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CustomerGamePlayLevelResults AS cgpr ";
        private static readonly Dictionary<CustomerGamePlayLevelResultDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomerGamePlayLevelResultDTO.SearchByParameters, string>
            {
                {CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID, "cgpr.GameMachineLevelId"},
                {CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID_LIST, "cgpr.GameMachineLevelId"},
                {CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_PLAY_LEVEL_RESULT_ID, "cgpr.CustomerGamePlayLevelResultId"},
                {CustomerGamePlayLevelResultDTO.SearchByParameters.CUSTOMER_ID, "cgpr.CustomerId"},
                {CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_PLAY_ID, "cgpr.GamePlayId"},
                {CustomerGamePlayLevelResultDTO.SearchByParameters.IS_ACTIVE, "cgpr.IsActive"},
                {CustomerGamePlayLevelResultDTO.SearchByParameters.MASTER_ENTITY_ID, "cgpr.MasterEntityId"},
                {CustomerGamePlayLevelResultDTO.SearchByParameters.SITE_ID, "cgpr.site_id"}
            };
        /// <summary>
        /// Default constructor of CustomerGamePlayLevelResultDataHandler class
        /// </summary>
        public CustomerGamePlayLevelResultDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerGamePlayLevelResultDTO Record.
        /// </summary>
        /// <param name="customerGamePlayLevelResultDTO">CustomerGamePlayLevelResultDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(CustomerGamePlayLevelResultDTO customerGamePlayLevelResultDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerGamePlayLevelResultDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameMachineLevelId", customerGamePlayLevelResultDTO.GameMachineLevelId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerGamePlayLevelResultId", customerGamePlayLevelResultDTO.CustomerGamePlayLevelResultId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GamePlayId", customerGamePlayLevelResultDTO.GamePlayId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", customerGamePlayLevelResultDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Score", customerGamePlayLevelResultDTO.Score));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerXP", customerGamePlayLevelResultDTO.CustomerXP));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customerGamePlayLevelResultDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", customerGamePlayLevelResultDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        public CustomerGamePlayLevelResultDTO Insert(CustomerGamePlayLevelResultDTO customerGamePlayLevelResultDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerGamePlayLevelResultDTO, loginId, siteId);
            string insertQuery = @"insert into CustomerGamePlayLevelResults 
                                                        (                                                         
                                                       GamePlayId ,
                                                       GameMachineLevelId,
                                                       CustomerId,
                                                       Score ,
                                                       CustomerXP,
                                                       IsActive ,
                                                       CreatedBy ,
                                                       CreationDate ,
                                                       LastUpdatedBy ,
                                                       LastUpdatedDate ,
                                                       Guid ,
                                                       site_id   ,
                                                       MasterEntityId 
                                                      ) 
                                                values 
                                                        (                                                        
                                                       @GamePlayId ,
                                                       @GameMachineLevelId,
                                                       @CustomerId,
                                                       @Score ,
                                                       @CustomerXP,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          )SELECT  * from CustomerGamePlayLevelResults where CustomerGamePlayLevelResultId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(customerGamePlayLevelResultDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerGamePlayLevelResultDTO(customerGamePlayLevelResultDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CustomerGamePlayLevelResultDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerGamePlayLevelResultDTO);
            return customerGamePlayLevelResultDTO;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="CustomerGamePlayLevelResultDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CustomerGamePlayLevelResultDTO Update(CustomerGamePlayLevelResultDTO CustomerGamePlayLevelResultDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(CustomerGamePlayLevelResultDTO, loginId, siteId);
            string updateQuery = @"update CustomerGamePlayLevelResults 
                                         set 
                                            GamePlayId =  @GamePlayId ,
                                            GameMachineLevelId= @GameMachineLevelId,
                                            CustomerId=  @CustomerId,
                                            Score = @Score ,
                                            CustomerXP = @CustomerXP ,
                                            IsActive =   @IsActive ,
                                            LastUpdatedBy =  @LastUpdatedBy,
                                            LastUpdatedDate =  GetDate(),
                                            MasterEntityId =    @MasterEntityId 
                                               where  CustomerGamePlayLevelResultId =  @CustomerGamePlayLevelResultId  
                                    SELECT  * from CustomerGamePlayLevelResults where CustomerGamePlayLevelResultId = @CustomerGamePlayLevelResultId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(CustomerGamePlayLevelResultDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerGamePlayLevelResultDTO(CustomerGamePlayLevelResultDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CustomerGamePlayLevelResultDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(CustomerGamePlayLevelResultDTO);
            return CustomerGamePlayLevelResultDTO;
        }

        /// <summary>
        /// UpdateCustomerGamePlayLevelResultDTO
        /// </summary>
        /// <param name="customerGamePlayLevelResultDTO"></param>
        /// <returns></returns>
        public CustomerGamePlayLevelResultDTO UpdateCustomerGamePlayLevelResultDTO(CustomerGamePlayLevelResultDTO customerGamePlayLevelResultDTO)
        {
            log.LogMethodEntry(customerGamePlayLevelResultDTO);
            string updateQuery = @"update CustomerGamePlayLevelResults 
                                         set 
                                            GamePlayId =  @GamePlayId ,
                                            GameMachineLevelId= @GameMachineLevelId,
                                            CustomerId=  @CustomerId,
                                            Score = @Score ,
                                            CustomerXP = @CustomerXP,
                                            IsActive =   @IsActive 
                                    where  CustomerGamePlayLevelResultId =  @CustomerGamePlayLevelResultId  
                                    SELECT  * from CustomerGamePlayLevelResults where CustomerGamePlayLevelResultId = @CustomerGamePlayLevelResultId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(customerGamePlayLevelResultDTO, string.Empty, -1).ToArray(), sqlTransaction);
                RefreshCustomerGamePlayLevelResultDTO(customerGamePlayLevelResultDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CustomerGamePlayLevelResultDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerGamePlayLevelResultDTO);
            return customerGamePlayLevelResultDTO;
        }
        private void RefreshCustomerGamePlayLevelResultDTO(CustomerGamePlayLevelResultDTO CustomerGamePlayLevelResultDTO, DataTable dt)
        {
            log.LogMethodEntry(CustomerGamePlayLevelResultDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                CustomerGamePlayLevelResultDTO.CustomerGamePlayLevelResultId = Convert.ToInt32(dt.Rows[0]["CustomerGamePlayLevelResultId"]);
                CustomerGamePlayLevelResultDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                CustomerGamePlayLevelResultDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                CustomerGamePlayLevelResultDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                CustomerGamePlayLevelResultDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                CustomerGamePlayLevelResultDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                CustomerGamePlayLevelResultDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private CustomerGamePlayLevelResultDTO GetCustomerGamePlayLevelResultDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomerGamePlayLevelResultDTO CustomerGamePlayLevelResultDTO = new CustomerGamePlayLevelResultDTO(Convert.ToInt32(dataRow["CustomerGamePlayLevelResultId"]),
                                                    dataRow["GamePlayId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GamePlayId"]),
                                                    dataRow["GameMachineLevelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameMachineLevelId"]),
                                                    dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                                    dataRow["Score"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Score"]),
                                                    dataRow["CustomerXP"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["CustomerXP"]),
                                                    null,
                                                     string.Empty,
                                                     string.Empty,
                                                     string.Empty,
                                                     string.Empty,
                                                     string.Empty,
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                    );
            log.LogMethodExit(CustomerGamePlayLevelResultDTO);
            return CustomerGamePlayLevelResultDTO;
        }

        /// <summary>
        /// Gets the CustomerGamePlayLevelResultDTO data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns CustomerGamePlayLevelResultDTO</returns>
        internal CustomerGamePlayLevelResultDTO GetCustomerGamePlayLevelResultDTO(int id)
        {
            log.LogMethodEntry(id);
            CustomerGamePlayLevelResultDTO customerGamePlayLevelResultDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where cgpr.CustomerGamePlayLevelResultId = @CustomerGamePlayLevelResultId";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@CustomerGamePlayLevelResultId", id);
            DataTable customerGamePlayLevelResultTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters, sqlTransaction);
            if (customerGamePlayLevelResultTable.Rows.Count > 0)
            {
                DataRow customerGamePlayLevelResultRow = customerGamePlayLevelResultTable.Rows[0];
                customerGamePlayLevelResultDTO = GetCustomerGamePlayLevelResultDTO(customerGamePlayLevelResultRow);
            }
            log.LogMethodExit(customerGamePlayLevelResultDTO);
            return customerGamePlayLevelResultDTO;

        }

        internal string GetLoyaltyAttribute(int loyaltyAttributeId)
        {
            log.LogMethodEntry(loyaltyAttributeId);
            string result=null;
            string query= @"SELECT Attribute FROM LoyaltyAttributes AS la WHERE la.LoyaltyAttributeId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", loyaltyAttributeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = Convert.ToString(dataTable.Rows[0]["Attribute"]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetCustomerGamePlayLevelResults
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CustomerGamePlayLevelResultDTO> GetCustomerGamePlayLevelResults(List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                //StringBuilder query = new StringBuilder(" left outer join gamePlayinfo gpi on gpi.gameplay_id = cgpr.GamePlayId   WHERE ");
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomerGamePlayLevelResultDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerGamePlayLevelResultDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(CustomerGamePlayLevelResultDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_PLAY_ID) ||
                                  searchParameter.Key.Equals(CustomerGamePlayLevelResultDTO.SearchByParameters.CUSTOMER_ID) ||
                                  searchParameter.Key.Equals(CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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

                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }
            DataTable data = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (data.Rows.Count > 0)
            {
                customerGamePlayLevelResultDTOList = new List<CustomerGamePlayLevelResultDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    CustomerGamePlayLevelResultDTO customerGamePlayLevelResultDTO = GetCustomerGamePlayLevelResultDTO(dataRow);
                    customerGamePlayLevelResultDTOList.Add(customerGamePlayLevelResultDTO);
                }
            }
            log.LogMethodExit(customerGamePlayLevelResultDTOList);
            return customerGamePlayLevelResultDTOList;
        }
    }
}

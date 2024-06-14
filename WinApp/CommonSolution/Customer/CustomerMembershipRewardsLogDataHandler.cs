/********************************************************************************************
 * Project Name - Customer Membership Rewards Log Data Handler
 * Description  - Data handler of the CustomerMembershipRewards class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       19-Jul-2019    Girish Kundar   Modified :Structure of data Handler - insert /Update methods
 *                                                    Fix for SQL Injection Issue
 *2.70.2       05-Dec-2019   Jinto Thomas            Removed siteid from update query                                                    
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{

    /// <summary>
    ///  CustomerMembershipRewardsLogDataHandler  - Handles insert, update and select of   MembershipRewardsLog
    /// </summary>
    public class CustomerMembershipRewardsLogDataHandler
    {

        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<CustomerMembershipRewardsLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomerMembershipRewardsLogDTO.SearchByParameters, string>
        {
            {CustomerMembershipRewardsLogDTO.SearchByParameters.MEMBERSHIP_REWARDS_LOG_ID, "mrl.MembershipRewardsLogId"},
            {CustomerMembershipRewardsLogDTO.SearchByParameters.CUSTOMER_ID, "mrl.CustomerId"},
            {CustomerMembershipRewardsLogDTO.SearchByParameters.CARD_ID,"mrl.CardId"},
            {CustomerMembershipRewardsLogDTO.SearchByParameters.MEMBERSHIP_ID, "mrl.MembershipId"},
            {CustomerMembershipRewardsLogDTO.SearchByParameters.MEMBERSHIP_REWARDS_ID, "mrl.MembershipRewardsId"},
            {CustomerMembershipRewardsLogDTO.SearchByParameters.MASTERENTITY_ID, "mrl.MasterEntityId"},
            {CustomerMembershipRewardsLogDTO.SearchByParameters.SITE_ID, "mrl.Site_id"}
        };
        private  DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from MembershipRewardsLog AS mrl";
        /// <summary>
        /// Default constructor of CustomerMembershipRewardsLogDataHandler class
        /// </summary>
        public CustomerMembershipRewardsLogDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new  DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MembershipRewardsLog Record.
        /// </summary>
        /// <param name="customerMemberrshipRewardsLogDTO">customerMemberrshipRewardsLogDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerMembershipRewardsLogDTO customerMemberrshipRewardsLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerMemberrshipRewardsLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipRewardsLogId", customerMemberrshipRewardsLogDTO.MembershipRewardsLogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", customerMemberrshipRewardsLogDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipId", customerMemberrshipRewardsLogDTO.MembershipId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipRewardsId", customerMemberrshipRewardsLogDTO.MembershipRewardsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RewardAttributeProductID", customerMemberrshipRewardsLogDTO.RewardAttributeProductID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RewardAttribute", customerMemberrshipRewardsLogDTO.RewardAttribute));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RewardAttributePercent", customerMemberrshipRewardsLogDTO.RewardAttributePercent));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RewardFunction", customerMemberrshipRewardsLogDTO.RewardFunction));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RewardFunctionPeriod", customerMemberrshipRewardsLogDTO.RewardFunctionPeriod));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UnitOfRewardFunctionPeriod", customerMemberrshipRewardsLogDTO.UnitOfRewardFunctionPeriod));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RewardFrequency", customerMemberrshipRewardsLogDTO.RewardFrequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UnitOfRewardFrequency", customerMemberrshipRewardsLogDTO.UnitOfRewardFrequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpireWithMembership", customerMemberrshipRewardsLogDTO.ExpireWithMembership));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", customerMemberrshipRewardsLogDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", customerMemberrshipRewardsLogDTO.TrxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxLineId", customerMemberrshipRewardsLogDTO.TrxLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardCreditPlusId", customerMemberrshipRewardsLogDTO.CardCreditPlusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardDiscountId", customerMemberrshipRewardsLogDTO.CardDiscountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardGameId", customerMemberrshipRewardsLogDTO.CardGameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", customerMemberrshipRewardsLogDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", customerMemberrshipRewardsLogDTO.CreatedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreationDate", customerMemberrshipRewardsLogDTO.CreationDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", customerMemberrshipRewardsLogDTO.LastUpdatedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdateDate", customerMemberrshipRewardsLogDTO.LastUpdatedDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", customerMemberrshipRewardsLogDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Site_id", customerMemberrshipRewardsLogDTO.Site_id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", customerMemberrshipRewardsLogDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customerMemberrshipRewardsLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AppliedDate", customerMemberrshipRewardsLogDTO.AppliedDate));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the MembershipRewardsLog record to the database
        /// </summary>
        /// <param name="customerMemberrshipRewardsLogDTO">CustomerMembershipRewardsLogDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerMembershipRewardsLogDTO</returns>
        public CustomerMembershipRewardsLogDTO InsertMembershipProgression(CustomerMembershipRewardsLogDTO customerMemberrshipRewardsLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerMemberrshipRewardsLogDTO, loginId, siteId);
            string query = @"INSERT INTO MembershipRewardsLog 
                                    ( 
                                         CustomerId,
                                         MembershipId,
                                         MembershipRewardsId,
                                         RewardAttributeProductID,
                                         RewardAttribute,
                                         RewardAttributePercent,
                                         RewardFunction,
                                         RewardFunctionPeriod,
                                         UnitOfRewardFunctionPeriod,
                                         RewardFrequency,
                                         UnitOfRewardFrequency,
                                         ExpireWithMembership,
                                         TrxId,
                                         TrxLineId,
                                         CardId,
                                         CreditPlusId,
                                         CardDiscountId,
                                         CardGameId,
                                         IsActive,
                                         CreatedBy,
                                         CreationDate,
                                         LastUpdatedBy,
                                         LastUpdatedDate,
                                         Guid,
                                         Site_id,
                                         MasterEntityId,
                                         AppliedDate
                                    ) 
                            VALUES 
                                    ( 
                                         @CustomerId,
                                         @MembershipId,
                                         @MembershipRewardsId,
                                         @RewardAttributeProductID,
                                         @RewardAttribute,
                                         @RewardAttributePercent,
                                         @RewardFunction,
                                         @RewardFunctionPeriod,
                                         @UnitOfRewardFunctionPeriod,
                                         @RewardFrequency,
                                         @UnitOfRewardFrequency,
                                         @ExpireWithMembership,
                                         @TrxId,
                                         @TrxLineId,
                                         @CardId,
                                         @CardCreditPlusId,
                                         @CardDiscountId,
                                         @CardGameId,
                                         @IsActive,
                                         @CreatedBy,
                                         GETDATE(),
                                         @LastUpdatedBy,
                                         GETDATE(),
                                         NEWID(),
                                         @Site_id,
                                         @MasterEntityId,
                                         @AppliedDate
                                    ) SELECT * FROM MembershipRewardsLog WHERE MembershipRewardsLogId  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerMemberrshipRewardsLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerMemberrshipRewardsLogDTO(customerMemberrshipRewardsLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customerMemberrshipRewardsLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerMemberrshipRewardsLogDTO);
            return customerMemberrshipRewardsLogDTO;
        }

        /// <summary>
        /// Updates the MembershipRewardsLog record
        /// </summary>
        /// <param name="customerMemberrshipRewardsLogDTO">CustomerMembershipRewardsLogDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerMembershipRewardsLogDTO</returns>
        public CustomerMembershipRewardsLogDTO UpdateMembershipProgression(CustomerMembershipRewardsLogDTO customerMemberrshipRewardsLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerMemberrshipRewardsLogDTO, loginId, siteId);
            string query = @"UPDATE MembershipRewardsLog 
                               SET CustomerId = @CustomerId,
                                   MembershipId = @MembershipId,      
                                   MembershipRewardsId = @MembershipRewardsId, 
                                   RewardAttributeProductID = @RewardAttributeProductID,
                                   RewardAttribute = @RewardAttribute,
                                   RewardAttributePercent = @RewardAttributePercent,
                                   RewardFunction = @RewardFunction,
                                   RewardFunctionPeriod = @RewardFunctionPeriod,
                                   UnitOfRewardFunctionPeriod = @UnitOfRewardFunctionPeriod,
                                   RewardFrequency = @RewardFrequency,
                                   UnitOfRewardFrequency = @UnitOfRewardFrequency,
                                   ExpireWithMembership = @ExpireWithMembership,
                                   TrxId = @TrxId,     
                                   TrxLineId = @TrxLineId,
                                   CardId = @CardId,
                                   CreditPlusId = @CardCreditPlusId,
                                   CardDiscountId = @CardDiscountId, 
                                   CardGameId = @CardGameId,
                                   IsActive = @IsActive,
                                   LastUpdatedBy = @LastUpdatedBy,
                                   LastUpdatedDate = GETDATE(), 
                                   --Site_id = @Site_id,
                                   MasterEntityId = @MasterEntityId,
                                   AppliedDate = @AppliedDate
                             WHERE MembershipRewardsLogId = @MembershipRewardsLogId 
                             SELECT * FROM MembershipRewardsLog WHERE MembershipRewardsLogId  = @MembershipRewardsLogId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerMemberrshipRewardsLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerMemberrshipRewardsLogDTO(customerMemberrshipRewardsLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating customerMemberrshipRewardsLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerMemberrshipRewardsLogDTO);
            return customerMemberrshipRewardsLogDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerMembershipRewardsLogDTO">CustomerMembershipRewardsLogDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustomerMemberrshipRewardsLogDTO(CustomerMembershipRewardsLogDTO customerMembershipRewardsLogDTO, DataTable dt)
        {
            log.LogMethodEntry(customerMembershipRewardsLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerMembershipRewardsLogDTO.MembershipRewardsLogId = Convert.ToInt32(dt.Rows[0]["MembershipRewardsLogId"]);
                customerMembershipRewardsLogDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                customerMembershipRewardsLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerMembershipRewardsLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                customerMembershipRewardsLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerMembershipRewardsLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerMembershipRewardsLogDTO.Site_id = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CustomerMembershipRewardsLogDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CustomerMembershipRewardsLogDTO</returns>
        private CustomerMembershipRewardsLogDTO GetCustomerMembershipRewardsLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomerMembershipRewardsLogDTO customerMemberrshipRewardsLogDTO = new CustomerMembershipRewardsLogDTO(Convert.ToInt32(dataRow["MembershipRewardsLogId"]),
                                            dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                            dataRow["MembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipId"]),
                                            dataRow["MembershipRewardsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipRewardsId"]),
                                            dataRow["RewardAttributeProductID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RewardAttributeProductID"]),
                                            dataRow["RewardAttribute"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RewardAttribute"]),
                                            dataRow["RewardAttributePercent"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["RewardAttributePercent"]),
                                            dataRow["RewardFunction"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RewardFunction"]),
                                            dataRow["RewardFunctionPeriod"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["RewardFunctionPeriod"]),
                                            dataRow["UnitOfRewardFunctionPeriod"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UnitOfRewardFunctionPeriod"]),
                                            dataRow["RewardFrequency"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RewardFrequency"]),
                                            dataRow["UnitOfRewardFrequency"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UnitOfRewardFrequency"]),
                                            dataRow["ExpireWithMembership"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ExpireWithMembership"]),
                                            dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                            dataRow["TrxLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxLineId"]),
                                            dataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardId"]),
                                            dataRow["CreditPlusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CreditPlusId"]),
                                            dataRow["CardDiscountId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardDiscountId"]),
                                            dataRow["CardGameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardGameId"]),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]), 
                                            dataRow["AppliedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["AppliedDate"])
                                            );
            log.LogMethodExit(customerMemberrshipRewardsLogDTO);
            return customerMemberrshipRewardsLogDTO;
        }

        /// <summary>
        /// Gets the MembershipRewardsLog data of passed MembershipRewardsLog Id
        /// </summary>
        /// <param name="membershipRewardsLogId">integer type parameter</param>
        /// <returns>Returns CustomerMembershipRewardsLogDTO</returns>
        public CustomerMembershipRewardsLogDTO GetCustomerMembershipRewardsLogDTO(int membershipRewardsLogId)
        {
            log.LogMethodEntry(membershipRewardsLogId);
            CustomerMembershipRewardsLogDTO returnValue = null;
            string query = SELECT_QUERY + "   WHERE mrl.MembershipRewardsLogId = @MembershipRewardsLogId";
            SqlParameter parameter = new SqlParameter("@MembershipRewardsLogId", membershipRewardsLogId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetCustomerMembershipRewardsLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the CustomerMembershipRewardsLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerMembershipRewardsLogDTO matching the search criteria</returns>
        public List<CustomerMembershipRewardsLogDTO> GetCustomerMembershipRewardsLogDTOList(List<KeyValuePair<CustomerMembershipRewardsLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomerMembershipRewardsLogDTO> list = null;
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CustomerMembershipRewardsLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if ((searchParameter.Key == CustomerMembershipRewardsLogDTO.SearchByParameters.CARD_ID) ||
                            (searchParameter.Key == CustomerMembershipRewardsLogDTO.SearchByParameters.CUSTOMER_ID) ||
                            (searchParameter.Key == CustomerMembershipRewardsLogDTO.SearchByParameters.MEMBERSHIP_ID) ||
                            (searchParameter.Key == CustomerMembershipRewardsLogDTO.SearchByParameters.MEMBERSHIP_REWARDS_ID) ||
                            (searchParameter.Key == CustomerMembershipRewardsLogDTO.SearchByParameters.MASTERENTITY_ID) ||
                            (searchParameter.Key == CustomerMembershipRewardsLogDTO.SearchByParameters.MEMBERSHIP_REWARDS_LOG_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerMembershipRewardsLogDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<CustomerMembershipRewardsLogDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerMembershipRewardsLogDTO customerMemberrshipRewardsLogDTO = GetCustomerMembershipRewardsLogDTO(dataRow);
                    list.Add(customerMemberrshipRewardsLogDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// GetCustomerMembershipRewardsLogsByCustomerIds
        /// </summary>
        /// <param name="custIdList"></param>
        /// <returns></returns>
        public List<CustomerMembershipRewardsLogDTO> GetCustomerMembershipRewardsLogsByCustomerIds(List<int> custIdList)
        {
            log.LogMethodEntry(custIdList);
            List<CustomerMembershipRewardsLogDTO> list = new List<CustomerMembershipRewardsLogDTO>();
            string query = SELECT_QUERY + @" , @custIdList List WHERE mrl.CustomerId = List.Id "; 
            DataTable table = dataAccessHandler.BatchSelect(query, "@custIdList", custIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetCustomerMembershipRewardsLogDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

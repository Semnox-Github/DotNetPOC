/********************************************************************************************
 * Project Name - CardDiscounts Data Handler
 * Description  - Data handler of the CardDiscounts class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        16-Jul-2017   Lakshminarayana     Created 
 *2.80.0      19-Mar-2020   Mathew NInan        Added new field ValidityStatus to track
 *                                              status of entitlements
 *2.110.0     11-Jan-2021   Guru S A            Subscription changes 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.CardCore
{
    /// <summary>
    ///  CardDiscounts Data Handler - Handles insert, update and select of  CardDiscounts objects
    /// </summary>
    public class CardDiscountsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<CardDiscountsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CardDiscountsDTO.SearchByParameters, string>
            {
                {CardDiscountsDTO.SearchByParameters.CARD_DISCOUNT_ID, "CardDiscountId"},
                {CardDiscountsDTO.SearchByParameters.CARD_ID, "card_id"},
                {CardDiscountsDTO.SearchByParameters.DISCOUNT_ID, "discount_id"},
                {CardDiscountsDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN, "expiry_date"},
                {CardDiscountsDTO.SearchByParameters.EXPIRY_DATE_LESS_THAN, "expiry_date"},
                {CardDiscountsDTO.SearchByParameters.IS_ACTIVE, "IsActive"},
                {CardDiscountsDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"},
                {CardDiscountsDTO.SearchByParameters.TRANSACTION_ID,"TransactionId"},
                {CardDiscountsDTO.SearchByParameters.LINE_ID,"LineId"},
                {CardDiscountsDTO.SearchByParameters.TASK_ID,"TaskId"},
                {CardDiscountsDTO.SearchByParameters.SITE_ID, "site_id"},
                {CardDiscountsDTO.SearchByParameters.EXPIREWITHMEMBERSHIP, "ExpireWithMembership"},
                {CardDiscountsDTO.SearchByParameters.MEMBERSHIPREWARDSID, "MembershipRewardsId"},
                {CardDiscountsDTO.SearchByParameters.MEMBERSHIPSID, "MembershipId"},
                {CardDiscountsDTO.SearchByParameters.VALIDITYSTATUS, "ValidityStatus"}
            };
        DataAccessHandler dataAccessHandler;
        SqlTransaction sqlTransaction = null;
        /// <summary>
        /// Default constructor of CardDiscountsDataHandler class
        /// </summary>
        public CardDiscountsDataHandler(SqlTransaction sqlTransaction)
        {
            log.Debug("Starts-CardDiscountsDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.Debug("Ends-CardDiscountsDataHandler() default constructor.");
        }

        private void ParameterHelper(List<SqlParameter> parameters, string parameterName, object value, bool negetiveValueNull = false)
        {
            log.Debug("Starts-ParameterHelper() method.");
            if(parameters != null && !string.IsNullOrEmpty(parameterName))
            {
                if(value is int)
                {
                    if(negetiveValueNull && ((int)value) < 0)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else if(value is string)
                {
                    if(string.IsNullOrEmpty(value as string))
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else
                {
                    if(value == null)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
            }
            log.Debug("Ends-ParameterHelper() Method");
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CardDiscounts Record.
        /// </summary>
        /// <param name="cardDiscountsDTO">CardDiscountsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(CardDiscountsDTO cardDiscountsDTO, string userId, int siteId)
        {
            log.Debug("Starts-BuildSQLParameters(cardDiscountsDTO, userId, siteId) Method.");
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParameterHelper(parameters, "@CardDiscountId", cardDiscountsDTO.CardDiscountId, true);
            ParameterHelper(parameters, "@card_id", cardDiscountsDTO.CardId, true);
            ParameterHelper(parameters, "@discount_id", cardDiscountsDTO.DiscountId, true);
            ParameterHelper(parameters, "@TransactionId", cardDiscountsDTO.TransactionId, true);
            ParameterHelper(parameters, "@LineId", cardDiscountsDTO.LineId, true);
            ParameterHelper(parameters, "@TaskId", cardDiscountsDTO.TaskId, true);
            ParameterHelper(parameters, "@expiry_date", cardDiscountsDTO.ExpiryDate);
            ParameterHelper(parameters, "@last_updated_user", userId);
            ParameterHelper(parameters, "@InternetKey", cardDiscountsDTO.InternetKey);
            ParameterHelper(parameters, "@CardTypeId", cardDiscountsDTO.CardTypeId,true);
            ParameterHelper(parameters, "@IsActive", cardDiscountsDTO.IsActive);
            ParameterHelper(parameters, "@site_id", siteId, true);
            ParameterHelper(parameters, "@MasterEntityId", cardDiscountsDTO.MasterEntityId, true);
            ParameterHelper(parameters, "@ExpireWithMembership", cardDiscountsDTO.ExpireWithMembership);
            ParameterHelper(parameters, "@MembershipId", cardDiscountsDTO.MembershipId, true);
            ParameterHelper(parameters, "@MembershipRewardsId", cardDiscountsDTO.MembershipRewardsId, true);
            ParameterHelper(parameters, "@ValidityStatus", (cardDiscountsDTO.ValidityStatus == CardCoreDTO.CardValidityStatus.Valid ? "Y" : "H"));
            ParameterHelper(parameters, "@SubscriptionBillingScheduleId", cardDiscountsDTO.SubscriptionBillingScheduleId,true);
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", cardDiscountsDTO.CreatedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreationDate", cardDiscountsDTO.CreationDate));
            log.Debug("Ends-BuildSQLParameters(cardDiscountsDTO, userId, siteId) Method.");
            return parameters;
        }

        /// <summary>
        /// Inserts the CardDiscounts record to the database
        /// </summary>
        /// <param name="cardDiscountsDTO">CardDiscountsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertCardDiscounts(CardDiscountsDTO cardDiscountsDTO, string userId, int siteId)
        {
            log.Debug("Starts-InsertCardDiscounts(cardDiscountsDTO, userId, siteId) Method.");
            int idOfRowInserted;
            string query = @"INSERT INTO CardDiscounts 
                                        ( 
                                            card_id,
                                            discount_id,
                                            TransactionId,
                                            LineId,
                                            TaskId,
                                            expiry_date,
                                            CreatedBy,
                                            CreationDate,
                                            last_updated_user,
                                            last_updated_date,
                                            InternetKey,
                                            CardTypeId,
                                            IsActive,
                                            site_id,
                                            MasterEntityId,
                                            SynchStatus,
                                            ExpireWithMembership,
                                            MembershipRewardsId,
                                            MembershipId,
                                            ValidityStatus,
                                            SubscriptionBillingScheduleId
                                        ) 
                                VALUES 
                                        (
                                            @card_id,
                                            @discount_id,
                                            @TransactionId,
                                            @LineId,
                                            @TaskId,
                                            @expiry_date,
                                            @CreatedBy,
                                            GETDATE(),
                                            @last_updated_user,
                                            GetDate(),
                                            @InternetKey,
                                            @CardTypeId,
                                            @IsActive,
                                            @site_id,
                                            @MasterEntityId,
                                            NULL,
                                            @ExpireWithMembership,
                                            @MembershipRewardsId,
                                            @MembershipId,
                                            @ValidityStatus,
                                            @SubscriptionBillingScheduleId
                                        )SELECT CAST(scope_identity() AS int)";


            List<SqlParameter> parameters = BuildSQLParameters(cardDiscountsDTO, userId, siteId);
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, parameters.ToArray(), sqlTransaction);
            }
            catch(Exception ex)
            {
                log.Error(ex);
                log.Error(cardDiscountsDTO.ToString());
                log.Error(query);
                throw ex;
            }

            log.Debug("Ends-InsertCardDiscounts(cardDiscountsDTO, userId, siteId) Method.");
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the CardDiscounts record
        /// </summary>
        /// <param name="cardDiscountsDTO">CardDiscountsDTO type parameter</param>
        /// <param name="userId">User updating the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateCardDiscounts(CardDiscountsDTO cardDiscountsDTO, string userId, int siteId)
        {
            log.Debug("Starts-UpdateCardDiscounts(cardDiscountsDTO, userId, siteId) Method.");
            int rowsUpdated;
            string query = @"UPDATE CardDiscounts 
                             SET card_id=@card_id,
                                 discount_id=@discount_id,
                                 TransactionId=@TransactionId,
                                 LineId=@LineId,
                                 TaskId=@TaskId,
                                 expiry_date=@expiry_date,
                                 last_updated_user=@last_updated_user,
                                 last_updated_date=GetDate(),
                                 InternetKey=@InternetKey,
                                 CardTypeId=@CardTypeId,
                                 IsActive=@IsActive,
                                 MasterEntityId=@MasterEntityId,
                                 SynchStatus=NULL,
                                 ExpireWithMembership = @ExpireWithMembership,
                                 MembershipRewardsId = @MembershipRewardsId,
                                 MembershipId = @MembershipId,
                                 ValidityStatus = @ValidityStatus,
                                 SubscriptionBillingScheduleId = @SubscriptionBillingScheduleId
                             WHERE CardDiscountId = @CardDiscountId";
            List<SqlParameter> parameters = BuildSQLParameters(cardDiscountsDTO, userId, siteId);
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, parameters.ToArray(), sqlTransaction);
            }
            catch(Exception ex)
            {
                log.Error(ex);
                log.Error(cardDiscountsDTO.ToString());
                log.Error(query);
                throw ex;
            }
            log.Debug("Ends-UpdateCardDiscounts(cardDiscountsDTO, userId, siteId) Method.");
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to CardDiscountsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CardDiscountsDTO</returns>
        private CardDiscountsDTO GetCardDiscountsDTO(DataRow dataRow)
        {
            log.Debug("Starts-GetCardDiscountsDTO(dataRow) Method.");
            CardDiscountsDTO cardDiscountsDTO = new CardDiscountsDTO(Convert.ToInt32(dataRow["CardDiscountId"]),
                                            dataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["card_id"]),
                                            dataRow["discount_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["discount_id"]),
                                            dataRow["expiry_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["expiry_date"]),
                                            dataRow["TransactionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionId"]),
                                            dataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LineId"]),
                                            dataRow["TaskId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TaskId"]),
                                            dataRow["last_updated_user"] == DBNull.Value ? "" : Convert.ToString(dataRow["last_updated_user"]),
                                            dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]),
                                            dataRow["InternetKey"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["InternetKey"]),
                                            dataRow["CardTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardTypeId"]),
                                            dataRow["IsActive"] == DBNull.Value ? "Y" : Convert.ToString(dataRow["IsActive"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["ExpireWithMembership"] == DBNull.Value ? "N" : dataRow["ExpireWithMembership"].ToString(),
                                            dataRow["MembershipRewardsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipRewardsId"]),
                                            dataRow["MembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["SubscriptionBillingScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SubscriptionBillingScheduleId"]),
                                            dataRow["ValidityStatus"] == DBNull.Value ? CardCoreDTO.CardValidityStatus.Valid : (dataRow["ValidityStatus"].ToString() == "Y" ? CardCoreDTO.CardValidityStatus.Valid : CardCoreDTO.CardValidityStatus.Hold)
                                            );
            log.Debug("Ends-GetCardDiscountsDTO(dataRow) Method.");
            return cardDiscountsDTO;
        }

        /// <summary>
        /// Gets the CardDiscounts data of passed CardDiscounts Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns CardDiscountsDTO</returns>
        public CardDiscountsDTO GetCardDiscountsDTO(int id)
        {
            log.Debug("Starts-GetCardDiscountsDTO(Id) Method.");
            CardDiscountsDTO returnValue = null;
            string query = @"SELECT *
                            FROM CardDiscounts
                            WHERE CardDiscountId = @CardDiscountId";
            SqlParameter parameter = new SqlParameter("@CardDiscountId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                returnValue = GetCardDiscountsDTO(dataTable.Rows[0]);
                log.Debug("Ends-GetCardDiscountsDTO(id) Method by returnting CardDiscountsDTO.");
            }
            else
            {
                log.Debug("Ends-GetCardDiscountsDTO(id) Method by returnting null.");
            }
            return returnValue;
        }

        /// <summary>
        /// Gets the CardDiscountsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CardDiscountsDTO matching the search criteria</returns>
        public List<CardDiscountsDTO> GetCardDiscountsDTOList(List<KeyValuePair<CardDiscountsDTO.SearchByParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetCardDiscountsDTOList(searchParameters) Method.");
            List<CardDiscountsDTO> list = null;
            int count = 0;
            string selectQuery = @"SELECT * FROM CardDiscounts ";
            if((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach(KeyValuePair<CardDiscountsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if(DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if(searchParameter.Key == CardDiscountsDTO.SearchByParameters.CARD_DISCOUNT_ID ||
                            searchParameter.Key == CardDiscountsDTO.SearchByParameters.DISCOUNT_ID ||
                            searchParameter.Key == CardDiscountsDTO.SearchByParameters.TRANSACTION_ID ||
                            searchParameter.Key == CardDiscountsDTO.SearchByParameters.LINE_ID ||
                            searchParameter.Key == CardDiscountsDTO.SearchByParameters.TASK_ID ||
                            searchParameter.Key == CardDiscountsDTO.SearchByParameters.CARD_ID ||
                            searchParameter.Key == CardDiscountsDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == CardDiscountsDTO.SearchByParameters.MEMBERSHIPSID ||
                            searchParameter.Key == CardDiscountsDTO.SearchByParameters.MEMBERSHIPREWARDSID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if(searchParameter.Key == CardDiscountsDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + " IS NULL OR " + DBSearchParameters[searchParameter.Key] + ">'" + searchParameter.Value + "')");
                        }
                        else if(searchParameter.Key == CardDiscountsDTO.SearchByParameters.EXPIRY_DATE_LESS_THAN)
                        {
                            query.Append(joiner + " ("+ DBSearchParameters[searchParameter.Key] + " IS NULL OR " +DBSearchParameters[searchParameter.Key] + "<'" + searchParameter.Value + "')");
                        }
                        else if(searchParameter.Key == CardDiscountsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == CardDiscountsDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + "'" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == CardDiscountsDTO.SearchByParameters.EXPIREWITHMEMBERSHIP)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = " + "'" + searchParameter.Value + "' ");
                        }
                        else if (searchParameter.Key == CardDiscountsDTO.SearchByParameters.VALIDITYSTATUS)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + "'" + searchParameter.Value + "' ");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetCardDiscountsDTOList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                list = new List<CardDiscountsDTO>();
                foreach(DataRow dataRow in dataTable.Rows)
                {
                    CardDiscountsDTO cardDiscountsDTO = GetCardDiscountsDTO(dataRow);
                    list.Add(cardDiscountsDTO);
                }
                log.Debug("Ends-GetCardDiscountsDTOList(searchParameters) Method by returning list.");
            }
            else
            {
                log.Debug("Ends-GetCardDiscountsDTOList(searchParameters) Method by returning null.");
            }
            return list;
        }
    }
}

/********************************************************************************************
* Project Name - Discounts
* Description  - DataHandler - Discounts
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.170.0     05-Jul-2023      Lakshminarayana     Created
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Discounts
{
    class DiscountsDataHandler
    {
        private Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;
        private readonly DataAccessHandler dataAccessHandler;
        /// <summary>
        /// dBSearchParameters for searching the respective Search fields of ProductGamesDTO 
        /// </summary>
        private static readonly Dictionary<DiscountsDTO.SearchByParameters, string> dBSearchParameters = new Dictionary<DiscountsDTO.SearchByParameters, string>
        {
            {DiscountsDTO.SearchByParameters.DISCOUNT_ID, "d.discount_id"},
            {DiscountsDTO.SearchByParameters.DISCOUNT_ID_LIST, "d.discount_id"},
            {DiscountsDTO.SearchByParameters.DISCOUNT_TYPE, "d.discount_type"},
            {DiscountsDTO.SearchByParameters.DISCOUNT_NAME, "d.discount_name"},
            {DiscountsDTO.SearchByParameters.DISPLAY_IN_POS, "d.display_in_POS"},
            {DiscountsDTO.SearchByParameters.AUTOMATIC_APPLY, "d.automatic_apply"},
            {DiscountsDTO.SearchByParameters.COUPON_MANDATORY, "d.CouponMandatory"},
            {DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "d.active_flag"},
            {DiscountsDTO.SearchByParameters.MASTER_ENTITY_ID,"d.MasterEntityId"},
            {DiscountsDTO.SearchByParameters.MINIMUM_CREDITS_GREATER_THAN,"d.minimum_credits"},
            {DiscountsDTO.SearchByParameters.MINIMUM_SALE_AMOUNT_GREATER_THAN,"d.minimum_sale_amount"},
            {DiscountsDTO.SearchByParameters.DISCOUNTED_GAME_ID,"d.GameId"},
            {DiscountsDTO.SearchByParameters.DISCOUNTED_PRODUCT_ID,"d.ProductId"},
            {DiscountsDTO.SearchByParameters.DISCOUNTED_CATEGORY_ID,"d.CategoryId"},
            {DiscountsDTO.SearchByParameters.SITE_ID, "site_id"}
        };
        private const string SELECT_QUERY = @"SELECT d.*
                                              FROM Discounts AS d";

        private const string SELECT_COUNT_QUERY = @"SELECT COUNT(1) TotalCount
                                                    FROM Discounts AS d";

        private const string INSERT_QUERY = @"INSERT INTO discounts 
                                                        ( 
                                                            discount_name,
                                                            discount_percentage,
                                                            automatic_apply,
                                                            minimum_sale_amount,
                                                            minimum_credits,
                                                            display_in_POS,
                                                            active_flag,
                                                            DiscountCriteriaLines,
                                                            AllowMultipleApplication,
                                                            ApplicationLimit,
                                                            sort_order,
                                                            manager_approval_required,
                                                            last_updated_date,
                                                            last_updated_user,
                                                            InternetKey,
                                                            discount_type,
                                                            site_id,
                                                            CouponMandatory,
                                                            DiscountAmount,
                                                            MasterEntityId,
                                                            RemarksMandatory,
                                                            VariableDiscounts,
                                                            ScheduleId,
                                                            TransactionProfileId,
                                                            CreatedBy,
                                                            CreationDate                                 
                                                        ) 
                                                VALUES 
                                                        (
                                                            @discount_name,
                                                            @discount_percentage,
                                                            @automatic_apply,
                                                            @minimum_sale_amount,
                                                            @minimum_credits,
                                                            @display_in_POS,
                                                            @active_flag,
                                                            @DiscountCriteriaLines,
                                                            @AllowMultipleApplication,
                                                            @ApplicationLimit,
                                                            @sort_order,
                                                            @manager_approval_required,
                                                            GETDATE(),
                                                            @last_updated_user,
                                                            @InternetKey,
                                                            @discount_type,
                                                            @site_id,
                                                            @CouponMandatory,
                                                            @DiscountAmount,
                                                            @MasterEntityId,
                                                            @RemarksMandatory,
                                                            @VariableDiscounts,
                                                            @ScheduleId,
                                                            @TransactionProfileId,
                                                            @createdBy,
                                                            GetDate()
                                                        ) SELECT* from discounts where discount_id = scope_identity()";
        private const string UPDATE_QUERY = @"UPDATE discounts 
                                                SET discount_name=@discount_name,
                                                    discount_percentage=@discount_percentage,
                                                    automatic_apply=@automatic_apply,
                                                    minimum_sale_amount=@minimum_sale_amount,
                                                    minimum_credits=@minimum_credits,
                                                    display_in_POS=@display_in_POS,
                                                    active_flag = @active_flag,
                                                    DiscountCriteriaLines=@DiscountCriteriaLines,
                                                    AllowMultipleApplication=@AllowMultipleApplication,
                                                    ApplicationLimit=@ApplicationLimit,
                                                    sort_order=@sort_order,
                                                    manager_approval_required=@manager_approval_required,
                                                    last_updated_date=GETDATE(),
                                                    last_updated_user=@last_updated_user,
                                                    InternetKey=@InternetKey,
                                                    discount_type=@discount_type,
                                                    CouponMandatory=@CouponMandatory,
                                                    DiscountAmount=@DiscountAmount,
                                                    MasterEntityId=@MasterEntityId,
                                                    RemarksMandatory=@RemarksMandatory,
                                                    VariableDiscounts=@VariableDiscounts,
                                                    ScheduleId=@ScheduleId,
                                                    TransactionProfileId=@TransactionProfileId
                                                    
                                                WHERE discount_id = @discount_id 
                                                SELECT* from discounts where discount_id = @discount_id";
        /// <summary>
        /// Default constructor of DiscountsDataHandler class
        /// </summary>
        public DiscountsDataHandler(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(DiscountsDTO discountsDTO)
        {
            log.LogMethodEntry(discountsDTO);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                dataAccessHandler.GetSQLParameter("@discount_id", discountsDTO.DiscountId, true),
                dataAccessHandler.GetSQLParameter("@discount_name", discountsDTO.DiscountName),
                dataAccessHandler.GetSQLParameter("@discount_percentage", discountsDTO.DiscountPercentage),
                dataAccessHandler.GetSQLParameter("@automatic_apply", discountsDTO.AutomaticApply),
                dataAccessHandler.GetSQLParameter("@minimum_sale_amount", discountsDTO.MinimumSaleAmount),
                dataAccessHandler.GetSQLParameter("@minimum_credits", discountsDTO.MinimumCredits),
                dataAccessHandler.GetSQLParameter("@display_in_POS", discountsDTO.DisplayInPOS),
                dataAccessHandler.GetSQLParameter("@sort_order", discountsDTO.SortOrder),
                dataAccessHandler.GetSQLParameter("@manager_approval_required", discountsDTO.ManagerApprovalRequired),
                dataAccessHandler.GetSQLParameter("@InternetKey", discountsDTO.InternetKey),
                dataAccessHandler.GetSQLParameter("@discount_type", discountsDTO.DiscountType),
                dataAccessHandler.GetSQLParameter("@CouponMandatory", discountsDTO.CouponMandatory),
                dataAccessHandler.GetSQLParameter("@DiscountAmount", discountsDTO.DiscountAmount),
                dataAccessHandler.GetSQLParameter("@RemarksMandatory", discountsDTO.RemarksMandatory),
                dataAccessHandler.GetSQLParameter("@VariableDiscounts", discountsDTO.VariableDiscounts),
                dataAccessHandler.GetSQLParameter("@ScheduleId", discountsDTO.ScheduleId, true),
                dataAccessHandler.GetSQLParameter("@TransactionProfileId", discountsDTO.TransactionProfileId, true),
                dataAccessHandler.GetSQLParameter("@active_flag", discountsDTO.IsActive ? "Y" : "N"),
                dataAccessHandler.GetSQLParameter("@DiscountCriteriaLines", discountsDTO.DiscountCriteriaLines),
                dataAccessHandler.GetSQLParameter("@AllowMultipleApplication", discountsDTO.AllowMultipleApplication),
                dataAccessHandler.GetSQLParameter("@ApplicationLimit", discountsDTO.ApplicationLimit),
                dataAccessHandler.GetSQLParameter("@last_updated_user", executionContext.UserId),
                dataAccessHandler.GetSQLParameter("@CreatedBy", executionContext.UserId),
                dataAccessHandler.GetSQLParameter("@site_id", executionContext.SiteId, true),
                dataAccessHandler.GetSQLParameter("@MasterEntityId", discountsDTO.MasterEntityId, true),
            };
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the Discounts record to the database
        /// </summary>
        /// <param name="discountsDTO">DiscountsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted DiscountsDTO</returns>
        internal DiscountsDTO Save(DiscountsDTO discountsDTO)
        {
            log.LogMethodEntry(discountsDTO);
            string query = discountsDTO.DiscountId <= -1 ? INSERT_QUERY : UPDATE_QUERY;

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(discountsDTO).ToArray(), unitOfWork.SQLTrx);
                DataRow dataRow = dt.Rows[0];
                discountsDTO.DiscountId = Convert.ToInt32(dt.Rows[0]["discount_id"]);
                discountsDTO.LastUpdatedDate = dt.Rows[0]["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["last_updated_date"]);
                discountsDTO.Guid = dt.Rows[0]["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                discountsDTO.LastUpdatedBy = dt.Rows[0]["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["last_updated_user"]);
                discountsDTO.SiteId = dt.Rows[0]["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
                discountsDTO.CreatedBy = dt.Rows[0]["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                discountsDTO.CreationDate = dt.Rows[0]["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(discountsDTO);
            return discountsDTO;
        }

        /// <summary>
        /// Returns the reference count of the discount record.
        /// <param name="id">Discounts Id</param>
        /// </summary>
        /// <returns>Returns reference count</returns>
        public int GetDiscountsReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int referenceCount = 0;
            string query = @"SELECT 
                            (
                            SELECT COUNT(1) 
                            FROM ProductDiscounts
                            WHERE discount_id = @discount_id
                            AND IsActive = 'Y' 
                            )
                            +
                            (
                            SELECT COUNT(1) 
                            FROM CardDiscounts
                            WHERE discount_id = @discount_id 
                            AND IsActive = 'Y' 
                            AND expiry_date > GETDATE()
                            )
                            +   
                            (
                            SELECT COUNT(1) 
                            FROM CardType
                            WHERE discount_id = @discount_id 
                            )
                            +
                            (
                            SELECT COUNT(1) 
                            FROM DiscountCouponsHeader dch
                            WHERE DiscountId = @discount_id
                            AND EXISTS(SELECT dc.CouponSetId 
		                               FROM DiscountCoupons dc
		                               WHERE dc.DiscountCouponHeaderId = dch.Id
                                       AND dc.ExpiryDate > GETDATE()
		                               AND dc.Count > (SELECT COUNT(1) 
						                                FROM DiscountCouponsUsed
							                            WHERE CouponSetId = dc.CouponSetId AND IsActive = 'Y'))
                            ) AS ReferenceCount";
            SqlParameter parameter = new SqlParameter("@discount_id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, unitOfWork.SQLTrx);
            if (dataTable.Rows.Count > 0)
            {
                referenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(referenceCount);
            return referenceCount;
        }

        /// <summary>
        /// Converts the Data row object to DiscountsDTO class type
        /// </summary>
        /// <param name="dataRow">Discounts DataRow</param>
        /// <returns>Returns DiscountsDTO</returns>
        private DiscountsDTO GetDiscountsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DiscountsDTO discountsDTO = new DiscountsDTO(Convert.ToInt32(dataRow["discount_id"]),
                                                                        dataRow["discount_name"] == DBNull.Value ? "" : dataRow["discount_name"].ToString(),
                                                                        dataRow["discount_percentage"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["discount_percentage"]),
                                                                        dataRow["automatic_apply"] == DBNull.Value ? "N" : dataRow["automatic_apply"].ToString(),
                                                                        dataRow["minimum_sale_amount"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["minimum_sale_amount"]),
                                                                        dataRow["minimum_credits"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["minimum_credits"]),
                                                                        dataRow["display_in_POS"] == DBNull.Value ? "" : dataRow["display_in_POS"].ToString(),
                                                                        dataRow["sort_order"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["sort_order"]),
                                                                        dataRow["manager_approval_required"] == DBNull.Value ? "N" : Convert.ToString(dataRow["manager_approval_required"]),
                                                                        dataRow["InternetKey"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["InternetKey"]),
                                                                        dataRow["discount_type"] == DBNull.Value ? "T" : Convert.ToString(dataRow["discount_type"]),
                                                                        dataRow["CouponMandatory"] == DBNull.Value ? "N" : Convert.ToString(dataRow["CouponMandatory"]),
                                                                        dataRow["DiscountAmount"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["DiscountAmount"]),
                                                                        dataRow["RemarksMandatory"] == DBNull.Value ? "N" : Convert.ToString(dataRow["RemarksMandatory"]),
                                                                        dataRow["VariableDiscounts"] == DBNull.Value ? "N" : Convert.ToString(dataRow["VariableDiscounts"]),
                                                                        dataRow["ScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScheduleId"]),
                                                                        dataRow["TransactionProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionProfileId"]),
                                                                        dataRow["active_flag"] == DBNull.Value || Convert.ToString(dataRow["active_flag"]) == "Y",
                                                                        dataRow["DiscountCriteriaLines"] != DBNull.Value && Convert.ToBoolean(dataRow["DiscountCriteriaLines"]),
                                                                        dataRow["AllowMultipleApplication"] != DBNull.Value && Convert.ToBoolean(dataRow["AllowMultipleApplication"]),
                                                                        dataRow["ApplicationLimit"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ApplicationLimit"]),
                                                                        dataRow["last_updated_user"] == DBNull.Value ? "" : Convert.ToString(dataRow["last_updated_user"]),
                                                                        dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]),
                                                                        dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                                        dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                                        dataRow["SynchStatus"] != DBNull.Value && Convert.ToBoolean(dataRow["SynchStatus"]),
                                                                        dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                                                        dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                                        dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])

                                                                        );
            log.LogMethodExit(discountsDTO);
            return discountsDTO;
        }

        /// <summary>
        /// Gets the GetDiscounts data
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns DiscountsDTO</returns>
        internal DiscountsDTO GetDiscounts(int id)
        {
            log.LogMethodEntry(id);
            DiscountsDTO result = null;
            string query = SELECT_QUERY + @" WHERE d.discount_id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, unitOfWork.SQLTrx);
            if (dataTable.Rows.Count > 0)
            {
                result = GetDiscountsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal int GetDiscountsDTOListCount(SearchParameterList<DiscountsDTO.SearchByParameters> searchParameters)
        {
            int result = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_COUNT_QUERY +
                                 GetWhereClause(searchParameters, parameters);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), unitOfWork.SQLTrx);
            result = Convert.ToInt32(dataTable.Rows[0]["TotalCount"]);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of DiscountsDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of DiscountsDTO </returns>
        public List<DiscountsDTO> GetDiscountsDTOList(SearchParameterList<DiscountsDTO.SearchByParameters> searchParameters,
                                                            int pageNumber,
                                                            int pageSize)
        {
            log.LogMethodEntry(searchParameters);
            List<DiscountsDTO> discountsDTOList = new List<DiscountsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY +
                                 GetWhereClause(searchParameters, parameters) +
                                 GetPaginationClause(pageSize, pageNumber);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), unitOfWork.SQLTrx);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DiscountsDTO discountsDTO = GetDiscountsDTO(dataRow);
                    discountsDTOList.Add(discountsDTO);
                }
            }
            log.LogMethodExit(discountsDTOList);
            return discountsDTOList;
        }

        /// <summary>
        /// Gets the DiscountsDTO List for discountsGuidList Value List
        /// </summary>
        /// <param name="discountsGuidList">string list parameter</param>
        /// <returns>Returns List of DiscountsDTO</returns>
        public List<DiscountsDTO> GetDiscountsDTOList(List<string> discountsGuidList)
        {
            log.LogMethodEntry(discountsGuidList);
            List<DiscountsDTO> discountsDTOList = new List<DiscountsDTO>();
            string query = SELECT_QUERY + @" , @DiscountsGuidList List
                                            where d.Guid = List.Value";

            DataTable table = dataAccessHandler.BatchSelect(query, "@DiscountsGuidList", discountsGuidList, null, unitOfWork.SQLTrx);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                discountsDTOList = table.Rows.Cast<DataRow>().Select(x => GetDiscountsDTO(x)).ToList();
            }
            log.LogMethodExit(discountsDTOList);
            return discountsDTOList;
        }

        private string GetWhereClause(List<KeyValuePair<DiscountsDTO.SearchByParameters, string>> searchParameters, List<SqlParameter> parameters)
        {
            log.LogMethodEntry(searchParameters, parameters);
            string joiner = string.Empty;
            int counter = 0;
            string result = string.Empty;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (var searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (dBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == DiscountsDTO.SearchByParameters.DISCOUNT_ID ||
                            searchParameter.Key == DiscountsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + dBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DiscountsDTO.SearchByParameters.DISCOUNT_ID_LIST)
                        {
                            query.Append(joiner + @"(" + dBSearchParameters[searchParameter.Key] + " IN ( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + "))");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == DiscountsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + dBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DiscountsDTO.SearchByParameters.AUTOMATIC_APPLY || searchParameter.Key == DiscountsDTO.SearchByParameters.COUPON_MANDATORY)
                        {
                            query.Append(joiner + "Isnull(" + dBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == DiscountsDTO.SearchByParameters.MINIMUM_CREDITS_GREATER_THAN ||
                            searchParameter.Key == DiscountsDTO.SearchByParameters.MINIMUM_SALE_AMOUNT_GREATER_THAN)
                        {
                            query.Append(joiner + dBSearchParameters[searchParameter.Key] + ">" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToDecimal(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DiscountsDTO.SearchByParameters.DISCOUNTED_PRODUCT_ID)
                        {
                            query.Append(joiner + @"d.discount_type = 'T'
                                                    AND
                                                    (
                                                    EXISTS(SELECT '1' 
                                                            FROM DiscountedProducts 
                                                            WHERE DiscountedProducts.DiscountId = d.discount_id 
                                                            AND DiscountedProducts.ProductId = " + searchParameter.Value + @" 
                                                            AND DiscountedProducts.Discounted = 'Y' 
                                                            AND ISNULL(DiscountedProducts.IsActive,'Y') = 'Y') 
                                                    OR 
                                                    NOT EXISTS(SELECT '1' 
                                                                FROM DiscountedProducts WHERE 
                                                                DiscountedProducts.DiscountId = d.discount_id 
                                                                AND ISNULL(DiscountedProducts.IsActive,'Y') = 'Y')
                                                    )");
                        }
                        else if (searchParameter.Key == DiscountsDTO.SearchByParameters.DISCOUNTED_CATEGORY_ID)
                        {
                            query.Append(joiner + @"d.discount_type = 'T'
                                                    AND
                                                    (
                                                    EXISTS(SELECT '1' 
                                                            FROM DiscountedProducts 
                                                            WHERE DiscountedProducts.DiscountId = d.discount_id 
                                                            AND DiscountedProducts.CategoryId = " + searchParameter.Value + @" 
                                                            AND DiscountedProducts.Discounted = 'Y' 
                                                            AND ISNULL(DiscountedProducts.IsActive,'Y') = 'Y') 
                                                    OR 
                                                    NOT EXISTS(SELECT '1' 
                                                                FROM DiscountedProducts WHERE 
                                                                DiscountedProducts.DiscountId = d.discount_id 
                                                                AND ISNULL(DiscountedProducts.IsActive,'Y') = 'Y')
                                                    )");
                        }
                        else if (searchParameter.Key == DiscountsDTO.SearchByParameters.DISCOUNTED_GAME_ID)
                        {
                            query.Append(joiner + @"(d.discount_type = 'G' OR d.discount_type = 'L') 
                                                    AND 
                                                    (
                                                    EXISTS(SELECT '1' 
                                                            FROM DiscountedGames 
                                                            WHERE DiscountedGames.DiscountId = d.discount_id 
                                                            AND DiscountedGames.GameId = " + searchParameter.Value + @" 
                                                            AND DiscountedGames.Discounted = 'Y' 
                                                            AND ISNULL(DiscountedGames.IsActive,'Y') = 'Y') 
                                                    OR 
                                                    NOT EXISTS(SELECT '1' 
                                                                FROM DiscountedGames WHERE 
                                                                DiscountedGames.DiscountId = d.discount_id 
                                                                AND ISNULL(DiscountedGames.IsActive,'Y') = 'Y')
                                                    )");
                        }
                        else if (searchParameter.Key == DiscountsDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + dBSearchParameters[searchParameter.Key] + ",'Y') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + dBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                result = query.ToString();
            }
            log.LogMethodExit(result);
            return result;
        }

        private string GetPaginationClause(int pageSize, int pageNumber)
        {
            log.LogMethodEntry(pageSize, pageNumber);
            string result = string.Empty;
            if (pageSize > 0)
            {
                result = " ORDER BY discount_id OFFSET " + (pageNumber * pageSize).ToString() + " ROWS FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            log.LogMethodExit(result);
            return result;
        }

        internal DateTime? GetDiscountsModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate
                            FROM (
                            select max(last_updated_date) LastUpdatedDate from discounts WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from DiscountPurchaseCriteria WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from DiscountedProducts WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from DiscountedGames WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select MAX(Case WHEN ISNULL(Schedule_ExclusionDays.LastupdatedDate,Schedule.LastupdatedDate) <= Schedule.LastupdatedDate  THEN Schedule.LastupdatedDate ELSE Schedule_ExclusionDays.LastupdatedDate END) LastUpdatedDate
                            from Schedule 
                            inner join discounts on discounts.ScheduleId = Schedule.ScheduleId and (discounts.site_id = @siteId or @siteId = -1)
                            left outer join Schedule_ExclusionDays on Schedule_ExclusionDays.ScheduleId = Schedule.ScheduleId) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, unitOfWork.SQLTrx);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }

    }

}
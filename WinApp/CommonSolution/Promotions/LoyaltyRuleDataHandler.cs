/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data Handler -LoyaltyRuleDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************************
 *2.70        03-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters() and SQL injection Issue Fix
 *2.70.2      10-Dec-2019   Jinto Thomas        Removed siteid from update query
 *2.80      06-Feb-2020   Girish Kundar       Modified : As per the 3 tier standard 
 *********************************************************************************************************/

using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Globalization;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    ///  LoyaltyRule Data Handler - Handles insert, update and select of  LoyaltyRule objects
    /// </summary>
    public class LoyaltyRuleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM LoyaltyRule AS lr ";

        /// <summary>
        /// Dictionary for searching Parameters for the LoyaltyRule object.
        /// </summary>
        private static readonly Dictionary<LoyaltyRuleDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LoyaltyRuleDTO.SearchByParameters, string>
            {
                {LoyaltyRuleDTO.SearchByParameters.LOYALTY_RULE_ID, "lr.LoyaltyRuleId"},
                {LoyaltyRuleDTO.SearchByParameters.ACTIVE_FLAG, "lr.ActiveFlag"},  //, Pass Y or N
                {LoyaltyRuleDTO.SearchByParameters.VIP_ONLY, "lr.VIPOnly"}, //, Pass Y or N
                {LoyaltyRuleDTO.SearchByParameters.APPLY_IMMEDIATE,"lr.ApplyImmediate"},//, Pass Y or N
                {LoyaltyRuleDTO.SearchByParameters.MEMBERSHIP_ID, "lr.MembershipId"},
                {LoyaltyRuleDTO.SearchByParameters.MASTER_ENTITY_ID, "lr.MasterEntityId"},
                {LoyaltyRuleDTO.SearchByParameters.SITE_ID, "lr.site_id"},
                  {LoyaltyRuleDTO.SearchByParameters.PURCHASE_OR_CONSUMPTION_APPLICABILITY, "lr.PurchaseOrConsumptionApplicability"},
                {LoyaltyRuleDTO.SearchByParameters.EXPIRY_DATE, "lr.ExpiryDate"}
            };


        /// <summary>
        /// Default constructor of LoyaltyRuleDataHandler class
        /// </summary>
        //public LoyaltyRuleDataHandler(SqlTransaction sqlTransaction)
        //{
        //    log.LogMethodEntry(sqlTransaction);
        //    this.sqlTransaction = sqlTransaction;
        //    dataAccessHandler = new DataAccessHandler();
        //    log.LogMethodExit(null);
        //}



        /// <summary>
        /// Parameterized Constructor for LoyaltyRuleDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public LoyaltyRuleDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LoyaltyRule Record.
        /// </summary>
        /// <param name="loyaltyRuleDTO">LoyaltyRuleDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site_id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(LoyaltyRuleDTO loyaltyRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyRuleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyRuleId", loyaltyRuleDTO.LoyaltyRuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", loyaltyRuleDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MinimumUsedCredits", loyaltyRuleDTO.MinimumUsedCredits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MinimumSaleAmount", loyaltyRuleDTO.MinimumSaleAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", loyaltyRuleDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PurchaseOrConsumptionApplicability", loyaltyRuleDTO.PurchaseOrConsumptionApplicability));
            //if (loyaltyRuleDTO.ActiveFlag)
            //    parameters.Add(new SqlParameter("@ActiveFlag", "Y"));
            //else
            //    parameters.Add(new SqlParameter("@ActiveFlag", "N"));
            //if (loyaltyRuleDTO.VIPOnly)
            //    parameters.Add(new SqlParameter("@VIPOnly", "Y"));
            //else
            //    parameters.Add(new SqlParameter("@VIPOnly", "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", (loyaltyRuleDTO.ActiveFlag == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VIPOnly", (loyaltyRuleDTO.VIPOnly == true ? "Y" : "N")));


            parameters.Add(dataAccessHandler.GetSQLParameter("@Instances", loyaltyRuleDTO.Instances));

            if (loyaltyRuleDTO.FirstInstancesOnly)
                parameters.Add(new SqlParameter("@FirstInstancesOnly", "Y"));
            else
                parameters.Add(new SqlParameter("@FirstInstancesOnly", "N"));

            if (loyaltyRuleDTO.OnDifferentDays)
                parameters.Add(new SqlParameter("@OnDifferentDays", "Y"));
            else
                parameters.Add(new SqlParameter("@OnDifferentDays", "N"));

            parameters.Add(dataAccessHandler.GetSQLParameter("@PeriodFrom", loyaltyRuleDTO.PeriodFrom));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PeriodTo", loyaltyRuleDTO.PeriodTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeFrom", loyaltyRuleDTO.TimeFrom));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeTo", loyaltyRuleDTO.TimeTo));

            if (loyaltyRuleDTO.Monday)
                parameters.Add(new SqlParameter("@Monday", "Y"));
            else
                parameters.Add(new SqlParameter("@Monday", "N"));

            if (loyaltyRuleDTO.Tuesday)
                parameters.Add(new SqlParameter("@Tuesday", "Y"));
            else
                parameters.Add(new SqlParameter("@Tuesday", "N"));

            if (loyaltyRuleDTO.Wednesday)
                parameters.Add(new SqlParameter("@Wednesday", "Y"));
            else
                parameters.Add(new SqlParameter("@Wednesday", "N"));

            if (loyaltyRuleDTO.Thursday)
                parameters.Add(new SqlParameter("@Thursday", "Y"));
            else
                parameters.Add(new SqlParameter("@Thursday", "N"));

            if (loyaltyRuleDTO.Friday)
                parameters.Add(new SqlParameter("@Friday", "Y"));
            else
                parameters.Add(new SqlParameter("@Friday", "N"));

            if (loyaltyRuleDTO.Saturday)
                parameters.Add(new SqlParameter("@Saturday", "Y"));
            else
                parameters.Add(new SqlParameter("@Saturday", "N"));

            if (loyaltyRuleDTO.Sunday)
                parameters.Add(new SqlParameter("@Sunday", "Y"));
            else
                parameters.Add(new SqlParameter("@Sunday", "N"));

            parameters.Add(dataAccessHandler.GetSQLParameter("@NumberOfDays", loyaltyRuleDTO.NumberOfDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));

            if (loyaltyRuleDTO.ExcludeNewCardIssue)
                parameters.Add(new SqlParameter("@ExcludeNewCardIssue", "Y"));
            else
                parameters.Add(new SqlParameter("@ExcludeNewCardIssue", "N"));

            // parameters.Add(dataAccessHandler.GetSQLParameter("@CardTypeId", loyaltyRuleDTO.CardTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerCount", loyaltyRuleDTO.CustomerCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerCountType", loyaltyRuleDTO.CustomerCountType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MaximumUsedCredits", loyaltyRuleDTO.MaximumUsedCredits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MaximumSaleAmount", loyaltyRuleDTO.MaximumSaleAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", loyaltyRuleDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RewardCount", loyaltyRuleDTO.RewardCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InternetKey", loyaltyRuleDTO.InternetKey, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DaysAfterFirstPurchase", loyaltyRuleDTO.DaysAfterFirstPurchase));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", loyaltyRuleDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApplyImmediate", loyaltyRuleDTO.ApplyImmediate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipId", loyaltyRuleDTO.MembershipId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));


            log.LogMethodExit(parameters);
            return parameters;
        }

        //For future use. Not tested yet. Test before using these methods
        ///// <summary>
        ///// Inserts the LoyaltyRule record to the database. 
        ///// </summary>
        ///// <param name="loyaltyRuleDTO">LoyaltyRuleDTO type object</param>
        ///// <param name="userId">User inserting the record</param>
        ///// <param name="siteId">Site to which the record belongs</param>
        ///// <returns>Returns inserted record id</returns>
        //public int InsertLoyaltyRule(LoyaltyRuleDTO loyaltyRuleDTO, string userId, int siteId)
        //{
        //    log.LogMethodEntry(loyaltyRuleDTO, userId, siteId);
        //    int idOfRowInserted;
        //    string query = @"INSERT INTO LoyaltyRule 
        //                                ( 
        //                                    Name
        //                                    , MinimumUsedCredits
        //                                    , MinimumSaleAmount
        //                                    , ExpiryDate
        //                                    , PurchaseOrConsumptionApplicability
        //                                    , ActiveFlag
        //                                    , VIPOnly
        //                                    , Instances
        //                                    , FirstInstancesOnly
        //                                    , OnDifferentDays
        //                                    , PeriodFrom
        //                                    , PeriodTo
        //                                    , TimeFrom
        //                                    , TimeTo
        //                                    , Monday
        //                                    , Tuesday
        //                                    , Wednesday
        //                                    , Thursday
        //                                    , Friday
        //                                    , Saturday
        //                                    , Sunday
        //                                    , NumberOfDays
        //                                    , LastUpdatedDate
        //                                    , LastUpdatedBy
        //                                    , ExcludeNewCardIssue
        //                                    , CardTypeId
        //                                    , CustomerCount
        //                                    , CustomerCountType
        //                                    , Guid
        //                                    , site_id
        //                                    , MaximumUsedCredits
        //                                    , MaximumSaleAmount
        //                                   -- , SynchStatus
        //                                    , RewardCount
        //                                    , InternetKey
        //                                    , DaysAfterFirstPurchase
        //                                    , MasterEntityId
        //                                    , ApplyImmediate
        //                                    , MembershipId
        //                                ) 
        //                        VALUES 
        //                                (
        //                                    @Name
        //                                    , @MinimumUsedCredits
        //                                    , @MinimumSaleAmount
        //                                    , @ExpiryDate
        //                                    , @PurchaseOrConsumptionApplicability
        //                                    , @ActiveFlag
        //                                    , @VIPOnly
        //                                    , @Instances
        //                                    , @FirstInstancesOnly
        //                                    , @OnDifferentDays
        //                                    , @PeriodFrom
        //                                    , @PeriodTo
        //                                    , @TimeFrom
        //                                    , @TimeTo
        //                                    , @Monday
        //                                    , @Tuesday
        //                                    , @Wednesday
        //                                    , @Thursday
        //                                    , @Friday
        //                                    , @Saturday
        //                                    , @Sunday
        //                                    , @NumberOfDays
        //                                    , GETDATE(),
        //                                    , @LastUpdatedBy
        //                                    , @ExcludeNewCardIssue
        //                                    , @CardTypeId
        //                                    , @CustomerCount
        //                                    , @CustomerCountType
        //                                    , NEWID()
        //                                    , @SiteId
        //                                    , @MaximumUsedCredits
        //                                    , @MaximumSaleAmount
        //                                    --, @SynchStatus
        //                                    , @RewardCount
        //                                    , @InternetKey
        //                                    , @DaysAfterFirstPurchase
        //                                    , @MasterEntityId
        //                                    , @ApplyImmediate
        //                                    , @MembershipId
        //                                )SELECT CAST(scope_identity() AS int)";
        //    try
        //    {
        //        idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(loyaltyRuleDTO, userId, siteId).ToArray(), sqlTransaction);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("", ex);
        //        log.LogMethodExit(null, "throwing exception");
        //        throw ex;
        //    }
        //    log.LogMethodExit(idOfRowInserted);
        //    return idOfRowInserted;
        //}

        ///// <summary>
        ///// Updates the LoyaltyRule record
        ///// </summary>
        ///// <param name="loyaltyRuleDTO">LoyaltyRuleDTO type parameter</param>
        ///// <param name="userId">User inserting the record</param>
        ///// <param name="siteId">Site to which the record belongs</param>
        ///// <returns>Returns the count of updated rows</returns>
        //public int UpdateLoyaltyRule(LoyaltyRuleDTO loyaltyRuleDTO, string userId, int siteId)
        //{
        //    log.LogMethodEntry(loyaltyRuleDTO, userId, siteId);
        //    int rowsUpdated;
        //    string query = @"UPDATE LoyaltyRule 
        //SET Name = @Name
        //            , MinimumUsedCredits = @MinimumUsedCredits
        //            , MinimumSaleAmount = @MinimumSaleAmount
        //            , ExpiryDate = @ExpiryDate
        //            , PurchaseOrConsumptionApplicability = @PurchaseOrConsumptionApplicability
        //            , ActiveFlag = @ActiveFlag
        //            , VIPOnly = @VIPOnly
        //            , Instances = @Instances
        //            , FirstInstancesOnly = @FirstInstancesOnly
        //            , OnDifferentDays = @OnDifferentDays
        //            , PeriodFrom = @PeriodFrom
        //            , PeriodTo = @PeriodTo
        //            , TimeFrom = @TimeFrom
        //            , TimeTo = @TimeTo
        //            , Monday = @Monday
        //            , Tuesday = @Tuesday
        //            , Wednesday = @Wednesday
        //            , Thursday = @Thursday
        //            , Friday = @Friday
        //            , Saturday = @Saturday
        //            , Sunday = @Sunday
        //            , NumberOfDays = @NumberOfDays
        //            , LastUpdatedDate = GETDATE(),

        //            , LastUpdatedBy = @LastUpdatedBy
        //            , ExcludeNewCardIssue = @ExcludeNewCardIssue
        //            , CardTypeId = @CardTypeId
        //            , CustomerCount = @CustomerCount
        //            , CustomerCountType = @CustomerCountType
        //            , site_id = @SiteId
        //            , MaximumUsedCredits = @MaximumUsedCredits
        //            , MaximumSaleAmount = @MaximumSaleAmount
        //            --, SynchStatus = @SynchStatus
        //            , RewardCount = @RewardCount
        //            , InternetKey = @InternetKey
        //            , DaysAfterFirstPurchase = @DaysAfterFirstPurchase
        //            , MasterEntityId = @MasterEntityId
        //            , ApplyImmediate = @ApplyImmediate
        //            , MembershipId = @MembershipId
        //                     WHERE LoyaltyRuleId = @LoyaltyRuleId";
        //    try
        //    {
        //        rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(loyaltyRuleDTO, userId, siteId).ToArray(), sqlTransaction);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("", ex);
        //        log.LogMethodExit(null, "throwing exception");
        //        throw ex;
        //    }
        //    log.LogMethodExit(rowsUpdated);
        //    return rowsUpdated;
        //}

        /// <summary>
        /// Converts the Data row object to LoyaltyRuleDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns LoyaltyRuleDTO</returns>
        private LoyaltyRuleDTO GetLoyaltyRuleDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LoyaltyRuleDTO loyaltyRuleDTO = new LoyaltyRuleDTO(dataRow["LoyaltyRuleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LoyaltyRuleId"]),
                                            dataRow["Name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name"]),
                                            dataRow["minimumUsedCredits"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["minimumUsedCredits"]),
                                            dataRow["MinimumSaleAmount"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["MinimumSaleAmount"]),
                                            dataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                            dataRow["PurchaseOrConsumptionApplicability"] == DBNull.Value ? "" : dataRow["PurchaseOrConsumptionApplicability"].ToString(),
                                             //dataRow["ActiveFlag"].ToString() == "Y" ? true : false,
                                             //dataRow["VIPOnly"].ToString() == "Y" ? true : false,
                                             dataRow["ActiveFlag"] == DBNull.Value ? true : (dataRow["ActiveFlag"].ToString() == "Y" ? true : false),
                                            dataRow["VIPOnly"] == DBNull.Value ? true : (dataRow["VIPOnly"].ToString() == "Y" ? true : false),
                                             dataRow["Instances"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Instances"]),
                                            dataRow["FirstInstancesOnly"].ToString() == "Y" ? true : false,
                                            dataRow["OnDifferentDays"].ToString() == "Y" ? true : false,
                                            dataRow["PeriodFrom"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["PeriodFrom"]),
                                            dataRow["PeriodTo"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["PeriodTo"]),
                                            dataRow["TimeFrom"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["TimeFrom"]),
                                            dataRow["TimeTo"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["TimeTo"]),
                                            dataRow["Monday"].ToString() == "Y" ? true : false,
                                            dataRow["Tuesday"].ToString() == "Y" ? true : false,
                                            dataRow["Wednesday"].ToString() == "Y" ? true : false,
                                            dataRow["Thursday"].ToString() == "Y" ? true : false,
                                            dataRow["Friday"].ToString() == "Y" ? true : false,
                                            dataRow["Saturday"].ToString() == "Y" ? true : false,
                                            dataRow["Sunday"].ToString() == "Y" ? true : false,
                                            dataRow["NumberOfDays"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["NumberOfDays"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["ExcludeNewCardIssue"].ToString() == "Y" ? true : false,
                                            //dataRow["CardTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardTypeId"]),
                                            dataRow["CustomerCount"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["CustomerCount"]),
                                            dataRow["CustomerCountType"] == DBNull.Value ? "" : Convert.ToString(dataRow["CustomerCountType"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MaximumUsedCredits"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["MaximumUsedCredits"]),
                                            dataRow["MaximumSaleAmount"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["MaximumSaleAmount"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["RewardCount"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["RewardCount"]),
                                            dataRow["InternetKey"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["InternetKey"]),
                                            dataRow["DaysAfterFirstPurchase"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["DaysAfterFirstPurchase"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["ApplyImmediate"] == DBNull.Value ? "N" : dataRow["ApplyImmediate"].ToString(),
                                            dataRow["MembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                            );
            log.LogMethodExit(loyaltyRuleDTO);
            return loyaltyRuleDTO;
        }

        /// <summary>
        /// Gets the LoyaltyRule data of passed LoyaltyRule Id
        /// </summary>
        /// <param name="loyaltyRuleId">integer type parameter</param>
        /// <returns>Returns LoyaltyRuleDTO</returns>
        public LoyaltyRuleDTO GetLoyaltyRuleDTO(int loyaltyRuleId)
        {
            log.LogMethodEntry(loyaltyRuleId);
            LoyaltyRuleDTO returnValue = null;
            string query = SELECT_QUERY + @"WHERE lr.LoyaltyRuleId = @LoyaltyRuleId";
            SqlParameter parameter = new SqlParameter("@LoyaltyRuleId", loyaltyRuleId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetLoyaltyRuleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        ///  Deletes the LoyaltyRule record
        /// </summary>
        /// <param name="loyaltyRuleDTO">LoyaltyRuleDTO is passed as parameter</param>
        internal void Delete(LoyaltyRuleDTO loyaltyRuleDTO)
        {
            log.LogMethodEntry(loyaltyRuleDTO);
            string query = @"DELETE  
                             FROM LoyaltyRule
                             WHERE LoyaltyRule.LoyaltyRuleId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", loyaltyRuleDTO.LoyaltyRuleId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            loyaltyRuleDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the LoyaltyRule Table.
        /// </summary>
        /// <param name="LoyaltyRuleDTO">LoyaltyRuleDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the LoyaltyRuleDTO </returns>
        public LoyaltyRuleDTO Insert(LoyaltyRuleDTO loyaltyRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyRuleDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[LoyaltyRule]
                                           (Name,
                                            MinimumUsedCredits,
                                            MinimumSaleAmount,
                                            ExpiryDate,
                                            PurchaseOrConsumptionApplicability,
                                            ActiveFlag,
                                            VIPOnly,
                                            Instances,
                                            FirstInstancesOnly,
                                            OnDifferentDays,
                                            PeriodFrom,
                                            PeriodTo,
                                            TimeFrom,
                                            TimeTo,
                                            Monday,
                                            Tuesday,
                                            Wednesday,
                                            Thursday,
                                            Friday,
                                            Saturday,
                                            Sunday,
                                            NumberOfDays,
                                            LastUpdatedDate,
                                            LastUpdatedBy,
                                            ExcludeNewCardIssue,
                                           -- CardTypeId,
                                            CustomerCount,
                                            CustomerCountType,
                                            Guid,
                                            site_id,
                                            MaximumUsedCredits,
                                            MaximumSaleAmount,
                                            RewardCount,
                                            InternetKey,
                                            DaysAfterFirstPurchase,
                                            MasterEntityId,
                                            ApplyImmediate,
                                            MembershipId,
                                            CreatedBy,
                                            CreationDate)
                     VALUES
                                           (@Name,
                                            @MinimumUsedCredits,
                                            @MinimumSaleAmount,
                                            @ExpiryDate,
                                            @PurchaseOrConsumptionApplicability,
                                            @ActiveFlag,
                                            @VIPOnly,
                                            @Instances,
                                            @FirstInstancesOnly,
                                            @OnDifferentDays,
                                            @PeriodFrom,
                                            @PeriodTo,
                                            @TimeFrom,
                                            @TimeTo,
                                            @Monday,
                                            @Tuesday,
                                            @Wednesday,
                                            @Thursday,
                                            @Friday,
                                            @Saturday,
                                            @Sunday,
                                            @NumberOfDays,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            @ExcludeNewCardIssue,
                                           -- @CardTypeId,
                                            @CustomerCount,
                                            @CustomerCountType,
                                            NEWID(),
                                            @SiteId,
                                            @MaximumUsedCredits,
                                            @MaximumSaleAmount,
                                            @RewardCount,
                                            @InternetKey,
                                            @DaysAfterFirstPurchase,
                                            @MasterEntityId,
                                            @ApplyImmediate,
                                            @MembershipId,
                                            @CreatedBy,
                                            GETDATE())
                                            SELECT * FROM LoyaltyRule WHERE LoyaltyRuleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyRuleDTO(loyaltyRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LoyaltyRuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyRuleDTO);
            return loyaltyRuleDTO;
        }


        /// <summary>
        ///  Updates the record to the LoyaltyRule Table.
        /// </summary>
        /// <param name="loyaltyRuleDTO">LoyaltyRuleDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the LoyaltyRuleDTO </returns>
        public LoyaltyRuleDTO Update(LoyaltyRuleDTO loyaltyRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyRuleDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[LoyaltyRule]
                           
                                    SET Name = @Name
                                  , MinimumUsedCredits = @MinimumUsedCredits
                                  , MinimumSaleAmount = @MinimumSaleAmount
                                  , ExpiryDate = @ExpiryDate
                                  , PurchaseOrConsumptionApplicability = @PurchaseOrConsumptionApplicability
                                  , ActiveFlag = @ActiveFlag
                                  , VIPOnly = @VIPOnly
                                  , Instances = @Instances
                                  , FirstInstancesOnly = @FirstInstancesOnly
                                  , OnDifferentDays = @OnDifferentDays
                                  , PeriodFrom = @PeriodFrom
                                  , PeriodTo = @PeriodTo
                                  , TimeFrom = @TimeFrom
                                  , TimeTo = @TimeTo
                                  , Monday = @Monday
                                  , Tuesday = @Tuesday
                                  , Wednesday = @Wednesday
                                  , Thursday = @Thursday
                                  , Friday = @Friday
                                  , Saturday = @Saturday
                                  , Sunday = @Sunday
                                  , NumberOfDays = @NumberOfDays
                                  , LastUpdatedDate = GETDATE()
                                  , LastUpdatedBy = @LastUpdatedBy
                                  , ExcludeNewCardIssue = @ExcludeNewCardIssue
                                  --, CardTypeId = @CardTypeId
                                  , CustomerCount = @CustomerCount
                                  , CustomerCountType = @CustomerCountType
                                  -- , site_id = @SiteId
                                  , MaximumUsedCredits = @MaximumUsedCredits
                                  , MaximumSaleAmount = @MaximumSaleAmount
                                  , RewardCount = @RewardCount
                                  , InternetKey = @InternetKey
                                  , DaysAfterFirstPurchase = @DaysAfterFirstPurchase
                                  , MasterEntityId = @MasterEntityId
                                  , ApplyImmediate = @ApplyImmediate
                                  , MembershipId = @MembershipId
                                  WHERE LoyaltyRuleId  = @LoyaltyRuleId
                                  SELECT * FROM LoyaltyRule WHERE LoyaltyRuleId = @LoyaltyRuleId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyRuleDTO(loyaltyRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LoyaltyRuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyRuleDTO);
            return loyaltyRuleDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="loyaltyRuleDTO">LoyaltyRuleDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        private void RefreshLoyaltyRuleDTO(LoyaltyRuleDTO loyaltyRuleDTO, DataTable dt)
        {
            log.LogMethodEntry(loyaltyRuleDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                loyaltyRuleDTO.LoyaltyRuleId = Convert.ToInt32(dt.Rows[0]["LoyaltyRuleId"]);
                loyaltyRuleDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                loyaltyRuleDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                loyaltyRuleDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                loyaltyRuleDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                loyaltyRuleDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                loyaltyRuleDTO.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the List of LoyaltyRuleDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of LoyaltyRuleDTO</returns>
        public List<LoyaltyRuleDTO> GetLoyaltyRuleDTOList(List<KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<LoyaltyRuleDTO> loyaltyRuleDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LoyaltyRuleDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LoyaltyRuleDTO.SearchByParameters.LOYALTY_RULE_ID
                            || searchParameter.Key == LoyaltyRuleDTO.SearchByParameters.MEMBERSHIP_ID
                            || searchParameter.Key == LoyaltyRuleDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyRuleDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyRuleDTO.SearchByParameters.ACTIVE_FLAG) // char
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == LoyaltyRuleDTO.SearchByParameters.PURCHASE_OR_CONSUMPTION_APPLICABILITY)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == LoyaltyRuleDTO.SearchByParameters.EXPIRY_DATE)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + " IS NULL OR " + DBSearchParameters[searchParameter.Key] + ">=" + "cast(" + searchParameter.Value + "as date)" + ")");
                        }
                        else if (searchParameter.Key == LoyaltyRuleDTO.SearchByParameters.VIP_ONLY) //char
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == LoyaltyRuleDTO.SearchByParameters.APPLY_IMMEDIATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                loyaltyRuleDTOList = new List<LoyaltyRuleDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LoyaltyRuleDTO loyaltyRuleDTO = GetLoyaltyRuleDTO(dataRow);
                    loyaltyRuleDTOList.Add(loyaltyRuleDTO);
                }
            }
            log.LogMethodExit(loyaltyRuleDTOList);
            return loyaltyRuleDTOList;
        }
    }
}

/********************************************************************************************
 * Project Name - SubscriptionHeader Data Handler
 * Description  - Data handler of the SubscriptionHeader 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     09-Dec-2020    Guru S A           Created for Subscription changes                                                                               
 *2.120.0     18-Mar-2021    Guru S A           For Subscription phase 2 changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// SubscriptionHeaderDataHandler
    /// </summary>
    public class SubscriptionHeaderDataHandler
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<SubscriptionHeaderDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<SubscriptionHeaderDTO.SearchByParameters, string>
            {
                {SubscriptionHeaderDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, "sh.SubscriptionHeaderId"},
                {SubscriptionHeaderDTO.SearchByParameters.TRANSACTION_ID, "sh.TransactionId"},
                {SubscriptionHeaderDTO.SearchByParameters.TRANSACTION_LINE_ID, "sh.TransactionLineId"},
                {SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_ID, "sh.CustomerId"},
                {SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_CONTACT_ID, "sh.CustomerContactId"},
                {SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_CREDIT_CARD_ID, "sh.CustomerCreditCardsID"},
                {SubscriptionHeaderDTO.SearchByParameters.PRODUCTS_ID, "sh.ProductsId"},
                {SubscriptionHeaderDTO.SearchByParameters.PRODUCT_SUBSCRIPTION_ID, "sh.ProductSubscriptionId"},
                {SubscriptionHeaderDTO.SearchByParameters.PRODUCT_SUBSCRIPTION_NAME, "sh.ProductSubscriptionName"},
                {SubscriptionHeaderDTO.SearchByParameters.SEASONAL_SUBSCRIPTION, ""},
                {SubscriptionHeaderDTO.SearchByParameters.SELECTED_PAYMENT_COLLECTION_MODE, "sh.SelectedPaymentCollectionMode"},
                {SubscriptionHeaderDTO.SearchByParameters.AUTO_RENEW, "sh.AutoRenew"},
                {SubscriptionHeaderDTO.SearchByParameters.TRX_STATUS, "th.Status"},
                {SubscriptionHeaderDTO.SearchByParameters.STATUS, "sh.Status"},
                {SubscriptionHeaderDTO.SearchByParameters.IS_ACTIVE, "sh.IsActive"},
                {SubscriptionHeaderDTO.SearchByParameters.MASTER_ENTITY_ID,"sh.MasterEntityId"},
                {SubscriptionHeaderDTO.SearchByParameters.SITE_ID, "sh.site_id"},
                {SubscriptionHeaderDTO.SearchByParameters.HAS_PAST_PENDING_BILL_CYCLES, ""},
                {SubscriptionHeaderDTO.SearchByParameters.NOT_REACHED_PAYMENT_RETRY_LIMIT, ""},
                {SubscriptionHeaderDTO.SearchByParameters.REACHED_PAYMENT_RETRY_LIMIT, ""},
                {SubscriptionHeaderDTO.SearchByParameters.HAS_UNBILLED_CYCLES, ""},
                {SubscriptionHeaderDTO.SearchByParameters.RENEWAL_REMINDER_IN_X_DAYS_IS_TRUE, ""},
                {SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_FIRST_NAME_LIKE, "pf.FirstName" },
                {SubscriptionHeaderDTO.SearchByParameters.LATEST_BILL_CYCLE_HAS_PAYMENT_ERROR, ""},
                {SubscriptionHeaderDTO.SearchByParameters.HAS_EXPIRED_CREDIT_CARD, ""},
                {SubscriptionHeaderDTO.SearchByParameters.CREDIT_CARD_EXPIRES_BEFORE_NEXT_BILLING, ""},
                {SubscriptionHeaderDTO.SearchByParameters.SUBSCRIPTION_EXPIRES_IN_XDAYS, "" },
                {SubscriptionHeaderDTO.SearchByParameters.CREATION_DATE_LESS_THAN, "sh.CreationDate" },
                {SubscriptionHeaderDTO.SearchByParameters.CREATION_DATE_GREATER_EQUAL_TO, "sh.CreationDate" },
                {SubscriptionHeaderDTO.SearchByParameters.CANCELLATION_DATE_LESS_THAN, "" },
                {SubscriptionHeaderDTO.SearchByParameters.CANCELLATION_DATE_GREATER_EQUAL_TO, "" },
                {SubscriptionHeaderDTO.SearchByParameters.SUBSCRIPTION_IS_EXPIRED, "" },
                {SubscriptionHeaderDTO.SearchByParameters.RENEWED, "" },
                {SubscriptionHeaderDTO.SearchByParameters.SOURCE_SUBSCRIPTION_HEADER_ID, "sh.SourceSubscriptionHeaderId"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * from SubscriptionHeader AS sh ";
        /// <summary>
        /// Default constructor of SubscriptionHeaderDataHandler class
        /// </summary>
        public SubscriptionHeaderDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
         
        private List<SqlParameter> BuildSQLParameters(SubscriptionHeaderDTO subscriptionHeaderDTO, string userId, int siteId)
        {
            log.LogMethodEntry(subscriptionHeaderDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionHeaderId", subscriptionHeaderDTO.SubscriptionHeaderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionId", subscriptionHeaderDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionLineId", subscriptionHeaderDTO.TransactionLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", subscriptionHeaderDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerContactId", subscriptionHeaderDTO.CustomerContactId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerCreditCardsId", subscriptionHeaderDTO.CustomerCreditCardsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductsId", subscriptionHeaderDTO.ProductsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductSubscriptionId", subscriptionHeaderDTO.ProductSubscriptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductSubscriptionName", subscriptionHeaderDTO.ProductSubscriptionName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductSubscriptionDescription", subscriptionHeaderDTO.ProductSubscriptionDescription));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionPrice", subscriptionHeaderDTO.SubscriptionPrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaxInclusivePrice", subscriptionHeaderDTO.TaxInclusivePrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionCycle", subscriptionHeaderDTO.SubscriptionCycle));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UnitOfSubscriptionCycle", subscriptionHeaderDTO.UnitOfSubscriptionCycle));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionCycleValidity", subscriptionHeaderDTO.SubscriptionCycleValidity));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@SeasonalSubscription", subscriptionHeaderDTO.SeasonalSubscription));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SeasonStartDate", subscriptionHeaderDTO.SeasonStartDate));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@SeasonEndDate", subscriptionHeaderDTO.SeasonEndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FreeTrialPeriodCycle", subscriptionHeaderDTO.FreeTrialPeriodCycle));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AllowPause", subscriptionHeaderDTO.AllowPause));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BillInAdvance", subscriptionHeaderDTO.BillInAdvance));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionPaymentCollectionMode", subscriptionHeaderDTO.SubscriptionPaymentCollectionMode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SelectedPaymentCollectionMode", subscriptionHeaderDTO.SelectedPaymentCollectionMode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AutoRenew", subscriptionHeaderDTO.AutoRenew));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AutoRenewalMarkupPercent", subscriptionHeaderDTO.AutoRenewalMarkupPercent));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RenewalGracePeriodCycle", subscriptionHeaderDTO.RenewalGracePeriodCycle));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NoOfRenewalReminders", subscriptionHeaderDTO.NoOfRenewalReminders));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReminderFrequencyInDays", subscriptionHeaderDTO.ReminderFrequencyInDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SendFirstReminderBeforeXDays", subscriptionHeaderDTO.SendFirstReminderBeforeXDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastRenewalReminderSentOn", subscriptionHeaderDTO.LastRenewalReminderSentOn));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RenewalReminderCount", subscriptionHeaderDTO.RenewalReminderCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastPaymentRetryLimitReminderSentOn", subscriptionHeaderDTO.LastPaymentRetryLimitReminderSentOn));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentRetryLimitReminderCount", subscriptionHeaderDTO.PaymentRetryLimitReminderCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SourceSubscriptionHeaderId", subscriptionHeaderDTO.SourceSubscriptionHeaderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionStartDate", subscriptionHeaderDTO.SubscriptionStartDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionEndDate", subscriptionHeaderDTO.SubscriptionEndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PausedBy", subscriptionHeaderDTO.PausedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UnPausedBy", subscriptionHeaderDTO.UnPausedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PauseApprovedBy", subscriptionHeaderDTO.PauseApprovedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UnPauseApprovedBy", subscriptionHeaderDTO.UnPauseApprovedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CancellationOption", subscriptionHeaderDTO.CancellationOption));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CancelledBy", subscriptionHeaderDTO.CancelledBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CancellationApprovedBy", subscriptionHeaderDTO.CancellationApprovedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Status", subscriptionHeaderDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", subscriptionHeaderDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", subscriptionHeaderDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SubscriptionNumber", subscriptionHeaderDTO.SubscriptionNumber));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the SubscriptionHeader record to the database
        /// </summary>
        /// <param name="subscriptionHeaderDTO">SubscriptionHeaderDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted SubscriptionHeader record</returns>
        public SubscriptionHeaderDTO InsertSubscriptionHeader(SubscriptionHeaderDTO subscriptionHeaderDTO, string userId, int siteId)
        {
            log.LogMethodEntry(subscriptionHeaderDTO, userId, siteId);
            string query = @"INSERT INTO SubscriptionHeader 
                                        (  
	                                        TransactionId,
	                                        TransactionLineId,
	                                        CustomerId,
	                                        CustomerContactId,
	                                        CustomerCreditCardsID,
                                            ProductsId,
	                                        ProductSubscriptionId,
	                                        ProductSubscriptionName,
	                                        ProductSubscriptionDescription,
	                                        SubscriptionPrice,
                                            TaxInclusivePrice,
	                                        SubscriptionCycle,
	                                        UnitOfSubscriptionCycle,
	                                        SubscriptionCycleValidity,
	                                        --SeasonalSubscription,
	                                        SeasonStartDate,
	                                        --SeasonEndDate,
                                            FreeTrialPeriodCycle,
	                                        AllowPause,
	                                        BillInAdvance,
	                                        SubscriptionPaymentCollectionMode,
	                                        SelectedPaymentCollectionMode,
	                                        AutoRenew,
	                                        AutoRenewalMarkupPercent, 
	                                        RenewalGracePeriodCycle,
	                                        NoOfRenewalReminders,
	                                        ReminderFrequencyInDays,
	                                        SendFirstReminderBeforeXDays,
                                            LastRenewalReminderSentOn,
	                                        RenewalReminderCount, 
	                                        LastPaymentRetryLimitReminderSentOn,
	                                        PaymentRetryLimitReminderCount,
                                            SourceSubscriptionHeaderId, 
                                            SubscriptionStartDate, 
                                            SubscriptionEndDate,
                                            PausedBy, 
                                            PauseApprovedBy, 
                                            UnPausedBy, 
                                            UnPauseApprovedBy, 
                                            CancellationOption, 
                                            CancelledBy, 
                                            CancellationApprovedBy, 
	                                        Status, 
	                                        IsActive,
	                                        CreatedBy,
	                                        CreationDate,
	                                        LastUpdatedBy,
	                                        LastUpdatedDate,
	                                        Guid, 
	                                        site_id,
	                                        MasterEntityId,
                                            SubscriptionNumber
                                        ) 
                                VALUES 
                                        (
                                           @TransactionId,
	                                       @TransactionLineId,
	                                       @CustomerId,
	                                       @CustomerContactId,
	                                       @CustomerCreditCardsID,
                                           @ProductsId,
	                                       @ProductSubscriptionId,
	                                       @ProductSubscriptionName,
	                                       @ProductSubscriptionDescription,
	                                       @SubscriptionPrice,
                                           @TaxInclusivePrice,
	                                       @SubscriptionCycle,
	                                       @UnitOfSubscriptionCycle,
	                                       @SubscriptionCycleValidity,
	                                       --@SeasonalSubscription,
	                                       @SeasonStartDate,
	                                       --@SeasonEndDate,
                                           @FreeTrialPeriodCycle,
	                                       @AllowPause,
	                                       @BillInAdvance,
	                                       @SubscriptionPaymentCollectionMode,
	                                       @SelectedPaymentCollectionMode,
	                                       @AutoRenew,
	                                       @AutoRenewalMarkupPercent, 
	                                       @RenewalGracePeriodCycle,
	                                       @NoOfRenewalReminders,
	                                       @ReminderFrequencyInDays,
	                                       @SendFirstReminderBeforeXDays,
                                           @LastRenewalReminderSentOn,
	                                       @RenewalReminderCount, 
	                                       @LastPaymentRetryLimitReminderSentOn,
	                                       @PaymentRetryLimitReminderCount,
                                           @SourceSubscriptionHeaderId, 
                                           @SubscriptionStartDate, 
                                           @SubscriptionEndDate,
                                           @PausedBy, 
                                           @PauseApprovedBy, 
                                           @UnPausedBy, 
                                           @UnPauseApprovedBy, 
                                           @CancellationOption, 
                                           @CancelledBy, 
                                           @CancellationApprovedBy, 
	                                       @Status, 
	                                       @IsActive,
	                                       @CreatedBy,
	                                       GETDATE(),
	                                       @LastUpdatedBy,
	                                       GETDATE(),
	                                       NEWID(),
	                                       @site_id,
	                                       @MasterEntityId,
                                           @SubscriptionNumber
                                        )
                                        SELECT * FROM SubscriptionHeader WHERE SubscriptionHeaderId = scope_identity()";


            List<SqlParameter> parameters = BuildSQLParameters(subscriptionHeaderDTO, userId, siteId);
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                RefreshSubscriptionHeaderDTO(subscriptionHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Inserting the Subscription Header  ", ex);
                log.LogVariableState("SubscriptionHeaderDTO", subscriptionHeaderDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(subscriptionHeaderDTO);
            return subscriptionHeaderDTO;
        }

        /// <summary>
        /// Updates the SubscriptionHeader record
        /// </summary>
        /// <param name="subscriptionHeaderDTO">SubscriptionHeaderDTO type parameter</param>
        /// <param name="userId">User updating the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public SubscriptionHeaderDTO UpdateSubscriptionHeader(SubscriptionHeaderDTO subscriptionHeaderDTO, string userId, int siteId)
        {
            log.LogMethodEntry(subscriptionHeaderDTO, userId, siteId);
            string query = @"INSERT INTO dbo.SubscriptionHeaderHistory 
                                           (SubscriptionHeaderId, TransactionId, TransactionLineId, CustomerId, CustomerContactId, CustomerCreditCardsID, ProductsId, ProductSubscriptionId, 
		                                    ProductSubscriptionName, ProductSubscriptionDescription, SubscriptionPrice, TaxInclusivePrice, SubscriptionCycle, UnitOfSubscriptionCycle, SubscriptionCycleValidity, 
			                                --SeasonalSubscription,
                                            SeasonStartDate, 
                                            --SeasonEndDate, 
                                            FreeTrialPeriodCycle, AllowPause, BillInAdvance, SubscriptionPaymentCollectionMode,
                                            SelectedPaymentCollectionMode, AutoRenew, AutoRenewalMarkupPercent, RenewalGracePeriodCycle, NoOfRenewalReminders, ReminderFrequencyInDays, 
                                            SendFirstReminderBeforeXDays, LastRenewalReminderSentOn, RenewalReminderCount, 
                                            LastPaymentRetryLimitReminderSentOn, PaymentRetryLimitReminderCount, SourceSubscriptionHeaderId, SubscriptionStartDate, 
                                            SubscriptionEndDate, PausedBy, PauseApprovedBy, UnPausedBy, UnPauseApprovedBy, CancellationOption, CancelledBy, CancellationApprovedBy,
                                            Status, IsActive, SynchStatus, site_id, MasterEntityId, CreatedBy, 
                                            CreationDate, LastUpdatedBy, LastUpdatedDate, Guid, SubscriptionNumber)  
	                                 SELECT SubscriptionHeaderId, TransactionId, TransactionLineId, CustomerId, CustomerContactId, CustomerCreditCardsID, ProductsId, ProductSubscriptionId, 
			                                ProductSubscriptionName, ProductSubscriptionDescription, SubscriptionPrice, TaxInclusivePrice, SubscriptionCycle, UnitOfSubscriptionCycle, SubscriptionCycleValidity, 
			                                --SeasonalSubscription, 
                                            SeasonStartDate, --SeasonEndDate, 
                                            FreeTrialPeriodCycle, AllowPause, BillInAdvance, SubscriptionPaymentCollectionMode, 
                                            SelectedPaymentCollectionMode, AutoRenew, AutoRenewalMarkupPercent, RenewalGracePeriodCycle, NoOfRenewalReminders, ReminderFrequencyInDays,
                                            SendFirstReminderBeforeXDays, LastRenewalReminderSentOn, RenewalReminderCount, LastPaymentRetryLimitReminderSentOn, 
                                            PaymentRetryLimitReminderCount, SourceSubscriptionHeaderId, SubscriptionStartDate, SubscriptionEndDate, PausedBy, 
                                            PauseApprovedBy, UnPausedBy, UnPauseApprovedBy, CancellationOption, CancelledBy, CancellationApprovedBy, 
                                            Status, IsActive, SynchStatus, site_id, MasterEntityId, @CreatedBy, GETDATE(), @LastUpdatedBy, GETDATE(), NEWID(),
                                            SubscriptionNumber
	                                   FROM dbo.SubscriptionHeader
	                                  WHERE SubscriptionHeaderId = @SubscriptionHeaderId;
                             UPDATE SubscriptionHeader 
                                SET TransactionId = @TransactionId,
	                                TransactionLineId = @TransactionLineId,
	                                CustomerId = @CustomerId,
	                                CustomerContactId = @CustomerContactId,
	                                CustomerCreditCardsID = @CustomerCreditCardsID,
                                    ProductsId = @ProductsId,
	                                ProductSubscriptionId = @ProductSubscriptionId,
	                                ProductSubscriptionName = @ProductSubscriptionName,
	                                ProductSubscriptionDescription = @ProductSubscriptionDescription,
	                                SubscriptionPrice =  @SubscriptionPrice,
                                    TaxInclusivePrice = @TaxInclusivePrice,
	                                SubscriptionCycle =  @SubscriptionCycle,
	                                UnitOfSubscriptionCycle =  @UnitOfSubscriptionCycle,
	                                SubscriptionCycleValidity =  @SubscriptionCycleValidity,
	                                --SeasonalSubscription = @SeasonalSubscription,
	                                SeasonStartDate = @SeasonStartDate,
	                                --SeasonEndDate = @SeasonEndDate,
                                    FreeTrialPeriodCycle = @FreeTrialPeriodCycle,
	                                AllowPause = @AllowPause,
	                                BillInAdvance = @BillInAdvance,
	                                SubscriptionPaymentCollectionMode = @SubscriptionPaymentCollectionMode,
	                                SelectedPaymentCollectionMode = @SelectedPaymentCollectionMode,
	                                AutoRenew = @AutoRenew,
	                                AutoRenewalMarkupPercent = @AutoRenewalMarkupPercent, 
	                                RenewalGracePeriodCycle =  @RenewalGracePeriodCycle,
	                                NoOfRenewalReminders = @NoOfRenewalReminders,
	                                ReminderFrequencyInDays = @ReminderFrequencyInDays,
	                                SendFirstReminderBeforeXDays = @SendFirstReminderBeforeXDays,
                                    LastRenewalReminderSentOn = @LastRenewalReminderSentOn,
	                                RenewalReminderCount = @RenewalReminderCount, 
	                                LastPaymentRetryLimitReminderSentOn = @LastPaymentRetryLimitReminderSentOn,
	                                PaymentRetryLimitReminderCount = @PaymentRetryLimitReminderCount,
                                    SourceSubscriptionHeaderId = @SourceSubscriptionHeaderId, 
                                    SubscriptionStartDate = @SubscriptionStartDate, 
                                    SubscriptionEndDate = @SubscriptionEndDate,
                                    PausedBy = @PausedBy, 
                                    PauseApprovedBy = @PauseApprovedBy, 
                                    UnPausedBy = @UnPausedBy, 
                                    UnPauseApprovedBy = @UnPauseApprovedBy, 
                                    CancellationOption = @CancellationOption, 
                                    CancelledBy = @CancelledBy, 
                                    CancellationApprovedBy = @CancellationApprovedBy, 
	                                Status = @Status, 
	                                IsActive = @IsActive, 
	                                LastUpdatedBy = @LastUpdatedBy,
	                                LastUpdatedDate = GETDATE(), 
	                                site_id = @site_id,
	                                MasterEntityId = @MasterEntityId
                              WHERE SubscriptionHeaderId = @SubscriptionHeaderId
                             SELECT * FROM SubscriptionHeader WHERE SubscriptionHeaderId = @SubscriptionHeaderId";
            List<SqlParameter> parameters = BuildSQLParameters(subscriptionHeaderDTO, userId, siteId);
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                RefreshSubscriptionHeaderDTO(subscriptionHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating the Subscription Header ", ex);
                log.LogVariableState("SubscriptionHeaderDTO", subscriptionHeaderDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(subscriptionHeaderDTO);
            return subscriptionHeaderDTO;
        } 
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="subscriptionHeaderDTO">SubscriptionHeaderDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshSubscriptionHeaderDTO(SubscriptionHeaderDTO subscriptionHeaderDTO, DataTable dt)
        {
            log.LogMethodEntry(subscriptionHeaderDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                subscriptionHeaderDTO.SubscriptionHeaderId = Convert.ToInt32(dt.Rows[0]["SubscriptionHeaderId"]);
                subscriptionHeaderDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                subscriptionHeaderDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                subscriptionHeaderDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                subscriptionHeaderDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                subscriptionHeaderDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                subscriptionHeaderDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        } 
        /// <summary>
        /// Converts the Data row object to SubscriptionHeaderDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns SubscriptionHeaderDTO</returns>
        private SubscriptionHeaderDTO GetSubscriptionHeaderDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            SubscriptionHeaderDTO subscriptionHeaderDTO = new SubscriptionHeaderDTO(Convert.ToInt32(dataRow["SubscriptionHeaderId"]),
                                             dataRow["TransactionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionId"]),
                                             dataRow["TransactionLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionLineId"]),
                                             dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                            dataRow["CustomerContactId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerContactId"]),
                                            dataRow["CustomerCreditCardsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerCreditCardsId"]),
                                            dataRow["ProductsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductsId"]),
                                            dataRow["ProductSubscriptionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductSubscriptionId"]),
                                            dataRow["ProductSubscriptionName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProductSubscriptionName"]),
                                            dataRow["ProductSubscriptionDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProductSubscriptionDescription"]),
                                            dataRow["SubscriptionPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["SubscriptionPrice"]),
                                            dataRow["TaxInclusivePrice"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["TaxInclusivePrice"]),
                                            dataRow["SubscriptionCycle"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SubscriptionCycle"]),
                                            dataRow["UnitOfSubscriptionCycle"] == DBNull.Value ? "" : Convert.ToString(dataRow["UnitOfSubscriptionCycle"]),
                                            dataRow["SubscriptionCycleValidity"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SubscriptionCycleValidity"]),
                                            //dataRow["SeasonalSubscription"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SeasonalSubscription"]),
                                            dataRow["SeasonStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["SeasonStartDate"]),
                                            //dataRow["SeasonEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["StartDate"]),
                                            dataRow["FreeTrialPeriodCycle"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["FreeTrialPeriodCycle"]),
                                            dataRow["BillInAdvance"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["BillInAdvance"]),
                                            dataRow["SubscriptionPaymentCollectionMode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SubscriptionPaymentCollectionMode"]),
                                            dataRow["SelectedPaymentCollectionMode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SelectedPaymentCollectionMode"]),
                                            dataRow["AutoRenew"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["AutoRenew"]),
                                            dataRow["AutoRenewalMarkupPercent"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["AutoRenewalMarkupPercent"]),
                                            dataRow["RenewalGracePeriodCycle"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["RenewalGracePeriodCycle"]),
                                            dataRow["NoOfRenewalReminders"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["NoOfRenewalReminders"]),
                                            dataRow["ReminderFrequencyInDays"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ReminderFrequencyInDays"]),
                                            dataRow["SendFirstReminderBeforeXDays"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SendFirstReminderBeforeXDays"]),
                                            dataRow["AllowPause"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["AllowPause"]),
                                            dataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Status"]),
                                            dataRow["LastRenewalReminderSentOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastRenewalReminderSentOn"]),
                                            dataRow["RenewalReminderCount"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["RenewalReminderCount"]),
                                            dataRow["LastPaymentRetryLimitReminderSentOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastPaymentRetryLimitReminderSentOn"]),
                                            dataRow["PaymentRetryLimitReminderCount"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["PaymentRetryLimitReminderCount"]),
                                            dataRow["sourceSubscriptionHeaderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["sourceSubscriptionHeaderId"]),
                                            dataRow["subscriptionStartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["subscriptionStartDate"]),
                                            dataRow["subscriptionEndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["subscriptionEndDate"]),
                                            dataRow["pausedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["pausedBy"]),
                                            dataRow["pauseApprovedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["pauseApprovedBy"]),
                                            dataRow["unPausedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["unPausedBy"]),
                                            dataRow["unPauseApprovedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["unPauseApprovedBy"]),
                                            dataRow["cancellationOption"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["cancellationOption"]),
                                            dataRow["cancelledBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["cancelledBy"]),
                                            dataRow["cancellationApprovedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["cancellationApprovedBy"]),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SubscriptionNumber"] == DBNull.Value ? string.Empty : dataRow["SubscriptionNumber"].ToString()
                                            );
            log.LogMethodExit(subscriptionHeaderDTO);
            return subscriptionHeaderDTO;
        }

        /// <summary>
        /// Gets the SubscriptionHeader data of passed SubscriptionHeader Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns SubscriptionHeaderDTO</returns>
        public SubscriptionHeaderDTO GetSubscriptionHeaderDTO(int id)
        {
            log.LogMethodEntry(id);
            SubscriptionHeaderDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE sh.SubscriptionHeaderId = @SubscriptionHeaderId";
            SqlParameter parameter = new SqlParameter("@SubscriptionHeaderId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetSubscriptionHeaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// GetSubscriptionHeaderDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<SubscriptionHeaderDTO> GetSubscriptionHeaderDTOList(List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SubscriptionHeaderDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_CONTACT_ID ||
                            searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_CREDIT_CARD_ID ||
                            searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_ID ||
                            searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.PRODUCTS_ID ||
                            searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.PRODUCT_SUBSCRIPTION_ID ||
                            searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID ||
                            searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.TRANSACTION_ID ||
                            searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.TRANSACTION_LINE_ID ||
                            searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.SOURCE_SUBSCRIPTION_HEADER_ID )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.AUTO_RENEW)
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.SEASONAL_SUBSCRIPTION )
                        {
                            query.Append(joiner + " CASE WHEN sh.SeasonalStartDate IS NULL THEN 0 ELSE 1 END =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.PRODUCT_SUBSCRIPTION_NAME ||
                                 searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.SELECTED_PAYMENT_COLLECTION_MODE ||
                                 searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.STATUS)
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ", '')" + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.TRX_STATUS)
                        {
                            query.Append(joiner + @" EXISTS (SELECT 1 
                                                               FROM Trx_Header th 
                                                              WHERE th.TrxId = sh.TransactionId 
                                                                AND Isnull(" + DBSearchParameters[searchParameter.Key] + ", '')" 
                                                                   + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) 
                                                                   + " ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.HAS_PAST_PENDING_BILL_CYCLES)
                        {
                            query.Append(joiner + @" ISNULL((SELECT TOP 1 1 
                                                               FROM SubscriptionBillingSchedule sbs 
                                                              WHERE sbs.SubscriptionHeaderId = sh.SubscriptionHeaderId 
                                                                AND ISNULL(sbs.IsActive,0) = 1
                                                                AND ISNULL(sbs.Status,'ACTIVE') != 'CANCELLED'
                                                                AND sbs.TransactionId is null 
                                                                AND (ISNULL(sbs.BillOnDate, GETDATE()+1) <= GETDATE())),0) = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.HAS_UNBILLED_CYCLES)
                        {
                            query.Append(joiner + @" ISNULL((SELECT TOP 1 1 
                                                               FROM SubscriptionBillingSchedule sbs 
                                                              WHERE sbs.SubscriptionHeaderId = sh.SubscriptionHeaderId 
                                                                AND ISNULL(sbs.Status,'ACTIVE') != 'CANCELLED'
                                                                AND ISNULL(sbs.IsActive,0) = 1
                                                                AND sbs.TransactionId is null ),0) = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        } 
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.RENEWAL_REMINDER_IN_X_DAYS_IS_TRUE)
                        {
                            query.Append(joiner + @" CASE WHEN (ISNULL((SELECT DATEDIFF(Day, getdate(),max(sbs.BillToDate))
                                                                          FROM SubscriptionBillingSchedule sbs  
                                                                         WHERE sbs.SubscriptionHeaderId = sh.SubscriptionHeaderId 
                                                                           AND ISNULL(sbs.Status,'ACTIVE') != 'CANCELLED'
                                                                           AND ISNULL(sbs.IsActive,0) = 1 ),0) <= ISNULL(sh.SendFirstReminderBeforeXDays,1)) THEN
		                                                              1 ELSE 0 END =  " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.NOT_REACHED_PAYMENT_RETRY_LIMIT)
                        {
                            query.Append(joiner + @" NOT EXISTS (SELECT 1 
                                                               FROM SubscriptionBillingSchedule sbs 
                                                              WHERE sbs.SubscriptionHeaderId = sh.SubscriptionHeaderId 
                                                                AND ISNULL(sbs.IsActive,0) = 1 
                                                                AND ISNULL(sbs.Status,'ACTIVE') != 'CANCELLED'
                                                                AND sbs.TransactionId is null
                                                                AND ISNULL(PaymentProcessingFailureCount,0) > " + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                                + " ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.REACHED_PAYMENT_RETRY_LIMIT)
                        { 
                            query.Append(joiner + @" EXISTS (SELECT 1 
                                                               FROM SubscriptionBillingSchedule sbs 
                                                              WHERE sbs.SubscriptionHeaderId = sh.SubscriptionHeaderId 
                                                                AND ISNULL(sbs.IsActive,0) = 1 
                                                                AND ISNULL(sbs.Status,'ACTIVE') != 'CANCELLED'
                                                                AND sbs.TransactionId is null
                                                                AND ISNULL(PaymentProcessingFailureCount,0) > " + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                                + " ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_FIRST_NAME_LIKE)
                        {
                            query.Append(joiner + @" EXISTS (SELECT 1 
                                                              FROM customers cu, profile pf 
                                                             WHERE cu.Customer_id = sh.customerId 
                                                               AND cu.profileId = pf.Id 
                                                               AND Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%' ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));

                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.HAS_EXPIRED_CREDIT_CARD ||
                                 searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.CREDIT_CARD_EXPIRES_BEFORE_NEXT_BILLING)
                        {
                            query.Append(joiner + @" 1 = 1 ");//to be handled in BL 
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.LATEST_BILL_CYCLE_HAS_PAYMENT_ERROR)
                        {
                            query.Append(joiner + @"CASE WHEN ((SELECT max(sbs.BillOnDate)
                                                                  FROM SubscriptionBillingSchedule sbs  
                                                                 WHERE sbs.SubscriptionHeaderId = sh.SubscriptionHeaderId 
                                                                   AND sbs.TransactionId is null 
                                                                   AND ISNULL(sbs.PaymentProcessingFailureCount,0) > 0
                                                                   AND ISNULL(sbs.Status,'ACTIVE') != 'CANCELLED'
                                                                   AND ISNULL(sbs.IsActive,0) = 1 ) <= GETDATE()) THEN
			                                            1 ELSE 0 END = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.SUBSCRIPTION_EXPIRES_IN_XDAYS)
                        {
                            query.Append(joiner + @" ISNULL((SELECT max(sbs.BillToDate)
                                                              FROM SubscriptionBillingSchedule sbs  
                                                             WHERE sbs.SubscriptionHeaderId = sh.SubscriptionHeaderId 
                                                               AND ISNULL(sbs.Status,'ACTIVE') != 'CANCELLED'
                                                               AND ISNULL(sbs.IsActive,0) = 1),GETDATE()+1000) <= GETDATE()+" + dataAccessHandler.GetParameterName(searchParameter.Key) + " ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.SUBSCRIPTION_IS_EXPIRED)
                        {
                            query.Append(joiner + @" CASE WHEN ISNULL((SELECT max(sbs.BillToDate)
                                                                         FROM SubscriptionBillingSchedule sbs  
                                                                        WHERE sbs.SubscriptionHeaderId = sh.SubscriptionHeaderId  
                                                                          AND ISNULL(sbs.IsActive,0) = 1),GETDATE()+1000) <= GETDATE()
                                                          THEN 1 ELSE 0 END = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.CREATION_DATE_LESS_THAN)
                        {
                            query.Append(joiner +" "+ DBSearchParameters[searchParameter.Key] + " < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.CREATION_DATE_GREATER_EQUAL_TO)
                        {
                            query.Append(joiner + " " + DBSearchParameters[searchParameter.Key] + " >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.CANCELLATION_DATE_LESS_THAN)
                        {
                            query.Append(joiner + @"     (SELECT min(CreationDate) 
                                                               from (
                                                             SELECT shh.CreationDate
                                                               FROM SubscriptionHeaderHistory shh
                                                              WHERE shh.SubscriptionHeaderId = sh.SubscriptionHeaderId
                                                                AND shh.CancelledBy is not null
                                                              union all
                                                             SELECT shh.CreationDate
                                                               FROM subscriptionHeader shh
                                                               WHERE shh.SubscriptionHeaderId = sh.SubscriptionHeaderId
                                                                AND shh.CancelledBy is not null
                                                             ) as a)  < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.CANCELLATION_DATE_GREATER_EQUAL_TO)
                        {
                            query.Append(joiner + @"    ( SELECT min (CreationDate) FROM (
                                                            SELECT shh.CreationDate 
                                                              FROM SubscriptionHeaderHistory shh
                                                             WHERE shh.SubscriptionHeaderId = sh.SubscriptionHeaderId
                                                               AND shh.CancelledBy is not null
                                                               union all
                                                               SELECT shh.CreationDate  
                                                              FROM subscriptionHeader shh
                                                             WHERE shh.SubscriptionHeaderId = sh.SubscriptionHeaderId
                                                               AND shh.CancelledBy is not null) as a)  >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.RENEWED)
                        {
                            query.Append(joiner + @" ISNULL((select 1 
                                                               from SubscriptionHeader shin 
                                                              where shin.SourceSubscriptionHeaderId = sh.SubscriptionHeaderId),0) = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        //else if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.GRACE_PERIOD_ENTITLEMENTS_ON_HOLD)
                        //{
                        //    query.Append(joiner + @" ISNULL((SELECT top 1 hasOnHold from (select 1 as hasOnHold from CardCreditPlus cp
                        //                                                                   where cp.TrxId = sh.TransactionId
                        //                                                                     and cp.SubscriptionBillingScheduleId = -1
                        //                                                                     and  cp.IsActive = 1
                        //                                                                     and ISNULL(cp.ValidityStatus,'V') = 'H'
                        //                                                                   union all
                        //                                                                  select 1 from CardGames cg
                        //                                                                   where cg.TrxId = sh.TransactionId
                        //                                                                     and cg.SubscriptionBillingScheduleId = -1
                        //                                                                     and cg.IsActive = 1
                        //                                                                     and ISNULL(cg.ValidityStatus,'V') = 'H'
                        //                                                                   union all
                        //                                                                  select 1 from CardDiscounts cd
                        //                                                                   where cd.TransactionId = sh.TransactionId
                        //                                                                     and cd.SubscriptionBillingScheduleId = -1
                        //                                                                     and cd.IsActive = 1
                        //                                                                     and ISNULL(cd.ValidityStatus,'V') = 'H' 
                        //                                                                   ) as hasOnHold),0)  = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ");
                        //    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value)); 
                        //}
                        else
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<SubscriptionHeaderDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SubscriptionHeaderDTO subscriptionHeaderDTO = GetSubscriptionHeaderDTO(dataRow);
                    list.Add(subscriptionHeaderDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the SubscriptionHeaderDTO List for Id List
        /// </summary>
        /// <param name="subscriptionHeaderIdList">integer list parameter</param>
        /// <returns>Returns List of SubscriptionHeaderDTO</returns>
        public List<SubscriptionHeaderDTO> GetSubscriptionHeaderDTOList(List<int> subscriptionHeaderIdList)
        {
            log.LogMethodEntry(subscriptionHeaderIdList);
            List<SubscriptionHeaderDTO> list = new List<SubscriptionHeaderDTO>();
            string query = @"SELECT sh.* 
                              FROM SubscriptionHeader AS sh
                                  inner join @SubscriptionHeaderIdList List on sh.SubscriptionHeaderId = List.Id ";

            DataTable table = dataAccessHandler.BatchSelect(query, "@SubscriptionHeaderIdList", subscriptionHeaderIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetSubscriptionHeaderDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// Gets the SubscriptionHeaderDTO List for ccc Id List
        /// </summary>
        /// <param name="customerCreditCardIdList">integer list parameter</param>
        /// <returns>Returns List of SubscriptionHeaderDTO</returns>
        public List<SubscriptionHeaderDTO> GetSubscriptionHeaderListByCreditCards(List<int> customerCreditCardIdList)
        {
            log.LogMethodEntry(customerCreditCardIdList);
            List<SubscriptionHeaderDTO> list = new List<SubscriptionHeaderDTO>();
            string query = @"SELECT sh.* 
                              FROM SubscriptionHeader AS sh
                                  inner join @CustomerCreditCardIdList List on sh.CustomerCreditCardsID = List.Id ";

            DataTable table = dataAccessHandler.BatchSelect(query, "@CustomerCreditCardIdList", customerCreditCardIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetSubscriptionHeaderDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        } 
    }
}

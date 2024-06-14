/********************************************************************************************
 * Project Name - TransactionPayments Data Handler
 * Description  - Data handler of the TransactionPayments class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *1.00        20-Jun-2017   Lakshminarayana Created 
 *1.10        28-Jan-2019   Mathew Ninan    Who Columns added 
 *2.60.2      06-Jun-2019   Akshay G        Code merge from Development to WebManagementStudio 
 *2.70.2      01-Jul-2019   Girish Kundar   Modified : Added RefreshDTO method and Fix For SQL Injection Issue.
 *2.70.2      06-Dec-2019   Jinto Thomas    Removed siteid from update query 
 *2.100.0     25-Aug-2020   Mathew Ninan    added approvedBy field       
 *2.110.0     08-Dec-2020   Guru S A        Subscription changes   
 *2.110.0     09-Feb-2021   Abhishek        Modified : Added new field ExternalSourceReference
 *2.130.7     13-Apr-2022   Guru S A        Payment mode OTP validation changes 
 *2.140.0     09-Nov-2021   Laster Menezes   Modified BuildQueryString method to handle LAST_UPDATE_FROM_TIME Search Parameter
 *2.140.2    14-APR-2022    Girish Kundar   Modified : Aloha BSP integration changes
 *2.140       14-Oct-2021   Fiona Lishal     Modified for Payment Settlements
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    ///  TransactionPayments Data Handler - Handles insert, update and select of  TransactionPayments objects
    /// </summary>
    public class TransactionPaymentsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<TransactionPaymentsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionPaymentsDTO.SearchByParameters, string>
            {
                {TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, "TrxId"},
                {TransactionPaymentsDTO.SearchByParameters.PAYMENT_MODE_ID, "PaymentModeId"},
                {TransactionPaymentsDTO.SearchByParameters.CARD_ID, "CardId"},
                {TransactionPaymentsDTO.SearchByParameters.ORDER_ID, "OrderId"},
                {TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID, "CCResponseId"},
                {TransactionPaymentsDTO.SearchByParameters.PARENT_PAYMENT_ID, "ParentPaymentId"},
                {TransactionPaymentsDTO.SearchByParameters.SPLIT_ID, "SplitId"},
                {TransactionPaymentsDTO.SearchByParameters.POS_MACHINE, "PosMachine"},
                {TransactionPaymentsDTO.SearchByParameters.LAST_UPDATED_USER, "LastUpdatedUser"},
                {TransactionPaymentsDTO.SearchByParameters.SITE_ID, "site_id"},
                {TransactionPaymentsDTO.SearchByParameters.CREDIT_CARD_AUTHORIZATION, "CreditCardAuthorization"},
                {TransactionPaymentsDTO.SearchByParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                {TransactionPaymentsDTO.SearchByParameters.PAYMENT_ID, "PaymentId"},
                {TransactionPaymentsDTO.SearchByParameters.LAST_UPDATE_FROM_TIME, "LastUpdateDate"},
                {TransactionPaymentsDTO.SearchByParameters.DELIVERY_CHANNEL_ID, "tod.DeliveryChannelId"},
                {TransactionPaymentsDTO.SearchByParameters.TRANSACTION_FROM_DATE, "th.Trxdate"},
                {TransactionPaymentsDTO.SearchByParameters.TRANSACTION_TO_DATE, "th.Trxdate"},
                {TransactionPaymentsDTO.SearchByParameters.NON_REVERSED_PAYMENT, ""}
            };
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT tp.* FROM TrxPayments tp ";
        List<SqlParameter> parameters = new List<SqlParameter>();
        /// <summary>
        /// Default constructor of TransactionPaymentsDataHandler class
        /// </summary>
        public TransactionPaymentsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// ParameterHelper method for building SQL parameters for query
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="parameterName">parameterName</param>
        /// <param name="value">value</param>
        /// <param name="negetiveValueNull">negetiveValueNull</param>
        //private void ParameterHelper(List<SqlParameter> parameters, string parameterName, object value, bool negetiveValueNull = false)
        //{
        //    log.LogMethodEntry(parameters, parameterName, value, negetiveValueNull);
        //    if(parameters != null && !string.IsNullOrEmpty(parameterName))
        //    {
        //        if(value is int)
        //        {
        //            if(negetiveValueNull && ((int)value) < 0)
        //            {
        //                parameters.Add(new SqlParameter(parameterName, DBNull.Value));
        //            }
        //            else
        //            {
        //                parameters.Add(new SqlParameter(parameterName, value));
        //            }
        //        }
        //        else if(value is string)
        //        {
        //            if(string.IsNullOrEmpty(value as string))
        //            {
        //                parameters.Add(new SqlParameter(parameterName, DBNull.Value));
        //            }
        //            else
        //            {
        //                parameters.Add(new SqlParameter(parameterName, value));
        //            }
        //        }
        //        else
        //        {
        //            if(value == null)
        //            {
        //                parameters.Add(new SqlParameter(parameterName, DBNull.Value));
        //            }
        //            else
        //            {
        //                parameters.Add(new SqlParameter(parameterName, value));
        //            }
        //        }
        //    }
        //    log.LogMethodExit();
        //}
        

        /// <summary>
        /// PassParametersHelper method to build SQL parameters for Insert/ Update
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="CCStatusPGWDTO">CCStatusPGWDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        ///  <returns> List of SQL Parameters</returns>
        private void PassParametersHelper(List<SqlParameter> parameters, TransactionPaymentsDTO transactionPaymentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(parameters, transactionPaymentsDTO, loginId, siteId);
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentId", transactionPaymentsDTO.PaymentId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", transactionPaymentsDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentModeId", transactionPaymentsDTO.PaymentModeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Amount", transactionPaymentsDTO.Amount));
            parameters.Add(dataAccessHandler.GetSecureSQLParameter("@CreditCardNumber", ((string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardNumber) ? transactionPaymentsDTO.CreditCardNumber
                                                                             : (new String('X', 12) + ((transactionPaymentsDTO.CreditCardNumber.Length > 4) ? transactionPaymentsDTO.CreditCardNumber.Substring(transactionPaymentsDTO.CreditCardNumber.Length - 4)
                                                                                                                         : transactionPaymentsDTO.CreditCardNumber))))));
            parameters.Add(dataAccessHandler.GetSecureSQLParameter("@NameOnCreditCard", transactionPaymentsDTO.NameOnCreditCard));
            parameters.Add(dataAccessHandler.GetSecureSQLParameter("@CreditCardName", transactionPaymentsDTO.CreditCardName));
            parameters.Add(dataAccessHandler.GetSecureSQLParameter("@CreditCardExpiry", transactionPaymentsDTO.CreditCardExpiry));
            parameters.Add(dataAccessHandler.GetSecureSQLParameter("@CreditCardAuthorization", transactionPaymentsDTO.CreditCardAuthorization));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", transactionPaymentsDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardEntitlementType", transactionPaymentsDTO.CardEntitlementType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardCreditPlusId", transactionPaymentsDTO.CardCreditPlusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderId", transactionPaymentsDTO.OrderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Reference", transactionPaymentsDTO.Reference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CCResponseId", transactionPaymentsDTO.CCResponseId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Memo", transactionPaymentsDTO.Memo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentPaymentId", transactionPaymentsDTO.ParentPaymentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TenderedAmount", transactionPaymentsDTO.TenderedAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TipAmount", transactionPaymentsDTO.TipAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SplitId", transactionPaymentsDTO.SplitId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PosMachine", transactionPaymentsDTO.PosMachine));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CurrencyCode", transactionPaymentsDTO.CurrencyCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CurrencyRate", transactionPaymentsDTO.CurrencyRate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", transactionPaymentsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CouponValue", transactionPaymentsDTO.CouponValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApprovedBy", transactionPaymentsDTO.ApprovedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerCardProfileId", transactionPaymentsDTO.CustomerCardProfileId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSourceReference", transactionPaymentsDTO.ExternalSourceReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute1", transactionPaymentsDTO.Attribute1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute2", transactionPaymentsDTO.Attribute2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute3", transactionPaymentsDTO.Attribute3));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute4", transactionPaymentsDTO.Attribute4));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute5", transactionPaymentsDTO.Attribute5));
            if(transactionPaymentsDTO.IsTaxable.HasValue)
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@IsTaxable", transactionPaymentsDTO.IsTaxable.Value ? "Y" : "N"));
            }
            else
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@IsTaxable", null));
            }
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentModeOTP", transactionPaymentsDTO.PaymentModeOTP));
            log.LogMethodExit(parameters);
        }

        /// <summary>
        /// Inserts the TransactionPayments record to the database
        /// </summary>
        /// <param name="transactionPaymentsDTO">TransactionPaymentsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns TransactionPaymentsDTO</returns>
        public TransactionPaymentsDTO InsertTransactionPayments(TransactionPaymentsDTO transactionPaymentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionPaymentsDTO, loginId, siteId);
            string query = @"INSERT INTO TrxPayments 
                                        ( 
                                            TrxId,
                                            PaymentModeId,
                                            Amount,
                                            CreditCardNumber,
                                            NameOnCreditCard,
                                            CreditCardName,
                                            CreditCardExpiry,
                                            CreditCardAuthorization,
                                            CardId,
                                            CardEntitlementType,
                                            CardCreditPlusId,
                                            OrderId,
                                            Reference,
                                            CCResponseId,
                                            Memo,
                                            PaymentDate,
                                            LastUpdatedUser,
                                            ParentPaymentId,
                                            TenderedAmount,
                                            TipAmount,
                                            SplitId,
                                            PosMachine,
                                            CurrencyCode,
                                            CurrencyRate,
                                            site_id,
                                            MasterEntityId,
                                            CouponValue,
                                            isTaxable,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdateDate,
                                            ApprovedBy,
                                            CustomerCardProfileId,
                                            ExternalSourceReference,
                                            Attribute1, Attribute2, Attribute3, Attribute4, Attribute5,
                                            PaymentModeOTP
                                        ) 
                                VALUES 
                                        (
                                            @TrxId,
                                            @PaymentModeId,
                                            @Amount,
                                            @CreditCardNumber,
                                            @NameOnCreditCard,
                                            @CreditCardName,
                                            @CreditCardExpiry,
                                            @CreditCardAuthorization,
                                            @CardId,
                                            @CardEntitlementType,
                                            @CardCreditPlusId,
                                            @OrderId,
                                            @Reference,
                                            @CCResponseId,
                                            @Memo,
                                            GETDATE(),
                                            @LastUpdatedUser,
                                            @ParentPaymentId,
                                            @TenderedAmount,
                                            @TipAmount,
                                            @SplitId,
                                            @PosMachine,
                                            @CurrencyCode,
                                            @CurrencyRate,
                                            @site_id,
                                            @MasterEntityId,
                                            @CouponValue,
                                            @isTaxable,
                                            @LastUpdatedUser,
                                            GETDATE(),
                                            GETDATE(),
                                            @ApprovedBy,
                                            @CustomerCardProfileId,
                                            @ExternalSourceReference,
                                            @Attribute1, @Attribute2, @Attribute3, @Attribute4, @Attribute5,
                                            @PaymentModeOTP
                                        )SELECT * FROM TrxPayments WHERE PaymentId = scope_identity()";
            List<SqlParameter> parameters = new List<SqlParameter>();
            PassParametersHelper(parameters, transactionPaymentsDTO, loginId, siteId);
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                RefreshTransactionPaymentsDTO(transactionPaymentsDTO, dt);
            }

            catch(Exception ex)
            {
                log.Error("Error occurred while inserting TransactionPaymentsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        /// <summary>
        /// Updates the TransactionPayments record
        /// </summary>
        /// <param name="transactionPaymentsDTO">TransactionPaymentsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns theTransactionPaymentsDTO</returns>
        public TransactionPaymentsDTO UpdateTransactionPayments(TransactionPaymentsDTO transactionPaymentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionPaymentsDTO, loginId, siteId);
            string query = @"UPDATE TrxPayments 
                             SET TrxId=@TrxId,
                                 PaymentModeId=@PaymentModeId,
                                 Amount=@Amount,
                                 CreditCardNumber=@CreditCardNumber,
                                 NameOnCreditCard=@NameOnCreditCard,  
                                 CreditCardName=@CreditCardName,
                                 CreditCardExpiry=@CreditCardExpiry,   
                                 CreditCardAuthorization=@CreditCardAuthorization,
                                 CardId=@CardId,
                                 CardEntitlementType=@CardEntitlementType,
                                 CardCreditPlusId=@CardCreditPlusId,
                                 OrderId=@OrderId,
                                 Reference=@Reference,
                                 CCResponseId=@CCResponseId,
                                 Memo=@Memo,
                                 LastUpdatedUser=@LastUpdatedUser,
                                 LastUpdateDate = getdate(),
                                 ParentPaymentId=@ParentPaymentId,
                                 TenderedAmount=@TenderedAmount,
                                 TipAmount=@TipAmount,
                                 SplitId=@SplitId,
                                 PosMachine=@PosMachine,
                                 CurrencyCode=@CurrencyCode,
                                 CurrencyRate=@CurrencyRate,
                                 --site_id=@site_id,
                                 CouponValue=@CouponValue,
                                 IsTaxable=@IsTaxable,
                                 ApprovedBy = @ApprovedBy,
                                 ExternalSourceReference = @ExternalSourceReference,
                                 CustomerCardProfileId = @CustomerCardProfileId,
                                 Attribute1 = @Attribute1,
                                 Attribute2 = @Attribute2,
                                 Attribute3 = @Attribute3,
                                 Attribute4 = @Attribute4,
                                 Attribute5 = @Attribute5,
                                 PaymentModeOTP = @PaymentModeOTP
                             WHERE PaymentId = @PaymentId 
                             SELECT * FROM TrxPayments WHERE PaymentId = @PaymentId ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            PassParametersHelper(parameters, transactionPaymentsDTO, loginId, siteId);
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                RefreshTransactionPaymentsDTO(transactionPaymentsDTO, dt);
            }

            catch (Exception ex)
            {
                log.Error("Error occurred while Updating TransactionPaymentsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="transactionPaymentsDTO">TransactionPaymentsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshTransactionPaymentsDTO(TransactionPaymentsDTO transactionPaymentsDTO, DataTable dt)
        {
            log.LogMethodEntry(transactionPaymentsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                transactionPaymentsDTO.PaymentId = Convert.ToInt32(dt.Rows[0]["PaymentId"]);
                transactionPaymentsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                transactionPaymentsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                transactionPaymentsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                transactionPaymentsDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                transactionPaymentsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                transactionPaymentsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to TransactionPaymentsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns TransactionPaymentsDTO</returns>
        private TransactionPaymentsDTO GetTransactionPaymentsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(Convert.ToInt32(dataRow["PaymentId"]),
                                                              dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                                              dataRow["PaymentModeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentModeId"]),
                                                              dataRow["Amount"] == DBNull.Value ? 0d : Convert.ToDouble(dataRow["Amount"]),
                                                              dataRow["CreditCardNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreditCardNumber"]),
                                                              dataRow["NameOnCreditCard"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["NameOnCreditCard"]),
                                                              dataRow["CreditCardName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreditCardName"]),
                                                              dataRow["CreditCardExpiry"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreditCardExpiry"]),
                                                              dataRow["CreditCardAuthorization"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreditCardAuthorization"]),
                                                              dataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardId"]),
                                                              dataRow["CardEntitlementType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CardEntitlementType"]),
                                                              dataRow["CardCreditPlusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardCreditPlusId"]),
                                                              dataRow["OrderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderId"]),
                                                              dataRow["Reference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Reference"]),
                                                              dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                              dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                              dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                              dataRow["CCResponseId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CCResponseId"]),
                                                              dataRow["Memo"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Memo"]),
                                                              dataRow["PaymentDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["PaymentDate"]),
                                                              dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]),
                                                              dataRow["ParentPaymentId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentPaymentId"]),
                                                              dataRow["TenderedAmount"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["TenderedAmount"]),
                                                              dataRow["TipAmount"] == DBNull.Value ? 0d : Convert.ToDouble(dataRow["TipAmount"]),
                                                              dataRow["SplitId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SplitId"]),
                                                              dataRow["PosMachine"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PosMachine"]),
                                                              dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                              dataRow["CurrencyCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CurrencyCode"]),
                                                              dataRow["CurrencyRate"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["CurrencyRate"]),
                                                              dataRow["IsTaxable"] == DBNull.Value ? (bool?)null : Convert.ToString(dataRow["IsTaxable"]) == "Y",
                                                              dataRow["CouponValue"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["CouponValue"]),
                                                              dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                              dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                              dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                              false,
                                                              dataRow["ApprovedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ApprovedBy"]) ,
                                                               dataRow["CustomerCardProfileId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CustomerCardProfileId"]),
                                                              dataRow["ExternalSourceReference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ExternalSourceReference"]),
                                                              dataRow["Attribute1"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Attribute1"]),
                                                              dataRow["Attribute2"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Attribute2"]),
                                                              dataRow["Attribute3"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Attribute3"]),
                                                              dataRow["Attribute4"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Attribute4"]),
                                                              dataRow["Attribute5"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Attribute5"]),
                                                              dataRow["PaymentModeOTP"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PaymentModeOTP"])

                                                                                      );
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        /// <summary>
        /// Gets the TransactionPayments data of passed requestId
        /// </summary>
        /// <param name="paymentId">integer type parameter</param>
        /// <returns>Returns TransactionPaymentsDTO</returns>
        public TransactionPaymentsDTO GetTransactionPaymentsDTO(int paymentId)
        {
            log.LogMethodEntry(paymentId);
            TransactionPaymentsDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE PaymentId = @PaymentId";
            SqlParameter parameter = new SqlParameter("@PaymentId", paymentId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                returnValue = GetTransactionPaymentsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private string BuildQueryString(List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" where ");
            string joiner = string.Empty;
            foreach (KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string> searchParameter in searchParameters)
            {
                if (DBSearchParameters.ContainsKey(searchParameter.Key))
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID ||
                        searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.PAYMENT_MODE_ID ||
                        searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.CARD_ID ||
                        searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.ORDER_ID ||
                        searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID ||
                        searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.PAYMENT_ID ||
                        searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.PARENT_PAYMENT_ID ||
                        searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.MASTER_ENTITY_ID ||
                        searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.SPLIT_ID)
                    {
                        query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                    }
                    else if (searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.SITE_ID)
                    {
                        query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " =-1)");
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                    }
                    else if (searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.POS_MACHINE ||
                            searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.LAST_UPDATED_USER ||
                            searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.ATTRIBUTE1 ||
                            searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.ATTRIBUTE2 ||
                            searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.ATTRIBUTE3 ||
                            searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.ATTRIBUTE4 ||
                            searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.ATTRIBUTE5)
                    {
                        query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                    }
                    else if (searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.LAST_UPDATE_FROM_TIME)
                    {
                        query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                    else if (searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.DELIVERY_CHANNEL_ID)
                    {
                        query.Append(joiner + @"exists (select 1 
                                                          from TransctionOrderDispensing tod 
                                                         where tod.TrxId = tp.TrxId
                                                           and  " + DBSearchParameters[searchParameter.Key] + " =" + dataAccessHandler.GetParameterName(searchParameter.Key) + ") ");
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                    }
                    else if (searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.TRANSACTION_FROM_DATE)
                    {
                        query.Append(joiner + @" exists(select 1
                                                          from trx_header th
                                                         where th.TrxId = tp.TrxId
                                                           and " + DBSearchParameters[searchParameter.Key] + " >= " + dataAccessHandler.GetParameterName(searchParameter.Key) + ") ");
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                    else if (searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.TRANSACTION_TO_DATE)
                    {
                        query.Append(joiner + @" exists(select 1
                                                          from trx_header th
                                                         where th.TrxId = tp.TrxId
                                                           and " + DBSearchParameters[searchParameter.Key] + " <= " + dataAccessHandler.GetParameterName(searchParameter.Key) + ") ");
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                    else if (searchParameter.Key == TransactionPaymentsDTO.SearchByParameters.NON_REVERSED_PAYMENT)
                    {
                        query.Append(joiner + @" ParentPaymentId IS NULL 
                                                   AND NOT EXISTS( SELECT * FROM TrxPayments WHERE ParentPaymentId = tp.PaymentId)
                                                   ");
                    }
                    else
                    {
                        query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
            log.LogMethodExit(query.ToString());
            return query.ToString();
        }

        /// <summary>
        /// Gets the TransactionPaymentsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TransactionPaymentsDTO matching the search criteria</returns>
        public List<TransactionPaymentsDTO> GetTransactionPaymentsDTOList(List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<TransactionPaymentsDTO> list = null;

            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                selectQuery = selectQuery + BuildQueryString(searchParameters);
            }
            log.Debug("Search query: " + selectQuery);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<TransactionPaymentsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TransactionPaymentsDTO transactionPaymentsDTO = GetTransactionPaymentsDTO(dataRow);
                    list.Add(transactionPaymentsDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the TransactionPaymentsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="cCResponseIdList">List of ResponseId. It is a optional parameter</param>
        /// <returns>Returns the list of TransactionPaymentsDTO matching the search criteria</returns>
        public List<TransactionPaymentsDTO> GetNonReversedTransactionPaymentsDTOList(List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters, List<int> cCResponseIdList = null)
        {
            log.LogMethodEntry(searchParameters, cCResponseIdList);
            List<TransactionPaymentsDTO> list = null;
            string selectQuery = @"SELECT tp.* FROM TrxPayments tp ";
            StringBuilder query = new StringBuilder(" where ");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                selectQuery = selectQuery + BuildQueryString(searchParameters) + " AND ";
            }
            else
            {
                selectQuery = selectQuery + " WHERE ";
            }
            selectQuery = selectQuery + @" ParentPaymentId IS NULL 
                                           AND NOT EXISTS( SELECT * FROM TrxPayments WHERE ParentPaymentId = tp.PaymentId)
                                           AND EXISTS (SELECT TrxId FROM trx_header th WHERE tp.TrxId = th.TrxId AND 
                                                           NOT EXISTS ( SELECT trxid FROM trx_header WHERE OriginalTrxID = th.TrxId))";
            if (cCResponseIdList != null && cCResponseIdList.Count > 0)
            {
                StringBuilder sb = new StringBuilder(" AND CCResponseId IN (");
                for (int i = 0; i < cCResponseIdList.Count; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(cCResponseIdList[i].ToString());
                }
                sb.Append(")");
                selectQuery = selectQuery + sb.ToString();
            }
            log.Debug("Search query: " + selectQuery);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<TransactionPaymentsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TransactionPaymentsDTO transactionPaymentsDTO = GetTransactionPaymentsDTO(dataRow);
                    list.Add(transactionPaymentsDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the TransactionPaymentsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="cCResponseIdList">List of ResponseId. It is a optional parameter</param>
        /// <returns>Returns the list of TransactionPaymentsDTO matching the search criteria</returns>
        public List<TransactionPaymentsDTO> GetNonReversedTransactionPaymentsDTOListForReversal(List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters, List<int> cCResponseIdList = null)
        {
            log.LogMethodEntry(searchParameters, cCResponseIdList);
            List<TransactionPaymentsDTO> list = null;
            string selectQuery = @"SELECT tp.* FROM TrxPayments tp ";
            StringBuilder query = new StringBuilder(" where ");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                selectQuery = selectQuery + BuildQueryString(searchParameters) + " AND ";
            }
            else
            {
                selectQuery = selectQuery + " WHERE ";
            }
            selectQuery = selectQuery + @" ParentPaymentId IS NULL 
                                           AND NOT EXISTS( SELECT * FROM TrxPayments WHERE ParentPaymentId = tp.PaymentId)";
            if (cCResponseIdList != null && cCResponseIdList.Count > 0)
            {
                StringBuilder sb = new StringBuilder(" AND CCResponseId IN (");
                for (int i = 0; i < cCResponseIdList.Count; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(cCResponseIdList[i].ToString());
                }
                sb.Append(")");
                selectQuery = selectQuery + sb.ToString();
            }
            log.Debug("Search query: " + selectQuery);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<TransactionPaymentsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TransactionPaymentsDTO transactionPaymentsDTO = GetTransactionPaymentsDTO(dataRow);
                    list.Add(transactionPaymentsDTO);
                }

            }
            log.LogMethodExit(list);
            return list;
        }

        internal List<TransactionPaymentsDTO> GetTransactionPaymentLines(List<int> transactionIdList)
        {
            log.LogMethodEntry(transactionIdList);
            List<TransactionPaymentsDTO> transactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
            string query = @"SELECT *
                            FROM TrxPayments, @transactionIdList List
                            WHERE TrxId = List.Id ";
            DataTable table = dataAccessHandler.BatchSelect(query, "@transactionIdList", transactionIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                transactionPaymentsDTOList = table.Rows.Cast<DataRow>().Select(x => GetTransactionPaymentsDTO(x)).ToList();
            }

            log.LogMethodExit(transactionPaymentsDTOList);
            return transactionPaymentsDTOList;
        }
        /// <summary>
        /// GetTransactionPaymentsDTOList
        /// </summary>
        /// <param name="paymentIdList"></param>
        /// <returns></returns>
        public List<TransactionPaymentsDTO> GetTransactionPaymentsDTOList(List<int> paymentIdList)
        {
            log.LogMethodEntry(paymentIdList);
            List<TransactionPaymentsDTO> list = new List<TransactionPaymentsDTO>();
            string query = "SELECT tp.* FROM TrxPayments tp, @PaymentIdList List WHERE tp.PaymentId = List.Id";
            DataTable table = dataAccessHandler.BatchSelect(query, "@PaymentIdList", paymentIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetTransactionPaymentsDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

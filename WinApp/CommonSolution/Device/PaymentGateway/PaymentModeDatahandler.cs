/********************************************************************************************
 * Project Name - PaymentMode Datahandler Programs
 * Description  - Data object of PaymentMode Datahandler  
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *1.00        26-Nov-2016   Rakshith        Created 
 *2.50.0      04-Dec-2018   Mathew Ninan    Transaction re-design 
 *2.60.0      24-Apr-2019   Mathew Ninan    Site Id check updated 
 *2.70        11-Apr-2019   Mushahid Faizan Added paymentReferenceMandatory & DisplayOrder column.
 *                                          Renamed column LastUpdatedDate to LastUpdateDate
 
 *2.70.2        01-Jul-2019   Girish Kundar  Modified : For SQL Injection Issue.  
 *                                                    Added missed Columns to Insert/Update
 *            25-Jul-2019   Mushahid Faizan  Added DeletePaymentMode() method. and IsActive.
 *2.70.2      06-Dec-2019   Jinto Thomas     Removed siteid from update query   
 *2.90        15-Jul-2020   Girish Kundar    Modified : Added PAYMENT_MODE_ID_LIST as search parameter
 *2.130.0     21-May-2021   Girish Kundar    Modified: Added Attribue1  to 5 columns to the table as part of Xero integration
 *2.130.7     13-Apr-2022   Guru S A         Payment mode OTP validation changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    ///  PaymentModeDatahandler Class
    /// </summary>
    public class PaymentModeDatahandler
    {
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM PaymentModes AS pm ";

        /// <summary>
        /// Parameterized constructor of PaymentModeDatahandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public PaymentModeDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// For search parameter Specified
        /// </summary>
        private static readonly Dictionary<PaymentModeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PaymentModeDTO.SearchByParameters, string>
        {
            {PaymentModeDTO.SearchByParameters.PAYMENT_MODE,"pm.PaymentMode"},
            {PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID,"pm.PaymentModeId"},
            {PaymentModeDTO.SearchByParameters.PAYMENT_MODE_GUID,"pm.Guid"},
            {PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID_LIST,"pm.PaymentModeId"},
            {PaymentModeDTO.SearchByParameters.SITE_ID,"pm.site_id"},
            {PaymentModeDTO.SearchByParameters.ISCASH,"pm.isCash"},
            {PaymentModeDTO.SearchByParameters.ISCREDITCARD,"pm.isCreditCard"},
            {PaymentModeDTO.SearchByParameters.ISDEBITCARD,"pm.isDebitCard"},
            {PaymentModeDTO.SearchByParameters.ISROUNDOFF,"pm.isRoundOff"},
            {PaymentModeDTO.SearchByParameters.ISACTIVE,"pm.IsActive"},
            {PaymentModeDTO.SearchByParameters.MASTER_ENTITY_ID,"pm.MasterEntityId"},
            {PaymentModeDTO.SearchByParameters.PAYMENT_CHANNEL_NAME,"v.LookupValue"},
            {PaymentModeDTO.SearchByParameters.OTP_VALIDATION_REQUIRED,"pm.OTPValidation"}
        };

        /// <summary>
        ///  GetPaymentGateway(int paymentModeId) Method
        /// </summary>
        /// <param name="paymentModeId">int paymentModeId</param>
        /// <returns>list of CoreKeyValueStruct</returns>
        public List<CoreKeyValueStruct> GetPaymentGateway(int paymentModeId)
        {
            log.LogMethodEntry(paymentModeId);
            try
            {
                string paymentQuery = @"select p.PaymentModeId,p.PaymentMode,v.LookupValue,v.Description   
                                                from PaymentModes p ,LookupValues v 
                                                where p.Gateway=v.LookupValueId 
                                                and p.PaymentModeId=@paymentModeId";

                SqlParameter[] paymentParameters = new SqlParameter[1];
                paymentParameters[0] = new SqlParameter("@paymentModeId", paymentModeId);

                DataTable dtPayment = dataAccessHandler.executeSelectQuery(paymentQuery, paymentParameters, sqlTransaction);
                List<CoreKeyValueStruct> payementDetails = new List<CoreKeyValueStruct>();

                if (dtPayment.Rows.Count > 0)
                {
                    DataRow paymentRow = dtPayment.Rows[0];
                    payementDetails.Add(new CoreKeyValueStruct("PaymentModeID", paymentRow["PaymentModeId"].ToString()));
                    payementDetails.Add(new CoreKeyValueStruct("PaymentMode", paymentRow["PaymentMode"].ToString()));
                    payementDetails.Add(new CoreKeyValueStruct("GatewayName", paymentRow["LookupValue"].ToString()));
                    payementDetails.Add(new CoreKeyValueStruct("Description", paymentRow["Description"].ToString()));

                }

                log.LogMethodExit(payementDetails);
                return payementDetails;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred at  GetPaymentGateway(int paymentModeId)", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// GetPaymentModeList method
        /// </summary>
        /// <param name="selectPaymentModesQuery">selectPaymentModesQuery</param>
        /// <param name="selectParameters">selectParameters</param>
        /// <returns>returns List of PaymentModeDTO</returns>
        private List<PaymentModeDTO> GetPaymentModeList(string selectPaymentModesQuery, SqlParameter[] selectParameters = null)
        {
            log.LogMethodEntry(selectPaymentModesQuery, selectParameters);

            List<PaymentModeDTO> paymentModeDTOList = new List<PaymentModeDTO>();
            DataTable dtPaymentModesDTO = dataAccessHandler.executeSelectQuery(selectPaymentModesQuery, selectParameters, sqlTransaction);
            if (dtPaymentModesDTO.Rows.Count > 0)
            {
                foreach (DataRow paymentRow in dtPaymentModesDTO.Rows)
                {
                    PaymentModeDTO paymentModeDTO = GetPaymentModeDTO(paymentRow);

                    if (paymentModeDTO.Gateway > 0)
                    {
                        LookupValuesDataHandler lookupValuesDataHandler = new LookupValuesDataHandler();
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookUpValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, paymentModeDTO.Gateway.ToString()));

                        List<LookupValuesDTO> lookupValuesDTOList = lookupValuesDataHandler.GetLookupValuesList(lookUpValuesSearchParams);
                        if (lookupValuesDTOList != null)
                        {
                            paymentModeDTO.PaymentGateway = lookupValuesDTOList[0];
                        }
                    }

                    paymentModeDTOList.Add(paymentModeDTO);
                }
            }

            log.LogMethodExit(paymentModeDTOList);
            return paymentModeDTOList;
        }



        /// <summary>
        /// Gets the PaymentModeDTO  matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of  PaymentModeDTO matching the search criteria</returns>
        public List<PaymentModeDTO> GetPaymentModeList(List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectPaymentModesQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<PaymentModeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : "  and ";

                        if (searchParameter.Key.Equals(PaymentModeDTO.SearchByParameters.PAYMENT_MODE)||
                            searchParameter.Key.Equals(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_GUID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID)
                        || searchParameter.Key.Equals(PaymentModeDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(PaymentModeDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(PaymentModeDTO.SearchByParameters.ISACTIVE))
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(PaymentModeDTO.SearchByParameters.OTP_VALIDATION_REQUIRED))
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID_LIST))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(PaymentModeDTO.SearchByParameters.PAYMENT_CHANNEL_NAME))
                        {
                            query.Append(joinOperartor + @"EXISTS (SELECT 1 
                                                                     FROM LookupView v, PaymentModeChannels pmc
                                                                    Where v.LookupName = 'PAYMENT_CHANNELS'
                                                                      and v.LookupValueId = pmc.LookupValueId
                                                                      and pmc.PaymentModeId = pm.PaymentModeId
                                                                      and v.LookupValue = " + dataAccessHandler.GetParameterName(searchParameter.Key) + ") ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                if (searchParameters.Count > 0)
                    selectPaymentModesQuery = selectPaymentModesQuery + query;
            }

            log.LogMethodExit();
            return GetPaymentModeList(selectPaymentModesQuery, parameters.ToArray());
        }


        /// <summary>
        /// return the record from the database
        /// Convert the datarow to PaymentModeDTO object
        /// </summary>
        /// <returns>return the PaymentModeDTO object</returns>
        private PaymentModeDTO GetPaymentModeDTO(DataRow paymentDataRow)
        {
            log.LogMethodEntry(paymentDataRow);

            PaymentModeDTO paymentModeDTO = new PaymentModeDTO(
                                                    paymentDataRow["PaymentModeId"] == DBNull.Value ? -1 : Convert.ToInt32(paymentDataRow["PaymentModeId"]),
                                                    paymentDataRow["PaymentMode"].ToString(),
                                                    paymentDataRow["isCreditCard"] == DBNull.Value || paymentDataRow["isCreditCard"].ToString().ToUpper() == "N" ? false : true,
                                                    paymentDataRow["creditCardSurchargePercentage"] == DBNull.Value ? -1 : Convert.ToDecimal(paymentDataRow["creditCardSurchargePercentage"]),
                                                    paymentDataRow["Guid"].ToString(),
                                                    paymentDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(paymentDataRow["SynchStatus"]),
                                                    paymentDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(paymentDataRow["site_id"]),
                                                    paymentDataRow["isCash"] == DBNull.Value || paymentDataRow["isCash"].ToString().ToUpper() == "N" ? false : true,
                                                    paymentDataRow["isDebitCard"] == DBNull.Value || paymentDataRow["isDebitCard"].ToString().ToUpper() == "N" ? false : true,
                                                    paymentDataRow["Gateway"] == DBNull.Value ? -1 : Convert.ToInt32(paymentDataRow["Gateway"]),
                                                    paymentDataRow["ManagerApprovalRequired"] == DBNull.Value || paymentDataRow["ManagerApprovalRequired"].ToString() == "N" ? false : true,
                                                    paymentDataRow["isRoundOff"] == DBNull.Value || paymentDataRow["isRoundOff"].ToString().ToUpper() == "N" ? false : true,
                                                   // paymentDataRow["POSAvailable"] == DBNull.Value || paymentDataRow["POSAvailable"].ToString() == "N" ? false : true,
                                                    paymentDataRow["POSAvailable"] == DBNull.Value ? true : Convert.ToBoolean(paymentDataRow["POSAvailable"]),
                                                    paymentDataRow["DisplayOrder"] == DBNull.Value ? -1 : Convert.ToInt32(paymentDataRow["DisplayOrder"]),
                                                    paymentDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(paymentDataRow["MasterEntityId"]),
                                                    paymentDataRow["ImageFileName"].ToString(),//Starts:ChinaICBC changes
                                                    paymentDataRow["IsQRCode"] == DBNull.Value ? 'N' : Convert.ToChar(paymentDataRow["IsQRCode"]),//Ends:ChinaICBC changes
                                                    paymentDataRow["PaymentReferenceMandatory"] == DBNull.Value || paymentDataRow["PaymentReferenceMandatory"].ToString() == "N" ? false : true,
                                                    paymentDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(paymentDataRow["IsActive"]),
                                                    paymentDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["CreatedBy"]),
                                                    paymentDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(paymentDataRow["CreationDate"]),
                                                    paymentDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["LastUpdatedBy"]),
                                                    paymentDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(paymentDataRow["LastUpdateDate"]),
                                                     paymentDataRow["Attribute1"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["Attribute1"]),
                                                     paymentDataRow["Attribute2"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["Attribute2"]),
                                                     paymentDataRow["Attribute3"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["Attribute3"]),
                                                     paymentDataRow["Attribute4"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["Attribute4"]),
                                                     paymentDataRow["Attribute5"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["Attribute5"]),
                                                    paymentDataRow["OTPValidation"] == DBNull.Value ? false : Convert.ToBoolean(paymentDataRow["OTPValidation"])
                                                 );
            log.LogMethodExit(paymentModeDTO);
            return paymentModeDTO;

        }

        /// <summary>
        /// Gets the PaymentModes data of passed paymentModeId
        /// </summary>
        /// <param name="paymentModeId">integer type parameter</param>
        /// <returns>Returns PaymentModesDTO</returns>
        public PaymentModeDTO GetPaymentModeDTO(int paymentModeId)
        {
            log.LogMethodEntry(paymentModeId);
            PaymentModeDTO returnValue = null;
            string query = SELECT_QUERY + "    WHERE pm.PaymentModeId = @PaymentModeId";
            SqlParameter parameter = new SqlParameter("@PaymentModeId", paymentModeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetPaymentModeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the lookup value data of passed lookup Value Id
        /// </summary>
        /// <param name="lookupValueId">integer type parameter</param>
        /// <returns>Returns List of PaymentModeDTO</returns>
        public List<PaymentModeDTO> GetPaymentModeListByLookupValueId(int lookupValueId)
        {
            log.LogMethodEntry(lookupValueId);
            List<PaymentModeDTO> paymentModeDTOList = new List<PaymentModeDTO>();
            string selectLookupValuesQuery = @" select pm.*
                                                    from PaymentModes pm
                                                    inner join PaymentModeChannels pc  
                                                    on pm.PaymentModeId = pc.PaymentModeId
                                                    and  LookupValueId =@lookupValueId ";

            SqlParameter[] selectLookupValuesParameters = new SqlParameter[1];
            selectLookupValuesParameters[0] = new SqlParameter("@lookupValueId", lookupValueId);
            paymentModeDTOList = GetPaymentModeList(selectLookupValuesQuery, selectLookupValuesParameters);
            log.LogMethodExit(paymentModeDTOList);
            return paymentModeDTOList;
        }

        /// <summary>
        /// Returns payment modes having payment gateways.
        /// </summary>
        /// <returns>List of payment modes</returns>
        public List<PaymentModeDTO> GetPaymentModesWithPaymentGateway(bool? posAvailable)
        {
            log.LogMethodEntry(posAvailable);
            List<PaymentModeDTO> list = null;
            string selectQuery = SELECT_QUERY + "  WHERE pm.Gateway IS NOT NULL";
            if (posAvailable != null && posAvailable.HasValue)
            {
                if (posAvailable.Value)
                {
                    selectQuery += " and isNull(POSAvailable, 0) = 1";
                }
                else
                {
                    selectQuery += " and isNull(POSAvailable, 0) = 0";
                }
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<PaymentModeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PaymentModeDTO paymentModesDTO = GetPaymentModeDTO(dataRow);
                    list.Add(paymentModesDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// GetSQLParameters method to build SQL parameters for Insert/ Update
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="PaymentModeChannelsDTO">PaymentModeChannelsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(PaymentModeDTO paymentModeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(paymentModeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@paymentModeId", paymentModeDTO.PaymentModeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isCreditCard", paymentModeDTO.IsCreditCard ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isDebitCard", paymentModeDTO.IsDebitCard ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@managerApprovalRequired", paymentModeDTO.ManagerApprovalRequired ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isRoundOff", paymentModeDTO.IsRoundOff ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isQRCode", paymentModeDTO.IsQRCode));// == false ? DBNull.Value : (Object)(paymentModeDTO.IsQRCode)
            parameters.Add(dataAccessHandler.GetSQLParameter("@paymentReferenceMandatory", paymentModeDTO.PaymentReferenceMandatory ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posAvailable", paymentModeDTO.POSAvailable ? (object)paymentModeDTO.POSAvailable : false));
            parameters.Add(dataAccessHandler.GetSQLParameter("@paymentMode", string.IsNullOrEmpty(paymentModeDTO.PaymentMode) ? DBNull.Value : (Object)paymentModeDTO.PaymentMode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@imageFileName", string.IsNullOrEmpty(paymentModeDTO.ImageFileName) ? DBNull.Value : (Object)paymentModeDTO.ImageFileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@creditCardSurchargePercentage", paymentModeDTO.CreditCardSurchargePercentage == -1 ? DBNull.Value : (Object)paymentModeDTO.CreditCardSurchargePercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gateway", paymentModeDTO.Gateway, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayOrder", paymentModeDTO.DisplayOrder == -1 ? DBNull.Value : (Object)paymentModeDTO.DisplayOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isCash", paymentModeDTO.IsCash ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", paymentModeDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", paymentModeDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute1", paymentModeDTO.Attribute1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute2", paymentModeDTO.Attribute2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute3", paymentModeDTO.Attribute3));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute4", paymentModeDTO.Attribute4));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute5", paymentModeDTO.Attribute5));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OTPValidation", paymentModeDTO.OTPValidation));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the payment mode record to the database
        /// </summary>
        /// <param name="paymentModeDTO">PaymentModeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns PaymentModeDTO</returns>
        public PaymentModeDTO InsertPaymentModes(PaymentModeDTO paymentModeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(paymentModeDTO, loginId, siteId);
            string insertPaymentModesQuery = @"insert into PaymentModes 
                                                        (                                                         
                                                        PaymentMode,
                                                        isCreditCard,                                                                                                              
                                                        CreditCardSurchargePercentage,
                                                        Guid,
                                                        site_id,
                                                        isCash,
                                                        isDebitCard,
                                                        Gateway,
                                                        ManagerApprovalRequired,
                                                        isRoundOff,
                                                        POSAvailable,
                                                        DisplayOrder,
                                                        ImageFileName,
                                                        MasterEntityId,
                                                        IsQRCode,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        PaymentReferenceMandatory,
                                                        IsActive,
                                                        Attribute1,                                        
                                                        Attribute2,                                        
                                                        Attribute3,                                        
                                                        Attribute4,                                        
                                                        Attribute5,
                                                        OTPValidation
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @paymentMode,
                                                        @isCreditCard,
                                                        @creditCardSurchargePercentage,
                                                        NewId(),
                                                        @siteId,
                                                        @isCash,
                                                        @isDebitCard,
                                                        @Gateway,
                                                        @managerApprovalRequired,
                                                        @isRoundOff,
                                                        @posAvailable,
                                                        @displayOrder,
                                                        @imageFileName,
                                                        @masterEntityId,
                                                        @isQRCode,
                                                        @createdBy,
                                                        GETDATE(),
                                                        @lastUpdatedBy,
                                                        GetDate(),
                                                        @paymentReferenceMandatory,
                                                        @isActive,
                                                        @Attribute1,                                        
                                                        @Attribute2,                                        
                                                        @Attribute3,                                        
                                                        @Attribute4,                                        
                                                        @Attribute5,
                                                        @OTPValidation
                                             )         SELECT * FROM PaymentModes WHERE PaymentModeId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertPaymentModesQuery, GetSQLParameters(paymentModeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPaymentModeDTO(paymentModeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting paymentModeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(paymentModeDTO);
            return paymentModeDTO;
        }

        /// <summary>
        /// Updates the payment mode record
        /// </summary>
        /// <param name="paymentModeDTO">PaymentModeDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the PaymentModeDTO</returns>
        public PaymentModeDTO UpdatePaymentModes(PaymentModeDTO paymentModeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(paymentModeDTO, loginId, siteId);
            string updatePaymentModesQuery = @"update PaymentModes 
                                         set PaymentMode= @paymentMode,
                                             isCreditCard= @isCreditCard,
                                             CreditCardSurchargePercentage = @creditCardSurchargePercentage,
                                             --site_id = @siteId,
                                             isCash = @isCash,
                                             isDebitCard = @isDebitCard,
                                             Gateway = @gateway,
                                             ManagerApprovalRequired = @managerApprovalRequired,
                                             isRoundOff = @isRoundOff,
                                             POSAvailable = @posAvailable,
                                             DisplayOrder = @displayOrder,
                                             ImageFileName = @imageFileName,
                                             MasterEntityId = @masterEntityId,
                                             IsQRCode = @isQRCode,
                                             LastUpdatedBy = @lastUpdatedBy,
                                             LastUpdateDate = GETDATE(),
                                             IsActive=@isActive,
                                             PaymentReferenceMandatory = @paymentReferenceMandatory,
                                             Attribute1 = @Attribute1 ,
                                             Attribute2 = @Attribute2 ,
                                             Attribute3 = @Attribute3 ,
                                             Attribute4 = @Attribute4 ,
                                             Attribute5 = @Attribute5,
                                             OTPValidation = @OTPValidation
                                       where PaymentModeId = @paymentModeId 
                            SELECT * FROM PaymentModes WHERE PaymentModeId = @paymentModeId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updatePaymentModesQuery, GetSQLParameters(paymentModeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPaymentModeDTO(paymentModeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating paymentModeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(paymentModeDTO);
            return paymentModeDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="paymentModeDTO">PaymentModeDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPaymentModeDTO(PaymentModeDTO paymentModeDTO, DataTable dt)
        {
            log.LogMethodEntry(paymentModeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                paymentModeDTO.PaymentModeId = Convert.ToInt32(dt.Rows[0]["PaymentModeId"]);
                paymentModeDTO.LastUpdateDate = dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"]);
                paymentModeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                paymentModeDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                paymentModeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                paymentModeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                paymentModeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
            }
            log.LogMethodExit();
        }

        ///// <summary>
        /////  Deletes the PaymentMode record
        ///// </summary>
        ///// <param name="paymentModeId">paymentModeId is passed as parameter</param>
        //internal void Delete(int paymentModeId)
        //{
        //    log.LogMethodEntry(paymentModeId);
        //    string query = @"DELETE  
        //                     FROM PaymentModes
        //                     WHERE PaymentModeId = @paymentModeId";
        //    SqlParameter parameter = new SqlParameter("@paymentModeId", paymentModeId);
        //    dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
        //    log.LogMethodExit();
        //}
        internal DateTime? GetPaymentModeLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"select max(LastUpdateDate) LastUpdatedDate from PaymentModes WHERE (site_id = @siteId or @siteId = -1)";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
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

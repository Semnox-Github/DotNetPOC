/********************************************************************************************
 * Project Name - Theme Data Handler
 * Description  - Data handler of the Theme class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Mar-2017   Lakshminarayana     Created
 *2.60        29-Apr-2019   Mushahid Faizan     Added Active_Flag DBSearchParameters Criteria in GetParafaitDefaultsDTOList,
                                                Handled Active_Flag in GetParafaitDefaultsDTO for manipulating string to bool datatype.
                                                Added Insert/Update and GetSqlParameters method.
 *2.70        4- Jul-2019   Girish Kundar       Modified : Added Missing who columns and LogMethodEntry and LogMethodExit
 *                                                         SQL Injection Issue fix. Added Active Flag as Search Parameter 
 *2.70.2        11-Dec-2019   Jinto Thomas        Removed siteid from update query                                                         
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Core.Utilities
{
    /// <summary>
    ///  Theme Data Handler - Select of ParafaitDefaults objects
    /// </summary>
    public class ParafaitDefaultsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<ParafaitDefaultsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ParafaitDefaultsDTO.SearchByParameters, string>
            {
                {ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_ID, "pd.default_value_id"},
                {ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "pd.default_value_name"},
                {ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME_LIST, "pd.default_value_name"},
                {ParafaitDefaultsDTO.SearchByParameters.SCREEN_GROUP, "pd.screen_group"},
                {ParafaitDefaultsDTO.SearchByParameters.ACTIVE_FLAG, "pd.active_flag"},
                {ParafaitDefaultsDTO.SearchByParameters.MASTER_ENTITY_ID,"pd.MasterEntityId"},
                {ParafaitDefaultsDTO.SearchByParameters.SITE_ID, "pd.site_id"}
            };
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * FROM parafait_defaults AS pd ";

        /// <summary>
        /// Default constructor of ParafaitDefaultsDataHandler class
        /// </summary>
        public ParafaitDefaultsDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of ParafaitDefaultsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ParafaitDefaultsDataHandler(SqlTransaction sqlTransaction) : this()
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ParafaitDefaultsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ParafaitDefaultsDTO</returns>
        private ParafaitDefaultsDTO GetParafaitDefaultsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ParafaitDefaultsDTO parafaitDefaultsDTO = new ParafaitDefaultsDTO(Convert.ToInt32(dataRow["default_value_id"]),
                                            dataRow["default_value_name"] == DBNull.Value ? string.Empty : dataRow["default_value_name"].ToString(),
                                            dataRow["description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["description"]),
                                            dataRow["default_value"] == DBNull.Value ? string.Empty : dataRow["default_value"].ToString(),
                                            dataRow["screen_group"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["screen_group"]),
                                            dataRow["datatype_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["datatype_id"]),
                                            dataRow["UserLevel"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UserLevel"]),
                                            dataRow["POSLevel"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["POSLevel"]),
                                            dataRow["Protected"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Protected"]),
                                            dataRow["active_flag"] == DBNull.Value ? true : dataRow["active_flag"].ToString() == "Y",
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                            );
            log.LogMethodExit();
            return parafaitDefaultsDTO;
        }

        /// <summary>
        /// returns the user and pos level overridden parafait default dictionary
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <param name="machineId">machine identifier</param>
        /// <param name="siteId">site identifier</param>
        /// <returns></returns>
        public ConcurrentDictionary<string, string> GetOverridenParafaitDefaultDictonary(int userId, int machineId, int siteId)
        {
            log.LogMethodEntry(userId, machineId, siteId);
            ConcurrentDictionary<string, string> parafaitDefaultsDTODictonary = new ConcurrentDictionary<string, string>();
            string query = @"select default_value_name, isnull(pos.optionvalue, us.optionValue) value 
                            from parafait_defaults pd 
                            left outer join ParafaitOptionValues pos 
                            on pd.default_value_id = pos.optionId 
                            and POSMachineId = @POSMachineId 
                            and pos.activeFlag = 'Y' 
                            left outer join ParafaitOptionValues us 
                            on pd.default_value_id = us.optionId 
                            and us.UserId = @UserId 
                            and us.activeFlag = 'Y' 
                            where pd.active_flag = 'Y' 
                            and (pd.site_id = @site_id or @site_id = -1)
                            and (pos.optionId is not null or us.optionId is not null)";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@site_id", siteId));
            parameters.Add(new SqlParameter("@UserId", userId));
            parameters.Add(new SqlParameter("@POSMachineId", machineId));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray());
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string defaultValueName = Convert.ToString(dataTable.Rows[i]["default_value_name"]);
                    string defaultValue = dataTable.Rows[i]["value"] == DBNull.Value ? string.Empty : Convert.ToString(dataTable.Rows[i]["value"]);
                    parafaitDefaultsDTODictonary[defaultValueName] = defaultValue;
                }
            }
            log.LogMethodExit(parafaitDefaultsDTODictonary);
            return parafaitDefaultsDTODictonary;
        }

        /// <summary>
        /// returns dictionary containing Transaction specific environment properties
        /// </summary>
        /// <param name="parafaitDefaultsDictionary">Object to be updated with Transaction Properties</param>
        /// <param name="userId">user id context</param>
        /// <param name="machineId">machine id context</param>
        /// <param name="siteId">site id context</param>
        public void GetOverridenTransactionDefaultDictonary(ConcurrentDictionary<string, string> parafaitDefaultsDictionary, int userId, int machineId, int siteId)
        {
            log.LogMethodEntry(parafaitDefaultsDictionary, userId, machineId, siteId);
            if (parafaitDefaultsDictionary == null)
                parafaitDefaultsDictionary = new ConcurrentDictionary<string, string>();
            string query = @"select top 1 username
                                  from users
                                  where user_id = @UserId
                                  and (site_id = @site_id or @site_id = -1)";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@site_id", siteId));
            parameters.Add(new SqlParameter("@UserId", userId));
            object userName = dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            //if (userName != null && userName != DBNull.Value)
            //    parafaitDefaultsDictionary["USERNAME"] = userName.ToString();
            //else
            //    parafaitDefaultsDictionary["USERNAME"] = string.Empty;
            query = @"select postypeId, POSName
                        from POSMachines
                        where POSMachineId = @posMachineId
                          and (site_id = @site_id or @site_id = -1)";
            parameters.Clear();
            parameters.Add(new SqlParameter("@posMachineId", machineId));
            parameters.Add(new SqlParameter("@site_id", siteId));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray());
            if (dataTable.Rows.Count > 0)
            {
                string defaultValue = dataTable.Rows[0]["postypeId"] == DBNull.Value ? string.Empty : Convert.ToString(dataTable.Rows[0]["postypeId"]);
                parafaitDefaultsDictionary["POSTYPEID"] = defaultValue;
                defaultValue = dataTable.Rows[0]["POSName"] == DBNull.Value ? string.Empty : Convert.ToString(dataTable.Rows[0]["POSName"]);
                parafaitDefaultsDictionary["POSMACHINE"] = defaultValue;
            }
        }

        /// <summary>
        /// returns dictionary containing Transaction specific environment properties
        /// </summary>
        /// <param name="parafaitDefaultsDictionary">Object to be updated with Transaction Properties</param>
        /// <param name="siteId">site id context</param>
        public void GetSiteTransactionDefaultDictonary(ConcurrentDictionary<string, string> parafaitDefaultsDictionary, int siteId)
        {
            log.LogMethodEntry(parafaitDefaultsDictionary, siteId);
            if (parafaitDefaultsDictionary == null)
                parafaitDefaultsDictionary = new ConcurrentDictionary<string, string>();
            string query = @"select top 1 product_id
                                  from Products p, product_type pt
                                  where product_type = 'CREDITCARDSURCHARGE'
                                  and p.product_type_id = pt.product_type_id
                                  and (p.site_id = @site_id or @site_id = -1)";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@site_id", siteId);
            object creditCardSurchargeProductId = dataAccessHandler.executeScalar(query, parameters, sqlTransaction);
            if (creditCardSurchargeProductId != null && creditCardSurchargeProductId != DBNull.Value)
                parafaitDefaultsDictionary["CREDIT_CARDSURCHARGE_PRODUCT_ID"] = creditCardSurchargeProductId.ToString();
            else
                parafaitDefaultsDictionary["CREDIT_CARDSURCHARGE_PRODUCT_ID"] = string.Empty;
            query = @"select top 1 product_id
                                  from Products p, product_type pt
                                  where product_type = 'CARDDEPOSIT'
                                  and p.product_type_id = pt.product_type_id
                                  and (p.site_id = @site_id or @site_id = -1)";
            SqlParameter[] cardDepositparameter = new SqlParameter[1];
            cardDepositparameter[0] = new SqlParameter("@site_id", siteId);
            object cardDepositProductId = dataAccessHandler.executeScalar(query, cardDepositparameter, sqlTransaction);
            if (cardDepositProductId != null && cardDepositProductId != DBNull.Value)
                parafaitDefaultsDictionary["CARD_DEPOSIT_PRODUCT_ID"] = cardDepositProductId.ToString();
            else
                parafaitDefaultsDictionary["CARD_DEPOSIT_PRODUCT_ID"] = string.Empty;
            query = @"select top 1 product_id
                                  from Products p, product_type pt
                                  where product_type = 'DEPOSIT'
                                  and p.product_type_id = pt.product_type_id
                                  and (p.site_id = @site_id or @site_id = -1)";
            SqlParameter[] rentalDepositparameter = new SqlParameter[1];
            rentalDepositparameter[0] = new SqlParameter("@site_id", siteId);
            object rentalDepositProductId = dataAccessHandler.executeScalar(query, rentalDepositparameter, sqlTransaction);
            if (rentalDepositProductId != null && rentalDepositProductId != DBNull.Value)
                parafaitDefaultsDictionary["RENTAL_DEPOSIT_PRODUCT_ID"] = rentalDepositProductId.ToString();
            else
                parafaitDefaultsDictionary["RENTAL_DEPOSIT_PRODUCT_ID"] = string.Empty;
            query = @"select top 1 product_id
                                  from Products p, product_type pt
                                  where product_type = 'LOCKERDEPOSIT'
                                  and p.product_type_id = pt.product_type_id
                                  and (p.site_id = @site_id or @site_id = -1)";
            SqlParameter[] lockerDepositparameter = new SqlParameter[1];
            lockerDepositparameter[0] = new SqlParameter("@site_id", siteId);
            object lockerDepositProductId = dataAccessHandler.executeScalar(query, lockerDepositparameter, sqlTransaction);
            if (lockerDepositProductId != null && lockerDepositProductId != DBNull.Value)
                parafaitDefaultsDictionary["LOCKER_DEPOSIT_PRODUCT_ID"] = lockerDepositProductId.ToString();
            else
                parafaitDefaultsDictionary["LOCKER_DEPOSIT_PRODUCT_ID"] = string.Empty;
            query = @"select top 1 product_id
                                  from Products p, product_type pt
                                  where product_type = 'EXCESSVOUCHERVALUE'
                                  and p.product_type_id = pt.product_type_id
                                  and (p.site_id = @site_id or @site_id = -1)";
            SqlParameter[] excessVoucherparameter = new SqlParameter[1];
            excessVoucherparameter[0] = new SqlParameter("@site_id", siteId);
            object excessVoucherProductId = dataAccessHandler.executeScalar(query, excessVoucherparameter, sqlTransaction);
            if (excessVoucherProductId != null && excessVoucherProductId != DBNull.Value)
                parafaitDefaultsDictionary["EXCESS_VOUCHERVALUE_PRODUCT_ID"] = excessVoucherProductId.ToString();
            else
                parafaitDefaultsDictionary["EXCESS_VOUCHERVALUE_PRODUCT_ID"] = string.Empty;
            query = @"select PaymentModeId 
                        from PaymentModes  
                       where isRoundOff = 'Y'
                         and (site_id = @site_id or @site_id = -1)";
            SqlParameter[] roundOffPaymentModeparameter = new SqlParameter[1];
            roundOffPaymentModeparameter[0] = new SqlParameter("@site_id", siteId);
            object roundOffPaymentModeId = dataAccessHandler.executeScalar(query, roundOffPaymentModeparameter, sqlTransaction);
            if (roundOffPaymentModeId != null && roundOffPaymentModeId != DBNull.Value)
                parafaitDefaultsDictionary["ROUNDOFF_PAYMENTMODE_ID"] = roundOffPaymentModeId.ToString();
            else
                parafaitDefaultsDictionary["ROUNDOFF_PAYMENTMODE_ID"] = string.Empty;
            string amountFormat = string.Empty;
            string currencySymbol = "Rs";
            string amountWithCurrencySymbol;
            if (parafaitDefaultsDictionary.ContainsKey("AMOUNT_FORMAT"))
            {
                amountFormat = parafaitDefaultsDictionary["AMOUNT_FORMAT"];
            }
            if (parafaitDefaultsDictionary.ContainsKey("CURRENCY_SYMBOL"))
            {
                currencySymbol = parafaitDefaultsDictionary["CURRENCY_SYMBOL"];
            }
            int RoundingPrecision = 0;
            try
            {
                if (amountFormat.Contains("#"))
                {
                    int pos = amountFormat.IndexOf(".");
                    if (pos >= 0)
                    {
                        RoundingPrecision = amountFormat.Length - pos - 1;
                    }
                    else
                    {
                        RoundingPrecision = 0;
                    }
                    amountWithCurrencySymbol = currencySymbol + " " + amountFormat;
                }
                else
                {
                    if (amountFormat.Length > 1)
                        RoundingPrecision = Convert.ToInt32(amountFormat.Substring(1));
                    else
                        RoundingPrecision = 0;
                    amountWithCurrencySymbol = "C" + " " + RoundingPrecision.ToString();
                }
                parafaitDefaultsDictionary["ROUNDING_PRECISION"] = RoundingPrecision.ToString();
                parafaitDefaultsDictionary["AMOUNT_WITH_CURRENCY_SYMBOL"] = amountWithCurrencySymbol;
            }
            catch { }

            query = @"Select top 1 CustomerKey from site
                       where (site_id = @site_id or @site_id = -1)";
            SqlParameter[] customerKeyParameter = new SqlParameter[1];
            customerKeyParameter[0] = new SqlParameter("@site_id", siteId);
            object customerKey = dataAccessHandler.executeScalar(query, customerKeyParameter, sqlTransaction);
            int key = -1;
            if (customerKey != DBNull.Value)
            {
                try
                {
                    key = Convert.ToInt32(double.Parse(customerKey.ToString().Trim()));
                }
                catch
                {
                    key = -1;
                }
            }
            parafaitDefaultsDictionary["MIFARE_CUSTOMER_KEY"] = key.ToString();
            int RoundOffAmountTo;
            double RoundOffAmountValue = 0;
            if (parafaitDefaultsDictionary.ContainsKey("ROUND_OFF_AMOUNT_TO"))
            {
                RoundOffAmountValue = double.Parse(parafaitDefaultsDictionary["ROUND_OFF_AMOUNT_TO"]);
            }
            try
            {
                RoundOffAmountTo = (int)(Math.Pow(10, RoundingPrecision) * RoundOffAmountValue);
                if (RoundOffAmountTo <= 0)
                    RoundOffAmountTo = 100;
            }
            catch
            {
                RoundOffAmountTo = 100;
            }
            parafaitDefaultsDictionary["ROUND_OFF_AMOUNT_TO"] = RoundOffAmountTo.ToString();
            query = @"select MaximumValue from Sequences where SeqName = 'Transaction'
                         and (site_id = @site_id or @site_id = -1)";
            SqlParameter[] maxTransactionParameter = new SqlParameter[1];
            maxTransactionParameter[0] = new SqlParameter("@site_id", siteId);
            object maxTransactionNumber = dataAccessHandler.executeScalar(query, maxTransactionParameter, sqlTransaction);
            if (maxTransactionNumber != null && maxTransactionNumber != DBNull.Value)
                parafaitDefaultsDictionary["MAX_TRANSACTION_NUMBER"] = maxTransactionNumber.ToString();
            else
                parafaitDefaultsDictionary["MAX_TRANSACTION_NUMBER"] = string.Empty;
            query = @"select top 1 1 from PostTransactionProcesses where Active = 1
                         and (site_id = @site_id or @site_id = -1)";
            SqlParameter[] postTransactionParameter = new SqlParameter[1];
            postTransactionParameter[0] = new SqlParameter("@site_id", siteId);
            object postTransactionProcessingExists = dataAccessHandler.executeScalar(query, postTransactionParameter, sqlTransaction);
            if (postTransactionProcessingExists != null && postTransactionProcessingExists != DBNull.Value)
                parafaitDefaultsDictionary["POSTTRANSACTION_PROCESSING_EXISTS"] = "1";
            else
                parafaitDefaultsDictionary["POSTTRANSACTION_PROCESSING_EXISTS"] = "0";
            query = @"select top 1 site_name from site
                         where (site_id = @site_id or @site_id = -1)";
            SqlParameter[] siteNameParameter = new SqlParameter[1];
            siteNameParameter[0] = new SqlParameter("@site_id", siteId);
            object siteName = dataAccessHandler.executeScalar(query, siteNameParameter, sqlTransaction);
            if (siteName != null && siteName != DBNull.Value)
                parafaitDefaultsDictionary["SITENAME"] = siteName.ToString();
            else
                parafaitDefaultsDictionary["SITENAME"] = string.Empty;
        }
        /// <summary>
        /// returns the site level parafait default dictionary without considering machine level and user level overrides
        /// </summary>
        /// <param name="siteId">site identifier</param>
        /// <returns></returns>
        public ConcurrentDictionary<string, string> GetSiteLevelParafaitDefaultDictonary(int siteId)
        {
            log.LogMethodEntry(siteId);
            ConcurrentDictionary<string, string> parafaitDefaultsDTODictonary = new ConcurrentDictionary<string, string>();
            string query = @"select default_value_name, pd.default_value value 
                            from parafait_defaults pd 
                            where pd.active_flag = 'Y' 
                            and (pd.site_id = @site_id or @site_id = -1)";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@site_id", siteId));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string defaultValueName = Convert.ToString(dataTable.Rows[i]["default_value_name"]);
                    string defaultValue = dataTable.Rows[i]["value"] == DBNull.Value ? string.Empty : Convert.ToString(dataTable.Rows[i]["value"]);
                    parafaitDefaultsDTODictonary[defaultValueName] = defaultValue;
                }
            }
            log.LogMethodExit(parafaitDefaultsDTODictonary);
            return parafaitDefaultsDTODictonary;
        }

       
        /// <summary>
        /// Gets the ParafaitDefaults data of passed default_value_id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ParafaitDefaultsDTO</returns>
        public ParafaitDefaultsDTO GetParafaitDefaultsDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id);
            ParafaitDefaultsDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE pd.default_value_id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetParafaitDefaultsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the ParafaitDefaultsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ParafaitDefaultsDTO matching the search criteria</returns>
        public List<ParafaitDefaultsDTO> GetParafaitDefaultsDTOList(List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ParafaitDefaultsDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_ID ||
                            searchParameter.Key == ParafaitDefaultsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ParafaitDefaultsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " =-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ParafaitDefaultsDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
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
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }

                selectQuery = selectQuery + query + " ORDER BY site_Id ASC ";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<ParafaitDefaultsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ParafaitDefaultsDTO parafaitDefaultsDTO = GetParafaitDefaultsDTO(dataRow);
                    list.Add(parafaitDefaultsDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        private List<SqlParameter> GetSQLParameters(ParafaitDefaultsDTO parafaitDefaultsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(parafaitDefaultsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@defaultValueId", parafaitDefaultsDTO.DefaultValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@defaultValue", parafaitDefaultsDTO.DefaultValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@defaultValueName", parafaitDefaultsDTO.DefaultValueName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", parafaitDefaultsDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@screenGroup", parafaitDefaultsDTO.ScreenGroup));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dataTypeId", parafaitDefaultsDTO.DataTypeId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userLevel", parafaitDefaultsDTO.UserLevel));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pOSLevel", parafaitDefaultsDTO.POSLevel));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isProtected", parafaitDefaultsDTO.IsProtected));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (parafaitDefaultsDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", parafaitDefaultsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;

        }

        /// <summary>
        /// Inserts the Parafait Default DTO record to the database
        /// </summary>
        /// <param name="parafaitDefaultsDTO">parafaitDefaultsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertDefaultValues(ParafaitDefaultsDTO parafaitDefaultsDTO, String userId, int siteId)
        {
            log.LogMethodEntry(parafaitDefaultsDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"insert into parafait_defaults 
                                                        (                                                         
                                                        default_value_name,
                                                        description, 
                                                        default_value,
                                                        active_flag,
                                                        screen_group,
                                                        datatype_id,
                                                        UserLevel,
                                                        POSLevel,                                                                                                             
                                                        Guid,
                                                        site_id,
                                                        Protected,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @defaultValueName,
                                                        @description,   
                                                        @defaultValue,
                                                        @isActive,
                                                        @screenGroup,
                                                        @dataTypeId,
                                                        @userLevel,
                                                        @pOSLevel,                                                      
                                                        NewId(),
                                                        @siteId,
                                                        @isProtected,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        GETDATE(),                                                        
                                                        @lastUpdatedBy,
                                                        GETDATE()
                                            )SELECT CAST(scope_identity() AS int)";


            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(parafaitDefaultsDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the Parafait Default Setting record in database 
        /// </summary>
        /// <param name="parafaitDefaultsDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int UpdateDefaultValues(ParafaitDefaultsDTO parafaitDefaultsDTO, String userId, int siteId)
        {

            log.LogMethodEntry(parafaitDefaultsDTO, userId, siteId);
            int rowsUpdated;
            string query = @"update parafait_defaults set
                                        default_value_name = @defaultValueName,
                                        description = @description,
                                        default_value = @defaultValue,
                                        active_flag = @isActive,
                                        screen_group = @screenGroup,
                                        datatype_id = @dataTypeId,
                                        UserLevel = @userLevel,
                                        POSLevel = @pOSLevel,
                                        -- site_id = @siteId,
                                        Protected = @isProtected,
                                        MasterEntityId = @masterEntityId,
                                        LastUpdatedBy = @lastUpdatedBy, 
                                        LastUpdatedDate = getdate()
                                        where default_value_id = @defaultValueId ";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(parafaitDefaultsDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        internal DateTime? GetParafaitDefaultModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate
                            FROM (
                            select max(LastUpdatedDate) LastUpdatedDate from parafait_defaults WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from ParafaitOptionValues WHERE (site_id = @siteId or @siteId = -1)) a";
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

        /// <summary>
        /// Returns the encrypted password data type Id
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns>data type id</returns>
        public int GetEncryptedPasswordDataTypeId(int siteId)
        {
            log.LogMethodEntry(siteId);
            int dataTypeId = -1;
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@site_id", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery("select datatype_id from defaults_datatype where datatype ='EncryptedPassword' and (site_id = @site_id or @site_id = -1) ORDER BY site_id ASC", sqlParameter, sqlTransaction);
            if (dataTable.Rows.Count > 0 )
            {
                dataTypeId = Convert.ToInt32(dataTable.Rows[0]["datatype_id"].ToString());
            }
            log.LogMethodExit(dataTypeId);
            return dataTypeId;
        }
    }
}
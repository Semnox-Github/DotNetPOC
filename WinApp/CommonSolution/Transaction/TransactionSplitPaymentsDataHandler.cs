/********************************************************************************************
 * Project Name - TransactionSplitPayments Data Handler
 * Description  - Data handler of the TransactionSplitPayments class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        20-Jun-2017   Lakshminarayana     Created 
 *2.70.2        11-Dec-2019   Jinto Thomas      Removed siteid from update query 
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    ///  TransactionSplitPayments Data Handler - Handles insert, update and select of  TransactionSplitPayments objects
    /// </summary>
    public class TransactionSplitPaymentsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<TransactionSplitPaymentsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionSplitPaymentsDTO.SearchByParameters, string>
            {
                {TransactionSplitPaymentsDTO.SearchByParameters.TRANSACTION_ID, "TrxId"},
                {TransactionSplitPaymentsDTO.SearchByParameters.SITE_ID, "site_id"}
            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of TransactionSplitPaymentsDataHandler class
        /// </summary>
        public TransactionSplitPaymentsDataHandler()
        {
            log.Debug("Starts-TransactionSplitPaymentsDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-TransactionSplitPaymentsDataHandler() default constructor.");
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

        private void PassParametersHelper(List<SqlParameter> parameters, TransactionSplitPaymentsDTO transactionSplitPaymentsDTO, string userId, int siteId)
        {
            log.Debug("Starts-PassParametersHelper() Method.");
            ParameterHelper(parameters, "@SplitId", transactionSplitPaymentsDTO.SplitId);
            ParameterHelper(parameters, "@TrxId", transactionSplitPaymentsDTO.TransactionId, true);
            ParameterHelper(parameters, "@UserReference", transactionSplitPaymentsDTO.UserReference);
            ParameterHelper(parameters, "@NoOfSplits", transactionSplitPaymentsDTO.NoOfSplits);
            ParameterHelper(parameters, "@LastUpdatedDate", ServerDateTime.Now);
            ParameterHelper(parameters, "@LastUpdatedBy", userId);
            ParameterHelper(parameters, "@site_id", siteId, true);
            ParameterHelper(parameters, "@MasterEntityId", transactionSplitPaymentsDTO.MasterEntityId, true);
            log.Debug("Ends-PassParametersHelper() Method.");
        }

        /// <summary>
        /// Inserts the TransactionSplitPayments record to the database
        /// </summary>
        /// <param name="transactionSplitPaymentsDTO">TransactionSplitPaymentsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertTransactionSplitPayments(TransactionSplitPaymentsDTO transactionSplitPaymentsDTO, string userId, int siteId)
        {
            log.Debug("Starts-InsertTransactionSplitPayments(transactionSplitPaymentsDTO, userId, siteId) Method.");
            int idOfRowInserted;
            string query = @"INSERT INTO TrxSplitPayments 
                                        ( 
                                            TrxId,
                                            UserReference,
                                            NoOfSplits,
                                            LastUpdatedDate,
                                            LastUpdatedBy,
                                            site_id,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @TrxId,
                                            @UserReference,
                                            @NoOfSplits,
                                            @LastUpdatedDate,
                                            @LastUpdatedBy,
                                            @site_id,
                                            @MasterEntityId
                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> parameters = new List<SqlParameter>();
            PassParametersHelper(parameters, transactionSplitPaymentsDTO, userId, siteId);
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, parameters.ToArray());
            }
            catch(Exception ex)
            {
                log.Error(ex);
                log.Error(transactionSplitPaymentsDTO.ToString());
                log.Error(query);
                throw ex;
            }


            log.Debug("Ends-InsertTransactionSplitPayments(transactionSplitPaymentsDTO, userId, siteId) Method.");
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the TransactionSplitPayments record
        /// </summary>
        /// <param name="transactionSplitPaymentsDTO">TransactionSplitPaymentsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateTransactionSplitPayments(TransactionSplitPaymentsDTO transactionSplitPaymentsDTO, string userId, int siteId)
        {
            log.Debug("Starts-UpdateTransactionSplitPayments(transactionSplitPaymentsDTO, userId, siteId) Method.");
            int rowsUpdated;
            string query = @"UPDATE TrxSplitPayments 
                             SET TrxId=@TrxId,
                                 UserReference=@UserReference,
                                 NoOfSplits=@NoOfSplits,
                                 LastUpdatedDate=@LastUpdatedDate,
                                 LastUpdatedBy=@LastUpdatedBy  
                                 -- site_id=@site_id
                             WHERE SplitId = @SplitId";
            List<SqlParameter> parameters = new List<SqlParameter>();
            PassParametersHelper(parameters, transactionSplitPaymentsDTO, userId, siteId);

            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, parameters.ToArray());
            }
            catch(Exception ex)
            {
                log.Error(ex);
                log.Error(transactionSplitPaymentsDTO.ToString());
                log.Error(query);
                throw ex;
            }
            log.Debug("Ends-UpdateTransactionSplitPayments(transactionSplitPaymentsDTO, userId, siteId) Method.");
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to TransactionSplitPaymentsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns TransactionSplitPaymentsDTO</returns>
        private TransactionSplitPaymentsDTO GetTransactionSplitPaymentsDTO(DataRow dataRow)
        {
            log.Debug("Starts-GetTransactionSplitPaymentsDTO(dataRow) Method.");
            TransactionSplitPaymentsDTO transactionSplitPaymentsDTO = new TransactionSplitPaymentsDTO(Convert.ToInt32(dataRow["SplitId"]),
                                                                                        dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                                                                        dataRow["UserReference"] == DBNull.Value ? "" : Convert.ToString(dataRow["UserReference"]),
                                                                                        dataRow["NoOfSplits"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["NoOfSplits"]),
                                                                                        dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                                                        dataRow["LastUpdatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                                                        dataRow["Guid"] == DBNull.Value ? "" : Convert.ToString(dataRow["Guid"]),
                                                                                        dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                                                        dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                                                        dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                                                        );
            log.Debug("Ends-GetTransactionSplitPaymentsDTO(dataRow) Method.");
            return transactionSplitPaymentsDTO;
        }

        /// <summary>
        /// Gets the TransactionSplitPayments data of passed requestId
        /// </summary>
        /// <param name="splitId">integer type parameter</param>
        /// <returns>Returns TransactionSplitPaymentsDTO</returns>
        public TransactionSplitPaymentsDTO GetTransactionSplitPaymentsDTO(int splitId)
        {
            log.Debug("Starts-GetTransactionSplitPaymentsDTO(splitId) Method.");
            TransactionSplitPaymentsDTO returnValue = null;
            string query = @"SELECT *
                            FROM TrxSplitPayments
                            WHERE SplitId = @SplitId";
            SqlParameter parameter = new SqlParameter("@SplitId", splitId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter });
            if(dataTable.Rows.Count > 0)
            {
                returnValue = GetTransactionSplitPaymentsDTO(dataTable.Rows[0]);
                log.Debug("Ends-GetTransactionSplitPaymentsDTO(splitId) Method by returnting TransactionSplitPaymentsDTO.");
            }
            else
            {
                log.Debug("Ends-GetTransactionSplitPaymentsDTO(splitId) Method by returnting null.");
            }
            return returnValue;
        }


        /// <summary>
        /// Gets the TransactionSplitPaymentsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TransactionSplitPaymentsDTO matching the search criteria</returns>
        public List<TransactionSplitPaymentsDTO> GetTransactionSplitPaymentsDTOList(List<KeyValuePair<TransactionSplitPaymentsDTO.SearchByParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetTransactionSplitPaymentsDTOList(searchParameters) Method.");
            List<TransactionSplitPaymentsDTO> list = null;
            int count = 0;
            string selectQuery = @"SELECT * FROM TrxSplitPayments";
            if((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach(KeyValuePair<TransactionSplitPaymentsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if(DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if(searchParameter.Key == TransactionSplitPaymentsDTO.SearchByParameters.TRANSACTION_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if(searchParameter.Key == TransactionSplitPaymentsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetTransactionSplitPaymentsDTOList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            log.Debug("Search query: " + selectQuery);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null);
            if(dataTable.Rows.Count > 0)
            {
                list = new List<TransactionSplitPaymentsDTO>();
                foreach(DataRow dataRow in dataTable.Rows)
                {
                    TransactionSplitPaymentsDTO transactionSplitPaymentsDTO = GetTransactionSplitPaymentsDTO(dataRow);
                    list.Add(transactionSplitPaymentsDTO);
                }
                log.Debug("Ends-GetTransactionSplitPaymentsDTOList(searchParameters) Method by returning list.");
            }
            else
            {
                log.Debug("Ends-GetTransactionSplitPaymentsDTOList(searchParameters) Method by returning null.");
            }
            return list;
        }
    }
}

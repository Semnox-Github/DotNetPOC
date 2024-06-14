/********************************************************************************************
 * Project Name - Products
 * Description  - Get and Insert or update methods for tax rules details.
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        14-Mar-2019   Jagan Mohana          Created 
 *2.70        29-Mar-2019   Nagesh Badiger        Added ParameterHelper() and modified InsertTransactionProfileTaxRules() and UpdateTransactionProfileTaxRules() and try and catch block
 *2.70        08-Apr-2019   Mushahid Faizan       Modified- Insert/Update Method, Added GetSQLParameters()
 *            25-Jul-2019   Mushahid Faizan       Added IsActive Parameter & Delete Method.
 *2.70.2        10-Dec-2019   Jinto Thomas         Removed siteid from update query
 *2.100.0     02-Sept-2020   Mushahid Faizan   siteId changes in GetSQLParameters().
 *2.110.00    08-Dec-2020   Prajwal S       Updated Three Tier
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Product
{
    public class TransactionProfileTaxRulesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TrxProfileTaxRules AS trxp";

        private static readonly Dictionary<TransactionProfileTaxRulesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionProfileTaxRulesDTO.SearchByParameters, string>
        {
            { TransactionProfileTaxRulesDTO.SearchByParameters.ID,"trxp.Id"},
            { TransactionProfileTaxRulesDTO.SearchByParameters.TRX_PROFILE_ID, "trxp.TrxProfileId"},
            { TransactionProfileTaxRulesDTO.SearchByParameters.TAX_ID, "trxp.TaxId"},
            { TransactionProfileTaxRulesDTO.SearchByParameters.TAX_STRUCTURE_ID, "trxp.TaxStructureId"},
            { TransactionProfileTaxRulesDTO.SearchByParameters.SITE_ID, "trxp.site_id"},
            { TransactionProfileTaxRulesDTO.SearchByParameters.ISACTIVE, "trxp.IsActive"},
            { TransactionProfileTaxRulesDTO.SearchByParameters.MASTER_ENTITY_ID, "trxp.MasterEntityId"},

        };

        /// <summary>
        /// Default constructor of TransactionProfileTaxRulesDataHandler class
        /// </summary>
        public TransactionProfileTaxRulesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating transactionProfileTaxRules Record.
        /// </summary>
        /// <param name="transactionProfileTaxRulesDTO">transactionProfileTaxRulesDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(TransactionProfileTaxRulesDTO transactionProfileTaxRulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionProfileTaxRulesDTO, loginId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", transactionProfileTaxRulesDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@trxProfileId", transactionProfileTaxRulesDTO.TrxProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxId", transactionProfileTaxRulesDTO.TaxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taxStructureId", transactionProfileTaxRulesDTO.TaxStructure, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@exempt", transactionProfileTaxRulesDTO.Exempt));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", transactionProfileTaxRulesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", transactionProfileTaxRulesDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Converts the Data row object to TransactionProfileTaxRulesDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private TransactionProfileTaxRulesDTO GetTransactionProfileTaxRulesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionProfileTaxRulesDTO transactionProfileTaxRulesDTO = new TransactionProfileTaxRulesDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["TrxProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxProfileId"]),
                                            dataRow["TaxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TaxId"]),
                                            dataRow["TaxStructureId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TaxStructureId"]),
                                            dataRow["Exempt"] == DBNull.Value ? "" : dataRow["Exempt"].ToString(),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                            );
            log.LogMethodExit(transactionProfileTaxRulesDTO);
            return transactionProfileTaxRulesDTO;
        }

        /// <summary>
        /// Gets the TransactionProfileTaxRulesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TransactionProfileTaxRulesDTO matching the search criteria</returns>
        public List<TransactionProfileTaxRulesDTO> GetTransactionProfileTaxRules(List<KeyValuePair<TransactionProfileTaxRulesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<TransactionProfileTaxRulesDTO> transactionProfileTaxRulesDTOList = new List<TransactionProfileTaxRulesDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TransactionProfileTaxRulesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(TransactionProfileTaxRulesDTO.SearchByParameters.ID)
                                || searchParameter.Key.Equals(TransactionProfileTaxRulesDTO.SearchByParameters.TRX_PROFILE_ID)
                                || searchParameter.Key.Equals(TransactionProfileTaxRulesDTO.SearchByParameters.TAX_ID)
                                || searchParameter.Key.Equals(TransactionProfileTaxRulesDTO.SearchByParameters.TAX_STRUCTURE_ID)
                                || searchParameter.Key.Equals(TransactionProfileTaxRulesDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionProfileTaxRulesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionProfileTaxRulesDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else 
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key),  searchParameter.Value));
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
                transactionProfileTaxRulesDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetTransactionProfileTaxRulesDTO(x)).ToList();
            }
            log.LogMethodExit(transactionProfileTaxRulesDTOList);
            return transactionProfileTaxRulesDTOList;
        }


        internal TransactionProfileTaxRulesDTO GetTransactionProfileTaxRulesId(int id)
        {
            log.LogMethodEntry(id);
            TransactionProfileTaxRulesDTO transactionProfileTaxRulesDTO = null;
            string query = SELECT_QUERY + @" WHERE trxp.Id = @Id";
            SqlParameter parameter = new SqlParameter("Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                transactionProfileTaxRulesDTO = GetTransactionProfileTaxRulesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(transactionProfileTaxRulesDTO);
            return transactionProfileTaxRulesDTO;
        }

        /// <summary>
        /// Inserts the tax rule record to the database
        /// </summary>
        /// <param name="transactionProfileTaxRulesDTO">TransactionProfileTaxRulesDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public TransactionProfileTaxRulesDTO Insert(TransactionProfileTaxRulesDTO transactionProfileTaxRulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionProfileTaxRulesDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[TrxProfileTaxRules]
                                                        (                                                         
                                                        TrxProfileId,
                                                        TaxId,
                                                        TaxStructureId,
                                                        Exempt,
                                                        site_id,
                                                        Guid,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        IsActive                                                        
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @trxProfileId,
                                                        @taxId,
                                                        @taxStructureId,
                                                        @exempt,
                                                        @siteId,
                                                        NewId(),
                                                        @masterEntityId,
                                                        @createdBy,
                                                        GETDATE(),                                                        
                                                        @lastUpdatedBy,                                                        
                                                        GetDate(),
                                                        @isActive
                                                        )
                                            SELECT * FROM TrxProfileTaxRules WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(transactionProfileTaxRulesDTO, loginId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    DataRow dataRow = dt.Rows[0];
                    transactionProfileTaxRulesDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                    transactionProfileTaxRulesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                    transactionProfileTaxRulesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                    transactionProfileTaxRulesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                    transactionProfileTaxRulesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                    transactionProfileTaxRulesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                    transactionProfileTaxRulesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                    //transactionProfileTaxRulesDTO.MasterEntityId = dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionProfileTaxRulesDTO);
            return transactionProfileTaxRulesDTO;
        }

        /// <summary>
        /// Updates the Transaction Profile Tax Rules record
        /// </summary>
        /// <param name="TransactionProfileTaxRulesDTO">TransactionProfileTaxRulesDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public TransactionProfileTaxRulesDTO Update(TransactionProfileTaxRulesDTO transactionProfileTaxRulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionProfileTaxRulesDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[TrxProfileTaxRules] set
                                            TrxProfileId=@trxProfileId,
                                             TaxId= @taxId,
                                             TaxStructureId= @taxStructureId,
                                             Exempt= @exempt,
                                             -- site_id = @siteId,
                                             MasterEntityId = @masterEntityId,
                                             LastUpdatedBy = @lastUpdatedBy,
                                             LastUpdateDate = GETDATE(),
                                             IsActive= @isActive
                                             where Id = @Id
                                     SELECT * FROM TrxProfileTaxRules WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(transactionProfileTaxRulesDTO, loginId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    DataRow dataRow = dt.Rows[0];
                    transactionProfileTaxRulesDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                    transactionProfileTaxRulesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                    transactionProfileTaxRulesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                    transactionProfileTaxRulesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                    transactionProfileTaxRulesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                    transactionProfileTaxRulesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                    transactionProfileTaxRulesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionProfileTaxRulesDTO);
            return transactionProfileTaxRulesDTO;
        }

        /// <summary>
        /// Based on the Id, appropriate TrxProfileTaxRules record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required 
        /// </summary>
        /// <param name="id">id is passed as parameter</param>
        internal void Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE  
                             FROM TrxProfileTaxRules
                             WHERE TrxProfileTaxRules.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - TransactionProfile Data Handler
 * Description  - Data handler of the TransactionProfile class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        1-Dec-2017   Lakshminarayana     Created
 *2.70.0      25-Jul-2019  Mushahid Faizan    Added Delete Method.
 *2.110.00    07-Dec-2020   Prajwal S       Updated Three Tier
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

namespace Semnox.Parafait.Product
{
    /// <summary>
    ///  TransactionProfile Data Handler - Handles insert, update and select of  TransactionProfile objects
    /// </summary>
    public class TransactionProfileDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TrxProfiles AS trx ";
        private static readonly Dictionary<TransactionProfileDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionProfileDTO.SearchByParameters, string>
            {
                {TransactionProfileDTO.SearchByParameters.TRANSACTION_PROFILE_ID, "trx.TrxProfileId"},
                {TransactionProfileDTO.SearchByParameters.PROFILE_NAME, "trx.ProfileName"},
                {TransactionProfileDTO.SearchByParameters.IS_ACTIVE, "trx.Active"},
                {TransactionProfileDTO.SearchByParameters.VERIFICATION_REQUIRED, "trx.VerificationRequired"},
                {TransactionProfileDTO.SearchByParameters.MASTER_ENTITY_ID,"trx.MasterEntityId"},
                {TransactionProfileDTO.SearchByParameters.SITE_ID, "trx.site_id"}
            };

        /// <summary>
        /// Default constructor of TransactionProfileDataHandler class
        /// </summary>
        public TransactionProfileDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating TransactionProfile Record.
        /// </summary>
        /// <param name="transactionProfileDTO">TransactionProfileDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        /// 
        private List<SqlParameter> GetSQLParameters(TransactionProfileDTO transactionProfileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionProfileDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionProfileId", transactionProfileDTO.TransactionProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProfileName", transactionProfileDTO.ProfileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Active", transactionProfileDTO.IsActive == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VerificationRequired", transactionProfileDTO.VerificationRequired == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", transactionProfileDTO.MasterEntityId, true));

            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the TransactionProfile record to the database
        /// </summary>
        /// <param name="transactionProfileDTO">TransactionProfileDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public TransactionProfileDTO Insert(TransactionProfileDTO transactionProfileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionProfileDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[TrxProfiles] 
                                        ( 
                                            ProfileName,
                                            Active,
                                            site_id,
                                            MasterEntityId,
                                            VerificationRequired,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate
                                        ) 
                                VALUES 
                                        (
                                            @ProfileName,
                                            @Active,
                                            @site_id,
                                            @MasterEntityId,
                                            @VerificationRequired,
                                            @CreatedBy,
                                            getdate(),
                                            @LastUpdatedBy,
                                            getdate()
                                         )
                              SELECT * FROM TrxProfiles WHERE TrxProfileId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(transactionProfileDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTransactionProfileDTO(transactionProfileDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionProfileDTO);
            return transactionProfileDTO;
        }

        /// <summary>
        /// Updates the TransactionProfile record
        /// </summary>
        /// <param name="transactionProfileDTO">TransactionProfileDTO type parameter</param>
        /// <param name="userId">User updating the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public TransactionProfileDTO Update(TransactionProfileDTO transactionProfileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionProfileDTO, loginId,  siteId);
            string query = @"UPDATE [dbo].[TrxProfiles] SET  
                              ProfileName=@ProfileName,
                                 Active = @Active,
                                 MasterEntityId=@MasterEntityId ,
                                 VerificationRequired = @VerificationRequired,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate = getdate()
                          where TrxProfileId = @TransactionProfileId
                             SELECT * FROM TrxProfiles WHERE TrxProfileId = @TransactionProfileId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(transactionProfileDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTransactionProfileDTO(transactionProfileDTO, dt);
               
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionProfileDTO);
            return transactionProfileDTO;
        }

        private void RefreshTransactionProfileDTO(TransactionProfileDTO transactionProfileDTO, DataTable dt)
        {
            log.LogMethodEntry(transactionProfileDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                transactionProfileDTO.TransactionProfileId = Convert.ToInt32(dt.Rows[0]["TrxProfileId"]);
                transactionProfileDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                transactionProfileDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                transactionProfileDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                transactionProfileDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                transactionProfileDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                transactionProfileDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }




        /// <summary>
        /// Converts the Data row object to TransactionProfileDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns TransactionProfileDTO</returns>
        private TransactionProfileDTO GetTransactionProfileDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionProfileDTO transactionProfileDTO = new TransactionProfileDTO(Convert.ToInt32(dataRow["TrxProfileId"]),
                                            dataRow["ProfileName"] == DBNull.Value ? "" : dataRow["ProfileName"].ToString(),
                                            dataRow["Active"] == DBNull.Value ? false : (Convert.ToString(dataRow["Active"])=="Y"? true: false),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["VerificationRequired"] == DBNull.Value ? false : (Convert.ToString(dataRow["VerificationRequired"]) == "Y" ? true : false),
                                            string.IsNullOrEmpty(dataRow["CreatedBy"].ToString()) ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            string.IsNullOrEmpty(dataRow["LastUpdatedBy"].ToString()) ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(transactionProfileDTO);
            return transactionProfileDTO;
        }

        /// <summary>
        /// Gets the TransactionProfile data of passed TransactionProfile Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns TransactionProfileDTO</returns>
        public TransactionProfileDTO GetTransactionProfileDTO(int id)
        {
            log.LogMethodEntry(id);
            TransactionProfileDTO transactionProfileDTO = null;
            string query = SELECT_QUERY + @" WHERE trx.TrxProfileId = @TransactionProfileId";
            SqlParameter parameter = new SqlParameter("@TransactionProfileId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                transactionProfileDTO = GetTransactionProfileDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(transactionProfileDTO);
            return transactionProfileDTO;
        }

        /// <summary>
        /// Gets the TransactionProfileDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TransactionProfileDTO matching the search criteria</returns>

        public List<TransactionProfileDTO> GetTransactionProfileDTOList(List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TransactionProfileDTO> transactionProfileDTOList = new List<TransactionProfileDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TransactionProfileDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TransactionProfileDTO.SearchByParameters.TRANSACTION_PROFILE_ID||
                            searchParameter.Key == TransactionProfileDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));

                        }
                        else if (searchParameter.Key == TransactionProfileDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                       
                        else if (searchParameter.Key == TransactionProfileDTO.SearchByParameters.IS_ACTIVE ||
                                 searchParameter.Key == TransactionProfileDTO.SearchByParameters.VERIFICATION_REQUIRED)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));

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
                transactionProfileDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetTransactionProfileDTO(x)).ToList();
            }
            log.LogMethodExit(transactionProfileDTOList);
            return transactionProfileDTOList;
        }

        internal DateTime? GetTransactionProfileModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastUpdateDate) LastUpdatedDate from TrxProfiles WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from TrxProfileTaxRules WHERE (site_id = @siteId or @siteId = -1)
                            ) a";
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
        ///  Deletes the transactionProfile record based on TransactionProfileId
        /// </summary>
        /// <param name="transactionProfileId">transactionProfileId is passed as parameter</param>
        internal void Delete(int transactionProfileId)
        {
            log.LogMethodEntry(transactionProfileId);
            string query = @"DELETE  
                             FROM TrxProfiles
                             WHERE TrxProfiles.TrxProfileId = @transactionProfileId";
            SqlParameter parameter = new SqlParameter("@transactionProfileId", transactionProfileId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - RedemptionCurrencyRuleDetail Data Handler
 * Description  - Data handler of the RedemptionCurrencyRuleDetail class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.2       19-Aug-2019    Dakshakh raj        Created
 *2.70.2       10-Dec-2019   Jinto Thomas         Removed siteid from update query
 *2.110.0     08-Oct-2020   Mushahid Faizan     Added GetRedemptionCurrencyRuleDetailDTOList() for pagination.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyRuleDetailDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"Select rcr.*, rc.CurrencyName, rc.ValueInTickets 
                                                 from RedemptionCurrencyRuleDetail as rcr 
                                                      left outer join RedemptionCurrency rc on rcr.CurrencyId = rc.CurrencyId ";

        /// <summary>
        /// Dictionary for searching Parameters for the RedemptionCurrencyRuleDetail object.
        /// </summary>
        private static readonly Dictionary<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string> DBSearchParameters = new Dictionary<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>
            {
                {RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_DETAIL_ID, "rcr.RedemptionCurrencyRuleDetailId"},
                {RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID, "rcr.RedemptionCurrencyRuleId"},
                {RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.CURRENCY_ID, "rcr.CurrencyId"},
                {RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.SITE_ID, "rcr.site_id"},
                {RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.IS_ACTIVE , "rcr.IsActive"},
                {RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.MASTER_ENTITY_ID, "rcr.MasterEntityId"},
                {RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID_LIST,"rcr.RedemptionCurrencyRuleId"}
            };

        /// <summary>
        /// Default constructor of RedemptionCurrencyRuleDetailDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RedemptionCurrencyRuleDetailDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating RedemptionCurrencyRuleDetailDTO parameters Record.
        /// </summary>
        /// <param name="redemptionCurrencyRuleDetailDTO">redemptionCurrencyRuleDetailDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDetailDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@redemptionCurrencyRuleDetailId", redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleDetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@redemptionCurrencyRuleId", redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@currencyId", redemptionCurrencyRuleDetailDTO.CurrencyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@quantity", redemptionCurrencyRuleDetailDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", redemptionCurrencyRuleDetailDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", redemptionCurrencyRuleDetailDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the RedemptionCurrencyRuleDetail record to the database
        /// </summary>
        /// <param name="redemptionCurrencyRuleDetailDTO">redemptionCurrencyRuleDetailDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>redemptionCurrencyRuleDetailDTO</returns>
        public RedemptionCurrencyRuleDetailDTO InsertRedemptionCurrencyRuleDetail(RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDetailDTO, loginId, siteId);
            string query = @"INSERT INTO RedemptionCurrencyRuleDetail 
                                        ( 
                                            RedemptionCurrencyRuleId,
                                            CurrencyId,
                                            Quantity,
                                            IsActive,
                                            Guid,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            site_id,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @redemptionCurrencyRuleId,
                                            @currencyId,
                                            @quantity,
                                            @isActive,
                                            NEWID(),
                                            @createdBy,
                                            GETDATE(),
                                            @lastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @masterEntityId
                                        )SELECT * FROM RedemptionCurrencyRuleDetail WHERE RedemptionCurrencyRuleDetailId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(redemptionCurrencyRuleDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionCurrencyRuleDetailDTO(redemptionCurrencyRuleDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting redemptionCurrencyRuleDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(redemptionCurrencyRuleDetailDTO);
            return redemptionCurrencyRuleDetailDTO;
        }

        /// <summary>
        /// Updates the RedemptionCurrencyRuleDetail record
        /// </summary>
        /// <param name="redemptionCurrencyRuleDetailDTO">RedemptionCurrencyRuleDetailDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>RedemptionCurrencyRuleDetailDTO</returns>
        public RedemptionCurrencyRuleDetailDTO UpdateRedemptionCurrencyRuleDetail(RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDetailDTO, loginId, siteId);
            string query = @"UPDATE RedemptionCurrencyRuleDetail 
                             SET RedemptionCurrencyRuleId=@redemptionCurrencyRuleId,
                                 CurrencyId=@currencyId,
                                 Quantity=@quantity,
                                 IsActive=@isActive, 
                                 LastUpdatedBy=@lastUpdatedBy,
                                 LastUpdateDate= GETDATE(),
                                 --site_id=@site_id,
                                 MasterEntityId=@masterEntityId
                             WHERE RedemptionCurrencyRuleDetailId = @redemptionCurrencyRuleDetailId
                             SELECT * FROM RedemptionCurrencyRuleDetail WHERE RedemptionCurrencyRuleDetailId = @redemptionCurrencyRuleDetailId ";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(redemptionCurrencyRuleDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionCurrencyRuleDetailDTO(redemptionCurrencyRuleDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating redemptionCurrencyRuleDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(redemptionCurrencyRuleDetailDTO);
            return redemptionCurrencyRuleDetailDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="redemptionCurrencyRuleDetailDTO">redemptionCurrencyRuleDetailDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshRedemptionCurrencyRuleDetailDTO(RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO, DataTable dt)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDetailDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleDetailId = Convert.ToInt32(dt.Rows[0]["RedemptionCurrencyRuleDetailId"]);
                redemptionCurrencyRuleDetailDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                redemptionCurrencyRuleDetailDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                redemptionCurrencyRuleDetailDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                redemptionCurrencyRuleDetailDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                redemptionCurrencyRuleDetailDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                redemptionCurrencyRuleDetailDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to RedemptionCurrencyRuleDetailDTO class type
        /// </summary>
        /// <param name="redemptionCurrencyRuleDetailDataRow">redemptionCurrencyRuleDetail DataRow</param>
        /// <returns>Returns redemptionCurrencyRuleDetail</returns>
        private RedemptionCurrencyRuleDetailDTO GetRedemptionCurrencyRuleDetailDTO(DataRow redemptionCurrencyRuleDetailDataRow)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDetailDataRow);
            RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDataObject = new RedemptionCurrencyRuleDetailDTO(Convert.ToInt32(redemptionCurrencyRuleDetailDataRow["RedemptionCurrencyRuleDetailId"]),
                                            redemptionCurrencyRuleDetailDataRow["RedemptionCurrencyRuleId"] == DBNull.Value ? -1 : Convert.ToInt32(redemptionCurrencyRuleDetailDataRow["RedemptionCurrencyRuleId"]),
                                            redemptionCurrencyRuleDetailDataRow["CurrencyId"] == DBNull.Value ? -1 : Convert.ToInt32(redemptionCurrencyRuleDetailDataRow["CurrencyId"]),
                                            redemptionCurrencyRuleDetailDataRow["Quantity"] == DBNull.Value ? (int?)null : Convert.ToInt32(redemptionCurrencyRuleDetailDataRow["Quantity"]),
                                            redemptionCurrencyRuleDetailDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean (redemptionCurrencyRuleDetailDataRow["IsActive"]),
                                            redemptionCurrencyRuleDetailDataRow["Guid"] == DBNull.Value ? string.Empty : redemptionCurrencyRuleDetailDataRow["Guid"].ToString(),
                                            redemptionCurrencyRuleDetailDataRow["CreatedBy"] == DBNull.Value ? string.Empty : redemptionCurrencyRuleDetailDataRow["CreatedBy"].ToString(),
                                            redemptionCurrencyRuleDetailDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(redemptionCurrencyRuleDetailDataRow["CreationDate"]),
                                            redemptionCurrencyRuleDetailDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : redemptionCurrencyRuleDetailDataRow["LastUpdatedBy"].ToString(),
                                            redemptionCurrencyRuleDetailDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(redemptionCurrencyRuleDetailDataRow["LastUpdateDate"]),
                                            redemptionCurrencyRuleDetailDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(redemptionCurrencyRuleDetailDataRow["site_id"]),
                                            redemptionCurrencyRuleDetailDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(redemptionCurrencyRuleDetailDataRow["SynchStatus"]),
                                            redemptionCurrencyRuleDetailDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(redemptionCurrencyRuleDetailDataRow["MasterEntityId"]),
                                            redemptionCurrencyRuleDetailDataRow["CurrencyName"] == DBNull.Value ? string.Empty : redemptionCurrencyRuleDetailDataRow["CurrencyName"].ToString(),
                                            redemptionCurrencyRuleDetailDataRow["ValueInTickets"] == DBNull.Value ? 0 : Convert.ToDouble(redemptionCurrencyRuleDetailDataRow["ValueInTickets"])
                                            );
            log.LogMethodExit(redemptionCurrencyRuleDetailDataObject);
            return redemptionCurrencyRuleDetailDataObject;
        }

        /// <summary>
        /// Gets the redemption Currency Rule Detail of passed RedemptionCurrencyRuleDetailId
        /// </summary>
        /// <param name="redemptionCurrencyRuleDetailId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns RedemptionCurrencyRuleDetailDTO</returns>
        public RedemptionCurrencyRuleDetailDTO GetRedemptionCurrencyRuleDetail(int redemptionCurrencyRuleDetailId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDetailId, sqlTransaction);
            RedemptionCurrencyRuleDetailDTO result = null;
            string selectRedemptionCurrencyRuleDetailQuery = SELECT_QUERY + @" WHERE RedemptionCurrencyRuleDetailId = @redemptionCurrencyRuleDetailId";
            SqlParameter parameter = new SqlParameter("@redemptionCurrencyRuleDetailId", redemptionCurrencyRuleDetailId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectRedemptionCurrencyRuleDetailQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetRedemptionCurrencyRuleDetailDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the RedemptionCurrencyRuleDetailDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns the list of RedemptionCurrencyRuleDetailDTO matching the search criteria</returns>
        public List<RedemptionCurrencyRuleDetailDTO> GetRedemptionCurrencyRuleDetailDTOList(List<KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParameters,int currentPage=0, int pageSize=0,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RedemptionCurrencyRuleDetailDTO> redemptionCurrencyRuleDetailDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_DETAIL_ID
                            || searchParameter.Key == RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.CURRENCY_ID
                            || searchParameter.Key == RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID
                            || searchParameter.Key == RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RedemptionCurrencyRuleDetailDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                //selectQuery = selectQuery + query;

                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
                if (currentPage > 0 && pageSize > 0)
                {
                    selectQuery += " ORDER BY rcr.RedemptionCurrencyRuleDetailId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                    selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
                }
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                redemptionCurrencyRuleDetailDTOList = new List<RedemptionCurrencyRuleDetailDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO = GetRedemptionCurrencyRuleDetailDTO(dataRow);
                    redemptionCurrencyRuleDetailDTOList.Add(redemptionCurrencyRuleDetailDTO);
                }
            }
            log.LogMethodExit(redemptionCurrencyRuleDetailDTOList);
            return redemptionCurrencyRuleDetailDTOList;
        }

    }
}

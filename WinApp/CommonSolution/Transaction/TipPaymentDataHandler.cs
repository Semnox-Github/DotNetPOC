/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler - TipPaymentDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      03-Jun-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the TipPaymentDataHandler data object class.  Handles insert, update and select of  TipPayment object
    /// </summary>
    public class TipPaymentDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TipPayment AS tp ";

        /// <summary>
        /// Dictionary for searching Parameters for the TipPayment object.
        /// </summary>
        private static readonly Dictionary<TipPaymentDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TipPaymentDTO.SearchByParameters, string>
        {
            { TipPaymentDTO.SearchByParameters.TIP_ID,"tp.TipId"},
            { TipPaymentDTO.SearchByParameters.TIP_ID_LIST,"tp.TipId"},
            { TipPaymentDTO.SearchByParameters.PAYMENT_ID,"tp.PaymentId"},
            { TipPaymentDTO.SearchByParameters.USER_ID,"tp.user_id"},
            { TipPaymentDTO.SearchByParameters.SITE_ID,"tp.site_id"},
            { TipPaymentDTO.SearchByParameters.MASTER_ENTITY_ID,"tp.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for TipPayment Data Handler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object </param>
        public TipPaymentDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating TipPayment Record.
        /// </summary>
        /// <param name="tipPaymentDTO">TipPaymentDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the List of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(TipPaymentDTO tipPaymentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tipPaymentDTO, loginId, siteId);
            loginId = "semnox";// login id is not null
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TipId", tipPaymentDTO.TipId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentId", tipPaymentDTO.PaymentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@user_id", tipPaymentDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SplitByPercentage", tipPaymentDTO.SplitByPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", tipPaymentDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Converts the Data row object to TipPaymentDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the TipPaymentDTO</returns>
        private TipPaymentDTO GetTipPaymentDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TipPaymentDTO tipPaymentDTO = new TipPaymentDTO(dataRow["TipId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TipId"]),
                                          dataRow["PaymentId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentId"]),
                                          dataRow["user_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["user_id"]),
                                          dataRow["SplitByPercentage"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SplitByPercentage"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                          dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                          );
            log.LogMethodExit(tipPaymentDTO);
            return tipPaymentDTO;
        }

        /// <summary>
        /// Gets the TipPayment data of passed id 
        /// </summary>
        /// <param name="tipId">tipId of TipPayment is passed as parameter</param>
        /// <returns>Returns TipPaymentDTO</returns>
        public TipPaymentDTO GetTipPaymentDTO(int tipId)
        {
            log.LogMethodEntry(tipId);
            TipPaymentDTO result = null;
            string query = SELECT_QUERY + @" WHERE tp.TipId = @TipId";
            SqlParameter parameter = new SqlParameter("@TipId", tipId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTipPaymentDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the TipPayment record
        /// </summary>
        /// <param name="tipPaymentDTO">TipPaymentDTO is passed as parameter</param>
        internal void Delete(TipPaymentDTO tipPaymentDTO)
        {
            log.LogMethodEntry(tipPaymentDTO);
            string query = @"DELETE  
                             FROM TipPayment
                             WHERE TipPayment.TipId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", tipPaymentDTO.TipId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            tipPaymentDTO.AcceptChanges();
            log.LogMethodExit();
        }


        /// <summary>
        ///  Inserts the record to the TipPayment Table.
        /// </summary>
        /// <param name="tipPaymentDTO">TipPaymentDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the TipPaymentDTO</returns>
        public TipPaymentDTO Insert(TipPaymentDTO tipPaymentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tipPaymentDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[TipPayment]
                           (PaymentId,
                            user_id,
                            SplitByPercentage,
                            Guid,
                            Site_id,
                            CreatedBy,
                            CreationDate,
                            MasterEntityId,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (@PaymentId,
                            @user_id,
                            @SplitByPercentage,
                            NEWID(),
                            @Site_id,
                            @CreatedBy,
                            GETDATE(),
                            @MasterEntityId,
                            @LastUpdatedBy,
                            GETDATE())
                                    SELECT * FROM TipPayment WHERE TipId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(tipPaymentDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTipPaymentDTO(tipPaymentDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TipPayment", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(tipPaymentDTO);
            return tipPaymentDTO;
        }
        /// <summary>
        ///  Updates the record to the TipPayment Table.
        /// </summary>
        /// <param name="tipPaymentDTO">TipPaymentDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the TipPaymentDTO</returns>
        public TipPaymentDTO Update(TipPaymentDTO tipPaymentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tipPaymentDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[TipPayment]
                            SET 
                            PaymentId         = @PaymentId,
                            user_id           = @user_id,
                            SplitByPercentage = @SplitByPercentage,
                            MasterEntityId    = @MasterEntityId,
                            LastUpdatedBy     = @LastUpdatedBy,
                            LastUpdateDate    = GETDATE()
                            WHERE tipId  = @TipId
                            SELECT * FROM TipPayment WHERE TipId =  @TipId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(tipPaymentDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTipPaymentDTO(tipPaymentDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating TipPayment", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(tipPaymentDTO);
            return tipPaymentDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="tipPaymentDTO">TipPaymentDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        private void RefreshTipPaymentDTO(TipPaymentDTO tipPaymentDTO, DataTable dt)
        {
            log.LogMethodEntry(tipPaymentDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                tipPaymentDTO.TipId = Convert.ToInt32(dt.Rows[0]["TipId"]);
                tipPaymentDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                tipPaymentDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                tipPaymentDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                tipPaymentDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                tipPaymentDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                tipPaymentDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of TipPaymentDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"> search Parameters</param>
        /// <returns>Returns the List of TipPaymentDTO</returns>
        public List<TipPaymentDTO> GetTipPaymentDTOList(List<KeyValuePair<TipPaymentDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TipPaymentDTO> tipPaymentDTOList = new List<TipPaymentDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TipPaymentDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TipPaymentDTO.SearchByParameters.TIP_ID
                            || searchParameter.Key == TipPaymentDTO.SearchByParameters.PAYMENT_ID
                            || searchParameter.Key == TipPaymentDTO.SearchByParameters.USER_ID
                            || searchParameter.Key == TipPaymentDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TipPaymentDTO.SearchByParameters.TIP_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == TipPaymentDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TipPaymentDTO tipPaymentDTO = GetTipPaymentDTO(dataRow);
                    tipPaymentDTOList.Add(tipPaymentDTO);
                }
            }
            log.LogMethodExit(tipPaymentDTOList);
            return tipPaymentDTOList;
        }
    }
}

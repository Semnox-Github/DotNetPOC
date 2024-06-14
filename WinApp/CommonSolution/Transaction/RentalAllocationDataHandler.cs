/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler - RentalAllocationDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      04-Jun-2019   Girish Kundar           Created 
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
    /// RentalAllocationDataHandler Data Handler - Handles insert, update and select of  RentalAllocation objects
    /// </summary>
    public class RentalAllocationDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM RentalAllocation AS ra ";

        /// <summary>
        /// Dictionary for searching Parameters for the RentalAllocation object.
        /// </summary>
        private static readonly Dictionary<RentalAllocationDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RentalAllocationDTO.SearchByParameters, string>
        {
            { RentalAllocationDTO.SearchByParameters.ID,"ra.Id"},
            { RentalAllocationDTO.SearchByParameters.ID_LIST,"ra.Id"},
            { RentalAllocationDTO.SearchByParameters.CARD_ID,"ra.CardId"},
            { RentalAllocationDTO.SearchByParameters.CARD_NUMBER,"ra.CardNumber"},
            { RentalAllocationDTO.SearchByParameters.ISSUED_BY,"ra.IssuedBy"},
            { RentalAllocationDTO.SearchByParameters.TRX_ID,"ra.TrxId"},
            { RentalAllocationDTO.SearchByParameters.PRODUCT_ID,"ra.ProductId"},
            { RentalAllocationDTO.SearchByParameters.TRX_LINE_ID,"ra.TrxLineId"},
            { RentalAllocationDTO.SearchByParameters.SITE_ID,"ra.site_id"},
            { RentalAllocationDTO.SearchByParameters.MASTER_ENTITY_ID,"ra.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for RentalAllocationDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object </param>
        public RentalAllocationDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating RentalAllocation Record.
        /// </summary>
        /// <param name="rentalAllocationDTO">RentalAllocationDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the List of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(RentalAllocationDTO rentalAllocationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(rentalAllocationDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", rentalAllocationDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", rentalAllocationDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", rentalAllocationDTO.TrxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IssuedBy", rentalAllocationDTO.IssuedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IssuedTime", rentalAllocationDTO.IssuedTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxLineId", rentalAllocationDTO.TrxLineId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DepositAmount", rentalAllocationDTO.DepositAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", rentalAllocationDTO.CardId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardNumber", rentalAllocationDTO.CardNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Refunded", rentalAllocationDTO.Refunded));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReturnTrxId", rentalAllocationDTO.ReturnTrxId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", rentalAllocationDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to KDSOrderEntryDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the RentalAllocationDTO</returns>
        private RentalAllocationDTO GetRentalAllocationDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            RentalAllocationDTO rentalAllocationDTO = new RentalAllocationDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                         dataRow["IssuedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["IssuedBy"]),
                                                         dataRow["IssuedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["IssuedTime"]),
                                                         dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                                         dataRow["TrxLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxLineId"]),
                                                         dataRow["DepositAmount"] == DBNull.Value ? (decimal?)null  : Convert.ToDecimal(dataRow["DepositAmount"]),
                                                         dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                                         dataRow["CardId"] == DBNull.Value ? -1 :Convert.ToInt32(dataRow["CardId"]),
                                                         dataRow["CardNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CardNumber"]),
                                                         dataRow["Refunded"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["Refunded"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationTime"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastUpdatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedTime"]),
                                                         dataRow["ReturnTrxId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ReturnTrxId"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                        );
            log.LogMethodExit(rentalAllocationDTO);
            return rentalAllocationDTO;
        }

        /// <summary>
        /// Gets the RentalAllocationDTO data of passed id 
        /// </summary>
        /// <param name="id">id of RentalAllocation is passed as parameter</param>
        /// <returns>Returns RentalAllocationDTO</returns>
        public RentalAllocationDTO GetRentalAllocationDTO(int id)
        {
            log.LogMethodEntry(id);
            RentalAllocationDTO result = null;
            string query = SELECT_QUERY + @" WHERE ra.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetRentalAllocationDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the RentalAllocation record
        /// </summary>
        /// <param name="rentalAllocationDTO">RentalAllocationDTO is passed as parameter</param>
        internal void Delete(RentalAllocationDTO rentalAllocationDTO)
        {
            log.LogMethodEntry(rentalAllocationDTO);
            string query = @"DELETE  
                             FROM RentalAllocation
                             WHERE RentalAllocation.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", rentalAllocationDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            rentalAllocationDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the RentalAllocation Table.
        /// </summary>
        /// <param name="rentalAllocationDTO">RentalAllocationDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the RentalAllocationDTO</returns>
        public RentalAllocationDTO Insert(RentalAllocationDTO rentalAllocationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(rentalAllocationDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[RentalAllocation]
                           (IssuedBy,
                            IssuedTime,
                            TrxId,
                            TrxLineId,
                            DepositAmount,
                            ProductId,
                            CardId,
                            CardNumber,
                            Refunded,
                            Guid,
                            Site_id,
                            CreatedBy,
                            CreationTime,
                            LastUpdatedBy,
                            LastUpdatedTime,
                            ReturnTrxId,
                            MasterEntityId)
                     VALUES
                           (@IssuedBy,
                            @IssuedTime,
                            @TrxId,
                            @TrxLineId,
                            @DepositAmount,
                            @ProductId,
                            @CardId,
                            @CardNumber,
                            @Refunded,
                            NEWID(),
                            @Site_id,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(),
                            @ReturnTrxId,
                            @MasterEntityId)
                                    SELECT * FROM RentalAllocation WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(rentalAllocationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRentalAllocationDTO(rentalAllocationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting RentalAllocationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(rentalAllocationDTO);
            return rentalAllocationDTO;
        }

        /// <summary>
        ///  Updates the record to the RentalAllocation Table.
        /// </summary>
        /// <param name="rentalAllocationDTO">RentalAllocationDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the RentalAllocationDTO</returns>
        public RentalAllocationDTO Update(RentalAllocationDTO rentalAllocationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(rentalAllocationDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[RentalAllocation]
                           SET
                            IssuedBy         = @IssuedBy,
                           IssuedTime       = @IssuedTime,
                           TrxId            = @TrxId,
                           TrxLineId        = @TrxLineId,
                           DepositAmount    = @DepositAmount,
                           ProductId        = @ProductId,
                           CardId           = @CardId,
                           CardNumber       = @CardNumber,
                           Refunded         = @Refunded,
                           LastUpdatedBy    = @LastUpdatedBy,
                           LastUpdatedTime  = GETDATE(),
                           MasterEntityId = @MasterEntityId,
                           ReturnTrxId      = @ReturnTrxId
                           Where Id = @Id
                            SELECT * FROM RentalAllocation WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(rentalAllocationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRentalAllocationDTO(rentalAllocationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting RentalAllocationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(rentalAllocationDTO);
            return rentalAllocationDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="rentalAllocationDTO">RentalAllocationDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
     
        private void RefreshRentalAllocationDTO(RentalAllocationDTO rentalAllocationDTO, DataTable dt)
        {
            log.LogMethodEntry(rentalAllocationDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                rentalAllocationDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                rentalAllocationDTO.LastUpdatedDate = dataRow["LastUpdatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedTime"]);
                rentalAllocationDTO.CreationDate = dataRow["CreationTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationTime"]);
                rentalAllocationDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                rentalAllocationDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                rentalAllocationDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                rentalAllocationDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of RentalAllocationDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Return the List of RentalAllocationDTO</returns>
        public List<RentalAllocationDTO> GetRentalAllocationDTOList(List<KeyValuePair<RentalAllocationDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<RentalAllocationDTO> rentalAllocationDTOList = new List<RentalAllocationDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RentalAllocationDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RentalAllocationDTO.SearchByParameters.ID
                            || searchParameter.Key == RentalAllocationDTO.SearchByParameters.PRODUCT_ID
                            || searchParameter.Key == RentalAllocationDTO.SearchByParameters.TRX_ID
                            || searchParameter.Key == RentalAllocationDTO.SearchByParameters.TRX_LINE_ID
                            || searchParameter.Key == RentalAllocationDTO.SearchByParameters.CARD_ID
                            || searchParameter.Key == RentalAllocationDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RentalAllocationDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RentalAllocationDTO.SearchByParameters.ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == RentalAllocationDTO.SearchByParameters.CARD_NUMBER
                                || searchParameter.Key == RentalAllocationDTO.SearchByParameters.ISSUED_BY)
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RentalAllocationDTO rentalAllocationDTO = GetRentalAllocationDTO(dataRow);
                    rentalAllocationDTOList.Add(rentalAllocationDTO);
                }
            }
            log.LogMethodExit(rentalAllocationDTOList);
            return rentalAllocationDTOList;
        }
    }
}

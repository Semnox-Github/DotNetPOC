/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler -CheckInPriceDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      16-May-2019   Girish Kundar           Created 
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
    /// CheckInPriceDataHandler Data Handler - Handles insert, update and select of  CheckInPrices object
    /// </summary>
    public class CheckInPriceDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CheckInPrices AS cip ";
        /// <summary>
        /// Dictionary for searching Parameters for the CheckInPrice object.
        /// </summary>
        private static readonly Dictionary<CheckInPriceDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CheckInPriceDTO.SearchByParameters, string>
        {
            { CheckInPriceDTO.SearchByParameters.ID,"cip.Id"},
            { CheckInPriceDTO.SearchByParameters.ID_LIST,"cip.Id"},
            { CheckInPriceDTO.SearchByParameters.PRODUCT_ID,"cip.ProductId"},
            { CheckInPriceDTO.SearchByParameters.TIME_SLAB,"cip.TimeSlab"},
            { CheckInPriceDTO.SearchByParameters.SITE_ID,"cip.site_id"},
            { CheckInPriceDTO.SearchByParameters.MASTER_ENTITY_ID,"cip.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for CheckInPriceDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public CheckInPriceDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CheckInPrice Record.
        /// </summary>
        /// <param name="checkInPriceDTO">CheckInPriceDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(CheckInPriceDTO checkInPriceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(checkInPriceDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", checkInPriceDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", checkInPriceDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeSlab", checkInPriceDTO.TimeSlab));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Price", checkInPriceDTO.Price));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", checkInPriceDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to CheckInPriceDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the CheckInPriceDTO</returns>
        private CheckInPriceDTO GetCheckInPriceDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CheckInPriceDTO checkInPriceDTO = new CheckInPriceDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                         dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                                         dataRow["TimeSlab"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TimeSlab"]),
                                                         dataRow["Price"] == DBNull.Value ? -1 : Convert.ToDecimal(dataRow["Price"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"])
                                                        );
            log.LogMethodExit(checkInPriceDTO);
            return checkInPriceDTO;
        }

        /// <summary>
        /// Gets the CheckInPrice data of passed id 
        /// </summary>
        /// <param name="id">id of CheckInPrice is passed as parameter</param>
        /// <returns>Returns CheckInPriceDTO</returns>
        public CheckInPriceDTO GetCheckInPriceDTO(int id)
        {
            log.LogMethodEntry(id);
            CheckInPriceDTO result = null;
            string query = SELECT_QUERY + @" WHERE cip.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCheckInPriceDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the CheckInPrice record
        /// </summary>
        /// <param name="checkInPriceDTO">CheckInPriceDTO is passed as parameter</param>
        internal void Delete(CheckInPriceDTO checkInPriceDTO)
        {
            log.LogMethodEntry(checkInPriceDTO);
            string query = @"DELETE  
                             FROM CheckInPrices
                             WHERE CheckInPrices.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", checkInPriceDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            checkInPriceDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the checkInPrice Table.
        /// </summary>
        /// <param name="checkInPriceDTO">CheckInPriceDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the CheckInPriceDTO </returns>
        public CheckInPriceDTO Insert(CheckInPriceDTO checkInPriceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(checkInPriceDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[CheckInPrices]
                           (ProductId,
                            TimeSlab,
                            Price,
                            site_id,
                            Guid,
                            LastUpdateDate,
                            LastUpdatedBy,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate)
                     VALUES
                           (@ProductId,
                           @TimeSlab,
                           @Price,
                           @site_id,
                           NEWID(),
                           GETDATE(),
                           @LastUpdatedBy,
                           @MasterEntityId,
                           @CreatedBy,
                           GETDATE())
                                    SELECT * FROM CheckInPrices WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(checkInPriceDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCheckInPriceDTO(checkInPriceDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CheckInDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(checkInPriceDTO);
            return checkInPriceDTO;
        }

        /// <summary>
        ///  Updates the record to the CheckInPrices Table.
        /// </summary>
        /// <param name="checkInPriceDTO">CheckInPriceDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the CheckInPriceDTO </returns>
        public CheckInPriceDTO Update(CheckInPriceDTO checkInPriceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(checkInPriceDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[CheckInPrices]
                           SET
                            ProductId = @ProductId
                           ,TimeSlab = @TimeSlab
                           ,Price = @Price
                           ,LastUpdateDate = GETDATE()
                           ,LastUpdatedBy = @LastUpdatedBy
                           ,MasterEntityId = @MasterEntityId
                            WHERE Id  = @Id
                                    SELECT * FROM CheckInPrices WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(checkInPriceDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCheckInPriceDTO(checkInPriceDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CheckInPriceDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(checkInPriceDTO);
            return checkInPriceDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="checkInPriceDTO">CheckInPriceDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCheckInPriceDTO(CheckInPriceDTO checkInPriceDTO, DataTable dt)
        {
            log.LogMethodEntry(checkInPriceDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                checkInPriceDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                checkInPriceDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                checkInPriceDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                checkInPriceDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                checkInPriceDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                checkInPriceDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                checkInPriceDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of CheckInPriceDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of CheckInPriceDTO</returns>
        public List<CheckInPriceDTO> GetCheckInPriceDTOList(List<KeyValuePair<CheckInPriceDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CheckInPriceDTO> checkInPriceDTOList = new List<CheckInPriceDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CheckInPriceDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CheckInPriceDTO.SearchByParameters.ID
                            || searchParameter.Key == CheckInPriceDTO.SearchByParameters.PRODUCT_ID
                            || searchParameter.Key == CheckInPriceDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CheckInPriceDTO.SearchByParameters.ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CheckInPriceDTO.SearchByParameters.SITE_ID)
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
                    CheckInPriceDTO checkInPriceDTO = GetCheckInPriceDTO(dataRow);
                    checkInPriceDTOList.Add(checkInPriceDTO);
                }
            }
            log.LogMethodExit(checkInPriceDTOList);
            return checkInPriceDTOList;
        }

    }
}

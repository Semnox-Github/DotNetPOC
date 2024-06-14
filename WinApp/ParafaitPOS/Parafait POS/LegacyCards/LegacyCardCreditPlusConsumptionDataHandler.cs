/*/********************************************************************************************
 * Project Name - LegacyCardCreditPlusConsumptionDataHandler
 * Description  - Data Handler for Legacy Card Credit Plus Consumption
 *
 **************
 ** Version Log
 **************
 *Version     Date Modified      By          Remarks
 *********************************************************************************************
 *2.130.4     21-Feb-2022        Dakshakh    Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parafait_POS
{
    /// <summary>
    ///  LegacyCardCreditPlusConsumption Data Handler - Handles insert, update and select of  LegacyCardCreditPlusConsumption objects
    /// </summary>
    public class LegacyCardCreditPlusConsumptionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;

        private static readonly Dictionary<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string>
            {
                {LegacyCardCreditPlusConsumptionDTO.SearchByParameters.LEGACY_CARDCREDIT_PLUS_CONSUMPTION_ID, "LegacyCardCreditPlusConsumption.LegacyCardCreditPlusConsumptionId"},
                {LegacyCardCreditPlusConsumptionDTO.SearchByParameters.LEGACY_CARD_CREDIT_PLUS_ID, "LegacyCardCreditPlusConsumption.LegacyCardCreditPlusId"},
                {LegacyCardCreditPlusConsumptionDTO.SearchByParameters.SITE_ID, "LegacyCardCreditPlusConsumption.site_id"},
                {LegacyCardCreditPlusConsumptionDTO.SearchByParameters.MASTER_ENTITY_ID, "LegacyCardCreditPlusConsumption.MasterEntityId"},
                {LegacyCardCreditPlusConsumptionDTO.SearchByParameters.ISACTIVE, "LegacyCardCreditPlusConsumption.IsActive"},
                {LegacyCardCreditPlusConsumptionDTO.SearchByParameters.CARD_ID_LIST, "LegacyCardCreditPlus.LegacyCard_id"}
            };
        private readonly DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of LegacyCardCreditPlusConsumptionDataHandler class
        /// </summary>
        public LegacyCardCreditPlusConsumptionDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LegacyCardCreditPlusConsumption Record.
        /// </summary>
        /// <param name="LegacyCardCreditPlusConsumptionDTO">LegacyCardCreditPlusConsumptionDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(LegacyCardCreditPlusConsumptionDTO LegacyCardCreditPlusConsumptionDTO, string userId, int siteId)
        {
            log.LogMethodEntry(LegacyCardCreditPlusConsumptionDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LegacyCardCreditPlusConsumptionId", LegacyCardCreditPlusConsumptionDTO.LegacyCardCreditPlusConsumptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LegacyCardCreditPlusId", LegacyCardCreditPlusConsumptionDTO.LegacyCardCreditPlusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PosCounterName", LegacyCardCreditPlusConsumptionDTO.PosCounterName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", LegacyCardCreditPlusConsumptionDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductName", LegacyCardCreditPlusConsumptionDTO.ProductName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameProfileName", LegacyCardCreditPlusConsumptionDTO.GameProfileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameName", LegacyCardCreditPlusConsumptionDTO.GameName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountPercentage", LegacyCardCreditPlusConsumptionDTO.DiscountPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountedPrice", LegacyCardCreditPlusConsumptionDTO.DiscountedPrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ConsumptionBalance", LegacyCardCreditPlusConsumptionDTO.ConsumptionBalance));
            parameters.Add(dataAccessHandler.GetSQLParameter("@QuantityLimit", LegacyCardCreditPlusConsumptionDTO.QuantityLimit));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Categoryname", LegacyCardCreditPlusConsumptionDTO.Categoryname));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DiscountAmount", LegacyCardCreditPlusConsumptionDTO.DiscountAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", LegacyCardCreditPlusConsumptionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", LegacyCardCreditPlusConsumptionDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ConsumptionQty", LegacyCardCreditPlusConsumptionDTO.ConsumptionQty));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the LegacyCardCreditPlusConsumption record to the database
        /// </summary>
        /// <param name="LegacyCardCreditPlusConsumptionDTO">LegacyCardCreditPlusConsumptionDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted LegacyCardCreditPlusConsumption record</returns>
        public LegacyCardCreditPlusConsumptionDTO InsertLegacyCardCreditPlusConsumption(LegacyCardCreditPlusConsumptionDTO LegacyCardCreditPlusConsumptionDTO, string userId, int siteId)
        {
            log.LogMethodEntry(LegacyCardCreditPlusConsumptionDTO, userId, siteId);
            string query = @"INSERT INTO LegacyCardCreditPlusConsumption 
                                        ( 
                                            LegacyCardCreditPlusId,
                                            PosCounterName,
                                            ExpiryDate,
                                            ProductName,
                                            GameName,
                                            GameProfileName,
                                            DiscountPercentage,
                                            DiscountedPrice,
                                            ConsumptionBalance,
                                            QuantityLimit,
                                            CategoryName,
                                            DiscountAmount,
                                            ConsumptionQty
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastupdatedDate,
                                            site_id,
                                            MasterEntityId,
                                        ) 
                                VALUES 
                                        (
                                            @LegacyCardCreditPlusId,
                                            @PosCounterName,
                                            @ExpiryDate,
                                            @ProductName,
                                            @GameName,
                                            @GameProfileName,
                                            @DiscountPercentage,
                                            @DiscountedPrice,
                                            @ConsumptionBalance,
                                            @QuantityLimit,
                                            @CategoryName,
                                            @DiscountAmount,
                                            @ConsumptionQty
                                            @IsActive,
                                            @CreatedBy,
                                            GetDate(),
                                            @LastUpdatedBy,
                                            GetDate(),
                                            @site_id,
                                            @MasterEntityId,
                                        )
                                        SELECT * FROM LegacyCardCreditPlusConsumption WHERE LegacyCardCreditPlusConsumptionId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(LegacyCardCreditPlusConsumptionDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshLegacyCardCreditPlusConsumptionDTO(LegacyCardCreditPlusConsumptionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit(LegacyCardCreditPlusConsumptionDTO);
            return LegacyCardCreditPlusConsumptionDTO;
        }

        /// <summary>
        /// Updates the LegacyCardCreditPlusConsumption record
        /// </summary>
        /// <param name="LegacyCardCreditPlusConsumptionDTO">LegacyCardCreditPlusConsumptionDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns updated LegacyCardCreditPlusConsumption record</returns>
        public LegacyCardCreditPlusConsumptionDTO UpdateLegacyCardCreditPlusConsumption(LegacyCardCreditPlusConsumptionDTO LegacyCardCreditPlusConsumptionDTO, string userId, int siteId)
        {
            log.LogMethodEntry(LegacyCardCreditPlusConsumptionDTO, userId, siteId);
            string query = @"UPDATE CardCreditPlusConsumption 
                             SET LegacyCardCreditPlusId = @LegacyCardCreditPlusId,
                                 PosCounterName = @PosCounterName,
                                 ExpiryDate = @ExpiryDate,
                                 ProductName = @ProductName,
                                 GameProfileName = @GameProfileName,
                                 GameName = @GameName,
                                 DiscountPercentage = @DiscountPercentage,
                                 DiscountedPrice = @DiscountedPrice,
                                 ConsumptionBalance = @ConsumptionBalance,
                                 QuantityLimit = @QuantityLimit,
                                 CategoryName = @CategoryName,
                                 DiscountAmount = @DiscountAmount,
                                 ConsumptionQty = @ConsumptionQty
                                 LastupdatedDate = GETDATE(),
                                 LastUpdatedBy = @LastUpdatedBy,
                                 MasterEntityId = @MasterEntityId,
                                 IsActive=@IsActive,
                             WHERE LegacyCardCreditPlusConsumptionId = @LegacyCardCreditPlusConsumptionId 
                             SELECT * FROM LegacyCardCreditPlusConsumption WHERE LegacyCardCreditPlusConsumptionId = @LegacyCardCreditPlusConsumptionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(LegacyCardCreditPlusConsumptionDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshLegacyCardCreditPlusConsumptionDTO(LegacyCardCreditPlusConsumptionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(LegacyCardCreditPlusConsumptionDTO);
            return LegacyCardCreditPlusConsumptionDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="LegacyCardCreditPlusConsumptionDTO">LegacyCardCreditPlusConsumptionDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshLegacyCardCreditPlusConsumptionDTO(LegacyCardCreditPlusConsumptionDTO LegacyCardCreditPlusConsumptionDTO, DataTable dt)
        {
            log.LogMethodEntry(LegacyCardCreditPlusConsumptionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                LegacyCardCreditPlusConsumptionDTO.LegacyCardCreditPlusConsumptionId = Convert.ToInt32(dt.Rows[0]["LegacyCardCreditPlusConsumptionId"]);
                LegacyCardCreditPlusConsumptionDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                LegacyCardCreditPlusConsumptionDTO.LastUpdateDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                LegacyCardCreditPlusConsumptionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                LegacyCardCreditPlusConsumptionDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                LegacyCardCreditPlusConsumptionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                LegacyCardCreditPlusConsumptionDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to LegacyCardCreditPlusConsumptionDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns LegacyCardCreditPlusConsumptionDTO</returns>
        private LegacyCardCreditPlusConsumptionDTO GetLegacyCardCreditPlusConsumptionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LegacyCardCreditPlusConsumptionDTO LegacyCardCreditPlusConsumptionDTO = new LegacyCardCreditPlusConsumptionDTO(Convert.ToInt32(dataRow["LegacyCardCreditPlusConsumptionId"]),
                                            dataRow["LegacyCardCreditPlusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LegacyCardCreditPlusId"]),
                                            dataRow["PosCounterName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PosCounterName"]),
                                            dataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                            dataRow["ProductName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProductName"]),
                                            dataRow["GameName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GameName"]),
                                            dataRow["GameProfileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GameProfileName"]),
                                            dataRow["DiscountPercentage"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["DiscountPercentage"]),
                                            dataRow["DiscountedPrice"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["DiscountedPrice"]),
                                            dataRow["ConsumptionBalance"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ConsumptionBalance"]),
                                            dataRow["QuantityLimit"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["QuantityLimit"]),
                                            dataRow["CategoryName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CategoryName"]),
                                            dataRow["DiscountAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["DiscountAmount"]),
                                            dataRow["ConsumptionQty"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ConsumptionQty"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(LegacyCardCreditPlusConsumptionDTO);
            return LegacyCardCreditPlusConsumptionDTO;
        }

        /// <summary>
        /// Gets the LegacyCardCreditPlusConsumption data of passed LegacyCardCreditPlusConsumption Id
        /// </summary>
        /// <param name="LegacyCardCreditPlusConsumptionId">integer type parameter</param>
        /// <returns>Returns LegacyCardCreditPlusConsumptionDTO</returns>
        public LegacyCardCreditPlusConsumptionDTO GetLegacyCardCreditPlusConsumptionDTO(int LegacyCardCreditPlusConsumptionId)
        {
            log.LogMethodEntry(LegacyCardCreditPlusConsumptionId);
            LegacyCardCreditPlusConsumptionDTO returnValue = null;
            string query = @"SELECT * 
                             FROM LegacyCardCreditPlusConsumption
                             WHERE LegacyCardCreditPlusConsumption.LegacyCardCreditPlusConsumptionId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", LegacyCardCreditPlusConsumptionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetLegacyCardCreditPlusConsumptionDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Returns the List of LegacyCard based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<LegacyCardCreditPlusConsumptionDTO> GetLegacyCardCreditPlusConsumptionDTOList(List<KeyValuePair<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<LegacyCardCreditPlusConsumptionDTO> list = null;
            string selectQuery = @"SELECT LegacyCardCreditPlusConsumption.* 
                                  FROM LegacyCardCreditPlusConsumption 
                                  LEFT OUTER JOIN LegacyCardCreditPlus ON LegacyCardCreditPlus.LegacyCardCreditPlusId =  LegacyCardCreditPlusConsumption.LegacyCardCreditPlusId ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LegacyCardCreditPlusConsumptionDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LegacyCardCreditPlusConsumptionDTO.SearchByParameters.LEGACY_CARDCREDIT_PLUS_CONSUMPTION_ID ||
                            searchParameter.Key == LegacyCardCreditPlusConsumptionDTO.SearchByParameters.LEGACY_CARD_CREDIT_PLUS_ID ||
                            searchParameter.Key == LegacyCardCreditPlusConsumptionDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LegacyCardCreditPlusConsumptionDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LegacyCardCreditPlusConsumptionDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == LegacyCardCreditPlusConsumptionDTO.SearchByParameters.CARD_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception in GetLegacyCardCreditPlusConsumptionDTO");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<LegacyCardCreditPlusConsumptionDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LegacyCardCreditPlusConsumptionDTO legacyCardCreditPlusConsumptionDTO = GetLegacyCardCreditPlusConsumptionDTO(dataRow);
                    list.Add(legacyCardCreditPlusConsumptionDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

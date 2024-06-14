/*/********************************************************************************************
 * Project Name - LegacyCardDiscountsDataHandler
 * Description  - Data Handler for LegacyCardDiscountsDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks 
 *********************************************************************************************
 *2.130.4     18-Feb-2022    Dakshakh                Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Parafait_POS
{
    public class LegacyCardDiscountsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * from LegacyCardDiscounts AS lcd ";
        private static readonly Dictionary<LegacyCardDiscountsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LegacyCardDiscountsDTO.SearchByParameters, string>
            {
                {LegacyCardDiscountsDTO.SearchByParameters.LEGACY_CARD_DISCOUNT_ID, "lcd.CardDiscountId"},
                {LegacyCardDiscountsDTO.SearchByParameters.LEGACY_CARD_ID, "lcd.Legacycard_id"},
                {LegacyCardDiscountsDTO.SearchByParameters.IS_ACTIVE, "lcd.IsActive"},
                {LegacyCardDiscountsDTO.SearchByParameters.MASTER_ENTITY_ID,"lcd.MasterEntityId"},
                {LegacyCardDiscountsDTO.SearchByParameters.SITE_ID, "lcd.site_id"},
                {LegacyCardDiscountsDTO.SearchByParameters.CARD_ID_LIST, "lcd.Legacycard_id"}
            };

        /// <summary>
        /// Default constructor of LegacyCardDiscountsDataHandler class
        /// </summary>
        public LegacyCardDiscountsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LegacyCardDiscount Record.
        /// </summary>
        /// <param name="LegacyCardDiscountsDTO">LegacyCardDiscountsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(LegacyCardDiscountsDTO LegacyCardDiscountsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(LegacyCardDiscountsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LegacyCardDiscountId", LegacyCardDiscountsDTO.LegacyCardDiscountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Legacycard_id", LegacyCardDiscountsDTO.Legacycard_id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@discount_name", LegacyCardDiscountsDTO.Discount_name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", LegacyCardDiscountsDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", LegacyCardDiscountsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", LegacyCardDiscountsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the LegacyCardDiscount record to the database
        /// </summary>
        /// <param name="LegacyCardDiscountsDTO">LegacyCardDiscountsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted LegacyCardDiscount record</returns>
        public LegacyCardDiscountsDTO InsertLegacyCardDiscount(LegacyCardDiscountsDTO LegacyCardDiscountsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(LegacyCardDiscountsDTO, userId, siteId);
            string query = @"INSERT INTO LegacyCardDiscounts 
                                        ( 
                                            Legacycard_id,
                                            discount_name,
                                            ExpiryDate,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastupdatedDate,
                                            IsActive,
                                            site_id,
                                            MasterEntityId,
                                            
                                        ) 
                                VALUES 
                                        (
                                            @Legacycard_id,
                                            @discount_name,
                                            @ExpiryDate,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GetDate(),
                                            @IsActive,
                                            @site_id,
                                            @MasterEntityId,
                                        )
                                        SELECT * FROM LegacyCardDiscounts WHERE LegacyCardDiscountId = scope_identity()";


            List<SqlParameter> parameters = BuildSQLParameters(LegacyCardDiscountsDTO, userId, siteId);
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                RefreshLegacyCardDiscountsDTO(LegacyCardDiscountsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Inserting the Legacy card Discount ", ex);
                log.LogVariableState("LegacyCardDiscountsDTO", LegacyCardDiscountsDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(LegacyCardDiscountsDTO);
            return LegacyCardDiscountsDTO;
        }

        /// <summary>
        /// Updates the LegacyCardDiscount record
        /// </summary>
        /// <param name="LegacyCardDiscountsDTO">LegacyCardDiscountsDTO type parameter</param>
        /// <param name="userId">User updating the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public LegacyCardDiscountsDTO UpdateAccountDiscount(LegacyCardDiscountsDTO LegacyCardDiscountsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(LegacyCardDiscountsDTO, userId, siteId);
            string query = @"UPDATE LegacyCardDiscounts 
                             SET Legacycard_id=@Legacycard_id,
                                 discount_name=@discount_name,
                                 ExpiryDate=@ExpiryDate,
                                 LastUpdatedBy=@LastUpdatedBy,
                                 LastupdatedDate=GetDate(),
                                 IsActive=@IsActive,
                                 MasterEntityId=@MasterEntityId
                             WHERE LegacyCardDiscountId = @LegacyCardDiscountId
                             SELECT * FROM LegacyCardDiscounts WHERE LegacyCardDiscountId = @LegacyCardDiscountId";
            List<SqlParameter> parameters = BuildSQLParameters(LegacyCardDiscountsDTO, userId, siteId);
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                RefreshLegacyCardDiscountsDTO(LegacyCardDiscountsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating the LegacyCard Discount ", ex);
                log.LogVariableState("LegacyCardDiscountsDTO", LegacyCardDiscountsDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(LegacyCardDiscountsDTO);
            return LegacyCardDiscountsDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="LegacyCardDiscountsDTO">LegacyCardDiscountsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshLegacyCardDiscountsDTO(LegacyCardDiscountsDTO LegacyCardDiscountsDTO, DataTable dt)
        {
            log.LogMethodEntry(LegacyCardDiscountsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                LegacyCardDiscountsDTO.LegacyCardDiscountId = Convert.ToInt32(dt.Rows[0]["LegacyCardDiscountId"]);
                LegacyCardDiscountsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                LegacyCardDiscountsDTO.LastUpdateDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                LegacyCardDiscountsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                LegacyCardDiscountsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                LegacyCardDiscountsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                LegacyCardDiscountsDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to LegacyCardDiscountsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns LegacyCardDiscountsDTO</returns>
        private LegacyCardDiscountsDTO GetLegacyCardDiscountsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LegacyCardDiscountsDTO LegacyCardDiscountsDTO = new LegacyCardDiscountsDTO(Convert.ToInt32(dataRow["LegacyCardDiscountId"]),
                                            dataRow["Legacycard_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Legacycard_id"]),
                                            dataRow["discount_name"] == DBNull.Value ? "" : Convert.ToString(dataRow["discount_name"]),
                                            dataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToString(dataRow["IsActive"]) == "Y",
                                            dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(LegacyCardDiscountsDTO);
            return LegacyCardDiscountsDTO;
        }

        /// <summary>
        /// Gets the AccountDiscount data of passed AccountDiscount Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns LegacyCardDiscountsDTO</returns>
        public LegacyCardDiscountsDTO GetLegacyCardDiscountsDTO(int id)
        {
            log.LogMethodEntry(id);
            LegacyCardDiscountsDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE lcd.LegacyCardDiscountId = @LegacyCardDiscountId";
            SqlParameter parameter = new SqlParameter("@LegacyCardDiscountId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetLegacyCardDiscountsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the LegacyCardDiscountsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LegacyCardDiscountsDTO matching the search criteria</returns>
        public List<LegacyCardDiscountsDTO> GetLegacyCardDiscountsDTOList(List<KeyValuePair<LegacyCardDiscountsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<LegacyCardDiscountsDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<LegacyCardDiscountsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == LegacyCardDiscountsDTO.SearchByParameters.LEGACY_CARD_ID ||
                            searchParameter.Key == LegacyCardDiscountsDTO.SearchByParameters.LEGACY_CARD_DISCOUNT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LegacyCardDiscountsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LegacyCardDiscountsDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == LegacyCardDiscountsDTO.SearchByParameters.CARD_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<LegacyCardDiscountsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LegacyCardDiscountsDTO LegacyCardDiscountsDTO = GetLegacyCardDiscountsDTO(dataRow);
                    list.Add(LegacyCardDiscountsDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data Handler File for PromotionExclusionDates 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.70        10-June-2019   Divya A                 Created 
 *2.80        08-Apr-2020     Mushahid Faizan         Modified : 3 tier changes for Rest API.
 ********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Linq;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// PromotionExclusionDate Data Handler - Handles insert, update and select of PromotionExclusionDate objects
    /// </summary>
    class PromotionExclusionDateDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PromotionExclusionDates as ped ";

        /// <summary>
        /// Dictionary for searching Parameters for the PromotionExclusionDate object.
        /// </summary>
        private static readonly Dictionary<PromotionExclusionDateDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PromotionExclusionDateDTO.SearchByParameters, string>
        {
            { PromotionExclusionDateDTO.SearchByParameters.PROMOTION_EXCLUSION_ID,"ped.PromotionExclusionId"},
            { PromotionExclusionDateDTO.SearchByParameters.PROMOTION_ID,"ped.Promotion_Id"},
             { PromotionExclusionDateDTO.SearchByParameters.PROMOTION_ID_LIST,"ped.Promotion_Id"},
            { PromotionExclusionDateDTO.SearchByParameters.INCLUDE_DATE,"ped.IncludeDate"},
            { PromotionExclusionDateDTO.SearchByParameters.SITE_ID,"ped.site_id"},
            { PromotionExclusionDateDTO.SearchByParameters.IS_ACTIVE,"ped.IsActive"},
            { PromotionExclusionDateDTO.SearchByParameters.MASTER_ENTITY_ID,"ped.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for PromotionExclusionDateDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction objects</param>
        public PromotionExclusionDateDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PromotionExclusionDate Record.
        /// </summary>
        /// <param name="promotionExclusionDateDTO">promotionExclusionDateDTO object is passed as parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(PromotionExclusionDateDTO promotionExclusionDateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(promotionExclusionDateDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PromotionExclusionId", promotionExclusionDateDTO.PromotionExclusionId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Promotion_Id", promotionExclusionDateDTO.PromotionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExclusionDate", promotionExclusionDateDTO.ExclusionDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", promotionExclusionDateDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IncludeDate", promotionExclusionDateDTO.IncludeDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Day", promotionExclusionDateDTO.Day));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", promotionExclusionDateDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", promotionExclusionDateDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to PromotionExclusionDateDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of PromotionExclusionDateDTO</returns>
        private PromotionExclusionDateDTO GetPromotionExclusionDateDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PromotionExclusionDateDTO promotionExclusionDateDTO = new PromotionExclusionDateDTO(dataRow["PromotionExclusionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PromotionExclusionId"]),
                                                dataRow["Promotion_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Promotion_Id"]),
                                                dataRow["ExclusionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExclusionDate"]),
                                                dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                                dataRow["IncludeDate"] == DBNull.Value ? (char?)null : Convert.ToChar(dataRow["IncludeDate"]),
                                                dataRow["Day"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Day"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]));
            log.LogMethodExit(promotionExclusionDateDTO);
            return promotionExclusionDateDTO;
        }

        /// <summary>
        /// Gets the PromotionExclusionDate data of passed PromotionExclusion ID
        /// </summary>
        /// <param name="promotionExclusionId">promotionExclusionId is passed as parameter</param>
        /// <returns>Returns PromotionExclusionDateDTO</returns>
        public PromotionExclusionDateDTO GetPromotionExclusionDateDTO(int promotionExclusionId)
        {
            log.LogMethodEntry(promotionExclusionId);
            PromotionExclusionDateDTO result = null;
            string query = SELECT_QUERY + @" WHERE ped.PromotionExclusionId = @PromotionExclusionId";
            SqlParameter parameter = new SqlParameter("@PromotionExclusionId", promotionExclusionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPromotionExclusionDateDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        ///  Deletes the PromotionExclusionDate record
        /// </summary>
        /// <param name="promotionExclusionDateDTO">PromotionExclusionDateDTO is passed as parameter</param>
        internal void Delete(PromotionExclusionDateDTO promotionExclusionDateDTO)
        {
            log.LogMethodEntry(promotionExclusionDateDTO);
            string query = @"DELETE  
                             FROM PromotionExclusionDates
                             WHERE PromotionExclusionDates.PromotionExclusionId = @PromotionExclusionId";
            SqlParameter parameter = new SqlParameter("@PromotionExclusionId", promotionExclusionDateDTO.PromotionExclusionId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            promotionExclusionDateDTO.AcceptChanges();
            log.LogMethodExit();
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="promotionExclusionDateDTO">PromotionExclusionDateDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshPromotionExclusionDateDTO(PromotionExclusionDateDTO promotionExclusionDateDTO, DataTable dt)
        {
            log.LogMethodEntry(promotionExclusionDateDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                promotionExclusionDateDTO.PromotionId = Convert.ToInt32(dt.Rows[0]["PromotionExclusionId"]);
                promotionExclusionDateDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                promotionExclusionDateDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                promotionExclusionDateDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                promotionExclusionDateDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                promotionExclusionDateDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                promotionExclusionDateDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the PromotionExclusionDates Table. 
        /// </summary>
        /// <param name="promotionExclusionDateDTO">promotionExclusionDateDTO object is passed as parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PromotionExclusionDateDTO</returns>
        public PromotionExclusionDateDTO Insert(PromotionExclusionDateDTO promotionExclusionDateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(promotionExclusionDateDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[PromotionExclusionDates]
                            (
                            Promotion_Id,
                            ExclusionDate,
                            Remarks,
                            Guid,
                            site_id,
                            IncludeDate,
                            Day,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate , IsActive
                            )
                            VALUES
                            (
                            @Promotion_Id,
                            @ExclusionDate,
                            @Remarks,
                            NEWID(),
                            @site_id,
                            @IncludeDate,
                            @Day,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE()  , @IsActive                   
                            )
                            SELECT * FROM PromotionExclusionDates WHERE PromotionExclusionId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(promotionExclusionDateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPromotionExclusionDateDTO(promotionExclusionDateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting PromotionExclusionDateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(promotionExclusionDateDTO);
            return promotionExclusionDateDTO;
        }

        /// <summary>
        /// Update the record in the PromotionExclusionDates Table. 
        /// </summary>
        /// <param name="promotionExclusionDateDTO">promotionExclusionDateDTO object is passed as parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PromotionExclusionDateDTO</returns>
        public PromotionExclusionDateDTO Update(PromotionExclusionDateDTO promotionExclusionDateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(promotionExclusionDateDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[PromotionExclusionDates]
                             SET
                             Promotion_Id = @Promotion_Id,
                             ExclusionDate = @ExclusionDate,
                             Remarks = @Remarks,
                             IncludeDate = @IncludeDate,
                             Day = @Day,
                             MasterEntityId = @MasterEntityId,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdateDate = GETDATE(),
                             IsActive = @IsActive
                             WHERE PromotionExclusionId = @PromotionExclusionId
                            SELECT * FROM PromotionExclusionDates WHERE PromotionExclusionId = @PromotionExclusionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(promotionExclusionDateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPromotionExclusionDateDTO(promotionExclusionDateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating PromotionExclusionDateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(promotionExclusionDateDTO);
            return promotionExclusionDateDTO;
        }

        internal List<PromotionExclusionDateDTO> GetPromotionExclusionDateDTOList(List<int> promotionIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(promotionIdList);
            List<PromotionExclusionDateDTO> promotionExclusionDateDTOList = new List<PromotionExclusionDateDTO>();
            string query = @"SELECT *
                            FROM PromotionExclusionDates, @promotionIdList List
                            WHERE Promotion_Id = List.Id ";
            if (activeRecords)
            {
                query += " AND (IsActive = 1 or IsActive is null)";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@promotionIdList", promotionIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                promotionExclusionDateDTOList = table.Rows.Cast<DataRow>().Select(x => GetPromotionExclusionDateDTO(x)).ToList();
            }
            log.LogMethodExit(promotionExclusionDateDTOList);
            return promotionExclusionDateDTOList;
        }

        /// <summary>
        /// Returns the List of PromotionExclusionDateDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of PromotionExclusionDateDTO</returns>
        public List<PromotionExclusionDateDTO> GetPromotionExclusionDateDTOList(List<KeyValuePair<PromotionExclusionDateDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<PromotionExclusionDateDTO> promotionExclusionDateDTOList = new List<PromotionExclusionDateDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PromotionExclusionDateDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == PromotionExclusionDateDTO.SearchByParameters.PROMOTION_EXCLUSION_ID ||
                            searchParameter.Key == PromotionExclusionDateDTO.SearchByParameters.PROMOTION_ID ||
                            searchParameter.Key == PromotionExclusionDateDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PromotionExclusionDateDTO.SearchByParameters.PROMOTION_ID_LIST)
                        {
                            query.Append(joiner + "( " + DBSearchParameters[searchParameter.Key] + " IN (" + searchParameter.Value + " ))");
                        }
                        else if (searchParameter.Key == PromotionExclusionDateDTO.SearchByParameters.INCLUDE_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == PromotionExclusionDateDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == PromotionExclusionDateDTO.SearchByParameters.SITE_ID)
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
                    PromotionExclusionDateDTO promotionExclusionDateDTO = GetPromotionExclusionDateDTO(dataRow);
                    promotionExclusionDateDTOList.Add(promotionExclusionDateDTO);
                }
            }
            log.LogMethodExit(promotionExclusionDateDTOList);
            return promotionExclusionDateDTOList;
        }
    }
}
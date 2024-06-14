
/********************************************************************************************
 * Project Name - RedemptionGifts Data Handler
 * Description  - Data handler of the RedemptionGifts class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-May-2017   Lakshminarayana     Created 
 *2.3.0       03-Jul-2018   Archana/Guru S A    Date param changes to insert and update. Updated 
 *                                              get dto queries to include required fields
 *                                              LastUpdatedBy, CreatedBy and OriginalPriceInTickets are added to table.
 *2.4.0       03-Sep-2018   Archana/Guru S A    Modified add gift line reversal changes
 *2.70.2        19-Jul-2019   Deeksha             Modifications as per three tier standard.
 *2.70.2        10-Dec-2019   Jinto Thomas        Removed siteid from update query
 *2.100.0     31-Aug-2020   Mushahid Faizan      siteId changes in GetSQLParameters().
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.SqlServer.Server;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    ///  RedemptionGifts Data Handler - Handles insert, update and select of  RedemptionGifts objects
    /// </summary>
    public class RedemptionGiftsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<RedemptionGiftsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RedemptionGiftsDTO.SearchByParameters, string>
            {
                {RedemptionGiftsDTO.SearchByParameters.REDEMPTION_GIFTS_ID, "rg.redemption_gifts_id"},
                {RedemptionGiftsDTO.SearchByParameters.REDEMPTION_ID, "rg.redemption_id"},
                {RedemptionGiftsDTO.SearchByParameters.MASTER_ENTITY_ID,"rg.MasterEntityId"},
                {RedemptionGiftsDTO.SearchByParameters.SITE_ID, "rg.site_id"},
                {RedemptionGiftsDTO.SearchByParameters.GIFT_LINE_IS_REVERSED, ""}
            };
        private DataAccessHandler dataAccessHandler;
        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS [RedemptionGiftsType];
                                            MERGE INTO redemption_gifts tbl
                                            USING @RedemptionGiftList AS src
                                            ON src.redemption_gifts_id = tbl.redemption_gifts_id
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            redemption_id=src.redemption_id,
                                             gift_code=src.gift_code,
                                             ProductId=src.ProductId,
                                             LocationId=src.LocationId,
                                             Tickets=src.Tickets,  
                                             GraceTickets=src.GraceTickets,   
                                             LotID=src.LotID,
                                             LastUpdatedBy=src.LastUpdatedBy,
                                             LastUpdateDate=getdate(), 
                                             OriginalPriceInTickets=src.OriginalPriceInTickets,
                                             OriginalRedemptionGiftId = src.OriginalRedemptionGiftId,
                                            MasterEntityId = src.MasterEntityId--,
                                            --site_id = src.site_id
                                            WHEN NOT MATCHED THEN INSERT (
                                            redemption_id,
                                            gift_code,
                                            ProductId,
                                            LocationId,
                                            Tickets,
                                            site_id,
                                            Guid,
                                            GraceTickets,
                                            MasterEntityId,
                                            LotID,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            CreationDate,
                                            CreatedBy,
                                            OriginalPriceInTickets,
                                            OriginalRedemptionGiftId
                                            )VALUES (
                                            src.redemption_id,
                                            src.gift_code,
                                            src.ProductId,
                                            src.LocationId,
                                            src.Tickets,
                                            src.site_id,
                                            src.Guid,
                                            src.GraceTickets,
                                            src.MasterEntityId,
                                            src.LotID,
                                            src.LastUpdatedBy,
                                            getdate(),
                                            getdate(),
                                            src.CreatedBy,
                                            src.OriginalPriceInTickets,
                                            src.OriginalRedemptionGiftId
                                            )
                                            OUTPUT
                                            inserted.redemption_gifts_id,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdateDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            redemption_gifts_id,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdateDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion 

        /// <summary>
        /// Default constructor of RedemptionGiftsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public RedemptionGiftsDataHandler(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating RedemptionGifts Record.
        /// </summary>
        /// <param name="redemptionGiftsDTO">redemptionGiftsDTO</param>
        /// <param name="userId">userId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>parameters</returns>
        private List<SqlParameter> PassParametersHelper( RedemptionGiftsDTO redemptionGiftsDTO, string userId, int siteId)
        {           
            log.LogMethodEntry( redemptionGiftsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@redemption_gifts_id", redemptionGiftsDTO.RedemptionGiftsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@redemption_id", redemptionGiftsDTO.RedemptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gift_code", redemptionGiftsDTO.GiftCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", redemptionGiftsDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LocationId", redemptionGiftsDTO.LocationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Tickets", redemptionGiftsDTO.Tickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GraceTickets", redemptionGiftsDTO.GraceTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", redemptionGiftsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LotID", redemptionGiftsDTO.LotId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OriginalPriceInTickets", redemptionGiftsDTO.OriginalPriceInTickets== -1 ? DBNull.Value :(object) redemptionGiftsDTO.OriginalPriceInTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OriginalRedemptionGiftId", redemptionGiftsDTO.OrignialRedemptionGiftId, true));            
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the RedemptionGifts record to the database
        /// </summary>
        /// <param name="redemptionGiftsDTO">RedemptionGiftsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">SQLTrx</param>
        /// <returns>Returns inserted record id</returns>
        public RedemptionGiftsDTO InsertRedemptionGifts(RedemptionGiftsDTO redemptionGiftsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionGiftsDTO, loginId, loginId);
            string query = @"INSERT INTO[dbo].[Redemption_gifts]
                                        ( 
                                            redemption_id,
                                            gift_code,
                                            ProductId,
                                            LocationId,
                                            Tickets,
                                            site_id,
                                            Guid,
                                            GraceTickets,
                                            MasterEntityId,
                                            LotID,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            CreationDate,
                                            CreatedBy,
                                            OriginalPriceInTickets,
                                            OriginalRedemptionGiftId
                                        ) 
                                VALUES 
                                        (
                                            @redemption_id,
                                            @gift_code,
                                            @ProductId,
                                            @LocationId,
                                            @Tickets,
                                            @site_id,
                                            NEWID(),
                                            @GraceTickets,
                                            @MasterEntityId,
                                            @LotID,
                                            @LastUpdatedBy,
                                            getdate(),
                                            getDate(),
                                            @CreatedBy,
                                            @OriginalPriceInTickets,
                                            @OriginalRedemptionGiftId
                                        )  SELECT* FROM Redemption_gifts WHERE redemption_gifts_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, PassParametersHelper(redemptionGiftsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionGiftsDTO(redemptionGiftsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting redemptionGiftsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(redemptionGiftsDTO);
            return redemptionGiftsDTO;
        }

        /// <summary>
        /// Updates the RedemptionGifts record
        /// </summary>
        /// <param name="redemptionGiftsDTO">RedemptionGiftsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx"></param>
        /// <returns>Returns the count of updated rows</returns>
        public RedemptionGiftsDTO UpdateRedemptionGifts(RedemptionGiftsDTO redemptionGiftsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionGiftsDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[Redemption_gifts]
                            SET  redemption_id=@redemption_id,
                                 gift_code=@gift_code,
                                 ProductId=@ProductId,
                                 LocationId=@LocationId,
                                 Tickets=@Tickets,  
                                 -- site_id=@site_id,
                                 GraceTickets=@GraceTickets,   
                                 LotID=@LotID,
                                 LastUpdatedBy=@LastUpdatedBy,
                                 LastUpdateDate=getdate(), 
                                 OriginalPriceInTickets=@OriginalPriceInTickets,
                                 OriginalRedemptionGiftId = @OriginalRedemptionGiftId
                             WHERE redemption_gifts_id = @redemption_gifts_id
                               SELECT * FROM Redemption_gifts WHERE redemption_gifts_id = @redemption_gifts_id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, PassParametersHelper(redemptionGiftsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionGiftsDTO(redemptionGiftsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating redemptionGiftsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(redemptionGiftsDTO);
            return redemptionGiftsDTO;
        }

        /// <summary>
        /// Delete the record from the RedemptionGifts database based on redemptionGiftsId
        /// </summary>
        /// <param name="redemptionGiftsId">redemptionGiftsId</param>
        /// <returns>return the int </returns>
        internal int Delete(int redemptionGiftsId)
        {
            log.LogMethodEntry(redemptionGiftsId);
            string query = @"DELETE  
                             FROM Redemption_gifts
                             WHERE Redemption_gifts.redemption_gifts_id = @redemption_gifts_id";
            SqlParameter parameter = new SqlParameter("@redemption_gifts_id", redemptionGiftsId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="redemptionGiftsDTO">RedemptionGiftsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshRedemptionGiftsDTO(RedemptionGiftsDTO redemptionGiftsDTO, DataTable dt)
        {
            log.LogMethodEntry(redemptionGiftsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                redemptionGiftsDTO.RedemptionGiftsId = Convert.ToInt32(dt.Rows[0]["redemption_gifts_id"]);
                redemptionGiftsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                redemptionGiftsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                redemptionGiftsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                redemptionGiftsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                redemptionGiftsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                redemptionGiftsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to RedemptionGiftsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns RedemptionGiftsDTO</returns>
        private RedemptionGiftsDTO GetRedemptionGiftsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            RedemptionGiftsDTO redemptionGiftsDTO = new RedemptionGiftsDTO(Convert.ToInt32(dataRow["redemption_gifts_id"]),
                                            dataRow["redemption_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["redemption_id"]),
                                            dataRow["gift_code"] == DBNull.Value ? string.Empty : dataRow["gift_code"].ToString(),
                                            dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                            dataRow["LocationId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LocationId"]),
                                            dataRow["Tickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Tickets"]),
                                            dataRow["GraceTickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["GraceTickets"]),
                                            dataRow["LotID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LotID"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["OriginalPriceInTickets"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OriginalPriceInTickets"]),
                                            dataRow["ProductName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProductName"]),
                                            dataRow["ProductDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProductDescription"]),
                                            dataRow["GiftLineIsReversed"].ToString() == "1" ? true : false,
                                            dataRow["OriginalRedemptionGiftId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OriginalRedemptionGiftId"]),
                                            Convert.ToInt32(dataRow["ProductQuantity"])
                                            );
            log.LogMethodExit(redemptionGiftsDTO);
            return redemptionGiftsDTO;
        }

        /// <summary>
        /// Gets the RedemptionGifts data of passed RedemptionGifts Id
        /// </summary>
        /// <param name="redemptionGiftsId">integer type parameter</param>
        /// <returns>Returns RedemptionGiftsDTO</returns>
        public RedemptionGiftsDTO GetRedemptionGiftsDTO(int redemptionGiftsId)
        {
            log.LogMethodEntry(redemptionGiftsId);
            RedemptionGiftsDTO returnValue = null;
            string query = @"SELECT rg.*, p.productName, p.description as ProductDescription,
                                    (select 1 from Redemption_gifts rgin where  rgin.originalRedemptiongiftId = rg.redemption_gifts_id)
	                                as GiftLineIsReversed,  1 as ProductQuantity
                            FROM Redemption_gifts rg, 
                                 product p
                            WHERE rg.redemption_gifts_id = @redemption_gifts_id
                             and rg.productId = p.productId ";
            SqlParameter parameter = new SqlParameter("@redemption_gifts_id", redemptionGiftsId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter });
            if(dataTable.Rows.Count > 0)
                returnValue = GetRedemptionGiftsDTO(dataTable.Rows[0]);
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the MediaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of RedemptionGiftsDTO matching the search criteria</returns>
        public List<RedemptionGiftsDTO> GetRedemptionGiftsDTOList(List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<RedemptionGiftsDTO> list = null;
            int count = 0;
            string selectQuery = @"SELECT rg.*, p.productName, p.description as ProductDescription,
                                          (select 1 from Redemption_gifts rgin where  rgin.originalRedemptiongiftId = rg.redemption_gifts_id) as GiftLineIsReversed, 1 as ProductQuantity
                                     FROM Redemption_gifts rg, 
                                           product p  ";
            if((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where rg.productId = p.productId and ");
                foreach(KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if(DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if(searchParameter.Key == RedemptionGiftsDTO.SearchByParameters.REDEMPTION_GIFTS_ID ||
                            searchParameter.Key == RedemptionGiftsDTO.SearchByParameters.REDEMPTION_ID||
                            searchParameter.Key == RedemptionGiftsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key == RedemptionGiftsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RedemptionGiftsDTO.SearchByParameters.GIFT_LINE_IS_REVERSED)
                        {
                            if (searchParameter.Value == "N")
                            {
                                query.Append(joiner + @" NOT EXISTS (SELECT 1 
                                                                       from redemption_gifts rgwhere 
                                                                      where rgwhere.originalRedemptiongiftId = rg.redemption_gifts_id ) ");
                            }
                            else
                            {
                                query.Append(joiner + @" EXISTS (SELECT 1 
                                                                   from redemption_gifts rgwhere 
                                                                  where rgwhere.originalRedemptiongiftId = rg.redemption_gifts_id ) ");
                            }
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
            if(dataTable.Rows.Count > 0)
            {
                list = new List<RedemptionGiftsDTO>();
                foreach(DataRow dataRow in dataTable.Rows)
                {
                    RedemptionGiftsDTO redemptionGiftsDTO = GetRedemptionGiftsDTO(dataRow);
                    list.Add(redemptionGiftsDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// Inserts the RedemptionGiftsDTOList record to the database
        /// </summary>
        /// <param name="redemptionGiftsDTOList">List of ProductBarcodeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<RedemptionGiftsDTO> redemptionGiftsDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionGiftsDTOList, loginId, siteId);
            Dictionary<string, RedemptionGiftsDTO> redemptionGiftsDTOGuidMap = GetRedemptionGiftsDTOGuidMap(redemptionGiftsDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(redemptionGiftsDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "RedemptionGiftsType",
                                                                "@RedemptionGiftList");
            Update(redemptionGiftsDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<RedemptionGiftsDTO> redemptionGiftsDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(redemptionGiftsDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[18];
            columnStructures[0] = new SqlMetaData("redemption_gifts_id", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("redemption_id", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("gift_code", SqlDbType.NVarChar, 100);
            columnStructures[3] = new SqlMetaData("ProductId", SqlDbType.Int);
            columnStructures[4] = new SqlMetaData("LocationId", SqlDbType.Int);
            columnStructures[5] = new SqlMetaData("Tickets", SqlDbType.Int);
            columnStructures[6] = new SqlMetaData("GraceTickets", SqlDbType.Int);
            columnStructures[7] = new SqlMetaData("LotID", SqlDbType.Int);
            columnStructures[8] = new SqlMetaData("OriginalPriceInTickets", SqlDbType.Int);
            columnStructures[9] = new SqlMetaData("OriginalRedemptionGiftId", SqlDbType.Int);
            columnStructures[10] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 100);
            columnStructures[11] = new SqlMetaData("LastUpdateDate", SqlDbType.DateTime);
            columnStructures[12] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[13] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[14] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[15] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[16] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 100);
            columnStructures[17] = new SqlMetaData("CreationDate", SqlDbType.DateTime);

            for (int i = 0; i < redemptionGiftsDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].RedemptionGiftsId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].RedemptionId, true));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].GiftCode));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].ProductId, true));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].LocationId, true));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].Tickets));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].GraceTickets));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].LotId, true));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].OriginalPriceInTickets));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].OrignialRedemptionGiftId, true));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].LastUpdateDate));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(Guid.Parse(redemptionGiftsDTOList[i].Guid)));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].SynchStatus));
                dataRecord.SetValue(15, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(16, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(17, dataAccessHandler.GetParameterValue(redemptionGiftsDTOList[i].CreationDate));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, RedemptionGiftsDTO> GetRedemptionGiftsDTOGuidMap(List<RedemptionGiftsDTO> redemptionGiftsDTOList)
        {
            Dictionary<string, RedemptionGiftsDTO> result = new Dictionary<string, RedemptionGiftsDTO>();
            for (int i = 0; i < redemptionGiftsDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(redemptionGiftsDTOList[i].Guid))
                {
                    redemptionGiftsDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(redemptionGiftsDTOList[i].Guid, redemptionGiftsDTOList[i]);
            }
            return result;
        }

        private void Update(Dictionary<string, RedemptionGiftsDTO> redemptionGiftsDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                RedemptionGiftsDTO redemptionGiftsDTO = redemptionGiftsDTOGuidMap[Convert.ToString(row["Guid"])];
                redemptionGiftsDTO.RedemptionGiftsId = row["Redemption_Gifts_Id"] == DBNull.Value ? -1 : Convert.ToInt32(row["Redemption_Gifts_Id"]);
                redemptionGiftsDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                redemptionGiftsDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                redemptionGiftsDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                redemptionGiftsDTO.LastUpdateDate = row["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdateDate"]);
                redemptionGiftsDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                redemptionGiftsDTO.AcceptChanges();
            }
        }
    }
}

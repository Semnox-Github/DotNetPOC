/********************************************************************************************
 * Project Name - RedemptionCards Data Handler
 * Description  - Data handler of the Redemption Cards class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-May-2017   Jeevan     Created 
 *2.3.0       03-Jul-2018   Guru S A   Date param changes to insert and update. 
                                       Updated get dto queries to include required fields
 *2.70.2        19-Jul-2019   Deeksha    Modifications as per three tier standard.
 *2.70.2        10-Dec-2019   Jinto Thomas         Removed siteid from update query
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
    ///  Redemption Data Handler - Handles insert, update and select of  Redemption objects
    /// </summary>
    public class RedemptionCardsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<RedemptionCardsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RedemptionCardsDTO.SearchByParameters, string>
            {
                {RedemptionCardsDTO.SearchByParameters.REDEMPTION_CARD_ID, "rcc.redemption_cards_id"},
                {RedemptionCardsDTO.SearchByParameters.REDEMPTION_ID, "rcc.redemption_id"},
                {RedemptionCardsDTO.SearchByParameters.CARD_NUMBER , "rcc.card_number"},
                {RedemptionCardsDTO.SearchByParameters.CURRENCY_ID,"rcc.CurrencyId"},
                {RedemptionCardsDTO.SearchByParameters.MASTER_ENTITY_ID,"rcc.MasterEntityId"},
                {RedemptionCardsDTO.SearchByParameters.SITE_ID, "rcc.site_id"}
            };
        private DataAccessHandler dataAccessHandler;
        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS [RedemptionCardsType];
                                            MERGE INTO redemption_cards tbl
                                            USING @RedemptionCardList AS src
                                            ON src.redemption_cards_id = tbl.redemption_cards_id
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            redemption_id=src.redemption_id,
                                             card_number=src.card_number,
                                             card_id=src.card_id,
                                             ticket_count=src.ticket_count,
                                             CurrencyId=src.CurrencyId,  
                                             CurrencyQuantity=src.CurrencyQuantity,   
                                             LastUpdatedBy=src.LastUpdatedBy,
                                             LastUpdateDate=getdate(), 
                                             SourceCurrencyRuleId=src.SourceCurrencyRuleId,
                                             CurrencyRuleId = src.CurrencyRuleId,
                                             ViewGroupingNumber = src.ViewGroupingNumber,
                                            MasterEntityId = src.MasterEntityId--,
                                            --site_id = src.site_id
                                            WHEN NOT MATCHED THEN INSERT (
                                            redemption_id,
                                            card_number,
                                            card_id,
                                            ticket_count,
                                            CurrencyId,
                                            CurrencyQuantity,
                                            site_id,
                                            Guid,
                                            MasterEntityId,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            CreationDate,
                                            CreatedBy,
                                            SourceCurrencyRuleId,
                                            CurrencyRuleId,
                                            ViewGroupingNumber
                                            )VALUES (
                                            src.redemption_id,
                                            src.card_number,
                                            src.card_id,
                                            src.ticket_count,
                                            src.CurrencyId,
                                            src.CurrencyQuantity,
                                            src.site_id,
                                            src.Guid,
                                            src.MasterEntityId,
                                            src.LastUpdatedBy,
                                            getdate(),
                                            getdate(),
                                            src.CreatedBy,
                                            src.SourceCurrencyRuleId,
                                            src.CurrencyRuleId,
                                            src.ViewGroupingNumber
                                            )
                                            OUTPUT
                                            inserted.redemption_cards_id,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdateDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            redemption_cards_id,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdateDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion 
        /// <summary>
        /// Default constructor of RedemptionDataHandler class
        /// </summary>
        public RedemptionCardsDataHandler(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

      
        private List<SqlParameter> PassParametersHelper(RedemptionCardsDTO redemptionCardsDTO, string userId, int siteId)
        {
            log.LogMethodEntry( redemptionCardsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@redemption_cards_id", redemptionCardsDTO.RedemptionCardsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@redemption_id", redemptionCardsDTO.RedemptionId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@card_number", redemptionCardsDTO.CardNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@card_id", redemptionCardsDTO.CardId ,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticket_count", redemptionCardsDTO.TicketCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@currencyId", redemptionCardsDTO.CurrencyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@currencyQuantity", redemptionCardsDTO.CurrencyQuantity, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", redemptionCardsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sourceCurrencyRuleId", redemptionCardsDTO.SourceCurrencyRuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@currencyRuleId", redemptionCardsDTO.CurrencyRuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@viewgroupingnumber", redemptionCardsDTO.ViewGroupingNumber));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the RedemptionCards record to the database
        /// </summary>
        /// <param name="redemptionCardsDTO">RedemptionDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx"></param>
        /// <returns>Returns inserted record id</returns>
        public RedemptionCardsDTO InsertRedemptionCards(RedemptionCardsDTO redemptionCardsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionCardsDTO, loginId, siteId);
            string query = @"INSERT INTO Redemption_cards 
                                        ( 
                                            redemption_id,
                                            card_number,
                                            card_id,
                                            ticket_count,
                                            CurrencyId,
                                            CurrencyQuantity,
                                            site_id,
                                            Guid,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            SourceCurrencyRuleId,
                                            CurrencyRuleId,
                                            ViewGroupingNumber
                                     ) 
                                VALUES 
                                        (
                                            @redemption_id,
                                            @card_number,
                                            @card_id,
                                            @ticket_count,
                                            @currencyId,
                                            @currencyQuantity,
                                            @site_id,
                                            NEWID(),
                                            @MasterEntityId,
                                            @LastUpdatedBy,
                                            getdate(),
                                            @LastUpdatedBy,
                                           getDate(),
                                            @sourceCurrencyRuleId,
                                            @currencyRuleId,
                                            @viewgroupingnumber
                                        )SELECT* FROM Redemption_cards WHERE redemption_cards_id = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, PassParametersHelper(redemptionCardsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionCardsDTO(redemptionCardsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting redemptionCardsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(redemptionCardsDTO);
            return redemptionCardsDTO;
        }

       

        /// <summary>
        /// Updates the Redemption Cards record
        /// </summary>
        /// <param name="redemptionCardsDTO">RedemptionDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx"></param>
        /// <returns>Returns the count of updated rows</returns>
        public RedemptionCardsDTO UpdateRedemptionCard(RedemptionCardsDTO redemptionCardsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(redemptionCardsDTO, userId, siteId);
            string query = @"UPDATE Redemption_cards 
                             SET redemption_id=@redemption_id,
                                 card_number=@card_number,
                                 card_id=@card_id,
                                 ticket_count=@ticket_count,
                                 CurrencyId=@currencyId,  
                                 CurrencyQuantity=@currencyQuantity,
                                 -- site_id=@site_id,
                                 MasterEntityId=@MasterEntityId,
                                 LastUpdatedBy=@LastUpdatedBy,
                                  LastUpdateDate=getdate(),
                                 SourceCurrencyRuleId = @sourceCurrencyRuleId,
                                 CurrencyRuleId = @currencyRuleId,
                                 ViewGroupingNumber=@viewgroupingnumber
                         WHERE redemption_cards_id = @redemption_cards_id
            SELECT * FROM Redemption_cards WHERE redemption_cards_id = @redemption_cards_id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, PassParametersHelper(redemptionCardsDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionCardsDTO(redemptionCardsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating redemptionCardsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(redemptionCardsDTO);
            return redemptionCardsDTO;
        }

        /// <summary>
        /// Delete the record from the RedemptionGifts database based on redemptionGiftsId
        /// </summary>
        /// <param name="redemptionCardsId">redemptionCardsId</param>
        /// <returns>return the int </returns>
        internal int Delete(int redemptionCardsId)
        {
            log.LogMethodEntry(redemptionCardsId);
            string query = @"DELETE  
                             FROM Redemption_cards
                             WHERE Redemption_cards.redemption_cards_id = @redemption_cards_id";
            SqlParameter parameter = new SqlParameter("@redemption_cards_id", redemptionCardsId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="redemptionCardsDTO">redemptionCardsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshRedemptionCardsDTO(RedemptionCardsDTO redemptionCardsDTO, DataTable dt)
        {
            log.LogMethodEntry(redemptionCardsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                redemptionCardsDTO.RedemptionCardsId = Convert.ToInt32(dt.Rows[0]["redemption_cards_id"]);
                redemptionCardsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                redemptionCardsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                redemptionCardsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                redemptionCardsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                redemptionCardsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                redemptionCardsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to RedemptionDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns RedemptionDTO</returns>
        private RedemptionCardsDTO GetRedemptionCardDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            RedemptionCardsDTO redemptionCardsDTO = new RedemptionCardsDTO(Convert.ToInt32(dataRow["redemption_cards_id"]),
                                                            Convert.ToInt32(dataRow["redemption_id"]),
                                                            dataRow["card_number"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["card_number"]),
                                                            dataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["card_id"]),
                                                            dataRow["ticket_count"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ticket_count"]),
                                                            dataRow["CurrencyId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["CurrencyId"]),
                                                            dataRow["CurrencyQuantity"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["CurrencyQuantity"]),
                                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                            dataRow["currencyValueInTickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["currencyValueInTickets"]),
                                                            dataRow["currencyName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["currencyName"]),
                                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                            dataRow["TotalCardTickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["TotalCardTickets"]),
                                                            dataRow["SourceCurrencyRuleId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["SourceCurrencyRuleId"]),
                                                            dataRow["CurrencyRuleId"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["CurrencyRuleId"]),
                                                            dataRow["ViewGroupingNumber"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ViewGroupingNumber"])
                                );

            log.LogMethodExit(redemptionCardsDTO);
            return redemptionCardsDTO;
        }

        /// <summary>
        /// Gets the Redemption Cards data of passed Redemption Card Id
        /// </summary>
        /// <param name="redemptionCardId">integer type parameter</param>
        /// <returns>Returns RedemptionCardsDTO</returns>
        public RedemptionCardsDTO GetRedemptionCardDTO(int redemptionCardId)
        {
            log.LogMethodEntry(redemptionCardId);
            RedemptionCardsDTO returnValue = null;
            string query = @"SELECT rcc.*, rc.currencyName, rc.ValueInTickets as currencyValueInTickets, c.ticket_count+c.CreditPlusTickets as TotalCardTickets
                            FROM Redemption_cards rcc
                                  left join cardView c on c.card_Id = rcc.card_id
                                  left join RedemptionCurrency rc on rc.currencyId = rcc.currencyId
                            WHERE rcc.redemption_cards_id = @redemptionCardId";
            SqlParameter parameter = new SqlParameter("@redemptionCardId", redemptionCardId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter },sqlTransaction);
            if(dataTable.Rows.Count > 0)
                returnValue = GetRedemptionCardDTO(dataTable.Rows[0]);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the GetRedemptionCardsDTOList list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of RedemptionCardsDTOList matching the search criteria</returns>
        public List<RedemptionCardsDTO> GetRedemptionCardsDTOList(List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<RedemptionCardsDTO> list = null;
            int count = 0;
            string selectQuery = @"SELECT rcc.*, rc.currencyName, rc.ValueInTickets as currencyValueInTickets, c.ticket_count+c.CreditPlusTickets as TotalCardTickets
                                    FROM Redemption_cards rcc
                                         left join cardView c on c.card_Id = rcc.card_id
                                         left join RedemptionCurrency rc on rc.currencyId = rcc.currencyId";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<RedemptionCardsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        
                        if (searchParameter.Key == RedemptionCardsDTO.SearchByParameters.REDEMPTION_CARD_ID ||
                            searchParameter.Key == RedemptionCardsDTO.SearchByParameters.REDEMPTION_ID ||
                            searchParameter.Key == RedemptionCardsDTO.SearchByParameters.CURRENCY_ID ||
                            searchParameter.Key == RedemptionCardsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RedemptionCardsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RedemptionCardsDTO.SearchByParameters.CARD_NUMBER)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
            if (dataTable.Rows.Count > 0)
            {
                list = new List<RedemptionCardsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RedemptionCardsDTO redemptionCardsDTO = GetRedemptionCardDTO(dataRow);
                    list.Add(redemptionCardsDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// Inserts the redemptionCardsDTOList record to the database
        /// </summary>
        /// <param name="redemptionCardsDTOList">List of redemptionCardsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<RedemptionCardsDTO> redemptionCardsDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionCardsDTOList, loginId, siteId);
            Dictionary<string, RedemptionCardsDTO> redemptionCardsDTOGuidMap = GetRedemptionCardsDTOGuidMap(redemptionCardsDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(redemptionCardsDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "RedemptionCardsType",
                                                                "@RedemptionCardList");
            Update(redemptionCardsDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<RedemptionCardsDTO> redemptionCardsDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(redemptionCardsDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[18];
            columnStructures[0] = new SqlMetaData("redemption_cards_id", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("redemption_id", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("card_number", SqlDbType.NVarChar, 100);
            columnStructures[3] = new SqlMetaData("card_id", SqlDbType.Int);
            columnStructures[4] = new SqlMetaData("ticket_count", SqlDbType.Int);
            columnStructures[5] = new SqlMetaData("CurrencyId", SqlDbType.Int);
            columnStructures[6] = new SqlMetaData("CurrencyQuantity", SqlDbType.Int);
            columnStructures[7] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[8] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[9] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[10] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[11] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 100);
            columnStructures[12] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[13] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 100);
            columnStructures[14] = new SqlMetaData("LastUpdateDate", SqlDbType.DateTime);
            columnStructures[15] = new SqlMetaData("SourceCurrencyRuleId", SqlDbType.Int);
            columnStructures[16] = new SqlMetaData("CurrencyRuleId", SqlDbType.Int);
            columnStructures[17] = new SqlMetaData("ViewGroupingNumber", SqlDbType.Int);

            for (int i = 0; i < redemptionCardsDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].RedemptionCardsId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].RedemptionId, true));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].CardNumber));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].CardId, true));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].TicketCount, true));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].CurrencyId, true));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].CurrencyQuantity));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(Guid.Parse(redemptionCardsDTOList[i].Guid)));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].SynchStatus));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].CreationDate));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].LastUpdateDate));
                dataRecord.SetValue(15, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].SourceCurrencyRuleId));
                dataRecord.SetValue(16, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].CurrencyRuleId, true));
                dataRecord.SetValue(17, dataAccessHandler.GetParameterValue(redemptionCardsDTOList[i].ViewGroupingNumber));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, RedemptionCardsDTO> GetRedemptionCardsDTOGuidMap(List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            Dictionary<string, RedemptionCardsDTO> result = new Dictionary<string, RedemptionCardsDTO>();
            for (int i = 0; i < redemptionCardsDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(redemptionCardsDTOList[i].Guid))
                {
                    redemptionCardsDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(redemptionCardsDTOList[i].Guid, redemptionCardsDTOList[i]);
            }
            return result;
        }

        private void Update(Dictionary<string, RedemptionCardsDTO> redemptionCardsDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                RedemptionCardsDTO redemptionCardsDTO = redemptionCardsDTOGuidMap[Convert.ToString(row["Guid"])];
                redemptionCardsDTO.RedemptionCardsId = row["redemption_cards_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["redemption_cards_id"]);
                redemptionCardsDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                redemptionCardsDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                redemptionCardsDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                redemptionCardsDTO.LastUpdateDate = row["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdateDate"]);
                redemptionCardsDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                redemptionCardsDTO.AcceptChanges();
            }
        }
    }
}


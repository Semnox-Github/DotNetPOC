/********************************************************************************************
 * Project Name - TicketReceipt
 * Description  - Ticket Station DataHandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       11-July-2018      Archana           Created 
 *2.70.2.0      16-Sept -2019     Girish Kundar     Modified :Part of Ticket Eater enhancements. 
 *2.110.0      21-Dec-2020      Abhishek           Modified : Modified for web API changes  
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 *2.150.3     29-MAY-2023   Sweedol             Modified: Removed generating management form access records for Ticket Station.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Ticket Station Data Handler
    /// </summary>
    public class TicketStationDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private static readonly Dictionary<TicketStationDTO.SearchByTicketStationParameters, string> DBSearchParameters = new Dictionary<TicketStationDTO.SearchByTicketStationParameters, string>
            {
                {TicketStationDTO.SearchByTicketStationParameters.ID, "Id"},
                {TicketStationDTO.SearchByTicketStationParameters.TICKET_STATION_ID, "TicketStationId"},
                {TicketStationDTO.SearchByTicketStationParameters.IS_ACTIVE, "IsActive"},
                {TicketStationDTO.SearchByTicketStationParameters.MASTER_ENTITY_ID,"MasterEntityId"},
                {TicketStationDTO.SearchByTicketStationParameters.TICKET_STATION_TYPE,"TicketStationType"},
                {TicketStationDTO.SearchByTicketStationParameters.SITE_ID, "site_id"}
            };
        private const string SELECT_QUERY = "Select * from TicketStations";
        /// <summary>
        /// Default constructor of TicketReceiptDataHandler class
        /// </summary>
        public TicketStationDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ticketStation Record.
        /// </summary>
        /// <param name="ticketStationDTO">ticketStationDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(TicketStationDTO ticketStationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(ticketStationDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", ticketStationDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketStationId", ticketStationDTO.TicketStationId.ToString()));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VoucherLength", ticketStationDTO.VoucherLength));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckDigit", ticketStationDTO.CheckDigit));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketStationType", ticketStationDTO.TicketStationType == 0 ? TicketStationDTO.TICKETSTATIONTYPE.POS_TICKET_STATION.ToString() : TicketStationDTO.TICKETSTATIONTYPE.PHYSICAL_TICKET_STATION.ToString()));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketLength", ticketStationDTO.TicketLength));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckBitAlgorithm", string.IsNullOrEmpty(ticketStationDTO.CheckBitAlgorithm) ? DBNull.Value : (object)ticketStationDTO.CheckBitAlgorithm));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", ticketStationDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", ticketStationDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to DiscountCouponsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DiscountCouponsDTO</returns>
        private TicketStationDTO GetTicketStationDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TicketStationDTO ticketStationDTO = new TicketStationDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                            dataRow["TicketStationId"] == DBNull.Value ? string.Empty : dataRow["TicketStationId"].ToString(),
                                            dataRow["VoucherLength"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["VoucherLength"]),
                                            dataRow["CheckDigit"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["CheckDigit"]),
                                            dataRow["TicketStationType"] == DBNull.Value ? TicketStationDTO.TICKETSTATIONTYPE.POS_TICKET_STATION : GetTicketStationType(dataRow["TicketStationType"].ToString()),
                                            dataRow["TicketLength"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TicketLength"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["checkBitAlgorithm"] == DBNull.Value ? string.Empty : dataRow["checkBitAlgorithm"].ToString(),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(ticketStationDTO);
            return ticketStationDTO;
        }

        /// <summary>
        /// Inserts the ticket station record to the table.
        /// </summary>
        /// <param name="ticketStationDTO">ticketStationDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>TicketStationDTO</returns>

        public TicketStationDTO Insert(TicketStationDTO ticketStationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(ticketStationDTO, loginId, siteId);
            string query = @"INSERT INTO TicketStations 
                                        ( 
                                            TicketStationId,
                                            VoucherLength,
                                            CheckDigit,
                                            TicketStationType,
                                            TicketLength,
                                            checkBitAlgorithm,
                                            IsActive,
                                            Guid,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastupdatedDate,
                                            site_id,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @TicketStationId,
                                            @VoucherLength,
                                            @CheckDigit,
                                            @TicketStationType,
                                            @TicketLength,
                                            @CheckBitAlgorithm,
                                            @IsActive,
                                            NewId(),
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @SiteId,
                                            @MasterEntityId
                                        ) SELECT * FROM TicketStations WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(ticketStationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTicketStationDTO(ticketStationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ticketStationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit();
            return ticketStationDTO;

        }

        /// <summary>
        /// Updates the ticket station record to the table.
        /// </summary>
        /// <param name="ticketStationDTO">ticketStationDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>TicketStationDTO</returns>
        public TicketStationDTO Update(TicketStationDTO ticketStationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(ticketStationDTO, loginId, siteId);
            string query = @"UPDATE  TicketStations 
                                     SET 
                                            TicketStationId = @TicketStationId,
                                            VoucherLength = @VoucherLength,
                                            CheckDigit =   @CheckDigit,
                                            TicketStationType = @TicketStationType,
                                            TicketLength = @TicketLength,
                                            IsActive = @IsActive,
                                            CheckBitAlgorithm = @CheckBitAlgorithm,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastupdatedDate = GETDATE(),
                                            site_id = @SiteId,
                                            MasterEntityId = @MasterEntityId
                                        Where Id = @Id 
                                        SELECT * FROM TicketStations WHERE Id  = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(ticketStationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTicketStationDTO(ticketStationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ticketStationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit();
            return ticketStationDTO;

        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="ticketStationDTO">TicketStationDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshTicketStationDTO(TicketStationDTO ticketStationDTO, DataTable dt)
        {
            log.LogMethodEntry(ticketStationDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                ticketStationDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                ticketStationDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                ticketStationDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                ticketStationDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                ticketStationDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                ticketStationDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                ticketStationDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        ///  Gets the Address Type by passing address Type string
        /// </summary>
        /// <param name="stationTypeString">stationTypeString </param>
        /// <returns>TICKETSTATIONTYPE</returns>
        private TicketStationDTO.TICKETSTATIONTYPE GetTicketStationType(string stationTypeString)
        {
            log.LogMethodEntry(stationTypeString);
            TicketStationDTO.TICKETSTATIONTYPE ticketStationType = TicketStationDTO.TICKETSTATIONTYPE.PHYSICAL_TICKET_STATION;
            try
            {
                ticketStationType = (TicketStationDTO.TICKETSTATIONTYPE)Enum.Parse(typeof(TicketStationDTO.TICKETSTATIONTYPE), stationTypeString, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while parsing the ticketStation type", ex);
                throw ex;
            }
            log.LogMethodExit(ticketStationType);
            return ticketStationType;
        }


        /// <summary>
        /// Gets the TicketStationDTO data of passed TicketStation Id
        /// </summary>
        /// <param name="ticketStationId">integer type parameter</param>
        /// <returns>Returns TicketStationDTO</returns>
        public TicketStationDTO GetTicketStationsDTO(string ticketStationId)
        {
            log.LogMethodEntry(ticketStationId);
            TicketStationDTO returnValue = null;
            string query = SELECT_QUERY + " WHERE TicketStationId = @TicketStationId";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@TicketStationId", ticketStationId), }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetTicketStationDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the TicketStationDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TicketStationDTO matching the search criteria</returns>
        public List<TicketStationDTO> GetTicketStationDTOList(List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<TicketStationDTO> list = new List<TicketStationDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == TicketStationDTO.SearchByTicketStationParameters.ID ||
                            searchParameter.Key == TicketStationDTO.SearchByTicketStationParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        if (searchParameter.Key == TicketStationDTO.SearchByTicketStationParameters.TICKET_STATION_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TicketStationDTO.SearchByTicketStationParameters.TICKET_STATION_TYPE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TicketStationDTO.SearchByTicketStationParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TicketStationDTO.SearchByTicketStationParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TicketStationDTO ticketStationDTO = GetTicketStationDTO(dataRow);
                    list.Add(ticketStationDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
        internal DateTime? GetTicketStationModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"select max(LastupdatedDate) LastupdatedDate from TicketStations WHERE (site_id = @siteId or @siteId = -1)";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastupdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastupdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}


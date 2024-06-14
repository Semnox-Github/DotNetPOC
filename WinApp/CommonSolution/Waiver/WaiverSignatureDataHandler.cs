/********************************************************************************************
 * Project Name - Waiver
 * Description  - Data handler - WaiverSignatureDataHandler 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70          01-Jul-2019   Girish Kundar    Modified : For SQL Injection Issue.  
 *2.70.2        27-Sep-2019   Deeksha          Waiver phase 2 changes
 *2.70.2        11-Dec-2019   Jinto Thomas     Removed siteid from update query
 *2.70.2        03-JAN-2020   Akshay G         Added searchParameter - LINE_ID_IN
 *2.140.0       14-Sep-2021   Guru S A         Waiver mapping UI enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Waiver
{
    /// <summary>
    ///  WaiverSignature Data Handler - Handles insert, update and select of  WaiverSignature objects
    /// </summary>
    public class WaiverSignatureDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string> DBSearchParameters = new Dictionary<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>
        {
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.WAIVER_SIGNED_ID, "ws.WaiverSignedId"},
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.CUSTOMER_SIGNED_WAIVER_ID, "ws.CustomerSignedWaiverId"},
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.WAIVERSETDETAIL_ID, "ws.waiverSetDetailId"},
                //{WaiverSignatureDTO.SearchByWaiverSignatureParameters.WAIVER_SIGNED_FILENAME, "ws.WaiverSignedFileName"},
                //{WaiverSignatureDTO.SearchByWaiverSignatureParameters.EXPIRY_DATE, "ws.ExpiryDate"},
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.TRX_ID, "ws.TrxId"},
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.LINE_ID, "ws.LineId"},
                //{WaiverSignatureDTO.SearchByWaiverSignatureParameters.CUSTOMER_ID, "ws.CustomerId"},
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.USER_ID, "ws.UserId"},
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.IS_ACTIVE, "ws.IsActive"},
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.LAST_UPDATED_DATE, "ws.LastUpdatedDate"},
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.LAST_UPDATED_BY, "ws.LastUpdatedBy"},
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.GUID, "ws.GUID"},
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.SITE_ID, "ws.Site_id"},
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.SYNCH_STATUS, "ws.SynchStatus"},
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.MASTER_ENTITY_ID, "ws.MasterEntityId"},
                {WaiverSignatureDTO.SearchByWaiverSignatureParameters.LINE_ID_IN, "ws.LineId"},
            };
        private const string SELECT_QUERY = @"SELECT ws.*, csw.SignedWaiverFileName as CustomerSignedWaiverFileName 
                                                FROM WaiversSigned As ws left outer join customersignedwaiver csw on csw.CustomerSignedWaiverId = ws.CustomerSignedWaiverId ";
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        /// <summary>
        /// Default constructor of WaiverSignatureDataHandler class
        /// </summary>
        public WaiverSignatureDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
  

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating WaiverSignature Record.
        /// </summary>
        /// <param name="waiverSignedDTO">WaiverSignatureDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(WaiverSignatureDTO waiverSignedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSignedDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@WaiverSignedId", waiverSignedDTO.WaiverSignedId, true);
            ParametersHelper.ParameterHelper(parameters, "@CustomerSignedWaiverId", waiverSignedDTO.CustomerSignedWaiverId, true);
            ParametersHelper.ParameterHelper(parameters, "@WaiverSetDetailId", waiverSignedDTO.WaiverSetDetailId, true);
            //ParametersHelper.ParameterHelper(parameters, "@WaiverSignedFileName", waiverSignedDTO.WaiverSignedFileName);
            //ParametersHelper.ParameterHelper(parameters, "@ExpiryDate", waiverSignedDTO.ExpiryDate == DateTime.MinValue ? (DateTime?)null : waiverSignedDTO.ExpiryDate);
            ParametersHelper.ParameterHelper(parameters, "@TrxId", waiverSignedDTO.TrxId, true);
            ParametersHelper.ParameterHelper(parameters, "@LineId", waiverSignedDTO.LineId, true);
            //ParametersHelper.ParameterHelper(parameters, "@CustomerId", waiverSignedDTO.CustomerId, true);
            ParametersHelper.ParameterHelper(parameters, "@UserId", waiverSignedDTO.UserId, true);
            ParametersHelper.ParameterHelper(parameters, "@SignedMode", waiverSignedDTO.SignedMode);
            //ParametersHelper.ParameterHelper(parameters, "@SigneeName", waiverSignedDTO.SigneeName);
            //ParametersHelper.ParameterHelper(parameters, "@SigneeEmail", waiverSignedDTO.SigneeEmail);
            //ParametersHelper.ParameterHelper(parameters, "@IsWaiverSigned", waiverSignedDTO.IsWaiverSigned);
            ParametersHelper.ParameterHelper(parameters, "@IsActive", waiverSignedDTO.IsActive);
            ParametersHelper.ParameterHelper(parameters, "@LastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@Site_id", siteId, true);
			ParametersHelper.ParameterHelper(parameters, "@MasterEntityId", waiverSignedDTO.MasterEntityId, true);
            ParametersHelper.ParameterHelper(parameters, "@IsOverriden", waiverSignedDTO.IsOverriden);
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Inserts the WaiverSignature record to the database
        /// </summary>
        /// <param name="waiverSignedDTO">WaiverSignatureDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns WaiverSignatureDTO</returns>
        public WaiverSignatureDTO InsertWaiverSignature(WaiverSignatureDTO waiverSignedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSignedDTO, loginId, siteId);
            string query = @"INSERT INTO WaiversSigned 
                                        ( 
                                            WaiverSetDetailId,
                                            --WaiverSignedFileName,
                                            --ExpiryDate,
                                            TrxId,
                                            LineId,
                                            --CustomerId,
                                            UserId,
                                            SignedMode,
                                            --SigneeName,
                                            --SigneeEmail,
                                            --IsWaiverSigned,
                                            IsActive,
                                            CreationDate,
                                            CreatedBy,
                                            LastUpdatedDate,
                                            LastUpdatedBy,
                                            GUID,
                                            Site_id,
                                            MasterEntityId,
                                            CustomerSignedWaiverId,
                                            IsOverriden
                                        ) 
                                VALUES 
                                        (
                                            @WaiverSetDetailId,
                                           --@WaiverSignedFileName,
                                           -- @ExpiryDate,
                                            @TrxId,
                                            @LineId,
                                           --@CustomerId,
                                            @UserId,
                                            @SignedMode,
                                            --@SigneeName,
                                            --@SigneeEmail,
                                            --@IsWaiverSigned,
                                            @IsActive,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            NewId(),
                                            @Site_id,
                                            @MasterEntityId,
                                            @CustomerSignedWaiverId,
                                            @IsOverriden
                                        )SELECT  * from WaiversSigned where WaiverSignedId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(waiverSignedDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWaiverSignedDTO(waiverSignedDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting WaiverSignedDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(waiverSignedDTO);
            return waiverSignedDTO;
        }

        /// <summary>
        /// Updates the WaiverSignature record
        /// </summary>
        /// <param name="waiverSignedDTO">WaiverSignatureDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the WaiverSignatureDTO</returns>
        public WaiverSignatureDTO UpdateWaiverSignature(WaiverSignatureDTO waiverSignedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSignedDTO, loginId, siteId);
            string query = @"UPDATE WaiversSigned 
                             SET 
                                            WaiverSetDetailId = @WaiverSetDetailId,
                                           -- WaiverSignedFileName = @WaiverSignedFileName,
                                           -- ExpiryDate = @ExpiryDate,
                                            TrxId = @TrxId,
                                            LineId = @LineId,
                                            UserId = @UserId,
                                           -- CustomerId = @CustomerId,
                                            SignedMode = @SignedMode,
                                           -- SigneeName = @SigneeName,
                                            --SigneeEmail = @SigneeEmail,
                                            --IsWaiverSigned = @IsWaiverSigned,
                                            IsActive = @IsActive,
                                            LastUpdatedDate = GETDATE(),
                                            LastUpdatedBy = @LastUpdatedBy,
                                            --Site_id = @Site_id,
                                            MasterEntityId = @MasterEntityId,
                                            CustomerSignedWaiverId = @CustomerSignedWaiverId,
                                            IsOverriden = @IsOverriden
                             WHERE WaiverSignedId = @WaiverSignedId
                             SELECT  * from WaiversSigned where WaiverSignedId = @WaiverSignedId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(waiverSignedDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWaiverSignedDTO(waiverSignedDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating WaiverSignedDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(waiverSignedDTO);
            return waiverSignedDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="waiverSignatureDTO">WaiverSignatureDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshWaiverSignedDTO(WaiverSignatureDTO waiverSignatureDTO, DataTable dt)
        {
            log.LogMethodEntry(waiverSignatureDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                waiverSignatureDTO.WaiverSignedId = Convert.ToInt32(dt.Rows[0]["WaiverSignedId"]);
                waiverSignatureDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                waiverSignatureDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                waiverSignatureDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                waiverSignatureDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                waiverSignatureDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                waiverSignatureDTO.Site_id = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to WaiverSignatureDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns WaiverSignatureDTO</returns>
        private WaiverSignatureDTO GetWaiverSignatureDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            WaiverSignatureDTO waiverSignedDTO = new WaiverSignatureDTO(Convert.ToInt32(dataRow["WaiverSignedId"]),
                                            dataRow["waiverSetDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["waiverSetDetailId"]),
                                            //dataRow["WaiverSignedFileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["WaiverSignedFileName"]),
                                            //dataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                            dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                            dataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LineId"]),
                                            //dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                            dataRow["UserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserId"]),
                                            dataRow["SignedMode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SignedMode"]),
                                            //dataRow["SigneeName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SigneeName"]),
                                            //dataRow["SigneeEmail"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SigneeEmail"]),
                                            //dataRow["IsWaiverSigned"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsWaiverSigned"]),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["GUID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GUID"]),
                                            dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CustomerSignedWaiverId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerSignedWaiverId"]),
                                            dataRow["CustomerSignedWaiverFileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CustomerSignedWaiverFileName"]),
                                            dataRow["IsOverriden"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsOverriden"])
                                            );
            log.LogMethodExit(waiverSignedDTO);
            return waiverSignedDTO;
        }


        /// <summary>
        /// Gets the WaiverSignature data of passed WaiverSignature Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns WaiverSignatureDTO</returns>
        public WaiverSignatureDTO GetWaiverSignatureDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            WaiverSignatureDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE WaiverSignedId = @WaiverSignedId";
            SqlParameter parameter = new SqlParameter("@WaiverSignedId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetWaiverSignatureDTO(dataTable.Rows[0]);
               
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the WaiverSignatureDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of WaiverSignatureDTO matching the search criteria</returns>
        public List<WaiverSignatureDTO> GetWaiverSignatureDTOList(List<KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<WaiverSignatureDTO> list = new List<WaiverSignatureDTO>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (
                            searchParameter.Key == WaiverSignatureDTO.SearchByWaiverSignatureParameters.WAIVERSETDETAIL_ID || 
                            searchParameter.Key == WaiverSignatureDTO.SearchByWaiverSignatureParameters.WAIVER_SIGNED_ID ||
                            searchParameter.Key == WaiverSignatureDTO.SearchByWaiverSignatureParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == WaiverSignatureDTO.SearchByWaiverSignatureParameters.TRX_ID || 
                            searchParameter.Key == WaiverSignatureDTO.SearchByWaiverSignatureParameters.LINE_ID ||
                           // searchParameter.Key == WaiverSignatureDTO.SearchByWaiverSignatureParameters.CUSTOMER_ID ||
                            searchParameter.Key == WaiverSignatureDTO.SearchByWaiverSignatureParameters.USER_ID)
                        {                            
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == WaiverSignatureDTO.SearchByWaiverSignatureParameters.SITE_ID)
                        {                            
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == WaiverSignatureDTO.SearchByWaiverSignatureParameters.IS_ACTIVE) //bit
                        {                           
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == WaiverSignatureDTO.SearchByWaiverSignatureParameters.LINE_ID_IN)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                list = new List<WaiverSignatureDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    WaiverSignatureDTO waiverSignedDTO = GetWaiverSignatureDTO(dataRow);
                    list.Add(waiverSignedDTO);
                }
               
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Checks whether WaiverSignature is in use.
        /// <param name="id">WaiverSignature Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        public int GetWaiverSignatureReferenceCount(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT 
                            (
                            SELECT COUNT(1) 
                            FROM WaiversSigned
                            WHERE WaiverSignedId = @WaiverSignedId
                            AND active_flag = 'Y' 
                            )
                            
                            AS ReferenceCount";
            SqlParameter parameter = new SqlParameter("@WaiverSignedId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }

        internal DataTable GetTransactionWaiverList(int trxId, DateTime fromDate, DateTime toDate,
                                                string phoneNumber1, string phoneNumber2, string cardNumber,
                                                string customerName, int customAttributeId, string customAttributeValue)
        {
            string query = @"select distinct(l.trxid) as TrxId,  trx.TrxDate, p.[product_name] as Product,
                            l.[price] as Price , c.customer_name, isnull(WaiverSignedCount,0) WaiversSigned , l.LineId
                                FROM trx_lines l 
                                    left outer join products p on p.product_id = l.product_id 
                                    left outer join tax t on t.tax_id = l.tax_id 
                                    left outer join (Select w.LineId,  
                                                        Count(DISTINCT w.CustomerSignedWaiverId) as WaiverSignedCount , w.TrxId
                                                            from waiversSigned w
                                                            where (trxid = @trxid or @trxid = -1) and 
                                                            isnull(isActive, 0) = 1 
                                                     and isnull(w.CustomerSignedWaiverId, -1) != -1  group by w.LineId , w.TrxId)  ws on 
                                                            l.Lineid  = ws.LineId and l.TrxId = ws.trxId 
                                    inner join trx_header trx on  trx.TrxId = l.TrxId
                                    left outer join customers c on trx.customerId = c.customer_id
                                    left outer join cards cr on c.customer_id = cr.customer_id
                                    left outer join CustomData cd on cd.CustomDataSetId =  c.CustomDataSetId
                                            where (WaiverSignedCount > 0 and l.TrxId = @trxid or 
                                                            (@trxid = -1 and trxdate >= @fromDate and trxdate < @toDate
                                                            and (c.customer_name like @customername or @customername is null)
                                                            and ((c.contact_phone1 = @phonenumber1 or @phonenumber1 is null) or 
                                                            (c.contact_phone2 = @phonenumber2 or @phonenumber2 is null))
                                                            and (cr.card_number = @cardnumber or @cardnumber is null)
                                                            and (cd.CustomAttributeId = @customAttribute or @customAttribute = -1)
                                                            and (cd.CustomDataText = @customAttributeValue or @customAttributeValue is null)
                                                            and WaiverSignedCount > 0)) 
                                            order by TrxId ";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@trxid", trxId));
            parameters.Add(new SqlParameter("@fromDate", fromDate));
            parameters.Add(new SqlParameter("@toDate", toDate));
            if (phoneNumber1 == string.Empty)
                parameters.Add(new SqlParameter("@phonenumber1", DBNull.Value));
            else
                parameters.Add(new SqlParameter("@phonenumber1", phoneNumber1));
            if (phoneNumber2 == string.Empty)
                parameters.Add(new SqlParameter("@phonenumber2", DBNull.Value));
            else
                parameters.Add(new SqlParameter("@phonenumber2", phoneNumber2));
            if (cardNumber == string.Empty)
                parameters.Add(new SqlParameter("@cardnumber", DBNull.Value));
            else
                parameters.Add(new SqlParameter("@cardnumber", cardNumber));
            if (customerName == string.Empty)
                parameters.Add(new SqlParameter("@customername", DBNull.Value));
            else
                parameters.Add(new SqlParameter("@customername", "%" + customerName + "%"));
            if (customAttributeId <= 0)
                parameters.Add(new SqlParameter("@customAttribute", -1));
            else
                parameters.Add(new SqlParameter("@customAttribute", customAttributeId));
            if (customAttributeValue == string.Empty)
                parameters.Add(new SqlParameter("@customAttributeValue", DBNull.Value));
            else
                parameters.Add(new SqlParameter("@customAttributeValue", customAttributeValue));
            DataAccessHandler dataAccessHandler = new DataAccessHandler();
            DataTable dtl = new DataTable();
            dtl = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);

            return dtl;
        }

    }
}

/********************************************************************************************
 * Project Name - PaymentMode Datahandler Programs
 * Description  - Data object of PaymentChannels Datahandler
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        26-Nov-2016   Rakshith        Created 
 *2.70.2        10-Jul-2019   Girish Kundar   Modified : For SQL Injection Issue.  
 *                                                         Added missed Columns to Insert/Update
 *            26-Jul-2019   Mushahid Faizan  Added IsActive & Delete Method.
 *2.70.2        06-Dec-2019   Jinto Thomas     Removed siteid from update query
 *2.140.0       07-Sep-2021   Fiona              Removed delete
 *                                               Added GetPaymentModeChannelDTOListOfPaymentModes method using paymentModesIdList parameters  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    ///  PaymentModeChannelsDatahandler Class
    /// </summary>
    public class PaymentModeChannelsDatahandler
    {
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM PaymentModeChannels AS pmc ";


        /// <summary>
        /// Parameterized constructor of PaymentModeChannelsDatahandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public PaymentModeChannelsDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        //////<summary>
        //////For search parameter Specified
        //////</summary>
        private static readonly Dictionary<PaymentModeChannelsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PaymentModeChannelsDTO.SearchByParameters, string>
        {
            {PaymentModeChannelsDTO.SearchByParameters.PAYMENT_MODE_CHANNEL_ID, "pmc.PaymentModeChannelId"},
            {PaymentModeChannelsDTO.SearchByParameters.PAYMENT_MODE_ID, "pmc.PaymentModeId"} ,
            {PaymentModeChannelsDTO.SearchByParameters.LOOKUP_VALUE_ID, "pmc.LookupValueId"} ,
            {PaymentModeChannelsDTO.SearchByParameters.MASTER_ENTITY_ID, "pmc.MasterEntityId"} ,
            {PaymentModeChannelsDTO.SearchByParameters.SITE_ID,"pmc.Site_id"},            
            { PaymentModeChannelsDTO.SearchByParameters.ISACTIVE,"pmc.IsActive"}

        };

        /// <summary>
        /// GetSQLParameters method to build SQL parameters for Insert/ Update
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="PaymentModeChannelsDTO">PaymentModeChannelsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(PaymentModeChannelsDTO paymentModeChannelsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(paymentModeChannelsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@paymentModeChannelId", paymentModeChannelsDTO.PaymentModeChannelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@paymentModeId", paymentModeChannelsDTO.PaymentModeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lookupValueId", paymentModeChannelsDTO.LookupValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", paymentModeChannelsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", paymentModeChannelsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;

        }

        /// <summary>
        /// Inserts the PaymentModeChannelsDTO record to the database
        /// </summary>
        /// <param name="paymentModeChannelsDTO">PaymentModeChannelsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns PaymentModeChannelsDTO</returns>
        public PaymentModeChannelsDTO InsertPaymentModeChannel(PaymentModeChannelsDTO paymentModeChannelsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(paymentModeChannelsDTO, loginId, siteId);
            string insertPaymentModeChannelQuery = @"insert into PaymentModeChannels 
                                                        (  
                                                            PaymentModeId,
                                                            LookupValueId,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedDate,
                                                            LastUpdatedBy,
                                                            Site_id,
                                                            Guid,
                                                            MasterEntityId,
                                                            IsActive
                                                        ) 
                                                values 
                                                        (
                                                            @paymentModeId,
                                                            @lookupValueId,
                                                            @createdBy,
                                                            GetDate(),
                                                            GetDate(),
                                                            @lastUpdatedBy,
                                                            @site_id,
                                                            NEWID(),
                                                            @masterEntityId,
                                                            @isActive
                                                           
                                                         ) SELECT * FROM PaymentModeChannels WHERE PaymentModeChannelId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertPaymentModeChannelQuery, GetSQLParameters(paymentModeChannelsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPaymentModeChannelsDTO(paymentModeChannelsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting paymentModeChannelsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(paymentModeChannelsDTO);
            return paymentModeChannelsDTO;

        }


        /// <summary>
        /// Updates the PaymentModeChannelsDTO record to the database
        /// </summary>
        /// <param name="paymentModeChannelsDTO">PaymentModeChannelsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns PaymentModeChannelsDTO</returns>
        public PaymentModeChannelsDTO UpdatePaymentModeChannel(PaymentModeChannelsDTO paymentModeChannelsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(paymentModeChannelsDTO, loginId, siteId);
            string UpdatePaymentModeChannelsQuery = @" update  PaymentModeChannels set  
                                                            PaymentModeId= @paymentModeId,
                                                            LookupValueId= @lookupValueId,
                                                            LastUpdatedDate=GetDate(), 
                                                            LastUpdatedBy=@lastUpdatedBy,
                                                            --Site_id= @site_id,
                                                            MasterEntityId=masterEntityId,
                                                            IsActive=@isActive
                                                            where PaymentModeChannelId=@paymentModeChannelId 
                                                 SELECT * FROM PaymentModeChannels WHERE PaymentModeChannelId = @paymentModeChannelId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(UpdatePaymentModeChannelsQuery, GetSQLParameters(paymentModeChannelsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPaymentModeChannelsDTO(paymentModeChannelsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating paymentModeChannelsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(paymentModeChannelsDTO);
            return paymentModeChannelsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="PaymentModeChannelsDTO">PaymentModeChannelsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPaymentModeChannelsDTO(PaymentModeChannelsDTO paymentModeChannelsDTO, DataTable dt)
        {
            log.LogMethodEntry(paymentModeChannelsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                paymentModeChannelsDTO.PaymentModeChannelId = Convert.ToInt32(dt.Rows[0]["PaymentModeChannelId"]);
                paymentModeChannelsDTO.SiteId = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
                paymentModeChannelsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                paymentModeChannelsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                paymentModeChannelsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                paymentModeChannelsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                paymentModeChannelsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
            }
            log.LogMethodExit();
        }

        ///// <summary>
        ///// DeletePaymentModeChannel method 
        ///// </summary>
        ///// <param name="paymentModeId">paymentModeChannelId</param>
        ///// <returns>return Id of deleted Record</returns>
        //public int DeletePaymentModeChannel(int paymentModeChannelId)
        //{
        //    log.LogMethodEntry(paymentModeChannelId);
        //    string paymentChannelQuery = @"delete  
        //                                  from PaymentModeChannels
        //                                  where PaymentModeChannelId = @paymentModeChannelId";
        //    SqlParameter[] paymentChannelParameters = new SqlParameter[1];
        //    paymentChannelParameters[0] = new SqlParameter("@paymentModeChannelId", paymentModeChannelId);
        //    int deleteStatus = dataAccessHandler.executeUpdateQuery(paymentChannelQuery, paymentChannelParameters, sqlTransaction);
        //    log.LogMethodExit(deleteStatus);
        //    return deleteStatus;
        //}


        /// <summary>
        /// return the record from the database
        /// Convert the datarow to paymentModeChannelDataRow object
        /// </summary>
        /// <returns>return the PaymentModeChannelsDTO object</returns>
        private PaymentModeChannelsDTO GetPaymentModeChannelsDTO(DataRow paymentModeChannelDataRow)
        {
            log.LogMethodEntry(paymentModeChannelDataRow);
            PaymentModeChannelsDTO paymentModeChannelsDTO = new PaymentModeChannelsDTO(
                                                        paymentModeChannelDataRow["PaymentModeChannelId"] == DBNull.Value ? -1 : Convert.ToInt32(paymentModeChannelDataRow["PaymentModeChannelId"]),
                                                        paymentModeChannelDataRow["PaymentModeId"] == DBNull.Value ? -1 : Convert.ToInt32(paymentModeChannelDataRow["PaymentModeId"]),
                                                        paymentModeChannelDataRow["LookupValueId"] == DBNull.Value ? -1 : Convert.ToInt32(paymentModeChannelDataRow["LookupValueId"]),
                                                        paymentModeChannelDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(paymentModeChannelDataRow["CreatedBy"]),
                                                        paymentModeChannelDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(paymentModeChannelDataRow["CreationDate"]),
                                                        paymentModeChannelDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(paymentModeChannelDataRow["LastUpdatedDate"]),
                                                        paymentModeChannelDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(paymentModeChannelDataRow["LastUpdatedBy"]),
                                                        paymentModeChannelDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(paymentModeChannelDataRow["Site_id"]),
                                                        paymentModeChannelDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(paymentModeChannelDataRow["Guid"]),
                                                        paymentModeChannelDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(paymentModeChannelDataRow["SynchStatus"]),
                                                        paymentModeChannelDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(paymentModeChannelDataRow["MasterEntityId"]),
                                                        paymentModeChannelDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(paymentModeChannelDataRow["IsActive"])

                                                 );
            log.LogMethodExit(paymentModeChannelsDTO);
            return paymentModeChannelsDTO;

        }


        /// <summary>
        /// return the record from the database based on  paymentModeChannelId
        /// </summary>
        /// <param name="paymentModeChannelId">paymentModeChannelId</param>
        /// <returns>return the PaymentModeChannelsDTO object</returns>
        /// or empty PaymentModeChannelsDTO
        public PaymentModeChannelsDTO GetPaymentModeChannelsDTO(int paymentModeChannelId)
        {
            log.LogMethodEntry(paymentModeChannelId);
            string paymentModeQuery = SELECT_QUERY + "   where pmc.PaymentModeChannelId = @paymentModeChannelId ";

            SqlParameter[] paymentModeparameters = new SqlParameter[1];
            paymentModeparameters[0] = new SqlParameter("@customerId", paymentModeChannelId);
            DataTable dtCustomersDTO = dataAccessHandler.executeSelectQuery(paymentModeQuery, paymentModeparameters, sqlTransaction);
            PaymentModeChannelsDTO paymentModeChannelsDTO = new PaymentModeChannelsDTO();
            if (dtCustomersDTO.Rows.Count > 0)
            {
                DataRow customersDTORow = dtCustomersDTO.Rows[0];
                paymentModeChannelsDTO = GetPaymentModeChannelsDTO(customersDTORow);

            }
            log.LogMethodExit(paymentModeChannelsDTO);
            return paymentModeChannelsDTO;

        }


        /// <summary>
        /// Gets the PaymentModeChannelsDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic PaymentModeChannelsDTO matching the search criteria</returns>
        public List<PaymentModeChannelsDTO> GetAllPaymentModeChannels(List<KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<PaymentModeChannelsDTO> paymentChannelsList = new List<PaymentModeChannelsDTO>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ?string.Empty : " and ";
                        if (searchParameter.Key.Equals(PaymentModeChannelsDTO.SearchByParameters.LOOKUP_VALUE_ID) ||
                             (searchParameter.Key.Equals(PaymentModeChannelsDTO.SearchByParameters.PAYMENT_MODE_CHANNEL_ID)) ||
                              (searchParameter.Key.Equals(PaymentModeChannelsDTO.SearchByParameters.PAYMENT_MODE_ID)) ||
                              (searchParameter.Key.Equals(PaymentModeChannelsDTO.SearchByParameters.MASTER_ENTITY_ID)))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(PaymentModeChannelsDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PaymentModeChannelsDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;

            }
            DataTable dtPaymentChannels = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dtPaymentChannels.Rows.Count > 0)
            {
                foreach (DataRow customersRow in dtPaymentChannels.Rows)
                {
                    PaymentModeChannelsDTO paymentModeChannelsDTO = GetPaymentModeChannelsDTO(customersRow);
                    paymentChannelsList.Add(paymentModeChannelsDTO);
                }

            }
            log.LogMethodExit(paymentChannelsList);
            return paymentChannelsList;

        }
        internal List<PaymentModeChannelsDTO> GetPaymentModeChannelDTOListOfPaymentModes(List<int> paymentModesIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(paymentModesIdList, activeChildRecords);
            List<PaymentModeChannelsDTO> paymentModeChannelsDTOList = new List<PaymentModeChannelsDTO>();
            string query = SELECT_QUERY + @" , @paymentModesIdList List
                            WHERE PaymentModeId = List.Id ";
            if (activeChildRecords)
            {
                query += " AND isActive = '1' ";
            }

            DataTable table = dataAccessHandler.BatchSelect(query, "@paymentModesIdList", paymentModesIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                paymentModeChannelsDTOList = table.Rows.Cast<DataRow>().Select(x => GetPaymentModeChannelsDTO(x)).ToList();
            }
            log.LogMethodExit(paymentModeChannelsDTOList);
            return paymentModeChannelsDTOList;

        }
    }
}

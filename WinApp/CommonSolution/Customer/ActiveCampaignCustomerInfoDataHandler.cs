/********************************************************************************************
 * Project Name - ActiveCampaignCustomerInfoDataHandler
 * Description  - Data handler of the Active Campaign Info data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.3      01-Feb-2020   Nitin Pai           Created, new data handler to crate the active campaign object 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    class ActiveCampaignCustomerInfoDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<ActiveCampaignCustomerInfoDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ActiveCampaignCustomerInfoDTO.SearchByParameters, string>
            {
                {ActiveCampaignCustomerInfoDTO.SearchByParameters.ACCOUNT_ID, "tl.card_id"},
                {ActiveCampaignCustomerInfoDTO.SearchByParameters.ACCOUNT_ID_LIST, "tl.card_id"},
                {ActiveCampaignCustomerInfoDTO.SearchByParameters.CUSTOMER_ID, "th.customerId"},
                {ActiveCampaignCustomerInfoDTO.SearchByParameters.FROM_DATE, "th.TrxDate"},
                {ActiveCampaignCustomerInfoDTO.SearchByParameters.TO_DATE, "th.TrxDate"},
            };
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"select PSV.segmentname As Product, th.CustomerId, th.site_id, tl.card_number, count(tl.TrxId) as Quantity, max(th.trxDate) as Date  
                                              from trx_lines tl 
                                              left outer join ProductSegmentDataView PSV on tl.product_id = PSV.product_id
                                              inner join trx_header th on tl.TrxId = th.TrxId ";

        private const string GROUP_BY = @" group by PSV.segmentname, th.CustomerId, th.site_id, tl.card_number order by date desc";
        /// <summary>
        /// Default constructor of AccountActivityViewDataHandler class
        /// </summary>
        public ActiveCampaignCustomerInfoDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to CustomerActivityDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CustomerActivityDTO</returns>
        public ActiveCampaignCustomerInfoDTO GetCustomerActivityDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ActiveCampaignCustomerInfoDTO accountActivityDTO = new ActiveCampaignCustomerInfoDTO(dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                            dataRow["Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Date"]),
                                            dataRow["Product"] == DBNull.Value ? "" : Convert.ToString(dataRow["Product"]),
                                            dataRow["Quantity"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Quantity"]),
                                            dataRow["PurchaseType"] == DBNull.Value ? "" : Convert.ToString(dataRow["PurchaseType"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"])
                                            );
            log.LogMethodExit(accountActivityDTO);
            return accountActivityDTO;
        }

        /// <summary>
        /// Gets the AccountActivityDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AccountActivityDTO matching the search criteria</returns>
        public List<ActiveCampaignCustomerInfoDTO> GetCustomerActivityDTOList(List<KeyValuePair<ActiveCampaignCustomerInfoDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ActiveCampaignCustomerInfoDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;

                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ActiveCampaignCustomerInfoDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ((searchParameter.Key == ActiveCampaignCustomerInfoDTO.SearchByParameters.ACCOUNT_ID) ||
                            (searchParameter.Key == ActiveCampaignCustomerInfoDTO.SearchByParameters.CUSTOMER_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ActiveCampaignCustomerInfoDTO.SearchByParameters.ACCOUNT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ActiveCampaignCustomerInfoDTO.SearchByParameters.FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ActiveCampaignCustomerInfoDTO.SearchByParameters.TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                    counter++;
                }
                selectQuery += query + GROUP_BY;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<ActiveCampaignCustomerInfoDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ActiveCampaignCustomerInfoDTO customerActivityDTO = GetCustomerActivityDTO(dataRow);
                    list.Add(customerActivityDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }


        public List<ActiveCampaignCustomerInfoDTO> BuildCustomerActivity(int customerId, string accountList, DateTime fromdate, DateTime toDate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerId, accountList, fromdate, toDate, sqlTransaction);
            List<ActiveCampaignCustomerInfoDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @"SELECT Product, CustomerId, site_Id, sum(Quantity) quantity, Max(date) date, PurchaseType FROM (
                                    select PSV.valuechar As Product, @CUSTOMER_ID As CustomerId, th.site_id as site_id, sum(tl.quantity) as Quantity, max(th.trxDate) as Date, 'REGULAR PURCHASE' as PurchaseType  
                                    from trx_lines tl 
                                    inner join ProductSegmentDataView PSV on tl.product_id = PSV.product_id and valuechar is not null and PSV.SegmentName='ACTIVECAMPAIGN_PRODUCTCATEGORY'
                                    inner join trx_header th on tl.TrxId = th.TrxId 
                                    and (th.CustomerId = @CUSTOMER_ID and tl.card_id is null)
                                    and th.trxDate >= @FROM_DATE
                                    and th.trxDate <= @TO_DATE
	                                and tl.promotion_id is null
                                    group BY PSV.valuechar, th.CustomerId, th.site_id
                                    UNION
                                    select PSV.valuechar As Product, @CUSTOMER_ID As CustomerId, th.site_id as site_id, sum(tl.quantity) as Quantity, max(th.trxDate) as Date, 'REGULAR PURCHASE' as PurchaseType  
                                    from trx_lines tl 
                                    inner join ProductSegmentDataView PSV on tl.product_id = PSV.product_id and valuechar is not null and PSV.SegmentName='ACTIVECAMPAIGN_PRODUCTCATEGORY'
                                    inner join trx_header th on tl.TrxId = th.TrxId 
                                    and (th.CustomerId is null and tl.card_id in (@ACCOUNT_ID_LIST))
                                    and th.trxDate >= @FROM_DATE
                                    and th.trxDate <= @TO_DATE
	                                and tl.promotion_id is null
                                    group BY PSV.valuechar, th.CustomerId, th.site_id
                                    UNION
                                    select PSV.valuechar As Product, @CUSTOMER_ID As CustomerId, th.site_id as site_id, sum(tl.quantity) as Quantity, max(th.trxDate) as Date, 'REGULAR PURCHASE' as PurchaseType  
                                    from trx_lines tl 
                                    inner join ProductSegmentDataView PSV on tl.product_id = PSV.product_id and valuechar is not null and PSV.SegmentName='ACTIVECAMPAIGN_PRODUCTCATEGORY'
                                    inner join trx_header th on tl.TrxId = th.TrxId 
                                    and (th.CustomerId = @CUSTOMER_ID and tl.card_id in (@ACCOUNT_ID_LIST))
                                    and th.trxDate >= @FROM_DATE
                                    and th.trxDate <= @TO_DATE
	                                and tl.promotion_id is null
                                    group BY PSV.valuechar, th.CustomerId, th.site_id
                                    UNION
                                    select PSV.valuechar As Product, @CUSTOMER_ID As CustomerId, th.site_id as site_id, sum(tl.quantity) as Quantity, max(th.trxDate) as Date, 'REGULAR PURCHASE' as PurchaseType  
                                    from trx_lines tl 
                                    inner join ProductSegmentDataView PSV on tl.product_id = PSV.product_id and valuechar is not null and PSV.SegmentName='ACTIVECAMPAIGN_PRODUCTCATEGORY'
                                    inner join (select TrxId, LineId, c.customer_id
			                                    from customers c,
					                                    Profile p,
					                                    CustomerSignedWaiver csw,
					                                    WaiversSigned ws
			                                    where ws.IsActive = 1
				                                    and ws.CustomerSignedWaiverId = csw.CustomerSignedWaiverId
				                                    and csw.SignedFor = c.customer_id
				                                    and p.id = c.profileId) AS WC 
                                    on WC.TrxId = tl.trxId and WC.LineId = tl.LineId and WC.customer_id = @CUSTOMER_ID
                                    inner join trx_header th on tl.TrxId = th.TrxId
                                    and th.trxDate >= @FROM_DATE
                                    and th.trxDate <= @TO_DATE
	                                and tl.promotion_id is null
                                    group BY PSV.valuechar, CustomerId, th.site_id
	                                UNION
	                                select PSV.valuechar As Product, @CUSTOMER_ID As CustomerId, th.site_id as site_id, sum(tl.quantity) as Quantity, max(th.trxDate) as Date, 'PROMOTION - ' + PV.promotion_name as PurchaseType  
                                    from trx_lines tl 
	                                inner join ProductSegmentDataView PSV on tl.product_id = PSV.product_id and valuechar is not null and PSV.SegmentName='ACTIVECAMPAIGN_PRODUCTCATEGORY'
                                    inner join promotions PV on tl.promotion_id = PV.promotion_id and promotion_name is not null
                                    inner join trx_header th on tl.TrxId = th.TrxId 
                                    and (th.CustomerId = @CUSTOMER_ID and tl.card_id is null)
                                    and th.trxDate >= @FROM_DATE
                                    and th.trxDate <= @TO_DATE
	                                and tl.promotion_id is not null
                                    group BY PSV.valuechar, PV.promotion_name, th.CustomerId, th.site_id
                                    UNION
                                    select PSV.valuechar As Product, @CUSTOMER_ID As CustomerId, th.site_id as site_id, sum(tl.quantity) as Quantity, max(th.trxDate) as Date, 'PROMOTION - ' + PV.promotion_name as PurchaseType  
                                    from trx_lines tl 
	                                inner join ProductSegmentDataView PSV on tl.product_id = PSV.product_id and valuechar is not null and PSV.SegmentName='ACTIVECAMPAIGN_PRODUCTCATEGORY'
                                    inner join promotions PV on tl.promotion_id = PV.promotion_id and promotion_name is not null
                                    inner join trx_header th on tl.TrxId = th.TrxId 
                                    and (th.CustomerId is null and tl.card_id in (@ACCOUNT_ID_LIST))
                                    and th.trxDate >= @FROM_DATE
                                    and th.trxDate <= @TO_DATE
	                                and tl.promotion_id is not null
                                    group BY PSV.valuechar, PV.promotion_name, th.CustomerId, th.site_id
                                    UNION
                                    select PSV.valuechar As Product, @CUSTOMER_ID As CustomerId, th.site_id as site_id, sum(tl.quantity) as Quantity, max(th.trxDate) as Date, 'PROMOTION - ' + PV.promotion_name as PurchaseType  
                                    from trx_lines tl 
	                                inner join ProductSegmentDataView PSV on tl.product_id = PSV.product_id and valuechar is not null and PSV.SegmentName='ACTIVECAMPAIGN_PRODUCTCATEGORY'
                                    inner join promotions PV on tl.promotion_id = PV.promotion_id and promotion_name is not null
                                    inner join trx_header th on tl.TrxId = th.TrxId 
                                    and (th.CustomerId = @CUSTOMER_ID and tl.card_id in (@ACCOUNT_ID_LIST))
                                    and th.trxDate >= @FROM_DATE
                                    and th.trxDate <= @TO_DATE
	                                and tl.promotion_id is not null
                                    group BY PSV.valuechar, PV.promotion_name, th.CustomerId, th.site_id
                                    UNION
                                    select PSV.valuechar As Product, @CUSTOMER_ID As CustomerId, th.site_id as site_id, sum(tl.quantity) as Quantity, max(th.trxDate) as Date, 'PROMOTION - ' + PV.promotion_name as PurchaseType  
                                    from trx_lines tl 
	                                inner join ProductSegmentDataView PSV on tl.product_id = PSV.product_id and valuechar is not null and PSV.SegmentName='ACTIVECAMPAIGN_PRODUCTCATEGORY'
                                    inner join promotions PV on tl.promotion_id = PV.promotion_id and promotion_name is not null
                                    inner join (select TrxId, LineId, c.customer_id
			                                    from customers c,
					                                    Profile p,
					                                    CustomerSignedWaiver csw,
					                                    WaiversSigned ws
			                                    where ws.IsActive = 1
				                                    and ws.CustomerSignedWaiverId = csw.CustomerSignedWaiverId
				                                    and csw.SignedFor = c.customer_id
				                                    and p.id = c.profileId) AS WC 
                                    on WC.TrxId = tl.trxId and WC.LineId = tl.LineId and WC.customer_id = @CUSTOMER_ID
                                    inner join trx_header th on tl.TrxId = th.TrxId
                                    and th.trxDate >= @FROM_DATE
                                    and th.trxDate <= @TO_DATE
	                                and tl.promotion_id is not null
                                    group BY PSV.valuechar, PV.promotion_name, CustomerId, th.site_id
                                ) AS CustomerActivity
                                Group by PurchaseType , Product, CustomerId, site_id";

            parameters.Add(new SqlParameter("@CUSTOMER_ID", Convert.ToInt32(customerId)));
            if (!String.IsNullOrEmpty(accountList))
            {
                selectQuery = selectQuery.Replace("@ACCOUNT_ID_LIST", dataAccessHandler.GetInClauseParameterName(ActiveCampaignCustomerInfoDTO.SearchByParameters.ACCOUNT_ID_LIST, accountList));
                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(ActiveCampaignCustomerInfoDTO.SearchByParameters.ACCOUNT_ID_LIST, accountList));
            }
            else
            {
                parameters.Add(new SqlParameter("@ACCOUNT_ID_LIST", accountList));
            }
            parameters.Add(new SqlParameter("@FROM_DATE", fromdate.ToString("yyyy-MM-dd HH:mm:ss")));
            parameters.Add(new SqlParameter("@TO_DATE", toDate.ToString("yyyy-MM-dd HH:mm:ss")));
            parameters.Add(new SqlParameter("@ACTCAMP_PRODUCTCATEGORY", "ACTIVECAMPAIGN_PRODUCTCATEGORY"));

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<ActiveCampaignCustomerInfoDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ActiveCampaignCustomerInfoDTO customerActivityDTO = GetCustomerActivityDTO(dataRow);
                    list.Add(customerActivityDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

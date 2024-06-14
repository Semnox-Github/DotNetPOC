/********************************************************************************************
 * Project Name - UpsellOffer Data Handler
 * Description  - Data handler of the UpsellOffer class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks                   
 ******************************************************************************************************************
 *1.00        13-Jan-2017  Amaresh          Created 
 *2.70        07-Jul-2019  Indrajeet K      Created DeleteUpsellOffer() for Hard Deletion.
 *2.70.2        10-Dec-2019  Jinto Thomas     Removed siteid from update query
*2.110.00    04-Dec-2020     Prajwal S       Updated Three Tier
*2.140.00    04-Dec-2020     Prajwal S       Modifier :GetUpsellOfferList() Added IsUpsell and productIdlist to the search parameters. 
 ******************************************************************************************************************/
using Semnox.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    ///<summary>
    ///UpsellOfferDataHandler Data Handler - Handles insert, update and select of UpsellOffer Data objects
    ///</summary>
    public class UpsellOfferDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM UpsellOffers AS uo ";


        private static readonly Dictionary<UpsellOffersDTO.SearchByUpsellOffersParameters, string> DBSearchParameters = new Dictionary<UpsellOffersDTO.SearchByUpsellOffersParameters, string>
            {
                {UpsellOffersDTO.SearchByUpsellOffersParameters.PRODUCT_ID, "uo.ProductId"},
                {UpsellOffersDTO.SearchByUpsellOffersParameters.PRODUCT_ID_LIST, "uo.ProductId"},
                {UpsellOffersDTO.SearchByUpsellOffersParameters.OFFER_PRODUCT_ID, "uo.OfferProductId"},
                {UpsellOffersDTO.SearchByUpsellOffersParameters.SALE_GROUP_ID, "uo.SaleGroupId"},
                {UpsellOffersDTO.SearchByUpsellOffersParameters.OFFER_ID, "uo.OfferId"},
                {UpsellOffersDTO.SearchByUpsellOffersParameters.ACTIVE_FLAG, "uo.ActiveFlag"},
                {UpsellOffersDTO.SearchByUpsellOffersParameters.SITE_ID, "uo.site_id"},
               {UpsellOffersDTO.SearchByUpsellOffersParameters.MASTER_ENTITY_ID, "uo.MasterEntityId"},
               {UpsellOffersDTO.SearchByUpsellOffersParameters.IS_UPSELL, "IsUpsell"}
            
    };
        /// Default constructor of UpsellOfferDataHandler class
        /// </summary>
        public UpsellOfferDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        private List<SqlParameter> GetSQLParameters(UpsellOffersDTO upsellOffersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(upsellOffersDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@offerId", upsellOffersDTO.OfferId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", upsellOffersDTO.ProductId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@offerProductId", upsellOffersDTO.OfferProductId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@saleGroupId", upsellOffersDTO.SaleGroupId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@offerMessage", string.IsNullOrEmpty(upsellOffersDTO.OfferMessage) ? DBNull.Value :(object) upsellOffersDTO.OfferMessage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@effectiveDate", upsellOffersDTO.EffectiveDate == DateTime.MinValue ? DBNull.Value: (object)upsellOffersDTO.EffectiveDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@activeFlag", upsellOffersDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", upsellOffersDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the upsellOffersDTO record to the database
        /// </summary>
        /// <param name="upsellOffersDTO">UpsellOffersDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public UpsellOffersDTO InsertUpsellOffer(UpsellOffersDTO upsellOffersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(upsellOffersDTO, loginId, siteId);
            string query = @"insert into UpsellOffers 
                                                        (                                                         
                                                         ProductId,
                                                         OfferProductId,
                                                         SaleGroupId,
                                                         OfferMessage,
                                                         EffectiveDate,
                                                         CreatedBy,
                                                         ActiveFlag,
                                                         site_id,
                                                         Guid,
                                                         LastUpdatedBy,
                                                         LastUpdatedDate,
                                                         MasterEntityId
                                                        ) 
                                                values 
                                                        (                                                        
                                                         @productId,
                                                         @offerProductId,
                                                         @saleGroupId,
                                                         @offerMessage,
                                                         @effectiveDate,
                                                         @createdBy,
                                                         @activeFlag,
                                                         @siteId,
                                                         NEWID(),
                                                         @lastUpdatedBy,
                                                         Getdate(),
                                                         @masterEntityId
                                             )SELECT * FROM UpsellOffers WHERE OfferId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(upsellOffersDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUpsellOffersDTO(upsellOffersDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(upsellOffersDTO);
            return upsellOffersDTO;
        }
    

        /// <summary>
        /// Updates the UpsellOfferDTO record
        /// </summary>
        /// <param name="upsellOffersDTO">UpsellOffersDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public UpsellOffersDTO UpdateUpsellOffer(UpsellOffersDTO upsellOffersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(upsellOffersDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[UpsellOffers] set 
                                                                   [ProductId] =  @productId,
                                                                   [OfferProductId] = @offerProductId,
                                                                    [SaleGroupId] = @saleGroupId,
                                                                    [OfferMessage] =  @offerMessage,
                                                                    [EffectiveDate ]= @effectiveDate,
                                                                    [CreatedBy] = @createdBy,
                                                                    [ActiveFlag] = @activeFlag,
                                                                    [site_id]=  @siteId,
                                                                    [LastUpdatedBy] =  @lastUpdatedBy,
                                                                    [LastUpdatedDate] = Getdate(),
                                                                    [MasterEntityId] =  @masterEntityId
                                                                    where OfferId = @offerId
                                                               SELECT* FROM UpsellOffers WHERE OfferId = @offerId";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(upsellOffersDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUpsellOffersDTO(upsellOffersDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(upsellOffersDTO);
            return upsellOffersDTO;
        }

        /// <summary>
        /// Converts the Data row object to UpsellOffersDTO class type
        /// </summary>
        /// <param name="upsellOfferDataRow">UpsellOffersDTO DataRow</param>
        /// <returns>Returns UpsellOffersDTO</returns>
        private UpsellOffersDTO GetUpsellOffersDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            UpsellOffersDTO UpsellOfferDataObject = new UpsellOffersDTO(dataRow["OfferId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OfferId"]),
                                                    dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                                    dataRow["OfferProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OfferProductId"]),
                                                    dataRow["OfferMessage"].ToString(),
                                                    dataRow["EffectiveDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EffectiveDate"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["ActiveFlag"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["ActiveFlag"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["Guid"].ToString(),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["SaleGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SaleGroupId"])
                                                    );
            log.LogMethodExit(UpsellOfferDataObject);
            return UpsellOfferDataObject;
        }

        /// <summary>
        /// Converts the Data row object to UpsellOffersDTO class type
        /// </summary>
        /// <param name="upsellOfferDataRow">UpsellOffersDTO DataRow</param>
        /// <returns>Returns UpsellOffersDTO</returns>
        private UpsellOfferProductsDTO GetUpsellOffersProductsDTO(DataRow upsellOfferDataRow)
        {
            log.LogMethodEntry(upsellOfferDataRow);
            UpsellOfferProductsDTO upsellOfferDataObject = new UpsellOfferProductsDTO(
                                                   upsellOfferDataRow["product_id"] == DBNull.Value ? -1 : Convert.ToInt32(upsellOfferDataRow["product_id"]),
                                                   upsellOfferDataRow["product_name"].ToString(),
                                                   upsellOfferDataRow["OfferMessage"].ToString(),
                                                   upsellOfferDataRow["price"] == DBNull.Value ? 0 : Convert.ToDouble(upsellOfferDataRow["price"]),
                                                   upsellOfferDataRow["description"].ToString()
                                                   );
            log.LogMethodExit(upsellOfferDataObject);
            return upsellOfferDataObject;
        }

        private void RefreshUpsellOffersDTO(UpsellOffersDTO upsellOffersDTO, DataTable dt)
        {
            log.LogMethodEntry(upsellOffersDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                upsellOffersDTO.OfferId = Convert.ToInt32(dt.Rows[0]["OfferId"]);
                upsellOffersDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                upsellOffersDTO.EffectiveDate = dataRow["EffectiveDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EffectiveDate"]);
                upsellOffersDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                upsellOffersDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                upsellOffersDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                upsellOffersDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// returns the Upsell productsDTO list
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<UpsellOfferProductsDTO> GetUpsellOfferProducts(int productId)
        {
            log.LogMethodEntry(productId);
            string selectUpsellOfferQuery = @"SELECT p.product_id, p.product_name, 
                                              uso.OfferMessage, isnull(case when TaxInclusivePrice = 'Y' then p.price else p.price * (1 + isnull(t.tax_percentage, 0)/100.0) end, 0) as Price,
                                              p.description
                                              FROM products p LEFT OUTER JOIN tax t
                                              on p.tax_id = t.tax_id, UpsellOffers uso
                                              WHERE uso.productId = @productId
                                              AND uso.OfferProductId = p.product_id
                                              AND p.active_flag = 'Y'
                                              AND uso.ActiveFlag = 1
                                              AND isnull(uso.EffectiveDate, getdate()) <= getdate()
                                              ORDER BY uso.effectiveDate DESC";

            SqlParameter[] selectUpsellOfferParameters = new SqlParameter[1];
            selectUpsellOfferParameters[0] = new SqlParameter("@productId", productId);
            DataTable upsellOfferProductsDT = dataAccessHandler.executeSelectQuery(selectUpsellOfferQuery, selectUpsellOfferParameters, sqlTransaction);

            List<UpsellOfferProductsDTO> upsellOfferProductsDTOList = new List<UpsellOfferProductsDTO>();

            if (upsellOfferProductsDT.Rows.Count > 0)
            {
                foreach (DataRow rw in upsellOfferProductsDT.Rows)
                {
                    UpsellOfferProductsDTO upsellOfferProductDataObject = GetUpsellOffersProductsDTO(rw);
                    upsellOfferProductsDTOList.Add(upsellOfferProductDataObject);
                }

                log.LogMethodExit(upsellOfferProductsDTOList);
                return upsellOfferProductsDTOList;
            }
            else
            {
                log.LogMethodExit();
                return new List<UpsellOfferProductsDTO>();
            }
        }

        internal List<UpsellOffersDTO> GetUpsellOffersDTOList(List<int> productIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(productIdList);
            List<UpsellOffersDTO> upsellOffersDTOList = new List<UpsellOffersDTO>();
            string query = @"SELECT *
                            FROM UpsellOffers, @productIdList List
                            WHERE ProductId = List.Id ";
            if(activeRecords)
            {
                query += " AND (ActiveFlag = 1 or ActiveFlag is null)";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@productIdList", productIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                upsellOffersDTOList = table.Rows.Cast<DataRow>().Select(x => GetUpsellOffersDTO(x)).ToList();
            }
            log.LogMethodExit(upsellOffersDTOList);
            return upsellOffersDTOList;
        }

        /// <summary>
        /// returns the Upsell productsDTO list with suggestive sell products
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<UpsellOfferProductsDTO> GetSuggestiveSellOfferProducts(int productId)
        {
            log.LogMethodEntry(productId);
            int dayofweek = -1;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday: dayofweek = 0; break;
                case DayOfWeek.Monday: dayofweek = 1; break;
                case DayOfWeek.Tuesday: dayofweek = 2; break;
                case DayOfWeek.Wednesday: dayofweek = 3; break;
                case DayOfWeek.Thursday: dayofweek = 4; break;
                case DayOfWeek.Friday: dayofweek = 5; break;
                case DayOfWeek.Saturday: dayofweek = 6; break;
                default: break;
            }

            string selectUpsellOfferQuery = @"select product_id, product_name,
                                              Price, description,offerMessage from (
                                                              select distinct p.product_id,
                                                                     p.product_name,
                                                                     isnull(case when TaxInclusivePrice = 'Y' then p.price else p.price * (1 + isnull(t.tax_percentage, 0)/100.0) end, 0) as Price,
                                                                     p.description,
                                                                     sugestiveoffers.offerMessage,
                                                                     sugestiveoffers.SequenceId
                                                                from products p left outer join tax t
                                                                        on p.tax_id = t.tax_id, 
                                                                    (SELECT ProductId, SequenceId, SaleGroupId , offerMessage
                                                                       FROM(SELECT sgpm.ProductId, sgpm.SequenceId, sog.SaleGroupId,uso.offerMessage, DENSE_RANK() OVER (Partition by sgpm.ProductId  ORDER BY sgpm.SequenceId) as ranking
                                                                        FROM (select distinct SaleGroupId , offerMessage
                                                                                from UpsellOffers USO 
                                                                                where uso.ActiveFlag = 1
                                                                                and isnull(uso.EffectiveDate, getdate()) <= getdate()
                                                                                and USO.ProductId = @productId) uso,
                                                                         SalesOfferGroup SoG,
                                                                         SaleGroupProductMap SGPM
                                                                    WHERE uso.SaleGroupId = SOG.SaleGroupId
                                                                        AND SGPM.SaleGroupId = SOG.SaleGroupId
                                                                        AND SOG.IsUpsell = 0
                                                                        AND SOG.IsActive = 1
                                                                        AND SGPM.IsActive = 1
                                                                        AND sgpm.ProductId != @productId)offers 
                                                                        where  ranking = 1 and (not exists (select 1
                                                                                                              from ProductCalendar pc
                                                                                                              where pc.product_id = offers.ProductId)
                                                                        or exists (select 1 from 
                                                                                    (select top 1 date, day, -- select in the order of specific date, day of month, weekday, every day. if there are multiple slots on same day, take the one which is in current hour
                                                                                            case when @nowHour between isnull(FromTime, @nowHour) and isnull(case ToTime when 0 then 24 else ToTime end, @nowHour) then 0 else 1 end sort, 
                                                                                            FromTime, ToTime, ShowHide  
                                                                                    from ProductCalendar pc 
                                                                                        where pc.product_id = offers.ProductId 
                                                                                        and (Date = @today -- specific day
                                                                                        or Day = @DayNumber -- day number 1001 - 1031
                                                                                        or Day = @weekDay -- week day 0-6
                                                                                        or Day = -1) -- everyday
                                                                                        order by 1 desc, 2 desc, 3) inView 
                                                                                        where (ShowHide = 'Y' 
                                                                                            and (@nowHour >= isnull(FromTime, 0) and @nowHour <= case isnull(ToTime, 0) when 0 then 24 else ToTime end))
                                                                                        or (ShowHide = 'N'
                                                                                            and (@nowHour < isnull(FromTime, 0) or @nowHour > case isnull(ToTime, 0) when 0 then 24 else ToTime end)))))sugestiveoffers
                                                                where p.product_id = sugestiveoffers.productId 
                                                                   and p.Modifier = 'N'
                                                                   and p.active_flag = 'Y') as a 
                                                              order by SequenceId";

            SqlParameter[] selectUpsellOfferParameters = new SqlParameter[5];
            selectUpsellOfferParameters[0] = new SqlParameter("@productId", productId);
            selectUpsellOfferParameters[1] = new SqlParameter("@today", DateTime.Now);
            selectUpsellOfferParameters[2] = new SqlParameter("@nowHour", DateTime.Now.Hour + DateTime.Now.Minute / 100.0);
            selectUpsellOfferParameters[3] = new SqlParameter("@DayNumber", DateTime.Now.Day + 1000);
            selectUpsellOfferParameters[4] = new SqlParameter("@weekDay", dayofweek);

            DataTable upsellOfferProductsDT;
            upsellOfferProductsDT = dataAccessHandler.executeSelectQuery(selectUpsellOfferQuery, selectUpsellOfferParameters, sqlTransaction);

            List<UpsellOfferProductsDTO> upsellOfferProductsDTOList;
            upsellOfferProductsDTOList = new List<UpsellOfferProductsDTO>();

            if (upsellOfferProductsDT.Rows.Count > 0)
            {
                foreach (DataRow rw in upsellOfferProductsDT.Rows)
                {
                    UpsellOfferProductsDTO upsellOfferProductDataObject = GetUpsellOffersProductsDTO(rw);
                    upsellOfferProductsDTOList.Add(upsellOfferProductDataObject);
                }

                log.LogMethodExit(upsellOfferProductsDTOList);
                return upsellOfferProductsDTOList;
            }
            else
            {
                log.LogMethodExit(upsellOfferProductsDTOList);
                return upsellOfferProductsDTOList;
            }
        }
        /// <summary>
        /// Gets the GetUpsellOffer data of passed offerId
        /// </summary>
        /// <param name="offerId">integer type parameter</param>
        /// <returns>Returns UpsellOffersDTO</returns>
        public UpsellOffersDTO GetUpsellOffer(int offerId)
        {
            log.LogMethodEntry(offerId);
            UpsellOffersDTO upsellOffersDTO = null;
            string query = SELECT_QUERY + @" WHERE uo.OfferId = @offerId";
            SqlParameter parameter = new SqlParameter("@offerId", offerId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                upsellOffersDTO = GetUpsellOffersDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(upsellOffersDTO);
            return upsellOffersDTO;
        }
        
        /// <summary>
        /// Gets the UpsellOffersDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of UpsellOffersDTO matching the search criteria</returns>
        public List<UpsellOffersDTO> GetUpsellOfferList(List<KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>> searchParameters, SqlTransaction sqlTransaction =null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<UpsellOffersDTO> upsellOffersDTOList = new List<UpsellOffersDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(UpsellOffersDTO.SearchByUpsellOffersParameters.OFFER_ID)
                            || searchParameter.Key.Equals(UpsellOffersDTO.SearchByUpsellOffersParameters.PRODUCT_ID) 
                            || searchParameter.Key.Equals(UpsellOffersDTO.SearchByUpsellOffersParameters.MASTER_ENTITY_ID) ||
                               searchParameter.Key.Equals(UpsellOffersDTO.SearchByUpsellOffersParameters.OFFER_PRODUCT_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(UpsellOffersDTO.SearchByUpsellOffersParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                         else if (searchParameter.Key.Equals(UpsellOffersDTO.SearchByUpsellOffersParameters.IS_UPSELL))
                        {
                            query.Append(joiner + "uo.SaleGroupId in (select SaleGroupId from SalesOfferGroup where " + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UpsellOffersDTO.SearchByUpsellOffersParameters.PRODUCT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(UpsellOffersDTO.SearchByUpsellOffersParameters.ACTIVE_FLAG))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y")));
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
                    count++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                upsellOffersDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetUpsellOffersDTO(x)).ToList();
            }
            log.LogMethodExit(upsellOffersDTOList);
            return upsellOffersDTOList;
        }

            
        /// <summary>
        /// Deletes the UpsellOffers based on the OfferId
        /// </summary>
        /// <param name="OfferId">OfferId</param>
        /// <returns>return the int</returns>
        public int DeleteUpsellOffer(int offerId)
        {
            log.LogMethodEntry(offerId);
            try
            {
                string deleteQuery = @"delete from UpsellOffers where OfferId = @offerId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@offerId", offerId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
    }
}

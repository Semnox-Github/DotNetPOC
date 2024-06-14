/* Project Name - BOMDataHandler 
* Description  - Data handler object of the BOM
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.60.3      14-Jun-2019   Nagesh Badiger          Added who columns in insert and update method.
*2.70.2      10-Dec-2019   Jinto Thomas            Removed site id from update query
*2.100.0     24-Aug-2020   Deeksha                 Modified as per 3 tier standard , Added new fields as part of Recipe Mgt enhancement.
********************************************************************************************/

using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Data Handler of BOM Class
    /// </summary>
    public class BOMDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ProductBOM AS bom ";
        private static readonly Dictionary<BOMDTO.SearchByBOMParameters, string> DBSearchParameters = new Dictionary<BOMDTO.SearchByBOMParameters, string>
              {
                    {BOMDTO.SearchByBOMParameters.BOMID, "bom.BOMId"},
                    {BOMDTO.SearchByBOMParameters.PRODUCT_ID, "bom.ProductId"},
                    {BOMDTO.SearchByBOMParameters.CHILDPRODUCT_ID, "bom.ChildProductId"},
                    {BOMDTO.SearchByBOMParameters.SITEID, "bom.site_id"},
                    {BOMDTO.SearchByBOMParameters.IS_ACTIVE, "bom.isactive"},
                    {BOMDTO.SearchByBOMParameters.UOM_ID, "bom.UOMId"},
                    {BOMDTO.SearchByBOMParameters.PRODUCT_ID_LIST, "bom.ProductId"}
               };



        public BOMDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to VendorDTO class type
        /// </summary>
        /// <param name="bomDataRow">VendorDTO DataRow</param>
        /// <returns>Returns VendorDTO</returns>
        private BOMDTO GetBOMDTO(DataRow bomDataRow)
        {
            log.LogMethodEntry(bomDataRow);

            BOMDTO bomDataObject = new BOMDTO(Convert.ToInt32(bomDataRow["BOMId"]),
                                            bomDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(bomDataRow["ProductId"]),
                                            bomDataRow["ChildProductId"] == DBNull.Value ? -1 : Convert.ToInt32(bomDataRow["ChildProductId"]),
                                            bomDataRow["Quantity"] == DBNull.Value ? 0 : Convert.ToDecimal(bomDataRow["Quantity"]),
                                            bomDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(bomDataRow["site_id"]),
                                            bomDataRow["Guid"].ToString(),
                                            bomDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(bomDataRow["SynchStatus"]),
                                            bomDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(bomDataRow["MasterEntityId"]),
                                            bomDataRow["Isactive"] == DBNull.Value ? true : Convert.ToBoolean(bomDataRow["isactive"]),
                                            string.IsNullOrEmpty(bomDataRow["CreatedBy"].ToString()) ? "" : Convert.ToString(bomDataRow["CreatedBy"]),
                                            bomDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(bomDataRow["CreationDate"]),
                                            string.IsNullOrEmpty(bomDataRow["LastUpdatedBy"].ToString()) ? "" : Convert.ToString(bomDataRow["LastUpdatedBy"]),
                                            bomDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(bomDataRow["LastUpdateDate"]),
                                            bomDataRow["PreparationOffsetMins"] == DBNull.Value ? (int?)null : Convert.ToInt32(bomDataRow["PreparationOffsetMins"]),
                                            bomDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(bomDataRow["UOMId"]),
                                            bomDataRow["PreparationRemarks"] == DBNull.Value ? null : Convert.ToString(bomDataRow["PreparationRemarks"])
            );
            log.LogMethodExit(bomDataObject);
            return bomDataObject;
        }
        /// <summary>
        /// Gets the BOM data of passed BOM id
        /// </summary>
        /// <param name="BOMId">integer type parameter</param>
        /// <returns>Returns BOMDTO</returns>
        public BOMDTO GetBOMDTO(int bomId)
        {
            log.LogMethodEntry(bomId);
            BOMDTO result = null;
            string query = SELECT_QUERY + @" WHERE bom.BOMId = @BOMId";
            SqlParameter parameter = new SqlParameter("@BOMId", bomId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetBOMDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ProductBarcodeDTO List for ProductBarcodeSet Id List
        /// </summary>
        /// <param name="productIdList">integer list parameter</param>
        /// <returns>Returns List of ProductBarcodeSetDTO</returns>
        public List<BOMDTO> GetProductBOMDTOListOfProducts(List<int> productIdList, bool activeRecords)
        {
            log.LogMethodEntry(productIdList);
            List<BOMDTO> list = new List<BOMDTO>();
            string query = @"SELECT ProductBOM.*
                                FROM  ProductBOM , @ProductIdList List 
                                WHERE ProductBOM.ProductId = List.Id";
            if (activeRecords)
            {
                query += " AND ProductBOM.isActive = 1";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@ProductIdList", productIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetBOMDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        ///<summary>
        ///Gets the BOMDTO list with the matching Product ID
        ///</summary>
        public List<BOMDTO> GetProductDetailsList(int productId)
        {
            log.LogMethodEntry(productId);
            string query = @"select * from ProductBOM where ProductId = @productId";
            SqlParameter[] selectBOMParameter = new SqlParameter[1];
            selectBOMParameter[0] = new SqlParameter("@productId", productId);
            DataTable bom = dataAccessHandler.executeSelectQuery(query, selectBOMParameter, sqlTransaction);
            if (bom.Rows.Count > 0)
            {
                List<BOMDTO> BOMList = new List<BOMDTO>();
                foreach (DataRow BOMDataRow in bom.Rows)
                {
                    BOMDTO BOMDataObject = GetBOMDTO(BOMDataRow);
                    BOMList.Add(BOMDataObject);
                }
                log.LogMethodExit(BOMList);
                return BOMList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }

        /// <summary>
        /// Gets the BOMDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of VendorDTO matching the search criteria</returns>
        public List<BOMDTO> GetBOMList(List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<BOMDTO> bomDTOList = new List<BOMDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectBOMQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<BOMDTO.SearchByBOMParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ((searchParameter.Key.Equals(BOMDTO.SearchByBOMParameters.PRODUCT_ID)) ||
                            (searchParameter.Key.Equals(BOMDTO.SearchByBOMParameters.BOMID)) ||
                            (searchParameter.Key.Equals(BOMDTO.SearchByBOMParameters.UOM_ID)) ||
                           (searchParameter.Key.Equals(BOMDTO.SearchByBOMParameters.CHILDPRODUCT_ID)))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == BOMDTO.SearchByBOMParameters.SITEID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(BOMDTO.SearchByBOMParameters.PRODUCT_ID_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == BOMDTO.SearchByBOMParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                if (searchParameters.Count > 0)
                    selectBOMQuery = selectBOMQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectBOMQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                bomDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetBOMDTO(x)).ToList();
            }
            log.LogMethodExit(bomDTOList);
            return bomDTOList;
        }

        private List<SqlParameter> GetSQLParameters(BOMDTO bomDTO, string userId, int siteId)
        {
            log.LogMethodEntry(bomDTO, siteId, userId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@BOMId", bomDTO.BOMId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", bomDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ChildProductId", bomDTO.ChildProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Quantity", bomDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", bomDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", bomDTO.Isactive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PreparationOffsetMins", bomDTO.PreparationOffsetMins));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UOMId", bomDTO.UOMId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PreparationRemarks", bomDTO.PreparationRemarks));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Insert Query for BOMDTO
        /// </summary>
        public BOMDTO InsertBOM(BOMDTO bomDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(bomDTO, loginId, siteId);
            string insertBOMQuery = @"INSERT INTO [ProductBOM]
                                               ([ProductId]
                                               ,[ChildProductId]
                                               ,[Quantity]
                                               ,[site_id]
                                               ,[Guid]
                                               ,[MasterEntityId]
                                               ,[Isactive]
                                               ,[CreatedBy]
                                               ,[CreationDate]         
                                               ,[LastUpdatedBy]
                                               ,[LastUpdateDate]
                                               ,[PreparationOffsetMins]
                                               ,[UOMId]
                                               ,[PreparationRemarks]
                                               )
                                         VALUES
                                               (@ProductID
                                               ,@ChildProductId
                                               ,@Quantity
                                               ,@site_id
                                               ,NewID()
                                               ,@MasterEntityId
                                               ,@Isactive
                                               ,@createdBy
                                               ,getdate()
                                               ,@lastUpdatedBy
                                               ,getdate()
                                               ,@PreparationOffsetMins
                                               ,@UOMId
                                               ,@PreparationRemarks
                                                )SELECT * FROM ProductBOM WHERE BOMId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertBOMQuery, GetSQLParameters(bomDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshBOMDTO(bomDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(bomDTO);
            return bomDTO;
        }

        private void RefreshBOMDTO(BOMDTO bomDTO, DataTable dt)
        {
            log.LogMethodEntry(bomDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                bomDTO.BOMId = Convert.ToInt32(dt.Rows[0]["BOMId"]);
                bomDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                bomDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                bomDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                bomDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                bomDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                bomDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Update Query for BOMDTO
        /// </summary>
        public BOMDTO UpdateBOM(BOMDTO bomDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(bomDTO, loginId, siteId);
            string updateBOMQuery = @"UPDATE [dbo].[ProductBOM]
                                           SET [ProductId] = @ProductId
                                              ,[ChildProductId] = @ChildProductId
                                              ,[Quantity] = @Quantity
                                              ,[site_id] = @site_id
                                              ,[MasterEntityId] = @MasterEntityId
                                              ,[Isactive] = @Isactive
                                              ,[LastUpdatedBy]=@lastUpdatedBy
                                              ,[LastUpdateDate]=getdate()
                                              ,[PreparationOffsetMins]=@PreparationOffsetMins
                                              ,[UOMId]=@UOMId
                                              ,[PreparationRemarks]=@PreparationRemarks
                                         WHERE BOMID = @BOMId
                                        SELECT * FROM ProductBOM WHERE BOMID = @BOMID"; 
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateBOMQuery, GetSQLParameters(bomDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshBOMDTO(bomDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(bomDTO);
            return bomDTO;
        }

        internal List<BOMDTO> GetEventProductBOM(DateTime fromDate , DateTime todate)
        {
            log.LogMethodEntry(fromDate);
            List<BOMDTO> bomDTOList = new List<BOMDTO>();
            BOMDTO bOMDTO = null;
            string query = @"select distinct(pb.ChildProductId),pb.Quantity,th.TrxDate from Bookings b , trx_header th , trx_lines tl, product p , ProductBOM pb
				 where th.TrxId = b.TrxId and th.Status = 'RESERVED' and  b.Status != 'CANCELLED'   
				and b.FromDate >= @fromDate and b.FromDate < @todate and tl.TrxId = th.TrxId and tl.product_id = p.ManualProductId and p.ProductId = pb.ProductId";

            List<SqlParameter> parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@fromDate", fromDate));
            parameter.Add(new SqlParameter("@todate", todate));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameter.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0 )
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    bOMDTO = new BOMDTO();
                    bOMDTO.Quantity = Convert.ToDecimal(dataTable.Rows[0]["Quantity"]);
                    bOMDTO.TrxDate = Convert.ToDateTime(dataTable.Rows[0]["TrxDate"]);
                    bOMDTO.ChildProductId = Convert.ToInt32(dataTable.Rows[0]["ChildProductId"]);
                    bomDTOList.Add(bOMDTO);
                }
            }
            log.LogMethodExit(bomDTOList);
            return bomDTOList;
        }

        /// <summary>
        /// UOM
        /// </summary>
        /// <param name="uomId"></param>
        /// <returns></returns>
        public string GetUOMById(int uomId = -1)
        {
            log.LogMethodEntry(uomId);
            string uOM = string.Empty;
            string selectUOMQuery = @"select UOM 
                                            from UOM u 
                                           where u.UOMId = @uOMId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@uOMId", uomId);
            DataTable dt = dataAccessHandler.executeSelectQuery(selectUOMQuery, selectParameters, sqlTransaction);
            foreach (DataRow i in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    uOM = i[0].ToString();
                    break;
                }
            }
            log.LogMethodExit(uOM);
            return uOM;
        }

        /// <summary>
        /// UOM
        /// </summary>
        /// <param name="uomId"></param>
        /// <returns></returns>
        public int GetUOMByName(string uOM = null)
        {
            log.LogMethodEntry(uOM);
            int uomId = -1;
            string selectUOMQuery = @"select UOMId 
                                            from UOM u 
                                           where u.UOM = @uOM ";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@uOM", uOM);
            DataTable dt = dataAccessHandler.executeSelectQuery(selectUOMQuery, selectParameters, sqlTransaction);
            foreach (DataRow i in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    uomId = Convert.ToInt32(i[0]);
                    break;
                }
            }
            log.LogMethodExit(uomId);
            return uomId;
        }
    }
}

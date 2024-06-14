/* Project Name - Semnox.Parafait.Product.ComboProductDataHandler 
* Description  - Data handler object of the ComboProduct
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes
*2.60        15-Feb-2019    Nagesh Badiger       Added  isActive property
*2.70        28-Mar-2018    Guru S A             Booking phase2 enhancement changes 
*            07-Jul-2019    Indrajeet Kumar      Created DeleteComboProduct() method for Hard Deletion
*2.70.2      10-Dec-2019    Jinto Thomas         Removed siteid from update query            
*            02-Jan-2020    Akshay G             Added ExternalSystemReference and Added searchParameters
*2.150.0      28-Mar-2022   Girish Kundar        Modified : Added a new column  MaximumQuantity & PauseType to Products
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class ComboProductDataHandler
    {
        DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Dictionary<ComboProductDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ComboProductDTO.SearchByParameters, string>
            {
                {ComboProductDTO.SearchByParameters.COMBOPRODUCT_ID, "comboP.id"},
                {ComboProductDTO.SearchByParameters.PRODUCT_ID, "comboP.Product_Id"},
                {ComboProductDTO.SearchByParameters.CHILD_PRODUCT_ID, "comboP.ChildProductId"},
                {ComboProductDTO.SearchByParameters.CHILD_PRODUCT_TYPE, "pt.product_type"},
                {ComboProductDTO.SearchByParameters.CATEGORY_ID,  "comboP.CategoryId"},
                {ComboProductDTO.SearchByParameters.DISPLAY_GROUP_ID,  "comboP.DisplayGroupId"},
                {ComboProductDTO.SearchByParameters.ADDITIONAL_PRODUCT,  "comboP.AdditionalProduct"},
                {ComboProductDTO.SearchByParameters.PRICE_INCLUSIVE,  "comboP.PriceInclusive"},
                {ComboProductDTO.SearchByParameters.MASTER_ENTITY_ID, "comboP.MasterEntityId"},
                {ComboProductDTO.SearchByParameters.SITE_ID, "comboP.site_id"},
                {ComboProductDTO.SearchByParameters.IS_ACTIVE, "comboP.IsActive"},
                {ComboProductDTO.SearchByParameters.PARENT_PRODUCT_TYPE, "ppt.product_type"},
                {ComboProductDTO.SearchByParameters.PARENT_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE, "pp.ExternalSystemReference"},
                {ComboProductDTO.SearchByParameters.CHILD_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE, "p.ExternalSystemReference"},
                {ComboProductDTO.SearchByParameters.COMBO_PRODUCT_WITH_ENTITY_LAST_UPDATE_DATE_IS_GREATER_THAN, "comboP.LastUpdateDate"},
                {ComboProductDTO.SearchByParameters.COMBO_PRODUCT_HAS_EXTERNAL_SYSTEM_REFERENCE, "comboP.ExternalSystemReference"},
                {ComboProductDTO.SearchByParameters.PRODUCT_ID_LIST, "comboP.Product_Id"},
                {ComboProductDTO.SearchByParameters.CHILD_PRODUCT_ID_LIST, "comboP.ChildProductId"},
                {ComboProductDTO.SearchByParameters.HAS_ACTIVE_SUBSCRIPTION_CHILD, ""}
            };

        private static readonly string cmbProductSelectQry = @"SELECT comboP.*, 
                                                                      pt.product_type as ChildProductType, p.product_name as ChildProductName, p.autogeneratecardnumber as ChildProductAutoGenerateCardNumber
                                                                 FROM ComboProduct comboP 
                                                                      left outer join products p on combop.ChildProductId = p.product_id 
                                                                      left outer join product_type pt on p.product_type_id = pt.product_type_id ";

        /// <summary>
        /// Default constructor of  ComboProductDataHandler class
        /// </summary>
        public ComboProductDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating comboProduct Record.
        /// </summary>
        /// <param name="comboProductDTO">ComboProductDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ComboProductDTO comboProductDTO, string userId, int siteId)
        {
            log.LogMethodEntry(comboProductDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                dataAccessHandler.GetSQLParameter("@ComboProductId", comboProductDTO.ComboProductId, true),
                dataAccessHandler.GetSQLParameter("@ProductId", comboProductDTO.ProductId, true),
                dataAccessHandler.GetSQLParameter("@ChildProductId", comboProductDTO.ChildProductId, true),
                dataAccessHandler.GetSQLParameter("@CategoryId", comboProductDTO.CategoryId, true),
                dataAccessHandler.GetSQLParameter("@DisplayGroupId", comboProductDTO.DisplayGroupId, true),
                dataAccessHandler.GetSQLParameter("@DisplayGroup", comboProductDTO.DisplayGroup),
                dataAccessHandler.GetSQLParameter("@Quantity", comboProductDTO.Quantity),
                dataAccessHandler.GetSQLParameter("@PriceInclusive", ((comboProductDTO.PriceInclusive == true) ? "Y" : "N")),
                dataAccessHandler.GetSQLParameter("@AdditionalProduct", ((comboProductDTO.AdditionalProduct == true) ? "Y" : "N")),
                dataAccessHandler.GetSQLParameter("@Price", comboProductDTO.Price),
                dataAccessHandler.GetSQLParameter("@SortOrder", comboProductDTO.SortOrder),
                dataAccessHandler.GetSQLParameter("@site_id", siteId, true),
                dataAccessHandler.GetSQLParameter("@MasterEntityId", comboProductDTO.MasterEntityId, true),
                dataAccessHandler.GetSQLParameter("@CreatedBy", userId),
                dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId),
                dataAccessHandler.GetSQLParameter("@isActive", comboProductDTO.IsActive),
                dataAccessHandler.GetSQLParameter("@externalSystemReference", comboProductDTO.ExternalSystemReference),
                dataAccessHandler.GetSQLParameter("@MaximumQuantity", comboProductDTO.MaximumQuantity)
            };
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ComboProduct record to the database
        /// </summary>
        /// <param name="comboProductDTO">ComboProductDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertComboProduct(ComboProductDTO comboProductDTO, string userId, int siteId)
        {
            log.LogMethodEntry(comboProductDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"
                            INSERT INTO dbo.ComboProduct
                                       (Product_Id
                                       ,ChildProductId
                                       ,Quantity
                                       ,Guid
                                       ,site_id 
                                       ,CategoryId
                                       ,display_group
                                       ,PriceInclusive
                                       ,AdditionalProduct
                                       ,MasterEntityId
                                       ,DisplayGroupId
                                       ,Price
                                       ,SortOrder
                                       ,CreatedBy
                                       ,CreationDate
                                       ,LastUpdatedBy
                                       ,LastUpdateDate
                                       ,IsActive
                                       ,ExternalSystemReference
                                       ,MaximumQuantity)
                                 VALUES
                                       (@ProductId
                                       ,@ChildProductId
                                       ,@Quantity 
                                       ,NEWID()
                                       ,@site_id  
                                       ,@CategoryId 
                                       ,@DisplayGroup
                                       ,@PriceInclusive
                                       ,@AdditionalProduct
                                       ,@MasterEntityId
                                       ,@DisplayGroupId
                                       ,@Price
                                       ,@SortOrder
                                       ,@CreatedBy
                                       ,getdate()
                                       ,@LastUpdatedBy
                                       ,getdate()
                                       ,@isActive
                                       ,@externalSystemReference
                                       ,@MaximumQuantity)
                             SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(comboProductDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the ComboProduct record
        /// </summary>
        /// <param name="comboProductDTO">ComboProductDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateComboProduct(ComboProductDTO comboProductDTO, string userId, int siteId)
        {
            log.LogMethodEntry(comboProductDTO, userId, siteId);
            int rowsUpdated;
            string query = @"
                            UPDATE dbo.ComboProduct
                               SET Product_Id = @ProductId
                                  ,ChildProductId = @ChildProductId
                                  ,Quantity = @Quantity 
                                  -- ,site_id = @site_id 
                                  ,CategoryId = @CategoryId
                                  ,display_group = @DisplayGroup
                                  ,PriceInclusive = @PriceInclusive
                                  ,AdditionalProduct = @AdditionalProduct
                                  ,MasterEntityId = @MasterEntityId
                                  ,DisplayGroupId = @DisplayGroupId
                                  ,Price = @Price
                                  ,SortOrder = @SortOrder 
                                  ,LastUpdatedBy = @LastUpdatedBy
                                  ,LastUpdateDate = getdate()
                                  ,IsActive=@isActive
                                  ,ExternalSystemReference=@externalSystemReference
                                  ,MaximumQuantity=@MaximumQuantity
                             WHERE Id = @ComboProductId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(comboProductDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }



        /// <summary>
        /// Converts the Data row object to GetComboProductDTO calss type
        /// </summary>
        /// <param name="dataRow">ComboProduct DataRow</param>
        /// <returns>Returns ComboProductDTO</returns>
        private ComboProductDTO GetComboProductDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ComboProductDTO comboProductDTO = new ComboProductDTO(
                                                                   Convert.ToInt32(dataRow["Id"]),
                                                                   string.IsNullOrEmpty(dataRow["Product_Id"].ToString()) ? -1 : Convert.ToInt32(dataRow["Product_Id"]),
                                                                   string.IsNullOrEmpty(dataRow["ChildProductId"].ToString()) ? -1 : Convert.ToInt32(dataRow["ChildProductId"]),
                                                                   string.IsNullOrEmpty(dataRow["Quantity"].ToString()) ? (int?)null : Convert.ToInt32(dataRow["Quantity"]),
                                                                   string.IsNullOrEmpty(dataRow["CategoryId"].ToString()) ? -1 : Convert.ToInt32(dataRow["CategoryId"]),
                                                                   dataRow["display_group"].ToString(),
                                                                   string.IsNullOrEmpty(dataRow["priceInclusive"].ToString()) ? false : ((dataRow["priceInclusive"].ToString() == "Y" ? true : false)),
                                                                   string.IsNullOrEmpty(dataRow["additionalProduct"].ToString()) ? false : ((dataRow["additionalProduct"].ToString() == "Y" ? true : false)),
                                                                   string.IsNullOrEmpty(dataRow["displayGroupId"].ToString()) ? -1 : Convert.ToInt32(dataRow["displayGroupId"]),
                                                                    string.IsNullOrEmpty(dataRow["Price"].ToString()) ? (double?)null : Convert.ToDouble(dataRow["Price"]),
                                                                   string.IsNullOrEmpty(dataRow["SortOrder"].ToString()) ? (int?)null : Convert.ToInt32(dataRow["SortOrder"]),
                                                                   string.IsNullOrEmpty(dataRow["site_id"].ToString()) ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                                   dataRow["Guid"].ToString(),
                                                                   string.IsNullOrEmpty(dataRow["SynchStatus"].ToString()) ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                                   string.IsNullOrEmpty(dataRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                                   string.IsNullOrEmpty(dataRow["CreatedBy"].ToString()) ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                                                   dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                                   string.IsNullOrEmpty(dataRow["LastUpdatedBy"].ToString()) ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                                    dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                                    dataRow["ChildProductType"].ToString(),
                                                                    dataRow["ChildProductName"].ToString(),
                                                                    dataRow["ChildProductAutoGenerateCardNumber"].ToString(),
                                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                                    dataRow["ExternalSystemReference"].ToString(),
                                                                    dataRow["MaximumQuantity"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["MaximumQuantity"])
                                                                   );
            log.LogMethodExit(comboProductDTO);
            return comboProductDTO;
        }

        /// <summary>
        /// Gets the ComboProduct data of passed comboProduct Id
        /// </summary>
        /// <param name="comboProductId">integer type parameter</param>
        /// <returns>Returns ComboProductDTO</returns>
        public ComboProductDTO GetComboProductDTO(int comboProductId)
        {
            log.LogMethodEntry(comboProductId);
            string selectComboProductQuery = cmbProductSelectQry + "  WHERE id = @comboProductId";
            SqlParameter[] selectComboProductParameters = new SqlParameter[1];
            selectComboProductParameters[0] = new SqlParameter("@comboProductId", comboProductId);
            DataTable comboProduct = dataAccessHandler.executeSelectQuery(selectComboProductQuery, selectComboProductParameters, sqlTransaction);
            ComboProductDTO comboProductDataObject = null;
            if (comboProduct.Rows.Count > 0)
            {
                DataRow ComboProductRow = comboProduct.Rows[0];
                comboProductDataObject = GetComboProductDTO(ComboProductRow);
            }
            log.LogMethodExit(comboProductDataObject);
            return comboProductDataObject;
        }

        internal List<ComboProductDTO> GetComboProductDTOList(List<int> productIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(productIdList);
            List<ComboProductDTO> productIdListDetailsDTOList = new List<ComboProductDTO>();
            string query = @"SELECT comboP.*, 
                            pt.product_type as ChildProductType, p.product_name as ChildProductName, p.autogeneratecardnumber as ChildProductAutoGenerateCardNumber
                            FROM ComboProduct comboP 
                            left outer join products p on combop.ChildProductId = p.product_id 
                            left outer join product_type pt on p.product_type_id = pt.product_type_id
                            INNER JOIN @productIdList List ON comboP.Product_Id = List.Id ";
            if (activeRecords)
            {
                query += " AND (comboP.IsActive = 1  or comboP.IsActive is null)";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@productIdList", productIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productIdListDetailsDTOList = table.Rows.Cast<DataRow>().Select(x => GetComboProductDTO(x)).ToList();
            }
            log.LogMethodExit(productIdListDetailsDTOList);
            return productIdListDetailsDTOList;
        }


        /// <summary>
        /// Gets the ComboProductDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ComboProductDTO matching the search criteria</returns>
        public List<ComboProductDTO> GetComboProductDTOList(List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ComboProductDTO> list = null;
            int count = 0;
            string selectQuery = cmbProductSelectQry;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ComboProductDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ComboProductDTO.SearchByParameters.COMBOPRODUCT_ID ||
                            searchParameter.Key == ComboProductDTO.SearchByParameters.CHILD_PRODUCT_ID ||
                            searchParameter.Key == ComboProductDTO.SearchByParameters.PRODUCT_ID ||
                            searchParameter.Key == ComboProductDTO.SearchByParameters.DISPLAY_GROUP_ID ||
                            searchParameter.Key == ComboProductDTO.SearchByParameters.CATEGORY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == ComboProductDTO.SearchByParameters.CHILD_PRODUCT_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "= '" + searchParameter.Value + "' ");
                        }
                        else if (searchParameter.Key == ComboProductDTO.SearchByParameters.SITE_ID ||
                                 searchParameter.Key == ComboProductDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == ComboProductDTO.SearchByParameters.ADDITIONAL_PRODUCT ||
                                 searchParameter.Key == ComboProductDTO.SearchByParameters.PRICE_INCLUSIVE
                            )
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = " + (searchParameter.Value == "1" ? "'Y'" : "'N'"));
                        }
                        else if (searchParameter.Key == ComboProductDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + "'" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == ComboProductDTO.SearchByParameters.PARENT_PRODUCT_TYPE)
                        {
                            query.Append(joiner + @"EXISTS(SELECT 1 FROM Products pp, product_type ppt
		                                                                WHERE pp.product_type_id = ppt.product_type_id
		                                                                AND pp.product_id = comboP.Product_Id
		                                                                AND " + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "' )");
                        }
                        else if (searchParameter.Key == ComboProductDTO.SearchByParameters.PARENT_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE)
                        {
                            query.Append(joiner + @"( ISNULL( (SELECT 1 FROM Products pp,product_type ppt
				                                                        WHERE pp.product_type_id = ppt.product_type_id
				                                                        AND pp.product_id = comboP.Product_Id
				                                                        AND " + DBSearchParameters[searchParameter.Key] + " IS NOT NULL),'0') = '" + searchParameter.Value + "' )");
                        }
                        else if (searchParameter.Key == ComboProductDTO.SearchByParameters.CHILD_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE ||
                                 searchParameter.Key == ComboProductDTO.SearchByParameters.COMBO_PRODUCT_HAS_EXTERNAL_SYSTEM_REFERENCE) // bit
                        {
                            query.Append(joiner + "( ISNULL( CASE WHEN " + DBSearchParameters[searchParameter.Key] + " IS NOT NULL THEN '1' ELSE '0' END, '0') = '" + searchParameter.Value + "' )");
                        }
                        else if (searchParameter.Key == ComboProductDTO.SearchByParameters.PRODUCT_ID_LIST ||
                                 searchParameter.Key == ComboProductDTO.SearchByParameters.CHILD_PRODUCT_ID_LIST)
                        {
                            query.Append(joiner + "( " + DBSearchParameters[searchParameter.Key] + " IN (" + searchParameter.Value + " ))");
                        }
                        else if (searchParameter.Key == ComboProductDTO.SearchByParameters.COMBO_PRODUCT_WITH_ENTITY_LAST_UPDATE_DATE_IS_GREATER_THAN)
                        {
                            query.Append(joiner + @" EXISTS( SELECT 1 FROM ProductsAllowedInFacility paif  
								                                    WHERE paif.ProductsId = comboP.ChildProductId
								                                    AND paif.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
                                                            UNION ALL 
																SELECT 1 FROM CustomData cd
																	WHERE cd.CustomDataSetId=p.CustomDataSetId
																	AND cd.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
						                                    UNION ALL 
							                                    SELECT 1 FROM FacilityMap vf, ProductsAllowedInFacility paif
								                                    WHERE paif.ProductsId=comboP.ChildProductId
								                                    AND paif.FacilityMapId=vf.FacilityMapId
								                                    AND vf.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
                                                            UNION ALL
                                                                SELECT 1 FROM FacilityMapDetails fmd, ProductsAllowedInFacility paif
								                                    WHERE paif.ProductsId=comboP.ChildProductId
								                                    AND fmd.FacilityMapId = paif.FacilityMapId 
								                                    AND fmd.LastUpdateDate > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
						                                    UNION ALL
							                                    SELECT 1 FROM CheckInFacility cif, FacilityMapDetails fmd, ProductsAllowedInFacility paif
								                                    WHERE paif.ProductsId = comboP.ChildProductId
								                                    AND fmd.FacilityMapId = paif.FacilityMapId
								                                    AND cif.FacilityId = fmd.FacilityId
								                                    AND cif.last_updated_date > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"'
					                                        OR  p.last_updated_date > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + @"' 
                                                            OR  " + DBSearchParameters[searchParameter.Key] + " > '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "' ) ");
                        }
                        else if (searchParameter.Key == ComboProductDTO.SearchByParameters.HAS_ACTIVE_SUBSCRIPTION_CHILD)
                        {
                            query.Append(joiner + @"  ISNULL((SELECT 1  --for child products
                                                               FROM ProductSubscription ps 
                                                              WHERE ps.ProductsId = combop.ChildProductId
                                                                AND ISNULL(combop.IsActive,1) = 1
                                                               UNION ALL
                                                              SELECT 1 -- for catagory products
                                                                FROM ProductSubscription ps, products catgp
                                                               WHERE ps.ProductsId = catgp.product_id
                                                                 AND catgp.CategoryId = comboP.CategoryId
                                                                 AND ISNULL(combop.IsActive,1) = 1
                                                                ),0) =  " + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + " N'%" + searchParameter.Value + "%'");
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
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<ComboProductDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ComboProductDTO comboProductDTO = GetComboProductDTO(dataRow);
                    list.Add(comboProductDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
        /// <summary>
        /// Deletes the ComboProduct based on the comboProductId
        /// </summary>
        /// <param name="comboProductId">comboProductId</param>
        /// <returns>return the int</returns>
        public int DeleteComboProduct(int comboProductId)
        {
            log.LogMethodEntry(comboProductId);
            try
            {
                string deleteQuery = @"delete from ComboProduct where Id = @id";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@id", comboProductId);
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
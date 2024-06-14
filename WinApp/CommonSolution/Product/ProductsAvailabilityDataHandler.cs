/********************************************************************************************
 * Project Name - Products Availability Data Handler
 * Description  - Data Handler for ProductsAvailabilityStatus functionality
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        05-Mar-2019      Nitin Pai      86-68 Created 
 *2.70.2      10-Dec-2019      Jinto Thomas   Removed siteid from update query
 *2.110.00    01-Dec-2020      Abhishek       Modified : Modified to 3 Tier Standard 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class ProductsAvailabilityDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Dictionary<ProductsAvailabilityDTO.SearchParameters, string> DBSearchParameters = new Dictionary<ProductsAvailabilityDTO.SearchParameters, string>
        {
            {ProductsAvailabilityDTO.SearchParameters.ID, "PAS.Id"},
            {ProductsAvailabilityDTO.SearchParameters.PRODUCT_ID, "PAS.ProductId"},
            {ProductsAvailabilityDTO.SearchParameters.IS_AVAILABLE, "PAS.IsAvailable"},
            {ProductsAvailabilityDTO.SearchParameters.UNAVAILABLE_TILL, "PAS.UnavailableTill"},
             {ProductsAvailabilityDTO.SearchParameters.IS_ACTIVE, "PAS.IsActive"},
            {ProductsAvailabilityDTO.SearchParameters.SITE_ID, "PAS.site_id"},
            {ProductsAvailabilityDTO.SearchParameters.MASTER_ENTITY_ID, "PAS.MasterEntityId"}
        };

        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of ProductDataHandler class
        /// </summary>
        public ProductsAvailabilityDataHandler(SqlTransaction sqlTransaction) 
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Product Availability Record.
        /// </summary>
        /// <param name="productAvailabilityDTO">DTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductsAvailabilityDTO pproductAvlDTO, string approverId, string userId, int siteId)
        {
            log.LogMethodEntry(pproductAvlDTO, userId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", pproductAvlDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", pproductAvlDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isAvailable", pproductAvlDTO.IsAvailable));
            parameters.Add(dataAccessHandler.GetSQLParameter("@availableQuantity", pproductAvlDTO.AvailableQty));
            parameters.Add(dataAccessHandler.GetSQLParameter("@unavailableTill", pproductAvlDTO.UnavailableTill));
            parameters.Add(dataAccessHandler.GetSQLParameter("@approvedBy", approverId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@comments", pproductAvlDTO.Comments));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", pproductAvlDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", pproductAvlDTO.MasterEntityId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// ProductsAvailabilityDTO(int productId) method
        /// </summary>
        /// <param name="productId">productId</param>
        /// <returns>returns ProductsDTO object</returns>
        public ProductsAvailabilityDTO GetProductsAvailabilityDTO(DataRow productDataRow)
        {
            log.LogMethodEntry(productDataRow);
            ProductsAvailabilityDTO productsAvailabilityDTO = new ProductsAvailabilityDTO(
            productDataRow.Table.Columns.Contains("Id") ? productDataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["Id"].ToString()) : -1,
            productDataRow.Table.Columns.Contains("ProductId") ? productDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["ProductId"].ToString()) : -1,
            productDataRow.Table.Columns.Contains("IsAvailable") ? productDataRow["IsAvailable"] == DBNull.Value ? true : Convert.ToBoolean(productDataRow["IsAvailable"]) : true,
            productDataRow.Table.Columns.Contains("AvailableQuantity") ? productDataRow["AvailableQuantity"] == DBNull.Value ? int.MaxValue : Convert.ToDecimal(productDataRow["AvailableQuantity"].ToString()) : int.MaxValue,
            productDataRow.Table.Columns.Contains("InitialAvailableQuantity") ? productDataRow["InitialAvailableQuantity"] == DBNull.Value ? int.MaxValue : Convert.ToDecimal(productDataRow["InitialAvailableQuantity"].ToString()) : int.MaxValue,
            productDataRow.Table.Columns.Contains("UnavailableTill") ? productDataRow["UnavailableTill"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productDataRow["UnavailableTill"].ToString()) : DateTime.MinValue,
            productDataRow.Table.Columns.Contains("ApprovedBy") ? productDataRow["ApprovedBy"] == DBNull.Value ? string.Empty : productDataRow["ApprovedBy"].ToString() : string.Empty,
            productDataRow.Table.Columns.Contains("Comments") ? productDataRow["Comments"] == DBNull.Value ? string.Empty : productDataRow["Comments"].ToString() : string.Empty,
            productDataRow.Table.Columns.Contains("ProductName") ? productDataRow["ProductName"] == DBNull.Value ? string.Empty : productDataRow["ProductName"].ToString() : string.Empty,
            productDataRow.Table.Columns.Contains("IsActive") ? productDataRow["IsActive"] == DBNull.Value ? false: Convert.ToBoolean(productDataRow["IsActive"]): true,
            productDataRow.Table.Columns.Contains("CreatedBy") ? productDataRow["CreatedBy"] == DBNull.Value ? string.Empty :  productDataRow["CreatedBy"].ToString() : string.Empty,
            productDataRow.Table.Columns.Contains("CreationDate") ? productDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productDataRow["CreationDate"]) : DateTime.MinValue,
            productDataRow.Table.Columns.Contains("LastUpdatedBy") ? productDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : productDataRow["LastUpdatedBy"].ToString() : string.Empty,
            productDataRow.Table.Columns.Contains("LastupdatedDate") ? productDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productDataRow["LastupdatedDate"]) : DateTime.MinValue,
            productDataRow.Table.Columns.Contains("siteId") ? productDataRow["siteId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["siteId"]) : -1,
            productDataRow.Table.Columns.Contains("MasterEntityId") ? productDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(productDataRow["MasterEntityId"]): -1,
            productDataRow.Table.Columns.Contains("SynchStatus") ? productDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(productDataRow["SynchStatus"]) : true,
            productDataRow.Table.Columns.Contains("GUID") ? productDataRow["GUID"] == DBNull.Value ? string.Empty : productDataRow["GUID"].ToString() : string.Empty
            );
            log.LogMethodExit();
            return productsAvailabilityDTO;
        }

        /// <summary>
        /// Inserts the ProductsAvailabilityDTO record to the database
        /// </summary>
        /// <param name="productDTO">ProductsAvailabilityDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <returns>Returns ProductsAvailabilityDTO</returns>
        public ProductsAvailabilityDTO Insert(ProductsAvailabilityDTO productsAvailabilityDTO, string approverId, string loginId, int siteId)
        {
            log.LogMethodEntry(productsAvailabilityDTO, approverId, loginId, siteId);
            string query = @"INSERT INTO ProductsAvailabilityStatus 
                                        ( 
                                            [ProductId],
                                            [IsAvailable],
                                            [AvailableQuantity],
                                            [InitialAvailableQuantity],
                                            [UnavailableTill],
                                            [ApprovedBy],
                                            [Comments],
                                            [IsActive],
                                            [CreatedBy],
                                            [CreationDate],
                                            [LastUpdatedBy],
                                            [LastupdatedDate],
                                            [site_id],
                                            [MasterEntityId]

                                        ) 
                                VALUES 
                                        (   @productId,
                                            @isAvailable,
                                            @availableQuantity,
                                            @availableQuantity,
                                            @unavailableTill,
                                            @approvedBy,
                                            @comments,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @CreatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                        )SELECT * FROM ProductsAvailabilityStatus WHERE Id = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productsAvailabilityDTO, approverId, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductsAvailabilityDTO(productsAvailabilityDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productsAvailabilityDTO);
            return productsAvailabilityDTO;
        }

        /// <summary>
        /// Updates the ProductsAvailabilityDTO record
        /// </summary>
        /// <param name="productDTO">ContactDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <returns>Returns the updated ProductsAvailabilityDTO</returns>
        public ProductsAvailabilityDTO Update(ProductsAvailabilityDTO productsAvailabilityDTO, string approverId, string loginId, int siteId)
        {
            log.LogMethodEntry(productsAvailabilityDTO, approverId, loginId, siteId);
            string query = @"UPDATE ProductsAvailabilityStatus 
                             SET [IsAvailable] = @isAvailable,
                                 [AvailableQuantity] = @availableQuantity,
                                 [UnavailableTill] = @unavailableTill,
                                 [Comments] = @comments,
                                 [ApprovedBy] = @approvedBy,
                                 -- [site_id] = @site_id,
                                 [LastUpdatedBy] = @LastUpdatedBy,
                                 [LastupdatedDate] = GETDATE(),
                                 [MasterEntityId] = @MasterEntityId
                             WHERE Id = @Id
                             SELECT * FROM ProductsAvailabilityStatus WHERE Id = @Id";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productsAvailabilityDTO, approverId, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductsAvailabilityDTO(productsAvailabilityDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productsAvailabilityDTO);
            return productsAvailabilityDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="productsAvailabilityDTO">ProductsAvailabilityDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshProductsAvailabilityDTO(ProductsAvailabilityDTO productsAvailabilityDTO, DataTable dt)
        {
            log.LogMethodEntry(productsAvailabilityDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                productsAvailabilityDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                productsAvailabilityDTO.UnavailableTill = dataRow["UnavailableTill"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["UnavailableTill"]);   
                productsAvailabilityDTO.LastUpdateDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                productsAvailabilityDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productsAvailabilityDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                productsAvailabilityDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productsAvailabilityDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                productsAvailabilityDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Products Availability data of passed Products Availability Id
        /// </summary>
        /// <param name="Id">integer type parameter</param>
        /// <returns>Returns ProductsAvailabilityDTO</returns>
        public ProductsAvailabilityDTO GetProductsAvailabilityDTO(int id)
        {
            log.LogMethodEntry(id);
            ProductsAvailabilityDTO productsAvailabilityDTO = null;
            string query = @"SELECT * FROM ProductsAvailabilityStatus WHERE Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                productsAvailabilityDTO = GetProductsAvailabilityDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(productsAvailabilityDTO);
            return productsAvailabilityDTO;
        }

        /// <summary>
        /// Gets the ProductsAvailabilityDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductsAvailabilityDTO matching the search criteria</returns>
        public List<ProductsAvailabilityDTO> GetUnavailableProductsList(List<KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<ProductsAvailabilityDTO> productsAvailabilityDTOList = null;
            List <SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @"SELECT PAS.*, p.product_name ProductName FROM ProductsAvailabilityStatus PAS left outer join Products P on P.product_id = PAS.ProductId ";

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = " ";
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? " " : " AND ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ProductsAvailabilityDTO.SearchParameters.ID ||
                            searchParameter.Key == ProductsAvailabilityDTO.SearchParameters.PRODUCT_ID ||
                            searchParameter.Key == ProductsAvailabilityDTO.SearchParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductsAvailabilityDTO.SearchParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductsAvailabilityDTO.SearchParameters.IS_AVAILABLE ||
                                 searchParameter.Key == ProductsAvailabilityDTO.SearchParameters.IS_ACTIVE)
                        {
                            //query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " ) = (" + (searchParameter.Value == "true" ? 1:0) + ")");
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == ProductsAvailabilityDTO.SearchParameters.UNAVAILABLE_TILL)
                        {
                            //query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " ) > (" + searchParameter.Value + ")");
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        //count++;
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
                selectQuery = selectQuery + query.ToString() + " ORDER by PAS.ProductId";
            }

            DataTable pasDTOsData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            productsAvailabilityDTOList = new List<ProductsAvailabilityDTO>();
            if (pasDTOsData.Rows.Count > 0)
            {
                foreach (DataRow dataRow in pasDTOsData.Rows)
                {
                    ProductsAvailabilityDTO productsAvailabilityDTO = GetProductsAvailabilityDTO(dataRow);
                    productsAvailabilityDTOList.Add(productsAvailabilityDTO);
                }
                //log.LogMethodExit(productsAvailabilityDTOList);
            }
            log.LogMethodExit(productsAvailabilityDTOList);
            return productsAvailabilityDTOList;
        }

        public int UpdatedExpiredProductsToAvailable()
        {
            log.LogMethodEntry();
            int rowsUpdated = 0;

            string UpdateQuery = @"UPDATE ProductsAvailabilityStatus 
                                   SET IsAvailable = 1
                                   WHERE IsAvailable = 0 and UnavailableTill <= GETDATE()";

            List<SqlParameter> updateQuantityParameters = new List<SqlParameter>();
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(UpdateQuery, updateQuantityParameters.ToArray(), sqlTransaction);
                log.LogVariableState("rowsUpdated", rowsUpdated);
            }
            catch (Exception e1)
            {
                log.Error("Unable to execute Update Available Quantity Query " + UpdateQuery + String.Join("-", updateQuantityParameters), e1);
                throw;
            }
            return rowsUpdated;
        }

        public int UpdateAvailableQuantityForCancelledTransaction(int TrxId, string userId, int siteId)
        {
            log.LogMethodEntry(TrxId);
            int rowsUpdated = 0;

            string UpdateQuery = @"UPDATE ProductsAvailabilityStatus 
                                   SET AvailableQuantity = AvailableQuantity + TRX.qty,
	                                   LastupdatedDate = getdate(), 
                                       LastUpdatedBy = @userId,
                                       site_id = @site_id
                                   FROM (SELECT SUM(quantity) qty, product_id from trx_lines where TrxId = @TrxId group by product_id) TRX
                                   WHERE ProductsAvailabilityStatus.ProductId = TRX.Product_id and IsAvailable = 0";

            List<SqlParameter> updateQuantityParameters = new List<SqlParameter>();
            try
            {
                updateQuantityParameters.Add(new SqlParameter("@TrxId", TrxId));
                updateQuantityParameters.Add(new SqlParameter("@userId", userId));
                updateQuantityParameters.Add(new SqlParameter("@site_id", siteId));
                rowsUpdated = dataAccessHandler.executeUpdateQuery(UpdateQuery, updateQuantityParameters.ToArray(), sqlTransaction);

                log.LogVariableState("rowsUpdated", rowsUpdated);
            }
            catch (Exception e1)
            {
                log.Error("Unable to execute Update Available Quantity Query " + UpdateQuery + String.Join("-",updateQuantityParameters), e1);
                throw;
            }
            return rowsUpdated;
        }
    }
}

/********************************************************************************************
 * Project Name - ProductModifiers Data Handler
 * Description  - Data handler of the ProductModifiers class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.40        17-Sep-2018      Indhu               Created 
 *2.70        11-Apr-2019      Akshay Gulaganji    modified UpdateProductModifiers() method 
 *2.70        07-Jul-2019      Mehraj              Added DeleteProductModifiers() method
 *2.70.2        10-Dec-2019     Jinto Thomas         Removed siteid from update query
 *2.110.00    01-Dec-2020      Abhishek            Modified : Modified to 3 Tier Standard 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Data;
using System.Globalization;

namespace Semnox.Parafait.Product
{
    public class ProductModifiersDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private Utilities utilities;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ProductModifiers AS pm ";

        private static readonly Dictionary<ProductModifiersDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProductModifiersDTO.SearchByParameters, string>
        {
                {ProductModifiersDTO.SearchByParameters.PRODUCT_MODIFIER_ID,"pm.Id"},
                {ProductModifiersDTO.SearchByParameters.ISACTIVE, "pm.IsActive"},
                {ProductModifiersDTO.SearchByParameters.SITE_ID, "pm.site_id"},
                {ProductModifiersDTO.SearchByParameters.PRODUCT_ID, "pm.ProductId"},
                {ProductModifiersDTO.SearchByParameters.MODIFIER_SET_ID, "pm.ModifierSetId"},
                {ProductModifiersDTO.SearchByParameters.PRODUCT_ID_LIST, "pm.ProductId"},
                {ProductModifiersDTO.SearchByParameters.MASTER_ENTITY_ID, "pm.MasterEntityId"},
                {ProductModifiersDTO.SearchByParameters.LAST_UPDATED_DATE, "pm.LastUpdateDate"}
        };

        /// <summary>
        /// Default constructor of ProductModifiersDataHandler class
        /// </summary>
        public ProductModifiersDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Product Modifiers Record.
        /// </summary>
        /// <param name="productModifiersDTO">ProductModifiersDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductModifiersDTO productModifiersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productModifiersDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", productModifiersDTO.ProductModifierId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@categoryId", productModifiersDTO.CategoryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", productModifiersDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@modifierSetId", productModifiersDTO.ModifierSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sortOrder", productModifiersDTO.SortOrder == -1 ? DBNull.Value : (object)productModifiersDTO.SortOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@autoShowInPos", productModifiersDTO.AutoShowinPOS));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", productModifiersDTO.IsActive == true ? "Y" : "N"));            
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", productModifiersDTO.MasterEntityId, true));     
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the  record to the database
        /// </summary>
        public ProductModifiersDTO Insert(ProductModifiersDTO productModifiersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productModifiersDTO, loginId, siteId);
            string InsertProductModifiersQuery = @"insert into ProductModifiers
                                                       (  
                                                          [CategoryId],
                                                          [ProductId],
                                                          [ModifierSetId],
                                                          [AutoShowInPos],
                                                          [SortOrder],
                                                          [IsActive],
                                                          [CreatedBy],
                                                          [CreationDate],
                                                          [LastUpdatedBy],
                                                          [LastUpdateDate],
                                                          [GUID],
                                                          [site_id], 
                                                          [MasterEntityId]
                                                       )
                                                       values 
                                                        ( @categoryId,
                                                          @productId,
                                                          @modifierSetId,
                                                          @autoShowInPos,
                                                          @sortOrder,
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(),
                                                          @lastUpdatedBy,
                                                          Getdate(),
                                                          NewId(),
                                                          @siteId,
                                                          @masterEntityId
                                                        )SELECT * FROM ProductModifiers WHERE Id = scope_identity()";

          
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(InsertProductModifiersQuery, GetSQLParameters(productModifiersDTO, loginId, siteId).ToArray().ToArray(), sqlTransaction);
                RefreshProductModifiersDTO(productModifiersDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productModifiersDTO);
            return productModifiersDTO;

        }

        public ProductModifiersDTO Update(ProductModifiersDTO productModifiersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productModifiersDTO, loginId, siteId);
            string updateProductModifiersQuery = @"update ProductModifiers 
                                                          set [CategoryId] = @categoryId,
                                                          [ProductId] = @productId,
                                                          [ModifierSetId] = @modifierSetId,
                                                          [AutoShowInPos] = @autoShowInPos,
                                                          [SortOrder] = @sortOrder,
                                                          [IsActive] = @isActive,
                                                          [LastUpdatedBy] = @lastUpdatedBy, 
                                                          [LastUpdateDate] = Getdate(),
                                                          -- [site_id] = @siteId,
                                                          [MasterEntityId] =  @masterEntityId
                                                          where  Id = @id
                                                          SELECT * FROM ProductModifiers WHERE Id = @id";

           
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateProductModifiersQuery, GetSQLParameters(productModifiersDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductModifiersDTO(productModifiersDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productModifiersDTO);
            return productModifiersDTO;
        }

        /// <summary>
        ///  Converts the Data row object to ProductModifiersDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private ProductModifiersDTO GetProductModifierDTO(DataRow dataRow)
        {
            log.LogMethodEntry();
            ProductModifiersDTO productModifiersDTO = new ProductModifiersDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["CategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CategoryId"]),
                                            dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                            dataRow["ModifierSetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ModifierSetId"]),
                                            dataRow["AutoShowInPos"] == DBNull.Value ? string.Empty : dataRow["AutoShowInPos"].ToString(),
                                            dataRow["SortOrder"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SortOrder"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : dataRow["IsActive"].ToString() == "Y",
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                            dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(productModifiersDTO);
            return productModifiersDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="productModifiersDTO">ProductModifiersDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshProductModifiersDTO(ProductModifiersDTO productModifiersDTO, DataTable dt)
        {
            log.LogMethodEntry(productModifiersDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                productModifiersDTO.ProductModifierId = Convert.ToInt32(dt.Rows[0]["Id"]);
                productModifiersDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                productModifiersDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productModifiersDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                productModifiersDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productModifiersDTO.GUID = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                productModifiersDTO.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the ProductModifiers data of passed productModifierId
        /// </summary>
        /// <param name="productModifierId">integer type parameter</param>
        /// <returns>Returns ProductModifiersDTO</returns>
        public ProductModifiersDTO GetProductModifier(int productModifierId)
        {
            log.LogMethodEntry(productModifierId);
            ProductModifiersDTO productModifiersDTO = null;
            string selectProductModifiersQuery = SELECT_QUERY + @" WHERE pm.Id = @id";
            SqlParameter parameter = new SqlParameter("@id", productModifierId);
            DataTable productModifiers = dataAccessHandler.executeSelectQuery(selectProductModifiersQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (productModifiers.Rows.Count > 0)
            {
                productModifiersDTO = GetProductModifierDTO(productModifiers.Rows[0]);               
            }
            log.LogMethodExit(productModifiersDTO);
            return productModifiersDTO;
        }


        /// <summary>
        /// Hard deletion of a product in ProductModifiers
        /// </summary>
        /// <param name="productModifierId"></param>
        /// <returns></returns>
        public void Delete(int productModifierId)
        {
            log.LogMethodEntry(productModifierId);
            try
            {
                string deleteQuery = @"delete from ProductModifiers where Id = @id";
                SqlParameter parameter = new SqlParameter("@id", productModifierId);
                dataAccessHandler.executeUpdateQuery(deleteQuery, new SqlParameter[] { parameter }, sqlTransaction);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception" + ex.Message);
                throw;
            }
        }

        internal List<ProductModifiersDTO> GetProductModifiersDTOList(List<int> productIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(productIdList);
            List<ProductModifiersDTO> productIdListDetailsDTOList = new List<ProductModifiersDTO>();
            string query = @"SELECT *
                            FROM ProductModifiers, @productIdList List
                            WHERE ProductId = List.Id ";
            if (activeRecords)
            {
                query += " AND (IsActive = 'Y' or IsActive is null)";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@productIdList", productIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productIdListDetailsDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductModifierDTO(x)).ToList();
            }
            log.LogMethodExit(productIdListDetailsDTOList);
            return productIdListDetailsDTOList;
        }

        /// <summary>
        /// Gets the ProductModifiersDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of productModifiersDTO matching the search criteria</returns>
        public List<ProductModifiersDTO> GetProductModifiers(List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int count = 0;
            List<ProductModifiersDTO> productModifiersDTOList = null;
            List <SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY ;
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductModifiersDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        {
                            if (searchParameter.Key.Equals(ProductModifiersDTO.SearchByParameters.PRODUCT_MODIFIER_ID) ||
                                searchParameter.Key.Equals(ProductModifiersDTO.SearchByParameters.PRODUCT_ID) ||
                                searchParameter.Key.Equals(ProductModifiersDTO.SearchByParameters.MODIFIER_SET_ID) ||
                                searchParameter.Key.Equals(ProductModifiersDTO.SearchByParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == ProductModifiersDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == ProductModifiersDTO.SearchByParameters.ISACTIVE)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                            }
                            else if (searchParameter.Key.Equals(ProductModifiersDTO.SearchByParameters.PRODUCT_ID_LIST))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                            }
                            else if (searchParameter.Key.Equals(ProductModifiersDTO.SearchByParameters.LAST_UPDATED_DATE))
                            {
                                query.Append(joiner + " ISNULL(" + DBSearchParameters[searchParameter.Key] + ", GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            }
                            else
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
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
                    selectQuery = selectQuery + query + " order by SortOrder";
            }
            DataTable productModifiersData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (productModifiersData.Rows.Count > 0)
            {
                productModifiersDTOList = new List<ProductModifiersDTO>();
                foreach (DataRow productModifiersDataRow in productModifiersData.Rows)
                {
                    ProductModifiersDTO productModifiersDTO = GetProductModifierDTO(productModifiersDataRow);
                    productModifiersDTOList.Add(productModifiersDTO);
                }                
            }
            log.LogMethodExit(productModifiersDTOList);
            return productModifiersDTOList;
        }
    }
}

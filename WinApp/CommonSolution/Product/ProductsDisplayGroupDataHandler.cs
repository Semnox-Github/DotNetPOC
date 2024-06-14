/********************************************************************************************
 * Project Name -ProductsDisplayGroup DataHandler
 * Description  -Data object of ProductsDisplayGroup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        21-Nov-2016   Amaresh           Created 
 *2.60        15-Mar-2019   Nitin Pai         Added new search for Display group id list 
 *            25-Mar-2019   Akshay Gulaganji  Added new search isActive in DBSearchParameters, InsertProductsDisplayGroup(), 
 *                                            UpdateProductsDisplayGroup(), GetProductsDisplayGroupDTO() and GetProductsDisplayGroupList() method
 *            11-Apr-2019   Archana           SqlTransaction parameter is added to Insert and update methods
 *            19-Apr-2019   Lakshminarayana   Updated to handle batch save
 *2.60.2      29-May-2019   Jagan Mohan       Code merge from Development to WebManagementStudio
 *2.80        25-Jun-2020   Deeksha           Added displayGroup Name
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Product
{
    /// <summary>
    /// ProductsDisplayGroupDataHandler - Handles insert, update and select of ProductsDisplayGroup objects
    /// </summary>
    public class ProductsDisplayGroupDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string> DBSearchParameters = new Dictionary<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>
            {
                {ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.ID, "pd.Id"},
                {ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.PRODUCT_ID, "pd.ProductId"},
                {ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID, "pd.DisplayGroupId"},
                {ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.SITE_ID, "pd.site_id"},
                {ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID_LIST, "pd.DisplayGroupId"},
                {ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.IS_ACTIVE, "pd.IsActive"},
                {ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.LAST_UPDATED_DATE, "pd.LastUpdatedDate"}
            };

        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;

        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS ProductsDisplayGroupType;
                                            MERGE INTO ProductsDisplayGroup tbl
                                            USING @ProductsDisplayGroupList AS src
                                            ON src.Id = tbl.Id
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            ProductId = src.ProductId,
                                            DisplayGroupId = src.DisplayGroupId,
                                            LastUpdatedBy = src.LastUpdatedBy,
                                            LastUpdatedDate = GETDATE(),
                                            Guid = src.Guid,
                                            site_id = src.site_id,
                                            MasterEntityId = src.MasterEntityId,
                                            IsActive = src.IsActive
                                            WHEN NOT MATCHED THEN INSERT (
                                            ProductId,
                                            DisplayGroupId,
                                            CreatedBy,
                                            CreatedDate,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            Guid,
                                            site_id,
                                            MasterEntityId,
                                            IsActive
                                            )
                                            VALUES (
                                            src.ProductId,
                                            src.DisplayGroupId,
                                            src.CreatedBy,
                                            GETDATE(),
                                            src.LastUpdatedBy,
                                            GETDATE(),
                                            src.Guid,
                                            src.site_id,
                                            src.MasterEntityId,
                                            src.IsActive
                                            )
                                            OUTPUT
                                            inserted.Id,
                                            inserted.CreatedBy,
                                            inserted.CreatedDate,
                                            inserted.LastUpdatedBy,
                                            inserted.LastUpdatedDate,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            Id,
                                            CreatedBy, 
                                            CreatedDate, 
                                            LastUpdatedBy, 
                                            LastUpdatedDate, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion
        /// <summary>
        /// Default constructor of ProductsDisplayGroupDataHandler class
        /// </summary>
        public ProductsDisplayGroupDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Inserts the ProductsDisplayGroup record to the database
        /// </summary>
        /// <param name="productsDisplayGroupDTO">ProductsDisplayGroupDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(ProductsDisplayGroupDTO productsDisplayGroupDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productsDisplayGroupDTO, userId, siteId);
            Save(new List<ProductsDisplayGroupDTO>() { productsDisplayGroupDTO }, userId, siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the ProductsDisplayGroup record to the database
        /// </summary>
        /// <param name="productsDisplayGroupDTOList">List of ProductsDisplayGroupDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(productsDisplayGroupDTOList, userId, siteId);
            Dictionary<string, ProductsDisplayGroupDTO> productsDisplayGroupDTOGuidMap = GetProductsDisplayGroupDTOGuidMap(productsDisplayGroupDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(productsDisplayGroupDTOList, userId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "ProductsDisplayGroupType",
                                                                "@ProductsDisplayGroupList");
            UpdateProductsDisplayGroupDTOList(productsDisplayGroupDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private void UpdateProductsDisplayGroupDTOList(Dictionary<string, ProductsDisplayGroupDTO> productsDisplayGroupDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                ProductsDisplayGroupDTO productsDisplayGroupDTO = productsDisplayGroupDTOGuidMap[Convert.ToString(row["Guid"])];
                productsDisplayGroupDTO.Id = row["Id"] == DBNull.Value ? -1 : Convert.ToInt32(row["Id"]);
                productsDisplayGroupDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                productsDisplayGroupDTO.CreatedDate = row["CreatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreatedDate"]);
                productsDisplayGroupDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                productsDisplayGroupDTO.LastUpdatedDate = row["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdatedDate"]);
                productsDisplayGroupDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                productsDisplayGroupDTO.AcceptChanges();
            }
        }

        private Dictionary<string, ProductsDisplayGroupDTO> GetProductsDisplayGroupDTOGuidMap(List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList)
        {
            Dictionary<string, ProductsDisplayGroupDTO> result = new Dictionary<string, ProductsDisplayGroupDTO>();
            for (int i = 0; i < productsDisplayGroupDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(productsDisplayGroupDTOList[i].Guid))
                {
                    productsDisplayGroupDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(productsDisplayGroupDTOList[i].Guid, productsDisplayGroupDTOList[i]);
            }
            return result;
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(productsDisplayGroupDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[12];
            int column = 0;
            columnStructures[column++] = new SqlMetaData("Id", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("ProductId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("DisplayGroupId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[column++] = new SqlMetaData("CreatedDate", SqlDbType.DateTime);
            columnStructures[column++] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 50);
            columnStructures[column++] = new SqlMetaData("LastUpdatedDate", SqlDbType.DateTime);
            columnStructures[column++] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[column++] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("SynchStatus", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[column++] = new SqlMetaData("IsActive", SqlDbType.Bit);
            for (int i = 0; i < productsDisplayGroupDTOList.Count; i++)
            {
                column = 0;
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(productsDisplayGroupDTOList[i].Id, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(productsDisplayGroupDTOList[i].ProductId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(productsDisplayGroupDTOList[i].DisplayGroupId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(productsDisplayGroupDTOList[i].CreatedDate));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(productsDisplayGroupDTOList[i].LastUpdatedDate));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(Guid.Parse(productsDisplayGroupDTOList[i].Guid)));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(productsDisplayGroupDTOList[i].SynchStatus ? 1 : 0));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(productsDisplayGroupDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(column++, dataAccessHandler.GetParameterValue(productsDisplayGroupDTOList[i].IsActive ? true : false));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Converts the Data row object to ProductsDisplayGroupDTO class type
        /// </summary>
        /// <param name="productsDisplayGroupDataRow">ProductsDisplayGroup DataRow</param>
        /// <returns>Returns ProductsDisplayGroup</returns>
        private ProductsDisplayGroupDTO GetProductsDisplayGroupDTO(DataRow productsDisplayGroupDataRow)
        {
            log.LogMethodEntry(productsDisplayGroupDataRow);
            ProductsDisplayGroupDTO productsDisplayGroupDataObject = new ProductsDisplayGroupDTO(
                                            productsDisplayGroupDataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(productsDisplayGroupDataRow["Id"]),
                                            productsDisplayGroupDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(productsDisplayGroupDataRow["ProductId"]),
                                            productsDisplayGroupDataRow["DisplayGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(productsDisplayGroupDataRow["DisplayGroupId"]),
                                            productsDisplayGroupDataRow["CreatedBy"].ToString(),
                                            productsDisplayGroupDataRow["CreatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productsDisplayGroupDataRow["CreatedDate"]),
                                            productsDisplayGroupDataRow["LastUpdatedBy"].ToString(),
                                            productsDisplayGroupDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productsDisplayGroupDataRow["LastUpdatedDate"]),
                                            productsDisplayGroupDataRow["Guid"].ToString(),
                                            productsDisplayGroupDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(productsDisplayGroupDataRow["site_id"]),
                                            productsDisplayGroupDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(productsDisplayGroupDataRow["SynchStatus"]),
                                            productsDisplayGroupDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(productsDisplayGroupDataRow["MasterEntityId"]),
                                            productsDisplayGroupDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(productsDisplayGroupDataRow["IsActive"]),
                                            productsDisplayGroupDataRow["displayGroup"].ToString()
                                            );
            log.LogMethodExit(productsDisplayGroupDataObject);
            return productsDisplayGroupDataObject;
        }

        /// <summary>
        /// Gets the ProductsDisplayGroup data of passed Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns ProductsDisplayGroupDTO</returns>
        public ProductsDisplayGroupDTO GetProductsDisplayGroup(int id)
        {
            log.LogMethodEntry(id);
            ProductsDisplayGroupDTO result = null;
            string selectQuery = @"SELECT pd.*, pdgf.DisplayGroup as displaygroup
                                    FROM ProductsDisplayGroup pd 
									    left outer join ProductDisplayGroupFormat pdgf on pd.DisplayGroupId = pdgf.id
                                    WHERE pd.Id = @id";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@id", id);
            DataTable table = dataAccessHandler.executeSelectQuery(selectQuery, parameters, sqlTransaction);
            if (table != null && table.Rows.Count > 0)
            {
                var list = table.Rows.Cast<DataRow>().Select(x => GetProductsDisplayGroupDTO(x));
                if (list != null)
                {
                    result = list.FirstOrDefault();
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ProductsDisplayGroup data of passed Id
        /// </summary>
        /// <param name="displayGroupId"></param>
        /// <returns>Returns ProductsDisplayGroupDTO</returns>
        public ProductsDisplayGroupDTO GetProductsDisplayGroupByDisplayGroupId(int displayGroupId, bool loadChildRecord)
        {
            log.LogMethodEntry(displayGroupId, loadChildRecord);
            ProductsDisplayGroupDTO result = null;
            string selectQuery = @"SELECT pd.*, pdgf.DisplayGroup as displaygroup
                                    FROM ProductsDisplayGroup pd 
									    left outer join ProductDisplayGroupFormat pdgf on pd.DisplayGroupId = pdgf.id
                                    WHERE pd.DisplayGroupId = @id";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@id", displayGroupId);
            DataTable table = dataAccessHandler.executeSelectQuery(selectQuery, parameters, sqlTransaction);
            if (table != null && table.Rows.Count > 0)
            {
                var list = table.Rows.Cast<DataRow>().Select(x => GetProductsDisplayGroupDTO(x));
                if (list != null)
                {
                    result = list.FirstOrDefault();
                    if (result != null && loadChildRecord)
                    {
                        Products products = new Products();
                        result.ProductsDTOList = products.GetProductDTOByDisplayGroupId(result.DisplayGroupId);
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Deete the ProductsDisplayGroup data of passed Id
        /// </summary>
        /// <param name="id"></param>
        public int DeleteProductsDisplayGroup(int id)
        {
            log.LogMethodEntry(id);
            try
            {
                string deleteProductsDisplayGroupQuery = @"DELETE 
                                                            FROM ProductsDisplayGroup
                                                            WHERE Id = @id";
                SqlParameter[] deleteProductsDisplayGrouparameters = new SqlParameter[1];
                deleteProductsDisplayGrouparameters[0] = new SqlParameter("@id", id);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteProductsDisplayGroupQuery, deleteProductsDisplayGrouparameters);

                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                log.Error(expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the ProductsDisplayGroupDTO List for ProductsDisplayGroupSet Id List
        /// </summary>
        /// <param name="productIdList">integer list parameter</param>
        /// <returns>Returns List of ProductsDisplayGroupDTO</returns>
        public List<ProductsDisplayGroupDTO> GetProductsDisplayGroupDTOListOfProducts(List<int> productIdList, bool activeChildRecords)
        {
            log.LogMethodEntry(productIdList, activeChildRecords);
            List<ProductsDisplayGroupDTO> list = new List<ProductsDisplayGroupDTO>();
            string query = @"SELECT pd.*, pdgf.DisplayGroup as displaygroup
							FROM ProductsDisplayGroup pd
							INNER JOIN @ProductIdList List on pd.ProductId = List.Id 
                            left outer join ProductDisplayGroupFormat pdgf on pd.DisplayGroupId = pdgf.Id";

            DataTable table = dataAccessHandler.BatchSelect(query, "@ProductIdList", productIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetProductsDisplayGroupDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the ProductsDisplayGroupDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductsDisplayGroupDTO matching the search criteria</returns>
        public List<ProductsDisplayGroupDTO> GetProductsDisplayGroupList(List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetProductsDisplayGroupList(searchParameters) Method.");
            int count = 0;
            string selectProductsDisplayGroupQuery = @"SELECT pd.*, pdf.DisplayGroup 
                                                            FROM ProductsDisplayGroup pd
                                                            LEFT outer JOIN ProductDisplayGroupFormat pdf ON pd.DisplayGroupId = pdf.Id";

            if (searchParameters != null)
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";

                        if (searchParameter.Key == ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.ID ||
                            searchParameter.Key == ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID ||
                            searchParameter.Key == ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.PRODUCT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID_LIST)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + ") IN (" + searchParameter.Value + ")");
                        }
                        else if (searchParameter.Key == ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.IS_ACTIVE)//IsActive is Added On 25-Mar-2019, if the earlier data fields of isActive is having Null then it will consider as true
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') like " + "'%" + searchParameter.Value + "%'");
                        }
                        else if (searchParameter.Key.Equals(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.LAST_UPDATED_DATE))
                        {
                            query.Append(joiner + " IsNull ( " + DBSearchParameters[searchParameter.Key] + ", GetDate()) >= '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }

                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetProductsDisplayGroupList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectProductsDisplayGroupQuery = selectProductsDisplayGroupQuery + query;
            }
            List<ProductsDisplayGroupDTO> list = null;
            DataTable table = dataAccessHandler.executeSelectQuery(selectProductsDisplayGroupQuery, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetProductsDisplayGroupDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

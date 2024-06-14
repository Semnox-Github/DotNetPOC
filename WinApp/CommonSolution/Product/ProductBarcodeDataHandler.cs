/********************************************************************************************
 * Project Name - Product Barcode BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *1.00        30-Aug-2017   Indhu           Created 
 *2.60.0      02-Apr-2019   Lakshminarayana Changed to use batch insert and update
 *2.60.3      19-Jun-2019   Nagesh Badiger  Modified isActive property string to bool and added isActive search param
 *2.110.00    30-Nov-2020   Abhishek        Modified : Modified to 3 Tier Standard 
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Product
{
    /// <summary>
    ///  ProductBarcode Data Handler - Handles insert, update and select of  ProductBarcode objects
    /// </summary>
    public class ProductBarcodeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ProductBarcode AS pb ";

        private static readonly Dictionary<ProductBarcodeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProductBarcodeDTO.SearchByParameters, string>
            {
                {ProductBarcodeDTO.SearchByParameters.ID, "pb.ID"},
                {ProductBarcodeDTO.SearchByParameters.PRODUCT_ID, "pb.ProductId"},
                {ProductBarcodeDTO.SearchByParameters.IS_ACTIVE, "pb.IsActive"},
                {ProductBarcodeDTO.SearchByParameters.SITE_ID, "pb.Site_id"},
                {ProductBarcodeDTO.SearchByParameters.MASTER_ENTITY_ID,"pb.MasterEntityId"},
                {ProductBarcodeDTO.SearchByParameters.BARCODE,"pb.BarCode"}
            };

        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS ProductBarcodeType;
                                            MERGE INTO ProductBarcode tbl
                                            USING @ProductBarcodeList AS src
                                            ON src.ID = tbl.ID
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            BarCode = src.BarCode,
                                            ProductId = src.ProductId,
                                            isActive = src.isActive,
                                            Lastupdated_userid = src.Lastupdated_userid,
                                            LastUpdatedDate = GETDATE(),
                                            MasterEntityId = src.MasterEntityId,
                                            site_id = src.site_id
                                            WHEN NOT MATCHED THEN INSERT (
                                            BarCode,
                                            ProductId,
                                            isActive,
                                            Lastupdated_userid,
                                            LastUpdatedDate,
                                            site_id,
                                            Guid,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate
                                            )VALUES (
                                            src.BarCode,
                                            src.ProductId,
                                            src.isActive,
                                            src.Lastupdated_userid,
                                            GETDATE(),
                                            src.site_id,
                                            src.Guid,
                                            src.MasterEntityId,
                                            src.CreatedBy,
                                            GETDATE()
                                            )
                                            OUTPUT
                                            inserted.ID,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdatedDate,
                                            inserted.Lastupdated_userid,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            ID,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdatedDate, 
                                            Lastupdated_userid, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion
        /// <summary>
        /// Default constructor of ProductBarcodeDataHandler class
        /// </summary>
        public ProductBarcodeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the ProductBarcode record to the database
        /// </summary>
        /// <param name="productBarcodeDTO">ProductBarcodeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(ProductBarcodeDTO productBarcodeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productBarcodeDTO, loginId, siteId);
            Save(new List<ProductBarcodeDTO>() { productBarcodeDTO }, loginId, siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the ProductBarcode record to the database
        /// </summary>
        /// <param name="productBarcodeDTOList">List of ProductBarcodeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<ProductBarcodeDTO> productBarcodeDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(productBarcodeDTOList, loginId, siteId);
            Dictionary<string, ProductBarcodeDTO> productBarcodeDTOGuidMap = GetProductBarcodeDTOGuidMap(productBarcodeDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(productBarcodeDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "ProductBarcodeType",
                                                                "@ProductBarcodeList");
            Update(productBarcodeDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<ProductBarcodeDTO> productBarcodeDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(productBarcodeDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[12];
            columnStructures[0] = new SqlMetaData("ID", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("BarCode", SqlDbType.NVarChar, 50);
            columnStructures[2] = new SqlMetaData("ProductId", SqlDbType.Int);
            columnStructures[3] = new SqlMetaData("isActive", SqlDbType.Char, 1);
            columnStructures[4] = new SqlMetaData("Lastupdated_userid", SqlDbType.NVarChar, 50);
            columnStructures[5] = new SqlMetaData("LastUpdatedDate", SqlDbType.DateTime);
            columnStructures[6] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[7] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[8] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[9] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[10] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[11] = new SqlMetaData("CreationDate", SqlDbType.DateTime);

            for (int i = 0; i < productBarcodeDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(productBarcodeDTOList[i].Id, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(productBarcodeDTOList[i].BarCode));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(productBarcodeDTOList[i].Product_Id, true));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(productBarcodeDTOList[i].IsActive ? "Y" : "N"));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(productBarcodeDTOList[i].LastUpdatedUserId));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(productBarcodeDTOList[i].LastUpdatedDate));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(Guid.Parse(productBarcodeDTOList[i].Guid)));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(productBarcodeDTOList[i].SynchStatus));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(productBarcodeDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(productBarcodeDTOList[i].CreationDate));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, ProductBarcodeDTO> GetProductBarcodeDTOGuidMap(List<ProductBarcodeDTO> productBarcodeDTOList)
        {
            Dictionary<string, ProductBarcodeDTO> result = new Dictionary<string, ProductBarcodeDTO>();
            for (int i = 0; i < productBarcodeDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(productBarcodeDTOList[i].Guid))
                {
                    productBarcodeDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(productBarcodeDTOList[i].Guid, productBarcodeDTOList[i]);
            }
            return result;
        }

        private void Update(Dictionary<string, ProductBarcodeDTO> productBarcodeDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                ProductBarcodeDTO productBarcodeDTO = productBarcodeDTOGuidMap[Convert.ToString(row["Guid"])];
                productBarcodeDTO.Id = row["ID"] == DBNull.Value ? -1 : Convert.ToInt32(row["ID"]);
                productBarcodeDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                productBarcodeDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                productBarcodeDTO.LastUpdatedUserId = row["Lastupdated_userid"] == DBNull.Value ? string.Empty : Convert.ToString(row["Lastupdated_userid"]);
                productBarcodeDTO.LastUpdatedDate = row["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdatedDate"]);
                productBarcodeDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                productBarcodeDTO.AcceptChanges();
            }
        }

        /// <summary>
        /// Converts the Data row object to SalesOfferGroupDTO class type
        /// </summary>
        /// <param name="productBarcodeDataRow">ProductBarcode DataRow</param>
        /// <returns>Returns ProductBarcode</returns>
        private ProductBarcodeDTO GetProductBarcodeDTO(System.Data.DataRow productBarcodeDataRow)
        {
            log.LogMethodEntry(productBarcodeDataRow);
            ProductBarcodeDTO productBarcodeDataObject = new ProductBarcodeDTO(Convert.ToInt32(productBarcodeDataRow["ID"]),
                                            productBarcodeDataRow["Barcode"] == DBNull.Value ? string.Empty : Convert.ToString(productBarcodeDataRow["Barcode"]),
                                             productBarcodeDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(productBarcodeDataRow["ProductId"]),
                                            productBarcodeDataRow["IsActive"] == DBNull.Value ? true : Convert.ToString(productBarcodeDataRow["IsActive"]) == "Y",
                                            productBarcodeDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productBarcodeDataRow["LastUpdatedDate"]),
                                            productBarcodeDataRow["Lastupdated_userid"].ToString(),
                                            productBarcodeDataRow["CreatedBy"].ToString(),
                                            productBarcodeDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productBarcodeDataRow["LastUpdatedDate"]),
                                            productBarcodeDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(productBarcodeDataRow["site_id"]),
                                            productBarcodeDataRow["Guid"].ToString(),
                                            productBarcodeDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(productBarcodeDataRow["SynchStatus"]),
                                            productBarcodeDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(productBarcodeDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(productBarcodeDataObject);
            return productBarcodeDataObject;
        }

        /// <summary>
        /// Gets the ProductBarcode data of passed ProductBarcode Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ProductBarcodeDTO</returns>
        public ProductBarcodeDTO GetProductBarcodeDTO(int id)
        {
            log.LogMethodEntry(id);
            ProductBarcodeDTO productBarcodeDTO = null;
            string query = SELECT_QUERY + @" WHERE pb.ID = @Id";
            DataTable table = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@Id", id, true) }, sqlTransaction);
            if (table != null && table.Rows.Count > 0)
            {
                var list = table.Rows.Cast<DataRow>().Select(x => GetProductBarcodeDTO(x));
                if (list != null)
                {
                    productBarcodeDTO = list.FirstOrDefault();
                }
            }
            log.LogMethodExit(productBarcodeDTO);
            return productBarcodeDTO;
        }

        /// <summary>
        /// Gets the ProductBarcodeDTO List for ProductBarcodeSet Id List
        /// </summary>
        /// <param name="productIdList">integer list parameter</param>
        /// <returns>Returns List of ProductBarcodeSetDTO</returns>
        public List<ProductBarcodeDTO> GetProductBarcodeDTOListOfProducts(List<int> productIdList, bool activeRecords)
        {
            log.LogMethodEntry(productIdList);
            List<ProductBarcodeDTO> list = new List<ProductBarcodeDTO>();
            string query = @"SELECT ProductBarcode.*
                            FROM ProductBarcode, @ProductIdList List
                            WHERE ProductId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = 'Y' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@ProductIdList", productIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetProductBarcodeDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the ProductBarcodeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductBarcodeDTO matching the search criteria</returns>
        public List<ProductBarcodeDTO> GetProductBarcodeDTOList(List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ProductBarcodeDTO> productBarcodeDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductBarcodeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ProductBarcodeDTO.SearchByParameters.ID ||
                            searchParameter.Key == ProductBarcodeDTO.SearchByParameters.PRODUCT_ID ||
                            searchParameter.Key == ProductBarcodeDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductBarcodeDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductBarcodeDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == ProductBarcodeDTO.SearchByParameters.BARCODE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable table = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productBarcodeDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductBarcodeDTO(x)).ToList();
            }
            log.LogMethodExit(productBarcodeDTOList);
            return productBarcodeDTOList;
        }

        /// <summary>
        /// Checks the Existance of Barcode with Product records
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="siteId"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public bool CheckExistanceForBarCodeWithProduct(int productId, int siteId, string barCode = null)
        {
            log.LogMethodEntry(productId, siteId, sqlTransaction);
            bool barCodeExist = false;
            string query = @"select * from product p, productbarcode b where p.productid = b.productid and p.isactive = 'Y' and b.isactive = 'Y' and b.barcode = @barcode and b.productid <> @productId and (b.site_id = @siteId or @siteId = -1)";
            SqlParameter[] selectScheduleExclusionParameters = new SqlParameter[3];
            selectScheduleExclusionParameters[0] = new SqlParameter("@siteId", siteId);
            selectScheduleExclusionParameters[1] = new SqlParameter("@productId", productId);
            selectScheduleExclusionParameters[2] = new SqlParameter("@barCode", barCode);
            DataTable dTable = dataAccessHandler.executeSelectQuery(query, selectScheduleExclusionParameters, sqlTransaction);
            if (dTable != null && dTable.Rows.Count > 0)
            {
                barCodeExist = true;
            }
            log.LogMethodExit(barCodeExist);
            return barCodeExist;
        }
    }
}

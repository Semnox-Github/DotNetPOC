/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu panel content Data handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.130.0        27-May-2021      Prajwal S       Created
 ********************************************************************************************/
using Microsoft.SqlServer.Server;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class ProductMenuPanelContentDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ProductMenuPanelContent AS pmc ";

        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS ProductMenuPanelContentType;
                                            MERGE INTO ProductMenuPanelContent tbl
                                            USING @ProductMenuPanelContentList AS src
                                            ON src.Id = tbl.Id
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            PanelId = src.PanelId,
                                            ObjectType = src.ObjectType,
                                            ObjectGuid = src.ObjectGuid,
                                            BackColor = src.BackColor,
                                            Font = src.Font,
                                            TextColor = src.TextColor,
                                            RowIndex = src.RowIndex,
                                            ButtonType = src.ButtonType,
                                            ImageURL = src.ImageURL,
                                            ColumnIndex = src.ColumnIndex,
                                            IsActive = src.IsActive,
                                            LastUpdatedBy = src.LastUpdatedBy,
                                            LastUpdatedDate = GETDATE(),
                                            MasterEntityId = src.MasterEntityId,
                                            site_id = src.site_id
                                            WHEN NOT MATCHED THEN INSERT (
                                            PanelId,
                                            ObjectType,
                                            ObjectGuid,
                                            BackColor,
                                            Font,
                                            TextColor,
                                            RowIndex,
                                            ButtonType,
                                            ImageURL,
                                            ColumnIndex,
                                            IsActive,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            site_id,
                                            Guid,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate
                                            )VALUES (
                                            src.PanelId,
                                            src.ObjectType,
                                            src.ObjectGuid,
                                            src.BackColor,
                                            src.Font,
                                            src.TextColor,
                                            src.RowIndex,
                                            src.ButtonType,
                                            src.ImageURL,
                                            src.ColumnIndex,
                                            src.IsActive,
                                            src.LastUpdatedBy,
                                            GETDATE(),
                                            src.site_id,
                                            src.Guid,
                                            src.MasterEntityId,
                                            src.CreatedBy,
                                            GETDATE()
                                            )
                                            OUTPUT
                                            inserted.Id,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdatedDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            Id,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdatedDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion

        /// <summary>
        /// Default constructor of ProductMenuPanelContentDataHandler class
        /// </summary>
        public ProductMenuPanelContentDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(ProductMenuPanelContentDTO productMenuPanelContentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPanelContentDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", productMenuPanelContentDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PanelId", productMenuPanelContentDTO.PanelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ObjectType", productMenuPanelContentDTO.ObjectType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ObjectGuid", productMenuPanelContentDTO.ObjectGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BackColor", productMenuPanelContentDTO.BackColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Font", productMenuPanelContentDTO.Font));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ImageURL", productMenuPanelContentDTO.ImageURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TextColor", productMenuPanelContentDTO.TextColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", productMenuPanelContentDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", productMenuPanelContentDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", productMenuPanelContentDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ProductMenuPanelContent record to the database
        /// </summary>
        /// <param name="productMenuPanelContentDTO">ProductMenuPanelContentDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(ProductMenuPanelContentDTO productMenuPanelContentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPanelContentDTO, loginId, siteId);
            Save(new List<ProductMenuPanelContentDTO>() { productMenuPanelContentDTO }, loginId, siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the ProductMenuPanelContent record to the database
        /// </summary>
        /// <param name="productMenuPanelContentDTOList">List of ProductMenuPanelContentDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPanelContentDTOList, loginId, siteId);
            Dictionary<string, ProductMenuPanelContentDTO> productMenuPanelContentDTOGuidMap = GetProductMenuPanelContentDTOGuidMap(productMenuPanelContentDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(productMenuPanelContentDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "ProductMenuPanelContentType",
                                                                "@ProductMenuPanelContentList");
            Update(productMenuPanelContentDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(productMenuPanelContentDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[20];
            int col = 0;
            columnStructures[col++] = new SqlMetaData("Id", SqlDbType.Int);
            columnStructures[col++] = new SqlMetaData("PanelId", SqlDbType.Int);
            columnStructures[col++] = new SqlMetaData("ObjectType", SqlDbType.NVarChar, 100);
            columnStructures[col++] = new SqlMetaData("ObjectGuid", SqlDbType.UniqueIdentifier);
            columnStructures[col++] = new SqlMetaData("ButtonType", SqlDbType.NVarChar, 1);
            columnStructures[col++] = new SqlMetaData("ImageURL", SqlDbType.NVarChar, 2000);
            columnStructures[col++] = new SqlMetaData("BackColor", SqlDbType.NVarChar, 100);
            columnStructures[col++] = new SqlMetaData("TextColor", SqlDbType.NVarChar, 100);
            columnStructures[col++] = new SqlMetaData("Font", SqlDbType.NVarChar, 100);
            columnStructures[col++] = new SqlMetaData("RowIndex", SqlDbType.Int);
            columnStructures[col++] = new SqlMetaData("ColumnIndex", SqlDbType.Int);
            columnStructures[col++] = new SqlMetaData("IsActive", SqlDbType.Bit);
            columnStructures[col++] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 50);
            columnStructures[col++] = new SqlMetaData("LastUpdatedDate", SqlDbType.DateTime);
            columnStructures[col++] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[col++] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[col++] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[col++] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[col++] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[col++] = new SqlMetaData("CreationDate", SqlDbType.DateTime);

            for (int i = 0; i < productMenuPanelContentDTOList.Count; i++)
            {
                col = 0;
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].Id));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].PanelId, true));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].ObjectType));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(string.IsNullOrWhiteSpace(productMenuPanelContentDTOList[i].ObjectGuid) == false? (object)Guid.Parse(productMenuPanelContentDTOList[i].ObjectGuid) : null));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].ButtonType));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].ImageURL));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].BackColor));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].TextColor));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].Font));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].RowIndex));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].ColumnIndex));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].IsActive));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].LastUpdatedBy));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].LastUpdatedDate));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(Guid.Parse(productMenuPanelContentDTOList[i].Guid)));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].SynchStatus));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(col++, dataAccessHandler.GetParameterValue(productMenuPanelContentDTOList[i].CreationDate));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, ProductMenuPanelContentDTO> GetProductMenuPanelContentDTOGuidMap(List<ProductMenuPanelContentDTO> ProductMenuPanelContentDTOList)
        {
            Dictionary<string, ProductMenuPanelContentDTO> result = new Dictionary<string, ProductMenuPanelContentDTO>();
            for (int i = 0; i < ProductMenuPanelContentDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(ProductMenuPanelContentDTOList[i].Guid))
                {
                    ProductMenuPanelContentDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(ProductMenuPanelContentDTOList[i].Guid, ProductMenuPanelContentDTOList[i]);
            }
            return result;
        }

        private void Update(Dictionary<string, ProductMenuPanelContentDTO> ProductMenuPanelContentDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                ProductMenuPanelContentDTO productMenuPanelContentDTO = ProductMenuPanelContentDTOGuidMap[Convert.ToString(row["Guid"])];
                productMenuPanelContentDTO.Id = row["Id"] == DBNull.Value ? -1 : Convert.ToInt32(row["Id"]);
                productMenuPanelContentDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                productMenuPanelContentDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                productMenuPanelContentDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                productMenuPanelContentDTO.LastUpdatedDate = row["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdatedDate"]);
                productMenuPanelContentDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                productMenuPanelContentDTO.AcceptChanges();
            }
        }

        /// <summary>
        /// Converts the Data row object to ProductMenuPanelContentDTO class type
        /// </summary>
        /// <param name="ProductMenuPanelContentDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns ProductMenuPanelContentFormat</returns>
        private ProductMenuPanelContentDTO GetProductMenuPanelContentDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProductMenuPanelContentDTO ProductMenuPanelContentDataObject = new ProductMenuPanelContentDTO(Convert.ToInt32(dataRow["Id"]),
                                                    dataRow["PanelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PanelId"]),
                                                    dataRow["ObjectType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ObjectType"]),
                                                    dataRow["ObjectGuid"] == DBNull.Value ? new Guid().ToString() : (dataRow["ObjectGuid"]).ToString(),
                                                    dataRow["ImageURL"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ImageURL"]),
                                                    dataRow["BackColor"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["BackColor"]),
                                                    dataRow["TextColor"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TextColor"]),
                                                    dataRow["Font"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Font"]),
                                                    dataRow["ColumnIndex"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ColumnIndex"]),
                                                    dataRow["RowIndex"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RowIndex"]),
                                                    dataRow["ButtonType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ButtonType"]),
                                                    dataRow["Guid"].ToString(),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["CreatedBy"].ToString(),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"].ToString(),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                                    );
            log.LogMethodExit();
            return ProductMenuPanelContentDataObject;
        }

        /// <summary>
        /// Gets the GetProductMenuPanelContent data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ProductMenuPanelContentDTO</returns>
        internal ProductMenuPanelContentDTO GetProductMenuPanelContent(int id)
        {
            log.LogMethodEntry(id);
            ProductMenuPanelContentDTO result = null;
            string query = SELECT_QUERY + @" WHERE pmc.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetProductMenuPanelContentDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<ProductMenuPanelContentDTO> GetProductMenuPanelContentDTOList(List<int> panelIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(panelIdList);
            List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList = new List<ProductMenuPanelContentDTO>();
            string query = @"SELECT *
                            FROM ProductMenuPanelContent, @panelIdList List
                            WHERE PanelId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@panelIdList", panelIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productMenuPanelContentDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductMenuPanelContentDTO(x)).ToList();
            }
            log.LogMethodExit(productMenuPanelContentDTOList);
            return productMenuPanelContentDTOList;
        }
    }
}
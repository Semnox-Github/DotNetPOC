/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu panel Data handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.130.0        27-May-2021      Prajwal S       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ProductMenuPanelDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ProductMenuPanel as pmp ";

        /// <summary>
        /// Dictionary for searching Parameters for the ProductMenuPanel object.
        /// </summary>
        private static readonly Dictionary<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string> DBSearchParameters = new Dictionary<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>
        {
            { ProductMenuPanelDTO.SearchByProductMenuPanelParameters.PANEL_ID,"pmp.PanelId"},
            { ProductMenuPanelDTO.SearchByProductMenuPanelParameters.PANEL_ID_LIST,"pmp.PanelId"},
            { ProductMenuPanelDTO.SearchByProductMenuPanelParameters.NAME,"pmp.Name"},
            { ProductMenuPanelDTO.SearchByProductMenuPanelParameters.GUID,"pmp.Guid"},
            { ProductMenuPanelDTO.SearchByProductMenuPanelParameters.SITE_ID,"pmp.site_id"},
            { ProductMenuPanelDTO.SearchByProductMenuPanelParameters.IS_ACTIVE,"pmp.IsActive"}
        };

        /// <summary>
        /// Parameterized Constructor for ProductMenuPanelDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public ProductMenuPanelDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProductMenuPanel Record.
        /// </summary>
        /// <param name="productMenuPanelDTO">ProductMenuPanelDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductMenuPanelDTO productMenuPanelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPanelDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PanelId", productMenuPanelDTO.PanelId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", productMenuPanelDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ColumnCount", productMenuPanelDTO.ColumnCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DisplayOrder", productMenuPanelDTO.DisplayOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CellMarginBottom", productMenuPanelDTO.CellMarginBottom));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CellMarginTop", productMenuPanelDTO.CellMarginTop));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CellMarginRight", productMenuPanelDTO.CellMarginRight));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CellMarginLeft", productMenuPanelDTO.CellMarginLeft));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RowCount", productMenuPanelDTO.RowCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ImageURL", productMenuPanelDTO.ImageURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", productMenuPanelDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", productMenuPanelDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to ProductMenuPanelDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of ProductMenuPanelDTO</returns>
        private ProductMenuPanelDTO GetProductMenuPanelDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProductMenuPanelDTO productMenuPanelDTO = new ProductMenuPanelDTO(dataRow["PanelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PanelId"]),
                                                dataRow["ColumnCount"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ColumnCount"]),
                                                dataRow["DisplayOrder"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DisplayOrder"]),
                                                dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                                dataRow["CellMarginLeft"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CellMarginLeft"]),
                                                dataRow["CellMarginRight"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CellMarginRight"]),
                                                dataRow["CellMarginTop"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CellMarginTop"]),
                                                dataRow["CellMarginBottom"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CellMarginBottom"]),
                                                dataRow["Row_Count"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Row_Count"]),
                                                dataRow["ImageURL"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ImageURL"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                                );
            return productMenuPanelDTO;
        }


        /// <summary>
        /// Gets the ProductMenuPanel data of passed ProductMenuPanelId 
        /// </summary>
        /// <param name="productMenuPanelId">productMenuPanelId is passed as parameter</param>
        /// <returns>Returns ProductMenuPanelDTO</returns>
        public ProductMenuPanelDTO GetProductMenuPanel(int panelId)
        {
            log.LogMethodEntry(panelId);
            ProductMenuPanelDTO result = null;
            string query = SELECT_QUERY + @" WHERE pmp.PanelId = @PanelId";
            SqlParameter parameter = new SqlParameter("@PanelId", panelId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetProductMenuPanelDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal bool GetIsRecordReferenced(int panelId)
        {
            log.LogMethodEntry(panelId);
            int referenceCount = 0;
            string query = @"SELECT 
                            (
                            SELECT COUNT(1) 
                            FROM ProductMenuPanelExclusion
                            WHERE PanelId = @PanelId
                            AND IsActive = 1
                            )
                            +
                            (
                            SELECT COUNT(1) 
                            FROM ProductMenuPanelMapping
                            WHERE PanelId = @PanelId
                            AND IsActive = 1
                            )
                            +   
                            (
                            SELECT COUNT(1) 
                            FROM ProductMenuPanelContent pmpc, ProductMenuPanel pmp
                            WHERE pmpc.ObjectGuid = pmp.Guid  
                            AND pmpc.ObjectType = 'PRODUCT_MENU_PANEL'
                            AND pmp.PanelId = @PanelId
                            ) AS ReferenceCount";
            SqlParameter parameter = new SqlParameter("@PanelId", panelId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                referenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            bool result = referenceCount > 0;
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="productMenuPanelDTO">ProductMenuPanelDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshProductMenuPanelDTO(ProductMenuPanelDTO productMenuPanelDTO, DataTable dt)
        {
            log.LogMethodEntry(productMenuPanelDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                productMenuPanelDTO.PanelId = Convert.ToInt32(dt.Rows[0]["PanelId"]);
                productMenuPanelDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                productMenuPanelDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productMenuPanelDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                productMenuPanelDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                productMenuPanelDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productMenuPanelDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the ProductMenuPanel Table. 
        /// </summary>
        /// <param name="productMenuPanelDTO">ProductMenuPanelDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated ProductMenuPanelDTO</returns>
        public ProductMenuPanelDTO Insert(ProductMenuPanelDTO productMenuPanelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPanelDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ProductMenuPanel]
                            (
                            ColumnCount,
                            DisplayOrder,
                            Name,
                            CellMarginLeft,
                            CellMarginRight,
                            CellMarginTop,
                            CellMarginBottom,
                            Row_Count,
                            ImageURL,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdatedDate , IsActive
                            )
                            VALUES
                            (
                            @ColumnCount,
                            @DisplayOrder,
                            @Name,
                            @CellMarginLeft,
                            @CellMarginRight,
                            @CellMarginTop,
                            @CellMarginBottom,
                            @RowCount,
                            @ImageURL,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(), @IsActive
                            )
                            SELECT * FROM ProductMenuPanel WHERE PanelId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productMenuPanelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductMenuPanelDTO(productMenuPanelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ProductMenuPanelDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productMenuPanelDTO);
            return productMenuPanelDTO;
        }

        /// <summary>
        /// Update the record in the ProductMenuPanel Table. 
        /// </summary>
        /// <param name="productMenuPanelDTO">ProductMenuPanelDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated ProductMenuPanelDTO</returns>
        public ProductMenuPanelDTO Update(ProductMenuPanelDTO productMenuPanelDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPanelDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ProductMenuPanel]
                             SET
                             ColumnCount = @ColumnCount,
                             DisplayOrder = @DisplayOrder,
                             Name = @Name,
                             CellMarginLeft = @CellMarginLeft,
                             CellMarginRight = @CellMarginRight,
                             CellMarginTop = @CellMarginTop,
                             CellMarginBottom = @CellMarginBottom,
                             Row_Count = @RowCount,
                             ImageURL = @ImageURL,
                             MasterEntityId = @MasterEntityId,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdatedDate = GETDATE(),
                             IsActive = @IsActive
                             WHERE PanelId = @PanelId
                            SELECT * FROM ProductMenuPanel WHERE PanelId = @PanelId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productMenuPanelDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductMenuPanelDTO(productMenuPanelDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating WorkShiftUserDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productMenuPanelDTO);
            return productMenuPanelDTO;
        }

        internal List<ProductMenuPanelDTO> GetProductMenuPanelDTOList(List<int> menuIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(menuIdList);
            List<ProductMenuPanelDTO> productMenuPanelDTOList = new List<ProductMenuPanelDTO>();
            string query = @"SELECT *
                            FROM ProductMenuPanel, @menuIdList List
                            WHERE MenuId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@menuIdList", menuIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productMenuPanelDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductMenuPanelDTO(x)).ToList();
            }
            log.LogMethodExit(productMenuPanelDTOList);
            return productMenuPanelDTOList;
        }

        /// <summary>
        /// Returns the List of ProductMenuPanelDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of ProductMenuPanelDTO </returns>
        public List<ProductMenuPanelDTO> GetProductMenuPanelDTOList(List<KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>> searchParameters,
                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ProductMenuPanelDTO> ProductMenuPanelDTOList = new List<ProductMenuPanelDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ProductMenuPanelDTO.SearchByProductMenuPanelParameters.PANEL_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductMenuPanelDTO.SearchByProductMenuPanelParameters.GUID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ProductMenuPanelDTO.SearchByProductMenuPanelParameters.PANEL_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ProductMenuPanelDTO.SearchByProductMenuPanelParameters.SITE_ID)
                        {

                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductMenuPanelDTO.SearchByProductMenuPanelParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1" || searchParameter.Value == "Y"));
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
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ProductMenuPanelDTO ProductMenuPanelDTO = GetProductMenuPanelDTO(dataRow);
                    ProductMenuPanelDTOList.Add(ProductMenuPanelDTO);
                }
            }
            log.LogMethodExit(ProductMenuPanelDTOList);
            return ProductMenuPanelDTOList;
        }

    }
}
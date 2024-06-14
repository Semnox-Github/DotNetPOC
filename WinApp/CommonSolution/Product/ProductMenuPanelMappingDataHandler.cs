/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu panel mapping Data handler
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

namespace Semnox.Parafait.Product
{
    public class ProductMenuPanelMappingDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ProductMenuPanelMapping as pmm ";

        /// <summary>
        /// Dictionary for searching Parameters for the ProductMenuPanelMapping object.
        /// </summary>
        private static readonly Dictionary<ProductMenuPanelMappingDTO.SearchByProductMenuPanelMappingParameters, string> DBSearchParameters = new Dictionary<ProductMenuPanelMappingDTO.SearchByProductMenuPanelMappingParameters, string>
        {
            { ProductMenuPanelMappingDTO.SearchByProductMenuPanelMappingParameters.MENU_ID,"pmm.MenuId"},
            { ProductMenuPanelMappingDTO.SearchByProductMenuPanelMappingParameters.ID,"pmm.Id"},
            { ProductMenuPanelMappingDTO.SearchByProductMenuPanelMappingParameters.PANEL_ID,"pmm.PanelId"}
        };

        /// <summary>
        /// Parameterized Constructor for ProductMenuPanelMappingDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public ProductMenuPanelMappingDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProductMenuPanelMapping Record.
        /// </summary>
        /// <param name="productMenuPanelMappingDTO">ProductMenuPanelMappingDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductMenuPanelMappingDTO productMenuPanelMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPanelMappingDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PanelId", productMenuPanelMappingDTO.PanelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MenuId", productMenuPanelMappingDTO.MenuId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", productMenuPanelMappingDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", productMenuPanelMappingDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", productMenuPanelMappingDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to ProductMenuPanelMappingDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of ProductMenuPanelMappingDTO</returns>
        private ProductMenuPanelMappingDTO GetProductMenuPanelMappingDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProductMenuPanelMappingDTO productMenuPanelMappingDTO = new ProductMenuPanelMappingDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                dataRow["PanelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PanelId"]),
                                                dataRow["MenuId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MenuId"]),
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
            return productMenuPanelMappingDTO;
        }


        /// <summary>
        /// Gets the ProductMenuPanelMapping data of passed ProductMenuPanelMappingId 
        /// </summary>
        /// <param name="productMenuPanelMappingId">productMenuPanelMappingId is passed as parameter</param>
        /// <returns>Returns ProductMenuPanelMappingDTO</returns>
        public ProductMenuPanelMappingDTO GetProductMenuPanelMapping(int id)
        {
            log.LogMethodEntry(id);
            ProductMenuPanelMappingDTO result = null;
            string query = SELECT_QUERY + @" WHERE pmm.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetProductMenuPanelMappingDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="productMenuPanelMappingDTO">ProductMenuPanelMappingDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshProductMenuPanelMappingDTO(ProductMenuPanelMappingDTO productMenuPanelMappingDTO, DataTable dt)
        {
            log.LogMethodEntry(productMenuPanelMappingDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                productMenuPanelMappingDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                productMenuPanelMappingDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                productMenuPanelMappingDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productMenuPanelMappingDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                productMenuPanelMappingDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                productMenuPanelMappingDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productMenuPanelMappingDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the ProductMenuPanelMapping Table. 
        /// </summary>
        /// <param name="productMenuPanelMappingDTO">ProductMenuPanelMappingDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated ProductMenuPanelMappingDTO</returns>
        public ProductMenuPanelMappingDTO Insert(ProductMenuPanelMappingDTO productMenuPanelMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPanelMappingDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ProductMenuPanelMapping]
                            (
                            MenuId,
                            PanelId,
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
                            @MenuId,
                            @PanelId,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(), @IsActive
                            )
                            SELECT * FROM ProductMenuPanelMapping WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productMenuPanelMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductMenuPanelMappingDTO(productMenuPanelMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ProductMenuPanelMappingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productMenuPanelMappingDTO);
            return productMenuPanelMappingDTO;
        }

        /// <summary>
        /// Update the record in the ProductMenuPanelMapping Table. 
        /// </summary>
        /// <param name="productMenuPanelMappingDTO">ProductMenuPanelMappingDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated ProductMenuPanelMappingDTO</returns>
        public ProductMenuPanelMappingDTO Update(ProductMenuPanelMappingDTO productMenuPanelMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPanelMappingDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ProductMenuPanelMapping]
                             SET
                             MenuId = @MenuId,
                             PanelId = @PanelId,
                             MasterEntityId = @MasterEntityId,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdatedDate = GETDATE(),
                             IsActive = @IsActive
                             WHERE Id = @Id
                            SELECT * FROM ProductMenuPanelMapping WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productMenuPanelMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductMenuPanelMappingDTO(productMenuPanelMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating WorkShiftUserDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productMenuPanelMappingDTO);
            return productMenuPanelMappingDTO;
        }

        internal List<ProductMenuPanelMappingDTO> GetProductMenuPanelMappingDTOList(List<int> menuIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(menuIdList);
            List<ProductMenuPanelMappingDTO> productMenuPanelMappingDTOList = new List<ProductMenuPanelMappingDTO>();
            string query = @"SELECT *
                            FROM ProductMenuPanelMapping, @menuIdList List
                            WHERE MenuId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@menuIdList", menuIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productMenuPanelMappingDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductMenuPanelMappingDTO(x)).ToList();
            }
            log.LogMethodExit(productMenuPanelMappingDTOList);
            return productMenuPanelMappingDTOList;
        }

        /// <summary>
        /// Returns the List of ProductMenuPanelMappingDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of ProductMenuPanelMappingDTO </returns>
        public List<ProductMenuPanelMappingDTO> GetProductMenuPanelMappingDTOList(List<KeyValuePair<ProductMenuPanelMappingDTO.SearchByProductMenuPanelMappingParameters, string>> searchParameters,
                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ProductMenuPanelMappingDTO> productMenuPanelMappingDTOList = new List<ProductMenuPanelMappingDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ProductMenuPanelMappingDTO.SearchByProductMenuPanelMappingParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ProductMenuPanelMappingDTO.SearchByProductMenuPanelMappingParameters.MENU_ID ||
                            searchParameter.Key == ProductMenuPanelMappingDTO.SearchByProductMenuPanelMappingParameters.ID ||
                            searchParameter.Key == ProductMenuPanelMappingDTO.SearchByProductMenuPanelMappingParameters.PANEL_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                    ProductMenuPanelMappingDTO productMenuPanelMappingDTO = GetProductMenuPanelMappingDTO(dataRow);
                    productMenuPanelMappingDTOList.Add(productMenuPanelMappingDTO);
                }
            }
            log.LogMethodExit(productMenuPanelMappingDTOList);
            return productMenuPanelMappingDTOList;
        }

    }
}
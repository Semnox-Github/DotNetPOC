/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu POS mapping data access object
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

namespace Semnox.Parafait.Product
{
    public class ProductMenuPOSMachineMapDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ProductMenuPOSMachineMap as pmp ";

        /// <summary>
        /// Parameterized Constructor for ProductMenuPOSMachineMapDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public ProductMenuPOSMachineMapDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProductMenuPOSMachineMap Record.
        /// </summary>
        /// <param name="productMenuPOSMachineMapDTO">ProductMenuPOSMachineMapDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductMenuPOSMachineMapDTO productMenuPOSMachineMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPOSMachineMapDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", productMenuPOSMachineMapDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MenuId", productMenuPOSMachineMapDTO.MenuId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachineId", productMenuPOSMachineMapDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", productMenuPOSMachineMapDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", productMenuPOSMachineMapDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to ProductMenuPOSMachineMapDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of ProductMenuPOSMachineMapDTO</returns>
        private ProductMenuPOSMachineMapDTO GetProductMenuPOSMachineMapDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProductMenuPOSMachineMapDTO productMenuPOSMachineMapDTO = new ProductMenuPOSMachineMapDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                dataRow["MenuId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MenuId"]),
                                                dataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSMachineId"]),
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
            return productMenuPOSMachineMapDTO;
        }


        /// <summary>
        /// Gets the ProductMenuPOSMachineMap data of passed ProductMenuPOSMachineMapId 
        /// </summary>
        /// <param name="productMenuPOSMachineMapId">productMenuPOSMachineMapId is passed as parameter</param>
        /// <returns>Returns ProductMenuPOSMachineMapDTO</returns>
        public ProductMenuPOSMachineMapDTO GetProductMenuPOSMachineMap(int id)
        {
            log.LogMethodEntry(id);
            ProductMenuPOSMachineMapDTO result = null;
            string query = SELECT_QUERY + @" WHERE pmp.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetProductMenuPOSMachineMapDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="productMenuPOSMachineMapDTO">ProductMenuPOSMachineMapDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshProductMenuPOSMachineMapDTO(ProductMenuPOSMachineMapDTO productMenuPOSMachineMapDTO, DataTable dt)
        {
            log.LogMethodEntry(productMenuPOSMachineMapDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                productMenuPOSMachineMapDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                productMenuPOSMachineMapDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                productMenuPOSMachineMapDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productMenuPOSMachineMapDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                productMenuPOSMachineMapDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                productMenuPOSMachineMapDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productMenuPOSMachineMapDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the ProductMenuPOSMachineMap Table. 
        /// </summary>
        /// <param name="productMenuPOSMachineMapDTO">ProductMenuPOSMachineMapDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated ProductMenuPOSMachineMapDTO</returns>
        public ProductMenuPOSMachineMapDTO Insert(ProductMenuPOSMachineMapDTO productMenuPOSMachineMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPOSMachineMapDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ProductMenuPOSMachineMap]
                            (
                            MenuId,
                            POSMachineId,
                            IsActive,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdatedDate
                            )
                            VALUES
                            (
                            @MenuId,
                            @POSMachineId,
                            @isActive,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE() 
                            )
                            SELECT * FROM ProductMenuPOSMachineMap WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productMenuPOSMachineMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductMenuPOSMachineMapDTO(productMenuPOSMachineMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ProductMenuPOSMachineMapDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productMenuPOSMachineMapDTO);
            return productMenuPOSMachineMapDTO;
        }

        /// <summary>
        /// Update the record in the ProductMenuPOSMachineMap Table. 
        /// </summary>
        /// <param name="productMenuPOSMachineMapDTO">ProductMenuPOSMachineMapDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated ProductMenuPOSMachineMapDTO</returns>
        public ProductMenuPOSMachineMapDTO Update(ProductMenuPOSMachineMapDTO productMenuPOSMachineMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPOSMachineMapDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ProductMenuPOSMachineMap]
                             SET
                             MenuId = @MenuId,
                             POSMachineId = @POSMachineId,
                             MasterEntityId = @MasterEntityId,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdatedDate = GETDATE(),
                             IsActive = @isActive
                             WHERE Id = @Id
                            SELECT * FROM ProductMenuPOSMachineMap WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productMenuPOSMachineMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductMenuPOSMachineMapDTO(productMenuPOSMachineMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating WorkShiftUserDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productMenuPOSMachineMapDTO);
            return productMenuPOSMachineMapDTO;
        }


        internal List<ProductMenuPOSMachineMapDTO> GetProductMenuPOSMachineMapDTOList(List<int> posMachineIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(posMachineIdList);
            List<ProductMenuPOSMachineMapDTO> productMenuPOSMachineMapDTOList = new List<ProductMenuPOSMachineMapDTO>();
            string query = @"SELECT *
                            FROM ProductMenuPOSMachineMap, @posMachineIdList List
                            WHERE POSMachineId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@posMachineIdList", posMachineIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productMenuPOSMachineMapDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductMenuPOSMachineMapDTO(x)).ToList();
            }
            log.LogMethodExit(productMenuPOSMachineMapDTOList);
            return productMenuPOSMachineMapDTOList;
        }

    }
}
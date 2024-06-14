/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu panel exclusion data access object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By           Remarks          
 ********************************************************************************************* 
 *2.130.0     19-Jul-2021      Lakshminarayana       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class ProductMenuPanelExclusionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ProductMenuPanelExclusion as pmpe ";

        /// <summary>
        /// Parameterized Constructor for ProductMenuPanelExclusionDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public ProductMenuPanelExclusionDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProductMenuPanelExclusion Record.
        /// </summary>
        /// <param name="productMenuPanelExclusionDTO">ProductMenuPanelExclusionDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductMenuPanelExclusionDTO productMenuPanelExclusionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPanelExclusionDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", productMenuPanelExclusionDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PanelId", productMenuPanelExclusionDTO.PanelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachineId", productMenuPanelExclusionDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserRoleId", productMenuPanelExclusionDTO.UserRoleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PosTypeId", productMenuPanelExclusionDTO.POSTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", productMenuPanelExclusionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", productMenuPanelExclusionDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to ProductMenuPanelExclusionDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of ProductMenuPanelExclusionDTO</returns>
        private ProductMenuPanelExclusionDTO GetProductMenuPanelExclusionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProductMenuPanelExclusionDTO productMenuPanelExclusionDTO = new ProductMenuPanelExclusionDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                dataRow["PanelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PanelId"]),
                                                dataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSMachineId"]),
                                                dataRow["UserRoleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserRoleId"]),
                                                dataRow["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSTypeId"]),
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
            return productMenuPanelExclusionDTO;
        }


        /// <summary>
        /// Gets the ProductMenuPanelExclusion data of passed ProductMenuPanelExclusionId 
        /// </summary>
        /// <param name="productMenuPanelExclusionId">productMenuPanelExclusionId is passed as parameter</param>
        /// <returns>Returns ProductMenuPanelExclusionDTO</returns>
        public ProductMenuPanelExclusionDTO GetProductMenuPanelExclusion(int id)
        {
            log.LogMethodEntry(id);
            ProductMenuPanelExclusionDTO result = null;
            string query = SELECT_QUERY + @" WHERE pmpe.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetProductMenuPanelExclusionDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="productMenuPanelExclusionDTO">ProductMenuPanelExclusionDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshProductMenuPanelExclusionDTO(ProductMenuPanelExclusionDTO productMenuPanelExclusionDTO, DataTable dt)
        {
            log.LogMethodEntry(productMenuPanelExclusionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                productMenuPanelExclusionDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                productMenuPanelExclusionDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                productMenuPanelExclusionDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productMenuPanelExclusionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                productMenuPanelExclusionDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                productMenuPanelExclusionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productMenuPanelExclusionDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the ProductMenuPanelExclusion Table. 
        /// </summary>
        /// <param name="productMenuPanelExclusionDTO">ProductMenuPanelExclusionDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated ProductMenuPanelExclusionDTO</returns>
        public ProductMenuPanelExclusionDTO Insert(ProductMenuPanelExclusionDTO productMenuPanelExclusionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPanelExclusionDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ProductMenuPanelExclusion]
                            (
                            PanelId,
                            POSMachineId,
                            UserRoleId,
                            PosTypeId,
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
                            @PanelId,
                            @POSMachineId,
                            @UserRoleId,
                            @PosTypeId,
                            @isActive,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE() 
                            )
                            SELECT * FROM ProductMenuPanelExclusion WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productMenuPanelExclusionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductMenuPanelExclusionDTO(productMenuPanelExclusionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ProductMenuPanelExclusionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productMenuPanelExclusionDTO);
            return productMenuPanelExclusionDTO;
        }

        /// <summary>
        /// Update the record in the ProductMenuPanelExclusion Table. 
        /// </summary>
        /// <param name="productMenuPanelExclusionDTO">ProductMenuPanelExclusionDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated ProductMenuPanelExclusionDTO</returns>
        public ProductMenuPanelExclusionDTO Update(ProductMenuPanelExclusionDTO productMenuPanelExclusionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuPanelExclusionDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ProductMenuPanelExclusion]
                             SET
                             PanelId = @PanelId,
                             POSMachineId = @POSMachineId,
                             UserRoleId = @UserRoleId,
                             PosTypeId = @PosTypeId,
                             MasterEntityId = @MasterEntityId,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdatedDate = GETDATE(),
                             IsActive = @isActive
                             WHERE Id = @Id
                            SELECT * FROM ProductMenuPanelExclusion WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productMenuPanelExclusionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductMenuPanelExclusionDTO(productMenuPanelExclusionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating WorkShiftUserDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productMenuPanelExclusionDTO);
            return productMenuPanelExclusionDTO;
        }


        internal List<ProductMenuPanelExclusionDTO> GetProductMenuPanelExclusionDTOListForPOSMachines(List<int> posMachineIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(posMachineIdList, activeRecords);
            List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionDTOList = new List<ProductMenuPanelExclusionDTO>();
            string query = @"SELECT *
                            FROM ProductMenuPanelExclusion, @posMachineIdList List
                            WHERE POSMachineId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@posMachineIdList", posMachineIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productMenuPanelExclusionDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductMenuPanelExclusionDTO(x)).ToList();
            }
            log.LogMethodExit(productMenuPanelExclusionDTOList);
            return productMenuPanelExclusionDTOList;
        }


        internal List<ProductMenuPanelExclusionDTO> GetProductMenuPanelExclusionDTOListForUserRoles(List<int> userRoleIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(userRoleIdList, activeRecords);
            List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionDTOList = new List<ProductMenuPanelExclusionDTO>();
            string query = @"SELECT *
                            FROM ProductMenuPanelExclusion, @userRoleIdList List
                            WHERE UserRoleId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@userRoleIdList", userRoleIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productMenuPanelExclusionDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductMenuPanelExclusionDTO(x)).ToList();
            }
            log.LogMethodExit(productMenuPanelExclusionDTOList);
            return productMenuPanelExclusionDTOList;
        }

        internal List<ProductMenuPanelExclusionDTO> GetProductMenuPanelExclusionDTOListForPosTypes(List<int> posTypeIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(posTypeIdList, activeRecords);
            List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionDTOList = new List<ProductMenuPanelExclusionDTO>();
            string query = @"SELECT *
                            FROM ProductMenuPanelExclusion, @posTypeIdList List
                            WHERE PosTypeId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@posTypeIdList", posTypeIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productMenuPanelExclusionDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductMenuPanelExclusionDTO(x)).ToList();
            }
            log.LogMethodExit(productMenuPanelExclusionDTOList);
            return productMenuPanelExclusionDTOList;
        }

    }
}
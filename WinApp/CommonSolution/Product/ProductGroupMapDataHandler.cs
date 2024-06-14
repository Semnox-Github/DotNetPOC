/********************************************************************************************
* Project Name - Product
* Description  - DataHandler - ProductGroupMap
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.170.0     05-Jul-2023      Lakshminarayana     Created
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.Product
{
    class ProductGroupMapDataHandler
    {
        private Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT pgm.*
                                              FROM ProductGroupMap AS pgm";

        private const string INSERT_QUERY = @"INSERT INTO[dbo].[ProductGroupMap] 
                                                        (                                                 
                                                         ProductGroupId,
                                                         ProductId,
                                                         SortOrder,
                                                         IsActive,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdatedBy,
                                                         LastUpdateDate,
                                                         site_id,
                                                         MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                          @ProductGroupId,
                                                          @ProductId,
                                                          @SortOrder,
                                                          @IsActive,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedBy,
                                                          GETDATE(),
                                                          @SiteId,
                                                          @MasterEntityId                                                        
                                                         )
                                                SELECT* FROM ProductGroupMap WHERE Id = scope_identity()";
        private const string UPDATE_QUERY = @"UPDATE [dbo].[ProductGroupMap] set
                                                [ProductGroupId]  = @ProductGroupId,
                                                [ProductId]       = @ProductId,
                                                [SortOrder]       = @SortOrder,
                                                [IsActive]        = @IsActive,
                                                [MasterEntityId]  = @MasterEntityId,
                                                [LastUpdatedBy]   = @LastUpdatedBy,
                                                [LastUpdateDate]  = GETDATE()
                                                where Id = @Id
                                                SELECT * FROM ProductGroupMap WHERE Id = @Id";
        /// <summary>
        /// Default constructor of ProductGroupMapDataHandler class
        /// </summary>
        public ProductGroupMapDataHandler(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(ProductGroupMapDTO productGroupMapDTO)
        {
            log.LogMethodEntry(productGroupMapDTO);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", productGroupMapDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductGroupId", productGroupMapDTO.ProductGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", productGroupMapDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SortOrder", productGroupMapDTO.SortOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", productGroupMapDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", executionContext.UserId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", executionContext.UserId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", executionContext.SiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", productGroupMapDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the ProductGroupMap record to the database
        /// </summary>
        /// <param name="productGroupMapDTO">ProductGroupMapDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted ProductGroupMapDTO</returns>
        internal ProductGroupMapDTO Save(ProductGroupMapDTO productGroupMapDTO)
        {
            log.LogMethodEntry(productGroupMapDTO);
            string query = productGroupMapDTO.Id <= -1 ? INSERT_QUERY : UPDATE_QUERY;

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productGroupMapDTO).ToArray(), unitOfWork.SQLTrx);
                DataRow dataRow = dt.Rows[0];
                productGroupMapDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                productGroupMapDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                productGroupMapDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productGroupMapDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                productGroupMapDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                productGroupMapDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productGroupMapDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productGroupMapDTO);
            return productGroupMapDTO;
        }

        /// <summary>
        /// Gets the GetProductGroupMap data
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ProductGroupMapDTO</returns>
        internal ProductGroupMapDTO GetProductGroupMap(int id)
        {
            log.LogMethodEntry(id);
            ProductGroupMapDTO result = null;
            string query = SELECT_QUERY + @" WHERE pgm.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, unitOfWork.SQLTrx);
            if (dataTable.Rows.Count > 0)
            {
                result = GetProductGroupMapDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Converts the Data row object to ProductGroupMapDTO class type
        /// </summary>
        /// <param name="dataRow">ProductGroupMap DataRow</param>
        /// <returns>Returns ProductGroupMapDTO</returns>
        private ProductGroupMapDTO GetProductGroupMapDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProductGroupMapDTO productGroupMapDTO = new ProductGroupMapDTO(Convert.ToInt32(dataRow["Id"]),
                                                                           dataRow["ProductGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductGroupId"]),
                                                                           dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                                                           dataRow["SortOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SortOrder"]),
                                                                           dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                                           dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                                           dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                                           dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                                           dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                                           dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                                           dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                                           dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                                           dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                                         );
            log.LogMethodExit(productGroupMapDTO);
            return productGroupMapDTO;
        }

        /// <summary>
        /// Gets the ProductGroupMapDTO List for Product Group Id List
        /// </summary>
        public List<ProductGroupMapDTO> GetProductGroupMapDTOListOfProductGroups(List<int> productGroupIdList, bool activeRecords)
        {
            log.LogMethodEntry(productGroupIdList, activeRecords);
            List<ProductGroupMapDTO> list = new List<ProductGroupMapDTO>();
            string query = SELECT_QUERY + @" INNER JOIN @ProductGroupIdList List
                                             ON pgm.ProductGroupId = List.Id ";
            if (activeRecords)
            {
                query += " WHERE ISNULL(pgm.IsActive, 1) = 1 ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@ProductGroupIdList", productGroupIdList, null, unitOfWork.SQLTrx);
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    list.Add(GetProductGroupMapDTO(row));
                }
            }
            log.LogMethodExit(list);
            return list;
        }

    }

}
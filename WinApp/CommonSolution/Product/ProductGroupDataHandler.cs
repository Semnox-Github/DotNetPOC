/********************************************************************************************
* Project Name - Product
* Description  - DataHandler - ProductGroup
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.170.0     05-Jul-2023      Lakshminarayana     Created
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Product
{
    class ProductGroupDataHandler
    {
        private Semnox.Parafait.logging.Logger log;
        private readonly ExecutionContext executionContext;
        private readonly UnitOfWork unitOfWork;
        private readonly DataAccessHandler dataAccessHandler;
        /// <summary>
        /// dBSearchParameters for searching the respective Search fields of ProductGamesDTO 
        /// </summary>
        private static readonly Dictionary<ProductGroupDTO.SearchByParameters, string> dBSearchParameters = new Dictionary<ProductGroupDTO.SearchByParameters, string>
        {
            { ProductGroupDTO.SearchByParameters.ID, "pg.Id"},
            { ProductGroupDTO.SearchByParameters.ID_LIST, "pg.Id"},
            { ProductGroupDTO.SearchByParameters.NAME, "pg.Name"},
            { ProductGroupDTO.SearchByParameters.SITE_ID, "pg.site_id"},
            { ProductGroupDTO.SearchByParameters.IS_ACTIVE, "pg.IsActive"},
            { ProductGroupDTO.SearchByParameters.MASTER_ENTITY_ID, "pg.MasterEntityId"},
        };
        private const string SELECT_QUERY = @"SELECT pg.*
                                              FROM ProductGroup AS pg";

        private const string SELECT_COUNT_QUERY = @"SELECT COUNT(1) TotalCount
                                                    FROM ProductGroup AS pg";

        private const string INSERT_QUERY = @"INSERT INTO[dbo].[ProductGroup] 
                                                        (                                                 
                                                         Name,
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
                                                          @Name,
                                                          @IsActive,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedBy,
                                                          GETDATE(),
                                                          @SiteId,
                                                          @MasterEntityId                                                        
                                                         )
                                                SELECT* FROM ProductGroup WHERE Id = scope_identity()";
        private const string UPDATE_QUERY = @"UPDATE [dbo].[ProductGroup] set
                                                [Name]            = @Name,
                                                [IsActive]        = @IsActive,
                                                [MasterEntityId]  = @MasterEntityId,
                                                [LastUpdatedBy]   = @LastUpdatedBy,
                                                [LastUpdateDate]  = GETDATE()
                                                where Id = @Id
                                                SELECT * FROM ProductGroup WHERE Id = @Id";
        /// <summary>
        /// Default constructor of ProductGroupDataHandler class
        /// </summary>
        public ProductGroupDataHandler(ExecutionContext executionContext, UnitOfWork unitOfWork)
        {
            log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, unitOfWork);
            this.executionContext = executionContext;
            this.unitOfWork = unitOfWork;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(ProductGroupDTO productGroupDTO)
        {
            log.LogMethodEntry(productGroupDTO);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", productGroupDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", productGroupDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", productGroupDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", executionContext.UserId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", executionContext.UserId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", executionContext.SiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", productGroupDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the ProductGroup record to the database
        /// </summary>
        /// <param name="productGroupDTO">ProductGroupDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted ProductGroupDTO</returns>
        internal ProductGroupDTO Save(ProductGroupDTO productGroupDTO)
        {
            log.LogMethodEntry(productGroupDTO);
            string query = productGroupDTO.Id <= -1 ? INSERT_QUERY : UPDATE_QUERY;

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productGroupDTO).ToArray(), unitOfWork.SQLTrx);
                DataRow dataRow = dt.Rows[0];
                productGroupDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                productGroupDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                productGroupDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productGroupDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                productGroupDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                productGroupDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productGroupDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productGroupDTO);
            return productGroupDTO;
        }

        /// <summary>
        /// Converts the Data row object to ProductGroupDTO class type
        /// </summary>
        /// <param name="dataRow">ProductGroup DataRow</param>
        /// <returns>Returns ProductGroupDTO</returns>
        private ProductGroupDTO GetProductGroupDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProductGroupDTO productGroupDTO = new ProductGroupDTO(Convert.ToInt32(dataRow["Id"]),
                                                                         dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
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
            log.LogMethodExit(productGroupDTO);
            return productGroupDTO;
        }

        /// <summary>
        /// Gets the GetProductGroup data
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ProductGroupDTO</returns>
        internal ProductGroupDTO GetProductGroup(int id)
        {
            log.LogMethodEntry(id);
            ProductGroupDTO result = null;
            string query = SELECT_QUERY + @" WHERE pg.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, unitOfWork.SQLTrx);
            if (dataTable.Rows.Count > 0)
            {
                result = GetProductGroupDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal bool IsProductGroupReferenced(int id)
        {
            log.LogMethodEntry(id);
            int referenceCount = 0;
            string query = @"SELECT SUM(ReferenceCount) ReferenceCount
                            FROM
                            (
                            SELECT COUNT(1) AS ReferenceCount
                            FROM DiscountPurchaseCriteria
                            WHERE ProductGroupId = @ProductGroupId
                            AND IsActive = 1
                            UNION ALL
                            SELECT COUNT(1) AS ReferenceCount
                            FROM DiscountedProducts
                            WHERE ProductGroupId = @ProductGroupId
                            AND IsActive = 1
                            ) A ";
            SqlParameter parameter = new SqlParameter("@ProductGroupId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, unitOfWork.SQLTrx);
            if (dataTable.Rows.Count > 0)
            {
                referenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            bool result = referenceCount > 0;
            log.LogMethodExit(result);
            return result;
        }

        internal int GetProductGroupDTOListCount(SearchParameterList<ProductGroupDTO.SearchByParameters> searchParameters)
        {
            int result = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_COUNT_QUERY +
                                 GetWhereClause(searchParameters, parameters);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), unitOfWork.SQLTrx);
            result = Convert.ToInt32(dataTable.Rows[0]["TotalCount"]);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ProductGroupDTO List for productgroupGuidList Value List
        /// </summary>
        /// <param name="productgroupGuidList">string list parameter</param>
        /// <returns>Returns List of ProductGroupDTO</returns>
        public List<ProductGroupDTO> GetProductGroupDTOList(List<string> productgroupGuidList)
        {
            log.LogMethodEntry(productgroupGuidList);
            List<ProductGroupDTO> productgroupDTOList = new List<ProductGroupDTO>();
            string query = SELECT_QUERY + @" , @ProductGroupGuidList List
                                            where pg.Guid = List.Value";

            DataTable table = dataAccessHandler.BatchSelect(query, "@ProductGroupGuidList", productgroupGuidList, null, unitOfWork.SQLTrx);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productgroupDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductGroupDTO(x)).ToList();
            }
            log.LogMethodExit(productgroupDTOList);
            return productgroupDTOList;
        }

        /// <summary>
        /// Returns the List of ProductGroupDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of ProductGroupDTO </returns>
        public List<ProductGroupDTO> GetProductGroupDTOList(SearchParameterList<ProductGroupDTO.SearchByParameters> searchParameters, 
                                                            int pageNumber, 
                                                            int pageSize)
        {
            log.LogMethodEntry(searchParameters);
            List<ProductGroupDTO> productGroupDTOList = new List<ProductGroupDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY + 
                                 GetWhereClause(searchParameters, parameters) +
                                 GetPaginationClause(pageSize, pageNumber);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), unitOfWork.SQLTrx);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ProductGroupDTO productGroupDTO = GetProductGroupDTO(dataRow);
                    productGroupDTOList.Add(productGroupDTO);
                }
            }
            log.LogMethodExit(productGroupDTOList);
            return productGroupDTOList;
        }

        private string GetWhereClause(List<KeyValuePair<ProductGroupDTO.SearchByParameters, string>> searchParameters, List<SqlParameter> parameters)
        {
            log.LogMethodEntry(searchParameters, parameters);
            string joiner = string.Empty;
            int counter = 0;
            string result = string.Empty;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (var searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (dBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ProductGroupDTO.SearchByParameters.ID)
                        {
                            query.Append(joiner + dBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductGroupDTO.SearchByParameters.ID_LIST)
                        {
                            query.Append(joiner + @"(" + dBSearchParameters[searchParameter.Key] + " IN ( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + "))");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ProductGroupDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + dBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductGroupDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + dBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1" || searchParameter.Value == "Y"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + dBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                result = query.ToString();
            }
            log.LogMethodExit(result);
            return result;
        }

        private string GetPaginationClause(int pageSize, int pageNumber)
        {
            log.LogMethodEntry(pageSize, pageNumber);
            string result = string.Empty;
            if (pageSize > 0)
            {
                result = " ORDER BY Id OFFSET " + (pageNumber * pageSize).ToString() + " ROWS FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            log.LogMethodExit(result);
            return result;
        }

        internal DateTime? GetProductGroupModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastUpdateDate) LastUpdatedDate from ProductGroup WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from ProductGroupMap WHERE (site_id = @siteId or @siteId = -1)
                            ) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, unitOfWork.SQLTrx);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }

    }

}
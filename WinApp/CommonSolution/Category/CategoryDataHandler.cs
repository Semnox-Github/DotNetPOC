/********************************************************************************************
 * Project Name - Category 
 * Description  - Data Handler -CategoryDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Apr-2016   Krishnanand    Created
 *2.60        20-Mar-2019   Akshay G       Modified isActive from string to bool and handled in this dataHandler 
 *2.60.2      23-Apr-2019   Guru S A       Is_Active search condition added
 *            29-May-2019   Jagan Mohan    Code merge from Development to WebManagementStudio
 *2.70        30-June-2019  Indrajeet K    Created DeleteCategory()method for implementation of Hard Deletion.       
 *            03-Jul-2019   Dakshakh raj   Modified : added GetSQLParameters() and SQL injection Issue Fix
 *2.70.2        05-Dec-2019   Jinto Thomas            Removed siteid from update query
 *2.110.0     07-Oct-2020   Mushahid Faizan  Modified : Added methods for Pagination .
 *2.130.0     20-Jul-2021   Mushahid Faizan Modified : POS UI Redesign changes 
 ********************************************************************************************/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using System.Globalization;

namespace Semnox.Parafait.Category
{
    /// <summary>
    /// Category Data Handler - Handles insert, update and select of category data objects
    /// </summary>

    public class CategoryDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Category AS c ";
        private List<SqlParameter> parameters = new List<SqlParameter>();

        /// <summary>
        /// Dictionary for searching Parameters for the Category object.
        /// </summary>

        private static readonly Dictionary<CategoryDTO.SearchByCategoryParameters, string> DBSearchParameters = new Dictionary<CategoryDTO.SearchByCategoryParameters, string>
        {
            {CategoryDTO.SearchByCategoryParameters.CATEGORY_ID, "c.CategoryId"},
            {CategoryDTO.SearchByCategoryParameters.CATEGORY_ID_LIST, "c.CategoryId"},
            {CategoryDTO.SearchByCategoryParameters.NAME, "c.Name"},
            {CategoryDTO.SearchByCategoryParameters.IS_ACTIVE, "c.IsActive"},
            {CategoryDTO.SearchByCategoryParameters.SITE_ID, "c.Site_Id"},
            {CategoryDTO.SearchByCategoryParameters.MASTER_ENTITY_ID, "c.MasterEntityId"} ,//Added search parameter 16-May-2017
            {CategoryDTO.SearchByCategoryParameters.LAST_UPDATED_DATE, "c.LastUpdateDate"} 
        };


        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS CategoryType;
                                            MERGE INTO Category tbl
                                            USING @CategoryList AS src
                                            ON src.CategoryId = tbl.CategoryId
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            ParentCategoryId = src.ParentCategoryId,
                                            Name = src.Name,
                                            IsActive = src.IsActive,
                                            Lastupdated_userid = src.Lastupdated_userid,
                                            site_id = src.site_id,
                                            Guid = src.Guid,
                                            MasterEntityId = src.MasterEntityId,
                                            LastUpdateDate = GETDATE()
                                            WHEN NOT MATCHED THEN INSERT (
                                            ParentCategoryId,
                                            Name,
                                            IsActive,
                                            Lastupdated_userid,
                                            site_id,
                                            Guid,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdateDate
                                            )VALUES (
                                            src.ParentCategoryId,
                                            src.Name,
                                            src.IsActive,
                                            src.Lastupdated_userid,
                                            src.site_id,
                                            src.Guid,
                                            src.MasterEntityId,
                                            src.CreatedBy,
                                            GETDATE(),
                                            GETDATE()
                                            )
                                            OUTPUT
                                            inserted.CategoryId,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.Lastupdated_userid,
                                            inserted.LastUpdateDate,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(CategoryId, CreatedBy, CreationDate, Lastupdated_userid, LastUpdateDate, site_id, Guid);
                                            SELECT * FROM @Output;";
        #endregion

        /// <summary>
        /// Parameterized Constructor for CategoryDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>

        public CategoryDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Category Record.
        /// </summary>
        /// <param name="categoryDTO">CategoryDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>

        private List<SqlParameter> GetSQLParameters(CategoryDTO categoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(categoryDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CategoryId", categoryDTO.CategoryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentCategoryId", categoryDTO.ParentCategoryId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", categoryDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", categoryDTO.IsActive ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", categoryDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Converts the Data row object to CategoryDTO class type
        /// </summary>
        /// <param name="categoryDataRow">CategoryDTO DataRow</param>
        /// <returns>Returns CategoryDTO</returns>

        private CategoryDTO GetCategoryDTO(DataRow categoryDataRow)
        {
            log.LogMethodEntry(categoryDataRow);
            CategoryDTO categoryDTO = new CategoryDTO(categoryDataRow["CategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(categoryDataRow["CategoryId"]),
                                                      categoryDataRow["ParentCategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(categoryDataRow["ParentCategoryId"]),
                                                      categoryDataRow["Name"].ToString(),
                                                      categoryDataRow["IsActive"] == DBNull.Value ? true : categoryDataRow["IsActive"].ToString() == "Y",
                                                      categoryDataRow["Lastupdated_userid"].ToString(),
                                                      categoryDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(categoryDataRow["site_id"]),
                                                      categoryDataRow["Guid"].ToString(),
                                                      categoryDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(categoryDataRow["SynchStatus"]),
                                                      categoryDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(categoryDataRow["MasterEntityId"]),
                                                      categoryDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(categoryDataRow["CreatedBy"]),
                                                      categoryDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(categoryDataRow["CreationDate"]),
                                                      categoryDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(categoryDataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(categoryDTO);
            return categoryDTO;
        }


        /// <summary>
        /// Gets the Category data of all categories
        /// </summary>
        /// <returns>Returns CategoryDTO</returns>
        public CategoryDTO GetCategory()
        {
            log.LogMethodEntry();
            string selectCategoryQuery = SELECT_QUERY;
            //SqlParameter[] selectCategoryParameters = new SqlParameter[1];
            //selectCategoryParameters[0] = new SqlParameter("@categoryId", CategoryId);
            DataTable category = dataAccessHandler.executeSelectQuery(selectCategoryQuery, null, sqlTransaction);
            if (category.Rows.Count > 0)
            {
                DataRow categoryRow = category.Rows[0];
                CategoryDTO categoryDataObject = GetCategoryDTO(categoryRow);
                log.LogMethodExit(categoryDataObject);
                return categoryDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// Gets the Category data of passed Category id
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns>Returns CategoryDTO</returns>

        public CategoryDTO GetCategory(int CategoryId)
        {
            log.LogMethodEntry(CategoryId);
            CategoryDTO result = null;
            string selectCategoryQuery = SELECT_QUERY + @" WHERE c.CategoryId = @CategoryId";
            SqlParameter[] selectCategoryParameters = new SqlParameter[1];
            selectCategoryParameters[0] = new SqlParameter("@categoryId", CategoryId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectCategoryQuery, selectCategoryParameters, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetCategoryDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// Retriving category by passing query
        /// </summary>
        /// <param name="sqlQuery">Query passed for retriving the category</param>
        /// <returns> List of CategoryDTO </returns>
        public List<CategoryDTO> GetCategoryList(string sqlQuery)
        {
            log.LogMethodEntry();
            string Query = sqlQuery.ToUpper();
            if (Query.Contains("DROP") || Query.Contains("UPDATE") || Query.Contains("DELETE"))
            {
                log.LogMethodExit();
                return null;
            }
            DataTable categoryData = dataAccessHandler.executeSelectQuery(sqlQuery, null, sqlTransaction);
            if (categoryData.Rows.Count > 0)
            {
                List<CategoryDTO> categoryList = new List<CategoryDTO>();
                foreach (DataRow categoryDataRow in categoryData.Rows)
                {
                    CategoryDTO categoryDataObject = GetCategoryDTO(categoryDataRow);
                    categoryList.Add(categoryDataObject);
                }
                log.LogMethodExit(categoryList);
                return categoryList; ;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Returns the no of Categories matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetCategoriesCount(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int categoryDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                categoryDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(categoryDTOCount);
            return categoryDTOCount;
        }


        /// <summary>
        ///  Returns the Category table columns
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategoryColumns()
        {
            string selectCategoryQuery = "SELECT columns FROM(SELECT '' AS columns UNION ALL SELECT COLUMN_NAME AS columns FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Category') a ORDER BY columns";
            DataTable categoryTableColumns = dataAccessHandler.executeSelectQuery(selectCategoryQuery, null, sqlTransaction);
            return categoryTableColumns;
        }

        /////// <summary>
        /////// Returns the List of CategoryDTO based on the search parameters.
        /////// </summary>
        /////// <param name="searchParameters">search Parameters</param>
        /////// <returns>Returns the List of CategoryDTO</returns>
        //public List<CategoryDTO> GetCategoryList(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters)
        //{
        //    log.LogMethodEntry(searchParameters, sqlTransaction);
        //    List<CategoryDTO> categoryDTOList = null;
        //    List<SqlParameter> parameters = new List<SqlParameter>();
        //    string selectQuery = SELECT_QUERY;
        //    if ((searchParameters != null) && (searchParameters.Count > 0))
        //    {
        //        string joiner;
        //        int counter = 0;
        //        StringBuilder query = new StringBuilder(" WHERE ");
        //        foreach (KeyValuePair<CategoryDTO.SearchByCategoryParameters, string> searchParameter in searchParameters)
        //        {
        //            joiner = counter == 0 ? string.Empty : " and ";
        //            if (DBSearchParameters.ContainsKey(searchParameter.Key))
        //            {
        //                if (searchParameter.Key == CategoryDTO.SearchByCategoryParameters.CATEGORY_ID
        //                    || searchParameter.Key == CategoryDTO.SearchByCategoryParameters.MASTER_ENTITY_ID)
        //                {
        //                    query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
        //                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
        //                }
        //                else if (searchParameter.Key.Equals(CategoryDTO.SearchByCategoryParameters.IS_ACTIVE))
        //                {
        //                    query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
        //                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
        //                }
        //                else if (searchParameter.Key == CategoryDTO.SearchByCategoryParameters.SITE_ID)
        //                {
        //                    query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
        //                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
        //                }
        //                else
        //                {
        //                    query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
        //                    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
        //                }
        //            }
        //            else
        //            {
        //                string message = "The query parameter does not exist " + searchParameter.Key;
        //                log.LogVariableState("searchParameter.Key", searchParameter.Key);
        //                log.LogMethodExit(null, "Throwing exception -" + message);
        //                throw new Exception(message);
        //            }
        //            counter++;
        //        }
        //        selectQuery = selectQuery + query;
        //    }
        //    DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
        //    if (dataTable.Rows.Count > 0)
        //    {
        //        categoryDTOList = new List<CategoryDTO>();
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            CategoryDTO categoryDTO = GetCategoryDTO(dataRow);
        //            categoryDTOList.Add(categoryDTO);
        //        }
        //    }
        //    log.LogMethodExit(categoryDTOList);
        //    return categoryDTOList;
        //}

        /// <summary>
        /// Gets the CategoryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CategoryDTO matching the search criteria</returns>
        public List<CategoryDTO> GetCategoryList(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters);
            List<CategoryDTO> categoryDTOList = new List<CategoryDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            if (currentPage > 0 && pageSize > 0)
            {
                selectQuery += " ORDER BY c.CategoryId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                categoryDTOList = new List<CategoryDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CategoryDTO categoryDTO = GetCategoryDTO(dataRow);
                    categoryDTOList.Add(categoryDTO);
                }
            }
            log.LogMethodExit(categoryDTOList);
            return categoryDTOList;
        }

        /// <summary>
        /// Returns the List of CategoryDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of CategoryDTO</returns>
        public string GetFilterQuery(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            StringBuilder query = new StringBuilder(" ");

            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<CategoryDTO.SearchByCategoryParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CategoryDTO.SearchByCategoryParameters.CATEGORY_ID
                            || searchParameter.Key == CategoryDTO.SearchByCategoryParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(CategoryDTO.SearchByCategoryParameters.IS_ACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == CategoryDTO.SearchByCategoryParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CategoryDTO.SearchByCategoryParameters.CATEGORY_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(CategoryDTO.SearchByCategoryParameters.LAST_UPDATED_DATE))
                        {
                            query.Append(joiner + " ISNULL(" + DBSearchParameters[searchParameter.Key] + ", GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
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
            }
            log.LogMethodExit(query);
            return query.ToString();
        }

        /// <summary>
        ///  Inserts the record to the Category Table.
        /// </summary>
        /// <param name="categoryDTO">CategoryDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the CategoryDTO </returns>

        public CategoryDTO InsertCategory(CategoryDTO categoryDTO, string loginId, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(categoryDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[Category]
                           (ParentCategoryId,
                            Name,
                            IsActive,
                            Lastupdated_userid,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdateDate
                             )
                     VALUES
                           (
                           @ParentCategoryId,
                           @Name,
                           @IsActive,
                           @LastUpdatedBy,
                           @site_id,
                           NEWID(),
                           @MasterEntityId,
                           @CreatedBy,
                           getDate() ,
                           getDate() 
                           ) SELECT * FROM Category WHERE CategoryId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(categoryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCategoryDTO(categoryDTO, dt);
                SaveInventoryActivityLog(categoryDTO, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting categoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(categoryDTO);
            return categoryDTO;
        }


        /// <summary>
        ///  Updates the record to the Category Table.
        /// </summary>
        /// <param name="categoryDTO">CategoryDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the CategoryDTO </returns>

        public CategoryDTO UpdateCategory(CategoryDTO categoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(categoryDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[Category]
                           SET
                            ParentCategoryId = @ParentCategoryId,
                            Name = @Name,
                            IsActive = @IsActive,
                            Lastupdated_userid = @LastUpdatedBy,
                            --site_id = @site_id,
                            MasterEntityId = @MasterEntityId,
                            LastUpdateDate = GETDATE()
                            WHERE CategoryId  = @CategoryId
                             SELECT * FROM Category WHERE CategoryId = @CategoryId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(categoryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCategoryDTO(categoryDTO, dt);
                SaveInventoryActivityLog(categoryDTO, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CategoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(categoryDTO);
            return categoryDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="categoryDTO">CategoryDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        private void RefreshCategoryDTO(CategoryDTO categoryDTO, DataTable dt)
        {
            log.LogMethodEntry(categoryDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                categoryDTO.CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]);
                categoryDTO.IsActive = dataRow["IsActive"] == DBNull.Value ? true : dataRow["IsActive"].ToString() == "Y";
                categoryDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                categoryDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                categoryDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                categoryDTO.LastUpdatedUserId = dataRow["Lastupdated_userid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Lastupdated_userid"]);
                categoryDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                categoryDTO.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Inserts the Category record to the database
        /// </summary>
        /// <param name="categoryDTOList">List of CategoryDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<CategoryDTO> categoryDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(categoryDTOList, loginId, siteId);
            Dictionary<string, CategoryDTO> categoryDTOGuidMap = GetCategoryDTOGuidMap(categoryDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(categoryDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "CategoryType",
                                                                "@CategoryList");
            UpdateCategoryDTOList(categoryDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<CategoryDTO> categoryDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(categoryDTOList, loginId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[12];
            columnStructures[0] = new SqlMetaData("CategoryId", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("ParentCategoryId", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("Name", SqlDbType.NVarChar, 100);
            columnStructures[3] = new SqlMetaData("IsActive", SqlDbType.VarChar, 1);
            columnStructures[4] = new SqlMetaData("Lastupdated_userid", SqlDbType.NVarChar, 50);
            columnStructures[5] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[6] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[7] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[8] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[9] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[10] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[11] = new SqlMetaData("LastUpdateDate", SqlDbType.DateTime);

            for (int i = 0; i < categoryDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(categoryDTOList[i].CategoryId, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(categoryDTOList[i].ParentCategoryId, true));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(categoryDTOList[i].Name));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(categoryDTOList[i].IsActive == true ? "Y" : "N"));
                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(loginId));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(Guid.Parse(categoryDTOList[i].Guid)));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(categoryDTOList[i].SynchStatus));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(categoryDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(loginId));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(categoryDTOList[i].CreationDate));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(categoryDTOList[i].LastUpdateDate));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private Dictionary<string, CategoryDTO> GetCategoryDTOGuidMap(List<CategoryDTO> categoryDTOList)
        {
            Dictionary<string, CategoryDTO> result = new Dictionary<string, CategoryDTO>();
            for (int i = 0; i < categoryDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(categoryDTOList[i].Guid))
                {
                    categoryDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(categoryDTOList[i].Guid, categoryDTOList[i]);
            }
            return result;
        }

        private void UpdateCategoryDTOList(Dictionary<string, CategoryDTO> categoryDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                CategoryDTO categoryDTO = categoryDTOGuidMap[Convert.ToString(row["Guid"])];
                categoryDTO.CategoryId = row["CategoryId"] == DBNull.Value ? -1 : Convert.ToInt32(row["CategoryId"]);
                categoryDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                categoryDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                categoryDTO.LastUpdatedUserId = row["Lastupdated_userid"] == DBNull.Value ? string.Empty : Convert.ToString(row["Lastupdated_userid"]);
                categoryDTO.LastUpdateDate = row["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdateDate"]);
                categoryDTO.Site_Id = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                categoryDTO.IsActive = row["IsActive"] == DBNull.Value ? true : row["site_id"].ToString() == "Y";
                categoryDTO.AcceptChanges();
            }
        }
        /// <summary>
        /// Delete the record Product Category - Hard Deletion
        /// </summary>
        /// <param name="productGameId"></param>
        /// <returns></returns>
        public int DeleteCategory(int categoryId)
        {
            log.LogMethodEntry(categoryId);
            try
            {
                string deleteQuery = @"delete from category where CategoryId = @categoryId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@categoryId", categoryId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }

        /// <summary>
        ///  Inserts the record to the InventoryActivityLogDTO Table.
        /// </summary>
        /// <param name="categoryInventoryActivityLogDTO">inventoryActivityLogDTO object passed as the Parameter</param>
        /// <param name="loginId">login id of the user </param>
        /// <param name="siteId">site id of the user</param>
        /// <returns> returns the InventoryActivityLogDTO</returns>
        public void SaveInventoryActivityLog(CategoryDTO categoryInventoryActivityLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(categoryInventoryActivityLogDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[InventoryActivityLog]
                           (TimeStamp,
                            Message,
                            Guid,
                            site_id,
                            SourceTableName,
                            InvTableKey,
                            SourceSystemId,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (@TimeStamp,
                            @Message,
                            @Guid,
                            @site_id,
                            @SourceTableName,
                            @InvTableKey,
                            @SourceSystemId,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE()
                            )SELECT CAST(scope_identity() AS int)";

            try
            {
                List<SqlParameter> categoryInventoryActivityLogParameters = new List<SqlParameter>();
                categoryInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@InvTableKey", DBNull.Value));
                categoryInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@Message", "Category Inserted"));
                categoryInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@SourceSystemId", categoryInventoryActivityLogDTO.CategoryId.ToString() + ":" + categoryInventoryActivityLogDTO.Name));
                categoryInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@SourceTableName", "Category"));
                categoryInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@TimeStamp", ServerDateTime.Now));
                categoryInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
                categoryInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
                categoryInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", categoryInventoryActivityLogDTO.MasterEntityId, true));
                categoryInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
                categoryInventoryActivityLogParameters.Add(dataAccessHandler.GetSQLParameter("@Guid", categoryInventoryActivityLogDTO.Guid));
                log.Debug(categoryInventoryActivityLogParameters);

                object rowInserted = dataAccessHandler.executeScalar(query, categoryInventoryActivityLogParameters.ToArray(), sqlTransaction);
                log.LogMethodExit(rowInserted);

            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting InventoryActivityLog ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        internal DateTime? GetCategoryLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdateDate) LastUpdateDate 
                            FROM (
                            select max(LastUpdateDate) LastUpdateDate from Category WHERE (site_id = @siteId or @siteId = -1)
                            )a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdateDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdateDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}

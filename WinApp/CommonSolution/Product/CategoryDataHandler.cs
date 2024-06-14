/********************************************************************************************
 * Project Name - Category Data Handler
 * Description  - Data handler of the category data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Apr-2016   Krishnanand    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Category Data Handler - Handles insert, update and select of category data objects
    /// </summary>
    public class CategoryDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Dictionary<CategoryDTO.SearchByCategoryParameters, string> DBSearchParameters = new Dictionary<CategoryDTO.SearchByCategoryParameters, string>
        {
            {CategoryDTO.SearchByCategoryParameters.CATEGORY_ID, "CategoryId"},
            {CategoryDTO.SearchByCategoryParameters.NAME, "Name"},
            {CategoryDTO.SearchByCategoryParameters.IS_ACTIVE, "IsActive"}, 
            {CategoryDTO.SearchByCategoryParameters.SITE_ID, "Site_Id"},
            {CategoryDTO.SearchByCategoryParameters.MASTER_ENTITY_ID, "MasterEntityId"} //Added search parameter 16-May-2017
        };

         DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default Constructor of CategoryDataHandler class
        /// </summary>
        public CategoryDataHandler()
        {
            log.Debug("Starts-CategoryDataHandler() default constructor.");
            dataAccessHandler = new  DataAccessHandler();
            log.Debug("Ends-CategoryDataHandler() default constructor.");
        }

        /// <summary>
        /// Converts the Data row object to CategoryDTO class type
        /// </summary>
        /// <param name="categoryDataRow">CategoryDTO DataRow</param>
        /// <returns>Returns CategoryDTO</returns>
        private CategoryDTO GetCategoryDTO(DataRow categoryDataRow)        
        {
            log.Debug("Starts-GetCategoryDTO(categoryDataRow) Method.");
            CategoryDTO categoryDataObject = new CategoryDTO(
                                                Convert.ToInt32(categoryDataRow["CategoryId"]),
                                                categoryDataRow["ParentCategoryId"]== DBNull.Value ? -1 : Convert.ToInt32(categoryDataRow["ParentCategoryId"]),
                                                categoryDataRow["Name"].ToString(),
                                                categoryDataRow["IsActive"].ToString(),
                                                categoryDataRow["Lastupdated_userid"].ToString(),
                                                categoryDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(categoryDataRow["site_id"]),
                                                categoryDataRow["Guid"].ToString(),
                                                categoryDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(categoryDataRow["SynchStatus"]),
                                                categoryDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(categoryDataRow["MasterEntityId"])
                                            );

            log.Debug("Ends-GetCategoryDTO(categoryDataRow) Method.");
            return categoryDataObject;
        }

        /// <summary>
        /// Gets the Category data of all categories
        /// </summary>
        /// <returns>Returns CategoryDTO</returns>
        public CategoryDTO GetCategory()
        {
            log.Debug("Starts-GetCategory(CategoryId) Method.");
            string selectCategoryQuery = @"SELECT *
                                           FROM Category";
            //SqlParameter[] selectCategoryParameters = new SqlParameter[1];
            //selectCategoryParameters[0] = new SqlParameter("@categoryId", CategoryId);
            DataTable category = dataAccessHandler.executeSelectQuery(selectCategoryQuery, null);
            if (category.Rows.Count > 0)
            {
                DataRow categoryRow = category.Rows[0];
                CategoryDTO categoryDataObject = GetCategoryDTO(categoryRow);
                log.Debug("Ends-GetCategory(CategoryId) Method by returning categoryDataObject.");
                return categoryDataObject;
            }
            else
            {
                log.Debug("Ends-GetCategory(CategoryId) Method by returning null.");
                return null;
            }
        }
        /// <summary>
        /// Gets the Category data of passed Category id
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns>Returns CategoryDTO</returns>
        public CategoryDTO GetCategory(int CategoryId)
        {
            log.Debug("Starts-GetCategory(CategoryId) Method.");
            string selectCategoryQuery = @"SELECT *
                                           FROM Category
                                           WHERE CategoryId = @categoryId";
            SqlParameter[] selectCategoryParameters = new SqlParameter[1];
            selectCategoryParameters[0] = new SqlParameter("@categoryId", CategoryId);
            DataTable category = dataAccessHandler.executeSelectQuery(selectCategoryQuery, selectCategoryParameters);
            if (category.Rows.Count > 0)
            {
                DataRow categoryRow = category.Rows[0];
                CategoryDTO categoryDataObject = GetCategoryDTO(categoryRow);
                log.Debug("Ends-GetCategory(CategoryId) Method by returning categoryDataObject.");
                return categoryDataObject;
            }
            else
            {
                log.Debug("Ends-GetCategory(CategoryId) Method by returning null.");
                return null;
            }
        }
        /// <summary>
        /// Retriving category by passing query
        /// </summary>
        /// <param name="sqlQuery">Query passed for retriving the category</param>
        /// <returns> List of CategoryDTO </returns>
        public List<CategoryDTO> GetCategoryList(string sqlQuery)
        {
            log.Debug("Starts-GetCategory(sqlQuery) Method.");
            string Query=sqlQuery.ToUpper();
            if(Query.Contains("DROP")||Query.Contains("UPDATE")||Query.Contains("DELETE"))
            {
                log.Debug("Ends-GetCategory(sqlQuery) Method by invalid query.");
                return null;
            }
            DataTable categoryData = dataAccessHandler.executeSelectQuery(sqlQuery, null);
            if (categoryData.Rows.Count > 0)
            {
                List<CategoryDTO> categoryList = new List<CategoryDTO>();
                foreach (DataRow categoryDataRow in categoryData.Rows)
                {
                    CategoryDTO categoryDataObject = GetCategoryDTO(categoryDataRow);
                    categoryList.Add(categoryDataObject);
                }
                log.Debug("Ends-GetCategoryList(sqlQuery) Method by returning categoryList.");
                return categoryList; ;
            }
            else
            {
                log.Debug("Ends-GetCategory(sqlQuery) Method by returning null.");
                return null;
            }
        }

        /// <summary>
        ///  Returns the Category table columns
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategoryColumns()
        {
            string selectCategoryQuery = "SELECT columns FROM(SELECT '' AS columns UNION ALL SELECT COLUMN_NAME AS columns FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Category') a ORDER BY columns";
            DataTable categoryTableColumns = dataAccessHandler.executeSelectQuery(selectCategoryQuery, null);
            return categoryTableColumns;
        }
    
        /// <summary>
        /// Gets the CategoryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CategoryDTO matching the search criteria</returns>
        public List<CategoryDTO>GetCategoryList(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetCategoryList(searchParameters) Method.");
            int count = 0;
            string selectCategoryQuery = @"SELECT *
                                           FROM Category";
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CategoryDTO.SearchByCategoryParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)
                        {
                            if (searchParameter.Key.Equals(CategoryDTO.SearchByCategoryParameters.CATEGORY_ID))
                            {
                                query.Append(DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            //Added condition to include MasterEntityID column 16-May-2017
                            else if (searchParameter.Key.Equals(CategoryDTO.SearchByCategoryParameters.MASTER_ENTITY_ID))
                            {
                                query.Append("isnull("  + DBSearchParameters[searchParameter.Key] + ", '') = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key == CategoryDTO.SearchByCategoryParameters.SITE_ID)
                            {
                                query.Append(" (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else
                            {
                                query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ", '') = N'" + searchParameter.Value + "'");
                            }
                        }
                        else
                        {
                            if (searchParameter.Key.Equals(CategoryDTO.SearchByCategoryParameters.CATEGORY_ID))
                            {
                                query.Append(" and " + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            //Added condition to include MasterEntityID column 16-May-2017
                            else if (searchParameter.Key.Equals(CategoryDTO.SearchByCategoryParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(" and isnull(" + DBSearchParameters[searchParameter.Key] + ", '') = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key == CategoryDTO.SearchByCategoryParameters.SITE_ID)
                            {
                                query.Append(" and (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else
                            {
                                query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') = N'" + searchParameter.Value + "'");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetCategoryList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectCategoryQuery = selectCategoryQuery + query;
            }

            DataTable categoryData = dataAccessHandler.executeSelectQuery(selectCategoryQuery, null);
            if (categoryData.Rows.Count > 0)
            {
                List<CategoryDTO> categoryList = new List<CategoryDTO>();
                foreach (DataRow categoryDataRow in categoryData.Rows)
                {
                    CategoryDTO categoryDataObject = GetCategoryDTO(categoryDataRow);
                    categoryList.Add(categoryDataObject);
                }
                log.Debug("Ends-GetCategoryList(searchParameters) Method by returning categoryList.");
                return categoryList;
            }
            else
            {
                log.Debug("Ends-GetCategoryList(searchParameters) Method by returning null.");
                return null;
            }
        }

        /// <summary>
        /// InsertCategory
        /// </summary>
        /// <param name="category"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public int InsertCategory(CategoryDTO category, string userId, int siteId, SqlTransaction SQLTrx)
        {
            log.Debug("Starts-InsertCategory(category, userId, siteId) Method.");
            string insertCategoryQuery = @"insert into category (ParentCategoryId, 
                                                                 Name, 
                                                                 IsActive, 
                                                                 Lastupdated_userid, 
                                                                 site_id,
                                                                 GUID,
                                                                 SynchStatus,
                                                                 MasterEntityID) 
                                           Values(@ParentCategoryId,
                                                  @Name,
                                                  @IsActive,
                                                  @Lastupdated_userid, 
                                                  @site_id,
                                                  NewID(),
                                                  @SynchStatus,
                                                  @MasterEntityID
                                           )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateCategoryParameters = new List<SqlParameter>();
            if (category.ParentCategoryId == -1)
            {
                updateCategoryParameters.Add(new SqlParameter("@ParentCategoryId", DBNull.Value));
            }
            else
            {
                updateCategoryParameters.Add(new SqlParameter("@ParentCategoryId", category.ParentCategoryId));
            }
            if(string.IsNullOrEmpty(category.Name))
            {
                updateCategoryParameters.Add(new SqlParameter("@Name", DBNull.Value));
            }
            else
            {
                updateCategoryParameters.Add(new SqlParameter("@Name", category.Name));
            }
            updateCategoryParameters.Add(new SqlParameter("@IsActive", string.IsNullOrEmpty(category.IsActive) ? "N" : category.IsActive));
            updateCategoryParameters.Add(new SqlParameter("@Lastupdated_userid", userId));
            if (siteId == -1)
            {
                updateCategoryParameters.Add(new SqlParameter("@site_id", DBNull.Value));
            }
            else
            {
                updateCategoryParameters.Add(new SqlParameter("@site_id", siteId));
            }
            if (category.SynchStatus)
            {
                updateCategoryParameters.Add(new SqlParameter("@SynchStatus", category.SynchStatus));
            }
            else
            {
                updateCategoryParameters.Add(new SqlParameter("@SynchStatus", DBNull.Value));
            }
            if (category.MasterEntityId == -1)
            {
                updateCategoryParameters.Add(new SqlParameter("@MasterEntityID", DBNull.Value));
            }
            else
            {
                updateCategoryParameters.Add(new SqlParameter("@MasterEntityID", category.MasterEntityId));
            }
            
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertCategoryQuery, updateCategoryParameters.ToArray());
            log.Debug("Ends-InsertCategory(category, userId, siteId) Method.");
            return idOfRowInserted;
        }

        /// <summary>
        /// UpdateCategory
        /// </summary>
        /// <param name="category"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public int UpdateCategory(CategoryDTO category, string userId, int siteId, SqlTransaction SQLTrx)
        {
            log.Debug("Starts-UpdateCategory(category, userId, siteId) Method.");
            string updateCategoryQuery = @"update category
                                           set ParentCategoryId=@ParentCategoryId,
                                            Name=@Name,
                                            IsActive=@IsActive,
                                            Lastupdated_userid=@Lastupdated_userid,
                                            -- site_id = @site_id,
                                            SynchStatus = @SynchStatus,
                                            MasterEntityID = @MasterEntityID
                                          where CategoryId = @categoryId";
            List<SqlParameter> updateCategoryParameters = new List<SqlParameter>();
            updateCategoryParameters.Add(new SqlParameter("@categoryId", category.CategoryId));
            if (category.ParentCategoryId == -1)
            {
                updateCategoryParameters.Add(new SqlParameter("@ParentCategoryId", DBNull.Value));
            }
            else
            {
                updateCategoryParameters.Add(new SqlParameter("@ParentCategoryId", category.ParentCategoryId));
            }
            if (string.IsNullOrEmpty(category.Name))
            {
                updateCategoryParameters.Add(new SqlParameter("@Name", DBNull.Value));
            }
            else
            {
                updateCategoryParameters.Add(new SqlParameter("@Name", category.Name));
            }
            updateCategoryParameters.Add(new SqlParameter("@IsActive", string.IsNullOrEmpty(category.IsActive) ? "N" : category.IsActive));
            updateCategoryParameters.Add(new SqlParameter("@Lastupdated_userid", userId));

            if (siteId == -1)
            {
                updateCategoryParameters.Add(new SqlParameter("@site_id", DBNull.Value));
            }
            else
            {
                updateCategoryParameters.Add(new SqlParameter("@site_id", siteId));
            }
            if (category.SynchStatus)
            {
                updateCategoryParameters.Add(new SqlParameter("@SynchStatus", category.SynchStatus));
            }
            else
            {
                updateCategoryParameters.Add(new SqlParameter("@SynchStatus", DBNull.Value));
            }
            if (category.MasterEntityId == -1)
            {
                updateCategoryParameters.Add(new SqlParameter("@MasterEntityID", DBNull.Value));
            }
            else
            {
                updateCategoryParameters.Add(new SqlParameter("@MasterEntityID", category.MasterEntityId));
            }
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateCategoryQuery, updateCategoryParameters.ToArray());
            log.Debug("Ends-UpdateCategory(category, userId, siteId) Method.");
            return rowsUpdated;
        }
    }
}

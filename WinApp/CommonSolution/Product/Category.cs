using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Category
    /// </summary>
    public class Category
    {
        CategoryDTO categoryDTO;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Category
        /// </summary>
        public Category()
        {
            log.Debug("Starts-Category() default constructor.");
            categoryDTO = null;
            log.Debug("Ends-Category() default constructor.");
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="categoryDTO">Parameter of the type CategoryDTO</param>
        public Category(CategoryDTO categoryDTO)
        {
            log.Debug("Starts-Category(categoryDTO) parameterized constructor.");
            this.categoryDTO = categoryDTO;
            log.Debug("Ends-Category(categoryDTO) parameterized constructor.");
        }
        /// <summary>
        /// Constructor with the Category id as the parameter
        /// Would fetch the Category object based on the ID passed. 
        /// </summary>
        /// <param name="CategoryId">Category id</param>
        public Category(int CategoryId)
            : this()
        {
            log.Debug("Starts-Category(CategoryId) parameterized constructor.");
            CategoryDataHandler categoryDataHandler = new CategoryDataHandler();
            log.Debug("Ends-Category(CategoryId) parameterized constructor.");
        }

        /// <summary>
        /// Saves the Category
        /// UOM will be inserted if CategoryId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");

            Semnox.Core.Utilities.ExecutionContext UserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
            CategoryDataHandler categoryDataHandler = new CategoryDataHandler();
            if (categoryDTO.CategoryId < 0)
            {
                int CategoryId = categoryDataHandler.InsertCategory(categoryDTO, UserContext.GetUserId(), UserContext.GetSiteId(), null);
                categoryDTO.CategoryId = CategoryId;
            }
            else
            {
                if (categoryDTO.IsChanged == true)
                {
                    categoryDataHandler.UpdateCategory(categoryDTO, UserContext.GetUserId(), UserContext.GetSiteId(), null);
                    categoryDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }
    }

    /// <summary>
    ///  Manages the list of category
    /// </summary>
    public class CategoryList
    {
        //Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the Category list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CategoryDTO> GetAllCategory(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllCategory(searchParameters) method.");
            CategoryDataHandler autoPatchDepPlanDataHandler = new CategoryDataHandler();
            log.Debug("Ends-GetAllCategory(searchParameters) method by returning the result of CategoryDataHandler.GetCategoryList() call.");
            return autoPatchDepPlanDataHandler.GetCategoryList(searchParameters);
        }
        /// <summary>
        /// Retriving category by passing query
        /// </summary>
        /// <param name="sqlQuery">Query passed for retriving the category</param>
        /// <returns> List of CategoryDTO </returns>
        public List<CategoryDTO> GetCategoryList(string sqlQuery)
        {
            log.Debug("Starts-GetAllCategory(sqlQuery) method.");
            CategoryDataHandler autoPatchDepPlanDataHandler = new CategoryDataHandler();
            log.Debug("Ends-GetAllCategory(sqlQuery) method by returning the result of CategoryDataHandler.GetCategoryList(sqlQuery) call.");
            return autoPatchDepPlanDataHandler.GetCategoryList(sqlQuery);
        }

        /// <summary>
        /// Returns the column name list of the Category table.
        /// </summary>
        /// <returns></returns>
        public DataTable GetCategoryColumnsName()
        {
            CategoryDataHandler autoPatchDepPlanDataHandler = new CategoryDataHandler();
            return autoPatchDepPlanDataHandler.GetCategoryColumns();
        }
    }
}

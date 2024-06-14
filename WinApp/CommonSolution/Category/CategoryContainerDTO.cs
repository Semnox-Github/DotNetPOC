/********************************************************************************************
* Project Name - Category
* Description  - CategoryContainerDTO class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    20-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/

using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Category
{
    public class CategoryContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int categoryId;
        private string name;
        private List<int> childCategoryIdList = new List<int>();
        private List<int> parentCategoryIdList = new List<int>();

        public CategoryContainerDTO()
        {
            log.LogMethodEntry();

            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        ///
        public CategoryContainerDTO(int categoryIdPassed, string name) : this()
        {
            log.LogMethodEntry(categoryIdPassed, name);
            this.categoryId = categoryIdPassed;
            this.name = name;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int CategoryId { get { return categoryId; } set { categoryId = value; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// Get/Set method of the Child Category Id field
        /// </summary>
        public List<int> ChildCategoryIdList
        {
            get
            {
                return childCategoryIdList;
            }

            set
            {
                childCategoryIdList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Parent Category Id field
        /// </summary>
        public List<int> ParentCategoryIdList
        {
            get
            {
                return parentCategoryIdList;
            }

            set
            {
                parentCategoryIdList = value;
            }
        }
    }
}

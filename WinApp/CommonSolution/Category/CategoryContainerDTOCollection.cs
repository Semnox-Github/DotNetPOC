/********************************************************************************************
* Project Name - Category
* Description  - CategoryContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    20-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Category
{
    public class CategoryContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CategoryContainerDTO> categoryContainerDTOList;
        private string hash;

        public CategoryContainerDTOCollection()
        {
            log.LogMethodEntry();
            categoryContainerDTOList = new List<CategoryContainerDTO>();
            log.LogMethodExit();
        }
        public CategoryContainerDTOCollection(List<CategoryContainerDTO> categoryContainerDTOList)
        {
            log.LogMethodEntry(categoryContainerDTOList);
            this.categoryContainerDTOList = categoryContainerDTOList;
            if (categoryContainerDTOList == null)
            {
                categoryContainerDTOList = new List<CategoryContainerDTO>();
            }
            hash = new DtoListHash(categoryContainerDTOList);
            log.LogMethodExit();
        }

        public List<CategoryContainerDTO> CategoryContainerDTOList
        {
            get
            {
                return categoryContainerDTOList;
            }

            set
            {
                categoryContainerDTOList = value;
            }
        }

        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }
    }
}

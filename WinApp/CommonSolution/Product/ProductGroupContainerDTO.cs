/********************************************************************************************
* Project Name - Product
* Description  - Data transfer object of  ProductGroupContainer class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.170.0    07-Jul-2023        Lakshminarayana            Created 
********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Data transfer object of  ProductGroupContainer class 
    /// </summary>
    public class ProductGroupContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int id;
        private string name;
        private string guid;
        private List<ProductGroupMapContainerDTO> productGroupMapContainerDTOList = new List<ProductGroupMapContainerDTO>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductGroupContainerDTO()
        {
            log.LogMethodEntry();
            id = -1;
            guid = string.Empty;
            name = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ProductGroupContainerDTO(int id, string name, string guid)
        {
            log.LogMethodEntry(id, name, guid);
            this.id = id;
            this.name = name;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }

            set
            {
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set methods for productGroupMapContainerDTOList 
        /// </summary>
        public List<ProductGroupMapContainerDTO> ProductGroupMapContainerDTOList
        {
            get
            {
                return productGroupMapContainerDTOList;
            }

            set
            {
                productGroupMapContainerDTOList = value;
            }
        }
    }
}

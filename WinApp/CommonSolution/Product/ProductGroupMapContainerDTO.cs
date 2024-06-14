/********************************************************************************************
* Project Name - Product
* Description  - Data transfer object of  ProductGroupMapContainer class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.170.0      07-Jul-2023      Lakshminarayana            Created 
********************************************************************************************/

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Data transfer object of  ProductGroupMapContainer class 
    /// </summary>
    public class ProductGroupMapContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int id;
        private int productGroupId;
        private int productId;
        private int sortOrder;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductGroupMapContainerDTO()
        {
            log.LogMethodEntry();
            this.id = -1;
            this.productGroupId = -1;
            this.productId = -1;
            this.sortOrder = 0;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public ProductGroupMapContainerDTO(int id, int productGroupId, int productId, int sortOrder)
            : this()
        {
            log.LogMethodEntry(id, productGroupId, productId, sortOrder);
            this.id = id;
            this.productGroupId = productGroupId;
            this.productId = productId;
            this.sortOrder = sortOrder;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>]
        public int Id { get { return id; } set { id = value;  } }
        /// <summary>
        /// Get/Set method of the ProductGroupId field
        /// </summary>
        public int ProductGroupId { get { return productGroupId; } set { productGroupId = value;  } }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value;  } }

        /// <summary>
        /// Get/Set method of the SortOrder field
        /// </summary>
        public int SortOrder { get { return sortOrder; } set { sortOrder = value;  } }

    }
}

/********************************************************************************************
 * Project Name - Sale Group Product Map Container DTO
 * Description  - Data object of sale group product map DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.0     29-Dec-2021   Prajwal S     Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the sale group product map data object class. This acts as data holder for the sale group product map business object
    /// </summary>
    public class SaleGroupProductMapContainerDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int typeMapId;
        int saleGroupId;
        int productId;
        int displayOrder;
        string guid;

        /// <summary>
        /// Default Contructor
        /// </summary>
        public SaleGroupProductMapContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>        
        public SaleGroupProductMapContainerDTO(int typeMapId, int saleGroupId, int productId, int displayOrder, string guid)
        {
            log.LogMethodEntry(typeMapId, saleGroupId, productId, displayOrder, guid);
            this.typeMapId = typeMapId;
            this.saleGroupId = saleGroupId;
            this.productId = productId;
            this.displayOrder = displayOrder;
            this.guid = guid;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the TypeMapId field
        /// </summary>
        public int TypeMapId { get { return typeMapId; } set { typeMapId = value; } }

        /// <summary>
        /// Get/Set method of the SaleGroupId field
        /// </summary>
        public int SaleGroupId { get { return saleGroupId; } set { saleGroupId = value;} }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value;} }

        /// <summary>
        /// Get/Set method of the SquenceId field
        /// </summary>
        public int DisplayOrder { get { return displayOrder; } set { displayOrder = value;} }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value;} }
    }
}

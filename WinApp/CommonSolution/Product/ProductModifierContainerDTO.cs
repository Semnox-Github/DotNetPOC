/********************************************************************************************
 * Project Name - Products
 * Description  - ProductModifierContainerDTO class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By      Remarks          
 *********************************************************************************************
 2.110.0      02-Dec-2020       Deeksha          Created : POS UI Redesign with REST API
 *2.120.0     14-Sep-2021       Prajwal S        Modified : Added child Container
 ********************************************************************************************/
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class ProductModifierContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int productModifierId;
        private int categoryId;
        private int productId;
        private int modifierSetId;
        private string autoShowinPOS;
        private int sortOrder;
        private ModifierSetContainerDTO modifierSetContainerDTO;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductModifierContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ProductModifierContainerDTO(int productModifierId, int categoryId, int productId,
                                           int modifierSetId, string autoShowinPOS, int sortOrder)
            : this()
        {
            log.LogMethodEntry(productModifierId, categoryId, productId, modifierSetId, autoShowinPOS, 
                                sortOrder);
            this.productModifierId = productModifierId;
            this.categoryId = categoryId;
            this.productId = productId;
            this.modifierSetId = modifierSetId;
            this.autoShowinPOS = autoShowinPOS;
            this.sortOrder = sortOrder;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public ProductModifierContainerDTO(ProductModifierContainerDTO productModifierContainerDTO)
            : this(productModifierContainerDTO.productModifierId, productModifierContainerDTO.categoryId, productModifierContainerDTO.productId,
                                           productModifierContainerDTO.modifierSetId, productModifierContainerDTO.autoShowinPOS, productModifierContainerDTO.sortOrder)
        {
            log.LogMethodEntry(productModifierContainerDTO);
            if(productModifierContainerDTO.modifierSetContainerDTO != null)
            {
                modifierSetContainerDTO = new ModifierSetContainerDTO(productModifierContainerDTO.modifierSetContainerDTO);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Product Modifier Id field
        /// </summary>
        [DisplayName("Product Modifier Id")]
        [ReadOnly(true)]
        public int ProductModifierId { get { return productModifierId; } set { productModifierId = value; } }

        /// <summary>
        /// Get/Set method of the Category Id field
        /// </summary>
        [DisplayName("Category Id")]
        [ReadOnly(true)]
        public int CategoryId { get { return categoryId; } set { categoryId = value;} }

        /// <summary>
        /// Get/Set method of the Child Product Id field
        /// </summary>
        [DisplayName("Product Id")]
        [ReadOnly(true)]
        public int ProductId { get { return productId; } set { productId = value; } }

        /// <summary>
        /// Get/Set method of the Modifier Set Id field
        /// </summary>
        [DisplayName("Modifier Set Id")]
        public int ModifierSetId { get { return modifierSetId; } set { modifierSetId = value;} }

        /// <summary>
        /// Get/Set method of the Auto Show In POS field
        /// </summary>
        [DisplayName("AutoShowinPOS")]
        public string AutoShoInPOS { get { return autoShowinPOS; } set { autoShowinPOS = value;} }
        

        /// <summary>
        /// Get/Set method of the Sort Order field
        /// </summary>
        [DisplayName("Sort Order")]
        public int SortOrder { get { return sortOrder; } set { sortOrder = value;} }

        /// <summary>
        /// Get/Set method of the ModifierSetContainerDTO field
        /// </summary>
        public ModifierSetContainerDTO ModifierSetContainerDTO { get { return modifierSetContainerDTO; } set { modifierSetContainerDTO = value; } }

    }
}

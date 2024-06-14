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
 2.120.0      18-Jun-2021       Prajwal S        Modified : added extra fields.
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class ProductsDisplayGroupContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int id;
        private int displayGroupId; 
        private string displayGroup;
       
        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductsDisplayGroupContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ProductsDisplayGroupContainerDTO(int id, int displayGroupId, string displayGroup/*, string imageFileName, int sortOrder*/)
            : this()
        {
            log.LogMethodEntry(id, displayGroupId);
            this.id = id;
            this.displayGroupId = displayGroupId;
            this.displayGroup = displayGroup;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public ProductsDisplayGroupContainerDTO(ProductsDisplayGroupContainerDTO productsDisplayGroupContainerDTO)
            : this(productsDisplayGroupContainerDTO.id, productsDisplayGroupContainerDTO.displayGroupId, productsDisplayGroupContainerDTO. displayGroup)
        {
            log.LogMethodEntry(productsDisplayGroupContainerDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the  Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id { get { return id; } set { id = value; } }

        /// <summary>
        /// Get/Set method of the Display Group Id field
        /// </summary>
        [DisplayName("DisplayGroup Id")]
        [ReadOnly(true)]
        public int DisplayGroupId { get { return displayGroupId; } set { displayGroupId = value;} } 
        
        /// <summary>
        /// Get/Set method of the Display Group Id field
        /// </summary>
        [DisplayName("DisplayGroup")]
        [ReadOnly(true)]
        public string DisplayGroup { get { return displayGroup; } set { displayGroup = value;} } 
        
    }
}

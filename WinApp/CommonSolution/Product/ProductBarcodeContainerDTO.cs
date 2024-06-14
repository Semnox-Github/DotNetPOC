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
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class ProductBarcodeContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int id;
        private string barCode;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductBarcodeContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ProductBarcodeContainerDTO(int id, string barCode)
            : this()
        {
            log.LogMethodEntry(id, barCode);
            this.id = id;
            this.barCode = barCode;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor with required data fields
        /// </summary>
        public ProductBarcodeContainerDTO(ProductBarcodeContainerDTO productBarcodeContainerDTO)
            : this(productBarcodeContainerDTO.id, productBarcodeContainerDTO.barCode)
        {
            log.LogMethodEntry(productBarcodeContainerDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the  Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id { get { return id; } set { id = value; } }

        /// <summary>
        /// Get/Set method of the BarCode field
        /// </summary>
        [DisplayName("BarCode")]
        [ReadOnly(true)]
        public string BarCode { get { return barCode; } set { barCode = value;} }
    }
}

/********************************************************************************************
 * Project Name - POS
 * Description  - Data object of POSProductExclusionsContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021        Prajwal S          Created : Web Inventory UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class POSProductExclusionsContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int exclusionId;
        private int posMachineId;
        private string productGroup;
        private int productDisplayGroupFormatId;
        private int posTypeId;

        /// <summary>
        /// Default constructor 
        /// </summary>
        public POSProductExclusionsContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public POSProductExclusionsContainerDTO(int exclusionId, int posMachineId, string productGroup,  int posTypeId, int productDisplayGroupFormatId)
        {
            log.LogMethodEntry(exclusionId, posMachineId, productGroup, posTypeId, productDisplayGroupFormatId);
            this.exclusionId = exclusionId;
            this.posMachineId = posMachineId;
            this.posTypeId = posTypeId;
            this.productDisplayGroupFormatId = productDisplayGroupFormatId;
            this.productGroup = productGroup;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("SI#")]
        [ReadOnly(true)]
        public int ExclusionId
        {
            get
            {
                return exclusionId;
            }

            set
            {
               
                exclusionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("PosMachineId")]
        public int PosMachineId
        {
            get
            {
                return posMachineId;
            }

            set
            {
               
                posMachineId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PosTypeId field
        /// </summary>
        [DisplayName("PosTypeId")]
        public int PosTypeId
        {
            get
            {
                return posTypeId;
            }

            set
            {
               
                posTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProductDisplayGroupFormatId field
        /// </summary>
        [DisplayName("ProductDisplayGroupFormatId")]
        public int ProductDisplayGroupFormatId
        {
            get
            {
                return productDisplayGroupFormatId;
            }

            set
            {
               
                productDisplayGroupFormatId = value;
            }
        }


        /// <summary>
        /// Get/Set method of the ProductGroup field
        /// </summary>
        [DisplayName("ProductGroup")]
        public string ProductGroup
        {
            get
            {
                return productGroup;
            }

            set
            {
               
                productGroup = value;
            }
        }

    }
}

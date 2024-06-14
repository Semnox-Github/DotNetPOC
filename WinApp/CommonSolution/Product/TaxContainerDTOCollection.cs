/********************************************************************************************
* Project Name - Product
* Description  - TaxContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.150.00    18-Jan-2022       Prajwal S                Created
********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// DTO Collection class.
    /// </summary>
    public class TaxContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<TaxContainerDTO> taxContainerDTOList;
        private string hash;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TaxContainerDTOCollection()
        {
            log.LogMethodEntry();
            taxContainerDTOList = new List<TaxContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Contructor with taxContainerDTOList parameter, created hash.
        /// </summary>
        /// <param name="taxContainerDTOList"></param>
        public TaxContainerDTOCollection(List<TaxContainerDTO> taxContainerDTOList)
        {
            log.LogMethodEntry(taxContainerDTOList);
            this.taxContainerDTOList = taxContainerDTOList;
            if (taxContainerDTOList == null)
            {
                this.taxContainerDTOList = new List<TaxContainerDTO>();
            }
            hash = new DtoListHash(this.taxContainerDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method
        /// </summary>
        public List<TaxContainerDTO> TaxContainerDTOList
        {
            get
            {
                return taxContainerDTOList;
            }

            set
            {
                taxContainerDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set for hash.
        /// </summary>
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

/********************************************************************************************
 * Project Name - Discounts 
 * Description  - Data object of DiscountContainerDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 2.150.0      13-Apr-2021    Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// This is the collection of Discount data object class. 
    /// </summary>
    public class DiscountContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<DiscountContainerDTO> discountContainerDTOList;
        private string hash;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DiscountContainerDTOCollection()
        {
            log.LogMethodEntry();
            discountContainerDTOList = new List<DiscountContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public DiscountContainerDTOCollection(List<DiscountContainerDTO> discountContainerDTOList)
        {
            log.LogMethodEntry(discountContainerDTOList);
            this.discountContainerDTOList = discountContainerDTOList;
            if (this.discountContainerDTOList == null)
            {
                this.discountContainerDTOList = new List<DiscountContainerDTO>();
            }
            hash = new DtoListHash(discountContainerDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set methods for discountContainerDTOList field
        /// </summary>
        public List<DiscountContainerDTO> DiscountContainerDTOList
        {
            get
            {
                return discountContainerDTOList;
            }

            set
            {
                discountContainerDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set methods for hash field
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

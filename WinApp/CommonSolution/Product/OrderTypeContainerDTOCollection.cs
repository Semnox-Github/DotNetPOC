/********************************************************************************************
* Project Name - Product
* Description  - OrderTypeContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    19-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class OrderTypeContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<OrderTypeContainerDTO> orderTypeContainerDTOList;
        private string hash;

        public OrderTypeContainerDTOCollection()
        {
            log.LogMethodEntry();
            orderTypeContainerDTOList = new List<OrderTypeContainerDTO>();
            log.LogMethodExit();
        }
        public OrderTypeContainerDTOCollection(List<OrderTypeContainerDTO> orderTypeContainerDTOList)
        {
            log.LogMethodEntry(orderTypeContainerDTOList);
            this.orderTypeContainerDTOList = orderTypeContainerDTOList;
            if (orderTypeContainerDTOList == null)
            {
                orderTypeContainerDTOList = new List<OrderTypeContainerDTO>();
            }
            hash = new DtoListHash(orderTypeContainerDTOList);
            log.LogMethodExit();
        }
        
        public List<OrderTypeContainerDTO> OrderTypeContainerDTOList
        {
            get
            {
                return orderTypeContainerDTOList;
            }

            set
            {
                orderTypeContainerDTOList = value;
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

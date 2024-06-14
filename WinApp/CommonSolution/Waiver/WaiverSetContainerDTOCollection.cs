/********************************************************************************************
* Project Name - WaiverSet
* Description  - WaiverSetContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    20-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Waiver
{
    public class WaiverSetContainerDTOCollection
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<WaiverSetContainerDTO> waiverSetContainerDTOList;
        private string hash;

        public WaiverSetContainerDTOCollection()
        {
            log.LogMethodEntry();
            waiverSetContainerDTOList = new List<WaiverSetContainerDTO>();
            log.LogMethodExit();
        }
        public WaiverSetContainerDTOCollection(List<WaiverSetContainerDTO> waiverSetContainerDTOList)
        {
            log.LogMethodEntry(waiverSetContainerDTOList);
            this.waiverSetContainerDTOList = waiverSetContainerDTOList;
            if (waiverSetContainerDTOList == null)
            {
                waiverSetContainerDTOList = new List<WaiverSetContainerDTO>();
            }
            hash = new DtoListHash(waiverSetContainerDTOList);
            log.LogMethodExit();
        }

        public List<WaiverSetContainerDTO> WaiverSetContainerDTOList
        {
            get
            {
                return waiverSetContainerDTOList;
            }

            set
            {
                waiverSetContainerDTOList = value;
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

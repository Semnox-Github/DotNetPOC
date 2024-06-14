/********************************************************************************************
* Project Name - Game
* Description  - HubContainerDTOCollection Class.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     11-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Game
{
    public class HubContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<HubContainerDTO> hubContainerDTOList;
        private string hash;

        public HubContainerDTOCollection()
        {
            log.LogMethodEntry();
            hubContainerDTOList = new List<HubContainerDTO>();
            log.LogMethodExit();
        }

        public HubContainerDTOCollection(List<HubContainerDTO> hubContainerDTOList)
        {
            log.LogMethodEntry(HubContainerDTOList);
            this.hubContainerDTOList = hubContainerDTOList;
            if (hubContainerDTOList == null)
            {
                hubContainerDTOList = new List<HubContainerDTO>();
            }
            hash = new DtoListHash(hubContainerDTOList);
            log.LogMethodExit();
        }

        public List<HubContainerDTO> HubContainerDTOList
        {
            get
            {
                return hubContainerDTOList;
            }

            set
            {
                hubContainerDTOList = value;
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


    


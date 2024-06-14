/********************************************************************************************
 * Project Name -POS
 * Description  - POSMachineViewDTOCollection Data object of POSMachineDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        16-Sep-2020   Girish Kundar          Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.POS
{
    public class POSMachineContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<POSMachineContainerDTO> pOSMachineContainerDTOList;
        private string hash;

        public POSMachineContainerDTOCollection()
        {
            log.LogMethodEntry();
            pOSMachineContainerDTOList = new List<POSMachineContainerDTO>();
            log.LogMethodExit();
        }

        public POSMachineContainerDTOCollection(List<POSMachineContainerDTO> pOSMachineContainerDTOList)
        {
            log.LogMethodEntry(pOSMachineContainerDTOList);
            this.pOSMachineContainerDTOList = pOSMachineContainerDTOList;
            if (pOSMachineContainerDTOList == null)
            {
                pOSMachineContainerDTOList = new List<POSMachineContainerDTO>();
            }
            hash = new DtoListHash(pOSMachineContainerDTOList);
            log.LogMethodExit();
        }

        public List<POSMachineContainerDTO> POSMachineContainerDTOList
        {
            get
            {
                return pOSMachineContainerDTOList;
            }

            set
            {
                pOSMachineContainerDTOList = value;
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

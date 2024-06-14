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
    public class POSMachineViewDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<POSMachineViewDTO> posMachineViewDTOList;
        private string hash;

        public POSMachineViewDTOCollection()
        {
            log.LogMethodEntry();
            posMachineViewDTOList = new List<POSMachineViewDTO>();
            log.LogMethodExit();
        }

        public POSMachineViewDTOCollection(List<POSMachineViewDTO> posMachineViewDTOList)
        {
            log.LogMethodEntry(posMachineViewDTOList);
            this.posMachineViewDTOList = posMachineViewDTOList;
            if (posMachineViewDTOList == null)
            {
                posMachineViewDTOList = new List<POSMachineViewDTO>();
            }
            hash = new DtoListHash(posMachineViewDTOList);
            log.LogMethodExit();
        }

        public List<POSMachineViewDTO> POSMachineViewDTOList
        {
            get
            {
                return posMachineViewDTOList;
            }

            set
            {
                posMachineViewDTOList = value;
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

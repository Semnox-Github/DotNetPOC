/********************************************************************************************
* Project Name - Game
* Description  - MachineContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.110.0        09-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.Game
{
    public class MachineContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MachineContainerDTO> machineContainerDTOList;
        private string hash;

        public MachineContainerDTOCollection()
        {
            log.LogMethodEntry();
            machineContainerDTOList = new List<MachineContainerDTO>();
            log.LogMethodExit();
        }

        public MachineContainerDTOCollection(List<MachineContainerDTO> machineContainerDTOList)
        {
            log.LogMethodEntry(machineContainerDTOList);
            this.machineContainerDTOList = machineContainerDTOList;
            if (MachineContainerDTOList == null)
            {
                machineContainerDTOList = new List<MachineContainerDTO>();
            }
            hash = new DtoListHash(machineContainerDTOList);
            log.LogMethodExit();
        }

        public List<MachineContainerDTO> MachineContainerDTOList
        {
            get
            {
                return machineContainerDTOList;
            }

            set
            {
                machineContainerDTOList = value;
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

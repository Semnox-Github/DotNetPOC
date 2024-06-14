/********************************************************************************************
 * Project Name - Utilities 
 * Description  - Data object of SystemOptionContainerDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System.Collections.Generic;
using System.Linq;
namespace Semnox.Core.Utilities
{
    public class SystemOptionContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<SystemOptionContainerDTO> systemOptionContainerDTOList;
        private string hash;

        public SystemOptionContainerDTOCollection()
        {
            log.LogMethodEntry();
            systemOptionContainerDTOList = new List<SystemOptionContainerDTO>();
            log.LogMethodExit();
        }

        public SystemOptionContainerDTOCollection(List<SystemOptionContainerDTO> systemOptionContainerDTOList)
        {
            log.LogMethodEntry(systemOptionContainerDTOList);
            this.systemOptionContainerDTOList = systemOptionContainerDTOList;
            if (this.systemOptionContainerDTOList == null)
            {
                this.systemOptionContainerDTOList = new List<SystemOptionContainerDTO>();
            }
            hash = new DtoListHash(systemOptionContainerDTOList);
            log.LogMethodExit();
        }

        public List<SystemOptionContainerDTO> SystemOptionContainerDTOList
        {
            get
            {
                return systemOptionContainerDTOList;
            }

            set
            {
                systemOptionContainerDTOList = value;
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

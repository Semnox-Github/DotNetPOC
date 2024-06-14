using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class LookupsContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LookupsContainerDTO> lookupContainerDTOList;
        private string hash;

        public LookupsContainerDTOCollection()
        {
            log.LogMethodEntry();
            lookupContainerDTOList = new List<LookupsContainerDTO>();
            log.LogMethodExit();
        }

        public LookupsContainerDTOCollection(List<LookupsContainerDTO> lookupContainerDTOList)
        {
            log.LogMethodEntry(LookupsContainerDTOList);
            this.lookupContainerDTOList = lookupContainerDTOList;
            if (lookupContainerDTOList == null)
            {
                lookupContainerDTOList = new List<LookupsContainerDTO>();
            }
            hash = new DtoListHash(lookupContainerDTOList);
            log.LogMethodExit();
        }

        public List<LookupsContainerDTO> LookupsContainerDTOList
        {
            get
            {
                return lookupContainerDTOList;
            }

            set
            {
                lookupContainerDTOList = value;
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


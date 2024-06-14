using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    public class ThemeContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ThemeContainerDTO> themeContainerDTOList;
        private string hash;

        public ThemeContainerDTOCollection()
        {
            log.LogMethodEntry();
            themeContainerDTOList = new List<ThemeContainerDTO>();
            log.LogMethodExit();
        }

        public ThemeContainerDTOCollection(List<ThemeContainerDTO> themeContainerDTOList)
        {
            log.LogMethodEntry(ThemeContainerDTOList);
            this.themeContainerDTOList = themeContainerDTOList;
            if (themeContainerDTOList == null)
            {
                themeContainerDTOList = new List<ThemeContainerDTO>();
            }
            hash = new DtoListHash(themeContainerDTOList);
            log.LogMethodExit();
        }
    
        public List<ThemeContainerDTO> ThemeContainerDTOList
        {
            get
            {
                return themeContainerDTOList;
            }

            set
            {
                themeContainerDTOList = value;
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


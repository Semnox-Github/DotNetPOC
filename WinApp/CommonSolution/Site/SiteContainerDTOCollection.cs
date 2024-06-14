/********************************************************************************************
 * Project Name - Site
 * Description  - SiteContainerDTOCollection class
 *
 **************
 ** Version Log
  **************
  * Version     Date Modified By Remarks
 *********************************************************************************************
 0.0         10-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Site
{
    public class SiteContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<SiteContainerDTO> siteContainerDTOList;
        private string hash;

        public SiteContainerDTOCollection()
        {
            log.LogMethodEntry();
            siteContainerDTOList = new List<SiteContainerDTO>();
            log.LogMethodExit();
        }

        public SiteContainerDTOCollection(List<SiteContainerDTO> siteContainerDTOList)
        {
            log.LogMethodEntry(siteContainerDTOList);
            this.siteContainerDTOList = siteContainerDTOList;
            if (siteContainerDTOList == null)
            {
                siteContainerDTOList = new List<SiteContainerDTO>();
            }
            hash = new DtoListHash(siteContainerDTOList);
            log.LogMethodExit();
        }

        public List<SiteContainerDTO> SiteContainerDTOList
        {
            get
            {
                return siteContainerDTOList;
            }

            set
            {
                siteContainerDTOList = value;
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

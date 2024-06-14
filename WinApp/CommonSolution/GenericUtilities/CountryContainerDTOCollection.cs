/********************************************************************************************
* Project Name - GenericUtilities
* Description  - CountryContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    08-Jul-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class CountryContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CountryContainerDTO> countryContainerDTOList;
        private string hash;

        public CountryContainerDTOCollection()
        {
            log.LogMethodEntry();
            countryContainerDTOList = new List<CountryContainerDTO>();
            log.LogMethodExit();
        }
        public CountryContainerDTOCollection(List<CountryContainerDTO> countryContainerDTOList)
        {
            log.LogMethodEntry(countryContainerDTOList);
            this.countryContainerDTOList = countryContainerDTOList;
            if (countryContainerDTOList == null)
            {
                countryContainerDTOList = new List<CountryContainerDTO>();
            }
            hash = new DtoListHash(countryContainerDTOList);
            log.LogMethodExit();
        }

        public List<CountryContainerDTO> CountryContainerDTOList
        {
            get
            {
                return countryContainerDTOList;
            }

            set
            {
                countryContainerDTOList = value;
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

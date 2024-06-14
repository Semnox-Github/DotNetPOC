/********************************************************************************************
* Project Name - GenericUtilities
* Description  - CountryContainer class 
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class CountryContainer: BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<CountryDTO> countryDTOList;
        private readonly DateTime? countryLastUpdateTime;
        private readonly int siteId;
        private readonly ConcurrentDictionary<int, CountryDTO> countryDTODictionary;

        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        internal CountryContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            countryDTODictionary = new ConcurrentDictionary<int, CountryDTO>();
            countryDTOList = new List<CountryDTO>();
            CountryList countryList = new CountryList();
            countryLastUpdateTime = countryList.GetCountryLastUpdateTime(siteId);

            CountryParams countryParams = new CountryParams();
            countryParams.SiteId = siteId;
            countryParams.ShowState = true;
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STATE_LOOKUP_FOR_COUNTRY") != "-1")
            {
                countryParams.ShowLookupCountry = true;
            }
            countryDTOList = countryList.GetCountryList(countryParams); if (countryDTOList != null && countryDTOList.Any())
            {
                foreach (CountryDTO countryDTO in countryDTOList)
                {
                    countryDTODictionary[countryDTO.CountryId] = countryDTO;
                }
            }
            else
            {
                countryDTOList = new List<CountryDTO>();
                countryDTODictionary = new ConcurrentDictionary<int, CountryDTO>();
            }
            log.LogMethodExit();
        }
        public List<CountryContainerDTO> GetCountryContainerDTOList()
        {
            log.LogMethodEntry();
            List<CountryContainerDTO> countryContainerDTOList = new List<CountryContainerDTO>();
            foreach (CountryDTO countryDTO in countryDTOList)
            {

                CountryContainerDTO countryContainerDTO = new CountryContainerDTO(countryDTO.CountryId, countryDTO.CountryName, countryDTO.CountryCode, countryDTO.IsActive);
                if(countryDTO.StateList != null && countryDTO.StateList.Any())
                {
                    foreach (StateDTO stateDTO in countryDTO.StateList)
                    {
                        countryContainerDTO.StateContainerDTOList.Add(new StateContainerDTO(stateDTO.StateId, stateDTO.State, stateDTO.Description, stateDTO.CountryId));
                    }
                }
                countryContainerDTOList.Add(countryContainerDTO);
            }
            log.LogMethodExit(countryContainerDTOList);
            return countryContainerDTOList;
        }
        public CountryContainer Refresh()
        {
            log.LogMethodEntry();
            CountryList countryList = new CountryList();
            DateTime? updateTime = countryList.GetCountryLastUpdateTime(siteId);
            if (countryLastUpdateTime.HasValue
                && countryLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Country since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            CountryContainer result = new CountryContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}

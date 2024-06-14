/********************************************************************************************
* Project Name - GenericUtilities
* Description  - Specification of the ICountryUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   11-May-2021   Roshan Devadiga          Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
  public interface ICountryUseCases
    {
        Task <List<CountryDTO>> GetCountries(CountryParams countryParams);
        Task<string> SaveCountries(List<CountryDTO> countryDTOList);
        Task<String> Delete(List<CountryDTO> countryDTOList);
        Task<CountryContainerDTOCollection> GetCountryContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}

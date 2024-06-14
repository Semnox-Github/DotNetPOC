/********************************************************************************************
 * Project Name - Country List Programs 
 * Description  - Data object of the CountryList
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *1.00        20-April-2017   Rakshith           Created 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Semnox.Core.GenericUtilities
{
    public class StateList
    {
         Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///Takes LookupParams as parameter
        /// </summary>
        /// <returns>Returns List<KeyValuePair<StateDTO.SearchByParameters, string>> by converting stateParams</returns>
        public List<KeyValuePair<StateDTO.SearchByParameters, string>> BuildStateSearchParametersList(StateParams stateParams)
        {
            log.Debug("Starts-BuildStateSearchParametersList Method");
            List<KeyValuePair<StateDTO.SearchByParameters, string>> customerSearchParams = new List<KeyValuePair<StateDTO.SearchByParameters, string>>();
            if (stateParams != null)
            {
                if (stateParams.StateId > 0)
                    customerSearchParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.COUNTRY_ID, stateParams.StateId.ToString()));
                if (!string.IsNullOrEmpty(stateParams.State))
                    customerSearchParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.SITE_ID, stateParams.State));
                if (stateParams.CountryId > 0)
                    customerSearchParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.COUNTRY_ID, stateParams.CountryId.ToString()));
                if (stateParams.SiteId > 0)
                    customerSearchParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.SITE_ID, stateParams.SiteId.ToString()));
            }
            log.Debug("Starts-BuildStateSearchParametersList Method");

            return customerSearchParams;
        }



        /// <summary>
        /// Returns the List of StateDTO
        /// </summary>
        public List<StateDTO> GetStateList(StateParams stateParams)
        {
            log.Debug("Starts-GetStateList(searchParameters) method.");
            List<KeyValuePair<StateDTO.SearchByParameters, string>> countrySearchParams = BuildStateSearchParametersList(stateParams);
            StateDataHandler stateDatahandler = new StateDataHandler();
            log.Debug("Ends-GetStateList(searchParameters) method .");
            return stateDatahandler.GetStateDTOList(countrySearchParams);

        }
    }
}

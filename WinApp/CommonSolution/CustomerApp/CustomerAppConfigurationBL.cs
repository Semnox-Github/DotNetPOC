/********************************************************************************************
 * Project Name - Customer App Configuration                                                                     
 * Description  - BL for Customer App configuration
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-May-2019   Nitin Pai            Created for Guest app
 *2.110        10-Feb-2021   Nitin Pai            Externalization enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.WebCMS;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.CustomerApp
{
    public class CustomerAppConfigurationBL
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        CustomerAppConfigurationDTO customerAppConfigurationDTO;

        public CustomerAppConfigurationBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            this.customerAppConfigurationDTO = null;
            log.LogMethodExit();
        }

        public CustomerAppConfigurationBL(ExecutionContext executionContext, int siteId, CustomerAppConfigurationDTO customerAppConfigurationDTO)
           : this(executionContext)
        {
            log.LogMethodEntry();
            this.customerAppConfigurationDTO = customerAppConfigurationDTO;
            log.LogMethodExit();
        }

        public CustomerAppConfigurationBL(ExecutionContext executionContext, int siteId)
           : this(executionContext)
        {
            log.LogMethodEntry();
            CustomerAppConfigurationDataHandler dataHandler = new CustomerAppConfigurationDataHandler();
            this.customerAppConfigurationDTO = dataHandler.GetCustomerAppConfiguration(siteId, this.executionContext);
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SELFSERVICEAPP_CUSTOMLINKS"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, siteId.ToString()));
            this.customerAppConfigurationDTO.CustomLinks = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);

            List<KeyValuePair<CMSSocialLinksDTO.SearchByRequestParameters, string>> socialParams = new List<KeyValuePair<CMSSocialLinksDTO.SearchByRequestParameters, string>>();
            socialParams.Add(new KeyValuePair<CMSSocialLinksDTO.SearchByRequestParameters, string>(CMSSocialLinksDTO.SearchByRequestParameters.ACTIVE, "1"));
            CMSSocialLinksList cmsSocialLinksList = new CMSSocialLinksList();
            this.customerAppConfigurationDTO.CMSSocialLinksDTO = cmsSocialLinksList.GetAllCmsSocialLinks(socialParams);
            this.customerAppConfigurationDTO.CMSSocialLinksDTO = this.customerAppConfigurationDTO.CMSSocialLinksDTO.Where(x =>x.Site_id == -1 || x.Site_id == executionContext.GetSiteId()).ToList();


            log.LogVariableState("ConfigurationDTO",customerAppConfigurationDTO.ToString());
            log.LogMethodExit();
        }

        public CustomerAppConfigurationDTO GetConfigurationDTO() { return this.customerAppConfigurationDTO; }
    }
}

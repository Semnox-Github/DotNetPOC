/********************************************************************************************
 * Project Name - Device  
 * Description  - RemoteEnabledAttributesUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      20-Aug-2021      Fiona           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Semnox.Core.Utilities;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class RemoteEnabledAttributesUseCases : RemoteUseCases, IEnabledAttributesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ENABLED_ATTRIBUTES_CONTAINER_URL = "api/TableAttributeSetUp/EnabledAttributesContainer";
        private const string ENABLED_ATTRIBUTES_URL = "api/TableAttributeSetUp/EnabledAttributes";

        public RemoteEnabledAttributesUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<EnabledAttributesContainerDTOCollection> GetEnabledAttributesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            EnabledAttributesContainerDTOCollection result = await Get<EnabledAttributesContainerDTOCollection>(ENABLED_ATTRIBUTES_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// SaveEnabledAttributes
        /// </summary>
        /// <param name="enabledAttributesDTOList"></param>
        /// <returns></returns>
        public async Task<string> SaveEnabledAttributes(List<EnabledAttributesDTO> enabledAttributesDTOList)
        {
            log.LogMethodEntry(enabledAttributesDTOList);
            try
            {
                string responseString = await Post<string>(ENABLED_ATTRIBUTES_URL, enabledAttributesDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<EnabledAttributesDTO>> GetEnabledAttributes(List<KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<EnabledAttributesDTO> result = await Get<List<EnabledAttributesDTO>>(ENABLED_ATTRIBUTES_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private IEnumerable<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<EnabledAttributesDTO.SearchByParameters, string> searchParameter in parameters)
            {
                switch (searchParameter.Key)
                {
                    case EnabledAttributesDTO.SearchByParameters.ENABLED_ATTRIBUTE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("enabledAttributeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case EnabledAttributesDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case EnabledAttributesDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
    }
}

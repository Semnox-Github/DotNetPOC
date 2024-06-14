/********************************************************************************************
 * Project Name -CustomAttributes
 * Description  -RemoteCustomAttributesUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         12-May-2021       B Mahesh Pai       Created
 2.130.0         27-Jul-2021       Mushahid Faizan    Modified :- POS UI redesign changes.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    class RemoteCustomAttributesUseCases: RemoteUseCases, ICustomAttributesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CUSTOMATTRIBUTE_URL = "api/Common/CustomAttributes";
        private const string CUSTOM_ATTRIBUTE_CONTAINER_URL = "api/Common/CustomAttributesContainer";
        public RemoteCustomAttributesUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<CustomAttributesDTO>> GetCustomAttributes(List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters,
         bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<CustomAttributesDTO> result = await Get<List<CustomAttributesDTO>>(CUSTOMATTRIBUTE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CustomAttributesDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case CustomAttributesDTO.SearchByParameters.ACCESS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Access".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomAttributesDTO.SearchByParameters.APPLICABILITY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Applicanbility".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomAttributesDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CustomArrtibuteId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomAttributesDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomAttributesDTO.SearchByParameters.NAME_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("NameList".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomAttributesDTO.SearchByParameters.NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Name".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomAttributesDTO.SearchByParameters.TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Type".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveCustomAttributes(List<CustomAttributesDTO> customAttributesDTOList)
        {
            log.LogMethodEntry(customAttributesDTOList);
            try
            {
                string responseString = await Post<string>(CUSTOMATTRIBUTE_URL, customAttributesDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<CustomAttributeContainerDTOCollection> GetCustomAttributesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            CustomAttributeContainerDTOCollection result = await Get<CustomAttributeContainerDTOCollection>(CUSTOM_ATTRIBUTE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}

    

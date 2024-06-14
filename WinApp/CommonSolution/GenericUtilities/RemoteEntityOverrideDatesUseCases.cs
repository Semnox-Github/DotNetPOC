/********************************************************************************************
* Project Name - User
* Description  - RemoteEntityOverrideDatesUseCases class
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    08-Apr-2021      B Mahesh Pai        Created : POS UI Redesign with REST API
********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class RemoteEntityOverrideDatesUseCases : RemoteUseCases, IEntityOverrideDatesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ENTITYOVERRIDEDATES_URL = "api/Product/ProductEntityExclusions";
        

        public RemoteEntityOverrideDatesUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<EntityOverrideDatesDTO>> GetEntityOverrideDates(List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>>
                         parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<EntityOverrideDatesDTO> result = await Get<List<EntityOverrideDatesDTO>>(ENTITYOVERRIDEDATES_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case EntityOverrideDatesDTO.SearchByEntityOverrideParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("entityName".ToString(), searchParameter.Value));
                        }
                        break;
                    case EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_GUID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("entityGUID".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveEntityOverrideDates(List<EntityOverrideDatesDTO> entityOverrideDatesDTOList)
        {
            log.LogMethodEntry(entityOverrideDatesDTOList);
            try
            {
                string responseString = await Post<string>(ENTITYOVERRIDEDATES_URL, entityOverrideDatesDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<EntityOverrideDatesDTO> entityOverrideDatesDTOList)
        {
            try
            {
                log.LogMethodEntry(entityOverrideDatesDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(entityOverrideDatesDTOList);
                string responseString = await Delete(ENTITYOVERRIDEDATES_URL, content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return response;
            }
            catch (WebApiException wex)
            {
                log.Error(wex);
                throw;
            }
        }

    }
}

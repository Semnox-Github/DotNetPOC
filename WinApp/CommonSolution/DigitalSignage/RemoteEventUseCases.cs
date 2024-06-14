/********************************************************************************************
 * Project Name -DigitalSignage
 * Description  - DepartmentUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00    21-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    class RemoteEventUseCases:RemoteUseCases,IEventUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string EVENT_URL = "api/DigitalSignage/Events";
       
        public RemoteEventUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<EventDTO>> GetEvents(List<KeyValuePair<EventDTO.SearchByParameters, string>>
                       searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<EventDTO> result = await Get<List<EventDTO>>(EVENT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<EventDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<EventDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case EventDTO.SearchByParameters.ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("id".ToString(), searchParameter.Value));
                        }
                        break;
                    case EventDTO.SearchByParameters.TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("typeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case EventDTO.SearchByParameters.NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("name".ToString(), searchParameter.Value));
                        }
                        break;
                    case EventDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveEvents(List<EventDTO> eventDTOList)
        {
            log.LogMethodEntry(eventDTOList);
            try
            {
                string responseString = await Post<string>(EVENT_URL, eventDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

    }
}

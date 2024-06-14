/********************************************************************************************
* Project Name - Game
* Description  - Remote UseCase for GenericCalendar Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     11-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    class RemoteGenericCalendarUseCases : RemoteUseCases, IGenericCalendarUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GenericCalendar_URL = "api/Game/GenericCalendar";

        public RemoteGenericCalendarUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<GenericCalendarDTO>> GetGenericCalendars(List<KeyValuePair<GenericCalendarDTO.SearchByGenericCalendarParameters, string>>
                           parameters, string genericColId = "", string moduleName = "", int entityId = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("genericColId".ToString(), genericColId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("moduleName".ToString(), moduleName.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("entityId".ToString(), entityId.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<GenericCalendarDTO> result = await Get<List<GenericCalendarDTO>>(GenericCalendar_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<GenericCalendarDTO.SearchByGenericCalendarParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<GenericCalendarDTO.SearchByGenericCalendarParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case GenericCalendarDTO.SearchByGenericCalendarParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                        //case GenericCalendarDTO.SearchByGenericCalendarParameters.CALENDAR_ID:
                        //    {
                        //        searchParameterList.Add(new KeyValuePair<string, string>("calendarId".ToString(), searchParameter.Value));
                        //    }
                        //    break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveGenericCalendars(List<GenericCalendarDTO> genericCalendarDTOList)
        {
            log.LogMethodEntry(genericCalendarDTOList);
            try
            {
                string responseString = await Post<string>(GenericCalendar_URL, genericCalendarDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public Task<int> GetGenericCalendarCount(List<KeyValuePair<GenericCalendarDTO.SearchByGenericCalendarParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            throw new NotImplementedException();
        }
    }
}

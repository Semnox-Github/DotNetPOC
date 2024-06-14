/********************************************************************************************
 * Project Name - Redemption 
 * Description  - RemoteTicketStationUseCases class to get the data 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     21-Dec-2020      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RemoteTicketStationUseCases : RemoteUseCases, ITicketStationUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string TICKET_STATION_URL = "api/Device/TicketStations";
        private const string TICKET_STATION_CONTAINER_URL = "api/Device/TicketStationContainer";

        public RemoteTicketStationUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<TicketStationDTO>> GetTicketStations(List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>> parameters,
                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<TicketStationDTO> result = await Get<List<TicketStationDTO>>(TICKET_STATION_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>> TicketStationSearchParams)
        {
            log.LogMethodEntry(TicketStationSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string> searchParameter in TicketStationSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case TicketStationDTO.SearchByTicketStationParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case TicketStationDTO.SearchByTicketStationParameters.TICKET_STATION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("TicketStationId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TicketStationDTO.SearchByTicketStationParameters.ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("StationId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveTicketStations(List<TicketStationDTO> ticketStationDTOList)
        {
            log.LogMethodEntry(ticketStationDTOList);
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(ticketStationDTOList);
                string responseString = await Post<string>(TICKET_STATION_URL, content);
                //dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<TicketStationContainerDTOCollection> GetTicketStationContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            TicketStationContainerDTOCollection result = await Get<TicketStationContainerDTOCollection>(TICKET_STATION_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}

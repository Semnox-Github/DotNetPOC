/********************************************************************************************
 * Project Name - Redemption  
 * Description  - ITicketStationUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      21-Dec-2020      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Redemption
{
    public interface ITicketStationUseCases
    {
        Task<List<TicketStationDTO>> GetTicketStations(List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>> parameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveTicketStations(List<TicketStationDTO> ticketStationDTOList);
        Task<TicketStationContainerDTOCollection> GetTicketStationContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}

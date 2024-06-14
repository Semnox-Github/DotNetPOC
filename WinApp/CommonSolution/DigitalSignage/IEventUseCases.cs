/********************************************************************************************
* Project Name - DigitalSignage
* Description  - Specification of the Event use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.140.00   21-Apr-2021   Roshan Devadiga        Created 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    public interface IEventUseCases
    {
        Task<List<EventDTO>> GetEvents(List<KeyValuePair<EventDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveEvents(List<EventDTO> eventDTOList);

    }
}

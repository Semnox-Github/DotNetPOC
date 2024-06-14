/********************************************************************************************
* Project Name - Communication
* Description  - Specification of the IMessagingTriggerUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   05-May-2021   Roshan Devadiga          Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// IMessagingTriggerUseCases
    /// </summary>
    public interface IMessagingTriggerUseCases
    {
        Task<List<MessagingTriggerDTO>> GetMessagingTrigges(List<KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>> searchParameters,
                                           bool loadChildRecords = false, bool activeChildRecords = true,
                                           SqlTransaction sqlTransaction = null);
        Task<String> SaveMessagingTrigges(List<MessagingTriggerDTO> messagingTriggerDTOList);
        Task<String> Delete(List<MessagingTriggerDTO> messagingTriggerDTOList);
    }
}

/********************************************************************************************
* Project Name - GenericUtitlities
* Description  - Interface for ParafaitMessageQueue Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.120.0     10-Mar-2020      Prajwal S             Created : urban Pipers changes
*2.130.0     08-Feb-2022      Fiona Lishal          Added GetParafaitMessageQueueDTOList
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.GenericUtilities
{
    public interface IParafaitMessageQueueUseCases
    {
        Task<List<ParafaitMessageQueueDTO>> GetParafaitMessageQueue(List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<ParafaitMessageQueueDTO>> SaveParafaitMessageQueue(List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList);
        Task<List<ParafaitMessageQueueDTO>> GetParafaitMessageQueueDTOList(List<string> entityGuids, List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> searchParameters);
    }
}

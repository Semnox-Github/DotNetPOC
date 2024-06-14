/********************************************************************************************
* Project Name - DigitalSignage
* Description  - Specification of the Ticker use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.140.00   22-Apr-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    public interface ITickerUseCases
    {
        Task<List<TickerDTO>> GetTickers(List<KeyValuePair<TickerDTO.SearchByParameters, string>> searchParameters);
        Task<string> SaveTickers(List<TickerDTO> tickerDTOList);
    }
}

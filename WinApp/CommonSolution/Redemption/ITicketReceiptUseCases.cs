/********************************************************************************************
 * Project Name - RedemptionUtils
 * Description  - ITicketReceiptUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          26-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Redemption
{
    public interface ITicketReceiptUseCases
    {
        Task<List<TicketReceiptDTO>> GetTicketReceipts(List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> parameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null);
        Task<string> UpdateTicketReceipts(List<TicketReceiptDTO> ticketReceiptDTOList);
        Task PerDayTicketLimitCheck(int manualTicket);
        Task<int> ValidateTicketReceipts(string barcode, SqlTransaction sqlTransaction = null);
    }
}

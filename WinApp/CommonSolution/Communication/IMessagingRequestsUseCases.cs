/********************************************************************************************
 * Project Name - Communication
 * Description  - IMessagingRequestsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By            Remarks          
 *********************************************************************************************
 2.150.01     24-Jan-2023      Yashodhara C H          Create
 ********************************************************************************************/
using System;
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// IMessagingRequestsUseCases
    /// </summary>
    public interface IMessagingRequestsUseCases
    {
        Task<string> SaveMessagingRequestDTO(string idList);

        Task<List<MessagingRequestSummaryViewDTO>> GetMessagingRequestSummaryViewDTOList(int messageId = -1, int parentAndChildMessagesById = -1, string messageIdList = null, int customerId = -1, int cardId = -1,
                                                                    string messageType = null, DateTime? fromDate = null, DateTime? toDate = null, string parafaitFunctionName = null,
                                                                    int originalMessageId = -1, string toMobileList = null, string toEmailList = null, string trxNumber = null, string trxOTP = null, 
                                                                    string parafaitFunctionEventName = null, int pageNumber = 0, int numberofRecords = -1, string trxNumberList = null, string trxOTPList = null);

    }
}

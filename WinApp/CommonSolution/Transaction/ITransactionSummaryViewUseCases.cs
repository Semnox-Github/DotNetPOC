/********************************************************************************************
* Project Name - Transaction
* Description  - ITransactionSummaryViewUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.150.01    16-Feb-2023       Yashodhara C H     Modified: Added usecases for TransactionSummaryView
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Specification of the user use cases
    /// </summary>
    public interface ITransactionSummaryViewUseCases
    { 
        /// <summary>
        /// Gets the List of transactionSummaryViewDTO
        /// </summary>
        /// <returns></returns>
        Task<List<TransactionSummaryViewDTO>> GetTransactionSummaryViewDTOList(int transactionId = -1, int orderId = -1, int posMachineId = -1, string transactionOTP = null,
                                                   string externalSystemReference = null, int customerId = -1, int poseTypeId = -1, DateTime? fromDate = null, DateTime? toDate = null,
                                                   string originalSystemReference = null, int user_id = -1, string transactionNumber = null, string remarks = null, string transactionIdList = null,
                                                   string transactionNumberList = null, string transactionOTPList = null, int originalTransactionId = -1,
                                                   string emailList = null, string phoneNumberList = null, int pageNumber = 0, int numberOdRecords = -1);
    }
}

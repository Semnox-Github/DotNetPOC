/********************************************************************************************
 * Project Name - Transaction
 * Description  - RemoteTransactionSummaryViewUseCases
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.150.01   17-Feb-2023       Yashodhara C H      Added usecases for TransactionSummaryView
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// RemoteTransactionSummaryViewUseCases
    /// </summary>
    public class RemoteTransactionSummaryViewUseCases : RemoteUseCases, ITransactionSummaryViewUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string TRANSACTION_SUMMARY_VIEW = "api/Transaction/TransactionSummaryView";

        /// <summary>
        /// RemoteTransactionSummaryViewUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteTransactionSummaryViewUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the List of transactionSummaryViewDTO
        /// </summary>
        /// <returns></returns>
        public async Task<List<TransactionSummaryViewDTO>> GetTransactionSummaryViewDTOList(int transactionId = -1, int orderId = -1, int posMachineId = -1, string transactionOTP = null,
                                                   string externalSystemReference = null, int customerId = -1, int poseTypeId = -1, DateTime? fromDate = null, DateTime? toDate = null,
                                                   string originalSystemReference = null, int user_id = -1, string transactionNumber = null, string remarks = null, string transactionIdList = null,
                                                   string transactionNumberList = null, string transactionOTPList = null, int originalTransactionId = -1,
                                                   string emailIdList = null, string phoneNumberList = null, int pageNumber = 0, int numberOdRecords = -1)
        {
            log.LogMethodEntry(transactionId, orderId, posMachineId, transactionOTP, externalSystemReference, customerId, poseTypeId, fromDate, toDate, originalSystemReference, user_id, transactionNumber,
                                    remarks, transactionIdList, transactionNumberList, transactionOTPList, originalTransactionId, emailIdList, phoneNumberList, pageNumber, numberOdRecords);
            List<KeyValuePair<string, string>> searchParameters = new List<KeyValuePair<string, string>>();
            searchParameters.Add(new KeyValuePair<string, string>("transactionId", transactionId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("orderId", orderId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("posMachineId", posMachineId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("transactionOTP", transactionOTP));
            searchParameters.Add(new KeyValuePair<string, string>("externalSystemReference", externalSystemReference));
            searchParameters.Add(new KeyValuePair<string, string>("customerId", customerId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("poseTypeId", poseTypeId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("fromDate", fromDate.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("toDate", toDate.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("originalSystemReference", originalSystemReference));
            searchParameters.Add(new KeyValuePair<string, string>("user_id", user_id.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("transactionNumber", transactionNumber));
            searchParameters.Add(new KeyValuePair<string, string>("remarks", remarks));
            searchParameters.Add(new KeyValuePair<string, string>("transactionIdList", transactionIdList));
            searchParameters.Add(new KeyValuePair<string, string>("transactionNumberList", transactionNumberList));
            searchParameters.Add(new KeyValuePair<string, string>("transactionOTPList", transactionOTPList));
            searchParameters.Add(new KeyValuePair<string, string>("originalTransactionId", originalTransactionId.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("emailIdList", emailIdList));
            searchParameters.Add(new KeyValuePair<string, string>("phoneNumberList", phoneNumberList));
            searchParameters.Add(new KeyValuePair<string, string>("pageNumber", pageNumber.ToString()));
            searchParameters.Add(new KeyValuePair<string, string>("numberOdRecords", numberOdRecords.ToString()));
            try
            {
                List<TransactionSummaryViewDTO> transactionSummaryViewDTOList = await Get<List<TransactionSummaryViewDTO>>(TRANSACTION_SUMMARY_VIEW, searchParameters);
                log.LogMethodExit(transactionSummaryViewDTOList);
                return transactionSummaryViewDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

    }
}

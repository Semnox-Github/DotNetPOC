/********************************************************************************************
* Project Name - Transaction
* Description  - LocalTransactionSummaryViewUseCase
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.150.01    16-Feb-2023   Yashodhara C H       Added usecases for TransactionSummaryView
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Implementation of user use-cases
    /// </summary>
    public class LocalTransactionSummaryViewUseCase : LocalUseCases, ITransactionSummaryViewUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// LocalTransactionSummaryViewUseCase
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalTransactionSummaryViewUseCase(ExecutionContext executionContext) : base(executionContext)
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
                                                   string transactionNumberList = null, string transactionOTPList = null,  int originalTransactionId = -1,
                                                   string emailIdList = null, string phoneNumberList = null, int pageNumber = 0, int numberOdRecords = -1)
        {
            log.LogMethodEntry(transactionId, orderId, posMachineId, transactionOTP, externalSystemReference, customerId, poseTypeId, fromDate, toDate, originalSystemReference, user_id, transactionNumber,
                                    remarks, transactionIdList, transactionNumberList, transactionOTPList, originalTransactionId, emailIdList, phoneNumberList, pageNumber, numberOdRecords);
            return await Task<List<TransactionSummaryViewDTO>>.Factory.StartNew(() => 
            {
                DateTime startDate = ServerDateTime.Now;
                DateTime endDate = ServerDateTime.Now.AddDays(1);
                List<KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>>();
                if (transactionId != -1)
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
                }
                if (orderId != -1)
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.ORDER_ID, orderId.ToString()));
                }
                if (posMachineId != -1)
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.POS_MACHINE_ID, posMachineId.ToString()));
                }
                if (!string.IsNullOrWhiteSpace(transactionOTP))
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_OTP, transactionOTP));
                }
                if (!string.IsNullOrWhiteSpace(externalSystemReference))
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, externalSystemReference));
                }
                if (customerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }
                if (poseTypeId != -1)
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.POS_TYPE_ID, poseTypeId.ToString()));
                }
                if (fromDate != null)
                {
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = MessageContainerList.GetMessage(executionContext, 15);
                        log.Error(customException);
                        throw new ValidationException(customException);
                    }
                    if (toDate == null)
                        toDate = startDate.AddDays(1);
                }

                if (toDate != null)
                {
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = MessageContainerList.GetMessage(executionContext, 15);
                        log.Error(customException);
                        throw new ValidationException(customException);
                    }
                }

                if (startDate > endDate)
                {
                    string customException = MessageContainerList.GetMessage(executionContext, 2642);
                    log.LogMethodExit("Throwing Exception - " + customException);
                    throw new ValidationException(customException);
                }
                if (fromDate != null || toDate != null)
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.FROM_DATE, startDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.TO_DATE, endDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                
                if (!string.IsNullOrEmpty(originalSystemReference))
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.ORIGINAL_SYSTEM_REFERENCE, originalSystemReference));
                }
                if (user_id  != -1)
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.USER_ID, user_id.ToString()));
                }
                if (!string.IsNullOrEmpty(transactionNumber))
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_NUMBER, transactionNumber));
                }
                if (!string.IsNullOrEmpty(remarks))
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.REMARKS, remarks));
                }
                if (!string.IsNullOrEmpty(transactionIdList))
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_ID_LIST, transactionIdList));
                }
                if (!string.IsNullOrEmpty(transactionNumberList))
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_NUMBER_LIST, transactionNumberList));
                }
                if (!string.IsNullOrEmpty(transactionOTPList))
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.TRANSACTION_OTP_LIST, transactionOTPList));
                }
                if (originalTransactionId != -1)
                {
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.ORIGINAL_TRX_ID, originalTransactionId.ToString()));
                }
                if (!string.IsNullOrEmpty(emailIdList))
                {
                    emailIdList = Regex.Replace(emailIdList, @"\s", "");
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.EMAIL_ID, emailIdList));
                }
                if (!string.IsNullOrEmpty(phoneNumberList))
                {
                    phoneNumberList = Regex.Replace(phoneNumberList, @"\s", "");
                    searchParameters.Add(new KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>(TransactionSummaryViewDTO.SearchByParameters.PHONE_NUMBER, phoneNumberList));
                }
                TransactionSummaryViewListBL transactionSummaryViewListBL = new TransactionSummaryViewListBL(executionContext);
                List<TransactionSummaryViewDTO> transactionSummaryViewDTOList = transactionSummaryViewListBL.GetTransactionSummaryViewDTOList(searchParameters, pageNumber, numberOdRecords);
                log.LogMethodExit(transactionSummaryViewDTOList);
                return transactionSummaryViewDTOList;
            });
        }
    }
}

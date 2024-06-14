/********************************************************************************************
* Project Name - Transaction
* Description  - transaction Data Access Handler for accessing web data
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.60        12-Nov-2019   Lakshminarayana           Created 
********************************************************************************************/

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    ///  TransactionDiscounts Data Handler - Handles update and select of  Transaction object through web api
    /// </summary>
    public class TransactionWebDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly WebDataAccessHandler webDataAccessHandler;
        private static readonly Dictionary<TransactionDTO.SearchByParameters, string> webSearchParameters = new Dictionary<TransactionDTO.SearchByParameters, string>
        {
            {TransactionDTO.SearchByParameters.TRANSACTION_ID,"transactionId"},
            {TransactionDTO.SearchByParameters.STATUS, "transactionStatus"},
            {TransactionDTO.SearchByParameters.SITE_ID, "siteId"},
            {TransactionDTO.SearchByParameters.ORDER_ID, "trx_header.OrderId"},
            {TransactionDTO.SearchByParameters.POS_MACHINE_ID, "posMachineId"},
            {TransactionDTO.SearchByParameters.POS_NAME, "posName"},
            {TransactionDTO.SearchByParameters.TRANSACTION_OTP, "transactionOTP"},
            {TransactionDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, "referenceId"},
            {TransactionDTO.SearchByParameters.ORIGINAL_SYSTEM_REFERENCE, "originalSystemReference"},
            {TransactionDTO.SearchByParameters.ONLINE_ONLY, "onlineTransactionsOnly"},
            {TransactionDTO.SearchByParameters.CUSTOMER_ID, "customerId"},
            {TransactionDTO.SearchByParameters.CUSTOMER_GUID_ID, "customerGuid"},
            {TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, "fromDate"},
            {TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, "toDate"},
        };

        private const string GET_URL = "/api/Transaction/Transactions";
        private const string GET_TRANSACTION_SERVICES_URL = "api/Transaction/TransactionServices";
        private const string GET_PRINTABLE_TRANSACTION_LINES_URL = "api/Transaction/PrintableTransactionLines";
        private const string UPDATE_URL = "/api/Transaction/Transactions";
        private string POST_PRINT_URL = "api/Transaction/{TransactionId}/TransactionServices";

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="overridingSiteId">site Id</param>
        public TransactionWebDataHandler(ExecutionContext executionContext, int overridingSiteId = -1)
        {
            log.LogMethodEntry(executionContext, overridingSiteId);
            webDataAccessHandler = new WebDataAccessHandler(executionContext, overridingSiteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the transaction entity using web-api
        /// </summary>
        /// <param name="transactionDTO"></param>
        /// <returns></returns>
        public TransactionDTO UpdateTransaction(TransactionDTO transactionDTO)
        {
            log.LogMethodEntry(transactionDTO);
            
            webDataAccessHandler.Post(UPDATE_URL,
                JsonConvert.SerializeObject(transactionDTO));
            log.LogMethodExit(transactionDTO);
            return transactionDTO;
        }

        /// <summary>
        /// Get Transaction print details
        /// </summary>
        /// <param name="transactionDTO"></param>
        /// <returns></returns>
        public TransactionDTO GetOnlineTransactionPrintDetails(TransactionDTO transactionDTO)
        {
            log.LogMethodEntry(transactionDTO);
            TransactionDTO result = null;
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("activityType", "VIRTUALSTORE_ONLINEPRINT"),
                new KeyValuePair<string, string>("transactionId", transactionDTO.TransactionId.ToString()),
            };
            string url = webDataAccessHandler.GetUrl(GET_TRANSACTION_SERVICES_URL, parameters);
            string responseString = webDataAccessHandler.Get(url);
            dynamic response = JsonConvert.DeserializeObject(responseString);
            if (response != null)
            {
                object data = response["data"];
                result = JsonConvert.DeserializeObject<TransactionDTO>(data.ToString());
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// DoOnlineTransactionPrint
        /// </summary> 
        /// <returns></returns>
        public TransactionDTO DoOnlineTransactionPrint(TransactionDTO transactionDTO, List<TransactionLineDTO> selectedLineDTOList)
        {
            log.LogMethodEntry(transactionDTO, selectedLineDTOList);
            TransactionDTO result = null; 
            POST_PRINT_URL = "api/Transaction/" + transactionDTO.TransactionId.ToString() + "/TransactionServices";
            string responseString = webDataAccessHandler.Post(POST_PRINT_URL, JsonConvert.SerializeObject(selectedLineDTOList)); 
            dynamic response = JsonConvert.DeserializeObject(responseString);
            if (response != null)
            {
                object data = response["data"];
                result = JsonConvert.DeserializeObject<TransactionDTO>(data.ToString());
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the TransactionDTO list matching the UserId
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="pageNumber">current page number</param>
        /// <param name="pageSize">page size</param>
        /// <param name="buildChildRecords">whether to include child records</param>
        /// <param name="buildTickets">whether to build tickets</param>
        /// <param name="buildReceipt">whether to build receipt</param>
        /// <returns>Returns the list of transactionDTO matching the search criteria</returns>
        public List<TransactionDTO> GetTransactionDTOList(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters, int pageNumber = 0, int pageSize = 10, bool buildChildRecords = false, bool buildTickets = false, bool buildReceipt = false)
        {
            log.LogMethodEntry(searchParameters);
            List<TransactionDTO> transactionDTOList = null;
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("pageNumber", pageNumber.ToString()),
                new KeyValuePair<string, string>("pageSize", pageSize.ToString()),
                new KeyValuePair<string, string>("buildChildRecords", buildChildRecords.ToString()),
                new KeyValuePair<string, string>("buildTickets", buildTickets.ToString()),
                new KeyValuePair<string, string>("buildReceipt", buildReceipt.ToString())
            };
            foreach (KeyValuePair<TransactionDTO.SearchByParameters, string> searchParameter in searchParameters)
            {
                if (searchParameter.Key == TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE ||
                    searchParameter.Key == TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE)
                {
                    parameters.Add(new KeyValuePair<string, string>(webSearchParameters[searchParameter.Key], webDataAccessHandler.GetUtcDateTimeString(searchParameter.Value)));
                }
                else
                {
                    parameters.Add(new KeyValuePair<string, string>(webSearchParameters[searchParameter.Key], searchParameter.Value));
                }
            }

            log.LogVariableState("GET_URL", GET_URL);
            string url = webDataAccessHandler.GetUrl(GET_URL, parameters);

            log.LogVariableState("url", url);
            string response = webDataAccessHandler.Get(url);

            log.LogVariableState("response", response);
            dynamic result = JsonConvert.DeserializeObject(response);
            if (result != null)
            {
                object data = result["data"];
                transactionDTOList = JsonConvert.DeserializeObject<List<TransactionDTO>>(data.ToString());
            }
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }

        internal List<KeyValuePair<string, List<TransactionLineDTO>>> GetPrintableTransactionLines(int transactionId, string printerTypeList, bool forVirtualStore)
        {
            log.LogMethodEntry(transactionId, printerTypeList, forVirtualStore);
            List<KeyValuePair<string, List<TransactionLineDTO>>> printableLinesList = new List<KeyValuePair<string, List<TransactionLineDTO>>>(); ;
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("TransactionId", transactionId.ToString()),
                new KeyValuePair<string, string>("printerTypeList", printerTypeList),
                new KeyValuePair<string, string>("forVirtualStore", (forVirtualStore? "true":"false") )
            };
            string url = webDataAccessHandler.GetUrl(GET_PRINTABLE_TRANSACTION_LINES_URL, parameters);
            string response = webDataAccessHandler.Get(url);
            dynamic result = JsonConvert.DeserializeObject(response);
            if (result != null)
            {
                object data = result["data"];
                printableLinesList = JsonConvert.DeserializeObject<List<KeyValuePair<string, List<TransactionLineDTO>>>>(data.ToString());
            }
            log.LogMethodExit(printableLinesList);
            return printableLinesList;
        }

        internal TransactionDTO DoExecuteOnlineReceipt(TransactionDTO transactionDTO, int cardCount, bool executeIsSuccessful)
        {
            log.LogMethodEntry(transactionDTO, cardCount, executeIsSuccessful);

            //List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string, string>("activityType", "VIRTUALSTORE_ONLINEPRINT"),
            //    new KeyValuePair<string, string>("transactionId", transactionDTO.TransactionId.ToString()),
            //};
            //string url = webDataAccessHandler.GetUrl(GET_TRANSACTION_SERVICES_URL, parameters);
            //string responseString = webDataAccessHandler.Get(url); 
            TransactionDTO result = null;
            string PRINT_URL = "api/Transaction/" + transactionDTO.TransactionId.ToString() + "/TransactionServices/ExecuteOnlineReceipt";
            List<int> taskInfoList = new List<int>();
            taskInfoList.Add((executeIsSuccessful ? 1: 0));
            taskInfoList.Add(cardCount);
            string responseString = webDataAccessHandler.Post(PRINT_URL, JsonConvert.SerializeObject(taskInfoList));
            dynamic response = JsonConvert.DeserializeObject(responseString);
            if (response != null)
            {
                object data = response["data"];
                result = JsonConvert.DeserializeObject<TransactionDTO>(data.ToString());
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}

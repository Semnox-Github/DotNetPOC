/********************************************************************************************
 * Project Name - RedemptionUtils
 * Description  - RemoteTicketReceiptUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          26-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RemoteTicketReceiptUseCases : RemoteUseCases, ITicketReceiptUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string TICKET_RECEIPT_URL = "api/Redemption/TicketReceipts";
        private const string TICKET_VALIDATE_URL = "api/Redemption/ValidateTickets";
        private const string TICKET_LIMIT_URL = "api/Redemption/TicketLimits";

        public RemoteTicketReceiptUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> ticketReceiptSearchParams)
        {
            log.LogMethodEntry(ticketReceiptSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string> searchParameter in ticketReceiptSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case TicketReceiptDTO.SearchByTicketReceiptParameters.ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ticketReceiptId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TicketReceiptDTO.SearchByTicketReceiptParameters.REDEMPTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("redemptionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_TICKET_RECEIPT_NO:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("manualTicketReceiptNo".ToString(), searchParameter.Value));
                        }
                        break;
                    case TicketReceiptDTO.SearchByTicketReceiptParameters.BALANCE_TICKETS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("balanceTickets".ToString(), searchParameter.Value));
                        }
                        break;
                    case TicketReceiptDTO.SearchByTicketReceiptParameters.IS_SUSPECTED:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isSuspected".ToString(), searchParameter.Value));
                        }
                        break;
                    case TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("sourceRedemptionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_FROM_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("issueFromDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_TO_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("issueToDate".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }


        public async Task<List<TicketReceiptDTO>> GetTicketReceipts(List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> parameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {

            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<TicketReceiptDTO> result = await Get<List<TicketReceiptDTO>>(TICKET_RECEIPT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> UpdateTicketReceipts(List<TicketReceiptDTO> ticketReceiptDTOList)
        {
            log.LogMethodEntry(ticketReceiptDTOList);
            try
            {
                string responseString = await Put<string>(TICKET_RECEIPT_URL, ticketReceiptDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task PerDayTicketLimitCheck(int manualTicket)
        {
            log.LogMethodEntry(manualTicket);
            try
            {
                List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
                searchParameterList.Add(new KeyValuePair<string, string>("manualTicket".ToString(), manualTicket.ToString()));
                string responseString = await Get<string>(TICKET_LIMIT_URL, searchParameterList);
                log.LogMethodExit(responseString);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> ValidateTicketReceipts(string barcode, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(barcode,sqlTransaction);
            try
            {
                List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
                searchParameterList.Add(new KeyValuePair<string, string>("receiptNumber".ToString(), barcode.ToString()));
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                var result = await Get<int>(TICKET_VALIDATE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}

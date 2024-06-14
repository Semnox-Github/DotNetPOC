/********************************************************************************************
 * Project Name - Redemption
 * Description  - LocalTicketReceiptUseCases class to get the data  from local DB 
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
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class LocalTicketReceiptUseCases : LocalUseCases, ITicketReceiptUseCases
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalTicketReceiptUseCases(ExecutionContext executionContext)
            :base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<TicketReceiptDTO>> GetTicketReceipts(List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> parameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<TicketReceiptDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                TicketReceiptList ticketReceiptList = new TicketReceiptList(executionContext);
                int siteId = GetSiteId();
                List<TicketReceiptDTO> ticketReceiptDTOList = ticketReceiptList.GetAllTicketReceipt(parameters);
                log.LogMethodExit(ticketReceiptDTOList);
                return ticketReceiptDTOList;
            });
        }

        public async Task<string> UpdateTicketReceipts(List<TicketReceiptDTO> ticketReceiptDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(ticketReceiptDTOList);
                    if (ticketReceiptDTOList == null)
                    {
                        throw new ValidationException("ticketReceiptDTOList is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        foreach (TicketReceiptDTO ticketReceiptDTO in ticketReceiptDTOList)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                TicketReceipt ticketReceipt = new TicketReceipt(executionContext, ticketReceiptDTO);
                                ticketReceipt.Save(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                throw;;
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw ;
                            }
                        }
                        result = "Success";
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task PerDayTicketLimitCheck(int manualTicket)
        {
            await Task.Factory.StartNew(() =>
            {
                log.LogMethodEntry(manualTicket);
                V2.RedemptionBL redemptionBL = new V2.RedemptionBL(executionContext);
                if (manualTicket > 0)
                {
                    redemptionBL.PerDayLimitCheckForManualTickets(manualTicket);
                }
                else if(manualTicket < 0)
                {
                    redemptionBL.PerDayLimitCheckForReducingManualTickets(manualTicket);
                }               
                log.LogMethodExit();
            });
        }
        public async Task<int> ValidateTicketReceipts(string receiptNumber, SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() => 
            {
                log.LogMethodEntry(receiptNumber);
                TicketReceipt ticketReceipt = new TicketReceipt(executionContext , receiptNumber);
                int result = ticketReceipt.GetTicketReceiptValue(receiptNumber);
                log.LogMethodExit();
                return result;
            }); 
        }

        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }
    }
}

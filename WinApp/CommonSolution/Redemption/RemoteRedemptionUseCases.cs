/********************************************************************************************
 * Project Name - RedemptionUtils
 * Description  - RemoteRedemptionUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          27-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;

namespace Semnox.Parafait.Redemption
{
    public class RemoteRedemptionUseCases : RemoteUseCases, IRedemptionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string REDEMPTION_ORDER_URL = "api/Redemption/RedemptionOrders";
        private const string SUSPEND_REDEMPTION_ORDER_URL = "api/Redemption/RedemptionOrders/Suspend";
        private string REDEMPTION_GIFT_URL = "api/Redemption/RedemptionOrder/{orderId}/Gifts";
        private string REDEMPTION_CARD_URL = "api/Redemption/RedemptionOrder/{orderId}/Cards";
        private string REDEMPTION_TICKET_URL = "api/Redemption/RedemptionOrder/{orderId}/Tickets";
        private string TURNIN_CARD_URL = "api/Redemption/RedemptionOrder/{orderId}/TurnInCards";
        private string TURNIN_GIFT_URL = "api/Redemption/RedemptionOrder/{orderId}/TurnInGifts";
        private string APPLICATION_REMARKS_URL = "api/Redemption/ApplicationRemarks";
        private string LOAD_TO_CARD_URL = "api/Redemption/RedemptionOrder/{orderId}/LoadToCard";
        private string CONSOLIDATE_TICKET_URL = "api/Redemption/RedemptionOrder/{orderId}/Consolidate";
        private string ADD_MANUAL_TICKET_URL = "api/Redemption/RedemptionOrder/{orderId}/ManualTickets";
        private string REDEMPTION_STATUS_UPDATE_URL = "api/Redemption/RedemptionOrder/{orderId}/Status";
        private string REDEMPTION_PRICE_CONTAINER_URL = "api/Redemption/RedemptionPriceContainer";
        private string REDEMPTION_PRINT_URL = "api/Redemption/RedemptionOrder/{orderId}/Print";
        private string REDEMPTION_ORDER_CURRENCY_URL = "api/Redemption/RedemptionOrder/{orderId}/Currencies";
        private string TICKET_PRINT_URL = "api/Redemption/TicketReceipt/{TicketId}/Print";
        private string REDEMPTION_REPRINT_URL = "api/Redemption/TicketReceipt/{TicketId}/RePrint";
        private string REALTICKET_RECEIPT_PRINT_URL = "api/Redemption/TicketReceipt/RealTicketPrint";
        private string REDEMPTION_SUSPEND_PRINT_URL = "api/Redemption/Suspend/{orderId}/Print";
        private string REDEMPTION_REVERSE_URL = "api/Redemption/RedemptionOrder/{orderId}/Reverse";


        public RemoteRedemptionUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<RedemptionDTO>> GetRedemptionOrders(List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> parameters,
            bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<RedemptionDTO> result = await Get<List<RedemptionDTO>>(REDEMPTION_ORDER_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> redemptionSearchParams)
        {
            log.LogMethodEntry(redemptionSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<RedemptionDTO.SearchByParameters, string> searchParameter in redemptionSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case RedemptionDTO.SearchByParameters.REDEPTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("orderId".ToString(), searchParameter.Value));
                        }
                        break;
                    case RedemptionDTO.SearchByParameters.CARD_NUMBER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("cardNumber".ToString(), searchParameter.Value));
                        }
                        break;
                    case RedemptionDTO.SearchByParameters.POS_MACHINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("posMachineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case RedemptionDTO.SearchByParameters.REDEMPTION_ORDER_NO:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("orderNumber".ToString(), searchParameter.Value));
                        }
                        break;
                    case RedemptionDTO.SearchByParameters.REDEMPTION_STATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("status".ToString(), searchParameter.Value));
                        }
                        break;
                    case RedemptionDTO.SearchByParameters.LOAD_GIFT_CARD_TICKET_ALLOCATION_DETAILS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("loadGiftCardTicketAllocationDetails".ToString(), searchParameter.Value));
                        }
                        break;
                    case RedemptionDTO.SearchByParameters.FROM_REDEMPTION_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("fromDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case RedemptionDTO.SearchByParameters.TO_REDEMPTION_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("toDate".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<RedemptionDTO> SaveRedemptionOrders(List<RedemptionDTO> redemptionDTOList)
        {
            log.LogMethodEntry(redemptionDTOList);
            try
            {
                RedemptionDTO responseString = await Post<RedemptionDTO>(REDEMPTION_ORDER_URL, redemptionDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<RedemptionDTO> SuspendOrders(List<RedemptionDTO> redemptionDTOList)
        {
            log.LogMethodEntry(redemptionDTOList);
            try
            {
                RedemptionDTO responseString = await Post<RedemptionDTO>(SUSPEND_REDEMPTION_ORDER_URL, redemptionDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> AddCard(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            log.LogMethodEntry(redemptionCardsDTOList);
            try
            {
                REDEMPTION_CARD_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Cards";
                RedemptionDTO responseString = await Post<RedemptionDTO>(REDEMPTION_CARD_URL, redemptionCardsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> UpdateCard(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            log.LogMethodEntry(redemptionCardsDTOList);
            try
            {
                REDEMPTION_CARD_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Cards";
                RedemptionDTO responseString = await Put<RedemptionDTO>(REDEMPTION_CARD_URL, redemptionCardsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<RedemptionDTO> AddTurnInCards(int orderId, List<RedemptionCardsDTO> redemptionCardDTOList)
        {
            log.LogMethodEntry(orderId, redemptionCardDTOList);
            try
            {
                TURNIN_CARD_URL = "api/Redemption/RedemptionOrder/" + orderId + "/TurnInCards";
                RedemptionDTO responseString = await Post<RedemptionDTO>(TURNIN_CARD_URL, redemptionCardDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> AddGift(int orderId, List<RedemptionGiftsDTO> redemptionGiftsDTOList)
        {
            log.LogMethodEntry(orderId, redemptionGiftsDTOList);
            try
            {
                REDEMPTION_GIFT_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Gifts";
                RedemptionDTO responseString = await Post<RedemptionDTO>(REDEMPTION_GIFT_URL, redemptionGiftsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> UpdateGift(int orderId, List<RedemptionGiftsDTO> redemptionGiftsDTOList)
        {
            log.LogMethodEntry(orderId, redemptionGiftsDTOList);
            try
            {
                REDEMPTION_GIFT_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Gifts";
                RedemptionDTO responseString = await Put<RedemptionDTO>(REDEMPTION_GIFT_URL, redemptionGiftsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<RedemptionDTO> AddTurnInGifts(int orderId, List<RedemptionGiftsDTO> redemptionGiftsDTOList)
        {
            log.LogMethodEntry(orderId, redemptionGiftsDTOList);
            try
            {
                TURNIN_GIFT_URL = "api/Redemption/RedemptionOrder/" + orderId + "/TurnInGifts";
                RedemptionDTO responseString = await Post<RedemptionDTO>(TURNIN_GIFT_URL, redemptionGiftsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> AddTicket(int orderId, List<TicketReceiptDTO> ticketReceiptDTOList)
        {
            log.LogMethodEntry(orderId, ticketReceiptDTOList);
            try
            {
                REDEMPTION_TICKET_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Tickets";
                RedemptionDTO responseString = await Post<RedemptionDTO>(REDEMPTION_TICKET_URL, ticketReceiptDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> RemoveCard(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            log.LogMethodEntry(redemptionCardsDTOList);
            try
            {
                REDEMPTION_CARD_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Cards";
                RedemptionDTO responseString = await Delete<RedemptionDTO>(REDEMPTION_CARD_URL, redemptionCardsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> RemoveGift(int orderId, List<RedemptionGiftsDTO> redemptionGiftsDTOList)
        {
            log.LogMethodEntry(orderId, redemptionGiftsDTOList);
            try
            {
                REDEMPTION_GIFT_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Gifts";
                RedemptionDTO responseString = await Delete<RedemptionDTO>(REDEMPTION_GIFT_URL, redemptionGiftsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> RemoveTurnInCards(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            log.LogMethodEntry(orderId, redemptionCardsDTOList);
            try
            {
                TURNIN_CARD_URL = "api/Redemption/RedemptionOrder/" + orderId + "/TurnInCards";
                RedemptionDTO responseString = await Delete<RedemptionDTO>(TURNIN_CARD_URL, redemptionCardsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> RemoveTurnInGifts(int orderId, List<RedemptionGiftsDTO> redemptionGiftsDTOList)
        {
            log.LogMethodEntry(redemptionGiftsDTOList);
            try
            {
                TURNIN_GIFT_URL = "api/Redemption/RedemptionOrder/" + orderId + "/TurnInGifts";
                RedemptionDTO responseString = await Delete<RedemptionDTO>(TURNIN_GIFT_URL, redemptionGiftsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> RemoveTicket(int orderId, List<TicketReceiptDTO> ticketReceiptDTOList)
        {
            log.LogMethodEntry(ticketReceiptDTOList);
            try
            {
                REDEMPTION_TICKET_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Tickets";
                RedemptionDTO responseString = await Delete<RedemptionDTO>(REDEMPTION_TICKET_URL, ticketReceiptDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<ApplicationRemarksDTO>> GetApplicationRemarks(List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildApplicationRemarksSearchParameter(searchParameters));
            }
            try
            {
                List<ApplicationRemarksDTO> result = await Get<List<ApplicationRemarksDTO>>(APPLICATION_REMARKS_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<ReceiptClass> GetRedemptionOrderPrint(int orderId)
        {
            log.LogMethodEntry(orderId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("orderId".ToString(), orderId.ToString()));
            try
            {
                REDEMPTION_PRINT_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Print";
                ReceiptClass result = await Get<ReceiptClass>(REDEMPTION_PRINT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildApplicationRemarksSearchParameter(List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>> applicationRemarksSearchParams)
        {
            log.LogMethodEntry(applicationRemarksSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string> searchParameter in applicationRemarksSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case ApplicationRemarksDTO.SearchByApplicationRemarksParameters.MODULE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("moduleName".ToString(), searchParameter.Value));
                        }
                        break;
                    case ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("sourceName".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<ApplicationRemarksDTO> SaveApplicationRemarks(List<ApplicationRemarksDTO> applicationRemarksDTOList)
        {
            log.LogMethodEntry(applicationRemarksDTOList);
            try
            {
                ApplicationRemarksDTO responseString = await Post<ApplicationRemarksDTO>(APPLICATION_REMARKS_URL, applicationRemarksDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<RedemptionDTO> ConsolidateTicketReceipts(int orderId, RedemptionActivityDTO redemptionActivityDTO)
        {
            log.LogMethodEntry(redemptionActivityDTO);
            try
            {
                CONSOLIDATE_TICKET_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Consolidate";
                RedemptionDTO responseString = await Post<RedemptionDTO>(CONSOLIDATE_TICKET_URL, redemptionActivityDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> LoadTicketsToCard(int orderId, RedemptionLoadToCardRequestDTO redemptionLoadToCardRequestDTO)
        {
            log.LogMethodEntry(redemptionLoadToCardRequestDTO);
            try
            {
                LOAD_TO_CARD_URL = "api/Redemption/RedemptionOrder/"+orderId+"/LoadToCard";
                RedemptionDTO responseString = await Post<RedemptionDTO>(LOAD_TO_CARD_URL, redemptionLoadToCardRequestDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> AddManualTickets(int orderId, RedemptionActivityDTO redemptionActivityDTO)
        {
            log.LogMethodEntry(redemptionActivityDTO);
            try
            {
                ADD_MANUAL_TICKET_URL = "api/Redemption/RedemptionOrder/" + orderId + "/ManualTickets";
                RedemptionDTO responseString = await Post<RedemptionDTO>(ADD_MANUAL_TICKET_URL, redemptionActivityDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> UpdateRedemptionStatus(int orderId, RedemptionActivityDTO redemptionActivityDTO)
        {
            log.LogMethodEntry(orderId, redemptionActivityDTO);
            try
            {
                REDEMPTION_STATUS_UPDATE_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Status";
                RedemptionDTO responseString = await Post<RedemptionDTO>(REDEMPTION_STATUS_UPDATE_URL, redemptionActivityDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionDTO> RemoveManualTickets(int orderId, RedemptionActivityDTO redemptionActivityDTO)
        {
            log.LogMethodEntry(orderId, redemptionActivityDTO);
            try
            {
                ADD_MANUAL_TICKET_URL = "api/Redemption/RedemptionOrder/" + orderId + "/ManualTickets";
                RedemptionDTO responseString = await Post<RedemptionDTO>(ADD_MANUAL_TICKET_URL, redemptionActivityDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        } 

        public async Task<RedemptionDTO> AddCurrency(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            log.LogMethodEntry(orderId, redemptionCardsDTOList);
            try
            {
                REDEMPTION_ORDER_CURRENCY_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Currencies";
                RedemptionDTO responseString = await Post<RedemptionDTO>(REDEMPTION_ORDER_CURRENCY_URL, redemptionCardsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<RedemptionDTO> ReverseRedemption(int orderId, RedemptionActivityDTO redemptionActivityDTO)
        {
            log.LogMethodEntry(orderId, redemptionActivityDTO);
            try
            {
                REDEMPTION_REVERSE_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Reverse";
                RedemptionDTO responseString = await Post<RedemptionDTO>(REDEMPTION_REVERSE_URL, redemptionActivityDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// remote proxy to retrieve the redemption price container data structure
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="hash">hash</param>
        /// <param name="rebuildCache">rebuild cache</param>
        /// <returns></returns>
        public async Task<RedemptionPriceContainerDTOCollection> GetRedemptionPriceContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            RedemptionPriceContainerDTOCollection result = await Get<RedemptionPriceContainerDTOCollection>(REDEMPTION_PRICE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<RedemptionDTO> RemoveCurrency(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            log.LogMethodEntry(orderId, redemptionCardsDTOList);
            try
            {
                REDEMPTION_ORDER_CURRENCY_URL = "api/Redemption/RedemptionOrder/" + orderId + "/Currencies";
                RedemptionDTO responseString = await Post<RedemptionDTO>(REDEMPTION_ORDER_CURRENCY_URL, redemptionCardsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<clsTicket> GetRealTicketReceiptPrint(int sourceRedemptionId, int tickets, DateTime? issueDate)
        {
            log.LogMethodEntry(sourceRedemptionId, tickets, issueDate);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("sourceRedemptionId".ToString(), sourceRedemptionId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("tickets".ToString(), tickets.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("issueDate".ToString(), issueDate.ToString()));
            try
            {
               
                REALTICKET_RECEIPT_PRINT_URL = "api/Redemption/TicketReceipt/RealTicketPrint";
                clsTicket result = await Get<clsTicket>(REALTICKET_RECEIPT_PRINT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<clsTicket> PrintManualTicketReceipt(int ticketId)
        {
            log.LogMethodEntry(ticketId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("ticketId".ToString(), ticketId.ToString()));
            try
            {
               
                TICKET_PRINT_URL = "api/Redemption/TicketReceipt/" + ticketId + "/Print";
                clsTicket result = await Get<clsTicket>(TICKET_PRINT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<clsTicket> ReprintManualTicketReceipt(int ticketId, RedemptionActivityDTO redemptionActivityDTO)
        {
            log.LogMethodEntry(ticketId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("ticketId".ToString(), ticketId.ToString()));
            try
            {
                REDEMPTION_REPRINT_URL = "api/Redemption/TicketReceipt/" + ticketId + "/RePrint";
                clsTicket result = await Post<clsTicket>(REDEMPTION_REPRINT_URL, redemptionActivityDTO);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<ReceiptClass> GetSuspendedReceiptPrint(int orderId)
        {
            log.LogMethodEntry(orderId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("orderId".ToString(), orderId.ToString()));
            try
            {
                REDEMPTION_SUSPEND_PRINT_URL = "api/Redemption/Suspend/" + orderId + "/Print";
                ReceiptClass result = await Get<ReceiptClass>(REDEMPTION_SUSPEND_PRINT_URL, searchParameterList);
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

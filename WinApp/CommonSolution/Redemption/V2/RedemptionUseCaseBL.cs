/********************************************************************************************
* Project Name - RedemptionUseCaseBL
* Description  - Business logic - For UseCaseFactory
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.110.0     12-Dec-2020      Girish Kundar       Created
*2.110.0     19-Jan-2020      Mushahid Faizan     Handled IsActive column changes.
*2.140.0     15-Nov-2021      Prashanth           Modified : Added GetApproverId method
*2.130.10    26-Aug-2022      Amitha Joy          Inventory tracking changes, remove turn in from location
*********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Windows.Forms;
using Microsoft.IdentityModel.Tokens;
using QRCoder;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Customer.Membership.Sample;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Product;
using Semnox.Parafait.Site;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using Semnox.Parafait.Vendor;

namespace Semnox.Parafait.Redemption.V2
{
    public class RedemptionBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private RedemptionDTO redemptionDTO;
        private string approvalId = "";
        private string managerToken = "";
        private bool PrintBalanceTicket = false;
        private bool LoadBalanceTickettoCard = false;
        //private Utilities utilities;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public RedemptionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            //utilities = GetUtility();
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates RedemptionUseCaseBL object using the RedemptionDTO
        /// </summary>
        /// <param name="redemptionDTO">RedemptionDTO object</param>
        /// <param name="executionContext">executionContext</param>
        public RedemptionBL(ExecutionContext executionContext, RedemptionDTO redemptionDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, redemptionDTO);
            this.redemptionDTO = redemptionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the redemption id and executionContext as the parameter
        /// Would fetch the redemption object from the database based on the id passed. 
        /// </summary>
        /// <param name="redemptionOrderId">redemptionOrderId</param>
        /// <param name="ExecutionContext">ExecutionContext</param>
        /// <param name="SqlTransaction">SqlTransaction</param>
        public RedemptionBL(ExecutionContext executionContext, int redemptionOrderId, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(redemptionOrderId, executionContext, sqlTransaction);
            RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
            redemptionDTO = redemptionDataHandler.GetRedemptionDTO(redemptionOrderId);
            if (redemptionDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Redemption DTO", redemptionOrderId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            Build(sqlTransaction);
            log.LogMethodExit();
        }

        private void Build(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            /// Load Redemption Cards record.
            RedemptionCardsListBL redemptionCardsListBL = new RedemptionCardsListBL(executionContext);
            List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>> redemptionCardsSearchParams = new List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>>();
            redemptionCardsSearchParams.Add(new KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>(RedemptionCardsDTO.SearchByParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()));
            redemptionDTO.RedemptionCardsListDTO = redemptionCardsListBL.GetRedemptionCardsDTOList(redemptionCardsSearchParams, sqlTransaction);
            if (redemptionDTO.RedemptionCardsListDTO == null)
            {
                redemptionDTO.RedemptionCardsListDTO = new List<RedemptionCardsDTO>();
            }
            /// Load Redemption Gifts record.
            RedemptionGiftsListBL redemptionGiftsListBL = new RedemptionGiftsListBL(executionContext);
            List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>> redemptionGiftsSearchParams = new List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>>();
            redemptionGiftsSearchParams.Add(new KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>(RedemptionGiftsDTO.SearchByParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()));
            redemptionDTO.RedemptionGiftsListDTO = redemptionGiftsListBL.GetRedemptionGiftsDTOList(redemptionGiftsSearchParams, sqlTransaction);

            if (redemptionDTO.RedemptionGiftsListDTO == null)
            {
                redemptionDTO.RedemptionGiftsListDTO = new List<RedemptionGiftsDTO>();
            }
            /// Load RedemptionTicketAllocation record.
            RedemptionTicketAllocationListBL redemptionTicketAllocationListBL = new RedemptionTicketAllocationListBL(executionContext);
            List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>> redemptionTicketAllocationsSearchParams = new List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>>();
            redemptionTicketAllocationsSearchParams.Add(new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()));
            redemptionDTO.RedemptionTicketAllocationListDTO = redemptionTicketAllocationListBL.GetRedemptionTicketAllocationDTOList(redemptionTicketAllocationsSearchParams, sqlTransaction);
            if (redemptionDTO.RedemptionTicketAllocationListDTO == null)
            {
                redemptionDTO.RedemptionTicketAllocationListDTO = new List<RedemptionTicketAllocationDTO>();
            }
            /// Load TicketReceipt record.
            TicketReceiptList ticketReceiptList = new TicketReceiptList(executionContext);
            List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> ticketReceiptsSearchParams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
            ticketReceiptsSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()));
            redemptionDTO.TicketReceiptListDTO = ticketReceiptList.GetAllTicketReceipt(ticketReceiptsSearchParams, sqlTransaction);
            if (redemptionDTO.TicketReceiptListDTO == null)
            {
                redemptionDTO.TicketReceiptListDTO = new List<TicketReceiptDTO>();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// add Redemption Currency
        /// </summary>
        /// <param name="currencyId">currencyId</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        public List<RedemptionCardsDTO> AddRedemptionCurrency(List<RedemptionCardsDTO> addRedemptionCardDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(addRedemptionCardDTO, sqlTransaction);
            string message = "";
            List<RedemptionCardsDTO> result = new List<RedemptionCardsDTO>();
            if (addRedemptionCardDTO != null)
            {
                foreach (RedemptionCardsDTO redemptionCardDTO in addRedemptionCardDTO)
                {
                    RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(executionContext);
                    List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParam = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>();
                    searchParam.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_ID, redemptionCardDTO.CurrencyId.ToString()));
                    searchParam.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.ISACTIVE, "1"));
                    List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = redemptionCurrencyList.GetAllRedemptionCurrency(searchParam,0,10,sqlTransaction);
                    if (redemptionCurrencyDTOList != null)
                    {
                        foreach (RedemptionCurrencyDTO redemptionCurrencyDTO in redemptionCurrencyDTOList)
                        {
                            RedemptionCardsDTO addRedemptionCardsDTO = new RedemptionCardsDTO(-1, redemptionCardDTO.RedemptionId, null,
                                redemptionCardDTO.CardId, Convert.ToInt32(redemptionCurrencyDTO.ValueInTickets), redemptionCardDTO.CurrencyId, redemptionCardDTO.CurrencyQuantity,
                                redemptionCardDTO.CurrencyValueInTickets, redemptionCurrencyDTO.CurrencyName, redemptionCardDTO.TotalCardTickets, redemptionCardDTO.ViewGroupingNumber);
                            result.Add(addRedemptionCardsDTO);
                        }
                        message = redemptionCardDTO.CurrencyName + "added";
                    }
                    else
                    {
                        message = "Invalid redemption currency";
                        log.Info("Ends-addRedemptionCurrency(" + redemptionCardDTO.CurrencyId + "," + message + ") - Invalid redemption currency");
                        result = null;
                        log.LogMethodExit(result);
                        return result;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// remove currency
        /// </summary>
        /// <param name="removeRedemptionCardDTO">currencyId</param>
        internal List<RedemptionCardsDTO> RemoveRedemptionCurrency(List<RedemptionCardsDTO> redemptionCardDTOList, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(redemptionCardDTOList);
            List<RedemptionCardsDTO> result = new List<RedemptionCardsDTO>();
            foreach (RedemptionCardsDTO redemptionCardsDTO in redemptionCardDTOList)
            {
                if (redemptionCardsDTO.CurrencyId != -1)
                {
                    redemptionCardsDTO.CurrencyId = null;
                }
                result.Add(redemptionCardsDTO);
            }
            log.LogMethodExit(result);
            return result;
        }
        // not used
        internal void Suspend(SqlTransaction sQLTrx)
        {
            log.LogMethodEntry();

            if (redemptionDTO.RedemptionId > -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1392);// "Suspended redemption is already processed";
                throw new ValidationException(errorMessage);
            }
            if (GetTotalTickets(sQLTrx) > 0)
            {
                RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sQLTrx);
                redemptionDTO.RedeemedDate = ServerDateTime.Now;
                redemptionDTO.RedemptionOrderNo = Redemption.RedemptionBL.GetNextSeqNo(executionContext.GetMachineId(), sQLTrx);
                redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString();
                redemptionDTO = redemptionDataHandler.InsertRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                redemptionDTO.AcceptChanges();
                // Save tickets
                if (redemptionDTO.TicketReceiptListDTO != null && redemptionDTO.TicketReceiptListDTO.Any())
                {
                    foreach (TicketReceiptDTO ticketReceiptDTO in redemptionDTO.TicketReceiptListDTO)
                    {
                        if (ticketReceiptDTO.RedemptionId == -1)
                        {
                            ticketReceiptDTO.RedemptionId = redemptionDTO.RedemptionId;
                        }
                        TicketReceipt ticketReceipt = new TicketReceipt(executionContext, ticketReceiptDTO);
                        ticketReceipt.Save(sQLTrx);
                    }
                }
                //Save Cards
                if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any())
                {
                    RedemptionCardsBL redemptionCardsBL;
                    foreach (RedemptionCardsDTO redemptionCardDTO in redemptionDTO.RedemptionCardsListDTO)
                    {
                        if (redemptionCardDTO.RedemptionId == -1)
                        {
                            redemptionCardDTO.RedemptionId = redemptionDTO.RedemptionId;
                        }
                        AccountBL cardAccount = new AccountBL(executionContext, redemptionCardDTO.CardId, true, true);
                        if (cardAccount != null && cardAccount.GetAccountId() != -1)
                        {
                            redemptionCardDTO.TotalCardTickets = cardAccount.GetTotalTickets();
                        }
                        redemptionCardsBL = new RedemptionCardsBL(executionContext, redemptionCardDTO);
                        redemptionCardsBL.Save(sQLTrx);
                    }
                    AccountBL accountBL = new AccountBL(executionContext, redemptionDTO.RedemptionCardsListDTO[0].CardId, true, true);
                    redemptionDTO.PrimaryCardNumber = accountBL.AccountDTO.TagNumber;
                    redemptionDTO = redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    redemptionDTO.AcceptChanges();
                }
                //Save Gifs
                if (redemptionDTO.RedemptionGiftsListDTO != null && redemptionDTO.RedemptionGiftsListDTO.Any())
                {
                    redemptionDTO.RedemptionGiftsListDTO.ForEach(x => x.RedemptionId = redemptionDTO.RedemptionId);
                    RedemptionGiftsListBL redemptionGiftsListBL = new RedemptionGiftsListBL(executionContext,redemptionDTO.RedemptionGiftsListDTO);
                    redemptionGiftsListBL.Save(sQLTrx);
                    LoadRedemptionGiftDTOList(sQLTrx);
                }
                RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO(-1, redemptionDTO.RedemptionId, -1, -1,
                   executionContext.GetUserId(), Convert.ToDateTime(redemptionDTO.RedeemedDate), executionContext.GetMachineId(),
                   RedemptionUserLogsDTO.RedemptionAction.SUSPEND_REDEMPTION.ToString(),
                   redemptionDTO.Source, approvalId.ToString(), ServerDateTime.Now);
                RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
                redemptionUserLogsBL.Save(sQLTrx);
            }
            else
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, "Empty redemption");
                log.Debug("Ends-suspend(message) as Empty redemption");
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();

        }

        /// <summary>
        /// GetManualTickets - Counts
        /// </summary>
        /// <returns></returns>
        internal int GetManualTickets()
        {
            log.LogMethodEntry();
            int manualTickets = 0;
            if (redemptionDTO != null && redemptionDTO.ManualTickets != null)
            {
                manualTickets = Convert.ToInt32(redemptionDTO.ManualTickets);
            }
            log.LogMethodExit(manualTickets);
            return manualTickets;

        }
        public RedemptionDTO RedemptionDTO
        {
            get
            {
                return redemptionDTO;
            }
            set
            {
                redemptionDTO = value;
            }
        }
        /// <summary>
        /// Checks whether card is required for the redemption 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <returns>bool</returns>
        public static bool IsCardRequiredForRedemption(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            bool cardReqForRedemption = !ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_REDEMPTION_WITHOUT_CARD");
            log.LogMethodExit(cardReqForRedemption);
            return cardReqForRedemption;
        }

        internal RedemptionDTO SaveTurnIns(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
            if (redemptionDTO.RedemptionId < 0)
            {
                redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.NEW.ToString();
                redemptionDTO.RedeemedDate = ServerDateTime.Now;
                redemptionDTO.RedemptionOrderNo = Redemption.RedemptionBL.GetNextSeqNo(executionContext.GetMachineId(), sqlTransaction);
                redemptionDTO.POSMachineId = executionContext.GetMachineId();
                redemptionDTO.GraceTickets = 0;
                redemptionDTO = redemptionDataHandler.InsertRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                redemptionDTO.AcceptChanges();
            }
            else
            {
                if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any())
                {
                    RedemptionCardsBL redemptionCardsBL;
                    foreach (RedemptionCardsDTO redemptionCardDTO in redemptionDTO.RedemptionCardsListDTO)
                    {
                        if (redemptionCardDTO.RedemptionId == -1)
                        {
                            redemptionCardDTO.RedemptionId = redemptionDTO.RedemptionId;
                        }
                        if (redemptionCardDTO.CardId >= 0)
                        {
                            AccountBL cardAccount = new AccountBL(executionContext, redemptionCardDTO.CardId, true, true, sqlTransaction);
                            if (cardAccount != null && cardAccount.GetAccountId() != -1)
                            {
                                redemptionCardDTO.TotalCardTickets = cardAccount.GetTotalTickets();
                                redemptionCardDTO.TicketCount = 0;
                                if (string.IsNullOrWhiteSpace(redemptionDTO.PrimaryCardNumber) || redemptionDTO.CardId < 0 || (redemptionDTO.CardId >= 0 && !redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId == redemptionDTO.CardId)))
                                {
                                    AccountBL accountBL = new AccountBL(executionContext, redemptionDTO.RedemptionCardsListDTO[0].CardId, true, true,sqlTransaction);
                                    redemptionDTO.PrimaryCardNumber = accountBL.AccountDTO.TagNumber;
                                    redemptionDTO.CardId = accountBL.AccountDTO.AccountId;
                                    redemptionDTO = redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                                    redemptionDTO.AcceptChanges();
                                }
                            }
                        }
                        redemptionCardsBL = new RedemptionCardsBL(executionContext, redemptionCardDTO);
                        redemptionCardsBL.Save(sqlTransaction);

                    }
                }
                if (redemptionDTO.RedemptionGiftsListDTO != null && redemptionDTO.RedemptionGiftsListDTO.Any())
                {
                    RedemptionGiftsListBL redemptionGiftsListBL;
                    List<RedemptionGiftsDTO> redemptionGiftsListDTOs = new List<RedemptionGiftsDTO>();
                    foreach (RedemptionGiftsDTO gift in redemptionDTO.RedemptionGiftsListDTO)
                    {
                        if (gift.RedemptionId == -1)
                        {
                            gift.RedemptionId = redemptionDTO.RedemptionId;
                        }
                        Semnox.Parafait.Product.ProductBL product = new Semnox.Parafait.Product.ProductBL(executionContext, gift.ProductId,true,true,sqlTransaction);
                        if (gift.ProductQuantity > 1) // For multiple
                        {
                            for (int i = 0; i < gift.ProductQuantity; i++)
                            {
                                RedemptionGiftsDTO redemptionGiftsDTO = new RedemptionGiftsDTO(gift);
                                if (redemptionGiftsDTO.Tickets != null)
                                {
                                    gift.Tickets = product.getProductDTO.TurnInPriceInTickets * -1;
                                    gift.OriginalPriceInTickets = Convert.ToInt32(product.getProductDTO.PriceInTickets * -1);
                                }
                                redemptionGiftsListDTOs.Add(redemptionGiftsDTO);
                            }
                        }
                        else
                        {
                            if (gift.Tickets != null)
                            {
                                gift.Tickets = product.getProductDTO.TurnInPriceInTickets * -1;
                                gift.OriginalPriceInTickets = Convert.ToInt32(product.getProductDTO.PriceInTickets * -1);
                            }
                            redemptionGiftsListDTOs.Add(gift);
                        }
                    }
                    if (redemptionGiftsListDTOs != null && redemptionGiftsListDTOs.Any())
                    {
                        redemptionGiftsListBL = new RedemptionGiftsListBL(executionContext, redemptionGiftsListDTOs);
                        redemptionGiftsListBL.Save(sqlTransaction);
                        LoadRedemptionGiftDTOList(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
            return redemptionDTO;
        }

        internal RedemptionDTO SaveRedemptionOrder(SqlTransaction sQLTrx, bool recalculatediscount = false)
        {
            log.LogMethodEntry(executionContext);
            RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sQLTrx);
            if (redemptionDTO.RedemptionId < 0)
            {
                redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.NEW.ToString();
                redemptionDTO.RedeemedDate = ServerDateTime.Now;
                redemptionDTO.RedemptionOrderNo = Redemption.RedemptionBL.GetNextSeqNo(executionContext.GetMachineId(), sQLTrx);
                redemptionDTO.POSMachineId = executionContext.GetMachineId();
                redemptionDTO.GraceTickets = 0;
                redemptionDTO = redemptionDataHandler.InsertRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                redemptionDTO.AcceptChanges();
            }
            else
            {
                // Save tickets or update
                if (redemptionDTO.TicketReceiptListDTO != null && redemptionDTO.TicketReceiptListDTO.Any())
                {
                    try
                    {
                        foreach (TicketReceiptDTO ticketReceiptDTO in redemptionDTO.TicketReceiptListDTO)
                        {
                            if (ticketReceiptDTO.Id < 0 || ticketReceiptDTO.IsChanged)
                            {
                                if (redemptionDTO.TicketReceiptListDTO.Count(x => x.ManualTicketReceiptNo == ticketReceiptDTO.ManualTicketReceiptNo) > 1)
                                {
                                    log.Error(ticketReceiptDTO.ManualTicketReceiptNo + " " + MessageContainerList.GetMessage(executionContext, 112));
                                    throw new ValidationException(ticketReceiptDTO.ManualTicketReceiptNo + " " + MessageContainerList.GetMessage(executionContext, 1625, ticketReceiptDTO.ManualTicketReceiptNo));
                                }
                                TicketReceipt ticketReceipt = new TicketReceipt(executionContext, ticketReceiptDTO.ManualTicketReceiptNo,sQLTrx);
                                if (ticketReceipt.ValidateTicketReceipts(ticketReceiptDTO.ManualTicketReceiptNo, sQLTrx) == false)
                                {
                                    log.Error(MessageContainerList.GetMessage(executionContext, 1625, ticketReceiptDTO.ManualTicketReceiptNo));
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1625, ticketReceiptDTO.ManualTicketReceiptNo));
                                }
                                TicketStationBL ticketStationBL = null;
                                TicketStationFactory ticketStationFactory = new TicketStationFactory();
                                ticketStationBL = ticketStationFactory.GetTicketStationObject(ticketReceiptDTO.ManualTicketReceiptNo);
                                if (ticketStationBL == null)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2321) +
                                                                   MessageContainerList.GetMessage(executionContext, " Ticket Station"));
                                }
                                else
                                {
                                    if (ticketStationBL.BelongsToThisStation(ticketReceiptDTO.ManualTicketReceiptNo)
                                        && ticketStationBL.ValidCheckBit(ticketReceiptDTO.ManualTicketReceiptNo) == false)
                                    {
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2321) +
                                                                    MessageContainerList.GetMessage(executionContext, " Ticket Station"));
                                    }
                                }
                                if (ticketReceiptDTO.RedemptionId == -1)
                                {
                                    ticketReceiptDTO.RedemptionId = redemptionDTO.RedemptionId;
                                }
                                ticketReceipt = new TicketReceipt(executionContext, ticketReceiptDTO.ManualTicketReceiptNo,sQLTrx);
                                if (ticketReceipt.TicketReceiptDTO != null)
                                {
                                    if (ticketReceipt.TicketReceiptDTO.RedemptionId == -1)
                                    {
                                        ticketReceipt.TicketReceiptDTO.RedemptionId = redemptionDTO.RedemptionId;
                                        ticketReceipt.Save(sQLTrx);
                                    }
                                }
                                else
                                {
                                    ticketReceipt = new TicketReceipt(executionContext, ticketReceiptDTO);
                                    if (ticketReceiptDTO.Id == -1 && ticketStationBL != null)
                                    {
                                        ticketReceiptDTO.BalanceTickets = ticketReceiptDTO.Tickets = ticketStationBL.GetTicketValue(ticketReceiptDTO.ManualTicketReceiptNo);
                                    }
                                    ticketReceipt.Save(sQLTrx);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                //Save Cards or Update
                if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any())
                {
                    // ApplyCurrencyRule();
                    foreach (RedemptionCardsDTO redemptionCardDTO in redemptionDTO.RedemptionCardsListDTO)
                    {
                        if (redemptionCardDTO.RedemptionCardsId < 0 || redemptionCardDTO.IsChanged)
                        {
                            if (redemptionCardDTO.RedemptionId == -1)
                            {
                                redemptionCardDTO.RedemptionId = redemptionDTO.RedemptionId;
                            }
                            if (redemptionCardDTO.CardId >= 0)
                            {
                                AccountBL cardAccount = new AccountBL(executionContext, redemptionCardDTO.CardId, true, true,sQLTrx);
                                if (cardAccount != null && cardAccount.GetAccountId() != -1)
                                {
                                    redemptionCardDTO.TotalCardTickets = cardAccount.GetTotalTickets();
                                    redemptionCardDTO.TicketCount = 0;
                                    if (string.IsNullOrWhiteSpace(redemptionDTO.PrimaryCardNumber) || redemptionDTO.CardId < 0 || (redemptionDTO.CardId >= 0 && !redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId == redemptionDTO.CardId)))
                                    {
                                        AccountBL accountBL = new AccountBL(executionContext, redemptionCardDTO.CardId, true, true, sQLTrx);
                                        redemptionDTO.PrimaryCardNumber = accountBL.AccountDTO.TagNumber;
                                        redemptionDTO.CardId = accountBL.AccountDTO.AccountId;
                                        if (accountBL.AccountDTO.MembershipId > 0)
                                        {
                                            recalculatediscount = true;
                                        }
                                        redemptionDTO = redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                                        redemptionDTO.AcceptChanges();
                                    }
                                }
                            }
                        }
                    }
                    if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.RedemptionCardsId < 0 || x.IsChanged))
                    {
                        RedemptionCardsListBL redemptionCardsListBL = new RedemptionCardsListBL(executionContext, redemptionDTO.RedemptionCardsListDTO.Where(x => x.RedemptionCardsId < 0 || x.IsChanged).ToList());
                        redemptionCardsListBL.Save(sQLTrx);
                        LoadRedemptionCardDTOList(sQLTrx);
                    }                    
                }
                //Save Gifs or Update 
                if (redemptionDTO.RedemptionGiftsListDTO != null && redemptionDTO.RedemptionGiftsListDTO.Any())
                {
                    HashSet<int> giftList = new HashSet<int>(redemptionDTO.RedemptionGiftsListDTO.Where(x => x.RedemptionGiftsId < 0 || x.IsChanged).Select(x => x.ProductId).Distinct());
                    List<int> membershipList = GetMembershipList(sQLTrx);
                    HashSet<int> discountgiftList;
                    if (recalculatediscount)
                    {
                        discountgiftList = new HashSet<int>(redemptionDTO.RedemptionGiftsListDTO.Select(x => x.ProductId).Distinct());
                        recalculatediscount = false;
                    }
                    else
                    {
                        discountgiftList = giftList;
                    }
                    foreach (int gift in giftList)
                    {
                        if (!ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_TRANSACTION_ON_ZERO_STOCK"))
                        {
                            Semnox.Parafait.Product.ProductBL product = new Semnox.Parafait.Product.ProductBL(executionContext, gift,true,true,sQLTrx);
                            int productQty = redemptionDTO.RedemptionGiftsListDTO.Where(x => x.ProductId == gift).Sum(y => y.ProductQuantity);
                            if (!product.IsAvailableInInventory(executionContext,productQty, false,sQLTrx))
                            {
                                string errorMessage = MessageContainerList.GetMessage(executionContext, 1641, gift);
                                throw new ValidationException(errorMessage);
                            }
                        }
                    }
                    foreach (int gift in discountgiftList)
                    {
                        Semnox.Parafait.Product.ProductBL product = new Semnox.Parafait.Product.ProductBL(executionContext, gift,true,true,sQLTrx);
                        if (product.getProductDTO.PriceInTickets != 0)
                        {
                            int discountedPrice = Convert.ToInt32(GetDiscountforProduct(gift, membershipList));
                            redemptionDTO.RedemptionGiftsListDTO.Where(c => c.ProductId == gift).ToList().ForEach(x => x.Tickets = (x.ProductQuantity * discountedPrice));
                        }
                    }
                    if (redemptionDTO.RedemptionGiftsListDTO.Any(x => x.RedemptionGiftsId < 0 || x.IsChanged))
                    {
                        redemptionDTO.RedemptionGiftsListDTO.Where(x => x.RedemptionGiftsId < 0 || x.IsChanged).ToList().ForEach(x => x.RedemptionId = redemptionDTO.RedemptionId);
                        RedemptionGiftsListBL redemptionGiftsListBL = new RedemptionGiftsListBL(executionContext, redemptionDTO.RedemptionGiftsListDTO.Where(x => x.RedemptionGiftsId < 0 || x.IsChanged).ToList());
                        redemptionGiftsListBL.Save(sQLTrx);
                        LoadRedemptionGiftDTOList(sQLTrx);
                    }
                }
            }
            log.LogMethodExit(redemptionDTO);
            return redemptionDTO;
        } 
        private List<int> GetMembershipList(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            List<int> membershipIdList = new List<int>();
            if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
            {
                foreach (RedemptionCardsDTO item in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0))
                {
                    AccountBL accountBL = new AccountBL(executionContext, item.CardId,true,true, sqlTransaction);
                    if (accountBL.AccountDTO.MembershipId > -1)
                    {
                        membershipIdList.Add(accountBL.AccountDTO.MembershipId);
                    }
                }
            }
            log.LogMethodExit(membershipIdList);
            return membershipIdList;
        }
        public decimal GetDiscountforProduct(int productId, List<int> membershipIdList)
        {
            log.LogMethodEntry(productId);
            decimal discountedprice = 0;
            discountedprice = RedemptionPriceContainerList.GetLeastPriceInTickets(executionContext.GetSiteId(), ProductsContainerList.GetActiveProductsContainerDTOList(executionContext.GetSiteId(), ManualProductType.REDEEMABLE.ToString()).FirstOrDefault(x => x.InventoryItemContainerDTO.ProductId == productId).ProductId, membershipIdList);
            log.LogMethodExit(discountedprice);
            return discountedprice;
        }
        public void RedemptionReversalTurnInLimitCheck(int ticketsToBeReversed, string token)
        {
            log.LogMethodEntry(ticketsToBeReversed);
            int mgrApprovalLimit = 0;
            try
            {
                mgrApprovalLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEMPTION_REVERSAL_LIMIT_FOR_MANAGER_APPROVAL"));
            }
            catch { mgrApprovalLimit = 0; }
            if ((ticketsToBeReversed > mgrApprovalLimit && mgrApprovalLimit != 0) || mgrApprovalLimit == 0)
            {

                if (ManagerApprovalReceived(token) == false)
                {
                    log.LogMethodExit("Authentication Error");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                }
            }
            log.LogMethodExit();
        }
        internal RedemptionDTO ReverseRedemption(RedemptionActivityDTO redemptionActivityDTO, Utilities utilities, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            if (redemptionActivityDTO==null || redemptionActivityDTO.ReversalRedemptionGiftDTOList==null || !redemptionActivityDTO.ReversalRedemptionGiftDTOList.Any())
            {
                log.LogMethodExit("No rows selected for reversal");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2666);
                throw new ValidationException(errorMessage);
            }
            List<RedemptionGiftsDTO> selectedGiftLinesForReversal = new List<RedemptionGiftsDTO>();
            int totalTicketsForReversal = 0;
            foreach (RedemptionGiftsDTO redemptionGiftsDTO in redemptionActivityDTO.ReversalRedemptionGiftDTOList)
            {
                if (redemptionDTO.RedemptionGiftsListDTO.Any(x => x.RedemptionGiftsId == redemptionGiftsDTO.RedemptionGiftsId && redemptionGiftsDTO.GiftLineIsReversed == true))
                {
                    log.LogMethodExit("Gift line is already reversed");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1327);
                    throw new ValidationException(errorMessage);
                }
                if (redemptionGiftsDTO.RedemptionGiftsId > -1 && redemptionGiftsDTO.GiftLineIsReversed == false)
                {
                    selectedGiftLinesForReversal.Add(redemptionGiftsDTO);
                    totalTicketsForReversal = totalTicketsForReversal + Convert.ToInt32(redemptionGiftsDTO.Tickets);
                }
            }
            if (selectedGiftLinesForReversal.Count == 0)
            {
                log.LogMethodExit("No rows selected for reversal");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2666);
                throw new ValidationException(errorMessage);
            }

            try
            {
                RedemptionReversalTurnInLimitCheck(totalTicketsForReversal, redemptionActivityDTO.ManagerToken);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit("Reversal approval required");
                throw new ValidationException(ex.Message);
            }

            if (string.Empty.Equals(redemptionActivityDTO.Remarks))
            {
                log.LogMethodExit("Ends-reverseRedemption() as Remarks was not entered for Reversal");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 134);
                throw new ValidationException(errorMessage);
            }
            try
            {
                CreateReversalRedemption(redemptionDTO, selectedGiftLinesForReversal, totalTicketsForReversal, utilities, redemptionActivityDTO.Remarks, redemptionActivityDTO.Source, sqlTransaction);
                if (redemptionDTO.CardId == -1 && totalTicketsForReversal > 0 && redemptionActivityDTO.PrintBalanceTicket)
                {
                    clsTicket clsticket = CreateManualTicketReceipt(totalTicketsForReversal, sqlTransaction, utilities);
                }
                RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO(-1, redemptionDTO.RedemptionId, -1, -1,
                                        executionContext.GetUserId(), ServerDateTime.Now, executionContext.GetMachineId(),
                                        RedemptionUserLogsDTO.RedemptionAction.REVERSAL_REDEMPTION.ToString(),
                                        redemptionDTO.Source, approvalId.ToString(), ServerDateTime.Now);
                RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
                redemptionUserLogsBL.Save(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            List<RedemptionDTO> redemptionDTOList = new List<RedemptionDTO>();
            redemptionDTOList.Add(redemptionDTO);
            RedemptionUseCaseListBL redemptionUseCaseListBL = new RedemptionUseCaseListBL(executionContext, redemptionDTOList);
            redemptionUseCaseListBL.SetToSiteTimeOffset(redemptionDTOList);
            redemptionDTO = redemptionDTOList.FirstOrDefault();
            redemptionDTO.AcceptChanges();
            log.LogMethodExit(redemptionDTO);
            return redemptionDTO;
        }
        public void CreateReversalRedemption(RedemptionDTO originalRedemptionDTO, List<RedemptionGiftsDTO> selectedGiftLinesForReversal,
                                              int totalTicketsForReversal, Utilities utilities, string trxRemarks, string source, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(originalRedemptionDTO, selectedGiftLinesForReversal, totalTicketsForReversal, utilities, trxRemarks, sqlTrx);
            RedemptionDTO reverseRedemptionDTO;
            List<RedemptionGiftsDTO> reverseRedemptionGiftsDTOList = new List<RedemptionGiftsDTO>();
            if (originalRedemptionDTO.CardId > -1)
            {
                RedemptionCardsListBL redemptionCardsListBL = new RedemptionCardsListBL(executionContext);
                List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>(RedemptionCardsDTO.SearchByParameters.REDEMPTION_ID, originalRedemptionDTO.RedemptionId.ToString()));
                List<RedemptionCardsDTO> redemptionCardsDTOList = redemptionCardsListBL.GetRedemptionCardsDTOList(searchParams,sqlTrx);
                if (redemptionCardsDTOList != null && redemptionCardsDTOList.Count > 0)
                {
                    redemptionDTO.RedemptionCardsListDTO = redemptionCardsDTOList;
                }

                AccountBL cardAccount = new AccountBL(executionContext, originalRedemptionDTO.CardId, false, false, sqlTrx);
                if (cardAccount != null && cardAccount.GetAccountId() != -1)
                {
                    if (!cardAccount.IsActive())
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 136));
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 136));
                }
            }

            try
            {
                reverseRedemptionDTO = new RedemptionDTO(-1, originalRedemptionDTO.PrimaryCardNumber, null, (originalRedemptionDTO.CardId > -1 ? totalTicketsForReversal * -1 : (int?)null), ServerDateTime.Now,
                                           originalRedemptionDTO.CardId, originalRedemptionDTO.RedemptionId, trxRemarks, originalRedemptionDTO.GraceTickets * -1, (originalRedemptionDTO.CardId > -1 ? (int?)null : totalTicketsForReversal * -1),
                                           null, executionContext.GetUserId(), executionContext.GetSiteId(), "", false, -1, source, GetNextSeqNo(executionContext.GetMachineId(), sqlTrx), ServerDateTime.Now, ServerDateTime.Now, ServerDateTime.Now,
                                           RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString(), ServerDateTime.Now, executionContext.GetUserId(), null, null, null, null, originalRedemptionDTO.CustomerName, executionContext.GetMachineId(),
                                           originalRedemptionDTO.CustomerId, null, null);

                if (reverseRedemptionDTO.CustomerId == -1 && reverseRedemptionDTO.CardId > -1)
                {
                    AccountDTO originalPrimaryCard = GetRedemptionPrimaryCard(sqlTrx);
                    if (originalPrimaryCard != null)
                    {
                        reverseRedemptionDTO.CustomerId = originalPrimaryCard.CustomerId;
                    }
                }
                RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTrx);
                reverseRedemptionDTO = redemptionDataHandler.InsertRedemption(reverseRedemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                foreach (RedemptionGiftsDTO giftLineForReversal in selectedGiftLinesForReversal)
                {
                    RedemptionGiftsDTO reverseRedemptionGiftsDTO = new RedemptionGiftsDTO(-1, reverseRedemptionDTO.RedemptionId, giftLineForReversal.GiftCode, giftLineForReversal.ProductId, giftLineForReversal.LocationId, giftLineForReversal.Tickets * -1,
                                                                        giftLineForReversal.GraceTickets * -1, -1, executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now,
                                                                        executionContext.GetUserId(), giftLineForReversal.OriginalPriceInTickets * -1, giftLineForReversal.ProductName, giftLineForReversal.ProductDescription,
                                                                        false, giftLineForReversal.RedemptionGiftsId, 1);
                    reverseRedemptionGiftsDTOList.Add(reverseRedemptionGiftsDTO);
                }
                if (reverseRedemptionGiftsDTOList != null && reverseRedemptionGiftsDTOList.Any())
                {
                    RedemptionGiftsListBL redemptionGiftsListBL = new RedemptionGiftsListBL(executionContext, reverseRedemptionGiftsDTOList);
                    redemptionGiftsListBL.Save(sqlTrx);
                }
                if (originalRedemptionDTO.CardId > -1)
                {
                   Loyalty Loyalty = new Loyalty(utilities);
                   Loyalty.CreateGenericCreditPlusLine(originalRedemptionDTO.CardId, "T", totalTicketsForReversal, false, 0, "N", executionContext.GetUserId(), "Redemption Reversal Tickets", sqlTrx, DateTime.MinValue, utilities.getServerTime());
                   
                    RedemptionCardsDTO reverseRedemptionCardsDTO = new RedemptionCardsDTO(-1, reverseRedemptionDTO.RedemptionId, originalRedemptionDTO.PrimaryCardNumber, originalRedemptionDTO.CardId, totalTicketsForReversal * -1, -1,
                                                                       0, executionContext.GetSiteId(), "", false, -1, ServerDateTime.Now, executionContext.GetUserId(), 0, null, ServerDateTime.Now, executionContext.GetUserId(), null, null, null,null);

                    RedemptionCardsBL redemptionCardsBL = new RedemptionCardsBL(executionContext, reverseRedemptionCardsDTO);
                    redemptionCardsBL.Save(sqlTrx);
                    reverseRedemptionDTO.RedemptionCardsListDTO.Add(redemptionCardsBL.RedemptionCardsDTO);
                }
                this.redemptionDTO = reverseRedemptionDTO;
                LoadRedemptionGiftDTOList(sqlTrx);
                foreach (RedemptionGiftsDTO redemptionGiftsDTO in reverseRedemptionDTO.RedemptionGiftsListDTO)
                {
                    SqlCommand sqlCommand = utilities.getCommand(sqlTrx);
                    Semnox.Parafait.Transaction.Inventory.updateStock(redemptionGiftsDTO.GiftCode, sqlCommand, 1 * -1, executionContext.GetMachineId(), executionContext.GetUserId(), redemptionGiftsDTO.RedemptionId, redemptionGiftsDTO.RedemptionGiftsId, 0, 0, "", executionContext.GetSiteId(), redemptionDTO.OrigRedemptionId, redemptionGiftsDTO.OrignialRedemptionGiftId, "Redemption");
                }
                SaveReversalLinesNCreateTicketAllocations(sqlTrx);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit();
        }

        public static string GetNextSeqNo(int posMachineId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(posMachineId, sqlTrx);
            RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTrx);
            string seqNo = redemptionDataHandler.GetNextRedemptionOrderNo(posMachineId, sqlTrx);
            log.LogMethodExit(seqNo);
            return seqNo;
        }
        /// <summary>
        /// Returns the total scanned tickets count
        /// </summary>
        /// <returns>Scanned Ticket Receipt Tickets</returns>
        public int GetScannedTickets()
        {
            log.LogMethodEntry();
            int scanedTickets = 0;
            if (redemptionDTO != null)
            {
                if (redemptionDTO.TicketReceiptListDTO != null && redemptionDTO.TicketReceiptListDTO.Count != 0)
                {
                    foreach (TicketReceiptDTO ticketReceiptDTO in redemptionDTO.TicketReceiptListDTO)
                    {
                        scanedTickets += ticketReceiptDTO.Tickets;
                    }
                }
            }
            log.LogMethodExit(scanedTickets);
            return scanedTickets;
        }
        public void AddManualTickets(RedemptionActivityDTO redemptionActivityDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(redemptionActivityDTO);
            bool managerApprovalReceived = ManagerApprovalReceived(redemptionActivityDTO.ManagerToken);
            RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
            string message = "";
            if (redemptionActivityDTO.Tickets != 0)
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "MANAGER_APPROVAL_TO_ADD_MANUAL_TICKET") && !managerApprovalReceived)
                {
                    message = MessageContainerList.GetMessage(executionContext, 268);
                    log.Debug("Manual ticket: " + (redemptionDTO.ManualTickets + redemptionActivityDTO.Tickets) + "," + message);
                    throw new ValidationException(message);
                }
                ManualTicketLimitChecks(managerApprovalReceived, redemptionActivityDTO.Tickets,sqlTransaction);
            }
            if (redemptionActivityDTO.Tickets > 0 && (redemptionDTO.ManualTickets + redemptionActivityDTO.Tickets) > ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "MAX_MANUAL_TICKETS_PER_REDEMPTION"))
            {
                message = MessageContainerList.GetMessage(executionContext, 2495, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MAX_MANUAL_TICKETS_PER_REDEMPTION"));
                log.Debug("Manual ticket: " + (redemptionDTO.ManualTickets + redemptionActivityDTO.Tickets) + "," + message);
                log.LogMethodExit();
                throw new ValidationException(message);
            }
            try
            {
                if (redemptionActivityDTO.Tickets > 0)
                {
                    PerDayLimitCheckForManualTickets(Convert.ToInt32(redemptionDTO.ManualTickets) + redemptionActivityDTO.Tickets, sqlTransaction);
                }
                else if(redemptionActivityDTO.Tickets < 0)
                {
                    PerDayLimitCheckForReducingManualTickets(Convert.ToInt32(redemptionDTO.ManualTickets) + redemptionActivityDTO.Tickets, sqlTransaction);
                }              
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw ex;
            }
            redemptionDTO.ManualTickets = Convert.ToInt32(redemptionDTO.ManualTickets) + redemptionActivityDTO.Tickets;
            redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO(-1, redemptionDTO.RedemptionId, -1, -1,
                         executionContext.GetUserId(), Convert.ToDateTime(redemptionDTO.RedeemedDate), executionContext.GetMachineId(),
                         RedemptionUserLogsDTO.RedemptionAction.CREATE_MANUAL_TICKET.ToString(),
                         redemptionActivityDTO.Source, approvalId.ToString(), ServerDateTime.Now);
            RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
            redemptionUserLogsBL.Save(sqlTransaction);
            message = MessageContainerList.GetMessage(executionContext, 2682, redemptionActivityDTO.Tickets.ToString());
            log.Info("addManualTickets(" + redemptionActivityDTO.Tickets + "," + message + ") - Manual tickets added ");
            log.Debug("Ends-addManualTickets(" + redemptionActivityDTO.Tickets + "," + message + ")");
            log.LogMethodExit();
        }

        public int GetRedeemedTickets()
        {
            log.LogMethodEntry();
            int redeemed = 0;
            if (redemptionDTO != null && redemptionDTO.RedemptionGiftsListDTO != null && redemptionDTO.RedemptionGiftsListDTO.Any())
            {
                redeemed = redemptionDTO.RedemptionGiftsListDTO.Sum(x => Convert.ToInt32(x.Tickets == null ? 0 : x.Tickets*x.ProductQuantity));
            }
            log.LogMethodExit(redeemed);
            return redeemed;
        }
        public int GetPhysicalTickets()
        {
            log.LogMethodEntry();
            int tickets = 0;
            if (redemptionDTO.TicketReceiptListDTO != null)
            {
                foreach (TicketReceiptDTO item in redemptionDTO.TicketReceiptListDTO)
                {
                    tickets += item.Tickets;
                }
            }
            log.LogMethodExit(tickets);
            return tickets;
        }
        public List<KeyValuePair<int,int>> GetETicketList(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            List<KeyValuePair<int, int>> cardTicketBalance = new List<KeyValuePair<int, int>>();
            AccountBL accountBL = null;            
            if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
            {
                List<RedemptionCardsDTO> filteredDTOList = redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0).ToList();
                foreach (RedemptionCardsDTO item in filteredDTOList)
                {
                    accountBL = new AccountBL(executionContext, item.CardId,true,true,sqlTransaction);
                    cardTicketBalance.Add(new KeyValuePair<int,int>( item.CardId, Convert.ToInt32(accountBL.GetTotalTickets())));
                }
            }            
            log.LogMethodExit(cardTicketBalance);
            return cardTicketBalance;
        }
        public int GetETickets(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            int tickets = 0;
            List<KeyValuePair<int, int>> cardTicketBalance = GetETicketList(sqlTransaction);
            if (cardTicketBalance!=null)
            {
                tickets=cardTicketBalance.Sum(x => x.Value);
            }           
            log.LogMethodExit(tickets);
            return tickets;
        }

        public int GetCurrencyTickets(int redemptionCardsId = -1,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry();
            int tickets = 0;
            if (redemptionDTO.RedemptionCardsListDTO != null)
            {
                if (redemptionCardsId < 0)
                {
                    if (redemptionDTO.RedemptionCardsListDTO.Any(x=>x.CurrencyId>=0))
                    {
                        tickets += Convert.ToInt32(redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0).Sum(y => (y.CurrencyValueInTickets==null?0: y.CurrencyValueInTickets) * (y.CurrencyQuantity==null?0: y.CurrencyQuantity)));
                    }
                    if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyRuleId >= 0))
                    {
                        foreach (RedemptionCardsDTO item in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyRuleId >= 0))
                        {
                            RedemptionCurrencyRuleBL redemptionCurrencyBL = new RedemptionCurrencyRuleBL(executionContext, Convert.ToInt32(item.CurrencyRuleId), true, true, sqlTransaction);
                            tickets += redemptionCurrencyBL.GetRuleDeltaTicket();
                        }
                    }
                }
                else
                {
                    RedemptionCardsDTO item = redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.RedemptionCardsId == redemptionCardsId);
                    if (item.CurrencyId >= 0)
                    {
                        tickets = Convert.ToInt32(item.CurrencyValueInTickets * item.CurrencyQuantity);
                    }
                    if (item.CurrencyRuleId >= 0)
                    {
                        RedemptionCurrencyRuleBL redemptionCurrencyBL = new RedemptionCurrencyRuleBL(executionContext, Convert.ToInt32(item.CurrencyRuleId), true, true);
                        tickets = redemptionCurrencyBL.GetRuleDeltaTicket();
                    }
                }
            }
            log.LogMethodExit(tickets);
            return tickets;
        }
        int GetGraceTickets(int Tickets)
        {
            log.LogMethodEntry(Tickets);
            if (Tickets > 0)
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "REDEMPTION_GRACE_TICKETS") > 0)
                    return ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "REDEMPTION_GRACE_TICKETS");
                else if (ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "REDEMPTION_GRACE_TICKETS_PERCENTAGE") > 0)
                    return Convert.ToInt32(Math.Round((double)(ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "REDEMPTION_GRACE_TICKETS_PERCENTAGE") * Tickets) / 100));
            }

            log.Debug("Ends-getGraceTickets(" + Tickets + ")");
            log.LogMethodExit(0);
            return 0;
        }
        public int GetTotalTickets(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            int etickets = GetETickets(sqlTransaction);
            int physicalTickets = GetPhysicalTickets() + GetCurrencyTickets(-1,sqlTransaction);
            log.Debug("Ends-getTotalTickets()");
            int totalTickets = GetManualTickets() + etickets + physicalTickets + GetGraceTickets(etickets + physicalTickets + GetManualTickets());
            log.LogMethodExit(totalTickets);
            return totalTickets;
        }
        internal void ValidateOrder(bool managerApprovalReceived, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            if (ApplyCurrencyRule(sqlTrx))
            {
                SaveRedemptionOrder(sqlTrx);
            }
            Build(sqlTrx);
            int totalTickets = GetTotalTickets(sqlTrx);
            if (IsCardRequiredForRedemption(executionContext) && !RedemptionHasCards())
            {
                //Please scan a card for redeeming gifts
                log.Error(MessageContainerList.GetMessage(executionContext, 475));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 475));
            }
            if (GetRedeemedTickets() > totalTickets )
            {
                //Redeemed tickets more than availble tickets.Cannot place order
                log.Error(MessageContainerList.GetMessage(executionContext, 1635));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1635));
            }
            TicketReceipt ticketReceipt;
            foreach (TicketReceiptDTO receipt in redemptionDTO.TicketReceiptListDTO)
            {
                ticketReceipt = new TicketReceipt(executionContext, receipt);
                if (ticketReceipt.IsUsedTicketReceipt(sqlTrx))
                {
                    //Ticket Receipt: &1 is alredy used
                    log.Error(MessageContainerList.GetMessage(executionContext, 1625, receipt.ManualTicketReceiptNo));
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1625, receipt.ManualTicketReceiptNo));
                }
            }
            if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
            {
                log.Info("Inside Card update status check");

                foreach (RedemptionCardsDTO redemptionCard in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0))
                {
                    AccountBL cardAccount = new AccountBL(executionContext, redemptionCard.CardId, true, true, sqlTrx);
                    int? totalCardTickets = redemptionCard.TotalCardTickets == null ? 0 : redemptionCard.TotalCardTickets;//added
                    if (cardAccount.GetAccountId() != redemptionCard.CardId || (cardAccount.GetTotalTickets() != totalCardTickets))
                    {
                        log.Error(redemptionCard.CardNumber + " - " + MessageContainerList.GetMessage(executionContext, 354));
                        throw new ValidationException(redemptionCard.CardNumber + " - " + MessageContainerList.GetMessage(executionContext, 354));
                    }
                }
            }
            if (redemptionDTO.RedemptionGiftsListDTO==null || !redemptionDTO.RedemptionGiftsListDTO.Any())
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 119));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 119));
            }

            if (!ParafaitDefaultContainerList.GetParafaitDefault<bool>(ExecutionContext.GetExecutionContext(), "ALLOW_TRANSACTION_ON_ZERO_STOCK", false))
            {
                foreach (int redemptionGift in redemptionDTO.RedemptionGiftsListDTO.Select(x => x.ProductId).Distinct())
                {
                    Semnox.Parafait.Product.ProductBL product = new Semnox.Parafait.Product.ProductBL(executionContext,redemptionGift,true,true,sqlTrx);
                    int productQty = redemptionDTO.RedemptionGiftsListDTO.Where(x => x.ProductId == redemptionGift).Sum(y => y.ProductQuantity);
                    if (!product.IsAvailableInInventory(executionContext,productQty, false,sqlTrx))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1641, product.getProductDTO.ProductName);
                        throw new ValidationException(errorMessage);
                    }
                }
            }
            if (GetRedeemedTickets() > GetTotalTickets(sqlTrx))
            {
                //Sorry, not enough tickets to add the gift
                log.Error(MessageContainerList.GetMessage(executionContext, 1632));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1632));
            }

            int redemptionTransactionTicketMgrLimit = 0;
            try
            {
                redemptionTransactionTicketMgrLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEMPTION_LIMIT_FOR_MANAGER_APPROVAL"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            if (redemptionTransactionTicketMgrLimit > 0 && (GetRedeemedTickets() > redemptionTransactionTicketMgrLimit && managerApprovalReceived == false))
            {
                //Redeem Ticket value entered is above user limit. Please get manager approval
                log.Error(MessageContainerList.GetMessage(executionContext, 1213));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1213));
            }

            int redemptionTransactionTicketLimit = 0;
            try
            {
                redemptionTransactionTicketLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEMPTION_TRANSACTION_TICKET_LIMIT"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            if (redemptionTransactionTicketLimit > 0 && GetRedeemedTickets() > redemptionTransactionTicketLimit)
            {
                //Redeemed ticket value (&1) should not be greater than &2 
                log.Error(MessageContainerList.GetMessage(executionContext, 1438, GetRedeemedTickets(), redemptionTransactionTicketLimit));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1438, GetRedeemedTickets(), redemptionTransactionTicketLimit));
            }
            log.LogMethodExit();
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                // Saves the newly created or updated cardsDTO after updating the currency details
                //ApplyCurrencyRule();
                //SaveRedemptionOrder(sqlTransaction);
                Build(sqlTransaction);
                int currencyTickets = 0;
                int eticketsToBeAllocated = 0;
                int physicalTicketsRedeemed = 0;
                int GraceTickets = 0;
                int manualTicketRedeemed = 0, currencyTicketRedeemed = 0, receiptTicketRedeemed = 0;
                int redeemedPoints = GetRedeemedTickets();
                int ticketsReceipt = GetPhysicalTickets();
                int ticketsCurrency = GetCurrencyTickets(-1,sqlTransaction);
                int ticketsTotalPhysical = ticketsReceipt + ticketsCurrency + Convert.ToInt32(redemptionDTO.ManualTickets);
                int ticketsCard = 0;
                List<KeyValuePair<int, int>> cardTicketBalanceList = GetETicketList(sqlTransaction);
                if (cardTicketBalanceList!=null)
                {
                    ticketsCard = cardTicketBalanceList.Sum(x => x.Value);
                }
                string message = "";
                if (redeemedPoints > (ticketsCard + ticketsTotalPhysical + GetGraceTickets(ticketsCard + ticketsTotalPhysical)))
                {
                    message = MessageContainerList.GetMessage(executionContext, 121, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEMPTION_TICKET_NAME_VARIANT"), ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEMPTION_TICKET_NAME_VARIANT"));
                    log.Info("Ends-UpdateDatabase(message) as redeemed_points > tickets_card + tickets_physical");
                    throw new ValidationException(message);
                }


                RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);

                if ((redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.PREPARED.ToString() || redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString()) && redemptionDTO.OrderCompletedDate == DateTime.MinValue)
                    redemptionDTO.OrderCompletedDate = ServerDateTime.Now;

                if (redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString() && redemptionDTO.OrderDeliveredDate == DateTime.MinValue)
                    redemptionDTO.OrderDeliveredDate = ServerDateTime.Now;

                PerDayLimitCheckForManualTickets(Convert.ToInt32(redemptionDTO.ManualTickets),sqlTransaction);

                int balancePhysicalTickets;
                if (redeemedPoints <= ticketsTotalPhysical)
                {
                    eticketsToBeAllocated = 0;
                    physicalTicketsRedeemed = redeemedPoints;
                    if (GetManualTickets() >= physicalTicketsRedeemed)
                    {
                        manualTicketRedeemed = physicalTicketsRedeemed;
                    }
                    else if (GetManualTickets() + ticketsCurrency >= physicalTicketsRedeemed)
                    {
                        manualTicketRedeemed = GetManualTickets();
                        currencyTicketRedeemed = physicalTicketsRedeemed - manualTicketRedeemed;
                    }
                    else
                    {
                        manualTicketRedeemed = GetManualTickets();
                        currencyTicketRedeemed = ticketsCurrency;
                        receiptTicketRedeemed = physicalTicketsRedeemed - manualTicketRedeemed - currencyTicketRedeemed;
                    }

                    balancePhysicalTickets = ticketsTotalPhysical - physicalTicketsRedeemed;
                }
                else if (redeemedPoints <= ticketsTotalPhysical + ticketsCard)
                {
                    eticketsToBeAllocated = redeemedPoints - ticketsTotalPhysical;
                    physicalTicketsRedeemed = ticketsTotalPhysical;

                    if (GetManualTickets() >= physicalTicketsRedeemed)
                    {
                        manualTicketRedeemed = physicalTicketsRedeemed;
                    }
                    else if (GetManualTickets() + ticketsCurrency >= physicalTicketsRedeemed)
                    {
                        manualTicketRedeemed = GetManualTickets();
                        currencyTicketRedeemed = physicalTicketsRedeemed - manualTicketRedeemed;
                    }
                    else
                    {
                        manualTicketRedeemed = GetManualTickets();
                        currencyTicketRedeemed = ticketsCurrency;
                        receiptTicketRedeemed = physicalTicketsRedeemed - manualTicketRedeemed - currencyTicketRedeemed;
                    }

                    balancePhysicalTickets = 0;
                }
                else // redemption using grace tickets
                {
                    eticketsToBeAllocated = ticketsCard;
                    physicalTicketsRedeemed = ticketsTotalPhysical;

                    if (GetManualTickets() >= physicalTicketsRedeemed)
                    {
                        manualTicketRedeemed = physicalTicketsRedeemed;
                    }
                    else if (GetManualTickets() + ticketsCurrency >= physicalTicketsRedeemed)
                    {
                        manualTicketRedeemed = GetManualTickets();
                        currencyTicketRedeemed = physicalTicketsRedeemed - manualTicketRedeemed;
                    }
                    else
                    {
                        manualTicketRedeemed = GetManualTickets();
                        currencyTicketRedeemed = ticketsCurrency;
                        receiptTicketRedeemed = physicalTicketsRedeemed - manualTicketRedeemed - currencyTicketRedeemed;
                    }
                    GraceTickets = redeemedPoints - ticketsTotalPhysical - ticketsCard;

                    balancePhysicalTickets = 0;
                }
                // Update the redemption 
                if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
                {
                    AccountBL accountBL = new AccountBL(executionContext, redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardId >= 0).CardId, true, true);
                    redemptionDTO.PrimaryCardNumber = accountBL.AccountDTO.TagNumber;
                    redemptionDTO.CardId = accountBL.AccountDTO.AccountId;
                    redemptionDTO.CustomerId = accountBL.AccountDTO.CustomerId;
                }
                redemptionDTO.ReceiptTickets = receiptTicketRedeemed;
                redemptionDTO.CurrencyTickets = currencyTicketRedeemed;
                //redemptionDTO.ManualTickets = manualTicketRedeemed;
                redemptionDTO.ETickets = eticketsToBeAllocated;
                redemptionDTO.GraceTickets = GraceTickets;
                redemptionDTO = redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                redemptionDTO.AcceptChanges();

                // Save manual tickets
                InsertManualTicketReceipts(redemptionDTO.RedemptionId, sqlTransaction);
                if (this.redemptionDTO.OrigRedemptionId == -1)
                {
                    SaveGiftLinesNCreateTicketAllocations(GraceTickets, redeemedPoints, sqlTransaction, cardTicketBalanceList);
                }
                else
                {
                    SaveReversalLinesNCreateTicketAllocations(sqlTransaction);
                }
                redemptionDTO.ManualTickets = manualTicketRedeemed;
                redemptionDTO = redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                redemptionDTO.AcceptChanges();
                if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any())
                {
                    int ticketsUsed = 0;                    
                    foreach (RedemptionCardsDTO redemptionCardDTO in redemptionDTO.RedemptionCardsListDTO)
                    {
                        if (redemptionCardDTO.RedemptionId == -1)
                        {
                            redemptionCardDTO.RedemptionId = redemptionDTO.RedemptionId;
                        }
                        if (redemptionCardDTO.CurrencyId > -1)  // Currency records
                        {
                            currencyTickets = 0;
                            currencyTickets = Convert.ToInt32(redemptionCardDTO.TicketCount * redemptionCardDTO.CurrencyQuantity);
                            if (currencyTickets <= 0)
                                continue;
                            if (redemptionCardDTO.TicketCount == null || redemptionCardDTO.TicketCount != currencyTickets)
                            {
                                redemptionCardDTO.TicketCount = currencyTickets;
                            }
                        }
                        else if (redemptionCardDTO.CardId > -1)
                        {
                            AccountBL cardAccount = new AccountBL(executionContext, redemptionCardDTO.CardId, true, true);
                            if (cardAccount != null && cardAccount.GetAccountId() != -1)
                            {
                                redemptionCardDTO.TotalCardTickets = cardAccount.GetTotalTickets();
                            }
                            if (eticketsToBeAllocated > 0)
                            {
                                currencyTickets = 0;
                                currencyTickets = Convert.ToInt32(cardAccount.GetTotalTickets());
                                if (currencyTickets <= 0)
                                    continue;
                                if (currencyTickets - eticketsToBeAllocated >= 0)
                                {
                                    currencyTickets -= eticketsToBeAllocated;
                                    ticketsUsed = eticketsToBeAllocated;
                                    eticketsToBeAllocated = 0;
                                }
                                else
                                {
                                    eticketsToBeAllocated -= currencyTickets;
                                    ticketsUsed = currencyTickets;
                                    currencyTickets = 0;
                                }

                                try
                                {
                                    using (Utilities utilities = GetUtility())
                                    {
                                        CreditPlus creditPlus = new CreditPlus(utilities);
                                        creditPlus.deductCreditPlusTicketsLoyaltyPoints(redemptionCardDTO.CardNumber, ticketsUsed, 0, sqlTransaction);
                                    }
                                    redemptionCardDTO.TicketCount = ticketsUsed;

                                }
                                catch (Exception Ex)
                                {
                                    message = MessageContainerList.GetMessage(executionContext, 123, Ex.Message);
                                    log.Fatal("Ends-UpdateDatabase(message) due to exception in creating redemption cards information error: " + Ex.Message);
                                    throw Ex;
                                }
                            }
                        }

                    }
                    if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.RedemptionCardsId < 0 || x.IsChanged))
                    {
                        RedemptionCardsListBL redemptionCardsListBL = new RedemptionCardsListBL(executionContext, redemptionDTO.RedemptionCardsListDTO.Where(x => x.RedemptionCardsId < 0 || x.IsChanged).ToList());
                        redemptionCardsListBL.Save(sqlTransaction);
                        LoadRedemptionCardDTOList(sqlTransaction);
                    }
                    if (redemptionDTO.RedemptionCardsListDTO.Any(x=>x.CurrencyId>=0))
                    {
                        foreach (int currency in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0).Select(y=>y.CurrencyId).Distinct())
                        {
                            int currencyqty = Convert.ToInt32(redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId == currency).Sum(y=>(y.CurrencyQuantity==null?0: y.CurrencyQuantity)));
                            using (Utilities utilities = GetUtility())
                            {
                                UpdateRedemptionCurrencyInventory(currency, currencyqty, utilities, sqlTransaction);
                            }
                        }
                    }
                }
                if (redemptionDTO.TicketReceiptListDTO != null)
                {
                    foreach (TicketReceiptDTO ticketReceiptDTO in redemptionDTO.TicketReceiptListDTO)
                    {
                        ticketReceiptDTO.BalanceTickets = ticketReceiptDTO.Tickets;
                    }
                }

                if (balancePhysicalTickets > 0)
                {
                    if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
                    {
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AUTO_LOAD_BALANCE_TICKETS_TO_CARD").Equals("Y")
                            || redemptionDTO.TicketReceiptListDTO.Any(x => x.BalanceTickets > 0) || LoadBalanceTickettoCard)
                        {
                            using (Utilities utilities = GetUtility())
                            {
                                TaskProcs tp = new TaskProcs(utilities);
                                int mgrApprovalLimit = 0;
                                try
                                {
                                    mgrApprovalLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL"));
                                }
                                catch { mgrApprovalLimit = 0; }
                                if ((balancePhysicalTickets > mgrApprovalLimit && mgrApprovalLimit != 0))
                                {
                                    if (ManagerApprovalReceived(managerToken) == false)
                                    {
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                                    }
                                }
                                int originalManagerId = -1;
                                if (!string.IsNullOrWhiteSpace(approvalId))
                                {
                                    originalManagerId = utilities.ParafaitEnv.ManagerId;
                                    utilities.ParafaitEnv.ManagerId = UserContainerList.GetUserContainerDTOOrDefault(approvalId, "", executionContext.GetSiteId()).UserId;
                                }
                                if (tp.loadTickets(new Card(redemptionDTO.PrimaryCardNumber, "", utilities), balancePhysicalTickets, "Redemption balance tickets", redemptionDTO.RedemptionId, ref message, sqlTransaction))
                                {
                                    message = MessageContainerList.GetMessage(executionContext, 36, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEMPTION_TICKET_NAME_VARIANT"));
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(approvalId))
                                    {
                                        utilities.ParafaitEnv.ManagerId = originalManagerId; // Reset back the manager Id
                                    }
                                    throw new Exception(message);
                                }
                                if (!string.IsNullOrWhiteSpace(approvalId))
                                {
                                    utilities.ParafaitEnv.ManagerId = originalManagerId; // Reset back the manager Id
                                }
                            }
                        }
                        else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AUTO_PRINT_BALANCE_TICKETS").Equals("Y")
                             || redemptionDTO.TicketReceiptListDTO.Any(x => x.BalanceTickets > 0) || PrintBalanceTicket)
                        {
                            if (balancePhysicalTickets > ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "LOAD_TICKETS_LIMIT"))
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2830, balancePhysicalTickets, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"), ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOAD_TICKETS_LIMIT")));
                            }
                            int mgrApprovalLimit = 0;
                            try
                            {
                                mgrApprovalLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL"));
                            }
                            catch { mgrApprovalLimit = 0; }
                            if ((balancePhysicalTickets > mgrApprovalLimit && mgrApprovalLimit != 0))
                            {
                                if (ManagerApprovalReceived(managerToken) == false)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                                }
                            }
                            PrintBalanceTickets(balancePhysicalTickets, sqlTransaction);
                        }

                    }
                    else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AUTO_PRINT_BALANCE_TICKETS").Equals("Y")
                         || redemptionDTO.TicketReceiptListDTO.Any(x => x.BalanceTickets > 0) || PrintBalanceTicket)
                    {
                        if (balancePhysicalTickets > ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "LOAD_TICKETS_LIMIT"))
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2830, balancePhysicalTickets, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"), ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOAD_TICKETS_LIMIT")));
                        }
                        int mgrApprovalLimit = 0;
                        try
                        { mgrApprovalLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL")); }
                        catch (Exception ex) { mgrApprovalLimit = 0; }

                        if ((balancePhysicalTickets > mgrApprovalLimit && mgrApprovalLimit != 0))
                        {
                            if (ManagerApprovalReceived(managerToken) == false)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                            }
                        }
                        PrintBalanceTickets(balancePhysicalTickets, sqlTransaction);
                    }
                    RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO(-1, redemptionDTO.RedemptionId, -1, -1,
                          executionContext.GetUserId(), Convert.ToDateTime(redemptionDTO.RedeemedDate), executionContext.GetMachineId(),
                          RedemptionUserLogsDTO.RedemptionAction.REDEMPTION.ToString(),
                          redemptionDTO.Source, approvalId.ToString(), ServerDateTime.Now);
                    RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
                    redemptionUserLogsBL.Save(sqlTransaction);

                    log.LogMethodExit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void PrintBalanceTickets(int balancePhysicalTickets, SqlTransaction sqlTransaction)
        {
            try
            {
                string BarCodeText = string.Empty;
                TicketStationFactory ticketStationFactory = new TicketStationFactory();
                POSCounterTicketStationBL posCounterTicketStationBL = ticketStationFactory.GetPosCounterTicketStationObject();
                if (posCounterTicketStationBL == null)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2322));
                }
                else
                {
                    BarCodeText = posCounterTicketStationBL.GenerateBarCode(balancePhysicalTickets);
                }
                // Create the new receipt for balance
                int newReceiptId = InsertPhysicalReceiptToDB(redemptionDTO.RedemptionId, BarCodeText, balancePhysicalTickets, sqlTransaction, ServerDateTime.Now);
                // Add it to the allocation
                //RedemptionTicketAllocationDTO redemptionTicketAllocationDTO = new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, -1, null,
                //null, -1, null, -1, null, null, BarCodeText, balancePhysicalTickets, null, newReceiptId, -1, -1, -1, null, -1);
                //RedemptionTicketAllocationBL redemptionTicketAllocationBL = new RedemptionTicketAllocationBL(executionContext, redemptionTicketAllocationDTO);
                //redemptionTicketAllocationBL.Save(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private Utilities GetUtility()
        {
            Utilities Utilities = new Utilities();
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
            Utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            if (string.IsNullOrWhiteSpace(executionContext.POSMachineName))
            {
                POSMachines pOSMachineContainerDTO = new POSMachines(executionContext, executionContext.GetMachineId());
                if (pOSMachineContainerDTO.POSMachineDTO != null)
                {
                    executionContext.POSMachineName = pOSMachineContainerDTO.POSMachineDTO.POSName;
                    Utilities.ParafaitEnv.SetPOSMachine("", executionContext.POSMachineName);
                }
            }
            else
            {
                Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            }
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            Utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            Utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            Utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            Utilities.ParafaitEnv.Initialize();
            return Utilities;
        }

        internal void DeleteRedemptionGifts(List<RedemptionGiftsDTO> redemptionGiftsDTOList, SqlTransaction sQLTrx)
        {
            log.LogMethodEntry();
            foreach (RedemptionGiftsDTO redemptionGiftsDTO in redemptionGiftsDTOList)
            {
                RedemptionGiftsBL redemptionGiftsBL = new RedemptionGiftsBL(executionContext, redemptionGiftsDTO);
                redemptionGiftsBL.Delete(sQLTrx);
            }
            log.LogMethodExit();
        }

        internal void DeleteRedemptionCards(List<RedemptionCardsDTO> redemptionCardsDTOList, SqlTransaction sQLTrx)
        {
            log.LogMethodEntry();
            foreach (RedemptionCardsDTO redemptionCardsDTO in redemptionCardsDTOList)
            {
                RedemptionCardsBL redemptionCardsBL = new RedemptionCardsBL(executionContext, redemptionCardsDTO);
                redemptionCardsBL.Delete(sQLTrx);
            }
            log.LogMethodExit();
        }



        private void SaveGiftLinesNCreateTicketAllocations(int GraceTickets, int redeemedPoints, SqlTransaction sqlTrx,List<KeyValuePair<int,int>> cardTicketBalance)
        {
            log.LogMethodEntry(sqlTrx);
            int manualTicketsRemaining = GetManualTickets();
            bool receiptBalanceExists = true, cardBalanceExists = true;
            bool currencyBalanceExists = true;
            int graceTicketRemaining = Convert.ToInt32(redemptionDTO.GraceTickets > 0 ? redemptionDTO.GraceTickets : 0);
            List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList = new List<RedemptionTicketAllocationDTO>();
            HashSet<int> usedCurrencies = new HashSet<int>();
            HashSet<int> usedCurrencyRules = new HashSet<int>();
            if (redemptionDTO.RedemptionGiftsListDTO != null && redemptionDTO.RedemptionGiftsListDTO.Any())
            {
                foreach (RedemptionGiftsDTO gift in redemptionDTO.RedemptionGiftsListDTO)
                {
                    int price = 0;
                    int grace = 0;
                    int balGraceTickets = GraceTickets;
                    if (GraceTickets > 0)
                    {
                        price = Convert.ToInt32(gift.Tickets);
                        grace = 0;
                        if (gift.Equals(redemptionDTO.RedemptionGiftsListDTO.Last())) // last line
                        {
                            price = price - balGraceTickets;
                            grace = balGraceTickets;
                        }
                        else
                        {
                            grace = price * GraceTickets / redeemedPoints;
                            price = price - grace;
                            balGraceTickets -= grace;
                        }
                        gift.Tickets = price;
                        gift.GraceTickets = grace;
                    }
                    else
                    {
                        price = Convert.ToInt32(gift.Tickets);
                        grace = 0;
                        gift.Tickets = price;
                        gift.GraceTickets = grace;
                    }
                    using (Utilities utilities = GetUtility())
                    {
                        using (SqlCommand sqlCommand = utilities.getCommand(sqlTrx))
                        {
                            Semnox.Parafait.Transaction.Inventory.updateStock(gift.GiftCode, sqlCommand, gift.ProductQuantity, executionContext.GetMachineId(), executionContext.GetUserId(), gift.RedemptionId, gift.RedemptionGiftsId, 0, 0, "", executionContext.GetSiteId(), -1, -1, "Redemption");
                        }
                    }
                    int totalGiftTicketsToAllocate = Convert.ToInt32(gift.Tickets);
                    while (totalGiftTicketsToAllocate != 0)
                    {
                        if (manualTicketsRemaining > 0)
                        {
                            int manualTicketsUsed = 0;
                            if (manualTicketsRemaining > totalGiftTicketsToAllocate)
                            {
                                manualTicketsUsed = totalGiftTicketsToAllocate;
                            }
                            else
                            {
                                manualTicketsUsed = manualTicketsRemaining;
                            }
                            totalGiftTicketsToAllocate = totalGiftTicketsToAllocate - manualTicketsUsed;
                            manualTicketsRemaining = manualTicketsRemaining - manualTicketsUsed;
                            redemptionTicketAllocationDTOList.Add( new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, gift.RedemptionGiftsId, manualTicketsUsed, null, -1, null, -1, null, null, null,
                                                                                                           null, null, executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(),
                                                                                                           -1, -1, -1, -1, null, -1));                            
                        }
                        else if (currencyBalanceExists == true)
                        {
                            currencyBalanceExists = false;
                            int ticketsUsed = 0;
                            if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyId >= 0))
                            {
                                int fromRedemptionCardsId = -1;
                                if (usedCurrencies != null && usedCurrencies.Any())
                                {
                                    fromRedemptionCardsId = usedCurrencies.Max();
                                }
                                foreach (RedemptionCardsDTO cardDTO in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0 && x.RedemptionCardsId > fromRedemptionCardsId).OrderBy(x => x.RedemptionCardsId))
                                {
                                    if (totalGiftTicketsToAllocate > 0)
                                    {
                                        cardDTO.TicketCount = GetCurrencyRemainingTickets(cardDTO.RedemptionCardsId, redemptionTicketAllocationDTOList);
                                        if (cardDTO.TicketCount <= 0)
                                        {
                                            usedCurrencies.Add(cardDTO.RedemptionCardsId);
                                        }
                                        if (cardDTO.TicketCount > 0)
                                        {
                                            if (cardDTO.TicketCount > totalGiftTicketsToAllocate)
                                                ticketsUsed = totalGiftTicketsToAllocate;
                                            else
                                                ticketsUsed = Convert.ToInt32(cardDTO.TicketCount);
                                            totalGiftTicketsToAllocate = totalGiftTicketsToAllocate - ticketsUsed;
                                            //cardDTO.TicketCount = cardDTO.TicketCount - ticketsUsed;
                                            decimal currencyqty = 0.00m;
                                            if (cardDTO.CurrencyValueInTickets != null && cardDTO.CurrencyValueInTickets > 0)
                                            {
                                                currencyqty = ((decimal)ticketsUsed / (decimal)cardDTO.CurrencyValueInTickets);
                                            }
                                            currencyBalanceExists = true;
                                            redemptionTicketAllocationDTOList.Add ( new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, gift.RedemptionGiftsId, null, null, -1, null, Convert.ToInt32(cardDTO.CurrencyId), currencyqty, ticketsUsed, null, null, null,
                                                                                                              executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(), -1, -1, -1, -1, null, (cardDTO.SourceCurrencyRuleId == null) ? -1 : Convert.ToInt32(cardDTO.SourceCurrencyRuleId)));                                                                                    }
                                    }
                                    else
                                        break;
                                }
                                ticketsUsed = 0;
                                if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyRuleId >= 0))
                                {
                                    int fromRedemptionCardsIdRule = -1;
                                    if (usedCurrencyRules != null && usedCurrencyRules.Any())
                                    {
                                        fromRedemptionCardsIdRule = usedCurrencyRules.Max();
                                    }
                                    foreach (RedemptionCardsDTO cardDTO in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyRuleId >= 0 && x.RedemptionCardsId > fromRedemptionCardsIdRule).OrderBy(x => x.RedemptionCardsId))
                                    {
                                        if (totalGiftTicketsToAllocate > 0)
                                        {
                                            //cardDTO.TicketCount = GetCurrencyTickets(cardDTO.RedemptionCardsId);
                                            cardDTO.TicketCount = GetCurrencyRuleRemainingTickets(cardDTO.RedemptionCardsId, redemptionTicketAllocationDTOList);
                                            if (cardDTO.TicketCount <= 0)
                                            {
                                                usedCurrencyRules.Add(cardDTO.RedemptionCardsId);
                                            }
                                            if (cardDTO.TicketCount > 0)
                                            {
                                                if (cardDTO.TicketCount > totalGiftTicketsToAllocate)
                                                    ticketsUsed = totalGiftTicketsToAllocate;
                                                else
                                                    ticketsUsed = Convert.ToInt32(cardDTO.TicketCount);
                                                totalGiftTicketsToAllocate = totalGiftTicketsToAllocate - ticketsUsed;
                                                //cardDTO.TicketCount = cardDTO.TicketCount - ticketsUsed;
                                                decimal currencyqty = 0.00m;
                                                int deltaticket = GetCurrencyTickets(cardDTO.RedemptionCardsId,sqlTrx);
                                                if (deltaticket > 0)
                                                {
                                                    currencyqty = ((decimal)ticketsUsed / (decimal)deltaticket);
                                                }
                                                currencyBalanceExists = true;
                                                redemptionTicketAllocationDTOList.Add(new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, gift.RedemptionGiftsId, null, null, -1, null, -1, currencyqty, null, null, null, null,
                                                                                                                  executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(), -1, -1, -1, Convert.ToInt32(cardDTO.CurrencyRuleId), ticketsUsed, -1));
                                                
                                            }
                                        }
                                        else
                                            break;
                                    }
                                }
                            }
                        }
                        else if (receiptBalanceExists == true)
                        {
                            receiptBalanceExists = false;
                            int ticketsUsed = 0;
                            if (redemptionDTO.TicketReceiptListDTO != null && redemptionDTO.TicketReceiptListDTO.Any(x => x.RedemptionId == redemptionDTO.RedemptionId))
                            {
                                foreach (TicketReceiptDTO ticketReceiptDTO in redemptionDTO.TicketReceiptListDTO.Where(x => x.RedemptionId == redemptionDTO.RedemptionId))
                                {
                                    if (totalGiftTicketsToAllocate > 0)
                                    {
                                        if (ticketReceiptDTO.BalanceTickets > 0)
                                        {
                                            if (ticketReceiptDTO.BalanceTickets > totalGiftTicketsToAllocate)
                                                ticketsUsed = totalGiftTicketsToAllocate;
                                            else
                                                ticketsUsed = ticketReceiptDTO.BalanceTickets;
                                            totalGiftTicketsToAllocate = totalGiftTicketsToAllocate - ticketsUsed;
                                            ticketReceiptDTO.BalanceTickets = ticketReceiptDTO.BalanceTickets - ticketsUsed;
                                            receiptBalanceExists = true;
                                            redemptionTicketAllocationDTOList.Add( new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, gift.RedemptionGiftsId, null, null, -1, null, -1, null, null, ticketReceiptDTO.ManualTicketReceiptNo,
                                                                                                               ticketsUsed, null, executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(),
                                                                                                               ticketReceiptDTO.Id, -1, -1, -1, null, -1));                                            
                                        }
                                    }
                                    else
                                        break;
                                }
                            }
                        }
                        else if (cardBalanceExists == true)
                        {
                            cardBalanceExists = false;
                            int ticketsUsed = 0;
                            if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
                            {
                                foreach (RedemptionCardsDTO cardDTO in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0))
                                {
                                    int cardTickets = 0;
                                    if (totalGiftTicketsToAllocate > 0)
                                    {
                                        if (cardTicketBalance!=null)
                                        {
                                            if (cardTicketBalance.Any(x => x.Key == cardDTO.CardId))
                                            {
                                                cardTickets = cardTicketBalance.FirstOrDefault(x => x.Key == cardDTO.CardId).Value;
                                            }
                                            if (redemptionTicketAllocationDTOList!=null && redemptionTicketAllocationDTOList.Any(x=>x.CardId== cardDTO.CardId))
                                            {
                                                cardTickets = cardTickets - redemptionTicketAllocationDTOList.Where(x => x.CardId == cardDTO.CardId).Sum(y => Convert.ToInt32(y.ETickets == null ? 0 : y.ETickets));
                                            }
                                        }
                                        cardDTO.TicketCount = cardTickets;
                                        if (cardDTO.TicketCount > 0)
                                        {
                                            if (cardDTO.TicketCount > totalGiftTicketsToAllocate)
                                                ticketsUsed = totalGiftTicketsToAllocate;
                                            else
                                                ticketsUsed = Convert.ToInt32(cardDTO.TicketCount);
                                            totalGiftTicketsToAllocate = totalGiftTicketsToAllocate - ticketsUsed;
                                            cardDTO.TicketCount = cardDTO.TicketCount - ticketsUsed;
                                            cardBalanceExists = true;
                                            redemptionTicketAllocationDTOList.Add( new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, gift.RedemptionGiftsId, null, null, cardDTO.CardId, ticketsUsed, -1, null, null, null, null, null,
                                                                                                              executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(), -1, -1, -1, -1, null, -1));                                            

                                        }
                                    }
                                    else
                                        break;
                                }
                            }
                        }

                    }
                    if (Convert.ToInt32(gift.GraceTickets) > 0)
                    {
                        if (graceTicketRemaining > 0)
                        {
                            int graceTicketsUsed = 0;
                            if (graceTicketRemaining > gift.GraceTickets)
                                graceTicketsUsed = Convert.ToInt32(gift.GraceTickets);
                            else
                                graceTicketsUsed = graceTicketRemaining;

                            redemptionTicketAllocationDTOList.Add(new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, gift.RedemptionGiftsId, 0, graceTicketsUsed, -1, null, -1, null, null, null, null, null,
                                                                                               executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(),
                                                                                               -1, -1, -1, -1, null, -1));                            

                        }
                    }
                }
                if (manualTicketsRemaining > 0 && (PrintBalanceTicket == true || LoadBalanceTickettoCard == true || redemptionDTO.TicketReceiptListDTO.Any(x => x.BalanceTickets > 0)))
                {   //Note for future implementation
                    //UI needs to make sure that whether the balance tickes left should be added to card or ticket receipt. 
                    //manualticket value should be set accordingly
                    redemptionTicketAllocationDTOList.Add(new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, -1, manualTicketsRemaining, null, -1, null, -1, null, null, null,
                                                                                                           null, null, executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(),
                                                                                                           -1, -1, -1, -1, null, -1));                    

                }
            }

            if (redemptionDTO.TicketReceiptListDTO != null)
            {
                foreach (TicketReceiptDTO ticketReceiptDTO in redemptionDTO.TicketReceiptListDTO.Where(x => x.RedemptionId == redemptionDTO.RedemptionId))
                {
                    if (ticketReceiptDTO.BalanceTickets > 0 && (PrintBalanceTicket == true || LoadBalanceTickettoCard == true))
                    {
                        redemptionTicketAllocationDTOList.Add (new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, -1, null, null, -1, null, -1, null, null, ticketReceiptDTO.ManualTicketReceiptNo, ticketReceiptDTO.BalanceTickets, null,
                                                                                          executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(), ticketReceiptDTO.Id, -1, -1, -1, null, -1));                        

                    }
                }
            }
            if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId < 0) && (PrintBalanceTicket == true || LoadBalanceTickettoCard == true || redemptionDTO.TicketReceiptListDTO.Any(x => x.BalanceTickets > 0)))
            {
                if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyId >= 0))
                {
                    foreach (RedemptionCardsDTO cardDTO in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0))
                    {
                        int balanceTickets = GetCurrencyRemainingTickets(cardDTO.RedemptionCardsId, redemptionTicketAllocationDTOList);
                        if (balanceTickets > 0)
                        {
                            decimal currencyqty = 0.00m;
                            if (cardDTO.CurrencyValueInTickets != null && cardDTO.CurrencyValueInTickets > 0)
                            {
                                currencyqty = ((decimal)balanceTickets / (decimal)cardDTO.CurrencyValueInTickets);
                            }
                            redemptionTicketAllocationDTOList.Add( new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, -1, null, null, -1, null, Convert.ToInt32(cardDTO.CurrencyId), currencyqty, balanceTickets, null, null, null,
                                                                                              executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(), -1, -1, -1, -1, null, (cardDTO.SourceCurrencyRuleId == null) ? -1 : Convert.ToInt32(cardDTO.SourceCurrencyRuleId)));                            
                        }
                    }
                }
                if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyRuleId >= 0))
                {
                    foreach (RedemptionCardsDTO cardDTO in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyRuleId >= 0))
                    {
                        int balanceTickets = GetCurrencyRuleRemainingTickets(cardDTO.RedemptionCardsId, redemptionTicketAllocationDTOList);
                        if (balanceTickets > 0)
                        {
                            decimal currencyqty = 0.00m;
                            int deltaticket = GetCurrencyTickets(cardDTO.RedemptionCardsId,sqlTrx);
                            if (deltaticket > 0)
                            {
                                currencyqty = ((decimal)balanceTickets / (decimal)deltaticket);
                            }
                            redemptionTicketAllocationDTOList.Add ( new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, -1, null, null, -1, null, -1, currencyqty, null, null, null, null,
                                                                                                                  executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(), -1, -1, -1, Convert.ToInt32(cardDTO.CurrencyRuleId), balanceTickets, -1));                            

                        }
                    }
                }
            }
            if (redemptionTicketAllocationDTOList != null && redemptionTicketAllocationDTOList.Any())
            {
                RedemptionTicketAllocationListBL redemptionTicketAllocationListBL = new RedemptionTicketAllocationListBL(executionContext, redemptionTicketAllocationDTOList);
                redemptionTicketAllocationListBL.Save(sqlTrx);
                LoadRedemptionTicketAllocationDTOList(sqlTrx);
            }
            log.LogMethodExit();
        }
        internal int GetCurrencyRemainingTickets(int redemptionCardsId,List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList)
        {
            log.LogMethodEntry();
            int tickets = 0;

            if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.RedemptionCardsId == redemptionCardsId))
            {
                RedemptionCardsDTO item = redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.RedemptionCardsId == redemptionCardsId);
                if (item.CurrencyId >= 0)
                {
                    //tickets = Convert.ToInt32(redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId == item.CurrencyId && x.CurrencyQuantity != null && x.CurrencyValueInTickets != null).Select(x => x.CurrencyQuantity* x.CurrencyValueInTickets).Sum());
                    tickets = Convert.ToInt32(redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId == item.CurrencyId && x.CurrencyQuantity != null && x.CurrencyValueInTickets != null && x.RedemptionCardsId <= redemptionCardsId).Select(x => x.CurrencyQuantity * x.CurrencyValueInTickets).Sum());
                    if (redemptionTicketAllocationDTOList.Any(x => x.CurrencyId == item.CurrencyId && x.CurrencyTickets != null && x.CurrencyTickets > 0))
                    {
                        int usedTickets = Convert.ToInt32(redemptionTicketAllocationDTOList.Where(x => x.CurrencyId == item.CurrencyId && x.CurrencyTickets != null && x.CurrencyTickets > 0).Select(x => x.CurrencyTickets).Sum());
                        tickets = tickets - usedTickets;
                    }
                }
            }
            log.LogMethodExit(tickets);
            return tickets;
        }
        internal int GetCurrencyRuleRemainingTickets(int redemptionCardsId, List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList)
        {
            log.LogMethodEntry();
            int tickets = 0;

            if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.RedemptionCardsId == redemptionCardsId))
            {
                RedemptionCardsDTO item = redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.RedemptionCardsId == redemptionCardsId);
                if (item.CurrencyRuleId >= 0)
                {
                    RedemptionCurrencyRuleBL redemptionCurrencyBL = new RedemptionCurrencyRuleBL(executionContext, Convert.ToInt32(item.CurrencyRuleId), true, true);
                    foreach (RedemptionCardsDTO rcards in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyRuleId == item.CurrencyRuleId && x.RedemptionCardsId <= redemptionCardsId))
                    {
                        tickets += redemptionCurrencyBL.GetRuleDeltaTicket();
                    }
                    if (redemptionTicketAllocationDTOList.Any(x => x.RedemptionCurrencyRuleId == item.CurrencyRuleId && x.RedemptionCurrencyRuleTicket != null && x.RedemptionCurrencyRuleTicket > 0))
                    {
                        int usedTickets = Convert.ToInt32(redemptionTicketAllocationDTOList.Where(x => x.RedemptionCurrencyRuleId == item.CurrencyRuleId && x.RedemptionCurrencyRuleTicket != null && x.RedemptionCurrencyRuleTicket > 0).Select(x => x.RedemptionCurrencyRuleTicket).Sum());
                        tickets = tickets - usedTickets;
                    }
                }
            }
            log.LogMethodExit(tickets);
            return tickets;
        }


        /// <summary>
        /// SaveReversalLinesNCreateTicketAllocations
        /// </summary>
        /// <param name="sqlTrx">sqlTrx</param>
        internal void SaveReversalLinesNCreateTicketAllocations(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            RedemptionTicketAllocationListBL reversalredemptionTicketAllocationListBL = null;
            RedemptionTicketAllocationListBL redemptionTicketAllocationListBL = new RedemptionTicketAllocationListBL(executionContext);
            List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>> redemptionTASearchParam = new List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>>();
            redemptionTASearchParam.Add(new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_ID, this.redemptionDTO.OrigRedemptionId.ToString()));
            redemptionTASearchParam.Add(new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<RedemptionTicketAllocationDTO> originalRTADTOList = redemptionTicketAllocationListBL.GetRedemptionTicketAllocationDTOList(redemptionTASearchParam,sqlTrx);
            if (redemptionDTO != null && redemptionDTO.RedemptionGiftsListDTO != null && redemptionDTO.RedemptionGiftsListDTO.Any())
            {
                List<RedemptionTicketAllocationDTO> reverseRedemptionTicketAllocationDTOs = new List<RedemptionTicketAllocationDTO>();
                foreach (RedemptionGiftsDTO giftLineForReversal in redemptionDTO.RedemptionGiftsListDTO)
                {
                    //if (giftLineForReversal.RedemptionId == -1)
                    //{
                    //    giftLineForReversal.RedemptionId = redemptionDTO.RedemptionId;
                    //}
                    //redemptionGiftsBL = new RedemptionGiftsBL(executionContext, giftLineForReversal);
                    //redemptionGiftsBL.Save(sqlTrx);

                    if (originalRTADTOList != null)
                    {
                        List<RedemptionTicketAllocationDTO> originalGiftLineRTADTOList = originalRTADTOList.FindAll(rta => (rta.RedemptionGiftId == giftLineForReversal.OrignialRedemptionGiftId && rta.RedemptionId == redemptionDTO.OrigRedemptionId));
                        if (originalGiftLineRTADTOList != null)
                        {
                            foreach (RedemptionTicketAllocationDTO originalGiftLineRTADTO in originalGiftLineRTADTOList)
                            {
                                RedemptionTicketAllocationDTO reversalGiftRTADTO = new RedemptionTicketAllocationDTO(-1, giftLineForReversal.RedemptionId, giftLineForReversal.RedemptionGiftsId, (originalGiftLineRTADTO.ManualTickets == null ? null : originalGiftLineRTADTO.ManualTickets * -1),
                                                                                       (originalGiftLineRTADTO.GraceTickets == null ? null : originalGiftLineRTADTO.GraceTickets * -1), originalGiftLineRTADTO.CardId, (originalGiftLineRTADTO.ETickets == null ? null : originalGiftLineRTADTO.ETickets * -1), originalGiftLineRTADTO.CurrencyId,
                                                                                       originalGiftLineRTADTO.CurrencyQuantity, (originalGiftLineRTADTO.CurrencyTickets == null ? null : originalGiftLineRTADTO.CurrencyTickets * -1), originalGiftLineRTADTO.ManualTicketReceiptNo, (originalGiftLineRTADTO.ReceiptTickets == null ? null : originalGiftLineRTADTO.ReceiptTickets * -1),
                                                                                       (originalGiftLineRTADTO.TurnInTickets == null ? null : originalGiftLineRTADTO.TurnInTickets * -1), executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(), originalGiftLineRTADTO.ManualTicketReceiptId, -1, -1, originalGiftLineRTADTO.RedemptionCurrencyRuleId, originalGiftLineRTADTO.RedemptionCurrencyRuleTicket, originalGiftLineRTADTO.SourceCurrencyRuleId);//DTO_Change
                                reverseRedemptionTicketAllocationDTOs.Add(reversalGiftRTADTO);
                                
                                RedemptionTicketAllocationDTO reversalTicketRTADTO = new RedemptionTicketAllocationDTO(-1, giftLineForReversal.RedemptionId, -1, (originalGiftLineRTADTO.ManualTickets == null ? null : originalGiftLineRTADTO.ManualTickets), (originalGiftLineRTADTO.GraceTickets == null ? null : originalGiftLineRTADTO.GraceTickets),
                                                                                         originalGiftLineRTADTO.CardId, (originalGiftLineRTADTO.ETickets == null ? null : originalGiftLineRTADTO.ETickets), originalGiftLineRTADTO.CurrencyId, originalGiftLineRTADTO.CurrencyQuantity,
                                                                                         (originalGiftLineRTADTO.CurrencyTickets == null ? null : originalGiftLineRTADTO.CurrencyTickets), originalGiftLineRTADTO.ManualTicketReceiptNo, (originalGiftLineRTADTO.ReceiptTickets == null ? null : originalGiftLineRTADTO.ReceiptTickets), (originalGiftLineRTADTO.TurnInTickets == null ? null : originalGiftLineRTADTO.TurnInTickets), executionContext.GetSiteId(), -1, false,
                                                                                         "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(), originalGiftLineRTADTO.ManualTicketReceiptId, -1, -1, originalGiftLineRTADTO.RedemptionCurrencyRuleId, originalGiftLineRTADTO.RedemptionCurrencyRuleTicket, originalGiftLineRTADTO.SourceCurrencyRuleId);
                                reverseRedemptionTicketAllocationDTOs.Add(reversalTicketRTADTO);
                            }
                        }
                    }
                }
                if (reverseRedemptionTicketAllocationDTOs != null && reverseRedemptionTicketAllocationDTOs.Any())
                {
                    reversalredemptionTicketAllocationListBL = new RedemptionTicketAllocationListBL(executionContext, reverseRedemptionTicketAllocationDTOs);
                    reversalredemptionTicketAllocationListBL.Save(sqlTrx);
                    LoadRedemptionTicketAllocationDTOList(sqlTrx);
                }
            }
            //if (redemptionDTO.RedemptionTicketAllocationListDTO != null && redemptionDTO.RedemptionTicketAllocationListDTO.Any())
            //{
            //    redemptionDTO.RedemptionTicketAllocationListDTO.Sort((x, y) => x.RedemptionGiftId.CompareTo(y.RedemptionGiftId));
            //}
            log.LogMethodExit();
        }

        internal void DeletTicketReceipt(List<TicketReceiptDTO> ticketReceiptDTOList, SqlTransaction sQLTrx)
        {
            log.LogMethodEntry();
            foreach (TicketReceiptDTO ticketReceiptDTO in ticketReceiptDTOList)
            {
                ticketReceiptDTO.RedemptionId = -1;
                TicketReceipt ticketReceipt = new TicketReceipt(executionContext, ticketReceiptDTO);
                ticketReceipt.Save(sQLTrx);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Consolidate ticket receipts
        /// </summary>
        /// <param name="managerApprovalReceived">bool</param>
        /// <param name="redemptionSource">Redemption source</param>
        /// <param name="sqlTrx">SqlTrx</param>
        /// <returns>TicketReceipt</returns>
        public TicketReceiptDTO ConsolidateTickets(RedemptionActivityDTO redemptionActivityDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(redemptionActivityDTO, sqlTransaction);

            bool managerApprovalReceived = ManagerApprovalReceived(redemptionActivityDTO.ManagerToken);
            //if (redemptionDTO != null && (redemptionDTO.TicketReceiptListDTO.Count < 2))
            //{
            //    log.Error(MessageContainerList.GetMessage(executionContext, 1623));
            //    throw new Exception(MessageContainerList.GetMessage(executionContext, 1623));
            //}
            if (redemptionDTO!=null && redemptionDTO.RedemptionGiftsListDTO!=null && redemptionDTO.RedemptionGiftsListDTO.Any())
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 1624));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1624));
            }
            //if (Redemption.RedemptionBL.IsCardRequiredForRedemption(executionContext) && !RedemptionHasCards())
            //{
            //    log.Error(MessageContainerList.GetMessage(executionContext, 1613));
            //    throw new Exception(MessageContainerList.GetMessage(executionContext, 1613));
            //}
            int totalTickets = 0;
            TicketReceipt newTicketReceipt = null;
            if (redemptionDTO.TicketReceiptListDTO != null && redemptionDTO.TicketReceiptListDTO.Count > 0)
            {
                TicketReceipt ticketReceipt;
                foreach (TicketReceiptDTO item in redemptionDTO.TicketReceiptListDTO)
                {
                    ticketReceipt = new TicketReceipt(executionContext, item);
                    if (ticketReceipt.IsUsedTicketReceipt(sqlTransaction))
                    {
                        log.Error(MessageContainerList.GetMessage(executionContext, 1625, item.ManualTicketReceiptNo));
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1625, item.ManualTicketReceiptNo));
                    }
                    totalTickets += item.Tickets;
                }
            }
            PerDayLimitCheckForManualTickets(GetManualTickets(),sqlTransaction);
            // Saves the newly created or updated cardsDTO after updating the currency details
            if (ApplyCurrencyRule(sqlTransaction))
            {
                SaveRedemptionOrder(sqlTransaction);
            }
            Build(sqlTransaction);
            totalTickets = totalTickets + GetManualTickets() + GetCurrencyTickets(-1,sqlTransaction);
            LoadTicketLimitCheck(managerApprovalReceived, totalTickets);
            if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0) && redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0).Count() > 0)
            {
                redemptionDTO.CardId = redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardId >= 0).CardId;
                redemptionDTO.PrimaryCardNumber = redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardId >= 0).CardNumber;
                foreach (RedemptionCardsDTO cards in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0))
                {
                    redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.RedemptionCardsId == cards.RedemptionCardsId).TicketCount = 0;
                }
                if (redemptionDTO.CustomerId == -1)
                {
                    AccountDTO primaryCard = GetRedemptionPrimaryCard(sqlTransaction);
                    redemptionDTO.CustomerId = primaryCard != null ? primaryCard.CustomerId : -1;
                }
            }
            redemptionDTO.ReceiptTickets = GetScannedTickets();
            redemptionDTO.ManualTickets = GetManualTickets();
            redemptionDTO.RedeemedDate = ServerDateTime.Now;
            redemptionDTO.OrderCompletedDate = ServerDateTime.Now;
            redemptionDTO.OrderDeliveredDate = ServerDateTime.Now;
            redemptionDTO.CurrencyTickets = GetCurrencyTickets(-1,sqlTransaction);
            redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString();
            redemptionDTO.Source = redemptionActivityDTO.Source;
            redemptionDTO.Remarks = redemptionActivityDTO.Remarks;
            redemptionDTO.POSMachineId = executionContext.GetMachineId();
            redemptionDTO.RedemptionOrderNo = Redemption.RedemptionBL.GetNextSeqNo(executionContext.GetMachineId(), sqlTransaction);
            redemptionDTO.Source = redemptionActivityDTO.Source;
            redemptionDTO.POSMachineId = executionContext.GetMachineId();
            redemptionDTO.ETickets = 0;

            RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
            redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());

            //Save(sqlTransaction);
            InsertManualTicketReceipts(redemptionDTO.RedemptionId, sqlTransaction);
            SetTicketAllocationDetails(sqlTransaction);
            newTicketReceipt = new TicketReceipt(executionContext);
            newTicketReceipt.CreateManualTicketReceipt(totalTickets, redemptionDTO.RedemptionId, sqlTransaction);
            RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO(-1, redemptionDTO.RedemptionId, -1, newTicketReceipt.TicketReceiptDTO.Id,
                executionContext.GetUserId(), ServerDateTime.Now, executionContext.GetMachineId(),
                RedemptionUserLogsDTO.RedemptionAction.CONSOLIDATE_TICKET.ToString(),
                redemptionActivityDTO.Source, approvalId.ToString(), ServerDateTime.Now);
            RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
            redemptionUserLogsBL.Save(sqlTransaction);
            log.LogMethodExit(newTicketReceipt);
            return newTicketReceipt.TicketReceiptDTO;
        }

        internal void RemoveManualTickets(RedemptionActivityDTO redemptionActivityDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(redemptionActivityDTO);
            bool managerApprovalReceived = ManagerApprovalReceived(redemptionActivityDTO.ManagerToken);
            redemptionDTO.ManualTickets = 0;
            RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
            redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            log.Info("Remove ManualTickets(" + redemptionActivityDTO.Tickets + ", Manual tickets removed ");
            log.LogMethodExit();
        }

        public RedemptionDTO UpdateRedemptionStatus(RedemptionActivityDTO redemptionActivityDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(redemptionActivityDTO, sqlTransaction);
            managerToken = redemptionActivityDTO.ManagerToken;
            PrintBalanceTicket = redemptionActivityDTO.PrintBalanceTicket;
            LoadBalanceTickettoCard = redemptionActivityDTO.LoadToCard;
            bool managerApprovalReceived = ManagerApprovalReceived(redemptionActivityDTO.ManagerToken);
            if (redemptionDTO.Remarks == "TURNINREDEMPTION") // Redemption is created for the turn in process
            {
                if (redemptionDTO.RedemptionStatus != redemptionActivityDTO.Status.ToString())
                {
                    redemptionDTO.POSMachineId = executionContext.GetMachineId();
                    redemptionDTO.Source = redemptionActivityDTO.Source;
                    redemptionDTO.OrderCompletedDate = ServerDateTime.Now;
                    redemptionDTO.OrderDeliveredDate = ServerDateTime.Now;
                    redemptionDTO.RedemptionStatus = redemptionActivityDTO.Status.ToString();
                    AddTurnIns(redemptionActivityDTO, sqlTransaction);
                    RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO(-1, redemptionDTO.RedemptionId, -1, -1,
                                           executionContext.GetUserId(), ServerDateTime.Now, executionContext.GetMachineId(),
                                           RedemptionUserLogsDTO.RedemptionAction.TURN_IN_REDEMPTION.ToString(),
                                           redemptionDTO.Source, approvalId.ToString(), ServerDateTime.Now);
                    RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
                    redemptionUserLogsBL.Save(sqlTransaction);
                }
            }
            else
            {
                redemptionDTO.Remarks = redemptionActivityDTO.Remarks;
                if ((redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.NEW.ToString() ||
                            redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString()) &&
               redemptionActivityDTO.Status != RedemptionActivityDTO.RedemptionActivityStatusEnum.ABANDONED &&
              redemptionActivityDTO.Status != RedemptionActivityDTO.RedemptionActivityStatusEnum.SUSPENDED
           )
                {
                    ValidateOrder(managerApprovalReceived, sqlTransaction);
                }
                if (redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString())
                {
                    log.Debug("The redemption is already in delivered state .");
                    return redemptionDTO;
                }
                if (redemptionActivityDTO.Status == RedemptionActivityDTO.RedemptionActivityStatusEnum.DELIVERED)
                {
                    redemptionDTO.POSMachineId = executionContext.GetMachineId();
                    redemptionDTO.Source = redemptionActivityDTO.Source;
                    string redemptionActivity = RedemptionUserLogsDTO.RedemptionAction.REDEMPTION.ToString();
                    // redemptionDTO.OrderCompletedDate = ServerDateTime.Now;
                    //redemptionDTO.OrderDeliveredDate = ServerDateTime.Now;
                    if (redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.NEW.ToString()
                        || redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString())
                    {
                        redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString();
                        Save(sqlTransaction);
                    }
                    if (redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.OPEN.ToString()
                        || redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.PREPARED.ToString())
                    {
                        redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString();
                        if (redemptionDTO.OrderDeliveredDate == null || redemptionDTO.OrderDeliveredDate == DateTime.MinValue)
                            redemptionDTO.OrderDeliveredDate = ServerDateTime.Now;
                        RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
                        redemptionDTO = redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        redemptionDTO.AcceptChanges();
                        redemptionActivity = RedemptionUserLogsDTO.RedemptionAction.REDEMPTION_DELIVERED.ToString();
                    }
                    RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO(-1, redemptionDTO.RedemptionId, -1, -1,
                                       executionContext.GetUserId(), ServerDateTime.Now, executionContext.GetMachineId(),
                                       redemptionActivity,
                                       redemptionDTO.Source, approvalId.ToString(), ServerDateTime.Now);
                    RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
                    redemptionUserLogsBL.Save(sqlTransaction);
                }
                if (redemptionActivityDTO.Status == RedemptionActivityDTO.RedemptionActivityStatusEnum.OPEN)
                {
                    redemptionDTO.POSMachineId = executionContext.GetMachineId();
                    redemptionDTO.Source = redemptionActivityDTO.Source;
                    redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.OPEN.ToString();
                    if (redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.NEW.ToString()
                        || redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString())
                    {
                        Save(sqlTransaction);
                    }
                    else
                    {
                        RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
                        redemptionDTO = redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        redemptionDTO.AcceptChanges();
                    }
                    RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO(-1, redemptionDTO.RedemptionId, -1, -1,
                                        executionContext.GetUserId(), ServerDateTime.Now, executionContext.GetMachineId(),
                                        RedemptionUserLogsDTO.RedemptionAction.REDEMPTION.ToString(),
                                        redemptionDTO.Source, approvalId.ToString(), ServerDateTime.Now);
                    RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
                    redemptionUserLogsBL.Save(sqlTransaction);
                }
                else if (redemptionActivityDTO.Status == RedemptionActivityDTO.RedemptionActivityStatusEnum.PREPARED)
                {
                    redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.PREPARED.ToString();
                    redemptionDTO.POSMachineId = executionContext.GetMachineId();
                    redemptionDTO.Source = redemptionActivityDTO.Source;
                    if (redemptionDTO.OrderCompletedDate == null || redemptionDTO.OrderCompletedDate == DateTime.MinValue)
                        redemptionDTO.OrderCompletedDate = ServerDateTime.Now;
                    //Save(sqlTransaction);
                    RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
                    redemptionDTO = redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    redemptionDTO.AcceptChanges();
                    RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO(-1, redemptionDTO.RedemptionId, -1, -1,
                                        executionContext.GetUserId(), ServerDateTime.Now, executionContext.GetMachineId(),
                                        RedemptionUserLogsDTO.RedemptionAction.REDEMPTION_PREPARED.ToString(),
                                        redemptionDTO.Source, approvalId.ToString(), ServerDateTime.Now);
                    RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
                    redemptionUserLogsBL.Save(sqlTransaction);
                }
                else if (redemptionActivityDTO.Status == RedemptionActivityDTO.RedemptionActivityStatusEnum.ABANDONED)
                {
                    redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.ABANDONED.ToString();
                    RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
                    redemptionDTO.Source = redemptionActivityDTO.Source;
                    redemptionDTO = redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    redemptionDTO.AcceptChanges();
                    RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO(-1, redemptionDTO.RedemptionId, -1, -1,
                                        executionContext.GetUserId(), ServerDateTime.Now, executionContext.GetMachineId(),
                                        RedemptionUserLogsDTO.RedemptionAction.REDEMPTION_ABANDONED.ToString(),
                                        redemptionDTO.Source, approvalId.ToString(), ServerDateTime.Now);
                    RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
                    redemptionUserLogsBL.Save(sqlTransaction);
                }
                else if (redemptionActivityDTO.Status == RedemptionActivityDTO.RedemptionActivityStatusEnum.SUSPENDED)
                {
                    redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString();
                    if (ApplyCurrencyRule(sqlTransaction))
                    {
                        SaveRedemptionOrder(sqlTransaction);
                    }
                    RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
                    redemptionDTO.Source = redemptionActivityDTO.Source;
                    redemptionDTO = redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    redemptionDTO.AcceptChanges();
                    RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO(-1, redemptionDTO.RedemptionId, -1, -1,
                                        executionContext.GetUserId(), ServerDateTime.Now, executionContext.GetMachineId(),
                                        RedemptionUserLogsDTO.RedemptionAction.SUSPEND_REDEMPTION.ToString(),
                                        redemptionDTO.Source, approvalId.ToString(), ServerDateTime.Now);
                    RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
                    redemptionUserLogsBL.Save(sqlTransaction);
                }
            }
            List<RedemptionDTO> redemptionDTOList = new List<RedemptionDTO>();
            redemptionDTOList.Add(redemptionDTO);
            //log.Error("update status deliver date before"+ redemptionDTO.OrderDeliveredDate);
            RedemptionUseCaseListBL redemptionUseCaseListBL = new RedemptionUseCaseListBL(executionContext, redemptionDTOList);
            redemptionUseCaseListBL.SetToSiteTimeOffset(redemptionDTOList);
            redemptionDTO = redemptionDTOList.FirstOrDefault();
            redemptionDTO.AcceptChanges();
            log.LogMethodExit(redemptionDTO);
            //log.Error("update status deliver date after" + redemptionDTO.OrderDeliveredDate);
            return redemptionDTO;
        }

        /// <summary>
        /// Checks whether card is present in the redemption
        /// </summary>
        /// <returns>bool</returns>
        public bool RedemptionHasCards()
        {
            log.LogMethodEntry();
            bool retValue = false;
            if (this.redemptionDTO != null && this.redemptionDTO.RedemptionCardsListDTO.Count > 0)
            {
                retValue = true;
            }
            log.LogMethodExit(retValue);
            return retValue;
        }

        /// <summary>
        /// Get first tapped card for the redemption
        /// </summary>
        /// <param name="sqlTrx">SqlTransaction</param>
        /// <returns>Card</returns>
        public AccountDTO GetRedemptionPrimaryCard(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            AccountDTO primaryCard = null;
            if (this.redemptionDTO != null && this.redemptionDTO.RedemptionCardsListDTO != null
               && this.redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0) && this.redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0).Count() > 0)
            {
                AccountBL accountBL = new AccountBL(executionContext, this.redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardId >= 0).CardId, true, true, sqlTrx);
                primaryCard = accountBL.AccountDTO;
            }
            log.LogMethodExit(primaryCard);
            return primaryCard;
        }

        public void ManualTicketLimitChecks(bool managerApprovalReceived, int manualTicketsToLoad,SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(managerApprovalReceived, manualTicketsToLoad, sqlTransaction);
            try
            {
                if (manualTicketsToLoad > 0)
                {
                    PerDayLimitCheckForManualTickets(manualTicketsToLoad, sqlTransaction);
                }
                else if(manualTicketsToLoad < 0)
                {
                    PerDayLimitCheckForReducingManualTickets(manualTicketsToLoad, sqlTransaction);
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            int managerApprovalLimit = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "ADD_TICKET_LIMIT_FOR_MANAGER_APPROVAL_REDEMPTION");
            if ((manualTicketsToLoad > 0 && manualTicketsToLoad > managerApprovalLimit && managerApprovalLimit != 0 && managerApprovalReceived == false))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
            }

            if (manualTicketsToLoad > 0 && manualTicketsToLoad > ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "MAX_MANUAL_TICKETS_PER_REDEMPTION"))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2495, ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "MAX_MANUAL_TICKETS_PER_REDEMPTION")));
            }
            log.LogMethodExit();
        }
        public void PerDayLimitCheckForManualTickets(int manualTickets,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(manualTickets);
            try
            {
                RedemptionTicketAllocationListBL redemptionTicketAllocationListBL = new RedemptionTicketAllocationListBL(executionContext);
                if (!redemptionTicketAllocationListBL.CanAddManualTicketForTheDay(executionContext.GetUserId(), manualTickets,sqlTransaction))
                {
                    int remainingManualTicketLimit = redemptionTicketAllocationListBL.GetRemainingAddManualTicketLimitForTheDay(executionContext.GetUserId(),sqlTransaction);
                    if (remainingManualTicketLimit > 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2488, remainingManualTicketLimit));/*"You can add only " + remainingManualTicketLimit + " more manual tickets";*/
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2489));
                    }
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        public void PerDayLimitCheckForReducingManualTickets(int manualTickets, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(manualTickets);
            try
            {
                RedemptionTicketAllocationListBL redemptionTicketAllocationListBL = new RedemptionTicketAllocationListBL(executionContext);
                if (!redemptionTicketAllocationListBL.CanReduceManualTicketForTheDay(executionContext.GetUserId(), manualTickets, sqlTransaction))
                {
                    int remainingManualTicketLimit = redemptionTicketAllocationListBL.GetRemainingReduceManualTicketLimitForTheDay(executionContext.GetUserId(), sqlTransaction);
                    if (remainingManualTicketLimit > 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 5093, remainingManualTicketLimit));/*"You can reduce only " + remainingManualTicketLimit + " more manual tickets";*/
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2489));
                    }
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        ///Checks the ticket while loading tickets to card
        /// </summary>
        /// <param name="managerApprovalReceived">bool</param>
        /// <param name="ticketsToLoad">TicketsToLoad</param>
        public void LoadTicketLimitCheck(bool managerApprovalReceived, int ticketsToLoad)
        {
            log.LogMethodEntry(managerApprovalReceived, ticketsToLoad);
            //DataAccessHandler dataAccessHandler = new DataAccessHandler();
            //List<SqlParameter> sqlParameter = new List<SqlParameter>();
            //sqlParameter.Add(new SqlParameter("@task_type", "LOADTICKETS"));
            //string mgrApprovalRequired = dataAccessHandler.executeScalar("select requires_manager_approval from task_type where task_type = @task_type", sqlParameter.ToArray(), null).ToString();
            if (ticketsToLoad == 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 5151));
            }
            bool managerApprovalNeeded = (ProductsContainerList.GetSystemProductContainerDTO(executionContext.GetSiteId(), "LOADTICKETS", "LOADTICKETS").ManagerApprovalRequired == "Y" || ProductsContainerList.GetSystemProductContainerDTO(executionContext.GetSiteId(), "LOADTICKETS", "LOADTICKETS_NOLOYALTY").ManagerApprovalRequired == "Y") ? true : false;

            int mgrApprovalLimit = 0;
            try
            {
                mgrApprovalLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL"));
            }
            catch (Exception ex) { log.Error(ex); }
            if ((ticketsToLoad > 0 && ticketsToLoad > mgrApprovalLimit && mgrApprovalLimit != 0 && managerApprovalReceived == false) || (ticketsToLoad > 0 && mgrApprovalLimit == 0 && (managerApprovalNeeded && managerApprovalReceived == false)))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
            }

            int mgrApprovalLoadTicketDeductionLimit = 0;
            try
            {
                mgrApprovalLoadTicketDeductionLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOAD_TICKET_DEDUCTION_LIMIT_FOR_MANAGER_APPROVAL"));
            }
            catch (Exception ex) { log.Error(ex); }
            if ((ticketsToLoad < 0 && (-1 * ticketsToLoad) > mgrApprovalLoadTicketDeductionLimit && mgrApprovalLoadTicketDeductionLimit != 0 && managerApprovalReceived == false) || (ticketsToLoad < 0 && mgrApprovalLoadTicketDeductionLimit == 0 && (managerApprovalNeeded && managerApprovalReceived == false)))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
            }

            int loadTicketLimit = 0;
            try
            {
                loadTicketLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOAD_TICKETS_LIMIT"));
            }
            catch (Exception ex) { log.Error(ex); }

            if (ticketsToLoad > 0 && ticketsToLoad > loadTicketLimit)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, MessageContainerList.GetMessage(executionContext, 35, loadTicketLimit.ToString(), ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEMPTION_TICKET_NAME_VARIANT"))));
            }
            int loadTicketDeductionLimit = 0;
            try
            {
                loadTicketDeductionLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOAD_TICKETS_DEDUCTION_LIMIT"));
            }
            catch (Exception ex) { log.Error(ex); }

            if (ticketsToLoad < 0 && (-1 * ticketsToLoad) > loadTicketDeductionLimit)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, MessageContainerList.GetMessage(executionContext, 5094, loadTicketDeductionLimit.ToString(), ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEMPTION_TICKET_NAME_VARIANT"))));
            }
            log.LogMethodExit();
        }
        internal bool ApplyCurrencyRule(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            bool isAnyCurrencyRuleAdded = false;
            RedemptionCurrencyRuleProvider RedemptionCurrencyRuleProvider = new RedemptionCurrencyRuleProvider(executionContext);
            RedemptionCurrencyRuleCalculator redemptionCurrencyRuleCalculator = new RedemptionCurrencyRuleCalculator(RedemptionCurrencyRuleProvider);
            List<RedemptionCardsDTO> currencyListforRule = new List<RedemptionCardsDTO>();
            if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyRuleId >= 0))
            {
                currencyListforRule = redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyRuleId >= 0).ToList();
                if (currencyListforRule != null && currencyListforRule.Count > 0)
                {
                    foreach (RedemptionCardsDTO currencyforRule in currencyListforRule)
                    {
                        RedemptionCardsDTO currencyDTOforRule = new RedemptionCardsDTO(currencyforRule);
                        redemptionDTO.RedemptionCardsListDTO.Remove(currencyforRule);
                        if (currencyDTOforRule.RedemptionCardsId > -1)
                        {
                            RedemptionCardsBL redemptionCardsBL = new RedemptionCardsBL(executionContext, currencyforRule.RedemptionCardsId, sqlTransaction);
                            redemptionCardsBL.Delete(sqlTransaction);
                        }
                    }
                    currencyListforRule.Clear();
                    isAnyCurrencyRuleAdded = true;
                }
            }
            if (redemptionCurrencyRuleCalculator != null)
            {
                currencyListforRule = new List<RedemptionCardsDTO>();
                foreach (RedemptionCardsDTO currencyforRule in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0))
                {
                    RedemptionCardsDTO currencyDTOforRule = new RedemptionCardsDTO(currencyforRule);
                    currencyDTOforRule.Guid = currencyforRule.Guid;
                    currencyListforRule.Add(currencyDTOforRule);
                }
                if (currencyListforRule != null && currencyListforRule.Any())
                {
                    currencyListforRule = redemptionCurrencyRuleCalculator.Calculate(currencyListforRule);
                    //List<RedemptionCardsDTO> filteredCurrencyList = currencyListforRule.Where(x => x.CurrencyRuleId >= 0).ToList();
                    if (currencyListforRule != null && currencyListforRule.Any(x => x.RedemptionCardsId < 0))
                    {
                        isAnyCurrencyRuleAdded = true;
                        List<RedemptionCardsDTO> backupCardsList = redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0).ToList();
                        if (backupCardsList != null && backupCardsList.Any())
                        {
                            foreach (RedemptionCardsDTO currencyforRule in backupCardsList)
                            {
                                redemptionDTO.RedemptionCardsListDTO.Remove(currencyforRule);
                            }
                        }
                        foreach (RedemptionCardsDTO currencyforRule in currencyListforRule)
                        {
                            redemptionDTO.RedemptionCardsListDTO.Add(currencyforRule);
                        }
                    }
                }
            }
            log.LogMethodExit();
            return isAnyCurrencyRuleAdded;
        }

        public void SetTicketAllocationDetails(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            List<RedemptionTicketAllocationDTO> setredemptionTicketAllocationDTOs = new List<RedemptionTicketAllocationDTO>();
            string message = "";
            if (GetManualTickets() != 0)
            {
                //load manual ticket info
                try
                {
                    setredemptionTicketAllocationDTOs.Add ( new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId,
                        -1, GetManualTickets(), null, -1, null, -1, null, null, null, null, null, -1, -1, -1, -1, null, -1));
                    
                }
                catch (Exception ex)
                {
                    message = MessageContainerList.GetMessage(executionContext, 1502);
                    throw new Exception(message);
                }
            }
            if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any())
            {
                List<RedemptionCardsDTO> filteredList = redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId > -1 || x.CurrencyRuleId > -1).ToList();
                if (filteredList != null && filteredList.Any())
                {
                    foreach (RedemptionCardsDTO redemptionCardsDTO in filteredList)
                    {
                        try
                        {
                            int currencyTickets = -1;
                            int redemptionCurrencyRuleId = -1;
                            if (redemptionCardsDTO.CurrencyRuleId > -1)
                            {
                                RedemptionCurrencyRuleBL redemptionCurrencyBL = new RedemptionCurrencyRuleBL(executionContext, Convert.ToInt32(redemptionCardsDTO.CurrencyRuleId), true, true,sqlTrx);
                                currencyTickets = redemptionCurrencyBL.GetRuleDeltaTicket();
                                //currencyTickets = Convert.ToInt32(redemptionCurrencyBL.GetRedemptionCurrencyRuleDTO.Cumulative) * Convert.ToInt32(redemptionCardsDTO.CurrencyQuantity);
                                redemptionCurrencyRuleId = redemptionCurrencyBL.GetRedemptionCurrencyRuleDTO.RedemptionCurrencyRuleId;
                                setredemptionTicketAllocationDTOs.Add( new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, -1, null, null, -1, null, -1, 1, null, null, null, null,
                                                                   executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(), -1, -1, -1, Convert.ToInt32(redemptionCardsDTO.CurrencyRuleId), currencyTickets, -1));
                            }
                            else
                            {
                                currencyTickets = Convert.ToInt32(redemptionCardsDTO.CurrencyValueInTickets * redemptionCardsDTO.CurrencyQuantity);
                                setredemptionTicketAllocationDTOs.Add( new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId,
                                                                                             -1, null, null, -1, null, Convert.ToInt32(redemptionCardsDTO.CurrencyId), redemptionCardsDTO.CurrencyQuantity, currencyTickets,
                                                                                               null, null, null, -1, -1, -1, redemptionCurrencyRuleId, 0, redemptionCardsDTO.SourceCurrencyRuleId == null ? -1 : Convert.ToInt32(redemptionCardsDTO.SourceCurrencyRuleId)));

                            }
                        }
                        catch (Exception ex)
                        {
                            message = MessageContainerList.GetMessage(executionContext, 1503);
                            throw new Exception(message);
                        }
                    }
                    if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyId >= 0))
                    {
                        foreach (int currency in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0).Select(y => y.CurrencyId).Distinct())
                        {
                            int currencyqty = Convert.ToInt32(redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId == currency).Sum(y => (y.CurrencyQuantity == null ? 0 : y.CurrencyQuantity)));
                            using (Utilities utilities = GetUtility())
                            {
                                UpdateRedemptionCurrencyInventory(currency, currencyqty, utilities, sqlTrx);
                            }
                        }
                    }
                }
            }
            if (redemptionDTO.TicketReceiptListDTO != null && redemptionDTO.TicketReceiptListDTO.Any())
            {
                foreach (TicketReceiptDTO ticketReceiptDTO in redemptionDTO.TicketReceiptListDTO)
                {
                    try
                    {
                        TicketReceipt ticketReceipt = new TicketReceipt(executionContext, ticketReceiptDTO);
                        setredemptionTicketAllocationDTOs.Add ( new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId,
                                 -1, null, null, -1, null, -1, null, null, null, ticketReceiptDTO.Tickets,
                                 null, ticketReceiptDTO.Id, -1, -1, -1, null, -1));                        
                    }
                    catch (Exception ex)
                    {
                        message = MessageContainerList.GetMessage(executionContext, 1504);
                        throw new Exception(message);
                    }
                }
            }
            if (setredemptionTicketAllocationDTOs != null && setredemptionTicketAllocationDTOs.Any())
            {
                RedemptionTicketAllocationListBL redemptionTicketAllocationListBL = new RedemptionTicketAllocationListBL(executionContext, setredemptionTicketAllocationDTOs);
                redemptionTicketAllocationListBL.Save(sqlTrx);
                LoadRedemptionTicketAllocationDTOList(sqlTrx);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the tickets to card
        /// </summary>
        /// <param name="utilities">Utilities</param>
        /// <param name="redemptionSource">Redemption Source</param>
        public void LoadTicketsToCard(RedemptionLoadToCardRequestDTO redemptionLoadToCardRequestDTO, Utilities utilities, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            bool managerApprovalReceived = ManagerApprovalReceived(redemptionLoadToCardRequestDTO.ManagerToken);
            string message = string.Empty;
            if (redemptionDTO != null)
            {

                if (redemptionDTO.RedemptionCardsListDTO==null || !redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 459));
                }
                if (redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0).Count() > 1)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1376));//message=Tickets can be loaded to the single card at a time
                }
                if (redemptionDTO!=null && redemptionDTO.RedemptionGiftsListDTO!=null && redemptionDTO.RedemptionGiftsListDTO.Any())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1377));
                }
                // Saves the newly created or updated cardsDTO after updating the currency details
                if (ApplyCurrencyRule(sqlTransaction))
                {
                    SaveRedemptionOrder(sqlTransaction);
                }
                Build(sqlTransaction);
                int ticketsToLoad = GetManualTickets() + GetCurrencyTickets(-1,sqlTransaction);
                foreach (TicketReceiptDTO ticketReceiptDTO in redemptionDTO.TicketReceiptListDTO)
                {
                    TicketReceipt ticketReceipt = new TicketReceipt(executionContext, ticketReceiptDTO);
                    if (ticketReceipt.IsUsedTicketReceipt(sqlTransaction))
                    {
                        log.Error(MessageContainerList.GetMessage(executionContext, 1625, ticketReceiptDTO.ManualTicketReceiptNo));
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1625, ticketReceiptDTO.ManualTicketReceiptNo));
                    }
                    else
                    {
                        ticketsToLoad += ticketReceiptDTO.Tickets;
                    }
                }
                int manualTickets = GetManualTickets();
                if (manualTickets > 0)
                {
                    PerDayLimitCheckForManualTickets(manualTickets, sqlTransaction);
                }
                else if(manualTickets < 0)
                {
                    PerDayLimitCheckForReducingManualTickets(manualTickets, sqlTransaction);
                }
                
                if (GetPhysicalTickets() + GetCurrencyTickets(-1,sqlTransaction) + GetManualTickets() != ticketsToLoad)
                {
                    log.Error("Ends-redeemGifts() - Tickets passed " + ticketsToLoad.ToString() + " is not mathcing with tickets available on order");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1390) + ": " + ticketsToLoad.ToString());
                }
                try
                {
                    LoadTicketLimitCheck(managerApprovalReceived, ticketsToLoad);
                }
                catch (ValidationException vex)
                {
                    log.Error(vex.Message);
                    throw;
                }
                Card primaryRedemptionCard = new Card((int)redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardId >= 0).CardId, "", utilities);
                try
                {
                    redemptionDTO.CardId = primaryRedemptionCard.card_id;
                    redemptionDTO.PrimaryCardNumber = primaryRedemptionCard.CardNumber;
                    redemptionDTO.CustomerId = primaryRedemptionCard.customer_id;
                    redemptionDTO.ReceiptTickets = GetScannedTickets();
                    redemptionDTO.ManualTickets = GetManualTickets();
                    redemptionDTO.RedeemedDate = ServerDateTime.Now;
                    redemptionDTO.OrderCompletedDate = ServerDateTime.Now;
                    redemptionDTO.OrderDeliveredDate = ServerDateTime.Now;
                    redemptionDTO.CurrencyTickets = GetCurrencyTickets(-1,sqlTransaction);
                    redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString();
                    redemptionDTO.Source = redemptionLoadToCardRequestDTO.Source;
                    redemptionDTO.Remarks = redemptionLoadToCardRequestDTO.Remarks;
                    redemptionDTO.POSMachineId = executionContext.GetMachineId();
                    redemptionDTO.ETickets = 0;

                    if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0) && redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0).Count() > 0)
                    {
                        foreach (RedemptionCardsDTO cards in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0))
                        {
                            redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.RedemptionCardsId == cards.RedemptionCardsId).TicketCount = 0;
                        }
                    }
                    RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
                    redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());

                    RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO(-1, redemptionDTO.RedemptionId, -1, -1,
                                    executionContext.GetUserId(), ServerDateTime.Now, executionContext.GetMachineId(),
                                    RedemptionUserLogsDTO.RedemptionAction.LOAD_TICKET.ToString(),
                                    redemptionLoadToCardRequestDTO.Source, approvalId.ToString(), ServerDateTime.Now);
                    RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
                    redemptionUserLogsBL.Save(sqlTransaction);
                    InsertManualTicketReceipts(redemptionDTO.RedemptionId, sqlTransaction);
                    SetTicketAllocationDetails(sqlTransaction);
                    int originalManagerId = -1;
                    if (!string.IsNullOrWhiteSpace(approvalId))
                    {
                        originalManagerId = utilities.ParafaitEnv.ManagerId;
                        utilities.ParafaitEnv.ManagerId = UserContainerList.GetUserContainerDTOOrDefault(approvalId, "", executionContext.GetSiteId()).UserId;
                    }
                    TaskProcs tp = new TaskProcs(utilities);
                    // if (!tp.loadTickets(primaryRedemptionCard, GetScannedTickets() + GetCurrencyTickets() + GetManualTickets(), "Redemption Load Tickets", redemptionDTO.RedemptionId, ref message, sqlTransaction))
                    if (!tp.loadTickets(primaryRedemptionCard, GetScannedTickets() + GetCurrencyTickets(-1,sqlTransaction) + GetManualTickets(), string.IsNullOrWhiteSpace(redemptionLoadToCardRequestDTO.Remarks) ? "Load Tickets under Redeem" : redemptionLoadToCardRequestDTO.Remarks, redemptionDTO.RedemptionId, ref message, sqlTransaction, redemptionLoadToCardRequestDTO.ConsiderForLoyalty))
                    {
                        if (!string.IsNullOrWhiteSpace(approvalId))
                        {
                            utilities.ParafaitEnv.ManagerId = originalManagerId; // Reset back the manager Id
                        }
                        throw new Exception(message);
                    }
                    if (!string.IsNullOrWhiteSpace(approvalId))
                    {
                        utilities.ParafaitEnv.ManagerId = originalManagerId; // Reset back the manager Id
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new Exception(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private bool ManagerApprovalReceived(string managerToken)
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                if (managerToken.Contains("Bearer"))
                {
                    managerToken = managerToken.Replace("Bearer ", "");
                }
                string jwtKey = Encryption.GetParafaitKeys("JWTKey");
                var now = DateTime.UtcNow;
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(jwtKey));
                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Encryption.GetParafaitKeys("JWTIssuer"),
                    ValidAudience = Encryption.GetParafaitKeys("JWTAudience"),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey,
                };
                System.Threading.Thread.CurrentPrincipal = handler.ValidateToken(managerToken, validationParameters, out securityToken);
                ClaimsPrincipal claims = handler.ValidateToken(managerToken, validationParameters, out securityToken);
                string roleId = claims.FindFirst(ClaimTypes.Role).Value;
                string siteId = claims.FindFirst(ClaimTypes.Sid).Value;
                string loginId = claims.FindFirst(ClaimTypes.Name).Value;
                UserContainerDTO redemptionuserContainerDTO = UserContainerList.GetUserContainerDTOOrDefault(executionContext.GetUserId(), "", Convert.ToInt32(siteId));
                UserContainerDTO manageruserContainerDTO = UserContainerList.GetUserContainerDTOOrDefault(loginId, "", Convert.ToInt32(siteId));
                if (redemptionuserContainerDTO != null)
                {
                    if (redemptionuserContainerDTO.SelfApprovalAllowed)
                    {
                        if (loginId == redemptionuserContainerDTO.LoginId)
                        {
                            result = true;
                            approvalId = redemptionuserContainerDTO.LoginId;
                            //utilities.ParafaitEnv.ManagerId = redemptionuserContainerDTO.UserId;
                            return result;
                        }
                    }
                    else
                    {
                        if (manageruserContainerDTO != null)
                        {
                            if (UserRoleContainerList.CanApproveFor(executionContext.SiteId, redemptionuserContainerDTO.RoleId, manageruserContainerDTO.RoleId))
                            {
                                result = true;
                                approvalId = manageruserContainerDTO.LoginId;
                                //utilities.ParafaitEnv.ManagerId = manageruserContainerDTO.UserId;
                                return result;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = false;
            }
            log.LogMethodExit(result);
            return result;
        }
        private bool LifetimeValidator(DateTime? notBefore, DateTime? expires, Microsoft.IdentityModel.Tokens.SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            log.LogMethodEntry();
            return true;
            //if (expires != null)
            //{
            //    if (DateTime.UtcNow < expires) return true;
            //}
            //return false;
        }

        private bool SetupThePrinting()
        {
            log.LogMethodEntry();
            PrintDialog myPrintDialog = new PrintDialog();
            myPrintDialog.AllowCurrentPage = false;
            myPrintDialog.AllowPrintToFile = false;
            myPrintDialog.AllowSelection = false;
            myPrintDialog.AllowSomePages = false;
            myPrintDialog.PrintToFile = false;
            myPrintDialog.ShowHelp = false;
            myPrintDialog.ShowNetwork = false;
            myPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;
            myPrintDialog.UseEXDialog = true;
            PrintDocument myPrintDocument = new PrintDocument
            {
                DocumentName = MessageContainerList.GetMessage(executionContext, "Redemption Receipt"),
                PrinterSettings = myPrintDialog.PrinterSettings,
                DefaultPageSettings = myPrintDialog.PrinterSettings.DefaultPageSettings
            };
            myPrintDocument.DefaultPageSettings.Margins = new Margins(10, 10, 20, 20);

            log.LogMethodExit(true);
            return true;
        }
        /// <summary>
        /// SetupThePrinting
        /// </summary>
        public bool SetupThePrinting(PrinterDTO Printer)
        {
            log.LogMethodEntry(Printer);
            PrintDialog myPrintDialog = new PrintDialog();
            if (Printer.PrinterName != "Default")
            {
                myPrintDialog.PrinterSettings.PrinterName = String.IsNullOrEmpty(Printer.PrinterLocation) ? Printer.PrinterName : Printer.PrinterLocation;
            }

            myPrintDialog.AllowCurrentPage = false;
            myPrintDialog.AllowPrintToFile = false;
            myPrintDialog.AllowSelection = false;
            myPrintDialog.AllowSomePages = false;
            myPrintDialog.PrintToFile = false;
            myPrintDialog.ShowHelp = false;
            myPrintDialog.ShowNetwork = true;
            myPrintDialog.UseEXDialog = false;

            PrintDocument myPrintDocument = new PrintDocument();
            myPrintDocument.DocumentName = MessageContainerList.GetMessage(executionContext, "Redemption Receipt");
            myPrintDocument.PrinterSettings = myPrintDialog.PrinterSettings;
            myPrintDocument.DefaultPageSettings = myPrintDialog.PrinterSettings.DefaultPageSettings;
            DateTime dateNow = ServerDateTime.Now;

            myPrintDocument.OriginAtMargins = true;
            // myPrintDocument.DefaultPageSettings.Margins = new Margins(utilities.ParafaitEnv.PRINTER_PAGE_LEFT_MARGIN, utilities.ParafaitEnv.PRINTER_PAGE_RIGHT_MARGIN, 10, 20);

            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Prinng redemption
        /// </summary>
        public ReceiptClass PrintRedemption(Utilities utilities, int templateId = -1, PrinterDTO printer = null, string ScreenNumber = null, bool isTurnIn = false, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(templateId, printer, ScreenNumber, isTurnIn, sqlTrx);

            bool isTurnin = isTurnIn;
            string screenNumber = ScreenNumber;
            int receiptTemplateId = -1;
            Printer.ReceiptClass receipt = null;
            PrintDocument myPrintDocument = new PrintDocument();
            if (printer == null ? SetupThePrinting() : SetupThePrinting(printer))
            {
                try
                {
                    if (templateId == -1)
                    {
                        try
                        {
                            receiptTemplateId = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEMPTION_RECEIPT_TEMPLATE"));
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            receiptTemplateId = -1;
                        }
                    }
                    else
                    {
                        receiptTemplateId = templateId;
                    }

                    if (receiptTemplateId == -1)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Redemption receipt template is not set up."));
                    }
                    else
                    {
                        receipt = GenerateRedemptionReceipt(utilities, receiptTemplateId, null, sqlTrx);
                    }
                    log.LogMethodExit(receipt);
                    return receipt;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new Exception(ex.Message + MessageContainerList.GetMessage(executionContext, "Print Error"));
                }
            }
            else
            {
                log.Error("Printer setup error");
                log.LogMethodExit(false);
                return null;
            }
        }
        private bool IsThisTurnInRedemption()
        {
            log.LogMethodEntry();
            bool turnInRedemption = false;
            if (this.redemptionDTO != null && string.IsNullOrEmpty(this.redemptionDTO.Remarks) == false && this.redemptionDTO.Remarks.Contains("TURNINREDEMPTION"))
            {
                turnInRedemption = true;
            }
            log.LogMethodExit(turnInRedemption);
            return turnInRedemption;
        }
        public int GetTotalRedemptionAllocationTickets(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            bool thisIsReversedRedemption = IsThisReversedRedemption();

            bool thisIsTurnInRedemption = IsThisTurnInRedemption();
            LoadRedemptionTicketAllocationDTOList(sqlTransaction);
            int totalTickets = 0;
            totalTickets = Convert.ToInt32(redemptionDTO.RedemptionTicketAllocationListDTO.Where(tline => ((thisIsReversedRedemption == true && tline.RedemptionGiftId != -1)
                                                                                                                                 || (thisIsReversedRedemption == false && thisIsTurnInRedemption == false)
                                                                                                                                 || (thisIsTurnInRedemption == true && tline.RedemptionGiftId != -1))
                                                                                                                                 ).Sum(line => ((line.CurrencyTickets == null ? 0 : line.CurrencyTickets)
                                                                                                                                                + (line.ETickets == null ? 0 : line.ETickets)
                                                                                                                                                + (line.GraceTickets == null ? 0 : line.GraceTickets)
                                                                                                                                                + (line.ManualTickets == null ? 0 : line.ManualTickets)
                                                                                                                                                + (line.ReceiptTickets == null ? 0 : line.ReceiptTickets)
                                                                                                                                                + (line.TurnInTickets == null ? 0 : line.TurnInTickets)
                                                                                                                                                + (line.RedemptionCurrencyRuleTicket == null ? 0 : line.RedemptionCurrencyRuleTicket))));
            if (thisIsReversedRedemption || thisIsTurnInRedemption)
            {
                totalTickets = totalTickets * -1;
            }
            log.LogMethodExit(totalTickets);
            return totalTickets;
        }
        /// <summary>
        /// Method to generate receipt class object for redemption
        /// </summary>
        /// <param name="redemptionOrder">RedemptionBL</param>
        /// <param name="receiptTemplate">int</param>
        /// <param name="inPrinter">POS.POSPrinterDTO</param>
        /// <param name="sqlTrx">SqlTransaction</param>
        /// <param name="ignoreReversedLines">bool</param>
        /// <returns></returns>
        public Printer.ReceiptClass GenerateRedemptionReceipt(Utilities utilities, int receiptTemplate = -1, POS.POSPrinterDTO inPrinter = null, SqlTransaction sqlTrx = null, bool ignoreReversedLines = false)
        {
            log.LogMethodEntry(receiptTemplate, inPrinter, sqlTrx, ignoreReversedLines);
            POS.POSPrinterDTO printer;
            PrinterBL printerBL = new PrinterBL(executionContext);
            if (inPrinter == null)
            {
                printer = new POS.POSPrinterDTO(-1, -1, -1, -1, -1, -1, receiptTemplate, null, null, null, true, ServerDateTime.Now, "", ServerDateTime.Now, "", -1, "", false, -1, -1);
                printer.ReceiptPrintTemplateHeaderDTO = (new ReceiptPrintTemplateHeaderBL(executionContext, receiptTemplate, true)).ReceiptPrintTemplateHeaderDTO;
                printer.PrinterDTO = new PrinterDTO();
                printer.PrinterDTO.PrinterName = "";

            }
            else
            {
                printer = inPrinter;
            }
            //DataTable dtReceiptTemplate = printer.ReceiptTemplate;

            int ticketsUsed = 0;

            if (redemptionDTO != null && redemptionDTO.RedemptionGiftsListDTO!=null && redemptionDTO.RedemptionGiftsListDTO.Any()) //(dtGifts.Rows.Count > 0)
            {
                ticketsUsed = (int)redemptionDTO.RedemptionGiftsListDTO.Sum(item => item.Tickets);
            }

            int maxLines = (redemptionDTO.RedemptionGiftsListDTO!=null ? redemptionDTO.RedemptionGiftsListDTO.Count : 0)  +
                           (redemptionDTO.RedemptionCardsListDTO != null ? redemptionDTO.RedemptionCardsListDTO.Count : 0) +
                           (redemptionDTO.RedemptionTicketAllocationListDTO != null ? redemptionDTO.RedemptionTicketAllocationListDTO.Count : 0) +
                           printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Count + 10;
            log.LogVariableState("maxLines", maxLines);
            Printer.ReceiptClass receipt = new Printer.ReceiptClass(maxLines);
            int rLines = 0;
            string dateFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            string dateTimeFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT");
            string numberFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            int colLength = 5;

            if (redemptionDTO != null)
            {
                List<ReceiptPrintTemplateDTO> receiptItemListDTOList = printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Where(iSlip => iSlip.Section == "ITEMSLIP").OrderBy(seq => seq.Sequence).ToList();

                if (printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList != null)
                {
                    foreach (ReceiptPrintTemplateDTO receiptTemplateDTO in printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Take(printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Count - (receiptItemListDTOList.Count - 1)))
                    {
                        log.LogVariableState("rLines", rLines);
                        string line = "";
                        int pos;
                        //get Col data and Col alignment into list
                        List<ReceiptColumnData> receiptTemplateColList = new List<ReceiptColumnData>();
                        ReceiptPrintTemplateBL receiptTemplateBL = new ReceiptPrintTemplateBL(executionContext, receiptTemplateDTO);
                        receiptTemplateColList = receiptTemplateBL.GetReceiptDTOColumnData();

                        receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                        receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                        int lineBarCodeHeight = 24;
                        if (receiptTemplateDTO.MetaData != null && (receiptTemplateDTO.MetaData.Contains("@lineHeight") || receiptTemplateDTO.MetaData.Contains("@lineBarCodeHeight")))
                        {
                            try
                            {
                                string[] metadata;
                                if (receiptTemplateDTO.MetaData.Contains("|"))
                                    metadata = receiptTemplateDTO.MetaData.Split('|');
                                else
                                {
                                    metadata = new string[] { receiptTemplateDTO.MetaData };
                                }
                                foreach (string s in metadata)
                                {
                                    if (s.Contains("@lineHeight="))
                                    {
                                        int iLineHeight = s.IndexOf("=") + 1;
                                        if (iLineHeight != -1)
                                            receipt.ReceiptLines[rLines].LineHeight = Convert.ToInt32(s.Substring(iLineHeight, s.Length - iLineHeight));
                                        else
                                            receipt.ReceiptLines[rLines].LineHeight = 0;
                                    }

                                    if (s.Contains("@lineBarCodeHeight="))
                                    {
                                        int iLineBarCodeHeight = s.IndexOf("=") + 1;
                                        if (iLineBarCodeHeight != -1)
                                            lineBarCodeHeight = Convert.ToInt32(s.Substring(iLineBarCodeHeight, s.Length - iLineBarCodeHeight));
                                        else
                                            lineBarCodeHeight = 24;
                                    }
                                }
                            }
                            catch
                            {
                                receipt.ReceiptLines[rLines].LineHeight = 0;
                                lineBarCodeHeight = 24;
                            }
                        }

                        switch (receiptTemplateDTO.Section)
                        {
                            case "FOOTER":
                            case "HEADER":
                                {
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        receipt.ReceiptLines[rLines].colCount = 1;
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;
                                            line.Replace("@ApprovedBy", GetApproverId(sqlTrx));
                                            line = line.Replace("@SiteName", utilities.ParafaitEnv.SiteName);
                                            if (utilities.ParafaitEnv.CompanyLogo != null && line.Contains("@SiteLogo"))
                                            {
                                                line = line.Replace("@SiteLogo", "");
                                                receipt.ReceiptLines[rLines].BarCode = utilities.ParafaitEnv.CompanyLogo;
                                            }
                                            else
                                                line = line.Replace("@SiteLogo", "");
                                            line = line.Replace("@SiteAddress", utilities.ParafaitEnv.SiteAddress);
                                            line = line.Replace("@CreditNote", "");
                                            try
                                            {
                                                line = line.Replace("@Date", Convert.ToDateTime(redemptionDTO.RedeemedDate).ToString(dateFormat));
                                            }
                                            catch
                                            {
                                                line = line.Replace("@Date", "");
                                            }

                                            line = line.Replace("@SystemDate", ServerDateTime.Now.ToString(dateTimeFormat));
                                            line = line.Replace("@TrxId", redemptionDTO.RedemptionId.ToString());
                                            line = line.Replace("@TrxNo", redemptionDTO.RedemptionOrderNo);
                                            line = line.Replace("@TrxOTP", "");
                                            line = line.Replace("@Cashier", executionContext.GetUserId());
                                            line = line.Replace("@Token", "");

                                            line = line.Replace("@POS", executionContext.POSMachineName);
                                            line = line.Replace("@Printer", printer.PrinterName);
                                            line = line.Replace("@TaxNo", "");
                                            line = line.Replace("@CustomerName", redemptionDTO.CustomerName);
                                            line = line.Replace("@Address", "");
                                            line = line.Replace("@City", "");
                                            line = line.Replace("@State", "");
                                            line = line.Replace("@Pin", "");
                                            line = line.Replace("@Phone", "");
                                            line = line.Replace("@CardBalance", "");
                                            line = line.Replace("@CreditBalance", "");
                                            line = line.Replace("@BonusBalance", "");
                                            line = line.Replace("@BarCodeTrxId", "");
                                            line = line.Replace("@BarCodeTrxOTP", "");
                                            line = line.Replace("@CardNumber", redemptionDTO.PrimaryCardNumber);
                                            line = line.Replace("@TableNumber", "");
                                            line = line.Replace("@Waiter", "");
                                            line = line.Replace("@CashAmount", "");
                                            line = line.Replace("@GameCardAmount", "");
                                            line = line.Replace("@CreditCardAmount", "");
                                            line = line.Replace("@NameOnCreditCard", redemptionDTO.CustomerName);
                                            line = line.Replace("@CreditCardName", "");
                                            line = line.Replace("@CreditCardReceipt", "");
                                            line = line.Replace("@OriginalTrxNo", redemptionDTO.OriginalRedemptionOrderNo);
                                            line = line.Replace("@InvoicePrefix", "");
                                            line = line.Replace("@OtherPaymentMode", "");
                                            line = line.Replace("@OtherModeAmount", "");
                                            line = line.Replace("@OtherCurrencyCode", "");
                                            line = line.Replace("@OtherCurrencyRate", "");
                                            line = line.Replace("@AmountInOtherCurrency", "");
                                            line = line.Replace("@RoundOffAmount", "");
                                            line = line.Replace("@CreditCardNumber", "");
                                            line = line.Replace("@TenderedAmount", "");
                                            line = line.Replace("@ChangeAmount", "");
                                            if (ticketsUsed != 0)
                                            {
                                                line = line.Replace("@Tickets", TicketValueInStringFormat(ticketsUsed));
                                            }
                                            else
                                            {
                                                line = line.Replace("@Tickets", "");
                                            }

                                            line = line.Replace("@LoyaltyPoints", "");
                                            line = line.Replace("@ExpiringCPCredits", "");
                                            line = line.Replace("@ExpiringCPBonus", "");
                                            line = line.Replace("@ExpiringCPLoyalty", "");
                                            line = line.Replace("@ExpiringCPTickets", "");
                                            line = line.Replace("@CPCreditsExpiryDate", "");
                                            line = line.Replace("@CPBonusExpiryDate", "");
                                            line = line.Replace("@CPLoyaltyExpiryDate", "");
                                            line = line.Replace("@CPTicketsExpiryDate", "");
                                            line = line.Replace("@TrxProfile", "");
                                            line = line.Replace("@Remarks", redemptionDTO.Remarks);
                                            line = line.Replace("@ResolutionNumber", "");
                                            line = line.Replace("@ResolutionDate", "");
                                            line = line.Replace("@ResolutionInitialRange", "");
                                            line = line.Replace("@ResolutionFinalRange", "");
                                            line = line.Replace("@Prefix", "");
                                            line = line.Replace("@SystemResolutionAuthorization", "");
                                            line = line.Replace("@InvoiceNumber", "");
                                            line = line.Replace("@OriginalTrxNetAmount", "");
                                            line = line.Replace("@Note", "");
                                            //line = line.Replace("@ScreenNumber", "");
                                            line = line.Replace("@CreditPlusCredits", "");
                                            line = line.Replace("@CreditPlusBonus", "");
                                            line = line.Replace("@TotalCreditPlusLoyaltyPoints", "");
                                            line = line.Replace("@CreditPlusTime", "");
                                            line = line.Replace("@CreditPlusTickets", "");
                                            line = line.Replace("@CreditPlusCardBalance", "");
                                            line = line.Replace("@TimeBalance", "");
                                            line = line.Replace("@RedeemableCreditPlusLoyaltyPoints ", "");

                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    break;
                                }
                            case "REDEEMED_GIFTS":
                            case "PRODUCT":
                                {
                                    if (redemptionDTO != null && redemptionDTO.RedemptionGiftsListDTO!=null&& redemptionDTO.RedemptionGiftsListDTO.Any())
                                    {
                                        string heading = "";
                                        foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                        {
                                            if (receiptColumnData.Alignment != "H")
                                            {
                                                receipt.ReceiptLines[rLines].colCount++;
                                                receipt.ReceiptLines[rLines + 1].colCount++;
                                            }

                                            line = receiptColumnData.Data;
                                            int temp = line.IndexOf(":");
                                            if (temp != -1)
                                                heading = line.Substring(0, temp);
                                            else
                                                continue;

                                            receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = heading;
                                            receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;

                                            receipt.ReceiptLines[rLines + 1].Data[receiptColumnData.Sequence - 1] = ("-").PadRight(heading.Length, '-');
                                            receipt.ReceiptLines[rLines + 1].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;

                                        }
                                        receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                                        receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                        receipt.ReceiptLines[rLines + 1].TemplateSection = receiptTemplateDTO.Section;
                                        receipt.ReceiptLines[rLines + 1].LineFont = receiptTemplateDTO.ReceiptFont;

                                        int savColCount = receipt.ReceiptLines[rLines].colCount;
                                        if (heading != "")
                                        {
                                            rLines += 2;
                                        }
                                        else
                                            receipt.ReceiptLines[rLines + 1].colCount = 0;

                                        for (int x = 0; x < redemptionDTO.RedemptionGiftsListDTO.Count; x++)
                                        {
                                            if (ignoreReversedLines && redemptionDTO.RedemptionGiftsListDTO[x].GiftLineIsReversed)
                                            {
                                                continue;
                                            }
                                            receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                                            receipt.ReceiptLines[rLines].colCount = savColCount;
                                            receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                            foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                            {
                                                line = "";
                                                if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                                {
                                                    line = receiptColumnData.Data;
                                                    pos = line.IndexOf(":");
                                                    int pos2 = line.IndexOf("::");
                                                    if (pos >= 0)
                                                    {
                                                        if (pos2 >= 0)
                                                            line = line.Substring(pos + 1, pos2 + 1);
                                                        else
                                                            line = line.Substring(pos + 1);
                                                    }

                                                    line = line.Replace("@Product", redemptionDTO.RedemptionGiftsListDTO[x].ProductDescription);
                                                    line = line.Replace("@Price", "");
                                                    line = line.Replace("@Quantity", "");
                                                    line = line.Replace("@PreTaxAmount", "");
                                                    line = line.Replace("@TaxName", "");
                                                    line = line.Replace("@Tax", "");
                                                    line = line.Replace("@Amount", "");
                                                    line = line.Replace("@LineRemarks", "");
                                                    line = line.Replace("@Tickets", TicketValueInStringFormat(redemptionDTO.RedemptionGiftsListDTO[x].Tickets));
                                                    line = line.Replace("@GraceTickets", TicketValueInStringFormat(redemptionDTO.RedemptionGiftsListDTO[x].GraceTickets));

                                                }
                                                receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                                receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;

                                            }
                                            rLines++;
                                        }
                                    }
                                    rLines = rLines - 1;
                                    break;
                                }
                            case "TAXLINE":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@TaxName", "");
                                            line = line.Replace("@TaxPercentage", "");
                                            line = line.Replace("@TaxAmount", "");
                                            line = line.Replace("@TaxableLineAmount", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "TAXABLECHARGES":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@ChargeName", "");
                                            line = line.Replace("@ChargeAmount", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "TAXTOTAL":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;
                                            line = line.Replace("@TaxableTotal", "");
                                            line = line.Replace("@NonTaxableTotal", "");
                                            line = line.Replace("@TaxExempt", "");
                                            line = line.Replace("@Tax", "");
                                            line = line.Replace("@ZeroRatedTaxable", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "NONTAXABLECHARGES":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@ChargeName", "");
                                            line = line.Replace("@ChargeAmount", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "TRANSACTIONTOTAL":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@RentalAmount", "");
                                            line = line.Replace("@RentalDeposit", "");
                                            line = line.Replace("@Total", "");
                                            line = line.Replace("@PreTaxTotal", "");
                                            if (redemptionDTO!=null && redemptionDTO.RedemptionGiftsListDTO!=null && redemptionDTO.RedemptionGiftsListDTO.Any())
                                            {
                                                if (ignoreReversedLines)
                                                {
                                                    line = line.Replace("@GiftTotal", redemptionDTO.RedemptionGiftsListDTO.Where(rgDTO => rgDTO.GiftLineIsReversed == false).ToList().Count().ToString());
                                                }
                                                else
                                                {
                                                    line = line.Replace("@GiftTotal", redemptionDTO.RedemptionGiftsListDTO.Count.ToString());
                                                }
                                            }
                                            else
                                            {
                                                line = line.Replace("@GiftTotal", "");
                                            }

                                            if (ticketsUsed != 0)
                                            {
                                                line = line.Replace("@TicketsTotal", TicketValueInStringFormat(ticketsUsed));
                                            }
                                            else
                                            {
                                                line = line.Replace("@TicketsTotal", "");
                                            }
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "DISCOUNTS":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@DiscountName", "");
                                            line = line.Replace("@DiscountPercentage", "");
                                            line = line.Replace("@DiscountAmount", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "DISCOUNTTOTAL":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@DiscountTotal", "");
                                            line = line.Replace("@DiscountAmountExclTax", "");
                                            line = line.Replace("@DiscountedTotal", "");
                                            line = line.Replace("@DiscountRemarks", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "GRANDTOTAL":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@GrandTotal", "");
                                            line = line.Replace("@RoundedOffGrandTotal", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "ITEMSLIP":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@TrxId", "");
                                            line = line.Replace("@TrxNo", "");
                                            line = line.Replace("@TrxOTP", "");
                                            line = line.Replace("@Token", "");
                                            line = line.Replace("@Product", "");
                                            line = line.Replace("@Quantity", "");
                                            line = line.Replace("@Price", "");
                                            line = line.Replace("@Tax", "");
                                            line = line.Replace("@Amount", "");
                                            line = line.Replace("@LineRemarks", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "CARDINFO":
                                {
                                    if (redemptionDTO != null && redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Count > 0)
                                    {
                                        string heading = "";
                                        foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                        {
                                            if (receiptColumnData.Alignment != "H")
                                            {
                                                receipt.ReceiptLines[rLines].colCount++;
                                                receipt.ReceiptLines[rLines + 1].colCount++;
                                            }

                                            line = receiptColumnData.Data;
                                            int temp = line.IndexOf(":");
                                            if (temp != -1)
                                            {
                                                heading = line.Substring(0, temp);
                                            }
                                            else //skip
                                            {
                                                continue;
                                            }

                                            receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = heading;
                                            receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;

                                            receipt.ReceiptLines[rLines + 1].Data[receiptColumnData.Sequence - 1] = ("-").PadRight(heading.Length, '-');
                                            receipt.ReceiptLines[rLines + 1].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                        }
                                        receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                                        receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                        receipt.ReceiptLines[rLines + 1].TemplateSection = receiptTemplateDTO.Section;
                                        receipt.ReceiptLines[rLines + 1].LineFont = receiptTemplateDTO.ReceiptFont;

                                        int savColCount = receipt.ReceiptLines[rLines].colCount;

                                        if (heading != "")
                                        {
                                            rLines += 2;
                                        }
                                        else
                                        {
                                            receipt.ReceiptLines[rLines + 1].colCount = 0;
                                        }

                                        for (int x = 0; x < redemptionDTO.RedemptionCardsListDTO.Count; x++)
                                        {
                                            receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                                            receipt.ReceiptLines[rLines].colCount = savColCount;
                                            receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                            foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                            {
                                                line = "";
                                                if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                                {
                                                    line = receiptColumnData.Data;
                                                    pos = line.IndexOf(":");
                                                    if (pos >= 0)
                                                        line = line.Substring(pos + 1);

                                                    line = line.Replace("@CardNumber", redemptionDTO.RedemptionCardsListDTO[x].CardNumber);
                                                    line = line.Replace("@CustomerName", "");
                                                    line = line.Replace("@FaceValue", "");
                                                    line = line.Replace("@Credits", "");
                                                    line = line.Replace("@Bonus", "");
                                                    line = line.Replace("@Time", "");
                                                    line = line.Replace("@TotalCardValue", "");
                                                    line = line.Replace("@Tax", "");
                                                    line = line.Replace("@Amount", "");

                                                    if (line.Contains("@BarCodeCardNumber"))
                                                    {
                                                        // replaceValue = cardRow.CardNumber;
                                                        // line = line.Replace("@BarCodeCardNumber", replaceValue);
                                                        line = line.Replace("@BarCodeCardNumber", redemptionDTO.RedemptionCardsListDTO[x].CardNumber);
                                                        if (string.IsNullOrEmpty(redemptionDTO.RedemptionCardsListDTO[x].CardNumber) == false)
                                                        {
                                                            if (receipt.ReceiptLines[rLines].LineFont.Size > 12)
                                                            {
                                                                //receipt.ReceiptLines[rLines].BarCode = GenCode128.Code128Rendering.MakeBarcodeImage(replaceValue, 2, true); 
                                                                receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(2, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), redemptionDTO.RedemptionCardsListDTO[x].CardNumber);
                                                            }
                                                            else
                                                            {
                                                                // receipt.ReceiptLines[rLines].BarCode = GenCode128.Code128Rendering.MakeBarcodeImage(replaceValue, 1, true);
                                                                receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(1, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), redemptionDTO.RedemptionCardsListDTO[x].CardNumber);
                                                            }
                                                        }
                                                    }


                                                    line = line.Replace("@LineRemarks", "");
                                                    if (line.Contains("@QRCodeCardNumber"))
                                                    {
                                                        // replaceValue = cardRow.CardNumber;
                                                        // line = line.Replace("@QRCodeCardNumber", replaceValue);
                                                        line = line.Replace("@QRCodeCardNumber", redemptionDTO.RedemptionCardsListDTO[x].CardNumber);
                                                        if (string.IsNullOrEmpty(redemptionDTO.RedemptionCardsListDTO[x].CardNumber) == false)
                                                        {
                                                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                                                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(redemptionDTO.RedemptionCardsListDTO[x].CardNumber, QRCodeGenerator.ECCLevel.Q);
                                                            QRCode qrCode = new QRCode(qrCodeData);
                                                            if (qrCode != null)
                                                            {
                                                                int pixelPerModule = 1;
                                                                if (receipt.ReceiptLines[rLines].LineFont.Size > 10)
                                                                {
                                                                    pixelPerModule = Convert.ToInt32(receipt.ReceiptLines[rLines].LineFont.Size / 10);
                                                                }
                                                                receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                                            }
                                                        }
                                                    }

                                                    //if (redemptionDTO.RedemptionCardsListDTO[x].CardId != -1)
                                                    //{
                                                    line = line.Replace("@CardBalanceTickets", TicketValueInStringFormat(redemptionDTO.RedemptionCardsListDTO[x].TotalCardTickets));
                                                    //}
                                                    line = line.Replace("@RedeemedTickets", TicketValueInStringFormat(redemptionDTO.RedemptionCardsListDTO[x].TicketCount));
                                                    line = line.Replace("@RedemptionCurrencyName", redemptionDTO.RedemptionCardsListDTO[x].CurrencyName);
                                                    line = line.Replace("@RedemptionCurrencyValue", TicketValueInStringFormat(redemptionDTO.RedemptionCardsListDTO[x].CurrencyValueInTickets));
                                                    line = line.Replace("@RedemptionCurrencyQuantity", redemptionDTO.RedemptionCardsListDTO[x].CurrencyQuantity.ToString());

                                                }
                                                receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                                receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                            }
                                            rLines++;
                                        }
                                    }
                                    rLines = rLines - 1;
                                    break;
                                }
                            case "REDEMPTION_SOURCE_HEADER":
                                {
                                    int savColCount = 0;

                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "REDEMPTION_SOURCE":
                                {

                                    List<Tuple<string, decimal, int, int>> redemptionSourceTupleList = null;
                                    redemptionSourceTupleList = GetRedemptionSourceTicketList(receiptTemplateColList, sqlTrx);
                                    if (redemptionSourceTupleList != null && redemptionSourceTupleList.Count > 0)
                                    {
                                        for (int x = 0; x < redemptionSourceTupleList.Count; x++)
                                        {
                                            int savColCount = 0;
                                            receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                                            receipt.ReceiptLines[rLines].colCount = savColCount;
                                            receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                            foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                            {
                                                line = "";
                                                if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                                {
                                                    line = receiptColumnData.Data;

                                                    line = line.Replace("@RedemptionReceiptNo", redemptionSourceTupleList[x].Item1.ToString());
                                                    line = line.Replace("@RedemptionCardNo", redemptionSourceTupleList[x].Item1.ToString());
                                                    line = line.Replace("@RedemptionCurrencyNo", redemptionSourceTupleList[x].Item1.ToString());
                                                    line = line.Replace("@RedemptionCurrencyRule", redemptionSourceTupleList[x].Item1.ToString());
                                                    line = line.Replace("@RedemptionManualTickets", TicketValueInStringFormat(redemptionSourceTupleList[x].Item1));
                                                    line = line.Replace("@RedemptionGraceTickets", TicketValueInStringFormat(redemptionSourceTupleList[x].Item1));
                                                    line = line.Replace("@TurnInTickets", TicketValueInStringFormat(redemptionSourceTupleList[x].Item1));
                                                    line = line.Replace("@TicketQuantity", redemptionSourceTupleList[x].Item2.ToString());
                                                    line = line.Replace("@TicketValue", TicketValueInStringFormat(redemptionSourceTupleList[x].Item3));
                                                    line = line.Replace("@TotalTickets", TicketValueInStringFormat(redemptionSourceTupleList[x].Item4));
                                                    savColCount++;
                                                }
                                                receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                                receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                                receipt.ReceiptLines[rLines].colCount = savColCount;
                                            }

                                            rLines++;
                                        }
                                        rLines = rLines - 1;
                                    }
                                    else
                                    {
                                        rLines = rLines - 1;
                                    }
                                    break;
                                }
                            case "REDEMPTION_SOURCE_TOTAL":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@TotalTickets", TicketValueInStringFormat(GetTotalRedemptionAllocationTickets(sqlTrx)));
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                        receipt.ReceiptLines[rLines].colCount = savColCount;
                                    }

                                    break;
                                }
                            case "REDEMPTION_BALANCE":
                                {
                                    int savColCount = 0;
                                    KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>();
                                    if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionReceiptNo")))
                                    {
                                        keyValuePair = GetBalanceTicketsDetails(RedemptionDTO.RedemptionTicketSource.TICKET_RECEIPT, sqlTrx);
                                    }
                                    else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCardNo")))
                                    {
                                        keyValuePair = GetBalanceTicketsDetails(RedemptionDTO.RedemptionTicketSource.CARD, sqlTrx);
                                    }
                                    if (keyValuePair.Value > 0)
                                    {
                                        foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                        {
                                            line = "";
                                            if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                            {
                                                line = receiptColumnData.Data;
                                                if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionReceiptNo")))
                                                {
                                                    line = line.Replace("@RedemptionReceiptNo", keyValuePair.Key);
                                                    line = line.Replace("@TicketValue", TicketValueInStringFormat(keyValuePair.Value));
                                                    savColCount++;
                                                }
                                                else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCardNo")))
                                                {
                                                    line = line.Replace("@RedemptionCardNo", keyValuePair.Key);
                                                    line = line.Replace("@TicketValue", TicketValueInStringFormat(keyValuePair.Value));
                                                    savColCount++;
                                                }
                                            }
                                            receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                            receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                        }
                                        receipt.ReceiptLines[rLines].colCount = savColCount;
                                    }
                                    break;
                                }
                            default: break;
                        }
                        rLines++;
                    }
                }
                receipt.TotalLines = rLines;
            }
            log.LogVariableState("receipt class", receipt);
            log.LogMethodExit(receipt);
            return receipt;
        }

        private string GetApproverId(SqlTransaction sqlTransaction)
        {
            string approverId = string.Empty;
            ReedemptionLogsListBL redemptionUserLogsListBL = new ReedemptionLogsListBL(executionContext);
            List<KeyValuePair<RedemptionUserLogsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RedemptionUserLogsDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<RedemptionUserLogsDTO.SearchByParameters, string>(RedemptionUserLogsDTO.SearchByParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()));
            List<RedemptionUserLogsDTO> redemptionUserLogsDTOs = redemptionUserLogsListBL.GetRedemptionUserLogsDTOList(searchParams,sqlTransaction);
            if (redemptionUserLogsDTOs != null && redemptionUserLogsDTOs.Any())
            {
                approverId = redemptionUserLogsDTOs[0].ApproverId;
            }
            return approverId;
        }

        public KeyValuePair<string, int> GetBalanceTicketsDetails(RedemptionDTO.RedemptionTicketSource redemptionTicketSource, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(redemptionTicketSource, sqlTransaction);
            KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>();
            LoadRedemptionTicketAllocationDTOList(sqlTransaction);
            switch (redemptionTicketSource)
            {
                case RedemptionDTO.RedemptionTicketSource.CARD:
                    {
                        if (redemptionDTO.RedemptionTicketAllocationListDTO.Exists(line => (line.TrxId != -1) && (line.RedemptionGiftId == -1)))
                        {
                            int trxId = -1;
                            int trxLineId = -1;
                            string cardNumber = string.Empty;
                            int ticketsLoadedToCard = 0;
                            ticketsLoadedToCard = Convert.ToInt32(redemptionDTO.RedemptionTicketAllocationListDTO.Where(line => (line.TrxId != -1) && (line.RedemptionGiftId == -1)).Sum(line => (line.ETickets == null ? 0 : line.ETickets)
                                                                                                         + (line.ManualTickets == null ? 0 : line.ManualTickets)
                                                                                                         + (line.ReceiptTickets == null ? 0 : line.ReceiptTickets)
                                                                                                         + (line.CurrencyTickets == null ? 0 : line.CurrencyTickets)
                                                                                                         + (line.RedemptionCurrencyRuleTicket == null ? 0 : line.RedemptionCurrencyRuleTicket)));
                            if (ticketsLoadedToCard > 0)
                            {
                                trxId = redemptionDTO.RedemptionTicketAllocationListDTO.Find(line => (line.TrxId != -1) && (line.RedemptionGiftId == -1)).TrxId;
                                trxLineId = redemptionDTO.RedemptionTicketAllocationListDTO.Find(line => (line.TrxLineId != -1) && (line.RedemptionGiftId == -1)).TrxLineId;
                                if (trxId != -1 && trxLineId != -1)
                                {
                                    using (Utilities parafaitUtils = new Utilities())
                                    {
                                        Semnox.Parafait.Transaction.Transaction transaction = new Semnox.Parafait.Transaction.Transaction();
                                        TransactionUtils trxUtils = new TransactionUtils(parafaitUtils);
                                        transaction = trxUtils.CreateTransactionFromDB(trxId, parafaitUtils);
                                        cardNumber = transaction.GetLinkedCardNumber(trxLineId);
                                    }
                                }
                                keyValuePair = new KeyValuePair<string, int>(cardNumber, ticketsLoadedToCard);
                            }
                        }
                    }
                    break;
                case RedemptionDTO.RedemptionTicketSource.TICKET_RECEIPT:
                    {
                        string receiptNumber = string.Empty;
                        if (redemptionDTO.RedemptionTicketAllocationListDTO.Exists(line => (line.RedemptionGiftId == -1) && (line.TrxId == -1)))
                        {
                            int manualTicketReceiptId = redemptionDTO.RedemptionTicketAllocationListDTO.Find(line => (line.RedemptionGiftId == -1) && (line.TrxId == -1)).ManualTicketReceiptId;
                            int ticketValue = 0;
                            //int ticketValue = Convert.ToInt32(redemptionDTO.RedemptionTicketAllocationListDTO.Find(li => li.RedemptionGiftId != -1).ReceiptTickets);
                            TicketReceiptList ticketReceipt = new TicketReceiptList(executionContext);
                            List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>
                            {
                            new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()),
                            new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, executionContext.GetSiteId().ToString())
                            };
                            List<TicketReceiptDTO> ticketReceiptDTOList = new List<TicketReceiptDTO>();
                            ticketReceiptDTOList = ticketReceipt.GetAllTicketReceipt(searchParams,sqlTransaction);
                            if (ticketReceiptDTOList != null && ticketReceiptDTOList.Count > 0)
                            {
                                receiptNumber = ticketReceiptDTOList[0].ManualTicketReceiptNo;
                                ticketValue = Convert.ToInt32(ticketReceiptDTOList[0].Tickets);
                            }
                            if (ticketValue > 0)
                            {
                                receiptNumber = GetMaskedReceiptText(receiptNumber);
                                keyValuePair = new KeyValuePair<string, int>(receiptNumber, ticketValue);
                            }
                        }
                    }
                    break;
                default: break;
            }
            log.LogMethodExit(keyValuePair);
            return keyValuePair;
        }
        private void LoadRedemptionTicketAllocationDTOList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (redemptionDTO != null && redemptionDTO.RedemptionTicketAllocationListDTO != null)//&& redemptionDTO.RedemptionTicketAllocationListDTO.Count == 0)
            {
                List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList = new List<RedemptionTicketAllocationDTO>();
                RedemptionTicketAllocationListBL redemptionTicketAllocationBL = new RedemptionTicketAllocationListBL(executionContext);
                List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()),
                    new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString())
                };
                redemptionTicketAllocationDTOList = redemptionTicketAllocationBL.GetRedemptionTicketAllocationDTOList(searchParams, sqlTransaction);
                redemptionDTO.RedemptionTicketAllocationListDTO = redemptionTicketAllocationDTOList;
            }
            log.LogMethodExit();
        }
        private void LoadRedemptionGiftDTOList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (redemptionDTO != null && redemptionDTO.RedemptionGiftsListDTO != null)//&& redemptionDTO.RedemptionTicketAllocationListDTO.Count == 0)
            {
                List<RedemptionGiftsDTO> redemptionGiftsDTOList = new List<RedemptionGiftsDTO>();
                RedemptionGiftsListBL redemptionGiftsBL = new RedemptionGiftsListBL(executionContext);
                List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>(RedemptionGiftsDTO.SearchByParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()),
                    new KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>(RedemptionGiftsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString())
                };
                redemptionGiftsDTOList = redemptionGiftsBL.GetRedemptionGiftsDTOList(searchParams, sqlTransaction);
                redemptionDTO.RedemptionGiftsListDTO = redemptionGiftsDTOList;
            }
            log.LogMethodExit();
        }
        private void LoadRedemptionCardDTOList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (redemptionDTO != null && redemptionDTO.RedemptionCardsListDTO != null)//&& redemptionDTO.RedemptionTicketAllocationListDTO.Count == 0)
            {
                List<RedemptionCardsDTO> redemptionCardsDTOList = new List<RedemptionCardsDTO>();
                RedemptionCardsListBL redemptionCardsBL = new RedemptionCardsListBL(executionContext);
                List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>(RedemptionCardsDTO.SearchByParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()),
                    new KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>(RedemptionCardsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString())
                };
                redemptionCardsDTOList = redemptionCardsBL.GetRedemptionCardsDTOList(searchParams, sqlTransaction);
                redemptionDTO.RedemptionCardsListDTO = redemptionCardsDTOList;
            }
            log.LogMethodExit();
        }
        private List<Tuple<string, decimal, int, int>> GetReedemptionCurrencyInfo(bool thisIsReversedRedemption,SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(thisIsReversedRedemption);
            List<Tuple<string, decimal, int, int>> currencyTicketAllocationTupleList = new List<Tuple<string, decimal, int, int>>();
            if (this.redemptionDTO.RedemptionTicketAllocationListDTO != null
                && this.redemptionDTO.RedemptionTicketAllocationListDTO.Any())
            {
                List<int> currencyIdList = redemptionDTO.RedemptionTicketAllocationListDTO.Where(tline => tline.CurrencyId != -1).Select(line => line.CurrencyId).Distinct().ToList();
                if (currencyIdList != null && currencyIdList.Count > 0)
                {
                    string redemptionCurrencyIds = string.Empty;
                    for (int i = 0; i < currencyIdList.Count; i++)
                    {
                        redemptionCurrencyIds += currencyIdList[i] + ",";
                    }
                    if (!String.IsNullOrEmpty(redemptionCurrencyIds))
                    {
                        redemptionCurrencyIds = redemptionCurrencyIds.Substring(0, redemptionCurrencyIds.Length - 1);
                        RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(executionContext);
                        List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = new List<RedemptionCurrencyDTO>();
                        List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParams = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>
                        {
                               new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_ID_LIST, redemptionCurrencyIds),
                               new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SITE_ID, executionContext.GetSiteId().ToString())
                        };
                        redemptionCurrencyDTOList = redemptionCurrencyList.GetAllRedemptionCurrency(searchParams,0,10, sqlTransaction);
                        if (redemptionCurrencyDTOList != null && redemptionCurrencyDTOList.Count > 0)
                        {
                            foreach (RedemptionCurrencyDTO currency in redemptionCurrencyDTOList)
                            {
                                int currencyTicketValue = Convert.ToInt32(redemptionDTO.RedemptionTicketAllocationListDTO.Where(tline => tline.CurrencyId == currency.CurrencyId
                                                                                                                                 && ((thisIsReversedRedemption == true && tline.RedemptionGiftId != -1)
                                                                                                                                 || (thisIsReversedRedemption == false))).Sum(tl => tl.CurrencyTickets));
                                if (thisIsReversedRedemption)
                                {
                                    currencyTicketValue = (currencyTicketValue * -1);
                                }
                                decimal currencyQuantity = (decimal)Math.Round((currencyTicketValue / currency.ValueInTickets), 2);
                                currencyTicketAllocationTupleList.Add(new Tuple<string, decimal, int, int>(currency.CurrencyName, currencyQuantity, Convert.ToInt32(currency.ValueInTickets), currencyTicketValue));
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(currencyTicketAllocationTupleList);
            return currencyTicketAllocationTupleList;
        }
        private bool IsThisReversedRedemption()
        {
            log.LogMethodEntry();
            bool reversedRedemption = false;
            if (this.redemptionDTO != null && this.redemptionDTO.OrigRedemptionId != -1)
            {
                reversedRedemption = true;
            }
            log.LogMethodExit(reversedRedemption);
            return reversedRedemption;
        }
        private List<Tuple<string, decimal, int, int>> GetRedemptionReceiptInfoForPrint(bool thisIsReversedRedemption,SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(thisIsReversedRedemption);
            List<Tuple<string, decimal, int, int>> receiptAllocationTupleList = new List<Tuple<string, decimal, int, int>>();
            if (this.redemptionDTO.RedemptionTicketAllocationListDTO != null
                && this.redemptionDTO.RedemptionTicketAllocationListDTO.Any())
            {
                List<int> idList = redemptionDTO.RedemptionTicketAllocationListDTO.Where(tline => tline.ManualTicketReceiptId > -1).Select(line => line.ManualTicketReceiptId).Distinct().ToList();
                if (idList != null && idList.Count > 0)
                {
                    string manualTicketReceiptIds = string.Empty;
                    for (int i = 0; i < idList.Count; i++)
                    {
                        manualTicketReceiptIds += idList[i] + ",";
                    }
                    if (!String.IsNullOrEmpty(manualTicketReceiptIds))
                    {
                        manualTicketReceiptIds = manualTicketReceiptIds.Substring(0, manualTicketReceiptIds.Length - 1);
                        TicketReceiptList ticketReceiptList = new TicketReceiptList(executionContext);
                        List<TicketReceiptDTO> ticketReceiptDTOList = new List<TicketReceiptDTO>();
                        List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>
                                {
                                    new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_RECEIPT_IDS, manualTicketReceiptIds),
                                    new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, executionContext.GetSiteId().ToString()),
                                };
                        ticketReceiptDTOList = ticketReceiptList.GetAllTicketReceipt(searchParams, sqlTransaction);
                        if (ticketReceiptDTOList != null && ticketReceiptDTOList.Count > 0)
                        {
                            foreach (TicketReceiptDTO ticket in ticketReceiptDTOList)
                            {
                                int? ticketValue = redemptionDTO.RedemptionTicketAllocationListDTO.Where(tline => (tline.ManualTicketReceiptId == ticket.Id)
                                                                                                              && ((thisIsReversedRedemption == true && tline.RedemptionGiftId != -1)
                                                                                                                   || (thisIsReversedRedemption == false))
                                                                                                                 ).Sum(tl => tl.ReceiptTickets);
                                if (thisIsReversedRedemption)
                                {
                                    ticketValue = (ticketValue * -1);
                                }

                                string receiptNumber = ticket.ManualTicketReceiptNo;
                                receiptNumber = GetMaskedReceiptText(receiptNumber);
                                receiptAllocationTupleList.Add(new Tuple<string, decimal, int, int>(receiptNumber, 1, Convert.ToInt32(ticketValue), Convert.ToInt32(ticketValue)));
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(receiptAllocationTupleList);
            return receiptAllocationTupleList;
        }
        private string GetMaskedReceiptText(string receiptNumber)
        {
            log.LogMethodEntry(receiptNumber);
            string maskedReceiptNumber = string.Empty;
            maskedReceiptNumber = receiptNumber.Substring(0, 4) + "***" + receiptNumber.Substring(receiptNumber.Length - 5);
            log.LogMethodExit(maskedReceiptNumber);
            return maskedReceiptNumber;
        }
        /// <summary>
        /// GetRedemptionCardInfoForPrint
        /// </summary>
        /// <param name="thisIsReversedRedemption">thisIsReversedRedemption</param>
        /// <returns></returns>
        private List<Tuple<string, decimal, int, int>> GetRedemptionCardInfoForPrint(bool thisIsReversedRedemption,SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(thisIsReversedRedemption);
            List<Tuple<string, decimal, int, int>> cardAllocationTupleList = new List<Tuple<string, decimal, int, int>>();
            if (this.redemptionDTO.RedemptionTicketAllocationListDTO != null
                && this.redemptionDTO.RedemptionTicketAllocationListDTO.Any())
            {
                LoadRedemptionCardDTOList(sqlTransaction);
                if (this.redemptionDTO.RedemptionCardsListDTO != null && this.redemptionDTO.RedemptionCardsListDTO.Any())
                {
                    foreach (RedemptionCardsDTO card in this.redemptionDTO.RedemptionCardsListDTO)
                    {
                        int? CardTicketValue = redemptionDTO.RedemptionTicketAllocationListDTO.Where(tline => (tline.CardId == card.CardId)
                                                                                                              && ((thisIsReversedRedemption == true && tline.RedemptionGiftId != -1)
                                                                                                                   || (thisIsReversedRedemption == false))
                                                                                                                 ).Sum(tl => tl.ETickets);
                        if (thisIsReversedRedemption)
                        {
                            CardTicketValue = (CardTicketValue * -1);
                        }
                        if (CardTicketValue > 0)
                        {
                            cardAllocationTupleList.Add(new Tuple<string, decimal, int, int>(card.CardNumber, 1, Convert.ToInt32(CardTicketValue), Convert.ToInt32(CardTicketValue)));
                        }
                    }
                }
            }
            log.LogMethodExit(cardAllocationTupleList);
            return cardAllocationTupleList;
        }

        /// <summary>
        /// GetTicketAllocationDetails
        /// </summary>
        /// <param name="redemptionTicketSource">redemptionTicketSource</param>
        /// <returns></returns>
        public List<Tuple<string, decimal, int, int>> GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource redemptionTicketSource, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(redemptionTicketSource, sqlTransaction);
            List<Tuple<string, decimal, int, int>> ticketAllocationTupleList = new List<Tuple<string, decimal, int, int>>();
            LoadRedemptionTicketAllocationDTOList(sqlTransaction);
            bool thisIsReversedRedemption = IsThisReversedRedemption();
            switch (redemptionTicketSource)
            {
                case RedemptionDTO.RedemptionTicketSource.CARD:
                    ticketAllocationTupleList = GetRedemptionCardInfoForPrint(thisIsReversedRedemption,sqlTransaction);
                    break;
                case RedemptionDTO.RedemptionTicketSource.TICKET_RECEIPT:
                    ticketAllocationTupleList = GetRedemptionReceiptInfoForPrint(thisIsReversedRedemption, sqlTransaction);
                    break;
                case RedemptionDTO.RedemptionTicketSource.REDEMPTION_CURRENCY:
                    ticketAllocationTupleList = GetReedemptionCurrencyInfo(thisIsReversedRedemption,sqlTransaction);
                    break;
                case RedemptionDTO.RedemptionTicketSource.GRACE_TICKETS:
                    {
                        int graceTicketsValue = 0;
                        if (thisIsReversedRedemption)
                        {
                            graceTicketsValue = Convert.ToInt32(redemptionDTO.RedemptionTicketAllocationListDTO.Where(line => line.RedemptionGiftId != -1).Sum(line => line.GraceTickets));
                            graceTicketsValue = graceTicketsValue * -1;
                        }
                        else
                        {
                            graceTicketsValue = Convert.ToInt32(redemptionDTO.RedemptionTicketAllocationListDTO.Sum(line => line.GraceTickets));
                        }
                        if (graceTicketsValue > 0)
                        {
                            ticketAllocationTupleList.Add(new Tuple<string, decimal, int, int>("", 1, graceTicketsValue, graceTicketsValue));
                        }
                    }
                    break;
                case RedemptionDTO.RedemptionTicketSource.MANUAL_TICKETS:
                    {
                        int manualTicketsValue = 0;
                        if (thisIsReversedRedemption)
                        {
                            manualTicketsValue = Convert.ToInt32(redemptionDTO.RedemptionTicketAllocationListDTO.Where(line => line.RedemptionGiftId != -1).Sum(line => line.ManualTickets));
                            manualTicketsValue = manualTicketsValue * -1;
                        }
                        else
                        {
                            manualTicketsValue = Convert.ToInt32(redemptionDTO.RedemptionTicketAllocationListDTO.Sum(line => line.ManualTickets));
                        }
                        if (manualTicketsValue > 0)
                        {
                            ticketAllocationTupleList.Add(new Tuple<string, decimal, int, int>("", 1, manualTicketsValue, manualTicketsValue));
                        }
                    }
                    break;
                case RedemptionDTO.RedemptionTicketSource.TURNIN_TICKETS:
                    {
                        int? turnInTicketsValue = 0;
                        turnInTicketsValue = redemptionDTO.RedemptionTicketAllocationListDTO.Where(li => li.RedemptionGiftId == -1).Sum(line => line.TurnInTickets);
                        if (turnInTicketsValue > 0)
                        {
                            ticketAllocationTupleList.Add(new Tuple<string, decimal, int, int>("", 1, Convert.ToInt32(turnInTicketsValue), Convert.ToInt32(turnInTicketsValue)));
                        }
                    }
                    break;
                case RedemptionDTO.RedemptionTicketSource.REDEMPTION_CURRENCY_RULE:
                    ticketAllocationTupleList = GetRedemptionCurrencyRuleInfo(thisIsReversedRedemption,sqlTransaction);
                    break;
                default: break;

            }
            log.LogMethodExit(ticketAllocationTupleList);
            return ticketAllocationTupleList;

        }
        private List<Tuple<string, decimal, int, int>> GetRedemptionCurrencyRuleInfo(bool thisIsReversedRedemption,SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(thisIsReversedRedemption);
            List<Tuple<string, decimal, int, int>> currencyRuleTicketAllocationTupleList = new List<Tuple<string, decimal, int, int>>();
            if (this.redemptionDTO.RedemptionTicketAllocationListDTO != null
                && this.redemptionDTO.RedemptionTicketAllocationListDTO.Any())
            {
                List<int> currencyRuleIdList = redemptionDTO.RedemptionTicketAllocationListDTO.Where(tline => tline.RedemptionCurrencyRuleId != -1).Select(line => line.RedemptionCurrencyRuleId).Distinct().ToList();
                if (currencyRuleIdList != null && currencyRuleIdList.Count > 0)
                {
                    string redemptionCurrencyRuleIds = string.Empty;
                    for (int i = 0; i < currencyRuleIdList.Count; i++)
                    {
                        redemptionCurrencyRuleIds += currencyRuleIdList[i] + ",";
                    }
                    if (!String.IsNullOrEmpty(redemptionCurrencyRuleIds))
                    {
                        redemptionCurrencyRuleIds = redemptionCurrencyRuleIds.Substring(0, redemptionCurrencyRuleIds.Length - 1);
                        RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL(executionContext);
                        List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList = new List<RedemptionCurrencyRuleDTO>();
                        List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParams = new List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>>
                        {
                               new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID_LIST, redemptionCurrencyRuleIds),
                               new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.SITE_ID, executionContext.GetSiteId().ToString())
                        };
                        redemptionCurrencyRuleDTOList = redemptionCurrencyRuleListBL.GetAllRedemptionCurrencyRuleList(searchParams, false,true,0,0,sqlTransaction);
                        if (redemptionCurrencyRuleDTOList != null && redemptionCurrencyRuleDTOList.Count > 0)
                        {
                            foreach (RedemptionCurrencyRuleDTO currencyRule in redemptionCurrencyRuleDTOList)
                            {

                                int? ruleTicketSum = redemptionDTO.RedemptionTicketAllocationListDTO.Where(tline => tline.RedemptionCurrencyRuleId == currencyRule.RedemptionCurrencyRuleId).Select(line => line.RedemptionCurrencyRuleTicket).Sum();
                                decimal? ruleQuantitySum = redemptionDTO.RedemptionTicketAllocationListDTO.Where(tline => tline.RedemptionCurrencyRuleId == currencyRule.RedemptionCurrencyRuleId).Select(line => line.CurrencyQuantity).Sum();
                                int ruleDetlaTicketValue = 0;
                                ruleDetlaTicketValue = (int)((ruleTicketSum == null ? 0 : ruleTicketSum) / (ruleQuantitySum == null ? 1 : ruleQuantitySum));
                                if (thisIsReversedRedemption)
                                {
                                    ruleTicketSum = ruleTicketSum * -1;

                                }
                                decimal ruleQuantity = (ruleQuantitySum == null ? 0 : (decimal)ruleQuantitySum);
                                int totalRuleTicketValue = (ruleTicketSum == null ? 0 : (int)ruleTicketSum);
                                currencyRuleTicketAllocationTupleList.Add(new Tuple<string, decimal, int, int>(currencyRule.RedemptionCurrencyRuleName, ruleQuantity, ruleDetlaTicketValue, totalRuleTicketValue));

                            }
                        }
                    }
                }
            }
            log.LogMethodExit(currencyRuleTicketAllocationTupleList);
            return currencyRuleTicketAllocationTupleList;
        }

        private List<Tuple<string, decimal, int, int>> GetRedemptionSourceTicketList(List<ReceiptColumnData> receiptTemplateColList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(receiptTemplateColList);
            List<Tuple<string, decimal, int, int>> redemptionSourceTupleList = new List<Tuple<string, decimal, int, int>>();
            if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionReceiptNo")))
            {
                redemptionSourceTupleList = GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.TICKET_RECEIPT, sqlTransaction);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCardNo")))
            {
                redemptionSourceTupleList = GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.CARD, sqlTransaction);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCurrencyNo")))
            {
                redemptionSourceTupleList = GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.REDEMPTION_CURRENCY, sqlTransaction);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCurrencyRule")))
            {
                redemptionSourceTupleList = GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.REDEMPTION_CURRENCY_RULE, sqlTransaction);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionManualTickets")))
            {
                redemptionSourceTupleList = GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.MANUAL_TICKETS, sqlTransaction);
            }

            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionGraceTickets")))
            {
                redemptionSourceTupleList = GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.GRACE_TICKETS, sqlTransaction);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@TurnInTickets")))
            {
                redemptionSourceTupleList = GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.TURNIN_TICKETS, sqlTransaction);
            }
            log.LogMethodExit(redemptionSourceTupleList);
            return redemptionSourceTupleList;
        }

        /// <summary>
        /// print new manual ticket receipt
        /// </summary>
        public clsTicket PrintManualTicketReceipt(TicketReceiptDTO ticketReceiptDTO,  Utilities utilities, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(ticketReceiptDTO, sqlTrx);
            clsTicket clsTicket = null;
            try
            {
                if (ticketReceiptDTO != null)
                {
                    clsTicket = SetupTicketPrint(ticketReceiptDTO.ManualTicketReceiptNo, ticketReceiptDTO.Tickets, utilities, sqlTrx);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //throw new Exception(MessageContainerList.GetMessage(executionContext, 1499)); //Error Printing Ticket receipt
                throw new Exception(MessageContainerList.GetMessage(executionContext, ex.Message));// modified to show message Invalid ticket template
            }
            log.LogMethodExit();
            return clsTicket;
        }



        /// <summary>
        /// CreateManualTicketReceipt
        /// </summary>
        public clsTicket CreateManualTicketReceipt(int totalTickets, SqlTransaction sqlTrx, Utilities utilities)
        {
            log.LogMethodEntry(totalTickets, sqlTrx);
            clsTicket clsTicket = PrintRealTicketReceipt(redemptionDTO.RedemptionId, totalTickets, utilities, sqlTrx);
            log.LogMethodExit(clsTicket);
            return clsTicket;
        }
        private clsTicket SetupTicketPrint(string barCodeText, int tickets, Utilities utilities, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(barCodeText, tickets, sqlTrx);
            Printer.clsTicket ticket = new Printer.clsTicket();
            PrinterBL printerBL = new PrinterBL(executionContext);
            int ticketReceiptTemplate = -1;
            try
            {
                ticketReceiptTemplate = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "TICKET_VOUCHER_TEMPLATE");
            }
            catch { ticketReceiptTemplate = -1; }

            if (ticketReceiptTemplate == -1)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Ticket template is not setup.Unable to print"));
            }
            else
            {

                log.LogMethodEntry(ticketReceiptTemplate, barCodeText, tickets, sqlTrx);
                clsTicketTemplate ticketTemplate = new clsTicketTemplate(ticketReceiptTemplate, utilities);
                printerBL = new PrinterBL(executionContext);
                ticket.TicketBorderProperty = new Rectangle(new Point(0, 0), ticketTemplate.getTicketSize());
                ticket.MarginProperty = ticketTemplate.Header.Margins;
                ticket.BorderWidthProperty = ticketTemplate.Header.BorderWidth;
                ticket.PaperSizeProperty = new PaperSize("custom", (int)(ticketTemplate.Header._Width * 100), (int)(ticketTemplate.Header._Height * 100));
                string dateFormat = utilities.ParafaitEnv.DATE_FORMAT;
                string dateTimeFormat = utilities.ParafaitEnv.DATETIME_FORMAT;
                string numberFormat = utilities.ParafaitEnv.AMOUNT_FORMAT;
                string redemptionDate = "";
                string redemptionId = "";
                string redemptionOrderNo = "";
                string redemptionCardNumber = "";
                string redemptionRemarks = "";
                string redemptionCustomerName = "";
                string cardTickets = "";
                if (redemptionDTO != null)
                {
                    redemptionDate = Convert.ToDateTime(redemptionDTO.RedeemedDate).ToString(dateTimeFormat);
                    redemptionId = redemptionDTO.RedemptionId.ToString();
                    redemptionOrderNo = redemptionDTO.RedemptionOrderNo;
                    redemptionCardNumber = redemptionDTO.PrimaryCardNumber;
                    redemptionRemarks = redemptionDTO.Remarks;
                    redemptionCustomerName = redemptionDTO.CustomerName;
                    if (redemptionDTO.CardId != -1)
                    {
                        RedemptionCardsDTO primaryRedemptionCardsDTO = redemptionDTO.RedemptionCardsListDTO.Find(cardLine => cardLine.CardId == redemptionDTO.CardId);
                        if (primaryRedemptionCardsDTO != null)
                        {
                            cardTickets = primaryRedemptionCardsDTO.TotalCardTickets.ToString();
                        }
                        else
                        {
                            cardTickets = "";
                            log.LogVariableState("Unable to fetch primaryRedemptionCardsDTO", cardTickets);
                        }
                    }

                }
                foreach (clsTicketTemplate.clsTicketElement element in ticketTemplate.TicketElements)
                {
                    Printer.clsTicket.PrintObject printObject = new Printer.clsTicket.PrintObject();
                    printObject.FontProperty = element.Font;
                    printObject.LocationProperty = element.Location;
                    ticket.PrintObjectList.Add(printObject);
                    printObject.AlignmentProperty = element.Alignment;
                    printObject.WidthProperty = element.Width;
                    printObject.RotateProperty = element.Rotate;
                    printObject.ColorProperty = element.Color;
                    string barCodeEncodeFormat = (element.formatId != -1) ? POSPrint.GetFormat(element.formatId, "BARCODE_ENCODE_TYPE") : BarcodeLib.TYPE.CODE128.ToString();
                    printObject.BarCodeHeightProperty = element.BarCodeHeight;
                    printObject.BarCodeEncodeTypeProperty = barCodeEncodeFormat;


                    string line = element.Value.Replace("@SiteName", ((string.IsNullOrEmpty(utilities.ParafaitEnv.POS_LEGAL_ENTITY) == false) ? utilities.ParafaitEnv.POS_LEGAL_ENTITY : utilities.ParafaitEnv.SiteName)).Replace
                                                ("@Date", redemptionDate).Replace
                                                ("@SystemDate", ServerDateTime.Now.ToString(dateTimeFormat)).Replace
                                                ("@TrxNo", redemptionOrderNo).Replace
                                                ("@TrxOTP", "").Replace
                                                ("@Cashier", executionContext.GetUserId()).Replace
                                                ("@Token", "").Replace
                                                ("@POS", utilities.ParafaitEnv.POSMachine).Replace
                                                ("@TaxNo", "").Replace
                                                ("@PrimaryCardNumber", redemptionCardNumber).Replace
                                                ("@CardNumber", redemptionCardNumber).Replace
                                                ("@CustomerName", redemptionCustomerName).Replace
                                                ("@Phone", "").Replace
                                                ("@Remarks", redemptionRemarks).Replace
                                                ("@CardBalance", "").Replace
                                                ("@CreditBalance", "").Replace
                                                ("@BonusBalance", "").Replace
                                                ("@SiteAddress", utilities.ParafaitEnv.SiteAddress).Replace
                                                ("@CardTickets", TicketValueInStringFormat(cardTickets))//.Replace
                                                                                                        //("@ScreenNumber", "")
                                                ;

                    line = line.Replace("@Product", "").Replace
                                            ("@Price", "").Replace
                                            ("@Quantity", "").Replace
                                            ("@Amount", "").Replace
                                            ("@LineRemarks", "").Replace
                                            ("@TaxName", "").Replace
                                            ("@Tax", "").Replace
                                            ("@Time", "").Replace
                                            ("@FromTime", "").Replace
                                            ("@ToTime", "").Replace
                                            ("@Seat", "").Replace
                                            ("@Tickets", TicketValueInStringFormat(tickets)).Replace
                                            ("@TicketBarCodeNo", barCodeText);
                    if (line.Contains("@TicketBarCode"))
                    {
                        line = line.Replace("@TicketBarCode", "");
                        // if (barcodeImage != null)
                        // {
                        // printObject.BarCodeProperty = barcodeImage;
                        int weight = 1;
                        if (printObject.FontProperty.Size >= 16)
                            weight = 3;
                        else if (printObject.FontProperty.Size >= 12)
                            weight = 2;
                        printObject.BarCodeProperty = printerBL.MakeBarcodeLibImage(weight, printObject.BarCodeHeightProperty, printObject.BarCodeEncodeTypeProperty, barCodeText);
                        // }
                    }

                    line = line.Replace("@Total", "").Replace
                                            ("@TaxTotal", "");

                    line = line.Replace("@CouponNumber", "").Replace
                                            ("@DiscountName", "").Replace
                                            ("@DiscountPercentage", "").Replace
                                            ("@DiscountAmount", "").Replace
                                            ("@CouponEffectiveDate", "").Replace
                                            ("@CouponExpiryDate", "");



                    line = line.Replace("@BarCodeCouponNumber", "").Replace
                                            ("@BarCodeCardNumber", "");
                    line = line.Replace("@QRCodeCouponNumber", "").Replace
                                           ("@QRCodeCardNumber", "");

                    if (redemptionId != "-1")
                        line = line.Replace("@TrxId", redemptionId);
                    else
                        line = line.Replace("@TrxId", "");

                    if (line.Contains("@SiteLogo"))
                    {
                        line = line.Replace("@SiteLogo", "");
                        printObject.ImageProperty = utilities.ParafaitEnv.CompanyLogo;
                    }

                    if (line.Contains("@CustomerPhoto"))
                    {
                        line = line.Replace("@CustomerPhoto", "");
                    }

                    printObject.TextProperty = line;
                }

                if (!string.IsNullOrEmpty(redemptionCardNumber))
                {
                    ticket.CardNumber = redemptionCardNumber;
                }

                ticket.BackgroundImage = ticketTemplate.Header.BackgroundImage;
                if (!string.IsNullOrEmpty(redemptionId))
                {
                    ticket.TrxId = Convert.ToInt32(redemptionId);
                }
            }
            log.LogMethodExit(ticket);
            return ticket;
        }

        public clsTicket ReprintManualTicketReceipt(int ticketId, RedemptionActivityDTO redemptionActivityDTO, Utilities utilities, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(ticketId, redemptionActivityDTO, utilities);
            TicketReceipt ticketReceipt = new TicketReceipt(executionContext, ticketId,sqlTransaction);
            clsTicket clsTicket;
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "MANAGER_APPROVAL_TO_REPRINT_TICKET_RECEIPT"))
            {
                if (ManagerApprovalReceived(redemptionActivityDTO.ManagerToken) == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                }
            }
            if (ticketReceipt.TicketReceiptDTO.BalanceTickets <= 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 112));
            }
            if (ticketReceipt.TicketReceiptDTO != null)
            {
                ticketReceipt.TicketReceiptDTO.ReprintCount += 1;
            }
            try
            {
                ticketReceipt.Save(sqlTransaction);
                clsTicket = PrintManualTicketReceipt(ticketReceipt.TicketReceiptDTO, utilities,sqlTransaction);
                RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO();
                redemptionUserLogsDTO.Action = RedemptionUserLogsDTO.RedemptionAction.REPRINT_TICKET.ToString();
                redemptionUserLogsDTO.LoginId = executionContext.GetUserId();
                redemptionUserLogsDTO.TicketReceiptId = ticketReceipt.TicketReceiptDTO.Id;

                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "MANAGER_APPROVAL_TO_REPRINT_TICKET_RECEIPT"))
                {
                    if (ManagerApprovalReceived(redemptionActivityDTO.ManagerToken))
                    {
                        redemptionUserLogsDTO.ApproverId = approvalId;
                        redemptionUserLogsDTO.ApprovalTime = ServerDateTime.Now;
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Manager approval required"));
                    }
                }
                redemptionUserLogsDTO.TicketReceiptId = ticketId;
                RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
                redemptionUserLogsBL.Save(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.LogMethodExit();
                throw ex;
            }
            log.LogMethodExit(clsTicket);
            return clsTicket;
        }

        static public string TicketValueInStringFormat(object ticketValue)
        {
            log.LogMethodEntry(ticketValue);
            string valueInString = string.Empty;
            if (ticketValue != null && string.IsNullOrWhiteSpace(ticketValue.ToString()) == false)
            {
                try
                {
                    int valueInInt = valueInInt = Convert.ToInt32(ticketValue);
                    decimal convertedValueInDecimal = Convert.ToDecimal(valueInInt);
                    decimal actualValueInDecimal = Convert.ToDecimal(ticketValue);
                    if (actualValueInDecimal == convertedValueInDecimal)
                    {
                        valueInString = valueInInt.ToString();
                    }
                    else
                    {   //decimal value, send it as decimal only
                        valueInString = ticketValue.ToString();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    valueInString = ticketValue.ToString();
                }
            }
            log.LogMethodExit(valueInString);
            return valueInString;
        }

        public clsTicket PrintRealTicketReceipt(int sourceRedemptionId, int Tickets, Utilities utilities, SqlTransaction sqlTrx, DateTime? issueDate = null)
        {
            log.LogMethodEntry(sourceRedemptionId, Tickets, utilities, sqlTrx, issueDate);
            clsTicket clsTicket = null;
            try
            {
                if (Tickets > 0)
                {
                    string BarCodeText = string.Empty;

                    TicketStationFactory ticketStationFactory = new TicketStationFactory();
                    POSCounterTicketStationBL posCounterTicketStationBL = ticketStationFactory.GetPosCounterTicketStationObject();
                    if (posCounterTicketStationBL == null)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2322));
                    }
                    else
                    {
                        BarCodeText = posCounterTicketStationBL.GenerateBarCode(Tickets);
                    }
                    clsTicket = SetupTicketPrint(BarCodeText, Tickets, utilities, sqlTrx);
                    int newReceiptId = InsertPhysicalReceiptToDB(sourceRedemptionId, BarCodeText, Tickets, sqlTrx, issueDate);
                    log.Info("Ends-printRealTicketReceipt(" + Tickets + ",message) as Ticket Receipt Printed");
                    return clsTicket;

                }
                else
                {
                    //message = MessageContainerList.GetMessage(executionContext, 145);
                    log.Info("Ends-printRealTicketReceipt(" + Tickets + ",message) as Nothing to Print");
                    return clsTicket;
                }
            }
            catch (Exception ex)
            {
                //message = ex.Message;
                log.Fatal("Ends-printRealTicketReceipt(" + Tickets + ",message) due to exception " + ex.Message);
                log.LogMethodExit(-1);
                return clsTicket;
            }
        }

        internal int InsertPhysicalReceiptToDB(int sourceRedemptionId, string receipt, int tickets, SqlTransaction sqlTrx, DateTime? issueDate = null)
        {
            log.LogMethodEntry(sourceRedemptionId, receipt, tickets, sqlTrx, issueDate);
            int returnValue = -1;

            DateTime dateTime = DateTime.MinValue;
            try
            {
                if (issueDate == null)
                    dateTime = ServerDateTime.Now;
                else
                    dateTime = Convert.ToDateTime(issueDate);

                TicketReceiptDTO ticketReceiptDTO = new TicketReceiptDTO(-1, -1, receipt, tickets, tickets, false, sourceRedemptionId, dateTime);
                TicketReceipt ticketReceipts = new TicketReceipt(executionContext, ticketReceiptDTO);
                ticketReceipts.Save(sqlTrx);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(-1);
                return -1;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        internal ReceiptClass PrintSuspended(Utilities utilities,SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(utilities);
            int receiptTemplateId = -1;
            Printer.ReceiptClass receipt = null;
            PrintDocument myPrintDocument = new PrintDocument();

            try
            {
                receiptTemplateId = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SUSPENDED_REDEMPTION_RECEIPT_TEMPLATE"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                receiptTemplateId = -1;
            }

            if (receiptTemplateId == -1)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Redemption receipt template is not set up."));
            }
            else
            {
                receipt = GenerateSuspendedRedemptionReceipt(receiptTemplateId, utilities, sqlTransaction);
            }
            log.LogMethodExit(receipt);
            return receipt;
        }

        private ReceiptClass GenerateSuspendedRedemptionReceipt(int receiptTemplateId, Utilities utilities,SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(receiptTemplateId, utilities);
            POS.POSPrinterDTO printer;
            PrinterBL printerBL = new PrinterBL(executionContext);
            printer = new POS.POSPrinterDTO(-1, -1, -1, -1, -1, -1, receiptTemplateId, null, null, null, true, ServerDateTime.Now, "", ServerDateTime.Now, "", -1, "", false, -1, -1);
            printer.ReceiptPrintTemplateHeaderDTO = (new ReceiptPrintTemplateHeaderBL(executionContext, receiptTemplateId, true,sqlTransaction)).ReceiptPrintTemplateHeaderDTO;
            printer.PrinterDTO = new PrinterDTO();
            printer.PrinterDTO.PrinterName = "";
            int maxLines = 15;
            log.LogVariableState("maxLines", maxLines);
            Printer.ReceiptClass receipt = new Printer.ReceiptClass(maxLines);
            int rLines = 0;
            string dateFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            string dateTimeFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT");
            string numberFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            int lineBarCodeHeight = 24;
            if (redemptionDTO != null)
            {
                List<ReceiptPrintTemplateDTO> receiptItemListDTOList = printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Where(iSlip => iSlip.Section == "ITEMSLIP").OrderBy(seq => seq.Sequence).ToList();

                if (printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList != null)
                {
                    foreach (ReceiptPrintTemplateDTO receiptTemplateDTO in printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Take(printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Count - (receiptItemListDTOList.Count - 1)))
                    {
                        log.LogVariableState("rLines", rLines);
                        string line = "";
                        int pos;
                        //get Col data and Col alignment into list
                        List<ReceiptColumnData> receiptTemplateColList = new List<ReceiptColumnData>();
                        ReceiptPrintTemplateBL receiptTemplateBL = new ReceiptPrintTemplateBL(executionContext, receiptTemplateDTO);
                        receiptTemplateColList = receiptTemplateBL.GetReceiptDTOColumnData();

                        receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                        receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                        lineBarCodeHeight = 24;
                        if (receiptTemplateDTO.MetaData != null && (receiptTemplateDTO.MetaData.Contains("@lineHeight") || receiptTemplateDTO.MetaData.Contains("@lineBarCodeHeight")))
                        {
                            try
                            {
                                string[] metadata;
                                if (receiptTemplateDTO.MetaData.Contains("|"))
                                    metadata = receiptTemplateDTO.MetaData.Split('|');
                                else
                                {
                                    metadata = new string[] { receiptTemplateDTO.MetaData };
                                }
                                foreach (string s in metadata)
                                {
                                    if (s.Contains("@lineHeight="))
                                    {
                                        int iLineHeight = s.IndexOf("=") + 1;
                                        if (iLineHeight != -1)
                                            receipt.ReceiptLines[rLines].LineHeight = Convert.ToInt32(s.Substring(iLineHeight, s.Length - iLineHeight));
                                        else
                                            receipt.ReceiptLines[rLines].LineHeight = 0;
                                    }

                                    if (s.Contains("@lineBarCodeHeight="))
                                    {
                                        int iLineBarCodeHeight = s.IndexOf("=") + 1;
                                        if (iLineBarCodeHeight != -1)
                                            lineBarCodeHeight = Convert.ToInt32(s.Substring(iLineBarCodeHeight, s.Length - iLineBarCodeHeight));
                                        else
                                            lineBarCodeHeight = 24;
                                    }
                                }
                            }
                            catch
                            {
                                receipt.ReceiptLines[rLines].LineHeight = 0;
                                lineBarCodeHeight = 24;
                            }
                        }
                        switch (receiptTemplateDTO.Section)
                        {
                            case "FOOTER":
                            case "HEADER":
                                {
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        receipt.ReceiptLines[rLines].colCount = 1;
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@SiteName", utilities.ParafaitEnv.SiteName);
                                            if (utilities.ParafaitEnv.CompanyLogo != null && line.Contains("@SiteLogo"))
                                            {
                                                line = line.Replace("@SiteLogo", "");
                                                receipt.ReceiptLines[rLines].BarCode = utilities.ParafaitEnv.CompanyLogo;
                                            }
                                            else
                                                line = line.Replace("@SiteLogo", "");
                                            line = line.Replace("@SiteAddress", utilities.ParafaitEnv.SiteAddress);
                                            try
                                            {
                                                line = line.Replace("@Date", Convert.ToDateTime(redemptionDTO.RedeemedDate).ToString(dateTimeFormat));
                                            }
                                            catch
                                            {
                                                line = line.Replace("@Date", ServerDateTime.Now.ToString(dateTimeFormat));
                                            }
                                            line = line.Replace("@SystemDate", ServerDateTime.Now.ToString(dateTimeFormat));
                                            line = line.Replace("@TrxId", redemptionDTO.RedemptionId.ToString());
                                            line = line.Replace("@TrxNo", redemptionDTO.RedemptionOrderNo);
                                            line = line.Replace("@Cashier", executionContext.GetUserId());
                                            line = line.Replace("@POS", executionContext.POSMachineName);
                                            line = line.Replace("@RedemptionReceiptNo", "RDSPND" + redemptionDTO.RedemptionId);
                                            //sline = line.Replace("@BarCodeCouponNumber", executionContext.POSMachineName);
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }

                                    break;
                                }
                            default: break;
                        }

                        rLines++;
                    }
                    string barcode = "RDSPND" + redemptionDTO.RedemptionId;
                    receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(2, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), barcode.ToString());
                    receipt.ReceiptLines[rLines].BarCode.Tag = barcode;
                }
                rLines++;
                receipt.TotalLines = rLines;
            }
            log.LogVariableState("receipt class", receipt);
            log.LogMethodExit(receipt);
            return receipt;
        }

        public int InsertManualTicketReceipts(int redemptionId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(redemptionId, SQLTrx);
            TicketReceiptDTO ticketReceiptDTO = null;
            TicketReceipt ticketReceipt = null;

            if (redemptionDTO.TicketReceiptListDTO != null && redemptionDTO.TicketReceiptListDTO.Count > 0)
            {
                ticketReceiptDTO = new TicketReceiptDTO(-1, redemptionId, "", 0, 0, false, -1, ServerDateTime.Now);

                foreach (TicketReceiptDTO item in redemptionDTO.TicketReceiptListDTO)
                {
                    TicketReceiptList ticketReceiptList = new TicketReceiptList(executionContext);
                    List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>
                               {
                                   new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_TICKET_RECEIPT_NO, item.ManualTicketReceiptNo),
                                   new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, executionContext.GetSiteId().ToString()),
                               };
                    List<TicketReceiptDTO> ticketReceiptDTOList = ticketReceiptList.GetAllTicketReceipt(searchParams,SQLTrx);
                    if (ticketReceiptDTOList == null)
                    {
                        ticketReceipt = new TicketReceipt(executionContext, ticketReceiptDTO);
                        ticketReceipt.Save(SQLTrx);
                        ticketReceiptDTO = ticketReceipt.TicketReceiptDTO;
                    }
                    else
                    {
                        ticketReceiptDTO = ticketReceiptDTOList.FirstOrDefault();
                        ticketReceipt = new TicketReceipt(executionContext, ticketReceiptDTO);
                        ticketReceipt.TicketReceiptDTO.BalanceTickets = 0;
                        ticketReceipt.Save(SQLTrx);
                        ticketReceiptDTO = ticketReceipt.TicketReceiptDTO;
                    }
                }
                log.LogMethodExit();
                return ticketReceiptDTO.Id;
            }
            log.Debug("Ends-InsertManualTicketReceipts(" + redemptionId + ",sqlTrx) ");
            log.LogMethodExit();
            return -1;
        }
        public clsTicket PrintTotalManualTickets(int sourceRedemptionId, Utilities utilities, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sourceRedemptionId, sqlTransaction);
            clsTicket clsTicket;
            try
            {
                int total = GetCurrencyTickets() + GetManualTickets() + GetPhysicalTickets();
                log.LogVariableState("total", total);
                if (total > 0)
                {
                    clsTicket = PrintRealTicketReceipt(sourceRedemptionId, total, utilities, sqlTransaction);
                    log.LogVariableState("clsTicket", clsTicket);

                    if (GetPhysicalTickets() > 0)
                    {
                        InsertManualTicketReceipts(sourceRedemptionId, sqlTransaction);
                    }

                    if (GetCurrencyTickets() > 0)
                    {
                        if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyId >= 0))
                        {
                            foreach (int currency in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0).Select(y => y.CurrencyId).Distinct())
                            {
                                int currencyqty = Convert.ToInt32(redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId == currency).Sum(y => (y.CurrencyQuantity == null ? 0 : y.CurrencyQuantity)));
                                UpdateRedemptionCurrencyInventory(currency, currencyqty, utilities, sqlTransaction);
                            }
                        }
                    }
                    log.Info("Ends-printTotalManualTickets(message)");
                    log.LogMethodExit(clsTicket);
                    return clsTicket;
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "total ticket =0"));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Fatal("Ends-printTotalManualTickets(message) due to exception " + ex.Message);
                throw ex;
            }
        }
        public void UpdateRedemptionCurrencyInventory(int currencyId,int currencyQuantity, Utilities utilities, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(currencyId, currencyQuantity,sqlTrx);
            RedemptionCurrencyContainerDTO redemptionCurrencyContainerDTO=  RedemptionCurrencyContainerList.GetRedemptionCurrencyContainerDTOList(executionContext.GetSiteId()).FirstOrDefault(x=>x.CurrencyId== currencyId);
            if (redemptionCurrencyContainerDTO != null && redemptionCurrencyContainerDTO.ProductId >= 0)
            {
                using (SqlCommand cmd = utilities.getCommand(sqlTrx))
                {
                    cmd.CommandText = "Update Inventory " +
                                                    "Set Quantity = (Quantity + @qty), Lastupdated_userid = @lmid, timestamp = getdate() " +
                                                    "where exists (select 1 from Product P left outer join posMachines pos on POSMachineId = @POSMachine " +
                                                        "where P.ProductId = @prod_id " +
                                                        "and P.ProductId = Inventory.ProductId " +
                                                        "and isnull(pos.InventoryLocationId, P.DefaultLocationId) = Inventory.LocationId)";

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@prod_id", redemptionCurrencyContainerDTO.ProductId);
                    cmd.Parameters.AddWithValue("@qty", currencyQuantity);
                    cmd.Parameters.AddWithValue("@lmid", executionContext.GetUserId());
                    cmd.Parameters.AddWithValue("@POSMachine", executionContext.GetMachineId());

                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        cmd.CommandText = "insert into Inventory (ProductId, LocationId, Quantity, Timestamp, LastUpdated_UserId) " +
                                            "select ProductId, isnull(pos.InventoryLocationId, P.DefaultLocationId), @qty, getdate(), @lmid " +
                                            "from Product P left outer join posMachines pos on POSMachineId = @POSMachine " +
                                           "where P.ProductId = @prod_id ";
                        cmd.ExecuteNonQuery();
                    }
                }
                object locId = utilities.executeScalar(@"select isnull(pos.InventoryLocationId, P.DefaultLocationId)
                                                                    from Product P 
                                                                        left outer join posMachines pos 
                                                                        on POSMachineId = @POSMachine
                                                                    where P.ProductId = @ProductId",
                                                            new SqlParameter("@ProductId", redemptionCurrencyContainerDTO.ProductId),
                                                            new SqlParameter("@POSMachine", executionContext.GetMachineId()));
                Transaction.Inventory.AdjustInventory(Transaction.Inventory.AdjustmentTypes.TradeIn,
                                                    utilities,
                                                    Convert.ToInt32(locId),
                                                    Convert.ToInt32(redemptionCurrencyContainerDTO.ProductId),
                                                    Convert.ToInt32(currencyQuantity),
                                                    executionContext.GetUserId(),
                                                    "Redemption Trade-In", null, sqlTrx);                
            }
            log.LogMethodExit();
        }

        internal RedemptionDTO AddTurnIns(RedemptionActivityDTO redemptionActivityDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(redemptionActivityDTO, sqlTransaction);
            int totalTickets = 0;
            List<RedemptionTicketAllocationDTO> turninAllocationDTOList = new List<RedemptionTicketAllocationDTO>();
            try
            {
                if (GetUtility().ParafaitEnv.ALLOW_REDEMPTION_WITHOUT_CARD == "N"
                   && redemptionDTO.RedemptionCardsListDTO.Count < 1)
                {
                    string message = MessageContainerList.GetMessage(executionContext, 257);
                    log.Debug("No card was tapped");
                    log.LogMethodExit(message);
                    throw new ValidationException(message);
                }
                if (GetUtility().ParafaitEnv.ALLOW_REDEMPTION_WITHOUT_CARD == "N"
                   && redemptionDTO.RedemptionCardsListDTO.Count > 1)
                {
                    string message = MessageContainerList.GetMessage(executionContext, "More than one card was tapped");
                    log.Debug("More than one card was tapped");
                    log.LogMethodExit(message);
                    throw new ValidationException(message);
                }
                if (redemptionDTO.RedemptionGiftsListDTO==null || !redemptionDTO.RedemptionGiftsListDTO.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, 119);
                    log.Debug("Please select gift(s) before saving");
                    log.LogMethodExit(message);
                    throw new ValidationException(message);
                }
                if (redemptionActivityDTO.TargetLocationId < 0)
                {
                    string message = MessageContainerList.GetMessage(executionContext, 806);
                    log.Debug("Not selected the target location");
                    log.LogMethodExit(message);
                    throw new ValidationException(message);
                }
                if (redemptionDTO != null && redemptionDTO.RedemptionGiftsListDTO != null && redemptionDTO.RedemptionGiftsListDTO.Any())
                {
                    totalTickets = redemptionDTO.RedemptionGiftsListDTO.Sum(x => Convert.ToInt32(x.Tickets) * x.ProductQuantity);
                }
                try
                {
                    RedemptionReversalTurnInLimitCheck(totalTickets, redemptionActivityDTO.ManagerToken);
                }
                catch (Exception ex)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, ex.Message);
                    throw new ValidationException(errorMessage);
                }
                foreach (RedemptionGiftsDTO item in redemptionDTO.RedemptionGiftsListDTO)
                {
                    string message = "";
                    using (Utilities utilities = GetUtility())
                    {
                        if (ReceiveTurnInGift(item.ProductId, redemptionActivityDTO.TargetLocationId, item.ProductQuantity, utilities, sqlTransaction) == false)
                        {
                            message = MessageContainerList.GetMessage(executionContext, 2929);
                            log.Error("ReceiveTurnInGift is false error :" + message);
                            throw new ValidationException(message);
                        }
                    }
                }

                if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any())
                {
                    RedemptionCardsDTO redemptionCardsDTO = redemptionDTO.RedemptionCardsListDTO.FirstOrDefault();
                    try
                    {
                        AccountListBL accountListBL = new AccountListBL(executionContext);
                        List<KeyValuePair<AccountDTO.SearchByParameters, string>> accountSearch = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                        accountSearch.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.ACCOUNT_ID, redemptionCardsDTO.CardId.ToString()));
                        List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(accountSearch, sqlTransaction);
                        if (accountDTOList != null && accountDTOList.Any())
                        {
                            foreach (AccountDTO accountDTO in accountDTOList)
                            {
                                redemptionDTO.CustomerId = accountDTO.CustomerId;
                                redemptionDTO.Source = redemptionActivityDTO.Source;
                                redemptionDTO.ETickets = totalTickets;
                                RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
                                redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                                redemptionDTO.AcceptChanges();

                                // Update Cards
                                redemptionCardsDTO.TicketCount = totalTickets;
                                RedemptionCardsBL redemptionCardsBL = new RedemptionCardsBL(executionContext, redemptionCardsDTO);
                                redemptionCardsBL.Save(sqlTransaction);
                                redemptionCardsDTO.AcceptChanges();
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 123, Ex.Message);
                        log.Error(" Error creating redemption cards information. Error: " + Ex.Message);
                        throw new ValidationException(errorMessage);
                    }
                    using (Utilities utilities = GetUtility())
                    {
                        Loyalty Loyalty = new Loyalty(utilities);
                        Loyalty.CreateGenericCreditPlusLine(redemptionCardsDTO.CardId, "T", totalTickets * -1, false, 0, "N", executionContext.GetUserId(), "Redemption turn in Tickets", sqlTransaction, DateTime.MinValue, GetUtility().getServerTime());
                    }
                    if (redemptionDTO.RedemptionGiftsListDTO != null && redemptionDTO.RedemptionGiftsListDTO.Any())
                    {
                        foreach (RedemptionGiftsDTO item in redemptionDTO.RedemptionGiftsListDTO)
                        {
                            //Insert into redemption gifts table 
                            int i = item.ProductQuantity;
                            while (i-- > 0)
                            {

                                // Create TicketAllocation
                                turninAllocationDTOList.Add ( new RedemptionTicketAllocationDTO(-1, item.RedemptionId, item.RedemptionGiftsId, null, null, -1, null, -1, null, null, null, null,
                                                        item.Tickets, executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(), -1, -1, -1, -1, null, -1));
                                // Allocation without RedemptionGiftId
                                turninAllocationDTOList.Add( new RedemptionTicketAllocationDTO(-1, item.RedemptionId, -1, null, null, -1, null, -1, null, null, null, null,
                                                        item.Tickets * -1, executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(), -1, -1, -1, -1, null, -1));                                
                            }
                        }
                    }
                }
                else
                {

                    redemptionDTO.Source = redemptionActivityDTO.Source;
                    redemptionDTO.ManualTickets = totalTickets;
                    RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
                    redemptionDataHandler.UpdateRedemption(redemptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    redemptionDTO.AcceptChanges();
                    if (redemptionDTO.RedemptionGiftsListDTO!=null && redemptionDTO.RedemptionGiftsListDTO.Any())
                    {
                        foreach (RedemptionGiftsDTO item in redemptionDTO.RedemptionGiftsListDTO)
                        {
                            //Insert into redemption gifts table  
                            int i = item.ProductQuantity;
                            while (i-- > 0)
                            {
                                // Create TicketAllocation
                                turninAllocationDTOList.Add( new RedemptionTicketAllocationDTO(-1, item.RedemptionId, item.RedemptionGiftsId, null, null, -1, null, -1, null, null, null, null,
                                                        item.Tickets, executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(), -1, -1, -1, -1, null, -1));

                                // Allocation without RedemptionGiftId
                                turninAllocationDTOList.Add( new RedemptionTicketAllocationDTO(-1, item.RedemptionId, -1, null, null, -1, null, -1, null, null, null, null,
                                                        item.Tickets * -1, executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(), ServerDateTime.Now, ServerDateTime.Now, executionContext.GetUserId(), -1, -1, -1, -1, null, -1));

                            }
                        }
                    }
                    if (redemptionActivityDTO.PrintBalanceTicket)
                    {
                        string message = "";
                        using (Utilities utilities = GetUtility())
                        {
                            if (PrintRealTicketReceipt(redemptionDTO.RedemptionId, totalTickets * -1, utilities, sqlTransaction) == null)
                            {
                                log.Error("Unable to printRealTicketReceipt error " + message);
                                throw new ValidationException("Unable to printRealTicketReceipt error " + message);
                            }
                        }
                    }
                }
                if (turninAllocationDTOList != null && turninAllocationDTOList.Any())
                {
                    RedemptionTicketAllocationListBL redemptionTicketAllocationListBL = new RedemptionTicketAllocationListBL(executionContext, turninAllocationDTOList);
                    redemptionTicketAllocationListBL.Save(sqlTransaction);
                    LoadRedemptionTicketAllocationDTOList(sqlTransaction);
                }
                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            }
            catch (Exception Ex)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 126, Ex.Message);
                log.Error("Error while updating database. Error due to exception " + Ex.Message);
                throw new ValidationException(errorMessage);
            }
        }

        internal bool ReceiveTurnInGift(int productId,  int turnInToLocationId, int quantity, Utilities utilities, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(productId, turnInToLocationId, quantity, sqlTransaction);
            string message = "";
            string remarks = "Customer gift turn-in"; //Receive remarks field.
            PurchaseOrder purchaseOrder = new PurchaseOrder(executionContext);
            string orderNumber = purchaseOrder.GetNextPurchaseOrderSequenceNo("Purchaseorder", sqlTransaction) + " Auto";
            try
            {
                ProductBL productBL = new ProductBL(executionContext, productId, true, true, sqlTransaction);
                decimal ticket_cost = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "TICKET_COST", 0);
                object amount = utilities.executeScalar(@"select TurnInPriceInTickets * @ticket_cost
                                                           from product
                                                          where ProductId = @ProductId", sqlTransaction, new SqlParameter("@ProductId", productId), new SqlParameter("@ticket_cost", ticket_cost));
                if (amount == null || amount == DBNull.Value)
                    amount = 0;

                decimal total = quantity * Convert.ToDecimal(amount);
                log.Debug("Total  :" + total);
                // New order to be inserted.
                int vendorId = productBL.getProductDTO.DefaultVendorId;
                bool lotControlled = productBL.getProductDTO.LotControlled;
                if (productBL.getProductDTO.DefaultVendorId < 0)
                {
                    VendorList vendorList = new VendorList(executionContext);
                    List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> SearchVendorListParameter = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
                    List<VendorDTO> vendorListOnDisplay = vendorList.GetAllVendors(SearchVendorListParameter, 0, 1,sqlTransaction);
                    if (vendorListOnDisplay != null && vendorListOnDisplay.Count > 0)
                    {
                        vendorId = vendorListOnDisplay[0].VendorId;
                    }
                }

                PurchaseOrderDTO purchaseOrderDTO = new PurchaseOrderDTO(-1, PurchaseOrderDTO.PurchaseOrderStatus.RECEIVED, orderNumber, ServerDateTime.Now, vendorId, null, null, null, null, null,
                                                                        null, null, null, null, null, null, null, null, null, null, ServerDateTime.Now, (double)total, utilities.ParafaitEnv.LoginID, ServerDateTime.Now, ServerDateTime.Now,
                                                                       "TURNINREDEMPTION", executionContext.GetSiteId(), "", false, ServerDateTime.Now, -1, "", -1, ServerDateTime.Now, ServerDateTime.Now, remarks, string.Empty, -1, -1, "",
                                                                        -1, -1, "", null, null, executionContext.GetUserId(), ServerDateTime.Now, productBL.getProductDTO.IsActive, false);
                purchaseOrder = new PurchaseOrder(purchaseOrderDTO, executionContext);
                purchaseOrder.Save(sqlTransaction);

                // Insert PurchaseOrderLine
                PurchaseOrderLineDTO purchaseOrderLineDTO = new PurchaseOrderLineDTO(-1, purchaseOrder.getPurchaseOrderDTO.PurchaseOrderId, productBL.getProductDTO.Code, productBL.getProductDTO.Description, quantity,
                                                                Convert.ToDouble(amount), Convert.ToDouble(total), ServerDateTime.Now, 0, 0, ServerDateTime.Now, executionContext.GetSiteId(), productId, "", false, "Y", ServerDateTime.Now, -1, -1, -1, -1,
                                                                0,//productBL.getProductDTO.TurnInPriceInTickets, 
                                                                "", string.Empty, -1, executionContext.GetUserId(), ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now, productBL.getProductDTO.InventoryUOMId == -1 ? productBL.getProductDTO.UomId : productBL.getProductDTO.InventoryUOMId);
                PurchaseOrderLine purchaseOrderLine = new PurchaseOrderLine(purchaseOrderLineDTO, executionContext);
                purchaseOrderLine.Save(sqlTransaction);

                // Insert Receipts
                InventoryReceiptDTO inventoryReceiptDTO = new InventoryReceiptDTO(-1, "TURNINREDEMPTION-" + redemptionDTO.RedemptionOrderNo, "", "", purchaseOrder.getPurchaseOrderDTO.PurchaseOrderId, remarks, ServerDateTime.Now,
                                                                    utilities.ParafaitEnv.LoginID, "", -1, -1, "", orderNumber, Convert.ToDouble(amount), ServerDateTime.Now, 0, null, purchaseOrder.getPurchaseOrderDTO.IsActive);
                InventoryReceiptsBL inventoryReceiptsBL = new InventoryReceiptsBL(inventoryReceiptDTO, executionContext);
                inventoryReceiptsBL.Save(sqlTransaction);

                // Insert PurchaseOrderReceive_Line
                InventoryReceiveLinesDTO inventoryReceiveLinesDTO = new InventoryReceiveLinesDTO(-1, purchaseOrder.getPurchaseOrderDTO.PurchaseOrderId, productId, productBL.getProductDTO.Description, null, quantity,
                                                                        turnInToLocationId, "Y", purchaseOrderLine.PurchaseOrderLineDTO.PurchaseOrderLineId, purchaseOrderLine.PurchaseOrderLineDTO.UnitPrice, -1,
                                                                        purchaseOrderLine.PurchaseOrderLineDTO.SubTotal, "", inventoryReceiptsBL.InventoryReceiptDTO.ReceiptId, "TURNINREDEMPTION-" + redemptionDTO.RedemptionOrderNo, ServerDateTime.Now, "", executionContext.GetUserId(),
                                                                        -1, -1, "", -1, -1, -1, -1, -1, null, 0, productBL.getProductDTO.PurchaseTaxId, 0, productBL.getProductDTO.InventoryUOMId == -1 ? productBL.getProductDTO.UomId : productBL.getProductDTO.InventoryUOMId, true);
                if (lotControlled)
                {
                    DateTime expiryDate = DateTime.MaxValue;
                    if (productBL.getProductDTO.ExpiryDays > 0)
                    {
                        expiryDate = ServerDateTime.Now.AddDays(productBL.getProductDTO.ExpiryDays);
                    }
                    InventoryLotDTO inventoryLotDTO = new InventoryLotDTO(-1, null, quantity, quantity, inventoryReceiveLinesDTO.Price, inventoryReceiveLinesDTO.PurchaseOrderReceiveLineId, expiryDate, true, null, inventoryReceiveLinesDTO.UOMId);
                    inventoryReceiveLinesDTO.InventoryLotListDTO = new List<InventoryLotDTO> { inventoryLotDTO };
                }

                InventoryReceiveLinesBL inventoryReceiveLinesBL = new InventoryReceiveLinesBL(inventoryReceiveLinesDTO, executionContext);
                inventoryReceiveLinesBL.Save(sqlTransaction);
            }
            catch (Exception Ex)
            {
                message = Ex.Message;
                log.Error("Exception :", Ex);
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }        
    }

    /// <summary>
    /// Manages the List of Records.
    /// </summary>
    public class RedemptionUseCaseListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<RedemptionDTO> redemptionDTOList = new List<RedemptionDTO>();
        private string approvalId = "";
        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public RedemptionUseCaseListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Paramaterized Construtor with RedemptionDTOList as parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="redemptionDTOList">redemptionDTOList</param>
        public RedemptionUseCaseListBL(ExecutionContext executionContext, List<RedemptionDTO> redemptionDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, redemptionDTOList);
            this.redemptionDTOList = redemptionDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the redemptionOrders list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>redemptionDTOList</returns>
        public List<RedemptionDTO> GetRedemptionDTOList(List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            SetFromSiteTimeOffset(searchParameters);
            RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
            redemptionDTOList = redemptionDataHandler.GetRedemptionDTOList(searchParameters);
            SetToSiteTimeOffset(redemptionDTOList);
            log.LogMethodExit(redemptionDTOList);
            return redemptionDTOList;
        }
        internal List<RedemptionDTO> SetToSiteTimeOffset(List<RedemptionDTO> redemptionDTOList)
        {
            log.LogMethodEntry(redemptionDTOList);
            List<RedemptionDTO> localredemptionDTOList = new List<RedemptionDTO>();
            if (redemptionDTOList != null)
            {
                localredemptionDTOList.AddRange(redemptionDTOList);
            }
            if (executionContext != null && executionContext.IsCorporate)
            {
                if (localredemptionDTOList != null && localredemptionDTOList.Any())
                {
                    for (int i = 0; i < localredemptionDTOList.Count; i++)
                    {
                        redemptionDTOList[i].CreationDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, redemptionDTOList[i].CreationDate);
                        redemptionDTOList[i].LastUpdateDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, redemptionDTOList[i].LastUpdateDate);
                        if (redemptionDTOList[i].RedeemedDate != null)
                        {
                            redemptionDTOList[i].RedeemedDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, (DateTime)redemptionDTOList[i].RedeemedDate);
                        }
                        if (redemptionDTOList[i].OrderCompletedDate != null && redemptionDTOList[i].OrderCompletedDate != DateTime.MinValue)
                        {
                            redemptionDTOList[i].OrderCompletedDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, (DateTime)redemptionDTOList[i].OrderCompletedDate);
                        }
                        if (redemptionDTOList[i].OrderDeliveredDate != null && redemptionDTOList[i].OrderDeliveredDate != DateTime.MinValue)
                        {
                            redemptionDTOList[i].OrderDeliveredDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, (DateTime)redemptionDTOList[i].OrderDeliveredDate);
                        }
                        if (localredemptionDTOList[i].RedemptionCardsListDTO != null && localredemptionDTOList[i].RedemptionCardsListDTO.Any())
                        {
                            for (int j = 0; j < localredemptionDTOList[i].RedemptionCardsListDTO.Count; j++)
                            {
                                redemptionDTOList[i].RedemptionCardsListDTO[j].CreationDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, redemptionDTOList[i].RedemptionCardsListDTO[j].CreationDate);
                                redemptionDTOList[i].RedemptionCardsListDTO[j].LastUpdateDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, redemptionDTOList[i].RedemptionCardsListDTO[j].LastUpdateDate);
                                redemptionDTOList[i].RedemptionCardsListDTO[j].AcceptChanges();
                            }
                        }
                        if (localredemptionDTOList[i].RedemptionGiftsListDTO != null && localredemptionDTOList[i].RedemptionGiftsListDTO.Any())
                        {
                            redemptionDTOList[i].RedemptionGiftsListDTO.All(c => { c.CreationDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, (DateTime)c.CreationDate); return true; });
                            redemptionDTOList[i].RedemptionGiftsListDTO.All(c => { c.LastUpdateDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, (DateTime)c.LastUpdateDate); return true; });
                            redemptionDTOList[i].AcceptChanges();
                        }
                        if (localredemptionDTOList[i].TicketReceiptListDTO != null && localredemptionDTOList[i].TicketReceiptListDTO.Any())
                        {
                            for (int j = 0; j < localredemptionDTOList[i].TicketReceiptListDTO.Count; j++)
                            {
                                if (redemptionDTOList[i].TicketReceiptListDTO[j].CreationDate != null)
                                {
                                    redemptionDTOList[i].TicketReceiptListDTO[j].CreationDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, (DateTime)redemptionDTOList[i].TicketReceiptListDTO[j].CreationDate);
                                }
                                redemptionDTOList[i].TicketReceiptListDTO[j].LastUpdatedDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, redemptionDTOList[i].TicketReceiptListDTO[j].LastUpdatedDate);
                                redemptionDTOList[i].TicketReceiptListDTO[j].IssueDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, redemptionDTOList[i].TicketReceiptListDTO[j].IssueDate);
                                redemptionDTOList[i].TicketReceiptListDTO[j].AcceptChanges();
                            }
                        }
                        if (localredemptionDTOList[i].RedemptionTicketAllocationListDTO != null && localredemptionDTOList[i].RedemptionTicketAllocationListDTO.Any())
                        {
                            for (int j = 0; j < localredemptionDTOList[i].RedemptionTicketAllocationListDTO.Count; j++)
                            {
                                if (redemptionDTOList[i].RedemptionTicketAllocationListDTO[j].CreationDate != null)
                                {
                                    redemptionDTOList[i].RedemptionTicketAllocationListDTO[j].CreationDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, (DateTime)redemptionDTOList[i].RedemptionTicketAllocationListDTO[j].CreationDate);
                                }
                                redemptionDTOList[i].RedemptionTicketAllocationListDTO[j].LastUpdatedDate = SiteContainerList.FromSiteDateTime(redemptionDTOList[i].SiteId, redemptionDTOList[i].RedemptionTicketAllocationListDTO[j].LastUpdatedDate);
                                redemptionDTOList[i].RedemptionTicketAllocationListDTO[j].AcceptChanges();
                            }
                        }
                        redemptionDTOList[i].AcceptChanges();
                    }
                }
            }
            log.LogMethodExit(redemptionDTOList);
            return redemptionDTOList;
        }
        internal List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> SetFromSiteTimeOffset(List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchparams = new List<KeyValuePair<RedemptionDTO.SearchByParameters, string>>();
            if (searchParameters != null)
            {
                searchparams.AddRange(searchParameters);
            }
            if (executionContext != null && executionContext.IsCorporate)
            {
                if (searchparams != null && searchparams.Any())
                {
                    foreach (KeyValuePair<RedemptionDTO.SearchByParameters, string> searchParameter in searchparams)
                    {
                        if ((searchParameter.Key == RedemptionDTO.SearchByParameters.FROM_REDEMPTION_DATE) ||
                            (searchParameter.Key == RedemptionDTO.SearchByParameters.TO_REDEMPTION_DATE) ||
                            (searchParameter.Key == RedemptionDTO.SearchByParameters.FROM_REDEMPTION_ORDER_COMPLETED_DATE) ||
                            (searchParameter.Key == RedemptionDTO.SearchByParameters.TO_REDEMPTION_ORDER_COMPLETED_DATE) ||
                            (searchParameter.Key == RedemptionDTO.SearchByParameters.FROM_REDEMPTION_ORDER_DELIVERED_DATE) ||
                            (searchParameter.Key == RedemptionDTO.SearchByParameters.TO_REDEMPTION_ORDER_DELIVERED_DATE))
                        {
                            if (!string.IsNullOrWhiteSpace(searchParameter.Value))
                            {
                                int index = searchParameters.IndexOf(searchParameter);
                                searchParameters[index] = new KeyValuePair<RedemptionDTO.SearchByParameters, string>(searchParameter.Key, SiteContainerList.ToSiteDateTime(executionContext.GetSiteId(), DateTime.Parse(searchParameter.Value)).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));
                            }
                        }
                    }
                }
            }
            log.LogMethodEntry(searchParameters);
            return searchParameters;
        }

        /// <summary>
        /// Save the RedmeptionDTO - for UseCase
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (redemptionDTOList == null ||
               redemptionDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < redemptionDTOList.Count; i++)
            {
                var redemptionDTO = redemptionDTOList[i];
                if (redemptionDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    RedemptionBL redemptionUseCaseBL = new RedemptionBL(executionContext, redemptionDTO);
                    redemptionUseCaseBL.Save(sqlTransaction);
                }
                catch (SqlException ex)
                {
                    log.Error(ex);
                    if (ex.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else if (ex.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving RedemptionDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("RedemptionDTOList", redemptionDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        
    }
}

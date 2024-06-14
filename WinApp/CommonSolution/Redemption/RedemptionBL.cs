/********************************************************************************************
* Project Name - Redemption BL
* Description  - Business logic
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*1.00        12-May-2017      Lakshminarayana     Created 
*2.3.0       25-Jun-2018      Archana/Guru S A    Redemption kiosk changes
*2.4.0       14-Sep-2018      Archana/Guru S A    Redemption reversal/RDS changes
*2.7.0       08-Jul-2019      Archana             Redemption Receipt changes to show ticket allocation details
*2.70.0      20-Jul-2019      Mathew Ninan        Added update for Card when credit plus is updated for RedeemTickets
*2.70.2       19-Jul-2019     Deeksha             Modifications as per three tier standard.
*2.70.2       19-Aug-2019     Dakshakh            Redemption currency rule enhancement changes 
*2.70.3       20-Feb-2020     Archana             Manulal Ticket Limit check changes
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.POS;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer.Accounts;
using System.Linq;
using System.Configuration;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Business logic for Redemption class.
    /// </summary>
    public class RedemptionBL
    {
        private RedemptionDTO redemptionDTO;
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int balanceTicketsRemaining;
        private ExecutionContext machineUserContext;
        private ParafaitDBTransaction parafaitDBTrx;

        /// <summary>
        /// Default constructor of RedemptionBL class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public RedemptionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            redemptionDTO = new RedemptionDTO();
            balanceTicketsRemaining = 0;
            machineUserContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the redemption id and executionContext as the parameter
        /// Would fetch the redemption object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="ExecutionContext">ExecutionContext</param>
        /// <param name="SqlTransaction">SqlTransaction</param>
        public RedemptionBL(int id, ExecutionContext executionContext, SqlTransaction sqlTransaction = null, bool loadChildren = false)
            : this(executionContext)
        {
            log.LogMethodEntry(id, executionContext, sqlTransaction, loadChildren);
            RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
            redemptionDTO = redemptionDataHandler.GetRedemptionDTO(id);
            balanceTicketsRemaining = 0;
            machineUserContext = executionContext;
            if (loadChildren && redemptionDTO != null)
            {
                LoadChildDTOs(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates RedemptionBL object using the RedemptionDTO
        /// </summary>
        /// <param name="redemptionDTO">RedemptionDTO object</param>
        /// <param name="executionContext">executionContext</param>
        public RedemptionBL(RedemptionDTO redemptionDTO, ExecutionContext executionContext)
            : this(executionContext)
        {
            log.LogMethodEntry(redemptionDTO, executionContext);
            this.redemptionDTO = redemptionDTO;
            balanceTicketsRemaining = 0;
            machineUserContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Redemption
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// 
        /// Further, creates the redemption allocations based on what was used to redeem
        /// There are three entities that could be used for redemption
        /// a. Card - Tickets on card, b. Receipts, c. Grace tickets
        /// Based on tickets required for redemption of gift, one or more of the entities are 
        /// used and accordingly allocation is created
        /// </summary>
        /// <param name="sqlTrx">SqlTransaction</param>
        public void Save(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTrx);

            if (redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.PREPARED.ToString() && redemptionDTO.OrderCompletedDate == DateTime.MinValue)
                redemptionDTO.OrderCompletedDate = DateTime.Now;

            if (redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString() && redemptionDTO.OrderDeliveredDate == DateTime.MinValue)
                redemptionDTO.OrderDeliveredDate = DateTime.Now;

            if (redemptionDTO.RedemptionId < 0)
            {

                redemptionDTO = redemptionDataHandler.InsertRedemption(redemptionDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                redemptionDTO.AcceptChanges();
            }
            else
            {
                if (redemptionDTO.IsChanged)
                {
                    redemptionDTO = redemptionDataHandler.UpdateRedemption(redemptionDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    redemptionDTO.AcceptChanges();
                }
            }

            foreach (TicketReceiptDTO ticketReceiptDTO in redemptionDTO.TicketReceiptListDTO)
            {
                if (ticketReceiptDTO.Id == -1)
                {
                    TicketReceipt ticketReceipt = new TicketReceipt(machineUserContext, ticketReceiptDTO);
                    ticketReceipt.Save(sqlTrx);
                }
            }

            if (redemptionDTO.RedemptionCardsListDTO.Count > 0)
            {
                RedemptionCardsBL redemptionCardsBL;
                foreach (RedemptionCardsDTO redemptionCardDTO in redemptionDTO.RedemptionCardsListDTO)
                {
                    if (redemptionCardDTO.RedemptionId == -1)
                    {
                        redemptionCardDTO.RedemptionId = redemptionDTO.RedemptionId;
                    }
                    AccountBL cardAccount = new AccountBL(machineUserContext, redemptionCardDTO.CardId, true, true, sqlTrx);
                    if (cardAccount != null && cardAccount.GetAccountId() != -1)
                    {
                        redemptionCardDTO.TotalCardTickets = cardAccount.GetTotalTickets();
                    }
                    redemptionCardsBL = new RedemptionCardsBL(machineUserContext, redemptionCardDTO);
                    redemptionCardsBL.Save(sqlTrx);
                }
            }
            foreach (TicketReceiptDTO ticketReceiptDTO in redemptionDTO.TicketReceiptListDTO)
            {
                ticketReceiptDTO.BalanceTickets = ticketReceiptDTO.Tickets;
            }

            if (this.redemptionDTO.OrigRedemptionId == -1)
            {
                SaveGiftLinesNCreateTicketAllocations(sqlTrx);
            }
            else
            {
                SaveReversalLinesNCreateTicketAllocations(sqlTrx);
            }

            if (redemptionDTO.RedemptionTicketAllocationListDTO.Count > 0)
            {
                RedemptionTicketAllocationBL redemptionTicketAllocationBL;
                foreach (RedemptionTicketAllocationDTO redemptionTicketAllocation in redemptionDTO.RedemptionTicketAllocationListDTO)
                {
                    redemptionTicketAllocationBL = new RedemptionTicketAllocationBL(machineUserContext, redemptionTicketAllocation);
                    redemptionTicketAllocationBL.Save(sqlTrx);
                }
            }
            foreach (TicketReceiptDTO ticketReceiptDTO in redemptionDTO.TicketReceiptListDTO)
            {
                ticketReceiptDTO.BalanceTickets = 0;
                ticketReceiptDTO.RedemptionId = redemptionDTO.RedemptionId;
                TicketReceipt ticketReceipts = new TicketReceipt(machineUserContext, ticketReceiptDTO);
                ticketReceipts.Save(sqlTrx);
            }
            log.LogMethodExit();
        }

        private void SaveGiftLinesNCreateTicketAllocations(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            int manualTicketsRemaining = GetManualTickets();

            bool receiptBalanceExists = true, cardBalanceExists = true;
            int graceTicketRemaining = Convert.ToInt32(redemptionDTO.GraceTickets > 0 ? redemptionDTO.GraceTickets : 0);

            if (redemptionDTO.RedemptionGiftsListDTO != null
                && redemptionDTO.RedemptionGiftsListDTO.Any())
            {
                RedemptionGiftsBL redemptionGiftsBL;
                foreach (RedemptionGiftsDTO gift in redemptionDTO.RedemptionGiftsListDTO)
                {
                    if (gift.RedemptionId == -1)
                    {
                        gift.RedemptionId = redemptionDTO.RedemptionId;
                    }
                    redemptionGiftsBL = new RedemptionGiftsBL(machineUserContext, gift);
                    redemptionGiftsBL.Save(sqlTrx);
                    using (Utilities utilities = new Utilities())
                    {
                        SqlCommand sqlCommand = utilities.getCommand(sqlTrx);
                        Semnox.Parafait.Transaction.Inventory.updateStock(gift.GiftCode, sqlCommand, gift.ProductQuantity, machineUserContext.GetMachineId(), machineUserContext.GetUserId(), redemptionDTO.RedemptionId, redemptionGiftsBL.RedemptionGiftsDTO.RedemptionGiftsId, 0, 0, "", machineUserContext.GetSiteId(), -1, -1, "Redemption");
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
                            RedemptionTicketAllocationDTO redemptionTicketAllocationDTO = new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, gift.RedemptionGiftsId, manualTicketsUsed, null, -1, null, -1, null, null, null,
                                                                                                           null, null, machineUserContext.GetSiteId(), -1, false, "", machineUserContext.GetUserId(), DateTime.Now, DateTime.Now, machineUserContext.GetUserId(),
                                                                                                           -1, -1, -1, -1, null, -1);
                            redemptionDTO.RedemptionTicketAllocationListDTO.Add(redemptionTicketAllocationDTO);
                        }
                        else if (receiptBalanceExists == true)
                        {
                            receiptBalanceExists = false;
                            int ticketsUsed = 0;
                            foreach (TicketReceiptDTO ticketReceiptDTO in redemptionDTO.TicketReceiptListDTO)
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
                                        RedemptionTicketAllocationDTO redemptionTicketAllocationDTO = new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, gift.RedemptionGiftsId, null, null, -1, null, -1, null, null, ticketReceiptDTO.ManualTicketReceiptNo,
                                                                                                           ticketsUsed, null, machineUserContext.GetSiteId(), -1, false, "", machineUserContext.GetUserId(), DateTime.Now, DateTime.Now, machineUserContext.GetUserId(),
                                                                                                           ticketReceiptDTO.Id, -1, -1, -1, null, -1);
                                        redemptionDTO.RedemptionTicketAllocationListDTO.Add(redemptionTicketAllocationDTO);
                                    }
                                }
                                else
                                    break;
                            }
                        }
                        else if (cardBalanceExists == true)
                        {
                            cardBalanceExists = false;
                            int ticketsUsed = 0;
                            foreach (RedemptionCardsDTO cardDTO in redemptionDTO.RedemptionCardsListDTO)
                            {
                                if (totalGiftTicketsToAllocate > 0)
                                {
                                    if (cardDTO.TicketCount > 0)
                                    {
                                        if (cardDTO.TicketCount > totalGiftTicketsToAllocate)
                                            ticketsUsed = totalGiftTicketsToAllocate;
                                        else
                                            ticketsUsed = Convert.ToInt32(cardDTO.TicketCount);
                                        totalGiftTicketsToAllocate = totalGiftTicketsToAllocate - ticketsUsed;
                                        cardDTO.TicketCount = cardDTO.TicketCount - ticketsUsed;
                                        cardBalanceExists = true;
                                        RedemptionTicketAllocationDTO redemptionTicketAllocationDTO = new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, gift.RedemptionGiftsId, null, null, cardDTO.CardId, ticketsUsed, -1, null, null, null, null, null,
                                                                                                          machineUserContext.GetSiteId(), -1, false, "", machineUserContext.GetUserId(), DateTime.Now, DateTime.Now, machineUserContext.GetUserId(), -1, -1, -1, -1, null, -1);
                                        redemptionDTO.RedemptionTicketAllocationListDTO.Add(redemptionTicketAllocationDTO);
                                    }
                                }
                                else
                                    break;
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

                            RedemptionTicketAllocationDTO redemptionTicketAllocationDTO = new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, gift.RedemptionGiftsId, 0, graceTicketsUsed, -1, null, -1, null, null, null, null, null,
                                                                                               machineUserContext.GetSiteId(), -1, false, "", machineUserContext.GetUserId(), DateTime.Now, DateTime.Now, machineUserContext.GetUserId(),
                                                                                               -1, -1, -1, -1, null, -1);
                            redemptionDTO.RedemptionTicketAllocationListDTO.Add(redemptionTicketAllocationDTO);
                        }
                    }
                }
                if (manualTicketsRemaining > 0)
                {   //Note for future implementation
                    //UI needs to make sure that whether the balance tickes left should be added to card or ticket receipt. 
                    //manualticket value should be set accordingly
                    RedemptionTicketAllocationDTO redemptionTicketAllocationDTO = new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, -1, manualTicketsRemaining, null, -1, null, -1, null, null, null,
                                                                                                           null, null, machineUserContext.GetSiteId(), -1, false, "", machineUserContext.GetUserId(), DateTime.Now, DateTime.Now, machineUserContext.GetUserId(),
                                                                                                           -1, -1, -1, -1, null, -1);
                    redemptionDTO.RedemptionTicketAllocationListDTO.Add(redemptionTicketAllocationDTO);
                }
            }

            foreach (TicketReceiptDTO ticketReceiptDTO in redemptionDTO.TicketReceiptListDTO)
            {
                if (ticketReceiptDTO.BalanceTickets > 0)
                {
                    RedemptionTicketAllocationDTO redemptionTicketAllocationDTO = new RedemptionTicketAllocationDTO(-1, redemptionDTO.RedemptionId, -1, null, null, -1, null, -1, null, null, ticketReceiptDTO.ManualTicketReceiptNo, ticketReceiptDTO.BalanceTickets, null,
                                                                                      machineUserContext.GetSiteId(), -1, false, "", machineUserContext.GetUserId(), DateTime.Now, DateTime.Now, machineUserContext.GetUserId(), ticketReceiptDTO.Id, -1, -1, -1, null, -1);
                    redemptionDTO.RedemptionTicketAllocationListDTO.Add(redemptionTicketAllocationDTO);
                }
            }
            log.LogMethodExit();

        }

        /// <summary>
        /// SaveReversalLinesNCreateTicketAllocations
        /// </summary>
        /// <param name="sqlTrx">sqlTrx</param>
        private void SaveReversalLinesNCreateTicketAllocations(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);

            RedemptionTicketAllocationListBL redemptionTicketAllocationListBL = new RedemptionTicketAllocationListBL(machineUserContext);
            List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>> redemptionTASearchParam = new List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>>();
            redemptionTASearchParam.Add(new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_ID, this.redemptionDTO.OrigRedemptionId.ToString()));
            redemptionTASearchParam.Add(new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<RedemptionTicketAllocationDTO> originalRTADTOList = redemptionTicketAllocationListBL.GetRedemptionTicketAllocationDTOList(redemptionTASearchParam);
            RedemptionGiftsBL redemptionGiftsBL;
            if (redemptionDTO.RedemptionGiftsListDTO != null)
            {
                foreach (RedemptionGiftsDTO giftLineForReversal in this.redemptionDTO.RedemptionGiftsListDTO)
                {
                    if (giftLineForReversal.RedemptionId == -1)
                    {
                        giftLineForReversal.RedemptionId = redemptionDTO.RedemptionId;
                    }
                    redemptionGiftsBL = new RedemptionGiftsBL(machineUserContext, giftLineForReversal);
                    redemptionGiftsBL.Save(sqlTrx);
                    using (Utilities utilities = new Utilities())
                    {
                        SqlCommand sqlCommand = utilities.getCommand(sqlTrx);
                        Semnox.Parafait.Transaction.Inventory.updateStock(giftLineForReversal.GiftCode, sqlCommand, 1 * -1, machineUserContext.GetMachineId(), machineUserContext.GetUserId(), giftLineForReversal.RedemptionId, redemptionGiftsBL.RedemptionGiftsDTO.RedemptionGiftsId, 0, 0, "", machineUserContext.GetSiteId(), redemptionDTO.OrigRedemptionId, giftLineForReversal.OrignialRedemptionGiftId, "Redemption");
                    }

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
                                                                                       (originalGiftLineRTADTO.TurnInTickets == null ? null : originalGiftLineRTADTO.TurnInTickets * -1), machineUserContext.GetSiteId(), -1, false, "", machineUserContext.GetUserId(), DateTime.Now, DateTime.Now, machineUserContext.GetUserId(), originalGiftLineRTADTO.ManualTicketReceiptId, -1, -1, originalGiftLineRTADTO.RedemptionCurrencyRuleId, originalGiftLineRTADTO.RedemptionCurrencyRuleTicket, originalGiftLineRTADTO.SourceCurrencyRuleId);//DTO_Change

                                this.redemptionDTO.RedemptionTicketAllocationListDTO.Add(reversalGiftRTADTO);
                                RedemptionTicketAllocationDTO reversalTicketRTADTO = new RedemptionTicketAllocationDTO(-1, giftLineForReversal.RedemptionId, -1, (originalGiftLineRTADTO.ManualTickets == null ? null : originalGiftLineRTADTO.ManualTickets), (originalGiftLineRTADTO.GraceTickets == null ? null : originalGiftLineRTADTO.GraceTickets),
                                                                                         originalGiftLineRTADTO.CardId, (originalGiftLineRTADTO.ETickets == null ? null : originalGiftLineRTADTO.ETickets), originalGiftLineRTADTO.CurrencyId, originalGiftLineRTADTO.CurrencyQuantity,
                                                                                         (originalGiftLineRTADTO.CurrencyTickets == null ? null : originalGiftLineRTADTO.CurrencyTickets), originalGiftLineRTADTO.ManualTicketReceiptNo, (originalGiftLineRTADTO.ReceiptTickets == null ? null : originalGiftLineRTADTO.ReceiptTickets), (originalGiftLineRTADTO.TurnInTickets == null ? null : originalGiftLineRTADTO.TurnInTickets), machineUserContext.GetSiteId(), -1, false,
                                                                                         "", machineUserContext.GetUserId(), DateTime.Now, DateTime.Now, machineUserContext.GetUserId(), originalGiftLineRTADTO.ManualTicketReceiptId, -1, -1, originalGiftLineRTADTO.RedemptionCurrencyRuleId, originalGiftLineRTADTO.RedemptionCurrencyRuleTicket, originalGiftLineRTADTO.SourceCurrencyRuleId);
                                this.redemptionDTO.RedemptionTicketAllocationListDTO.Add(reversalTicketRTADTO);//DTO_Change
                            }
                        }
                    }
                }
            }
            this.redemptionDTO.RedemptionTicketAllocationListDTO.Sort((x, y) => x.RedemptionGiftId.CompareTo(y.RedemptionGiftId));

            log.LogMethodExit();
        }
        /// <summary>
        /// Returns order Number
        /// </summary>
        /// <param name="posMachineId">POS Machine Id</param>
        /// <param name="sqlTrx">SqlTransaction</param>
        /// <returns>string</returns>
        public static string GetNextSeqNo(int posMachineId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(posMachineId, sqlTrx);
            RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTrx);
            string seqNo = redemptionDataHandler.GetNextRedemptionOrderNo(posMachineId, sqlTrx);
            log.LogMethodExit(seqNo);
            return seqNo;
        }


        /// <summary>
        /// Redeem Tickets
        /// Used by Webservice
        /// </summary>
        /// <param name="redemptionParams">RedemptionParams</param>
        /// <returns>Returns bool</returns>
        public bool RedeemTickets(RedemptionParams redemptionParams)
        {
            log.LogMethodEntry(redemptionParams);
            string errorMessage;

            try
            {
                if (redemptionParams.TicketCount <= 0)
                {
                    errorMessage = MessageContainerList.GetMessage(machineUserContext, 1646);
                    throw new Exception(errorMessage);
                }

                Semnox.Parafait.Product.ProductDTO productDTO = new Semnox.Parafait.Product.ProductList().GetProduct(redemptionParams.ProductId);
                if (productDTO == null)
                {
                    errorMessage = MessageContainerList.GetMessage(machineUserContext, 111) + " : " + redemptionParams.ProductId;
                    throw new Exception(errorMessage);
                }

                CardCoreDTO cardCoreDTO = new CardCoreBL(redemptionParams.CardNumber).GetCardCoreDTO;
                if (cardCoreDTO.CardId <= 0 || (cardCoreDTO.ExpiryDate != DateTime.MinValue && cardCoreDTO.ExpiryDate.Date <= DateTime.Now.Date))
                {
                    errorMessage = MessageContainerList.GetMessage(machineUserContext, 56) + " : " + redemptionParams.CardNumber;
                    throw new Exception(errorMessage);
                }
                else
                {
                    CardCreditPlusBalanceDTO cardCreditPlusBalanceDTO = new CardCreditPlus().GetCreditPlusBalances(cardCoreDTO.CardId);
                    if (cardCreditPlusBalanceDTO == null || (cardCoreDTO.Ticket_count + cardCreditPlusBalanceDTO.CreditPlusTickets < redemptionParams.TicketCount))
                    {
                        errorMessage = MessageContainerList.GetMessage(machineUserContext, 1647);
                        throw new Exception(errorMessage);
                    }

                }

                int inventoryLocationId = -1;
                int posMachineId = -1;
                if (!string.IsNullOrEmpty(redemptionParams.PosMachine))
                {
                    // Get Pos Machine
                    List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_NAME, redemptionParams.PosMachine));

                    List<POSMachineDTO> pOSMachineDTOList = new POSMachineList(machineUserContext).GetAllPOSMachines(searchParameters);
                    if (pOSMachineDTOList != null && pOSMachineDTOList.Count > 0)
                    {
                        inventoryLocationId = pOSMachineDTOList[0].InventoryLocationId;
                        posMachineId = pOSMachineDTOList[0].POSMachineId;

                    }
                    else
                    {
                        errorMessage = MessageContainerList.GetMessage(machineUserContext, 1648, redemptionParams.PosMachine);
                        throw new Exception(errorMessage);
                    }
                    log.Info("RedeemTickets : Setting Pos InventoryLocationId : " + inventoryLocationId.ToString());

                }
                if (inventoryLocationId == -1 && productDTO.OutboundLocationId != -1)
                {
                    inventoryLocationId = productDTO.OutboundLocationId;
                    log.Info("RedeemTickets : Setting Pos InventoryLocationId with productDTO.OutboundLocationId" + inventoryLocationId.ToString());
                }

                log.Info("ReedemTickets : Start Saving Redemption Data ");
                machineUserContext.SetSiteId(-1);
                machineUserContext.SetUserId("Semnox");
                machineUserContext.SetMachineId(posMachineId);

                using (parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        log.Info("ReedemTickets : Start Redemption Transaction  : " + redemptionParams.CardNumber);
                        parafaitDBTrx.BeginTransaction();

                        // Grace Tickets + Price in Tickets  Grace Tickets - Site Level Parafait Tickets 

                        // Deduct CreditPlus 
                        double balanceToDeduct = new CardCreditPlus().DeductGenericCreditPlus(cardCoreDTO.CardId, "T", redemptionParams.TicketCount, parafaitDBTrx.SQLTrx);
                        SqlConnection utilityConnection = new SqlConnection(parafaitDBTrx.SQLTrx.Connection.ConnectionString);
                        using (Utilities parafaitUtility = new Utilities(utilityConnection))
                        {
                            Card updateCard = new Card((int)cardCoreDTO.CardId, machineUserContext.GetUserId(), parafaitUtility, parafaitDBTrx.SQLTrx);
                            updateCard.updateCardTime(parafaitDBTrx.SQLTrx);
                        }
                        // Deduct Card Balance
                        if (balanceToDeduct > 0)
                        {
                            new CardCoreBL().RedeemBalanceFromCard(cardCoreDTO.CardId, (int)balanceToDeduct, parafaitDBTrx.SQLTrx);
                        }

                        // Deduct Inventory
                        if (inventoryLocationId > 0)
                        {
                            List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParameters = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                            searchParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, redemptionParams.ProductId.ToString()));
                            searchParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, inventoryLocationId.ToString()));

                            // 0 && inventoryDTOList[0].Quantity > 0

                            List<InventoryDTO> inventoryDTOList = new InventoryList().GetAllInventory(searchParameters);
                            if (inventoryDTOList != null && inventoryDTOList.Count > 0)
                            {
                                InventoryDTO inventoryDTO = inventoryDTOList[0];
                                // Semnox.Parafait.Inventory.Inventory inventory = new Semnox.Parafait.Inventory.Inventory(inventoryDTO, executionUserContext);
                                Semnox.Parafait.Inventory.Inventory inventory = new Semnox.Parafait.Inventory.Inventory(inventoryDTO, machineUserContext);
                                inventoryDTO.Quantity = inventoryDTO.Quantity - 1;
                                inventory.Save(parafaitDBTrx.SQLTrx);
                            }
                        }

                        // Check Gift Availability - if POS location Inventroy Id is linked requited  , if -ve inventory allowed.


                        redemptionDTO = new RedemptionDTO(-1, cardCoreDTO.Card_number, null, redemptionParams.TicketCount, System.DateTime.Now, cardCoreDTO.CardId, -1, null, null, null, null, machineUserContext.GetUserId(),
                                            machineUserContext.GetSiteId(), "", false, -1, "Web", RedemptionBL.GetNextSeqNo(machineUserContext.GetMachineId(), parafaitDBTrx.SQLTrx), System.DateTime.Now, System.DateTime.Now,
                                            System.DateTime.Now, RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString(), System.DateTime.Now, machineUserContext.GetUserId(), null, null, null, null, null, machineUserContext.GetMachineId(), cardCoreDTO.Customer_id, null, null);



                        RedemptionCardsDTO redemptionCardsDTO = new RedemptionCardsDTO(-1, redemptionDTO.RedemptionId, cardCoreDTO.Card_number, cardCoreDTO.CardId, redemptionParams.TicketCount, -1, null,
                                                                    machineUserContext.GetSiteId(), "", false, -1, DateTime.Now, machineUserContext.GetUserId(), null, null, DateTime.Now, machineUserContext.GetUserId(),
                                                                    null, null, null,null);
                        redemptionDTO.RedemptionCardsListDTO.Add(redemptionCardsDTO);


                        RedemptionGiftsDTO redemptionGiftsDTO = new RedemptionGiftsDTO(-1, redemptionDTO.RedemptionId, cardCoreDTO.Card_number, redemptionParams.ProductId, inventoryLocationId, redemptionParams.TicketCount,
                                                                    null, -1, machineUserContext.GetSiteId(), -1, false, "", machineUserContext.GetUserId(), DateTime.Now, DateTime.Now, machineUserContext.GetUserId(),
                                                                    Convert.ToInt32(productDTO.PriceInTickets), null, null, false, -1,1);//DTO_Change
                        redemptionGiftsDTO.ProductQuantity = 1;

                        if (redemptionDTO.RedemptionGiftsListDTO == null)
                        {
                            redemptionDTO.RedemptionGiftsListDTO = new List<RedemptionGiftsDTO>();
                        }
                        redemptionDTO.RedemptionGiftsListDTO.Add(redemptionGiftsDTO);
                        this.Save(parafaitDBTrx.SQLTrx);
                        log.Debug("ReedemTickets : Completed Saving Redemption Data ");

                        parafaitDBTrx.EndTransaction();
                        log.Debug("ReedemTickets : Commit Redemption Transaction  : " + redemptionParams.CardNumber);

                        log.Debug("ReedemTickets : Completed Saving Redemption Data ");
                        log.Debug("ENDS - ReedemTickets : Saved Redemption Data Successfully");
                        log.LogMethodExit(true);
                        return true;

                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        parafaitDBTrx.RollBack();
                        log.Info("ReedemTickets : Rollback Redemption Transaction  : " + redemptionParams.CardNumber);
                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

        }
        /// <summary>
        /// Gets the Total Ticket Count which are added to the redemptionCards and Ticket Receipts
        /// </summary>
        /// <returns>TotalTickets</returns>
        public int GetTotalTickets()
        {
            log.LogMethodEntry();
            int tickets = GetCardTickets() + GetScannedTickets() + GetManualTickets();
            log.LogMethodExit(tickets);
            return tickets;
        }

        /// <summary>
        /// Gets the RedeemedTickets
        /// </summary>
        /// <returns>RedeemedTickets</returns>
        public int GetRedeemedTickets()
        {
            log.LogMethodEntry();
            int RedeemedTickets = 0;
            if (redemptionDTO != null)
            {
                if (redemptionDTO.RedemptionGiftsListDTO != null && redemptionDTO.RedemptionGiftsListDTO.Count != 0)
                {
                    foreach (RedemptionGiftsDTO giftProduct in redemptionDTO.RedemptionGiftsListDTO)
                    {
                        RedeemedTickets += Convert.ToInt32(giftProduct.ProductQuantity * giftProduct.Tickets);
                    }
                }
            }
            log.LogMethodExit(RedeemedTickets);
            return RedeemedTickets;
        }

        ///<summary>
        ///Gets available tickets
        ///</summary>
        /// <returns>Available Tickets</returns>
        public int GetAvailbleTickets()
        {
            log.LogMethodEntry();
            log.LogMethodEntry(GetTotalTickets() - GetRedeemedTickets());
            return (GetTotalTickets() - GetRedeemedTickets());
        }

        ///<summary>
        ///Returns the grace Tickets
        ///</summary>
        ///<param name="tickets">Tickets</param>
        /// <returns>Grace Tickets</returns>
        public static int GetGraceTickets(int tickets)
        {
            log.LogMethodEntry(tickets);
            if (tickets > 0)
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<int>(ExecutionContext.GetExecutionContext(), "REDEMPTION_GRACE_TICKETS") > 0)
                {
                    return ParafaitDefaultContainerList.GetParafaitDefault<int>(ExecutionContext.GetExecutionContext(), "REDEMPTION_GRACE_TICKETS");
                }
                else if (ParafaitDefaultContainerList.GetParafaitDefault<int>(ExecutionContext.GetExecutionContext(), "REDEMPTION_GRACE_TICKETS_PERCENTAGE") > 0)
                {
                    return ParafaitDefaultContainerList.GetParafaitDefault<int>(ExecutionContext.GetExecutionContext(), "REDEMPTION_GRACE_TICKETS_PERCENTAGE") * tickets / 100;
                }
            }
            log.LogMethodExit(0);
            return 0;
        }

        ///<summary>
        ///adds the card to order
        ///</summary>
        ///<param name="cardNumber">Card Number</param>
        ///<returns>message</returns>
        public string AddCard(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            string message = "";
            if (this.redemptionDTO == null)
            {
                this.redemptionDTO = new RedemptionDTO();
            }
            else
            {
                if (redemptionDTO.RedemptionCardsListDTO.Find(delegate (RedemptionCardsDTO item) { return (item.CardNumber == cardNumber); }) != null)
                {
                    message = MessageContainerList.GetMessage(machineUserContext, 59);
                    log.Debug(cardNumber + "," + message);
                    throw new Exception(message);
                }
            }
            AccountBL cardAccount = new AccountBL(machineUserContext, cardNumber, true, true);
            if (cardAccount != null && cardAccount.GetAccountId() != -1)
            {

                if (!cardAccount.AccountAllowedToRoam())
                {
                    message = MessageContainerList.GetMessage(machineUserContext, 110);
                    log.Info(cardNumber + "," + message);
                    throw new Exception(message);
                }
                if (cardAccount.IsTechnicianAccount())
                {
                    message = MessageContainerList.GetMessage(machineUserContext, 197, cardNumber);
                    throw new Exception(message);
                }

                RedemptionCardsDTO redemptionCard = new RedemptionCardsDTO
                {
                    CardId = cardAccount.GetAccountId(),
                    CardNumber = cardNumber,
                    TotalCardTickets = cardAccount.GetTotalTickets()
                };
                if (cardAccount.GetCustomerId() > -1 && this.redemptionDTO.RedemptionCardsListDTO.Count == 0)
                {
                    this.redemptionDTO.CustomerId = cardAccount.GetCustomerId();
                }
                this.redemptionDTO.RedemptionCardsListDTO.Add(redemptionCard);


                message = MessageContainerList.GetMessage(machineUserContext, "Card") + ": " + cardNumber;
                log.LogMethodExit(message);
                return message;
            }
            else
            {
                message = MessageContainerList.GetMessage(machineUserContext, 110, cardNumber);
                log.Info(message);
                throw new Exception(message);
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
        /// <summary>
        /// Checks whether card is present in the redemption
        /// </summary>
        /// <param name="cardNumber">Card Number</param>
        /// <returns>bool</returns>
        public bool IsCardPresentInOrder(string cardNumber)
        {
            log.LogMethodEntry();
            bool retValue = false;
            if (redemptionDTO != null && redemptionDTO.RedemptionCardsListDTO.Count > 0)
            {
                retValue = redemptionDTO.RedemptionCardsListDTO.Exists(cards => cards.CardNumber == cardNumber);
            }
            log.LogMethodExit(retValue);
            return retValue;
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
        /// Checks whether gifts are selected for redemption
        /// </summary>
        /// <returns>bool</returns>
        public bool RedemptionHasGifts()
        {
            log.LogMethodEntry();
            bool retValue = false;
            if (this.redemptionDTO != null 
                && this.redemptionDTO.RedemptionGiftsListDTO != null && this.redemptionDTO.RedemptionGiftsListDTO.Any())
            {
                retValue = true;
            }
            log.LogMethodExit(retValue);
            return retValue;
        }

        /// <summary>
        /// Checks whether gifts is already added to the redemption
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns>RedemptionGiftsDTO</returns>
        public RedemptionGiftsDTO GetGiftEntry(int productId)
        {
            log.LogMethodEntry(productId);
            RedemptionGiftsDTO retValue = null;
            if (this.redemptionDTO != null && this.redemptionDTO.RedemptionGiftsListDTO != null 
                && this.redemptionDTO.RedemptionGiftsListDTO.Any())
            {
                retValue = this.redemptionDTO.RedemptionGiftsListDTO.Find(gift => gift.ProductId == productId);
            }
            log.LogMethodExit(retValue);
            return retValue;
        }

        /// <summary>
        /// Loads the tickets to card
        /// </summary>
        /// <param name="utilities">Utilities</param>
        /// <param name="redemptionSource">Redemption Source</param>
        public void LoadTicketsToCard(Utilities utilities, string redemptionSource)
        {
            log.LogMethodEntry(utilities, redemptionSource);
            string message = string.Empty;
            if (this.redemptionDTO != null)
            {
                if (this.redemptionDTO.TicketReceiptListDTO.Count <= 0)
                {
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1380));
                }
                if (this.redemptionDTO.RedemptionGiftsListDTO != null 
                    && this.redemptionDTO.RedemptionGiftsListDTO.Any())
                {
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1377));
                }
                if (this.redemptionDTO.RedemptionCardsListDTO.Count > 1)
                {
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1376));
                }

                if (IsCardRequiredForRedemption(machineUserContext) && !RedemptionHasCards())
                {
                    //Card is required to complete the order
                    log.Error(MessageContainerList.GetMessage(machineUserContext, 1613));
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1613));
                }

                bool managerApprovalReceived = (utilities.ParafaitEnv.ManagerId != -1);
                if (GetManualTickets() > 0)
                {
                    ManualTicketLimitChecks(managerApprovalReceived, GetManualTickets());
                }
                LoadTicketLimitCheck(managerApprovalReceived, (GetScannedTickets() + GetManualTickets()));

                using (parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    Card primaryRedemptionCard = new Card((int)redemptionDTO.RedemptionCardsListDTO[0].CardId, "", utilities);
                    try
                    {
                        redemptionDTO.CardId = redemptionDTO.RedemptionCardsListDTO[0].CardId;
                        redemptionDTO.PrimaryCardNumber = redemptionDTO.RedemptionCardsListDTO[0].CardNumber;
                        redemptionDTO.ReceiptTickets = GetScannedTickets();
                        redemptionDTO.RedeemedDate = DateTime.Now;
                        redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString();
                        redemptionDTO.Source = redemptionSource;
                        redemptionDTO.RedemptionOrderNo = RedemptionBL.GetNextSeqNo(machineUserContext.GetMachineId(), parafaitDBTrx.SQLTrx);
                        redemptionDTO.Remarks = "Scanned Tickets loaded to card";

                        redemptionDTO.POSMachineId = machineUserContext.GetMachineId();
                        if (redemptionDTO.CustomerId == -1)
                        {
                            redemptionDTO.CustomerId = primaryRedemptionCard.customer_id;
                        }

                        for (int i = 0; i < this.redemptionDTO.RedemptionCardsListDTO.Count; i++)
                        {
                            this.redemptionDTO.RedemptionCardsListDTO[i].TicketCount = 0;
                        }

                        TicketReceipt ticketReceipt;
                        foreach (TicketReceiptDTO ticketReceiptDTO in this.redemptionDTO.TicketReceiptListDTO)
                        {
                            ticketReceipt = new TicketReceipt(machineUserContext, ticketReceiptDTO);
                            if (ticketReceipt.IsUsedTicketReceipt(parafaitDBTrx.SQLTrx))
                            {
                                log.Error(MessageContainerList.GetMessage(machineUserContext, 1625, ticketReceiptDTO.ManualTicketReceiptNo));
                                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1625, ticketReceiptDTO.ManualTicketReceiptNo));
                            }
                        }
                        Save(parafaitDBTrx.SQLTrx);

                        TaskProcs tp = new TaskProcs(utilities);

                        if (!tp.loadTickets(primaryRedemptionCard, GetScannedTickets() + GetManualTickets(), "Redemption Kiosk-Load Tickets", redemptionDTO.RedemptionId, ref message, parafaitDBTrx.SQLTrx))
                        {
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new Exception(message);
                        }
                        else
                        {
                            parafaitDBTrx.EndTransaction();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        parafaitDBTrx.RollBack();
                        throw new Exception(ex.Message);
                    }
                }
            }

            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the total card tickets count
        /// </summary>
        /// <returns>Card Tickets</returns>
        public int GetCardTickets()
        {
            log.LogMethodEntry();
            int tickets = 0;
            if (redemptionDTO != null)
            {
                if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Count != 0)
                {
                    foreach (RedemptionCardsDTO card in redemptionDTO.RedemptionCardsListDTO)
                    {
                        tickets += Convert.ToInt32(card.TotalCardTickets);
                    }
                }
            }
            log.LogMethodExit(tickets);
            return tickets;
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
        /// <summary>
        /// Returns true if the confirmation sucessfull
        /// </summary>
        /// <param name="managerApprovalReceived">boolean</param>
        /// <param name="sqlTrx">SqlTransaction</param>
        /// <returns>List of ValidationError </returns>
        internal void ValidateOrder(bool managerApprovalReceived, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(managerApprovalReceived, sqlTrx);
            int totalTickets = GetTotalTickets();
            if (IsCardRequiredForRedemption(machineUserContext) && !RedemptionHasCards())
            {
                //Please scan a card for redeeming gifts
                log.Error(MessageContainerList.GetMessage(machineUserContext, 475));
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 475));
            }
            if (GetRedeemedTickets() > totalTickets + GetGraceTickets(totalTickets))
            {
                //Redeemed tickets more than availble tickets.Cannot place order
                log.Error(MessageContainerList.GetMessage(machineUserContext, 1635));
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1635));
            }
            TicketReceipt ticketReceipt;
            foreach (TicketReceiptDTO receipt in redemptionDTO.TicketReceiptListDTO)
            {
                ticketReceipt = new TicketReceipt(machineUserContext, receipt);
                if (ticketReceipt.IsUsedTicketReceipt(sqlTrx))
                {
                    //Ticket Receipt: &1 is alredy used
                    log.Error(MessageContainerList.GetMessage(machineUserContext, 1625, receipt.ManualTicketReceiptNo));
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1625, receipt.ManualTicketReceiptNo));
                }
            }
            if (redemptionDTO.RedemptionCardsListDTO.Count > 0)
            {
                log.Info("Inside Card update status check");

                foreach (RedemptionCardsDTO redemptionCard in redemptionDTO.RedemptionCardsListDTO)
                {
                    AccountBL cardAccount = new AccountBL(machineUserContext, redemptionCard.CardId, true, true, sqlTrx);
                    if (cardAccount.GetAccountId() != redemptionCard.CardId || (cardAccount.GetTotalTickets() != redemptionCard.TotalCardTickets))
                    {
                        log.Error(redemptionCard.CardNumber + " - " + MessageContainerList.GetMessage(machineUserContext, 354));
                        throw new Exception(redemptionCard.CardNumber + " - " + MessageContainerList.GetMessage(machineUserContext, 354));
                    }
                }
            }
            if (redemptionDTO.RedemptionGiftsListDTO.Count == 0)
            {
                log.Error(MessageContainerList.GetMessage(machineUserContext, 119));
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 119));
            }

            foreach (RedemptionGiftsDTO redemptionGift in redemptionDTO.RedemptionGiftsListDTO)
            {
                ValidationError validationError = new ValidationError();
                Semnox.Parafait.Product.ProductBL product = new Semnox.Parafait.Product.ProductBL(machineUserContext,redemptionGift.ProductId);
                int productQty = redemptionDTO.RedemptionGiftsListDTO.Where(x => x.ProductId == redemptionGift.ProductId).Sum(y => y.ProductQuantity);
                if (!product.IsAvailableInInventory(machineUserContext, productQty))
                {
                    string errorMessage = MessageContainerList.GetMessage(machineUserContext, 1641, redemptionGift.ProductName);
                    throw new ValidationException(errorMessage);
                }
            }
            if (GetRedeemedTickets() > GetTotalTickets())
            {
                //Sorry, not enough tickets to add the gift
                log.Error(MessageContainerList.GetMessage(machineUserContext, 1632));
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1632));
            }

            int redemptionTransactionTicketMgrLimit = 0;
            try
            {
                redemptionTransactionTicketMgrLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "REDEMPTION_LIMIT_FOR_MANAGER_APPROVAL"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            if (redemptionTransactionTicketMgrLimit > 0 && (GetRedeemedTickets() > redemptionTransactionTicketMgrLimit && managerApprovalReceived == false))
            {
                //Redeem Ticket value entered is above user limit. Please get manager approval
                log.Error(MessageContainerList.GetMessage(machineUserContext, 1213));
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1213));
            }

            int redemptionTransactionTicketLimit = 0;
            try
            {
                redemptionTransactionTicketLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "REDEMPTION_TRANSACTION_TICKET_LIMIT"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            if (redemptionTransactionTicketLimit > 0 && GetRedeemedTickets() > redemptionTransactionTicketLimit)
            {
                //Redeemed ticket value (&1) should not be greater than &2 
                log.Error(MessageContainerList.GetMessage(machineUserContext, 1438, GetRedeemedTickets(), redemptionTransactionTicketLimit));
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1438, GetRedeemedTickets(), redemptionTransactionTicketLimit));
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns remaining tickets after redeeming the gift
        /// </summary>        
        /// <param name="cmdTrx">SqlTransaction</param>
        /// <returns>balancePhysicalTickets</returns>
        private int DeductRedeemedPoints(SqlTransaction cmdTrx)
        {
            log.LogMethodEntry(cmdTrx);
            int? customerTickets = 0;
            int? eticketsToBeAllocated = 0;
            int physicalTicketsRedeemed = 0;
            int graceTickets = 0;
            int manualTickets = GetManualTickets();
            int manualTicketRedeemed = 0, currencyTicketRedeemed = 0, receiptTicketRedeemed = 0;

            using (Utilities parafaitUtility = new Utilities())
            {
                try
                {
                    int redeemedPoints = GetRedeemedTickets();
                    int ticketsReceipt = GetScannedTickets();
                    int ticketsCurrency = 0; // GetCurrencyTickets(); to be developed
                    int ticketsTotalPhysical = ticketsReceipt + ticketsCurrency + manualTickets;
                    int ticketsCards = GetCardTickets();

                    if (redeemedPoints > (ticketsCards + ticketsTotalPhysical + GetGraceTickets(ticketsCards + ticketsTotalPhysical)))
                    {
                        log.Error(MessageContainerList.GetMessage(machineUserContext, 121));
                        throw new Exception(MessageContainerList.GetMessage(machineUserContext, 121));
                    }


                    int balancePhysicalTickets;
                    if (redeemedPoints <= ticketsTotalPhysical)
                    {
                        eticketsToBeAllocated = 0;
                        physicalTicketsRedeemed = redeemedPoints;
                        if (manualTickets >= physicalTicketsRedeemed)
                        {
                            manualTicketRedeemed = physicalTicketsRedeemed;
                        }
                        else if (manualTickets + ticketsCurrency >= physicalTicketsRedeemed)
                        {
                            manualTicketRedeemed = manualTickets;
                            currencyTicketRedeemed = physicalTicketsRedeemed - manualTicketRedeemed;
                        }
                        else
                        {
                            manualTicketRedeemed = manualTickets;
                            currencyTicketRedeemed = ticketsCurrency;
                            receiptTicketRedeemed = physicalTicketsRedeemed - manualTicketRedeemed - currencyTicketRedeemed;
                        }

                        balancePhysicalTickets = ticketsTotalPhysical - physicalTicketsRedeemed;
                    }
                    else if (redeemedPoints <= ticketsTotalPhysical + ticketsCards)
                    {
                        eticketsToBeAllocated = redeemedPoints - ticketsTotalPhysical;
                        physicalTicketsRedeemed = ticketsTotalPhysical;

                        if (manualTickets >= physicalTicketsRedeemed)
                        {
                            manualTicketRedeemed = physicalTicketsRedeemed;
                        }
                        else if (manualTickets + ticketsCurrency >= physicalTicketsRedeemed)
                        {
                            manualTicketRedeemed = manualTickets;
                            currencyTicketRedeemed = physicalTicketsRedeemed - manualTicketRedeemed;
                        }
                        else
                        {
                            manualTicketRedeemed = manualTickets;
                            currencyTicketRedeemed = ticketsCurrency;
                            receiptTicketRedeemed = physicalTicketsRedeemed - manualTicketRedeemed - currencyTicketRedeemed;
                        }

                        balancePhysicalTickets = 0;
                    }
                    else // redemption using grace tickets
                    {
                        eticketsToBeAllocated = ticketsCards;
                        physicalTicketsRedeemed = ticketsTotalPhysical;

                        if (manualTickets >= physicalTicketsRedeemed)
                        {
                            manualTicketRedeemed = physicalTicketsRedeemed;
                        }
                        else if (manualTickets + ticketsCurrency >= physicalTicketsRedeemed)
                        {
                            manualTicketRedeemed = manualTickets;
                            currencyTicketRedeemed = physicalTicketsRedeemed - manualTicketRedeemed;
                        }
                        else
                        {
                            manualTicketRedeemed = manualTickets;
                            currencyTicketRedeemed = ticketsCurrency;
                            receiptTicketRedeemed = physicalTicketsRedeemed - manualTicketRedeemed - currencyTicketRedeemed;
                        }

                        graceTickets = redeemedPoints - ticketsTotalPhysical - ticketsCards;
                        if (graceTickets > 0)
                        {
                            redemptionDTO.GraceTickets = graceTickets;
                            //TicketSourceInfo ticketSourceObj = new TicketSourceInfo();
                            //ticketSourceObj.ticketSource = "Grace";
                            //ticketSourceObj.ticketValue = graceTickets;
                            //ticketSourceObj.balanceTickets = ticketSourceObj.ticketValue;
                            //ticketSourceInfoObj.Add(ticketSourceObj);
                        }
                        balancePhysicalTickets = 0;
                    }

                    redemptionDTO.ReceiptTickets = receiptTicketRedeemed;
                    redemptionDTO.CurrencyTickets = currencyTicketRedeemed;
                    redemptionDTO.ManualTickets = manualTicketRedeemed;
                    redemptionDTO.ETickets = eticketsToBeAllocated;

                    int? balGraceTickets = graceTickets;
                    List<RedemptionGiftsDTO> finalGiftDTOList = new List<RedemptionGiftsDTO>();
                    SqlCommand cmd = parafaitUtility.getCommand();
                    if (redemptionDTO.RedemptionGiftsListDTO != null)
                    {
                        foreach (RedemptionGiftsDTO giftItem in redemptionDTO.RedemptionGiftsListDTO)
                        {

                            int? price = 0;
                            int? grace = 0;
                            for (int i = 0; i < giftItem.ProductQuantity; i++)
                            {
                                if (graceTickets > 0)
                                {
                                    price = giftItem.Tickets;
                                    grace = 0;
                                    if (giftItem.Equals(redemptionDTO.RedemptionGiftsListDTO[redemptionDTO.RedemptionGiftsListDTO.Count - 1]) && i == giftItem.ProductQuantity - 1) // last line
                                    {
                                        price = price - balGraceTickets;
                                        grace = balGraceTickets;
                                    }
                                    else
                                    {
                                        grace = price * graceTickets / redeemedPoints;
                                        price = price - grace;
                                        balGraceTickets -= grace;
                                    }
                                }
                                else
                                {
                                    price = giftItem.Tickets;
                                    grace = 0;
                                }

                                RedemptionGiftsDTO redemptionGiftsDTONew = new RedemptionGiftsDTO(giftItem.RedemptionGiftsId, giftItem.RedemptionId, giftItem.GiftCode, giftItem.ProductId, giftItem.LocationId, price, grace, giftItem.LotId, giftItem.SiteId,
                                                                               giftItem.MasterEntityId, giftItem.SynchStatus, "", giftItem.LastUpdatedBy, giftItem.LastUpdateDate, giftItem.CreationDate, giftItem.CreatedBy, giftItem.OriginalPriceInTickets,
                                                                               giftItem.ProductName, giftItem.ProductDescription, giftItem.GiftLineIsReversed, -1, 1);
                                redemptionGiftsDTONew.ProductQuantity = 1;
                                finalGiftDTOList.Add(redemptionGiftsDTONew);
                            }
                        }
                    }
                    redemptionDTO.RedemptionGiftsListDTO = null;
                    redemptionDTO.RedemptionGiftsListDTO = finalGiftDTOList;

                    int? ticketsUsed = 0;

                    CreditPlus creditPlus = new CreditPlus(parafaitUtility);
                    for (int i = 0; i < redemptionDTO.RedemptionCardsListDTO.Count; i++)
                    {
                        redemptionDTO.RedemptionCardsListDTO[i].TicketCount = 0;
                        if (eticketsToBeAllocated > 0)
                        {
                            customerTickets = 0;
                            customerTickets = redemptionDTO.RedemptionCardsListDTO[i].TotalCardTickets;
                            if (customerTickets <= 0)
                            {
                                continue;
                            }

                            if (customerTickets - eticketsToBeAllocated >= 0)
                            {
                                customerTickets -= eticketsToBeAllocated;
                                ticketsUsed = eticketsToBeAllocated;
                                eticketsToBeAllocated = 0;
                            }
                            else
                            {
                                eticketsToBeAllocated -= customerTickets;
                                ticketsUsed = customerTickets;
                                customerTickets = 0;
                            }

                            try
                            {
                                creditPlus.deductCreditPlusTicketsLoyaltyPoints(redemptionDTO.RedemptionCardsListDTO[i].CardNumber, (int)ticketsUsed, 0, cmdTrx);

                                redemptionDTO.RedemptionCardsListDTO[i].TicketCount = ticketsUsed;
                            }
                            catch (Exception ex)
                            {
                                log.Error("Card No" + redemptionDTO.RedemptionCardsListDTO[i].CardNumber + " " + MessageContainerList.GetMessage(machineUserContext, 1636));
                                log.Error(ex);
                                throw new Exception("Card No" + redemptionDTO.RedemptionCardsListDTO[i].CardNumber + " " + MessageContainerList.GetMessage(machineUserContext, 1636));
                            }
                        }
                    }

                    log.LogMethodExit(balancePhysicalTickets);
                    return balancePhysicalTickets;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Place RedemptionOrder 
        /// </summary>
        /// <param name="managerApprovalReceived">bool</param>
        /// <param name="redemptionSource">Redemption Source</param>
        /// <param name="sqlTrx">Sql Transaction</param>
        /// <returns>Order No</returns>
        public string PlaceRedemptionOrder(bool managerApprovalReceived, string redemptionSource, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(managerApprovalReceived, redemptionSource, sqlTrx);
            string orderNo = string.Empty;
            List<ValidationError> validationErrorList = null;
            try
            {
                ValidateOrder(managerApprovalReceived, sqlTrx);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            if (validationErrorList != null && validationErrorList.Count > 0)
            {
                throw new ValidationException("Insufficient product stock", validationErrorList);
            }

            try
            {
                balanceTicketsRemaining = DeductRedeemedPoints(sqlTrx);
                redemptionDTO.RedemptionOrderNo = RedemptionBL.GetNextSeqNo(machineUserContext.GetMachineId(), sqlTrx);
                if (redemptionDTO.RedemptionCardsListDTO.Count > 0)
                {
                    redemptionDTO.CardId = redemptionDTO.RedemptionCardsListDTO[0].CardId;
                    redemptionDTO.PrimaryCardNumber = redemptionDTO.RedemptionCardsListDTO[0].CardNumber;
                    if (redemptionDTO.CustomerId == -1)
                    {
                        redemptionDTO.CustomerId = GetRedemptionPrimaryCard(sqlTrx).customer_id;
                    }
                }
                redemptionDTO.Source = redemptionSource;
                redemptionDTO.RedeemedDate = DateTime.Now;
                redemptionDTO.POSMachineId = machineUserContext.GetMachineId();

                redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.OPEN.ToString();
                Save(sqlTrx);
                orderNo = redemptionDTO.RedemptionOrderNo;
                if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any())
                {
                    for (int i = 0; i < redemptionDTO.RedemptionCardsListDTO.Count; i++)
                    {
                        if (redemptionDTO.RedemptionCardsListDTO[i].TicketCount != null && redemptionDTO.RedemptionCardsListDTO[i].TicketCount != 0)
                        {
                            AccountBL accountBL = new AccountBL(machineUserContext, redemptionDTO.RedemptionCardsListDTO[i].CardId, false, true, sqlTrx);
                            accountBL.AccountDTO.IsChanged = true;
                            accountBL.Save(sqlTrx);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(orderNo);
            return orderNo;
        }

        /// <summary>
        /// AddTicket - Add ticket receipt to the Redemption
        /// </summary>
        /// <param name="ticketReceipt">TicketReceipt</param>
        public void AddTicket(TicketReceipt ticketReceipt)
        {
            log.LogMethodEntry(ticketReceipt);
            if (ticketReceipt != null)
            {
                if (this.redemptionDTO != null && this.redemptionDTO.TicketReceiptListDTO.Exists(rcpt => rcpt.ManualTicketReceiptNo == ticketReceipt.TicketReceiptDTO.ManualTicketReceiptNo))
                {
                    log.Error(MessageContainerList.GetMessage(machineUserContext, 1622));
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1622));//Ticket is already Scanned
                }
                if (this.redemptionDTO == null)
                {
                    this.redemptionDTO = new RedemptionDTO();
                }

                this.redemptionDTO.TicketReceiptListDTO.Add(ticketReceipt.TicketReceiptDTO);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the DTO
        /// </summary>
        public RedemptionDTO RedemptionDTO
        {
            get
            {
                return redemptionDTO;
            }
        }
        /// <summary>
        /// Consolidate ticket receipts
        /// </summary>
        /// <param name="managerApprovalReceived">bool</param>
        /// <param name="redemptionSource">Redemption source</param>
        /// <param name="sqlTrx">SqlTrx</param>
        /// <returns>TicketReceipt</returns>
        public TicketReceipt ConsolidateTicketReceipts(bool managerApprovalReceived, string redemptionSource, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(managerApprovalReceived, redemptionSource, sqlTrx);
            if (this.redemptionDTO != null && (this.redemptionDTO.TicketReceiptListDTO.Count < 2))
            {
                log.Error(MessageContainerList.GetMessage(machineUserContext, 1623));
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1623));
                //Scan multiple tickets receipts for consolidation
            }
            if (this.redemptionDTO.RedemptionGiftsListDTO != null 
                && this.redemptionDTO.RedemptionGiftsListDTO.Any())
            {
                log.Error(MessageContainerList.GetMessage(machineUserContext, 1624));
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1624));
                //Gifts are selected for Redemption. Can not proceed with ticket receipt consolidation
            }
            if (IsCardRequiredForRedemption(machineUserContext) && !RedemptionHasCards())
            {
                //Card is required to complete the order
                log.Error(MessageContainerList.GetMessage(machineUserContext, 1613));
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1613));
            }
            int totalTickets = 0;
            TicketReceipt newTicketReceipt = null;
            TicketReceipt ticketReceipt;
            foreach (TicketReceiptDTO item in this.redemptionDTO.TicketReceiptListDTO)
            {
                ticketReceipt = new TicketReceipt(machineUserContext, item);
                if (ticketReceipt.IsUsedTicketReceipt(sqlTrx))
                {
                    log.Error(MessageContainerList.GetMessage(machineUserContext, 1625, item.ManualTicketReceiptNo));
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1625, item.ManualTicketReceiptNo));
                }
                totalTickets += item.Tickets;
            }
            if (GetManualTickets() > 0)
            {
                ManualTicketLimitChecks(managerApprovalReceived, GetManualTickets());
            }
            LoadTicketLimitCheck(managerApprovalReceived, totalTickets + GetManualTickets());
            try
            {
                //this.redemptionDTO.LastUpdateDate = DateTime.Now;
                if (this.redemptionDTO.RedemptionCardsListDTO != null && this.redemptionDTO.RedemptionCardsListDTO.Count > 0)
                {
                    this.redemptionDTO.CardId = this.redemptionDTO.RedemptionCardsListDTO[0].CardId;
                    this.redemptionDTO.PrimaryCardNumber = this.redemptionDTO.RedemptionCardsListDTO[0].CardNumber;
                    for (int i = 0; i < this.redemptionDTO.RedemptionCardsListDTO.Count; i++)
                    {
                        this.redemptionDTO.RedemptionCardsListDTO[i].TicketCount = 0;
                    }
                    if (this.redemptionDTO.CustomerId == -1)
                    {
                        this.redemptionDTO.CustomerId = GetRedemptionPrimaryCard(sqlTrx).customer_id;
                    }
                }
                this.redemptionDTO.RedeemedDate = DateTime.Now;
                this.redemptionDTO.Remarks = "Ticket consolidation";
                this.redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString();
                this.redemptionDTO.RedemptionOrderNo = RedemptionBL.GetNextSeqNo(machineUserContext.GetMachineId(), sqlTrx);
                this.redemptionDTO.ReceiptTickets = totalTickets;//GetScannedTickets();
                this.redemptionDTO.Source = redemptionSource;
                this.redemptionDTO.POSMachineId = machineUserContext.GetMachineId();
                this.Save(sqlTrx);
                newTicketReceipt = new TicketReceipt(machineUserContext);
                newTicketReceipt.CreateManualTicketReceipt(totalTickets, this.redemptionDTO.RedemptionId, sqlTrx);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(newTicketReceipt);
            return newTicketReceipt;
        }

        /// <summary>
        /// Add gift to the order
        /// </summary>
        /// <param name="productDTO">ProductDTO</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="redemptionDiscount">Redemption Discount</param>
        public void AddGift(ProductDTO productDTO, int quantity, double redemptionDiscount)
        {
            log.LogMethodEntry(productDTO, quantity, redemptionDiscount);
            RedemptionGiftsDTO redemptionGiftsDTO = null;
            if (this.redemptionDTO != null && this.redemptionDTO.RedemptionGiftsListDTO != null && this.redemptionDTO.RedemptionGiftsListDTO.Count != 0 && this.redemptionDTO.RedemptionGiftsListDTO.Exists(item => item.ProductId == productDTO.ProductId))
            {
                redemptionGiftsDTO = this.redemptionDTO.RedemptionGiftsListDTO.Find(r => r.ProductId == productDTO.ProductId);
                if (quantity == 0)
                {
                    this.redemptionDTO.RedemptionGiftsListDTO.Remove(redemptionGiftsDTO);
                }
                else
                {
                    redemptionGiftsDTO.ProductQuantity = quantity;
                }
            }
            else
            {
                int posInventoryLocationId = -1;
                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_MACHINE_ID, machineUserContext.GetMachineId().ToString()));

                List<POSMachineDTO> pOSMachineDTOList = new POSMachineList(machineUserContext).GetAllPOSMachines(searchParameters);
                if (pOSMachineDTOList != null && pOSMachineDTOList.Count > 0)
                {
                    posInventoryLocationId = pOSMachineDTOList[0].InventoryLocationId;
                }
                redemptionGiftsDTO = new RedemptionGiftsDTO
                {
                    ProductId = productDTO.ProductId,
                    ProductQuantity = quantity,
                    GiftCode = productDTO.Code,
                    Tickets = Convert.ToInt32(productDTO.PriceInTickets * redemptionDiscount),
                    ProductName = productDTO.ProductName,
                    ProductDescription = productDTO.Description,
                    ImageFileName = productDTO.ImageFileName,
                    OriginalPriceInTickets = Convert.ToInt32(productDTO.PriceInTickets),
                    LocationId = (posInventoryLocationId == -1 ? productDTO.OutboundLocationId : posInventoryLocationId)
                };
                if (this.redemptionDTO.RedemptionGiftsListDTO == null)
                {
                    this.redemptionDTO.RedemptionGiftsListDTO = new List<RedemptionGiftsDTO>();
                }
                this.redemptionDTO.RedemptionGiftsListDTO.Add(redemptionGiftsDTO);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete gift from the order
        /// </summary>
        /// <param name="productId">ProductId</param>
        public void RemoveGift(int productId)
        {
            log.LogMethodEntry(productId);
            RedemptionGiftsDTO redemptionGiftsDTO = null;
            if (this.redemptionDTO != null && this.redemptionDTO.RedemptionGiftsListDTO != null && this.redemptionDTO.RedemptionGiftsListDTO.Count != 0)
            {
                redemptionGiftsDTO = this.redemptionDTO.RedemptionGiftsListDTO.Find(r => r.ProductId == productId);
                if (redemptionGiftsDTO != null)
                {
                    this.redemptionDTO.RedemptionGiftsListDTO.Remove(redemptionGiftsDTO);
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Get first tapped card for the redemption
        /// </summary>
        /// <param name="sqlTrx">SqlTransaction</param>
        /// <returns>Card</returns>
        public Card GetRedemptionPrimaryCard(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            Card primaryCard = null;
            if (this.redemptionDTO != null && this.redemptionDTO.RedemptionCardsListDTO != null && this.redemptionDTO.RedemptionCardsListDTO.Count > 0)
            {
                using (Utilities parafaitUtility = new Utilities())
                {
                    primaryCard = new Card(this.redemptionDTO.RedemptionCardsListDTO[0].CardId, machineUserContext.GetUserId(), parafaitUtility, sqlTrx);
                }
            }
            log.LogMethodExit(primaryCard);
            return primaryCard;
        }

        /// <summary>
        /// Checks whether manager approval is required
        /// </summary>
        /// <returns>bool</returns>
        public bool RedemptionTransactionNeedsManagerApproval()
        {
            log.LogMethodEntry();
            bool retVal = false;
            int redemptionTransactionTicketMgrLimit = 0;
            try
            {
                redemptionTransactionTicketMgrLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "REDEMPTION_LIMIT_FOR_MANAGER_APPROVAL"));
            }
            catch (Exception ex)
            {
                //redemptionTransactionTicketMgrLimit = 0;
                log.Error(ex);
            }
            if (redemptionTransactionTicketMgrLimit > 0 && (GetRedeemedTickets() > redemptionTransactionTicketMgrLimit))
            {
                retVal = true;
            }
            log.LogMethodExit(retVal);
            return retVal;
        }

        /// <summary>
        /// GetManualTickets
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
        /// <summary>
        /// Get remaining ticket balance
        /// </summary>
        /// <returns>BalanceTicketsRemaining</returns>
        public int GetBalanceTicketsRemaining()
        {
            log.LogMethodEntry();
            log.LogMethodExit(balanceTicketsRemaining);
            return balanceTicketsRemaining;
        }
        /// <summary>
        ///Checks the ticket while loading tickets to card
        /// </summary>
        /// <param name="managerApprovalReceived">bool</param>
        /// <param name="ticketsToLoad">TicketsToLoad</param>
        public void LoadTicketLimitCheck(bool managerApprovalReceived, int ticketsToLoad)
        {
            log.LogMethodEntry(managerApprovalReceived, ticketsToLoad);
            DataAccessHandler dataAccessHandler = new DataAccessHandler();
            List<SqlParameter> sqlParameter = new List<SqlParameter>();
            sqlParameter.Add(new SqlParameter("@task_type", "LOADTICKETS"));
            string mgrApprovalRequired = dataAccessHandler.executeScalar("select requires_manager_approval from task_type where task_type = @task_type", sqlParameter.ToArray(), null).ToString();
            int mgrApprovalLimit = 0;
            try
            {
                mgrApprovalLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL"));
            }
            catch (Exception ex) { log.Error(ex); }
            if ((ticketsToLoad > mgrApprovalLimit && mgrApprovalLimit != 0 && managerApprovalReceived == false) || (mgrApprovalLimit == 0 && (mgrApprovalRequired == "Y" && managerApprovalReceived == false)))
            {
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 268));
            }

            int loadTicketLimit = 0;
            try
            {
                loadTicketLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "LOAD_TICKETS_LIMIT"));
            }
            catch (Exception ex) { log.Error(ex); }

            if (ticketsToLoad > loadTicketLimit)
            {
                //throw new Exception(MessageContainer.GetMessage(machineUserContext, MessageContainer.GetMessage(machineUserContext, 35, loadTicketLimit.ToString(), ParafaitDefaultContainer.GetParafaitDefault(machineUserContext, "REDEMPTION_TICKET_NAME_VARIANT"))));
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 2830, ticketsToLoad,
                                    ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "REDEMPTION_TICKET_NAME_VARIANT"), loadTicketLimit.ToString()));

            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Method to check is order reversed completely
        /// </summary>
        /// <param name="redemptionDTO"></param>
        /// <returns></returns>
        public bool IsOrderCompletelyReversed()
        {
            log.LogMethodEntry();
            bool completlyReversed = false;
            if (this.redemptionDTO != null &&
                this.redemptionDTO.RedemptionGiftsListDTO != null && 
                this.redemptionDTO.RedemptionGiftsListDTO.Any())
            {
                completlyReversed = true;
                foreach (RedemptionGiftsDTO redemptionGift in this.redemptionDTO.RedemptionGiftsListDTO)
                {
                    if (!redemptionGift.GiftLineIsReversed)
                    {
                        completlyReversed = false;
                        break;
                    }
                }
            }
            log.LogMethodExit(completlyReversed);
            return completlyReversed;
        }

        /// <summary>
        /// Create Reversal Redemption Order
        /// </summary>
        /// <param name="originalRedemptionDTO">RedemptionDTO</param>
        /// <param name="selectedGiftLinesForReversal">List<RedemptionGiftsDTO></param>
        /// <param name="totalTicketsForReversal">int</param>
        /// <param name="utilities">Utilities</param>
        /// <param name="trxRemarks">String</param>
        /// <param name="sqlTrx">SQLTransaction</param>
        /// <returns></returns>
        public void CreateReversalRedemption(RedemptionDTO originalRedemptionDTO, List<RedemptionGiftsDTO> selectedGiftLinesForReversal, int totalTicketsForReversal, Utilities utilities, string trxRemarks, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(originalRedemptionDTO, selectedGiftLinesForReversal, totalTicketsForReversal, utilities, trxRemarks, sqlTrx);
            RedemptionDTO reverseRedemptionDTO;
            if (originalRedemptionDTO.CardId > -1)
            {
                RedemptionCardsListBL redemptionCardsListBL = new RedemptionCardsListBL(machineUserContext);
                List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>(RedemptionCardsDTO.SearchByParameters.REDEMPTION_ID, originalRedemptionDTO.RedemptionId.ToString()));
                List<RedemptionCardsDTO> redemptionCardsDTOList = redemptionCardsListBL.GetRedemptionCardsDTOList(searchParams);
                if (redemptionCardsDTOList != null && redemptionCardsDTOList.Count > 0)
                {
                    this.RedemptionDTO.RedemptionCardsListDTO = redemptionCardsDTOList;
                }

                AccountBL cardAccount = new AccountBL(machineUserContext, originalRedemptionDTO.CardId, false, false, sqlTrx);
                if (cardAccount != null && cardAccount.GetAccountId() != -1)
                {
                    if (!cardAccount.IsActive())
                    {
                        throw new Exception(MessageContainerList.GetMessage(machineUserContext, 136));
                    }
                }
                else
                {
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 136));
                }

            }

            try
            {
                reverseRedemptionDTO = new RedemptionDTO(-1, originalRedemptionDTO.PrimaryCardNumber, null, (originalRedemptionDTO.CardId > -1 ? totalTicketsForReversal * -1 : (int?)null), DateTime.Now,
                                           originalRedemptionDTO.CardId, originalRedemptionDTO.RedemptionId, trxRemarks, originalRedemptionDTO.GraceTickets * -1, (originalRedemptionDTO.CardId > -1 ? (int?)null : totalTicketsForReversal * -1),
                                           null, machineUserContext.GetUserId(), machineUserContext.GetSiteId(), "", false, -1, "POS Redemption", RedemptionBL.GetNextSeqNo(machineUserContext.GetMachineId(), sqlTrx), DateTime.Now, DateTime.Now, DateTime.Now,
                                           RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString(), DateTime.Now, machineUserContext.GetUserId(), null, null, null, null, originalRedemptionDTO.CustomerName, machineUserContext.GetMachineId(),
                                           originalRedemptionDTO.CustomerId, null, null);

                if (reverseRedemptionDTO.CustomerId == -1 && reverseRedemptionDTO.CardId > -1)
                {
                    Card originalPrimaryCard = GetRedemptionPrimaryCard(sqlTrx);
                    if (originalPrimaryCard != null)
                    {
                        reverseRedemptionDTO.CustomerId = originalPrimaryCard.customer_id;
                    }
                }

                if (reverseRedemptionDTO.RedemptionGiftsListDTO == null)
                {
                    reverseRedemptionDTO.RedemptionGiftsListDTO = new List<RedemptionGiftsDTO>();
                }
                foreach (RedemptionGiftsDTO giftLineForReversal in selectedGiftLinesForReversal)
                {
                    RedemptionGiftsDTO reverseRedemptionGiftsDTO = new RedemptionGiftsDTO(-1, -1, giftLineForReversal.GiftCode, giftLineForReversal.ProductId, giftLineForReversal.LocationId, giftLineForReversal.Tickets * -1,
                                                                        giftLineForReversal.GraceTickets * -1, -1, machineUserContext.GetSiteId(), -1, false, "", machineUserContext.GetUserId(), DateTime.Now, DateTime.Now,
                                                                        machineUserContext.GetUserId(), giftLineForReversal.OriginalPriceInTickets * -1, giftLineForReversal.ProductName, giftLineForReversal.ProductDescription,
                                                                        false, giftLineForReversal.RedemptionGiftsId,1);
                    reverseRedemptionDTO.RedemptionGiftsListDTO.Add(reverseRedemptionGiftsDTO);
                }

                if (originalRedemptionDTO.CardId > -1)
                {
                    Loyalty Loyalty = new Loyalty(utilities);
                    Loyalty.CreateGenericCreditPlusLine(originalRedemptionDTO.CardId, "T", totalTicketsForReversal, false, 0, "N", machineUserContext.GetUserId(), "Redemption Reversal Tickets", sqlTrx, DateTime.MinValue, utilities.getServerTime());

                    RedemptionCardsDTO reverseRedemptionCardsDTO = new RedemptionCardsDTO(-1, -1, originalRedemptionDTO.PrimaryCardNumber, originalRedemptionDTO.CardId, totalTicketsForReversal * -1, -1,
                                                                       0, machineUserContext.GetSiteId(), "", false, -1, DateTime.Now, machineUserContext.GetUserId(), 0, null, DateTime.Now, machineUserContext.GetUserId(), null, null, null,null);

                    reverseRedemptionDTO.RedemptionCardsListDTO.Add(reverseRedemptionCardsDTO);
                }
                this.redemptionDTO = reverseRedemptionDTO;
                this.Save(sqlTrx);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Method to update customer id
        /// </summary>
        /// <param name="customerId"></param>
        public void UpdateCustomer(int customerId)
        {
            log.LogMethodEntry(customerId);
            if (this.redemptionDTO != null)
            {
                this.redemptionDTO.CustomerId = customerId;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Method to check whether order is having customer 
        /// </summary>
        /// <returns></returns>
        public bool HasCustomerDetails()
        {
            log.LogMethodEntry();
            bool hasCustomerId = false;
            if (this.redemptionDTO != null)
            {
                hasCustomerId = (this.redemptionDTO.CustomerId > -1);
            }
            log.LogMethodExit(hasCustomerId);
            return hasCustomerId;
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
                    ticketAllocationTupleList = GetRedemptionCardInfoForPrint(thisIsReversedRedemption);
                    break;
                case RedemptionDTO.RedemptionTicketSource.TICKET_RECEIPT:
                    ticketAllocationTupleList = GetRedemptionReceiptInfoForPrint(thisIsReversedRedemption);
                    break;
                case RedemptionDTO.RedemptionTicketSource.REDEMPTION_CURRENCY:
                    ticketAllocationTupleList = GetReedemptionCurrencyInfo(thisIsReversedRedemption);
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
                    ticketAllocationTupleList = GetRedemptionCurrencyRuleInfo(thisIsReversedRedemption);
                    break;
                default: break;

            }
            log.LogMethodExit(ticketAllocationTupleList);
            return ticketAllocationTupleList;

        }

        /// <summary>
        /// IsThisReversedRedemption
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// IsThisTurnInRedemption
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// GetRedemptionCardInfoForPrint
        /// </summary>
        /// <param name="thisIsReversedRedemption">thisIsReversedRedemption</param>
        /// <returns></returns>
        private List<Tuple<string, decimal, int, int>> GetRedemptionCardInfoForPrint(bool thisIsReversedRedemption)
        {
            log.LogMethodEntry(thisIsReversedRedemption);
            List<Tuple<string, decimal, int, int>> cardAllocationTupleList = new List<Tuple<string, decimal, int, int>>();
            if (this.redemptionDTO.RedemptionTicketAllocationListDTO != null
                && this.redemptionDTO.RedemptionTicketAllocationListDTO.Any())
            {
                LoadRedemptionCardDTOList();
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
        /// GetRedemptionReceiptInfoForPrint
        /// </summary>
        /// <param name="thisIsReversedRedemption">thisIsReversedRedemption</param>
        /// <returns></returns>
        private List<Tuple<string, decimal, int, int>> GetRedemptionReceiptInfoForPrint(bool thisIsReversedRedemption)
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
                        TicketReceiptList ticketReceiptList = new TicketReceiptList(machineUserContext);
                        List<TicketReceiptDTO> ticketReceiptDTOList = new List<TicketReceiptDTO>();
                        List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>
                                {
                                    new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_RECEIPT_IDS, manualTicketReceiptIds),
                                    new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, machineUserContext.GetSiteId().ToString()),
                                };
                        ticketReceiptDTOList = ticketReceiptList.GetAllTicketReceipt(searchParams);
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

        /// <summary>
        /// GetReedemptionCurrencyInfo
        /// </summary>
        /// <param name="thisIsReversedRedemption">thisIsReversedRedemption</param>
        /// <returns></returns>
        private List<Tuple<string, decimal, int, int>> GetReedemptionCurrencyInfo(bool thisIsReversedRedemption)
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
                        RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(machineUserContext);
                        List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = new List<RedemptionCurrencyDTO>();
                        List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParams = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>
                        {
                               new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_ID_LIST, redemptionCurrencyIds),
                               new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
                        };
                        redemptionCurrencyDTOList = redemptionCurrencyList.GetAllRedemptionCurrency(searchParams);
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

        /// <summary>
        /// GetRedemptionCurrencyRuleInfo
        /// </summary>
        /// <param name="thisIsReversedRedemption">thisIsReversedRedemption</param>
        /// <returns></returns>
        private List<Tuple<string, decimal, int, int>> GetRedemptionCurrencyRuleInfo(bool thisIsReversedRedemption)
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
                        RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL(machineUserContext);
                        List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList = new List<RedemptionCurrencyRuleDTO>();
                        List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> searchParams = new List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>>
                        {
                               new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID_LIST, redemptionCurrencyRuleIds),
                               new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
                        };
                        redemptionCurrencyRuleDTOList = redemptionCurrencyRuleListBL.GetAllRedemptionCurrencyRuleList(searchParams);
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

        /// <summary>
        /// Gets balance tickets details
        /// </summary>
        /// <param name="redemptionTicketSource"></param>
        /// <returns>receiptNumber/cardNumber and tickets value as KeyValuePair</returns>
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
                            TicketReceiptList ticketReceipt = new TicketReceiptList(machineUserContext);
                            List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>
                            {
                            new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()),
                            new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
                            };
                            List<TicketReceiptDTO> ticketReceiptDTOList = new List<TicketReceiptDTO>();
                            ticketReceiptDTOList = ticketReceipt.GetAllTicketReceipt(searchParams);
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
        /// <summary>
        /// Returns sum of ticket value from redemptionTicketAllocation
        /// </summary>
        /// <returns>Tickets sum</returns>
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

        private void LoadRedemptionTicketAllocationDTOList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (redemptionDTO != null && redemptionDTO.RedemptionTicketAllocationListDTO != null)//&& redemptionDTO.RedemptionTicketAllocationListDTO.Count == 0)
            {
                List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList = new List<RedemptionTicketAllocationDTO>();
                RedemptionTicketAllocationListBL redemptionTicketAllocationBL = new RedemptionTicketAllocationListBL(machineUserContext);
                List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()),
                    new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
                };
                redemptionTicketAllocationDTOList = redemptionTicketAllocationBL.GetRedemptionTicketAllocationDTOList(searchParams, sqlTransaction);
                redemptionDTO.RedemptionTicketAllocationListDTO = redemptionTicketAllocationDTOList;
            }
            log.LogMethodExit();
        }

        private void LoadRedemptionCardDTOList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (this.redemptionDTO.RedemptionCardsListDTO == null || this.redemptionDTO.RedemptionCardsListDTO.Any() == false)
            {
                RedemptionCardsListBL redemptionCardsListBL = new RedemptionCardsListBL(machineUserContext);

                List<RedemptionCardsDTO> redemptionCardsDTOList = new List<RedemptionCardsDTO>();
                List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>>
                        {
                            new KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>(RedemptionCardsDTO.SearchByParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()),
                            new KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>(RedemptionCardsDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
                        };
                redemptionCardsDTOList = redemptionCardsListBL.GetRedemptionCardsDTOList(searchParams, sqlTransaction);
                this.redemptionDTO.RedemptionCardsListDTO = redemptionCardsDTOList;
            }
            log.LogMethodExit();
        }

        private string GetMaskedReceiptText(string receiptNumber)
        {
            log.LogMethodEntry(receiptNumber);
            string maskedReceiptNumber = string.Empty;
            maskedReceiptNumber = receiptNumber.Substring(0, 4) + "***" + receiptNumber.Substring(receiptNumber.Length - 5);
            log.LogMethodExit(maskedReceiptNumber);
            return maskedReceiptNumber;
        }

        public void ManualTicketLimitChecks(bool managerApprovalReceived, int manualTicketsToLoad)
        {
            log.LogMethodEntry(managerApprovalReceived, manualTicketsToLoad);
            try
            {
                PerDayLimitCheckForManualTickets(manualTicketsToLoad);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            int managerApprovalLimit = ParafaitDefaultContainerList.GetParafaitDefault<int>(machineUserContext, "ADD_TICKET_LIMIT_FOR_MANAGER_APPROVAL_REDEMPTION");
            if ((manualTicketsToLoad > managerApprovalLimit && managerApprovalLimit != 0 && managerApprovalReceived == false))
            {
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 268));
            }

            if (manualTicketsToLoad > ParafaitDefaultContainerList.GetParafaitDefault<int>(machineUserContext, "MAX_MANUAL_TICKETS_PER_REDEMPTION"))
            {
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 2495, ParafaitDefaultContainerList.GetParafaitDefault<int>(machineUserContext, "MAX_MANUAL_TICKETS_PER_REDEMPTION")));
            }
            log.LogMethodExit();
        }

        public void PerDayLimitCheckForManualTickets(int manualTickets)
        {
            log.LogMethodEntry(manualTickets);
            try
            {
                RedemptionTicketAllocationListBL redemptionTicketAllocationListBL = new RedemptionTicketAllocationListBL(machineUserContext);
                if (!redemptionTicketAllocationListBL.CanAddManualTicketForTheDay(machineUserContext.GetUserId(), manualTickets))
                {
                    int remainingManualTicketLimit = redemptionTicketAllocationListBL.GetRemainingAddManualTicketLimitForTheDay(machineUserContext.GetUserId());
                    if (remainingManualTicketLimit > 0)
                    {
                        throw new Exception(MessageContainerList.GetMessage(machineUserContext, 2488, remainingManualTicketLimit));/*"You can add only " + remainingManualTicketLimit + " more manual tickets";*/
                    }
                    else
                    {
                        throw new Exception(MessageContainerList.GetMessage(machineUserContext, 2489));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        private void LoadChildDTOs(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                LoadRedemptionGiftDTOList(sqlTransaction);
                LoadRedemptionCardDTOList(sqlTransaction);
                LoadRedemptionTicketAllocationDTOList(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        private void LoadRedemptionGiftDTOList(SqlTransaction sqlTransaction)
        {

            log.LogMethodEntry(sqlTransaction);
            if (this.redemptionDTO.RedemptionGiftsListDTO == null || this.redemptionDTO.RedemptionGiftsListDTO.Any() == false)
            {
                RedemptionGiftsListBL redemptionGiftsListBL = new RedemptionGiftsListBL(machineUserContext);

                List<RedemptionGiftsDTO> redemptionGiftsDTOList = new List<RedemptionGiftsDTO>();
                List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>>
                        {
                            new KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>(RedemptionGiftsDTO.SearchByParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()),
                            new KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>(RedemptionGiftsDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
                        };
                redemptionGiftsDTOList = redemptionGiftsListBL.GetRedemptionGiftsDTOList(searchParams, sqlTransaction);
                this.redemptionDTO.RedemptionGiftsListDTO = redemptionGiftsDTOList;
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of redemption
    /// </summary>
    public class RedemptionListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the redemption list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>redemptionDTOs</returns>
        public List<RedemptionDTO> GetRedemptionDTOList(List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            RedemptionDataHandler redemptionDataHandler = new RedemptionDataHandler(sqlTransaction);
            List<RedemptionDTO> redemptionDTOs = new List<RedemptionDTO>();
            redemptionDTOs = redemptionDataHandler.GetRedemptionDTOList(searchParameters);
            log.LogMethodExit(redemptionDTOs);
            return redemptionDTOs;
        }


    }
}

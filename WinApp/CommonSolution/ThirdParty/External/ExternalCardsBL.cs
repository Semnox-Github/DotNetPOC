/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the card details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   Abhishek           Created : External  REST API.
 ***************************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.ThirdParty.External
{
    /// <summary>
    /// Bussiness logic of the ExternalCardsListBL class
    /// </summary>
    public class ExternalCardsBL
    {
        //private List<ExternalCardsDTO> externalCardsDTOList = new List<ExternalCardsDTO>();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private AccountDTOCollection accountDTOCollection;
        private int accountId;

        /// <summary>
        /// Constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        private ExternalCardsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates TaxStructureBL object using the TaxStructureDTO 
        /// </summary>
        /// <param name="executionContex"></param>
        /// <param name="accountDTOCollection"></param>
        public ExternalCardsBL(ExecutionContext executionContext, AccountDTOCollection accountDTOCollection)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountDTOCollection);
            this.accountDTOCollection = accountDTOCollection;
            log.LogMethodExit();
        }

        public ExternalCardsBL(ExecutionContext executionContext, int accountId)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountId);
            this.accountId = accountId;
            log.LogMethodExit();
        }

        public void LinkCardToCustomer(int customerId = -1, SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(customerId);
            AccountBL existingAccountBL = new AccountBL(executionContext, accountId, false, false, sqltransaction);
            CustomerBL customerBL = new CustomerBL(executionContext, customerId);
            if (customerBL.CustomerDTO == null || customerBL.CustomerDTO.Id < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 632);
                log.Error("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            if (existingAccountBL.AccountDTO.CustomerId == -1)
            {
                existingAccountBL.AccountDTO.CustomerId = customerId;
                log.Debug("link card to customer:");
                AccountBL accountBL = new AccountBL(executionContext, existingAccountBL.AccountDTO);
                accountBL.Save(sqltransaction);
            }
            else if (existingAccountBL.AccountDTO.CustomerId != customerId)
            {
                log.Debug("The card is already linked to another customer.");
                throw new ValidationException("The card is already linked to another customer." );
            }
            log.LogMethodExit();
        }


        public List<ExternalCardsDTO> GetAllExternalCardsDTOList()
        {
            log.LogMethodEntry();
            List<ExternalCardsDTO> externalCardsDTOList = new List<ExternalCardsDTO>();
            ExternalCardsDTO externalCardsDTO = new ExternalCardsDTO();
            if (accountDTOCollection.data != null && accountDTOCollection.data.Any())
            {
                foreach (AccountDTO accountDTO in accountDTOCollection.data)
                {
                    List<CreditPlusDTO> creditPlusDTOList = new List<CreditPlusDTO>();
                    if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any())
                    {
                        foreach (AccountCreditPlusDTO accountCreditPlusDTO in accountDTO.AccountCreditPlusDTOList)
                        {
                            AvaialbleOn creditplusAvaialbleOn = new AvaialbleOn(accountCreditPlusDTO.Monday, accountCreditPlusDTO.Tuesday, accountCreditPlusDTO.Wednesday,
                                accountCreditPlusDTO.Thursday, accountCreditPlusDTO.Friday, accountCreditPlusDTO.Saturday, accountCreditPlusDTO.Sunday);
                            CreditPlusDTO creditPlusDTO = new CreditPlusDTO(accountCreditPlusDTO.AccountCreditPlusId, accountCreditPlusDTO.CreditPlusType.ToString(),
                                accountCreditPlusDTO.CreditPlus, accountCreditPlusDTO.CreditPlusBalance, accountCreditPlusDTO.PeriodFrom, accountCreditPlusDTO.PeriodTo,
                                accountCreditPlusDTO.ExtendOnReload, accountCreditPlusDTO.Refundable, accountCreditPlusDTO.TimeFrom, accountCreditPlusDTO.TimeTo,
                                creditplusAvaialbleOn, accountCreditPlusDTO.TicketAllowed, accountCreditPlusDTO.PauseAllowed, accountCreditPlusDTO.Remarks, accountCreditPlusDTO.ExpireWithMembership);
                            creditPlusDTOList.Add(creditPlusDTO);
                        }
                    }
                    List<CardGameDTO> cardGameDTOList = new List<CardGameDTO>();
                    if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any())
                    {
                        foreach (AccountGameDTO accountGameDTO in accountDTO.AccountGameDTOList)
                        {
                            AvaialbleOn gamesAvaialbleOn = new AvaialbleOn(accountGameDTO.Monday, accountGameDTO.Tuesday, accountGameDTO.Wednesday,
                                accountGameDTO.Thursday, accountGameDTO.Friday, accountGameDTO.Saturday, accountGameDTO.Sunday);
                            CardGameDTO cardGameDTO = new CardGameDTO(accountGameDTO.AccountGameId, accountGameDTO.GameProfileId, accountGameDTO.GameId,
                                accountGameDTO.Quantity, accountGameDTO.Frequency, accountGameDTO.BalanceGames, accountGameDTO.LastPlayedTime, accountGameDTO.FromDate,
                                accountGameDTO.ExpiryDate, accountGameDTO.EntitlementType, accountGameDTO.TicketAllowed, accountGameDTO.OptionalAttribute, gamesAvaialbleOn, accountGameDTO.ExpireWithMembership,
                                accountGameDTO.ValidityStatus.ToString());
                            cardGameDTOList.Add(cardGameDTO);
                        }
                    }
                    List<CardDiscount> cardDiscountDTOList = new List<CardDiscount>();
                    if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Any())
                    {
                        foreach (AccountDiscountDTO accountDiscountDTO in accountDTO.AccountDiscountDTOList)
                        {
                            CardDiscount cardDiscount = new CardDiscount(accountDiscountDTO.AccountDiscountId, accountDiscountDTO.DiscountId,
                                accountDiscountDTO.ExpiryDate, accountDiscountDTO.ExpireWithMembership, accountDiscountDTO.ValidityStatus.ToString());
                            cardDiscountDTOList.Add(cardDiscount);
                        }
                    }
                    CardSummaryDTO cardSummaryDTO = new CardSummaryDTO();
                    if (accountDTO.AccountSummaryDTO != null)
                    {
                        cardSummaryDTO = new CardSummaryDTO(accountDTO.AccountSummaryDTO.CreditPlusCardBalance, accountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits,
                        accountDTO.AccountSummaryDTO.CreditPlusItemPurchase, accountDTO.AccountSummaryDTO.CreditPlusBonus, accountDTO.AccountSummaryDTO.CreditPlusLoyaltyPoints,
                        accountDTO.AccountSummaryDTO.CreditPlusTickets, accountDTO.AccountSummaryDTO.CreditPlusVirtualPoints, accountDTO.AccountSummaryDTO.CreditPlusTime,
                        accountDTO.AccountSummaryDTO.CreditPlusRefundableBalance, accountDTO.AccountSummaryDTO.RedeemableCreditPlusLoyaltyPoints,
                        accountDTO.AccountSummaryDTO.AccountGameBalance, accountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance, accountDTO.AccountSummaryDTO.TotalBonusBalance,
                        accountDTO.AccountSummaryDTO.TotalCourtesyBalance, accountDTO.AccountSummaryDTO.TotalTimeBalance, accountDTO.AccountSummaryDTO.TotalVirtualPointBalance,
                        accountDTO.AccountSummaryDTO.TotalGamesBalance, accountDTO.AccountSummaryDTO.TotalTicketsBalance, accountDTO.AccountSummaryDTO.TotalLoyaltyPointBalance);
                    }
                    string accountIdentifier = accountDTO.AccountIdentifier;
                    if (string.IsNullOrEmpty(accountDTO.AccountIdentifier) && accountDTO.CustomerId != -1)
                    {
                        CustomerBL customerBL = new CustomerBL(executionContext, accountDTO.CustomerId);
                        accountIdentifier = customerBL.CustomerDTO.ProfileDTO.NickName;
                    }
                    externalCardsDTO = new ExternalCardsDTO(accountDTO.AccountId, accountDTO.TagNumber, accountDTO.CustomerName,
                    accountDTO.IssueDate, accountDTO.CreditsPlayed, accountDTO.RealTicketMode, accountDTO.VipCustomer, accountDTO.TicketAllowed
                    , accountDTO.TechnicianCard, accountDTO.TimerResetCard, accountDTO.TechGames, accountDTO.ValidFlag, accountDTO.RefundFlag,
                    accountDTO.RefundAmount, accountDTO.RefundDate, accountDTO.ExpiryDate, accountDTO.StartTime, accountDTO.LastPlayedTime,
                    accountDTO.PrimaryAccount, accountDTO.CustomerId, accountIdentifier, accountDTO.SiteId, creditPlusDTOList, cardGameDTOList, cardDiscountDTOList, cardSummaryDTO,
                    accountDTO.TotalCreditPlusBalance, accountDTO.TotalCreditsBalance, accountDTO.TotalBonusBalance, accountDTO.TotalCourtesyBalance,
                    accountDTO.TotalTimeBalance, accountDTO.TotalGamesBalance, accountDTO.TotalTicketsBalance, accountDTO.TotalVirtualPointBalance);
                    externalCardsDTOList.Add(externalCardsDTO);
                }
            }
            else
            {
                log.Error("Card does not exist. Enter a valid card number");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 776));
            }
            log.LogMethodExit(externalCardsDTOList);
            return externalCardsDTOList;
        }
    }

    public class ExternalCardsListBL
    {
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ExternalCardsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

       
    }

}

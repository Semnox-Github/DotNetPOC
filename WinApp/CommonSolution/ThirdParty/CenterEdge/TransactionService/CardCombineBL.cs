/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - CardCombineBL class - This is class does the card consolidation task for CE
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/

using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using System.Linq;
using System.Data.SqlClient;

namespace Semnox.Parafait.ThirdParty.CenterEdge.TransactionService
{
    public class CardCombineBL
    {

        private readonly ExecutionContext executionContext;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TransactionServiceDTO transactionServiceDTO;

        private CardCombineBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor of CardCombineBL class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public CardCombineBL(ExecutionContext executionContext, TransactionServiceDTO transactionServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, transactionServiceDTO);
            AccountBL accountBL = new AccountBL(executionContext, transactionServiceDTO.SourceAccountDTO.TagNumber, true, true);
            if (accountBL.AccountDTO == null || accountBL.AccountDTO.ValidFlag == false)
            {
                log.Debug("Card Not Found");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Card Not Found"));
            }
            AccountBuilderBL accountBuilderBL = new AccountBuilderBL(executionContext);
            accountBuilderBL.Build(accountBL.AccountDTO, true);
            transactionServiceDTO.SourceAccountDTO = accountBL.AccountDTO;
            this.transactionServiceDTO = transactionServiceDTO;
        }


        public Card ConsolidateCards()
        {
            try
            {
                log.LogMethodEntry();
                Card ceCardResponse = new Card();
                //AccountDTO destinationAccountDTO = new AccountDTO();
            
                using (ParafaitDBTransaction trx = new ParafaitDBTransaction())
                {
                    trx.BeginTransaction();
                    Validate(transactionServiceDTO, trx.SQLTrx);
                    AccountBL accountBL = new AccountBL(executionContext, transactionServiceDTO.SourceAccountDTO.TagNumber, false, false, trx.SQLTrx);
                    transactionServiceDTO.AccountDTOList.Add(accountBL.AccountDTO); // Add first card - source
                    accountBL = new AccountBL(executionContext, transactionServiceDTO.DestinationAccountDTO.TagNumber, false, false, trx.SQLTrx);
                    AccountDTO accountDTO = null;
                    if (accountBL.AccountDTO == null) // if not exists then create 
                    {
                        TransactionBL transactionBL = new TransactionBL(executionContext);
                        accountDTO = transactionBL.Activate(transactionServiceDTO.DestinationAccountDTO.TagNumber, trx.SQLTrx);
                        transactionServiceDTO.AccountDTOList.Add(accountDTO);  // Add second card - Destination
                    }
                    else
                    {
                        transactionServiceDTO.AccountDTOList.Add(accountBL.AccountDTO);  // Add second card - Destination
                    }
                    transactionServiceDTO.Remarks = "CenterEdge card combine task";
                    ConsolidateCardBL consolidateCardBL = new ConsolidateCardBL(executionContext, transactionServiceDTO);
                    consolidateCardBL.CardConsolidate(trx.SQLTrx);

                    // CE Card response build 
                    CardBL cardBL = new CardBL(executionContext, transactionServiceDTO.DestinationAccountDTO.TagNumber, trx.SQLTrx);
                    ceCardResponse = cardBL.GetDetails(trx.SQLTrx);
                    trx.EndTransaction();
                }
                log.LogMethodExit(ceCardResponse);
                return ceCardResponse;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private void Validate(TransactionServiceDTO transactionServiceDTO, SqlTransaction sQLTrx)
        {
            log.LogMethodEntry(transactionServiceDTO, sQLTrx);
            TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
            if (tagNumberParser.IsValid(transactionServiceDTO.SourceAccountDTO.TagNumber) == false)
            {
                string errorMessage = tagNumberParser.Validate(transactionServiceDTO.SourceAccountDTO.TagNumber);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage, "CardNumber", "CardNumber", errorMessage);
            }
            if (tagNumberParser.IsValid(transactionServiceDTO.DestinationAccountDTO.TagNumber) == false)
            {
                string errorMessage = tagNumberParser.Validate(transactionServiceDTO.DestinationAccountDTO.TagNumber);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage, "CardNumber", "CardNumber", errorMessage);
            }

            // Check how many time plays are there in source card
            AccountBL accountBL = new AccountBL(executionContext,transactionServiceDTO.SourceAccountDTO.TagNumber, true, true, sQLTrx);
            int sourceTimePlayCount = 0;
            int destinationTimePlayCount = 0;
            if (accountBL.AccountDTO == null)
            {
                log.Debug("Source card is not found :" + transactionServiceDTO.SourceAccountDTO.TagNumber);
                log.Debug("Card Not Found");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Card Not Found"));
            }
            if (accountBL.AccountDTO.AccountCreditPlusDTOList != null
                   && accountBL.AccountDTO.AccountCreditPlusDTOList.Any()
                   && accountBL.AccountDTO.AccountCreditPlusDTOList.Exists(x => x.CreditPlusType == CreditPlusType.TIME))
            {
                sourceTimePlayCount = accountBL.AccountDTO.AccountCreditPlusDTOList.
                                                       Where(x => x.CreditPlusType == CreditPlusType.TIME &&
                                                       x.CreditPlusBalance > 0 &&
                                                       x.ValidityStatus == AccountDTO.AccountValidityStatus.Valid)
                                                       .Count();
                log.Debug("sourceTimePlayCount :" + sourceTimePlayCount);
            }
            accountBL = new AccountBL(executionContext, transactionServiceDTO.DestinationAccountDTO.TagNumber, true, true,sQLTrx);
            if (accountBL.AccountDTO != null && accountBL.AccountDTO.ValidFlag == false)
            {
                log.Debug("Card Not Found");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Card Not Found"));
            }
            if (accountBL.AccountDTO != null &&
                  accountBL.AccountDTO.AccountCreditPlusDTOList != null
                  && accountBL.AccountDTO.AccountCreditPlusDTOList.Any()
                  && accountBL.AccountDTO.AccountCreditPlusDTOList.Exists(x => x.CreditPlusType == CreditPlusType.TIME))
            {
                destinationTimePlayCount = accountBL.AccountDTO.AccountCreditPlusDTOList.Where(x => x.CreditPlusType == CreditPlusType.TIME
                                               && x.CreditPlusBalance > 0
                                               && x.ValidityStatus == AccountDTO.AccountValidityStatus.Valid)
                                               .Count();
                log.Debug("destinationTimePlayCount :" + destinationTimePlayCount);
            }
            CapabilitiesBL capabilitiesBL = CapabilitiesBL.Instance;
            int maximumTimePlaysPerCard = capabilitiesBL.GetCapabilityDTO.timePlay.maximumTimePlaysPerCard;
            if (destinationTimePlayCount + sourceTimePlayCount > maximumTimePlaysPerCard)
            {
                log.Debug("destinationTimePlayCount + sourceTimePlayCount > maximumTimePlaysPerCard");
                log.Debug(MessageContainerList.GetMessage(executionContext, "maximum Time Plays Per Card is " + maximumTimePlaysPerCard));
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Maximum Time Plays Per Card is " + maximumTimePlaysPerCard));
            }
            log.LogMethodExit();
        }
    }
}

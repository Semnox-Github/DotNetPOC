/********************************************************************************************
 * Project Name - ThirdParty
 * Description  - TransactionBL - Handles the center edge transaction details - Save/Update and Get
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *0.0        28-Sept-2020           Girish Kundar          Created 
 *2.140.8    03-Oct-2023            Abhishek               Modified : In Get card transaction, 
                                                            return the product id of privilege product.
 *2.151.0    11-Oct-2023            Abhishek               Modified : The card creation through transaction.   
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Game;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using Semnox.Parafait.ThirdParty.CenterEdge.TransactionService;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using System.Data;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public class TransactionBL
    {
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CETransactionServiceDTO ceTransactionServiceDTO;
        private TransactionDTO postTransactionDTO;

        /// <summary>
        /// Parameterized constructor of TransactionBL class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public TransactionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public TransactionBL(ExecutionContext executionContext, CETransactionServiceDTO ceTransactionServiceDTO)
          : this(executionContext)
        {
            log.LogMethodEntry(executionContext, ceTransactionServiceDTO);
            this.ceTransactionServiceDTO = ceTransactionServiceDTO;
            log.LogMethodExit();
        }
        public TransactionBL(ExecutionContext executionContext, TransactionDTO postTransactionDTO)
          : this(executionContext)
        {
            log.LogMethodEntry(executionContext, ceTransactionServiceDTO);
            this.postTransactionDTO = postTransactionDTO;
            log.LogMethodExit();
        }
        public TransactionDTO PostTransactionDTO
        {
            get { return postTransactionDTO; }
        }
        /// <summary>
        /// GetTransactionDetails
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public CardTransaction GetTransactionDetails(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            CardTransaction cardTransaction = new CardTransaction();
            cardTransaction.transactions = new List<TransactionDTO>();
            TransactionList ceTransaction = new TransactionList(executionContext);
            if (postTransactionDTO != null)
            {
                if (postTransactionDTO.type == TransactionTypes.gamePlay.ToString())
                {
                    postTransactionDTO.adjustments = null;
                    cardTransaction.transactions.AddRange(ceTransaction.GetGamePlayTransactions(postTransactionDTO.cardNumber, 0, postTransactionDTO.id, 0, 100));
                }
                else
                {
                    AccountBL accountBL = new AccountBL(executionContext, postTransactionDTO.cardNumber, true, true, sqlTransaction);
                    if (accountBL.AccountDTO.AccountCreditPlusDTOList == null)
                    {
                        accountBL.AccountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                    }
                    if (accountBL.AccountDTO.AccountGameDTOList == null)
                    {
                        accountBL.AccountDTO.AccountGameDTOList = new List<AccountGameDTO>();
                    }
                    cardTransaction.transactions.AddRange(ceTransaction.GetAdjustmentTransactions(new List<int> { postTransactionDTO.id }, (accountBL.AccountDTO)));
                }

            }
            cardTransaction.totalCount = cardTransaction.transactions.Count;
            log.LogMethodExit(cardTransaction);
            log.LogVariableState("cardTransaction", cardTransaction);
            return cardTransaction;
        }


        public void Save()
        {
            log.LogMethodEntry();
            Utilities utilities = GetUtility();
            Validate();
            using (ParafaitDBTransaction trx = new ParafaitDBTransaction())
            {
                trx.BeginTransaction();
                int refundProductId = -1;
                if (postTransactionDTO != null && postTransactionDTO.adjustments != null && postTransactionDTO.adjustments.Any())
                {
                    if (postTransactionDTO.type == TransactionTypes.adjustment.ToString())
                    {
                        Transaction.Transaction transaction = new Parafait.Transaction.Transaction(utilities);
                        if (transaction.TransactionPaymentsDTOList == null)
                        {
                            transaction.TransactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
                        }
                        // Add the CE transaction Id to trx_heder table
                        transaction.externalSystemReference = postTransactionDTO.id > 0 ? postTransactionDTO.id.ToString() : string.Empty;

                        // Add the CE transaction remarks to trx_heder table
                        transaction.Remarks = "CenterEdge Transactions" + ServerDateTime.Now;
                        TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
                        TagNumber tagNumber;
                        if (tagNumberParser.TryParse(postTransactionDTO.cardNumber, out tagNumber) == false)
                        {
                            string message = tagNumberParser.Validate(postTransactionDTO.cardNumber);
                            log.LogMethodExit(null, "Throwing Exception- " + message);
                            throw new Exception(message);
                        }

                        refundProductId = GetRefundProductId();
                        var orderByAdjustmentTypeList = postTransactionDTO.adjustments.OrderBy(x => x.type).ToList(); // Make sure transaction is saved at the end
                        AccountBL accountBL = new AccountBL(executionContext, postTransactionDTO.cardNumber, false, false, trx.SQLTrx);
                        AccountDTO accountDTO = accountBL.AccountDTO;
                        if (accountDTO == null)
                        {
                            log.Debug("Account does not exists .Creating new Account");
                            //DateTime currentTime = ServerDateTime.Now;
                            //AccountDTO accountDTO = new AccountDTO(-1, postTransactionDTO.cardNumber, "", currentTime, null, false, null, null, true,
                            //                            null, null, null, null, null, null, -1, null, true, true, false, null, null, "N", null,
                            //                            false, null, -1, null, null, -1, null, null, false, -1, "", "",
                            //                            currentTime, executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(),
                            //                            currentTime);
                            //accountBL = new AccountBL(executionContext, accountDTO);
                            //accountBL.Save(trx.SQLTrx);
                            TransactionBL transactionBL = new TransactionBL(executionContext);
                            accountDTO = transactionBL.Activate(postTransactionDTO.cardNumber, trx.SQLTrx);
                            //accountBL = new AccountBL(executionContext, postTransactionDTO.cardNumber, false, false);
                        }
                        Transaction.Card cards = new Parafait.Transaction.Card(accountDTO.AccountId, executionContext.GetUserId(), utilities, trx.SQLTrx);

                        foreach (Adjustments adjustment in orderByAdjustmentTypeList)
                        {
                            CreateTransactionLine(cards, adjustment, transaction, utilities, trx.SQLTrx, refundProductId);
                        }
                        PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                        if (paymentModeDTOList != null)
                        {
                            TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, transaction.Transaction_Amount,
                                                                                                "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", ServerDateTime.Now,
                                                                                                utilities.ParafaitEnv.LoginID, -1, null, 0, -1, executionContext.POSMachineName, -1, "", null);
                            trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];

                            transaction.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                        }
                        string transactionMessage = string.Empty;

                        int trxId = transaction.SaveTransacation(trx.SQLTrx, ref transactionMessage);
                        if (trxId != 0)
                        {
                            log.Debug("Failed to create transaction ");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction"));
                        }
                        transaction.InsertTrxLogs(transaction.Trx_id, -1, executionContext.GetUserId(), "Save Transacation", "CenterEdge save transaction status : " + transaction.Trx_id, trx.SQLTrx);
                        log.Debug("SaveTransaction" + transactionMessage);
                        if (postTransactionDTO.id <= 0)
                        {
                            postTransactionDTO.id = transaction.Trx_id;
                            Parafait.Transaction.TransactionBL transactionBL = new Transaction.TransactionBL(executionContext, transaction.Trx_id, trx.SQLTrx);
                            transactionBL.TransactionDTO.ExternalSystemReference = transaction.Trx_id.ToString();
                            transactionBL.Save(trx.SQLTrx);
                        }
                    }
                }

                if (postTransactionDTO != null && postTransactionDTO.type == TransactionTypes.gamePlay.ToString())
                {
                    CreateGamePlayTransactionLine(postTransactionDTO, utilities, trx.SQLTrx);
                }
                trx.EndTransaction();
                log.LogMethodExit();
            }
        }

        internal int GetRefundProductId()
        {
            log.LogMethodEntry();
            int refundProductId = -1;
            ProductsList productsList = new ProductsList(executionContext);
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, ProductTypeValues.REFUND));
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
            List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParams);
            if (productsDTOList != null && productsDTOList.Any())
            {
                refundProductId = productsDTOList[0].ProductId;
            }
            log.LogMethodExit(refundProductId);
            return refundProductId;
        }

        private void Validate()
        {
            log.LogMethodEntry();
            string[] allowedAdjCombinations = Enum.GetNames(typeof(AdjustmentTypes));
            // BulkIssue Data
            if (ceTransactionServiceDTO != null && ceTransactionServiceDTO.adjustments != null && ceTransactionServiceDTO.adjustments.Any())
            {
                int sourceTimePlayCount = ceTransactionServiceDTO.adjustments.Where(x => x.type == AdjustmentTypes.addMinutes.ToString()
                                                                         && x.minutes > 0
                                                                         && x.groupId > 0).Count();
                log.Debug("sourceTimePlayCount Bulk Issue: " + sourceTimePlayCount);
                CapabilitiesBL capabilitiesBL = CapabilitiesBL.Instance;
                int maximumTimePlaysPerCard = capabilitiesBL.GetCapabilityDTO.timePlay.maximumTimePlaysPerCard;
                if (sourceTimePlayCount > maximumTimePlaysPerCard)
                {
                    log.Debug("sourceTimePlayCount > maximumTimePlaysPerCard");
                    log.Debug(MessageContainerList.GetMessage(executionContext, "maximum Time Plays Per Card is " + maximumTimePlaysPerCard));
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Maximum Time Plays Per Card is " + maximumTimePlaysPerCard));
                }
                var list = ceTransactionServiceDTO.adjustments.Where(x => x.type == AdjustmentTypes.removeMinutes.ToString()
                                                                          || x.type == AdjustmentTypes.removePrivilege.ToString()
                                                                          || x.type == AdjustmentTypes.removeValue.ToString()).ToList();
                if (list != null && list.Any())
                {
                    log.Error("Invalid Input");
                    throw new ValidationException("Invalid Input");
                }
                foreach (Adjustments adjustments in ceTransactionServiceDTO.adjustments)
                {
                    if (allowedAdjCombinations.Contains(adjustments.type) == false)
                    {
                        log.Debug("adjustments type  Not Found");
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Adjustments type  Not Found"));
                    }
                    if (adjustments.type == AdjustmentTypes.addMinutes.ToString())
                    {
                        if (adjustments.minutes < 0)
                        {
                            log.Debug("minutes Value cannot be negative");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 745));
                        }
                        if (adjustments.groupId.HasValue && adjustments.groupId <= 0)
                        {
                            log.Debug("groupId Value cannot be negative");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 745));
                        }
                    }
                    if (adjustments.type == AdjustmentTypes.addValue.ToString()
                        || adjustments.type == AdjustmentTypes.removeValue.ToString())
                    {
                        if (adjustments.amount.bonusPoints < 0
                                  || adjustments.amount.regularPoints < 0
                                  || adjustments.amount.redemptionTickets < 0)
                        {
                            log.Debug(" bonusPoints Value cannot be negative");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 745));
                        }
                        ValidateMaxDecimalPoints(adjustments);
                    }
                    if (adjustments.type == AdjustmentTypes.addPrivilege.ToString())
                    {
                        if (adjustments.count < 0)
                        {
                            log.Debug("count Value cannot be negative");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 745));
                        }
                        if (adjustments.groupId.HasValue && adjustments.groupId <= 0)
                        {
                            log.Debug("Value cannot be negative");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 745));
                        }
                    }
                }
            }
            if (postTransactionDTO != null && postTransactionDTO.adjustments != null && postTransactionDTO.adjustments.Any())
            {
                foreach (Adjustments adjustments in postTransactionDTO.adjustments)
                {
                    if (allowedAdjCombinations.Contains(adjustments.type) == false)
                    {
                        log.Debug("adjustments type  Not Found");
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Adjustments type  Not Found"));
                    }

                }
            }

            if (postTransactionDTO != null)
            {
                string[] allowedTransactions = Enum.GetNames(typeof(TransactionTypes));

                if (allowedTransactions.Contains(postTransactionDTO.type) == false)
                {
                    log.Debug("transactions type  Not Found");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Transactions type  Not Found"));
                }
                if (postTransactionDTO.adjustments != null && postTransactionDTO.adjustments.Any())
                {
                    foreach (Adjustments adjustments in postTransactionDTO.adjustments)
                    {
                        if (allowedAdjCombinations.Contains(adjustments.type) == false)
                        {
                            log.Debug("adjustments type  Not Found");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Adjustments type  Not Found"));
                        }
                        if (adjustments.amount != null && adjustments.amount.redemptionTickets > 0)
                        {
                            decimal value = adjustments.amount.redemptionTickets - Convert.ToInt32(adjustments.amount.redemptionTickets);
                            log.Debug("Ticket value : " + value);

                            if (value != 0)
                            {
                                log.Error("Invalid Input");
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Invalid ticket value"));
                            }
                        }
                        if (adjustments.type == AdjustmentTypes.addValue.ToString()
                             || adjustments.type == AdjustmentTypes.removeValue.ToString())
                        {
                            if (adjustments.amount.bonusPoints < 0
                                      || adjustments.amount.regularPoints < 0
                                      || adjustments.amount.redemptionTickets < 0)
                            {
                                log.Debug(" bonusPoints Value cannot be negative");
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 745));
                            }
                            if (adjustments.amount.bonusPoints == 0
                                      && adjustments.amount.regularPoints == 0
                                      && adjustments.amount.redemptionTickets == 0)
                            {
                                log.Debug("Not a valid input");
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter valid input"));
                            }
                            ValidateMaxDecimalPoints(adjustments);
                        }
                        if (adjustments.type == AdjustmentTypes.addMinutes.ToString()
                            || adjustments.type == AdjustmentTypes.removeMinutes.ToString())
                        {
                            if (adjustments.minutes < 0)
                            {
                                log.Debug("minutes Value cannot be negative");
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 745));
                            }
                            if (adjustments.minutes == 0)
                            {
                                log.Debug("Please enter valid input");
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter valid input"));
                            }
                            if (adjustments.groupId.HasValue && adjustments.groupId <= 0)
                            {
                                log.Debug("groupId Value cannot be negative");
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 745));
                            }
                            // Check how many time plays are there in source card
                            ValidateMaxTimePlays(postTransactionDTO.cardNumber);
                        }
                        if (adjustments.type == AdjustmentTypes.addPrivilege.ToString()
                            || adjustments.type == AdjustmentTypes.removeMinutes.ToString())
                        {
                            if (adjustments.count < 0)
                            {
                                log.Debug("count Value cannot be negative");
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 745));
                            }
                            if (adjustments.count == 0)
                            {
                                log.Debug("Please enter valid input");
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter valid input"));
                            }
                            if (adjustments.groupId.HasValue && adjustments.groupId <= 0)
                            {
                                log.Debug("Value cannot be negative");
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 745));
                            }
                        }
                    }
                    var list = postTransactionDTO.adjustments.Where(x => x.type == AdjustmentTypes.removeMinutes.ToString()
                                                                           || x.type == AdjustmentTypes.removePrivilege.ToString()
                                                                           || x.type == AdjustmentTypes.removeValue.ToString()).ToList();
                    if (list != null && list.Any())
                    {
                        AccountBL accountBL = new AccountBL(executionContext, postTransactionDTO.cardNumber, false, false);
                        if (accountBL.AccountDTO == null)
                        {
                            log.Error("Card not found");
                            throw new ValidationException("Card Not Found");
                        }
                    }
                }


            }
            log.LogMethodExit();
        }
        private void ValidateMaxTimePlays(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            // Check how many time plays are there in source card
            AccountBL accountBL = new AccountBL(executionContext, cardNumber, true, true);
            int sourceTimePlayCount = 0;
            if (accountBL.AccountDTO != null)
            {
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
                CapabilitiesBL capabilitiesBL = CapabilitiesBL.Instance;
                int maximumTimePlaysPerCard = capabilitiesBL.GetCapabilityDTO.timePlay.maximumTimePlaysPerCard;
                if (sourceTimePlayCount == maximumTimePlaysPerCard)
                {
                    log.Debug("sourceTimePlayCount > maximumTimePlaysPerCard");
                    log.Debug(MessageContainerList.GetMessage(executionContext, "maximum Time Plays Per Card is " + maximumTimePlaysPerCard));
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Maximum Time Plays Per Card is " + maximumTimePlaysPerCard));
                }
            }
            log.LogMethodExit();
        }
        private void ValidateMaxDecimalPoints(Adjustments adjustments)
        {
            log.LogMethodEntry(adjustments);
            CapabilitiesBL capabilitiesBL = CapabilitiesBL.Instance;
            if (adjustments.amount.regularPoints > 0)
            {
                int maxDecimalPlace = capabilitiesBL.GetCapabilityDTO.pointTypes.regularPoints.maxDecimalPlaces;
                decimal input = adjustments.amount.regularPoints * Convert.ToDecimal(Math.Pow(10, maxDecimalPlace));
                int denominator = (int)(input);
                log.Debug("denominator " + denominator);
                log.Debug("input " + input);
                if (denominator > 0)
                {
                    decimal value = (input % denominator);
                    if (value > 0)
                    {
                        log.Debug("value > 0");
                        log.Debug(MessageContainerList.GetMessage(executionContext, "maximum decimal Point " + value));
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Maximum decimal Point voilation: " + maxDecimalPlace));
                    }
                }
                if (denominator == 0)
                {
                    log.Debug("denominator == 0");
                    log.Debug(MessageContainerList.GetMessage(executionContext, "maximum decimal Point " + maxDecimalPlace));
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Maximum decimal Point voilation: " + maxDecimalPlace));
                }
            }
            if (adjustments.amount.bonusPoints > 0)
            {
                int maxDecimalPlace = capabilitiesBL.GetCapabilityDTO.pointTypes.bonusPoints.maxDecimalPlaces;
                decimal input = adjustments.amount.bonusPoints * Convert.ToDecimal(Math.Pow(10, maxDecimalPlace));
                int denominator = (int)(input);
                log.Debug("denominator " + denominator);
                log.Debug("input " + input);
                if (denominator > 0)
                {
                    decimal value = (input % denominator);
                    if (value > 0)
                    {
                        log.Debug("value > 0");
                        log.Debug(MessageContainerList.GetMessage(executionContext, "maximum decimal Point " + maxDecimalPlace));
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Maximum decimal Point voilation: " + maxDecimalPlace));
                    }
                }
                if (denominator == 0)
                {
                    log.Debug("denominator == 0");
                    log.Debug(MessageContainerList.GetMessage(executionContext, "maximum decimal Point " + maxDecimalPlace));
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Maximum decimal Point voilation: " + maxDecimalPlace));
                }
            }
            if (adjustments.amount.redemptionTickets > 0)
            {
                int maxDecimalPlace = capabilitiesBL.GetCapabilityDTO.pointTypes.redemptionTickets.maxDecimalPlaces;
                decimal input = adjustments.amount.redemptionTickets * Convert.ToDecimal(Math.Pow(10, maxDecimalPlace));
                int denominator = (int)(input);
                log.Debug("denominator " + denominator);
                log.Debug("input " + input);
                if (denominator > 0)
                {
                    decimal value = (input % denominator);
                    if (value > 0)
                    {
                        log.Debug("value > 0");
                        log.Debug(MessageContainerList.GetMessage(executionContext, "maximum decimal Point " + value));
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Maximum decimal Point voilation: " + maxDecimalPlace));
                    }
                }
                if (denominator == 0)
                {
                    log.Debug("denominator == 0");
                    log.Debug(MessageContainerList.GetMessage(executionContext, "maximum decimal Point " + denominator));
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Maximum decimal Point voilation: " + denominator));
                }
            }
            log.LogMethodExit();
        }


        private void CreateGamePlayTransactionLine(TransactionDTO cETransactionDTO, Utilities utilities, SqlTransaction sqlTransaction)
        {
            try
            {
                log.LogMethodEntry(cETransactionDTO);
                CapabilitiesBL capabilitiesBL = CapabilitiesBL.Instance;
                if (capabilitiesBL.GetCapabilityDTO.virtualPlay == false)
                {
                    log.Debug("GetCapabilityDTO.virtualPlay == false");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "virtualPlay is not enabled in the capability"));
                }
                AccountBL accountBL = new AccountBL(executionContext, postTransactionDTO.cardNumber, false, false, sqlTransaction);
                if (accountBL.AccountDTO == null)
                {
                    log.Error("Card not found");
                    throw new ValidationException("Card Not Found");
                }
                int accountId = accountBL.GetAccountId();
                if (cETransactionDTO.amount == null)
                {
                    cETransactionDTO.amount = new Points();
                }
                GamesListBL gamesListBL = new GamesListBL(executionContext);
                MachineDTO machineDTO = gamesListBL.GetMachineDTO(Convert.ToInt32(postTransactionDTO.gameId));
                if (machineDTO == null)
                {
                    log.Debug("Machine with Id " + postTransactionDTO.gameId + " Not found");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Game id not found"));
                }

                Semnox.Parafait.ServerCore.GameServerEnvironment gameServerEnvironment = new Semnox.Parafait.ServerCore.GameServerEnvironment(utilities.ExecutionContext);
                Semnox.Parafait.ServerCore.Machine servercoreMachine = new Semnox.Parafait.ServerCore.Machine(Convert.ToInt32(postTransactionDTO.gameId), gameServerEnvironment);
                GamePlayDTO gamePlayDTO = servercoreMachine.PlayGame(Convert.ToInt32(postTransactionDTO.gameId), postTransactionDTO.cardNumber, sqlTransaction);
                postTransactionDTO.id = gamePlayDTO.GameplayId;
                postTransactionDTO.transactionTime = gamePlayDTO.PlayDate.ToString("yyyy-MM-ddTHH:mm:ssK");
                // do we need to add to external reference
                log.LogMethodExit();
            }

            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        internal void CreateTransactionLine(Transaction.Card cards, Adjustments adjustment,
                                          Parafait.Transaction.Transaction transaction,
                                          Utilities utilities, SqlTransaction sqlTransaction, int refundProductId = -1)
        {
            try
            {
                log.LogMethodEntry(cards, adjustment, transaction, utilities);
                int productId = -1;
                int trxLineReturnValue = -1;
                int transactionId = -1;
                string transactionLineMessage = string.Empty;
                CreditPlus creditPlus = new CreditPlus(utilities);
                CapabilitiesBL capabilitiesBL = CapabilitiesBL.Instance;
                if (adjustment.type == AdjustmentTypes.addValue.ToString())
                {
                    if (adjustment.amount == null)
                    {
                        log.Error("Invalid input");
                        throw new ValidationException("Invalid input");
                    }
                    if (adjustment.amount.bonusPoints > 0)
                    {
                        if (capabilitiesBL.GetCapabilityDTO.pointTypes.bonusPoints.isSupported == false)
                        {
                            log.Debug("Failed to create transaction line: bonusPoints not supported");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "bonusPoints not supported"));
                        }
                        productId = GetVariableProductId("CEBonus");
                        trxLineReturnValue = transaction.createTransactionLine(cards, productId, Convert.ToDouble(adjustment.amount.bonusPoints), 1, ref transactionLineMessage);
                        log.Debug(transactionLineMessage);
                        if (trxLineReturnValue != 0)
                        {
                            log.Debug("Failed to create transaction line:AdjustmentTypes.addValue");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line"));
                        }
                    }
                    if (adjustment.amount.regularPoints > 0)
                    {
                        if (capabilitiesBL.GetCapabilityDTO.pointTypes.regularPoints.isSupported == false)
                        {
                            log.Debug("Failed to create transaction line: regularPoints not supported");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "regularPoints not supported"));
                        }
                        productId = GetVariableProductId("CECredits");
                        trxLineReturnValue = transaction.createTransactionLine(cards, productId, Convert.ToDouble(adjustment.amount.regularPoints), 1, ref transactionLineMessage);
                        log.Debug(transactionLineMessage);
                        if (trxLineReturnValue != 0)
                        {
                            log.Debug("Failed to create transaction line:AdjustmentTypes.addValue");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line"));
                        }
                    }
                    if (adjustment.amount.redemptionTickets > 0)
                    {
                        if (capabilitiesBL.GetCapabilityDTO.pointTypes.redemptionTickets.isSupported == false)
                        {
                            log.Debug("Failed to create transaction line: redemptionTickets not supported");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "redemptionTickets not supported"));
                        }
                        productId = GetVariableProductId("CETickets");
                        trxLineReturnValue = transaction.createTransactionLine(cards, productId, Convert.ToDouble(Convert.ToInt32(adjustment.amount.redemptionTickets)), 1, ref transactionLineMessage);
                        log.Debug(transactionLineMessage);
                        if (trxLineReturnValue != 0)
                        {
                            log.Debug("Failed to create transaction line:AdjustmentTypes.addValue");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line"));
                        }
                    }
                    //transaction.GameCardReadTime = ServerDateTime.Now;
                }
                else if (adjustment.type == AdjustmentTypes.addMinutes.ToString())
                {
                    if (adjustment.minutes == null)
                    {
                        log.Error("Invalid input");
                        throw new ValidationException("Invalid input");
                    }
                    if (adjustment.minutes > 0)
                    {
                        productId = Convert.ToInt32(adjustment.groupId);
                        transactionLineMessage = adjustment.groupId.ToString(); // This is used to get the product Id during the building transactions 
                        trxLineReturnValue = transaction.createTransactionLine(cards, productId, Convert.ToDouble(adjustment.minutes), 1, ref transactionLineMessage);
                        log.Debug(transactionLineMessage);
                        if (trxLineReturnValue != 0)
                        {
                            log.Debug("Failed to create transaction line:AdjustmentTypes.addMinutes");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line"));
                        }
                        // transaction.GameCardReadTime = ServerDateTime.Now;
                    }
                }
                else if (adjustment.type == AdjustmentTypes.addPrivilege.ToString())
                {
                    if (adjustment.count == null)
                    {
                        log.Error("Invalid input");
                        throw new ValidationException("Invalid input");
                    }
                    productId = Convert.ToInt32(adjustment.groupId);
                    transactionLineMessage = adjustment.groupId.ToString();
                    if (adjustment.count > 0)
                    {
                        for (int i = 0; i < adjustment.count; i++)
                        {
                            trxLineReturnValue = transaction.createTransactionLine(cards, productId, 1, ref transactionLineMessage);
                            log.Debug(transactionLineMessage);
                            if (trxLineReturnValue != 0)
                            {
                                log.Debug("Failed to create transaction line:AdjustmentTypes.addPrivilege");
                                throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line"));
                            }
                        }
                        //transaction.GameCardReadTime = ServerDateTime.Now;
                    }
                }
                else if (adjustment.type == AdjustmentTypes.removeValue.ToString())
                {

                    AccountBL accountBL = new AccountBL(executionContext, cards.CardNumber, true, true, sqlTransaction);
                    if (refundProductId < 0)
                    {
                        log.LogMethodExit("Refund product not found");
                        throw new Exception("Refund product not found");
                    }
                    if (transaction.Trx_id <= 0)
                    {
                        int saveOrderValue = transaction.SaveOrder(ref transactionLineMessage, sqlTransaction);
                        if (saveOrderValue != 0)
                        {
                            log.Debug("Failed to create Order");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create Order"));
                        }
                        log.Debug(transactionLineMessage + ":" + transaction.Trx_id);
                        transactionId = transaction.Trx_id;
                    }
                    else
                    {
                        transactionId = transaction.Trx_id;
                    }
                    TaskProcs tp = new TaskProcs(utilities);
                    if (adjustment.amount.bonusPoints > 0)
                    {
                        if (Convert.ToDecimal(accountBL.AccountDTO.TotalBonusBalance) - adjustment.amount.bonusPoints < 0)
                        {
                            log.Error("Insufficient Credits: Required: " + adjustment.amount.bonusPoints + " Available: " + accountBL.AccountDTO.TotalBonusBalance);
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 49, adjustment.amount.bonusPoints, accountBL.AccountDTO.TotalBonusBalance));
                        }
                        double bonusToDeduct = Convert.ToDouble(adjustment.amount.bonusPoints);
                        if (cards.CreditPlusBonus > 0 && bonusToDeduct > 0)
                        {
                            double balance = creditPlus.DeductGenericCreditPlus(cards, "B", bonusToDeduct, sqlTransaction);
                            if (balance > 0)
                            {
                                bonusToDeduct = bonusToDeduct - balance;
                            }
                            transactionLineMessage = "Bonus"; // pass negative value to this method
                            trxLineReturnValue = transaction.createTransactionLine(cards, refundProductId, bonusToDeduct, 1, ref transactionLineMessage);
                            if (trxLineReturnValue != 0)
                            {
                                log.Debug("Failed to  create TransactionLine");
                                throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create TransactionLine"));
                            }
                            var transactionLine = transaction.TransactionLineList.LastOrDefault();
                            {
                                if (transactionLine.ProductID == refundProductId)
                                {
                                    transactionLine.LineAmount = Convert.ToDouble(bonusToDeduct) * -1;
                                    transactionLine.Price = Convert.ToDouble(bonusToDeduct) * -1;
                                    transaction.updateAmounts(false);
                                }
                            }
                            tp.createTask(cards.card_id, TaskProcs.DEDUCTBALANCE, -1, -1, 0, 0, -1, -1, -1, "center edge deduct bonus", sqlTransaction, -1, -1, bonusToDeduct, -1, transactionId);
                            transaction.GameCardReadTime = ServerDateTime.Now;
                        }
                    }
                    if (adjustment.amount.regularPoints > 0)
                    {
                        if (Convert.ToDecimal(accountBL.AccountDTO.TotalCreditsBalance) - adjustment.amount.regularPoints < 0)
                        {
                            log.Error("Insufficient Credits: Required: " + adjustment.amount.regularPoints + " Available: " + accountBL.AccountDTO.TotalCreditsBalance);
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 49, adjustment.amount.regularPoints, accountBL.AccountDTO.TotalCreditsBalance));
                        }
                        double creditsToDeduct = Convert.ToDouble(adjustment.amount.regularPoints);
                        double balance = creditPlus.DeductGenericCreditPlus(cards, "A", creditsToDeduct, sqlTransaction);
                        if (balance > 0)
                        {
                            creditsToDeduct = creditsToDeduct - balance;
                        }
                        transactionLineMessage = "RegularPoints";
                        trxLineReturnValue = transaction.createTransactionLine(cards, refundProductId, creditsToDeduct, 1, ref transactionLineMessage);
                        if (trxLineReturnValue != 0)
                        {
                            log.Debug("Failed to  create TransactionLine");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create TransactionLine"));
                        }
                        var transactionLine = transaction.TransactionLineList.LastOrDefault();
                        {
                            if (transactionLine.ProductID == refundProductId)
                            {
                                transactionLine.Price = Convert.ToDouble(creditsToDeduct) * -1;
                                transactionLine.LineAmount = Convert.ToDouble(creditsToDeduct) * -1;
                                transaction.updateAmounts(false);
                            }
                        }
                        tp.createTask(cards.card_id, TaskProcs.DEDUCTBALANCE, -1, -1, 0, 0, -1, -1, -1, "center edge deduct credits", sqlTransaction, creditsToDeduct, -1, -1, -1, transactionId);
                        transaction.GameCardReadTime = ServerDateTime.Now;
                    }
                    if (adjustment.amount.redemptionTickets > 0)
                    {
                        if (Convert.ToDecimal(accountBL.AccountDTO.TotalTicketsBalance) - adjustment.amount.redemptionTickets < 0)
                        {
                            log.Error("Insufficient Credits: Required: " + adjustment.amount.redemptionTickets + " Available: " + accountBL.AccountDTO.TotalTicketsBalance);
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 49, adjustment.amount.redemptionTickets, accountBL.AccountDTO.TotalTicketsBalance));
                        }
                        double ticketsToDeduct = Convert.ToDouble(adjustment.amount.redemptionTickets);
                        double balance = creditPlus.DeductGenericCreditPlus(cards, "T", ticketsToDeduct, sqlTransaction);
                        if (balance > 0)
                        {
                            ticketsToDeduct = ticketsToDeduct - balance;
                        }
                        transactionLineMessage = "Ticket";
                        trxLineReturnValue = transaction.createTransactionLine(cards, refundProductId, ticketsToDeduct, 1, ref transactionLineMessage);
                        if (trxLineReturnValue != 0)
                        {
                            log.Debug("Failed to  create TransactionLine");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create TransactionLine"));
                        }
                        var transactionLine = transaction.TransactionLineList.LastOrDefault();
                        {
                            if (transactionLine.ProductID == refundProductId)
                            {
                                transactionLine.LineAmount = Convert.ToDouble(ticketsToDeduct) * -1;
                                transactionLine.Price = Convert.ToDouble(ticketsToDeduct) * -1;
                                transaction.updateAmounts(false);
                            }
                        }
                        tp.createTask(cards.card_id, TaskProcs.DEDUCTBALANCE, -1, -1, 0, 0, -1, -1, -1, "center edge deduct tickets", sqlTransaction, -1, -1, -1, Convert.ToInt32(ticketsToDeduct), transactionId);
                        transaction.GameCardReadTime = ServerDateTime.Now;
                    }
                }
                else if (adjustment.type == AdjustmentTypes.removeMinutes.ToString())
                {

                    if (adjustment.minutes == null)
                    {
                        log.Error("Invalid input");
                        throw new ValidationException("Invalid input");
                    }

                    if (refundProductId < 0)
                    {
                        log.LogMethodExit("Refund product not found");
                        throw new Exception("Refund product not found");
                    }
                    if (transaction.Trx_id <= 0)
                    {
                        int saveOrderValue = transaction.SaveOrder(ref transactionLineMessage, sqlTransaction);
                        if (saveOrderValue != 0)
                        {
                            log.Debug("Failed to create Order");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create Order"));
                        }
                        log.Debug(transactionLineMessage + ":" + transaction.Trx_id);
                        transactionId = transaction.Trx_id;
                    }
                    else
                    {
                        transactionId = transaction.Trx_id;
                    }
                    TaskProcs tp = new TaskProcs(utilities);

                    if (adjustment.minutes.HasValue && adjustment.minutes > 0)
                    {
                        // Check for the time balance 0 , then do not allow to refund
                        DataTable dtSourceCreditPlus = creditPlus.getCreditPlusDetails(cards.card_id, "M", sqlTransaction);
                        if (dtSourceCreditPlus.Rows.Count == 0)
                        {
                            log.Error("Credit plus balance of type Time (M) is zero. Cannot refund");
                            throw new ValidationException("Time balance is zero. Cannot refund");
                        }
                        if (dtSourceCreditPlus.Rows.Count > 0)
                        {
                            decimal usedCPBalance = 0;
                            foreach (DataRow sourceCreditPlusRow in dtSourceCreditPlus.Rows)
                            {
                                usedCPBalance += Convert.ToDecimal(sourceCreditPlusRow["CreditPlusBalance"]);
                            }
                            if (usedCPBalance == 0)
                            {
                                log.Error("Credit plus balance of type Time (M) is zero. Cannot refund");
                                throw new ValidationException("Time balance is zero. Cannot refund");
                            }
                            if (adjustment.minutes > usedCPBalance)
                            {
                                log.Error("Insufficient Time balance: Required: " + adjustment.minutes + " Available: " + usedCPBalance);
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 49, adjustment.minutes, usedCPBalance));
                            }
                        }
                        double timeToDeduct = Convert.ToInt32(adjustment.minutes);
                        double balance = creditPlus.DeductGenericCreditPlus(cards, "M", timeToDeduct, sqlTransaction);
                        if (balance > 0)
                        {
                            timeToDeduct = timeToDeduct - balance;
                        }
                        transactionLineMessage = "Minute";
                        trxLineReturnValue = transaction.createTransactionLine(cards, refundProductId, timeToDeduct, 1, ref transactionLineMessage);
                        if (trxLineReturnValue != 0)
                        {
                            log.Debug("Failed to  create TransactionLine");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create TransactionLine"));
                        }
                        var transactionLine = transaction.TransactionLineList.LastOrDefault();
                        {
                            if (transactionLine.ProductID == refundProductId)
                            {
                                transactionLine.LineAmount = Convert.ToDouble(timeToDeduct) * -1;
                                transactionLine.Price = Convert.ToDouble(timeToDeduct) * -1;
                                transaction.updateAmounts(false);
                            }
                        }
                        tp.createTask(cards.card_id, TaskProcs.DEDUCTBALANCE, -1, -1, 0, 0, -1, -1, -1, "center edge deduct Time : " + timeToDeduct, sqlTransaction, -1, -1, -1, -1, transactionId);
                        transaction.GameCardReadTime = ServerDateTime.Now;
                    }
                    log.LogMethodExit();
                }
                else if (adjustment.type == AdjustmentTypes.removePrivilege.ToString())
                {
                    if (adjustment.count == null)
                    {
                        log.Error("Invalid input");
                        throw new ValidationException("Invalid input");
                    }
                    if (adjustment.count > 0)
                    {
                        int gameId = -1;
                        if (refundProductId < 0)
                        {
                            log.LogMethodExit("Refund product not found");
                            throw new Exception("Refund product not found");
                        }
                        if (transaction.Trx_id <= 0)
                        {
                            int saveOrderValue = transaction.SaveOrder(ref transactionLineMessage, sqlTransaction);
                            if (saveOrderValue != 0)
                            {
                                log.Debug("Failed to create Order");
                                throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create Order"));
                            }
                            log.Debug(transactionLineMessage + ":" + transaction.Trx_id);
                            transactionId = transaction.Trx_id;
                        }
                        else
                        {
                            transactionId = transaction.Trx_id;
                        }

                        ProductGamesListBL productGamesListBL = new ProductGamesListBL(executionContext);
                        List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>> searchParameters = new List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_ID, adjustment.groupId.ToString()));
                        List<ProductGamesDTO> productGamesDTOList = productGamesListBL.GetProductGamesDTOList(searchParameters, sqlTransaction);
                        log.Debug("productGamesDTOList : " + productGamesDTOList);
                        if (productGamesDTOList != null && productGamesDTOList.Any())
                        {
                            gameId = productGamesDTOList[0].Game_id;
                        }
                        else
                        {
                            log.Error("CenterEdge Privilege products are not created." + adjustment.groupId);
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Privilege group id Not found"));
                        }
                        double gameToDeduct = Convert.ToInt32(adjustment.count);
                        TaskProcs tp = new TaskProcs(utilities);
                        if (!tp.DeductGameBalance(cards, gameToDeduct, gameId, ref transactionLineMessage, utilities, sqlTransaction))
                        {
                            log.Debug("Failed to deduct balance");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to deduct balance"));
                        }
                        transactionLineMessage = "Privilege." + adjustment.groupId;
                        log.Debug("transactionLineMessage:" + transactionLineMessage);

                        trxLineReturnValue = transaction.createTransactionLine(cards, refundProductId, gameToDeduct, 1, ref transactionLineMessage);
                        if (trxLineReturnValue != 0)
                        {
                            log.Debug("Failed to  create TransactionLine");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create TransactionLine"));
                        }
                        var transactionLine = transaction.TransactionLineList.LastOrDefault();
                        {
                            if (transactionLine.ProductID == refundProductId)
                            {
                                transactionLine.LineAmount = Convert.ToDouble(gameToDeduct) * -1;
                                transactionLine.Price = Convert.ToDouble(gameToDeduct) * -1;
                                transaction.updateAmounts(false);
                            }
                        }
                        tp.createTask(cards.card_id, TaskProcs.DEDUCTBALANCE, -1, -1, 0, 0, -1, -1, -1, "center edge deduct Game balance : " + gameToDeduct, sqlTransaction, -1, -1, -1, -1, transactionId);
                        transaction.GameCardReadTime = ServerDateTime.Now;
                    }
                    log.LogMethodExit();
                }
            }
            
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        internal int GetVariableProductId(string adjustmentProductName)
        {
            log.LogMethodEntry(adjustmentProductName);
            int variableProductId = -1;
            string productName = string.Empty;
            switch (adjustmentProductName)
            {
                case "CEBonus": { productName = "CEBONUSVARIABLE"; } break;
                case "CETickets": { productName = "CETICKETSVARIABLE"; } break;
                case "CECredits": { productName = "CECREDITSVARIABLE"; } break;
                case "CETime": { productName = "CETIMEVARIABLE"; } break;
                case "CECARDISSUE": { productName = "CECARDISSUEVARIABLE"; } break;
                default:
                    productName = adjustmentProductName; break;
            }
            ProductsList productsList = new ProductsList(executionContext);
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_EXACT_NAME, productName));
            List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParameters);
            log.Debug("productsDTOList : " + productsDTOList);
            if (productsDTOList != null && productsDTOList.Any())
            {
                variableProductId = productsDTOList[0].ProductId;
            }
            else
            {
                log.Error("CenterEdge variable products are not created.");
            }
            log.LogMethodExit(variableProductId);
            return variableProductId;
        }


        internal void ConsolidateCards(TransactionServiceDTO transactionServiceDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            try
            {
                ConsolidateCardBL consolidateCardBL = new ConsolidateCardBL(executionContext, transactionServiceDTO);
                consolidateCardBL.CardConsolidate(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method is used to create transactions for card issue 
        /// </summary>
        /// <returns></returns>
        public List<Card> IssueMultipleCards()
        {
            try
            {
                log.LogMethodEntry(ceTransactionServiceDTO);
                List<Card> cardList = new List<Card>();
                LookupValuesList lookupsList = new LookupValuesList(executionContext);
                List<string> cardNumberList = ceTransactionServiceDTO.cardNumbers;
                Utilities utilities = GetUtility();
                CapabilitiesBL capabilitiesBL = CapabilitiesBL.Instance;
                int maximumAdjustments = capabilitiesBL.GetCapabilityDTO.adjustments.maximumAdjustmentsPerTransaction;
                if (ceTransactionServiceDTO.adjustments.Count > maximumAdjustments)
                {
                    log.Error("Number of adjustments is more than the configuaration in capability");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Number of adjustments is more than the configuaration in capability"));
                }
                TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
                using (ParafaitDBTransaction trx = new ParafaitDBTransaction())
                {
                    trx.BeginTransaction();
                    Validate();
                    Transaction.Transaction transaction = new Transaction.Transaction(utilities);
                    foreach (string tagNumber in cardNumberList)
                    {
                        if (tagNumberParser.IsValid(tagNumber) == false)
                        {
                            string errorMessage = tagNumberParser.Validate(tagNumber);
                            log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                            throw new ValidationException(errorMessage, "CardNumber", "CardNumber", errorMessage);
                        }
                        AccountBL accountBL = new AccountBL(executionContext, tagNumber, false, false, trx.SQLTrx);
                        if (accountBL.AccountDTO != null)
                        {
                            log.Debug("Card exists");
                            throw new ValidationException(tagNumber + ": This card is already issued");
                        }
                        DateTime issueDate = ServerDateTime.Now;
                        AccountDTO accountDTO = new AccountDTO(-1, tagNumber, "", issueDate, null, false, null, null, true,
                                                    null, null, null, null, null, null, -1, null, true, true, false, null, null, "N", null,
                                                    false, null, -1, null, null, -1, null, null, false, -1, "", "",
                                                    issueDate, executionContext.GetSiteId(), -1, false, "", executionContext.GetUserId(),
                                                    issueDate);
                        accountBL = new AccountBL(executionContext, accountDTO);
                        accountBL.Save(trx.SQLTrx);
                        transaction.Remarks = "Bulk Issue : CenterEdge transactions for card No:" + tagNumber;
                        Transaction.Card cards = new Parafait.Transaction.Card(accountBL.AccountDTO.AccountId, executionContext.GetUserId(), utilities, trx.SQLTrx);
                        foreach (Adjustments adjustment in ceTransactionServiceDTO.adjustments)
                        {
                            CreateTransactionLine(cards, adjustment, transaction, utilities, trx.SQLTrx);
                        }

                    }
                    PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                    if (paymentModeDTOList != null)
                    {
                        TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, transaction.Transaction_Amount,
                                                                                            "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", ServerDateTime.Now,
                                                                                            utilities.ParafaitEnv.LoginID, -1, null, 0, -1, executionContext.POSMachineName, -1, "", null);
                        trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];

                        transaction.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                    }
                    string transactionMessage = string.Empty;
                    int returnValue = transaction.SaveTransacation(trx.SQLTrx, ref transactionMessage);
                    transaction.InsertTrxLogs(transaction.Trx_id, -1, executionContext.GetUserId(), "Save Transacation", "CenterEdge save transaction status : " + returnValue, trx.SQLTrx);
                    if (returnValue != 0)
                    {
                        log.Error("saveTrx() in SaveTransaction as retcode != 0 error: " + transactionMessage);
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 240));
                    }
                    Transaction.TransactionBL transactionBL = new Transaction.TransactionBL(executionContext, transaction.Trx_id, trx.SQLTrx);
                    transactionBL.TransactionDTO.ExternalSystemReference = transaction.Trx_id.ToString();
                    transactionBL.Save(trx.SQLTrx);

                    foreach (string tagNumber in cardNumberList)
                    {
                        Card card = GetCard(tagNumber, trx.SQLTrx);
                        cardList.Add(card);
                    }
                    trx.EndTransaction();
                }
                log.LogMethodExit(cardList);
                return cardList;
            }
            catch (ValidationException vx)
            {
                log.Error(vx.Message);
                throw vx;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }

        }

        public Card GetCard(string tagNumber, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            CardBL cardBL = new CardBL(executionContext, tagNumber, sqlTransaction);
            Card card = cardBL.GetDetails(sqlTransaction);
            log.LogMethodExit(card);
            return card;
        }

        /// <summary>
        /// Activate - Issue new cards through transaction. 
        /// </summary>
        /// <param name="accountNumber">accountNumber</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AccountDTO Activate(string accountNumber, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountNumber, sqlTransaction);
            Utilities utilities = GetUtility();
            Transaction.Transaction transaction = new Parafait.Transaction.Transaction(utilities);
            Transaction.Card cards = new Transaction.Card(utilities);
            if (!string.IsNullOrEmpty(accountNumber))
            {
                log.Debug("Account does not exists.Creating new Account");
                cards = new Parafait.Transaction.Card(accountNumber, executionContext.GetUserId(), utilities, sqlTransaction);
            }
            int trxLineReturnValue = -1;
            string transactionLineMessage = string.Empty;
            int productId = GetVariableProductId("CECARDISSUE");
            if (productId > -1)
            {
                trxLineReturnValue = transaction.createTransactionLine(cards, productId, -1, 1, ref transactionLineMessage);
            }
            else
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 111);
                log.Error("Throwing Exception- " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.Debug(transactionLineMessage);
            if (trxLineReturnValue != 0)
            {
                log.Debug("Failed to create transaction line:Account Activate. " + transactionLineMessage);
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to create transaction line. " + transactionLineMessage));
            }

            PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
            searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
            List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
            if (paymentModeDTOList != null)
            {

                TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, transaction.Transaction_Amount,
                                                                                    "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", ServerDateTime.Now,
                                                                                    utilities.ParafaitEnv.LoginID, -1, null, 0, -1, executionContext.POSMachineName, -1, "", null);
                trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];

                transaction.TransactionPaymentsDTOList.Add(trxPaymentDTO);
            }
            string transactionMessage = string.Empty;
            int trxId = transaction.SaveTransacation(sqlTransaction, ref transactionMessage);
            if (trxId != 0)
            {
                log.Debug("Failed to save transaction " + transactionMessage);
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Failed to save transaction" + transactionMessage));
            }
            transaction.InsertTrxLogs(transaction.Trx_id, -1, executionContext.GetUserId(), "Save Transacation", "CenterEdge save transaction status : " + transaction.Trx_id, sqlTransaction);
            log.Debug("SaveTransaction" + transactionMessage);
            AccountBL accountBL = new AccountBL(executionContext, accountNumber, false, false, sqlTransaction);
            log.LogMethodExit(accountBL.AccountDTO);
            return accountBL.AccountDTO;
        }

        internal Utilities GetUtility()
        {
            log.LogMethodEntry();
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ParafaitEnv.POSMachineId = executionContext.GetMachineId();
            POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(executionContext.GetSiteId(), "CenterEdge", "", -1);
            if (pOSMachineContainerDTO != null)
            {
                utilities.ParafaitEnv.SetPOSMachine("", pOSMachineContainerDTO.POSName);
            }
            else
            {
                utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            }

            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            UserContainerDTO user = UserContainerList.GetUserContainerDTOOrDefault(executionContext.GetUserId(), "", executionContext.GetSiteId());
            utilities.ParafaitEnv.User_Id = user.UserId;
            utilities.ParafaitEnv.RoleId = user.RoleId;
            utilities.ExecutionContext.SetUserId(user.LoginId);
            utilities.ParafaitEnv.Initialize();
            log.LogMethodExit();
            return utilities;
        }
    }
    public class TransactionList
    {
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ConcurrentDictionary<int, TransactionDTO> ceTransactionDictionary = new ConcurrentDictionary<int, TransactionDTO>();
        private List<TransactionDTO> ceCardTransactions = new List<TransactionDTO>();
        public TransactionList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CardTransaction GetCardTransactions(string cardNumber, int transactionId = -1, int skip = 0,
                                                            int take = 100, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cardNumber, transactionId, skip, take);
            TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
            TagNumber tagNumber;
            if (tagNumberParser.TryParse(cardNumber, out tagNumber) == false)
            {
                string message = tagNumberParser.Validate(cardNumber);
                log.LogMethodExit(null, "Throwing Exception- " + message);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Card Not Found"));
            }
            //Check for the  card which is already exists If not throw an error
            AccountBL accountBL = new AccountBL(executionContext, cardNumber, true, true, sqlTransaction);
            if (accountBL.AccountDTO == null || accountBL.AccountDTO.ValidFlag == false)
            {
                log.Debug("Card Not Found");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Card Not Found"));
            }
            List<int> uniqueTrxIdList = new List<int>();
            CardTransaction cardTransaction = new CardTransaction();
            cardTransaction.cardNumber = cardNumber;
            AccountDTO accountDTO = accountBL.AccountDTO;

            if (accountDTO.AccountCreditPlusDTOList == null)
            {
                accountDTO.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
            }
            if (accountDTO.AccountGameDTOList == null)
            {
                accountDTO.AccountGameDTOList = new List<AccountGameDTO>();
            }
            if (cardTransaction.transactions == null)
            {
                cardTransaction.transactions = new List<TransactionDTO>();
            }
            if (accountDTO.AccountCreditPlusDTOList.Any() || accountDTO.AccountGameDTOList.Any())
            {
                accountDTO.AccountCreditPlusDTOList.OrderByDescending(credit => credit.LastUpdateDate);
                accountDTO.AccountGameDTOList.OrderByDescending(games => games.LastUpdateDate);
                uniqueTrxIdList = GetTransactionIdList(accountDTO.AccountId, transactionId, sqlTransaction);
                if (uniqueTrxIdList.Count > 0)
                {
                    ceCardTransactions = GetAdjustmentTransactions(uniqueTrxIdList, accountDTO, sqlTransaction);
                    ceCardTransactions.AddRange(GetGamePlayTransactions(cardNumber, 0, -1, skip, take, sqlTransaction));
                    take = take > ceCardTransactions.Count ? ceCardTransactions.Count : take;
                    ceCardTransactions = ceCardTransactions.OrderByDescending(x => Convert.ToDateTime(x.transactionTime)).ToList();
                    List<TransactionDTO> result = ceCardTransactions.GetRange(skip, take);
                    cardTransaction.transactions.AddRange(result);
                    cardTransaction.totalCount = cardTransaction.transactions.Count;
                }

            }
            log.LogMethodExit(cardTransaction);
            return cardTransaction;
        }


        public List<TransactionDTO> GetAdjustmentTransactions(List<int> transactionIdList, AccountDTO accountDTO,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionIdList);
            int refundProductId = new TransactionBL(executionContext).GetRefundProductId();
            List<TransactionDTO> adjustmentTransactions = new List<TransactionDTO>();
            for (int i = 0; i < transactionIdList.Count; i++)
            {
                int id = transactionIdList[i];
                int productId = 0;
                Parafait.Transaction.TransactionDTO transactionDTO = GetTransactionDTO(id, sqlTransaction);
                TransactionDTO ceCardTransaction = new TransactionDTO();

                ceCardTransaction.cardNumber = accountDTO.TagNumber;
                ceCardTransaction.id = string.IsNullOrWhiteSpace(transactionDTO.ExternalSystemReference) ? transactionDTO.TransactionId : Convert.ToInt32(transactionDTO.ExternalSystemReference);
                ceCardTransaction.transactionTime = transactionDTO.TransactionDate.ToString("yyyy-MM-ddTHH:mm:ssK");
                ceCardTransaction.type = TransactionTypes.adjustment.ToString();
                if (transactionDTO.TransactionLinesDTOList == null && transactionDTO.TransactionLinesDTOList.Any() == false)
                {
                    continue;
                }
                //This code adds the card creditplus details for each transactions
                List<TransactionLineDTO> refundTransactionLineDTOList = transactionDTO.TransactionLinesDTOList.Where(trxLine => trxLine.Price < 0 && trxLine.ProductId == refundProductId).ToList();
                if (refundTransactionLineDTOList != null && refundTransactionLineDTOList.Any())
                {
                    foreach (TransactionLineDTO transactionLineDTO in refundTransactionLineDTOList)
                    {
                        Adjustments adjustments = new Adjustments();

                        if (transactionLineDTO.Remarks.Contains("Privilege"))
                        {
                            adjustments.groupId = transactionLineDTO.ProductId;
                            var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                            if (regex.IsMatch(transactionLineDTO.Remarks))
                            {
                                string privilegeId = regex.Match(transactionLineDTO.Remarks).Value;
                                log.Debug("privilegeId: " + privilegeId);
                                int decimalPlaceCount = privilegeId.Length;
                                log.Debug("decimalPlaceCount: " + decimalPlaceCount);
                                if (decimalPlaceCount > 0)
                                {
                                    adjustments.groupId = Convert.ToInt32(privilegeId);
                                }
                            }
                            adjustments.type = AdjustmentTypes.removePrivilege.ToString();
                            adjustments.count = Convert.ToInt32(transactionLineDTO.Price * -1);
                            adjustments.amount = null;
                        }
                        else if (transactionLineDTO.Remarks == "Minute")
                        {
                            adjustments.minutes = Convert.ToInt32(transactionLineDTO.Price * -1);
                            adjustments.type = AdjustmentTypes.removeMinutes.ToString();
                            TimePlayGroupBL timePlayGroupBL = new TimePlayGroupBL(executionContext);
                            TimePlayGroups timePlayGroups = timePlayGroupBL.GetTimePlayGroups(0, -1);
                            if (timePlayGroups.timePlayGroups != null && timePlayGroups.timePlayGroups.Any())
                            {
                                adjustments.groupId = timePlayGroups.timePlayGroups.FirstOrDefault().id;
                            }
                            else
                            {
                                adjustments.groupId = transactionLineDTO.ProductId;
                            }

                            adjustments.amount = null;

                        }
                        else if (transactionLineDTO.Remarks == "Bonus")
                        {
                            adjustments.amount.bonusPoints = Convert.ToDecimal(transactionLineDTO.Price * -1);
                            adjustments.type = AdjustmentTypes.removeValue.ToString();

                        }
                        else if (transactionLineDTO.Remarks == "RegularPoints")
                        {
                            adjustments.amount.regularPoints = Convert.ToDecimal(transactionLineDTO.Price * -1);
                            adjustments.type = AdjustmentTypes.removeValue.ToString();

                        }
                        else if (transactionLineDTO.Remarks == "Ticket")
                        {
                            adjustments.amount.redemptionTickets = Convert.ToDecimal(transactionLineDTO.Price * -1);
                            adjustments.type = AdjustmentTypes.removeValue.ToString();
                        }
                        ceCardTransaction.adjustments.Add(adjustments);
                    }
                    //adjustmentTransactions.Add(ceCardTransaction);
                }
                List<AccountCreditPlusDTO> filteredList = accountDTO.AccountCreditPlusDTOList.Where(x => x.TransactionId == id).ToList();
                int fromCardProductId = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CONSOLIDATION_TASK_FROM_CARD_PRODUCT"));
                int toCardProductId = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CONSOLIDATION_TASK_TO_CARD_PRODUCT"));

                if (filteredList != null && filteredList.Any())
                {
                    foreach (AccountCreditPlusDTO accountCreditPlusDTO in filteredList)
                    {
                        Adjustments valueAdjustments = new Adjustments();
                        List<TransactionLineDTO> transactionLineDTOList = transactionDTO.TransactionLinesDTOList.Where(trxLine => trxLine.LineId == accountCreditPlusDTO.TransactionLineId).ToList();
                        if (transactionLineDTOList != null && transactionLineDTOList.Any())
                        {
                            productId = transactionLineDTOList[0].ProductId;
                            if (productId == fromCardProductId || productId == toCardProductId)
                            {
                                TimePlayGroupBL timePlayGroupBL = new TimePlayGroupBL(executionContext);
                                TimePlayGroups timePlayGroups = timePlayGroupBL.GetTimePlayGroups(0, -1);
                                if (timePlayGroups.timePlayGroups != null && timePlayGroups.timePlayGroups.Any())
                                {
                                    productId = timePlayGroups.timePlayGroups.FirstOrDefault().id;
                                    log.Debug("productId: " + productId);
                                }
                            }
                        }
                        if (accountCreditPlusDTO.CreditPlusType == CreditPlusType.CARD_BALANCE)
                        {
                            valueAdjustments.amount.regularPoints = Convert.ToDecimal(accountCreditPlusDTO.CreditPlus);
                            if (Convert.ToInt32(accountCreditPlusDTO.CreditPlus) < 0)
                            {
                                valueAdjustments.amount.regularPoints = valueAdjustments.amount.regularPoints * -1;
                                valueAdjustments.type = AdjustmentTypes.removeValue.ToString();
                            }
                            else
                            {
                                valueAdjustments.type = AdjustmentTypes.addValue.ToString();
                            }
                        }

                        else if (accountCreditPlusDTO.CreditPlusType == CreditPlusType.GAME_PLAY_BONUS)
                        {
                            valueAdjustments.amount.bonusPoints = Convert.ToDecimal(accountCreditPlusDTO.CreditPlus);
                            if (Convert.ToInt32(accountCreditPlusDTO.CreditPlus) < 0)
                            {
                                valueAdjustments.amount.bonusPoints = valueAdjustments.amount.bonusPoints * -1;
                                valueAdjustments.type = AdjustmentTypes.removeValue.ToString();
                            }
                            else
                            {
                                valueAdjustments.type = AdjustmentTypes.addValue.ToString();
                            }
                        }
                        else if (accountCreditPlusDTO.CreditPlusType == CreditPlusType.TICKET)
                        {
                            valueAdjustments.amount.redemptionTickets = Convert.ToDecimal(accountCreditPlusDTO.CreditPlus);
                            if (Convert.ToInt32(accountCreditPlusDTO.CreditPlus) < 0)
                            {
                                valueAdjustments.amount.redemptionTickets = valueAdjustments.amount.redemptionTickets * -1;
                                valueAdjustments.type = AdjustmentTypes.removeValue.ToString();
                            }
                            else
                            {
                                valueAdjustments.type = AdjustmentTypes.addValue.ToString();
                            }
                        }
                        else if (accountCreditPlusDTO.CreditPlusType == CreditPlusType.TIME)
                        {
                            valueAdjustments.minutes = Convert.ToInt32(accountCreditPlusDTO.CreditPlus);
                            if (Convert.ToInt32(accountCreditPlusDTO.CreditPlus) < 0)
                            {
                                valueAdjustments.minutes = valueAdjustments.minutes * -1;  // Give positive value incase of remove credit plus
                                valueAdjustments.type = AdjustmentTypes.removeMinutes.ToString();
                            }
                            else
                            {
                                valueAdjustments.type = AdjustmentTypes.addMinutes.ToString();
                            }
                            valueAdjustments.groupId = productId;
                            valueAdjustments.amount = null;

                        }
                        else
                        {
                            log.Debug("No matching credit plus Line");
                            continue;
                        }
                        ceCardTransaction.adjustments.Add(valueAdjustments);
                    }
                }

                List<AccountGameDTO> filteredGamesTrxList = accountDTO.AccountGameDTOList.Where(x => x.TransactionId == id).ToList();
                if (filteredGamesTrxList != null && filteredGamesTrxList.Any())
                {
                    // This code adds the card games details for each transactions

                    foreach (AccountGameDTO accountGameDTO in filteredGamesTrxList)
                    {
                        Adjustments gameAdjustments = new Adjustments();
                        List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>> searchParameters = new List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        if (accountGameDTO.GameId > -1)
                        {
                            searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.GAME_ID, accountGameDTO.GameId.ToString()));
                        }
                        else
                        {
                            searchParameters.Add(new KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>(ProductGamesDTO.SearchByProductGamesParameters.GAME_PROFILE_ID, accountGameDTO.GameProfileId.ToString()));
                        }
                        ProductGamesListBL productGamesListBL = new ProductGamesListBL(executionContext);
                        List<ProductGamesDTO> productGamesDTOList = productGamesListBL.GetProductGamesDTOList(searchParameters);
                        List<TransactionLineDTO> transactionLineDTOList = transactionDTO.TransactionLinesDTOList.Where(trxLine => trxLine.LineId == accountGameDTO.TransactionLineId).ToList();
                        if (transactionLineDTOList != null && transactionLineDTOList.Any())
                        {
                            if (productGamesDTOList != null && productGamesDTOList.Any())
                            {
                                gameAdjustments.groupId = productGamesDTOList.FirstOrDefault().Product_id;
                            }
                            else
                            {
                                gameAdjustments.groupId = transactionLineDTOList[0].ProductId;
                            }
                            if (accountGameDTO.Quantity < 0 && transactionLineDTOList[0].ProductId == fromCardProductId) // incase if card combine - from card
                            {
                                gameAdjustments.type = AdjustmentTypes.removePrivilege.ToString();
                                gameAdjustments.count = Convert.ToInt32(accountGameDTO.Quantity) * -1;
                                gameAdjustments.amount = null;

                            }
                            else if (accountGameDTO.Quantity > 0 && transactionLineDTOList[0].ProductId == toCardProductId) // incase if card combine - from card
                            {
                                gameAdjustments.type = AdjustmentTypes.addPrivilege.ToString();
                                gameAdjustments.count = Convert.ToInt32(accountGameDTO.Quantity);
                                gameAdjustments.amount = null;

                            }
                            else
                            {
                                gameAdjustments.type = AdjustmentTypes.addPrivilege.ToString();
                                gameAdjustments.count = Convert.ToInt32(accountGameDTO.Quantity);
                                gameAdjustments.amount = null;
                            }
                        }
                        ceCardTransaction.adjustments.Add(gameAdjustments);
                    }
                }
                adjustmentTransactions.Add(ceCardTransaction);
            }
            log.LogMethodExit(adjustmentTransactions);
            return adjustmentTransactions;
        }

        public List<TransactionDTO> GetGamePlayTransactions(string cardNumber, int sinceId, int gamePlayId, int skip, int take, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cardNumber, sinceId, gamePlayId, skip, take, sqlTransaction);
            GamePlaySummaryListBL gamePlaySummaryListBL = new GamePlaySummaryListBL(executionContext);
            GamePlayListBL gamePlayListBL = new GamePlayListBL(executionContext);
            List<TransactionDTO> gamePlayTransactions = new List<TransactionDTO>();
            List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> gameSearchParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
            if (string.IsNullOrWhiteSpace(cardNumber) == false)
            {
                AccountDTO accountDTO = new AccountBL(executionContext, cardNumber, false, false, sqlTransaction).AccountDTO;
                gameSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID, accountDTO.AccountId.ToString()));
            }
            if (gamePlayId > -1)
            {
                gameSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.GAME_PLAY_ID, gamePlayId.ToString()));
            }
            else
            {
                // Get all game play transaction greater than sinceId 
                gameSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.GAME_PLAY_ID_GREATER_THAN, sinceId.ToString()));
            }
            gameSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<GamePlayDTO> gamePlayDTOList = gamePlayListBL.GetGamePlays(gameSearchParameters, take, skip, sqlTransaction); //(gameSearchParameters,false, sqlTransaction: sqlTransaction, numberOfRecords: take, pageNumber: skip);
            if (gamePlayDTOList != null && gamePlayDTOList.Any())
            {
                GamesListBL gamesListBL = new GamesListBL(executionContext);
                foreach (GamePlayDTO gamePlayDTO in gamePlayDTOList)
                {
                    TransactionDTO ceGamePlayTransactionDTO = new TransactionDTO();
                    ceGamePlayTransactionDTO.cardNumber = gamePlayDTO.CardNumber;
                    ceGamePlayTransactionDTO.type = TransactionTypes.gamePlay.ToString();
                    ceGamePlayTransactionDTO.id = gamePlayDTO.GameplayId;
                    ceGamePlayTransactionDTO.adjustments = null;

                    MachineDTO machineDTO = gamesListBL.GetMachineDTO(gamePlayDTO.MachineId);
                    ceGamePlayTransactionDTO.gameId = machineDTO.MachineId;
                    ceGamePlayTransactionDTO.gameDescription = machineDTO.MachineNameGameName;
                    ceGamePlayTransactionDTO.transactionTime = gamePlayDTO.PlayDate.ToString("yyyy-MM-ddTHH:mm:ssK");
                    if (ceGamePlayTransactionDTO.amount == null)
                    {
                        ceGamePlayTransactionDTO.amount = new Points();
                    }
                    ceGamePlayTransactionDTO.amount.bonusPoints = Convert.ToDecimal(gamePlayDTO.CPBonus) + Convert.ToDecimal(gamePlayDTO.Bonus);
                    ceGamePlayTransactionDTO.amount.regularPoints = Convert.ToDecimal(gamePlayDTO.CPCredits) + Convert.ToDecimal(gamePlayDTO.Credits) +
                            Convert.ToDecimal(gamePlayDTO.CPCardBalance);
                    ceGamePlayTransactionDTO.amount.redemptionTickets = Convert.ToDecimal(gamePlayDTO.TicketCount);
                    if (gamePlayDTO.Time > 0)
                    {
                        ceGamePlayTransactionDTO.usedTimePlay = true;
                    }
                    else
                    {
                        ceGamePlayTransactionDTO.usedTimePlay = false;
                    }
                    if (gamePlayDTO.CardGame > 0)
                    {
                        ceGamePlayTransactionDTO.usedPlayPrivilege = true;
                    }
                    else
                    {
                        ceGamePlayTransactionDTO.usedPlayPrivilege = false;
                    }
                    gamePlayTransactions.Add(ceGamePlayTransactionDTO);
                }
            }
            log.LogMethodExit(gamePlayTransactions);
            return gamePlayTransactions;
        }



        private Parafait.Transaction.TransactionDTO GetTransactionDTO(int transactionId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionId);
            TransactionListBL transactionListBL = new TransactionListBL(executionContext);
            TransactionBL transactionBL = new TransactionBL(executionContext);
            Utilities utilities = transactionBL.GetUtility();
            List<KeyValuePair<Parafait.Transaction.TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<Parafait.Transaction.TransactionDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<Parafait.Transaction.TransactionDTO.SearchByParameters, string>(Parafait.Transaction.TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<Parafait.Transaction.TransactionDTO.SearchByParameters, string>(Parafait.Transaction.TransactionDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
            List<Parafait.Transaction.TransactionDTO> transactionList = transactionListBL.GetTransactionDTOList(searchParameters, utilities, sqlTransaction, 0, 10, true);
            log.LogMethodExit(transactionList);
            return transactionList[0];
        }


        /// <summary>
        /// This methods gets the parafait transaction Ids mapping to CE transactions
        /// CE transactions are stored in Trx_header table starts with CE+ TrxId
        /// </summary>
        private List<int> GetTransactionIdList(int cardId, int transactionId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cardId);
            List<int> parafaitTrxIdList = new List<int>();
            TransactionListBL transactionBL = new TransactionListBL(executionContext);
            parafaitTrxIdList = transactionBL.GetTransactionIdList(cardId, transactionId, executionContext.GetSiteId(), sqlTransaction);
            log.LogMethodExit(parafaitTrxIdList);
            return parafaitTrxIdList;
        }
    }
}

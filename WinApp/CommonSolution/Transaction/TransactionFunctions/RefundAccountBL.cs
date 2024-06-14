/********************************************************************************************
 * Project Name - AccountService
 * Description  -Refund Entitlements from the cards
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80       12-Mar-2020   Girish Kundar          Created
 *2.120.0    22-Apr-2021   Prajwal S              Modified : Full refund changes.
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// RefundAccountBL
    /// </summary>
    public class RefundAccountBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private AccountServiceDTO accountServiceDTO;
        private AccountBL accountBL;
        private double creditsRefund, creditPlusAmount, creditPlusRefund, refundAmount, refundCardDeposit;

        private RefundAccountBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="accountServiceDTO"></param>
        public RefundAccountBL(ExecutionContext executionContext, AccountServiceDTO accountServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountServiceDTO);
            this.accountServiceDTO = accountServiceDTO;
            HelperClassBL.SetParafairEnvValues(executionContext, utilities);
            log.LogMethodExit();
        }


        /// <summary>
        /// Refund mwthod
        /// </summary>
        public void Refund(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            SqlConnection sqlConnection = null;
            SqlTransaction parafaitDBTrx;
            if (sqlTransaction == null)
            {
                sqlConnection = utilities.createConnection();
                parafaitDBTrx = sqlConnection.BeginTransaction();
            }
            else
            {
                parafaitDBTrx = sqlTransaction;
            }
            try
            {
                if (accountServiceDTO.RefundAmount == -1)  // full refund
                {
                    FullCardRefund(parafaitDBTrx);
                }

                else if (accountServiceDTO.RefundAmount > 0) // partial refund with amount specified
                {
                    PartialCardAmountRefund(parafaitDBTrx);
                }

                else if ((accountServiceDTO.SourceAccountDTO.RefundAccountCreditPlusDTOList != null &&
                                  accountServiceDTO.SourceAccountDTO.RefundAccountCreditPlusDTOList.Count > 0) ||
                    (accountServiceDTO.SourceAccountDTO.RefundAccountGameDTOList != null &&
                    accountServiceDTO.SourceAccountDTO.RefundAccountGameDTOList.Count > 0))
                {
                    PartialCardRefund(parafaitDBTrx);
                }
                if (sqlTransaction == null)
                {
                    parafaitDBTrx.Commit();
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                if (sqlTransaction == null)
                {
                    parafaitDBTrx.Rollback();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Full refund 
        /// </summary>
        private void FullCardRefund(SqlTransaction parafaitDBTrx)
        {
            log.LogMethodEntry(parafaitDBTrx);
            try
            {
                string message = string.Empty;
                HelperClassBL.SetParafairEnvValues(executionContext, utilities);
                TaskProcs taskProcs = new TaskProcs(utilities);
                List<Card> cardList = new List<Card>();
                Card card = new Card(accountServiceDTO.SourceAccountDTO.AccountId, executionContext.GetUserId(), utilities);
                CalculateTotalRefundAmount(card);
                if (!taskProcs.RefundCard(card, Convert.ToDouble(accountServiceDTO.SourceAccountDTO.FaceValue),
                          Convert.ToDouble(accountServiceDTO.SourceAccountDTO.TotalCreditsBalance),
                          Convert.ToDouble(accountServiceDTO.SourceAccountDTO.TotalCreditPlusBalance),
                          accountServiceDTO.Remarks, ref message, accountServiceDTO.MakeNewCardOnFullRefund,
                          parafaitDBTrx))
                {
                    message = "Error" + message;
                    log.LogMethodExit(message);
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private void CalculateTotalRefundAmount(Card card)
        {
            log.LogMethodEntry(card);
            CreditPlus creditPlus = new CreditPlus(utilities);
            double cardDeposit = 0;
            double totalCredits = 0;
            double totalCreditPlus = 0;
            double totalAmount = 0;
            double refundAmount = 0;
            double refund = 0;

            creditPlusAmount = creditPlus.getCreditPlusRefund(card.card_id);
            if (utilities.ParafaitEnv.ALLOW_REFUND_OF_CARD_DEPOSIT == "Y" && accountServiceDTO.MakeNewCardOnFullRefund)
            {
                cardDeposit += card.face_value;
                log.Debug("Allowed to Refund Card Deposit/Make Card New on Full Refund, CardDeposit:" + cardDeposit);
            }
            else
            {
                cardDeposit = 0;
                log.Debug("Not Allowed to Refund Card Deposit/Make Card New on Full Refund, CardDeposit:" + cardDeposit);
            }
            if (utilities.ParafaitEnv.ALLOW_REFUND_OF_CARD_CREDITS == "Y")
            {
                totalCredits += card.credits;
                log.Debug("Allowed to Refund Card Credits:" + creditsRefund);
            }

            if (utilities.ParafaitEnv.ALLOW_REFUND_OF_CREDITPLUS == "Y")
            {
                totalCreditPlus += creditPlusAmount;
                log.Debug("Allowed to Refund Credits Plus:" + creditPlusRefund);
            }

            try
            {
                totalAmount = Math.Round(totalCredits + totalCreditPlus + cardDeposit, 4, MidpointRounding.AwayFromZero);
                refundAmount = Math.Round(totalCredits + totalCreditPlus + cardDeposit, 4, MidpointRounding.AwayFromZero);
                refund = refundAmount;
                if (refundAmount > totalAmount)
                {
                    log.Error("Ends-calculateRefundAmounts as Refund amount exceeds Total Balance on card");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Refund amount exceeds Total Balance on card"));
                }
                else if (refundAmount > totalCredits + totalCreditPlus && refundAmount < totalAmount)
                {
                    log.Error("Ends-calculateRefundAmounts as Cannot refund Card Deposit partially");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Cannot refund Card Deposit partially"));
                }
            }
            catch
            {
                log.Error("Ends-calculateRefundAmounts as Entered refund amount was not valid");
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Entered refund amount was not valid"));
            }

            if (refundAmount <= totalCreditPlus)
            {
                accountServiceDTO.SourceAccountDTO.TotalCreditPlusBalance = Convert.ToDecimal(refundAmount);
                refundAmount = 0;
                log.Debug("Refund amount:" + refundAmount);//Added for logger function on 08-Mar-2016
            }
            else
            {
                accountServiceDTO.SourceAccountDTO.TotalCreditPlusBalance = Convert.ToDecimal(totalCreditPlus);
                refundAmount = refundAmount - totalCreditPlus;
                log.Debug("Refund amount:" + refundAmount);//Added for logger function on 08-Mar-2016
            }

            if (refundAmount <= totalCredits)
            {
                accountServiceDTO.SourceAccountDTO.TotalCreditsBalance = Convert.ToDecimal(refundAmount);
                refundAmount = 0;
                log.Debug("Refund amount:" + refundAmount);//Added for logger function on 08-Mar-2016
            }
            else
            {
                accountServiceDTO.SourceAccountDTO.TotalCreditsBalance = Convert.ToDecimal(totalCredits);
                refundAmount = refundAmount - totalCredits;
                log.Debug("Refund amount:" + refundAmount);//Added for logger function on 08-Mar-2016
            }

            if (refundAmount <= cardDeposit)
            {
                accountServiceDTO.SourceAccountDTO.FaceValue = Convert.ToDecimal(refundAmount);
                refundAmount = 0;
                log.Debug("Refund amount:" + refundAmount);//Added for logger function on 08-Mar-2016
            }
            else
            {
                accountServiceDTO.SourceAccountDTO.FaceValue = Convert.ToDecimal(cardDeposit);
                refundAmount = refundAmount - totalCredits;
                log.Debug("Refund amount:" + refundAmount);//Added for logger function on 08-Mar-2016
            }
            //log.Info("calculateRefundAmounts() - Total Refund amount: " + refAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));//Added for logger function on 08-Mar-2016
            log.Debug("Ends-calculateRefundAmounts()");//Added for logger function on 08-Mar-2016
            log.LogMethodExit();
        }


        /// <summary>
        /// Partial CardAmount Refund
        /// </summary>
        private void PartialCardAmountRefund(SqlTransaction parafaitDBTrx)
        {

            log.LogMethodEntry(parafaitDBTrx);
            string message = string.Empty;
            TaskProcs taskProcs = new TaskProcs(utilities);
            List<Card> cardIdList = new List<Card>();
            Card card = new Card(accountServiceDTO.SourceAccountDTO.AccountId, executionContext.GetUserId(), utilities, parafaitDBTrx);
            cardIdList.Add(card);
            double refundAmount = GetRefundAmount(cardIdList);
            try
            {
                if ((utilities.ParafaitEnv.ALLOW_REFUND_OF_CARD_CREDITS == "Y" ||
                       utilities.ParafaitEnv.ALLOW_REFUND_OF_CREDITPLUS == "Y") &&
                       utilities.ParafaitEnv.ALLOW_PARTIAL_REFUND == "Y")
                {

                    if (refundAmount > 0)
                    {
                        if (string.IsNullOrEmpty(accountServiceDTO.Remarks) && utilities.ParafaitEnv.REFUND_REMARKS_MANDATORY == "Y")
                        {
                            log.Error("REFUNDCARD- Enter remarks for Card Refund");
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 69));
                        }
                        decimal refundCardAmount = 0;
                        if (!HelperClassBL.ManagerApprovalCheck(refundCardAmount, TaskProcs.REFUNDCARD.ToString()))
                        {
                            throw new Exception(MessageContainerList.GetMessage(executionContext, "Manager Approval Check falied.Please check the Approval limit"));
                        }
                        if (!taskProcs.RefundCard(cardIdList, refundCardDeposit, creditsRefund, refundAmount,
                                                        accountServiceDTO.Remarks, ref message, accountServiceDTO.MakeNewCardOnFullRefund, parafaitDBTrx))
                        {
                            log.Error(message);
                            throw new Exception(MessageContainerList.GetMessage(executionContext, message));
                        }
                        log.Debug("REFUNDCARD- " + card + " Card Refunded successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private double GetRefundAmount(List<Card> inCard)
        {
            log.LogMethodEntry(inCard);
            CreditPlus creditPlus = new CreditPlus(utilities);
            foreach (var item in inCard)
            {
                creditPlusAmount = creditPlus.getCreditPlusRefund(item.card_id);
                if (utilities.ParafaitEnv.ALLOW_REFUND_OF_CARD_DEPOSIT == "Y" && accountServiceDTO.MakeNewCardOnFullRefund)
                {
                    refundCardDeposit += item.face_value;
                    log.Debug("RefundCard() - Allowed to Refund Card Deposit/Make Card New on Full Refund, CardDeposit:" + refundCardDeposit);
                }
                else
                {
                    refundCardDeposit = 0;
                    log.Debug("RefundCard() - Not Allowed to Refund Card Deposit/Make Card New on Full Refund, CardDeposit:" + refundCardDeposit);
                }
                if (utilities.ParafaitEnv.ALLOW_REFUND_OF_CARD_CREDITS == "Y")
                {
                    creditsRefund += item.credits;
                    log.Debug("RefundCard() - Allowed to Refund Card Credits:" + creditsRefund);
                }

                if (utilities.ParafaitEnv.ALLOW_REFUND_OF_CREDITPLUS == "Y")
                {
                    creditPlusRefund += creditPlusAmount;
                    log.Debug("RefundCard() - Allowed to Refund Credits Plus:" + creditPlusRefund);
                }
            }
            refundAmount = calculateRefundAmounts(refundCardDeposit, creditsRefund, creditPlusRefund);
            log.LogMethodExit(refundAmount);
            return refundAmount;

        }


        private double calculateRefundAmounts(double cardDeposit, double totalCredits, double totalCreditPlus)
        {
            log.LogMethodEntry(cardDeposit, totalCredits, totalCreditPlus);
            double totalAmount = 0, refundTaxAmount = 0, taxAmount = 0, refundAmount = 0;
            totalAmount = Math.Round(totalCredits + totalCreditPlus + cardDeposit, 4, MidpointRounding.AwayFromZero);
            try
            {
                refundAmount = Math.Round(Convert.ToDouble(accountServiceDTO.RefundAmount), 4, MidpointRounding.AwayFromZero);
                if (refundAmount > totalAmount)
                {
                    log.Error("Ends-calculateRefundAmounts as Refund amount exceeds Total Balance on card");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Refund amount exceeds Total Balance on card"));
                }
                else if (refundAmount > totalCredits + totalCreditPlus && refundAmount < totalAmount)
                {
                    log.Error("Ends-calculateRefundAmounts as Cannot refund Card Deposit partially");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Cannot refund Card Deposit partially"));
                }
            }
            catch
            {
                log.Error("Ends-calculateRefundAmounts as Entered refund amount was not valid");
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Entered refund amount was not valid"));
            }

            if (refundAmount <= totalCreditPlus)
            {
                refundAmount = 0;
                refundCardDeposit = 0;
                creditsRefund = 0;    //amount is lesser than the creditplus
                log.Debug("calculateRefundAmounts - Refund amount:" + refundAmount);
            }
            else
            {
                refundAmount = refundAmount - totalCreditPlus;
                log.Debug("calculateRefundAmounts - Refund amount:" + refundAmount);
            }

            if (refundAmount <= totalCredits)
            {
                refundAmount = 0;
                refundCardDeposit = 0; //amount is lesser than credits
                log.Debug("calculateRefundAmounts as Refund amount:" + refundAmount);
            }
            else
            {
                refundAmount = refundAmount - totalCredits;
                log.Debug("calculateRefundAmounts - Refund amount:" + refundAmount);
            }

            if (refundAmount <= cardDeposit)
            {
                refundAmount = 0;
                log.Debug("calculateRefundAmounts - Refund amount:" + refundAmount);
            }
            else
            {
                refundAmount = refundAmount - totalCredits;
                log.Debug("calculateRefundAmounts - Refund amount:" + refundAmount);
            }
            double refAmount = Convert.ToDouble(accountServiceDTO.RefundAmount);
            DataTable dtRefundTax = utilities.executeDataTable(@"select top 1 t.tax_id taxId ,TaxInclusivePrice, t.tax_percentage taxPercentage
                                                                 from Products p, product_type pt ,tax t
                                                                 where product_type = 'REFUND' 
                                                                 and p.product_type_id = pt.product_type_id
                                                                 and t.tax_id = p.tax_id");

            if (dtRefundTax.Rows.Count > 0)
            {
                if (dtRefundTax.Rows[0]["TaxInclusivePrice"].ToString() == "Y")
                {
                    refundTaxAmount = refAmount - cardDeposit;
                    taxAmount = (refundTaxAmount - refundTaxAmount / (1 + Convert.ToDouble(dtRefundTax.Rows[0]["taxPercentage"]) / 100));

                }
                else
                {
                    refundTaxAmount = refAmount - cardDeposit;
                    taxAmount = (refundTaxAmount * utilities.ParafaitEnv.RefundCardTaxPercent / 100);
                    refAmount = (refundTaxAmount * utilities.ParafaitEnv.RefundCardTaxPercent / 100) + refAmount;
                }
            }

            DataTable dtRefundDepositTax = utilities.executeDataTable(@"select top 1 t.tax_id taxId ,TaxInclusivePrice, t.tax_percentage taxPercentage
                                                                 from Products p, product_type pt ,tax t
                                                                 where product_type = 'REFUNDCARDDEPOSIT' 
                                                                 and p.product_type_id = pt.product_type_id
                                                                 and t.tax_id = p.tax_id");

            if (dtRefundDepositTax.Rows.Count > 0)
            {
                if (dtRefundDepositTax.Rows[0]["TaxInclusivePrice"].ToString() == "Y")
                {
                    taxAmount = (taxAmount + (cardDeposit - cardDeposit / (1 + Convert.ToDouble(dtRefundDepositTax.Rows[0]["taxPercentage"]) / 100)));
                }
                else
                {
                    taxAmount = (taxAmount + (cardDeposit * Convert.ToDouble(dtRefundDepositTax.Rows[0]["taxPercentage"]) / 100));
                    refAmount = (cardDeposit * Convert.ToDouble(dtRefundDepositTax.Rows[0]["taxPercentage"]) / 100) + refAmount;//
                }
            }
            log.Debug("calculateRefundAmounts - Total Refund amount: " + refAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
            log.LogMethodExit(refAmount);
            return refAmount;
        }


        /// <summary>
        ///  PartialCardRefund 
        /// </summary>
        private void PartialCardRefund(SqlTransaction parafaitDBTrx)
        {
            log.LogMethodEntry(parafaitDBTrx);
            try
            {
                string message = string.Empty;
                HelperClassBL.SetParafairEnvValues(executionContext, utilities);
                TaskProcs taskProcs = new TaskProcs(utilities);
                List<AccountGameDTO> accountGameDTOList = accountServiceDTO.SourceAccountDTO.RefundAccountGameDTOList;
                Card card = new Card(accountServiceDTO.SourceAccountDTO.AccountId, executionContext.GetUserId(), utilities);
                if (accountServiceDTO.SourceAccountDTO.RefundAccountGameDTOList != null &&
                                       accountServiceDTO.SourceAccountDTO.RefundAccountGameDTOList.Count > 0)
                {
                    foreach (AccountGameDTO accountGameDTO in accountServiceDTO.SourceAccountDTO.RefundAccountGameDTOList)
                    {
                        AccountGameBL accountGameBL = new AccountGameBL(executionContext, accountGameDTO.AccountGameId);
                        if (accountGameBL.AccountGameDTO != null && accountGameBL.AccountGameDTO.IsActive && accountGameBL.AccountGameDTO.BalanceGames > 0 && accountGameBL.AccountGameDTO.ExpiryDate > ServerDateTime.Now)
                        {
                            double refundGameAmount = HelperClassBL.GetRefundAmount(accountGameDTO.TransactionId, accountGameDTO.TransactionLineId);
                            if (!taskProcs.RefundCardGames(card, Convert.ToDouble(refundGameAmount), accountGameDTO.TransactionId, accountServiceDTO.Remarks, ref message))
                            {
                                message = "Error" + message;
                                log.LogMethodExit(message);
                                throw new Exception(message);
                            }
                        }
                    }
                }
                // refund time entitlements 
                if (accountServiceDTO.SourceAccountDTO.RefundAccountCreditPlusDTOList != null && accountServiceDTO.SourceAccountDTO.RefundAccountCreditPlusDTOList.Any())
                {
                    foreach (AccountCreditPlusDTO accountCreditPlusDTO in accountServiceDTO.SourceAccountDTO.RefundAccountCreditPlusDTOList)
                    {
                        AccountCreditPlusBL accountCreditPlusBL = new AccountCreditPlusBL(executionContext, accountCreditPlusDTO.AccountCreditPlusId, false, true, parafaitDBTrx);
                        if (accountCreditPlusBL.AccountCreditPlusDTO != null)
                        {
                            if (accountCreditPlusBL.AccountCreditPlusDTO.CreditPlusType == CreditPlusType.TIME && accountCreditPlusBL.AccountCreditPlusDTO.Refundable)
                            {
                                double refundTimeAmount = HelperClassBL.GetRefundAmount(accountCreditPlusBL.AccountCreditPlusDTO.TransactionId, accountCreditPlusBL.AccountCreditPlusDTO.TransactionLineId);
                                if (refundTimeAmount > 0)
                                {
                                    if (!taskProcs.RefundTime(card, Convert.ToDouble(refundTimeAmount), accountCreditPlusBL.AccountCreditPlusDTO.TransactionId,
                                                                                                        accountServiceDTO.Remarks, ref message))
                                    {
                                        message = "Error" + message;
                                        log.LogMethodExit(message);
                                        throw new Exception(message);
                                    }
                                }
                            }
                            else
                            {
                                double refundCreditAmount = HelperClassBL.GetRefundAmount(accountCreditPlusBL.AccountCreditPlusDTO.TransactionId, accountCreditPlusBL.AccountCreditPlusDTO.TransactionLineId);
                                if (refundCreditAmount > 0)
                                {
                                    if (!taskProcs.RefundCard(card, 0, 0, Convert.ToDouble(accountCreditPlusBL.AccountCreditPlusDTO.CreditPlusBalance),
                                                            accountServiceDTO.Remarks, ref message, accountServiceDTO.MakeNewCardOnFullRefund, parafaitDBTrx))
                                    {
                                        log.Error(message);
                                        throw new Exception(MessageContainerList.GetMessage(executionContext, message));
                                    }
                                }
                            }
                        }
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}

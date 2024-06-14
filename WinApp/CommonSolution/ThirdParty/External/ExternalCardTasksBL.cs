/********************************************************************************************
 * Project Name - ThirdParty
 * Description  - ExternalCardTasksBL class.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.140.5    19-Jan-2022   Abhishek                Created : External  REST API.
 ***************************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Semnox.Parafait.ThirdParty.External
{

    /// <summary>
    /// Account Task types
    /// </summary>
    public enum AccountTaskType
    {
        /// <summary>
        /// Balance Transfer
        /// </summary>
        BALANCE_TRANSFER,
        /// <summary>
        /// Legacy Transfer
        /// </summary>
        LEGACY_TRANSFER,
        /// <summary>
        /// Card Transfer
        /// </summary>
        TRANSFER_CARD,
        /// <summary>
        /// Hold Card
        /// </summary>
        HOLD_CARD,
        /// <summary>
        /// UnHold Card
        /// </summary>
        UNHOLD_CARD,
        /// <summary>
        /// UnHold Card
        /// </summary>
        DEACTIVATE_CARD,
    }

    /// <summary>
    /// Bussiness logic of the ExternalCardTasksBL class
    /// </summary>
    public class ExternalCardTasksBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private ExternalCardTasksDTO externalCardTasksDTO;

        /// <summary>
        /// Constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        private ExternalCardTasksBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="externalCardTasksDTO"></param>
        public ExternalCardTasksBL(ExecutionContext executionContext, ExternalCardTasksDTO externalCardTasksDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            string result = string.Empty;
            this.externalCardTasksDTO = externalCardTasksDTO;
        }

        /// <summary>
        /// Perform Account Tasks 
        /// </summary>
        public string PerformTask()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            switch (externalCardTasksDTO.AccountTaskType)
            {
                case AccountTaskType.TRANSFER_CARD:
                    {
                        result = TransferCard(externalCardTasksDTO);
                        break;
                    }
                //case AccountTaskType.LEGACY_TRANSFER:
                //    {
                //        result = new AccountLegacyTransferTaskBL(executionContext, accountTaskDTO, sqlTransaction);
                //        break;
                //    }
                case AccountTaskType.HOLD_CARD:
                    {
                        result = HoldCard(externalCardTasksDTO);
                        break;
                    }
                case AccountTaskType.UNHOLD_CARD:
                    {
                        result = HoldCard(externalCardTasksDTO);
                        break;
                    }
                //case AccountTaskType.BALANCE_TRANSFER:
                //    {
                //        result = new AccountBalanceTransferTaskBL(executionContext, accountTaskDTO, sqlTransaction);
                //        break;
                //    }
                default:
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 4442);// "Invalid product type";
                        log.LogMethodExit("Throwing Exception : " + errorMessage);
                        throw new ValidationException(errorMessage);
                    }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// HoldCard 
        /// </summary>
        /// <param name="externalCardTasksDTO"></param>
        public string HoldCard(ExternalCardTasksDTO externalCardTasksDTO)
        {
            log.LogMethodEntry(externalCardTasksDTO);
            Utilities utilities = GetUtility();
            string result = string.Empty;
            try
            {
                ValidateHoldCardDetails();
                Card cards = new Card(utilities);
                bool holdCard = false;
                cards = new Parafait.Transaction.Card(externalCardTasksDTO.SourceAccountNumber, executionContext.GetUserId(), utilities);
                if (externalCardTasksDTO.AccountTaskType == AccountTaskType.HOLD_CARD)
                {
                    holdCard = true;
                }
                string message = string.Empty;
                TaskProcs tp = new TaskProcs(utilities);
                bool holdEntitlements = tp.HoldEntitlements(cards, externalCardTasksDTO.Remarks, ref message, holdCard);
                if (holdEntitlements && holdCard)
                {
                    result = "Account Hold is Successful";
                }
                else if(holdEntitlements && !holdCard)
                {
                    result = "Account Unhold is Successful" + message;
                }
                else if (holdCard) 
                {
                    result = "Account Hold is Failed" + message;
                }
                else
                {
                    result = "Account Unhold is Failed" + message;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// TransferCard 
        /// </summary>
        /// <param name="externalCardTasksDTO"></param>
        public string TransferCard(ExternalCardTasksDTO externalCardTasksDTO)
        {
            log.LogMethodEntry(externalCardTasksDTO);
            Utilities utilities = GetUtility();
            string result = string.Empty;
            try
            {
                ValidateTransferCardDetails();
                Card fromCard = new Transaction.Card(utilities);
                fromCard = new Parafait.Transaction.Card(externalCardTasksDTO.SourceAccountNumber, executionContext.GetUserId(), utilities);
                Card toCard = new Transaction.Card(utilities);
                toCard = new Parafait.Transaction.Card(externalCardTasksDTO.DestinationAccountNumber, executionContext.GetUserId(), utilities);
                string message = string.Empty;
                TaskProcs tp = new TaskProcs(utilities);
                bool transferCard = tp.transferCard(fromCard, toCard, externalCardTasksDTO.Remarks, ref message);
                if (transferCard)
                {
                    result = "Account Transfer is Successful";
                }
                else
                {
                    result = "Account Transfer Failed" + message;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Validate TransferCardDetails
        /// </summary>
        public void ValidateTransferCardDetails()
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(externalCardTasksDTO.DestinationAccountNumber))
            {
                string message = MessageContainerList.GetMessage(executionContext, 458);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            if (string.IsNullOrEmpty(externalCardTasksDTO.SourceAccountNumber))
            {
                string message = MessageContainerList.GetMessage(executionContext, 67);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            AccountBL accountBL = new AccountBL(executionContext, externalCardTasksDTO.SourceAccountNumber);
            AccountDTO sourceAccountDTO = accountBL.AccountDTO;
            if (sourceAccountDTO == null || sourceAccountDTO.AccountId < 0)
            {
                string message = MessageContainerList.GetMessage(executionContext, 67);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            accountBL = new AccountBL(executionContext, externalCardTasksDTO.DestinationAccountNumber);
            AccountDTO destinationAccountDTO = accountBL.AccountDTO;
            if (destinationAccountDTO != null && destinationAccountDTO.AccountId > -1)
            {
                string message = MessageContainerList.GetMessage(executionContext, 58);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            if (sourceAccountDTO.TechnicianCard == "Y")
            {
                string message = MessageContainerList.GetMessage(executionContext, 1161);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate HoldCardDetails
        /// </summary>
        public void ValidateHoldCardDetails()
        {
            if (string.IsNullOrEmpty(externalCardTasksDTO.SourceAccountNumber))
            {
                string message = MessageContainerList.GetMessage(executionContext, 2204);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            AccountBL accountBL = new AccountBL(executionContext, externalCardTasksDTO.SourceAccountNumber);
            AccountDTO sourceAccountDTO = accountBL.AccountDTO;
            if (sourceAccountDTO == null || sourceAccountDTO.AccountId < 0)
            {
                string message = MessageContainerList.GetMessage(executionContext, "Invalid Card Number");
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            if (sourceAccountDTO.TechnicianCard == "Y")
            {
                string message = MessageContainerList.GetMessage(executionContext, 4738);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            if (externalCardTasksDTO.AccountTaskType == AccountTaskType.HOLD_CARD)
            {
                ThrowIfAccountGamesOnHold(sourceAccountDTO);
                ThrowIfAccountCreditPlusOnHold(sourceAccountDTO);
                ThrowIfAccountDiscountsOnHold(sourceAccountDTO);
            }
        }

        /// <summary>
        /// Validate Hold Status of Account Games
        /// </summary>
        protected void ThrowIfAccountGamesOnHold(AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            if (accountDTO != null)
            {
                if (accountDTO.AccountGameDTOList != null
                            && accountDTO.AccountGameDTOList.Exists(x => x.BalanceGames != 0
                                                                         && x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 2610);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ValidationException(message);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate Hold Status of Account Credit Plus
        /// </summary>
        protected void ThrowIfAccountCreditPlusOnHold(AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            if (accountDTO != null)
            {
                if (accountDTO.AccountCreditPlusDTOList != null
                      && accountDTO.AccountCreditPlusDTOList.Exists(x => x.CreditPlusBalance != 0
                                                                      && x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 2610);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ValidationException(message);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate Hold Status of Account Discounts
        /// </summary>
        protected void ThrowIfAccountDiscountsOnHold(AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            if (accountDTO != null)
            {
                if (accountDTO.AccountDiscountDTOList != null
                            && accountDTO.AccountDiscountDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 2610);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ValidationException(message);
                }
            }
            log.LogMethodExit();
        }

        internal Utilities GetUtility()
        {
            log.LogMethodEntry();
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
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
}

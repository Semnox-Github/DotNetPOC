/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - TaskRedeemBonousForTickets view model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     26-Apr-2021   Fiona                   Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI
{
    /// <summary>
    /// TaskRedeemBonousForTicketsVM
    /// </summary>
    public class TaskRedeemBonusForTicketsVM : TaskBaseViewModel
    {
        #region members
        private TaskRedeemBonusForTicketsView taskRedeemBonusForTicketsView;
        private TaskType taskType;
        private ICommand loaded;
        private ICommand navigationClickCommand;
        private ICommand cardAddedCommand;
        private ICommand toggleCheckedCommand;
        private ICommand onTextChangedCommand;
        private CardDetailsVM cardDetailsVM;
        private string moduleName;
        private string fromEntitlement;
        private string toEntitlement;
        private string chooseRedeemType;
        private string bonus;
        private string cardBalance;
        private string bonusTicketsAvailableHeading;
        private string bonusTicketsAvailable;
        private string bonusTicketsToRedeemHeading;
        private string bonusTicketsToRedeem;
        private string bonusTicketsEligibleHeading;
        private string bonusTicketsEligible;
        private Visibility redeemTypeVisibility;
        private Visibility headingArrowVisibility;
        private GenericToggleButtonsVM genericToggleButtonsVM;
        private string ticketEligibleValue;
        private string bonusEligibleValue;
        private string creditsEligibleValue;
        private double bonustoRedeem;
        private string amountFormat;




        private string ticketTermVariant;
        private string ticketsToRedeem;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        #region Properties
        public ICommand Loaded
        {
            set
            {
                log.LogMethodEntry(loaded, value);
                SetProperty(ref loaded, value);
                log.LogMethodExit(loaded);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loaded);
                return loaded;
            }
        }
        public ICommand OnTextChangedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(onTextChangedCommand);
                return onTextChangedCommand;
            }
            set
            {
                log.LogMethodEntry(onTextChangedCommand, value);
                SetProperty(ref onTextChangedCommand, value);
                log.LogMethodExit(onTextChangedCommand);
            }
        }
        public GenericToggleButtonsVM GenericToggleButtonsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericToggleButtonsVM);
                return genericToggleButtonsVM;
            }
        }
        public ICommand ToggleCheckedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(toggleCheckedCommand);
                return toggleCheckedCommand;
            }
            set
            {
                log.LogMethodEntry(toggleCheckedCommand, value);
                SetProperty(ref toggleCheckedCommand, value);
                log.LogMethodExit(toggleCheckedCommand);
            }
        }

        public string ModuleName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(moduleName);
                return moduleName;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref moduleName, value);
                }
                log.LogMethodExit();
            }
        }
        public string FromEntitlement
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(fromEntitlement);
                return fromEntitlement;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref fromEntitlement, value);
                }
                log.LogMethodExit();
            }
        }

        public string ToEntitlement
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(toEntitlement);
                return toEntitlement;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref toEntitlement, value);
                }
                log.LogMethodExit();
            }

        }
        public ICommand NavigationClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return navigationClickCommand;
            }
            set
            {
                log.LogMethodEntry();
                navigationClickCommand = value;
                log.LogMethodExit();
            }
        }

        public ICommand CardAddedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardAddedCommand);
                return cardAddedCommand;
            }
            set
            {
                log.LogMethodEntry(cardAddedCommand, value);
                SetProperty(ref cardAddedCommand, value);
                log.LogMethodExit(cardAddedCommand);
            }
        }

        public CardDetailsVM CardDetailsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardDetailsVM);
                return cardDetailsVM;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref cardDetailsVM, value);
                log.LogMethodExit();
            }
        }
        public string ChooseRedeemType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(chooseRedeemType);
                return chooseRedeemType;
            }
            set
            {
                log.LogMethodEntry();
                if (value != null)
                {
                    SetProperty(ref chooseRedeemType, value);
                }
                log.LogMethodExit();
            }
        }
        public string Bonus
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(bonus);
                return bonus;
            }
            set
            {
                log.LogMethodEntry();
                if (value != null)
                {
                    SetProperty(ref bonus, value);
                }
                log.LogMethodExit();
            }
        }
        public string CardBalance
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardBalance);
                return cardBalance;
            }
            set
            {
                log.LogMethodEntry();
                if (value != null)
                {
                    SetProperty(ref cardBalance, value);
                }
                log.LogMethodExit();
            }
        }
        public string BonusTicketsAvailableHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(bonusTicketsAvailableHeading);
                return bonusTicketsAvailableHeading;
            }
            set
            {
                log.LogMethodEntry();
                if (value != null)
                {
                    SetProperty(ref bonusTicketsAvailableHeading, value);
                }
                log.LogMethodExit();
            }
        }
        public string BonusTicketsAvailable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(bonusTicketsAvailable);
                return bonusTicketsAvailable;
            }
            set
            {
                log.LogMethodEntry();
                if (value != null)
                {
                    SetProperty(ref bonusTicketsAvailable, value);
                }
                log.LogMethodExit();
            }
        }
        public string BonusTicketsToRedeemHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(bonusTicketsToRedeemHeading);
                return bonusTicketsToRedeemHeading;
            }
            set
            {
                log.LogMethodEntry();
                if (value != null)
                {
                    SetProperty(ref bonusTicketsToRedeemHeading, value);
                }
                log.LogMethodExit();
            }
        }


        public string BonusTicketsToRedeem
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(bonusTicketsToRedeem);
                return bonusTicketsToRedeem;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref bonusTicketsToRedeem, value);
                    this.OnTextChanged(null);
                }
                log.LogMethodExit();
            }
        }


        public string BonusTicketsEligibleHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(bonusTicketsEligibleHeading);
                return bonusTicketsEligibleHeading;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref bonusTicketsEligibleHeading, value);
                }
                log.LogMethodExit();
            }
        }


        public string BonusTicketsEligible
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(bonusTicketsEligible);
                return bonusTicketsEligible;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref bonusTicketsEligible, value);
                }
                log.LogMethodExit();
            }
        }
        public Visibility HeadingArrowVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(headingArrowVisibility);
                return headingArrowVisibility;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref headingArrowVisibility, value);
                log.LogMethodExit();
            }
        }
        public Visibility RedeemTypeVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redeemTypeVisibility);
                return redeemTypeVisibility;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref redeemTypeVisibility, value);
                log.LogMethodExit();
            }
        }


        #endregion
        #region methods
        private void OnCardAdded(object parameter)
        {
            log.LogMethodEntry(parameter);
            HandleCardRead();
            log.LogMethodExit();
        }

        internal void HandleCardRead()
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);
            if (TappedAccountDTO != null)
            {
                CardDetailsVM.AccountDTO = TappedAccountDTO;
            }
            if (CardDetailsVM.AccountDTO == null)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 257, null), MessageType.Warning);
                return;
            }

            if(CardDetailsVM.AccountDTO.TechnicianCard=="Y")
            {
                ErrorMessage= MessageViewContainerList.GetMessage(ExecutionContext, 197, CardDetailsVM.AccountDTO.TagNumber);
                PerformClose(this.taskRedeemBonusForTicketsView);
                return;
            }
            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.AccountId >= 0)
            {
                SetFooterContent(string.Empty, MessageType.None);
            }
            else
            {
                if (taskType == TaskType.REDEEMBONUSFORTICKET)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1193, MessageType.Error), MessageType.Warning);
                }
                else if (taskType == TaskType.REDEEMTICKETSFORBONUS)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 267, MessageType.Error), MessageType.Warning);
                }
                CardDetailsVM.AccountDTO = null;
                return;
            }


            string numberFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT");
            if (taskType == TaskType.REDEEMTICKETSFORBONUS)
            {
                BonusTicketsAvailable = (CalculateTickets()).ToString(numberFormat);
                PoleDisplay.writeSecondLine(BonusTicketsAvailable + ticketTermVariant + " Available");
                BonusTicketsToRedeem = (CalculateTickets()).ToString();
            }
            else if (taskType == TaskType.REDEEMBONUSFORTICKET)
            {
                BonusTicketsAvailable = (CalculateBonus()).ToString(amountFormat);
                PoleDisplay.writeSecondLine(BonusTicketsAvailable +  "Bonus Available");
                BonusTicketsToRedeem = (CalculateBonus()).ToString(amountFormat);
            }
            log.LogMethodExit();
        }

        private double CalculateBonus()
        {
            log.LogMethodEntry();
            double bonus =Convert.ToDouble((CardDetailsVM.AccountDTO.Bonus != null ? CardDetailsVM.AccountDTO.Bonus : 0) + (CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusBonus!=null ? CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusBonus : 0));
            log.LogMethodExit(bonus);
            return bonus;
        }

        private int CalculateTickets()
        {
            log.LogMethodEntry();
            int? tickets = 0;
            if (cardDetailsVM.AccountDTO!=null)
            {
                tickets =Convert.ToInt32((CardDetailsVM.AccountDTO.TicketCount == null ? 0 : CardDetailsVM.AccountDTO.TicketCount)  +  (CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTickets == null ? 0: CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTickets));
            }
            log.LogMethodExit(tickets);
            return (int)tickets;
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            taskRedeemBonusForTicketsView = parameter as TaskRedeemBonusForTicketsView;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            Remarks = string.Empty;

            if (!ManagerApprovalCheck(taskType,parameter))
            {
                ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 268);
                PerformClose(parameter);
                return;
            }

            log.LogMethodExit();
        }
        private void OnClearCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            CardDetailsVM.AccountDTO = null;
            Remarks = string.Empty;
            BonusTicketsAvailable = "0";
            BonusTicketsToRedeem = "0";
            bonusEligibleValue = "0";
            creditsEligibleValue = "0";
            ticketEligibleValue = "0";
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
            SetFooterContent(string.Empty, MessageType.None);
            log.LogMethodExit();
        }
        private void OnNavigationClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                PerformClose(parameter);
            }
            log.LogMethodExit();
        }

        private bool ValidateBonusToRedeemValue()
        {
            log.LogMethodEntry();
            try
            {
                if (Convert.ToDouble(BonusTicketsToRedeem) > Convert.ToDouble(BonusTicketsAvailable))
                {
                    ticketEligibleValue = "0";
                    BonusTicketsEligible = "= " + ticketEligibleValue + " " + ticketTermVariant;
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1195, null), MessageType.Warning);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ticketEligibleValue = "0";
                BonusTicketsEligible = "= " + ticketEligibleValue + " " + ticketTermVariant;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 473, null), MessageType.Warning);
                log.LogMethodExit(false);
                return false;
            }

            log.LogMethodExit(true);
            return true;
        }

        private bool ValidateTicketToRedeemValue()
        {
            log.LogMethodEntry();
            try
            {
                if (Convert.ToInt32(BonusTicketsToRedeem) > CalculateTickets())
                {
                    ClearBonusCardBalanceEligibleValue();
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 50, ticketTermVariant, ticketTermVariant), MessageType.Warning);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            catch (Exception)
            {
                ClearBonusCardBalanceEligibleValue();
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 473, ticketTermVariant), MessageType.Warning);
                log.LogMethodExit(false);
                return false;
            }
            if(Convert.ToInt32(BonusTicketsToRedeem) > CalculateTickets())
            {
                ClearBonusCardBalanceEligibleValue();
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 50, ticketTermVariant, ticketTermVariant), MessageType.Warning);
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        private void ClearBonusCardBalanceEligibleValue()
        {
            log.LogMethodEntry();
            if (GenericToggleButtonsVM != null && GenericToggleButtonsVM.SelectedToggleButtonItem != null)
            {
                switch (GenericToggleButtonsVM.SelectedToggleButtonItem.Key.ToLower())
                {
                    case "bonus":
                        bonusEligibleValue = "0";
                        BonusTicketsEligible = "= " + bonusEligibleValue + " "+ MessageViewContainerList.GetMessage(ExecutionContext, "Bonus", null);
                        break;
                    case "cardbalance":
                        creditsEligibleValue = "0";
                        BonusTicketsEligible = "= " + creditsEligibleValue + " "+ MessageViewContainerList.GetMessage(ExecutionContext, "Card Balance", null);
                        break;
                }
            }
            log.LogMethodExit();
        }
        private void OnTextChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty, MessageType.None);
            if(CardDetailsVM.AccountDTO!=null && !string.IsNullOrEmpty(BonusTicketsToRedeem))
            {
                if (taskType == TaskType.REDEEMBONUSFORTICKET)
                {
                    bonusTicketsToRedeem = Convert.ToDecimal(BonusTicketsToRedeem).ToString(amountFormat);
                    if (ValidateBonusToRedeemValue())
                    {
                        ticketEligibleValue = (Convert.ToDouble(BonusTicketsToRedeem) * Convert.ToDouble(ticketsToRedeem)).ToString(amountFormat);
                        BonusTicketsEligible = "= " + ticketEligibleValue + " "+ ticketTermVariant;
                    }
                }
                else if (taskType == TaskType.REDEEMTICKETSFORBONUS)
                {
                    if(ValidateTicketToRedeemValue())
                    {
                        if (GenericToggleButtonsVM != null && GenericToggleButtonsVM.SelectedToggleButtonItem != null)
                        {
                            decimal ticketstoRedeemPerBonus = Convert.ToDecimal(ticketsToRedeem);
                            switch (GenericToggleButtonsVM.SelectedToggleButtonItem.Key.ToLower())
                            {

                                case "bonus":

                                    bonusEligibleValue = ((Convert.ToDecimal(BonusTicketsToRedeem)) / ticketstoRedeemPerBonus).ToString(amountFormat);
                                    BonusTicketsEligible = "= " + bonusEligibleValue + " "+ MessageViewContainerList.GetMessage(ExecutionContext, "Bonus", null);
                                    break;
                                case "cardbalance":
                                    string ticketstoRedeemPerCredit = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "TICKETS_TO_REDEEM_PER_CREDIT");
                                    if (string.IsNullOrEmpty(ticketstoRedeemPerCredit))
                                    {
                                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1170), MessageType.Error);
                                        creditsEligibleValue = "0";
                                        BonusTicketsEligible = "= " + 0.ToString(amountFormat) + " " + MessageViewContainerList.GetMessage(ExecutionContext, "Card Balance", null);
                                        return;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (Convert.ToInt32(ticketstoRedeemPerCredit) < 1)
                                            {
                                                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1170), MessageType.Error);
                                                creditsEligibleValue = "0";
                                                BonusTicketsEligible = "= " + 0.ToString(amountFormat) + " " + MessageViewContainerList.GetMessage(ExecutionContext, "Card Balance", null);
                                                return;
                                            }
                                        }
                                        catch
                                        {
                                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1170), MessageType.Error);
                                            creditsEligibleValue = "0";
                                            BonusTicketsEligible = "= " +0.ToString(amountFormat) + " " + MessageViewContainerList.GetMessage(ExecutionContext, "Card Balance", null);
                                            return;
                                        }
                                    }
                                    creditsEligibleValue = ((Convert.ToDecimal(BonusTicketsToRedeem))/ Convert.ToDecimal(ticketstoRedeemPerCredit)).ToString(amountFormat);
                                    BonusTicketsEligible = "= " + creditsEligibleValue + " "+ MessageViewContainerList.GetMessage(ExecutionContext, "Card Balance", null);
                                    break;

                            }
                        }
                    }
                }
            }
            else
            {
                if (taskType == TaskType.REDEEMBONUSFORTICKET)
                {
                    ticketEligibleValue = "0";
                    BonusTicketsEligible = "= "+ ticketEligibleValue + " "+ ticketTermVariant;
                }
                else if (taskType == TaskType.REDEEMTICKETSFORBONUS)
                {
                    ClearBonusCardBalanceEligibleValue();
                }
            }
            log.LogMethodExit();
        }
        private async void OnOkButtonClicked(object parameter)
        {
            log.LogMethodEntry();
            if (CardDetailsVM.AccountDTO == null)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 257, null), MessageType.Warning);
                return;
            }
            if (CardDetailsVM.AccountDTO.AccountId < 0)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 459, null), MessageType.Error);
                return;
            }
            RedeemEntitlementDTO redeemEntitlementDTO;
            RedeemEntitlementDTO result;
            ITaskUseCases taskUseCases = TaskUseCaseFactory.GetTaskUseCases(ExecutionContext);
            if (taskType == TaskType.REDEEMBONUSFORTICKET)
            {
                if(Convert.ToDouble(ticketEligibleValue) <= 0)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 3061, ticketTermVariant), MessageType.Error);
                    return;
                }

                string minimumBonus = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "MINIMUM_BONUS_VALUE_FOR_TICKET_REDEMPTION");
                if (!string.IsNullOrEmpty(minimumBonus))
                {
                    try
                    {
                        double minumumBonusValue = Convert.ToDouble(minimumBonus);
                        if (minumumBonusValue > Convert.ToDouble(BonusTicketsToRedeem))
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1196, minumumBonusValue), MessageType.Error);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, ex.Message), MessageType.Error);
                        return;
                    }
                }
                if (Convert.ToDouble(BonusTicketsToRedeem) > Convert.ToDouble(BonusTicketsAvailable))
                {
                    ticketEligibleValue = "0";
                    BonusTicketsEligible = "= " + ticketEligibleValue + " " + ticketTermVariant;
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1195, null), MessageType.Warning);
                    log.LogMethodExit();
                    return;
                }
                double bonustoredeem = Convert.ToDouble(BonusTicketsToRedeem);
                if (!ManagerApprovalLimitCheck(taskType, Convert.ToInt32(bonustoredeem),parameter))
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268), MessageType.Error);
                    return;
                }

                redeemEntitlementDTO = new RedeemEntitlementDTO(CardDetailsVM.AccountDTO.AccountId, ManagerId, RedeemEntitlementDTO.FromTypeEnum.BONUS, RedeemEntitlementDTO.FromTypeEnum.TICKETS, Convert.ToDecimal(BonusTicketsToRedeem), Remarks);
                try
                {
                    IsLoadingVisible = true;
                    result = await taskUseCases.RedeemEntitlements(redeemEntitlementDTO);
                    SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 1194);
                    SetFooterContent(SuccessMessage, MessageType.None);
                    PerformClose(taskRedeemBonusForTicketsView);
                    PoleDisplay.executionContext = ExecutionContext;
                    PoleDisplay.writeSecondLine(BonusTicketsToRedeem + " Bonus redeemed");
                }
                catch (ValidationException vex)
                {
                    log.Error(vex);
                    SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
                catch (UnauthorizedException uaex)
                {
                    log.Error(uaex);
                    IsLoadingVisible = false;
                    throw;
                }
                catch (ParafaitApplicationException pax)
                {
                    log.Error(pax);
                    SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
                finally
                {
                    IsLoadingVisible = false;
                }
            }
            else if(taskType== TaskType.REDEEMTICKETSFORBONUS)
            {
                double ticketToRedeem = 0;
                try
                {
                    ticketToRedeem = Convert.ToDouble(BonusTicketsToRedeem);
                }
                catch (Exception ex)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 473, ticketTermVariant), MessageType.Warning);
                    return;
                }

                if (ticketToRedeem <= 0)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 473, ticketTermVariant), MessageType.Error);
                    return;
                }
                if (!ManagerApprovalLimitCheck(taskType, Convert.ToInt32(ticketToRedeem),parameter))
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1213), MessageType.Error);
                    return;
                }

                try
                {
                    if (GenericToggleButtonsVM != null && GenericToggleButtonsVM.SelectedToggleButtonItem != null)
                    {
                        switch (GenericToggleButtonsVM.SelectedToggleButtonItem.Key.ToLower())
                        {
                            case "bonus":
                                double bonusCardBalanceEligible = Convert.ToDouble(bonusEligibleValue);
                                //if (bonusCardBalanceEligible <= 0)
                                //{
                                //    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 3063, null), MessageType.Error);//'Bonus Eligible is less than 1'
                                //    return;
                                //}
                                
                                if (bonusCardBalanceEligible > ParafaitDefaultViewContainerList.GetParafaitDefault<double>(ExecutionContext, "LOAD_BONUS_LIMIT"))
                                {
                                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 43, ParafaitDefaultViewContainerList.GetParafaitDefault<double>(ExecutionContext, "LOAD_BONUS_LIMIT").ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_FORMAT"))), MessageType.Error);
                                    return;
                                }
                                IsLoadingVisible = true;
                                redeemEntitlementDTO = new RedeemEntitlementDTO(CardDetailsVM.AccountDTO.AccountId, ManagerId, RedeemEntitlementDTO.FromTypeEnum.TICKETS, RedeemEntitlementDTO.FromTypeEnum.BONUS, Convert.ToDecimal(BonusTicketsToRedeem), Remarks);
                                result = await taskUseCases.RedeemEntitlements(redeemEntitlementDTO);
                                SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 47,ticketTermVariant);
                                SetFooterContent(SuccessMessage, MessageType.None);
                                IsLoadingVisible = false;
                                PerformClose(taskRedeemBonusForTicketsView);
                                PoleDisplay.executionContext = ExecutionContext;
                                PoleDisplay.writeSecondLine(BonusTicketsToRedeem + ticketTermVariant + " redeemed");
                                break;
                            case "cardbalance":
                                 //if (Convert.ToDecimal(creditsEligibleValue ) <= 0)
                                //{
                                //    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 3062, null) , MessageType.Error);//Card Balance Eligible is less than 1
                                //    return;
                                //}
                                string ticketstoRedeemPerCredit = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "TICKETS_TO_REDEEM_PER_CREDIT");
                                if (string.IsNullOrEmpty(ticketstoRedeemPerCredit))
                                {
                                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1170), MessageType.Error);
                                    return;
                                }
                                else
                                {
                                    try
                                    {
                                        if (Convert.ToInt32(ticketstoRedeemPerCredit) < 1)
                                        {
                                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1170), MessageType.Error);
                                            return;
                                        }
                                    }
                                    catch
                                    {
                                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1170), MessageType.Error);
                                        return;
                                    }
                                }
                                IsLoadingVisible = true;
                                redeemEntitlementDTO = new RedeemEntitlementDTO(CardDetailsVM.AccountDTO.AccountId, ManagerId, RedeemEntitlementDTO.FromTypeEnum.TICKETS, RedeemEntitlementDTO.FromTypeEnum.CREDITS, Convert.ToDecimal(BonusTicketsToRedeem), Remarks);
                                result = await taskUseCases.RedeemEntitlements(redeemEntitlementDTO);
                                SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 1171, ticketTermVariant);
                                SetFooterContent(SuccessMessage, MessageType.None);
                                IsLoadingVisible = false;
                                PerformClose(taskRedeemBonusForTicketsView);
                                PoleDisplay.executionContext = ExecutionContext;
                                PoleDisplay.writeSecondLine(BonusTicketsToRedeem + ticketTermVariant+ " redeemed");
                                break;
                        }
                    }
                }
                catch (ValidationException vex)
                {
                    log.Error(vex);
                    SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
                catch (UnauthorizedException uaex)
                {
                    log.Error(uaex);
                    IsLoadingVisible = false;
                    throw;
                }
                catch (ParafaitApplicationException pax)
                {
                    log.Error(pax);
                    SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
                finally
                {
                    IsLoadingVisible = false;
                }
            }
            log.LogMethodExit();
        }
        private void getTicketToRedeemperBonus()
        {
            log.LogMethodEntry();
            try
            {
                ticketsToRedeem = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "TICKETS_TO_REDEEM_PER_BONUS");
                if (string.IsNullOrEmpty(ticketsToRedeem) || Convert.ToDecimal(ticketsToRedeem) <= 0)
                {
                    ticketsToRedeem = "100";
                }
            }
            catch (Exception)
            {
                ticketsToRedeem = "100";
            }
            log.LogMethodExit();
        }

        private void OnToggleChecked(object parameter)
        {
            log.LogMethodEntry(parameter);
            Remarks = string.Empty;
            SetFooterContent(string.Empty, MessageType.None);

            if (GenericToggleButtonsVM != null && GenericToggleButtonsVM.SelectedToggleButtonItem != null)
            {
                switch (GenericToggleButtonsVM.SelectedToggleButtonItem.Key.ToLower())
                {
                    case "bonus":
                        if(BonusTicketsAvailable == "0")
                        {
                            string numberFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT");
                            BonusTicketsAvailable = (CalculateTickets()).ToString(numberFormat);
                        }
                        bonusEligibleValue = ((Convert.ToDecimal(BonusTicketsToRedeem)) / Convert.ToDecimal(ticketsToRedeem)).ToString(amountFormat);
                        BonusTicketsEligible = "= " + bonusEligibleValue + " " + MessageViewContainerList.GetMessage(ExecutionContext, "Bonus", null);
                        break;
                    case "cardbalance":
                        string ticketstoRedeemPerCredit;
                        try
                        {
                            ticketstoRedeemPerCredit = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "TICKETS_TO_REDEEM_PER_CREDIT");
                            if (string.IsNullOrEmpty(ticketstoRedeemPerCredit) || Convert.ToDecimal(ticketstoRedeemPerCredit) < 1)
                            {
                                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1170), MessageType.Error);
                                BonusTicketsAvailable = "0";
                                BonusTicketsToRedeem = "0";
                                creditsEligibleValue = "0";
                                BonusTicketsEligible = "= "+ creditsEligibleValue+ " "+ MessageViewContainerList.GetMessage(ExecutionContext, "Card Balance", null);
                                return;
                            }
                        }
                        catch (Exception)
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1170), MessageType.Error);
                            BonusTicketsAvailable = "0";
                            BonusTicketsToRedeem = "0";
                            creditsEligibleValue = "0";
                            BonusTicketsEligible = "= " + creditsEligibleValue + " "+ MessageViewContainerList.GetMessage(ExecutionContext, "Card Balance", null);
                            return;
                        }
                        creditsEligibleValue = ((Convert.ToDecimal(BonusTicketsToRedeem)) / Convert.ToDecimal(ticketstoRedeemPerCredit)).ToString(amountFormat);
                        BonusTicketsEligible = "= " + (creditsEligibleValue) + " "+ MessageViewContainerList.GetMessage(ExecutionContext, "Card Balance", null);
                        break;
                }
            }
            log.LogMethodExit();
        }
        private void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            (NavigationClickCommand as DelegateCommand).RaiseCanExecuteChanged();
            (OkCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ClearCommand as DelegateCommand).RaiseCanExecuteChanged();
            log.LogMethodExit();
        }
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            switch (e.PropertyName)
            {
                case "IsLoadingVisible":
                    if (sender is CardDetailsVM)
                    {
                        IsLoadingVisible = cardDetailsVM.IsLoadingVisible;
                    }
                    else
                    {
                        RaiseCanExecuteChanged();
                    }
                    break;
            }
            log.LogMethodExit();
        }
        #endregion
        #region Constructor
        /// <summary>
        /// TaskRedeemBonousForTicketsVM
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="taskType"></param>
        /// <param name="cardReader"></param>
        public TaskRedeemBonusForTicketsVM(ExecutionContext executionContext, TaskType taskType, DeviceClass cardReader) : base(executionContext, cardReader)
        {
            log.Info("TaskRedeemBonousForTickets screen is opened");
            log.LogMethodEntry();
            this.ExecutionContext = executionContext;
            this.taskType = taskType;
            creditsEligibleValue = "0";
            bonusEligibleValue = "0";
            amountFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_FORMAT");

            getTicketToRedeemperBonus();
            Loaded = new DelegateCommand(OnLoaded);
            CardAddedCommand = new DelegateCommand(OnCardAdded);
            base.CardTappedEvent += HandleCardRead;
            ClearCommand = new DelegateCommand(OnClearCommand, ButtonEnable);
            PropertyChanged += OnPropertyChanged;
            CardDetailsVM = new CardDetailsVM(ExecutionContext);
            CardDetailsVM.PropertyChanged += OnPropertyChanged;
            OnTextChangedCommand = new DelegateCommand(OnTextChanged);

            ticketTermVariant = MessageViewContainerList.GetMessage(ExecutionContext, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"));

            if (taskType == TaskType.REDEEMBONUSFORTICKET)
            {
                ModuleName = MessageViewContainerList.GetMessage(executionContext, "Redeem", null);
                FromEntitlement = MessageViewContainerList.GetMessage(executionContext, "Bonus", null);
                ToEntitlement = ticketTermVariant;
                BonusTicketsAvailableHeading = MessageViewContainerList.GetMessage(executionContext, "Bonus Available", null);
                BonusTicketsToRedeemHeading = MessageViewContainerList.GetMessage(executionContext, "Bonus to Redeem", null);
                HeadingArrowVisibility= Visibility.Visible;
                RedeemTypeVisibility = Visibility.Collapsed;
            }
            else if (taskType == TaskType.REDEEMTICKETSFORBONUS)
            {
                ModuleName = MessageViewContainerList.GetMessage(executionContext, "Redeem "+ ticketTermVariant, null);
                BonusTicketsAvailableHeading = MessageViewContainerList.GetMessage(executionContext, "Tickets Available", null).Replace("Tickets", ticketTermVariant);
                BonusTicketsToRedeemHeading = MessageViewContainerList.GetMessage(executionContext, "Tickets to Redeem", null).Replace("Tickets",ticketTermVariant);
                HeadingArrowVisibility = Visibility.Collapsed;
                RedeemTypeVisibility = Visibility.Visible;
                ObservableCollection<CustomToggleButtonItem> toggleButtonItems = new ObservableCollection<CustomToggleButtonItem>();
                toggleButtonItems.Add(new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(this.ExecutionContext, "Bonus",null),
                            FontWeight = FontWeights.Bold,
                            TextSize = TextSize.Small,
                            HorizontalAlignment = HorizontalAlignment.Center
                        }
                    },
                    Key = "Bonus"
                });
                toggleButtonItems.Add(new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(this.ExecutionContext,"Card Balance",null),
                            FontWeight = FontWeights.Bold,
                            TextSize = TextSize.Small,
                            HorizontalAlignment = HorizontalAlignment.Center
                        }
                    },
                    Key = "CardBalance"
                });
                genericToggleButtonsVM = new GenericToggleButtonsVM()
                {
                    ToggleButtonItems = toggleButtonItems,
                    Columns=2
                };
                ToggleCheckedCommand = new DelegateCommand(OnToggleChecked);
                ChooseRedeemType = MessageViewContainerList.GetMessage(executionContext, "Choose Redeem Type", null);
                Bonus = MessageViewContainerList.GetMessage(executionContext, "Bonus", null);
                CardBalance = MessageViewContainerList.GetMessage(executionContext, "Card Balance", null);
            }
            NavigationClickCommand = new DelegateCommand(OnNavigationClick, ButtonEnable);
            OkCommand = new DelegateCommand(OnOkButtonClicked,ButtonEnable);
        }
        #endregion
    }
}

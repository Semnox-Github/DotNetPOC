/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Base View model for Tasks
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     18-Mar-2021   Amitha Joy            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.AccountsUI
{
    public enum TaskType
    {
        EXCHANGETOKENFORCREDIT = 0,
        EXCHANGECREDITFORTOKEN = 1,
        REDEEMTICKETSFORBONUS = 2,
        REDEEMBONUSFORTICKET = 3,
        EXCHANGECREDITFORTIME = 4,
        EXCHANGETIMEFORCREDIT = 5,
        LOADTICKETS = 6,
        LOADBONUS = 7,
        REDEEMLOYALTY = 8,
        REALETICKET = 9,
        BALANCETRANSFER = 10,
        CONSOLIDATE = 11,
        PAUSETIMEENTITLEMENT=12
    }
    public class TaskBaseViewModel : ViewModelBase
    {
        #region Members
        private string remarks;
        private string tappedCardNumber;
        private int managerId;
        private bool isRemarkMandatory;
        private bool showRemark;
        private string errorMessage;
        private string successMessage;
        private AccountDTO tappedAccountDTO;
        private DeviceClass cardReader;
        private FooterVM footerVM;
        private ICommand clearCommand;
        private ICommand okCommand;
        public delegate void CardTapped();
        public event CardTapped CardTappedEvent;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion
        #region Properties
        public string ErrorMessage
        {
            set
            {
                log.LogMethodEntry(errorMessage, value);
                SetProperty(ref errorMessage, value);
                log.LogMethodExit(errorMessage);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(errorMessage);
                return errorMessage;
            }
        }
        public string SuccessMessage
        {
            set
            {
                log.LogMethodEntry(successMessage, value);
                SetProperty(ref successMessage, value);
                log.LogMethodExit(successMessage);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(successMessage);
                return successMessage;
            }
        }
        public string Remarks
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(remarks);
                return remarks;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref remarks, value);
                log.LogMethodExit(remarks);
            }
        }
        public bool ShowRemark
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showRemark);
                return showRemark;
            }
            set
            {
                log.LogMethodEntry(showRemark, value);
                SetProperty(ref showRemark, value);
                log.LogMethodExit(showRemark);
            }
        }
        public int ManagerId
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(managerId);
                return managerId;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref managerId, value);
                log.LogMethodExit(managerId);
            }
        }
        public bool IsRemarkMandatory
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isRemarkMandatory);
                return isRemarkMandatory;
            }
            set
            {
                log.LogMethodEntry(isRemarkMandatory, value);
                SetProperty(ref isRemarkMandatory, value);
                log.LogMethodExit(isRemarkMandatory);
            }
        }
        public string TappedCardNumber
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tappedCardNumber);
                return tappedCardNumber;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref tappedCardNumber, value);
                log.LogMethodExit(tappedCardNumber);
            }
        }
        public AccountDTO TappedAccountDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tappedAccountDTO);
                return tappedAccountDTO;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref tappedAccountDTO, value);
                log.LogMethodExit(tappedAccountDTO);
            }
        }
        public DeviceClass CardReader
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardReader);
                return cardReader;
            }
            set
            {
                log.LogMethodEntry(cardReader, value);
                SetProperty(ref cardReader, value);
                log.LogMethodExit(cardReader);
            }
        }
        public FooterVM FooterVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(footerVM);
                return footerVM;
            }
            set
            {
                log.LogMethodEntry(footerVM, value);
                SetProperty(ref footerVM, value);
                log.LogMethodExit(footerVM);
            }
        }
        public ICommand ClearCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(clearCommand);
                return clearCommand;
            }
            set
            {
                log.LogMethodEntry(clearCommand, value);
                SetProperty(ref clearCommand, value);
                log.LogMethodExit(clearCommand);
            }
        }
        public ICommand OkCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(okCommand);
                return okCommand;
            }
            set
            {
                log.LogMethodEntry(okCommand, value);
                SetProperty(ref okCommand, value);
                log.LogMethodExit(okCommand);
            }
        }
        #endregion
        public void PerformClose(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                Window window = parameter as Window;
                if (window != null)
                {
                    UnRegisterReader();
                    window.Close();
                }
            }
            log.LogMethodExit();
        }
        internal void UnRegisterReader()
        {
            log.LogMethodEntry();
            if (this.cardReader != null)
            {
                log.Debug("Card Reader: " + cardReader);
                cardReader.UnRegister();
            }
            log.LogMethodExit();
        }
        #region Methods
        private async void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                DeviceClass deviceClass = sender as DeviceClass;
                log.Debug("Scanned Number: " + checkScannedEvent.Message);
                TagNumberViewParser tagNumberViewParser = new TagNumberViewParser(ExecutionContext);
                TagNumberView tagNumberView;
                if (tagNumberViewParser.TryParse(checkScannedEvent.Message, out tagNumberView) == false)
                {
                    errorMessage = tagNumberViewParser.Validate(checkScannedEvent.Message);
                    SetFooterContent(errorMessage, MessageType.Warning);
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }
                tappedCardNumber = tagNumberView.Value;
                try
                {
                    await GetAccountDetails(tagNumberView.Value, deviceClass.TagSiteId);
                    if (CardTappedEvent != null)
                    {
                        CardTappedEvent.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    SetFooterContent(ex.Message, MessageType.Error);
                }
            }
            log.LogMethodExit();
        }
        public void SetFooterContent(string message, MessageType messageType)
        {
            log.LogMethodEntry(message, messageType);
            if (footerVM != null)
            {
                this.footerVM.Message = message;
                this.footerVM.MessageType = messageType;
            }
            log.LogMethodExit();
        }
        private async Task<AccountDTO> GetAccountDetails(string cardNumber, int cardSiteId)
        {
            log.LogMethodEntry(cardNumber, cardSiteId);
            tappedAccountDTO = null;
            AccountDTOCollection accountDTOCollection;
            try
            {
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(ExecutionContext);
                accountDTOCollection = await accountUseCases.GetAccounts(accountNumber: cardNumber, tagSiteId: cardSiteId, buildChildRecords: true, activeRecordsOnly: true);
                if (accountDTOCollection != null && accountDTOCollection.data != null)
                {
                    tappedAccountDTO = accountDTOCollection.data[0];
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                SetFooterContent(vex.ToString(), MessageType.Error);
            }
            catch (UnauthorizedException uaex)
            {
                log.Info("unauthroized exception while retreiving card info - show relogin");
                //ShowRelogin(this.ExecutionContext.GetUserId());
                throw;
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                SetFooterContent(pax.ToString(), MessageType.Error);
            }
            catch (Exception ex)
            {
                string message = MessageViewContainerList.GetMessage(ExecutionContext, 2914, cardNumber);
                log.Error(message, ex);
                this.SetFooterContent(message + ex.Message, MessageType.Error);
            }
            log.LogMethodExit(tappedAccountDTO);
            return tappedAccountDTO;
        }
        public int FetchManagerApprovalLimit(TaskType taskType)
        {
            log.LogMethodEntry(taskType);
            int result = 0;
            switch (taskType)
            {
                case TaskType.EXCHANGETOKENFORCREDIT:
                    result = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(this.ExecutionContext, "TOKEN_FOR_CREDIT_LIMIT_FOR_MANAGER_APPROVAL", 0);
                    break;
                case TaskType.LOADTICKETS:
                    result = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(this.ExecutionContext, "LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL", 0);
                    break;
                case TaskType.LOADBONUS:
                    result = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(this.ExecutionContext, "LOAD_BONUS_LIMIT_FOR_MANAGER_APPROVAL", 0);
                    break;
                case TaskType.REDEEMTICKETSFORBONUS:
                    result = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(this.ExecutionContext, "REDEEM_TICKET_LIMIT_FOR_MANAGER_APPROVAL", 0);
                    break;
                case TaskType.REDEEMBONUSFORTICKET:
                    result = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(this.ExecutionContext, "REDEEM_BONUS_LIMIT_FOR_MANAGER_APPROVAL", 0);
                    break;
                case TaskType.REDEEMLOYALTY:
                    result = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(this.ExecutionContext, "REDEEM_LOYALTY_LIMIT_FOR_MANAGER_APPROVAL", 0);
                    break;
            }
            log.LogMethodExit(result);
            return result;
        }
        public bool ManagerApprovalLimitCheck(TaskType taskType, int taskValue,object sender)
        {
            bool result = false;
            log.LogMethodEntry(taskType, taskValue);
            int mgtLimitValue = FetchManagerApprovalLimit(taskType);
            if (mgtLimitValue > 0 && taskValue > mgtLimitValue)
            {
                managerId = -1;
                result = ShowManagerApproval(sender);
            }
            else
            {
                result = true;
            }
            log.LogMethodExit(result);
            if (!result)
            {
                this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268), MessageType.Error);
            }
            return result;
        }
        public bool ShowManagerApproval(object sender)
        {
            bool result = false;
            log.LogMethodEntry();
            if (!UserViewContainerList.IsSelfApprovalAllowed(this.ExecutionContext))
            {
                AuthenticateManagerView managerView = new AuthenticateManagerView();
                AuthenticateManagerVM managerVM = new AuthenticateManagerVM(this.ExecutionContext, this.cardReader);
                managerView.DataContext = managerVM;
                if (sender != null)
                {
                    managerView.Owner = sender as Window;
                }
                managerView.ShowDialog();
                if (managerVM.IsValid)
                {
                    managerId = Convert.ToInt32(managerVM.ManagerId);
                    result = true;
                }
                else
                {
                    managerId = -1;
                    result = false;
                }
            }
            else
            {
                managerId = Convert.ToInt32(this.ExecutionContext.GetUserPKId());
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }

        public bool ManagerApprovalCheck(TaskType taskType,object sender)
        {
            bool result = false;
            log.LogMethodEntry(taskType);
            managerId = -1;
            List<TaskTypesContainerDTO> taskTypes = TaskTypesViewContainerList.GetTaskTypesContainerDTOList(ExecutionContext);
            if (taskTypes.FirstOrDefault(x => x.TaskType == taskType.ToString()).RequiresManagerApproval == "Y")
            {
                result = ShowManagerApproval(sender);
            }
            else
            {
                managerId = -1;
                result = true;
            }
            log.LogMethodExit(result);
            if (!result)
            {
                this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268), MessageType.Error);
            }
            return result;
        }
        #endregion
        #region Constructor
        public TaskBaseViewModel(ExecutionContext executioncontext, DeviceClass cardReader)
        {
            log.LogMethodEntry(executioncontext);
            this.ExecutionContext = executioncontext;
            this.showRemark = true;
            this.FooterVM = new FooterVM(this.ExecutionContext)
            {
                Message = string.Empty,
                MessageType = MessageType.None,
                HideSideBarVisibility = Visibility.Collapsed,
            };
            this.cardReader = cardReader;
            if (this.cardReader != null)
            {
                log.Debug("Card Reader: " + cardReader);
                cardReader.Register(new EventHandler(CardScanCompleteEventHandle));
            }
            log.LogMethodExit();
        }
        #endregion
    }
}

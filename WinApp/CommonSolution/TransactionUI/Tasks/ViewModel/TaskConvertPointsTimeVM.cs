/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - TaskConvertPointsTime view model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     30-Mar-2021   Fiona                   Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI
{
    public class TaskConvertPointsTimeVM : TaskBaseViewModel
    {
        #region Members
        private string moduleName;
        private ICommand confirmCommand;
        private string pointsOrTimeAvailable;
        private string pointsOrTimeToConvert;
        private string heading1;
        private string heading2;
        private string fromEntitlement;
        private string toEntitlement;
        private TaskType taskType;
        private string totalPointsOrTime;
        private CardDetailsVM cardDetailsVM;
        private ICommand backButtonCommand;
        private ICommand cardAddedCommand;
        private TaskConvertPointsTimeView taskConvertPointsTimeView;
        private ICommand loaded;
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

        public ICommand BackButtonCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(backButtonCommand);
                return backButtonCommand;
            }
            set
            {
                log.LogMethodEntry(backButtonCommand, value);
                SetProperty(ref backButtonCommand, value);
                log.LogMethodExit(backButtonCommand);
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


        public string ExchangeCreditTimeHeading1
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(heading1);
                return heading1;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref heading1, value);
                }
                log.LogMethodExit();
            }
        }


        public string ExchangeCreditTimeHeading2
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(heading2);
                return heading2;
            }

            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref heading2, value);
                }
                log.LogMethodExit();
            }
        }


        public string PointsOrTimeAvailable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(pointsOrTimeAvailable);
                return pointsOrTimeAvailable;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref pointsOrTimeAvailable, value);
                }
                log.LogMethodExit();
            }
        }


        public string PointsOrTimeToConvert
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(pointsOrTimeToConvert);
                return pointsOrTimeToConvert;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref pointsOrTimeToConvert, value);
                    this.OnTextChanged(taskType);
                }
                log.LogMethodExit();
            }
        }
        public string TotalPointsOrTime
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(totalPointsOrTime);
                return totalPointsOrTime;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref totalPointsOrTime, value);
                }
                log.LogMethodExit();
            }
        }
        public ICommand ConfirmCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(confirmCommand);
                return confirmCommand;
            }
            set
            {
                log.LogMethodEntry();
                confirmCommand = value;
                log.LogMethodExit();
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
                cardDetailsVM = value;
                log.LogMethodExit();
            }
        }
        #endregion

        #region Methods

        private void OnClearCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            CardDetailsVM.AccountDTO = null;
            Remarks = string.Empty;
            PointsOrTimeToConvert = "0";
            PointsOrTimeAvailable = "0";
            TotalPointsOrTime = "";
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
            SetFooterContent(string.Empty, MessageType.None);
            log.LogMethodExit();
        }
        private void OnBackClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            PerformClose(parameter);
            log.LogMethodExit();
        }
        private double AvailableTime()
        {
            log.LogMethodEntry();
            double totalTime = 0;
            if (cardDetailsVM.AccountDTO.Time != null)
            {
                totalTime += (double)cardDetailsVM.AccountDTO.Time;
            }
            if (cardDetailsVM.AccountDTO.AccountSummaryDTO != null)
            {
                if (cardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTime != null)
                {
                    totalTime += (double)cardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusTime;
                }
            }
            log.LogMethodExit(totalTime);
            return totalTime;
        }
        private double AvailableCredits()
        {
            log.LogMethodEntry();
            double totalCredits = 0;
            if (cardDetailsVM.AccountDTO.Credits != null)
            {
                totalCredits += (double)cardDetailsVM.AccountDTO.Credits;
            }
            if (cardDetailsVM.AccountDTO.AccountSummaryDTO != null)
            {
                if (cardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusCardBalance != null)
                {
                    totalCredits += (double)cardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusCardBalance;
                }
                if(cardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits != null)
                {
                    totalCredits += (double)cardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits;
                }
            }
            log.LogMethodExit(totalCredits);
            return totalCredits;
        }


        private void OnTextChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty,MessageType.None);
            string timePerCredit = getTimePerCredits();
            decimal timePerCredits = Convert.ToDecimal(timePerCredit);
            if (taskType == TaskType.EXCHANGETIMEFORCREDIT)
            {
                if (PointsOrTimeToConvert == "0" || PointsOrTimeToConvert == string.Empty)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1442, null), MessageType.Info);
                    if(CardDetailsVM.AccountDTO != null)
                    {
                        PointsOrTimeAvailable = AvailableTime().ToString();
                    }
                    TotalPointsOrTime = " = 0 " + MessageViewContainerList.GetMessage(ExecutionContext, "Credits", null);
                    return;
                }
                int time = Convert.ToInt32(PointsOrTimeToConvert);
                if (CardDetailsVM.AccountDTO != null)
                {
                    if (Convert.ToDouble(PointsOrTimeToConvert) > AvailableTime())
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1441, AvailableTime()), MessageType.Error);
                        PointsOrTimeAvailable = AvailableTime().ToString();
                        TotalPointsOrTime = " = 0 " + MessageViewContainerList.GetMessage(ExecutionContext, "Credits", null);
                        return;
                    }
                    PointsOrTimeAvailable = Convert.ToInt32((AvailableTime() - time)).ToString();
                    TotalPointsOrTime = " = " + (Convert.ToInt32((time / timePerCredits))) + " " + MessageViewContainerList.GetMessage(ExecutionContext, "Credits", null);
                }
                else
                {
                    TotalPointsOrTime = " = 0 "  + MessageViewContainerList.GetMessage(ExecutionContext, "Credits", null);
                }
            }
            else if (taskType == TaskType.EXCHANGECREDITFORTIME)
            {
                if (PointsOrTimeToConvert == "0" || PointsOrTimeToConvert == string.Empty)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1382, null), MessageType.Info);
                    if (CardDetailsVM.AccountDTO != null)
                    {
                        PointsOrTimeAvailable = AvailableCredits().ToString();
                    }
                    TotalPointsOrTime = " = 0 " + MessageViewContainerList.GetMessage(ExecutionContext, "Minutes", null);
                    return;
                }
                int pointsToConvert = Convert.ToInt32(PointsOrTimeToConvert);
                if (CardDetailsVM.AccountDTO != null)
                {
                    if (Convert.ToDouble(PointsOrTimeToConvert) > AvailableCredits())
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1383, AvailableCredits()), MessageType.Error);
                        PointsOrTimeAvailable = AvailableCredits().ToString();
                        TotalPointsOrTime = " = 0 " + MessageViewContainerList.GetMessage(ExecutionContext, "Minutes", null);
                        return;
                    }
                    PointsOrTimeAvailable = (Convert.ToInt32((AvailableCredits()) - pointsToConvert)).ToString();
                    TotalPointsOrTime = " = " +Convert.ToInt32(((pointsToConvert * timePerCredits))) + " " + MessageViewContainerList.GetMessage(ExecutionContext, "Minutes", null);
                }
                else
                {
                    TotalPointsOrTime = " = 0 "+ MessageViewContainerList.GetMessage(ExecutionContext, "Minutes", null);
                }
            }
            log.LogMethodExit();
        }

        private async void OnOkButtonClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            string timePerCredit = getTimePerCredits();
            int timePerCredits = Convert.ToInt32(Convert.ToDecimal(timePerCredit));
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
            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.AccountId > -1)
            {
                ITaskUseCases taskUseCase = TaskUseCaseFactory.GetTaskUseCases(ExecutionContext);
                RedeemEntitlementDTO redeemEntitlementDTO=null;
                RedeemEntitlementDTO result=null;

                if (taskType == TaskType.EXCHANGECREDITFORTIME)
                {
                    if (PointsOrTimeToConvert == "0" || PointsOrTimeToConvert == string.Empty)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1382, null), MessageType.Error);
                        return;
                    }
                    double totalPoints = AvailableCredits();
                    double additionalPoints = Convert.ToDouble(PointsOrTimeToConvert);
                    if (additionalPoints > totalPoints)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1383, totalPoints), MessageType.Error);
                        return;
                    }
                    redeemEntitlementDTO = new RedeemEntitlementDTO(CardDetailsVM.AccountDTO.AccountId, ManagerId, RedeemEntitlementDTO.FromTypeEnum.CREDITS, RedeemEntitlementDTO.FromTypeEnum.TIME, Convert.ToDecimal(PointsOrTimeToConvert), Remarks);
                    try
                    {
                        IsLoadingVisible = true;
                        result = await taskUseCase.ExchangeEntitilements(redeemEntitlementDTO);
                        SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 1384);
                        SetFooterContent(SuccessMessage, MessageType.None);
                        IsLoadingVisible = false;
                        PerformClose(taskConvertPointsTimeView);
                        PoleDisplay.executionContext = ExecutionContext;
                        PoleDisplay.writeSecondLine(SuccessMessage);
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
                else if (taskType == TaskType.EXCHANGETIMEFORCREDIT)
                {
                    double totalTime = AvailableTime();
                    double additionalTime = Convert.ToDouble(PointsOrTimeToConvert);
                    if (additionalTime > totalTime)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1441, totalTime), MessageType.Error);
                        return;
                    }
                    if (PointsOrTimeToConvert == "0" || PointsOrTimeToConvert == string.Empty)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1442, null), MessageType.Error);
                        return;
                    }

                    redeemEntitlementDTO = new RedeemEntitlementDTO(CardDetailsVM.AccountDTO.AccountId, ManagerId = ManagerId, RedeemEntitlementDTO.FromTypeEnum.TIME, RedeemEntitlementDTO.FromTypeEnum.CREDITS, Convert.ToDecimal(PointsOrTimeToConvert), Remarks);

                    try
                    {
                        IsLoadingVisible = true;
                        result = await taskUseCase.ExchangeEntitilements(redeemEntitlementDTO);
                        SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 1443);
                        SetFooterContent(SuccessMessage, MessageType.None);
                        IsLoadingVisible = false;
                        PerformClose(taskConvertPointsTimeView);
                        PoleDisplay.executionContext = ExecutionContext;
                        PoleDisplay.writeSecondLine(SuccessMessage);
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
            }
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
                PointsOrTimeToConvert = "0";
                TotalPointsOrTime = string.Empty;
                return;
            }

            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.AccountId >= 0)
            {
                SetFooterContent(string.Empty, MessageType.None);
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 454, MessageType.Warning), MessageType.Error);
                PointsOrTimeToConvert = "0";
                TotalPointsOrTime = string.Empty;
                log.Error(ErrorMessage);
                return;
            }
            if (CardDetailsVM.AccountDTO.TechnicianCard == "Y")
            {
                ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 197, CardDetailsVM.AccountDTO.TagNumber);
                PerformClose(taskConvertPointsTimeView);
                return;
            }
            if (taskType == TaskType.EXCHANGETIMEFORCREDIT)
            {
                if ((AvailableTime()) <= 0)
                {
                    ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 1466);
                    PerformClose(taskConvertPointsTimeView);
                    log.Error(ErrorMessage);
                    return;
                }
                else
                {
                    PointsOrTimeAvailable = Convert.ToInt32(AvailableTime()).ToString();
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1442, null), MessageType.Info);
                }
            }
            else if(taskType == TaskType.EXCHANGECREDITFORTIME)
            {
                if((AvailableCredits())<=0)
                {
                    ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 3060);
                    PerformClose(taskConvertPointsTimeView);
                    log.Error(ErrorMessage);
                    return;
                }
                else
                {
                    PointsOrTimeAvailable = Convert.ToInt32(AvailableCredits()).ToString();
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1382, null), MessageType.Info);
                }
            }
            PointsOrTimeToConvert = "0";
            TotalPointsOrTime = string.Empty;
            log.LogMethodExit();
        }
        private void OnCardAdded(object parameter)
        {
            log.LogMethodEntry(parameter);
            HandleCardRead();
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            TotalPointsOrTime =string.Empty;
            PointsOrTimeAvailable = "0";
            PointsOrTimeToConvert = "0";
            taskConvertPointsTimeView = parameter as TaskConvertPointsTimeView;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            Remarks = string.Empty;
            if (!ManagerApprovalCheck(taskType,parameter))
            {
                ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 268);
                PerformClose(parameter);
            }
            log.LogMethodExit();
        }
        private string getTimePerCredits()
        {
            log.LogMethodEntry();
            string timePerCredits = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "TIME_IN_MINUTES_PER_CREDIT");
            if (string.IsNullOrEmpty(timePerCredits) || Convert.ToDecimal(timePerCredits) <= 0)
            {
                timePerCredits = "0";
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1339, null), MessageType.Info);//1339
            }
            log.LogMethodExit(timePerCredits);
            return timePerCredits;
        }
        private void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            (BackButtonCommand as DelegateCommand).RaiseCanExecuteChanged();
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
        public TaskConvertPointsTimeVM(ExecutionContext executionContext, TaskType taskType, DeviceClass cardReader) : base(executionContext, cardReader)
        {
            log.Info("TaskConvertPointsTime screen is opened");
            log.LogMethodEntry(executionContext, taskType, cardReader);
            this.ExecutionContext = executionContext;
            this.taskType = taskType;

            if (taskType == TaskType.EXCHANGECREDITFORTIME)
            {
                ModuleName = MessageViewContainerList.GetMessage(executionContext, " Convert ", null); ;
                FromEntitlement = MessageViewContainerList.GetMessage(executionContext, "Points", null);
                ToEntitlement =  MessageViewContainerList.GetMessage(executionContext, "Time", null);
                ExchangeCreditTimeHeading1 = MessageViewContainerList.GetMessage(executionContext, "Points Available", null);
                ExchangeCreditTimeHeading2 =  MessageViewContainerList.GetMessage(executionContext, "Points to convert", null);
            }
            else if (taskType == TaskType.EXCHANGETIMEFORCREDIT)
            {
                ModuleName =  MessageViewContainerList.GetMessage(executionContext, "Convert", null);
                FromEntitlement =  MessageViewContainerList.GetMessage(executionContext, "Time", null);
                ToEntitlement =  MessageViewContainerList.GetMessage(executionContext, "Points", null);
                ExchangeCreditTimeHeading1 = MessageViewContainerList.GetMessage(executionContext, "Time Available", null);
                ExchangeCreditTimeHeading2 = MessageViewContainerList.GetMessage(executionContext, "Time to convert", null);
            }

            Loaded = new DelegateCommand(OnLoaded);
            PropertyChanged += OnPropertyChanged;
            CardDetailsVM = new CardDetailsVM(ExecutionContext);
            CardDetailsVM.PropertyChanged += OnPropertyChanged;
            OkCommand = new DelegateCommand(OnOkButtonClicked, ButtonEnable);
            ClearCommand = new DelegateCommand(OnClearCommand, ButtonEnable);
            BackButtonCommand = new DelegateCommand(OnBackClicked, ButtonEnable);
            base.CardTappedEvent += HandleCardRead;
            CardAddedCommand = new DelegateCommand(OnCardAdded);

            log.LogMethodExit();
        }
        #endregion
    }
}

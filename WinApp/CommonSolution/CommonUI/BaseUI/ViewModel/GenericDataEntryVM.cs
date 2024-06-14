/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - view model for generic data entry page
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 *2.110.0     25-Nov-2020   Raja Uthanda            Modified to handle numeric up down
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;


namespace Semnox.Parafait.CommonUI
{
    public enum DataEntryType
    {
        TextBox = 0,
        ComboBox = 1,
        NumericUpDown = 2,
        TextBlock = 3,
        DatePicker = 4,
        RadioButton = 5,
        CheckBox = 6,
        Button = 7,
        Image = 8
    }
    public enum ButtonClickType
    {
        Ok,
        Cancel
    }

    public class GenericDataEntryVM : ViewModelBase
    {
        #region Members        
        private bool isKeyboardVisible;

        private ButtonClickType buttonClickType;

        private string statusMessage;
        private string errorMessage;
        private string title;
        private string okButtonContent;

        private ObservableCollection<DataEntryElement> dataEntryCollections;

        private ICommand cancelCommand;
        private ICommand okCommand;
        private ICommand numericDeletedCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties   
        public bool IsKeyboardVisible
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isKeyboardVisible);
                return isKeyboardVisible;
            }
            set
            {
                log.LogMethodEntry(isKeyboardVisible, value);
                SetProperty(ref isKeyboardVisible, value);
                log.LogMethodExit(isKeyboardVisible);
            }
        }

        public string OkButtonContent
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(okButtonContent);
                return okButtonContent;
            }
            set
            {
                log.LogMethodEntry(okButtonContent, value);
                SetProperty(ref okButtonContent, value);
                log.LogMethodExit(okButtonContent);
            }
        }

        public ButtonClickType ButtonClickType
        {
            get
            {
                log.LogMethodEntry();
                return buttonClickType;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref buttonClickType, value);
            }
        }

        public string StatusMessage
        {
            get
            {
                log.LogMethodEntry();
                return statusMessage;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref statusMessage, value);
            }
        }

        public string ErrorMessage
        {
            get
            {
                log.LogMethodEntry();
                return errorMessage;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref errorMessage, value);
            }
        }

        public ObservableCollection<DataEntryElement> DataEntryCollections
        {
            get
            {
                log.LogMethodEntry();
                return dataEntryCollections;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref dataEntryCollections, value);
            }
        }

        public string Heading
        {
            get
            {
                log.LogMethodEntry();
                return title;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref title, value);
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                log.LogMethodEntry();
                return cancelCommand;

            }
            set
            {
                log.LogMethodEntry(value);
                cancelCommand = value;
            }
        }

        public ICommand OkCommand
        {
            get
            {
                log.LogMethodEntry();
                return okCommand;

            }
            set
            {
                log.LogMethodEntry(value);
                okCommand = value;
            }
        }

        public ICommand NumericDeletedCommand
        {
            get
            {
                log.LogMethodEntry();
                return numericDeletedCommand;

            }
            set
            {
                log.LogMethodEntry(value);
                numericDeletedCommand = value;
            }
        }
        #endregion

        #region Constructors
        public GenericDataEntryVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            ExecutionContext = executionContext;
            cancelCommand = new DelegateCommand(OnCancelClicked);
            okCommand = new DelegateCommand(OnOkClicked);
            numericDeletedCommand = new DelegateCommand(OnNumericDeleteClicked);

            dataEntryCollections = new ObservableCollection<DataEntryElement>();

            title = string.Empty;
            statusMessage = string.Empty;
            errorMessage = MessageViewContainerList.GetMessage(ExecutionContext,"Please enter required fields.");
            okButtonContent = MessageViewContainerList.GetMessage(ExecutionContext, "OK");

            buttonClickType = ButtonClickType.Cancel;

            isKeyboardVisible = true;

            log.LogMethodExit();
        }
        #endregion

        #region Methods
        private void OnNumericDeleteClicked(object parameter)
        {
            log.LogMethodEntry();
            if (parameter != null)
            {
                GenericDataEntryView genericDataEntryView = parameter as GenericDataEntryView;
                if (genericDataEntryView != null)
                {
                    genericDataEntryView.RaiseCustomEvent();
                }
            }
            log.LogMethodExit();
        }

        private void OnCancelClicked(object parameter)
        {
            if (parameter != null)
            {
                GenericDataEntryView dataEntryView = parameter as GenericDataEntryView;

                if (dataEntryView != null && dataEntryCollections != null && dataEntryCollections.Count > 0)
                {
                    this.ButtonClickType = ButtonClickType.Cancel;
                    dataEntryView.Close();
                }
            }
        }

        private void OnOkClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericDataEntryView dataEntryView = parameter as GenericDataEntryView;

                if (dataEntryView != null)
                {
                    List<DataEntryElement> mandatoryList = this.dataEntryCollections.Where(item => item.IsMandatory == true).ToList();

                    bool isRequired = false;

                    foreach (DataEntryElement dataEntryElement in mandatoryList)
                    {
                        if (dataEntryElement != null && dataEntryElement.IsMandatory)
                        {
                            switch(dataEntryElement.Type)
                            {
                                case DataEntryType.TextBox:
                                    if (string.IsNullOrWhiteSpace(dataEntryElement.Text))
                                    {
                                        isRequired = true;
                                    }
                                    break;
                                case DataEntryType.DatePicker:
                                    if (string.IsNullOrWhiteSpace(dataEntryElement.Text) || dataEntryElement.ErrorState)
                                    {
                                        isRequired = true;
                                    }
                                    break;
                                case DataEntryType.ComboBox:
                                    if(dataEntryElement.SelectedItem == null || (dataEntryElement.SelectedItem != null && !dataEntryElement.Options.Contains(dataEntryElement.SelectedItem)))
                                    {
                                        isRequired = true;
                                    }
                                    break;
                            }
                            if(isRequired)
                            {
                                break;
                            }
                        }
                    }
                    if (!isRequired)
                    {
                        this.ButtonClickType = ButtonClickType.Ok;
                        StatusMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Filled the details successfully");
                        dataEntryView.Close();
                    }
                    else
                    {
                        StatusMessage = TranslateHelper.TranslateMessage(2790);
                        GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                        messagePopupView.Owner = dataEntryView;
                        messagePopupView.DataContext = new GenericMessagePopupVM(ExecutionContext)
                        {
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Error"),
                            SubHeading = MessageViewContainerList.GetMessage(ExecutionContext, "Required fields"),
                            Content = ErrorMessage,
                            CancelButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "OK"),
                            TimerMilliSeconds = 5000,
                            PopupType = PopupType.Timer,
                        };
                        messagePopupView.ShowDialog();
                    }

                }
            }
            log.LogMethodExit();
        }

        #endregion

    }
}

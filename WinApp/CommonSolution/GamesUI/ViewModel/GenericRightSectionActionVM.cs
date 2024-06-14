/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Game Management - right section View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Amitha Joy              Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Parafait.CommonUI;
using System.Windows;
using System.Windows.Input;

namespace Semnox.Parafait.GamesUI
{
    public class GenericRightSectionActionVM : ViewModelBase
    {
        #region Members
        private Visibility actionFirstGroupVisibility;
        private string editButtonContent;
        private bool isEditEnabled;
        private string lastButtonContent;
        private bool isLastButtonEnabled;
        private ButtonGroupVM buttonGroupVM;
        private ICommand editCommand;
        private ICommand lastButtonCommand;
        private ICommand buttonGroupClickedCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public ICommand EditCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(editCommand);
                return editCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                editCommand = value;
            }
        }

        public ICommand LastButtonCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(lastButtonCommand);
                return lastButtonCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                lastButtonCommand = value;
            }
        }

        public ICommand ButtonGroupClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonGroupClickedCommand);
                return buttonGroupClickedCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                buttonGroupClickedCommand = value;
            }
        }

        public bool IsEditEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isEditEnabled);
                return isEditEnabled;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isEditEnabled, value);
                RaiseCanExecuteChanged();
            }
        }

        public bool IsLastButtonEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isLastButtonEnabled);
                return isLastButtonEnabled;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isLastButtonEnabled, value);
                RaiseCanExecuteChanged();
            }
        }

        public Visibility ActionFirstGroupVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(actionFirstGroupVisibility);
                return actionFirstGroupVisibility;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref actionFirstGroupVisibility, value);
            }
        }

        public string EditButtonContent
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(editButtonContent);
                return editButtonContent;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref editButtonContent, value);
            }
        }

        public string LastButtonContent
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(lastButtonContent);
                return lastButtonContent;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref lastButtonContent, value);
            }
        }

        public ButtonGroupVM ButtonGroupVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonGroupVM);
                return buttonGroupVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref buttonGroupVM, value);
            }
        }
        #endregion

        #region Methods
        public void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            if (EditCommand != null)
            {
                (EditCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
            if (LastButtonCommand != null)
            {
                (LastButtonCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
            log.LogMethodExit();
        }
        private void OnEditClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericRightSectionActionUserControl rightSectionActionUserControl = parameter as GenericRightSectionActionUserControl;

                if (rightSectionActionUserControl != null)
                {
                    rightSectionActionUserControl.RaiseEditClickedEvent();
                }
            }
            log.LogMethodExit();
        }

        private bool CanEditExecute(object parameter)
        {
            log.LogMethodEntry(parameter);
            log.LogMethodExit(IsEditEnabled);
            return IsEditEnabled;
        }

        private void OnButtonGroupClickedClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericRightSectionActionUserControl rightSectionActionUserControl = parameter as GenericRightSectionActionUserControl;

                if (rightSectionActionUserControl != null)
                {
                    rightSectionActionUserControl.RaiseButtonGroupClickedEvent();
                }
            }
            log.LogMethodExit();
        }

        private void OnLastButtonClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericRightSectionActionUserControl rightSectionActionUserControl = parameter as GenericRightSectionActionUserControl;

                if (rightSectionActionUserControl != null)
                {
                    rightSectionActionUserControl.RaiseLastClickedEvent();
                }
            }
            log.LogMethodExit();
        }

        private bool CanLastExecute(object parameter)
        {
            log.LogMethodEntry(parameter);
            log.LogMethodExit(IsLastButtonEnabled);
            return IsLastButtonEnabled;
        }
        #endregion

        #region Constructors
        public GenericRightSectionActionVM()
        {
            buttonGroupClickedCommand = new DelegateCommand(OnButtonGroupClickedClicked);
            editCommand = new DelegateCommand(OnEditClicked, CanEditExecute);
            lastButtonCommand = new DelegateCommand(OnLastButtonClicked, CanLastExecute);

            log.LogMethodEntry();
            lastButtonContent = "";
            editButtonContent = "";
            buttonGroupVM = new ButtonGroupVM();
            actionFirstGroupVisibility = Visibility.Visible;
            isLastButtonEnabled = isEditEnabled = false;
            log.LogMethodExit();
        }
        #endregion
    }
}

/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Button group View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Windows.Input;

namespace Semnox.Parafait.CommonUI
{
    public class ButtonGroupVM : ViewModelBase
    {
        #region Members
        private string firstButtonContent;
        private string secondButtonContent;
        private bool isFirstButtonEnabled;
        private bool isSecondButtonEnabled;
        private ICommand firstButtonCommand;
        private ICommand secondButtonCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public ICommand FirstButtonCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(firstButtonCommand);
                return firstButtonCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref firstButtonCommand, value);
            }
        }

        public ICommand SecondButtonCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(secondButtonCommand);
                return secondButtonCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref secondButtonCommand, value);
            }
        }

        public string FirstButtonContent
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(firstButtonContent);
                return firstButtonContent;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref firstButtonContent, value);
            }
        }

        public string SecondButtonContent
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(secondButtonContent);
                return secondButtonContent;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref secondButtonContent, value);
            }
        }

        public bool IsFirstButtonEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isFirstButtonEnabled);
                return isFirstButtonEnabled;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isFirstButtonEnabled, value);
                RaiseCanExecuteChanged();
            }
        }

        public bool IsSecondButtonEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isSecondButtonEnabled);
                return isSecondButtonEnabled;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isSecondButtonEnabled, value);
                RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Methods
        public void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            if (FirstButtonCommand != null)
            {
                (FirstButtonCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
            if (SecondButtonCommand != null)
            {
                (SecondButtonCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
            log.LogMethodExit();
        }
        private void OnFirstButtonClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                ButtonGroupUserControl rightSectionUserControl = parameter as ButtonGroupUserControl;

                if (rightSectionUserControl != null)
                {
                    rightSectionUserControl.RaiseButtonGroupClickedEvent();
                }
            }
            log.LogMethodEntry();
        }

        private void OnSecondButtonClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                ButtonGroupUserControl rightSectionUserControl = parameter as ButtonGroupUserControl;

                if (rightSectionUserControl != null)
                {
                    rightSectionUserControl.RaiseButtonGroupClickedEvent();
                }
            }
            log.LogMethodEntry();
        }

        private bool CanSecondExecute(object parameter)
        {
            log.LogMethodEntry(parameter);
            log.LogMethodExit(IsSecondButtonEnabled);
            return IsSecondButtonEnabled;
        }

        private bool CanFirstExecute(object parameter)
        {
            log.LogMethodEntry(parameter);
            log.LogMethodExit(IsFirstButtonEnabled);
            return IsFirstButtonEnabled;
        }
        #endregion

        #region Constructors
        public ButtonGroupVM()
        {
            log.LogMethodEntry();

            firstButtonCommand = new DelegateCommand(OnFirstButtonClicked, CanFirstExecute);
            secondButtonCommand = new DelegateCommand(OnSecondButtonClicked, CanSecondExecute);

            firstButtonContent = string.Empty;
            secondButtonContent = string.Empty;

            isFirstButtonEnabled = false;
            isSecondButtonEnabled = false;

            log.LogMethodExit();
        }
        #endregion
    }
}

/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Generic right section content View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 *2.110.0     25-Sep-2020   Raja Uthanda            modified for multi screen
 ********************************************************************************************/
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Semnox.Parafait.CommonUI
{
    public class GenericRightSectionContentVM : ViewModelBase
    {
        #region Members
        private bool ismultiScreenRowTwo;
        private bool multiScreenMode;
        private bool isScreenUserAreaVisble;
        private bool showNavigationButton;
        private string screenNumber;
        private string userName;
        private ICommand searchButtonClickedCommand;
        private ICommand previousNavigationCommand;
        private ICommand nextNavigationCommand;
        private string heading;
        private string subHeading;
        private ObservableCollection<RightSectionPropertyValues> propertyCollection;
        private bool isNextNavigationEnabled;
        private bool isPreviousNavigationEnabled;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public bool ShowNavigationButton
        {
            get { return showNavigationButton; }
            set { SetProperty(ref showNavigationButton, value); }
        }
        public bool IsMultiScreenRowTwo
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ismultiScreenRowTwo);
                return ismultiScreenRowTwo;
            }
            set
            {
                log.LogMethodEntry(ismultiScreenRowTwo, value);
                SetProperty(ref ismultiScreenRowTwo, value);
                log.LogMethodExit(ismultiScreenRowTwo);
            }
        }

        public bool MultiScreenMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(multiScreenMode);
                return multiScreenMode;
            }
            set
            {
                log.LogMethodEntry(multiScreenMode, value);
                SetProperty(ref multiScreenMode, value);
                log.LogMethodExit(multiScreenMode);
            }
        }

        public bool IsScreenUserAreaVisble
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isScreenUserAreaVisble);
                return isScreenUserAreaVisble;
            }
            set
            {
                log.LogMethodEntry(isScreenUserAreaVisble, value);
                SetProperty(ref isScreenUserAreaVisble, value);
                log.LogMethodExit(isScreenUserAreaVisble);
            }
        }

        public string ScreenNumber
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(screenNumber);
                return screenNumber;
            }
            set
            {
                log.LogMethodEntry(screenNumber, value);
                SetProperty(ref screenNumber, value);
                log.LogMethodExit(screenNumber);
            }
        }

        public string UserName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(userName);
                return userName;
            }
            set
            {
                log.LogMethodEntry(userName, value);
                SetProperty(ref userName, value);
                log.LogMethodExit(userName);
            }
        }

        public ICommand SearchButtonClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchButtonClickedCommand);
                return searchButtonClickedCommand;
            }
            set
            {
                log.LogMethodEntry(searchButtonClickedCommand, value);
                SetProperty(ref searchButtonClickedCommand, value);
                log.LogMethodExit(searchButtonClickedCommand);
            }
        }

        public bool IsNextNavigationEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isNextNavigationEnabled);
                return isNextNavigationEnabled;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isNextNavigationEnabled, value);
            }
        }

        public bool IsPreviousNavigationEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isPreviousNavigationEnabled);
                return isPreviousNavigationEnabled;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isPreviousNavigationEnabled, value);
            }
        }

        public ICommand PreviousNavigationCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(previousNavigationCommand);
                return previousNavigationCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                previousNavigationCommand = value;
            }
        }

        public ICommand NextNavigationCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(nextNavigationCommand);
                return nextNavigationCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                nextNavigationCommand = value;
            }
        }

        public string Heading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(heading);
                return heading;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref heading, value);
            }
        }

        public string SubHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(subHeading);
                return subHeading;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref subHeading, value);
            }
        }

        public ObservableCollection<RightSectionPropertyValues> PropertyCollections
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(propertyCollection);
                return propertyCollection;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref propertyCollection, value);
            }
        }

        #endregion

        #region Methods

        private void OnPreviousNavigation(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericRightSectionContentUserControl rightSectionUserControl = parameter as GenericRightSectionContentUserControl;

                if (rightSectionUserControl != null)
                {
                    rightSectionUserControl.RaisePreviousNavigationClickedEvent();
                }
            }
            log.LogMethodExit();
        }

        private void OnNextNavigation(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericRightSectionContentUserControl rightSectionUserControl = parameter as GenericRightSectionContentUserControl;

                if (rightSectionUserControl != null)
                {
                    rightSectionUserControl.RaiseNextNavigationClickedEvent();
                }
            }
            log.LogMethodExit();
        }

        private void OnSearchButtonClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericRightSectionContentUserControl rightSectionContentUserControl = parameter as GenericRightSectionContentUserControl;
                if (rightSectionContentUserControl != null)
                {
                    rightSectionContentUserControl.RaiseSearchButtonClickedEvent();
                }
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public GenericRightSectionContentVM()
        {
            log.LogMethodEntry();
            previousNavigationCommand = new DelegateCommand(OnPreviousNavigation);
            nextNavigationCommand = new DelegateCommand(OnNextNavigation);
            heading = string.Empty;
            subHeading = string.Empty;
            propertyCollection = new ObservableCollection<RightSectionPropertyValues>();

            multiScreenMode = false;
            isScreenUserAreaVisble = false;
            screenNumber = string.Empty;
            userName = string.Empty;
            searchButtonClickedCommand = new DelegateCommand(OnSearchButtonClicked);

            log.LogMethodExit();
        }
        #endregion
    }
}

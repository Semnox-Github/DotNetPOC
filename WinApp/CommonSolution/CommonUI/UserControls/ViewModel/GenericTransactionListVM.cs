/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Generic transaction list View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Sep-2020   Raja Uthanda            modified for multi screen
 ********************************************************************************************/
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Semnox.Parafait.CommonUI
{
    public class GenericTransactionListVM : ViewModelBase
    {
        #region Members
        private bool multiScreenMode;
        private bool ismultiScreenRowTwo;
        private bool showNavigationButton;
        private bool screenUserAreaVisible;

        private string transactionID;
        private string userName;
        private string screenNumber;

        private ObservableCollection<GenericTransactionListItem> itemCollection;
        private GenericTransactionListItem selectedItem;
        private GenericTransactionListUserControl transactionListUserControl;

        private ICommand deleteCommand;
        private ICommand resetCommand;
        private ICommand itemClickedCommand;
        private ICommand searchCommand;
        private ICommand loadedCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public bool ShowNavigationButton
        {
            get { return showNavigationButton; }
            set { SetProperty(ref showNavigationButton, value); }
        }
        public GenericTransactionListUserControl TransactionListUserControl
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(transactionListUserControl);
                return transactionListUserControl;
            }
        }
        public bool ScreenUserAreaVisible
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(screenUserAreaVisible);
                return screenUserAreaVisible;
            }
            set
            {
                log.LogMethodEntry(screenUserAreaVisible, value);
                SetProperty(ref screenUserAreaVisible, value);
                log.LogMethodExit(screenUserAreaVisible);
            }
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

        public string TransactionID
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(transactionID);
                return transactionID;
            }
            set
            {
                log.LogMethodEntry(transactionID, value);
                SetProperty(ref transactionID, value);
                log.LogMethodExit(transactionID);
            }
        }

        public ObservableCollection<GenericTransactionListItem> ItemCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(itemCollection);
                return itemCollection;
            }
            set
            {
                log.LogMethodEntry(itemCollection, value);
                SetProperty(ref itemCollection, value);
                log.LogMethodExit(itemCollection);
            }
        }

        public GenericTransactionListItem SelectedItem
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedItem);
                return selectedItem;
            }
            set
            {
                log.LogMethodEntry(selectedItem, value);
                SetProperty(ref selectedItem, value);
                log.LogMethodExit(selectedItem);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(deleteCommand);
                return deleteCommand;
            }
            private set
            {
                log.LogMethodEntry(deleteCommand, value);
                SetProperty(ref deleteCommand, value);
                log.LogMethodExit(deleteCommand);
            }
        }

        public ICommand ResetCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(resetCommand);
                return resetCommand;
            }
            private set
            {
                log.LogMethodEntry(resetCommand, value);
                SetProperty(ref resetCommand, value);
                log.LogMethodExit(resetCommand);
            }
        }

        public ICommand ItemClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(itemClickedCommand);
                return itemClickedCommand;
            }
            set
            {
                log.LogMethodEntry(itemClickedCommand, value);
                SetProperty(ref itemClickedCommand, value);
                log.LogMethodExit(itemClickedCommand);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchCommand);
                return searchCommand;
            }
            set
            {
                log.LogMethodEntry(searchCommand, value);
                SetProperty(ref searchCommand, value);
                log.LogMethodExit(searchCommand);
            }
        }

        public ICommand LoadedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loadedCommand);
                return loadedCommand;
            }
            set
            {
                log.LogMethodEntry(loadedCommand, value);
                SetProperty(ref loadedCommand, value);
                log.LogMethodExit(loadedCommand);
            }
        }
        #endregion

        #region Methods
        private void OnDeleteClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (transactionListUserControl != null)
            {
                transactionListUserControl.RaiseDeleteEvent();
            }
            log.LogMethodExit();
        }

        private void OnResetClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (transactionListUserControl != null)
            {
                transactionListUserControl.RaiseResetEvent();
            }
            log.LogMethodExit();
        }

        private void OnItemClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                selectedItem = parameter as GenericTransactionListItem;
            }
            if (transactionListUserControl != null && selectedItem != null && selectedItem.IsEnabled)
            {
                transactionListUserControl.RaiseItemClickedEvent();
            }
            log.LogMethodExit();
        }

        private void OnSearchClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (transactionListUserControl != null)
            {
                transactionListUserControl.RaiseSearchEvent();
            }
            log.LogMethodExit();
        }

        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                transactionListUserControl = parameter as GenericTransactionListUserControl;
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public GenericTransactionListVM()
        {
            log.LogMethodEntry();
            multiScreenMode = false;
            ismultiScreenRowTwo = false;

            transactionID = string.Empty;
            screenNumber = string.Empty;
            screenNumber = string.Empty;

            itemCollection = new ObservableCollection<GenericTransactionListItem>();
            selectedItem = null;

            deleteCommand = new DelegateCommand(OnDeleteClicked);
            resetCommand = new DelegateCommand(OnResetClicked);
            itemClickedCommand = new DelegateCommand(OnItemClicked);
            searchCommand = new DelegateCommand(OnSearchClicked);
            loadedCommand = new DelegateCommand(OnLoaded);
            log.LogMethodExit();
        }
        #endregion
    }
}

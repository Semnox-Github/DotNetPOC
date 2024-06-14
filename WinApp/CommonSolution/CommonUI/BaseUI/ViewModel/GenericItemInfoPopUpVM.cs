/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - view model for item info popup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/

using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI
{
    public class GenericItemInfoPopUpVM : ViewModelBase
    {
        #region Members
        private bool ismultiScreenRowTwo;
        private bool multiScreenMode;
        private bool addClicked;

        private int id;
        private int maxValue;
        private int selectedValue;
        private string listBoxItemHeader;
        private string numericCounterHeader;
        private string imageSourcePath;
        private Visibility imageVisibility;

        private ObservableCollection<ItemInfoPopupTextBlockModel> itemDetailsHeadersContent;
        private ObservableCollection<string> displayInventoryDetails;
        private ICommand addCommand;
        private ICommand closeCommand;
        private ICommand selectedItemCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Properties
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

        public bool AddClicked
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addClicked);
                return addClicked;
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

        public int Id
        {
            get
            {
                log.LogMethodEntry();
                return id;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref id, value);
            }
        }

        public int SelectedValue
        {
            get
            {
                log.LogMethodEntry();
                return selectedValue;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedValue, value);
            }
        }

        public int MaxValue
        {
            get
            {
                log.LogMethodEntry();
                return maxValue;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref maxValue, value);
            }
        }

        public ObservableCollection<ItemInfoPopupTextBlockModel> ItemDetailsHeadersContent
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(itemDetailsHeadersContent);
                return itemDetailsHeadersContent;
            }
            set
            {
                log.LogMethodEntry(value);
                itemDetailsHeadersContent = value;
                SetProperty(ref itemDetailsHeadersContent, value);
            }
        }

        public ObservableCollection<string> DisplayInventoryDetails
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayInventoryDetails);
                return displayInventoryDetails;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayInventoryDetails, value);
            }
        }

        private void DisplayInventoryDetails_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                //   backupInventoryDetails.Add(e.NewItems as DisplayItemDetails);
            }
        }

        public Visibility ImageVisibility
        {
            get
            {
                log.LogMethodEntry();
                return imageVisibility;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref imageVisibility, value);
            }
        }

        public string ListBoxItemHeader
        {
            get
            {
                log.LogMethodEntry();
                return listBoxItemHeader;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref listBoxItemHeader, value);
            }
        }


        public string NumericCounterHeader
        {
            get
            {
                log.LogMethodEntry();
                return numericCounterHeader;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref numericCounterHeader, value);
            }
        }


        public string ImageSourcePath
        {
            get
            {
                log.LogMethodEntry();
                return imageSourcePath;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref imageSourcePath, value);
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                log.LogMethodEntry();
                return closeCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref closeCommand, value);
            }
        }

        public ICommand AddCommand
        {
            get
            {
                log.LogMethodEntry();
                return addCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref addCommand, value);
            }
        }

        public ICommand SelectedItemCommand
        {
            get
            {
                log.LogMethodEntry();
                return selectedItemCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                selectedItemCommand = value;
            }
        }
        #endregion

        #region Methods

        private void OnCloseClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            addClicked = false;
            PerformClose(parameter);
            log.LogMethodExit();
        }

        private void PerformClose(object parameter)
        {
            if (parameter != null)
            {
                GenericItemInfoPopUp infoPopup = parameter as GenericItemInfoPopUp;
                if (infoPopup != null && infoPopup.KeyBoardHelper != null &&
                    infoPopup.KeyBoardHelper.NumberKeyboardView == null)
                {
                    infoPopup.Close();
                }
            }
        }

        private void OnAddClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            addClicked = true;
            PerformClose(parameter);
            log.LogMethodExit();
        }
        #endregion

        #region Constructor

        public GenericItemInfoPopUpVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.ExecutionContext = executionContext;
            closeCommand = new DelegateCommand(OnCloseClicked);
            addCommand = new DelegateCommand(OnAddClicked);
            ismultiScreenRowTwo = false;
            multiScreenMode = false;
            selectedValue = 0;
            maxValue = 100;
            listBoxItemHeader = MessageViewContainerList.GetMessage(ExecutionContext, "Inventory");
            numericCounterHeader = MessageViewContainerList.GetMessage(ExecutionContext, "ENTER QUANTITY");
            imageVisibility = Visibility.Visible;
            itemDetailsHeadersContent = new ObservableCollection<ItemInfoPopupTextBlockModel>();
            displayInventoryDetails = new ObservableCollection<string>();
            DisplayInventoryDetails.CollectionChanged += DisplayInventoryDetails_CollectionChanged;

            log.LogMethodExit();

        }


        #endregion
    }
}

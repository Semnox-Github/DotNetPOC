/********************************************************************************************
 * Project Name - Invenotry UI
 * Description  - ProductVM
 * 
 **************
 **Version log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.130       04-Jun-2021   Girish Kundar          Created - POS stock  change
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.ViewContainer;
namespace Semnox.Parafait.InventoryUI
{

    /// <summary>
    /// NotificationTagsVM
    /// </summary>
    public class ProductVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ObservableCollection<ProductViewDTO> productViewDTOList;
        private Dictionary<string, CustomDataGridColumnElement> dataEntryElements;
        private List<ProductViewDTO> selectedProductViewDTO;

        private DialogResult dialogResult;
        private string message;
        private string buttonClose;
        private string buttonOK;
        private ProductView productView;
        private DisplayTagsVM displayTagsVM;
        private CustomDataGridVM customDataGridVM;
        private ObservableCollection<ProductViewDTO> gridSource;
        private ICommand addCommand;
        private ICommand closeCommand;
        private ICommand rowSectionCommand;
        private ICommand navigationClickCommand;
        private ICommand selectionChangedCmd;
        private string labelStatus;
        private string moduleName;
        #endregion Members
        #region Properties
        /// <summary>
        /// CustomDataGridVM
        /// </summary>
        public CustomDataGridVM CustomDataGridVM
        {
            get
            {
                return customDataGridVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref customDataGridVM, value);
                log.LogMethodExit(customDataGridVM);
            }

        }

        /// <summary>
        /// DisplayTagsVM
        /// </summary>
        public DisplayTagsVM DisplayTagsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTagsVM);
                return displayTagsVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTagsVM, value);
                log.LogMethodExit(displayTagsVM);
            }
        }

        /// <summary>
        /// GridSource
        /// </summary>
        public ObservableCollection<ProductViewDTO> GridSource
        {
            get
            {
                return gridSource;
            }
            set
            {
                gridSource = value;
            }
        }

        /// <summary>
        /// DataEntryElements
        /// </summary>
        public Dictionary<string, CustomDataGridColumnElement> DataEntryElements
        {
            get
            {
                return dataEntryElements;
            }
        }
        /// <summary>
        /// labelStatus
        /// </summary>
        public string LabelStatus
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(labelStatus);
                return labelStatus;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref labelStatus, value);
            }
        }

        /// <summary>
        /// ModuleName
        /// </summary>
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
            }
        }
        /// <summary>
        /// ModuleName
        /// </summary>
        public string ButtonClose
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonClose);
                return buttonClose;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref buttonClose, value);
                }
            }
        }
        public string ButtonOK
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonOK);
                return buttonOK;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref buttonOK, value);
                }
            }
        }
        public DialogResult FormDialogResult
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(dialogResult);
                return dialogResult;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref dialogResult, value);
            }
        }

        /// <summary>
        /// List<ProductViewDTO>
        /// </summary>
        public List<ProductViewDTO> SelectedProductViewDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedProductViewDTO);
                return selectedProductViewDTO;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref selectedProductViewDTO, value);
                }
            }
        }
        /// <summary>
        /// ProductViewDTOList
        /// </summary>
        /// <summary>
        /// vendorDTOList
        /// </summary>
        public ObservableCollection<ProductViewDTO> ProductViewDTOList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(productViewDTOList);
                return productViewDTOList;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref productViewDTOList, value);
            }
        }
        /// <summary>
        /// SuccessMessage
        /// </summary>
        public string SuccessMessage
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(message);
                return message;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref message, value);
            }
        }

        #endregion Properties

        #region Commands
        public ICommand NavigationClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(navigationClickCommand);
                return navigationClickCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                navigationClickCommand = value;
            }
        }

        /// <summary>
        /// AddCommand
        /// </summary>
        public ICommand AddCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addCommand);
                return addCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                addCommand = value;
            }
        }

        /// <summary>
        /// SearchCommand   
        /// </summary>
        public ICommand RowSectionCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rowSectionCommand);
                return rowSectionCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                rowSectionCommand = value;
            }
        }

        /// <summary>
        /// CloseCommand
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(closeCommand);
                return closeCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                closeCommand = value;
            }
        }

        /// <summary>
        /// navigationClickCommand
        /// </summary>
        public ICommand SelectionChangedCmd
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(selectionChangedCmd);
                return selectionChangedCmd;
            }
            set
            {
                log.LogMethodEntry(value);
                selectionChangedCmd = value;
            }
        }
        #endregion Properties
        #region Constructor
        /// <summary>
        /// ProductVM
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="SearchParameters"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public ProductVM(ExecutionContext executionContext, List<ProductViewDTO> productViewDTOList)
        {
            log.LogMethodEntry();
            this.ExecutionContext = executionContext;
            this.productViewDTOList = new ObservableCollection<ProductViewDTO>(productViewDTOList);
            LoadLables();
            SetDisplayTagsVM();
            dataEntryElements = new Dictionary<string, CustomDataGridColumnElement>();
            dataEntryElements.Add("Code", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "  Product Code  "), Type = DataEntryType.TextBlock, IsReadOnly = true });
            dataEntryElements.Add("Description", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, " Description  "), Type = DataEntryType.TextBlock, IsReadOnly = true });
            dataEntryElements.Add("BarCode", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "  Product Barcode "), Type = DataEntryType.TextBlock, IsReadOnly = true });
            using (NoSynchronizationContextScope.Enter())
            {
                addCommand = new DelegateCommand(AddButtonClick);
                closeCommand = new DelegateCommand(CloseButtonClick);
                navigationClickCommand = new DelegateCommand(NavigationClick);
            };
            customDataGridVM = new CustomDataGridVM(ExecutionContext)
            {
                CollectionToBeRendered = new ObservableCollection<object>(ProductViewDTOList),
                HeaderCollection = dataEntryElements,
                ShowSearchTextBox = false,
                SelectOption = SelectOption.CheckBox,
                IsComboAndSearchVisible = false
            };
            log.LogMethodExit();
        }
        #endregion Constructor
        #region Methods
        private void SetDisplayTagsVM()
        {
            try
            {
                POSMachineContainerDTO posMachineContainerDTO = POSMachineViewContainerList.GetPOSMachineContainerDTO(ExecutionContext);
                if (DisplayTagsVM == null)
                {
                    DisplayTagsVM = new DisplayTagsVM();
                }
                DisplayTagsVM.DisplayTags = new ObservableCollection<ObservableCollection<DisplayTag>>()
                                    {
                                      new ObservableCollection<DisplayTag>()
                                      {
                                          new DisplayTag()
                                          {
                                               Text = MessageContainerList.GetMessage(ExecutionContext,  " SELECT PRODUCT "),
                                               TextSize = TextSize.Medium,
                                               FontWeight = System.Windows.FontWeights.Bold
                                          }
                                      }
                                    };
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }

        private void LoadLables()
        {
            log.LogMethodEntry();
            buttonClose = MessageContainerList.GetMessage(ExecutionContext, "CANCEL");
            buttonOK = MessageContainerList.GetMessage(ExecutionContext, "ADD");
            moduleName = MessageContainerList.GetMessage(ExecutionContext, "PRODUCT VIEW");
            log.LogMethodExit();
        }
        private void AddButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            productView = param as ProductView;
            selectedProductViewDTO = new List<ProductViewDTO>();
            try
            {
                int count = this.CustomDataGridVM.SelectedItems.Count;
                if (count == 0)
                {
                    ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 2460);
                    ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Validation"), MessageContainerList.GetMessage(ExecutionContext, "Required"), ErrorMessage);
                    CustomDataGridVM.SelectedItems.Clear();
                    return;
                }
                foreach (object data in CustomDataGridVM.SelectedItems)
                {
                    ProductViewDTO productViewDTO = data as ProductViewDTO;
                    if (productViewDTO != null)
                    {
                        selectedProductViewDTO.Add(productViewDTO);
                    }
                }
                FormDialogResult = DialogResult.OK;
                productView.Close();
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
            }
            
            log.LogMethodExit();
        }


        private void NavigationClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            productView = param as ProductView;
            try
            {
                if (productView != null)
                {
                    FormDialogResult = DialogResult.OK;
                    productView.Close();
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
                FormDialogResult = DialogResult.Cancel;

            }

        }
        private void CloseButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            productView = param as ProductView;
            try
            {
                if (productView != null)
                {
                    FormDialogResult = DialogResult.OK;
                    productView.Close();
                }
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;

            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
                FormDialogResult = DialogResult.Cancel;
            };

        }


        private void ShowMessagePopup(string heading, string subHeading, string content)
        {
            log.LogMethodEntry(heading, subHeading, content);

            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            messagePopupView.Owner = productView;

            GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext)
            {
                Heading = heading,
                SubHeading = subHeading,
                Content = content,
                CancelButtonText = MessageContainerList.GetMessage(ExecutionContext, "OK"),
                TimerMilliSeconds = 5000,
                PopupType = PopupType.Timer,
            };

            messagePopupView.DataContext = messagePopupVM;
            messagePopupView.ShowDialog();
            log.LogMethodExit();
        }

        private void CloseAddWindow(string message)
        {
            SuccessMessage = message;
            if (productView != null)
            {
                productView.Close();
            }
        }
        #endregion Methods
    }
}

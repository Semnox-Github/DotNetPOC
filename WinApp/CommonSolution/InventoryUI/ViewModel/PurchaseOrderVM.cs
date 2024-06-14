/********************************************************************************************
 * Project Name - Invenotry UI
 * Description  - PurchaseOrderVM
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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.InventoryUI
{
    /// <summary>
    /// PurchaseOrderVM
    /// </summary>
    public class PurchaseOrderVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ObservableCollection<PurchaseOrderDTO> purchaseOrderDTOList;
        private Dictionary<string, CustomDataGridColumnElement> dataEntryElements;
        private List<PurchaseOrderDTO> selectedPurchaseOrderDTOList;

        private DialogResult dialogResult;
        private string message;
        private string buttonClose;
        private string buttonOK;
        private PurchaseOrderView purchaseOrderView;
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
        /// SelectedPurchaseOrderDTOList
        /// </summary>
        public List<PurchaseOrderDTO> SelectedPurchaseOrderDTOList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedPurchaseOrderDTOList);
                return selectedPurchaseOrderDTOList;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref selectedPurchaseOrderDTOList, value);
                }
            }
        }
        /// <summary>
        /// PurchaseOrderDTOList
        /// </summary>
        /// <summary>
        /// vendorDTOList
        /// </summary>
        public ObservableCollection<PurchaseOrderDTO> PurchaseOrderDTOList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(purchaseOrderDTOList);
                return purchaseOrderDTOList;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref purchaseOrderDTOList, value);
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
        /// PurchaseOrderVM
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="SearchParameters"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public PurchaseOrderVM(ExecutionContext executionContext, List<PurchaseOrderDTO> purchaseOrderDTOList)
        {
            log.LogMethodEntry(purchaseOrderDTOList);
            this.ExecutionContext = executionContext;
            using (NoSynchronizationContextScope.Enter())
            {
                Task<List<PurchaseOrderDTO>> task = UpdatePORemarks(purchaseOrderDTOList);
                task.Wait();
                List<PurchaseOrderDTO> taskResult = task.Result;
                log.LogVariableState("taskResult - ", taskResult);
                this.purchaseOrderDTOList = new ObservableCollection<PurchaseOrderDTO>(taskResult);
            }
            LoadLables();
            SetDisplayTagsVM();
            dataEntryElements = new Dictionary<string, CustomDataGridColumnElement>();
            dataEntryElements.Add("OrderNumber", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "  Order Number  "), Type = DataEntryType.TextBlock, IsReadOnly = true });
            dataEntryElements.Add("OrderDate", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, " Order Date  "), Type = DataEntryType.TextBlock, IsReadOnly = true });
            dataEntryElements.Add("OrderStatus", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, " Order Status "), Type = DataEntryType.TextBlock, IsReadOnly = true });
            dataEntryElements.Add("OrderRemarks", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, " Order Remarks "), Type = DataEntryType.TextBlock, IsReadOnly = true, DataGridColumnFixedSize = 200 });
            dataEntryElements.Add("CreatedBy", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, " User Name "), Type = DataEntryType.TextBlock, IsReadOnly = true });
            using (NoSynchronizationContextScope.Enter())
            {
                addCommand = new DelegateCommand(AddButtonClick);
                closeCommand = new DelegateCommand(CloseButtonClick);
                navigationClickCommand = new DelegateCommand(NavigationClick);
            };
            customDataGridVM = new CustomDataGridVM(ExecutionContext)
            {
                CollectionToBeRendered = new ObservableCollection<object>(PurchaseOrderDTOList.OrderByDescending(x => x.OrderDate).ToList()),
                HeaderCollection = dataEntryElements,
                ShowSearchTextBox = false,
                SelectOption = SelectOption.CheckBox,
                IsComboAndSearchVisible = false
            };
            log.LogMethodExit();
        }
        #endregion Constructor
        #region Methods
        private async Task<List<PurchaseOrderDTO>> UpdatePORemarks(List<PurchaseOrderDTO> purchaseOrderDTOList)
        {
            log.LogMethodEntry(purchaseOrderDTOList);
            if (purchaseOrderDTOList != null && purchaseOrderDTOList.Any())
            {
                List<int> purchaseorderIdList = new List<int>();
                purchaseorderIdList = purchaseOrderDTOList.Select(x => x.PurchaseOrderId).ToList();
                string idList = string.Join(",", purchaseorderIdList);
                using (NoSynchronizationContextScope.Enter())
                {

                    IInventoryNotesUseCases inventoryNotesUseCases = InventoryUseCaseFactory.GetInventoryNotesUseCases(ExecutionContext);
                    List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>> inventoryNotesSearchParams = new List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>>();
                    inventoryNotesSearchParams.Add(new KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>(InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_ID_LIST, idList));
                    inventoryNotesSearchParams.Add(new KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>(InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_NAME, "PurchaseOrder"));
                    List<InventoryNotesDTO> inventoryNotesListOnDisplay = await inventoryNotesUseCases.GetInventoryNotes(inventoryNotesSearchParams);
                    if (inventoryNotesListOnDisplay != null && inventoryNotesListOnDisplay.Any())
                    {
                        foreach (PurchaseOrderDTO purchaseOrderDTO in purchaseOrderDTOList)
                        {
                            if (inventoryNotesListOnDisplay.Exists(x => x.ParafaitObjectId == purchaseOrderDTO.PurchaseOrderId))
                            {
                                var orderRemarks = inventoryNotesListOnDisplay.Where(x => x.ParafaitObjectId == purchaseOrderDTO.PurchaseOrderId).FirstOrDefault().Notes;
                                purchaseOrderDTO.OrderRemarks = orderRemarks;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(purchaseOrderDTOList);
            return purchaseOrderDTOList;
        }
        /// <summary>
        /// SetDisplayTagsVM
        /// </summary>
        private void SetDisplayTagsVM()
        {
            log.LogMethodEntry();
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
                                               Text = MessageContainerList.GetMessage(ExecutionContext,  " PURCHASE ORDER "),
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
            log.LogMethodExit();
        }

        /// <summary>
        /// LoadLables
        /// </summary>
        private void LoadLables()
        {
            log.LogMethodEntry();
            buttonClose = MessageContainerList.GetMessage(ExecutionContext, "CANCEL");
            buttonOK = MessageContainerList.GetMessage(ExecutionContext, "ADD");
            moduleName = MessageContainerList.GetMessage(ExecutionContext, "PURCHASE ORDER VIEW");
            log.LogMethodExit();
        }

        /// <summary>
        /// AddButtonClick
        /// </summary>
        /// <param name="param"></param>
        private void AddButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            purchaseOrderView = param as PurchaseOrderView;
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
                selectedPurchaseOrderDTOList = new List<PurchaseOrderDTO>();
                foreach (object data in CustomDataGridVM.SelectedItems)
                {
                    PurchaseOrderDTO purchaseOrderDTO = data as PurchaseOrderDTO;
                    if (purchaseOrderDTO != null)
                    {
                        selectedPurchaseOrderDTOList.Add(purchaseOrderDTO);
                    }
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
            }
            FormDialogResult = DialogResult.OK;
            purchaseOrderView.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// NavigationClick
        /// </summary>
        /// <param name="param"></param>
        private void NavigationClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            purchaseOrderView = param as PurchaseOrderView;
            try
            {
                if (purchaseOrderView != null)
                {
                    FormDialogResult = DialogResult.OK;
                    purchaseOrderView.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SuccessMessage = ex.ToString();
                FormDialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// CloseButtonClick
        /// </summary>
        /// <param name="param"></param>
        private void CloseButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            purchaseOrderView = param as PurchaseOrderView;
            try
            {
                if (purchaseOrderView != null)
                {
                    FormDialogResult = DialogResult.OK;
                    purchaseOrderView.Close();
                }
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
                FormDialogResult = DialogResult.Cancel;
            };
        }

        /// <summary>
        /// ShowMessagePopup
        /// </summary>
        /// <param name="heading"></param>
        /// <param name="subHeading"></param>
        /// <param name="content"></param>
        private void ShowMessagePopup(string heading, string subHeading, string content)
        {
            log.LogMethodEntry(heading, subHeading, content);
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            messagePopupView.Owner = purchaseOrderView;
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
        #endregion Methods
    }
}

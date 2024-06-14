/********************************************************************************************
 * Project Name - Invenotry UI
 * Description  - ReceiptVM
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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Inventory.Location;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.ViewContainer;
namespace Semnox.Parafait.InventoryUI
{

    /// <summary>
    /// ReceiptViewDTO
    /// </summary>
    public class ReceiptViewDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string poNumber;
        private string productCode;
        private string productDescription;
        private string poDate;
        private string invoiceNumber;
        private string grnNumber;
        private string receivedQuantity;
        private string location;
        private string receivedDate;
        public string PONumber { get { return poNumber; } set { poNumber = value; } }
        public string Code { get { return productCode; } set { productCode = value; } }
        public string Description { get { return productDescription; } set { productDescription = value; } }
        public string PODate { get { return poDate; } set { poDate = value; } }
        public string InvoiceNumber { get { return invoiceNumber; } set { invoiceNumber = value; } }
        public string GRNNumber { get { return grnNumber; } set { grnNumber = value; } }
        public string ReceivedQuantity { get { return receivedQuantity; } set { receivedQuantity = value; } }
        public string Location { get { return location; } set { location = value; } }
        public string ReceivedDate { get { return receivedDate; } set { receivedDate = value; } }

        public ReceiptViewDTO()
        {
            log.LogMethodEntry();
            this.productCode = string.Empty;
            this.productDescription = string.Empty;
            this.poNumber = string.Empty;
            this.poDate = string.Empty;
            this.invoiceNumber = string.Empty;
            this.grnNumber = string.Empty;
            this.receivedQuantity = string.Empty;
            this.location = string.Empty;
            this.receivedDate = string.Empty;
            log.LogMethodExit();
        }

        public ReceiptViewDTO(string productCode, string productDescription, string poNumber, string poDate,
                                string invoiceNumber, string receivedQuantity, string location, string receivedDate,string grnNumber)
        {
            log.LogMethodEntry(productCode, productDescription, poNumber, poDate, invoiceNumber, receivedQuantity, location, receivedDate, grnNumber);
            this.productCode = productCode;
            this.productDescription = productDescription;
            this.poNumber = poNumber;
            this.poDate = poDate;
            this.invoiceNumber = invoiceNumber;
            this.receivedQuantity = receivedQuantity;
            this.location = location;
            this.receivedDate = receivedDate;
            this.grnNumber = grnNumber;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// PurchaseOrderVM
    /// </summary>
    public class ReceiptVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ObservableCollection<InventoryReceiptDTO> inventoryReceiptDTOList;
        private Dictionary<string, CustomDataGridColumnElement> dataEntryElements;
        private ObservableCollection<ReceiptViewDTO> receiptViewDTOList;
        private DialogResult dialogResult;
        private string message;
        private string buttonClose;
        private string buttonOK;
        private ReceiptView receiptView;
        private DisplayTagsVM displayTagsVM;
        private CustomDataGridVM customDataGridVM;
        private ObservableCollection<ReceiptViewDTO> gridSource;
        private ICommand closeCommand;
        private ICommand navigationClickCommand;
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
        public ObservableCollection<ReceiptViewDTO> GridSource
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
        /// InventoryReceiptDTOList
        /// </summary>
        public ObservableCollection<InventoryReceiptDTO> InventoryReceiptDTOList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(inventoryReceiptDTOList);
                return inventoryReceiptDTOList;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref inventoryReceiptDTOList, value);
            }
        }

        /// <summary>
        /// ReceiptViewDTOList
        /// </summary>
        public ObservableCollection<ReceiptViewDTO> ReceiptViewDTOList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(receiptViewDTOList);
                return receiptViewDTOList;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref receiptViewDTOList, value);
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

        #endregion Properties
        #region Constructor
        /// <summary>
        /// ReceiptVM
        /// </summary>
        /// <param name="executionContext"></param>
        public ReceiptVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.ExecutionContext = executionContext;
            LoadLables();
            SetDisplayTagsVM();
            string quantityFormat = ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, "INVENTORY_QUANTITY_FORMAT");
            dataEntryElements = new Dictionary<string, CustomDataGridColumnElement>();
            dataEntryElements.Add("PONumber", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "Order Number"), Type = DataEntryType.TextBox, IsReadOnly = true});
            dataEntryElements.Add("InvoiceNumber", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "Invoice Number"), Type = DataEntryType.TextBlock, IsReadOnly = true });
            dataEntryElements.Add("ReceivedDate", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "Received Date"), Type = DataEntryType.TextBlock, IsReadOnly = true });
            dataEntryElements.Add("Code", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "Product Code"), Type = DataEntryType.TextBlock, IsReadOnly = true});
            dataEntryElements.Add("ReceivedQuantity", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "Received Quantity"), Type = DataEntryType.TextBlock, IsReadOnly = true , DataGridColumnStringFormat = quantityFormat, DataGridColumnHorizontalAlignment = System.Windows.HorizontalAlignment.Right });
            dataEntryElements.Add("Description", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "Description"), Type = DataEntryType.TextBlock, IsReadOnly = true });
            dataEntryElements.Add("GRNNumber", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "GRN Number"), Type = DataEntryType.TextBlock, IsReadOnly = true });
            dataEntryElements.Add("Location", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "Received Location"), Type = DataEntryType.TextBlock, IsReadOnly = true });
             using (NoSynchronizationContextScope.Enter())
            {
                closeCommand = new DelegateCommand(CloseButtonClick);
                navigationClickCommand = new DelegateCommand(NavigationClick);
            };
            using (NoSynchronizationContextScope.Enter())
            {
                
                Task<List<ReceiptViewDTO>> t = GetReceiptLines();
                t.Wait();
                List<ReceiptViewDTO> result = t.Result;
                log.LogVariableState("ReceiveStockViewDTO - ", result);
                if (result != null && result.Any())
                {
                    ReceiptViewDTOList = new ObservableCollection<ReceiptViewDTO>(result);
                    GridSource = new ObservableCollection<ReceiptViewDTO>(ReceiptViewDTOList);
                }
                else
                {
                    string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 3078); // "No items are not received today"); // Message Number 
                    //ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Information"), MessageContainerList.GetMessage(ExecutionContext, "Receipts"), ErrorMessage);
                    log.Debug("ErrorMessage  : " + ErrorMessage);
                    throw new ValidationException(ErrorMessage);
                }
            }
            customDataGridVM = new CustomDataGridVM(ExecutionContext)
            {
                CollectionToBeRendered = new ObservableCollection<object>(GridSource),
                HeaderCollection = dataEntryElements,
                ShowSearchTextBox = false,
                SelectOption = SelectOption.None,
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
                                               Text = MessageContainerList.GetMessage(ExecutionContext,  " RECEIPTS "),
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
            moduleName = MessageContainerList.GetMessage(ExecutionContext, "RECEIPTS VIEW");
            log.LogMethodExit();
        }



        private void NavigationClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            receiptView = param as ReceiptView;
            try
            {
                if (receiptView != null)
                {
                    FormDialogResult = DialogResult.OK;
                    receiptView.Close();
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
            receiptView = param as ReceiptView;
            try
            {
                if (receiptView != null)
                {
                    FormDialogResult = DialogResult.OK;
                    receiptView.Close();
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

        private async Task<List<ReceiptViewDTO>> GetReceiptLines()
        {
            log.LogMethodEntry();
            List<InventoryReceiptDTO> inventoryReceiptDTOList = new List<InventoryReceiptDTO>();
            List<ReceiptViewDTO> result = new List<ReceiptViewDTO>();
            double startTime = 6;
            string businessStartTime = ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, "BUSINESS_DAY_START_TIME");
            log.Debug("businessStartTime : " + businessStartTime);
            try
            {
                startTime = Convert.ToDouble(businessStartTime);
                log.Debug("startTime : " + startTime);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                startTime = 6;
            }
            List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParams = new List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>>
                {
                new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIVE_FROM_DATE, startTime.ToString()),
                new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIVE_TO_DATE, ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)),
                new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.RECEIVED_BY, ExecutionContext.GetUserId().ToString()),
                new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.SITE_ID, ExecutionContext.GetSiteId().ToString())
                };
            try
            {
                using (NoSynchronizationContextScope.Enter())
                {
                    IReceiptUseCases receiptUseCases = InventoryUseCaseFactory.GetReceiptsUseCases(ExecutionContext);
                    inventoryReceiptDTOList = await receiptUseCases.GetReceipts(searchParams, true, true);
                    log.LogVariableState("InventoryReceiptDTOList ", result);
                    if (inventoryReceiptDTOList != null && inventoryReceiptDTOList.Any())
                    {
                        inventoryReceiptDTOList = inventoryReceiptDTOList.OrderByDescending(x => x.ReceiptId).ToList();
                        ILocationUseCases locationUseCases = InventoryUseCaseFactory.GetLocationUseCases(ExecutionContext);
                        List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParameters = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
                        searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.IS_ACTIVE, "1"));
                        searchParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                        List<LocationDTO> locationDTOList = await locationUseCases.GetLocations(searchParameters);
                        if (locationDTOList == null)
                        {
                            locationDTOList = new List<LocationDTO>();
                        }
                        foreach (InventoryReceiptDTO inventoryReceiptDTO in inventoryReceiptDTOList)
                        {
                            if (inventoryReceiptDTO.InventoryReceiveLinesListDTO != null && inventoryReceiptDTO.InventoryReceiveLinesListDTO.Any())
                            {
                                foreach (InventoryReceiveLinesDTO inventoryReceiveLinesDTO in inventoryReceiptDTO.InventoryReceiveLinesListDTO)
                                {
                                    string locationName = locationDTOList.Where(x => x.LocationId == inventoryReceiveLinesDTO.LocationId).FirstOrDefault().Name;
                                    ReceiptViewDTO receiptViewDTO = new ReceiptViewDTO(inventoryReceiveLinesDTO.ProductCode, inventoryReceiveLinesDTO.Description,
                                                                                        inventoryReceiptDTO.OrderNumber, inventoryReceiptDTO.OrderDate.ToString(ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT")),
                                                                                        inventoryReceiptDTO.VendorBillNumber,
                                                                                        inventoryReceiveLinesDTO.Quantity.ToString(),
                                                                                        locationName,
                                                                                        inventoryReceiptDTO.ReceiveDate.ToString(ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT")), inventoryReceiptDTO.GRN);
                                    result.Add(receiptViewDTO);
                                }
                            }
                        }
                    }
                    log.LogMethodExit(result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private void ShowMessagePopup(string heading, string subHeading, string content)
        {
            log.LogMethodEntry(heading, subHeading, content);

            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            messagePopupView.Owner = receiptView;

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
            if (receiptView != null)
            {
                receiptView.Close();
            }
        }
        #endregion Methods
    }
}

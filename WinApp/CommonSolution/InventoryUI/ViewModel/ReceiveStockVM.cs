/********************************************************************************************
 * Project Name - Invenotry UI
 * Description  - Receive Stock UI is used to receive the item at POS
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
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Inventory.Location;
using Semnox.Parafait.Inventory.Requisition;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using Semnox.Parafait.RedemptionUI;
using Semnox.Parafait.Vendor;
using Semnox.Parafait.ViewContainer;
namespace Semnox.Parafait.InventoryUI
{
    /// <summary>
    /// ProductViewDTO
    /// </summary>
    public class ProductViewDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int productId;
        private string productCode;
        private string productDescription;
        private string barCode;
        public int ProductId { get { return productId; } set { productId = value; } }
        public string Code { get { return productCode; } set { productCode = value; } }
        public string Description { get { return productDescription; } set { productDescription = value; } }
        public string BarCode { get { return barCode; } set { barCode = value; } }

        public ProductViewDTO()
        {
            log.LogMethodEntry();
            this.productCode = string.Empty;
            this.productDescription = string.Empty;
            this.barCode = string.Empty;
            this.productId = -1;
            log.LogMethodExit();
        }

        public ProductViewDTO(int productId, string productCode, string productDescription, string barCode)
        {
            log.LogMethodEntry(productId, productCode, productDescription, barCode);
            this.barCode = barCode;
            this.productId = productId;
            this.productCode = productCode;
            this.productDescription = productDescription;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// ReceiveStockViewDTO
    /// </summary>
    public class ReceiveStockViewDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string poNumber;
        private string poDate;
        private string productCode;
        private string productDescription;
        private decimal quantity;
        private string unitOfMeasurement;
        private string remarks;
        private bool isReceivedLine;
        private int purchaseOrderLineId;
        private int receiveLocationId;
        private decimal quantityAlreadyReceived;
        private decimal poBalanceQuantity;
        private bool notifyingObjectIsChanged;

        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        public string PONumber { get { return poNumber; } set { poNumber = value; } }
        public string PODate { get { return poDate; } set { poDate = value; } }
        public string ProductCode { get { return productCode; } set { productCode = value; } }
        public string ProductDescription { get { return productDescription; } set { productDescription = value; } }
        public decimal Quantity { get { return quantity; } set { quantity = value; this.IsChanged = true; } }
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }
        public string UnitOfMeasurement { get { return unitOfMeasurement; } set { unitOfMeasurement = value; this.IsChanged = true; } }
        public bool IsReceivedLine { get { return isReceivedLine; } set { isReceivedLine = value; this.IsChanged = true; } }
        public int PurchaseOrderLineId { get { return purchaseOrderLineId; } set { purchaseOrderLineId = value; } }
        public int ReceiveLocationId { get { return receiveLocationId; } set { receiveLocationId = value; } }
        public decimal QuantityAlreadyReceived { get { return quantityAlreadyReceived; } set { quantityAlreadyReceived = value; } }
        public decimal POBalanceQuantity { get { return poBalanceQuantity; } set { poBalanceQuantity = value; } }

        public ReceiveStockViewDTO()
        {
            log.LogMethodEntry();
            this.poNumber = string.Empty;
            this.poDate = string.Empty;
            this.productCode = string.Empty;
            this.productDescription = string.Empty;
            this.quantity = 0;
            this.remarks = string.Empty;
            this.unitOfMeasurement = string.Empty;
            this.isReceivedLine = false;
            this.purchaseOrderLineId = -1;
            this.receiveLocationId = -1;
            this.quantityAlreadyReceived = 0;
            this.poBalanceQuantity = 0;
            log.LogMethodExit();
        }
        /// <summary>
        /// ReceiveStockViewDTO
        /// </summary>
        /// <param name="poNumber"></param>
        /// <param name="poDate"></param>
        /// <param name="productCode"></param>
        /// <param name="productDescription"></param>
        /// <param name="quantity"></param>
        /// <param name="remarks"></param>
        /// <param name="isReceivedLine"></param>
        /// <param name="purchaseOrderLineId"></param>
        /// <param name="poBalanceQuantity"></param>
        public ReceiveStockViewDTO(string poNumber, string poDate, string productCode, string productDescription,
                                   decimal quantity, string remarks, int purchaseOrderLineId,
                                   int receiveLocationId, decimal quantityAlreadyReceived, decimal poBalanceQuantity, string unitOfMeasurement)
        {
            log.LogMethodEntry(poNumber, poDate, productCode, productDescription, quantity, remarks, isReceivedLine,
                               purchaseOrderLineId, receiveLocationId, quantityAlreadyReceived, poBalanceQuantity, unitOfMeasurement);
            this.poNumber = poNumber;
            this.poDate = poDate;
            this.productCode = productCode;
            this.productDescription = productDescription;
            this.quantity = quantity;
            this.remarks = remarks;
            this.purchaseOrderLineId = purchaseOrderLineId;
            this.receiveLocationId = receiveLocationId;
            this.quantityAlreadyReceived = quantityAlreadyReceived;
            this.poBalanceQuantity = poBalanceQuantity;
            this.unitOfMeasurement = unitOfMeasurement;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }
        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// StatusCode
    /// </summary>
    public enum StatusCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Success,
        /// <summary>
        /// Failure
        /// </summary>
        Failure
    }

    /// <summary>
    /// ReceiveStockVM
    /// </summary>
    public class ReceiveStockVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ObservableCollection<VendorDTO> vendorDTOList;
        private Dictionary<string, CustomDataGridColumnElement> dataEntryElements;
        private RedemptionDeviceCollection redemptionDeviceCollection;
        private List<PurchaseOrderDTO> purchaseOrderDTOList;
        DeviceClass barcodeReader;
        private VendorDTO selectedVendorDTO;
        private string selectedGRNNumber;
        private string selectedProductCode;
        private string selectedPONumber;
        private StatusCode statusCode;
        private string message;
        private string buttonClose;
        private string buttonRefresh;
        private string buttonReceive;
        private string buttonReceipt;
        private List<int> productIdList;
        private int productDefaultLocationId;
        private string clearButtonHeading;
        private string searchButtonHeading;
        private string searchHeading;
        private string poHeading;
        private string vendorHeading;
        private string grnNumberHeading;

        private string scannedBarCode;
        private int barcodedeviceAddress = -1;

        private ReceiveStockView receiveStockView;
        private ObservableCollection<ReceiveStockViewDTO> receiveStockViewDTOList;
        private ReceiveStockViewDTO selectedReceiveStockViewDTO;
        private DisplayTagsVM displayTagsVM;
        private CustomDataGridVM customDataGridVM;
        private ObservableCollection<ReceiveStockViewDTO> gridSource;
        private ICommand searchCommand;
        private ICommand clearCommand;
        private ICommand closeCommand;
        private ICommand receiptCommand;
        private ICommand removeButtonClickCommand;
        private ICommand saveCommand;
        private ICommand refreshCommand;
        private ICommand rowSectionCommand;
        private ICommand navigationClickCommand;
        private ICommand onLoadCommand;
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

        public DeviceClass BarCodeReader
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(barcodeReader);
                return barcodeReader;
            }
            set
            {
                log.LogMethodEntry(barcodeReader, value);
                SetProperty(ref barcodeReader, value);
                log.LogMethodExit(barcodeReader);
            }
        }

        public RedemptionDeviceCollection RedemptionDeviceCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionDeviceCollection);
                return redemptionDeviceCollection;
            }
            set
            {
                log.LogMethodEntry(redemptionDeviceCollection, value);
                SetProperty(ref redemptionDeviceCollection, value);
                log.LogMethodExit(redemptionDeviceCollection);
            }
        }

        public List<PurchaseOrderDTO> PurchaseOrderDTOList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(purchaseOrderDTOList);
                return purchaseOrderDTOList;
            }
            set
            {
                log.LogMethodEntry(purchaseOrderDTOList, value);
                SetProperty(ref purchaseOrderDTOList, value);
                log.LogMethodExit(purchaseOrderDTOList);
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
        public ObservableCollection<ReceiveStockViewDTO> GridSource
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
        /// ReceiveStockViewDTOList
        /// </summary>
        public ObservableCollection<ReceiveStockViewDTO> ReceiveStockViewDTOList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(receiveStockViewDTOList);
                return receiveStockViewDTOList;
            }
            set
            {
                log.LogMethodEntry(value);
                receiveStockViewDTOList = value;
            }
        }

        public int BarCodeDeviceAddress
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(barcodedeviceAddress);
                return barcodedeviceAddress;
            }
            set
            {
                log.LogMethodEntry(barcodedeviceAddress, value);
                SetProperty(ref barcodedeviceAddress, value);
                log.LogMethodExit(barcodedeviceAddress);
            }
        }
        public string ScannedBarCode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(scannedBarCode);
                return scannedBarCode;
            }
            set
            {
                log.LogMethodEntry(scannedBarCode, value);
                SetProperty(ref scannedBarCode, value);
                log.LogMethodExit(scannedBarCode);
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
        public string ButtonRefresh
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonRefresh);
                return buttonRefresh;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref buttonRefresh, value);
                }
            }
        }
        public string ButtonReceive
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonReceive);
                return buttonReceive;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref buttonReceive, value);
                }
            }
        }
        public string ButtonReceipt
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return buttonReceipt;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref buttonReceipt, value);
                }
            }
        }
        public string ClearButtonHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(clearButtonHeading);
                return clearButtonHeading;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref clearButtonHeading, value);
                }
            }
        }
        public string SearchButtonHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchButtonHeading);
                return searchButtonHeading;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref searchButtonHeading, value);
                }
            }
        }
        public string SearchHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchHeading);
                return searchHeading;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref searchHeading, value);
                }
            }
        }
        public string POHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(poHeading);
                return poHeading;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref poHeading, value);
                }
            }
        }
        public List<int> ProductIdList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(productIdList);
                return productIdList;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref productIdList, value);
            }
        }
        public string VendorHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(vendorHeading);
                return vendorHeading;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref vendorHeading, value);
                }
            }
        }
        public string GRNNumberHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(grnNumberHeading);
                return grnNumberHeading;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref grnNumberHeading, value);
                }
            }
        }

        /// <summary>
        /// SelectedReceiveStockViewDTO
        /// </summary>
        public ReceiveStockViewDTO SelectedReceiveStockViewDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedReceiveStockViewDTO);
                return selectedReceiveStockViewDTO;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref selectedReceiveStockViewDTO, value);
                }
            }
        }
        /// <summary>
        /// SelectedVendor
        /// </summary>
        public VendorDTO SelectedVendor
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedVendorDTO);
                return selectedVendorDTO;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref selectedVendorDTO, value);
                }
            }
        }
        /// <summary>
        /// navigationClickCommand
        /// </summary>
        /// <summary>
        /// vendorDTOList
        /// </summary>
        public ObservableCollection<VendorDTO> VendorList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(vendorDTOList);
                return vendorDTOList;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref vendorDTOList, value);
            }
        }
        /// <summary>
        /// selectedGRNNumber
        /// </summary>
        public string SelectedGRNValue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedGRNNumber);
                return selectedGRNNumber;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedGRNNumber, value);
            }
        }

        /// <summary>
        /// selectedProductCode
        /// </summary>
        public string SelectedProductCode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedProductCode);
                return selectedProductCode;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedProductCode, value);
            }
        }
        /// <summary>
        /// selectedPONumber
        /// </summary>
        public string SelectedPONumber
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedPONumber);
                return selectedPONumber;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedPONumber, value);
            }
        }

        /// <summary>
        /// StatusCode
        /// </summary>
        public StatusCode StatusCode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(statusCode);
                return statusCode;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref statusCode, value);
                log.LogMethodEntry(statusCode);
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
        /// SearchCommand
        /// </summary>
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
                log.LogMethodEntry(value);
                searchCommand = value;
            }
        }

        /// <summary>   
        /// SearchCommand
        /// </summary>
        public ICommand OnLoadCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(onLoadCommand);
                return onLoadCommand;
            }
            set
            {
                log.LogMethodEntry(onLoadCommand, value);
                SetProperty(ref onLoadCommand, value);
                log.LogMethodExit(onLoadCommand);
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
                log.LogMethodEntry(rowSectionCommand, value);
                SetProperty(ref rowSectionCommand, value);
                log.LogMethodExit(rowSectionCommand);
            }
        }

        /// <summary>
        /// ClearCommand
        /// </summary>
        public ICommand ClearCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(clearCommand);
                return clearCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                clearCommand = value;
            }
        }
        public ICommand RemoveButtonClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(removeButtonClickCommand);
                return removeButtonClickCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                removeButtonClickCommand = value;
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
        /// ReceiptCommand
        /// </summary>
        public ICommand ReceiptCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(receiptCommand);
                return receiptCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                receiptCommand = value;
            }
        }

        /// <summary>
        /// SaveCommand
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(saveCommand);
                return saveCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                saveCommand = value;
            }
        }

        /// <summary>
        /// RefreshCommand
        /// </summary>
        public ICommand RefreshCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(refreshCommand);
                return refreshCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                refreshCommand = value;
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
        /// NotificationTagsVM
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="SearchParameters"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public ReceiveStockVM(ExecutionContext executionContext, DeviceClass barcodeReader)
        {
            log.LogMethodEntry(executionContext, barcodeReader);
            try
            {
                this.ExecutionContext = executionContext;
                this.barcodeReader = barcodeReader;

                if (barcodeReader != null)
                {
                    barcodeReader.Register(BarCodeScanCompleteEventHandle);
                }
                LoadLables();
                SetDisplayTagsVM();
                string quantityFormat = ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, "INVENTORY_QUANTITY_FORMAT");
                dataEntryElements = new Dictionary<string, CustomDataGridColumnElement>();
                dataEntryElements.Add("PONumber", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "PO Number"), Type = DataEntryType.TextBlock, IsReadOnly = true, DataGridColumnFixedSize = 150 });
                dataEntryElements.Add("PODate", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "PO Date"), Type = DataEntryType.TextBlock, IsReadOnly = true, DataGridColumnFixedSize = 230 });
                dataEntryElements.Add("ProductCode", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "Product Code"), Type = DataEntryType.TextBlock, IsReadOnly = true, DataGridColumnFixedSize = 200 });
                dataEntryElements.Add("ProductDescription", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "Description"), Type = DataEntryType.TextBlock, IsReadOnly = true, DataGridColumnFixedSize = 200 });
                dataEntryElements.Add("POBalanceQuantity", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "PO BalanceQty"), Type = DataEntryType.TextBlock, IsReadOnly = true, DataGridColumnFixedSize = 150, DataGridColumnStringFormat = quantityFormat, DataGridColumnHorizontalAlignment = System.Windows.HorizontalAlignment.Right });
                dataEntryElements.Add("UnitOfMeasurement", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "UOM"), Type = DataEntryType.TextBlock, IsReadOnly = true, DataGridColumnFixedSize = 100 });
                dataEntryElements.Add("Quantity", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "Quantity"), Type = DataEntryType.TextBox, IsEnable = true, DataGridColumnFixedSize = 100, DataGridColumnStringFormat = quantityFormat, DataGridColumnHorizontalAlignment = System.Windows.HorizontalAlignment.Right });
                dataEntryElements.Add(" ", new CustomDataGridColumnElement() { Heading = MessageContainerList.GetMessage(ExecutionContext, "..."), Type = DataEntryType.Button, ActionButtonType = DataGridButtonType.More });

                using (NoSynchronizationContextScope.Enter())
                {
                    searchCommand = new DelegateCommand(SearchButtonClick);
                    clearCommand = new DelegateCommand(ClearSearchButtonClick);
                    closeCommand = new DelegateCommand(CloseButtonClick);
                    saveCommand = new DelegateCommand(ReceiveButtonClick);
                    receiptCommand = new DelegateCommand(ReceiptButtonClick);
                    refreshCommand = new DelegateCommand(ClearButtonClick);
                    removeButtonClickCommand = new DelegateCommand(RemoveButtonClick);
                    navigationClickCommand = new DelegateCommand(NavigationClick);
                    onLoadCommand = new DelegateCommand(OnLoaded);
                    selectionChangedCmd = new DelegateCommand(SelectionChanged);
                    LoadVendorsAsync();
                };
                List<ReceiveStockViewDTO> result = new List<ReceiveStockViewDTO>();
                ReceiveStockViewDTOList = new ObservableCollection<ReceiveStockViewDTO>(result);
                gridSource = ReceiveStockViewDTOList;
                customDataGridVM = new CustomDataGridVM(ExecutionContext)
                {
                    CollectionToBeRendered = new ObservableCollection<object>(gridSource),
                    HeaderCollection = dataEntryElements,
                    ShowSearchTextBox = false,
                    IsComboAndSearchVisible = false
                };
                FooterVM = new FooterVM(this.ExecutionContext)
                {
                    Message = "",
                    MessageType = MessageType.None,
                    HideSideBarVisibility = Visibility.Collapsed
                };
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, ex.Message);
                //ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Validation"), MessageContainerList.GetMessage(ExecutionContext, "Receive"), ErrorMessage);
                SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
            }
            log.LogMethodExit();
        }

        private void CellContentClick(object obj)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        #endregion Constructor

        #region Methods

        /// <summary>
        /// SetFooterContent
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        internal void SetFooterContent(string message, MessageType messageType)
        {
            log.LogMethodEntry(message);
            if (FooterVM != null)
            {
                FooterVM.Message = message;
                FooterVM.MessageType = messageType;
            }
            log.LogMethodExit();
        }

        internal void RegisterDevices()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (barcodeReader != null)
                {
                    barcodeReader.Register(BarCodeScanCompleteEventHandle);
                }
            });
            log.LogMethodExit();
        }
        internal void UnRegisterDevices()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (barcodeReader != null)
                {
                    barcodeReader.UnRegister();
                }
            });
            log.LogMethodExit();
        }


        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            string code = "";
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                code = ProcessScannedBarCode(
                                            checkScannedEvent.Message,
                                            ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LEFT_TRIM_BARCODE", 0),
                                            ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "RIGHT_TRIM_BARCODE", 0));
                log.Debug("code: " + code);
                SelectedProductCode = code;
            }
            log.LogMethodExit();
        }
        private string ProcessScannedBarCode(string Code, int leftTrim, int rightTrim)
        {
            log.LogMethodEntry(Code, leftTrim, rightTrim);
            try
            {
                Code = System.Text.RegularExpressions.Regex.Replace(Code, @"\W+", "");
                log.LogMethodExit((Code.Substring(leftTrim, Code.Length - rightTrim - leftTrim)));
                return (Code.Substring(leftTrim, Code.Length - rightTrim - leftTrim));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in Processing Scanned Bar Code", ex);
                log.LogMethodExit(Code);
                return Code;
            }
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            receiveStockView = parameter as ReceiveStockView;
            receiveStockView.Closing += OnClosingCommand;
            log.LogMethodExit();
        }
        private async Task<List<PurchaseOrderDTO>> GetPurchaseOrders(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>
                                                                                  SearchParameters)
        {
            log.LogMethodEntry(SearchParameters);
            List<PurchaseOrderDTO> result = new List<PurchaseOrderDTO>();
            IPurchaseOrderUseCases purchaseOrderUseCases = InventoryUseCaseFactory.GetPurchaseOrderUseCases(ExecutionContext);
            result = await purchaseOrderUseCases.GetPurchaseOrders(SearchParameters, true, true);
            log.LogVariableState("PurchaseOrderDTOList ", result);
            log.LogMethodExit(result);
            return result;
        }

        private void OnClosingCommand(object sender, System.ComponentModel.CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UnRegisterDevices();
            DisposeAllDevices();
            log.LogMethodExit();
        }
        internal void DisposeAllDevices()
        {
            log.LogMethodEntry();
            if (RedemptionDeviceCollection != null)
            {
                RedemptionDeviceCollection.Dispose();
            }
            log.LogMethodExit();
        }
        private async Task LoadVendorsAsync()
        {
            log.LogMethodEntry();
            List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> vendorSearchParams = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>
            {
                new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, ExecutionContext.GetSiteId().ToString())
            };
            IVendorUseCases vendorUseCases = InventoryUseCaseFactory.GetVendorUseCases(ExecutionContext);
            List<VendorDTO> result = await vendorUseCases.GetVendors(vendorSearchParams);
            log.LogVariableState("vendorDTOList", result);
            if (result == null)
            {
                result = new List<VendorDTO>();
            }
            VendorDTO vendorDTO = new VendorDTO();
            vendorDTO.Name = MessageContainerList.GetMessage(ExecutionContext, "Select");
            result.Insert(0, vendorDTO);
            vendorDTOList = new ObservableCollection<VendorDTO>(result);
            log.LogVariableState("vendorDTOList", vendorDTOList);
            SelectedVendor = vendorDTOList.FirstOrDefault();
            log.LogVariableState("SelectedVendor", SelectedVendor);
            log.LogMethodExit();
        }

        private void SetDisplayTagsVM()
        {
            log.LogMethodEntry("SetDisplayTagsVM");
            try
            {
                POSMachineContainerDTO posMachineContainerDTO = POSMachineViewContainerList.GetPOSMachineContainerDTO(ExecutionContext);
                LocationContainerDTO locationContainerDTO = LocationViewContainerList.GetLocationContainerDTOList(ExecutionContext)
                                                             .Where(x => x.LocationId == posMachineContainerDTO.InventoryLocationId).FirstOrDefault();
                string locationName = string.Empty;
                if (locationContainerDTO != null)
                {
                    locationName = locationContainerDTO.Name;
                }
                else
                {
                    locationName = MessageContainerList.GetMessage(ExecutionContext, "POS");
                }
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
                                                     Text = MessageViewContainerList.GetMessage(ExecutionContext,"POS Name"), TextSize = TextSize.Medium,FontWeight = FontWeights.Bold
                                                 },
                                                 new DisplayTag()
                                                 {
                                                     Text =ExecutionContext.POSMachineName , FontWeight = FontWeights.Bold,TextSize = TextSize.Small,
                                                 }
                                          //new DisplayTag()
                                          //{
                                          //     Text = MessageContainerList.GetMessage(ExecutionContext,  "POS Name: " + ExecutionContext.POSMachineName  ),
                                          //     TextSize = TextSize.Medium,
                                          //     FontWeight = System.Windows.FontWeights.Bold
                                          //}
                                      }
                                    };
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private async Task<List<ReceiveStockViewDTO>> GetReceiveStockView(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>
                                                                       SearchParameters)
        {
            try
            {
                log.LogMethodEntry(SearchParameters);
                List<ReceiveStockViewDTO> result = new List<ReceiveStockViewDTO>();
                purchaseOrderDTOList = await GetPurchaseOrders(SearchParameters);  // await purchaseOrderUseCases.GetPurchaseOrders(SearchParameters, true, true);
                log.LogVariableState("purchaseOrderDTOList ", purchaseOrderDTOList);
                if (purchaseOrderDTOList != null && purchaseOrderDTOList.Any())
                {
                    if (string.IsNullOrWhiteSpace(SelectedProductCode) && string.IsNullOrWhiteSpace(SelectedPONumber) == false)
                    {
                        result = BuildReceiveStockLines(purchaseOrderDTOList.FirstOrDefault());
                        log.LogVariableState("result : ", result);
                    }
                    else
                    {
                        foreach (PurchaseOrderDTO purchaseOrderDTO in purchaseOrderDTOList)
                        {
                            log.LogVariableState("purchaseOrderDTO : ", purchaseOrderDTO);
                            string productCode = string.Empty;
                            string description = string.Empty;
                            string poLineUOM = string.Empty;
                            decimal quantity = 0;

                            int purchaseOrderLineId = -1;
                            PurchaseOrderLineDTO purchaseOrderLineDTO = null;
                            if (purchaseOrderDTO.PurchaseOrderLineListDTO != null && purchaseOrderDTO.PurchaseOrderLineListDTO.Any() && productIdList.Any())
                            {
                                foreach (int productId in productIdList)
                                {
                                    purchaseOrderLineDTO = purchaseOrderDTO.PurchaseOrderLineListDTO.Where(x => x.ProductId == productId).FirstOrDefault();
                                    if (purchaseOrderLineDTO == null)
                                    {
                                        continue;
                                    }
                                    log.LogVariableState("purchaseOrderLineDTO : ", purchaseOrderLineDTO);
                                    productCode = purchaseOrderLineDTO.ItemCode;
                                    description = purchaseOrderLineDTO.Description;
                                    quantity = Convert.ToDecimal(purchaseOrderLineDTO.Quantity);
                                    purchaseOrderLineId = purchaseOrderLineDTO.PurchaseOrderLineId;
                                    int requisitionId = purchaseOrderLineDTO.RequisitionId;
                                    int requisitionLineId = purchaseOrderLineDTO.RequisitionLineId;
                                    decimal quantityAlreadyReceived = 0;
                                    quantityAlreadyReceived = GetReceivedLineQuantity(purchaseOrderDTO, purchaseOrderLineId);
                                    quantity = Convert.ToDecimal(purchaseOrderLineDTO.Quantity) - quantityAlreadyReceived;
                                    quantity = Math.Round(Convert.ToDecimal(quantity), 2, MidpointRounding.AwayFromZero);
                                    log.Debug("quantity to receive := " + quantity);
                                    if (quantity <= 0)
                                    {
                                        log.Error("Purchase Order line is already received");
                                        continue;
                                    }
                                    //UOMContainer uomcontainer = CommonFuncs.GetUOMContainer();
                                    UOMContainer uOMContainer = new UOMContainer(ExecutionContext);
                                    List<UOMDTO> uomDTOList = uOMContainer.relatedUOMDTOList.FirstOrDefault(x => x.Key == purchaseOrderLineDTO.UOMId).Value;
                                    if (uomDTOList != null && uomDTOList.Any())
                                    {
                                        poLineUOM = uomDTOList.FirstOrDefault().UOM;
                                    }
                                    ReceiveStockViewDTO receiveStockViewDTO = new ReceiveStockViewDTO(purchaseOrderDTO.OrderNumber, purchaseOrderDTO.OrderDate.ToString(ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT")),
                                                                                                          productCode, description, 0, purchaseOrderDTO.OrderRemarks,
                                                                                                           purchaseOrderLineId, productDefaultLocationId,
                                                                                                          Convert.ToDecimal(quantityAlreadyReceived), quantity, poLineUOM);
                                    receiveStockViewDTO.AcceptChanges();
                                    log.LogVariableState("receiveStockViewDTO", receiveStockViewDTO);
                                    result.Add(receiveStockViewDTO);
                                }
                            }
                        }
                    }
                }
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<ReceiveStockViewDTO> BuildReceiveStockLines(PurchaseOrderDTO purchaseOrderDTO)
        {
            log.LogMethodEntry(purchaseOrderDTO);
            List<ReceiveStockViewDTO> result = new List<ReceiveStockViewDTO>();
            if (purchaseOrderDTO != null)
            {
                string productCode = string.Empty;
                string description = string.Empty;
                decimal quantity = 0;
                string poLineUOM = string.Empty;

                int purchaseOrderLineId = -1;
                if (purchaseOrderDTO.PurchaseOrderLineListDTO != null && purchaseOrderDTO.PurchaseOrderLineListDTO.Any())
                {
                    log.LogVariableState("purchaseOrderDTO.PurchaseOrderLineListDTO", purchaseOrderDTO.PurchaseOrderLineListDTO);
                    foreach (PurchaseOrderLineDTO lineDTO in purchaseOrderDTO.PurchaseOrderLineListDTO)
                    {
                        productCode = lineDTO.ItemCode;
                        description = lineDTO.Description;
                        quantity = Convert.ToDecimal(lineDTO.Quantity);
                        purchaseOrderLineId = lineDTO.PurchaseOrderLineId;
                        int requisitionId = lineDTO.RequisitionId;
                        int requisitionLineId = lineDTO.RequisitionLineId;
                        List<ProductsContainerDTO> productsContainerDTOList = ProductViewContainerList.GetActiveProductsContainerDTOList(ExecutionContext, ManualProductType.SELLABLE.ToString()).Where(x => x.InventoryItemContainerDTO != null).ToList();

                        log.LogVariableState("productsContainerDTOList", productsContainerDTOList);
                        ProductsContainerDTO filteredProductsContainerDTO = productsContainerDTOList.Where(x => x.InventoryItemContainerDTO.Code == lineDTO.ItemCode).FirstOrDefault();
                        if(filteredProductsContainerDTO == null)
                        {
                            log.Debug("Product not found in the container. Please check the product setup. IsSellable =Y");
                            continue;
                        }
                        log.LogVariableState("filteredProductsContainerDTO", filteredProductsContainerDTO);
                        if (filteredProductsContainerDTO.InventoryItemContainerDTO.LotControlled)
                        {
                            log.Debug(" LotControlled: " + filteredProductsContainerDTO.InventoryItemContainerDTO.LotControlled);
                            if (filteredProductsContainerDTO.InventoryItemContainerDTO.ExpiryType == "E")
                            {
                                log.Error("Product expiry type is Date. Cannot receive product &1.");
                                string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 3077, filteredProductsContainerDTO.InventoryItemContainerDTO.Code);
                                SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
                                continue;
                            }
                        }
                        if (filteredProductsContainerDTO.InventoryItemContainerDTO.MarketListItem)
                        {
                            log.Error("Product MarketList Item type . Cannot receive product &1.");
                            string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, " Cannot receive product " + filteredProductsContainerDTO.InventoryItemContainerDTO.Code + " Market List item");
                            SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
                            continue;
                        }
                        if (filteredProductsContainerDTO != null)
                        {
                            productDefaultLocationId = filteredProductsContainerDTO.InventoryItemContainerDTO.DefaultLocationId;
                            log.Debug("productDefaultLocationId := " + productDefaultLocationId);
                        }
                        else
                        {
                            string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "No Product added to receive ");
                            //ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Validation"), MessageContainerList.GetMessage(ExecutionContext, "Product"), ErrorMessage);
                            SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
                            log.Debug("ErrorMessage := " + ErrorMessage);
                            return result;
                        }

                        // To check if the order is already received partially 
                        // check the receive lines and get the recevied quantity
                        decimal quantityAlreadyReceived = 0;
                        quantityAlreadyReceived = GetReceivedLineQuantity(purchaseOrderDTO, purchaseOrderLineId);
                        quantity = Convert.ToDecimal(lineDTO.Quantity) - quantityAlreadyReceived;
                        quantity = Math.Round(Convert.ToDecimal(quantity), 2, MidpointRounding.AwayFromZero);
                        log.Debug("quantity to receive := " + quantity);
                        if (quantity <= 0)
                        {
                            log.Error("Purchase Order line is already received");
                            continue;
                        }
                        UOMContainer uomcontainer = new UOMContainer(ExecutionContext);
                        List<UOMDTO> uomDTOList = uomcontainer.relatedUOMDTOList.FirstOrDefault(x => x.Key == lineDTO.UOMId).Value;
                        if (uomDTOList != null && uomDTOList.Any())
                        {
                            poLineUOM = uomDTOList.FirstOrDefault().UOM;
                        }
                        ReceiveStockViewDTO receiveStockViewDTO = new ReceiveStockViewDTO(purchaseOrderDTO.OrderNumber, purchaseOrderDTO.OrderDate.ToString(ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT")),
                                                                                          productCode, description, 0, purchaseOrderDTO.OrderRemarks,
                                                                                          purchaseOrderLineId, productDefaultLocationId,
                                                                                          Convert.ToDecimal(quantityAlreadyReceived), quantity, poLineUOM);
                        receiveStockViewDTO.AcceptChanges();
                        log.LogVariableState("ReceiveStockViewDTO:= ", receiveStockViewDTO);
                        result.Add(receiveStockViewDTO);
                    }
                }
            }
            if (result.Any() == false)
            {
                string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 1307, purchaseOrderDTO.OrderNumber);
                //ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Validation"), MessageContainerList.GetMessage(ExecutionContext, "Purchase Order"), ErrorMessage);
                SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Error);
                log.Debug("ErrorMessage := " + ErrorMessage);
                return result;
            }
            log.LogMethodExit(result);
            return result;
        }

        private void LoadLables()
        {
            log.LogMethodEntry();
            GRNNumberHeading = MessageContainerList.GetMessage(ExecutionContext, "Invoice/Bill No.");
            vendorHeading = MessageContainerList.GetMessage(ExecutionContext, "Vendor");
            poHeading = MessageContainerList.GetMessage(ExecutionContext, "PO Number");
            searchButtonHeading = MessageContainerList.GetMessage(ExecutionContext, "SEARCH");
            clearButtonHeading = MessageContainerList.GetMessage(ExecutionContext, "CLEAR");
            buttonRefresh = MessageContainerList.GetMessage(ExecutionContext, "CLEAR");
            buttonClose = MessageContainerList.GetMessage(ExecutionContext, "CLOSE");
            buttonReceive = MessageContainerList.GetMessage(ExecutionContext, "RECEIVE");
            buttonReceipt = MessageContainerList.GetMessage(ExecutionContext, "RECEIPTS");
            SearchHeading = MessageContainerList.GetMessage(ExecutionContext, "Enter Product Code");
            moduleName = MessageContainerList.GetMessage(ExecutionContext, "RECEIVE STOCK");
            log.LogMethodExit();
        }


        private void SearchButtonClick(object param)
        {
            log.LogMethodEntry(param);
            IsLoadingVisible = true;
            SetFooterContent("", MessageType.None);
            ReceiveStockViewDTOList = new ObservableCollection<ReceiveStockViewDTO>();
            GridSource = new ObservableCollection<ReceiveStockViewDTO>(ReceiveStockViewDTOList);
            CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(GridSource);
            String ErrorMessage = String.Empty;
            receiveStockView = param as ReceiveStockView;
            try
            {
                List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> SearchParameters = new List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>();
                SearchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                SearchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERSTATUS, "Open,InProgress,Drop Ship"));
                SearchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.DOCUMENT_STATUS, "F"));
                if (string.IsNullOrWhiteSpace(selectedPONumber) && string.IsNullOrWhiteSpace(selectedProductCode))
                {
                    log.Debug("Please enter search input");
                    ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 1671); // Please enter search input
                    SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(selectedPONumber) == false && selectedPONumber.Contains("%"))
                {
                    SearchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERNUMBER_LIKE, selectedPONumber));
                    if (string.IsNullOrWhiteSpace(selectedProductCode) == false)
                    {
                        log.Debug("selectedProductCode : " + selectedProductCode);
                        productIdList = GetProductId(selectedProductCode);
                        if (productIdList.Any())
                        {
                            string idList = string.Join(",", productIdList);
                            SearchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.PRODUCT_ID_LIST, idList.ToString()));
                        }
                    }
                    BuildReceiveLines(SearchParameters);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(selectedPONumber) == false)
                    {
                        log.Debug("selectedPONumber : " + selectedPONumber);
                        SearchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERNUMBER, selectedPONumber));
                    }
                    if (string.IsNullOrWhiteSpace(selectedProductCode) == false)
                    {
                        log.Debug("selectedProductCode : " + selectedProductCode);
                        productIdList = GetProductId(selectedProductCode);
                        if (productIdList.Any())
                        {
                            string idList = string.Join(",", productIdList);
                            SearchParameters.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.PRODUCT_ID_LIST, idList.ToString()));
                        }
                    }
                    using (NoSynchronizationContextScope.Enter())
                    {
                        Task<List<ReceiveStockViewDTO>> t = GetReceiveStockView(SearchParameters);
                        t.Wait();
                        List<ReceiveStockViewDTO> result = t.Result;
                        log.LogVariableState("ReceiveStockViewDTO - ", result);
                        if (result != null && result.Any())
                        {
                            ReceiveStockViewDTOList = new ObservableCollection<ReceiveStockViewDTO>(result);
                            GridSource = new ObservableCollection<ReceiveStockViewDTO>(ReceiveStockViewDTOList);
                        }
                        else
                        {
                            ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 3075); // "No Open/InProgress Purchase Order record in Inventory"); // Message Number 
                            SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
                            log.Debug("ErrorMessage  : " + ErrorMessage);
                        }
                    }
                }
                CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(GridSource);
                if (CustomDataGridVM.CollectionToBeRendered.Count > 0)
                {
                    CustomDataGridVM.SelectedItem = CustomDataGridVM.CollectionToBeRendered[0];
                }
                //receiveStockView.ReceiveStockView_ContentRendered(null, null);
                IsLoadingVisible = false;
            }
            catch (UnauthorizedException ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.Message;
            }
        }


        /// <summary>
        /// GetProductId method returns the Product Id for the scanned product
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        private List<int> GetProductId(string searchText)
        {
            log.LogMethodEntry(searchText);
            List<int> selectedProductIdList = new List<int>();
            List<ProductsContainerDTO> productsContainerDTOList = ProductViewContainerList.GetActiveProductsContainerDTOList(ExecutionContext, ManualProductType.SELLABLE.ToString()).Where(x => x.InventoryItemContainerDTO != null).ToList();

            log.LogVariableState("productsContainerDTOList", productsContainerDTOList);
            List<ProductsContainerDTO> filteredProductsContainerDTO = productsContainerDTOList.Where(x => x.InventoryItemContainerDTO.Code == searchText
                                                                                                     || (x.InventoryItemContainerDTO.Description.ToLower().Contains(searchText.ToLower()))
                                                                                                     || (x.InventoryItemContainerDTO.ProductBarcodeContainerDTOList != null &&
                                                                                                        x.InventoryItemContainerDTO.ProductBarcodeContainerDTOList.Exists(y => y.BarCode == searchText))).ToList();

            log.LogVariableState("filteredProductsContainerDTO", filteredProductsContainerDTO);

            if (filteredProductsContainerDTO != null && filteredProductsContainerDTO.Any())
            {
                if (filteredProductsContainerDTO.Count > 1)
                {
                    List<ProductViewDTO> productViewDTOList = new List<ProductViewDTO>();
                    foreach (ProductsContainerDTO productsContainerDTO in filteredProductsContainerDTO)
                    {
                        log.LogVariableState("productsContainerDTO", productsContainerDTO);
                        string barcode = string.Empty;
                        if (productsContainerDTO.InventoryItemContainerDTO.ProductBarcodeContainerDTOList != null && productsContainerDTO.InventoryItemContainerDTO.ProductBarcodeContainerDTOList.Any())
                        {
                            barcode = productsContainerDTO.InventoryItemContainerDTO.ProductBarcodeContainerDTOList.FirstOrDefault().BarCode;
                            log.LogVariableState("barcode : ", barcode);
                        }

                        // If product is of type lot controlled then do not show up to the User
                        if (productsContainerDTO.InventoryItemContainerDTO.LotControlled)
                        {
                            log.Debug(" LotControlled: " + productsContainerDTO.InventoryItemContainerDTO.LotControlled);
                            if (productsContainerDTO.InventoryItemContainerDTO.ExpiryType == "E")
                            {
                                log.Error("Product expiry type is Date. Cannot receive product &1.");
                                continue;
                            }
                        }
                        if (productsContainerDTO.InventoryItemContainerDTO.MarketListItem)
                        {
                            log.Error("Product MarketList Item type . Cannot receive product &1.");
                            continue;
                        }
                        ProductViewDTO productViewDTO = new ProductViewDTO(productsContainerDTO.InventoryItemContainerDTO.ProductId,
                                                                             productsContainerDTO.InventoryItemContainerDTO.Code,
                                                                             productsContainerDTO.InventoryItemContainerDTO.Description,
                                                                             barcode);
                        log.LogVariableState("productViewDTO", productViewDTO);
                        productViewDTOList.Add(productViewDTO);
                    }

                    log.LogVariableState("productViewDTOList", productViewDTOList);

                    ProductView productView = new ProductView();
                    ProductVM productVM = new ProductVM(ExecutionContext, productViewDTOList);
                    productView.DataContext = productVM;
                    productView.ShowDialog();

                    if (productVM.FormDialogResult == DialogResult.OK)
                    {
                        selectedProductIdList = productVM.SelectedProductViewDTO.Select(x => x.ProductId).ToList();
                        log.Debug("Selected Product Id List  : " + selectedProductIdList);
                    }
                }
                else
                {
                    if (filteredProductsContainerDTO.FirstOrDefault().InventoryItemContainerDTO.LotControlled)
                    {
                        log.Debug(" LotControlled: " + filteredProductsContainerDTO.FirstOrDefault().InventoryItemContainerDTO.LotControlled);
                        if (filteredProductsContainerDTO.FirstOrDefault().InventoryItemContainerDTO.ExpiryType == "E")
                        {
                            log.Error("Product expiry type is Date. Cannot receive product &1.");
                            string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 3077, filteredProductsContainerDTO.FirstOrDefault().InventoryItemContainerDTO.Code);
                            SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
                            return selectedProductIdList;
                        }
                    }
                    if (filteredProductsContainerDTO.FirstOrDefault().InventoryItemContainerDTO.MarketListItem)
                    {
                        log.Error("Product MarketList Item type . Cannot receive product &1.");
                        string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, " Cannot receive product " + filteredProductsContainerDTO.FirstOrDefault().InventoryItemContainerDTO.Code + " Market List item");
                        SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
                        return selectedProductIdList;
                    }
                    int selectedProductId = filteredProductsContainerDTO.FirstOrDefault().InventoryItemContainerDTO.ProductId;
                    selectedProductIdList.Add(selectedProductId);
                    productDefaultLocationId = filteredProductsContainerDTO.FirstOrDefault().InventoryItemContainerDTO.DefaultLocationId;
                    log.LogVariableState("selectedProductIdList  : ", selectedProductIdList);
                }
            }
            else
            {
                string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 111);
                SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
                log.Debug(ErrorMessage);
            }

            log.LogMethodExit(selectedProductIdList);
            return selectedProductIdList;
        }

        private void BuildReceiveLines(List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> SearchParameters)
        {
            log.LogMethodEntry(SearchParameters);
            using (NoSynchronizationContextScope.Enter())
            {
                Task<List<PurchaseOrderDTO>> task = GetPurchaseOrders(SearchParameters);
                task.Wait();
                List<PurchaseOrderDTO> taskResult = task.Result;
                log.LogVariableState("taskResult - ", taskResult);
                if (taskResult != null && taskResult.Any())
                {
                    PurchaseOrderView purchaseOrderView = new PurchaseOrderView();
                    PurchaseOrderVM purchaseOrderVM = new PurchaseOrderVM(ExecutionContext, taskResult);
                    purchaseOrderView.DataContext = purchaseOrderVM;
                    purchaseOrderView.ShowDialog();
                    if (purchaseOrderVM.FormDialogResult == DialogResult.OK)
                    {
                        List<ReceiveStockViewDTO> result = new List<ReceiveStockViewDTO>();
                        PurchaseOrderDTOList = purchaseOrderVM.SelectedPurchaseOrderDTOList;
                        log.Debug("Selected PurchaseOrderDTO  : " + PurchaseOrderDTOList);
                        foreach (PurchaseOrderDTO PurchaseOrderDTO in PurchaseOrderDTOList)
                        {
                            result.AddRange(BuildReceiveStockLines(PurchaseOrderDTO));
                            log.LogVariableState("result : ", result);
                        }
                        if (result != null && result.Any())
                        {
                            ReceiveStockViewDTOList = new ObservableCollection<ReceiveStockViewDTO>(result);
                            GridSource = new ObservableCollection<ReceiveStockViewDTO>(ReceiveStockViewDTOList);
                            if (CustomDataGridVM.CollectionToBeRendered.Count > 0)
                            {
                                CustomDataGridVM.SelectedItem = CustomDataGridVM.CollectionToBeRendered[0];
                            }
                        }
                    }
                }
                else
                {
                    string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 3075); // "No Open/InProgress Purchase Order record in Inventory"
                    SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
                    log.Debug("ErrorMessage  : " + ErrorMessage);

                }
            }
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
            receiveStockView = param as ReceiveStockView;
            try
            {
                if (receiveStockView != null)
                {
                    receiveStockView.Close();
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.Message;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// RemoveButtonClick
        /// </summary>
        /// <param name="param"></param>
        private void RemoveButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            receiveStockView = param as ReceiveStockView;
            try
            {
                if (receiveStockView != null)
                {
                    ReceiveStockViewDTO receiveStockViewDTO = (ReceiveStockViewDTO)customDataGridVM.ButtonClickedModel.Item;
                    if (ReceiveStockViewDTOList != null && ReceiveStockViewDTOList.Any())
                    {
                        POReceiveRemarkView pOReceiveRemarkView = new POReceiveRemarkView();
                        POReceiveRemarksVM POReceiveRemarksVM = new POReceiveRemarksVM(ExecutionContext, "Remarks");
                        pOReceiveRemarkView.DataContext = POReceiveRemarksVM;
                        pOReceiveRemarkView.ShowDialog();
                        receiveStockViewDTO.Remarks = POReceiveRemarksVM.Remarks;
                    }
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// SelectionChanged
        /// </summary>
        /// <param name="param"></param>
        private void SelectionChanged(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            receiveStockView = param as ReceiveStockView;
            try
            {
                if (receiveStockView != null)
                {
                    receiveStockView.UpdateLayout();
                    receiveStockView.ReceiveStockView_ContentRendered(null, null);
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// ClearSearchButtonClick
        /// </summary>
        /// <param name="param"></param>
        private void ClearSearchButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            receiveStockView = param as ReceiveStockView;
            try
            {
                SelectedGRNValue = string.Empty;
                SelectedPONumber = string.Empty;
                SelectedProductCode = string.Empty;
                selectedVendorDTO = VendorList.First();
                ReceiveStockViewDTOList = new ObservableCollection<ReceiveStockViewDTO>();
                GridSource = new ObservableCollection<ReceiveStockViewDTO>(ReceiveStockViewDTOList);
                CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(GridSource);
                SetFooterContent("", MessageType.None);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// ClearButtonClick
        /// </summary>
        /// <param name="param"></param>
        private void ClearButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            receiveStockView = param as ReceiveStockView;
            try
            {
                // Loop through the DTO list and clear the quantity and remarks fields
                List<ReceiveStockViewDTO> modifiedItems = new List<ReceiveStockViewDTO>();
                foreach (object obj in customDataGridVM.CollectionToBeRendered)
                {
                    ReceiveStockViewDTO receiveStockViewDTO = (ReceiveStockViewDTO)obj;
                    receiveStockViewDTO.Quantity = 0;
                    receiveStockViewDTO.Remarks = string.Empty;
                    receiveStockViewDTO.AcceptChanges();
                }
                GridSource = new ObservableCollection<ReceiveStockViewDTO>(ReceiveStockViewDTOList);
                CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(GridSource);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// This method refreshed the receive UI
        ///  If the PO line is received completely then line will be removed 
        ///  If partial received then PO Balnace quntity will be updated
        /// </summary>
        /// <param name="purchaseOrderDTO"></param>
        private void RefreshReceiveStockView(PurchaseOrderDTO purchaseOrderDTO)
        {
            log.LogMethodEntry(purchaseOrderDTO);
            try
            {

                List<ReceiveStockViewDTO> refreshedReceiveStockViewDTOList = BuildReceiveStockLines(purchaseOrderDTO);
                foreach (ReceiveStockViewDTO ReceiveStockViewDTO in refreshedReceiveStockViewDTOList)
                {
                    ReceiveStockViewDTO receiveStockViewDTO = ReceiveStockViewDTOList.Where(x => x.PurchaseOrderLineId == ReceiveStockViewDTO.PurchaseOrderLineId).FirstOrDefault();

                    // If the receive line exists incase of partial receive then update the with POBalanceQuantity refreshed DTO
                    if (receiveStockViewDTO != null)
                    {
                        receiveStockViewDTO.POBalanceQuantity = ReceiveStockViewDTO.POBalanceQuantity;
                        receiveStockViewDTO.Quantity = 0;
                        receiveStockViewDTO.Remarks = string.Empty;
                        ReceiveStockViewDTO.AcceptChanges();
                    }
                }
                // To remove the receive lines which are received fully - Balance is zero
                // The collection of the grid still has the list of receive lines which needs to refreshed 
                // Partial lines are updated with new pobalance  
                foreach (object obj in customDataGridVM.CollectionToBeRendered)
                {
                    ReceiveStockViewDTO modifiedReceiveStockViewDTO = (ReceiveStockViewDTO)obj;
                    if (modifiedReceiveStockViewDTO.Quantity > 0)
                    {
                        foreach (PurchaseOrderLineDTO lineDTO in purchaseOrderDTO.PurchaseOrderLineListDTO)
                        {
                            PurchaseOrderLineDTO purchaseOrderLineDTO = purchaseOrderDTO.PurchaseOrderLineListDTO.Where(x => x.PurchaseOrderLineId == modifiedReceiveStockViewDTO.PurchaseOrderLineId).FirstOrDefault();
                            if (purchaseOrderLineDTO != null)
                            {
                                ReceiveStockViewDTOList.Remove(modifiedReceiveStockViewDTO);
                            }
                        }
                    }
                }
                GridSource = new ObservableCollection<ReceiveStockViewDTO>(ReceiveStockViewDTOList);
                CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(GridSource);
                if (CustomDataGridVM.CollectionToBeRendered.Count > 0)
                {
                    CustomDataGridVM.SelectedItem = CustomDataGridVM.CollectionToBeRendered[0];
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
            log.LogMethodExit();
        }

        private void CloseButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            receiveStockView = param as ReceiveStockView;
            try
            {
                if (receiveStockView != null)
                {
                    receiveStockView.Close();
                }
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
            log.LogMethodExit();
        }
        private void ReceiptButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            IsLoadingVisible = true;
            SetFooterContent("", MessageType.None);
            receiveStockView = param as ReceiveStockView;
            try
            {
                if (receiveStockView != null)
                {
                    ReceiptView receiptView = new ReceiptView();
                    ReceiptVM receiptVM = new ReceiptVM(ExecutionContext);
                    receiptView.DataContext = receiptVM;
                    receiptView.ShowDialog();
                }
                IsLoadingVisible = false;
            }
            catch (UnauthorizedException ex)
            {
                IsLoadingVisible = false;
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
            catch (ValidationException ex)
            {
                IsLoadingVisible = false;
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
            log.LogMethodExit();
        }
        private void ReceiveButtonClick(object param)
        {
            try
            {
                log.LogMethodEntry(param);
                IsLoadingVisible = true;
                String ErrorMessage = String.Empty;
                List<ReceiveStockViewDTO> modifiedItems = new List<ReceiveStockViewDTO>();
                ExecuteAction(async () =>
                {
                    List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> taxSearchParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                    taxSearchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                    ITaxUseCases taxUseCases = InventoryUseCaseFactory.GetTaxUseCases(ExecutionContext);
                    List<TaxDTO> taxDTOList = await taxUseCases.GetTaxes(taxSearchParameters, false, false, null);
                    log.LogVariableState("taxDTOList", taxDTOList);

                    // Get the modified lines with quantity greater than 0
                    List<ReceiveStockViewDTO> modifiedList = GetModifiedReceiveLines();
                    if (modifiedList != null && modifiedList.Any() == false)
                    {
                        ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "Nothing to receive ");
                        //ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Information", null), MessageContainerList.GetMessage(ExecutionContext, "Try Again!", null), ErrorMessage);
                        SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
                        return;
                    }
                    // Get the PO numbers of modified lines with quantity greater than 0
                    List<string> PONumberList = modifiedList.Select(x => x.PONumber).ToList();

                    // Get the Distinct PO numbers of modified lines with quantity greater than 0
                    List<string> distinctPONumberList = PONumberList.Distinct().ToList();
                    if (string.IsNullOrWhiteSpace(SelectedGRNValue))
                    {
                        ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 851);
                        //ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Validation"), MessageContainerList.GetMessage(ExecutionContext, 3076), ErrorMessage);
                        SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
                        log.Debug(ErrorMessage);
                        return;
                    }
                    // Iterate through the PO numbers of modified lines with quantity greater than 0
                    foreach (string PONumber in distinctPONumberList)
                    {
                        //Create receive lines list 
                        List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList = new List<InventoryReceiveLinesDTO>();
                        // Get the receive lines for the selected PO
                        List<ReceiveStockViewDTO> modifiedReceiveStockViewDTOList = modifiedList.Where(x => x.PONumber == PONumber).ToList();

                        // Create variables to capture the recepts details
                        string vendorName = string.Empty;
                        string remarks = string.Empty;
                        string orderNumber = string.Empty;
                        decimal totalPOReceiveAmount = 0;

                        // Get PO details 
                        PurchaseOrderDTO purchaseOrderDTO = PurchaseOrderDTOList.Where(po => po.OrderNumber == PONumber).FirstOrDefault();
                        int detailPurchaseOrderId = purchaseOrderDTO.PurchaseOrderId;
                        log.LogVariableState("purchaseOrderDTO", purchaseOrderDTO);

                        foreach (ReceiveStockViewDTO modifiedReceiveStockViewDTO in modifiedReceiveStockViewDTOList)
                        {

                            if (modifiedReceiveStockViewDTO.Quantity > modifiedReceiveStockViewDTO.POBalanceQuantity)
                            {
                                ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, 1019) + " :" + modifiedReceiveStockViewDTO.ProductCode;
                                //ShowMessagePopup(MessageContainerList.GetMessage(ExecutionContext, "Validation"), MessageContainerList.GetMessage(ExecutionContext, 3076), ErrorMessage);
                                SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
                                log.Debug(ErrorMessage);
                                return;
                            }

                            PurchaseOrderLineDTO purchaseOrderLineDTO = purchaseOrderDTO.PurchaseOrderLineListDTO.Where(x => x.ItemCode == modifiedReceiveStockViewDTO.ProductCode).FirstOrDefault();
                            log.LogVariableState("purchaseOrderLineDTO ", purchaseOrderLineDTO);


                            vendorName = vendorDTOList.Where(ven => ven.VendorId == purchaseOrderDTO.VendorId).FirstOrDefault().Name;
                            List<InventoryLotDTO> inventoryLotDTOList = new List<InventoryLotDTO>();
                            ProductsContainerDTO productsContainerDTO = ProductViewContainerList.GetActiveProductsContainerDTOList(ExecutionContext, ManualProductType.SELLABLE.ToString()).Where(x => x.InventoryItemContainerDTO != null && x.InventoryItemContainerDTO.Code == modifiedReceiveStockViewDTO.ProductCode).FirstOrDefault();

                            decimal poUnitPrice = Convert.ToDecimal(purchaseOrderLineDTO.UnitPrice);
                            log.Debug("purchaseOrderLineDTO.UnitPrice : " + purchaseOrderLineDTO.UnitPrice);
                            log.Debug("PurchaseTaxId: " + purchaseOrderLineDTO.PurchaseTaxId);

                            double taxPercentage = 0;
                            if (purchaseOrderLineDTO.PurchaseTaxId > -1)
                            {
                                if (taxDTOList != null && taxDTOList.Any())
                                {
                                    taxPercentage = taxDTOList.Where(tax => tax.TaxId == purchaseOrderLineDTO.PurchaseTaxId).FirstOrDefault().TaxPercentage;
                                    log.Debug("taxPercentage: " + taxPercentage);
                                }
                                if (productsContainerDTO.InventoryItemContainerDTO.TaxInclusiveCost.ToString() == "N")
                                {
                                    log.Debug("TaxInclusiveCost: N");
                                    poUnitPrice = poUnitPrice + Convert.ToDecimal(purchaseOrderLineDTO.TaxAmount);
                                    log.Debug("poUnitPrice after adding tax amount: " + poUnitPrice);
                                }
                            }
                            log.Debug("taxAmount : " + purchaseOrderLineDTO.TaxAmount);
                            log.Debug("uomId: " + purchaseOrderLineDTO.UOMId);
                            decimal receiveQuantity = modifiedReceiveStockViewDTO.Quantity;
                            receiveQuantity = Math.Round(Convert.ToDecimal(receiveQuantity), 2, MidpointRounding.AwayFromZero);
                            log.Debug("receiveQty: " + receiveQuantity);
                            decimal poReceiveAmount = (poUnitPrice * receiveQuantity);
                            poReceiveAmount = Math.Round(Convert.ToDecimal(poReceiveAmount), 5, MidpointRounding.AwayFromZero);
                            log.Debug(" Receiveamount: " + poReceiveAmount);

                            // Total receving amount 
                            totalPOReceiveAmount += poReceiveAmount;
                            totalPOReceiveAmount = Math.Round(Convert.ToDecimal(totalPOReceiveAmount), 5, MidpointRounding.AwayFromZero);

                            //Create and Save the lot details
                            if (productsContainerDTO.InventoryItemContainerDTO.LotControlled)
                            {
                                log.Debug(" LotControlled: " + productsContainerDTO.InventoryItemContainerDTO.LotControlled);
                                InventoryLotDTO inventoryLotDTO = new InventoryLotDTO();
                                if (productsContainerDTO.InventoryItemContainerDTO.ExpiryType == "D")
                                {
                                    inventoryLotDTO.Expirydate = DateTime.Today.Date.AddDays(productsContainerDTO.InventoryItemContainerDTO.ExpiryInDays);
                                }
                                inventoryLotDTO.PurchaseOrderReceiveLineId = -1;
                                inventoryLotDTO.ReceivePrice = Convert.ToDouble(poUnitPrice);
                                inventoryLotDTO.OriginalQuantity = 0;
                                inventoryLotDTO.BalanceQuantity = Convert.ToDouble(receiveQuantity);
                                inventoryLotDTO.LotId = -1;
                                inventoryLotDTO.UOMId = purchaseOrderLineDTO.UOMId;
                                log.LogVariableState("inventoryLotDTO", inventoryLotDTO);
                                inventoryLotDTOList.Add(inventoryLotDTO);
                            }
                            InventoryReceiveLinesDTO inventoryReceiveLinesDTO = new InventoryReceiveLinesDTO(-1, detailPurchaseOrderId, purchaseOrderLineDTO.ProductId,
                                                                                     purchaseOrderLineDTO.Description, "", Convert.ToDouble(receiveQuantity), productDefaultLocationId, "Y",
                                                                                     modifiedReceiveStockViewDTO.PurchaseOrderLineId, Convert.ToDouble(poUnitPrice), taxPercentage, Convert.ToDouble(poReceiveAmount),
                                                                                     productsContainerDTO.InventoryItemContainerDTO.TaxInclusiveCost, -1, SelectedGRNValue, ServerDateTime.Now,
                                                                                     string.Empty, ExecutionContext.GetUserId(), purchaseOrderLineDTO.RequisitionId, purchaseOrderLineDTO.RequisitionLineId, modifiedReceiveStockViewDTO.ProductCode,
                                                                                     purchaseOrderLineDTO.Quantity, purchaseOrderLineDTO.UnitPrice, purchaseOrderLineDTO.TaxAmount, purchaseOrderLineDTO.SubTotal,
                                                                                     0, inventoryLotDTOList.Any() ? inventoryLotDTOList : null, purchaseOrderLineDTO.PriceInTickets, purchaseOrderLineDTO.PurchaseTaxId, Convert.ToDecimal(purchaseOrderLineDTO.TaxAmount),
                                                                                     purchaseOrderLineDTO.UOMId, true, "", modifiedReceiveStockViewDTO.Remarks);

                            log.LogVariableState("inventoryReceiveLinesDTO", inventoryReceiveLinesDTO);
                            inventoryReceiveLinesDTOList.Add(inventoryReceiveLinesDTO);
                        }

                        //Create InventoryReceiptDTO  for all the receiving line for this PO
                        InventoryReceiptDTO inventoryReceiptDTO = new InventoryReceiptDTO(-1, SelectedGRNValue, string.Empty, "", detailPurchaseOrderId,
                                                                                           remarks, ServerDateTime.Now, ExecutionContext.GetUserId(),
                                                                                           "", productDefaultLocationId, purchaseOrderDTO.DocumentTypeID, vendorName,
                                                                                           purchaseOrderDTO.OrderNumber, Convert.ToDouble(totalPOReceiveAmount), purchaseOrderDTO.OrderDate,
                                                                                           0, inventoryReceiveLinesDTOList, true);

                        log.LogVariableState("inventoryReceiptDTO", inventoryReceiptDTO);
                        if (purchaseOrderDTO.InventoryReceiptListDTO == null)
                        {
                            purchaseOrderDTO.InventoryReceiptListDTO = new List<InventoryReceiptDTO>();
                        }
                        try
                        {
                            purchaseOrderDTO.InventoryReceiptListDTO.Add(inventoryReceiptDTO);
                            // Saving the PO  and get the refreshed PO details 
                            IReceiptUseCases receiptUseCases = InventoryUseCaseFactory.GetReceiptsUseCases(ExecutionContext);
                            purchaseOrderDTO = await receiptUseCases.ReceivePurchaseOrder(purchaseOrderDTO.PurchaseOrderId, new List<InventoryReceiptDTO> { inventoryReceiptDTO });
                            log.LogVariableState("purchaseOrderDTO : ", purchaseOrderDTO);
                            RefreshReceiveStockView(purchaseOrderDTO);
                            IsLoadingVisible = false;
                            ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "Received");
                            SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Info);
                        }
                        catch (ValidationException ex)
                        {
                            log.Error(ex);
                            IsLoadingVisible = false;
                            ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, ex.Message);
                            SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
                        }
                    }
                });
                
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, ex.Message);
                SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "Save failed ");
                SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
        }

        private List<ReceiveStockViewDTO> GetModifiedReceiveLines()
        {
            log.LogMethodEntry();
            List<ReceiveStockViewDTO> modifiedList = new List<ReceiveStockViewDTO>();

            foreach (object obj in customDataGridVM.CollectionToBeRendered)
            {
                ReceiveStockViewDTO modifiedReceiveStockViewDTO = (ReceiveStockViewDTO)obj;
                if (modifiedReceiveStockViewDTO.IsChanged && modifiedReceiveStockViewDTO.Quantity > 0)
                {
                    modifiedList.Add(modifiedReceiveStockViewDTO);
                }
            }
            log.LogMethodExit(modifiedList);
            return modifiedList;
        }

        private decimal GetReceivedLineQuantity(PurchaseOrderDTO purchaseOrderDTO, int purchaseOrderLineId)
        {
            log.LogMethodEntry(purchaseOrderDTO, purchaseOrderLineId);
            decimal quantityAlreadyReceived = 0;
            if (purchaseOrderDTO.InventoryReceiptListDTO != null && purchaseOrderDTO.InventoryReceiptListDTO.Any())
            {
                log.LogVariableState("purchaseOrderDTO.InventoryReceiptListDTO", purchaseOrderDTO.InventoryReceiptListDTO);
                foreach (InventoryReceiptDTO inventoryReceiptDTO in purchaseOrderDTO.InventoryReceiptListDTO)
                {
                    quantityAlreadyReceived += Convert.ToDecimal(inventoryReceiptDTO.InventoryReceiveLinesListDTO.Where(x => x.PurchaseOrderLineId == purchaseOrderLineId && x.PurchaseOrderId == purchaseOrderDTO.PurchaseOrderId && x.IsReceived == "Y").Sum(line => line.Quantity));
                    log.Debug("quantityAlreadyReceived : " + quantityAlreadyReceived);
                }
            }
            log.LogMethodExit(quantityAlreadyReceived);
            return quantityAlreadyReceived;
        }

        private void ShowMessagePopup(string heading, string subHeading, string content)
        {
            log.LogMethodEntry(heading, subHeading, content);
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            messagePopupView.Owner = receiveStockView;
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
            if (messagePopupVM != null && messagePopupVM.Heading != null &&
                messagePopupVM.Heading.ToLower() == MessageContainerList.GetMessage(ExecutionContext, "Success").ToLower())
            {
                StatusCode = StatusCode.Success;
            }
            log.LogMethodExit();
        }

        private void CloseAddWindow(string message)
        {
            SuccessMessage = message;
            if (receiveStockView != null)
            {
                receiveStockView.Close();
            }
        }
        #endregion Methods
    }
}

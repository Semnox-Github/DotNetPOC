/********************************************************************************************
 * Project Name - Parafait_POS.Waivers
 * Description  - frmMapWaiversToTransaction
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.80.0      26-Sep-2019   Guru S A                Created for Waiver phase 2 enhancement changes  
 *2.120.1     31-May-2021   Nitin Pai               Validate license on customer waiver mapping
 *2.140.0     14-Sep-2021   Guru S A                Waiver mapping UI enhancements
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.Product;
using Semnox.Parafait.Device.Peripherals;
using Semnox.Parafait.Device;

namespace Parafait_POS.Waivers
{
    public partial class frmMapWaiversToTransaction : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool incorrectCustomerSetupForWaiver;
        private string setupErrorMsg = string.Empty;
        private Utilities utilities;
        private Semnox.Core.Utilities.ExecutionContext executionContext;
        private Transaction transaction;
        private List<Transaction.TransactionLine> transactionLines;
        private List<CustomerDTO> selectedCustomerDTOList;
        private List<CustomerDTO> searchedCustomerDTOList;
        private List<int> selectedCustomerIdList;
        private List<WaiversDTO> trxWaiversDTOList;
        private List<Transaction.TransactionLine> selectedWaiverTrxLines;
        private int guestCustomerId = -1;
        private AutoResetEvent resetBGWCustomer = new AutoResetEvent(false);
        private AutoResetEvent resetBGWTrxPanel = new AutoResetEvent(false);
        private VirtualKeyboardController virtualKeyboard;
        private bool isReservationTransaction = false;
        private const string COLLAPSEPANEL = "C";
        private const string EXPANDPANEL = "E";
        private AutoResetEvent resetBGWCustomerSearch = new AutoResetEvent(false);
        private DeviceClass barcodeScanner = null;
        private int maxDisplayRecordCount = 50;
        private int countOfLatestWaiverSignedCustomers = 10;
        private string MAPWAIVER = string.Empty;
        private string SIGNWAIVER = string.Empty;
        private string CUSTSEARCH = string.Empty;
        private string TAPCARD = string.Empty;
        private string WAIVERCODE = string.Empty;
        private string NEWCUSTOMER = string.Empty;
        private string CUSTOMERLOOKUP = string.Empty;
        private string OVERRIDEWAIVERS = string.Empty;
        private string RESETOVERRIDEWAIVERS = string.Empty;
        private const string EXACT_SEARCH = "E";
        private const string LIKE_SEARCH = "L";
        private const string DOEXACTSEARCHKEYWORD = "EXACT_SEARCH";
        /// <summary>
        /// device
        /// </summary>
        internal class Device
        {
            internal string DeviceName;
            internal string DeviceType;
            internal string DeviceSubType;
            internal string VID, PID, OptString;
        }
        // private CustomerDTO guestCustomerDTO;
        // private List<Control> customerControlList = new List<Control>();
        //private List<Control> trxProductControlList = new List<Control>();
        //private DateTime startTime, endTime;
        public frmMapWaiversToTransaction(Utilities utilities, Transaction transaction, List<Transaction.TransactionLine> transactionLines = null)
        {
            log.LogMethodEntry(transaction);
            POSUtils.SetLastActivityDateTime();
            InitializeComponent();
            this.selectedCustomerIdList = new List<int>();
            this.utilities = utilities;
            this.executionContext = utilities.ExecutionContext;
            SetCueForSearchTextBoxes();
            this.transaction = transaction;
            if (transactionLines != null && transactionLines.Any())
            {
                this.transactionLines = transactionLines;
            }
            else
            {
                this.transactionLines = this.transaction.TrxLines;

            }
            this.guestCustomerId = CustomerListBL.GetGuestCustomerId(executionContext);

            virtualKeyboard = new VirtualKeyboardController();
            //virtualKeyboard.Initialize(this, new List<Control>() { btnShowKeyPad, btnPkgTabShowKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"), new List<Control>() { txtSearchGuestCount, pnlCustomerDetail, txtServiceChargeAmount, txtServiceChargePercentage });
            virtualKeyboard.Initialize(this, new List<Control>() { btnShowKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            isReservationTransaction = transaction.IsReservationTransaction(null);
            BuildChannelList();
            //panelHeight = this.fPnlProductWaiverMap.Height - 30;
            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));
            RegisterBarcodeScanner();
            GetMaxRecordDisplayCount();
            countOfLatestWaiverSignedCustomers = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "COUNT_OF_LATEST_WAIVER__SIGNED_CUSTOMERS", 10);
            SetLiterals();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }     

        private void ValidateWaiverSetup()
        {
            log.LogMethodEntry();
            try
            { 
                incorrectCustomerSetupForWaiver = true;
                WaiverCustomerUtils.HasValidWaiverSetup(executionContext);
                incorrectCustomerSetupForWaiver = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                setupErrorMsg = ex.Message;
                //POSUtils.ParafaitMessageBox(ex.Message);
                DisplayMessage(ex.Message, MAPWAIVER, true);
                incorrectCustomerSetupForWaiver = true;
            }
            log.LogMethodExit();
        }

        private void frmMapWaiversToTransaction_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                ValidateWaiverSetup();
                SetOverridePendingButtonText();
                ShowHideCustomerButtons();
                this.Cursor = Cursors.WaitCursor;
                LoadCustomerToPanels();
                InitialLoadProductWaiverMappingPanel();
                //DoDefaultAssignment(); 
                TriggerDoDefault();
                POSUtils.SetLastActivityDateTime();
                ActiveControl = chkSelectAll;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //POSUtils.ParafaitMessageBox(ex.Message);
                DisplayMessage(ex.Message, MAPWAIVER, true);
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void TriggerDoDefault()
        {
            log.LogMethodEntry();
            //if (bgwDoDefaultMap.IsBusy == false)
            //{
            //    bgwDoDefaultMap.WorkerReportsProgress = true;
            //    bgwDoDefaultMap.RunWorkerAsync();
            //}
            log.LogMethodExit();
        }

        private void LoadCustomerToPanels()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            selectedCustomerDTOList = new List<CustomerDTO>();
            trxWaiversDTOList = new List<WaiversDTO>();
            if (this.transaction != null)
            {
                AddTrxAttendees();
                AddTrxCustomer();
                trxWaiversDTOList = transaction.GetWaiversDTOList();
                AddGuestCustomer();
                AddMappedCustomers();
                if ((selectedCustomerIdList != null && selectedCustomerIdList.Any())
                    || ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "WAIVER_CODE_IS_MANDATORY_TO_FETCH_CUSTOMER", false))
                {
                    CollapseExpandPanels(btnExpandCollapseSelected);
                }
                else
                {
                    CollapseExpandPanels(btnExpandCollapseSearch);
                }
                InitialRefreshCustomerPanel();
                LoadLatestWaiverSignedCustomers();
            }
            log.LogMethodExit();
        }
        private void AddTrxAttendees()
        {
            log.LogMethodEntry();
            if (this.transaction.HasActiveAttendeeList())
            {
                List<int> tempIdList = transaction.GetAttendeeCustomerIds();
                if (tempIdList != null && tempIdList.Any())
                {
                    for (int i = 0; i < tempIdList.Count; i++)
                    {
                        AddToSelectedCustomerIdList(tempIdList[i]);
                    }
                }
            }
            log.LogMethodExit();
        }
        private CustomerDTO AddTrxCustomer()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            CustomerDTO trxCustomerDTO = null;
            CustomerDTO trxPrimaryCardCustomerDTO = null;
            if (this.transaction.customerDTO != null)
            {
                trxCustomerDTO = this.transaction.customerDTO;
            }
            if (this.transaction.PrimaryCard != null && this.transaction.PrimaryCard.customerDTO != null)
            {
                trxPrimaryCardCustomerDTO = this.transaction.PrimaryCard.customerDTO;
            }
            try
            {
                if (trxCustomerDTO != null && trxPrimaryCardCustomerDTO != null && trxPrimaryCardCustomerDTO == trxCustomerDTO && string.IsNullOrEmpty(trxCustomerDTO.FirstName) == false)
                {
                    transaction.SaveCustomer(null);
                    trxCustomerDTO = this.transaction.customerDTO;
                    trxPrimaryCardCustomerDTO = this.transaction.PrimaryCard.customerDTO;
                }
                else
                {
                    if (trxCustomerDTO != null && string.IsNullOrEmpty(trxCustomerDTO.FirstName) == false && trxCustomerDTO.Id == -1)
                    {
                        transaction.SaveCustomer(null);
                        trxCustomerDTO = this.transaction.customerDTO;
                    }
                    if (trxPrimaryCardCustomerDTO != null && string.IsNullOrEmpty(trxPrimaryCardCustomerDTO.FirstName) == false && trxPrimaryCardCustomerDTO.Id == -1)
                    {
                        transaction.SavePrimaryCardCustomer(null);
                        trxPrimaryCardCustomerDTO = this.transaction.PrimaryCard.customerDTO;
                    }
                }
            }
            catch (Exception ex)
            {
                //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 255, ex.Message));
                DisplayMessage(MessageContainerList.GetMessage(executionContext, 255, ex.Message), MAPWAIVER);
                log.Error(ex);
            }
            List<int> trxCustomerIdList = new List<int>();
            if (trxCustomerDTO != null && trxCustomerDTO.Id > -1)
            {
                AddToSelectedCustomerIdList(trxCustomerDTO.Id);
                trxCustomerIdList.Add(trxCustomerDTO.Id);
                AddRelatedCustomerIds(trxCustomerDTO.Id);
            }
            if (trxPrimaryCardCustomerDTO != null && trxPrimaryCardCustomerDTO.Id > -1)
            {
                AddToSelectedCustomerIdList(trxPrimaryCardCustomerDTO.Id);
                if (trxCustomerIdList.Exists(id => id == trxPrimaryCardCustomerDTO.Id) == false)
                {
                    trxCustomerIdList.Add(trxCustomerDTO.Id);
                }
                AddRelatedCustomerIds(trxPrimaryCardCustomerDTO.Id);
            }
            AddTrxLineCustomerIds(trxCustomerIdList);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodEntry();
            return trxCustomerDTO;
        }

        private void AddTrxLineCustomerIds(List<int> trxCustomerIdList)
        {
            log.LogMethodEntry(trxCustomerIdList);
            if (transaction != null && transaction.TrxLines != null && transaction.TrxLines.Any())
            {
                List<Transaction.TransactionLine> linesWithCards = transaction.TrxLines.Where(tl => tl.LineValid && tl.card != null
                                                                                                   && tl.card.customer_id > -1
                                                                                                   && trxCustomerIdList.Exists(tci => tci == tl.card.customer_id) == false).ToList();
                if (linesWithCards != null && linesWithCards.Any())
                {
                    List<int> cardCustomerIdList = linesWithCards.Where(tl => tl.LineValid && tl.card != null && tl.card.customer_id > -1).Select(tl => tl.card.customer_id).ToList();
                    for (int i = 0; i < cardCustomerIdList.Count; i++)
                    {
                        AddToSelectedCustomerIdList(cardCustomerIdList[i]);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void AddMappedCustomers()
        {
            log.LogMethodEntry();
            List<int> custIdList = transaction.GetMappedCustomerIdListForWaiver();
            if (custIdList != null && custIdList.Any())
            {
                for (int i = 0; i < custIdList.Count; i++)
                {
                    AddToSelectedCustomerIdList(custIdList[i]);
                }
            }
            log.LogMethodExit();
        }
        private void AddRelatedCustomerIds(int customerId)
        {
            log.LogMethodEntry(customerId);
            if (customerId > -1)
            {
                List<CustomerRelationshipDTO> tempRelDTOList = GetCustomerRelations(customerId);
                if (tempRelDTOList != null && tempRelDTOList.Any())
                {
                    List<int> customerIdList = tempRelDTOList.Where(rCust => rCust.IsActive == true && rCust.RelatedCustomerId > -1).Select(rCust => rCust.RelatedCustomerId).Distinct().ToList();
                    List<int> customerIdList2 = tempRelDTOList.Where(rCust => rCust.IsActive == true && rCust.CustomerId > -1).Select(rCust => rCust.CustomerId).Distinct().ToList();
                    if (customerIdList != null && customerIdList.Any())
                    {
                        for (int i = 0; i < customerIdList.Count; i++)
                        {
                            AddToSelectedCustomerIdList(customerIdList[i]);
                        }
                    }
                    if (customerIdList2 != null && customerIdList2.Any())
                    {
                        for (int i = 0; i < customerIdList2.Count; i++)
                        {
                            AddToSelectedCustomerIdList(customerIdList2[i]);
                        }
                    }
                }
            }
            log.LogMethodEntry();
        }
        private List<int> GetRelatedCustomerIds(int customerId)
        {
            log.LogMethodEntry(customerId);
            List<int> customerIdList = new List<int>();
            if (customerId > -1)
            {
                List<CustomerRelationshipDTO> tempRelDTOList = GetCustomerRelations(customerId);
                if (tempRelDTOList != null && tempRelDTOList.Any())
                {
                    customerIdList = tempRelDTOList.Where(rCust => rCust.IsActive == true && rCust.RelatedCustomerId > -1).Select(rCust => rCust.RelatedCustomerId).Distinct().ToList();
                    List<int> customerIdList2 = tempRelDTOList.Where(rCust => rCust.IsActive == true && rCust.CustomerId > -1).Select(rCust => rCust.CustomerId).Distinct().ToList();
                    if (customerIdList2 != null && customerIdList2.Any())
                    {
                        customerIdList.AddRange(customerIdList2);
                    }
                }
            }
            log.LogMethodEntry(customerIdList);
            return customerIdList;
        }
        private void AddToSelectedCustomerIdList(int customerId)
        {
            log.LogMethodEntry(customerId);
            if (selectedCustomerIdList.Exists(custId => custId == customerId) == false)
            {
                selectedCustomerIdList.Add(customerId);
            }
            log.LogMethodExit();
        }
        private List<CustomerRelationshipDTO> GetCustomerRelations(int customerId)
        {
            log.LogMethodEntry(customerId);
            POSUtils.SetLastActivityDateTime();
            CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(executionContext);
            List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchParm = new List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>>();
            searchParm.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
            searchParm.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParm.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<CustomerRelationshipDTO> tempRelDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(searchParm);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodEntry(tempRelDTOList);
            return tempRelDTOList;
        }
        private void AddGuestCustomer()
        {
            log.LogMethodEntry();
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SIGN_WAIVER_WITHOUT_CUSTOMER_REGISTRATION", false) == true)
            {
                if (guestCustomerId > -1)
                {
                    //CustomerBL guestCustomerBL = new CustomerBL(executionContext, guestCustomerId); 
                    //guestCustomerDTO = guestCustomerBL.CustomerDTO;
                    AddToSelectedCustomerIdList(guestCustomerId);
                }
            }
            log.LogMethodExit();
        }
        private usrCtlCustomer CreateUsrCtlCustomerElement(CustomerDTO selectedCustDTO, List<WaiversDTO> trxWaiversDTOList, int recordCount)
        {
            log.LogMethodEntry(selectedCustDTO, trxWaiversDTOList, recordCount);
            usrCtlCustomer usrCtlCustomer = new usrCtlCustomer(executionContext, selectedCustDTO, trxWaiversDTOList, this.transaction, guestCustomerId);
            usrCtlCustomer.signWaiverDelegate += new usrCtlCustomer.SignWaiverDelegate(SignWaiver);
            log.LogMethodExit();
            return usrCtlCustomer;
        }
        private void RefreshCustomerPanel()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            fpnlCustomerSelection.Controls.Clear();
            //customerControlList = new List<Control>();
            int recordCount = 0;
            foreach (CustomerDTO selectedCustDTO in selectedCustomerDTOList)
            {
                usrCtlCustomer usrCtlCustomer = CreateUsrCtlCustomerElement(selectedCustDTO, trxWaiversDTOList, recordCount);
                // customerControlList.Add(usrCtlCustomer);
                recordCount++;
                fpnlCustomerSelection.Controls.Add(usrCtlCustomer);
            }
            vScrollBarCustomerSelection.UpdateButtonStatus();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void InitialRefreshCustomerPanel()
        {
            log.LogMethodEntry();
            //this.Cursor = Cursors.WaitCursor;
            if (bgwCustomerPanel.IsBusy == false)
            {
                bgwCustomerPanel.WorkerReportsProgress = true;
                bgwCustomerPanel.RunWorkerAsync();
            }
            log.LogMethodExit();
        }
        private void RefreshSelectedCustomerUsrCtlCustomer(CustomerDTO updatedCustomerDTO)
        {
            log.LogMethodEntry(updatedCustomerDTO);
            List<usrCtlCustomer> customerUsrControlList = fpnlCustomerSelection.Controls.OfType<usrCtlCustomer>().ToList();
            if (customerUsrControlList != null)
            {
                foreach (usrCtlCustomer item in customerUsrControlList)
                {
                    if (item.CustomerDTO != null && item.CustomerDTO.Id == updatedCustomerDTO.Id)
                    {
                        item.CustomerDTO = updatedCustomerDTO;
                        item.SetDisplayElements();
                        break;
                    }
                }
                vScrollBarCustomerSelection.UpdateButtonStatus();
            }
            log.LogMethodExit();
        }
        private void ResetUsrCtlCustomer(int customerId)
        {
            log.LogMethodEntry(customerId);
            List<usrCtlCustomer> customerUsrControlList = fpnlCustomerSelection.Controls.OfType<usrCtlCustomer>().ToList();
            if (customerUsrControlList != null)
            {
                foreach (usrCtlCustomer item in customerUsrControlList)
                {
                    if (item.CustomerDTO != null && item.CustomerDTO.Id == customerId)
                    {
                        item.SetDisplayElements();
                        item.IsSelected = false;
                        break;
                    }
                }
                vScrollBarCustomerSelection.UpdateButtonStatus();
            }
            log.LogMethodExit();
        }
        private void AddCustomerToSelectionPanel(CustomerDTO customerDTO)
        {
            log.LogMethodEntry();
            if (this.selectedCustomerDTOList == null)
            {
                this.selectedCustomerDTOList = new List<CustomerDTO>();
            }
            if (this.selectedCustomerDTOList.Exists(sCust => sCust.Id == customerDTO.Id) == false)
            {
                this.selectedCustomerDTOList.Add(customerDTO);
                usrCtlCustomer usrCtlCustomer = CreateUsrCtlCustomerElement(customerDTO, trxWaiversDTOList, this.selectedCustomerDTOList.Count - 1);
                fpnlCustomerSelection.Controls.Add(usrCtlCustomer);
                vScrollBarCustomerSelection.UpdateButtonStatus();
            }
            log.LogMethodExit();
        }
        private void SignWaiver(CustomerDTO customerDTO)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (incorrectCustomerSetupForWaiver)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2316));
                }
                if (IsBackgroundJobRunning() == false)
                {
                    if (customerDTO != null)
                    {
                        List<int> waiverSetIdList = new List<int>();
                        string waiverSetIdListString = string.Empty;
                        CustomerBL customerBL = new CustomerBL(executionContext, customerDTO);
                        //if (customerDTO.Id != guestCustomerId)
                        //{
                        //    foreach (WaiversDTO waivers in trxWaiversDTOList)
                        //    {
                        //        //DateTime trxDatevalue = GetTrxDate();
                        //        //if (customerBL.HasSigned(waivers, trxDatevalue) == false)
                        //        //{
                        //        if (waiverSetIdList.Exists(ws => ws == waivers.WaiverSetId) == false)
                        //        {
                        //            waiverSetIdList.Add(waivers.WaiverSetId);
                        //        }
                        //        //}
                        //    }
                        //}
                        if (customerDTO.Id == guestCustomerId && guestCustomerId > -1)
                        {
                            if (this.transaction.IsWaiverSignaturePending() == false)
                            {
                                //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2383));
                                DisplayMessage(MessageContainerList.GetMessage(executionContext, 2383), MAPWAIVER);
                                log.LogMethodExit("Transaction lines are mapped. Cannot proceed with guest customer");
                                return;
                            }
                        }

                        foreach (WaiversDTO trxWaivers in trxWaiversDTOList)
                        {
                            if (waiverSetIdList.Exists(ws => ws == trxWaivers.WaiverSetId) == false)
                            {
                                waiverSetIdList.Add(trxWaivers.WaiverSetId);
                            }
                        }

                        //WaiverSetListBL waiverSetListBL = new WaiverSetListBL(executionContext);
                        //List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> searchParam = new List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>>();
                        //searchParam.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        //searchParam.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.WAIVER_SET_ID_LIST, waiverSetIdListString));

                        WaiverSetContainer waiverSetContainer = null;
                        try
                        {
                            waiverSetContainer = WaiverSetContainer.GetInstance;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2435));
                            //Unexpected error while getting waiver file details. Please check the setup
                        }
                        List<WaiverSetDTO> waiverSetDTOList = waiverSetContainer.GetWaiverSetDTOList(executionContext.GetSiteId());
                        List<WaiverSetDTO> selectedWaiverSetDTOList = new List<WaiverSetDTO>();
                        if (waiverSetIdList != null && waiverSetDTOList != null)
                        {
                            for (int i = 0; i < waiverSetIdList.Count; i++)
                            {
                                List<WaiverSetDTO> selectedWaiverSetDTO = waiverSetDTOList.Where(ws => ws.WaiverSetId == waiverSetIdList[i]).ToList();
                                if (selectedWaiverSetDTO != null)
                                {
                                    selectedWaiverSetDTOList.AddRange(new List<WaiverSetDTO>(selectedWaiverSetDTO));
                                }
                            }
                        }
                        POSUtils.SetLastActivityDateTime();

                        using (frmCustomerWaiverUI frmCustomerWaiverUI = new frmCustomerWaiverUI(utilities, customerDTO, selectedWaiverSetDTOList, POSUtils.ParafaitMessageBox, this.transaction))
                        {
                            if (frmCustomerWaiverUI.Width > Application.OpenForms["POS"].Width + 28)
                            {
                                frmCustomerWaiverUI.Width = Application.OpenForms["POS"].Width - 30;
                            }
                            frmCustomerWaiverUI.ShowDialog();
                        }
                        POSUtils.SetLastActivityDateTime();
                        //if (customerDTO.Id != guestCustomerId)
                        //{
                        //    customerDTO.CustomerSignedWaiverDTOList = customerBL.GetCustomerWaiverList();
                        //}
                        RefreshUIForSelectedCustomers(customerDTO);
                        RefreshUIForSearchCustomer(customerDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //POSUtils.ParafaitMessageBox(ex.Message);
                DisplayMessage(ex.Message, SIGNWAIVER);
            }
            log.LogMethodExit();
        }
        private DateTime GetTrxDate(int lineId)
        {
            log.LogMethodEntry(lineId);
            DateTime lineScheduleDate = DateTime.MinValue;
            if (lineId > -1 && this.transaction != null && this.transaction.TrxLines != null && this.transaction.TrxLines.Count > lineId)
            {
                if (this.transaction.TrxLines[lineId].LineAtb != null && this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO != null)
                {
                    lineScheduleDate = this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO.ScheduleFromDate;
                }
            }
            log.LogVariableState("lineScheduleDate", lineScheduleDate);
            log.LogVariableState("this.transaction.TrxDate", this.transaction.TrxDate);
            DateTime trxDatevalue = (lineScheduleDate == DateTime.MinValue ? ((this.transaction.TrxDate == DateTime.MinValue ?
                                     (this.transaction.TransactionDate == DateTime.MinValue ? utilities.getServerTime() : this.transaction.TransactionDate)
                                     : this.transaction.TrxDate)) : lineScheduleDate);
            log.LogMethodExit(trxDatevalue);
            return trxDatevalue;
        }
        private void RefreshUIForSelectedCustomers(CustomerDTO customerDTO)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (selectedCustomerDTOList != null && selectedCustomerDTOList.Any() && selectedCustomerDTOList.Exists(cDTO => cDTO.Id == customerDTO.Id))
                {
                    List<int> customerIdList = new List<int>();
                    customerIdList.Add(customerDTO.Id);
                    //RefreshUsrCtlCustomer(customerDTO.Id);
                    //RefreshProductWaiverMapping(customerDTO);
                    List<CustomerRelationshipDTO> customerRelationshipDTOList = GetCustomerRelations(customerDTO.Id);
                    if (customerRelationshipDTOList != null && customerRelationshipDTOList.Any())
                    {
                        List<int> relateCustomerIdList = customerRelationshipDTOList.Where(rCust => rCust.IsActive == true && rCust.RelatedCustomerId > -1).Select(rCust => rCust.RelatedCustomerId).Distinct().ToList();
                        if (relateCustomerIdList != null && relateCustomerIdList.Any())
                        {
                            customerIdList.AddRange(relateCustomerIdList);
                        }
                    }
                    POSUtils.SetLastActivityDateTime();
                    CustomerListBL customerListBL = new CustomerListBL(executionContext);
                    List<CustomerDTO> custDTOList = customerListBL.GetCustomerDTOList(customerIdList, true, true, true);
                    POSUtils.SetLastActivityDateTime();
                    if (custDTOList != null && custDTOList.Any())
                    {
                        for (int i = 0; i < custDTOList.Count; i++)
                        {
                            CustomerDTO relatedCustDTO = selectedCustomerDTOList.Find(cust => cust.Id == custDTOList[i].Id);
                            if (relatedCustDTO != null)
                            {
                                int lineIndex = selectedCustomerDTOList.IndexOf(relatedCustDTO);
                                if (lineIndex > -1)
                                {
                                    selectedCustomerDTOList[lineIndex] = custDTOList[i];
                                    RefreshSelectedCustomerUsrCtlCustomer(custDTOList[i]);
                                    RefreshProductWaiverMapping(selectedCustomerDTOList[lineIndex]);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                vScrollBarCustomerSelection.UpdateButtonStatus();
                vScrollBarProductCustomerMap.UpdateButtonStatus();
                hScrollBarProductCustomerMap.UpdateButtonStatus();
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }
        private void InitialLoadProductWaiverMappingPanel()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            if (bgwTransactionPanel.IsBusy == false)
            {
                fPnlProductWaiverMap.Controls.Clear();
                bgwTransactionPanel.WorkerReportsProgress = true;
                bgwTransactionPanel.RunWorkerAsync();
            }
            log.LogMethodExit();
        }
        private void LoadProductWaiverMappingPanel()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            selectedWaiverTrxLines = null;
            fPnlProductWaiverMap.Controls.Clear();
            //trxProductControlList = new List<Control>();
            //panelHeight = fPnlProductWaiverMap.Height - 30;
            if (this.transaction != null)
            {
                List<Transaction.TransactionLine> waiverTrxLines = this.transaction.GetWaiverTransactionLines();
                if (waiverTrxLines != null && waiverTrxLines.Any())
                {
                    selectedWaiverTrxLines = waiverTrxLines.Where(tl => this.transactionLines.Exists(tll => tll == tl)).ToList();
                }
                if (selectedWaiverTrxLines != null && selectedWaiverTrxLines.Any())
                {
                    List<int> productIdList = selectedWaiverTrxLines.Select(tl => tl.ProductID).Distinct().ToList();
                    if (productIdList != null && productIdList.Any())
                    {
                        //startTime = DateTime.Now;
                        int xLocation = 1;// fPnlProductWaiverMap.Location.X;
                        for (int i = 0; i < productIdList.Count; i++)
                        {
                            List<AttractionBookingDTO> atbDTOList = selectedWaiverTrxLines.Where(tl => tl.ProductID == productIdList[i] && tl.LineAtb != null
                                                                                           && tl.LineAtb.AttractionBookingDTO != null).Select(tl => tl.LineAtb.AttractionBookingDTO).ToList();
                            if (atbDTOList != null && atbDTOList.Any())
                            {
                                atbDTOList = atbDTOList.GroupBy(atb => new { atb.AttractionScheduleId, atb.ScheduleFromDate }).Select(atb => atb.FirstOrDefault()).ToList();
                            }
                            else
                            {
                                atbDTOList.Add(new AttractionBookingDTO());
                            }
                            for (int j = 0; j < atbDTOList.Count; j++)
                            {
                                FlowLayoutPanel pnlProductPanel = LoadProductWaiverDetails(xLocation, productIdList[i], atbDTOList[j]);
                                if (pnlProductPanel != null)
                                {
                                    pnlProductPanel.BorderStyle = BorderStyle.FixedSingle;
                                    //if (pnlProductPanel.Height < panelHeight -30)
                                    //{
                                    //    pnlProductPanel.Height = panelHeight - 30;
                                    //}
                                    //else
                                    //{
                                    //    if (pnlProductPanel.Height > panelHeight)
                                    //    {
                                    //        panelHeight = pnlProductPanel.Height;
                                    //    }
                                    //}
                                    //trxProductControlList.Add(pnlProductPanel);
                                    fPnlProductWaiverMap.Controls.Add(pnlProductPanel);
                                    xLocation = xLocation + pnlProductPanel.Width + 2;
                                }
                            }
                        }
                        AdjustPanelHeight();
                    }
                }
                vScrollBarProductCustomerMap.UpdateButtonStatus();
                hScrollBarProductCustomerMap.UpdateButtonStatus();
            }
            log.LogMethodExit();
        }
        private FlowLayoutPanel LoadProductWaiverDetails(int xLocation, int productId, AttractionBookingDTO atbDTO)
        {
            log.LogMethodEntry(productId, atbDTO);
            POSUtils.SetLastActivityDateTime();
            List<Transaction.TransactionLine> productTrxLines = null;
            if (atbDTO.AttractionScheduleId == -1)
            {
                productTrxLines = selectedWaiverTrxLines.Where(tl => tl.ProductID == productId).ToList();
            }
            else
            {
                productTrxLines = selectedWaiverTrxLines.Where(tl => tl.ProductID == productId
                                                                     && tl.LineAtb != null && tl.LineAtb.AttractionBookingDTO != null
                                                                     && tl.LineAtb.AttractionBookingDTO.AttractionScheduleId == atbDTO.AttractionScheduleId
                                                                     && tl.LineAtb.AttractionBookingDTO.ScheduleFromDate == atbDTO.ScheduleFromDate).ToList();
            }
            FlowLayoutPanel pnlProductPanel = null;
            if (productTrxLines != null)
            {
                pnlProductPanel = new FlowLayoutPanel();
                pnlProductPanel.Tag = new Tuple<int, AttractionBookingDTO>(productId, atbDTO);
                pnlProductPanel.Font = lblSample.Font;
                pnlProductPanel.Width = 252;
                pnlProductPanel.Margin = new Padding(1);
                pnlProductPanel.Location = new Point(xLocation, 0);
                pnlProductPanel.Click += new EventHandler(pnlProductPanel_Click);
                Label lblProductName = new Label
                {
                    Margin = new Padding(1),
                    Width = 250,
                    Font = new Font(lblSample.Font, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = System.Drawing.SystemColors.GradientActiveCaption,
                    Name = "ProductName" + productId.ToString(),
                    Tag = new Tuple<int, AttractionBookingDTO>(productId, atbDTO),
                    AutoEllipsis = true
                };
                lblProductName.Click += new EventHandler(pnlProductPanelLabel_Click);
                pnlProductPanel.Controls.Add(lblProductName);
                int mappedCount = 0;
                //int yCoordinates = lblProductName.Height + 1;
                List<Control> panelList = new List<Control>();
                foreach (Transaction.TransactionLine trxLineEntry in productTrxLines)
                {
                    if (bgwTransactionPanel.IsBusy)
                    {
                        bgwTransactionPanel.ReportProgress(1);
                    }
                    int lineId = this.transaction.TrxLines.IndexOf(trxLineEntry);
                    Panel pnlTrxLine = BuildLineWaiverMapDisplayElement(lineId);
                    if (pnlTrxLine != null)
                    {
                        panelList.Add(pnlTrxLine);
                        // yCoordinates = yCoordinates + pnlTrxLine.Height + 1;
                        mappedCount++;
                    }
                }
                if (panelList.Any())
                {
                    pnlProductPanel.Controls.AddRange(panelList.ToArray());
                }
                string panelTitle = this.transaction.TrxLines.Find(tl => tl.LineValid == true && tl.ProductID == productId).ProductName
                                      + " (" + mappedCount.ToString() + "/" + productTrxLines.Count().ToString() + ") ";
                if (atbDTO.AttractionScheduleId > -1)
                {
                    string atbInfo = ": " + this.transaction.TrxLines.Find(tl => tl.LineValid == true && tl.ProductID == productId && tl.LineAtb != null
                                                     && tl.LineAtb.AttractionBookingDTO != null
                                                     && tl.LineAtb.AttractionBookingDTO.AttractionScheduleId == atbDTO.AttractionScheduleId
                                                     && tl.LineAtb.AttractionBookingDTO.ScheduleFromDate == atbDTO.ScheduleFromDate).LineAtb.AttractionBookingDTO.ScheduleFromDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                    //atbInfo = " : " + this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO.AttractionScheduleName
                    //               + " - " + this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO.ScheduleFromDate.ToString(POSStatic.ParafaitEnv.DATETIME_FORMAT);
                    if (string.IsNullOrEmpty(atbInfo) == false)
                    {
                        panelTitle = panelTitle + atbInfo;
                    }
                }
                lblProductName.Text = panelTitle;
                lblProductName.Height = 40;
                new ToolTip().SetToolTip(lblProductName, lblProductName.Text);
            }
            log.LogMethodExit(pnlProductPanel);
            return pnlProductPanel;
        }
        private void pnlProductPanel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    FlowLayoutPanel pnlProductPanel = (FlowLayoutPanel)sender;
                    int productId = -1;
                    AttractionBookingDTO atbDTO = new AttractionBookingDTO();
                    if (pnlProductPanel.Tag != null)
                    {
                        Tuple<int, AttractionBookingDTO> prodScheduleIds = (Tuple<int, AttractionBookingDTO>)pnlProductPanel.Tag;
                        productId = prodScheduleIds.Item1;
                        atbDTO = prodScheduleIds.Item2;
                        // int.TryParse(pnlProductPanel.Tag.ToString(), out productId);
                        MapCustomerWaiverToProdcutTrxLine(productId, atbDTO, pnlProductPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessage(ex.Message, MAPWAIVER);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void pnlProductPanelLabel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    Label pnlProductPanelLabel = (Label)sender;
                    int productId = -1;
                    AttractionBookingDTO atbDTO = new AttractionBookingDTO();
                    if (pnlProductPanelLabel.Tag != null)
                    {
                        Tuple<int, AttractionBookingDTO> prodScheduleIds = (Tuple<int, AttractionBookingDTO>)pnlProductPanelLabel.Tag;
                        productId = prodScheduleIds.Item1;
                        atbDTO = prodScheduleIds.Item2;
                        //int.TryParse(pnlProductPanelLabel.Tag.ToString(), out productId);
                        FlowLayoutPanel pnlProduct = (FlowLayoutPanel)pnlProductPanelLabel.Parent;
                        MapCustomerWaiverToProdcutTrxLine(productId, atbDTO, pnlProduct);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessage(ex.Message, MAPWAIVER);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void pnlTrxPanelLabel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    Label pnlProductPanelLabel = (Label)sender;
                    int productId = -1;
                    AttractionBookingDTO atbDTO = new AttractionBookingDTO();
                    if (pnlProductPanelLabel.Tag != null)
                    {
                        Tuple<int, AttractionBookingDTO> prodScheduleIds = (Tuple<int, AttractionBookingDTO>)pnlProductPanelLabel.Tag;
                        productId = prodScheduleIds.Item1;
                        atbDTO = prodScheduleIds.Item2;
                        //int.TryParse(pnlProductPanelLabel.Tag.ToString(), out productId);
                        Panel pnlParent = (Panel)pnlProductPanelLabel.Parent;
                        if (pnlParent != null)
                        {
                            FlowLayoutPanel pnlProduct = (FlowLayoutPanel)pnlParent.Parent;
                            MapCustomerWaiverToProdcutTrxLine(productId, atbDTO, pnlProduct);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessage(ex.Message, MAPWAIVER);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void MapCustomerWaiverToProdcutTrxLine(int productId, AttractionBookingDTO atbDTO, FlowLayoutPanel pnlProduct, bool removealreadyMapped = false)
        {
            log.LogMethodEntry(productId, atbDTO, removealreadyMapped);
            if (productId > -1 && pnlProduct != null)
            {
                MapDToFromSelectedCustomerPanel(productId, atbDTO, pnlProduct, removealreadyMapped);
                MapDToFromSearchCustomerPanel(productId, atbDTO, pnlProduct, removealreadyMapped);
                vScrollBarProductCustomerMap.UpdateButtonStatus();
                hScrollBarProductCustomerMap.UpdateButtonStatus();
            }
            log.LogMethodExit();
        }
        private void MapDToFromSelectedCustomerPanel(int productId, AttractionBookingDTO atbDTO, FlowLayoutPanel pnlProduct, bool removealreadyMapped)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<CustomerDTO> userSelectedCustomerDTOList = GetSelectedCustomerDTOList();
            if (removealreadyMapped)
            {
                RemoveAlreadyMappedCustomerDTO(pnlProduct, userSelectedCustomerDTOList);
            }
            if (userSelectedCustomerDTOList != null && userSelectedCustomerDTOList.Any())
            {
                List<int> selectedCustId = userSelectedCustomerDTOList.Select(cust => cust.Id).ToList();
                MapSelectedCustomerToProductLine(productId, atbDTO, pnlProduct, userSelectedCustomerDTOList);
                AdjustPanelHeight();
                if (this.chkSelectAll.Checked)
                {
                    this.chkSelectAll.Checked = false;
                }
                else
                {
                    for (int i = 0; i < selectedCustId.Count; i++)
                    {
                        ResetUsrCtlCustomer(selectedCustId[i]);
                    }
                }
            }
            log.LogMethodExit();
        }
        private void MapDToFromSearchCustomerPanel(int productId, AttractionBookingDTO atbDTO, FlowLayoutPanel pnlProduct, bool removealreadyMapped)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<CustomerDTO> selectedCustomerDTOListFromSearchPanel = GetSelectedCustomerDTOListFromSearchPanel();
            if (removealreadyMapped)
            {
                RemoveAlreadyMappedCustomerDTO(pnlProduct, selectedCustomerDTOListFromSearchPanel);
            }
            if (selectedCustomerDTOListFromSearchPanel != null && selectedCustomerDTOListFromSearchPanel.Any())
            {
                List<int> selectedCustId = selectedCustomerDTOListFromSearchPanel.Select(cust => cust.Id).ToList();
                MapSelectedCustomerToProductLine(productId, atbDTO, pnlProduct, selectedCustomerDTOListFromSearchPanel);
                AdjustPanelHeight();
                for (int i = 0; i < selectedCustId.Count; i++)
                {
                    ResetSearchPanelUsrCtlCustomer(selectedCustId[i]);
                }
            }
        }
        private void AdjustPanelHeight()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                int heightAdjuster = 20;
                int panelHeight = fPnlProductWaiverMap.Height - heightAdjuster;
                List<FlowLayoutPanel> productPanelList = fPnlProductWaiverMap.Controls.OfType<FlowLayoutPanel>().ToList();
                if (productPanelList != null && productPanelList.Any())
                {
                    for (int i = 0; i < productPanelList.Count; i++)
                    {
                        int yCoordinates = GetYCoordinate(productPanelList[i]) + 1;
                        if (yCoordinates > panelHeight)
                        {
                            panelHeight = yCoordinates;
                        }
                    }
                    if (panelHeight < fpnlCustomerSelection.Height - heightAdjuster)
                    {
                        panelHeight = fpnlCustomerSelection.Height - heightAdjuster;
                    }
                    for (int i = 0; i < productPanelList.Count; i++)
                    {
                        productPanelList[i].Height = panelHeight;
                    }
                }
                vScrollBarProductCustomerMap.UpdateButtonStatus();
                hScrollBarProductCustomerMap.UpdateButtonStatus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private List<CustomerDTO> GetSelectedCustomerDTOList()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<CustomerDTO> userSelectedCustomerDTOList = new List<CustomerDTO>();
            foreach (Control panelControl in fpnlCustomerSelection.Controls)
            {
                if (panelControl is usrCtlCustomer)
                {
                    usrCtlCustomer usrCtlCustomer = (usrCtlCustomer)panelControl;
                    if (usrCtlCustomer.IsSelected)
                    {
                        userSelectedCustomerDTOList.Add(usrCtlCustomer.CustomerDTO);
                    }
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(userSelectedCustomerDTOList);
            return userSelectedCustomerDTOList;
        }
        private List<CustomerDTO> GetSelectedCustomerDTOListFromSearchPanel()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<CustomerDTO> userSelectedCustomerDTOList = new List<CustomerDTO>();
            foreach (Control panelControl in fpnlCustomerSearch.Controls)
            {
                if (panelControl is usrCtlCustomer)
                {
                    usrCtlCustomer usrCtlCustomer = (usrCtlCustomer)panelControl;
                    if (usrCtlCustomer.IsSelected)
                    {
                        userSelectedCustomerDTOList.Add(usrCtlCustomer.CustomerDTO);
                    }
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(userSelectedCustomerDTOList);
            return userSelectedCustomerDTOList;
        }
        private void RemoveAlreadyMappedCustomerDTO(FlowLayoutPanel pnlProductPanel, List<CustomerDTO> selectedCustomerDTOList)
        {
            log.LogMethodEntry(pnlProductPanel, selectedCustomerDTOList);
            POSUtils.SetLastActivityDateTime();
            if (selectedCustomerDTOList != null && selectedCustomerDTOList.Any())
            {
                foreach (Control panelControl in pnlProductPanel.Controls)
                {
                    if (panelControl is Panel)
                    {
                        Panel trxPanel = (Panel)panelControl;

                        if (trxPanel.Tag != null)
                        {
                            KeyValuePair<int, int> lineIdCustId = (KeyValuePair<int, int>)trxPanel.Tag;
                            foreach (CustomerDTO item in selectedCustomerDTOList)
                            {
                                if (lineIdCustId.Value == item.Id)
                                {
                                    ResetUsrCtlCustomer(item.Id);
                                    selectedCustomerDTOList.Remove(item);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void MapSelectedCustomerToProductLine(int productId, AttractionBookingDTO atbDTO, FlowLayoutPanel pnlProductPanel, List<CustomerDTO> selectedCustomerDTOList)
        {
            log.LogMethodEntry(productId, atbDTO, selectedCustomerDTOList);
            POSUtils.SetLastActivityDateTime();
            bool allowApplyingSameCustomer = false;
            bool receivedUserInout = false;
            bool signWithOutCustomerRegistration = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SIGN_WAIVER_WITHOUT_CUSTOMER_REGISTRATION");
            if (selectedCustomerDTOList != null && selectedCustomerDTOList.Any())
            {
                List<Transaction.TransactionLine> productTrxLines = null;
                if (atbDTO.AttractionScheduleId == -1)
                {
                    productTrxLines = selectedWaiverTrxLines.Where(tl => tl.ProductID == productId).ToList();
                }
                else
                {
                    productTrxLines = selectedWaiverTrxLines.Where(tl => tl.ProductID == productId && tl.LineAtb != null
                                                                         && tl.LineAtb.AttractionBookingDTO != null
                                                                         && tl.LineAtb.AttractionBookingDTO.AttractionScheduleId == atbDTO.AttractionScheduleId
                                                                         && tl.LineAtb.AttractionBookingDTO.ScheduleFromDate == atbDTO.ScheduleFromDate).ToList();
                }
                // Entry - 
                //Begin - Modification on 10 - Dec - 2020 for validate License Type in Waiver
                if (isReservationTransaction == false)
                {
                    string licenseRequiredType = productTrxLines.FirstOrDefault().LicenseType;
                    if (!String.IsNullOrEmpty(licenseRequiredType))
                    {
                        if (!ValidateLicenseTypeInWaiver(productTrxLines.FirstOrDefault(), selectedCustomerDTOList))
                        {
                            //POSUtils.ParafaitMessageBox("Customer or Card does not have a valid license.", "Message");
                            log.Error("Customer or Card does not have a valid license.");
                            return;
                        }
                    }
                }
                //End- Modification on 10 - Dec - 2020 for validate License Type in Waiver                

                RemoveMappedTrxLines(pnlProductPanel, productTrxLines);
                //int yCoordinates = GetYCoordinate(pnlProductPanel) + 1;
                int rows = 0;
                foreach (Transaction.TransactionLine trxLineEntry in productTrxLines)
                {

                    int lineId = this.transaction.TrxLines.IndexOf(trxLineEntry);
                    if (selectedCustomerDTOList != null && selectedCustomerDTOList.Any())
                    {
                        CustomerDTO customerDTO = selectedCustomerDTOList[0];
                        if (customerDTO.Id != guestCustomerId)
                        {
                            this.transaction.MapCustomerWaiversToLine(lineId, customerDTO);
                            Panel pnlTrxLine = BuildLineWaiverMapDisplayElement(lineId, customerDTO);
                            if (pnlTrxLine != null)
                            {
                                pnlProductPanel.Controls.Add(pnlTrxLine);
                                // yCoordinates = yCoordinates + pnlTrxLine.Height + 1;
                            }
                            if (signWithOutCustomerRegistration && allowApplyingSameCustomer == false && receivedUserInout == false && rows + 1 < productTrxLines.Count)
                            {
                                receivedUserInout = true;
                                //Do you want to map same customer to other lines as well?
                                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2381),
                                                                MessageContainerList.GetMessage(executionContext, "Map Waiver"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    allowApplyingSameCustomer = true;
                                }
                            }
                            if (allowApplyingSameCustomer == false)
                            {
                                selectedCustomerDTOList.Remove(customerDTO);
                            }
                        }
                        else
                        {
                            //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2382),
                            //                                    MessageContainerList.GetMessage(executionContext, "Map Waiver"));
                            DisplayMessage(MessageContainerList.GetMessage(executionContext, 2382), MAPWAIVER);
                            selectedCustomerDTOList.Remove(customerDTO);
                        }
                    }
                    else
                    {
                        break;
                    }
                    rows++;
                }
                UpdateTrxProductHeaderInfo(productId, atbDTO, pnlProductPanel);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private bool ValidateLicenseTypeInWaiver(Transaction.TransactionLine trxLineEntry, List<CustomerDTO> selectedCustomerDTOList)// return bool and calling function should take action
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            bool validLicenseType = false;
            if (trxLineEntry != null)
            {
                DateTime entitlementConsumptionDate = transaction.EntitlementReferenceDate;
                if (trxLineEntry.LineAtb != null)
                {
                    entitlementConsumptionDate = trxLineEntry.LineAtb.AttractionBookingDTO.ScheduleFromDate;
                }

                if (!String.IsNullOrEmpty(trxLineEntry.LicenseType))
                {
                    string requiredLicenseType = CreditPlusTypeConverter.ToString(trxLineEntry.LicenseType);
                    if (selectedCustomerDTOList != null && selectedCustomerDTOList.Any())
                    {
                        foreach (CustomerDTO customerDTO in selectedCustomerDTOList)
                        {
                            String licenseCheck = this.transaction.CheckLicenseForCustomerAndCard(null, trxLineEntry.LicenseType, entitlementConsumptionDate, customerDTO.Id, null);
                            if (!String.IsNullOrEmpty(licenseCheck))
                            {
                                //POSUtils.ParafaitMessageBox("Customer " + customerDTO.FirstName + " does not have a valid license.", "Message");
                                DisplayMessage(MessageContainerList.GetMessage(executionContext, 4090, customerDTO.FirstName),
                                              MessageContainerList.GetMessage(executionContext, "Map Waiver"));
                                //'Customer &1 does not have a valid license.'
                                return false;
                            }
                        }
                        validLicenseType = true;
                    }
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
            return validLicenseType;
        }

        private void UpdateTrxProductHeaderInfo(int productId, AttractionBookingDTO atbDTO, FlowLayoutPanel pnlProductPanel)
        {
            log.LogMethodEntry(productId, atbDTO);
            POSUtils.SetLastActivityDateTime();
            int mappedCount = GetMappedCustomerCount(pnlProductPanel);
            List<Transaction.TransactionLine> productTrxLines = null;
            if (atbDTO.AttractionScheduleId == -1)
            {
                productTrxLines = selectedWaiverTrxLines.Where(tl => tl.ProductID == productId).ToList();
            }
            else
            {
                productTrxLines = selectedWaiverTrxLines.Where(tl => tl.ProductID == productId
                                                                     && tl.LineAtb != null && tl.LineAtb.AttractionBookingDTO != null
                                                                     && tl.LineAtb.AttractionBookingDTO.AttractionScheduleId == atbDTO.AttractionScheduleId
                                                                     && tl.LineAtb.AttractionBookingDTO.ScheduleFromDate == atbDTO.ScheduleFromDate).ToList();
            }
            var lblObject = pnlProductPanel.Controls.Find("ProductName" + productId.ToString(), true);
            if (lblObject != null)
            {
                Label lblProductName = (Label)lblObject[0];

                string panelTitle = this.transaction.TrxLines.Find(tl => tl.LineValid == true && tl.ProductID == productId).ProductName
                                      + " (" + mappedCount.ToString() + "/" + productTrxLines.Count().ToString() + ") ";
                if (atbDTO.AttractionScheduleId > -1)
                {
                    string atbInfo = ": " + this.transaction.TrxLines.Find(tl => tl.LineValid == true && tl.ProductID == productId && tl.LineAtb != null
                                                     && tl.LineAtb.AttractionBookingDTO != null
                                                     && tl.LineAtb.AttractionBookingDTO.AttractionScheduleId == atbDTO.AttractionScheduleId
                                                     && tl.LineAtb.AttractionBookingDTO.ScheduleFromDate == atbDTO.ScheduleFromDate).LineAtb.AttractionBookingDTO.ScheduleFromDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                    //atbInfo = " : " + this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO.AttractionScheduleName
                    //               + " - " + this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO.ScheduleFromDate.ToString(POSStatic.ParafaitEnv.DATETIME_FORMAT);
                    if (string.IsNullOrEmpty(atbInfo) == false)
                    {
                        panelTitle = panelTitle + atbInfo;
                    }
                }
                lblProductName.Text = panelTitle;
                lblProductName.Height = 40;
                new ToolTip().SetToolTip(lblProductName, lblProductName.Text);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private int GetMappedCustomerCount(FlowLayoutPanel pnlProductPanel)
        {
            log.LogMethodEntry(pnlProductPanel);
            int mappedCount = 0;
            foreach (Control panelControl in pnlProductPanel.Controls)
            {
                if (panelControl is Panel)
                {
                    mappedCount++;
                }
            }
            log.LogMethodExit(mappedCount);
            return mappedCount;
        }
        private int GetYCoordinate(FlowLayoutPanel pnlProductPanel)
        {
            log.LogMethodEntry(pnlProductPanel);
            int yCoordinate = 0;
            Panel lastPanel = null;
            int i = 0;
            foreach (Control panelControl in pnlProductPanel.Controls)
            {
                if (i == 0 && panelControl is Label)
                {
                    yCoordinate = panelControl.Location.Y + panelControl.Height;
                }
                if (panelControl is Panel)
                {
                    lastPanel = (Panel)panelControl;
                }
            }
            if (lastPanel != null)
            {
                yCoordinate = lastPanel.Location.Y + lastPanel.Height;
            }
            log.LogMethodExit(yCoordinate);
            return yCoordinate;
        }
        private void RemoveMappedTrxLines(FlowLayoutPanel pnlProductPanel, List<Transaction.TransactionLine> productTrxLines)
        {
            log.LogMethodEntry(pnlProductPanel, productTrxLines);
            POSUtils.SetLastActivityDateTime();
            if (productTrxLines != null && productTrxLines.Any())
            {
                foreach (Control panelControl in pnlProductPanel.Controls)
                {
                    if (panelControl is Panel)
                    {
                        Panel trxPanel = (Panel)panelControl;

                        if (trxPanel.Tag != null)
                        {
                            KeyValuePair<int, int> lineIdCustId = (KeyValuePair<int, int>)trxPanel.Tag;
                            foreach (Transaction.TransactionLine trxLineEntry in productTrxLines)
                            {
                                int lineId = this.transaction.TrxLines.IndexOf(trxLineEntry);
                                if (lineIdCustId.Key == lineId)
                                {
                                    //pnlProductPanel.Controls.Remove(trxPanel);
                                    productTrxLines.Remove(trxLineEntry);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private Panel BuildLineWaiverMapDisplayElement(int lineId, CustomerDTO selectedCustomerDTO = null)
        {
            log.LogMethodEntry(lineId, selectedCustomerDTO);
            Panel pnlTrxLine = null;
            if (lineId > -1)
            {
                CustomerDTO mappedCustomerDTO = this.transaction.GetMappedCustomerForWaiver(lineId);
                bool alreadyMapped = false;
                bool alreadySigned = false;
                if (mappedCustomerDTO != null)
                {
                    CustomerDTO existingDTO = selectedCustomerDTOList.Find(cust => cust.Id == mappedCustomerDTO.Id);
                    if (existingDTO != null)
                    {
                        mappedCustomerDTO = existingDTO;
                    }
                    alreadyMapped = true;
                    List<WaiversDTO> waiversDTOList = this.transaction.GetWaiversDTOList(lineId);
                    if (waiversDTOList != null && waiversDTOList.Any())
                    {
                        if (mappedCustomerDTO.Id != guestCustomerId)
                        {
                            CustomerBL customerBL = new CustomerBL(executionContext, mappedCustomerDTO);
                            DateTime trxDatevalue = GetTrxDate(lineId);
                            if (customerBL.HasSigned(waiversDTOList, trxDatevalue))
                            {
                                alreadySigned = true;
                            }
                        }
                        else if (mappedCustomerDTO.Id == guestCustomerId && guestCustomerId > -1 && this.transaction.IsWaiverSignaturePending(lineId) == false)
                        {
                            alreadySigned = true;
                        }
                    }
                }
                else if (guestCustomerId > -1)
                {
                    if (mappedCustomerDTO != null && guestCustomerId == mappedCustomerDTO.Id && guestCustomerId > -1)
                    {
                        alreadyMapped = true;
                        if (this.transaction.IsWaiverSignaturePending(lineId) == false)
                        {
                            alreadySigned = true;
                        }
                    }
                    //else
                    //{
                    //    mappedCustomerDTO = new CustomerDTO();
                    //    mappedCustomerDTO.Id = guestCustomerId;
                    //    mappedCustomerDTO.FirstName = guestCustomerDTO.FirstName;
                    //    mappedCustomerDTO.LastName = guestCustomerDTO.LastName;
                    //}
                }
                if (alreadyMapped == false)
                {
                    if (selectedCustomerDTO != null)
                    {
                        alreadyMapped = true;
                        mappedCustomerDTO = selectedCustomerDTO;
                    }
                }
                if (alreadyMapped)
                {
                    pnlTrxLine = new Panel();
                    pnlTrxLine.Margin = new Padding(1);
                    pnlTrxLine.Font = lblSample.Font;
                    pnlTrxLine.Width = 250;
                    pnlTrxLine.Height = 36;
                    pnlTrxLine.Tag = new KeyValuePair<int, int>(lineId, mappedCustomerDTO.Id);
                    // pnlTrxLine.Location = new Point(0, yCoordinate + 1);
                    pnlTrxLine.BackColor = System.Drawing.SystemColors.InactiveCaption;
                    int productId = -1;
                    AttractionBookingDTO atbDTO = new AttractionBookingDTO();
                    if (this.transaction != null && this.transaction.TrxLines != null)
                    {
                        productId = this.transaction.TrxLines[lineId].ProductID;
                        if (this.transaction.TrxLines[lineId].LineAtb != null && this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO != null)
                        {
                            atbDTO = this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO;
                        }
                    }
                    /* InputAllocation.productAllocation.Add(new clsProductAllocation(ProductName + Environment.NewLine +
                    atb.AttractionBookingDTO.AttractionScheduleName + " - " + atb.AttractionBookingDTO.ScheduleFromDate.ToString(POSStatic.ParafaitEnv.DATETIME_FORMAT),
                    atb.AttractionBookingDTO.BookedUnits,
                    atb));*/
                    string atbInfo = string.Empty;
                    if (this.transaction.TrxLines[lineId].LineAtb != null && this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO != null)
                    {
                        //atbInfo = " : " + this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO.AttractionScheduleName
                        //             + " - " + this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO.ScheduleFromDate.ToString(POSStatic.ParafaitEnv.DATETIME_FORMAT);
                        atbInfo = " : " + this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO.ScheduleFromDate.ToString(POSStatic.ParafaitEnv.DATETIME_FORMAT);
                    }
                    Label lblCustomerName = new Label
                    {
                        Location = new Point(0, 0),
                        Tag = new Tuple<int, AttractionBookingDTO>(productId, atbDTO),
                        Width = 170,
                        Height = 39,
                        Font = lblSample.Font,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Text = (mappedCustomerDTO != null ? mappedCustomerDTO.FirstName + " " + (string.IsNullOrEmpty(mappedCustomerDTO.LastName) ? "" : mappedCustomerDTO.LastName) : "") + atbInfo
                    };
                    if (lblCustomerName.Text.Length > 50)
                    {
                        lblCustomerName.MaximumSize = new Size(170, 0);
                        lblCustomerName.AutoSize = true;
                        pnlTrxLine.Height = 46;
                    }
                    new ToolTip().SetToolTip(lblCustomerName, lblCustomerName.Text);
                    lblCustomerName.Click += new EventHandler(pnlTrxPanelLabel_Click);
                    PictureBox pbxSignedStatus = new PictureBox();
                    pbxSignedStatus.Size = new Size(30, 30);
                    pbxSignedStatus.Location = new Point(lblCustomerName.Location.X + lblCustomerName.Width + 2, 3);

                    pnlTrxLine.Controls.Add(lblCustomerName);
                    pnlTrxLine.Controls.Add(pbxSignedStatus);

                    DateTime entitlementConsumptionDate = transaction.EntitlementReferenceDate;
                    if (atbDTO != null)
                    {
                        entitlementConsumptionDate = atbDTO.ScheduleFromDate;
                    }

                    bool hasValidLicense = true;
                    if (!String.IsNullOrEmpty(transaction.TrxLines[lineId].LicenseType))
                    {
                        string requiredLicenseType = CreditPlusTypeConverter.ToString(transaction.TrxLines[lineId].LicenseType);
                        hasValidLicense = String.IsNullOrEmpty(transaction.CheckLicenseForCustomerAndCard(null, transaction.TrxLines[lineId].LicenseType, entitlementConsumptionDate, mappedCustomerDTO.Id));
                    }
                    if (alreadySigned && hasValidLicense)
                    {
                        pbxSignedStatus.BackgroundImage = Properties.Resources.GreenTick;
                        pbxSignedStatus.Tag = "Yes";
                        pbxSignedStatus.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    else
                    {
                        ShowAlertForCustomerSignedWaiverExpiryDate(mappedCustomerDTO, lineId);
                        if (hasValidLicense == false)
                        {
                            string firstName = mappedCustomerDTO.FirstName != null ? mappedCustomerDTO.FirstName : "";
                            string productName = transaction.TrxLines[lineId].ProductName;

                            // POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2962, firstName, productName), MessageContainerList.GetMessage(executionContext, "Message"));
                            DisplayMessage(MessageContainerList.GetMessage(executionContext, 2962, firstName, productName),MAPWAIVER);
                            log.Error("LicenseType is Expired, Proceed to Sale Process");
                        }
                        pbxSignedStatus.BackgroundImage = Properties.Resources.RedCross;
                        pbxSignedStatus.BackgroundImageLayout = ImageLayout.Stretch;
                    }

                    Button btnDeleteWaiverMapping = new Button();
                    btnDeleteWaiverMapping.Size = new Size(30, 30);
                    btnDeleteWaiverMapping.Location = new Point(pbxSignedStatus.Location.X + pbxSignedStatus.Width + 10, 3);
                    btnDeleteWaiverMapping.BackgroundImage = Properties.Resources.Delete_Icon_Normal;
                    btnDeleteWaiverMapping.BackgroundImageLayout = ImageLayout.Stretch;
                    btnDeleteWaiverMapping.Tag = new KeyValuePair<int, int>(lineId, mappedCustomerDTO.Id);
                    btnDeleteWaiverMapping.Click += btnDeleteWaiverMapping_Click;

                    pnlTrxLine.Controls.Add(btnDeleteWaiverMapping);
                }
            }
            log.LogMethodExit(pnlTrxLine);
            return pnlTrxLine;
        }
        private void ShowAlertForCustomerSignedWaiverExpiryDate(CustomerDTO mappedCustomerDTO, int lineId)
        {
            log.LogMethodEntry(lineId);
            try
            {
                POSUtils.SetLastActivityDateTime();
                List<ValidationError> validationErrorList = this.transaction.ValidateCustomerSignedWaiverExpiryDate(mappedCustomerDTO, lineId);
                if (validationErrorList != null && validationErrorList.Any())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Expiry Date"), validationErrorList);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //POSUtils.ParafaitMessageBox(ex.Message);
                DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), MAPWAIVER);
            }
            log.LogMethodExit();
        }

        private void btnDeleteWaiverMapping_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                if (IsBackgroundJobRunning() == false)
                {
                    Button btnDeleteWaiverMapping = (Button)sender;
                    if (btnDeleteWaiverMapping.Tag != null)
                    {
                        KeyValuePair<int, int> lineCustomerIdTag = (KeyValuePair<int, int>)(btnDeleteWaiverMapping.Tag);
                        CustomerDTO custDTO = this.transaction.GetMappedCustomerForWaiver(lineCustomerIdTag.Key);
                        this.transaction.RemoveMappedWaivers(lineCustomerIdTag.Key);
                        RemovePanelForTrxLine(lineCustomerIdTag.Key);
                        ResetUsrCtlCustomer((custDTO == null ? lineCustomerIdTag.Value : custDTO.Id));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //POSUtils.ParafaitMessageBox(ex.Message);
                DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), MAPWAIVER);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void RemovePanelForTrxLine(int lineId)
        {
            log.LogMethodEntry(lineId);
            int productId = this.transaction.TrxLines[lineId].ProductID;
            AttractionBookingDTO atbDTO = new AttractionBookingDTO();
            if (this.transaction.TrxLines[lineId].LineAtb != null && this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO != null)
            {
                atbDTO = this.transaction.TrxLines[lineId].LineAtb.AttractionBookingDTO;
            }
            List<FlowLayoutPanel> productPanelList = fPnlProductWaiverMap.Controls.OfType<FlowLayoutPanel>().ToList();
            if (productPanelList != null)
            {
                FlowLayoutPanel productPanel = productPanelList.FirstOrDefault(pnl => pnl.Tag != null && ((Tuple<int, AttractionBookingDTO>)pnl.Tag).Item1 == productId
                                                                                                      && ((Tuple<int, AttractionBookingDTO>)pnl.Tag).Item2.AttractionScheduleId == atbDTO.AttractionScheduleId
                                                                                                      && ((Tuple<int, AttractionBookingDTO>)pnl.Tag).Item2.ScheduleFromDate == atbDTO.ScheduleFromDate);
                if (productPanel != null)
                {
                    List<Panel> linePanelList = productPanel.Controls.OfType<Panel>().ToList();
                    if (linePanelList != null)
                    {
                        int locationY = 0;
                        Panel linePanel = linePanelList.FirstOrDefault(pnl => pnl.Tag != null && ((KeyValuePair<int, int>)pnl.Tag).Key == lineId);
                        if (linePanel != null)
                        {
                            locationY = linePanel.Location.Y;
                            int panelIndex = linePanelList.IndexOf(linePanel);
                            productPanel.Controls.Remove(linePanel);
                            //linePanelList = productPanel.Controls.OfType<Panel>().ToList();
                            //if (linePanelList != null)
                            //{
                            //    for (int i = panelIndex; i < linePanelList.Count; i++)
                            //    {
                            //        int oldLocationY = linePanelList[i].Location.Y;
                            //        linePanelList[i].Location = new Point(linePanelList[i].Location.X, locationY);
                            //        locationY = oldLocationY;
                            //    }
                            //}
                        }
                        productPanel.Refresh();
                        UpdateTrxProductHeaderInfo(productId, atbDTO, productPanel);
                        AdjustPanelHeight();
                    }
                }
            }
            log.LogMethodExit();
        }
        private void ShowHideCustomerButtons()
        {
            log.LogMethodEntry();
            //btnCustomerLookup.Visible = true;
            btnNewCustomer.Visible = true;
            POSUtils.SetLastActivityDateTime();
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "WAIVER_CODE_IS_MANDATORY_TO_FETCH_CUSTOMER", false))
            {
               //btnCustomerLookup.Visible = false;
                btnNewCustomer.Visible = false;
                pnlCustSearchHeader.Enabled = false;
                pnlCustSearchTab.Enabled = false;
            }
            log.LogMethodExit();
        }

        private void DoDefaultAssignment()
        {
            log.LogMethodEntry();
            try
            {
                //this.Cursor = Cursors.WaitCursor;
                //List<Transaction.TransactionLine> waiverTrxLines = this.transaction.GetWaiverTransactionLines();
                //if (waiverTrxLines != null && waiverTrxLines.Any())
                //{
                //    List<int> productIdList = waiverTrxLines.Select(tl => tl.ProductID).Distinct().ToList();
                //    if (productIdList != null && productIdList.Count == 1)
                //    {
                //        int productId = productIdList[0];
                //        List<FlowLayoutPanel> productPanelList = fPnlProductWaiverMap.Controls.OfType<FlowLayoutPanel>().ToList();
                //        if (productPanelList != null && productPanelList.Any())
                //        {
                //            int mappedCustCount = GetMappedCustomerCount(productPanelList[0]);
                //            int productTrxLineCount = selectedWaiverTrxLines.Where(tl => tl.ProductID == productId).Count();
                //            int actualCustCount = (selectedCustomerDTOList == null ? 0 : selectedCustomerDTOList.Count);
                //            if (mappedCustCount != productTrxLineCount && actualCustCount > mappedCustCount)
                //            {
                //                SetCustomerRecordAsSelected();
                //                MapCustomerWaiverToProdcutTrxLine(productId, productPanelList[0], true);
                //            }
                //        }
                //    }
                //}
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void SetCustomerRecordAsSelected()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            SetCustomerIsSelected(true);
            log.LogMethodExit();
        }
        private void btnGetWaiverCodeCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                GetCustomersbyWaiverCode();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //POSUtils.ParafaitMessageBox(ex.Message);
                DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), WAIVERCODE);
            }
        }
        private void GetCustomersbyWaiverCode()
        {
            log.LogMethodEntry();
            if (IsBackgroundJobRunning() == false)
            {
                CustomerSignedWaiverHeaderListBL customerSignedWaiverHeaderList = new CustomerSignedWaiverHeaderListBL(executionContext);
                List<CustomerDTO> customerDTOList = customerSignedWaiverHeaderList.GetCustomersbyWaiverCode(txtWaiverCode.Text);
                if (customerDTOList != null && customerDTOList.Any())
                {
                    if (trxWaiversDTOList == null || trxWaiversDTOList.Count == 0)
                    {
                        trxWaiversDTOList = transaction.GetWaiversDTOList();
                    }
                    for (int i = 0; i < customerDTOList.Count; i++)
                    {
                        AddCustomerToSelectionPanel(customerDTOList[i]);
                    }

                    ExpandSelectionTab();
                    DisplayMessage(MessageContainerList.GetMessage(executionContext, 4099), WAIVERCODE);
                    //'Customer(s) linked with Waiver code are loaded'
                }
                else
                {
                    //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2420), MessageContainerList.GetMessage(executionContext, "WaiverCode"));
                    DisplayMessage(MessageContainerList.GetMessage(executionContext, 2420), WAIVERCODE);
                }
            }
            log.LogMethodExit();
        }

        private void btnCustomerLookup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    using (CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities))
                    {
                        POSUtils.SetLastActivityDateTime();
                        if (customerLookupUI.ShowDialog() == DialogResult.OK)
                        {
                            CustomerDTO selectedCustomerDTO = customerLookupUI.SelectedCustomerDTO;
                            CustomerListBL customerListBL = new CustomerListBL(executionContext);
                            List<int> custIdList = new List<int>();
                            custIdList.Add(selectedCustomerDTO.Id);
                            List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(custIdList, true, true, true, null);
                            if (customerDTOList != null && customerDTOList.Any())
                            {
                                AddCustomerToSelectionPanel(customerDTOList[0]);
                                DoDefaultAssignment();
                            }
                        }
                        POSUtils.SetLastActivityDateTime();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), CUSTOMERLOOKUP);
            }
            log.LogMethodExit();
        }


        private void btnNewCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    CustomerDTO newCustomerDTO = new CustomerDTO();
                    using (CustomerDetailForm customerDetailForm = new CustomerDetailForm(utilities, newCustomerDTO, POSUtils.ParafaitMessageBox,
                                                                                          ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD")))
                    {
                        POSUtils.SetLastActivityDateTime();
                        customerDetailForm.SetControlsEnabled(true);
                        if (customerDetailForm.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                POSUtils.SetLastActivityDateTime();
                                newCustomerDTO = customerDetailForm.CustomerDTO;
                                CustomerBL customerBL = new CustomerBL(executionContext, newCustomerDTO);
                                customerBL.Save(null);
                                AddCustomerToSelectionPanel(customerBL.CustomerDTO);
                                DoDefaultAssignment();
                                POSUtils.SetLastActivityDateTime();
                            }
                            catch (Exception ex)
                            {
                                log.Error("Validation failed", ex);
                                //POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(executionContext, "Customer Details"));
                                DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), NEWCUSTOMER);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), NEWCUSTOMER);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void RefreshProductWaiverMapping(CustomerDTO customerDTO)
        {
            log.LogMethodEntry(customerDTO);
            if (customerDTO != null)
            {
                POSUtils.SetLastActivityDateTime();
                if (customerDTO.Id == guestCustomerId && guestCustomerId > -1)
                {
                    LoadProductWaiverMappingPanel();
                }
                else
                {
                    RefreshProductMappingPanel(customerDTO);
                }
                vScrollBarProductCustomerMap.UpdateButtonStatus();
                hScrollBarProductCustomerMap.UpdateButtonStatus();
            }
            log.LogMethodExit();
        }
        private void RefreshProductMappingPanel(CustomerDTO customerDTO)
        {
            log.LogMethodEntry();
            List<FlowLayoutPanel> productPanelList = fPnlProductWaiverMap.Controls.OfType<FlowLayoutPanel>().ToList();
            if (productPanelList != null)
            {
                POSUtils.SetLastActivityDateTime();
                foreach (FlowLayoutPanel productPanel in productPanelList)
                {
                    List<Panel> productTrxLinePanelList = productPanel.Controls.OfType<Panel>().ToList();
                    if (productTrxLinePanelList != null)
                    {
                        foreach (Panel trxLinePanel in productTrxLinePanelList)
                        {
                            if (trxLinePanel.Tag != null)
                            {
                                KeyValuePair<int, int> lineCustomerIdPair = (KeyValuePair<int, int>)trxLinePanel.Tag;
                                if (lineCustomerIdPair.Value == customerDTO.Id)
                                {

                                    List<WaiversDTO> waiversDTOList = this.transaction.GetWaiversDTOList(lineCustomerIdPair.Key);
                                    if (waiversDTOList != null && waiversDTOList.Any())
                                    {
                                        CustomerBL customerBL = new CustomerBL(executionContext, customerDTO);
                                        PictureBox pbxSigned = trxLinePanel.Controls.OfType<PictureBox>().ToList()[0];
                                        DateTime trxDatevalue = GetTrxDate(lineCustomerIdPair.Key);
                                        if (customerBL.HasSigned(waiversDTOList, trxDatevalue))
                                        {
                                            this.transaction.MapCustomerWaiversToLine(lineCustomerIdPair.Key, customerDTO);
                                            pbxSigned.BackgroundImage = Properties.Resources.GreenTick;
                                            pbxSigned.BackgroundImageLayout = ImageLayout.Stretch;
                                            pbxSigned.Tag = "Yes";
                                        }
                                        else
                                        {
                                            pbxSigned.BackgroundImage = Properties.Resources.RedCross;
                                            pbxSigned.BackgroundImageLayout = ImageLayout.Stretch;
                                            pbxSigned.Tag = "No";
                                        }
                                    }
                                }
                            }

                        }
                        int productId = -1;
                        AttractionBookingDTO atbDTO = new AttractionBookingDTO();
                        if (productPanel.Tag != null)
                        {
                            Tuple<int, AttractionBookingDTO> productScheduleIds = (Tuple<int, AttractionBookingDTO>)productPanel.Tag;
                            //int.TryParse(productPanel.Tag.ToString(), out productId);
                            productId = productScheduleIds.Item1;
                            atbDTO = productScheduleIds.Item2;
                        }
                        if (productId > -1)
                        {
                            UpdateTrxProductHeaderInfo(productId, atbDTO, productPanel);
                        }
                    }
                }
                vScrollBarProductCustomerMap.UpdateButtonStatus();
                hScrollBarProductCustomerMap.UpdateButtonStatus();
            }
            log.LogMethodExit();
        }
        private void btnOkay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    //SaveAndClose();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), MAPWAIVER);
            }
            log.LogMethodExit();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    this.DialogResult = DialogResult.Cancel;
                    //if (this.transaction.IsWaiverSignaturePending()) //Add selected trxline check
                    //{
                    //    //2320,'Waiver signature is pending. Do you want to proceed?
                    //    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2320), MessageContainerList.GetMessage(executionContext, "Save"), MessageBoxButtons.YesNo) == DialogResult.No)
                    //    {
                    //        return;
                    //    }
                    //}
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), MAPWAIVER);
            }
            log.LogMethodExit();
        }
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning(true) == false)
                {
                    txtMessage.Clear(); 
                    SetCustomerIsSelected(chkSelectAll.Checked);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), MAPWAIVER);
            }
            log.LogMethodExit();
        }
        private void chkSelectAll_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning())
                {
                    txtMessage.Clear();
                    chkSelectAll.Checked = !chkSelectAll.Checked;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), MAPWAIVER);
            }
            log.LogMethodExit();
        }
        private void SetCustomerIsSelected(bool flagValue)
        {
            log.LogMethodEntry(flagValue);
            foreach (Control panelControl in fpnlCustomerSelection.Controls)
            {
                if (panelControl is usrCtlCustomer)
                {
                    usrCtlCustomer usrCtlCustomer = (usrCtlCustomer)panelControl;
                    usrCtlCustomer.IsSelected = flagValue;
                }
            }
            log.LogMethodExit();
        }
        private void frmMapWaiversToTransaction_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (IsBackgroundJobRunning())
                {
                    e.Cancel = true;
                    return;
                }
                if (this.transaction.IsWaiverSignaturePending()) //Add selected trxline check
                {
                    //2320,'Waiver signature is pending. Do you want to proceed?
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2320), MAPWAIVER, MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                if (this.DialogResult != DialogResult.OK)
                {
                    this.DialogResult = DialogResult.Cancel;
                }
                Common.Devices.UnregisterCardReaders();
                if (barcodeScanner != null)
                {
                    barcodeScanner.UnRegister();
                    barcodeScanner.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e is DeviceScannedEventArgs)
                {
                    POSUtils.SetLastActivityDateTime();
                    this.Cursor = Cursors.WaitCursor;
                    DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                    TagNumber tagNumber;
                    TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
                    string scannedTagNumber = checkScannedEvent.Message;
                    DeviceClass encryptedTagDevice = sender as DeviceClass;
                    if (tagNumberParser.IsTagDecryptApplicable(encryptedTagDevice, checkScannedEvent.Message.Length))
                    {
                        string decryptedTagNumber = string.Empty;
                        try
                        {
                            decryptedTagNumber = tagNumberParser.GetDecryptedTagData(encryptedTagDevice, checkScannedEvent.Message);
                        }
                        catch (Exception ex)
                        {
                            log.LogVariableState("Decrypted Tag Number result: ", ex);
                            //POSUtils.ParafaitMessageBox(ex.Message);
                            DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), TAPCARD);
                            return;
                        }
                        try
                        {
                            scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, utilities.ParafaitEnv.SiteId);
                        }
                        catch (ValidationException ex)
                        {
                            log.LogVariableState("Decrypted Tag Number validation: ", ex);
                            //POSUtils.ParafaitMessageBox(ex.Message);
                            DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), TAPCARD);
                            return;
                        }
                        catch (Exception ex)
                        {
                            log.LogVariableState("Decrypted Tag Number validation: ", ex);
                            //POSUtils.ParafaitMessageBox(ex.Message);
                            DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), TAPCARD);
                            return;
                        }
                    }
                    if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                    {
                        string message = tagNumberParser.Validate(scannedTagNumber);
                        //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, message), MessageContainerList.GetMessage(executionContext, "Tap Card"));
                        DisplayMessage(MessageContainerList.GetMessage(executionContext, message), TAPCARD);
                        log.LogMethodExit(null, "Invalid Tag Number.");
                        return;
                    }
                    CardSwiped(tagNumber.Value);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void CardSwiped(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            try
            {
                txtMessage.Clear();
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                if (IsBackgroundJobRunning() == false)
                {
                    ValidateCardAndAddCustomerDetails(cardNumber);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MessageContainerList.GetMessage(executionContext, "Tap Card"));
                DisplayMessage(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), TAPCARD);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void ValidateCardAndAddCustomerDetails(string cardNumber)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            if (string.IsNullOrEmpty(cardNumber.Trim()) == false)
            {
                Card card = new Card(cardNumber.Trim(), "", utilities);
                if (card.CardStatus.Equals("NEW"))
                {
                    //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 172), MessageContainerList.GetMessage(executionContext, "Tap Card"));
                    DisplayMessage(MessageContainerList.GetMessage(executionContext, 172), TAPCARD);
                    log.LogMethodExit("Card is Invalid");
                    this.Cursor = Cursors.Default;
                    return;
                }
                else if (card.technician_card.Equals('N') == false)
                {
                    //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 197, cardNumber), MessageContainerList.GetMessage(executionContext, "Tap Card"));
                    DisplayMessage(MessageContainerList.GetMessage(executionContext, 197, cardNumber), TAPCARD);
                    log.LogMethodExit("Tech card used");
                    this.Cursor = Cursors.Default;
                    return;
                }
                else if (card.customer_id > 0)
                {
                    CustomerDTO customerDTO = (new CustomerBL(executionContext, card.customer_id)).CustomerDTO;
                    if (customerDTO != null)
                    {
                        //AddToSelectedCustomerIdList(customerDTO.Id);
                        List<int> custIds = GetRelatedCustomerIds(customerDTO.Id);
                        List<int> finalList = new List<int>();
                        custIds.Add(customerDTO.Id);
                        for (int i = 0; i < custIds.Count; i++)
                        {
                            if (selectedCustomerDTOList.Exists(cust => cust.Id == custIds[i]) == false)
                            {
                                finalList.Add(custIds[i]);
                            }
                        }
                        if (finalList != null && finalList.Any())
                        {
                            //RefreshCustomerPanel();
                            CustomerListBL customerListBL = new CustomerListBL(executionContext);
                            List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(finalList, true, true, true);
                            if (customerDTOList != null && customerDTOList.Any())
                            {
                                foreach (CustomerDTO item in customerDTOList)
                                {
                                    AddCustomerToSelectionPanel(item);
                                }
                            }
                            DoDefaultAssignment();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void btnOverridePending_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                if (IsBackgroundJobRunning() == false)
                {
                    if (btnOverridePending.Tag != null && btnOverridePending.Tag.ToString() == "O")
                    {
                        OverRidePendingWaivers();
                    }
                    else if (btnOverridePending.Tag != null && btnOverridePending.Tag.ToString() == "R")
                    {
                        ResetOverRidePendingWaivers();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MessageContainerList.GetMessage(executionContext, "Override"));
                DisplayMessage(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), OVERRIDEWAIVERS);
            }
            log.LogMethodExit();
        }
        private void OverRidePendingWaivers()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (this.transaction.IsWaiverSignaturePending())
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_WAIVER_OVERRIDE"))
                {
                    //Do you want to override pending waiver mapping?
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2378), OVERRIDEWAIVERS, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        int mgrId = -1;
                        if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "WAIVER_OVERRIDE_NEEDS_MANAGER_APPROVAL"))
                        {
                            if (Authenticate.Manager(ref mgrId) == false)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Manager Approval Required"));
                            }
                        }
                        POSUtils.SetLastActivityDateTime();
                        this.transaction.OverridePendingWaivers(mgrId, utilities.getServerTime());
                        POSUtils.SetLastActivityDateTime();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2379), MessageContainerList.GetMessage(executionContext, "Override"));
                    //'Waiver override is not allowed'
                    DisplayMessage(MessageContainerList.GetMessage(executionContext, 2379), OVERRIDEWAIVERS);
                }
            }
            else
            {
                //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, "Nothing to update"), MessageContainerList.GetMessage(executionContext, "Override"));
                DisplayMessage(MessageContainerList.GetMessage(executionContext, "Nothing to update"), OVERRIDEWAIVERS);
            }
            log.LogMethodExit();
        }

        private void ResetOverRidePendingWaivers()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (TransactionHasWaiverOverridenLines())
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ALLOW_WAIVER_OVERRIDE"))
                {
                    //'Do you want to proceed with reseting overriden waivers for the transaction?'
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 4070), RESETOVERRIDEWAIVERS, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        int mgrId = -1;
                        if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "WAIVER_OVERRIDE_NEEDS_MANAGER_APPROVAL"))
                        {
                            if (Authenticate.Manager(ref mgrId) == false)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Manager Approval Required"));
                            }
                        }
                        POSUtils.SetLastActivityDateTime();
                        this.transaction.ResetOverridenWaivers(mgrId, utilities.getServerTime());
                        POSUtils.SetLastActivityDateTime();
                        //this.DialogResult = DialogResult.OK;
                        //this.Close();
                        LoadProductWaiverMappingPanel();
                        SetOverridePendingButtonText();
                    }
                }
                else
                {
                    //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2379), MessageContainerList.GetMessage(executionContext, "Reset Override"));
                    DisplayMessage(MessageContainerList.GetMessage(executionContext, 2379), RESETOVERRIDEWAIVERS);
                    //'Waiver override is not allowed'
                }
            }
            else
            {
                //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, "Nothing to update"), MessageContainerList.GetMessage(executionContext, "Override"));
                DisplayMessage(MessageContainerList.GetMessage(executionContext, "Nothing to update"), RESETOVERRIDEWAIVERS);
            }
            log.LogMethodExit();
        }
        private void bgwCustomerPanel_DoWork(object sender, DoWorkEventArgs e)
        {
            log.LogMethodEntry();
            fpnlCustomerSelection.Controls.Clear();
            //customerControlList = new List<Control>();
            CustomerDTO guestCustDTO = null;
            if (guestCustomerId > -1 && selectedCustomerDTOList != null && selectedCustomerDTOList.Any())
            {
                guestCustDTO = selectedCustomerDTOList.Find(cust => cust.Id == guestCustomerId);
                selectedCustomerIdList.Remove(guestCustomerId); //remove from id list
                if (guestCustDTO == null && ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SIGN_WAIVER_WITHOUT_CUSTOMER_REGISTRATION", false) == true)
                {
                    CustomerBL customerBL = new CustomerBL(executionContext, guestCustomerId);
                    guestCustDTO = customerBL.CustomerDTO;
                }
            }
            selectedCustomerDTOList = new List<CustomerDTO>();
            CustomerListBL customerListBL = new CustomerListBL(executionContext);
            selectedCustomerDTOList = customerListBL.GetCustomerDTOList(selectedCustomerIdList, true, true, true, null);
            int recordCount = 0;
            foreach (CustomerDTO selectedCustDTO in selectedCustomerDTOList)
            {
                usrCtlCustomer usrCtlCustomer = CreateUsrCtlCustomerElement(selectedCustDTO, trxWaiversDTOList, recordCount);
                // customerControlList.Add(usrCtlCustomer);
                recordCount++;
                bgwCustomerPanel.ReportProgress(recordCount, usrCtlCustomer);
            }
            Thread.Sleep(100);
            resetBGWCustomer.Set();
            log.LogMethodExit();
        }
        private void bgwCustomerPanel_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            usrCtlCustomer usrCtlCustomer = (usrCtlCustomer)e.UserState;
            if (usrCtlCustomer != null)
            {
                POSUtils.SetLastActivityDateTime();
                fpnlCustomerSelection.Controls.Add(usrCtlCustomer);
            }
            log.LogMethodExit();
        }
        private void bgwCustomerPanel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.Default;
            vScrollBarCustomerSelection.UpdateButtonStatus();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void bgwTransactionPanel_DoWork(object sender, DoWorkEventArgs e)
        {
            log.LogMethodEntry();
            selectedWaiverTrxLines = null;
            //trxProductControlList = new List<Control>();
            //panelHeight = fPnlProductWaiverMap.Height - 30;
            if (this.transaction != null)
            {
                List<Transaction.TransactionLine> waiverTrxLines = this.transaction.GetWaiverTransactionLines();
                if (waiverTrxLines != null && waiverTrxLines.Any())
                {
                    selectedWaiverTrxLines = waiverTrxLines.Where(tl => this.transactionLines.Exists(tll => tll == tl)).ToList();
                }
                if (selectedWaiverTrxLines != null && selectedWaiverTrxLines.Any())
                {
                    List<int> productIdList = selectedWaiverTrxLines.Select(tl => tl.ProductID).Distinct().ToList();
                    if (productIdList != null && productIdList.Any())
                    {
                        //startTime = DateTime.Now;
                        int xLocation = 1;// fPnlProductWaiverMap.Location.X;
                        for (int i = 0; i < productIdList.Count; i++)
                        {
                            bgwTransactionPanel.ReportProgress(i);
                            List<AttractionBookingDTO> atbScheduleDTOList = selectedWaiverTrxLines.Where(tl => tl.ProductID == productIdList[i]
                                                                                                              && tl.LineAtb != null
                                                                                                              && tl.LineAtb.AttractionBookingDTO != null).Select(tl => tl.LineAtb.AttractionBookingDTO).ToList();
                            if (atbScheduleDTOList != null && atbScheduleDTOList.Any())
                            {
                                atbScheduleDTOList = atbScheduleDTOList.GroupBy(atb => new { atb.AttractionScheduleId, atb.ScheduleFromDate }).Select(atb => atb.FirstOrDefault()).ToList();
                            }
                            else
                            {
                                atbScheduleDTOList.Add(new AttractionBookingDTO());
                            }
                            for (int j = 0; j < atbScheduleDTOList.Count; j++)
                            {
                                FlowLayoutPanel pnlProductPanel = LoadProductWaiverDetails(xLocation, productIdList[i], atbScheduleDTOList[j]);
                                if (pnlProductPanel != null)
                                {
                                    pnlProductPanel.BorderStyle = BorderStyle.FixedSingle;
                                    //trxProductControlList.Add(pnlProductPanel);
                                    // fPnlProductWaiverMap.Controls.Add(pnlProductPanel);
                                    xLocation = xLocation + pnlProductPanel.Width + 2;
                                    bgwTransactionPanel.ReportProgress(i, pnlProductPanel);
                                }
                            }
                        }
                    }
                }
            }
            Thread.Sleep(100);
            resetBGWTrxPanel.Set();
            log.LogMethodExit();
        }
        private void bgwTransactionPanel_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (e.UserState != null)
                {
                    FlowLayoutPanel pnlProductPanel = (FlowLayoutPanel)e.UserState;
                    if (pnlProductPanel != null)
                    {
                        fPnlProductWaiverMap.Controls.Add(pnlProductPanel);
                        POSUtils.SetLastActivityDateTime();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void bgwTransactionPanel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                AdjustPanelHeight();
                POSUtils.SetLastActivityDateTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MessageContainerList.GetMessage(executionContext, "Load"));
                DisplayMessage(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MAPWAIVER);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void bgwDoDefaultMap_DoWork(object sender, DoWorkEventArgs e)
        {
            log.LogMethodEntry();
            resetBGWTrxPanel.WaitOne();
            resetBGWCustomer.WaitOne();
            log.LogMethodExit();
        }
        private void bgwDoDefaultMap_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            log.LogMethodExit();
        }
        private void bgwDoDefaultMap_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.Default;
            DoDefaultAssignment();
            log.LogMethodExit();
        }
        private bool IsBackgroundJobRunning(bool supressMsg = false)
        {
            log.LogMethodEntry(supressMsg);
            POSUtils.SetLastActivityDateTime();
            bool jobRunning = false;
            if (bgwCustomerPanel.IsBusy || bgwTransactionPanel.IsBusy || bgwDoDefaultMap.IsBusy || bgwCustomerSearchPanel.IsBusy)
            {
                jobRunning = true;
                if (supressMsg == false)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1448));//Loading... Please wait...
                }
            }
            log.LogMethodExit(jobRunning);
            return jobRunning;
        }
        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    this.Cursor = Cursors.WaitCursor;
                    PerformCustomerSearch();
                    ShowSearchAlerts();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MessageContainerList.GetMessage(executionContext, "Search"));
                DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), CUSTSEARCH);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void PerformCustomerSearch(bool getLatestSigned = false)
        {
            log.LogMethodEntry(getLatestSigned);
            POSUtils.SetLastActivityDateTime();
            List<KeyValuePair<CustomerSearchByParameters, string>> searchParams = BuildSearchParameters(getLatestSigned);
            this.Cursor = Cursors.WaitCursor;
            string msg = MessageContainerList.GetMessage(executionContext, "Fetching Customer records.") + " " +
                          MessageContainerList.GetMessage(executionContext, 684);// "Please wait..."
            searchedCustomerDTOList = BackgroundProcessRunner.Run<List<CustomerDTO>>(() => { return GetCustomerData(searchParams); }, msg, BackgroundProcessRunner.LaunchWaitScreenAfterXSeconds);
            this.Cursor = Cursors.WaitCursor;
            POSUtils.SetLastActivityDateTime();
            //searchedCustomerDTOList  = GetCustomerData(searchParams);
            LoadCustomerSearchTab();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private List<CustomerDTO> GetCustomerData(List<KeyValuePair<CustomerSearchByParameters, string>> searchParams)
        {
            log.LogMethodExit();
            CustomerListBL customerListBL = new CustomerListBL(executionContext);
            List<CustomerDTO> customerDTOLocalList = new List<CustomerDTO>();
            customerDTOLocalList = customerListBL.GetCustomerDTOList(searchParams, true, true, true);
            if (cbxGetRelatedCustomers.Checked && customerDTOLocalList != null && customerDTOLocalList.Any())
            {
                List<CustomerDTO> relatedCustomerDTOList = GetReletedCustomerDTO(customerDTOLocalList);
                if (relatedCustomerDTOList != null && relatedCustomerDTOList.Any())
                {
                    customerDTOLocalList.AddRange(relatedCustomerDTOList);
                }
            }
            if (customerDTOLocalList != null && customerDTOLocalList.Any()
                && searchParams != null && searchParams.Exists( k => k.Key == CustomerSearchByParameters.LATEST_TO_SIGN_WAIVER))
            {
                customerDTOLocalList = customerDTOLocalList.OrderByDescending(c => c.CustomerSignedWaiverDTOList.Max(w => w.SignedDate)).ToList();
            }
            log.LogMethodExit();
            return customerDTOLocalList;
        }

        private void ShowSearchAlerts()
        {
            if (searchedCustomerDTOList == null || searchedCustomerDTOList.Any() == false)
            {
                //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 3015), MessageContainerList.GetMessage(executionContext, "Search"));
                //Search returned zero records
                DisplayMessage(MessageContainerList.GetMessage(executionContext, 3015), CUSTSEARCH);
            }
            else if (searchedCustomerDTOList.Count > maxDisplayRecordCount)
            {
                //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 4089, searchedCustomerDTOList.Count, maxDisplayRecordCount), MessageContainerList.GetMessage(executionContext, "Search"));
                //'Search returned &1 records. Displaying first &2 records only'
                DisplayMessage(MessageContainerList.GetMessage(executionContext, 4089, searchedCustomerDTOList.Count, maxDisplayRecordCount), CUSTSEARCH);
            }
        }

        private List<KeyValuePair<CustomerSearchByParameters, string>> BuildSearchParameters(bool getLatestSigned = false)
        {
            log.LogMethodEntry(getLatestSigned);
           //CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
            
            List<KeyValuePair<CustomerSearchByParameters, string>> searchParams = new List<KeyValuePair<CustomerSearchByParameters, string>>();
            if (string.IsNullOrWhiteSpace(txtFirstNameSearch.Text) == false)
            {
                searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.PROFILE_FIRST_NAME_LIKE, txtFirstNameSearch.Text));
                //customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_FIRST_NAME, Operator.LIKE, txtFirstNameSearch.Text);
            }
            if (string.IsNullOrWhiteSpace(txtLastNameSearch.Text) == false)
            {
                searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.PROFILE_LAST_NAME_LIKE, txtLastNameSearch.Text));
                //customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_LAST_NAME, Operator.LIKE, txtLastNameSearch.Text);
            }
            if (string.IsNullOrWhiteSpace(txtEmailSearch.Text) == false)// && string.IsNullOrWhiteSpace(txtPhoneSearch.Text))
            {
                searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, ContactType.EMAIL.ToString()));
                //customerSearchCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.EMAIL.ToString());
                //customerSearchCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, txtEmailSearch.Text);
                if (btnEmailSearchOption.Tag == null || btnEmailSearchOption.Tag.ToString() == EXACT_SEARCH)
                {
                    searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, DOEXACTSEARCHKEYWORD + txtEmailSearch.Text));
                }
                else
                {
                    searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, txtEmailSearch.Text));
                }
            }
            if (string.IsNullOrWhiteSpace(txtPhoneSearch.Text) == false)// && string.IsNullOrWhiteSpace(txtEmailSearch.Text))
            {
                searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, ContactType.PHONE.ToString()));
                if (btnPhoneSearchOption.Tag == null || btnPhoneSearchOption.Tag.ToString() == EXACT_SEARCH)
                {
                    searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, DOEXACTSEARCHKEYWORD + txtPhoneSearch.Text));
                }
                else
                {
                    searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, txtPhoneSearch.Text));
                }
            } 
            if (getLatestSigned)
            {
                string waiverIdList = string.Empty;
                if (trxWaiversDTOList != null && trxWaiversDTOList.Any())
                {
                    for (int i = 0; i < trxWaiversDTOList.Count; i++)
                    {
                        waiverIdList = waiverIdList + trxWaiversDTOList[i].WaiverSetDetailId.ToString() + ",";
                    }
                    waiverIdList = waiverIdList.Substring(0, waiverIdList.Length - 1);
                }
                searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.LATEST_TO_SIGN_WAIVER, countOfLatestWaiverSignedCustomers.ToString() + "|" + waiverIdList));
                //customerSearchCriteria.And(CustomerSearchByParameters.LATEST_TO_SIGN_WAIVER, Operator.EQUAL_TO, countOfLatestWaiverSignedCustomers.ToString() + "|" + waiverIdList);
            }
            if (cmbChannels.SelectedIndex > 0)
            {
                searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CHANNEL_USED_TO_SIGN_WAIVER, cmbChannels.SelectedValue.ToString()));
                //customerSearchCriteria.And(CustomerSearchByParameters.CHANNEL_USED_TO_SIGN_WAIVER, Operator.EQUAL_TO, cmbChannels.SelectedValue.ToString());
            }
            //if (customerSearchCriteria != null && customerSearchCriteria.GetCriteriaParameters().Count == 0)
            if (searchParams.Any() == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1671));
                //Please enter search input
            }
            log.LogMethodExit(searchParams);
            return searchParams;
        }
        private void LoadCustomerSearchTab()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (searchedCustomerDTOList != null && searchedCustomerDTOList.Any())
            {
                if (bgwCustomerSearchPanel.IsBusy == false)
                {
                    fpnlCustomerSearch.Controls.Clear();
                    bgwCustomerSearchPanel.WorkerReportsProgress = true;
                    bgwCustomerSearchPanel.RunWorkerAsync();
                }
            }
            log.LogMethodExit();
        }
        private void bgwCustomerSearchPanel_DoWork(object sender, DoWorkEventArgs e)
        {
            log.LogMethodEntry();
            int recordCount = 0;
            foreach (CustomerDTO selectedCustDTO in searchedCustomerDTOList)
            {
                usrCtlCustomer usrCtlCustomer = CreateUsrCtlCustomerElement(selectedCustDTO, trxWaiversDTOList, recordCount);
                // customerControlList.Add(usrCtlCustomer);
                recordCount++;
                bgwCustomerSearchPanel.ReportProgress(recordCount, usrCtlCustomer);
                if (recordCount > maxDisplayRecordCount)
                {
                    break;
                }
            }
            Thread.Sleep(100);
            resetBGWCustomerSearch.Set();
            log.LogMethodExit();
        }
        private void bgwCustomerSearchPanel_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            usrCtlCustomer usrCtlCustomer = (usrCtlCustomer)e.UserState;
            if (usrCtlCustomer != null)
            {
                fpnlCustomerSearch.Controls.Add(usrCtlCustomer);
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }
        private void bgwCustomerSearchPanel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.Default;
            vScrollBarCustomerSearch.UpdateButtonStatus();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void btnExpandCollapse_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    this.Cursor = Cursors.WaitCursor;
                    Button senderBtn = (Button)sender;
                    if (senderBtn != null)
                    {
                        CollapseExpandPanels(senderBtn);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void CollapseExpandPanels(Button senderBtn)
        {
            log.LogMethodEntry();
            bool isExpandPanel = false;
            POSUtils.SetLastActivityDateTime();
            if (senderBtn.Tag == null || string.IsNullOrWhiteSpace(senderBtn.Tag.ToString()) || senderBtn.Tag.ToString() == COLLAPSEPANEL)
            {
                isExpandPanel = true;
            }
            if (senderBtn.Name == "btnExpandCollapseSearch")
            {
                if (isExpandPanel)
                {
                    ExpandSearchTab();
                }
                else
                {
                    ExpandSelectionTab();
                }
            }
            else if (senderBtn.Name == "btnExpandCollapseSelected")
            {
                if (isExpandPanel)
                {
                    ExpandSelectionTab();
                }
                else
                {
                    ExpandSearchTab();
                }
            }
            log.LogMethodExit();
        }
        private void ExpandSearchTab()
        {
            log.LogMethodEntry();
            this.pnlSelectedCustHeader.Location = new Point(this.pnlSelectedCustHeader.Location.X, (this.btnGetWaiverCodeCustomer.Location.Y - this.pnlSelectedCustHeader.Height - 12));
            this.pnlCustSearchTab.Show();
            //this.btnClearSearchFilter.Visible = true;
            //this.btnSearchCustomer.Visible = true;
            this.btnLatestSignedCustomers.Visible = true;
            this.pnlSelectedCustTab.Hide();
            this.pbxRemove.Visible = false;
            this.chkSelectAll.Visible = false;
            this.btnExpandCollapseSearch.Tag = EXPANDPANEL;
            this.btnExpandCollapseSearch.Image = Properties.Resources.CollapseArrow;
            this.btnExpandCollapseSelected.Tag = COLLAPSEPANEL;
            this.btnExpandCollapseSelected.Image = Properties.Resources.ExpandArrow;
            log.LogMethodExit();
        }
        private void ExpandSelectionTab()
        {
            log.LogMethodEntry();
            this.pnlCustSearchTab.Hide();
            //this.btnClearSearchFilter.Visible = false;
            //this.btnSearchCustomer.Visible = false;
            this.btnLatestSignedCustomers.Visible = false;
            this.pnlSelectedCustHeader.Location = new Point(this.pnlSelectedCustHeader.Location.X, this.pnlCustSearchHeader.Location.Y + this.pnlCustSearchHeader.Height + 1);
            this.pnlSelectedCustTab.Show();
            this.pbxRemove.Visible = true;
            this.chkSelectAll.Visible = true;
            this.btnExpandCollapseSearch.Tag = COLLAPSEPANEL;
            this.btnExpandCollapseSearch.Image = Properties.Resources.ExpandArrow;
            this.btnExpandCollapseSelected.Tag = EXPANDPANEL;
            this.btnExpandCollapseSelected.Image = Properties.Resources.CollapseArrow;
            log.LogMethodExit();
        }
        private void LoadLatestWaiverSignedCustomers()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (selectedCustomerIdList == null || selectedCustomerIdList.Any() == false)
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "WAIVER_CODE_IS_MANDATORY_TO_FETCH_CUSTOMER", false) == false)
                {
                    PerformCustomerSearch(true);
                    DisplayMessage(MessageContainerList.GetMessage(executionContext, 4091), MAPWAIVER);
                }
            } 
            log.LogMethodExit();
        }
        private void SetCueForSearchTextBoxes()
        {
            log.LogMethodEntry();
            try
            {
                this.txtFirstNameSearch.Cue = MessageContainerList.GetMessage(executionContext, "First Name");
                this.txtLastNameSearch.Cue = MessageContainerList.GetMessage(executionContext, "Last Name");
                this.txtEmailSearch.Cue = MessageContainerList.GetMessage(executionContext, "Email Id");
                this.txtPhoneSearch.Cue = MessageContainerList.GetMessage(executionContext, "Phone No");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void btnClearSearchFilter_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    ClearSearchFilters();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void ClearSearchFilters()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            txtFirstNameSearch.Clear();
            txtLastNameSearch.Clear();
            txtEmailSearch.Clear();
            txtPhoneSearch.Clear();
            //btnLatestSignedCustomers.Checked = false;
            cbxGetRelatedCustomers.Checked = false;
            cmbChannels.SelectedIndex = 0;
            ResetSearchOptionButtons();
            searchedCustomerDTOList = new List<CustomerDTO>();
            fpnlCustomerSearch.Controls.Clear();
            log.LogMethodExit();
        }
        private void pbxRemove_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StringBuilder errMsgs = new StringBuilder();
            try
            {
                txtMessage.Clear();
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    List<CustomerDTO> removeSelectedCustomerDTOList = GetSelectedCustomerDTOList();
                    if (removeSelectedCustomerDTOList != null && removeSelectedCustomerDTOList.Any())
                    {
                        for (int i = 0; i < removeSelectedCustomerDTOList.Count; i++)
                        {
                            try
                            {
                                if (this.selectedCustomerDTOList != null && this.selectedCustomerDTOList.Any())
                                {
                                    CustomerDTO custDTO = selectedCustomerDTOList.Find(cust => cust.Id == removeSelectedCustomerDTOList[i].Id);
                                    if (custDTO != null)
                                    {
                                        CanRemoveCustomerFromPanel(custDTO);
                                        selectedCustomerDTOList.Remove(custDTO);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                errMsgs.Append(ex.Message);
                                //errMsgs.Append(Environment.NewLine);
                            }
                        }
                    }
                    else
                    {
                        errMsgs.Append(MessageContainerList.GetMessage(executionContext, 2305));
                        //Please select a customer record to proceed
                    }
                    if (errMsgs != null && errMsgs.Length > 1)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, errMsgs.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MessageContainerList.GetMessage(executionContext, "Remove"));
                DisplayMessage(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MessageContainerList.GetMessage(executionContext, "Remove"));
            }
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    RefreshCustomerPanel();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void CanRemoveCustomerFromPanel(CustomerDTO custDTO)
        {
            log.LogMethodEntry((custDTO != null ? custDTO.Id : -1));
            POSUtils.SetLastActivityDateTime();
            if (this.transaction != null && custDTO.Id > -1)
            {
                CustomerIsNotGuestCustomer(custDTO);
                DoesNotBelongToAttendeeList(custDTO);
                if (this.transaction.customerDTO != null)
                {
                    DoesNotBelongToTrxCustomer(this.transaction.customerDTO, custDTO);
                }
                if (this.transaction.PrimaryCard != null && this.transaction.PrimaryCard.customerDTO != null)
                {
                    DoesNotBelongToTrxCustomer(this.transaction.PrimaryCard.customerDTO, custDTO);
                }
                DoesNotBelongToWaiverMappedCustomers(custDTO);
            }
            log.LogMethodExit();
        }
        private void CustomerIsNotGuestCustomer(CustomerDTO custDTO)
        {
            log.LogMethodEntry(custDTO.Id);
            POSUtils.SetLastActivityDateTime();
            if (guestCustomerId > -1 && guestCustomerId == custDTO.Id)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4067, custDTO.FirstName));
                //"Cannot remove Guest Customer - &1"
            }
            log.LogMethodExit();
        }
        private void DoesNotBelongToTrxCustomer(CustomerDTO trxCustDTO, CustomerDTO custDTO)
        {
            log.LogMethodEntry(trxCustDTO.Id, custDTO.Id);
            if (trxCustDTO.Id == custDTO.Id)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4068, custDTO.FirstName));
                //"Customer is linked to transaction. Cannot remove - &1"
            }
            log.LogMethodExit();
        }
        private void DoesNotBelongToAttendeeList(CustomerDTO custDTO)
        {
            log.LogMethodEntry(custDTO.Id);
            POSUtils.SetLastActivityDateTime();
            if (this.transaction != null && this.transaction.HasActiveAttendeeList())
            {
                List<int> tempIdList = transaction.GetAttendeeCustomerIds();
                if (tempIdList != null && tempIdList.Any())
                {
                    if (tempIdList.Exists(custId => custId == custDTO.Id))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4069, custDTO.FirstName));
                        //"Customer is mapped as transaction attendee. Cannot remove - &1"
                    }
                }
            }
            log.LogMethodExit();
        }
        private void DoesNotBelongToWaiverMappedCustomers(CustomerDTO custDTO)
        {
            log.LogMethodEntry(custDTO.Id);
            List<int> custIdList = transaction.GetMappedCustomerIdListForWaiver();
            if (custIdList == null)
            {
                custIdList = new List<int>();
            }
            List<int> unMappedCustIdList = GetUnSignedButMappedCustomers();
            if (unMappedCustIdList != null && unMappedCustIdList.Any())
            {
                custIdList.AddRange(unMappedCustIdList);
            }
            if (custIdList != null && custIdList.Any())
            {
                if (custIdList.Exists(custId => custId == custDTO.Id))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4092, custDTO.FirstName));
                    //"Customer is mapped to waiver required item on the transaction. Cannot remove - &1"
                }
            }
            log.LogMethodExit();
        }
        private List<int> GetUnSignedButMappedCustomers()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<int> custIdList = new List<int>();
            List<FlowLayoutPanel> productPanelList = fPnlProductWaiverMap.Controls.OfType<FlowLayoutPanel>().ToList();
            if (productPanelList != null)
            {
                foreach (FlowLayoutPanel productPanel in productPanelList)
                {
                    List<Panel> productTrxLinePanelList = productPanel.Controls.OfType<Panel>().ToList();
                    if (productTrxLinePanelList != null)
                    {
                        foreach (Panel trxLinePanel in productTrxLinePanelList)
                        {
                            if (trxLinePanel.Tag != null)
                            {
                                //PictureBox pbxSigned = trxLinePanel.Controls.OfType<PictureBox>().ToList()[0];
                                //if (pbxSigned != null && pbxSigned.Tag != null && pbxSigned.Tag.ToString() == "No")
                                {
                                    KeyValuePair<int, int> lineCustomerIdPair = (KeyValuePair<int, int>)trxLinePanel.Tag;
                                    if (custIdList.Exists(id => id == lineCustomerIdPair.Value) == false)
                                    {
                                        custIdList.Add(lineCustomerIdPair.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(custIdList);
            return custIdList;
        }
        private void RefreshSearchPanelUsrCtlCustomer(CustomerDTO updatedCustomerDTO)
        {
            log.LogMethodEntry(updatedCustomerDTO);
            POSUtils.SetLastActivityDateTime();
            List<usrCtlCustomer> customerUsrControlList = fpnlCustomerSearch.Controls.OfType<usrCtlCustomer>().ToList();
            if (customerUsrControlList != null)
            {
                foreach (usrCtlCustomer item in customerUsrControlList)
                {
                    if (item.CustomerDTO != null && item.CustomerDTO.Id == updatedCustomerDTO.Id)
                    {
                        item.CustomerDTO = updatedCustomerDTO;
                        item.SetDisplayElements();
                        break;
                    }
                }
                vScrollBarCustomerSearch.UpdateButtonStatus();
            }
            log.LogMethodExit();
        }
        private void ResetSearchPanelUsrCtlCustomer(int customerId)
        {
            log.LogMethodEntry(customerId);
            POSUtils.SetLastActivityDateTime();
            List<usrCtlCustomer> customerUsrControlList = fpnlCustomerSearch.Controls.OfType<usrCtlCustomer>().ToList();
            if (customerUsrControlList != null)
            {
                foreach (usrCtlCustomer item in customerUsrControlList)
                {
                    if (item.CustomerDTO != null && item.CustomerDTO.Id == customerId)
                    {
                        item.SetDisplayElements();
                        item.IsSelected = false;
                        break;
                    }
                }
                vScrollBarCustomerSearch.UpdateButtonStatus();
            }
            log.LogMethodExit();
        }
        private void SetOverridePendingButtonText()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (TransactionHasWaiverOverridenLines())
            {
                btnOverridePending.Text = MessageContainerList.GetMessage(executionContext, "Reset Override");
                btnOverridePending.Tag = "R";
            }
            else
            {
                btnOverridePending.Text = MessageContainerList.GetMessage(executionContext, "Override Pending");
                btnOverridePending.Tag = "O";
            }
            log.LogMethodExit();
        }
        private bool TransactionHasWaiverOverridenLines()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            bool hasOverridenLines = false;
            if (this.transaction != null && this.transaction.TrxLines != null)
            {
                hasOverridenLines = this.transaction.TrxLines.Exists(tl => tl.LineValid && tl.WaiverSignedDTOList != null
                                                                           && tl.WaiverSignedDTOList.Exists(ws => ws.IsOverriden == true));
            }
            log.LogMethodExit(hasOverridenLines);
            return hasOverridenLines;
        }
        private bool RegisterBarcodeScanner()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (barcodeScanner != null)
            {
                barcodeScanner.Dispose();
            }
            List<Device> deviceList = new List<Device>();
            PeripheralsListBL peripheralsListBL = new PeripheralsListBL(executionContext);
            List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchPeripheralsParams = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_TYPE, "BarcodeReader"));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, (executionContext.GetMachineId()).ToString()));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.ACTIVE, "1"));
            List<PeripheralsDTO> peripheralsDTOList = peripheralsListBL.GetPeripheralsDTOList(searchPeripheralsParams);
            if (peripheralsDTOList != null && peripheralsDTOList.Count > 0)
            {
                foreach (PeripheralsDTO peripheralsList in peripheralsDTOList)
                {
                    if (peripheralsList.Vid.ToString().Trim() == string.Empty)
                        continue;
                    Device device = new Device();

                    device.DeviceName = peripheralsList.DeviceName.ToString();
                    device.DeviceType = peripheralsList.DeviceType.ToString();
                    device.DeviceSubType = peripheralsList.DeviceSubType.ToString();
                    device.VID = peripheralsList.Vid.ToString().Trim();
                    device.PID = peripheralsList.Pid.ToString().Trim();
                    device.OptString = peripheralsList.OptionalString.ToString().Trim();
                    deviceList.Add(device);
                }
            }

            string USBReaderVID = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USB_BARCODE_READER_VID");
            string USBReaderPID = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USB_BARCODE_READER_PID");
            string USBReaderOptionalString = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USB_BARCODE_READER_OPT_STRING");

            if (USBReaderVID.Trim() != string.Empty)
            {
                string[] optStrings = USBReaderOptionalString.Split('|');
                foreach (string optValue in optStrings)
                {
                    Device device = new Device();
                    device.DeviceName = "Default";
                    device.DeviceType = "BarcodeReader";
                    device.DeviceSubType = "KeyboardWedge";
                    device.VID = USBReaderVID.Trim();
                    device.PID = USBReaderPID.Trim();
                    device.OptString = optValue.ToString();
                    deviceList.Add(device);
                }
            }

            USBDevice barcodeListener;
            if (IntPtr.Size == 4) //32 bit
                barcodeListener = new KeyboardWedge32();
            else
                barcodeListener = new KeyboardWedge64();

            if (deviceList.Any())
            {
                for (int i = 0; i < deviceList.Count; i++)
                {
                    Device deviceSelected = deviceList[i];
                    try
                    {
                        bool flag = barcodeListener.InitializeUSBReader(this, deviceSelected.VID, deviceSelected.PID, deviceSelected.OptString.Trim());
                        if (barcodeListener.isOpen)
                        {
                            barcodeScanner = barcodeListener;
                            if (barcodeScanner != null)
                            {
                                barcodeScanner.Register(new EventHandler(BarCodeScanCompleteEventHandle));
                            }
                            log.Info("USB Bar Code scanner is added");
                            log.LogMethodExit(true);
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.Info("Unable to find USB Bar Code scanner");
            log.LogMethodExit(false);
            return false;
        }
        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs && utilities != null)
            {
                try
                {
                    POSUtils.SetLastActivityDateTime();
                    DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                    string scannedBarcode = utilities.ProcessScannedBarCode(checkScannedEvent.Message,
                                            ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "LEFT_TRIM_BARCODE", 0),
                                            ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "RIGHT_TRIM_BARCODE", 0));
                    this.Invoke((MethodInvoker)delegate
                    {
                        ProcessBarcode(scannedBarcode);
                    });
                }
                catch (Exception ex)
                {
                    //DisplayMessageLine(ex.Message);
                    log.Error(ex);
                    DisplayMessage(MessageContainerList.GetMessage(executionContext, ex.Message), MessageContainerList.GetMessage(executionContext, "Barcode Scan"));
                }
            }
            log.LogMethodExit();
        }
        private void ProcessBarcode(string barCode)
        {
            log.LogMethodEntry(barCode);
            POSUtils.SetLastActivityDateTime();
            if (string.IsNullOrWhiteSpace(barCode) == false)
            {
                txtMessage.Clear();
                txtWaiverCode.Text = barCode;
                btnExpandCollapseSelected.Tag = COLLAPSEPANEL;
                CollapseExpandPanels(btnExpandCollapseSelected);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void BuildChannelList()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<string> channelList = new List<string>();
            channelList.Add(WaiverSignatureDTO.WaiverSignatureChannel.NONE.ToString());
            channelList.Add(WaiverSignatureDTO.WaiverSignatureChannel.POS.ToString());
            channelList.Add(WaiverSignatureDTO.WaiverSignatureChannel.KIOSK.ToString());
            channelList.Add(WaiverSignatureDTO.WaiverSignatureChannel.TABLET.ToString());
            channelList.Add(WaiverSignatureDTO.WaiverSignatureChannel.WEBSITE.ToString());
            cmbChannels.DataSource = channelList;
            cmbChannels.SelectedIndex = 0;
            log.LogMethodExit();
        }
        private List<CustomerDTO> GetReletedCustomerDTO(List<CustomerDTO> searchedCustomerDTOList)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<CustomerDTO> relatedCustomerDTOList = new List<CustomerDTO>();
            List<int> custIdList = searchedCustomerDTOList.Select(cDTO => cDTO.Id).ToList();
            List<CustomerRelationshipDTO> customerRelationshipDTOList = GetCustomerRelations(custIdList);
            if (customerRelationshipDTOList != null && customerRelationshipDTOList.Any())
            {
                for (int i = 0; i < customerRelationshipDTOList.Count; i++)
                {
                    CustomerRelationshipDTO custRelDTO = customerRelationshipDTOList[i];
                    if (custIdList.Exists(id => id == custRelDTO.CustomerDTO.Id) == false
                        && relatedCustomerDTOList.Exists(cDTO => cDTO.Id == custRelDTO.CustomerDTO.Id) == false)
                    {
                        relatedCustomerDTOList.Add(custRelDTO.CustomerDTO);
                    }
                    if (custIdList.Exists(id => id == custRelDTO.RelatedCustomerDTO.Id) == false
                        && relatedCustomerDTOList.Exists(cDTO => cDTO.Id == custRelDTO.RelatedCustomerDTO.Id) == false)
                    {
                        relatedCustomerDTOList.Add(custRelDTO.RelatedCustomerDTO);
                    }
                }
            }
            log.LogMethodExit();
            return relatedCustomerDTOList;
        }
        private List<CustomerRelationshipDTO> GetCustomerRelations(List<int> customerIdList)
        {
            log.LogMethodEntry(customerIdList);
            POSUtils.SetLastActivityDateTime();
            CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(executionContext);
            List<CustomerRelationshipDTO> tempRelDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(customerIdList, true, true);
            log.LogMethodExit(tempRelDTOList);
            return tempRelDTOList;
        }
        private void GetMaxRecordDisplayCount()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<LookupValuesDTO> lookupValuesDTOList = WaiverCustomerUtils.GetWaiverSetupLookupValues(executionContext);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
            {
                for (int i = 0; i < lookupValuesDTOList.Count; i++)
                {
                    if (lookupValuesDTOList[i].LookupValue == WaiverCustomerUtils.MAX_COUNT_FOR_POS_MAP_UI_CUST_SEARCH)
                    {
                        try
                        {
                            maxDisplayRecordCount = Convert.ToInt32(lookupValuesDTOList[i].Description);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            maxDisplayRecordCount = 50;
                        }
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }
        private void btnLatestSignedCustomers_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                if (IsBackgroundJobRunning() == false)
                {
                    ClearSearchFilters();
                    PerformCustomerSearch(true);
                    DisplayMessage(MessageContainerList.GetMessage(executionContext, 4091), MAPWAIVER);
                    ShowSearchAlerts();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MessageContainerList.GetMessage(executionContext, "Search"));
                DisplayMessage(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), CUSTSEARCH);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void RefreshUIForSearchCustomer(CustomerDTO customerDTO)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                if (searchedCustomerDTOList != null && searchedCustomerDTOList.Any() && searchedCustomerDTOList.Exists(cDTO => cDTO.Id == customerDTO.Id))
                {
                    List<int> customerIdList = new List<int>();
                    customerIdList.Add(customerDTO.Id);
                    List<CustomerRelationshipDTO> customerRelationshipDTOList = GetCustomerRelations(customerDTO.Id);
                    if (customerRelationshipDTOList != null && customerRelationshipDTOList.Any())
                    {
                        List<int> relateCustomerIdList = customerRelationshipDTOList.Where(rCust => rCust.IsActive == true && rCust.RelatedCustomerId > -1).Select(rCust => rCust.RelatedCustomerId).Distinct().ToList();
                        if (relateCustomerIdList != null && relateCustomerIdList.Any())
                        {
                            customerIdList.AddRange(relateCustomerIdList);
                        }
                    }
                    CustomerListBL customerListBL = new CustomerListBL(executionContext);
                    List<CustomerDTO> custDTOList = customerListBL.GetCustomerDTOList(customerIdList, true, true, true);
                    if (custDTOList != null && custDTOList.Any())
                    {
                        for (int i = 0; i < custDTOList.Count; i++)
                        {
                            CustomerDTO relatedCustDTO = searchedCustomerDTOList.Find(cust => cust.Id == custDTOList[i].Id);
                            if (relatedCustDTO != null)
                            {
                                int lineIndex = searchedCustomerDTOList.IndexOf(relatedCustDTO);
                                if (lineIndex > -1)
                                {
                                    searchedCustomerDTOList[lineIndex] = custDTOList[i];
                                    RefreshSearchPanelUsrCtlCustomer(custDTOList[i]);
                                    RefreshProductWaiverMapping(searchedCustomerDTOList[lineIndex]);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                vScrollBarCustomerSearch.UpdateButtonStatus();
                vScrollBarProductCustomerMap.UpdateButtonStatus();
                hScrollBarProductCustomerMap.UpdateButtonStatus();
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }
        private void SetLiterals()
        {
            log.LogMethodEntry();
            MAPWAIVER = MessageContainerList.GetMessage(executionContext, "Map Waiver");
            SIGNWAIVER = MessageContainerList.GetMessage(executionContext, "Sign Waiver");
            CUSTSEARCH = MessageContainerList.GetMessage(executionContext, "Search");
            TAPCARD = MessageContainerList.GetMessage(executionContext, "Tap Card");
            WAIVERCODE = MessageContainerList.GetMessage(executionContext, "Waiver Code");
            NEWCUSTOMER = MessageContainerList.GetMessage(executionContext, "New Customer");
            CUSTOMERLOOKUP = MessageContainerList.GetMessage(executionContext, "Customer Lookup");
            OVERRIDEWAIVERS = MessageContainerList.GetMessage(executionContext, "Override");
            RESETOVERRIDEWAIVERS = MessageContainerList.GetMessage(executionContext, "Reset Override");
            log.LogMethodExit();
        }
        private void DisplayMessage(string messageText, string messageHeader = "", bool showPopup = false)
        {
            log.LogMethodEntry();
            txtMessage.Text = messageText;
            if (showPopup || (string.IsNullOrWhiteSpace(messageText) == false && ((messageText.Contains(Environment.NewLine) && messageText.Length > 180)
                                             || messageText.Length > 180)))
            {
                POSUtils.ParafaitMessageBox(messageText, (string.IsNullOrWhiteSpace(messageHeader) == false
                                                         ? messageHeader : MessageContainerList.GetMessage(executionContext, "Map Waiver")));

            }
            log.LogMethodExit();
        }

        private void btnPhoneSearchOption_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                Button btnClicked = (Button)sender;
                ToggleSearchOptionButtonTag(btnClicked);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnEmailSearchOption_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                Button btnClicked = (Button)sender;
                ToggleSearchOptionButtonTag(btnClicked);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void ToggleSearchOptionButtonTag(Button btnClicked)
        {
            log.LogMethodEntry();
            if (btnClicked != null)
            {
                if (btnClicked.Tag == null || btnClicked.Tag.ToString() == "L")
                {
                    btnClicked.Tag = EXACT_SEARCH;
                    btnClicked.BackgroundImage = Properties.Resources.BlueToggleOn;
                    btnClicked.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Exact");
                    btnClicked.TextAlign = ContentAlignment.MiddleLeft;
                }
                else
                {
                    btnClicked.Tag = LIKE_SEARCH;
                    btnClicked.BackgroundImage = Properties.Resources.BlueToggleOff;
                    btnClicked.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Like");
                    btnClicked.TextAlign = ContentAlignment.MiddleRight;
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void ResetSearchOptionButtons()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                btnPhoneSearchOption.Tag = LIKE_SEARCH;
                ToggleSearchOptionButtonTag(btnPhoneSearchOption);
                btnEmailSearchOption.Tag = LIKE_SEARCH;
                ToggleSearchOptionButtonTag(btnEmailSearchOption);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void IconObject_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                PictureBox pictureBox = sender as PictureBox;
                if (pictureBox != null && pictureBox.Tag != null)
                {
                    ToolTip iconToolTip = new ToolTip();
                    string msg = MessageContainerList.GetMessage(executionContext, pictureBox.Tag.ToString());
                    iconToolTip.SetToolTip(pictureBox, msg);
                    int screenX = pnlCustSearchTab.Location.X + pictureBox.Location.X + 2;
                    int screenY = pnlCustSearchTab.Location.Y + pictureBox.Location.Y + 2;
                    iconToolTip.Show(msg, this, new Point(screenX + 10, screenY), 1000);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void IconObjectMouseEnter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.Hand;
            log.LogMethodExit();
        }

        private void IconObjectMouseHover(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.Hand;
            log.LogMethodExit();
        }
    }
}

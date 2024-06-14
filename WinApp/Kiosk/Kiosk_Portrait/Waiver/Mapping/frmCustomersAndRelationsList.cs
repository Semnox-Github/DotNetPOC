/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - Waiver Mapping
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.151.2    29-Dec-2023      Sathyavathi        Created for Waiver Mapping Enhancement
 ********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;

namespace Parafait_Kiosk
{
    public partial class frmCustomersAndRelationsList : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;
        private Semnox.Parafait.Transaction.Transaction.TransactionLine trxLineinProgress;
        private CustomerDTO signatoryCustomerDTO;
        private CustomerDTO signForcustomerDTO;
        private List<int> customersIdList;
        private List<CustomerRelationshipDTO> customerRelationshipDTOs;
        private UsrCtrlCustomersAndRelationsList previouslySelectedUsrCtrl;
        private bool isCustomerMandatory = false;
        private List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList = new List<CustomerRelationshipTypeDTO>();

        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        public CustomerDTO SignatoryCustomerDTO { get { return signatoryCustomerDTO; } }
        public CustomerDTO SignForcustomerDTO { get { return signForcustomerDTO; } }
        private int guestCustomerId;

        public frmCustomersAndRelationsList(KioskTransaction kioskTransaction, Semnox.Parafait.Transaction.Transaction.TransactionLine line)
        {
            log.LogMethodEntry("kioskTransaction", line);
            KioskStatic.logToFile("In frmCustomersAndRelationsList()");
            this.kioskTransaction = kioskTransaction;
            this.trxLineinProgress = line;
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetKioskTimerTickValue(30);
            DisplaybtnCancel(false);
            DisplaybtnPrev(true);
            KioskStatic.setDefaultFont(this);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            KioskStatic.Utilities.setLanguage(this);
            guestCustomerId = CustomerListBL.GetGuestCustomerId(KioskStatic.Utilities.ExecutionContext);
            SetDisplayElements();
            log.LogMethodExit();
        }

        private void frmCustomersAndRelationsList_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                LoadRelationShipTypes();
                int count = GetCountOfCustomerIds();
                if (count == 0)
                {
                    isCustomerMandatory = true;
                    btnSearchAnotherCustomer.PerformClick();
                    log.LogMethodExit();
                    Close();
                }
                else
                {
                    RefreshCustomersList();
                    SetDefaultExpandCollapseButtons();
                }
            }
            catch (CustomerStatic.TimeoutOccurred ex)
            {
                KioskStatic.logToFile("Timeout occured");
                log.Error(ex);
                PerformTimeoutAbortAction(kioskTransaction, null);
                this.DialogResult = DialogResult.Cancel;
                log.LogMethodExit();
                return;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in frmCustomersAndRelationsList_Load()" + ex.Message);
            }
            log.LogMethodExit();
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                KioskStatic.logToFile("Cancel Button Pressed : Triggering Home Button Action ");
                base.btnHome_Click(sender, e);
            }
            catch (Exception ex)
            {
                log.Error("Error in btnCancel_Click of frmCustomersAndRelationsList", ex);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            ExecuteProceedAction();
            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DisableButtons();
            try
            {
                DialogResult = DialogResult.No;
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in btnBack_Click() of frmCustomersAndRelationsList" + ex.Message);
            }
            finally
            {
                EnableButtons();
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }

        private void BtnAddNewMember_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1448);//Loading... Please wait...
                Application.DoEvents();
                DisableButtons();
                int parentId = -1;
                if (customersIdList.Count == 1)
                {
                    parentId = customersIdList[0];
                }
                else if (previouslySelectedUsrCtrl != null)
                {
                    parentId = previouslySelectedUsrCtrl.GroupOwner;
                }
                else
                {
                    //New member will always be added to the parent
                    using (frmGroupOwners frmFilteredCustomers = new frmGroupOwners(customersIdList, customerRelationshipDTOs))
                    {
                        DialogResult dr = frmFilteredCustomers.ShowDialog();
                        if (dr == DialogResult.OK)
                        {
                            parentId = frmFilteredCustomers.SelectedCustomerId;
                        }
                    }
                }
                if (parentId > -1)
                {
                    CustomerBL selectedCustomerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, parentId, true, true, null);
                    if (selectedCustomerBL.CustomerDTO != null)
                    {
                        CustomerDTO parent = selectedCustomerBL.CustomerDTO;
                        string parentName = parent.FirstName;
                        if (!string.IsNullOrWhiteSpace(parent.LastName))
                        {
                            parentName += " " + parent.LastName;
                        }
                        using (frmAddCustomerRelation frmAddCustomer = new frmAddCustomerRelation(parent.Id, parentName, parent.CardNumber))
                        {
                            DialogResult dr = frmAddCustomer.ShowDialog();
                            if (dr == DialogResult.OK)
                            {
                                RefreshCustomersList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errMsg = "Error occurred while executing BtnAddNewMember_Click() of frmCustomersAndRelationsList";
                log.Error(ex);
                KioskStatic.logToFile(errMsg + ex);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                txtMessage.Text = lblGreetingMsg.Text;
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private void btnSearchAnotherCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1448);//Loading... Please wait...
                Application.DoEvents();
                DisableButtons();
                CustomerDTO customer = CustomerStatic.GetCustomer(null, isCustomerMandatory);
                if (customer != null)
                {
                    if (!kioskTransaction.HasCustomerRecord())
                    {
                        kioskTransaction.SetTransactionCustomer(customer);
                    }

                    signForcustomerDTO = customer;
                    CustomerBL customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, customer); //customer is not fully loaded
                    bool isAdult = customerBL.IsAdult();
                    if (isAdult)
                    {
                        signatoryCustomerDTO = customer; //pick the first adult linked to sign for the minor
                    }
                    else
                    {
                        List<CustomerRelationshipDTO> relatedCustomers = GetRelatedCustomers(customer.Id);
                        if (relatedCustomers != null && relatedCustomers.Any())
                        {
                            signatoryCustomerDTO = relatedCustomers[0].CustomerDTO; //pick the first adult linked to sign for the minor
                        }
                    }
                }
                ExecuteProceedAction();
            }
            catch (Exception ex)
            {
                string errMsg = "Error occurred while executing BtnSearhAnotherCustomer_Click()";
                log.Error(errMsg, ex);
                KioskStatic.logToFile(errMsg + ex.Message);
            }
            finally
            {
                txtMessage.Text = lblGreetingMsg.Text;
                EnableButtons();
                ResetKioskTimer();
            }
        }

        private void ExecuteProceedAction()
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1448);//Loading... Please wait...
                Application.DoEvents();
                DisableButtons();
                if (signatoryCustomerDTO == null && signForcustomerDTO == null)
                {
                    //Please select a record to proceed
                    string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 2460);
                    throw new ValidationException(errMsg);
                }


                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("btnProceed_Click() in frmCustomersAndRelationsList : " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                txtMessage.Text = lblGreetingMsg.Text;
                EnableButtons();
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }

        private void bigVerticalScrollView_ButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error in bigVerticalScrollView_ButtonClick", ex);
            }
            log.LogMethodExit();
        }

        private void SetDisplayElements()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                SetOnScreenMessages();
                SetCustomImages();
                SetCustomizedFontColors();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetDisplayElements() of frmCustomersAndRelationsList" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetOnScreenMessages()
        {
            log.LogMethodEntry();

            //5464 - 'Selecting participant for &1'
            lblGreetingMsg.Text =
                txtMessage.Text = MessageContainerList.GetMessage(executionContext, 5464, trxLineinProgress.ProductName);
            btnProceed.Text = MessageContainerList.GetMessage(executionContext, "Assign");//Literal
            btnAddNewMember.Text = MessageContainerList.GetMessage(executionContext, "Add New Member");//Literal
            btnSearchAnotherCustomer.Text = MessageContainerList.GetMessage(executionContext, "Search/Add Customer");//Literal

            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();

            this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.SelectSlotBackgroundImage);
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnPrev.BackgroundImage =
                btnAddNewMember.BackgroundImage =
                btnSearchAnotherCustomer.BackgroundImage =
                btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            this.bigVerticalScrollView.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
            pnlCustomers.BackgroundImage = ThemeManager.CurrentThemeImages.PanelTimeSection;

            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            this.lblGreetingMsg.ForeColor = KioskStatic.CurrentTheme.CustomerRelationsGreetingTextForeColor;
            this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CustomerRelationsBackButtonTextForeColor;
            btnAddNewMember.ForeColor = KioskStatic.CurrentTheme.CustomerRelationsAddNewMemberButtonTextForeColor;
            btnSearchAnotherCustomer.ForeColor = KioskStatic.CurrentTheme.CustomerRelationsSearchAnotherCustomerButtonTextForeColor;
            this.btnProceed.ForeColor = KioskStatic.CurrentTheme.CustomerRelationsProceedButtonTextForeColor;
            this.txtMessage.ForeColor = KioskStatic.CurrentTheme.CustomerRelationsFooterTextForeColor;
            this.btnHome.ForeColor = KioskStatic.CurrentTheme.SelectSlotHomeButtonTextForeColor;
            log.LogMethodExit();
        }

        private int GetCountOfCustomerIds()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("in GetAllCustomerIds() of frmCustomersAndRelationsList");
            try
            {
                customersIdList = new List<int>();
                AddTrxAttendees();
                AddTrxCustomer();
                AddTrxLineCardCustomers();
                AddWaiverMappedCustomers();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Getting Customers Id");
            }
            log.LogMethodExit(customersIdList.Count);
            return customersIdList.Count;
        }

        private List<int> GetAllCustomerIds()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("in GetAllCustomerIds() of frmCustomersAndRelationsList");
            try
            {
                customersIdList = new List<int>();
                AddTrxAttendees();
                AddTrxCustomer();
                AddTrxLineCardCustomers();
                AddWaiverMappedCustomers();
                if (customersIdList.Count > 0)
                {
                    List<int> sortedIdList = SortCustomersAndPullRelationship();
                    if (sortedIdList.Count > 0)
                    {
                        customersIdList = new List<int>();
                        customersIdList = sortedIdList;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Getting Customers Id");
            }
            log.LogMethodExit(customersIdList);
            return customersIdList;
        }

        private void AddTrxLineCardCustomers()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine in kioskTransaction.GetActiveTransactionLines)
            {
                if (trxLine.card != null && trxLine.card.customer_id > -1)
                {
                    AddToCustomersIdList(trxLine.card.customer_id);
                }
            }
            log.LogMethodExit(customersIdList);
        }

        private void AddTrxCustomer()
        {
            log.LogMethodEntry();
            if (kioskTransaction.HasCustomerRecord())
            {
                AddToCustomersIdList(kioskTransaction.GetTransactionCustomer().Id);
            }
            log.LogMethodExit();
        }

        private void AddTrxAttendees()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (this.kioskTransaction.HasActiveAttendeeList())
            {
                List<int> tempIdList = kioskTransaction.GetAttendeeCustomerIds();
                if (tempIdList != null && tempIdList.Any())
                {
                    for (int i = 0; i < tempIdList.Count; i++)
                    {
                        AddToCustomersIdList(tempIdList[i]);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void AddWaiverMappedCustomers()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            List<int> custIdList = kioskTransaction.GetMappedCustomerIdListForWaiver();
            if (custIdList != null && custIdList.Any())
            {
                for (int i = 0; i < custIdList.Count; i++)
                {
                    AddToCustomersIdList(custIdList[i]);
                }
            }
            log.LogMethodExit();
        }

        private void AddToCustomersIdList(int customerId)
        {
            log.LogMethodEntry(customerId);
            ResetKioskTimer();
            if (customerId == guestCustomerId)
            {
                KioskStatic.logToFile("Warning: Guest Customer Found with id" + guestCustomerId + "skipping to add to the list");
                log.Info("Warning: Guest Customer Found with id" + guestCustomerId + "skipping to add to the list");
                log.LogMethodExit();
                return;
            }
            if (!customersIdList.Exists(custId => custId == customerId))
            {
                customersIdList.Add(customerId);
            }
            log.LogMethodExit();
        }

        private List<CustomerRelationshipDTO> GetRelatedCustomers(int customerId)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            List<CustomerRelationshipDTO> relatedCustomerDTOList = null;
            try
            {
                CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(KioskStatic.Utilities.ExecutionContext);
                List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
                relatedCustomerDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in GetRelatedCustomers of frmCustomersAndRelationsList" + ex.Message);
            }
            log.LogMethodExit(relatedCustomerDTOList);
            return relatedCustomerDTOList;
        }

        private List<CustomerRelationshipDTO> GetRelationsOfAllCustomerIds()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            List<CustomerRelationshipDTO> relatedCustomerDTOList = null;
            try
            {
                CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(KioskStatic.Utilities.ExecutionContext);
                relatedCustomerDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(customersIdList, true, true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in GetRelatedCustomers of frmCustomersAndRelationsList" + ex.Message);
            }
            log.LogMethodExit(relatedCustomerDTOList);
            return relatedCustomerDTOList;
        }

        private List<int> SortCustomersAndPullRelationship()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("in SortCustomersAndPullRelationship() of frmCustomersAndRelationsList");
            List<int> sortedCustomersIdList = new List<int>();
            try
            {
                customerRelationshipDTOs = new List<CustomerRelationshipDTO>();
                List<CustomerRelationshipDTO> allRelationsOfAllCustomerIds = GetRelationsOfAllCustomerIds();
                if (allRelationsOfAllCustomerIds != null && allRelationsOfAllCustomerIds.Any())
                {
                    foreach (int id in customersIdList.ToList())
                    {
                        if (allRelationsOfAllCustomerIds.Exists(cr => cr.CustomerId == id || cr.RelatedCustomerId == id))
                        {
                            foreach (CustomerRelationshipDTO relation in allRelationsOfAllCustomerIds)
                            {
                                if (id == relation.CustomerId)
                                {
                                    if (!sortedCustomersIdList.Exists(x => x == id))
                                    {
                                        sortedCustomersIdList.Add(id);
                                    }
                                }
                                else if (id == relation.RelatedCustomerId && !sortedCustomersIdList.Exists(x => x == relation.CustomerId))
                                {
                                    sortedCustomersIdList.Add(relation.CustomerId); //adding the parent/group owner
                                }
                            }
                        }
                        else
                        {
                            if (!sortedCustomersIdList.Exists(x => x == id))
                            {
                                sortedCustomersIdList.Add(id);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Getting sorted customers Id");
            }
            log.LogMethodExit(sortedCustomersIdList);
            return sortedCustomersIdList;
        }

        private void RefreshCustomersList()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                flpUsrCtrls.SuspendLayout();
                flpUsrCtrls.Controls.Clear();
                customersIdList = GetAllCustomerIds();
                customerRelationshipDTOs = GetRelationsOfAllCustomerIds();
                foreach (int id in customersIdList.ToList())
                {
                    CustomerBL customerBL = null;
                    CustomerRelationshipDTO cusRel = customerRelationshipDTOs == null ? null : customerRelationshipDTOs.FirstOrDefault(c => c.CustomerId == id);
                    if (cusRel != null && cusRel.CustomerDTO != null)
                    {
                        customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, cusRel.CustomerDTO);
                    }
                    else
                    {
                        customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, id, true, true, null);
                    }
                    if (customerBL != null && customerBL.CustomerDTO != null)
                    {
                        UsrCtrlCustomer usrCtrlCustomer = CreateUsrCtrlForParentRecord(customerBL);
                        flpUsrCtrls.Controls.Add(usrCtrlCustomer);

                        FlowLayoutPanel flpCustomers = CreateNewFLPToHoldCustomers();
                        flpUsrCtrls.Controls.Add(flpCustomers);
                        if (customersIdList.Count == 1)
                        {
                            usrCtrlCustomer.SetHideExpandCollapseButton(true);
                        }
                        //parent record should also be listed along with child records to allow guests to select the parent as participant
                        UsrCtrlCustomersAndRelationsList usrCtrlParentCustomer = CreateChildUsrCtrlForParentRecord(customerBL.CustomerDTO);
                        flpCustomers.Controls.Add(usrCtrlParentCustomer);

                        if (customerRelationshipDTOs != null && customerRelationshipDTOs.Any())
                        {
                            foreach (CustomerRelationshipDTO relation in customerRelationshipDTOs)
                            {
                                if (relation.CustomerId == id && relation.RelatedCustomerId > -1 && relation.RelatedCustomerId != guestCustomerId)
                                {
                                    UsrCtrlCustomersAndRelationsList usrCtrlCustomerRelations = CreateUsrCtrlForRelationships(relation);
                                    flpCustomers.Controls.Add(usrCtrlCustomerRelations);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in RefreshCustomersList()" + ex.Message);
            }
            finally
            {
                flpUsrCtrls.ResumeLayout();
            }
            log.LogMethodExit();
        }

        private UsrCtrlCustomersAndRelationsList CreateUsrCtrlForRelationships(CustomerRelationshipDTO relation)
        {
            log.LogMethodEntry(); log.LogMethodEntry();
            ResetKioskTimer();
            UsrCtrlCustomersAndRelationsList usrCtrlCustomerRelations = new UsrCtrlCustomersAndRelationsList(relation.RelatedCustomerDTO, customerRelationshipTypeDTOList,
                relation.CustomerRelationshipTypeId, trxLineinProgress, kioskTransaction.GetTrxDate(), relation.CustomerId);
            usrCtrlCustomerRelations.Tag = relation;
            usrCtrlCustomerRelations.selctedAttendee += new UsrCtrlCustomersAndRelationsList.SelectedDelegate(SelectedAttendee);
            log.LogMethodExit();
            return usrCtrlCustomerRelations;
        }

        private UsrCtrlCustomersAndRelationsList CreateChildUsrCtrlForParentRecord(CustomerDTO customerDTO)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            int relationshipIdOfParent = -1;
            UsrCtrlCustomersAndRelationsList usrCtrlParentCustomer = new UsrCtrlCustomersAndRelationsList(customerDTO, customerRelationshipTypeDTOList,
                relationshipIdOfParent, trxLineinProgress, kioskTransaction.GetTrxDate(), customerDTO.Id);
            usrCtrlParentCustomer.Tag = customerDTO;
            usrCtrlParentCustomer.selctedAttendee += new UsrCtrlCustomersAndRelationsList.SelectedDelegate(SelectedAttendee);
            log.LogMethodExit();
            return usrCtrlParentCustomer;
        }

        private void LoadRelationShipTypes()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            CustomerRelationshipTypeListBL customerRelationshipTypeListBL = new CustomerRelationshipTypeListBL(KioskStatic.Utilities.ExecutionContext);
            List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID, KioskStatic.Utilities.ExecutionContext.SiteId.ToString()));
            searchParameters.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
            customerRelationshipTypeDTOList = customerRelationshipTypeListBL.GetCustomerRelationshipTypeDTOList(searchParameters);
            customerRelationshipTypeDTOList.Insert(0, new CustomerRelationshipTypeDTO(-1, string.Empty, string.Empty, true));
            log.LogMethodExit(customerRelationshipTypeDTOList);
        }

        private UsrCtrlCustomer CreateUsrCtrlForParentRecord(CustomerBL customerBL)
        {
            string customerName = customerBL.CustomerDTO.FirstName;
            if (!string.IsNullOrWhiteSpace(customerBL.CustomerDTO.LastName))
            {
                customerName += " "; //add a space
                customerName += customerBL.CustomerDTO.LastName;
            }
            UsrCtrlCustomer usrCtrlCustomer = new UsrCtrlCustomer(customerName);
            usrCtrlCustomer.selctedParent += new UsrCtrlCustomer.Delegate(SelectedGroupOwner);
            return usrCtrlCustomer;
        }

        private void SetDefaultExpandCollapseButtons()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            for (int i = 0; i < flpUsrCtrls.Controls.Count; i++)
            {
                string type = flpUsrCtrls.Controls[i].GetType().ToString();
                if (type.Contains("UsrCtrlCustomer"))
                {
                    UsrCtrlCustomer usrCtrl = flpUsrCtrls.Controls[i] as UsrCtrlCustomer;
                    usrCtrl.Tag = UsrCtrlCustomer.EXPAND;
                    if (customersIdList.Count == 1)
                    {
                        usrCtrl.Tag = UsrCtrlCustomer.COLLAPSE;
                        HideExpandCollapseButton();
                    }
                    usrCtrl.TriggerExpandCollapse();
                }
            }
            log.LogMethodExit();
        }

        private void HideExpandCollapseButton()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            UsrCtrlCustomer usrCtrl = flpUsrCtrls.Controls[0] as UsrCtrlCustomer;
            usrCtrl.SetHideExpandCollapseButton(true);
            FlowLayoutPanel flpRelation = flpUsrCtrls.Controls[1] as FlowLayoutPanel;
            flpRelation.Show();

            log.LogMethodExit();
        }

        private void SelectedGroupOwner(UsrCtrlCustomer usrCtrl)
        {
            log.LogMethodEntry(usrCtrl);
            try
            {
                ResetKioskTimer();
                foreach (Control c in flpUsrCtrls.Controls)
                {
                    string ctrlType = c.GetType().ToString();
                    if (ctrlType.Contains("UsrCtrlCustomer") && (c == usrCtrl))
                    {
                        int indexOfCurrentItem = flpUsrCtrls.Controls.IndexOf(usrCtrl);
                        if (usrCtrl.Tag.Equals("COLLAPSE"))
                        {
                            flpUsrCtrls.Controls[indexOfCurrentItem + 1].Hide();
                        }
                        else
                        {
                            flpUsrCtrls.Controls[indexOfCurrentItem + 1].Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR in SelectedParent() of frmCustomersAndRelationsList" + ex.Message);
            }
            log.LogMethodExit();
        }

        private FlowLayoutPanel CreateNewFLPToHoldCustomers()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            FlowLayoutPanel flpCustomers = new FlowLayoutPanel();
            flpCustomers.AutoScroll = false;
            flpCustomers.BackColor = System.Drawing.Color.Transparent;
            flpCustomers.Location = new System.Drawing.Point(30, 96);
            flpCustomers.Name = "flpCustomers";
            flpCustomers.AutoSize = true;
            flpCustomers.AutoSizeMode = AutoSizeMode.GrowOnly;
            flpCustomers.TabIndex = 0;

            log.LogMethodExit(flpCustomers);
            return flpCustomers;
        }

        public void SelectedAttendee(UsrCtrlCustomersAndRelationsList usrCtrl)
        {
            log.LogMethodEntry(usrCtrl);
            try
            {
                ResetKioskTimer();
                UsrCtrlCustomersAndRelationsList selectedUsrCtrl = usrCtrl;

                if (previouslySelectedUsrCtrl == selectedUsrCtrl && selectedUsrCtrl.IsSelected)
                {
                    selectedUsrCtrl.IsSelected = false;
                    signatoryCustomerDTO = null;
                    signForcustomerDTO = null;
                }
                else
                {
                    foreach (Control c in flpUsrCtrls.Controls)
                    {
                        string ctrlType = c.GetType().ToString();
                        if (ctrlType.Contains("FlowLayoutPanel"))
                        {
                            foreach (UsrCtrlCustomersAndRelationsList userControl in c.Controls)
                            {
                                userControl.IsSelected = false;
                            }
                        }
                    }

                    foreach (Control c in flpUsrCtrls.Controls)
                    {
                        string ctrlType = c.GetType().ToString();
                        if (ctrlType.Contains("FlowLayoutPanel"))
                        {
                            foreach (UsrCtrlCustomersAndRelationsList userControl in c.Controls)
                            {
                                if (userControl.Tag == selectedUsrCtrl.Tag)
                                {
                                    previouslySelectedUsrCtrl = userControl;
                                    userControl.IsSelected = true;
                                    break;
                                }
                            }
                        }
                    }

                    signForcustomerDTO = null;
                    signForcustomerDTO = null;
                    string tagType = selectedUsrCtrl.Tag.GetType().ToString();
                    if (tagType.Equals("Semnox.Parafait.Customer.CustomerDTO")) //parent himself is the attendee
                    {
                        CustomerDTO parent = (CustomerDTO)selectedUsrCtrl.Tag;
                        signatoryCustomerDTO = parent;
                        signForcustomerDTO = parent;
                    }
                    else //relation is the attendee
                    {
                        Semnox.Parafait.Customer.CustomerRelationshipDTO relation = (Semnox.Parafait.Customer.CustomerRelationshipDTO)selectedUsrCtrl.Tag;
                        CustomerBL customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, relation.RelatedCustomerDTO.Id);
                        bool isAdult = customerBL.IsAdult();
                        signatoryCustomerDTO = isAdult ? relation.RelatedCustomerDTO : relation.CustomerDTO;
                        signForcustomerDTO = relation.RelatedCustomerDTO;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void DisableButtons()
        {
            log.LogMethodEntry();
            pnlCustomers.SuspendLayout();
            this.btnProceed.Enabled = false;
            this.btnPrev.Enabled = false;
            this.btnAddNewMember.Enabled = false;
            this.btnSearchAnotherCustomer.Enabled = false;
            log.LogMethodExit();
        }

        private void EnableButtons()
        {
            log.LogMethodEntry();

            pnlCustomers.ResumeLayout();
            this.btnProceed.Enabled = true;
            this.btnPrev.Enabled = true;
            this.btnAddNewMember.Enabled = true;
            this.btnSearchAnotherCustomer.Enabled = true;
            log.LogMethodExit();
        }

        private void frmCustomersAndRelationsList_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmCustomersAndRelationsList_FormClosed()", ex);
            }
            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - Parafait_POS.Waivers
 * Description  - Customer Waiver UI form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.80.0      26-Sep-2019   Guru S A                Created for Waiver phase 2 enhancement changes  
 *2.100       19-Oct-2020   Guru S A                Enabling minor signature option for waiver
 *2.140.0     14-Sep-2021   Guru S A                Waiver mapping UI enhancements
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.Device.Peripherals;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Parafait_POS.Waivers
{
    public partial class frmCustomerWaiverUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomerDTO signedBycustomerDTO;
        private CustomerBL selectedCustomerBL;
        private CustomerBL signatoryCustomerBL;
        private Transaction trx;
        private int guestCustomerId;
        private List<WaiverSetDTO> waiverSetDTOList;
        private List<CustomerRelationshipDTO> relatedCustomerDTOList;
        private List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList;
        private ExecutionContext executionContext = null;
        private Utilities utilities = null;
        private MessageBoxDelegate messageBoxDelegate;
        private const string RELATIONSHIPTYPEISCHILD = "CHILD";
        private string SIGNATURECHANNEL = WaiverSignatureDTO.WaiverSignatureChannel.POS.ToString();
        private WaiverSetDTO selectedWaiverSetDTO;
        private List<CustomerDTO> signForCustomerDTOList;
        private List<WaiverCustomerAndSignatureDTO> selectedWaiverAndCustomerSignatureList;
        private List<WaiverCustomerAndSignatureDTO> deviceSignatureList;
        private List<WaiverCustomerAndSignatureDTO> manualSignatureList;
        private List<KeyValuePair<int, int>> managerApprovalList;
        private LookupValuesList serverTimeObj;
        private frmStatus frmstatus;
        private List<WaiverSetDTO> waiverSetDTOListForCombBox;
        private CustomCheckBox cbxHeaderSelectRecord;
        public frmCustomerWaiverUI(Utilities utils, CustomerDTO customerDTO, List<WaiverSetDTO> waiverSetDTOList, MessageBoxDelegate messageBoxDelegate, Transaction trx = null)
        {
            log.LogMethodEntry(customerDTO, waiverSetDTOList, trx);
            InitializeComponent();
            this.utilities = utils;
            this.signedBycustomerDTO = customerDTO;
            signatoryCustomerBL = new CustomerBL(utilities.ExecutionContext, signedBycustomerDTO);
            this.waiverSetDTOList = waiverSetDTOList;
            this.messageBoxDelegate = messageBoxDelegate;
            this.trx = trx;
            SetExecutionContext();
            this.guestCustomerId = CustomerListBL.GetGuestCustomerId(executionContext);
            serverTimeObj = new LookupValuesList(executionContext);
            utilities.setupDataGridProperties(ref this.dgvCustomers);
            utilities.setupDataGridProperties(ref this.dgvCustomerSignedWaivers);
            this.dgvCustomerSignedWaivers.BackgroundColor = this.dgvCustomers.BackgroundColor;
            SetDGVCellFont(dgvCustomers);
            dgvCustomers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvCustomers.DefaultCellStyle.SelectionBackColor = Color.Transparent;
            dgvCustomers.DefaultCellStyle.SelectionForeColor = Color.Transparent;
            dgvCustomers.RowTemplate.Height = dgvCustomerSignedWaivers.RowTemplate.Height = 33;
            this.dgvCustomers.RowHeadersVisible = false;
            log.LogMethodExit();
        }

        private void SetExecutionContext()
        {
            log.LogMethodEntry();
            executionContext = utilities.ExecutionContext;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                executionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                executionContext.SetSiteId(-1);
            }
            executionContext.SetMachineId(utilities.ParafaitEnv.POSMachineId);
            log.LogMethodExit();
        }

        private void frmCustomerWaiverUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadUsers();
            LoadRelationShipTypes();
            DisplayCustomerDetails();
            SelectSingleCustomer();
            selectedCustomerBL = new CustomerBL(executionContext, this.signedBycustomerDTO);
            LoadWaiverSetS();
            LoadWaiverSetData();
            SelectSingleWaiverSet();
            LoadCustomerSignedWaivers();
            CreateHeaderCheckBox();

            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            log.LogVariableState("workingRectangle.Width", workingRectangle.Width);
            log.LogVariableState("this.Width", this.Width);
            log.LogVariableState("pnlBase.Size- Before", this.pnlBase.Size);
            if (this.Width > workingRectangle.Width - 100)
            {
                this.SuspendLayout();
                this.Size = new System.Drawing.Size(workingRectangle.Width - 20, this.Height);
                this.pnlBase.Size = new System.Drawing.Size(this.Width - 10, pnlBase.Height);
                this.pnlBase.AutoScroll = true; 
                this.ResumeLayout(true);
            }
            log.LogVariableState("pnlBase.Size- After", this.pnlBase.Size);

            log.LogMethodExit();
        }

        private void LoadRelationShipTypes()
        {
            log.LogMethodEntry();
            CustomerRelationshipTypeListBL customerRelationshipTypeListBL = new CustomerRelationshipTypeListBL(executionContext);
            List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
            customerRelationshipTypeDTOList = customerRelationshipTypeListBL.GetCustomerRelationshipTypeDTOList(searchParameters);
            if (customerRelationshipTypeDTOList == null || customerRelationshipTypeDTOList.Count == 0)
            {
                customerRelationshipTypeDTOList = new List<CustomerRelationshipTypeDTO>();
            }
            customerRelationshipTypeDTOList.Insert(0, new CustomerRelationshipTypeDTO(-1, string.Empty, string.Empty, true));
            log.LogMethodExit(customerRelationshipTypeDTOList);
        }
        private void DisplayCustomerDetails()
        {
            log.LogMethodEntry();
            grpbxCustomer.Text = MessageContainerList.GetMessage(executionContext, "Customer") + ": " + this.signedBycustomerDTO.FirstName + (string.IsNullOrEmpty(this.signedBycustomerDTO.LastName) ? "" : this.signedBycustomerDTO.LastName);
            GetRelatedCustomers();
            RefreshDGVCustomers();
            log.LogMethodExit();
        }

        private void GetRelatedCustomers()
        {
            log.LogMethodEntry();
            if (guestCustomerId > -1 && guestCustomerId == signedBycustomerDTO.Id)
            {
                log.LogMethodExit("guestCustomerId > -1 && guestCustomerId == signedBycustomerDTO.Id");
                return;
            }
            else
            {
                CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(executionContext);
                List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>>();
                //searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, signedBycustomerDTO.Id.ToString()));
                searchParameters.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
                relatedCustomerDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(searchParameters);
                //if (relatedCustomerDTOList != null)
                //{
                //    for (int i = 0; i < relatedCustomerDTOList.Count; i++)
                //    {
                //        if (relatedCustomerDTOList[i].RelatedCustomerId == signedBycustomerDTO.Id)
                //        {
                //            relatedCustomerDTOList.RemoveAt(i);
                //            i = i - 1;
                //        }
                //    }
                //}
            }
            log.LogMethodExit();
        }

        private void RefreshDGVCustomers()
        {
            log.LogMethodEntry();
            List<CustomerDTO> customerDTOList = PrepareCustomerDTOList();
            if (customerDTOList != null && customerDTOList.Any() && customerDTOList[0].Id != signedBycustomerDTO.Id)
            {
                for (int i = 0; i < customerDTOList.Count; i++)
                {
                    if (customerDTOList[i].Id == signedBycustomerDTO.Id)
                    {
                        List<CustomerDTO> customerSignedByDTOList = new List<CustomerDTO>();
                        customerSignedByDTOList.Add(customerDTOList[i]);
                        customerDTOList.RemoveAt(i);
                        customerDTOList.Insert(0, customerSignedByDTOList[0]);
                        break;
                    }
                }

            }

            customerName.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.customerRelationType.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvCustomers.Rows.Clear();
            for (int i = 0; i < customerDTOList.Count; i++)
            {
                string customerNameDisplay = (customerDTOList[i].FirstName + " " + (string.IsNullOrEmpty(customerDTOList[i].LastName) ? "" : customerDTOList[i].LastName));
                int relationshipTypeId = (relatedCustomerDTOList != null ? ((relatedCustomerDTOList.Find(rCust => rCust.RelatedCustomerId == customerDTOList[i].Id) != null ? relatedCustomerDTOList.Find(rCust => rCust.RelatedCustomerId == customerDTOList[i].Id).CustomerRelationshipTypeId : -1)) : -1);
                string relationShipName = string.Empty;
                if (customerRelationshipTypeDTOList != null && customerRelationshipTypeDTOList.Any())
                {
                    relationShipName = (customerRelationshipTypeDTOList.Find(crt => crt.Id == relationshipTypeId) != null ? customerRelationshipTypeDTOList.Find(crt => crt.Id == relationshipTypeId).Description : string.Empty);
                }
                dgvCustomers.Rows.Add();
                dgvCustomers.Rows[i].Cells["customerName"].Value = customerNameDisplay;
                dgvCustomers.Rows[i].Cells["customerName"].ToolTipText = customerNameDisplay;
                dgvCustomers.Rows[i].Cells["customerRelationType"].Value = relationShipName;
                dgvCustomers.Rows[i].Cells["signFor"].Value = false;
                dgvCustomers.Rows[i].Cells["RelationshipTypeId"].Value = relationshipTypeId;
                dgvCustomers.Rows[i].Tag = new CustomerBL(executionContext, customerDTOList[i]);
            }
            dgvCustomers.EndEdit();
            if (dgvCustomers.Rows.Count > 0)
            {
                dgvCustomers.Rows[0].Cells["customerName"].Selected = true;
                selectedCustomerBL = new CustomerBL(executionContext, this.signedBycustomerDTO);
            }
            vScrollCustomers.UpdateButtonStatus();
            grpbxCustomer.Refresh();
            if (chkSelectAll.Checked)
            {
                chkSelectAll.Checked = false;
            }
            log.LogMethodExit();
        }

        private List<CustomerDTO> PrepareCustomerDTOList()
        {
            log.LogMethodEntry();
            List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
            List<CustomerDTO> relatedCustDTOList = null;
            List<int> customerIdList = new List<int>();
            //customerDTOList.Add(this.signedBycustomerDTO);
            customerIdList.Add(this.signedBycustomerDTO.Id);
            if (relatedCustomerDTOList != null && relatedCustomerDTOList.Any())
            {
                List<int> relCustomerIdList = relatedCustomerDTOList.Select(rcust => rcust.RelatedCustomerId).ToList();
                if (relatedCustomerDTOList[0].CustomerId != signedBycustomerDTO.Id && customerIdList.Exists(custId => custId == relatedCustomerDTOList[0].CustomerId) == false)
                {
                    relCustomerIdList.Add(relatedCustomerDTOList[0].CustomerId);
                }
                customerIdList.AddRange(relCustomerIdList);
            }
            customerIdList = customerIdList.Distinct().ToList();
            CustomerListBL customerListBL = new CustomerListBL(executionContext);
            relatedCustDTOList = customerListBL.GetCustomerDTOList(customerIdList, true, true, true);
            if (relatedCustDTOList != null && relatedCustDTOList.Any())
            {
                customerDTOList.AddRange(relatedCustDTOList);
            }
            log.LogMethodExit();
            return customerDTOList;
        }

        private void LoadWaiverSetS()
        {
            log.LogMethodEntry();
            try
            {
                WaiverSetContainer waiverSetContainer = null;
                try
                {
                    waiverSetContainer = WaiverSetContainer.GetInstance;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2435));//Unexpected error while getting waiver file details. Please check the setup
                }
                this.waiverSetDTOListForCombBox = new List<WaiverSetDTO>(waiverSetContainer.GetWaiverSetDTOList(executionContext.GetSiteId()));
                //LoadWaiverSetComboList();
                if (this.waiverSetDTOList == null || this.waiverSetDTOList.Any() == false)
                {
                    this.waiverSetDTOList = new List<WaiverSetDTO>(waiverSetContainer.GetWaiverSetDTOList(executionContext.GetSiteId()));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MessageContainerList.GetMessage(executionContext, "Error"));

            }
            log.LogMethodExit();
        }

        //private void LoadWaiverSetComboList()
        //{
        //    log.LogMethodEntry();
        //    if (waiverSetDTOListForCombBox == null || waiverSetDTOListForCombBox.Any() == false)
        //    {
        //        waiverSetDTOListForCombBox = new List<WaiverSetDTO>();
        //    }

        //    WaiverSetDTO waiverSetDTO = new WaiverSetDTO();
        //    waiverSetDTO.Description = string.Empty;
        //    waiverSetDTOListForCombBox.Insert(0, waiverSetDTO);

        //    waiverSetDetailId.DataSource = waiverSetDTOListForCombBox;
        //    waiverSetDetailId.DisplayMember = "Description";
        //    waiverSetDetailId.ValueMember = "waiverSetId";
        //    log.LogMethodExit();
        //}

        private void LoadWaiverSetData()
        {
            log.LogMethodEntry();
            grpBxSignWaiver.Text = MessageContainerList.GetMessage(executionContext, "Waiver details for Customer") + ": " + selectedCustomerBL.CustomerDTO.FirstName + " " + (string.IsNullOrEmpty(selectedCustomerBL.CustomerDTO.LastName) ? "" : selectedCustomerBL.CustomerDTO.LastName);

            pnlSignWaivers.Controls.Clear();
            pnlSignWaivers.Controls.Add(tPnlSignWaiverHeader);
            //pnlSignWaivers.Controls.Add(lblWaiverSetName);
            //pnlSignWaivers.Controls.Add(lblWaiveName);
            //pnlSignWaivers.Controls.Add(lblViewWaiver);
            //pnlSignWaivers.Controls.Add(lblValidityDays);
            //pnlSignWaivers.Controls.Add(lblCustomerHasSigned);
            if (this.waiverSetDTOList != null && this.waiverSetDTOList.Any())
            {
                int startYCoordinateAt = this.tPnlSignWaiverHeader.Height - 6;
                for (int i = 0; i < waiverSetDTOList.Count; i++)
                {
                    bool evenRowNo = (i % 2 == 0 ? true : false);
                    usrCtlWaiverSet usrCtlWaiverSet = new usrCtlWaiverSet(selectedCustomerBL, waiverSetDTOList[i], utilities, evenRowNo, this.trx);
                    usrCtlWaiverSet.ResetOtherWaiverSetSelection = new usrCtlWaiverSet.ResetOtherWaiverSetSelectionDelegate(ResetOtherWaiverSetSelection);
                    usrCtlWaiverSet.Name = "usrCtlWaiverSet" + i.ToString();
                    usrCtlWaiverSet.Location = new Point(usrCtlWaiverSet.Location.X, startYCoordinateAt);
                    startYCoordinateAt = startYCoordinateAt + usrCtlWaiverSet.Height + 2;
                    pnlSignWaivers.Controls.Add(usrCtlWaiverSet);
                    usrCtlWaiverSet.BringToFront();
                }
            }
            this.pnlSignWaivers.Refresh();
            log.LogMethodExit();
        }

        private void ResetOtherWaiverSetSelection(int waiverSetId)
        {
            log.LogMethodEntry(waiverSetId);
            foreach (Control waiverItem in pnlSignWaivers.Controls)
            {
                if (waiverItem is usrCtlWaiverSet)
                {
                    usrCtlWaiverSet waiverSetCtrl = (usrCtlWaiverSet)waiverItem;
                    if (waiverSetCtrl.WaiverSetDTO.WaiverSetId != waiverSetId)
                    {
                        waiverSetCtrl.IsSignWaiverSet = false;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void SelectSingleWaiverSet()
        {
            log.LogMethodEntry();
            if (waiverSetDTOList != null && waiverSetDTOList.Count == 1)
            {
                foreach (Control waiverItem in pnlSignWaivers.Controls)
                {
                    if (waiverItem is usrCtlWaiverSet)
                    {
                        usrCtlWaiverSet waiverSetCtrl = (usrCtlWaiverSet)waiverItem;
                        waiverSetCtrl.IsSignWaiverSet = true;
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }


        private void SelectSingleCustomer()
        {
            log.LogMethodEntry();
            if (dgvCustomers != null && dgvCustomers.Rows.Count == 1)
            {
                SetCustomerSignFor(true);
            }
            log.LogMethodExit();
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.RowIndex > -1)
                {
                    CustomerBL customerBL = (CustomerBL)dgvCustomers.Rows[e.RowIndex].Tag;
                    signatoryCustomerBL = new CustomerBL(utilities.ExecutionContext, this.signedBycustomerDTO);
                    if (dgvCustomers.Columns[e.ColumnIndex].Name == "signFor")
                    {
                        SetSignForFlag(e.RowIndex, customerBL, false);
                    }
                    else
                    {
                        if (customerBL.CustomerDTO.Id != selectedCustomerBL.CustomerDTO.Id)
                        {
                            selectedCustomerBL = customerBL;
                            LoadWaiverSetData();
                            LoadCustomerSignedWaivers();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MessageContainerList.GetMessage(executionContext, "Error"));
            }
            log.LogMethodExit();
        }

        private void SetSignForFlag(int rowIndex, CustomerBL customerBL, bool bulkSet)
        {
            log.LogMethodEntry(rowIndex, bulkSet);
            DataGridViewCheckBoxCell checkBox = (dgvCustomers["signFor", rowIndex] as DataGridViewCheckBoxCell);
            if (Convert.ToBoolean(checkBox.Value))
            {
                checkBox.Value = false;
            }
            else
            {
                if (bulkSet == false)
                {
                    if (customerBL.CustomerDTO.Id != selectedCustomerBL.CustomerDTO.Id)
                    {
                        selectedCustomerBL = customerBL;
                        LoadWaiverSetData();
                        LoadCustomerSignedWaivers();
                    }
                }

                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SIGN_WAIVER_WITHOUT_CUSTOMER_REGISTRATION", false)
                    && customerBL.CustomerDTO.Id == guestCustomerId && guestCustomerId > -1)
                {
                    checkBox.Value = true;
                }
                else
                {
                    ValidateAndSetSignForFlag(customerBL, checkBox, rowIndex);
                }
            }
            log.LogMethodExit();
        }

        private void ValidateAndSetSignForFlag(CustomerBL customerBL, DataGridViewCheckBoxCell checkBox, int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            checkBox.Value = false;
            int relationShipTypeId = Convert.ToInt32(dgvCustomers["RelationshipTypeId", rowIndex].Value);
            log.LogVariableState("relationShipTypeId", relationShipTypeId);
            if ((customerRelationshipTypeDTOList.Exists(rel => rel.Id == relationShipTypeId && rel.Name == RELATIONSHIPTYPEISCHILD) && customerBL.CustomerDTO.Id != this.signedBycustomerDTO.Id)
                || customerBL.CustomerDTO.Id == this.signedBycustomerDTO.Id)
            {

                if (customerBL.HasValidDateOfBirth() == false && customerBL.CustomerDTO.DateOfBirth != null && customerBL.CustomerDTO.DateOfBirth != DateTime.MinValue)
                {
                    if (messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 5335), MessageContainerList.GetMessage(executionContext, "Customer Details"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //Invalid date of birth. Do you want to update the same now?
                        CustomerDetailForm customerDetailForm = new CustomerDetailForm(utilities, customerBL.CustomerDTO, messageBoxDelegate, ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
                        customerDetailForm.SetControlsEnabled(true);
                        if (customerDetailForm.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                customerBL.Save(null);
                            }
                            catch (Exception ex)
                            {
                                // log.LogVariableState("accountDTO", accountDTO);
                                customerBL = new CustomerBL(executionContext, customerBL.CustomerDTO.Id);
                                log.Error("Validation failed", ex);
                                messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(executionContext, "Customer Details"));
                            }
                            dgvCustomers.Rows[rowIndex].Tag = customerBL;
                            dgvCustomers.Rows[rowIndex].Cells["customerName"].Value = customerBL.CustomerDTO.FirstName + " " + (String.IsNullOrEmpty(customerBL.CustomerDTO.LastName) ? "" : customerBL.CustomerDTO.LastName);
                            if (customerBL != null && customerBL.CustomerDTO.Id == this.signedBycustomerDTO.Id)
                            {
                                this.signedBycustomerDTO = customerBL.CustomerDTO;
                            }
                        }

                    }
                }

                if (customerBL.HasValidDateOfBirth() == true || (customerBL.CustomerDTO.DateOfBirth == null) || (customerBL.CustomerDTO.DateOfBirth == DateTime.MinValue))
                {
                    if (customerBL.IsAdult())
                    {
                        if (customerBL.CustomerDTO.Id != this.signedBycustomerDTO.Id)
                        {
                            //&1 is not a minor anymore, Parents/Guardians cannot sign on his/her behalf
                            messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 2304, customerBL.CustomerDTO.FirstName + " " + customerBL.CustomerDTO.LastName),
                                               MessageContainerList.GetMessage(executionContext, "Waivers"));
                        }
                        else
                        {
                            checkBox.Value = true;
                        }
                    }
                    else
                    { 
                        
                        if (customerBL.CustomerDTO.Id != this.signedBycustomerDTO.Id)
                        {
                            if (signatoryCustomerBL.IsAdult() == false)
                            {
                                throw new Exception(MessageContainerList.GetMessage(executionContext, 2849));// "Sorry, minor customer cannot sign for others"
                            }
                            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WHO_CAN_SIGN_FOR_MINOR")
                                != WaiverCustomerAndSignatureDTO.WhoCanSignForMinor.MINOR.ToString())
                            {
                                checkBox.Value = true;
                            }
                            else
                            {
                                messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 2850),
                                               MessageContainerList.GetMessage(executionContext, "Waivers"));
                                //Sorry, minors are allowed to sign for themselves
                            }
                        }
                        else
                        {
                            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WHO_CAN_SIGN_FOR_MINOR")
                                != WaiverCustomerAndSignatureDTO.WhoCanSignForMinor.MINOR.ToString())
                            {
                                messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 2302),
                                               MessageContainerList.GetMessage(executionContext, "Waivers"));
                            }
                            else
                            {
                                checkBox.Value = true;
                            }
                        }

                    }
                }
            }
            log.LogMethodExit();
        }

        private void btnSignWaiver_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SignWaiver();
                SelectCustomerForSigning();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MessageContainerList.GetMessage(executionContext, "Error"));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void SignWaiver()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CheckCustomerSelection();
                CheckWaiverSetSelection();
                CreateWaiverCustomerMapList();
                GetSignatureAndSave();
                RefreshDGVCustomers();
                LoadWaiverSetData();
                LoadCustomerSignedWaivers();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(executionContext, "Sign Waivers"));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }


        private void CheckCustomerSelection()
        {
            log.LogMethodEntry();
            signForCustomerDTOList = new List<CustomerDTO>();
            for (int i = 0; i < dgvCustomers.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell checkBox = (dgvCustomers["SignFor", i] as DataGridViewCheckBoxCell);
                if (Convert.ToBoolean(checkBox.Value))
                {
                    signForCustomerDTOList.Add(((CustomerBL)dgvCustomers.Rows[i].Tag).CustomerDTO);
                }
            }
            if (signForCustomerDTOList.Count == 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2305));//Please select a customer record to proceed
            }
            log.LogMethodExit();
        }
        private void CheckWaiverSetSelection()
        {
            log.LogMethodEntry();
            selectedWaiverSetDTO = null;
            List<usrCtlWaiverSet> waiverSetControlList = pnlSignWaivers.Controls.OfType<usrCtlWaiverSet>().ToList();
            if (waiverSetControlList != null)
            {
                foreach (usrCtlWaiverSet item in waiverSetControlList)
                {
                    if (item.IsSignWaiverSet)
                    {
                        selectedWaiverSetDTO = item.WaiverSetDTO;
                        break;
                    }
                }
            }
            if (selectedWaiverSetDTO == null)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2306));//Please select a waiver set record to proceed
            }
            log.LogMethodExit();
        }

        private void CreateWaiverCustomerMapList()
        {
            log.LogMethodEntry();
            managerApprovalList = new List<KeyValuePair<int, int>>();
            CreateWaiverCustomerAndSignatureDTOList();
            UpdateListBasedOnCustomerSignatureStatus();
            log.LogMethodExit();
        }

        private void CreateWaiverCustomerAndSignatureDTOList()
        {
            log.LogMethodEntry();
            selectedWaiverAndCustomerSignatureList = new List<WaiverCustomerAndSignatureDTO>();
            foreach (WaiversDTO waiversDTO in selectedWaiverSetDTO.WaiverSetDetailDTOList)
            {
                WaiverCustomerAndSignatureDTO waiverCustomerAndSignatureDTO = new WaiverCustomerAndSignatureDTO();
                waiverCustomerAndSignatureDTO.WaiversDTO = waiversDTO;
                waiverCustomerAndSignatureDTO.SignatoryCustomerDTO = signedBycustomerDTO;
                waiverCustomerAndSignatureDTO.SignForCustomerDTOList = new List<CustomerDTO>(signForCustomerDTOList);
                waiverCustomerAndSignatureDTO.CustIdNameSignatureImageList = new List<WaiveSignatureImageWithCustomerDetailsDTO>();
                waiverCustomerAndSignatureDTO.CustomerContentDTOList = null;
                selectedWaiverAndCustomerSignatureList.Add(waiverCustomerAndSignatureDTO);
            }
            log.LogMethodExit();
        }

        private void UpdateListBasedOnCustomerSignatureStatus()
        {
            log.LogMethodEntry();
            foreach (CustomerDTO selectedCustomer in signForCustomerDTOList)
            {
                CustomerBL customerBL = new CustomerBL(executionContext, selectedCustomer);
                if (customerBL.CustomerDTO != null && customerBL.CustomerDTO.Id != guestCustomerId)
                {

                    List<WaiversDTO> pendingWaiversList = customerBL.GetSignaturePendingWaivers(selectedWaiverSetDTO.WaiverSetDetailDTOList, serverTimeObj.GetServerDateTime());
                    if (pendingWaiversList == null || pendingWaiversList.Count != selectedWaiverSetDTO.WaiverSetDetailDTOList.Count)
                    {
                        List<WaiversDTO> signedWaivers = new List<WaiversDTO>();
                        if (pendingWaiversList == null || pendingWaiversList.Any() == false)
                        {
                            signedWaivers = new List<WaiversDTO>(selectedWaiverSetDTO.WaiverSetDetailDTOList);
                        }
                        else
                        {
                            for (int i = 0; i < selectedWaiverAndCustomerSignatureList.Count; i++)
                            {
                                if (pendingWaiversList.Exists(waiver => waiver.WaiverSetDetailId == selectedWaiverAndCustomerSignatureList[i].WaiversDTO.WaiverSetDetailId) == false)
                                {
                                    signedWaivers.Add(selectedWaiverAndCustomerSignatureList[i].WaiversDTO);
                                }
                            }
                        }
                        if (signedWaivers != null && signedWaivers.Any())
                        {
                            CustomerSignedWaiverListBL customerSignedWaiverListBL = new CustomerSignedWaiverListBL(executionContext);
                            List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>> searchParameters = new List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>>();
                            searchParameters.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.SIGNED_FOR, selectedCustomer.Id.ToString()));
                            //searchParameters.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            searchParameters.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.IS_ACTIVE, "1"));
                            searchParameters.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.IGNORE_EXPIRED, serverTimeObj.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = customerSignedWaiverListBL.GetAllCustomerSignedWaiverList(searchParameters);
                            if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
                            {
                                for (int i = 0; i < signedWaivers.Count; i++)
                                {
                                    List<CustomerSignedWaiverDTO> signedWaiverCopyList = customerSignedWaiverDTOList.Where(cw => cw.WaiverSetDetailId == signedWaivers[i].WaiverSetDetailId).ToList();
                                    if (signedWaiverCopyList != null && signedWaiverCopyList.Any())
                                    {
                                        foreach (CustomerSignedWaiverDTO item in signedWaiverCopyList)
                                        {
                                            if (CanDeactivate(item) == false)
                                            {
                                                log.Info("remove entry from signedWaivers: " + signedWaivers[i].WaiverSetDetailId);
                                                messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 2351, MessageContainerList.GetMessage(executionContext, "re-sign"), signedWaivers[i].Name), MessageContainerList.GetMessage(executionContext, "Sign"));
                                                this.Cursor = Cursors.WaitCursor;
                                                signedWaivers.RemoveAt(i);
                                                i = i - 1;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //Customer &1 has signed one or more selected waivers. Do they want to re-sign the waivers?
                        if ((signedWaivers != null && signedWaivers.Any())
                            && messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 2307, customerBL.CustomerDTO.FirstName + " " + (string.IsNullOrEmpty(customerBL.CustomerDTO.LastName) ? string.Empty : customerBL.CustomerDTO.LastName)),
                                               MessageContainerList.GetMessage(executionContext, "Sign Waivers"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "WAIVER_DEACTIVATION_NEEDS_MANAGER_APPROVAL", true))
                            {
                                int mgrId = -1;
                                if (Authenticate.Manager(ref mgrId) == false)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Manager Approval Required"));
                                }
                                else
                                {
                                    managerApprovalList.Add(new KeyValuePair<int, int>(customerBL.CustomerDTO.Id, mgrId));
                                }
                                this.Cursor = Cursors.WaitCursor;
                            }
                        }
                        else
                        {
                            this.Cursor = Cursors.WaitCursor;
                            for (int i = 0; i < selectedWaiverAndCustomerSignatureList.Count; i++)
                            {
                                if (pendingWaiversList.Exists(waiver => waiver.WaiverSetDetailId == selectedWaiverAndCustomerSignatureList[i].WaiversDTO.WaiverSetDetailId) == false)
                                {
                                    selectedWaiverAndCustomerSignatureList[i].SignForCustomerDTOList.Remove(selectedCustomer);
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < selectedWaiverAndCustomerSignatureList.Count; i++)
            {
                if (selectedWaiverAndCustomerSignatureList[i].SignForCustomerDTOList == null || selectedWaiverAndCustomerSignatureList[i].SignForCustomerDTOList.Any() == false)
                {
                    selectedWaiverAndCustomerSignatureList.RemoveAt(i);
                    i = i - 1;
                }
            }
            for (int i = 0; i < selectedWaiverAndCustomerSignatureList.Count; i++)
            {
                selectedWaiverAndCustomerSignatureList[i] = WaiverCustomerAndSignatureBL.CreateCustomerContentForWaiver(utilities.ExecutionContext, selectedWaiverAndCustomerSignatureList[i]);
            }
            log.LogMethodExit();
        }

        private void GetSignatureAndSave()
        {
            log.LogMethodEntry();
            SplitBySigningOption();
            DeviceWaivers();
            ManualWaivers();
            SaveCustomerSignedWaivers();
            log.LogMethodExit();
        }

        private void SplitBySigningOption()
        {
            log.LogMethodEntry();
            deviceSignatureList = null;
            manualSignatureList = new List<WaiverCustomerAndSignatureDTO>();
            bool device = false;
            bool manual = false;
            if (selectedWaiverSetDTO.WaiverSetSigningOptionDTOList != null)
            {
                foreach (WaiverSetSigningOptionsDTO item in selectedWaiverSetDTO.WaiverSetSigningOptionDTOList)
                {
                    if (item.OptionName == WaiverSetSigningOptionsDTO.WaiverSigningOptions.DEVICE.ToString() || item.OptionName == WaiverSetSigningOptionsDTO.WaiverSigningOptions.ONLINE.ToString())
                    {
                        device = true;
                    }
                    if (item.OptionName == WaiverSetSigningOptionsDTO.WaiverSigningOptions.MANUAL.ToString())
                    {
                        manual = true;
                    }
                }
            }
            if (device && manual || device)
            {
                deviceSignatureList = new List<WaiverCustomerAndSignatureDTO>();
                deviceSignatureList = selectedWaiverAndCustomerSignatureList;
            }
            else if (manual)
            {
                manualSignatureList = new List<WaiverCustomerAndSignatureDTO>();
                manualSignatureList = selectedWaiverAndCustomerSignatureList;
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2308, selectedWaiverSetDTO.Name));//Please check the waiver signing option for &1
            }
            log.LogMethodExit();
        }

        private void DeviceWaivers()
        {
            log.LogMethodEntry();
            if (deviceSignatureList != null && deviceSignatureList.Any())
            {
                if (Screen.AllScreens.Length == 1)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1006));
                }
                else
                {
                    //Log Waiver Transaction Event
                    //utilities.EventLog.logEvent("WAIVERTRANSACTION", 'I', executionContext.GetUserId(), "Waiver Agreement for certain products", "", 0, " ", "", null);
                    //dataGridViewTransaction.ScrollBars = ScrollBars.Both;

                    //DataGridView dgvTrx = DisplayDatagridView.createRefTrxDatagridview(Utilities);
                    //dgvTrx.RowTemplate.Height = 40;
                    //DisplayDatagridView.RefreshTrxDataGrid(NewTrx, dgvTrx, NewTrx.Utilities);
                    //Control cntrl = dgvTrx;
                    //if (cntrl != null) //if transaction grid not null then start waiver
                    {
                        log.Info("Loading Status Message window");
                        frmWaiverSignature_DTU1031 frmWaiverSignature_DTU1031 = null;
                        frmWaiverSignature_DTH1152 frmWaiverSignature_DTH1152 = null;

                        string pid = string.Empty;
                        pid = GetWacomPID();
                        List<LookupValuesDTO> lookupValuesDTOList = GetWacomDevicesPIdLookups();
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                        {
                            LookupValuesDTO lookupValuesDTO = lookupValuesDTOList.Find(lp => lp.LookupValue == pid);

                            if (lookupValuesDTO != null)
                            {
                                log.Debug(lookupValuesDTO.Description);
                                if (lookupValuesDTO.Description.Equals("frmWaiverSignature_DTU1031"))
                                {
                                    frmWaiverSignature_DTU1031 = new frmWaiverSignature_DTU1031(deviceSignatureList, utilities);
                                }
                                else
                                {
                                    frmWaiverSignature_DTH1152 = new frmWaiverSignature_DTH1152(deviceSignatureList, utilities);
                                }
                            }

                        }
                        if (frmWaiverSignature_DTU1031 == null && frmWaiverSignature_DTH1152 == null)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1007));
                        }
                        Screen[] sc;
                        sc = Screen.AllScreens;
                        try
                        {
                            frmstatus = new frmStatus(frmWaiverSignature_DTU1031, frmWaiverSignature_DTH1152);
                            frmstatus.TopMost = true;
                            frmstatus.StartPosition = FormStartPosition.CenterScreen;
                            frmstatus.Location = sc[0].Bounds.Location;
                            Point p1 = new Point(sc[0].Bounds.Location.X, sc[0].Bounds.Location.Y);
                            frmstatus.Location = p1;
                            frmstatus.WindowState = FormWindowState.Normal;
                            DialogResult statusResult = new System.Windows.Forms.DialogResult();

                            System.Threading.Thread thread = new System.Threading.Thread(() =>
                            {
                                statusResult = frmstatus.ShowDialog();
                            });
                            thread.Start();

                            CaptureSignature(frmWaiverSignature_DTU1031, frmWaiverSignature_DTH1152);

                            if (statusResult == DialogResult.Cancel)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1007));
                            }
                        }
                        finally
                        {
                            this.Invoke(new MethodInvoker(() => frmstatus.Close()));
                        }

                        //}
                        //else // if transaction grid is null then continue with transaction with displaying msg 
                        //{
                        //    retStatus = false;
                        //    displayMessageLine(MessageUtils.getMessage(1003), ERROR);
                        //}
                        // }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void CaptureSignature(frmWaiverSignature_DTU1031 frmWaiverSignature_DTU1031, frmWaiverSignature_DTH1152 frmWaiverSignature_DTH1152)
        {
            log.LogMethodEntry();
            //bool status = true;
            //Image signatureImage;
            Dictionary<int, Image> signatureImageFileList = new Dictionary<int, Image>();
            Screen[] sc;
            sc = Screen.AllScreens;
            if (sc.Length > 1)
            {
                if (frmWaiverSignature_DTU1031 != null)
                {
                    try
                    {
                        frmWaiverSignature_DTU1031.FormBorderStyle = FormBorderStyle.None;
                        frmWaiverSignature_DTU1031.Left = sc[1].Bounds.Width;
                        frmWaiverSignature_DTU1031.Top = sc[1].Bounds.Height;
                        frmWaiverSignature_DTU1031.StartPosition = FormStartPosition.Manual;
                        frmWaiverSignature_DTU1031.Location = sc[1].Bounds.Location;
                        Point p = new Point(sc[1].Bounds.Location.X, sc[1].Bounds.Location.Y);
                        frmWaiverSignature_DTU1031.Location = p;
                        frmWaiverSignature_DTU1031.WindowState = FormWindowState.Maximized;
                        DialogResult result = frmWaiverSignature_DTU1031.ShowDialog();
                        List<WaiverCustomerAndSignatureDTO> receivedSignatureDetails = frmWaiverSignature_DTU1031.WaiverCustomerAndSignatureDTOList;
                        deviceSignatureList = receivedSignatureDetails;
                        //System.Drawing.Image image = null;// frmWaiverSignature_DTU1031.ImageFile;
                        //Dictionary<int, Image> imageFileList = null;// frmWaiverSignature_DTU1031.imageFileList;
                        if (result == DialogResult.OK)
                        {
                            if (receivedSignatureDetails != null 
                                && receivedSignatureDetails.Exists(rcvd => rcvd.CustIdNameSignatureImageList == null || rcvd.CustIdNameSignatureImageList.Any() == false) == false)
                            {
                                //Bitmap bitmap = new Bitmap(image);
                                //signatureImage = image;
                                //signatureImageFileList = imageFileList;
                                //String filename = Guid.NewGuid().ToString() + ".png";
                                //isSigned = true;
                                using (frmWaiverCashierWindow frmCashierWindow = new frmWaiverCashierWindow(receivedSignatureDetails))
                                {
                                    try
                                    {
                                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WAIVER_CONFIRMATION_REQUIRED") == "Y") // if Cashier verification enabled then open the cashier verification window
                                        {
                                            log.Info("Loading Cashier signature verification window");
                                            frmCashierWindow.WindowState = FormWindowState.Normal;
                                            frmCashierWindow.BringToFront();
                                            frmCashierWindow.TopMost = true;
                                            DialogResult cashierFrmResult = frmCashierWindow.ShowDialog();

                                            if (cashierFrmResult == DialogResult.OK)
                                            {
                                                // bitmap.Save("D:\\Wacom\\SignagtureImage\\"+ filename, System.Drawing.Imaging.ImageFormat.Png); // copy image into specified directory in file system
                                                frmWaiverSignature_DTU1031.Close();
                                            }
                                            if (cashierFrmResult == DialogResult.Cancel)//if signature verification failed, customer asks to re-sign
                                            {
                                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1004));
                                                // status = false;
                                                // signatureImage = null;
                                                //displayMessageLine(MessageUtils.getMessage(1004), ERROR); // not valid signature                                    
                                            }
                                        }
                                        //else if (ManualWaiverSetDetailDTOList != null && ManualWaiverSetDetailDTOList.Count > 0)
                                        //{
                                        //    if (frmstatus != null)
                                        //        this.Invoke(new MethodInvoker(() => frmstatus.Close()));
                                        //    frmCashierWindow = new frmWaiverCashierWindow(null, null, ManualWaiverSetDetailDTOList);
                                        //    DialogResult cashierFrmResult = frmCashierWindow.ShowDialog();

                                        //    if (cashierFrmResult == DialogResult.Cancel)
                                        //    {
                                        //        status = false;
                                        //        //displayMessageLine(MessageUtils.getMessage(1007), WARNING);
                                        //    }
                                        //    else
                                        //        status = true;
                                        //}
                                        //else
                                        //{
                                        //   // status = true; //signature valid 
                                        //}
                                    }
                                    catch (Exception ex) //
                                    {
                                        log.Error(ex);
                                        frmCashierWindow.Close();
                                        throw;
                                    }
                                }
                            }
                            else
                            {
                                if (result == DialogResult.Abort)
                                {
                                    // status = false; 
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1003));
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1001));
                                }
                            }
                            //frmWaiverSignature_DTU1031.Close();
                        }
                        if (result == DialogResult.Cancel)// if customer not signed
                        {
                            //status = false;
                            //frmWaiverSignature_DTU1031.Close();
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1001));
                        }

                        if (result == DialogResult.None)// Error while capturing signature
                        {
                            //status = false;
                            //frmWaiverSignature_DTU1031.Close();
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1005));
                            // displayMessageLine(MessageUtils.getMessage(1005), ERROR);
                        }
                    }
                    finally
                    {
                        if (frmWaiverSignature_DTU1031 != null)
                        {
                            frmWaiverSignature_DTU1031.Close();
                        }
                    }
                }
                else if (frmWaiverSignature_DTH1152 != null)
                {
                    try
                    {
                        frmWaiverSignature_DTH1152.FormBorderStyle = FormBorderStyle.None;
                        frmWaiverSignature_DTH1152.Left = sc[1].Bounds.Width;
                        frmWaiverSignature_DTH1152.Top = sc[1].Bounds.Height;
                        frmWaiverSignature_DTH1152.StartPosition = FormStartPosition.Manual;
                        frmWaiverSignature_DTH1152.Location = sc[1].Bounds.Location;
                        Point p = new Point(sc[1].Bounds.Location.X, sc[1].Bounds.Location.Y);
                        frmWaiverSignature_DTH1152.Location = p;
                        frmWaiverSignature_DTH1152.WindowState = FormWindowState.Maximized;
                        DialogResult result = frmWaiverSignature_DTH1152.ShowDialog();
                        List<WaiverCustomerAndSignatureDTO> receivedSignatureDetails = frmWaiverSignature_DTH1152.WaiverCustomerAndSignatureDTOList;
                        deviceSignatureList = receivedSignatureDetails;
                        //System.Drawing.Image image = null;// frmWaiverSignature_DTH1152.ImageFile;
                        // Dictionary<int, Image> imageFileList = null;// frmWaiverSignature_DTH1152.imageFileList;
                        if (result == DialogResult.OK)
                        {
                            if (receivedSignatureDetails != null
                                 && receivedSignatureDetails.Exists(rcvd => rcvd.CustIdNameSignatureImageList == null || rcvd.CustIdNameSignatureImageList.Any() == false) == false)
                            {
                                // Bitmap bitmap = new Bitmap(image);
                                //signatureImage = image;
                                //signatureImageFileList = imageFileList;
                                //String filename = Guid.NewGuid().ToString() + ".png";
                                //isSigned = true;
                                using (frmWaiverCashierWindow frmCashierWindow = new frmWaiverCashierWindow(receivedSignatureDetails))
                                {
                                    try
                                    {
                                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WAIVER_CONFIRMATION_REQUIRED") == "Y") // if Cashier verification enabled then open the cashier verification window
                                        {
                                            log.Info("Loading Cashier signature verification window");
                                            frmCashierWindow.WindowState = FormWindowState.Normal;
                                            frmCashierWindow.BringToFront();
                                            frmCashierWindow.TopMost = true;
                                            DialogResult cashierFrmResult = frmCashierWindow.ShowDialog();

                                            if (cashierFrmResult == DialogResult.OK)
                                            {
                                                // bitmap.Save("D:\\Wacom\\SignagtureImage\\"+ filename, System.Drawing.Imaging.ImageFormat.Png); // copy image into specified directory in file system
                                                frmWaiverSignature_DTH1152.Close();
                                            }
                                            if (cashierFrmResult == DialogResult.Cancel)//if signature verification failed, customer asks to re-sign
                                            {
                                                //status = false;
                                                //signatureImage = null;
                                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1004));
                                                //displayMessageLine(MessageUtils.getMessage(1004), ERROR); // not valid signature                                    
                                            }
                                        }
                                        //else if (ManualWaiverSetDetailDTOList != null && ManualWaiverSetDetailDTOList.Count > 0)
                                        //{
                                        //    if (frmstatus != null)
                                        //        this.Invoke(new MethodInvoker(() => frmstatus.Close()));
                                        //    frmCashierWindow = new frmWaiverCashierWindow(null, null, ManualWaiverSetDetailDTOList);
                                        //    DialogResult cashierFrmResult = frmCashierWindow.ShowDialog();

                                        //    if (cashierFrmResult == DialogResult.Cancel)
                                        //    {
                                        //        status = false;
                                        //        //displayMessageLine(MessageUtils.getMessage(1007), WARNING);
                                        //    }
                                        //    else
                                        //        status = true;
                                        //}
                                        //else
                                        //    status = true;
                                    }
                                    catch (Exception ex) //
                                    {
                                        log.Error(ex);
                                        frmCashierWindow.Close();
                                        throw;
                                    }
                                }
                            }
                            else
                            {
                                if (result == DialogResult.Abort)
                                {
                                    //status = false;
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1003));
                                    //displayMessageLine(MessageUtils.getMessage(1003), ERROR);
                                }
                                else
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1001));
                                }
                            }
                            //frmWaiverSignature_DTH1152.Close();
                        }
                        if (result == DialogResult.Cancel)// if customer not signed
                        {
                            //status = false;
                            //frmWaiverSignature_DTH1152.Close();
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1001));
                            //displayMessageLine(MessageUtils.getMessage(1001), ERROR);
                        }

                        if (result == DialogResult.None)// Error while capturing signature
                        {
                            //status = false;
                            //frmWaiverSignature_DTH1152.Close();
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1005));
                            //displayMessageLine(MessageUtils.getMessage(1005), ERROR);
                        }

                    }
                    finally
                    {
                        if (frmWaiverSignature_DTH1152 != null)
                        {
                            frmWaiverSignature_DTH1152.Close();
                        }
                    }
                }


            }
            log.LogMethodExit();
        }

        private string GetWacomPID()
        {
            log.LogMethodEntry();
            string pid;
            PeripheralsListBL peripheralsListBL = new PeripheralsListBL(executionContext);
            List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchPeripheralsParams = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_TYPE, "Waiver"));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, (utilities.ParafaitEnv.POSMachineId).ToString()));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.ACTIVE, "1"));
            List<PeripheralsDTO> peripheralsDTOList = peripheralsListBL.GetPeripheralsDTOList(searchPeripheralsParams);
            if (peripheralsDTOList != null && peripheralsDTOList.Count > 0)
            {
                pid = peripheralsDTOList[0].Pid;
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1768));
            }
            log.LogMethodExit(pid);
            return pid;
        }

        private List<LookupValuesDTO> GetWacomDevicesPIdLookups()
        {
            log.LogMethodEntry();
            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WACOM_DEVICES_PID"));
            lookupValuesDTOList = new LookupValuesList(executionContext).GetAllLookupValues(searchlookupParameters);
            log.LogMethodExit(lookupValuesDTOList);
            return lookupValuesDTOList;
        }

        private void ManualWaivers()
        {
            log.LogMethodEntry();
            if (manualSignatureList != null && manualSignatureList.Any())
            {

                using (frmWaiverCashierWindow frmCashierWindow = new frmWaiverCashierWindow(manualSignatureList))
                {
                    DialogResult cashierFrmResult = frmCashierWindow.ShowDialog();

                    if (cashierFrmResult == DialogResult.Cancel)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1007));
                        //if (!retStatus)
                        //{
                        //    retStatus = false;
                        //    displayMessageLine(MessageUtils.getMessage(1007), WARNING);
                        //}
                    }
                }
                //else
                //    retStatus = true;

            }
            log.LogMethodExit();
        }

        private void SaveCustomerSignedWaivers()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<WaiverCustomerAndSignatureDTO> receivedSignatureDetailsList = new List<WaiverCustomerAndSignatureDTO>();
                if (deviceSignatureList != null && deviceSignatureList.Any())
                {
                    receivedSignatureDetailsList.AddRange(deviceSignatureList);
                }
                if (manualSignatureList != null && manualSignatureList.Any())
                {
                    receivedSignatureDetailsList.AddRange(manualSignatureList);
                }
                if (receivedSignatureDetailsList != null && receivedSignatureDetailsList.Any())
                {
                    CustomerBL signatoryCustomerBL = new CustomerBL(executionContext, receivedSignatureDetailsList[0].SignatoryCustomerDTO);
                    ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    int custSignedWaiverHeaderId = -1;
                    try
                    {
                        custSignedWaiverHeaderId = signatoryCustomerBL.CreateCustomerSignedWaiverHeader(SIGNATURECHANNEL, dBTransaction.SQLTrx);
                        bool customerIsAnAdult = signatoryCustomerBL.IsAdult();
                        int guardianId = customerIsAnAdult ? signatoryCustomerBL.CustomerDTO.Id : -1; 
                        foreach (WaiverCustomerAndSignatureDTO custSignatureDetails in receivedSignatureDetailsList)
                        {
                            foreach (CustomerDTO signForCustomerDTO in custSignatureDetails.SignForCustomerDTOList)
                            {
                                CustomerBL customerBL = new CustomerBL(executionContext, signForCustomerDTO);
                                int mgrId = -1;
                                if (managerApprovalList != null && managerApprovalList.Any())
                                {
                                    KeyValuePair<int, int> mgrIdDetails = managerApprovalList.Find(mAprv => mAprv.Key == signForCustomerDTO.Id);
                                    mgrId = (mgrIdDetails.Value > -1 ? mgrIdDetails.Value : -1);
                                }
                                customerBL.CreateCustomerSignedWaiver(custSignatureDetails.WaiversDTO, custSignedWaiverHeaderId, custSignatureDetails.CustomerContentDTOList, custSignatureDetails.CustIdNameSignatureImageList, mgrId, utilities, guardianId);
                                customerBL.Save(dBTransaction.SQLTrx);
                            }
                        }
                        dBTransaction.EndTransaction();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        dBTransaction.RollBack();
                        throw;
                    }
                    try
                    {
                        signatoryCustomerBL.LoadCustomerSignedWaivers();
                        SendWaiverEmail(signatoryCustomerBL.CustomerDTO, custSignedWaiverHeaderId);
                        if (this.trx != null && signatoryCustomerBL.CustomerDTO.Id > -1 && signatoryCustomerBL.CustomerDTO.Id == guestCustomerId && custSignedWaiverHeaderId > -1)
                        {
                            CustomerSignedWaiverListBL customerSignedWaiverListBl = new CustomerSignedWaiverListBL(executionContext);
                            List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>> searchParams = new List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>>();
                            searchParams.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.CUSTOMER_SIGNED_WAIVER_HEADER_ID, custSignedWaiverHeaderId.ToString()));
                            //searchParams.Add(new KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>(CustomerSignedWaiverDTO.SearchByCSWParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = customerSignedWaiverListBl.GetAllCustomerSignedWaiverList(searchParams);
                            if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
                            {
                                foreach (WaiverCustomerAndSignatureDTO custSignatureDetails in receivedSignatureDetailsList)
                                {
                                    CustomerSignedWaiverDTO customerSignedWaiverDTO = customerSignedWaiverDTOList.FirstOrDefault(csw => csw.WaiverSetDetailId == custSignatureDetails.WaiversDTO.WaiverSetDetailId
                                                                                                                                         && csw.IsActive
                                                                                                                                         && csw.ExpiryDate == null || csw.ExpiryDate >= serverTimeObj.GetServerDateTime());
                                    if (customerSignedWaiverDTO != null)
                                    {
                                        this.trx.MapCustomerSignedWaiver(custSignatureDetails.WaiversDTO, customerSignedWaiverDTO.CustomerSignedWaiverId);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MessageContainerList.GetMessage(executionContext, "Send Email"));
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void SendWaiverEmail(CustomerDTO customerDTO, int custSignedWaiverHeaderId)
        {
            log.LogMethodEntry(customerDTO, custSignedWaiverHeaderId);
            if (customerDTO != null && customerDTO.Id > -1 && customerDTO.Id != guestCustomerId)
            {
                CustomerSignedWaiverHeaderListBL customerSignedWaiverHeaderList = new CustomerSignedWaiverHeaderListBL(executionContext);
                customerSignedWaiverHeaderList.SendWaiverEmail(customerDTO, custSignedWaiverHeaderId, utilities, null);
            }
            log.LogMethodExit();
        }

        private void tpViewSignedWaivers_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }


        private void LoadUsers()
        {
            log.LogMethodEntry();
            UsersList usersList = new UsersList(executionContext);
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParam = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            searchParam.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<UsersDTO> usersDTOList = usersList.GetAllUsers(searchParam);
            if (usersDTOList == null || usersDTOList.Count == 0)
            {
                usersDTOList = new List<UsersDTO>();
            }
            UsersDTO usersDTO = new UsersDTO();
            usersDTO.LoginId = string.Empty;
            usersDTOList.Insert(0, usersDTO);
            deactivatedBy.DataSource = usersDTOList;
            deactivatedBy.DisplayMember = "LoginId";
            deactivatedBy.ValueMember = "userId";

            deactivationApprovedBy.DataSource = usersDTOList;
            deactivationApprovedBy.DisplayMember = "LoginId";
            deactivationApprovedBy.ValueMember = "userId";
            log.LogMethodExit(customerRelationshipTypeDTOList);
        }

        private void LoadCustomerSignedWaivers()
        {
            log.LogMethodEntry();
            if (selectedCustomerBL != null && selectedCustomerBL.CustomerDTO != null && selectedCustomerBL.CustomerDTO.Id != guestCustomerId)
            {
                selectedCustomerBL.LoadCustomerSignedWaivers();
                List<CustomerSignedWaiverDTO> customerSignedByWaiverDTOList = selectedCustomerBL.CustomerDTO.CustomerSignedWaiverDTOList;
                dgvCustomerSignedWaivers.DataSource = customerSignedByWaiverDTOList;
                SetFormatFordgvCustomerSignedWaivers();
                dgvCustomerSignedWaivers.Refresh();
            }
            log.LogMethodExit();
        }

        private void SetFormatFordgvCustomerSignedWaivers()
        {
            log.LogMethodEntry();
            dgvCustomerSignedWaivers.Columns["expiryDateDataGridViewTextBoxColumn"].DefaultCellStyle =
                 dgvCustomerSignedWaivers.Columns["signedDateDataGridViewTextBoxColumn"].DefaultCellStyle =
                  dgvCustomerSignedWaivers.Columns["deactivationDateDataGridViewTextBoxColumn"].DefaultCellStyle =
                   dgvCustomerSignedWaivers.Columns["creationDateDataGridViewTextBoxColumn"].DefaultCellStyle =
                    dgvCustomerSignedWaivers.Columns["lastUpdateDateDataGridViewTextBoxColumn"].DefaultCellStyle = this.utilities.gridViewDateTimeCellStyle();
            SetDGVCellFont(dgvCustomerSignedWaivers);
            log.LogMethodExit();
        }

        private void SetDGVCellFont(DataGridView dgvInput)
        {
            log.LogMethodEntry();
            //dgvInput.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            System.Drawing.Font font;
            try
            {
                font = new Font(utilities.ParafaitEnv.DEFAULT_GRID_FONT, 15, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while applying new font", ex);
                font = new Font("Tahoma", 15, FontStyle.Regular);
            }
            foreach (DataGridViewColumn c in dgvInput.Columns)
            {
                c.DefaultCellStyle.Font = new Font(font.FontFamily, 15F, FontStyle.Regular);
            }
            dgvInput.AllowUserToResizeRows = false;
            log.LogMethodExit();
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = GetSelectedDTOList();
                if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
                {
                    try
                    {
                        //Do you want to proceed with deactivation of signed waiver(s)?
                        if (messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 2344), MessageContainerList.GetMessage(executionContext, "Deactivation"), MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            log.LogMethodExit("No");
                            return;
                        }

                        for (int i = 0; i < customerSignedWaiverDTOList.Count; i++)
                        {
                            CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(executionContext, customerSignedWaiverDTOList[i]);
                            if (CanDeactivate(customerSignedWaiverDTOList[i]) == false)
                            {

                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2351, MessageContainerList.GetMessage(executionContext, "deactivate"), customerSignedWaiverBL.GetCustomerSignedWaiverDTO.WaiverName));
                                //Cannot &1 the signed waiver id: &2, it is linked with active transaction(s) 
                            }
                        }

                        int mgrId = GetManagerApproval();
                        for (int i = 0; i < customerSignedWaiverDTOList.Count; i++)
                        {
                            CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(executionContext, customerSignedWaiverDTOList[i]);
                            customerSignedWaiverBL.GetCustomerSignedWaiverDTO.DeactivatedBy = executionContext.GetUserPKId();
                            customerSignedWaiverBL.GetCustomerSignedWaiverDTO.ExpiryDate = serverTimeObj.GetServerDateTime();
                            customerSignedWaiverBL.GetCustomerSignedWaiverDTO.DeactivationDate = serverTimeObj.GetServerDateTime();
                            customerSignedWaiverBL.GetCustomerSignedWaiverDTO.IsActive = false;
                            customerSignedWaiverBL.GetCustomerSignedWaiverDTO.DeactivationApprovedBy = mgrId;
                            customerSignedWaiverBL.Save();
                        }
                        messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 48), MessageContainerList.GetMessage(executionContext, "Deactivate"));
                        RefreshDGVCustomers();
                        LoadWaiverSetData();
                        LoadCustomerSignedWaivers();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(executionContext, "Deactivate"));
            }
            log.LogMethodExit();
        }

        //private void RefreshBLInCustomerDGV(List<int> custIdList)
        //{
        //    log.LogMethodEntry();
        //    if (custIdList != null && custIdList.Any())
        //    {
        //        for (int i = 0; i < dgvCustomers.Rows.Count; i++)
        //        {
        //            if (dgvCustomers.Rows[i].Tag != null)
        //            {
        //                CustomerBL customerBL = (CustomerBL)dgvCustomers.Rows[i].Tag;
        //                if (customerBL != null && customerBL.CustomerDTO != null
        //                    && customerBL.CustomerDTO.Id != selectedCustomerBL.CustomerDTO.Id && custIdList.Exists(custId => custId == customerBL.CustomerDTO.Id))
        //                {
        //                    customerBL.LoadCustomerSignedWaivers();
        //                    dgvCustomers.Rows[i].Tag = customerBL;
        //                }
        //            }
        //        }
        //    }
        //    dgvCustomers.EndEdit();
        //    log.LogMethodExit();
        //}

        private int GetManagerApproval()
        {
            log.LogMethodEntry();
            int mgrId = -1;
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "WAIVER_DEACTIVATION_NEEDS_MANAGER_APPROVAL", false))
            {
                if (Authenticate.Manager(ref mgrId) == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Manager Approval Required"));
                }
            }
            log.LogMethodExit(mgrId);
            return mgrId;
        }

        private List<CustomerSignedWaiverDTO> GetSelectedDTOList()
        {
            log.LogMethodEntry();
            List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = new List<CustomerSignedWaiverDTO>();
            if (dgvCustomerSignedWaivers != null && dgvCustomerSignedWaivers.Rows.Count > 0)
            {
                for (int rowIndex = 0; rowIndex < dgvCustomerSignedWaivers.Rows.Count; rowIndex++)
                {
                    DataGridViewCheckBoxCell checkBox = (dgvCustomerSignedWaivers["selectRecord", rowIndex] as DataGridViewCheckBoxCell);
                    if (Convert.ToBoolean(checkBox.Value))
                    {
                        CustomerSignedWaiverDTO customerSignedWaiverDTO = (dgvCustomerSignedWaivers.DataSource as List<CustomerSignedWaiverDTO>)[rowIndex];
                        customerSignedWaiverDTOList.Add(customerSignedWaiverDTO);
                    }
                }
            }
            log.LogMethodExit(customerSignedWaiverDTOList);
            return customerSignedWaiverDTOList;
        }

        private void btnViewSignedWaiver_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = GetSelectedDTOList();
                if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
                {
                    for (int i = 0; i < customerSignedWaiverDTOList.Count; i++)
                    {
                        if (string.IsNullOrEmpty(customerSignedWaiverDTOList[i].SignedWaiverFileName))
                        {
                            messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 2345, customerSignedWaiverDTOList[i].WaiverName, MessageContainerList.GetMessage(executionContext, "view")),
                                                  MessageContainerList.GetMessage(executionContext, "View Waivers"));
                            customerSignedWaiverDTOList.RemoveAt(i);
                            i = i - 1;
                        }
                    }
                    if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
                    {
                        if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Count > 5)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2434));
                            //Please select max 5 records to view the files
                        }

                        using (frmViewWaiverUI frmViewWaiver = new frmViewWaiverUI(customerSignedWaiverDTOList, utilities))
                        {
                            if (frmViewWaiver.Width > Application.OpenForms["POS"].Width + 28)
                            {
                                frmViewWaiver.Width = Application.OpenForms["POS"].Width - 30;
                            }
                            frmViewWaiver.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(executionContext, "View"));
            }
            log.LogMethodExit();
        }

        private void btnPrintWaiver_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = GetSelectedDTOList();
                if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
                {
                    using (PrintDocument pd = new PrintDocument())
                    {
                        SetupThePrinting(pd, MessageContainerList.GetMessage(executionContext, "Waivers"));

                        for (int i = 0; i < customerSignedWaiverDTOList.Count; i++)
                        {
                            if (string.IsNullOrEmpty(customerSignedWaiverDTOList[i].SignedWaiverFileName) == false)
                            {
                                CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(executionContext, customerSignedWaiverDTOList[i]);
                                string fileWithPath = customerSignedWaiverBL.GetDecryptedWaiverFile(utilities.ParafaitEnv.SiteId);
                                if (string.IsNullOrEmpty(fileWithPath) == false)
                                {
                                    try
                                    {
                                        string docName = MessageContainerList.GetMessage(executionContext, "Waivers") + "_" + (string.IsNullOrEmpty(customerSignedWaiverDTOList[i].WaiverName) ? string.Empty : customerSignedWaiverDTOList[i].WaiverName)
                                                          + "_" + (string.IsNullOrEmpty(customerSignedWaiverDTOList[i].SignedForName) ? string.Empty : customerSignedWaiverDTOList[i].SignedForName);
                                        docName = WaiverCustomerUtils.StripNonAlphaNumericExceptUnderScore(docName);
                                        pd.DocumentName = docName;
                                        PDFFilePrinter pdfFilePrinter = new PDFFilePrinter(pd, fileWithPath);
                                        pdfFilePrinter.SendPDFToPrinter();
                                        pdfFilePrinter = null;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.LogVariableState("fileWithPath", fileWithPath);
                                        log.Error(ex);
                                        throw;
                                    }
                                }
                                else
                                {
                                    messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 2377, customerSignedWaiverDTOList[i].WaiverName), MessageContainerList.GetMessage(executionContext, "ERROR")); //Unexpected error, unble to fetch signed waiver file for &1 customerSignedWaiverDTOList[i].WaiverName
                                }
                            }
                            else
                            {
                                messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 2345, customerSignedWaiverDTOList[i].WaiverName, MessageContainerList.GetMessage(executionContext, "print")),
                                                  MessageContainerList.GetMessage(executionContext, "Print"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(executionContext, "Print"));
            }
            log.LogMethodExit();
        }



        private void SetupThePrinting(PrintDocument MyPrintDocument, string docName)
        {
            log.LogMethodEntry(MyPrintDocument);
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;
            MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;
            MyPrintDocument.DocumentName = docName;
            MyPrintDialog.UseEXDialog = true;
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SHOW_PRINT_DIALOG_IN_POS"))
            {
                if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Print dialog was cancelled"));
                }
            }
            MyPrintDocument.DocumentName = docName;
            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            MyPrintDocument.DefaultPageSettings.Margins =
                             new Margins(10, 10, 20, 20);

            log.LogMethodExit();
        }

        private void btnEmailWaiver_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = GetSelectedDTOList();
                if (customerSignedWaiverDTOList != null && customerSignedWaiverDTOList.Any())
                {
                    CustomerSignedWaiverHeaderListBL customerSignedWaiverHeaderList = new CustomerSignedWaiverHeaderListBL(executionContext);
                    customerSignedWaiverHeaderList.ReSendWaiverEmail(customerSignedWaiverDTOList, utilities, null);
                    messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 48), MessageContainerList.GetMessage(executionContext, "Email"));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(executionContext, "Print"));
            }
            log.LogMethodExit();
        }


        private void dgvCustomers_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dgvCustomerSignedWaivers_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void tpSignWaivers_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.pnlSignWaivers.Refresh();
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SetCustomerSignFor(chkSelectAll.Checked);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                messageBoxDelegate(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MessageContainerList.GetMessage(executionContext, "Error"));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private void SetCustomerSignFor(bool flagValue)
        {
            log.LogMethodEntry(flagValue);
            try
            {
                signatoryCustomerBL = new CustomerBL(utilities.ExecutionContext, this.signedBycustomerDTO);
                foreach (DataGridViewRow dgvRow in dgvCustomers.Rows)
                {
                    this.Cursor = Cursors.WaitCursor;
                    DataGridViewCheckBoxCell checkBox = (dgvCustomers["signFor", dgvRow.Index] as DataGridViewCheckBoxCell);
                    if (flagValue)
                    {
                        if (Convert.ToBoolean(checkBox.Value))
                        {
                            continue; //already set proceed with next line
                        }
                        CustomerBL customerBL = (CustomerBL)dgvRow.Tag;
                        SetSignForFlag(dgvRow.Index, customerBL, true);
                    }
                    else
                    {
                        checkBox.Value = false;
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void dgvCustomerSignedWaivers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.RowIndex > -1)
            {
                if (dgvCustomerSignedWaivers.Columns[e.ColumnIndex].Name == "selectRecord")
                {
                    DataGridViewCheckBoxCell checkBox = (dgvCustomerSignedWaivers["selectRecord", e.RowIndex] as DataGridViewCheckBoxCell);
                    if (Convert.ToBoolean(checkBox.Value))
                    {
                        checkBox.Value = false;
                    }
                    else
                    {
                        checkBox.Value = true;
                    }
                }
            }
            log.LogMethodExit();
        }


        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvCustomerSignedWaivers.EndEdit();
            CheckBox headerCheckBox = (sender as CheckBox);

            for (int rowIndex = 0; rowIndex < dgvCustomerSignedWaivers.Rows.Count; rowIndex++)
            {
                DataGridViewCheckBoxCell checkBox = (dgvCustomerSignedWaivers["selectRecord", rowIndex] as DataGridViewCheckBoxCell);
                checkBox.Value = headerCheckBox.Checked;
            }

            log.LogMethodExit();
        }

        private void CreateHeaderCheckBox()
        {
            log.LogMethodEntry();
            //if (!dgvRedemptions.Controls.ContainsKey("ReverseGiftHeaderCheckBox"))
            //{
            cbxHeaderSelectRecord = new CustomCheckBox();
            //cbxHeaderSelectRecord.Name = "HeaderCheckBox";
            //cbxHeaderSelectRecord.Appearance = System.Windows.Forms.Appearance.Button;
            cbxHeaderSelectRecord.FlatAppearance.BorderSize = 0;
            //cbxHeaderSelectRecord.Image = global::Parafait_POS.Properties.Resources.UncheckedNew;
            //headerCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            //headerCheckBox.UseVisualStyleBackColor = true;
            //headerCheckBox.Image = Parafait_POS.Properties.Resources.UncheckedNew;
            //headerCheckBox.ImageAlign = ContentAlignment.BottomCenter;
            cbxHeaderSelectRecord.ImageAlign = ContentAlignment.MiddleCenter;
            cbxHeaderSelectRecord.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            cbxHeaderSelectRecord.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            cbxHeaderSelectRecord.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            cbxHeaderSelectRecord.Text = string.Empty;
            cbxHeaderSelectRecord.Font = dgvCustomerSignedWaivers.Font;
            cbxHeaderSelectRecord.Location = new Point(dgvCustomerSignedWaivers.Columns["selectRecord"].HeaderCell.ContentBounds.X + 1, dgvCustomerSignedWaivers.Columns["selectRecord"].HeaderCell.ContentBounds.Y + 1);
            cbxHeaderSelectRecord.BackColor = Color.Transparent;
            cbxHeaderSelectRecord.Size = new Size(60, 28);
            cbxHeaderSelectRecord.Click += new EventHandler(HeaderCheckBox_Clicked);
            dgvCustomerSignedWaivers.Controls.Add(cbxHeaderSelectRecord);
            //}
            //else
            //{
            //    CheckBox headerCheckBox = dgvRedemptions.Controls.Find("ReverseGiftHeaderCheckBox", false)[0] as CheckBox;
            //    headerCheckBox.Checked = false;
            //    headerCheckBox.Image = global::Parafait_POS.Properties.Resources.UncheckedNew;
            //}
            log.LogMethodExit();
        }

        private bool CanDeactivate(CustomerSignedWaiverDTO customerSignedWaiverDTO)
        {
            log.LogMethodEntry(customerSignedWaiverDTO);
            bool canDeactivate = true;
            if (customerSignedWaiverDTO != null && customerSignedWaiverDTO.CustomerSignedWaiverId > -1)
            { 
                TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_SIGNED_WAIVER_ID, customerSignedWaiverDTO.CustomerSignedWaiverId.ToString()));
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS_NOT_IN, Transaction.TrxStatus.CANCELLED.ToString()+","+ Transaction.TrxStatus.SYSTEMABANDONED.ToString()));
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, serverTimeObj.GetServerDateTime().Date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                List<TransactionDTO> transactionDTOList = transactionListBL.GetTransactionDTOList(searchParam, utilities);
                log.LogVariableState("transactionDTOList", transactionDTOList);
                if (transactionDTOList != null && transactionDTOList.Any())
                {
                    canDeactivate = false;
                }

            }
            log.LogMethodExit(canDeactivate);
            return canDeactivate;
        }

        private void SelectCustomerForSigning()
        {
            log.LogMethodEntry();
            if (dgvCustomers != null && dgvCustomers.Rows.Count == 1)
            {
                SetCustomerSignFor(true);
            }
            log.LogMethodExit();
        }
    }
}
